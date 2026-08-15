using System;
using AtomicWar._Game.Data;
using AtomicWar._Game.Factions;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Lore bible 05_FACTIONS — the Currents' code layer.
    ///
    /// Constructs the five new Current state classes (the other three —
    /// Archivists, Sun-Seekers, Osteophages — already exist in code) and
    /// initialises each from its currents.json entry via
    /// CurrentsCatalogLoader. GameBootstrap is the bridge because the
    /// Factions assembly has no reference to Data.
    ///
    /// Every class is save-safe (serializable state + Capture/Restore) and
    /// raises C# events on state change, per the house rules.
    /// </summary>
    public partial class GameBootstrap
    {
        public NPC_Archivists NPCArchivists { get; private set; }
        public NPC_SunSeekers NPCSunSeekers { get; private set; }
        public NPC_Osteophages NPCOsteophages { get; private set; }
        public NPC_Lamplighters NPCLamplighters { get; private set; }
        public NPC_QuietHouse NPCQuietHouse { get; private set; }
        public NPC_GrainExchange NPCGrainExchange { get; private set; }
        public NPC_Tally NPCTally { get; private set; }
        public NPC_Undertow NPCUndertow { get; private set; }

        /// <summary>Called from InitDeepLore after the narrative systems boot.</summary>
        private void BootCurrents()
        {
            // The three pre-existing classes become live Currents alongside the five new.
            NPCArchivists = new NPC_Archivists();
            NPCSunSeekers = new NPC_SunSeekers();
            NPCOsteophages = new NPC_Osteophages();
            NPCLamplighters = new NPC_Lamplighters();
            NPCQuietHouse = new NPC_QuietHouse();
            NPCGrainExchange = new NPC_GrainExchange();
            NPCTally = new NPC_Tally();
            NPCUndertow = new NPC_Undertow();

            // Apply the data catalog: display names + badge adoptions.
            var archivists = CurrentsCatalogLoader.GetById("faction_archivists");
            if (archivists != null)
                NPCArchivists.Initialise(archivists.display_name);

            var sunSeekers = CurrentsCatalogLoader.GetById("faction_sun_seekers");
            if (sunSeekers != null)
                NPCSunSeekers.Initialise(sunSeekers.display_name);

            var osteophages = CurrentsCatalogLoader.GetById("faction_osteophages");
            if (osteophages != null)
                NPCOsteophages.Initialise(osteophages.display_name);

            var lamplighters = CurrentsCatalogLoader.GetById("faction_lamplighters");
            if (lamplighters != null)
                NPCLamplighters.Initialise(lamplighters.display_name, lamplighters.badge_asset_id);

            var quietHouse = CurrentsCatalogLoader.GetById("faction_quiet_house");
            if (quietHouse != null)
                NPCQuietHouse.Initialise(quietHouse.display_name);

            var grainExchange = CurrentsCatalogLoader.GetById("faction_grain_exchange");
            if (grainExchange != null)
                NPCGrainExchange.Initialise(grainExchange.display_name, grainExchange.badge_asset_id);

            var tally = CurrentsCatalogLoader.GetById("faction_the_tally");
            if (tally != null)
                NPCTally.Initialise(tally.display_name, tally.badge_asset_id);

            var undertow = CurrentsCatalogLoader.GetById("faction_undertow");
            if (undertow != null)
                NPCUndertow.Initialise(undertow.display_name);

            _registry.RegisterPerSubstep("npc_lamplighters", h => NPCLamplighters.Tick(h));
            _registry.RegisterPerSubstep("npc_sun_seekers", h => TickSunSeekersNightRule(h));
            _registry.RegisterDaily("npc_tally", day => NPCTally.EnforceDueContracts(day));
            _registry.RegisterDaily("npc_grain_exchange", day => NPCTallySafeSeasonal(day));

            // Purely event-driven Currents — constructed, save-wired, fired by
            // flags/AI actions rather than the tick loop (C-1 registry hygiene).
            _registry.RegisterEventDriven("npc_archivists");
            _registry.RegisterEventDriven("npc_osteophages");
            _registry.RegisterEventDriven("npc_quiet_house");
            _registry.RegisterEventDriven("npc_undertow");

            GameLog.Log("[GameBootstrap] Currents booted: 8 state classes initialised from currents.json.");
        }

        /// <summary>
        /// Lore bible interlocks — the Sun-Seekers are the only group that does
        /// not want the routes lit at night. Assessed each substep; one
        /// grievance per night while the Lamplighters keep the beacons burning.
        /// </summary>
        private void TickSunSeekersNightRule(float gameHours)
        {
            if (NPCSunSeekers == null || NPCLamplighters == null) return;
            int day = TimeSystem != null ? TimeSystem.CurrentDay : 1;
            float hour = TimeSystem != null ? TimeSystem.CurrentHour : 12f;
            NPCSunSeekers.AssessNightLamps(NPCLamplighters, hour, day);
        }

        /// <summary>
        /// The Exchange declines by season, not by day; the daily hook advances
        /// it once per ~30 days so the ninety-day quietening reads correctly.
        /// </summary>
        private int _lastGrainSeasonDay;
        private void NPCTallySafeSeasonal(int day)
        {
            if (NPCGrainExchange == null || !NPCGrainExchange.State.playerControlsBoard) return;
            if (day < _lastGrainSeasonDay + 30) return;
            _lastGrainSeasonDay = day;
            NPCGrainExchange.TickSeasonalDecline();
        }
    }
}
