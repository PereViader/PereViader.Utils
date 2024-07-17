#!/bin/bash

sh ./PereViader.Utils.Unity3d/Assets/Tools/GenerateUnityPackageReleaseFolder.sh
sh ./PereViader.Utils.Unity3d/Assets/Tools/GenerateUnity3dMetas.sh
cd UnityPackageRelease
npm pack