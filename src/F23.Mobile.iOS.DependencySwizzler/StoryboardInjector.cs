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
        private static readonly IntPtr Method;
        private static readonly object _lock = new object();

        private static IBuildUpStrategy _buildUpStrategy;
        private static Action<string> _logger;
        private static IntPtr _originalImpl;

        static StoryboardInjector()
        {
            Method = class_getInstanceMethod(
                Class.GetHandle(typeof(UIStoryboard)),
                Selector.GetHandle("instantiateViewControllerWithIdentifier:")
            );
        }

        /// <summary>
        /// Set up dependency injection using the provided delegate.
        /// </summary>
        /// <param name="buildUp">The delegate used to build up instances of <see cref="UIKit.UIViewController"/>.</param>
        public static void SetUp(Action<UIViewController> buildUp)
        {
            SetUp(buildUp, null);
        }

        /// <summary>
        /// Set up dependency injection using the provided delegate and logging callback.
        /// </summary>
        /// <param name="buildUp">The delegate used to build up instances of <see cref="UIKit.UIViewController"/>.</param>
        /// /// <param name="logger"">A callback for logging messages.</param>
        public static void SetUp(Action<UIViewController> buildUp, Action<string> logger)
        {
            SetUp(new CustomBuildUpStrategy(buildUp), logger);
        }

        /// <summary>
        /// Sets up dependency injection using the provided 
        /// <see cref="F23.Mobile.iOS.DependencySwizzler.IBuildUpStrategy"/>.
        /// </summary>
        /// <param name="buildUpStrategy">The <see cref="F23.Mobile.iOS.DependencySwizzler.IBuildUpStrategy" /> 
        /// used to build up instances of <see cref="UIKit.UIViewController"/>.</param>
        public static void SetUp(IBuildUpStrategy buildUpStrategy)
        {
            SetUp(buildUpStrategy, null);
        }

        /// <summary>
        /// Sets up dependency injection using the provided 
        /// <see cref="F23.Mobile.iOS.DependencySwizzler.IBuildUpStrategy"/>
        /// and logging callback.
        /// </summary>
        /// <param name="buildUpStrategy">The <see cref="F23.Mobile.iOS.DependencySwizzler.IBuildUpStrategy" /> 
        /// used to build up instances of <see cref="UIKit.UIViewController"/>.</param>
        /// <param name="logger"">A callback for logging messages.</param>
        public static void SetUp(IBuildUpStrategy buildUpStrategy, Action<string> logger)
        {
            if (buildUpStrategy == null)
            {
                throw new ArgumentNullException("buildUpStrategy");
            }

            lock (_lock)
            {
                ResetInternal();

                SetUpInternal(buildUpStrategy, logger);
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
                ResetInternal();
            }
        }

        private static void SetUpInternal(IBuildUpStrategy buildUpStrategy, Action<string> logger)
        {
            _buildUpStrategy = buildUpStrategy;
            _logger = logger;

            Log("Setting up UIStoryboard for UIViewController dependency injection.");

            CallbackDelegateNamed d = Inject;
            var newImpl = Marshal.GetFunctionPointerForDelegate(d);

            _originalImpl = method_setImplementation(Method, newImpl);
        }

        private static void ResetInternal()
        {
            if (_originalImpl == IntPtr.Zero)
            {
                return;
            }

            Log("Resetting UIStoryboard. No more dependency injection... :(");

            method_setImplementation(Method, _originalImpl);

            _originalImpl = IntPtr.Zero;
            _buildUpStrategy = null;
            _logger = null;
        }

        [MonoPInvokeCallback(typeof(CallbackDelegateNamed))]
        private static IntPtr Inject(IntPtr block, IntPtr self, IntPtr name)
        {
            var impl = Marshal.GetDelegateForFunctionPointer<CallbackDelegateNamed>(_originalImpl);

            var handle = impl(block, self, name);

            var viewController = Runtime.GetNSObject<UIViewController>(handle);

            Log("Preparing to inject dependencies into '{0}'...", viewController.GetType().Name);

            InjectRecursive(viewController);

            return handle;
        }

        private static void InjectRecursive(UIViewController vc)
        {
            Log("Building up instance of '{0}'.", vc.GetType().Name);

            _buildUpStrategy.BuildUp(vc);

            if (vc.ChildViewControllers != null)
            {
                foreach (var child in vc.ChildViewControllers)
                {
                    InjectRecursive(child);
                }
            }
        }

        private static void Log(string message)
        {
            if (_logger != null)
                _logger(message);
        }

        private static void Log(string format, params object[] args)
        {
            if (_logger != null)
                _logger(string.Format(format, args));
        }

        [MonoNativeFunctionWrapper]
        delegate IntPtr CallbackDelegateNamed(IntPtr block, IntPtr self, IntPtr name);

        [DllImport(Constants.ObjectiveCLibrary)]
        private extern static IntPtr class_getInstanceMethod(IntPtr @class, IntPtr sel);
//
//        [DllImport(Constants.ObjectiveCLibrary)]
//        private extern static IntPtr method_getImplementation(IntPtr method);

        [DllImport(Constants.ObjectiveCLibrary)]
        private extern static IntPtr imp_implementationWithBlock(ref BlockLiteral block);

        [DllImport(Constants.ObjectiveCLibrary)]
        private extern static IntPtr method_setImplementation(IntPtr method, IntPtr imp);
    }
}
