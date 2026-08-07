using UnityEngine;

namespace AtomicWar._Game.Utilities
{
    /// <summary>
    /// Backing store for runtime art/audio lookups by path.
    ///
    /// The indirection exists so the project is not welded to <c>Resources</c>.
    /// Everything under a Resources folder is force-included in every build and
    /// stays resident, which is the wrong default once there are thousands of item
    /// sprites. Swapping in an Addressables-backed provider later should not require
    /// touching a single gameplay or UI call site.
    /// </summary>
    public interface IGameAssetProvider
    {
        /// <summary>
        /// Load an asset at <paramref name="path"/>, or null when absent.
        /// Implementations must not throw for a missing asset — a missing sprite is
        /// an art-pipeline gap, not a crash.
        /// </summary>
        T Load<T>(string path) where T : Object;
    }

    /// <summary>
    /// <c>Resources</c>-backed provider. Correct for the project's current size and
    /// the only option without adding the Addressables package.
    /// </summary>
    public sealed class ResourcesAssetProvider : IGameAssetProvider
    {
        public T Load<T>(string path) where T : Object
        {
            if (string.IsNullOrEmpty(path)) return null;
            return Resources.Load<T>(path);
        }
    }
}
