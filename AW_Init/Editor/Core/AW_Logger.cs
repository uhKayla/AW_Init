using UnityEngine;

namespace ANGELWARE.AW_Init
{
    public class AW_Logger
    {
        internal void LogInfoDebug(string message)
        {
            #if AW_DEBUG
            Debug.Log($"AW_ADK: {message}");
            #endif
        }

        internal void LogWarningDebug(string message)
        {
            #if AW_DEBUG
            Debug.LogWarning($"AW_ADK: {message}");
            #endif
        }

        internal void LogErrorDebug(string message)
        {
            #if AW_DEBUG
            Debug.LogError($"AW_ADK: {message}");
            #endif
        }

        internal void LogInfo(string message)
        {
            Debug.Log($"AW_ADK: {message}");
        }

        internal void LogWarning(string message)
        {
            Debug.LogWarning($"AW_ADK: {message}");
        }

        internal void LogError(string message)
        {
            Debug.LogError($"AW_ADK: {message}");
        }
    }
}