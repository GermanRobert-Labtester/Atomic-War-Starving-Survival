// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Excavation;
using Ashfall.Core.IO;
using Ashfall.Core.Quests;
using Ashfall.Core.Radio;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Godot;
using System;
using System.Collections.Generic;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private ExcavationHazardSystem? _excavationHazards;
        private bool _excavationHazardsDirty;

        // ── Plan 49: Subterranean Hazard Operations ─────────────────────

        public ExcavationHazardSystem EnsureExcavationHazards()
        {
            if (_excavationHazards != null) return _excavationHazards;

            SetupInventory();
            var inv = _inventory.Inventory;
            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("excavation_hazards") : new SeededRng(49);
            var excavation = _excavation?.System;

            _excavationHazards = new ExcavationHazardSystem(inv, rng, excavation, null, new GodotLog());

            string catalogPath = CatalogPath.ResolveCatalog("excavation_hazard_mitigation.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    _excavationHazards.LoadCatalog(json);
                }
            }

            var saved = ExcavationHazardSaveStore.TryLoad();
            if (saved != null)
            {
                _excavationHazards.RestoreState(saved);
            }

            _excavationHazards.OnRescueStarted += (sec, count) =>
            {
                _journal?.TryAddRawEntry("excavation_rescue_started", $"Cave-in emergency! {count} miner(s) trapped in {sec}", null!, _simDay);
                var trapped = _excavationHazards.State.sectors.TryGetValue(sec, out var s) ? s.ActiveTrappedMiners : new List<string>();
                EnsureDynamicQuests().TriggerRescueMinersQuest($"rescue_{sec}_{_simDay}", sec, trapped, _simDay);
            };

            _excavationHazards.OnRescueSucceeded += (sec) =>
            {
                var q = _dynamicQuests?.GetActiveQuest(DynamicQuestlineSystem.RescueMinersQuestId);
                if (q != null && q.TargetLocationId == sec)
                {
                    _dynamicQuests?.CompleteQuest(q.QuestId);
                }
            };

            _excavationHazards.OnRescueFailed += (sec) =>
            {
                var q = _dynamicQuests?.GetActiveQuest(DynamicQuestlineSystem.RescueMinersQuestId);
                if (q != null && q.TargetLocationId == sec)
                {
                    _dynamicQuests?.FailQuest(q.QuestId);
                }

                if (_excavationHazards.State.sectors.TryGetValue(sec, out var s))
                {
                    foreach (var minerId in s.ActiveTrappedMiners)
                    {
                        _survivorFate?.ReportDeath(minerId, SurvivorDeathCause.Scripted, $"Died in excavation cave-in ({sec})", source: "excavation_hazard");
                    }
                }
            };

            _excavationHazards.OnHazardStateChanged += () => _excavationHazardsDirty = true;
            return _excavationHazards;
        }

        private void SetupExcavationHazards()
        {
            EnsureExcavationHazards();
        }

        private void SaveExcavationHazards()
        {
            if (_excavationHazards != null)
            {
                CaptureSection("excavation_hazards", ExcavationHazardSaveStore.TryCapturePersisted(_excavationHazards.CaptureState()));
                _excavationHazardsDirty = false;
            }
        }

    }
}
