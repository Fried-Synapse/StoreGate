# FROM unityci/editor:UNITY_VERSION
FROM alpine:latest

RUN pwd

COPY ActivateUnity.sh .
COPY index.sh .

ENTRYPOINT ["index.sh"]
