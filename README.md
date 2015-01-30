iOS Dependency Swizzler
=======================

> Uses "method swizzling" from the Objective-C runtime to allow property-setter dependency injection of UIViewControllers instantiated by storyboards.

## Examples
### Unity
```C#
var container = new UnityContainer();

/* Initialize container */

var buildUpStrategy = new UnityBuildUpStrategy(container);

StoryboardInjector.SetUp(buildUpStrategy);

```
*Note: With Unity you must use the `[Dependency]` attribute for property-setter injection, or you may optionally use the provided extension method to inject any properties with types registered in the container.*
```C#
container.AddSetterBuildUpExtension();
```

### TinyIoC
```C#
var container = new TinyIoCContainer();

/* Initialize container */

var buildUpStrategy = new TinyIoCBuildUpStrategy(container);

StoryboardInjector.SetUp(buildUpStrategy);
```

### Custom
```C#
var buildUpStrategy = new CustomBuildUpStrategy(viewController => {
	/* custom logic to build up dependencies */
});

StoryboardInjector.SetUp(buildUpStrategy);
```

## Installation
Installation is simplest via NuGet. There are separate packages for the core library and each container implementation. You may also compile the source directly in your project.

## License
This project is subject to the MIT license.