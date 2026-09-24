// SPDX-License-Identifier: MIT
// ASHFALL Plan 175 — Meta Progression & Cross-Run Profile Store Host Wiring.

using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Endgame;
using AtomicWar.GodotApp.Host;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private MetaProgressionHostSession? _metaProgression;
        private bool _metaProgressionDirty;

        public MetaProgressionHostSession? MetaProgression => _metaProgression;

        public void SetupMetaProgression()
        {
            if (_metaProgression != null) return;

            _metaProgression = MetaProgressionHostSession.Create(_dataDir);

            var saved = MetaProgressionSaveStore.TryLoad();
            if (saved != null)
            {
                _metaProgression.RestoreState(saved);
            }

            _metaProgression.StateChanged += () => _metaProgressionDirty = true;

            // Plan 175 — the append-only completion history is the single
            // user-level cross-run authority. Deriving prestige and unlocks
            // from it here makes meta progression genuinely persist across
            // campaigns; the per-slot section only snapshots this run's
            // derived state.
            SeedMetaProgressionFromProfile();
        }

        /// <summary>
        /// Seeds the meta system's profile store from the user-level append-only
        /// completion history, then re-evaluates prestige and unlocks. Idempotent:
        /// the store de-duplicates by completion id. This is what makes the
        /// unlock set carry from one campaign to the next.
        /// </summary>
        public void SeedMetaProgressionFromProfile()
        {
            if (_metaProgression == null) return;
            try
            {
                _completionHistory ??= CompletionHistoryStore.Load();
                var records = _completionHistory.Records;
                if (records.Count == 0) return;

                var history = new CampaignCompletionHistory
                {
                    records = new List<CampaignCompletionRecord>(records)
                };

                var endings = new List<string>();
                for (int i = 0; i < records.Count; i++)
                {
                    string endingId = records[i].endingId;
                    if (!string.IsNullOrEmpty(endingId) && !endings.Contains(endingId))
                        endings.Add(endingId);
                }

                _metaProgression.RecordCampaignCompletion(history, null, endings);
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[Ashfall Godot] Meta progression profile seed failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Plan 175 — activates or deactivates a selected NG+ boon. On activation
        /// the boon's authored grants are applied through the canonical inventory
        /// authority: the meta system owns the unlock decision, inventory owns the
        /// items, and no second resource ledger is created.
        /// </summary>
        public bool SetNgPlusBoonActive(string id, bool active)
        {
            if (_metaProgression == null) SetupMetaProgression();
            if (_metaProgression == null) return false;

            bool changed = _metaProgression.SetNgPlusBoonActive(id, active);
            if (changed && active)
                ApplyBoonGrants(id);
            return changed;
        }

        private void ApplyBoonGrants(string boonId)
        {
            if (_metaProgression == null || _inventory?.Inventory == null) return;

            var grants = _metaProgression.GetGrantsForBoon(boonId);
            for (int i = 0; i < grants.Count; i++)
            {
                var grant = grants[i];
                if (!_inventory.Inventory.AddById(grant.item_id, grant.quantity))
                    GD.PrintErr($"[Ashfall Godot] NG+ boon item '{grant.item_id}' x{grant.quantity} could not be added.");
            }
            if (grants.Count > 0)
                SaveInventory();
        }

        public void SaveMetaProgression()
        {
            if (_metaProgression == null) return;
            var state = _metaProgression.CaptureState();
            MetaProgressionSaveStore.TrySave(state);
            if (CaptureSection("meta_progression", MetaProgressionSaveStore.TryCapturePersisted(state)))
            {
                _metaProgressionDirty = false;
            }
        }

        public void TickMetaProgression(int day)
        {
            if (_metaProgression == null) SetupMetaProgression();
            // Heartbeat daily progression
        }

        public void FlushMetaProgressionIfDirty()
        {
            if (_metaProgressionDirty)
            {
                SaveMetaProgression();
            }
        }

        public void ResetMetaProgression()
        {
            _metaProgression = null;
            _metaProgressionDirty = false;
        }
    }
}
