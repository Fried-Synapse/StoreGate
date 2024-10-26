# FROM unityci/editor:UNITY_VERSION
FROM alpine:latest

COPY ../../sh/ActivateUnity.sh /workspace/ActivateUnity.sh
COPY index.sh /workspace/index.sh

WORKDIR /workspace

RUN chmod +x /workspace/ActivateUnity.sh /workspace/index.sh

ENTRYPOINT ["index.sh"]
