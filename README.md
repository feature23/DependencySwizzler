iOS Dependency Swizzler
=======================

> A Xamarin.iOS library that uses "method swizzling" from the Objective-C runtime to allow property-setter dependency injection of UIViewControllers instantiated by storyboards. If you're unfamiliar with the concept of method swizzling see [Mattt Thompson's excellent post](http://nshipster.com/method-swizzling/) at NSHipster. For a deeper dive, see Apple's documentation for the [Objective-C runtime reference](https://developer.apple.com/library/ios/documentation/Cocoa/Reference/ObjCRuntimeRef/index.html).

## Examples
### [Unity](https://unity.codeplex.com/)
```C#
var container = new UnityContainer();

/* Initialize container */

var buildUpStrategy = new UnityBuildUpStrategy(container);

StoryboardInjector.SetUp(buildUpStrategy);

```
*__Note__: Under the hood, the `UnityBuildUpStrategy` adds a Unity extension to enable property setter injection of all publicly settable properties of a type registered with the container.*

### [TinyIoC](https://github.com/grumpydev/TinyIoC)
```C#
var container = new TinyIoCContainer();

/* Initialize container */

var buildUpStrategy = new TinyIoCBuildUpStrategy(container);

StoryboardInjector.SetUp(buildUpStrategy);
```
*__Note__: Because TinyIoC is not installable as a DLL via [NuGet](http://www.nuget.org) (it is installed as a single C# source file), TinyIoC is included with the DependencySwizzler.TinyIoC [NuGet](http://www.nuget.org) package. If you want to install this package via NuGet, you __cannot__ do so with TinyIoC already installed in your project.*

### Custom
```C#
var buildUpStrategy = new CustomBuildUpStrategy(viewController => {
	/* custom logic to build up dependencies */
});

StoryboardInjector.SetUp(buildUpStrategy);
```

*You may also pass a delegate directly to the `SetUp()` method.*

```C#
StoryboardInjector.SetUp(viewController => {
	/* custom logic to build up dependencies */
});
```

## Installation
Installation is simplest via [NuGet](http://www.nuget.org). There are separate packages for the core library and each container implementation. You may also compile the source directly in your project.
- `Install-Package DependencySwizzler` ([NuGet Page](https://www.nuget.org/packages/DependencySwizzler/))
- `Install-Package DependencySwizzler.Unity` ([Nuget Page](https://www.nuget.org/packages/DependencySwizzler.Unity/))
- `Install-Package DependencySwizzler.TinyIoC` ([Nuget Page](https://www.nuget.org/packages/DependencySwizzler.TinyIoC/))

## License
This project is subject to the MIT license.