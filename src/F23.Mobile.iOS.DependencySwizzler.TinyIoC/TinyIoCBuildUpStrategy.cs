using System;
using UIKit;
using TinyIoC;

namespace F23.Mobile.iOS.DependencySwizzler.TinyIoC
{
    /// <summary>
    /// Implementation of <see cref="F23.Mobile.iOS.DependencySwizzler.IBuildUpStrategy"/> that builds up
    /// instances of UIViewController using a provided TinyIoCContainer instance.
    /// </summary>
    public class TinyIoCBuildUpStrategy : IBuildUpStrategy
    {
        private TinyIoCContainer _container;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="F23.Mobile.iOS.DependencySwizzler.TinyIoC.TinyIoCBuildUpStrategy"/> class
        /// using the provided TinyIoCContainer instance.
        /// </summary>
        /// <param name="container">Instance of TinyIoC container.</param>
        public TinyIoCBuildUpStrategy(TinyIoCContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            _container = container;
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
