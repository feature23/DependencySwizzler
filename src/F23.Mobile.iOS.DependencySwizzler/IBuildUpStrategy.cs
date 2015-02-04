using System;
using UIKit;

namespace F23.Mobile.iOS.DependencySwizzler
{
    /// <summary>
    /// Used by StoryboardInjector to build up dependencies for <see cref="UIKit.UIViewController"/>s.
    /// Implementers of <see cref="F23.Mobile.iOS.DependencySwizzler.IBuildUpStrategy"/> 
    /// can use the IoC strategy of their choosing to perform the buildup.
    /// </summary>
    public interface IBuildUpStrategy
    {
        /// <summary>
        /// Builds up the dependencies of the provided <see cref="UIKit.UIViewController"/> instance.
        /// </summary>
        /// <param name="viewController">Instance of <see cref="UIKit.UIViewController"/> to build up.</param>
        void BuildUp(UIViewController viewController);
    }
}
