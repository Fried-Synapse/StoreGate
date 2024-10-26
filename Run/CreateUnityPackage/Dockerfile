# FROM unityci/editor:UNITY_VERSION
FROM alpine:latest

COPY . /UnityProject

RUN ls -l .
RUN ls -l /UnityProject

ENTRYPOINT ["/UnityProject/Index.sh"]