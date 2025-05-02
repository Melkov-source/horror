using System;

namespace Code.DI
{
    public class BindInfo
    {
        public Type Type { get; }
        public BIND_SCOPE Scope { get; private set; } = BIND_SCOPE.TRANSIENT;
        public object Instance { get; private set; }

        public BindInfo(Type type)
        {
            Type = type;
        }

        public void SetScope(BIND_SCOPE scope)
        {
            Scope = scope;
        }

        public void SetInstance(object instance)
        {
            Instance = instance;
        }
    }
}