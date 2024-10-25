ARG GITHUB_ACTION_PATH
ENV GITHUB_ACTION_PATH=${GITHUB_ACTION_PATH}

FROM unityci/editor:UNITY_VERSION

WORKDIR /workspace

RUN echo "GITHUB_ACTION_PATH is set to: ${GITHUB_ACTION_PATH}"

# Print the current directory path
RUN pwd

# List contents of /workspace (initially empty unless files are added in previous layers)
RUN ls -la /workspace

# Copy the license activation script
COPY ${GITHUB_ACTION_PATH}/../sh/ActivateUnity.sh ActivateUnity.sh
RUN chmod +x ActivateUnity.sh

# Copy the entrypoint script into the container
COPY ${GITHUB_ACTION_PATH}/index.sh index.sh
RUN chmod +x index.sh

# Set the entrypoint to your script
ENTRYPOINT ["index.sh"]
