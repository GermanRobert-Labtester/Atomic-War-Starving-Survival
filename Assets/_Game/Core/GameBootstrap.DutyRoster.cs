using System;
using System.Collections.Generic;
using AtomicWar._Game.Data;
using AtomicWar._Game.Factions;
using AtomicWar._Game.Utilities;
using Ashfall.Core;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        public DutyRosterSystem DutyRosterSystem { get; private set; }
        public MoraleMarkSystem MoraleMarkSystem { get; private set; }
        public ShelterEncounterSystem ShelterEncounterSystem { get; private set; }
        public NPC_KessAdler NPCKessAdler { get; private set; }
        public NPC_AnselDuth NPCAnselDuth { get; private set; }
        public NPC_TamsinRook NPCTamsinRook { get; private set; }
        public NPC_LenQuill NPCLenQuill { get; private set; }
        public NPC_HadiMorrow NPCHadiMorrow { get; private set; }
        public NPC_NilaBrant NPCNilaBrant { get; private set; }

        private void BootDutyRoster()
        {
            DutyRosterSystem = new DutyRosterSystem(_worldSeed + 1208);
            MoraleMarkSystem = new MoraleMarkSystem();
            ShelterEncounterSystem = new ShelterEncounterSystem(_worldSeed + 1208);

            var catalog = new DutyRosterCatalogLoader(
                new FileSystemIO(),
                new SystemTextJsonSerializer()
            ).Load(UnityEngine.Application.streamingAssetsPath + "/Data");
            MoraleMarkSystem.BindCatalog(catalog);

            NPCKessAdler = new NPC_KessAdler();
            NPCAnselDuth = new NPC_AnselDuth();
            NPCTamsinRook = new NPC_TamsinRook();
            NPCLenQuill = new NPC_LenQuill();
            NPCHadiMorrow = new NPC_HadiMorrow();
            NPCNilaBrant = new NPC_NilaBrant();

            var kess = CharactersCatalogLoader.GetById(DutyRosterSystem.NpcKessAdler);
            NPCKessAdler.Initialise(kess != null ? kess.display_name : "Kess Adler");
            var ansel = CharactersCatalogLoader.GetById(DutyRosterSystem.NpcAnselDuth);
            NPCAnselDuth.Initialise(ansel != null ? ansel.display_name : "Ansel Duth");
            var tamsin = CharactersCatalogLoader.GetById("npc_tamsin_rook");
            NPCTamsinRook.Initialise(tamsin != null ? tamsin.display_name : "Tamsin Rook");
            var len = CharactersCatalogLoader.GetById("npc_len_quill");
            NPCLenQuill.Initialise(len != null ? len.display_name : "Len Quill");
            var hadi = CharactersCatalogLoader.GetById("npc_hadi_morrow");
            NPCHadiMorrow.Initialise(hadi != null ? hadi.display_name : "Hadi Morrow");
            var nila = CharactersCatalogLoader.GetById("npc_nila_brant");
            NPCNilaBrant.Initialise(nila != null ? nila.display_name : "Nila Brant");

            MergeDutyRosterLocations(expansionUnlocked: false);

            WireDutyRosterEvents();

            _registry.RegisterDaily("duty_roster_system", TickDutyRosterDaily);
            _registry.RegisterDaily("morale_mark_system", TickMoraleMarksDaily);
            _registry.RegisterDaily("shelter_encounter_system", TickShelterEncountersDaily);
            _registry.RegisterEventDriven("npc_kess_adler");
            _registry.RegisterEventDriven("npc_ansel_duth");
            _registry.RegisterEventDriven("npc_tamsin_rook");
            _registry.RegisterEventDriven("npc_len_quill");
            _registry.RegisterEventDriven("npc_hadi_morrow");
            _registry.RegisterEventDriven("npc_nila_brant");

            GameLog.Log("[GameBootstrap] Duty Roster booted: roster, marks, encounters, Kess, Ansel, Tamsin, Len, Hadi, Nila.");
        }

        private void MergeDutyRosterLocations(bool expansionUnlocked)
        {
            if (_locationCatalog == null) return;
            int n = DutyRosterLocationsCatalogLoader.ApplyToCatalog(_locationCatalog, expansionUnlocked);
            if (n > 0)
                GameLog.Log("[GameBootstrap] Duty Roster locations applied: " + n);
        }

        private void WireDutyRosterEvents()
        {
            if (DutyRosterSystem != null)
            {
                DutyRosterSystem.OnNameWritten += id =>
                {
                    if (id == DutyRosterSystem.NpcKessAdler)
                        NPCKessAdler?.AllowPencil(true);
                };
                DutyRosterSystem.OnRosterBurned += () =>
                {
                    NPCKessAdler?.NotifyErased();
                };
                DutyRosterSystem.OnStateChanged += state =>
                {
                    if (SaveSystem != null && state.expansionUnlocked)
                        SaveSystem.SetWorldFlag(DutyRosterSystem.FlagExpUnlocked, true);
                };
                _subscriptions.Track(() =>
                {
                    DutyRosterSystem.OnNameWritten -= _ => { };
                    DutyRosterSystem.OnRosterBurned -= () => { };
                });
            }

            if (MoraleMarkSystem != null)
            {
                MoraleMarkSystem.OnMarkSet += (id, payload) =>
                {
                    var day = TimeSystem != null ? TimeSystem.CurrentDay : 1;
                    GameLog.Log("[DutyRoster] Mark set: " + id + " day " + day);
                };
                MoraleMarkSystem.OnMarkCleared += id =>
                    GameLog.Log("[DutyRoster] Mark cleared: " + id);
            }

            if (ShelterEncounterSystem != null)
            {
                ShelterEncounterSystem.OnShelterEncounterStarted += rec =>
                {
                    if (rec == null) return;
                    var day = TimeSystem != null ? TimeSystem.CurrentDay : 1;
                    GameLog.Log("[DutyRoster] Shelter encounter started: " + rec.id + " (" + rec.kind + ") day " + day);
                };
                ShelterEncounterSystem.OnShelterEncounterResolved += rec =>
                {
                    if (rec == null) return;
                    GameLog.Log("[DutyRoster] Shelter encounter resolved: " + rec.id);
                };
            }

            if (NPCTamsinRook != null)
            {
                NPCTamsinRook.OnStateChanged += state =>
                {
                    if (state.watchShort && MoraleMarkSystem != null)
                        MoraleMarkSystem.SetMark("mark_tamsin_watch_short",
                            null, TimeSystem != null ? TimeSystem.CurrentDay : 1);
                };
            }

            if (NPCKessAdler != null)
            {
                NPCKessAdler.OnStateChanged += state =>
                {
                    if (DutyRosterSystem != null)
                    {
                        if (state.pencilAllowed)
                            DutyRosterSystem.ResolveChartChoice(DutyRosterSystem.ChoiceWritePencil,
                                TimeSystem != null ? TimeSystem.CurrentDay : 1);
                        else if (state.waitInk)
                            DutyRosterSystem.ResolveChartChoice(DutyRosterSystem.ChoiceWaitInk,
                                TimeSystem != null ? TimeSystem.CurrentDay : 1);
                    }
                };
            }
        }

        private void TickDutyRosterDaily(int day)
        {
            if (DutyRosterSystem == null || !DutyRosterSystem.IsUnlocked) return;

            bool loreWrongness = SaveSystem != null &&
                SaveSystem.GetWorldFlag("lore_allocation_wrongness");
            bool holdfastClerkStarted = SaveSystem != null &&
                SaveSystem.GetWorldFlag("quest_holdfast_the_clerk_started");

            if (!DutyRosterSystem.State.wallInspected && !DutyRosterSystem.State.mutationRosterInUse)
            {
                if (DutyRosterSystem.CanBeginChart(day, loreWrongness, holdfastClerkStarted))
                {
                    DutyRosterSystem.NotifyWallInspected();
                }
            }

            if (DutyRosterSystem.State.kessPencilAllowed)
            {
                var occupants = GatherHomeOccupants();
                DutyRosterSystem.TickMorning(day, occupants);
            }
            else if (DutyRosterSystem.ChartScript == DutyRosterSystem.ScriptBlank
                && !DutyRosterSystem.State.waitInk
                && !DutyRosterSystem.State.kessPencilAllowed)
            {
                var occupants = GatherHomeOccupants();
                DutyRosterSystem.TickMorning(day, occupants);
            }

            TickDutyRosterSecondWinter(day);
            TickDutyRosterEndings(day);

            SyncDutyRosterSaveFlags();
        }

        /// <summary>
        /// Second Winter season profile (spec §5.4). When active it shortens Ice Road
        /// windows and raises shelter encounter rate. Data, not a 4th simulation class.
        /// Seeded year-2 winter or forced after first Ice Road window if Holdfast live.
        /// </summary>
        private void TickDutyRosterSecondWinter(int day)
        {
            if (DutyRosterSystem == null) return;

            bool winterDue = day >= 360; // seeded year-2 winter
            bool roadHadWindow = IceRoadSystem != null
                && IceRoadSystem is { } ice && ice.State != null && ice.State.windowsCompleted > 0;
            bool forceAfterWindow = roadHadWindow && day >= 300;

            if (winterDue || forceAfterWindow)
            {
                if (!DutyRosterSystem.IsSecondWinterActive)
                {
                    DutyRosterSystem.SetSecondWinterActive(true);
                    ShelterEncounterSystem?.SetSecondWinter(
                        DutyRosterSystem.SecondWinterEncounterWeight, day);
                    if (MoraleMarkSystem != null)
                        MoraleMarkSystem.SetMark("mark_second_winter", null, day);
                    if (IceRoadSystem != null)
                        IceRoadSystem.ShortenWindowLength(DutyRosterSystem.SecondWinterWindowMinDays,
                            DutyRosterSystem.SecondWinterWindowMaxDays,
                            DutyRosterSystem.SeedUtilityOffset + day);
                    GameLog.Log("[DutyRoster] Second Winter active day " + day);
                }
            }
        }

        /// <summary>
        /// Endings (spec §3). Every ending writes world state the hatch reversed reads.
        /// The game does not rank them.
        /// </summary>
        private void TickDutyRosterEndings(int day)
        {
            if (DutyRosterSystem == null || SaveSystem == null) return;

            if (DutyRosterSystem.State.chartScript == DutyRosterSystem.ScriptInk
                && string.IsNullOrEmpty(DutyRosterSystem.State.endingId))
            {
                DutyRosterSystem.ResolveInkEnding(day);
            }
        }

        private void TickMoraleMarksDaily(int day)
        {
            if (MoraleMarkSystem == null) return;
        }

        private void TickShelterEncountersDaily(int day)
        {
            if (ShelterEncounterSystem == null || !ShelterEncounterSystem.IsUnlocked) return;

            // Reset the per-night counter at the start of a new day.
            if (ShelterEncounterSystem.LastEncounterDay != day)
                ShelterEncounterSystem.ResetNightCounter(day);
        }

        private List<DutyRosterOccupant> GatherHomeOccupants()
        {
            var list = new List<DutyRosterOccupant>();
            if (Survivors == null) return list;
            for (int i = 0; i < Survivors.Count; i++)
            {
                var sv = Survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                if (sv.IsOnExpedition) continue;
                list.Add(new DutyRosterOccupant
                {
                    survivorId = sv.Id,
                    displayName = sv.DisplayName,
                    occupationObserved = sv.PreWarProfessionId ?? sv.ArchetypeId ?? "",
                    sleptHere = true
                });
            }
            return list;
        }

        private void SyncDutyRosterSaveFlags()
        {
            if (DutyRosterSystem == null || SaveSystem == null) return;
            if (DutyRosterSystem.State.mutationRosterInUse)
                SaveSystem.SetWorldFlag(DutyRosterSystem.MutationRosterInUse, true);
            if (DutyRosterSystem.State.mutationRosterStillBlank)
                SaveSystem.SetWorldFlag(DutyRosterSystem.MutationRosterStillBlank, true);
            if (DutyRosterSystem.State.mutationRosterBurned)
                SaveSystem.SetWorldFlag(DutyRosterSystem.MutationRosterBurned, true);
            if (DutyRosterSystem.State.mutationRationProtocol)
                SaveSystem.SetWorldFlag(DutyRosterSystem.MutationRationProtocol, true);
        }
    }
}