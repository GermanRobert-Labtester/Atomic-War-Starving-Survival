// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 142 — Clothing & Warmth host wiring.
// DEC-109: ClothingWarmthSystem owns equipped-layer records, wetness, gear
// condition, and the cold-loss reduction fraction. NeedsSystem remains the
// sole mutable warmth authority; the host binds the existing (previously
// never-assigned) ClothingWarmthReductionProvider seam to this system.
// WeatherSystem remains the weather authority; Inventory remains item custody.
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private ClothingWarmthHostSession? _clothingWarmth;
        private bool _clothingWarmthDirty;

        public ClothingWarmthHostSession? ClothingWarmth => _clothingWarmth;

        /// <summary>
        /// Weather kinds that soak clothing. Read only from the canonical
        /// WeatherSystem; this set is a presentation-free classification, not a
        /// second weather authority.
        /// </summary>
        private static readonly HashSet<Ashfall.Core.WeatherKind> s_precipitatingWeather = new()
        {
            Ashfall.Core.WeatherKind.Rain,
            Ashfall.Core.WeatherKind.BlackRain,
            Ashfall.Core.WeatherKind.BloodRain,
            Ashfall.Core.WeatherKind.FalloutStorm,
            Ashfall.Core.WeatherKind.Blizzard,
            Ashfall.Core.WeatherKind.AcidSnow,
            Ashfall.Core.WeatherKind.BlackSnow,
            Ashfall.Core.WeatherKind.RadHail,
            Ashfall.Core.WeatherKind.IceStorm,
            Ashfall.Core.WeatherKind.GlassStorm,
        };

        public void SetupClothingWarmth()
        {
            if (_clothingWarmth != null) return;

            var saved = ClothingWarmthSaveStore.TryLoad();
            _clothingWarmth = ClothingWarmthHostSession.Create(saved);
            _clothingWarmth.StateChanged += () => _clothingWarmthDirty = true;

            BindClothingWarmthProvider();
        }

        /// <summary>
        /// Binds the pre-existing NeedsSystem.ClothingWarmthReductionProvider
        /// delegate seam to the live clothing authority. Called after
        /// survivors are set up; reads the current reduction each time Needs
        /// evaluates cold loss.
        /// </summary>
        private void BindClothingWarmthProvider()
        {
            SetupSurvivors();
            if (_survivors?.Needs == null || _clothingWarmth == null) return;

            _survivors.Needs.ClothingWarmthReductionProvider = survivorId =>
                _clothingWarmth.CalculateColdLossReduction(survivorId);
        }

        public bool EquipSurvivorClothing(string survivorId, string itemId, float condition = 1.0f)
        {
            SetupClothingWarmth();
            bool ok = _clothingWarmth!.Equip(survivorId, itemId, condition);
            if (ok) _clothingWarmthDirty = true;
            return ok;
        }

        public bool UnequipSurvivorClothing(string survivorId, string itemId)
        {
            SetupClothingWarmth();
            bool ok = _clothingWarmth!.Unequip(survivorId, itemId);
            if (ok) _clothingWarmthDirty = true;
            return ok;
        }

        /// <summary>
        /// Deterministic campaign-day advance: precipitation applies wetness
        /// through the canonical weather owner, the shelter dries gear, and
        /// every equipped layer accrues one day of wear.
        /// </summary>
        public void TickClothingWarmth(int day)
        {
            SetupClothingWarmth();
            if (_clothingWarmth == null) return;

            bool precipitating = _world?.Weather != null
                && s_precipitatingWeather.Contains(_world.Weather.Current);

            var roster = _survivors?.RosterState;
            if (roster == null) return;

            foreach (var survivor in roster)
            {
                if (survivor == null || string.IsNullOrEmpty(survivor.Id)) continue;
                if (survivor.IsAlive == false) continue;

                if (precipitating) _clothingWarmth.ApplyWetness(survivor.Id, 0.35f);
                _clothingWarmth.DryClothing(survivor.Id, 8f);
                _clothingWarmth.DegradeCondition(survivor.Id, 24f);
            }

            _clothingWarmthDirty = true;
        }

        public ClothingWarmthCensus GetClothingWarmthCensus()
        {
            SetupClothingWarmth();
            return _clothingWarmth!.GetCensus();
        }

        /// <summary>Read-only clothing/warmth readout for the survivor detail route.</summary>
        public (int Layers, int TotalWarmth, float Wetness, int ColdReductionBp)? GetClothingWarmthReadout(string survivorId)
        {
            SetupClothingWarmth();
            if (_clothingWarmth == null || string.IsNullOrEmpty(survivorId)) return null;

            var equipped = _clothingWarmth.GetEquipped(survivorId);
            if (equipped.Count == 0) return null;

            int warmth = 0;
            foreach (var eq in equipped)
            {
                if (_clothingWarmth.System.Profiles.TryGetValue(eq.item_id, out var profile))
                    warmth += profile.warmth_value;
            }

            float reduction = _clothingWarmth.CalculateColdLossReduction(survivorId);
            int reductionBp = (int)Math.Round(reduction * 10000f);
            return (equipped.Count, warmth, _clothingWarmth.System.State.survivors.TryGetValue(survivorId, out var rec) ? rec.wetness : 0f, reductionBp);
        }

        public void SaveClothingWarmth()
        {
            if (_clothingWarmth == null) return;
            var state = _clothingWarmth.CaptureState();
            ClothingWarmthSaveStore.TrySave(state);
            if (CaptureSection(
                    ClothingWarmthSaveStore.SectionName,
                    ClothingWarmthSaveStore.TryCapturePersisted(state)))
            {
                _clothingWarmthDirty = false;
            }
        }

        public void FlushClothingWarmthIfDirty()
        {
            if (_clothingWarmthDirty)
            {
                SaveClothingWarmth();
            }
        }

        public void ResetClothingWarmth()
        {
            if (_survivors?.Needs != null)
            {
                _survivors.Needs.ClothingWarmthReductionProvider = null;
            }
            _clothingWarmth = null;
            _clothingWarmthDirty = false;
        }
    }
}
