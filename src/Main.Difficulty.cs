// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : XP-01 Campaign Difficulty Host Binding
// Subsystem    : Difficulty authority (catalog + director + scalar provider)
// ============================================================================
// The director is a read-only scalar provider (W1 invariant): it owns no
// endings, completion history, UI state, or RNG. This partial only loads the
// canonical catalog, resolves the campaign's immutable preset into typed
// scalars, and exposes the provider to premise-checked consumers. A missing
// or invalid catalog keeps every consumer on the legacy all-ones provider —
// loud, never silent, never a crash on the day tick.
using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Difficulty;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private DifficultyDirector? _difficultyDirector;
        private DifficultyScalarsProvider _difficultyScalars = DifficultyScalarsProvider.Legacy;
        private string _difficultyPresetId = string.Empty;
        private string? _cliDifficultyPresetId;

        /// <summary>
        /// Typed difficulty scalars for premise-checked consumers. Starts at
        /// the legacy all-ones provider; only <see cref="SetupDifficulty"/>
        /// replaces it.
        /// </summary>
        public DifficultyScalarsProvider DifficultyScalars => _difficultyScalars;

        /// <summary>Resolved preset id (empty until a campaign binds one).</summary>
        public string CurrentDifficultyPresetId => _difficultyPresetId;

        /// <summary>
        /// Bind the campaign difficulty: load the canonical catalog, resolve
        /// the immutable preset id (null/empty ⇒ catalog default), and publish
        /// the scalar provider. Unknown preset ids resolve loudly to the
        /// legacy provider rather than aborting the campaign load.
        /// </summary>
        private void SetupDifficulty(string? campaignPresetId)
        {
            try
            {
                string dataDir = CatalogPath.ResolveDataDir();
                var fileIo = CatalogPath.CreateFileIOForDataDir(dataDir);
                var catalog = DifficultyPresetCatalogLoader.Load(dataDir, fileIo);
                _difficultyDirector = new DifficultyDirector(catalog);
                DifficultyScalarsProvider provider =
                    _difficultyDirector.ResolveProvider(campaignPresetId);
                _difficultyScalars = provider;
                _difficultyPresetId = provider.PresetId;
                GD.Print($"[Difficulty] Campaign preset '{provider.PresetId}' bound.");
            }
            catch (Exception ex)
            {
                _difficultyDirector = null;
                _difficultyScalars = DifficultyScalarsProvider.Legacy;
                _difficultyPresetId = string.Empty;
                string requested = campaignPresetId ?? "(default)";
                GD.PrintErr(
                    $"[Difficulty] Bind failed for preset '{requested}'; " +
                    $"legacy scalar behavior retained: {ex.Message}");
            }
        }

        /// <summary>
        /// Restore-path binding: the envelope manifest is the authority for
        /// the campaign's preset; legacy v1 manifests carry no id and resolve
        /// to the catalog default.
        /// </summary>
        private void SetupDifficultyFromSave()
        {
            string? presetId = null;
            if (_saveLoadHost != null &&
                _saveLoadHost.TryGetActiveDifficultyPresetId(out string fromManifest))
            {
                presetId = fromManifest;
            }
            SetupDifficulty(presetId);
        }
    }
}
