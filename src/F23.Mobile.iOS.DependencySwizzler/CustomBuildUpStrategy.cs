using System;
using UIKit;

namespace F23.Mobile.iOS.DependencySwizzler
{
    /// <summary>
    /// Implementation of <see cref="F23.Mobile.iOS.DependencySwizzler.IBuildUpStrategy"/> that builds up
    /// instances of UIViewController using a provided delegate.
    /// </summary>
    public class CustomBuildUpStrategy : IBuildUpStrategy
    {
        private readonly Action<UIViewController> _buildUp;

        /// <summary>
        /// Initializes a new instance of the <see cref="F23.Mobile.iOS.DependencySwizzler.CustomBuildUpStrategy"/> class
        /// using the provided delegate. The delegate is executed for each instance of UIViewController that needs
        /// to be built up.
        /// </summary>
        /// <param name="buildUp">Delegate to perform the build up on UIViewController instances.</param>
        public CustomBuildUpStrategy(Action<UIViewController> buildUp)
        {
            if (buildUp == null)
            {
                throw new ArgumentNullException("buildUp");
            }

            _buildUp = buildUp;
        }

        /// <summary>
        /// Builds up the dependencies of the provided UIViewController instance.
        /// </summary>
        /// <param name="viewController">Instance of UIViewController to build up.</param>
        public void BuildUp(UIViewController viewController)
        {
            _buildUp(viewController);
        }
    }
}
