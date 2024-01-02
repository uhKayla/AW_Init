using System;
using System.Collections.Generic;
using UnityEngine;

namespace ANGELWARE.AW_Init
{
    [Serializable]
    [CreateAssetMenu(fileName = "AW_InitPackageManifest", menuName = "ANGELWARE/Package Manifest", order = 0)]
    public class AW_InitPackageManifest : ScriptableObject
    {
        public List<AW_DependentPackage> packages;
        public string packagePath;
        public List<AW_UPM> upmPackages;
    }

    [Serializable]
    public class AW_DependentPackage
    {
        public string packageName;
        public string packageRepoUrl;
        public string packagePath;
        public string versionString;
    }

    [Serializable]
    public class AW_UPM
    {
        public string packageName;
        public string packageUrl;
        public string packageString;
        public string packageVersion;
    }
}