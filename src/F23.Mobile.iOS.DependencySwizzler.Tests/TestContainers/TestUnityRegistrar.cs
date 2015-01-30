using System;
using Microsoft.Practices.Unity;
using F23.Mobile.iOS.DependencySwizzler.Tests.TestServices;

namespace F23.Mobile.iOS.DependencySwizzler.Tests.TestContainers
{
    public static class TestUnityRegistrar
    {
        private static IUnityContainer _container;

        public static IUnityContainer Container { get { return _container; } }

        public static void Init()
        {
            if (_container != null)
                return;

            var container = new UnityContainer();

            container.RegisterType<ITestService, TestService>();

            _container = container;
        }

        public static void Reset()
        {
            _container.Dispose();
            _container = null;
        }
    }
}
