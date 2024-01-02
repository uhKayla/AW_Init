# AW_Init

This is meant to be a dependency checker that runs before a package is imported. It has libraries or accessing UPM and 
VPM packages. 

Packages can be set up via scriptable objects.

### Usage
Use or create a scriptable object at the directory `Assets/ANGELWARE/COMMON/SCRIPTS/AW_Init/dependencies.asset` using the `Create > ANGELWARE > Dependencies` menu.

Add package names, vcc urls, and paths to folders you would like to check for (version support coming soon).

The package can programatically be removed from the project using your own script afterwards, we do not provide this for safety reasons.

You can also define openUPM packages for other automatic dependency maangement. It's recommended to use this for ease of use for the end-user opposed to VPM, as VPM packages currently cannot be added automatically.

It's recommended to create a blank project for setting this up, and use the example package file in releases.

### Building

This project builds outside Unity into a self-contained .NET DLL. This is just to make deleting it easier later, and packaging easier overall. If you prefer editor scripts you may attempt to use them instead by importing these scripts into an editor folder in the same location, replacing the `Plugins` folder for an `Editor` folder. This is not supported.

Debug and Release versions are available in releases for specific use-cases (Debug just logs more).

To build map dependencies in your IDE (Rider should find 2022.3.6f1 automatically) and build the desired configuration.