using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Code.DI
{
	public class Graph
	{
		private readonly Dictionary<Type, HashSet<Type>> _dependencies = new();
		private readonly HashSet<Type> _types = new();

		public void Register(Type type)
		{
			if (_types.Add(type) == false)
			{
				return;
			}

			if (_dependencies.TryGetValue(type, out var dependencies) == false)
			{
				dependencies = new HashSet<Type>();

				_dependencies.Add(type, dependencies);
			}

			var inject_members = GetInjectMembersByType(type, true);

			foreach (var (member_type, members) in inject_members)
			{
				switch (member_type)
				{
					case MemberTypes.Constructor:
					case MemberTypes.Method:
					{
						List<MethodBase> methods = new();

						if (member_type == MemberTypes.Constructor)
						{
							var inject_constructor = (MethodBase)members[0];

							if (members.Count > 1)
							{
								for (int index = 0, count = members.Count;
								     index < count;
								     ++index)
								{
									var constructor = members[index];

									var attribute =
										constructor
											.GetCustomAttribute<
												InjectAttribute>();

									if (attribute == null)
									{
										continue;
									}

									inject_constructor = (MethodBase)constructor;
									break;
								}
							}

							methods.Add(inject_constructor);
						}
						else
						{
							methods = members
								.Select(member => (MethodBase)member)
								.ToList();
						}

						for (int index = 0, count = methods.Count; index < count; ++index)
						{
							var method = methods[index];

							var parameters = method.GetParameters();

							RegisterParameters(parameters, dependencies);
						}

						break;
					}

					case MemberTypes.Property:
					case MemberTypes.Field:
					{
						for (int index = 0, count = members.Count; index < count; ++index)
						{
							var member = members[index];

							var field = (FieldInfo)member;

							RegisterType(field.FieldType, dependencies);
						}

						break;
					}

					case MemberTypes.All:
					case MemberTypes.Custom:
					case MemberTypes.Event:
					case MemberTypes.NestedType:
					case MemberTypes.TypeInfo:
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		//Kahn's
		public List<Type> GetDependencies()
		{
			// Step 1: Построим словарь входящих степеней
			var in_degree = new Dictionary<Type, int>();

			foreach (var type in _types)
			{
				in_degree.TryAdd(type, 0);

				if (_dependencies.TryGetValue(type, out var deps) == false)
				{
					continue;
				}

				foreach (var dep in deps)
				{
					in_degree.TryAdd(dep, 0);

					in_degree[dep]++;
				}
			}

			// Step 2: Найдём все типы с in-degree == 0
			var types_zero = in_degree
				.Where(p => p.Value == 0)
				.Select(p => p.Key);

			var queue = new Queue<Type>(types_zero);

			var result = new List<Type>();

			while (queue.Count > 0)
			{
				var current = queue.Dequeue();

				result.Add(current);

				if (_dependencies.TryGetValue(current, out var deps) == false)
				{
					continue;
				}

				foreach (var dep in deps)
				{
					in_degree[dep]--;

					if (in_degree[dep] == 0)
					{
						queue.Enqueue(dep);
					}
				}
			}

			// Step 3: Проверка на цикл
			if (result.Count == in_degree.Count)
			{
				return result.AsEnumerable().Reverse().ToList();
			}

			var cyclic_types = in_degree
				.Where(kv => kv.Value > 0)
				.Select(kv => kv.Key)
				.ToList();

			var message = "Cycle Types: " + string.Join(", ", cyclic_types.Select(t => t.FullName));

			throw new InvalidOperationException(message);
		}

		[NotNull]
		public static Dictionary<MemberTypes, List<MemberInfo>> GetInjectMembersByType([NotNull] Type type,
			bool is_constructor)
		{
			const BindingFlags FLAGS = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;

			var all_members = type.GetMembers(FLAGS);

			var inject_members = new Dictionary<MemberTypes, List<MemberInfo>>();

			for (int index = 0, count = all_members.Length; index < count; ++index)
			{
				var member = all_members[index];

				if (inject_members.TryGetValue(member.MemberType, out var members) == false)
				{
					members = new List<MemberInfo>();
				}

				if (member.MemberType != MemberTypes.Constructor)
				{
					var attribute = member.GetCustomAttribute<InjectAttribute>();

					if (attribute == null)
					{
						continue;
					}
				}

				switch (member.MemberType)
				{
					case MemberTypes.Constructor:
					case MemberTypes.Method:
					{
						if (member.MemberType == MemberTypes.Constructor &&
						    is_constructor == false)
						{
							continue;
						}

						var method = (MethodBase)member;

						var parameters = method.GetParameters();

						if (parameters.Length == 0)
						{
							continue;
						}

						members.Add(method);
						break;
					}

					case MemberTypes.Property:
					{
						var property = (PropertyInfo)member;

						var backing_field = type.GetField($"<{property.Name}>k__BackingField",
							FLAGS);

						members.Add(backing_field);
						break;
					}

					case MemberTypes.Field:
					{
						members.Add(member);
						break;
					}

					case MemberTypes.All:
					case MemberTypes.Custom:
					case MemberTypes.Event:
					case MemberTypes.NestedType:
					case MemberTypes.TypeInfo:
					default:
						continue;
				}

				inject_members[member.MemberType] = members;
			}

			return inject_members;
		}

		private void RegisterParameters(ParameterInfo[] parameters, in HashSet<Type> dependencies)
		{
			for (int index_2 = 0, count_2 = parameters.Length; index_2 < count_2; ++index_2)
			{
				var parameter = parameters[index_2];

				RegisterType(parameter.ParameterType, in dependencies);
			}
		}

		private void RegisterType(Type type, in HashSet<Type> dependencies)
		{
			dependencies.Add(type);

			if (_types.Contains(type) == false)
			{
				Register(type);
			}
		}
	}
}