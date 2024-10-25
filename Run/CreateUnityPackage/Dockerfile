# FROM unityci/editor:UNITY_VERSION
FROM alpine:latest

WORKDIR /workspace

# Debugging: List contents of /workspace
RUN ls -la /workspace

RUN chmod +x /workspace/ActivateUnity.sh /workspace/index.sh

# Set the entrypoint to your script
ENTRYPOINT ["index.sh"]
