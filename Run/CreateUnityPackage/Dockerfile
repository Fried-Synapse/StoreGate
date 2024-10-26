# FROM unityci/editor:UNITY_VERSION
FROM alpine:latest

RUN pwd

# COPY ../sh/ActivateUnity.sh /workspace
COPY index.sh .

ENTRYPOINT ["index.sh"]
