using System;
using TinyIoC;
using F23.Mobile.iOS.DependencySwizzler.Tests.TestServices;

namespace F23.Mobile.iOS.DependencySwizzler.Tests.TestContainers
{
    public static class TestTinyIoCRegistrar
    {
        private static TinyIoCContainer _container;

        public static TinyIoCContainer Container { get { return _container; } }

        public static void Init()
        {
            if (_container != null)
                return;

            var container = new TinyIoCContainer();

            container.Register<ITestService, TestService>();

            _container = container;
        }

        public static void Reset()
        {
            _container.Dispose();
            _container = null;
        }
    }
}
