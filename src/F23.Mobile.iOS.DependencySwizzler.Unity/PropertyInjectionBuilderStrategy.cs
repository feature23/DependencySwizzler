using System;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace F23.Mobile.iOS.DependencySwizzler.Unity
{
    internal class PropertyInjectionBuilderStrategy : BuilderStrategy
    {
        private readonly IUnityContainer _container;

        public PropertyInjectionBuilderStrategy(IUnityContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");

            _container = container;
        }

        public override void PreBuildUp(IBuilderContext context)
        {
            if (!context.BuildKey.Type.FullName.StartsWith("Microsoft.Practices"))
            {
                var obj = context.Existing;

                var ti = obj.GetType().GetTypeInfo();

                var dependencyProps = ti.GetRuntimeProperties()
                    .Where(p => p.CanWrite)
                    .Where(p => _container.IsRegistered(p.PropertyType));

                foreach (var p in dependencyProps)
                {
                    var type = p.PropertyType;

                    var resolved = _container.Resolve(type);

                    p.SetValue(obj, resolved);
                }
            }
        }
    }
}
