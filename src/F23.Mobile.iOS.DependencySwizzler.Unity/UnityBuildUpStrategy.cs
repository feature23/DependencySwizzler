using System;
using Microsoft.Practices.Unity;
using UIKit;

namespace F23.Mobile.iOS.DependencySwizzler.Unity
{
    /// <summary>
    /// Implementation of <see cref="F23.Mobile.iOS.DependencySwizzler.IBuildUpStrategy"/> that
    /// builds up instances of <see cref="UIKit.UIViewController"/> using a provided
    /// <see cref="Microsoft.Practices.Unity.IUnityContainer"/> instance.
    /// </summary>
    public class UnityBuildUpStrategy : IBuildUpStrategy
    {
        private readonly IUnityContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="F23.Mobile.iOS.DependencySwizzler.Unity.UnityBuildUpStrategy"/> class
        /// using the provided <see cref="Microsoft.Practices.Unity.IUnityContainer"/> instance.
        /// </summary>
        /// <param name="container">Instance of <see cref="Microsoft.Practices.Unity.IUnityContainer"/>.</param>
        /// <param name="configurePropertyInjectionExtension">If set to <c>true</c> configure the property injection Unity extension.</param>
        /// <remarks>
        /// Using this class adds an extension to the provided
        /// <see cref="Microsoft.Practices.Unity.IUnityContainer"/>,
        /// allowing dependencies to be injected without using the 
        /// <c>[Dependency]</c> attribute.
        /// </remarks>
        public UnityBuildUpStrategy(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            _container = container;
            _container.AddSetterBuildUpExtension();
        }

        /// <summary>
        /// Builds up the dependencies of the provided <see cref="UIKit.UIViewController"/> instance.
        /// </summary>
        /// <param name="viewController">Instance of <see cref="UIKit.UIViewController"/> to build up.</param>
        public void BuildUp(UIViewController viewController)
        {
            _container.BuildUp(viewController);
        }
    }
}
