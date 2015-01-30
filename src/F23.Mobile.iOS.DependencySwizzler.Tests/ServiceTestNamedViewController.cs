using System;
using System.CodeDom.Compiler;
using F23.Mobile.iOS.DependencySwizzler.Tests.TestServices;
using Foundation;
using Microsoft.Practices.Unity;
using UIKit;

namespace F23.Mobile.iOS.DependencySwizzler.Tests
{
	partial class ServiceTestNamedViewController : UIViewController
	{
        [Dependency]
        public ITestService TestService { get; set; }

		public ServiceTestNamedViewController (IntPtr handle) : base (handle)
		{
		}
	}
}
