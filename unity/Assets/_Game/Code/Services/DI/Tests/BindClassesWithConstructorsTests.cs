using NUnit.Framework;

namespace Code.DI.Tests
{
    public class BindClassesWithConstructorsTests
    {
        private DIContainer _container;
    
        [SetUp]
        public void Initialize() 
        {
            _container = new DIContainer();
        
            _container
                .Bind<A>()
                .AsSingleton();
        
            _container
                .Bind<B>()
                .AsSingleton();
        
            _container
                .Bind<C>()
                .AsSingleton();
            
            _container
                .Bind<E>()
                .AsSingleton();
            
            _container
                .Bind<F>()
                .AsSingleton();
            
            _container
                .Bind<G>()
                .AsSingleton();
            
            _container.Build();
        }

        [Test]
        public void Activate_CreateInstanceWithTargetTypeInConstructor_TargetDependencyInjected()
        {
            var d = _container.Activate<D>();
        
            Assert.IsNotNull(d);
            Assert.IsNotNull(d.a);
        }
        
        [Test]
        public void Resolve_ClassesWithConstructors_AllDependenciesInjected()
        {
            var a = _container.Resolve<A>();
            
            Assert.IsNotNull(a);
            Assert.IsNotNull(a.b);
            Assert.IsNotNull(a.b.c);
        }

        [Test]
        public void Resolve_NotBindType_NotInjected()
        {
            var d = _container.Resolve<D>();

            Assert.IsNull(d);
        }

        [Test]
        public void Resolve_TwoConstructorsButInjectOne_InjectedTargetConstructor()
        {
            var e = _container.Resolve<E>();
            
            Assert.IsNotNull(e);
            Assert.IsNotNull(e.a);
            Assert.IsNotNull(e.c);
        }
        
        [Test]
        public void Resolve_TwoConstructorsButInjectOne_InjectedFirstConstructor()
        {
            var f = _container.Resolve<F>();
            
            Assert.IsNotNull(f);
            Assert.IsNotNull(f.a);
            Assert.IsNull(f.c);
        }
        
        [Test]
        public void Resolve_TwoConstructorsButInjectTwo_InjectedFirstConstructor()
        {
            var g = _container.Resolve<G>();
            
            Assert.IsNotNull(g);
            Assert.IsNotNull(g.a);
            Assert.IsNull(g.c);
        }

        #region nested types

        private class A
        {
            public readonly B b;
            
            public A(B b)
            {
                this.b = b;
            }
        }
        
        private class B
        {
            public readonly C c;
            
            public B(C c)
            {
                this.c = c;   
            }
        }
        
        private class C
        {
            public C()
            {
            
            }
        }
    
        private class D
        {
            public readonly A a;
        
            public D(A a)
            {
                this.a = a;
            }
        }
        
        private class E
        {
            public readonly A a;
            public readonly C c;
            
            public E(A a)
            {
                this.a = a;
            }
            
            [Inject]
            public E(A a, C c)
            {
                this.a = a;
                this.c = c;
            }
        }
        
        private class F
        {
            public readonly A a;
            public readonly C c;
            
            public F(A a)
            {
                this.a = a;
            }
            
            public F(A a, C c)
            {
                this.a = a;
                this.c = c;
            }
        }
        
        private class G
        {
            public readonly A a;
            public readonly C c;
            
            [Inject]
            public G(A a)
            {
                this.a = a;
            }
            
            [Inject]
            public G(A a, C c)
            {
                this.a = a;
                this.c = c;
            }
        }
        
        #endregion
    }
}