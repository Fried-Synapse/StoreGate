#!/bin/bash

UnityVersion="$1"
UnityProjectPath="$2"
PackagePath="$3"
PackageName="$4"
UnitySerial="$UNITY_SERIAL"

# Extract Unity Serial from License if provided
if [ -n "$UNITY_LICENSE" ]; then
  UnitySerial=$($GITHUB_ACTION_PATH/../sh/GetSerialFromLicence.sh "$UNITY_LICENSE")
fi

echo "First 4 characters of UNITY_EMAIL: ${UNITY_EMAIL}"

# Run Docker to build Unity Package
docker run --rm \
  -v "$GITHUB_WORKSPACE":"$GITHUB_WORKSPACE" \
  -w "$GITHUB_WORKSPACE" \
  unityci/editor:"$UnityVersion" \
  /bin/bash -c "
    # Activate Unity license using the provided credentials
    unity-editor -batchmode -quit \
      -username \"$UNITY_EMAIL\" \
      -password \"$UNITY_PASSWORD\" \
      -serial \"$UnitySerial\" \
      -logFile /dev/stdout && \
    # Export the package
    unity-editor -batchmode -quit \
      -projectPath \"$UnityProjectPath\" \
      -exportPackage \"$PackagePath\" \"$PackageName.unitypackage\" \
      -logFile /dev/stdout"
