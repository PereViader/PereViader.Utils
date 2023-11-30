# PereViader.Utils

## Overview
`PereViader.Utils` is a collection of C# utility libraries designed to streamline programming tasks across various contexts. 

Given that this library is primarily intended for my personal use and is currently in its early development phases, it is essential to clarify that extensive efforts toward ensuring backward compatibility will not be a primary focus. Consequently, users should be prepared for the possibility of encountering changes that may disrupt functionality if they opt to utilize the library and subsequently update it in the future.

This repository is divided into three main projects: `PereViader.Utils.Common`, `PereViader.Utils.Unity3d`, and `PereViader.Utils.Unity3dCodegen`, each catering to different needs and environments.

Developed primarily for my personal use as a game developer, these libraries reflect a practical approach to common challenges in game development. I've made them public to assist others in the community, understanding that while my solutions are tailored to game development, they may be beneficial in a variety of programming contexts.

### PereViader.Utils.Common
This library offers a suite of general utility functions and extensions that are useful in a wide range of C# applications, providing streamlined solutions for common programming tasks.

### PereViader.Utils.Unity3d
Tailored for Unity3D developers, this library includes a set of utilities and extensions specifically designed to enhance the development experience in Unity3D.

### PereViader.Utils.Unity3dCodegen
Focusing on source generation for Unity3D, this project provides tools and utilities that automate and simplify code generation within the Unity3D environment, enhancing efficiency and productivity.

## Installation
For now, this project is still a work in progress and thus cannot be easily installed. You can manually install it in your project by copying and pasting the relevant C# files. 
I have plans to publish this project in the following ways:
- A NuGet package with `PereViader.Utils.Common`
- A UPM (Unity Package Manager) package published on OpenUPM with the contents of both `PereViader.Utils.Common` and `PereViader.Utils.Unity3d`
- As code generators in Unity still can't be included through OpenUPM, my current plan is to publish the DLL to NuGet and as a release on GitHub, which you can download and include in your project manually.

## Contributing
We welcome contributions to all parts of PereViader.Utils. There are currently no contributing guidelines, but all PRs are welcome. We can continue the conversation there.


## External projects

### Workflow
When developing in Unity3d, these assets are an integral part of my development plan to a quick add menu for the following projects.
- https://github.com/Guillemsc/ImplementationSelector
- https://github.com/PereViader/ManualDi.Main
- https://github.com/PereViader/ManualDi.Unity3d
- https://github.com/PereViader/ManualReserialization

### Benchmarking and Performance analysis
- https://github.com/dotnet/BenchmarkDotNet
- https://github.com/needle-tools/compilation-visualizer
- https://github.com/Unity-Technologies/UnityDataTools
- https://github.com/Unity-Technologies/ProjectAuditor
- https://superluminal.eu/

## CI and CD
- https://github.com/PereViader/CSharpProjectToUnity3dPackage


## Acknowledgements
[@Guillem Sunyer](https://github.com/Guillemsc)
