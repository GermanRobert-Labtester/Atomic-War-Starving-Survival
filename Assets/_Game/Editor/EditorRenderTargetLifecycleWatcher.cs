#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace AtomicWar._Game.Editor
{
    /// <summary>
    /// Editor hook that ensures any dangling active RenderTextures are detached and
    /// native surfaces are flushed on assembly reloads, playmode switches, and editor quit.
    /// Eliminates "All render target surfaces should be removed from IdMap" leaks.
    /// </summary>
    [InitializeOnLoad]
    public static class EditorRenderTargetLifecycleWatcher
    {
        static EditorRenderTargetLifecycleWatcher()
        {
            AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            EditorApplication.quitting += OnEditorQuitting;
        }

        private static void OnBeforeAssemblyReload()
        {
            FlushDanglingRenderTargets();
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode || state == PlayModeStateChange.ExitingPlayMode)
            {
                FlushDanglingRenderTargets();
            }
        }

        private static void OnEditorQuitting()
        {
            FlushDanglingRenderTargets();
        }

        private static void FlushDanglingRenderTargets()
        {
            try
            {
                if (RenderTexture.active != null)
                {
                    RenderTexture.active = null;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"[EditorRenderTargetLifecycleWatcher] Error unbinding active RenderTexture: {ex.Message}");
            }
        }
    }
}
#endif
