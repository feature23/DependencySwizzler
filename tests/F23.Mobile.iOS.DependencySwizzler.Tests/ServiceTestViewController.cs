using System;
using System.CodeDom.Compiler;
using F23.Mobile.iOS.DependencySwizzler.Tests.TestServices;
using Foundation;
using Microsoft.Practices.Unity;
using UIKit;

namespace F23.Mobile.iOS.DependencySwizzler.Tests
{
	partial class ServiceTestViewController : UIViewController
	{
        public ITestService TestService { get; set; }

		public ServiceTestViewController (IntPtr handle) : base (handle)
		{
		}
	}
}
