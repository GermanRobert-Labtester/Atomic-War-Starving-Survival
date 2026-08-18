using System.Collections.Generic;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Phase-2 snapshot registry. Lists ASHFALL runtime panels that the
    /// SnapshotOrchestrator can mount into an isolated SubViewport and
    /// capture to PNG. Stable IDs stay constant across regenerations so
    /// visual-regression diffs are valid.
    /// </summary>
    public static class SnapshotHarness
    {
        public struct Target
        {
            public string StableId;
            public string Title;
            public string PanelCtor;
            public string StateHint;
            public int    Width;
            public int    Height;
        }

        public static readonly Target[] Targets = new[]
        {
            new Target{ StableId="inventory_default",         Title="Inventory (default state)",            PanelCtor="AtomicWar.GodotApp.UI.InventoryPanel",                 StateHint="default",  Width=1280, Height=720 },
            new Target{ StableId="survivors_default",         Title="Survivors roster (default state)",     PanelCtor="AtomicWar.GodotApp.UI.SurvivorsPanel",                 StateHint="default",  Width=1280, Height=720 },
            new Target{ StableId="medical_default",           Title="Medical triage (default state)",       PanelCtor="AtomicWar.GodotApp.UI.MedicalPanel",                   StateHint="default",  Width=1280, Height=720 },
            new Target{ StableId="radio_default",             Title="Radio intercept (default state)",      PanelCtor="AtomicWar.GodotApp.UI.RadioPanel",                     StateHint="default",  Width=1280, Height=720 },
            new Target{ StableId="weather_default",           Title="Weather fallout (default state)",      PanelCtor="AtomicWar.GodotApp.UI.WeatherPanel",                   StateHint="default",  Width=1280, Height=720 },
            new Target{ StableId="shelter_default",           Title="Shelter panel (default state)",        PanelCtor="AtomicWar.GodotApp.UI.ShelterPanel",                   StateHint="default",  Width=1280, Height=720 },
            new Target{ StableId="journal_default",           Title="Journal book (default state)",         PanelCtor="AtomicWar.GodotApp.UI.JournalPanel",                   StateHint="default",  Width=1280, Height=720 },
            new Target{ StableId="verdict_default",           Title="Verdict panel (default state)",        PanelCtor="AtomicWar.GodotApp.VerdictPanel",                     StateHint="default",  Width=1280, Height=720 },
            new Target{ StableId="trade_default",             Title="Trade screen (default state)",         PanelCtor="AtomicWar.GodotApp.Economy.TradeScreenGodotPanel",     StateHint="default",  Width=1280, Height=720 },

            // Phase 12 pilots — mapped to the new dashboard shell + data-grid primitives.
            new Target{ StableId="survival_workstation_default", Title="Survival Workstation (#19 Stitch)",          PanelCtor="AtomicWar.GodotApp.UI.SurvivalWorkstationPanel",        StateHint="default", Width=1280, Height=800 },
            new Target{ StableId="caravan_barter_default",       Title="Caravan Barter Ledger (#35 Stitch)",         PanelCtor="AtomicWar.GodotApp.UI.CaravanBarterLedgerPanel",      StateHint="default", Width=1280, Height=800 },
            new Target{ StableId="shelter_hud_default",          Title="Shelter HUD (#40 Stitch)",                   PanelCtor="AtomicWar.GodotApp.UI.ShelterHudPanel",                  StateHint="default", Width=1280, Height=800 },

            // Phase 13 — Tier-2 matrices / dashboard panels.
            new Target{ StableId="faction_matrix_default",       Title="Faction Matrix (#49/#53 Stitch)",            PanelCtor="AtomicWar.GodotApp.UI.FactionMatrixPanel",             StateHint="default", Width=1280, Height=800 },
            new Target{ StableId="dose_ledger_default",          Title="Dose Ledger (#59 Stitch)",                  PanelCtor="AtomicWar.GodotApp.UI.DoseLedgerPanel",                 StateHint="default", Width=1280, Height=800 },
            new Target{ StableId="verdict_dashboard_default",    Title="Verdict Dashboard (#15 Stitch)",             PanelCtor="AtomicWar.GodotApp.UI.VerdictDashboardPanel",           StateHint="default", Width=1280, Height=800 },
            new Target{ StableId="weather_dashboard_default",    Title="Weather Dashboard (#24 Stitch)",             PanelCtor="AtomicWar.GodotApp.UI.WeatherPanel",                    StateHint="default", Width=1280, Height=800 },

            // Phase 15 — Tier-A4 Hydroponics dashboard (#51 Stitch).
            new Target{ StableId="greenhouse_default",          Title="Glass Orchard / Hydroponics (#51 Stitch)",   PanelCtor="AtomicWar.GodotApp.UI.GreenhousePanel",                 StateHint="default", Width=1280, Height=800 },

            // Phase 16 — Tier-A1 Silent Foundry dashboard (#1 Stitch).
            new Target{ StableId="silent_foundry_default",      Title="Silent Foundry / Cupola (#1 Stitch)",     PanelCtor="AtomicWar.GodotApp.UI.SilentFoundryPanel",             StateHint="default", Width=1280, Height=800 },

            // Phase 17 — Tier-A5 Expedition Radar (#10 Stitch).
            new Target{ StableId="expedition_radar_default",    Title="Expedition Radar (#10 Stitch)",          PanelCtor="AtomicWar.GodotApp.UI.ExpeditionRadarPanel",           StateHint="default", Width=1280, Height=800 },

            // Phase 19 — Skill Matrix Dashboard (#22 Stitch).
            new Target{ StableId="skill_matrix_default",        Title="Skill Matrix (#22 Stitch)",             PanelCtor="AtomicWar.GodotApp.UI.SkillMatrixPanel",               StateHint="default", Width=1280, Height=800 },

            // Phase 20 — Tier-A3 Duty Roster shift half (#22 Stitch matrix).
            new Target{ StableId="duty_roster_default",         Title="Duty Roster (shift half) #22",         PanelCtor="AtomicWar.GodotApp.UI.DutyRosterPanel",                StateHint="default", Width=1280, Height=800 },

            // Phase 21 — Factions Narrative shell (#49 / #53 Stitch).
            new Target{ StableId="factions_narrative_default",   Title="Factions Narrative (#49/#53)",       PanelCtor="AtomicWar.GodotApp.UI.FactionsNarrativePanel",       StateHint="default", Width=1280, Height=800 },

            // Phase 22 — Tier-3 Combat HUD overlay (#58 Stitch).
            new Target{ StableId="combat_hud_default",            Title="Combat HUD Overlay (#58)",           PanelCtor="AtomicWar.GodotApp.UI.CombatHudOverlay",               StateHint="default", Width=1280, Height=800 },

            // Phase 23 — Tier-3 Map Atlas (#5 Stitch).
            new Target{ StableId="map_atlas_default",             Title="Map Atlas (#5)",                       PanelCtor="AtomicWar.GodotApp.UI.MapAtlasPanel",                  StateHint="default", Width=1280, Height=800 },

            // Phase 24 — Tier-3 Maritime Atlas (#48 Stitch).
            new Target{ StableId="maritime_atlas_default",        Title="Maritime Atlas (#48)",                  PanelCtor="AtomicWar.GodotApp.UI.MaritimeAtlasPanel",             StateHint="default", Width=1280, Height=800 },

            // Phase 25 — Tier-3 Muster Atlas (Expansion 06).
            new Target{ StableId="muster_atlas_default",          Title="The Muster (Expansion 06)",             PanelCtor="AtomicWar.GodotApp.UI.MusterAtlasPanel",              StateHint="default", Width=1280, Height=800 },

            // Phase 26 — Tier-3 Quests Atlas.
            new Target{ StableId="quests_atlas_default",           Title="Quests Atlas",                          PanelCtor="AtomicWar.GodotApp.UI.QuestsAtlasPanel",              StateHint="default", Width=1280, Height=800 },

            // Phase 27 — Tier-3 Standing Record Atlas (Expansion 03).
            new Target{ StableId="standing_record_atlas_default",  Title="Standing Record Atlas (Expansion 03)",  PanelCtor="AtomicWar.GodotApp.UI.StandingRecordAtlasPanel",     StateHint="default", Width=1280, Height=800 },

            // Phase 28 — Tier-3 Research Atlas.
            new Target{ StableId="research_atlas_default",          Title="Research Atlas",                        PanelCtor="AtomicWar.GodotApp.UI.ResearchAtlasPanel",            StateHint="default", Width=1280, Height=800 },
        };
    }
}
