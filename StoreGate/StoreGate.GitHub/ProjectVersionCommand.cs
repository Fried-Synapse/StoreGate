// using StoreGate.GitHub.Models;
//
// namespace StoreGate.GitHub;
//
// public class ProjectVersionCommand
// {
//     private const string VersionRegex = "version=\"([^\"]*)\"";
//     private string Version { get; set; }
//
//     public ProjectVersionCommand(GitHubData data, string variableName)
//         : base(data)
//     {
//     }
//
//
//     private void ReleaseVersionField()
//     {
//         EditorGUILayout.BeginHorizontal(ContentStyle);
//         GUILayout.FlexibleSpace();
//
//         EditorGUILayout.LabelField("Version:", GUILayout.Width(LabelWidth));
//         EditorGUILayout.LabelField(Version, GUILayout.Width(LabelContentWidth - 100));
//         string[] versionParts = Version.Substring(1).Split('.');
//         int major = int.Parse(versionParts[0]);
//         int minor = int.Parse(versionParts[1]);
//         int patch = int.Parse(versionParts[2]);
//
//         GUILayout.BeginVertical(GUILayout.ExpandWidth(false));
//         {
//             static int getNumber(string name, int value)
//             {
//                 EditorGUILayout.BeginHorizontal();
//
//                 EditorGUILayout.LabelField(name, GUILayout.Width(35));
//                 if (GUILayout.Button("^", GUILayout.MaxWidth(20)))
//                 {
//                     value++;
//                 }
//
//                 if (GUILayout.Button("˅", GUILayout.MaxWidth(20)))
//                 {
//                     value--;
//                 }
//
//                 if (GUILayout.Button("-", GUILayout.MaxWidth(20)))
//                 {
//                     value = 0;
//                 }
//
//                 GUILayout.EndVertical();
//
//                 return value;
//             }
//
//             major = getNumber("major", major);
//             minor = getNumber("minor", minor);
//             patch = getNumber("patch", patch);
//         }
//
//         GUILayout.EndVertical();
//
//         string version = $"v{major}.{minor}.{patch}";
//         if (Version != version)
//         {
//             Version = version;
//             SaveVersion();
//         }
//
//         GUILayout.FlexibleSpace();
//         EditorGUILayout.EndHorizontal();
//     }
//
//
//     #region Utils
//
//     private void Init()
//     {
//         if (Version != null)
//         {
//             return;
//         }
//
//         string config = File.ReadAllText(ConfigPath);
//         Match result = new Regex(VersionRegex).Match(config);
//         Version = result.Groups[1].Value;
//     }
//
//     private void SaveVersion()
//     {
//         string config = File.ReadAllText(ConfigPath);
//         config = new Regex(VersionRegex).Replace(config, $"version=\"{Version}\"");
//         File.WriteAllText(ConfigPath, config);
//     }
//
//     private void Release()
//     {
//         string directoryPath = Path.GetDirectoryName(FilePath);
//
//         if (!Directory.Exists(directoryPath))
//         {
//             Directory.CreateDirectory(directoryPath);
//         }
//
//         AssetDatabase.ExportPackage($"Assets/{Application.productName}", FilePath, ExportPackageOptions.Recurse);
//         Upload();
//     }
//
//     private void Upload()
//     {
//         ProcessStartInfo psi = new ProcessStartInfo();
//         psi.FileName = "/bin/sh";
//         psi.WorkingDirectory = Path.Combine(Application.dataPath, "Scripts/Editor");
//         psi.Arguments = "upload.sh " +
//                         $"\'{Application.productName}\' " +
//                         $"\'{Destination}\' " +
//                         $"\'{FileName}\' ";
//         psi.WindowStyle = ProcessWindowStyle.Minimized;
//         psi.CreateNoWindow = true;
//         psi.UseShellExecute = false;
//         psi.RedirectStandardOutput = true;
//         Process p = Process.Start(psi);
//         p.WaitForExit();
//         string strOutput = p.StandardOutput.ReadToEnd();
//         UnityDebug.Log($"{strOutput}");
//     }
//
//     #endregion
// }