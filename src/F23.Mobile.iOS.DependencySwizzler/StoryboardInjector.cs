using System;
using System.Runtime.InteropServices;
using ObjCRuntime;
using UIKit;

namespace F23.Mobile.iOS.DependencySwizzler
{
    public static class StoryboardInjector
    {
        private static readonly object _lock = new object();
        private static IBuildUpStrategy _buildUpStrategy;

        public static void SetUp(Action<UIViewController> buildUp)
        {
            SetUp(new CustomBuildUpStrategy(buildUp));
        }

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
            if (OriginalImpInitial == null)
            {
                return;
            }

            var method = GetMethodInitial();

            method_setImplementation(method, OriginalImpInitial);
        }

        private static void ResetNamed()
        {
            if (OriginalImpNamed == null)
            {
                return;
            }

            var method = GetMethodNamed();

            method_setImplementation(method, OriginalImpNamed);
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
