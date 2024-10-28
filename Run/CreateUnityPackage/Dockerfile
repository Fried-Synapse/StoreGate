FROM unityci/editor:UNITY_VERSION
# FROM alpine:latest

COPY . /UnityProject

WORKDIR /UnityProject

ENTRYPOINT ["./StoreGate/StoreGate", 
    "unityCreatePackage", 
    "--assetsPaths \"Assets/StoreGate\"", 
    "--packageName \"StoreGate\""""] 