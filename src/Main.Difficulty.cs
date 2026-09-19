// SPDX-License-Identifier: MIT
using Godot;
using Ashfall.Core;
using Ashfall.Core.Difficulty;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// XP-01 difficulty director host: loads difficulty_presets.json once and
    /// exposes the scalar provider. Consumers multiply existing owners; this
    /// partial never becomes a second campaign or ending authority.
    /// </summary>
    public partial class Main
    {
        private DifficultyDirector? _difficulty;
        private DifficultyScalarsProvider _difficultyScalars = DifficultyScalarsProvider.Legacy;

        public DifficultyScalarsProvider DifficultyScalars => _difficultyScalars;

        private void SetupDifficulty()
        {
            if (_difficulty != null) return;
            string dataDir = string.IsNullOrEmpty(_dataDir) ? CatalogPath.ResolveDataDir() : _dataDir;
            try
            {
                var catalog = DifficultyPresetCatalogLoader.Load(dataDir, new FileSystemIO());
                _difficulty = new DifficultyDirector(catalog);
                _difficultyScalars = _difficulty.ResolveProvider(null);
            }
            catch (System.Exception ex)
            {
                GD.PrintErr("[Ashfall Godot] Difficulty catalog failed, using standard scalars: " + ex.Message);
                _difficultyScalars = DifficultyScalarsProvider.Legacy;
            }
        }
    }
}
