#!/bin/bash

echo Remove previous package
rm -rf UnityPackageRelease
rm PereViader.Utils.Unity3d.tar.gz

echo Set the base paths for source and destination
SOURCE_UNITY3D="PereViader.Utils.Unity3d/Assets/PereViader.Utils.Unity3d"
DEST_UNITY3D="UnityPackageRelease/PereViader.Utils.Unity3d"

SOURCE_COMMON="PereViader.Utils.Common/PereViader.Utils.Common"
DEST_COMMON="UnityPackageRelease/PereViader.Utils.Common"

echo Ensure the destination directories exist
mkdir -p "$DEST_UNITY3D"
mkdir -p "$DEST_COMMON"

echo Copy the Utils Unity3d folder
cp -r "$SOURCE_UNITY3D/." "$DEST_UNITY3D"

echo Copy the Common folder
cp -r "$SOURCE_COMMON/." "$DEST_COMMON"

echo Delete unnecesary files from common
rm "UnityPackageRelease/PereViader.Utils.Common/PereViader.Utils.Common.csproj"
rm -rf "UnityPackageRelease/PereViader.Utils.Common/bin"
rm -rf "UnityPackageRelease/PereViader.Utils.Common/obj"

# Path to the .csproj file and package.json
CSPROJ_FILE="$SOURCE_COMMON/PereViader.Utils.Common.csproj"

# Extract version from the .csproj file
version=$(grep -oP '(?<=<Version>)[^<]+' "$CSPROJ_FILE")

# Check if we extracted a version
if [ -z "$version" ]; then
    echo "Version not found in $CSPROJ_FILE"
    exit 1
fi

echo "Found version $version"

# Replace the placeholder in the package.json file
sed -i "s/\"version\": \"\$version\"/\"version\": \"$version\"/g" "$DEST_UNITY3D/package.json"

echo "Version in package.json updated to $version"

echo Move package file to the root 
# Move the package version file to the root
mv "$DEST_UNITY3D/package.json" "$DEST_UNITY3D/../package.json"
mv "$DEST_UNITY3D/package.json.meta" "$DEST_UNITY3D/../package.json.meta"

echo Remove Unity3d test folder
rm -rf "$DEST_UNITY3D/Scripts/Test"
rm "$DEST_UNITY3D/Scripts/Test.meta"


echo Rename Samples directory to Samples~
mv "$DEST_UNITY3D/Samples" "$DEST_UNITY3D/../Samples~"
echo Remove Samples meta
rm "$DEST_UNITY3D/Samples.meta"


echo Move runtime assambly to root
mv "$DEST_UNITY3D/Scripts/Runtime/PereViader.Utils.Unity3d.Runtime.asmdef" "$DEST_UNITY3D/../PereViader.Utils.Unity3d.Runtime.asmdef" 
mv "$DEST_UNITY3D/Scripts/Runtime/PereViader.Utils.Unity3d.Runtime.asmdef.meta" "$DEST_UNITY3D/../PereViader.Utils.Unity3d.Runtime.asmdef.meta" 

echo Build generator
dotnet build PereViader.Utils.Common --configuration Release

echo Copy generator dll to package
cp "PereViader.Utils.Common/PereViader.Utils.Common.Generators/bin/Release/netstandard2.0/PereViader.Utils.Common.Generators.dll" "UnityPackageRelease/PereViader.Utils.Common.Generators.dll"

echo Generating meta for PereViader.Utils.Common.Generators.dll
cp "PereViader.Utils.Unity3d/Assets/Tools/Meta~/PereViader.Utils.Common.Generators.dll.meta" "UnityPackageRelease/PereViader.Utils.Common.Generators.dll"