using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

/*
 * TODO: Кеширование
 * 1. Кешировать рефлексию для все типов
 * 2. Кешировать аттрибуты для членов типа
 * 3. Сделать кастомные обработчики для получения аттрибутов и членов
 */
namespace Code.DI
{
    public class DIContainer : IDisposable
    {
        [CanBeNull] private readonly DIContainer _parent_container;
        private readonly List<DIContainer> _sub_containers = new();


        private readonly Graph _graph;
        private readonly List<IDIDependency> _dependencies;

        private readonly Dictionary<Type, object> _instances;

        public DIContainer(DIContainer parent_container = null)
        {
            _parent_container = parent_container;

            _graph = new Graph(parent_container);
            
            _dependencies = new List<IDIDependency>();
            _instances = new Dictionary<Type, object>();
            
            var type = typeof(DIContainer);
            var dependency = new DIDependency<DIContainer>(type, null);
            
            dependency
                .FromInstance(this)
                .AsSingleton();
            
            _dependencies.Add(dependency);
        }
        
        public IDIDependency<TType> Bind<TType>()
        {
            var type = typeof(TType);
            
            var dependency = new DIDependency<TType>(type, null);
            
            _dependencies.Add(dependency);

            return dependency;
        }

        public IDIDependency<TContract> Bind<TContract, TImpl>() where TImpl : TContract
        {
            var type = typeof(TContract);
            var type_impl = typeof(TImpl);
            
            var dependency = new DIDependency<TContract>(type, type_impl);
            
            _dependencies.Add(dependency);
            
            return dependency;
        }
        
        public void Build()
        {
            _dependencies.ForEach(d =>
            {
                if (d.bind_info.Instance != null)
                {
                    _graph.Register(d.bind_info);
                    
                    _instances[d.bind_info.Type] = d.bind_info.Instance;
                }
            });
            
            _dependencies.ForEach(d =>
            {
                if (d.bind_info.Instance == null)
                {
                    _graph.Register(d.bind_info);
                }
            });
            
            var binds = _dependencies
                .Select(d => d.bind_info)
                .ToDictionary(b => b.Type, b => b);

            var sort_dependencies = _graph.GetDependencies();

            for (int index_1 = 0, count_1 = sort_dependencies.Count; index_1 < count_1; ++index_1)
            {
                object instance;
                
                var dependency = sort_dependencies[index_1];

                if (_parent_container?.Resolve(dependency) != null)
                {
                    continue;
                }
                    
                if (Resolve(dependency) != null)
                {
                    continue;
                }
                
                Assert.This
                (
                    binds.ContainsKey(dependency), 
                    $"Dependency '{dependency}' not found"
                );

                var bind_info = binds[dependency];

                if (bind_info.Instance != null)
                {
                    instance = bind_info.Instance;
                }
                else
                {
                    var info = binds[dependency];

                    if (info.TypeImpl != null)
                    {
                        instance = Activate(info.TypeImpl);
                    }
                    else
                    {
                        instance = Activate(dependency);
                    }
                }
                    
                _instances[dependency] = instance;
            }
        }

        public bool TryResolve(Type type, out object instance)
        {
            instance = Resolve(type);
            
            return instance != null;
        }

        [CanBeNull]
        public object Resolve(Type type)
        {
            if (_parent_container?.TryResolve(type, out var value) ?? false)
            {
                return value;
            }
            
            return _instances.GetValueOrDefault(type);
        }

        [CanBeNull]
        public TType Resolve<TType>() where TType : class
        {
            var type = typeof(TType);

            var result = Resolve(type);

            return result as TType;
        }

        public void Inject([NotNull] object instance, params object[] args)
        {
            var type = instance.GetType();

            var inject_members = Graph.GetInjectMembersByType(type, false);
        
            InjectInternal(in instance, in inject_members, args);
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
        
        public void Dispose()
        {
            foreach (var instance in _instances.Values)
            {
                if (instance == this)
                {
                    continue;
                }
                
                (instance as IDisposable)?.Dispose();
            }
            
            _instances.Clear();
            _dependencies.Clear();
            _graph.Dispose();
        }
    
        private void InjectInternal([NotNull] in object instance, [NotNull] in Dictionary<MemberTypes, List<MemberInfo>> inject_members, params object[] custom_args)
        {
            var custom_instances = new Dictionary<Type, object>();

            foreach (var arg in custom_args)
            {
                var type = arg.GetType();
                
                custom_instances[type] = arg;
            }
            
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

                            var args = GetMethodArgs(method, custom_args);
                        
                            method.Invoke(instance, args);
                            break;
                        }

                        case MemberTypes.Property:
                        case MemberTypes.Field:
                        {
                            var field = (FieldInfo)member;

                            if (field.FieldType != typeof(DIContainer))
                            {
                                if (_parent_container?.TryResolve(field.FieldType, out var parent_value) ?? false)
                                {
                                    field.SetValue(instance, parent_value);
                                    break;
                                }
                            }

                            if (TryResolve(field.FieldType, out var value))
                            {
                                field.SetValue(instance, value);
                                break;
                            }
                            
                            if (custom_instances.TryGetValue(field.FieldType, out value))
                            {
                                field.SetValue(instance, value);
                                break;
                            }

                            var msg = $"Unable to resolve parameter type {field.FieldType} from instance: {instance}";
                            
                            throw new Exception(msg);
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
        private object[] GetMethodArgs([NotNull] MethodBase method, params object[] custom_args)
        {
            var custom_instances = new Dictionary<Type, object>();

            foreach (var arg in custom_args)
            {
                var type = arg.GetType();
                
                custom_instances[type] = arg;
            }
            
            var parameters = method.GetParameters();
                        
            var args = new object[parameters.Length];

            for (int index_3 = 0, count_3 = parameters.Length; index_3 < count_3; index_3++)
            {
                var parameter = parameters[index_3];

                var parameter_type = parameter.ParameterType;

                if (parameter_type != typeof(DIContainer))
                {
                    if (_parent_container?.TryResolve(parameter_type, out var parent_value) ?? false)
                    {
                        args[index_3] = parent_value;
                        continue;
                    }
                }

                if (TryResolve(parameter_type, out var value) == false)
                {
                    if (custom_instances.TryGetValue(parameter_type, out value) == false)
                    {
                        throw new Exception($"Unable to resolve parameter type {parameter_type}");
                    }
                }
                            
                args[index_3] = value;
            }

            return args;
        }
    }
}