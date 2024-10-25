#!/bin/bash

# Read the XML input from the parameter
licence="$1"

# Use xmllint to extract DeveloperData value
developerData=$(echo "$licence" | xmllint --xpath 'string(//DeveloperData/@Value)' -)

# Check if extraction was successful
if [ -z "$developerData" ]; then
    echo "Error: DeveloperData value not found."
    exit 1
fi

# Remove the last 4 characters
serial=${developerData:0:${#developerData}-4}

echo "$serial"