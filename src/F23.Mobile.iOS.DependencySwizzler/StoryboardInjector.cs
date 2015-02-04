using System;
using System.Runtime.InteropServices;
using ObjCRuntime;
using UIKit;

namespace F23.Mobile.iOS.DependencySwizzler
{
    /// <summary>
    /// Static class responsible for setting up (or tearing down) dependency injection
    /// via Objective-C runtime method swizzling. For a detailed discussion of method
    /// swizzling, see this excellent article: http://nshipster.com/method-swizzling/.
    /// Also, see the Apple documentation for the Objective-C runtime reference: 
    /// https://developer.apple.com/library/ios/documentation/Cocoa/Reference/ObjCRuntimeRef/index.html
    /// </summary>
    public static class StoryboardInjector
    {
        private static readonly object _lock = new object();
        private static IBuildUpStrategy _buildUpStrategy;

        /// <summary>
        /// Set up dependency injection using the provided delegate.
        /// </summary>
        /// <param name="buildUp">The delegate used to build up instances of <see cref="UIKit.UIViewController"/>.</param>
        public static void SetUp(Action<UIViewController> buildUp)
        {
            SetUp(new CustomBuildUpStrategy(buildUp));
        }

        /// <summary>
        /// Sets up dependency injection using the provided 
        /// <see cref="F23.Mobile.iOS.DependencySwizzler.IBuildUpStrategy"/>.
        /// </summary>
        /// <param name="buildUpStrategy">The <see cref="F23.Mobile.iOS.DependencySwizzler.IBuildUpStrategy/> 
        /// used to build up instances of <see cref="UIKit.UIViewController"/>.</param>
        public static void SetUp(IBuildUpStrategy buildUpStrategy)
        {
            if (buildUpStrategy == null)
            {
                throw new ArgumentNullException("buildUpStrategy");
            }

            _buildUpStrategy = buildUpStrategy;

            lock (_lock)
            {
                InitInitial();
                InitNamed();
            }
        }

        /// <summary>
        /// Swizzles <see cref="UIKit.UIStoryboard"/> back to its
        /// default implementation, disabling dependency injection.
        /// </summary>
        public static void Reset()
        {
            lock (_lock)
            {
                ResetInitial();
                ResetNamed();
            }
        }

        private static void InitInitial()
        {
            var method = GetMethodInitial();

            OriginalImpInitial = method_getImplementation(method);

            CallbackDelegateInitial d = InjectInitial;
            var imp = Marshal.GetFunctionPointerForDelegate(d);

            method_setImplementation(method, imp);
        }

        private static void InitNamed()
        {
            var method = GetMethodNamed();

            OriginalImpNamed = method_getImplementation(method);

            CallbackDelegateNamed d = InjectNamed;
            var imp = Marshal.GetFunctionPointerForDelegate(d);

            method_setImplementation(method, imp);
        }

        private static void ResetInitial()
        {
            if (OriginalImpInitial == IntPtr.Zero)
            {
                return;
            }

            var method = GetMethodInitial();

            method_setImplementation(method, OriginalImpInitial);

            OriginalImpInitial = IntPtr.Zero;
        }

        private static void ResetNamed()
        {
            if (OriginalImpNamed == IntPtr.Zero)
            {
                return;
            }

            var method = GetMethodNamed();

            method_setImplementation(method, OriginalImpNamed);

            OriginalImpNamed = IntPtr.Zero;
        }

        private static IntPtr GetMethodInitial()
        {
            var sel = Selector.GetHandle(Sel_Initial);
            var @class = Class.GetHandle(typeof(UIStoryboard));

            return class_getInstanceMethod(@class, sel);
        }

        private static IntPtr GetMethodNamed()
        {
            var sel = Selector.GetHandle(Sel_Named);
            var @class = Class.GetHandle(typeof(UIStoryboard));

            return class_getInstanceMethod(@class, sel);
        }

        [MonoPInvokeCallback(typeof(CallbackDelegateInitial))]
        private static IntPtr InjectInitial(IntPtr block, IntPtr self)
        {
            var imp = Marshal.GetDelegateForFunctionPointer<CallbackDelegateInitial>(OriginalImpInitial);

            var handle = imp(block, self);

            var viewController = Runtime.GetNSObject<UIViewController>(handle);

            _buildUpStrategy.BuildUp(viewController);

            return handle;
        }

        [MonoPInvokeCallback(typeof(CallbackDelegateNamed))]
        private static IntPtr InjectNamed(IntPtr block, IntPtr self, IntPtr name)
        {
            var imp = Marshal.GetDelegateForFunctionPointer<CallbackDelegateNamed>(OriginalImpNamed);

            var handle = imp(block, self, name);

            var viewController = Runtime.GetNSObject<UIViewController>(handle);

            _buildUpStrategy.BuildUp(viewController);

            return handle;
        }

        private static IntPtr OriginalImpInitial;
        private static IntPtr OriginalImpNamed;

        [MonoNativeFunctionWrapper]
        delegate IntPtr CallbackDelegateInitial(IntPtr block, IntPtr self);

        [MonoNativeFunctionWrapper]
        delegate IntPtr CallbackDelegateNamed(IntPtr block, IntPtr self, IntPtr name);

        private const string Sel_Initial = "instantiateInitialViewController";
        private const string Sel_Named = "instantiateViewControllerWithIdentifier:";

        [DllImport(Constants.ObjectiveCLibrary)]
        private extern static IntPtr class_getInstanceMethod(IntPtr @class, IntPtr sel);

        [DllImport(Constants.ObjectiveCLibrary)]
        private extern static IntPtr method_getImplementation(IntPtr method);

        [DllImport(Constants.ObjectiveCLibrary)]
        private extern static IntPtr imp_implementationWithBlock(ref BlockLiteral block);

        [DllImport(Constants.ObjectiveCLibrary)]
        private extern static void method_setImplementation(IntPtr method, IntPtr imp);
    }
}
