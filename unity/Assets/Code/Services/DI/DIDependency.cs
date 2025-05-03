using System;

namespace Code.DI
{
    public interface IDIDependency
    {
        public BindInfo bind_info { get; }
    }
    
    public interface IDIDependencySetScope<TDependency>
    {
        void AsTransient();
        void AsSingleton();
    }
    
    public interface IDIDependency<TDependency> : IDIDependencySetScope<TDependency>
    {
        IDIDependencySetScope<TDependency> FromInstance(TDependency instance);
        IDIDependencySetScope<TDependency> FromMethod(Func<TDependency> method);
    }
    
    public class DIDependency<TDependency> : 
        IDIDependency, 
        IDIDependency<TDependency>
    {
        public BindInfo bind_info { get; }

        public DIDependency(Type type)
        {
            bind_info = new BindInfo(type);
        }
        
        public IDIDependencySetScope<TDependency> FromInstance(TDependency instance)
        {
            bind_info.SetInstance(instance);

            return this;
        }

        public IDIDependencySetScope<TDependency> FromMethod(Func<TDependency> method)
        {
            return this;
        }

        public void AsSingleton()
        {
            bind_info.SetScope(BIND_SCOPE.SINGLETON);
        }

        public void AsTransient()
        {
            bind_info.SetScope(BIND_SCOPE.TRANSIENT);
        }
    }
}