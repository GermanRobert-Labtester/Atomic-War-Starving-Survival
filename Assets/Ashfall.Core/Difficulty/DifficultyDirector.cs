// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core.Difficulty
{
    /// <summary>
    /// Resolves an immutable campaign preset ID into a typed scalar provider.
    /// The director owns neither the campaign header nor any consuming system.
    /// </summary>
    public sealed class DifficultyDirector
    {
        private readonly DifficultyPresetCatalog _catalog;

        public DifficultyDirector(DifficultyPresetCatalog catalog)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            if (!_catalog.Validate(out string error))
                throw new ArgumentException("invalid difficulty catalog: " + error, nameof(catalog));
            _catalog.Index();
        }

        public DifficultyPreset ResolvePreset(string? campaignPresetId)
        {
            string id = string.IsNullOrWhiteSpace(campaignPresetId)
                ? _catalog.default_preset_id
                : campaignPresetId;
            if (!_catalog.TryGet(id, out DifficultyPreset preset))
                throw new InvalidOperationException("unknown difficulty preset '" + id + "'");
            return preset;
        }

        public DifficultyScalarsProvider ResolveProvider(string? campaignPresetId)
        {
            DifficultyPreset preset = ResolvePreset(campaignPresetId);
            return DifficultyScalarsProvider.FromPreset(preset);
        }
    }
}
