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
*Note: With Unity you must use the `[Dependency]` attribute for property-setter injection, or you may 
optionally configure the strategy to add a Unity extension and remove the attribute requirement. In this case,
all publicly settable properties of a type registered with the container will be build up.*
```C#
var buildUpStrategy = new UnityBuildUpStrategy(container, configurePropertyInjectionExtension: true);
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

*You may also pass a delegate directly to the `SetUp()` method.*

```C#
StoryboardInjector.SetUp(viewController => {
	/* custom logic to build up dependencies */
});
```

## Installation
Installation is simplest via NuGet. There are separate packages for the core library and each container implementation. You may also compile the source directly in your project.

## License
This project is subject to the MIT license.