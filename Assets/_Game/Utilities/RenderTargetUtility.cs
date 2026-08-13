using UnityEngine;

namespace AtomicWar._Game.Utilities
{
    /// <summary>
    /// Lifecycle helper for RenderTextures, TargetTextures, and dynamic Textures.
    /// Ensures native surfaces are released from Unity's RenderTargetMap and internal
    /// IdMap before object destruction, preventing leaks across scene transitions,
    /// edit-mode tests, and domain reloads.
    /// </summary>
    public static class RenderTargetUtility
    {
        /// <summary>
        /// Explicitly unbinds, releases native hardware render surfaces, and destroys the RenderTexture object.
        /// Prevents "Internal: Possible leak. All render target surfaces should be removed from IdMap" errors.
        /// </summary>
        public static void SafeRelease(ref RenderTexture rt)
        {
            if (rt == null) return;

            try
            {
                if (RenderTexture.active == rt)
                {
                    RenderTexture.active = null;
                }

                if (rt.IsCreated())
                {
                    rt.Release();
                }

                if (Application.isEditor && !Application.isPlaying)
                {
                    Object.DestroyImmediate(rt);
                }
                else
                {
                    Object.Destroy(rt);
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"[RenderTargetUtility] Exception during RenderTexture teardown: {ex.Message}");
            }
            finally
            {
                rt = null;
            }
        }

        /// <summary>
        /// Safely releases a temporary RenderTexture allocated via RenderTexture.GetTemporary().
        /// </summary>
        public static void SafeReleaseTemporary(RenderTexture tempRt)
        {
            if (tempRt == null) return;

            try
            {
                if (RenderTexture.active == tempRt)
                {
                    RenderTexture.active = null;
                }

                RenderTexture.ReleaseTemporary(tempRt);
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"[RenderTargetUtility] Exception during ReleaseTemporary: {ex.Message}");
            }
        }

        /// <summary>
        /// Unbinds a camera's targetTexture, releases the underlying RenderTexture, and clears camera target.
        /// </summary>
        public static void SafeReleaseCameraTarget(Camera cam)
        {
            if (cam == null) return;

            var target = cam.targetTexture;
            if (target != null)
            {
                cam.targetTexture = null;
                SafeRelease(ref target);
            }
        }

        /// <summary>
        /// Safely destroys a dynamically generated Texture2D in both Editor and runtime.
        /// </summary>
        public static void SafeDestroy(ref Texture2D tex)
        {
            if (tex == null) return;

            try
            {
                if (Application.isEditor && !Application.isPlaying)
                {
                    Object.DestroyImmediate(tex);
                }
                else
                {
                    Object.Destroy(tex);
                }
            }
            finally
            {
                tex = null;
            }
        }

        /// <summary>
        /// Safely destroys a dynamically created Sprite along with its underlying texture if standalone.
        /// </summary>
        public static void SafeDestroy(ref Sprite sprite, bool destroyTexture = false)
        {
            if (sprite == null) return;

            try
            {
                Texture2D tex = destroyTexture ? sprite.texture : null;

                if (Application.isEditor && !Application.isPlaying)
                {
                    Object.DestroyImmediate(sprite);
                }
                else
                {
                    Object.Destroy(sprite);
                }

                if (tex != null)
                {
                    SafeDestroy(ref tex);
                }
            }
            finally
            {
                sprite = null;
            }
        }
    }
}
