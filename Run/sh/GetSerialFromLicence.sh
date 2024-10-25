#!/bin/bash

developerData=$(echo "$1" | grep -oP '(?<=<DeveloperData Value=")[^"]*')

# Check if extraction was successful
if [ -z "$developerData" ]; then
    echo "Error: DeveloperData value not found."
    exit 1
fi

# Remove the last 4 characters
serial=${developerData:0:${#developerData}-4}

echo "$serial"