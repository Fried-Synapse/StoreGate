FROM unityci/editor:UNITY_VERSION

WORKDIR /workspace


ARG GITHUB_ACTION_PATH
ENV GITHUB_ACTION_PATH=${GITHUB_ACTION_PATH}

# Copy the license activation script
COPY ${GITHUB_ACTION_PATH}/../sh/ActivateUnity.sh ActivateUnity.sh
RUN chmod +x ActivateUnity.sh

# Copy the entrypoint script into the container
COPY ${GITHUB_ACTION_PATH}/index.sh index.sh
RUN chmod +x index.sh

# Set the entrypoint to your script
ENTRYPOINT ["index.sh"]
