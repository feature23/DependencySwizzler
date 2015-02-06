using System;
using F23.Mobile.iOS.DependencySwizzler.Tests.TestContainers;
using NUnit.Framework;
using UIKit;
using F23.Mobile.iOS.DependencySwizzler.TinyIoC;

namespace F23.Mobile.iOS.DependencySwizzler.Tests
{
    [TestFixture]
    public class TinyIocTests
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            TestTinyIoCRegistrar.Init();

            var container = TestTinyIoCRegistrar.Container;

            var strategy = new TinyIoCBuildUpStrategy(container);

            StoryboardInjector.SetUp(strategy, msg => Console.WriteLine(msg));
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            StoryboardInjector.Reset();
            TestTinyIoCRegistrar.Reset();
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
