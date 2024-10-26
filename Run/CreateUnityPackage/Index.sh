#!/bin/sh

PackagePath="$1"
PackageName="$2"

echo "ls ."
ls -l .
echo "ls UnityProject"
ls -l UnityProject

./ActivateUnity.sh
echo "index from inside"

# unity-editor -batchmode -quit \
#   -projectPath "${UNITY_PROJECT_PATH}" \
#   -exportPackage "$PackagePath" "$PackageName.unitypackage" \
#   -logFile /dev/stdout