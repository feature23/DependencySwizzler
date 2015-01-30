using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace F23.Mobile.iOS.DependencySwizzler.Unity
{
    internal class PropertyInjectionContainerExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Context.Strategies.Add(new PropertyInjectionBuilderStrategy(Container), UnityBuildStage.Initialization);
        }
    }
}
