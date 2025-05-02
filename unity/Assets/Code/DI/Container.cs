using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Melkov.DI.Debug;

/*
 * TODO: Кеширование
 * 1. Кешировать рефлексию для все типов
 * 2. Кешировать аттрибуты для членов типа
 * 3. Сделать кастомные обработчики для получения аттрибутов и членов
 */
namespace Melkov.DI
{
    public class Container
    {
        private readonly Graph _graph = new();
        private readonly List<IDIDependency> _dependencies = new ();
        
        private readonly Dictionary<Type, object> _instances = new();
        
        public IDIDependency<TType> Bind<TType>()
        {
            var type = typeof(TType);
            
            var dependency = new DIDependency<TType>(type);
            
            _dependencies.Add(dependency);
            _graph.Register(type);

            return dependency;
        }
        
        public void Build()
        {
            var binds = _dependencies
                .Select(d => d.bind_info)
                .ToDictionary(b => b.Type, b => b);

            var sort_dependencies = _graph.GetDependencies();

            for (int index_1 = 0, count_1 = sort_dependencies.Count; index_1 < count_1; ++index_1)
            {
                object instance;
                
                var dependency = sort_dependencies[index_1];
                
                Assert.This
                (
                    binds.ContainsKey(dependency), 
                    $"Dependency '{dependency}' not found"
                );

                instance = Activate(dependency);
                    
                _instances[dependency] = instance;
            }
        }

        [CanBeNull]
        public TType Resolve<TType>() where TType : class
        {
            var type = typeof(TType);

            if (_instances.TryGetValue(type, out var result) == false)
            {
                return null;
            }

            return result as TType;
        }

        public void Inject([NotNull] object instance)
        {
            var type = instance.GetType();

            var inject_members = Graph.GetInjectMembersByType(type, false);
        
            InjectInternal(in instance, in inject_members);
        }

        public object Activate(Type type)
        {
            var inject_members = Graph.GetInjectMembersByType(type, true);
        
            object instance = null;

            if (inject_members.TryGetValue(MemberTypes.Constructor, out var constructors))
            {
                var inject_constructor = (MethodBase)constructors[0];

                if (constructors.Count > 1)
                {
                    for (int index = 0, count = constructors.Count; index < count; ++index)
                    {
                        var constructor = constructors[index];
                    
                        var attribute = constructor.GetCustomAttribute<InjectAttribute>();

                        if (attribute == null)
                        {
                            continue;
                        }
                    
                        inject_constructor = (MethodBase)constructor;
                        break;
                    }
                }

                var args = GetMethodArgs(inject_constructor);

                instance = Activator.CreateInstance(type, args);
            }
            else
            {
                instance = Activator.CreateInstance(type);
            }
        
            InjectInternal(in instance, in inject_members);

            return instance;
        }

        [CanBeNull]
        public TType Activate<TType>() where TType : class
        {
            var type = typeof(TType);

            return Activate(type) as TType;
        }
    
        private void InjectInternal([NotNull] in object instance, [NotNull] in Dictionary<MemberTypes, List<MemberInfo>> inject_members)
        {
            var priorities = new Dictionary<byte, MemberTypes>()
            {
                { 0, MemberTypes.Method },
                { 1, MemberTypes.Property },
                { 2, MemberTypes.Field },
            };
        
            for (byte index_1 = 0, count_1 = (byte)priorities.Count; index_1 < count_1; ++index_1)
            {
                var member_type = priorities[index_1];

                if (inject_members.TryGetValue(member_type, out var members) == false)
                {
                    continue;
                }

                for (int index_2 = 0, count_2 = members.Count; index_2 < count_2; ++index_2)
                {
                    var member = members[index_2];

                    switch (member.MemberType)
                    {
                        case MemberTypes.Method:
                        {
                            var method = (MethodBase)member;

                            var args = GetMethodArgs(method);
                        
                            method.Invoke(instance, args);
                            break;
                        }

                        case MemberTypes.Property:
                        case MemberTypes.Field:
                        {
                            var field = (FieldInfo)member;

                            if (_instances.TryGetValue(field.FieldType, out var value) == false)
                            {
                                throw new Exception($"Unable to resolve parameter type {field.FieldType} from instance: {instance}");
                            }
                        
                            field.SetValue(instance, value);
                            break;
                        }

                        case MemberTypes.All:
                        case MemberTypes.Constructor:
                        case MemberTypes.Custom:
                        case MemberTypes.Event:
                        case MemberTypes.NestedType:
                        case MemberTypes.TypeInfo:
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }
    
        [NotNull]
        private object[] GetMethodArgs([NotNull] MethodBase method)
        {
            var parameters = method.GetParameters();
                        
            var args = new object[parameters.Length];

            for (int index_3 = 0, count_3 = parameters.Length; index_3 < count_3; index_3++)
            {
                var parameter = parameters[index_3];

                var parameter_type = parameter.ParameterType;

                if (_instances.TryGetValue(parameter_type, out var value) == false)
                {
                    throw new Exception($"Unable to resolve parameter type {parameter_type}");
                }
                            
                args[index_3] = value;
            }

            return args;
        }
    }
}