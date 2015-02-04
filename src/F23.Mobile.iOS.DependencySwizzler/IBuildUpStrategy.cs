using System;
using UIKit;

namespace F23.Mobile.iOS.DependencySwizzler
{
    /// <summary>
    /// Used by StoryboardInjector to build up dependencies for UIViewControllers.
    /// Implementers of IBuildUpStrategy can use the IoC strategy of their choosing
    /// to perform the buildup.
    /// </summary>
    public interface IBuildUpStrategy
    {
        /// <summary>
        /// Builds up the dependencies of the provided UIViewController instance.
        /// </summary>
        /// <param name="viewController">Instance of UIViewController to build up.</param>
        void BuildUp(UIViewController viewController);
    }
}
