using System;
using F23.Mobile.iOS.DependencySwizzler.Tests.TestContainers;
using NUnit.Framework;
using UIKit;
using F23.Mobile.iOS.DependencySwizzler.Unity;

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

            StoryboardInjector.SetUp(strategy);
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

            var vc = storyboard.InstantiateInitialViewController() as ServiceTestViewController;

            Assert.IsNotNull(vc);

            Assert.IsNotNull(vc.TestService);
        }

        [Test]
        public void TestNamedViewController()
        {
            var storyboard = UIStoryboard.FromName("TestStoryboard", null);

            const string identifier = "ServiceTestNamedViewController";

            var vc = storyboard.InstantiateViewController(identifier) as ServiceTestNamedViewController;

            Assert.IsNotNull(vc);

            Assert.IsNotNull(vc.TestService);
        }
    }
}
