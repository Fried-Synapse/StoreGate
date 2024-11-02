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
    --detach \
    --name unityContainer \
    --env UNITY_EMAIL="$UNITY_EMAIL" \
    --env UNITY_PASSWORD="$UNITY_PASSWORD" \
    --env UNITY_LICENCE="$UNITY_LICENCE" \
    --env UNITY_SERIAL="$UNITY_SERIAL" \
    unity-editor

sleep 5

docker exec \
    unityContainer \
    ls -la < /dev/null

# docker exec \
#     unityContainer \
#     ./StoreGate/StoreGate unityCreatePackage \
#         --assetsPaths "$assetsPaths" \
#         --packageName "$packageName" \
#         < /dev/null