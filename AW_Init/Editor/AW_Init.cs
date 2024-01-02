using System.IO;
using ANGELWARE.AW_ADK.Editor;
using UnityEditor;
using UnityEngine;

namespace ANGELWARE.AW_Init
{
    [InitializeOnLoad]
    public class AW_Init
    {
        #region Instantiate Class

        private static AW_Init _instance;

        static AW_Init()
        {
            _instance = new AW_Init();
            _instance.AW_InitMethod();
        }

        public static AW_Init Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AW_Init();
                    _instance.AW_InitMethod();
                }

                return _instance;
            }
        }

        #endregion


        #region Instantiate Internal Classes

        private readonly AW_Logger _logger;
        private readonly AW_OpenUpmManager _openUpmManager;
        private AW_InitPackageManifest _manifest;

        internal AW_Init()
        {
            _manifest =
                AssetDatabase.LoadAssetAtPath<AW_InitPackageManifest>(
                    "Assets/ANGELWARE/COMMON/SCRIPTS/AW_Init/dependencies.asset");
            _logger = new AW_Logger();
            _openUpmManager = new AW_OpenUpmManager(_logger, _manifest);
        }

        #endregion


        private void AW_InitMethod()
        {
            _logger.LogInfo("Initiating dependency check before initiating package import...");

            
            var packages = _manifest.packages;

            var failCount = 0;

            foreach (var package in packages)
            {
                if (Directory.Exists(package.packagePath)) continue;

                var dialogComplex = EditorUtility.DisplayDialogComplex("Initialization",
                    $"The package or dependency {package.packageName} is missing. Please install it before trying to import this package!",
                    "Add to VCC", "Okay, I'll do it myself.", "I know what I'm doing, skip this check.");

                switch (dialogComplex)
                {
                    case 0:
                        Application.OpenURL(package.packageRepoUrl);
                        failCount++;
                        break;
                    case 1:
                        failCount++;
                        break;
                    case 2:
                        EditorUserSettings.SetConfigValue("AW_SkipCheck", "true");
                        break;
                }
            }

            if (failCount > 0) return;

            if (Directory.Exists(_manifest.packagePath))
                AssetDatabase.ImportPackage(_manifest.packagePath, true);
            else
                _logger.LogError("Critical Error! Package couldn't be found!");
        }
    }
}