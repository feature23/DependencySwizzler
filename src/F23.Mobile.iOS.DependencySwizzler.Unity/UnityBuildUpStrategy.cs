using System;
using Microsoft.Practices.Unity;
using UIKit;

namespace F23.Mobile.iOS.DependencySwizzler.Unity
{
    public class UnityBuildUpStrategy : IBuildUpStrategy
    {
        private readonly IUnityContainer _container;

        public UnityBuildUpStrategy(IUnityContainer container, bool configurePropertyInjectionExtension = true)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            _container = container;

            if (configurePropertyInjectionExtension)
            {
                container.AddSetterBuildUpExtension();
            }
        }

        public void BuildUp(UIViewController viewController)
        {
            _container.BuildUp(viewController);
        }
    }
}
