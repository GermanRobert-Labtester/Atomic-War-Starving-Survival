// SPDX-License-Identifier: MIT
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Difficulty;
using Ashfall.Core.Save;

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
        private DifficultyDirector? _difficultyDirector => _difficulty;
        private DifficultyPresetCatalog? _difficultyCatalog;
        private DifficultyScalarsProvider _difficultyScalars = DifficultyScalarsProvider.Legacy;
        private string _difficultyPresetId = DifficultyScalarsProvider.Legacy.PresetId;
        private bool _difficultyBonusesGrantedForCampaign;

        /// <summary>
        /// The only live difficulty scalar source. Consumers are added only
        /// after their individual calculation sites have been premise-checked.
        /// </summary>
        public DifficultyScalarsProvider DifficultyScalars => _difficultyScalars;
        public string DifficultyPresetId => _difficultyPresetId;

        public IReadOnlyList<DifficultyPreset> DifficultyPresets
        {
            get
            {
                return EnsureDifficultyAuthority(out _)
                    ? _difficultyCatalog!.AllPresets
                    : Array.Empty<DifficultyPreset>();
            }
        }

        private DifficultyPresetCatalog EnsureDifficultyCatalog()
        {
            if (_difficultyCatalog != null) return _difficultyCatalog;

            string dataDir = string.IsNullOrEmpty(_dataDir) ? CatalogPath.ResolveDataDir() : _dataDir;
            _difficultyCatalog = DifficultyPresetCatalogLoader.Load(
                dataDir,
                new FileSystemIO());
            _difficulty = new DifficultyDirector(_difficultyCatalog);
            return _difficultyCatalog;
        }

        private DifficultyDirector EnsureDifficultyDirector()
        {
            EnsureDifficultyCatalog();
            return _difficulty
                ?? throw new InvalidOperationException("Difficulty director did not initialize.");
        }

        private string DefaultDifficultyPresetId() =>
            EnsureDifficultyDirector().ResolvePreset(null).id;

        private string ResolveDifficultyPresetId(string? presetId) =>
            EnsureDifficultyDirector().ResolvePreset(presetId).id;

        private bool EnsureDifficultyAuthority(out string error)
        {
            if (_difficulty != null && _difficultyCatalog != null)
            {
                error = string.Empty;
                return true;
            }

            try
            {
                EnsureDifficultyCatalog();
                error = string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        private void SetupDifficulty()
        {
            if (!EnsureDifficultyAuthority(out string authorityError))
            {
                GD.PrintErr("[Ashfall Godot] Difficulty catalog failed, using standard scalars: " + authorityError);
                _difficultyScalars = DifficultyScalarsProvider.Legacy;
                return;
            }

            try
            {
                _difficultyScalars = _difficulty!.ResolveProvider(
                    string.IsNullOrWhiteSpace(_difficultyPresetId) ? null : _difficultyPresetId);
                _difficultyPresetId = _difficultyScalars.PresetId;
            }
            catch (Exception ex)
            {
                GD.PrintErr("[Ashfall Godot] Difficulty preset failed, using standard scalars: " + ex.Message);
                _difficultyScalars = DifficultyScalarsProvider.Legacy;
                _difficultyPresetId = _difficultyScalars.PresetId;
            }
        }

        /// <summary>
        /// Called exclusively by the fresh-campaign transaction after the
        /// player commits the menu selection. The resolved ID is later stored
        /// in the checksummed campaign header; there is no in-run edit path.
        /// </summary>
        private void SelectDifficultyForNewCampaign(string? presetId)
        {
            var director = EnsureDifficultyDirector();
            var preset = director.ResolvePreset(presetId);
            _difficultyPresetId = preset.id;
            _difficultyScalars = DifficultyScalarsProvider.FromPreset(preset);
        }

        private bool TrySelectDifficultyForNewCampaign(string? requestedId, out string error)
        {
            if (!EnsureDifficultyAuthority(out error)) return false;
            try
            {
                SelectDifficultyForNewCampaign(requestedId);
                error = string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Restores a header-bound selection. An absent v1 field uses the
        /// catalog default; an explicit unknown ID fails closed via the
        /// director rather than silently changing a campaign's difficulty.
        /// </summary>
        private void RestoreDifficultyFromCampaignHeader(CampaignDaySave? save)
        {
            SelectDifficultyForNewCampaign(save?.difficulty_preset_id);
        }

        /// <summary>
        /// Returns the selected preset's authored starter-item IDs. The
        /// inventory owner performs the actual grants during fresh setup.
        /// </summary>
        private IReadOnlyList<string> DifficultyStartingBonusItemIds()
        {
            var preset = EnsureDifficultyDirector().ResolvePreset(_difficultyPresetId);
            return preset.starting_bonus_item_ids.ToArray();
        }

        private void ResetDifficultyForCampaign()
        {
            _difficultyPresetId = string.Empty;
            _difficultyScalars = DifficultyScalarsProvider.Legacy;
        }

        private string? ValidateDifficultyEnvelope(AggregateSaveEnvelope envelope)
        {
            if (envelope?.manifest == null)
                return "campaign manifest is missing";
            if (!EnsureDifficultyAuthority(out string authorityError))
                return "difficulty catalog unavailable: " + authorityError;

            try
            {
                _difficulty!.ResolveProvider(envelope.manifest.difficultyPresetId);
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private void BindDifficultyConsumers()
        {
            if (_survivors != null)
            {
                _survivors.Needs.HungerRateMultiplier = () => _difficultyScalars.HungerMult;
                _survivors.Needs.ThirstRateMultiplier = () => _difficultyScalars.ThirstMult;
                _survivors.Radiation.ExposureRateMultiplier = () => _difficultyScalars.RadiationMult;
            }
            if (_disease?.Engine != null)
                _disease.Engine.OnsetProbabilityMultiplier = () => _difficultyScalars.DiseaseMult;
            if (_economy?.Market != null)
                _economy.Market.PriceMultiplierProvider = () => _difficultyScalars.MarketPriceMult;
            if (_equipmentCondition?.System != null)
                _equipmentCondition.System.WearRateMultiplierProvider = () => _difficultyScalars.EquipmentDecayMult;
        }

        private void GrantDifficultyStartingBonusesOnce()
        {
            if (_difficultyBonusesGrantedForCampaign || _inventory?.Inventory == null) return;
            _difficultyBonusesGrantedForCampaign = true;

            if (!EnsureDifficultyAuthority(out _)) return;
            DifficultyPreset preset;
            try
            {
                preset = _difficulty!.ResolvePreset(_difficultyPresetId);
            }
            catch (Exception ex)
            {
                GD.PrintErr("[Ashfall Godot] Difficulty bonus grant refused: " + ex.Message);
                return;
            }

            for (int i = 0; i < preset.starting_bonus_item_ids.Count; i++)
            {
                string itemId = preset.starting_bonus_item_ids[i];
                if (!_inventory.Inventory.AddById(itemId, 1))
                    GD.PrintErr($"[Ashfall Godot] Difficulty bonus item '{itemId}' could not be added.");
            }
            SaveInventory();
        }

        private void PrepareDifficultyForNewCampaign()
        {
            _difficultyBonusesGrantedForCampaign = false;
        }

        private void ApplyDifficultyFromLoadedManifest()
        {
            string persistedId = _saveLoadHost?.ActiveEnvelope?.manifest?.difficultyPresetId ?? string.Empty;
            _difficultyPresetId = string.IsNullOrWhiteSpace(persistedId)
                ? DifficultyScalarsProvider.Legacy.PresetId
                : persistedId;
        }
    }
}
