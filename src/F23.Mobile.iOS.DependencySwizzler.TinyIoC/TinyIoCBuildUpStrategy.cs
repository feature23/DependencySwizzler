using System;
using UIKit;
using TinyIoC;

namespace F23.Mobile.iOS.DependencySwizzler.TinyIoC
{
    public class TinyIoCBuildUpStrategy : IBuildUpStrategy
    {
        private TinyIoCContainer _container;

        public TinyIoCBuildUpStrategy(TinyIoCContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            _container = container;
        }

        public void BuildUp(UIViewController viewController)
        {
            _container.BuildUp(viewController);
        }
    }
}
