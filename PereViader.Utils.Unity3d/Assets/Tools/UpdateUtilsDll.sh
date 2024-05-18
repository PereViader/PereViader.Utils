#!/bin/bash

dotnet build PereViader.Utils.Common -c Release

mkdir -p PereViader.Utils.Unity3d/Assets/Plugins/PereViader.Utils.Common/
rm -rf PereViader.Utils.Unity3d/Assets/Plugins/PereViader.Utils.Common/*

SourceCommonDll="PereViader.Utils.Common/PereViader.Utils.Common/bin/Release/netstandard2.1/PereViader.Utils.Common.dll"
DestinyCommonDll="PereViader.Utils.Unity3d/Assets/Plugins/PereViader.Utils.Common/PereViader.Utils.Common.dll"
cp $SourceCommonDll $DestinyCommonDll

SourceGeneratorDll="PereViader.Utils.Common/PereViader.Utils.Common.Generators/bin/Release/netstandard2.0/PereViader.Utils.Common.Generators.dll"
DestinyGeneratorDll="PereViader.Utils.Unity3d/Assets/Plugins/PereViader.Utils.Common/PereViader.Utils.Common.Generators.dll"
cp $SourceGeneratorDll $DestinyGeneratorDll

SourceGeneratorMeta="PereViader.Utils.Unity3d/Assets/Tools/Meta~/PereViader.Utils.Common.Generators.dll.meta"
DestinyGeneratorMeta="PereViader.Utils.Unity3d/Assets/Plugins/PereViader.Utils.Common/PereViader.Utils.Common.Generators.dll.meta"
cp $SourceGeneratorMeta $DestinyGeneratorMeta