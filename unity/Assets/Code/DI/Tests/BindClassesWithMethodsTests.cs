using NUnit.Framework;

namespace Code.DI.Tests
{
    public class BindClassesWithMethodsTests
    {
        private Container _container;
    
        [SetUp]
        public void Initialize() 
        {
            _container = new Container();
        
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
                .Bind<D>()
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
        public void Resolve_ClassesWithMethods_AllDependenciesInjected()
        {
            var a = _container.Resolve<A>();
            
            Assert.IsNotNull(a);
            Assert.IsNotNull(a.b);
            Assert.IsNotNull(a.b.c);
        }
        
        [Test]
        public void Resolve_ClassesWithMethodsButOneInjected_InjectTargetMethod()
        {
            var d = _container.Resolve<D>();
            
            Assert.IsNotNull(d);
            Assert.IsFalse(d.Check);
            Assert.IsNotNull(d.a);
            Assert.IsNotNull(d.c);
        }
        
        [Test]
        public void Resolve_ClassesWithMethodsButNotInjected_NotAllInjectedMethods()
        {
            var f = _container.Resolve<F>();
            
            Assert.IsNotNull(f);

            Assert.IsNull(f.a);
            Assert.IsNull(f.c);
        }
        
        [Test]
        public void Resolve_ClassesWithMethodsButAllInjected_AllInjectedMethods()
        {
            var g = _container.Resolve<G>();
            
            Assert.IsNotNull(g);

            Assert.IsNotNull(g.a);
            Assert.IsNotNull(g.c);
            
            Assert.IsTrue(g.AInjectIndex == 1);
            Assert.IsTrue(g.CInjectIndex == 2);
        }
        
        [Test]
        public void Resolve_ClassWithConstructorAndMethodInjected_InjectedConstructorAndMethod()
        {
            var e = _container.Resolve<E>();
            
            Assert.IsNotNull(e);

            Assert.IsNotNull(e.a);
            Assert.IsNotNull(e.g);
        }

        #region nested types

        private class A
        {
            public B b;

            [Inject]
            public void Constructor(B b)
            {
                this.b = b;
            }
        }
        
        private class B
        {
            public C c;
            
            [Inject]
            public void Constructor(C c)
            {
                this.c = c;   
            }
        }

        private class C
        {
            [Inject]
            public void Constructor()
            {
                
            }
        }
        
        private class D
        {
            public A a;
            public C c;

            public bool Check;
            
            public void Constructor(A a)
            {
                this.a = a;
                Check = true;
            }
            
            [Inject]
            public void Constructor(A a, C c)
            {
                this.a = a;
                this.c = c;
            }
        }

        private class E
        {
            public readonly A a;
            public G g;

            public E(A a)
            {
                this.a = a;
            }

            [Inject]
            public void Constructor(G g)
            {
                this.g = g;
            }
        }
        
        private class F
        {
            public A a;
            public C c;
            
            public void Constructor(A a)
            {
                this.a = a;
            }
            
            public void Constructor(A a, C c)
            {
                this.a = a;
                this.c = c;
            }
        }
        
        private class G
        {
            public A a;
            public C c;

            private int _index = 0;

            public int AInjectIndex;
            public int CInjectIndex;
            
            [Inject]
            public void Constructor(A a)
            {
                this.a = a;

                _index++;

                AInjectIndex = _index;
            }
            
            [Inject]
            public void Constructor(C c)
            {
                this.c = c;
                
                _index++;
                
                CInjectIndex = _index;
            }
        }
        
        #endregion
    }
}