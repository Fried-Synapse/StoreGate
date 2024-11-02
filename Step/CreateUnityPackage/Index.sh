projectPath="$1"
assetsPaths="$2"
packageName="$3"

cp -r "$GITHUB_ACTION_PATH/Dockerfile" "$projectPath/Dockerfile"
cp -r "$GITHUB_ACTION_PATH/../bin" "$projectPath/StoreGate"
cp -r "$GITHUB_ACTION_PATH/../../Unity/Assets/StoreGate" "$projectPath/Assets/StoreGate"
sed -i "s/UNITY_VERSION/$UNITY_VERSION/" "$projectPath/Dockerfile"

docker build \
    --quiet \
    --tag unity-editor \
    "$projectPath"

docker run \
    --rm \
    --tty \
    --name unity-editor-container \
    --env UNITY_EMAIL="$UNITY_EMAIL" \
    --env UNITY_PASSWORD="$UNITY_PASSWORD" \
    --env UNITY_LICENCE="$UNITY_LICENCE" \
    --env UNITY_SERIAL="$UNITY_SERIAL" \
    unity-editor  \
    tail -f /dev/null

docker logs unity-editor-container

docker exec \
    --tty \
    unity-editor-container \
    /bin/sh -c "ls -la"

# docker exec \
#     --tty \
#     unity-editor-container \
#     /bin/sh -c "\
#         ./StoreGate/StoreGate unityCreatePackage \
#             --assetsPaths \"$assetsPaths\" \
#             --packageName \"$packageName\""