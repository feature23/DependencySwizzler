using System;
using Microsoft.Practices.Unity;

namespace F23.Mobile.iOS.DependencySwizzler.Unity
{
    public static class UnityContainerExtensions
    {
        public static void AddSetterBuildUpExtension(this IUnityContainer container)
        {
            container.AddNewExtension<PropertyInjectionContainerExtension>();
        }
    }
}
