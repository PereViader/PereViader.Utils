#!/bin/bash

echo Remove previous package
rm -rf UnityPackageRelease

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

echo Copy all .cs files from the Common source to the destination, preserving folder structure
find "$SOURCE_COMMON" -type f -name "*.cs" | while read -r file; do
    # Calculate subdirectory path
    subdir=$(dirname "${file#$SOURCE_COMMON}")
    # Create the subdirectory in the destination if it doesn't exist
    mkdir -p "$DEST_COMMON$subdir"
    # Copy the file
    cp "$file" "$DEST_COMMON$subdir"
done



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

echo move package file to the root 
# Move the package version file to the root
mv "$DEST_UNITY3D/package.json" "$DEST_UNITY3D/../package.json"



# Directory containing the assets
start_dir="UnityPackageRelease"

# Generate a lowercase GUID in the Unity format
generate_guid() {
    # Try using uuidgen, if it fails use PowerShell
    (uuidgen 2>/dev/null || powershell -Command "[guid]::NewGuid().ToString()") | tr '[:upper:]' '[:lower:]' | sed 's/-//g'
}

# Function to generate a .meta file for .cs files
generate_cs_meta() {
    local path="$1"
    local guid=$(generate_guid)
    cat <<EOF >"$path.meta"
fileFormatVersion: 2
guid: $guid
MonoImporter:
  externalObjects: {}
  serializedVersion: 2
  defaultReferences: []
  executionOrder: 0
  icon: {instanceID: 0}
  userData: 
  assetBundleName: 
  assetBundleVariant: 
EOF
}

# Function to generate a .meta file for other files (non-.cs)
generate_text_meta() {
    local path="$1"
    local guid=$(generate_guid)
    cat <<EOF >"$path.meta"
fileFormatVersion: 2
guid: $guid
TextScriptImporter:
  externalObjects: {}
  userData: 
  assetBundleName: 
  assetBundleVariant: 
EOF
}

# Function to generate a .meta file for folders
generate_folder_meta() {
    local path="$1"
    local guid=$(generate_guid)
    cat <<EOF >"$path.meta"
fileFormatVersion: 2
guid: $guid
folderAsset: yes
DefaultImporter:
  externalObjects: {}
  userData: 
  assetBundleName: 
  assetBundleVariant: 
EOF
}

# Traverse the directory structure and generate .meta files
traverse_and_generate() {
    local dir="$1"
    for item in "$dir"/*; do
        # Skip if the item is a .meta file
        if [[ "$item" == *.meta ]]; then
            continue
        fi
        # Check if a .meta file already exists
        if [ ! -f "$item.meta" ]; then
            if [ -d "$item" ]; then
                echo "Generating .meta for directory: $item"
                generate_folder_meta "$item"
                traverse_and_generate "$item"  # Recursive call for subdirectories
            elif [ -f "$item" ]; then
                if [[ "$item" == *.cs ]]; then
                    # Generate .meta for .cs files
                    echo "Generating .meta for script: $item"
                    generate_cs_meta "$item"
                else
                    # Generate .meta for non-.cs files
                    echo "Generating .meta for other file: $item"
                    generate_text_meta "$item"
                fi
            fi
        fi
    done
}

# Start the process
traverse_and_generate "$start_dir"

echo "Meta files generation complete."


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

# Function to generate a .meta file for folders
generate_common_meta() {
    local path="$1"
    cat <<EOF >"$path.meta"
fileFormatVersion: 2
guid: a70aabf67ee715c48a729eeabba229c7
labels:
- RoslynAnalyzer
PluginImporter:
  externalObjects: {}
  serializedVersion: 2
  iconMap: {}
  executionOrder: {}
  defineConstraints: []
  isPreloaded: 0
  isOverridable: 0
  isExplicitlyReferenced: 0
  validateReferences: 1
  platformData:
  - first:
      : Any
    second:
      enabled: 0
      settings:
        Exclude Editor: 1
        Exclude Linux64: 1
        Exclude OSXUniversal: 1
        Exclude Win: 1
        Exclude Win64: 1
  - first:
      Any: 
    second:
      enabled: 0
      settings: {}
  - first:
      Editor: Editor
    second:
      enabled: 0
      settings:
        CPU: AnyCPU
        DefaultValueInitialized: true
        OS: AnyOS
  - first:
      Standalone: Linux64
    second:
      enabled: 0
      settings:
        CPU: None
  - first:
      Standalone: OSXUniversal
    second:
      enabled: 0
      settings:
        CPU: None
  - first:
      Standalone: Win
    second:
      enabled: 0
      settings:
        CPU: None
  - first:
      Standalone: Win64
    second:
      enabled: 0
      settings:
        CPU: None
  - first:
      Windows Store Apps: WindowsStoreApps
    second:
      enabled: 0
      settings:
        CPU: AnyCPU
  userData: 
  assetBundleName: 
  assetBundleVariant: 
EOF
}

echo Generating meta for PereViader.Utils.Common.Generators.dll
generate_common_meta "UnityPackageRelease/PereViader.Utils.Common.Generators.dll"

echo Package publishing complete