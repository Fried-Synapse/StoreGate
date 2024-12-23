projectPath="$1"
assetsPaths="$2"
packageName="$3"
packagePath="./$packageName.unitypackage"

cp -r "$GITHUB_ACTION_PATH/Dockerfile" "$projectPath/Dockerfile"
cp -r "$GITHUB_ACTION_PATH/../bin" "$projectPath/StoreGate"
cp -r "$GITHUB_ACTION_PATH/../../StoreGate.Unity/Assets/StoreGate" "$projectPath/Assets/StoreGate"
sed -i "s/UNITY_VERSION/$UNITY_VERSION/" "$projectPath/Dockerfile"

docker build \
    --quiet \
    --tag unity-editor \
    "$projectPath"

docker run \
    --detach \
    --tty \
    --name unity-editor-container \
    --env UNITY_USERNAME="$UNITY_USERNAME" \
    --env UNITY_PASSWORD="$UNITY_PASSWORD" \
    --env UNITY_LICENCE="$UNITY_LICENCE" \
    --env UNITY_SERIAL="$UNITY_SERIAL" \
    unity-editor \
    tail -f /dev/null

docker exec unity-editor-container /bin/sh -c "\
    ./StoreGate/StoreGate unityCreatePackage \
        --assetsPaths \"$assetsPaths\" \
        --packageName \"$packageName\""

docker cp "unity-editor-container:/UnityProject/$packageName.unitypackage" $packagePath

echo "packagePath=\"$packagePath\"" >> $GITHUB_OUTPUT 