using System;
using UIKit;

namespace F23.Mobile.iOS.DependencySwizzler
{
    public class CustomBuildUpStrategy : IBuildUpStrategy
    {
        private readonly Action<UIViewController> _buildUp;

        public CustomBuildUpStrategy(Action<UIViewController> buildUp)
        {
            if (buildUp == null)
            {
                throw new ArgumentNullException("buildUp");
            }

            _buildUp = buildUp;
        }

        public void BuildUp(UIViewController viewController)
        {
            _buildUp(viewController);
        }
    }
}
