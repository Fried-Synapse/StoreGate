# FROM unityci/editor:UNITY_VERSION
FROM alpine:latest

RUN pwd

# COPY ../sh/ActivateUnity.sh /workspace
COPY index.sh /workspace

WORKDIR /workspace

RUN chmod +x /workspace/ActivateUnity.sh /workspace/index.sh

ENTRYPOINT ["index.sh"]
