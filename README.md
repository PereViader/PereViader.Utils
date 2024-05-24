[![License: 0BSD](https://img.shields.io/badge/License-0BSD-yellow.svg)](https://opensource.org/license/0bsd) [![Test and publish](https://github.com/PereViader/PereViader.Utils/actions/workflows/TestAndPublish.yml/badge.svg)](https://github.com/PereViader/PereViader.Utils/actions/workflows/TestAndPublish.yml)

# PereViader.Utils

## Overview
The project `PereViader.Utils` is a collection of C# utilities meant to quickly start developing new stuff instead of needing to repeat fluff over and over again.

Given that this library is primarily intended for my personal use, extensive efforts won't be made towards ensuring backward compatibility. Consequently, be prepared to encounter functionality that has changed / been removed, if you opt to utilize the library and subsequently update it in the future.

The project consists of the the following "modules"

### PereViader.Utils.Common
Utilites that can be applied to any c# game / application

### PereViader.Utils.Common.Generators
Roslyn source generators that can be applied to any c# game / application

### PereViader.Utils.Unity3d
Utilites specifically tailored towards Unity3d game development
Notice the package provides samples to experiment with the functionality provided

### PereViader.Utils.Godot
Utilites specifically tailored towards Godot game development
I haven't delved into it much so there is not much to it for now

## Installation
- `PereViader.Utils.Common` and `PereViader.Utils.Common.Generators` can be installed through Nuget here https://www.nuget.org/packages/PereViader.Utils.Common/
- `PereViader.Utils.Unity3d` can be installed through Github Release https://github.com/PereViader/PereViader.Utils/releases/latest
  - Download the .tgz package
  - Add it to your project (don't add it within the  `Assets` folder. Make it a sibling for example)
  - Unity > Package Manager > + > Add package from tarball

## Contributing
We welcome bug reports and feature requests through issues.
We welcome contributions through PRs.
Beware that merging of PRs is not guaranteed given this is a very opinionated personal project. 

## External projects
This section is destined as a collection of useful third party projects that have previously given me good results and may want to use in the future. 

### Workflow
When developing in Unity3d, these assets are an integral part of my development plan to a quick add menu for the following projects.
- https://github.com/Guillemsc/ImplementationSelector : Serialize data classes as interfaces and get a dropdown to select the desired underlying implementation from the editor 
- https://github.com/PereViader/ManualDi.Main : C# Dependency injection container with 0 reflection
- https://github.com/PereViader/ManualDi.Unity3d : Integration of the dependency injection with Unity3d 
- https://github.com/PereViader/ManualReserialization : Reserialization utility for assets in Unity3d
- https://docs.unity3d.com/Packages/com.unity.localization@1.4 : Localization
- https://github.com/ayellowpaper/SerializedDictionary: A serializable dictionary library

### Benchmarking and Performance analysis
- https://github.com/dotnet/BenchmarkDotNet : Benchmarking library for c# code
- https://github.com/needle-tools/compilation-visualizer : Unity3d tool to visualize the compilation tree
- https://github.com/brunomikoski/SpriteAuditor: Benchmark sprite usages
- https://github.com/Unity-Technologies/UnityDataTools : Be able to programmatically query the asset database of unity from the outside. Can provide a sql database of the assets where sql queries can be done on them to programmatically look for issues on the assets
- https://github.com/Unity-Technologies/ProjectAuditor : Provides hints, tips and fixes to apply on your Unity3d project 
- https://superluminal.eu/ : Agnostic stack polling analyzer that polls at regular intervals and provides a completely external overview of the performance of applications

### Multiplatform
- https://github.com/VolodymyrBS/WebGLThreadingPatcher: Utility to be able to work with Tasks on webgl

## CI and CD
- https://github.com/PereViader/CSharpProjectToUnity3dPackage : Can turn a c# solution to the unity package manager format needed to be able to consume it by unity3d 

## Git
- https://github.com/rtyley/bfg-repo-cleaner : Easily remove unwanted data from the git history   

## Acknowledgements
[@Guillem Sunyer](https://github.com/Guillemsc)
