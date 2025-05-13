using System;
using JetBrains.Annotations;

namespace Code.DI
{
    public class BindInfo
    {
        public Type Type { get; }
        [CanBeNull] public Type TypeImpl { get; }
        
        public object Instance { get; private set; }
        public BIND_SCOPE Scope { get; private set; }

        public Func<object> FactoryMethod { get; private set; }

        public BindInfo(Type type, Type impl)
        {
            Type = type;
            TypeImpl = impl;
        }

        public void SetInstance(object instance)
        {
            Instance = instance;
        }

        public void SetFactoryMethod(Func<object> factory)
        {
            FactoryMethod = factory;
        }

        public void SetScope(BIND_SCOPE scope)
        {
            Scope = scope;
        }
    }
}