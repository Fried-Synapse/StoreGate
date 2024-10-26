#!/bin/sh

GetSerialFromLicence() 
{
    developerData=$(echo "$1" | sed -n 's/.*<DeveloperData Value="\([^"]*\)".*/\1/p')
    
    if [ -z "$developerData" ]; then
        echo "Error: DeveloperData value not found."
        exit 1
    fi

    # Remove the last 4 characters
    serial=${developerData:0:${#developerData}-4}

    echo "$serial"
}

UnitySerial="$UNITY_SERIAL"

# Extract Unity Serial from License if provided
if [ -n "$UNITY_LICENSE" ]; then
  UnitySerial=$(GetSerialFromLicence "$UNITY_LICENSE")
fi

echo "serial $UnitySerial"

# Activate Unity license using the provided credentials
unity-editor -batchmode -quit \
  -username "$UNITY_EMAIL" \
  -password "$UNITY_PASSWORD" \
  -serial "$UnitySerial" \
  -logFile /dev/stdout 