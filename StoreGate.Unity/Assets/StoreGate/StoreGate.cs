using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace StoreGate
{
    public class StoreGateHeadless
    {
        public static void CreatePackage()
        {
            List<string> args = GetArgs();
            string assetsPaths = args[0];
            string packageName = args[1];
            Debug.Log($"assetsPaths: {assetsPaths}");
            Debug.Log($"packageName: {packageName}");
            
            string[] assets = assetsPaths.Split(';');
            AssetDatabase.ExportPackage(assets, packageName, ExportPackageOptions.Recurse);
        }

        private static List<string> GetArgs()
        {
            List<string> args = Environment.GetCommandLineArgs().ToList();
            int index = args.IndexOf("-executeMethod") + 2;
            return args.Skip(index).ToList();
        }
    }
}