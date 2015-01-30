using System;
using UIKit;

namespace F23.Mobile.iOS.DependencySwizzler
{
    public interface IBuildUpStrategy
    {
        void BuildUp(UIViewController viewController);
    }
}
