#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace AtomicWar._Game.Editor
{
    /// <summary>
    /// One-shot batchmode entry for remaining editor work:
    /// catalogs, placeholder sprites, import settings, SO assignment, AI prompt files.
    ///
    /// -executeMethod AtomicWar._Game.Editor.RemainingWorkPipeline.Run
    /// </summary>
    public static class RemainingWorkPipeline
    {
        [MenuItem("Tools/ASHFALL/Run Remaining Work Pipeline")]
        public static void Run()
        {
            CatalogGenerator.GenerateAll();
            ArtAssetImporter.GeneratePlaceholdersAndAssign();
            PromptGeneratorWindow.GenerateAllFromManifest();
            Debug.Log("[ASHFALL] RemainingWorkPipeline finished.");
        }
    }
}
#endif
