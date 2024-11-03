# StoreGate

Set of GitHub actions that can be used to release libraries with 1 click.

## Steps

### 1. Update Version
`Fried-Synapse/StoreGate/Step/UpdateVersion@v1`
Updates the version of the library based on the selected input.

- **Environment Variables**:
  - `ACCESS_TOKEN`: Access token stored in GitHub secrets.
- **Inputs**:
  - `type`: The type of version update (`major`/`minor`/`patch`)

```yaml
      - name: UpdateVersion
        id: updateVersion
        uses: Fried-Synapse/StoreGate/Step/UpdateVersion@v1
        env:
          ACCESS_TOKEN: ${{ secrets.ACCESS_TOKEN }}
        with:
          type: ${{ github.event.inputs.versionUpdatePart }}
```

### 2. Release to GitHub
`Fried-Synapse/StoreGate/Step/ReleaseGitHub@v1`
Releases the specified file to GitHub.

- **Environment Variables**:
  - `ACCESS_TOKEN`: Access token stored in GitHub secrets.
- **Inputs**:
  - `version`: The updated version of the library.
  - `name`: The name of the package file, including the version.
  - `file`: The path to the created package.

```yaml
      - name: Release GitHub
        uses: Fried-Synapse/StoreGate/Step/ReleaseGitHub@v1
        env:
          ACCESS_TOKEN: ${{ secrets.ACCESS_TOKEN }}
        with:
          version: ${{ steps.updateVersion.outputs.version }}
          name: StoreGate.v${{ steps.updateVersion.outputs.version }}.zip
          file: StoreGate.zip
```

### 3. Create a Unity package
`Fried-Synapse/StoreGate/Step/CreateUnityPackage@v1`
Creates a Unity package using the specified assets and name.

- **Environment Variables**:
  - `UNITY_VERSION`: Specifies the Unity version (e.g., `ubuntu-2022.3.39f1-android-3`). Find all options at [GameCI's docker hub](https://hub.docker.com/r/unityci/editor/tags).
  - `UNITY_LICENCE`: Unity license stored in GitHub secrets.
  - `UNITY_USERNAME`: Unity username stored in GitHub secrets.
  - `UNITY_PASSWORD`: Unity password stored in GitHub secrets.
- **Inputs**:
  - `assetsPaths`: The path to the assets (e.g., `Assets/StoreGate`).
  - `packageName`: The name of the package.

```yaml
      - name: Create Package
        id: createPackage
        uses: Fried-Synapse/StoreGate/Step/CreateUnityPackage@v1
        env:
          UNITY_VERSION: "ubuntu-2022.3.39f1-android-3"
          UNITY_LICENCE: ${{ secrets.UNITY_LICENCE }}
          UNITY_USERNAME: ${{ secrets.UNITY_USERNAME }}
          UNITY_PASSWORD: ${{ secrets.UNITY_PASSWORD }}
        with:
          assetsPaths: Assets/StoreGate
          packageName: StoreGate.v${{ steps.updateVersion.outputs.version }}
```

## Examples

Release a unity package: [Action](https://github.com/Fried-Synapse/StoreGate/blob/v1/.github/workflows/Release.yaml)
