using System;
using Microsoft.Practices.Unity;
using UIKit;

namespace F23.Mobile.iOS.DependencySwizzler.Unity
{
    /// <summary>
    /// Implementation of <see cref="F23.Mobile.iOS.DependencySwizzler.IBuildUpStrategy"/> that builds up
    /// instances of UIViewController using a provided UnityContainer instance.
    /// </summary>
    public class UnityBuildUpStrategy : IBuildUpStrategy
    {
        private readonly IUnityContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="F23.Mobile.iOS.DependencySwizzler.Unity.UnityBuildUpStrategy"/> class
        /// using the provided UnityContainer instance.
        /// </summary>
        /// <param name="container">Instance of UnityContainer.</param>
        /// <param name="configurePropertyInjectionExtension">If set to <c>true</c> configure the property injection Unity extension.</param>
        /// <remarks>
        /// By default, dependencies must be marked with the <c>[Dependency]</c> attribute
        /// to be build up by the UnityContainer. By setting the <c>configurePropertyInjectionExtension</c>
        /// parameter to <c>true</c>, an extension is added to the container that will build up all publically 
        /// settable properties with a type that is registered in the container. In this case, the </c>[Dependency]</c>
        /// attribute is not needed.
        /// </remarks>
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

        /// <summary>
        /// Builds up the dependencies of the provided UIViewController instance.
        /// </summary>
        /// <param name="viewController">Instance of UIViewController to build up.</param>
        public void BuildUp(UIViewController viewController)
        {
            _container.BuildUp(viewController);
        }
    }
}
