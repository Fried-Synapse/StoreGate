# FROM unityci/editor:UNITY_VERSION
FROM alpine:latest

COPY . /UnityProject

WORKDIR /UnityProject

ENTRYPOINT ["./StoreGate/StoreGate", "unityCreatePackage"]
CMD ["--assetsPaths", "${ASSETS_PATHS}", "--packageName", "${PACKAGE_NAME}"]

RUN ls -la .