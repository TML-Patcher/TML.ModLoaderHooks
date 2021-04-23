# TML.ModLoaderHooks
TML.ModLoaderHooks is a .NET 5.0 console application that uses MonoMod's run-time HookGen software to automatically generate MonoMod hooks for the tModLoader executable.
The tModLoader-packaged TerrariaHooks DLL only comes with hooks for types not located in the Terraria.ModLoader namespace.
This program generates a DLL the same way tModLoader does for TerrariaHooks, but instead of removing all types in Terraria.ModLoader, it removes all types *not* located in Terraria.ModLoader.

## Usage
Usage is simple. All you need is .NET 5.0 installed.

1. Download the latest release.
2. Extract the downloaded .zip file.
3. Take your tModLoader executable (**or** the DLL file if you're using the .NET Core version of tML 1.4), and drop it on **TML.ModLoaderHooks.exe**.
4. Wait for it to finish creating the DLL.
5. Navigate to \Generated\ and copy the newly-generated "ModLoaderHooks" DLL.
6. Add the DLL as instructed in [the tML wiki](https://github.com/tModLoader/tModLoader/wiki/Expert-Cross-Mod-Content#strong-references-aka-modreferences-expert).
