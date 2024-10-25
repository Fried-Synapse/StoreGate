ARG UNITY_VERSION
ARG GITHUB_ACTION_PATH

RUN echo "MY_ENV_VAR is: $UNITY_VERSION"

FROM unityci/editor:${UNITY_VERSION}

# Set working directory
WORKDIR /workspace

# Copy the license extraction script
COPY ${GITHUB_ACTION_PATH}/../sh/GetSerialFromLicence.sh GetSerialFromLicence.sh
RUN chmod +x GetSerialFromLicence.sh

# Copy the entrypoint script into the container
COPY ${GITHUB_ACTION_PATH}/index.sh index.sh
RUN chmod +x index.sh

# Set the entrypoint to your script
ENTRYPOINT ["index.sh"]
