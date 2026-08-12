// GameBootstrap.IntoTheAsh.cs — wire "Into the Ash" expansion (Parts II & III)
// into the host. Follows the same turn pattern as GameBootstrap.AshGetsDeeper.cs:
// small focused boot methods, SaveSystem + registry integration, location classes.
//
// PART II: 8 new locations with full environmental storytelling and loot tables.
// PART III: 6 multi-stage narrative quest chains driven by EventBus + world flags.
using System;
using System.Collections.Generic;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.World;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        // ── "Into the Ash" public accessors ──────────────────────────────

        /// <summary>Multi-stage narrative quest chains (Part III — 6 questlines).</summary>
        public ExpansionQuestChainsSystem ExpansionQuests { get; private set; }

        /// <summary>8 new location classes (Part II). Constructed but dormant;
        /// the host's expedition runner reads their loot tables + narrative text.</summary>
        public Location_DistrictCoordinationOffice DistrictCoordinationOffice { get; private set; }
        public Location_CheckpointKiloMemorial CheckpointKiloMemorial { get; private set; }
        public Location_MilitiaGrainExchange MilitiaGrainExchange { get; private set; }
        public Location_GlowChapel GlowChapel { get; private set; }
        public Location_TollHouse TollHouse { get; private set; }
        public Location_StMarenHospitalAnnex StMarenHospitalAnnex { get; private set; }
        public Location_RadioTowerSevenBunker RadioTowerSevenBunker { get; private set; }
        public Location_MartaFarmhouse MartaFarmhouse { get; private set; }

        // ── Item pool (read-only view for the host's catalog builder) ───
        private readonly List<IntoTheAshItemsCatalog.ItemSpecRow> _intoTheAshItemPool
            = new List<IntoTheAshItemsCatalog.ItemSpecRow>();
        public IReadOnlyList<IntoTheAshItemsCatalog.ItemSpecRow> IntoTheAshItemPool
            => _intoTheAshItemPool;

        // ─────────────────────────────────────────────────────────────────
        // Master boot (called once from InitFoundation, after AshGetsDeeper)
        // ─────────────────────────────────────────────────────────────────

        public void BootIntoTheAshContent()
        {
            BootIntoTheAshItems();
            BootIntoTheAshLocations();
            BootIntoTheAshQuestSystem();
            GameLog.Log($"[GameBootstrap] Into the Ash booted: " +
                $"{_intoTheAshItemPool.Count} items, 8 locations, 6 questlines.");
        }

        // ── 31 new items → spec pool (host's ItemCatalogBuilder converts later) ──
        private void BootIntoTheAshItems()
        {
            _intoTheAshItemPool.Clear();
            var rows = IntoTheAshItemsCatalog.SpecRows;
            var seen = new System.Collections.Generic.HashSet<string>();
            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                if (row == null || string.IsNullOrEmpty(row.Id)) continue;
                if (!seen.Add(row.Id)) continue;
                _intoTheAshItemPool.Add(row);
            }
            GameLog.Log($"[GameBootstrap] Into the Ash items: pooled " +
                $"{_intoTheAshItemPool.Count} of {rows.Count}.");
        }

        // ── 8 location classes → construct (dormant ghosts; expedition host reads them) ──
        private void BootIntoTheAshLocations()
        {
            DistrictCoordinationOffice = new Location_DistrictCoordinationOffice();
            CheckpointKiloMemorial      = new Location_CheckpointKiloMemorial();
            MilitiaGrainExchange       = new Location_MilitiaGrainExchange();
            GlowChapel                 = new Location_GlowChapel();
            TollHouse                  = new Location_TollHouse();
            StMarenHospitalAnnex       = new Location_StMarenHospitalAnnex();
            RadioTowerSevenBunker      = new Location_RadioTowerSevenBunker();
            MartaFarmhouse             = new Location_MartaFarmhouse();
        }

        // ── Quest chains system → construct + wire + hook events ──
        private void BootIntoTheAshQuestSystem()
        {
            ExpansionQuests = new ExpansionQuestChainsSystem();

            // Wire delegates pointing back into GameBootstrap's own systems.
            // Use CountById/RemoveById (the Inventory API takes ItemDefinition,
            // not string; the ById variants take item id strings directly).
            ExpansionQuests.Wire(
                getDay:     () => TimeSystem?.CurrentDay ?? 0,
                hasFlag:    flag => SaveSystem != null && SaveSystem.GetWorldFlag(flag),
                setFlag:    (flag, val) => SaveSystem?.SetWorldFlag(flag, val),
                countItem:  itemId => Inventory != null ? Inventory.CountById(itemId) : 0,
                consumeItem:(itemId, amt) => Inventory != null && Inventory.RemoveById(itemId, amt),
                hasItem:    itemId => Inventory != null && Inventory.CountById(itemId) > 0
            );

            // ── Faction trust callbacks ─────────────────────────────────
            ExpansionQuests.GetFactionTrust = factionId =>
                EconomySystem != null ? EconomySystem.GetTrust(factionId) : 0f;
            ExpansionQuests.ModifyFactionTrust = (factionId, delta) =>
                EconomySystem?.ModifyTrust(factionId, delta);

            // ── Raid probability — adjust via faction-specific delta on
            // the garrison/militia/warlord channels. The quest system calls
            // this with a multiplier; we map it to a trust delta that the
            // hatch defence system reads each hour.
            ExpansionQuests.ModifyRaidProbability = multiplier =>
            {
                // Use AdjustRaidChance on the generic "warlord" faction when
                // the tribute quest escalates; the garrison when Kilo truth
                // is broadcast; the militia when the grain war breaks.
                // For now, apply as a trust penalty that feeds into raid rates.
                if (EconomySystem != null)
                    EconomySystem.ModifyTrust("warlord", -5f * multiplier);
            };

            // ── Global morale — use NeedsSystem.Modify() which takes a
            // survivor, a NeedKind, and a delta (the same pattern used by
            // ExpeditionSystem.ApplyMoraleDelta).
            ExpansionQuests.ApplyGlobalMorale = delta =>
            {
                if (Survivors == null || NeedsSystem == null) return;
                for (int i = 0; i < Survivors.Count; i++)
                {
                    if (Survivors[i] != null && Survivors[i].IsAlive)
                        NeedsSystem.Modify(Survivors[i], NeedKind.Morale, delta);
                }
            };

            // ── Per-survivor morale — same pattern, filtered by name.
            ExpansionQuests.ApplySurvivorMorale = (name, delta) =>
            {
                if (Survivors == null || NeedsSystem == null) return;
                for (int i = 0; i < Survivors.Count; i++)
                {
                    if (Survivors[i] != null && Survivors[i].IsAlive
                        && Survivors[i].DisplayName == name)
                    {
                        NeedsSystem.Modify(Survivors[i], NeedKind.Morale, delta);
                        return;
                    }
                }
            };

            // ── Hook quest events → GameLog + world flags ───────────────
            Action<string, int, string> onQuestStageChanged = (questId, stage, desc) =>
            {
                GameLog.Log($"[Quest] {questId} → stage {stage}: {desc}");
            };
            ExpansionQuests.OnQuestStageChanged += onQuestStageChanged;
            _subscriptions.Track(() => ExpansionQuests.OnQuestStageChanged -= onQuestStageChanged);

            Action<string, string> onQuestCompleted = (questId, reward) =>
            {
                GameLog.Log($"[Quest] {questId} COMPLETED: {reward}");
                SaveSystem?.SetWorldFlag(QuestFlags.QuestCompletedPrefix + questId, true);
            };
            ExpansionQuests.OnQuestCompleted += onQuestCompleted;
            _subscriptions.Track(() => ExpansionQuests.OnQuestCompleted -= onQuestCompleted);

            Action<string, string> onQuestFailed = (questId, reason) =>
            {
                GameLog.Log($"[Quest] {questId} FAILED: {reason}");
            };
            ExpansionQuests.OnQuestFailed += onQuestFailed;
            _subscriptions.Track(() => ExpansionQuests.OnQuestFailed -= onQuestFailed);

            // ── Expedition request → set flag for map screen to read ──
            Action<string> onRequestExpedition = nodeId =>
            {
                GameLog.Log($"[Quest] Requested expedition to: {nodeId}");
                SaveSystem?.SetWorldFlag($"quest_expedition_requested:{nodeId}", true);
            };
            ExpansionQuests.OnRequestExpedition += onRequestExpedition;
            _subscriptions.Track(() => ExpansionQuests.OnRequestExpedition -= onRequestExpedition);

            // ── Bunker event request → EventRunner schedule ────────────
            Action<string, string, int> onRequestBunkerEvent = (eventId, questId, day) =>
            {
                GameLog.Log($"[Quest] {questId} requests bunker event '{eventId}' on day {day}");
                EventRunner?.ScheduleEvent(eventId, day, null);
            };
            ExpansionQuests.OnRequestBunkerEvent += onRequestBunkerEvent;
            _subscriptions.Track(() => ExpansionQuests.OnRequestBunkerEvent -= onRequestBunkerEvent);
        }
    }
}
