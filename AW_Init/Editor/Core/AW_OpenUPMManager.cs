using ANGELWARE.AW_Init;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ANGELWARE.AW_ADK.Editor
{
    public class AW_OpenUpmManager
    {
        private AW_Logger _logger;
        private AW_InitPackageManifest _manifest;
        
        internal AW_OpenUpmManager(AW_Logger logger, AW_InitPackageManifest manifest)
        {
            _logger = logger;
            _manifest = manifest;
            FirstTimeSetup();
        }

        #if AW_DEBUG
        [MenuItem("ANGELWARE/Debug/Resolve OpenUPM Packages")]
        #endif
        public static void StartOpenUPMResolution()
        {
            var manifest = AssetDatabase.LoadAssetAtPath<AW_InitPackageManifest>("Assets/ANGELWARE/COMMON/SCRIPTS/AW_Init/dependencies.asset");
            var logger = new AW_Logger();
            var manager = new AW_OpenUpmManager(logger, manifest);

            manager.FirstTimeSetup();
        }

        private void FirstTimeSetup()
        {
            var runCheck = EditorUserSettings.GetConfigValue("AW_UPM");
    
            if (runCheck == "true") return;

            EditorApplication.delayCall += () =>
            {
                var setupType = EditorUtility.DisplayDialog("AW_ADK",
                    "In order to provide some functions, we require a few packages from OpenUPM. " +
                    "Would you like to set them up automatically? Or do you prefer to do it manually? (Advanced)",
                    "Automatically",
                    "Manually");
            };
            
            AddOpenUpmScopedRegistry();
        }

        private void AddOpenUpmScopedRegistry()
        {
           // create scope list for registry
            var scopes = new List<string>();
            foreach (var package in _manifest.packages)
                if (!string.IsNullOrWhiteSpace(package.packageName) && !scopes.Contains(package.packageName))
                    scopes.Add(package.packageName);

            // add registry entries
            EditorApplication.delayCall += () => AddScopedRegistry("package.openupm.com", "https://package.openupm.com", scopes);

            // after registry add packages
            foreach (var package in _manifest.upmPackages)
                EditorApplication.delayCall += () => AddPackage(package.packageUrl, package.packageVersion);
            
            EditorUserSettings.SetConfigValue("AW_UPM", "true");
        }

        private void AddScopedRegistry(string name, string url, List<string> scopes)
        {
            // Look for a matching scoped registry within the existing list
            var registry = _manifest.upmPackages.FirstOrDefault(x => x.packageUrl == url);

            // If none found, add a new one
            if (registry == null)
            {
                _manifest.upmPackages.Add(
                    new AW_UPM 
                    { 
                        packageUrl = url, 
                        packageName = name, 
                        packageString = String.Join(",", scopes) 
                    }
                );

                Debug.Log("Scoped Registry added successfully.");
            }
            else
            {
                Debug.Log("Scoped Registry already exists.");
            }
        }
        
        public void AddPackage(string packageName, string version)
        {
            AW_DependentPackage package = _manifest.packages.FirstOrDefault(p => p.packageName == packageName);

            if (package == null)
            {
                package = new AW_DependentPackage { packageName = packageName, versionString = version };

                _manifest.packages.Add(package);
                Debug.Log($"Package {packageName}@{version} added successfully.");
            }
            else
            {
                Debug.Log($"Package {packageName} already exists! Updating version to {version}...");
                package.versionString = version;
            }

            // It's good practice to mark the asset as dirty if you modified it outside 
            // of an undoable context, such as an interface event or the inspector.

            EditorUtility.SetDirty(_manifest);
            AssetDatabase.SaveAssets();
        }
    }

    // [Serializable]
    // public class PackageModel
    // {
    //     [SerializeField] public string packageUrl;
    //     [SerializeField] public string versionString;
    //     [SerializeField] public string packageString;
    // }
    //
    // [Serializable]
    // public class ScopedRegistry
    // {
    //     public string name;
    //     public string url;
    //     public List<string> scopes;
    // }
    //
    // [Serializable]
    // public class Manifest
    // {
    //     public Dictionary<string, string> dependencies = new();
    //     public List<ScopedRegistry> scopedRegistries = new();
    // }
}
