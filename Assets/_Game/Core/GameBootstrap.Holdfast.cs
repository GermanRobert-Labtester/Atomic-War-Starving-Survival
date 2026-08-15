using System;
using System.Collections.Generic;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Factions;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Utilities;
using Ashfall.Core;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// ASHFALL: THE HOLDFAST — host wiring. Ice road, census, brine, waystation, quests.
    /// Called from InitDeepLore after Currents. Old saves: Ice Road dark until unlock.
    /// </summary>
    public partial class GameBootstrap
    {
        public IceRoadSystem IceRoadSystem { get; private set; }
        public CensusClaimSystem CensusClaimSystem { get; private set; }
        public BrineWaterSystem BrineWaterSystem { get; private set; }
        public WaystationSystem WaystationSystem { get; private set; }
        public HoldfastQuestSystem HoldfastQuests { get; private set; }
        public NPC_EdorVale NPCEdorVale { get; private set; }
        public NPC_YaraHolm NPCYaraHolm { get; private set; }
        public NPC_TheOffice NPCTheOffice { get; private set; }

        private void BootHoldfast()
        {
            IceRoadSystem = new IceRoadSystem(_worldSeed + 808);
            CensusClaimSystem = new CensusClaimSystem();
            BrineWaterSystem = new BrineWaterSystem();
            WaystationSystem = new WaystationSystem();
            HoldfastQuests = new HoldfastQuestSystem();
            HoldfastQuests.BindCatalog(HoldfastQuestCatalogLoader.Load());

            NPCEdorVale = new NPC_EdorVale();
            NPCYaraHolm = new NPC_YaraHolm();
            NPCTheOffice = new NPC_TheOffice();

            var edor = CharactersCatalogLoader.GetById("npc_edor_vale");
            NPCEdorVale.Initialise(edor != null ? edor.display_name : "Edor Vale");
            var yara = CharactersCatalogLoader.GetById("npc_yara_holm");
            NPCYaraHolm.Initialise(yara != null ? yara.display_name : "Yara Holm");
            var office = HoldfastFactionsCatalogLoader.GetById("faction_the_office");
            NPCTheOffice.Initialise(office != null ? office.display_name : "The Office");

            MergeHoldfastItems();
            MergeHoldfastLocations(expansionUnlocked: false);
            if (GeneratedMap != null)
                HoldfastMapSeeder.Attach(GeneratedMap, HoldfastLocationsCatalogLoader.Load());

            if (ExpeditionSystem != null)
            {
                ExpeditionSystem.SetIceRoadSystem(IceRoadSystem);
                ExpeditionSystem.SetCensusClaimSystem(CensusClaimSystem);
            }

            WireHoldfastEvents();

            _registry.RegisterDaily("ice_road_system", TickHoldfastDaily);
            _registry.RegisterDaily("census_claim_system", day => CensusClaimSystem?.TickDaily(day));
            _registry.RegisterDaily("brine_water_system", TickBrineDaily);
            _registry.RegisterDaily("waystation_system", day =>
                WaystationSystem?.TickDaily(IceRoadSystem != null && IceRoadSystem.IsOpen));
            _registry.RegisterDaily("holdfast_quest_system", TickHoldfastQuestsDaily);
            _registry.RegisterEventDriven("npc_edor_vale");
            _registry.RegisterEventDriven("npc_yara_holm");
            _registry.RegisterEventDriven("npc_the_office");

            GameLog.Log("[GameBootstrap] Holdfast booted: IceRoad, Census, Brine, Waystation, quests, Edor, Yara.");
        }

        private void MergeHoldfastItems()
        {
            if (_itemCatalog == null) return;
            var defs = HoldfastItemsCatalogLoader.MaterialiseAll();
            int added = 0;
            for (int i = 0; i < defs.Count; i++)
            {
                var d = defs[i];
                if (d == null || string.IsNullOrEmpty(d.id)) continue;
                if (_itemCatalog.GetById(d.id) != null) continue;
                _itemCatalog.items.Add(d);
                added++;
            }
            if (added > 0)
                GameLog.Log("[GameBootstrap] Holdfast items merged: " + added);
        }

        private void MergeHoldfastLocations(bool expansionUnlocked)
        {
            if (_locationCatalog == null) return;
            int n = HoldfastLocationsCatalogLoader.ApplyToCatalog(_locationCatalog, expansionUnlocked);
            if (n > 0)
                GameLog.Log("[GameBootstrap] Holdfast locations applied: " + n);
        }

        private void WireHoldfastEvents()
        {
            if (IceRoadSystem != null)
            {
                IceRoadSystem.OnIceRoadOpened += HandleIceRoadOpened;
                IceRoadSystem.OnIceRoadClosed += HandleIceRoadClosed;
                IceRoadSystem.OnBeaconDark += loc =>
                    GameLog.Log("[Holdfast] Beacon dark: " + loc);
                IceRoadSystem.OnStateChanged += _ => SyncIceRoadWorldFlag();
                _subscriptions.Track(() =>
                {
                    IceRoadSystem.OnIceRoadOpened -= HandleIceRoadOpened;
                    IceRoadSystem.OnIceRoadClosed -= HandleIceRoadClosed;
                });
            }

            if (CensusClaimSystem != null)
            {
                CensusClaimSystem.OnLevyResolved += HandleLevyResolved;
                CensusClaimSystem.On12CActivated += () =>
                    SaveSystem?.SetWorldFlag(CensusClaimSystem.FlagOrder12c, true);
                _subscriptions.Track(() => CensusClaimSystem.OnLevyResolved -= HandleLevyResolved);
            }

            if (EventRunner != null)
            {
                Action<GameEvent, EventChoice, EventContext> onHoldfastChoice = HandleHoldfastEventChoice;
                EventRunner.OnChoiceApplied += onHoldfastChoice;
                _subscriptions.Track(() => EventRunner.OnChoiceApplied -= onHoldfastChoice);
            }

            if (NPCYaraHolm != null)
            {
                NPCYaraHolm.OnAccessWithdrawn += st =>
                    IceRoadSystem?.BeginLampsOut(st != null && st.withdrewPermanently);
            }

            if (HoldfastQuests != null)
            {
                HoldfastQuests.OnQuestStarted += id =>
                {
                    if (id == HoldfastQuestSystem.Sheet)
                        UnlockHoldfast(TimeSystem != null ? TimeSystem.CurrentDay : 1);
                    if (id == HoldfastQuestSystem.Clerk)
                        IceRoadSystem?.NotifyClerkStarted();
                    if (id == HoldfastQuestSystem.Window)
                        WaystationSystem?.Unlock();
                };
                HoldfastQuests.OnQuestCompleted += id =>
                {
                    if (id == HoldfastQuestSystem.Sheet)
                    {
                        GiveHoldfastItem("item_map_sheet_ice_road", 1);
                        SaveSystem?.SetWorldFlag("lore_hf_sheet", true);
                    }
                    if (id == HoldfastQuestSystem.Clerk)
                        GiveHoldfastItem("item_census_return_blank", 1);
                    if (id == HoldfastQuestSystem.Window)
                        WaystationSystem?.Unlock();
                    if (id == HoldfastQuestSystem.Plant)
                        BrineWaterSystem?.UnlockSaltTrade();
                };
            }

            if (BrineWaterSystem != null)
            {
                BrineWaterSystem.OnSteamTrip += () =>
                {
                    int day = TimeSystem != null ? TimeSystem.CurrentDay : 1;
                    HoldfastQuests?.TryStart(HoldfastQuestSystem.Membrane, day);
                };
                if (WaterEconomySystem != null)
                {
                    Action onWater = () => { /* inversion hook — brine is a third politics, not a lockout */ };
                    WaterEconomySystem.OnWaterStateChanged += onWater;
                    _subscriptions.Track(() => WaterEconomySystem.OnWaterStateChanged -= onWater);
                }
            }

            SyncIceRoadWorldFlag();
        }

        private void TickHoldfastDaily(int day)
        {
            var weather = WeatherSystem != null ? WeatherSystem.Current : WeatherKind.Clear;
            float outdoor = TemperatureSystem != null ? TemperatureSystem.AmbientCelsius : -10f;
            IceRoadSystem?.TickDaily(day, weather, outdoor);

            if (OzoneScourgeSystem != null && IceRoadSystem != null)
                OzoneScourgeSystem.IceAlbedoMultiplier =
                    IceRoadSystem.IsOpen ? IceRoadSystem.IceAlbedoUvMultiplier : 1f;

            MaybeUnlockHoldfastFromGates(day);
            SyncIceRoadWorldFlag();
            ApplyLevyAwayFlags();
        }

        private void TickBrineDaily(int day)
        {
            var weather = WeatherSystem != null ? WeatherSystem.Current : WeatherKind.Clear;
            float outdoor = TemperatureSystem != null ? TemperatureSystem.AmbientCelsius : -18f;
            BrineWaterSystem?.TickDaily(day, weather, outdoor, outfallShifted: false);
        }

        private void TickHoldfastQuestsDaily(int day)
        {
            bool hasMap = Inventory != null && Inventory.CountById("item_map_sheet_ice_road") > 0;
            bool formula = JournalSystem != null && JournalSystem.Knowledge != null
                && JournalSystem.Knowledge.Has("lore_pre_the_formula");
            bool letters = JournalSystem != null && JournalSystem.Knowledge != null
                && JournalSystem.Knowledge.Has("lore_pre_allocation_letters");
            HoldfastQuests?.TickDaily(day, hasMap, formula, letters);
            MaybeUnlockHoldfastFromGates(day);
        }

        private void MaybeUnlockHoldfastFromGates(int day)
        {
            if (IceRoadSystem != null && IceRoadSystem.IsUnlocked) return;
            if (day < HoldfastQuestSystem.SheetMinDay) return;
            bool hasMap = Inventory != null && Inventory.CountById("item_map_sheet_ice_road") > 0;
            bool formula = JournalSystem != null && JournalSystem.Knowledge != null
                && JournalSystem.Knowledge.Has("lore_pre_the_formula");
            bool letters = JournalSystem != null && JournalSystem.Knowledge != null
                && JournalSystem.Knowledge.Has("lore_pre_allocation_letters");
            if (hasMap || formula || letters || HoldfastQuests != null && HoldfastQuests.IsStarted(HoldfastQuestSystem.Sheet))
                UnlockHoldfast(day);
        }

        private void UnlockHoldfast(int day)
        {
            IceRoadSystem?.Unlock(day);
            SaveSystem?.SetWorldFlag(IceRoadSystem.FlagExpUnlocked, true);
            MergeHoldfastLocations(expansionUnlocked: true);
            SyncIceRoadWorldFlag();
        }

        private void SyncIceRoadWorldFlag()
        {
            bool open = IceRoadSystem != null && IceRoadSystem.IsOpen;
            SaveSystem?.SetWorldFlag(IceRoadSystem.FlagIceRoadOpen, open);
        }

        private void HandleIceRoadOpened()
        {
            SyncIceRoadWorldFlag();
            int day = TimeSystem != null ? TimeSystem.CurrentDay : 1;
            HoldfastQuests?.TryStart(HoldfastQuestSystem.Window, day);
            GameLog.Log("[Holdfast] Ice Road open. Window " +
                (IceRoadSystem != null ? IceRoadSystem.WindowDaysRemaining : 0) + " days.");
        }

        private void HandleIceRoadClosed()
        {
            SyncIceRoadWorldFlag();
            if (ExpeditionSystem == null) return;
            var active = ExpeditionSystem.ActiveExpeditions;
            if (active == null) return;
            for (int i = 0; i < active.Count; i++)
            {
                var exp = active[i];
                if (exp == null) continue;
                if (IceRoadSystem != null && IceRoadSystem.IsCutNode(exp.TargetLocationId))
                {
                    SaveSystem?.SetWorldFlag(IceRoadSystem.FlagStuckNorth, true);
                    GameLog.Log("[Holdfast] Window closed with a column on the Cut. Stuck-north.");
                }
            }
        }

        private void HandleLevyResolved(string flag)
        {
            SaveSystem?.SetWorldFlag(CensusClaimSystem.FlagLevyHonour, flag == CensusClaimSystem.FlagLevyHonour);
            SaveSystem?.SetWorldFlag(CensusClaimSystem.FlagLevySubstitute, flag == CensusClaimSystem.FlagLevySubstitute);
            SaveSystem?.SetWorldFlag(CensusClaimSystem.FlagLevyRefuse, flag == CensusClaimSystem.FlagLevyRefuse);

            if (flag == CensusClaimSystem.FlagLevyRefuse)
            {
                IceRoadSystem?.BeginLampsOut(permanentWithdraw: false);
                NPCEdorVale?.SetWaitingAtHatch(true);
                SaveSystem?.SetWorldFlag("mutation_levy_column", true);
            }
            if (flag == CensusClaimSystem.FlagLevyHonour)
            {
                SaveSystem?.SetWorldFlag("mutation_levy_column", true);
                NPCTheOffice?.AdjustTrust(12f);
            }
            if (flag == CensusClaimSystem.FlagLevySubstitute)
                NPCTheOffice?.AdjustTrust(-6f);

            ApplyLevyAwayFlags();
        }

        private void HandleHoldfastEventChoice(GameEvent evt, EventChoice choice, EventContext ctx)
        {
            if (evt == null || choice == null || string.IsNullOrEmpty(evt.id)) return;
            int day = TimeSystem != null ? TimeSystem.CurrentDay : 1;

            if (evt.id == EventRunner.HoldfastClerkEventId)
            {
                IceRoadSystem?.NotifyClerkStarted();
                HoldfastQuests?.TryStart(HoldfastQuestSystem.Clerk, day);
                if (choice.ChoiceId == "let_wait_hatch")
                    NPCEdorVale?.SetWaitingAtHatch(true);
            }
            else if (evt.id == EventRunner.HoldfastLevyEventId && CensusClaimSystem != null)
            {
                EnsureLevyIssued(day);
                if (choice.ChoiceId == "holdfast_levy_honour")
                    CensusClaimSystem.HonourLevy();
                else if (choice.ChoiceId == "holdfast_levy_substitute")
                    CensusClaimSystem.SubstituteLevy(PickLevyIds(skipIssued: true));
                else if (choice.ChoiceId == "holdfast_levy_refuse")
                    CensusClaimSystem.RefuseLevy(day);
            }
            else if (evt.id == EventRunner.HoldfastHatchEventId)
            {
                HoldfastQuests?.TryStart(HoldfastQuestSystem.Hatch, day);
                HoldfastQuests?.ChooseBranch(HoldfastQuestSystem.Hatch, choice.ChoiceId);
                HoldfastQuests?.SetEnding(choice.ChoiceId == "keep_shut"
                    ? "ending_holdfast_dark_road"
                    : "ending_holdfast_schedule");
            }
        }

        private void EnsureLevyIssued(int day)
        {
            if (CensusClaimSystem == null) return;
            if (CensusClaimSystem.ActiveLevy != null
                && CensusClaimSystem.ActiveLevy.survivorIds != null
                && CensusClaimSystem.ActiveLevy.survivorIds.Length > 0)
                return;
            CensusClaimSystem.IssueLevy(PickLevyIds(skipIssued: false), day);
        }

        private List<string> PickLevyIds(bool skipIssued)
        {
            var ids = new List<string>(CensusClaimSystem.MaxLevyCount);
            var skip = new HashSet<string>();
            if (skipIssued && CensusClaimSystem != null && CensusClaimSystem.ActiveLevy?.survivorIds != null)
            {
                var issued = CensusClaimSystem.ActiveLevy.survivorIds;
                for (int i = 0; i < issued.Length; i++)
                    if (!string.IsNullOrEmpty(issued[i])) skip.Add(issued[i]);
            }
            if (Survivors == null) return ids;
            for (int i = 0; i < Survivors.Count && ids.Count < CensusClaimSystem.MaxLevyCount; i++)
            {
                var sv = Survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                if (skip.Contains(sv.Id)) continue;
                ids.Add(sv.Id);
            }
            return ids;
        }

        private void ApplyLevyAwayFlags()
        {
            if (CensusClaimSystem == null || Survivors == null) return;
            var away = CensusClaimSystem.AssignedAwayIds();
            for (int i = 0; i < Survivors.Count; i++)
            {
                var sv = Survivors[i];
                if (sv == null) continue;
                bool assigned = false;
                for (int a = 0; a < away.Count; a++)
                    if (away[a] == sv.Id) { assigned = true; break; }
                if (assigned)
                    sv.IsOnExpedition = true;
            }
        }

        private void GiveHoldfastItem(string itemId, int count)
        {
            if (Inventory == null || string.IsNullOrEmpty(itemId) || count <= 0) return;
            var def = _itemCatalog != null ? _itemCatalog.GetById(itemId) : null;
            if (def == null)
            {
                GameLog.LogWarning("[Holdfast] GiveItem skipped — unknown id " + itemId);
                return;
            }
            Inventory.Add(def, count);
        }
    }
}
