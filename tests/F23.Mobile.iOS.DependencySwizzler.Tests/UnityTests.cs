using System;
using F23.Mobile.iOS.DependencySwizzler.Tests.TestContainers;
using F23.Mobile.iOS.DependencySwizzler.Unity;
using NUnit.Framework;
using UIKit;

namespace F23.Mobile.iOS.DependencySwizzler.Tests
{
    [TestFixture]
    public class UnityTests
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            TestUnityRegistrar.Init();

            var container = TestUnityRegistrar.Container;

            var strategy = new UnityBuildUpStrategy(container);

            StoryboardInjector.SetUp(strategy, msg => Console.WriteLine(msg));
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            StoryboardInjector.Reset();
            TestUnityRegistrar.Reset();
        }

        [Test]
        public void TestInitialViewController()
        {
            var storyboard = UIStoryboard.FromName("TestStoryboard", null);

            var nav = storyboard.InstantiateInitialViewController() as UINavigationController;

            var vc = nav.ViewControllers[0] as ServiceTestViewController;

            Assert.IsNotNull(vc);

            Assert.IsNotNull(vc.TestService);
        }

        [Test]
        public void TestNamedViewController()
        {
            var storyboard = UIStoryboard.FromName("TestStoryboard", null);

            const string identifier = "ServiceTestNamedViewController";

            var nav = storyboard.InstantiateViewController(identifier) as UINavigationController;

            var vc = nav.ViewControllers[0] as ServiceTestNamedViewController;

            Assert.IsNotNull(vc);

            Assert.IsNotNull(vc.TestService);
        }
    }
}
