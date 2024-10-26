#!/bin/bash

PackagePath="$1"
PackageName="$2"

ls UnityProject

./ActivateUnity.sh

unity-editor -batchmode -quit \
  -projectPath "${UNITY_PROJECT_PATH}" \
  -exportPackage "$PackagePath" "$PackageName.unitypackage" \
  -logFile /dev/stdout
