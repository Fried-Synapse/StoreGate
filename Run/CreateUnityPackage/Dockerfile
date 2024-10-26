# FROM unityci/editor:UNITY_VERSION
FROM alpine:latest

COPY . /UnityProject

RUN ls -l /UnityProject
RUN cat /UnityProject/Index.sh

RUN /UnityProject/Index.sh