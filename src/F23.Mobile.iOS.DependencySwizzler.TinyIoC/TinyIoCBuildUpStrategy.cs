using System;
using UIKit;
using TinyIoC;

namespace F23.Mobile.iOS.DependencySwizzler.TinyIoC
{
    /// <summary>
    /// Implementation of <see cref="F23.Mobile.iOS.DependencySwizzler.IBuildUpStrategy"/>
    /// that builds up instances of <see cref="UIKit.UIViewController"/> using a provided 
    /// <see cref="TinyIoC.TinyIoCContainer"/>  instance.
    /// </summary>
    public class TinyIoCBuildUpStrategy : IBuildUpStrategy
    {
        private TinyIoCContainer _container;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="F23.Mobile.iOS.DependencySwizzler.TinyIoC.TinyIoCBuildUpStrategy"/> class
        /// using the provided <see cref="TinyIoC.TinyIoCContainer"/>  instance.
        /// </summary>
        /// <param name="container">Instance of <see cref="TinyIoC.TinyIoCContainer"/> .</param>
        public TinyIoCBuildUpStrategy(TinyIoCContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            _container = container;
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
