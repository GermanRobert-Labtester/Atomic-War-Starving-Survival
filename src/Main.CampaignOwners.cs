// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Economy;
using Ashfall.Core.Expeditions;
using Godot;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private void RegisterProductionCampaignOwners()
        {
            if (_campaignDay == null) return;

            // Phase 1: Environment, Weather & Base Core
            _campaignDay.Register("holdfast_core", new HoldfastCoreDayOwner(this), phase: 1);
            _campaignDay.Register("maritime_deep_coast", new DeepCoastMaritimeDayOwner(this), phase: 1);
            _campaignDay.Register("power_grid", new PowerGridDayOwner(this), phase: 1);
            // Plan B98: nuclear output is an external, fuel-free projection.
            // Its ordinal sorts before power_grid so restored RTG/sealed-cell
            // output is visible when the day's load is resolved.
            _campaignDay.Register("nuclear_core", new NuclearCoreDayOwner(this), phase: 1);
            // Plans B74-B77: ORC output is published before the grid owner
            // resolves the day's load, while chamber and tube milestones run
            // in the production phase.
            _campaignDay.Register("geothermal_orc", new GeothermalOrcDayOwner(this), phase: 1);
            _campaignDay.Register("weather_world", new WeatherWorldDayOwner(this), phase: 1);
            // Plan B68 — geological pulse progression precedes production
            // (foundry sees the quake's interruption context the same day).
            _campaignDay.Register("seismic_geology", new SeismicGeologyDayOwner(this), phase: 1);

            // Phase 2: Production, Infrastructure & Survival Basics
            _campaignDay.Register("crafting_production", new CraftingProductionDayOwner(this), phase: 2);
            _campaignDay.Register("economy_market", new EconomyMarketDayOwner(this), phase: 2);
            _campaignDay.Register("greenhouse_foundry", new GreenhouseFoundryDayOwner(this), phase: 2);
            _campaignDay.Register("aeroponics", new AeroponicsDayOwner(this), phase: 2);
            _campaignDay.Register("pneumatic_dispatch", new PneumaticDispatchDayOwner(this), phase: 2);
            // Plan B69 — cryo thermal/viability update follows the foundry
            // (shared grid: brownout from the furnace reaches the vault same-day).
            _campaignDay.Register("cryo_vault", new CryoVaultDayOwner(this), phase: 2);
            // Plan B89 — precision metrology drift + workshop projection after
            // seismic (phase 1) so quake disturbance lands before daily drift.
            _campaignDay.Register("precision_metrology", new PrecisionMetrologyDayOwner(this), phase: 2);
            // Plan B87 — aquaponics ecology after power/thermal owners so
            // brownout and room heat are queryable for the same day.
            _campaignDay.Register("aquaponics", new AquaponicsDayOwner(this), phase: 2);
            _campaignDay.Register("shelter_facilities", new ShelterFacilitiesDayOwner(this), phase: 2);
            _campaignDay.Register("shelter_fire", new ShelterFireDayOwner(this), phase: 2);
            _campaignDay.Register("starting_level_rations", new StartingLevelRationsDayOwner(this), phase: 2);
            _campaignDay.Register("plan_166_research", new Plan166ResearchDayOwner(this), phase: 4);
            _campaignDay.Register("plan_168_fluid", new Plan168FluidDayOwner(this), phase: 2);

            // Phase 3: Survivors, Medical, Disease & Social
            _campaignDay.Register("duty_roster", new DutyRosterDayOwner(this), phase: 3);
            // Plan 210 — sanitation runs BEFORE the disease tick within phase 3:
            // `hygiene` (h) sorts alphabetically before `medical_disease` (m),
            // so waste burden → hygiene → pathogen exposure modifiers are
            // current when the disease authority resolves its daily tick.
            _campaignDay.Register("hygiene", new HygieneDayOwner(this), phase: 3);
            _campaignDay.Register("medical_disease", new MedicalDiseaseDayOwner(this), phase: 3);
            _campaignDay.Register("phase0_psychology", new Phase0PsychologyDayOwner(this), phase: 3);
            _campaignDay.Register("survivor_social", new SurvivorSocialDayOwner(this), phase: 3);
            _campaignDay.Register("survivors_needs", new SurvivorsNeedsDayOwner(this), phase: 3);

            // Phase 4: Expeditions, World, Factions & Quests
            _campaignDay.Register("expeditions_caravans", new ExpeditionsCaravansDayOwner(this), phase: 4);
            _campaignDay.Register("narrative_quests_verdict", new NarrativeQuestsVerdictDayOwner(this), phase: 4);
            // Task 122: ticks after expeditions (ordinal 'w' > 'e') so it reads
            // fresh sortie results, and after narrative for fresh faction dominance.
            _campaignDay.Register("world_evolution", new EvolvingWorldDayOwner(this), phase: 4);
            // Plan IV: ledger debt ages with the campaign; forfeits dispatch
            // consequences into faction war / raids / inventory / labor.
            _campaignDay.Register("debt_ledger", new DebtLedgerDayOwner(this), phase: 4);
            // Plan 211 — underworld stock refresh + debt/heat tick AFTER the
            // debt-ledger tick: `underworld_market` (u) sorts alphabetically
            // after `debt_ledger` (d) within phase 4 (owner id is not the
            // section key — the save section is `black_market`).
            _campaignDay.Register("underworld_market", new UnderworldMarketDayOwner(this), phase: 4);
            // Flagship XI (Plan 156): underground hazards tick after expeditions
            // (ordinal 's' > 'e', < 'w') so the bridge reads fresh sortie phases.
            _campaignDay.Register("subterranean_network", new SubterraneanDayOwner(this), phase: 4);
            // Flagship XI (Plan 157): psyops broadcast day resolves after
            // expeditions (leaflets) and alongside the world-evolution radio feed.
            _campaignDay.Register("psyops", new PsyOpsDayOwner(this), phase: 4);
            // Plan 173 Phase 2: program prep ticks after psyops so StartCampaign
            // on delivery can reach an already-constructed PsyOpsSystem.
            _campaignDay.Register("radio_program_production", new RadioProgramProductionDayOwner(this), phase: 4);
            // Plans 162-165 (Plan 164): breakdown arcs evaluate AFTER the
            // phase-3 needs tick finalized canonical stress (plan §11.3).
            _campaignDay.Register("psychology_arcs_162", new PsychologyArcsDayOwner(this), phase: 4);
            _campaignDay.Register("plan_167_espionage", new Plan167EspionageDayOwner(this), phase: 4);
            _campaignDay.Register("plan_169_procedural_narrative", new Plan169NarrativeDayOwner(this), phase: 4);
            // Plan 38 — commitments/deadlines evaluate late in phase 4 so the day's
            // expedition/faction facts are settled before a missed obligation routes
            // its consequence into faction standing and the consequence ledger.
            // "shelter_commitments" is the owner id (not the section key `commitment`).
            _campaignDay.Register("shelter_commitments", new CommitmentDayOwner(this), phase: 4);

            // Phase 5: Events, Memorial & Final Evaluation
            _campaignDay.Register("host_events", new HostEventsDayOwner(this), phase: 5);
            _campaignDay.Register("memorial", new MemorialDayOwner(this), phase: 5);
            // Plan 29 29A: room-history day milestones. Reads only the identity
            // catalog and writes journal knowledge keys; no system ticks here.
            _campaignDay.Register("shelter_room_history", new ShelterRoomHistoryDayOwner(this), phase: 5);
            // Plan 58 — the outpost network runs after the roster, expedition and
            // economy owners so its garrison, rations and hostile pressure read
            // the day's already-finalized population and supply state.
            _campaignDay.Register("outpost_settlement", new OutpostSettlementDayOwner(this), phase: 5);
            // Plan 135 — the weather→gameplay cascade expires fronts whose
            // duration ended, so it runs after the owners that consumed the
            // day's weather and before retention bounds the logs they wrote.
            _campaignDay.Register("weather_cascade", new WeatherCascadeDayOwner(this), phase: 5);
            // Plan 134 — dynamic faction territory and supply line control: delivers
            // active corridors and reinforces held nodes.
            _campaignDay.Register("territory_control", new TerritoryControlDayOwner(this), phase: 5);
            // Plan 136 — wildlife trapping food pipeline & cooking system: progresses
            // active cooking operations and decontaminates fallout-tainted meat.
            _campaignDay.Register("cooking", new CookingDayOwner(this), phase: 5);
            // Plan 137 — needs to performance cascade: evaluates hunger/thirst/fatigue/cold on survivor performance.
            _campaignDay.Register("needs_performance", new NeedsPerformanceDayOwner(this), phase: 5);
            // Plan 140 — generational legacy and campaign inheritance: evaluates active traits and heritage continuity.
            _campaignDay.Register("campaign_legacy", new CampaignLegacyDayOwner(this), phase: 5);
            // Plan 141 — research downstream unlocks bridge: grants breakthrough items, crafting recipes, and capabilities.
            _campaignDay.Register("research_unlock", new ResearchUnlockDayOwner(this), phase: 5);
            // Plan 145 — unified ending resolution: evaluates whole-campaign state and epilogue personalization.
            _campaignDay.Register("unified_ending", new UnifiedEndingDayOwner(this), phase: 5);
            // Plan 147 — per-NPC memory and relationship depth: decays old memories and grudges.
            _campaignDay.Register("npc_memory", new NpcMemoryDayOwner(this), phase: 5);
            // Plan 148 — ideological friction: evaluates bunker frictions, conversions, and confrontations.
            _campaignDay.Register("ideological_friction", new IdeologicalFrictionDayOwner(this), phase: 5);
            // Plan 150 — romance & family: advances bonded tenure and forms new attractions from canonical affinity.
            _campaignDay.Register("romance_family", new RomanceFamilyDayOwner(this), phase: 5);
            // Plan 152 — vehicle customization & mobile base: keeps the module catalog bound for the day report.
            _campaignDay.Register("vehicle_customization", new VehicleCustomizationDayOwner(this), phase: 5);
            // Plan 174 — procedural survivor backstories: updates origin mechanics.
            _campaignDay.Register("backstory", new BackstoryDayOwner(this), phase: 5);
            // Plan 175 — meta progression: evaluates prestige and New Game+ boons.
            _campaignDay.Register("meta_progression", new MetaProgressionDayOwner(this), phase: 5);
            // Plan 167 — underground tunnel network: applies daily structural wear and collapse risk.
            _campaignDay.Register("tunnel_network", new TunnelNetworkDayOwner(this), phase: 5);
            // Plan 166 — shelter identity: selects the founding origin on a fresh campaign.
            _campaignDay.Register("shelter_identity", new ShelterIdentityDayOwner(this), phase: 5);
            // Plan 192 — scheduled trade route contracts: advances run schedules and audits tariffs.
            _campaignDay.Register("trade_routes", new TradeRouteDayOwner(this), phase: 5);
            // Plan 199 — seasonal human migration: tracks regional population weight transitions.
            _campaignDay.Register("human_migration", new HumanMigrationDayOwner(this), phase: 5);
            // Plan 159 — shelter governance: evaluates policy consent, disputes, and shelter stability.
            _campaignDay.Register("shelter_governance", new ShelterGovernanceDayOwner(this), phase: 5);
            // Plan 55 — retention runs last of all: it bounds the campaign logs
            // every other owner just appended to for this day.
            _campaignDay.Register("retention", new RetentionDayOwner(this), phase: 5);
            // Flagship institutions (Tasks 5-8): culture, diplomacy, sky defense, sanatorium.
            RegisterFlagshipInstitutionsOwner();
        }

        /// <summary>Plan 134 territory-control day owner (ownerId <c>territory_control</c>, phase 5).</summary>
        private sealed class TerritoryControlDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            private Ashfall.Core.Factions.TerritoryControlSaveState? _snapshot;
            public TerritoryControlDayOwner(Main m) => _m = m;

            public void CapturePreDaySnapshot(int day)
            {
                _m.EnsureTerritoryControl();
                _snapshot = _m.TerritoryControl?.System.CaptureState();
            }

            public void RestorePreDaySnapshot(int day)
            {
                if (_snapshot != null) _m.TerritoryControl?.System.RestoreState(_snapshot);
            }

            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.EnsureTerritoryControl();
                _m.TickTerritoryControl(day, _m._campaignDay?.Rng?.GetStream(Ashfall.Core.Random.CampaignStreamIds.Shelter)?.Rng);
                var census = _m.TerritoryControl?.ReadCensus();
                events.Add(new DayStateChangeEvent(
                    "territory_control_ticked", "territory_control", null, null, census?.ContestedLocations ?? 0));
            }
        }

        /// <summary>Plan 136 cooking day owner (ownerId <c>cooking</c>, phase 5).</summary>
        private sealed class CookingDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            private Ashfall.Core.Cooking.CookingState? _snapshot;
            public CookingDayOwner(Main m) => _m = m;

            public void CapturePreDaySnapshot(int day)
            {
                _m.EnsureCooking();
                _snapshot = _m.Cooking?.System.CaptureState();
            }

            public void RestorePreDaySnapshot(int day)
            {
                if (_snapshot != null) _m.Cooking?.System.RestoreState(_snapshot);
            }

            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.EnsureCooking();
                _m.TickCooking(day);
                var census = _m.Cooking?.Census;
                events.Add(new DayStateChangeEvent(
                    "cooking_ticked", "cooking", null, null, census?.TotalMealsPrepared ?? 0));
            }
        }

        /// <summary>Plan 135 weather-cascade day owner (ownerId <c>weather_cascade</c>, phase 5).</summary>
        private sealed class WeatherCascadeDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            private Ashfall.Core.Weather.WeatherCascadeState? _snapshot;
            public WeatherCascadeDayOwner(Main m) => _m = m;

            public void CapturePreDaySnapshot(int day)
            {
                _m.EnsureWeatherCascade();
                _snapshot = _m.WeatherCascade?.System.CaptureState();
            }

            public void RestorePreDaySnapshot(int day)
            {
                if (_snapshot != null) _m.WeatherCascade?.System.RestoreState(_snapshot);
            }

            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                int activeBefore = _m.WeatherCascade?.System.State.activeEvents.Count ?? 0;
                _m.TickWeatherCascade(day);
                int activeAfter = _m.WeatherCascade?.System.State.activeEvents.Count ?? 0;
                if (activeAfter != activeBefore)
                    events.Add(new DayStateChangeEvent(
                        "weather_cascade_ticked", "weather_cascade", null, null, activeAfter));
            }
        }

        /// <summary>Plan 58 outpost day owner (ownerId <c>outpost_settlement</c>, phase 5).</summary>
        private sealed class OutpostSettlementDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            private Ashfall.Core.Settlements.OutpostSettlementState? _snapshot;
            public OutpostSettlementDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day)
            {
                _m.EnsureOutpostSettlement();
                _snapshot = _m.OutpostSettlement?.CaptureState();
            }
            public void RestorePreDaySnapshot(int day)
            {
                if (_snapshot != null) _m.OutpostSettlement?.RestoreState(_snapshot);
            }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.EnsureOutpostSettlement();
                if (_m.OutpostSettlement == null) return;
                _m.TickOutpostSettlement(day);
                var census = _m.OutpostSettlement.ReadCensus();
                events.Add(new DayStateChangeEvent("outpost_network_ticked", "outpost_settlement", null, null,
                    census.Established));
            }
        }

        /// <summary>Plan 55 retention day owner (ownerId <c>retention</c>, phase 5).</summary>
        private sealed class RetentionDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public RetentionDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { /* retention is idempotent; captured via save section */ }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.EnsureRetention();
                int before = _m.Retention?.Passes ?? 0;
                _m.TickRetention(day);
                int after = _m.Retention?.Passes ?? 0;
                if (after != before)
                    events.Add(new DayStateChangeEvent("retention_ticked", "retention", null, null, after));
            }
        }

        /// <summary>Plan 137 needs-performance day owner (ownerId <c>needs_performance</c>, phase 5).</summary>
        private sealed class NeedsPerformanceDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public NeedsPerformanceDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { /* derived read projection */ }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.EnsureNeedsPerformance();
                _m.TickNeedsPerformance(day);
                var census = _m.GetNeedsPerformanceCensus();
                events.Add(new DayStateChangeEvent(
                    "needs_performance_ticked", "needs_performance", null, null, census.TotalSurvivorsEvaluated));
            }
        }

        /// <summary>Plan 140 campaign-legacy day owner (ownerId <c>campaign_legacy</c>, phase 5).</summary>
        private sealed class CampaignLegacyDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            private Ashfall.Core.Legacy.CampaignLegacyState? _snapshot;
            public CampaignLegacyDayOwner(Main m) => _m = m;

            public void CapturePreDaySnapshot(int day)
            {
                _m.EnsureCampaignLegacy();
                _snapshot = _m._campaignLegacy?.System.CaptureState();
            }

            public void RestorePreDaySnapshot(int day)
            {
                if (_snapshot != null) _m._campaignLegacy?.System.RestoreState(_snapshot);
            }

            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.EnsureCampaignLegacy();
                _m.TickCampaignLegacy(day);
                var census = _m._campaignLegacy?.GetCensus();
                events.Add(new DayStateChangeEvent(
                    "campaign_legacy_ticked", "campaign_legacy", null, null, census?.CompletedCampaignsCount ?? 0));
            }
        }

        /// <summary>Plan 141 research-unlock day owner (ownerId <c>research_unlock</c>, phase 5).</summary>
        private sealed class ResearchUnlockDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            private Ashfall.Core.Research.ResearchUnlockState? _snapshot;
            public ResearchUnlockDayOwner(Main m) => _m = m;

            public void CapturePreDaySnapshot(int day)
            {
                _m.SetupResearchUnlockBridge();
                _snapshot = _m._researchUnlock?.CaptureState();
            }

            public void RestorePreDaySnapshot(int day)
            {
                if (_snapshot != null) _m._researchUnlock?.RestoreState(_snapshot);
            }

            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupResearchUnlockBridge();
                _m.TickResearchUnlock(day);
                var census = _m._researchUnlock?.Census;
                events.Add(new DayStateChangeEvent(
                    "research_unlock_ticked", "research_unlock", null, null, census?.GrantedUnlocksCount ?? 0));
            }
        }

        /// <summary>Plan 145 unified-ending day owner (ownerId <c>unified_ending</c>, phase 5).</summary>
        private sealed class UnifiedEndingDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            private Ashfall.Core.Endgame.UnifiedEndingSaveState? _snapshot;
            public UnifiedEndingDayOwner(Main m) => _m = m;

            public void CapturePreDaySnapshot(int day)
            {
                _m.SetupUnifiedEnding();
                _snapshot = _m._unifiedEnding?.CaptureState();
            }

            public void RestorePreDaySnapshot(int day)
            {
                if (_snapshot != null) _m._unifiedEnding?.RestoreState(_snapshot);
            }

            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupUnifiedEnding();
                _m.TickUnifiedEnding(day);
                var census = _m._unifiedEnding?.Census;
                events.Add(new DayStateChangeEvent(
                    "unified_ending_ticked", "unified_ending", null, null, census?.IsResolved == true ? 1f : 0f));
            }
        }

        /// <summary>Plan 147 per-NPC memory day owner (ownerId <c>npc_memory</c>, phase 5).</summary>
        private sealed class NpcMemoryDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            private Ashfall.Core.Narrative.NpcMemorySaveState? _snapshot;
            public NpcMemoryDayOwner(Main m) => _m = m;

            public void CapturePreDaySnapshot(int day)
            {
                _m.SetupNpcMemory();
                _snapshot = _m._npcMemory?.CaptureState();
            }

            public void RestorePreDaySnapshot(int day)
            {
                if (_snapshot != null) _m._npcMemory?.RestoreState(_snapshot);
            }

            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupNpcMemory();
                _m.TickNpcMemory(day);
                var census = _m._npcMemory?.Census;
                events.Add(new DayStateChangeEvent(
                    "npc_memory_ticked", "npc_memory", null, null, census?.TotalTrackedNpcs ?? 0));
            }
        }

        /// <summary>Plan 148 ideological friction day owner (ownerId <c>ideological_friction</c>, phase 5).</summary>
        private sealed class IdeologicalFrictionDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            private Ashfall.Core.Survivors.IdeologicalFrictionEventSaveState? _snapshot;
            public IdeologicalFrictionDayOwner(Main m) => _m = m;

            public void CapturePreDaySnapshot(int day)
            {
                _m.SetupIdeologicalFriction();
                _snapshot = _m._ideologicalFriction?.CaptureState();
            }

            public void RestorePreDaySnapshot(int day)
            {
                if (_snapshot != null) _m._ideologicalFriction?.RestoreState(_snapshot);
            }

            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupIdeologicalFriction();
                _m.TickIdeologicalFriction(day);
                var census = _m._ideologicalFriction?.Census;
                events.Add(new DayStateChangeEvent(
                    "ideological_friction_ticked", "ideological_friction", null, null, census?.TotalEventsFired ?? 0));
            }
        }

        /// <summary>Plan 150 romance &amp; family day owner (ownerId <c>romance_family</c>, phase 5).</summary>
        private sealed class RomanceFamilyDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            private string? _snapshot;
            public RomanceFamilyDayOwner(Main m) => _m = m;

            public void CapturePreDaySnapshot(int day)
            {
                _m.SetupRomanceFamily();
                _snapshot = _m._romanceFamily?.CaptureCoreState();
            }

            public void RestorePreDaySnapshot(int day)
            {
                if (_snapshot != null) _m._romanceFamily?.RestoreCoreState(_snapshot);
            }

            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupRomanceFamily();
                _m.TickRomanceFamily(day);
                var census = _m._romanceFamily?.Census;
                events.Add(new DayStateChangeEvent(
                    "romance_family_ticked", "romance_family", null, null, census?.TotalRelationships ?? 0));
            }
        }

        /// <summary>Plan 152 vehicle customization day owner (ownerId <c>vehicle_customization</c>, phase 5).</summary>
        private sealed class VehicleCustomizationDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            private string? _snapshot;
            public VehicleCustomizationDayOwner(Main m) => _m = m;

            public void CapturePreDaySnapshot(int day)
            {
                _m.SetupVehicleCustomization();
                _snapshot = _m._vehicleCustomization?.CaptureCoreState();
            }

            public void RestorePreDaySnapshot(int day)
            {
                if (_snapshot != null) _m._vehicleCustomization?.RestoreCoreState(_snapshot);
            }

            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupVehicleCustomization();
                _m.TickVehicleCustomization(day);
                var census = _m._vehicleCustomization?.Census;
                events.Add(new DayStateChangeEvent(
                    "vehicle_customization_ticked", "vehicle_customization", null, null, census?.TotalInstalledModules ?? 0));
            }
        }

        /// <summary>Plan 174 procedural survivor backstory day owner (ownerId <c>backstory</c>, phase 5).</summary>
        private sealed class BackstoryDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            private Ashfall.Core.Survivors.BackstoryState? _snapshot;
            public BackstoryDayOwner(Main m) => _m = m;

            public void CapturePreDaySnapshot(int day)
            {
                _m.SetupBackstory();
                _snapshot = _m._backstory?.CaptureState();
            }

            public void RestorePreDaySnapshot(int day)
            {
                if (_snapshot != null) _m._backstory?.RestoreState(_snapshot);
            }

            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupBackstory();
                _m.TickBackstory(day);
                var census = _m._backstory?.Census;
                events.Add(new DayStateChangeEvent(
                    "backstory_ticked", "backstory", null, null, census?.TotalBackstories ?? 0));
            }
        }

        /// <summary>Plan 175 meta progression day owner (ownerId <c>meta_progression</c>, phase 5).</summary>
        private sealed class MetaProgressionDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            private Ashfall.Core.Endgame.MetaProgressionSaveState? _snapshot;
            public MetaProgressionDayOwner(Main m) => _m = m;

            public void CapturePreDaySnapshot(int day)
            {
                _m.SetupMetaProgression();
                _snapshot = _m._metaProgression?.CaptureState();
            }

            public void RestorePreDaySnapshot(int day)
            {
                if (_snapshot != null) _m._metaProgression?.RestoreState(_snapshot);
            }

            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupMetaProgression();
                _m.TickMetaProgression(day);
                var census = _m._metaProgression?.Census;
                events.Add(new DayStateChangeEvent(
                    "meta_progression_ticked", "meta_progression", null, null, census?.PrestigeScore ?? 0));
            }
        }



        /// <summary>Plan 167 underground tunnel network day owner (ownerId <c>tunnel_network</c>, phase 5).</summary>
        private sealed class TunnelNetworkDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            private Ashfall.Core.Underground.TunnelNetworkState? _snapshot;
            public TunnelNetworkDayOwner(Main m) => _m = m;

            public void CapturePreDaySnapshot(int day)
            {
                _m.SetupTunnelNetwork();
                _snapshot = _m.TunnelNetwork?.CaptureState();
            }

            public void RestorePreDaySnapshot(int day)
            {
                if (_snapshot != null) _m.TunnelNetwork?.RestoreState(_snapshot);
            }

            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupTunnelNetwork();
                _m.TickTunnelNetwork(day);
                var census = _m.TunnelNetwork?.GetCensus();
                events.Add(new DayStateChangeEvent(
                    "tunnel_network_ticked", "tunnel_network", null, null, census?.TotalSegments ?? 0));
            }
        }

        /// <summary>Plan 166 shelter identity day owner (ownerId <c>shelter_identity</c>, phase 5).</summary>
        private sealed class ShelterIdentityDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            private Ashfall.Core.Shelter.ShelterIdentityState? _snapshot;
            public ShelterIdentityDayOwner(Main m) => _m = m;

            public void CapturePreDaySnapshot(int day)
            {
                _m.SetupShelterIdentity();
                _snapshot = _m.ShelterIdentity?.CaptureState();
            }

            public void RestorePreDaySnapshot(int day)
            {
                if (_snapshot != null) _m.ShelterIdentity?.RestoreState(_snapshot);
            }

            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupShelterIdentity();
                _m.TickShelterIdentity(day);
                var census = _m.ShelterIdentity?.Census;
                events.Add(new DayStateChangeEvent(
                    "shelter_identity_ticked", "shelter_identity", null, null, census?.Infamy ?? 0));
            }
        }

        /// <summary>Plan 192 scheduled trade route contract day owner (ownerId <c>trade_routes</c>, phase 5).</summary>
        private sealed class TradeRouteDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            private Ashfall.Core.Economy.PlayerTradeRouteSaveState? _snapshot;
            public TradeRouteDayOwner(Main m) => _m = m;

            public void CapturePreDaySnapshot(int day)
            {
                _m.SetupTradeRoutes();
                _snapshot = _m._tradeRoutes?.CaptureState();
            }

            public void RestorePreDaySnapshot(int day)
            {
                if (_snapshot != null) _m._tradeRoutes?.RestoreState(_snapshot);
            }

            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupTradeRoutes();
                _m.TickTradeRoutes(day);
                var census = _m._tradeRoutes?.Census;
                events.Add(new DayStateChangeEvent(
                    "trade_route_ticked", "trade_routes", null, null, census?.ActiveContracts ?? 0));
            }
        }

        /// <summary>Plan 199 seasonal human migration day owner (ownerId <c>human_migration</c>, phase 5).</summary>
        private sealed class HumanMigrationDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            private Ashfall.Core.Economy.SeasonalMigrationSaveState? _snapshot;
            public HumanMigrationDayOwner(Main m) => _m = m;

            public void CapturePreDaySnapshot(int day)
            {
                _m.SetupHumanMigration();
                _snapshot = _m._humanMigration?.CaptureState();
            }

            public void RestorePreDaySnapshot(int day)
            {
                if (_snapshot != null) _m._humanMigration?.RestoreState(_snapshot);
            }

            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupHumanMigration();
                _m.TickHumanMigration(day);
                var census = _m._humanMigration?.Census;
                events.Add(new DayStateChangeEvent(
                    "human_migration_ticked", "human_migration", null, null, census?.TotalTrackedRegions ?? 0));
            }
        }

        /// <summary>Plan 159 shelter governance day owner (ownerId <c>shelter_governance</c>, phase 5).</summary>
        private sealed class ShelterGovernanceDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            private Ashfall.Core.Governance.ShelterGovernanceSaveState? _snapshot;
            public ShelterGovernanceDayOwner(Main m) => _m = m;

            public void CapturePreDaySnapshot(int day)
            {
                _m.SetupShelterGovernance();
                _snapshot = _m._shelterGovernance?.CaptureState();
            }

            public void RestorePreDaySnapshot(int day)
            {
                if (_snapshot != null) _m._shelterGovernance?.RestoreState(_snapshot);
            }

            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupShelterGovernance();
                _m.TickShelterGovernance(day);
                int stability = _m._shelterGovernance?.StabilityRating ?? 100;
                events.Add(new DayStateChangeEvent(
                    "shelter_governance_ticked", "shelter_governance", null, null, stability));
            }
        }

        // ── Phase 1 Owners ───────────────────────────────────────────────

        private sealed class WeatherWorldDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            private Ashfall.Core.World.WorldWeatherState? _snapshot;
            public WeatherWorldDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day)
            {
                _m.SetupWorld();
                _snapshot = _m._world.Weather.CaptureState();
            }
            public void RestorePreDaySnapshot(int day)
            {
                if (_snapshot != null)
                    _m._world.Weather.RestoreState(_snapshot);
            }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupWorld();
                // C2 / Plan 20C (§39) — capture the station's prediction for
                // TODAY before the weather advances, so a hazard arrival can be
                // attributed: predicted / missed / unexpected. The radio layer
                // has no authored weather predictions yet — the radio/station
                // distinction is documented as not-yet-authored data.
                var stationState = _m._world.WeatherIntelligence?.Station?.State;
                var predictedToday = stationState?.cachedForecast?
                    .FirstOrDefault(f => f != null && f.day == day)?.weather;
                bool stationCouldKnow = stationState != null
                    && stationState.isInstalled
                    && stationState.lastForecastDay >= 0
                    && day <= stationState.lastForecastDay + stationState.forecastHorizonDays;

                _m._world.TickHours(24f);
                _m._world.WeatherIntelligence?.TickDay(day);

                var actual = _m._world.Weather.Current;
                events.Add(new DayStateChangeEvent("weather_ticked", "weather_world",
                    actual.ToString(), null, _m._world.Weather.OutdoorRadModifier));

                // Severe-weather arrivals get attribution (§39): the briefing
                // can say why the player wasn't warned — never a silent storm.
                if (_m._world.IsSevereWeather(actual) && actual != predictedToday)
                {
                    events.Add(stationCouldKnow
                        ? new DayStateChangeEvent("weather_forecast_miss", "weather_world",
                            actual.ToString(), "station_predicted_other", day)
                        : new DayStateChangeEvent("weather_unexpected_storm", "weather_world",
                            actual.ToString(), "no_station_forecast", day));
                }
            }
        }

        private sealed class DeepCoastMaritimeDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public DeepCoastMaritimeDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupMaritime();
                if (_m._maritime.Dive.IsActive)
                    _m._maritime.TickDive(60f);
                _m.SetupDeepCoast();
                _m._deepCoast.TickDaily(day, _m._core.Weather);
                _m._deepCoastPanel?.SetSimDay(day);
                events.Add(new DayStateChangeEvent("maritime_ticked", "maritime_deep_coast", null, null, day));
            }
        }

        private sealed class PowerGridDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public PowerGridDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.TickPowerGrid(day);

                // SHELTER_HARDENING: the distribution subgrid observes the grid's
                // per-room draws, then advances its thermal/fuse model. Runs in
                // the same phase as the grid so downstream consumers (phase 2+)
                // see post-surge, post-thermal state.
                _m.TickPowerSubgrids(day);

                events.Add(new DayStateChangeEvent("power_ticked", "power_grid", null, null, day));

                // C2[6] 23B: attributed shedding. The grid reports exactly what the
                // deterministic allocator served/shed; the briefing distinguishes
                // grid-automatic shed from the player's own breaker actions.
                var summary = _m._powerGrid?.LastTickSummary;
                if (summary != null)
                {
                    if (summary.HasCriticalDeficit)
                        events.Add(new DayStateChangeEvent("power_critical_deficit", "power_grid",
                            "life_support", null, summary.UnservedWatts));
                    if (summary.ShedRoomIds != null && summary.ShedRoomIds.Count > 0)
                        events.Add(new DayStateChangeEvent("power_shed_automatic", "power_grid",
                            string.Join(",", summary.ShedRoomIds), null, summary.UnservedWatts));
                    if (summary.BrownoutBegan)
                        events.Add(new DayStateChangeEvent("power_brownout_began", "power_grid", null, null, day));
                    if (summary.BrownoutEnded)
                        events.Add(new DayStateChangeEvent("power_brownout_restored", "power_grid", null, null, day));
                }
                if (_m._pendingPlayerSheds.Count > 0)
                {
                    events.Add(new DayStateChangeEvent("power_shed_player", "power_grid",
                        string.Join(",", _m._pendingPlayerSheds), null, _m._pendingPlayerSheds.Count));
                    _m._pendingPlayerSheds.Clear();
                }

                // C2[6] 23C: fact-reading cascade authority runs after the grid
                // resolves, so every stressor reflects the day's real outcome.
                _m.TickCascade(day, events);
            }
        }

        private sealed class NuclearCoreDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public NuclearCoreDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                var nuclear = _m.EnsureNuclearCore();
                _m.PublishNuclearCoreGeneration();
                events.Add(new DayStateChangeEvent(
                    "nuclear_generation_published",
                    "nuclear_core",
                    null,
                    null,
                    nuclear.GetTotalGenerationWatts()));
            }
        }

        private sealed class HoldfastCoreDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            private int _clockDaySnapshot;
            public HoldfastCoreDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day)
            {
                // The clock is the double-day hazard: if a later phase fails,
                // a retry must not tick the calendar twice.
                _clockDaySnapshot = _m._core.Clock.Day;
            }
            public void RestorePreDaySnapshot(int day)
            {
                _m._core.Clock.SetDay(_clockDaySnapshot);
            }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupIceRoad();
                string delta = _m._core.TickDay();
                if (_m._holdfastRuntime != null && !_m._holdfastRuntime.IsDead)
                {
                    _m._holdfastRuntime.Survivors = _m._survivors;
                    _m._holdfastRuntime.TickDay();
                }
                // Calendar-led authority: the clock is a projection and must
                // land exactly on the campaign day being committed, whatever
                // its internal tick state said.
                _m._core.Clock.SetDay(day);
                events.Add(new DayStateChangeEvent("holdfast_ticked", "holdfast_core", delta, null, _m._core.Clock.Day));
            }
        }

        // ── Phase 2 Owners ───────────────────────────────────────────────

        private sealed class StartingLevelRationsDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public StartingLevelRationsDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupStartingLevel();
                // SHELTER_FAILURE_EFFECTS (G6): fx_filtration_off — air filtration
                // follows the canonical room_air_filtration breaker.
                float airPower = _m._powerGrid?.System == null || _m._powerGrid.System.IsRoomPowered("room_air_filtration") ? 1f : 0f;
                _m._startingLevel.TickDay(isFilterDutyAssigned: false, outdoorWeather: WeatherKind.Clear, powerAvailability01: airPower);

                _m.SetupInventory();
                _m.SetupDoseLedger();
                int baseFood = _m._startingLevel.System.State.rationPolicy == Ashfall.Core.StartingLevel.RationPolicy.Half ? 2 : 3;
                int childFood = _m._doseLedger?.Cohort?.CalculateChildFoodUnits(_m._startingLevel.System.State.rationPolicy) ?? 0;
                int foodToConsume = baseFood + childFood;
                int waterToConsume = _m._startingLevel.System.State.rationPolicy == Ashfall.Core.StartingLevel.RationPolicy.Irradiated ? 0 : (_m._startingLevel.System.State.rationPolicy == Ashfall.Core.StartingLevel.RationPolicy.Half ? 2 : 3);
                _m._inventory.Remove("canned_food", foodToConsume);
                if (waterToConsume > 0)
                    _m._inventory.Remove("clean_water", waterToConsume);
                else
                    _m._inventory.Remove("irradiated_water", 2);

                events.Add(new DayStateChangeEvent("consumed_rations", "starting_level_rations", "canned_food", null, foodToConsume));
                if (childFood > 0)
                {
                    events.Add(new DayStateChangeEvent("consumed_child_rations", "starting_level_rations", "canned_food", null, childFood));
                }
            }
        }

        private sealed class CraftingProductionDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public CraftingProductionDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupCrafting();
                _m._crafting.CompleteAll(24f);
                events.Add(new DayStateChangeEvent("crafting_completed", "crafting_production", null, null, 24f));
            }
        }

        private sealed class GreenhouseFoundryDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public GreenhouseFoundryDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                // Single growth authority: player GreenhouseHostSession (shared
                // into the expansion hub). Do not also TickGreenhouse on a twin.
                _m.SetupGreenhouse();
                _m.SetupExpansions();
                _m.SetupAgriculture();
                if (_m._agriculture != null)
                {
                    // Plan 162: advanced agriculture derives the grow-light and
                    // ash inputs from power + weather, then ticks the canonical
                    // greenhouse growth authority exactly once inside Core.
                    _m.TickAgricultureDay(day);
                }
                else
                {
                    _m._greenhouse.TickDay(day, growLightHours: 6f, ashContaminationRate: 0.04f);
                }

                _m.SetupSilentFoundry();
                _m._silentFoundry.TickDaily(day);
                _m._silentFoundryPanel?.RefreshView();
                if (_m._foundryDirty) _m.SaveExpansionHub();

                events.Add(new DayStateChangeEvent("greenhouse_foundry_ticked", "greenhouse_foundry", null, null, day));
            }
        }

        /// <summary>
        /// Plan B68 — geological pulse progression runs in phase 1, after
        /// weather/power: fault tension accumulates, slips route damage to
        /// thermal/excavation authorities, and severe quakes request a cryo
        /// vault breach (Scenario E) before the production owners tick.
        /// </summary>
        private sealed class SeismicGeologyDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public SeismicGeologyDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupSeismicDynamics();
                _m._seismicDynamics!.TickDay(day);
                if (_m._seismicDirty) _m.SaveSeismicDynamics();
                events.Add(new DayStateChangeEvent("seismic_geology_ticked", "seismic_dynamics", null, null, day));
            }
        }

        /// <summary>
        /// Plan B69 — cryo thermal/viability update runs in phase 2 after the
        /// foundry owner: grid brownout from the furnace reaches the vault the
        /// same day, so a brownout day degrades samples exactly once.
        /// </summary>
        private sealed class CryoVaultDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public CryoVaultDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupCryoVault();
                _m._cryoVault!.TickDay(day);
                if (_m._cryoVaultDirty) _m.SaveCryoVault();
                events.Add(new DayStateChangeEvent("cryo_vault_ticked", "cryo_vault", null, null, day));
            }
        }

        /// <summary>
        /// Plan B89 — precision metrology daily drift and workshop Calibration
        /// projection. Runs in phase 2 after seismic so quake disturbance is
        /// applied first, then passive drift.
        /// </summary>
        private sealed class PrecisionMetrologyDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public PrecisionMetrologyDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.TickPrecisionMetrology(day);
                events.Add(new DayStateChangeEvent("precision_metrology_ticked", "precision_metrology", null, null, day));
            }
        }

        /// <summary>
        /// Plan B87 — closed-loop aquaponics daily ecology tick. Phase 2 so
        /// power/thermal query results for the day are already settled.
        /// </summary>
        private sealed class AquaponicsDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public AquaponicsDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.TickAquaponics(day);
                events.Add(new DayStateChangeEvent("aquaponics_ticked", "aquaponics", null, null, day));
            }
        }

        private sealed class PsychologyArcsDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public PsychologyArcsDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.TickPsychologyArcsDay(day);
                events.Add(new DayStateChangeEvent("psychology_arcs_ticked", "psychology_arcs_162", null, null, day));
            }
        }

        private sealed class EconomyMarketDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            private Ashfall.Core.Economy.MarketState? _snapshot;
            public EconomyMarketDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day)
            {
                _m.SetupEconomy();
                _snapshot = _m._economy.CaptureSave();
            }
            public void RestorePreDaySnapshot(int day)
            {
                if (_snapshot != null)
                    _m._economy.RestoreSave(_snapshot);
            }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupEconomy();
                // Plan 212 — weather (phase 1) already ticked; convert today's
                // severity into a bounded market shock BEFORE the index update.
                _m.TickEconomyWeatherBridge(day);
                _m._economy.TickDay(day, _m._campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Economy, day, 0));
                var activeShockCount = _m._economy.Market.ActiveShocks.Count;
                events.Add(new DayStateChangeEvent("market_ticked", "economy_market", null, null, _m._economy.Market.Day));
                if (activeShockCount > 0)
                    events.Add(new DayStateChangeEvent("market_shocks_active", "economy_market", null, null, activeShockCount));
            }
        }

        private sealed class ShelterFacilitiesDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public ShelterFacilitiesDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                // C2 / Plan 20B (§29) — capture pre-tick shelter shielding state
                // so degradation/unseal transitions become semantic day events
                // (canonical vocabulary; the briefing builder renders them).
                string filterBandBefore = _m._startingLevel?.System.AirFilterConditionBand ?? "healthy";
                bool deconActiveBefore = _m._decontamination?.System.HasActiveCase ?? false;
                AirlockDoorState airlockBefore = _m._airlockSecurity?.System.State.doorState
                    ?? AirlockDoorState.Secure;

                _m.TickAllExpandedShelterSystems(day);
                _m._kitchenNutrition?.DrainDayEvents(events);

                string filterBandAfter = _m._startingLevel?.System.AirFilterConditionBand ?? "healthy";
                if (FilterBandRank(filterBandAfter) < FilterBandRank(filterBandBefore))
                {
                    events.Add(new DayStateChangeEvent("shelter_filter_degraded",
                        "starting_level_air_filter", "air_filter", filterBandAfter,
                        _m._startingLevel?.System.State.airFilterHealthPercent ?? 0f));
                }

                bool deconActiveAfter = _m._decontamination?.System.HasActiveCase ?? false;
                if (!deconActiveBefore && deconActiveAfter)
                    events.Add(new DayStateChangeEvent("shelter_decon_started", "decontamination"));
                else if (deconActiveBefore && !deconActiveAfter)
                    events.Add(new DayStateChangeEvent("shelter_decon_completed", "decontamination"));

                AirlockDoorState airlockAfter = _m._airlockSecurity?.System.State.doorState
                    ?? AirlockDoorState.Secure;
                if (airlockBefore == AirlockDoorState.Secure && airlockAfter != AirlockDoorState.Secure)
                {
                    events.Add(new DayStateChangeEvent("shelter_hatch_unsealed",
                        "airlock_security", "airlock", airlockAfter.ToString()));
                }

                events.Add(new DayStateChangeEvent("shelter_facilities_ticked", "shelter_facilities", null, null, day));
            }

            /// <summary>Ordering for filter condition bands (higher = healthier).</summary>
            private static int FilterBandRank(string? band) => band switch
            {
                "healthy" => 2,
                "degraded" => 1,
                "critical" => 0,
                _ => 2
            };
        }

        /// <summary>
        /// Plan 38 — one daily owner for the commitment authority. Forwards the
        /// pre-day snapshot (fail-closed rollback), the day tick, and the
        /// buffered semantic events into the briefing report. Null-guarded so the
        /// owner survives session resets without outliving a disposed system.
        /// </summary>
        private sealed class CommitmentDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            public CommitmentDayOwner(Main m) => _m = m;

            public void CapturePreDaySnapshot(int day) => _m._commitments?.System.CapturePreDaySnapshot(day);

            public void RestorePreDaySnapshot(int day) => _m._commitments?.System.RestorePreDaySnapshot(day);

            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                var session = _m.EnsureCommitments();
                session.TickDay(day);
                session.DrainDayEvents(events);
                _m._commitmentsDirty = true;
            }
        }

        private sealed class ShelterFireDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            private Dictionary<string, Ashfall.Core.Shelter.FireIncidentState>? _snapshot;

            public ShelterFireDayOwner(Main m) => _m = m;

            public void CapturePreDaySnapshot(int day)
            {
                _m.SetupShelterFireHazard();
                _snapshot = _m._shelterFireHazard?.CaptureState();
            }

            public void RestorePreDaySnapshot(int day)
            {
                if (_snapshot != null)
                {
                    _m.SetupShelterFireHazard();
                    _m._shelterFireHazard!.RestoreState(_snapshot);
                }
            }

            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupShelterFireHazard();
                int advanced = _m._shelterFireSession!.TickDay(
                    day,
                    _m._campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Shelter, day, 15));
                events.Add(new DayStateChangeEvent("shelter_fire_ticked", "shelter_fire", null, null, advanced));
            }
        }

        // ── Phase 3 Owners ───────────────────────────────────────────────

        private sealed class SurvivorsNeedsDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            private SurvivorsSaveState? _snapshot;
            public SurvivorsNeedsDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day)
            {
                _m.SetupSurvivors();
                _snapshot = _m._survivors.CaptureSave();
            }
            public void RestorePreDaySnapshot(int day)
            {
                if (_snapshot != null)
                    _m._survivors.RestoreSave(_snapshot);
            }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupSurvivors();
                _m._survivors.Needs.CurrentDay = day;
                _m.ApplyScheduleNeedsModifiers();
                _m._survivors.TickHour(24f);
                _m.VacateInvalidFitnessAssignments();
                // Plan 24B A2 + 24C A3: measured overwork routes data-authored
                // fatigue/morale rates through the shared needs seam; grief
                // decays through the same seam from the relationship ledger's
                // persisted facts. Both read the post-validation assignment
                // state so vacated shifts never keep a stale rate.
                _m.ApplyOverworkNeedsModifiers();
                _m.ApplyGriefNeedsModifiers();
                _m.SetupInventory();
                if (_m._inventory != null)
                {
                    _m._inventory.CurrentDay = day;
                    _m._inventory.DrainDayEvents(events);
                }
                _m.SetupShelterDecor();
                // Plan 71: room_common_mess_hall — communal comfort morale is a
                // shed-able low-priority load (level gate, applied once per day;
                // no per-tick penalty accumulation).
                bool messHallPowered = _m._powerGrid?.System?.IsRoomPowered("room_common_mess_hall") ?? true;
                int decorRecipients = messHallPowered ? (_m._shelterDecor?.ApplyDailyMorale(day) ?? 0) : 0;
                if (decorRecipients > 0)
                    events.Add(new DayStateChangeEvent("shelter_decor_morale", "shelter_decor", null, null, decorRecipients));
                // Flagship XI (Plan 154): contagion runs after needs + decor morale
                // so it reads the day's final morale; its deltas are part of today.
                _m.SetupMoraleContagion();
                if (_m._moraleContagion != null)
                {
                    _m._moraleContagion.EvaluateDay(day);
                    events.Add(new DayStateChangeEvent("morale_contagion_ticked", "morale_contagion", null, null,
                        _m._moraleContagion.System.State.survivors.Count));
                }
                // Drain any survivor_perished events from the death pipeline
                // into the briefing feed. Needs/radiation OnDied fires inside
                // TickHour — every death this day lands here exactly once.
                _m.SetupSurvivorFate();
                if (_m._survivorFate != null)
                    _m._survivorFate.DrainDayEvents(events);
                _m.SetupDutyRoster();
                _m._dutyRoster!.DrainDayEvents(events);
                _m._medicalWardSession?.DrainDayEvents(events);
                events.Add(new DayStateChangeEvent("survivors_ticked", "survivors_needs", null, null, _m._survivors.RosterState.Count));
            }
        }

        /// <summary>
        /// Plan 210 — sanitation day owner (ownerId `hygiene`, phase 3).
        /// Feeds the living population into the sanitation authority and
        /// persists its state. Facility powered/staffed flags ride their
        /// persisted values until the Wave 6 power-grid feed lands.
        /// </summary>
        private sealed class HygieneDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            private Ashfall.Core.Shelter.SanitationState? _snapshot;
            public HygieneDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day)
            {
                _m.SetupSanitation();
                _snapshot = _m._sanitation!.CaptureSave();
            }
            public void RestorePreDaySnapshot(int day)
            {
                if (_snapshot != null)
                    _m._sanitation!.RestoreSave(_snapshot);
            }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupSanitation();
                int population = 0;
                if (_m._survivors != null)
                {
                    foreach (var entry in _m._survivors.Roster.Roster)
                        if (entry != null && entry.isAlive) population++;
                }
                _m._sanitation!.TickDay(day, population);

                TickSanitationConsequences(day, events);

                if (_m._sanitationDirty) _m.SaveSanitation();
                var spill = _m._sanitation.System.ActiveSpill;
                events.Add(new DayStateChangeEvent("sanitation_ticked", "hygiene", null, null,
                    _m._sanitation.System.GetShelterHygienePermille()));
                if (spill != null)
                    events.Add(new DayStateChangeEvent("sanitation_spill", "hygiene", spill.roomId, null, spill.severity));
            }

            /// <summary>
            /// Plan 210 Wave 6 — the three cross-plan consequences (Core
            /// policy, host adapter): a bounded daily cholera sweep through
            /// the AUTHORED foul_water_draw source (disease system keeps
            /// infection ownership), a reversible Hazardous morale mark, and
            /// a small bounded medical demand shock on crisis.
            /// </summary>
            private void TickSanitationConsequences(int day, List<DayStateChangeEvent> events)
            {
                var system = _m._sanitation!.System;
                var band = system.GetShelterHygieneBand();
                bool spillActive = system.ActiveSpill != null;

                // 1. Disease sweep — DiseaseSystem rolls and owns the outcome.
                if (_m._disease?.Engine != null
                    && Ashfall.Core.Shelter.SanitationConsequenceRules.ShouldRunDailyExposureSweep(band, spillActive))
                {
                    foreach (var entry in _m._survivors?.Roster.Roster ?? new List<Ashfall.Core.Survivors.SurvivorRosterEntry>())
                    {
                        if (entry == null || !entry.isAlive) continue;
                        string roomId = system.State.rooms.Count > 0 ? system.State.rooms[0].roomId : string.Empty;
                        _m._disease.Engine.TryExpose(new Ashfall.Core.Disease.DiseaseExposureContext
                        {
                            SurvivorId = entry.survivorId,
                            DiseaseId = Ashfall.Core.Disease.DiseaseIds.Cholera,
                            SourceId = Ashfall.Core.Shelter.SanitationConsequenceRules.CholeraSourceId,
                            Day = day,
                            ProbabilityModifier = system.GetPathogenExposureModifier(roomId)
                        });
                    }
                    events.Add(new DayStateChangeEvent("sanitation_disease_sweep", "hygiene", null, null, day));
                }

                // 2. Reversible morale mark (Hazardous only; cleared on recovery).
                if (_m._dutyRoster?.Marks != null)
                {
                    if (Ashfall.Core.Shelter.SanitationConsequenceRules.ShouldSetHazardousMark(band)
                        && !_m._dutyRoster.Marks.HasMark(Ashfall.Core.Shelter.SanitationConsequenceRules.HazardousMarkId))
                    {
                        _m._dutyRoster.Marks.SetMark(
                            Ashfall.Core.Shelter.SanitationConsequenceRules.HazardousMarkId,
                            "The shelter has become genuinely hazardous.", day);
                    }
                    else if (Ashfall.Core.Shelter.SanitationConsequenceRules.ShouldClearHazardousMark(band)
                        && _m._dutyRoster.Marks.HasMark(Ashfall.Core.Shelter.SanitationConsequenceRules.HazardousMarkId))
                    {
                        _m._dutyRoster.Marks.ClearMark(Ashfall.Core.Shelter.SanitationConsequenceRules.HazardousMarkId);
                    }
                }

                // 3. Crisis demand shock (idempotent refresh via source id).
                if (_m._economy != null
                    && Ashfall.Core.Shelter.SanitationConsequenceRules.ShouldApplyCrisisDemandShock(band, spillActive))
                {
                    _m._economy.Market.ApplyShock(
                        "medical", isShortage: true,
                        severityBp: Ashfall.Core.Shelter.SanitationConsequenceRules.CrisisShockSeverityBp,
                        startDay: day, durationDays: Ashfall.Core.Shelter.SanitationConsequenceRules.CrisisShockDurationDays,
                        sourceId: Ashfall.Core.Shelter.SanitationConsequenceRules.CrisisShockSourceId);
                }
            }
        }

        private sealed class MedicalDiseaseDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            private static readonly Ashfall.Core.SystemTextJsonSerializer s_json = new Ashfall.Core.SystemTextJsonSerializer();
            private Ashfall.Core.Medical.ChemicalDependencyLedgerState? _medicalSnapshot;
            private string? _diseaseSnapshotJson;
            private Ashfall.Core.Medical.MedicalPipelineSaveState? _pipelineSnapshot;
            public MedicalDiseaseDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day)
            {
                _m.SetupMedical();
                _medicalSnapshot = _m._medical.CaptureSave();
                _m.EnsureMedicalPipeline();
                _pipelineSnapshot = _m._medical.CapturePipelineSave();

                _m.SetupDisease();
                // CaptureState() already returns an independent clone; serialize
                // to JSON so RestorePreDaySnapshot can rebuild from the same
                // durable save format used by the hub envelope.
                _diseaseSnapshotJson = s_json.Serialize(_m._disease.Engine.CaptureState());
            }
            public void RestorePreDaySnapshot(int day)
            {
                if (_medicalSnapshot != null)
                    _m._medical.RestoreSave(_medicalSnapshot);
                if (_pipelineSnapshot != null && _m._medical.Pipeline != null)
                    _m._medical.Pipeline.RestoreState(_pipelineSnapshot);
                if (_diseaseSnapshotJson != null && _m._disease != null)
                {
                    var restored = s_json.Deserialize<Ashfall.Core.Disease.DiseaseSystemState>(_diseaseSnapshotJson);
                    if (restored != null)
                        _m._disease.Engine.RestoreState(restored);
                }
            }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupMedical();
                _m.EnsureMedicalPipeline();

                // Task #133 medical progression order (documented in the plan):
                // 1. scheduled procedures resolve (consume + apply at completion)
                // 2. chemical dependency progression (single tick owner)
                // 3. disease progression
                if (_m._medical.Pipeline != null)
                {
                    // SHELTER_EMP_MEDICAL_POWER: procedures advance only while the
                    // clinic has power — an outage freezes remaining hours (no
                    // reroll, no cost anomaly; costs consume at completion).
                    float clinicPower = _m._powerGrid?.System == null || _m._powerGrid.System.IsRoomPowered("room_clinic") ? 1f : 0f;
                    _m._medical.Pipeline.AdvanceScheduled(24f * clinicPower, day);
                }
                _m._medical.TickHours(24f);

                _m.SetupDisease();
                _m._disease.TickDaily(day);

                // Flagship XI (Plan 155): the strain layer runs immediately after
                // disease progression — mutations transition this day's outcomes,
                // cure research advances before triage reads the ward.
                _m.SetupPathogenStrains();
                if (_m._pathogenStrains != null)
                {
                    _m._pathogenStrains.TickMutations(day);
                    _m._pathogenStrains.AdvanceCureProjects(day);
                }

                // Plan 60 / D5 + D7 — bridge illness into the shared sick-list band
                // ladder and keep the memorial grief sink bound. Runs after the
                // disease tick so it reads this day's stage, and is idempotent.
                _m.SyncDiseaseTriage(day, events);
                _m.VacateInvalidFitnessAssignments();
                _m.ApplyOverworkNeedsModifiers();
                _m.ApplyGriefNeedsModifiers();
                _m._medicalWardSession?.DrainDayEvents(events);

                if (_m._expansionHubDirty) _m.SaveExpansionHub();

                events.Add(new DayStateChangeEvent("medical_disease_ticked", "medical_disease", null, null, day));
            }
        }

        private sealed class DutyRosterDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public DutyRosterDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupDutyRoster();
                _m._dutyRoster!.SyncDay(day);
                _m._dutyRoster!.TickDay(_m.BuildHomeOccupantSnapshot());
                _m._dutyRoster.DrainDayEvents(events);
                _m.SetupIceRoad();
                _m._dutyRoster.SyncHoldfastToDuty(_m._core.Census, _m._core.IceRoad, _m._expansions.Waystation, _m._core.Brine, day);
                _m._dutyRosterPanel?.RefreshView();
                if (_m._dutyRosterDirty) _m.SaveDutyRoster();

                events.Add(new DayStateChangeEvent("duty_roster_ticked", "duty_roster", null, null, day));
            }
        }

        private sealed class SurvivorSocialDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public SurvivorSocialDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.TickSurvivorSocial(day);
                events.Add(new DayStateChangeEvent("survivor_social_ticked", "survivor_social", null, null, day));
            }
        }

        private sealed class Phase0PsychologyDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            private Phase0EffectsSaveState? _snapshot;
            public Phase0PsychologyDayOwner(Main m) => _m = m;
            // Phase0EffectsSaveState is built fresh by CaptureSave (deep-copied
            // sub-states), so holding it directly is a true independent snapshot.
            public void CapturePreDaySnapshot(int day)
            {
                _m.SetupPhase0();
                _snapshot = _m._phase0.CaptureSave();
            }
            public void RestorePreDaySnapshot(int day)
            {
                if (_snapshot != null)
                    _m._phase0.RestoreSave(_snapshot);
            }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupPhase0();
                _m._phase0.CurrentDay = day;
                _m._phase0.IsInFalloutStorm = _m._world != null && _m._world.Weather.Current == Ashfall.Core.WeatherKind.FalloutStorm;
                _m._phase0.IsNightTime = day % 2 == 0;
                _m._phase0.TickDay(day);

                events.Add(new DayStateChangeEvent("phase0_ticked", "phase0_psychology", null, null, day));
            }
        }

        // ── Phase 4 Owners ───────────────────────────────────────────────

        private sealed class ExpeditionsCaravansDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            private ExpeditionAggregateState? _expeditionSnapshot;
            private TravelingCaravanState? _caravanSnapshot;
            private VehicleGarageState? _garageSnapshot;
            public ExpeditionsCaravansDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day)
            {
                _m.SetupExpeditions();
                _expeditionSnapshot = _m._expeditions.CaptureSaveAggregate();
                _m.SetupCaravans();
                _caravanSnapshot = _m._caravans.CaptureSave();
                // Plan 50 — recovery progress is part of the day's deterministic
                // state; a failed day restore must roll it back with the rest.
                _garageSnapshot = _m.EnsureVehicleGarage().CaptureState();
            }
            public void RestorePreDaySnapshot(int day)
            {
                if (_expeditionSnapshot != null)
                    _m._expeditions.RestoreSaveAggregate(_expeditionSnapshot);
                if (_caravanSnapshot != null)
                    _m._caravans.RestoreSave(_caravanSnapshot);
                if (_garageSnapshot != null)
                    _m.EnsureVehicleGarage().RestoreState(_garageSnapshot);
            }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupExpeditions();
                _m._expeditions.TickHours(24f);
                // Plan 50 — recovery teams work across campaign days. The garage
                // owns the mission ledger; this owner supplies the day progress
                // (the vehicle-garage section is captured by SaveOrchestrator).
                _m.EnsureVehicleGarage().AdvanceRecoveries(24);
                _m.SetupReconTelemetry();
                _m._reconTelemetry?.TickDay(day);

                _m.SetupDutyRoster();
                var expeditions = _m._expeditions.Engine.CaptureState();
                if (expeditions != null && _m._dutyRoster != null)
                {
                    for (int i = 0; i < expeditions.Count; i++)
                    {
                        var ex = expeditions[i];
                        if (ex == null) continue;
                        if (ex.phase == (int)ExpeditionPhase.Completed && !string.IsNullOrEmpty(ex.survivorId))
                        {
                            bool crisis = _m._dutyRoster.Quests.IsCrisisQuestActive();
                            _m._dutyRoster.BridgeHatchReturn(ex.survivorId, crisis: crisis);
                            // Flagship XI Slice 8: a completed sortie is a hope
                            // source; contagion spreads the relief naturally.
                            _m.SetupMoraleContagion();
                            _m._moraleContagion?.System.StartContagionEvent(
                                "contagion_successful_rescue_hope", string.Empty, day);
                            break;
                        }
                    }
                }

                _m.SetupCaravans();
                // Plan 14A — the authoritative weather (advanced in phase 1)
                // drives caravan embargo blocking/slowing for this movement day.
                _m._caravans.TickRoute(
                    _m._world != null && _m._world.Weather != null
                        ? _m._world.Weather.Current
                        : WeatherKind.Clear,
                    day,
                    _m._campaignDay?.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Economy, day, 3));
                _m.EnsureCaravanTrade();
                if (_m._caravanTradeNetwork != null)
                {
                    _m._caravanTradeNetwork.Map = _m._world?.WastelandMap;
                    _m._caravanTradeNetwork.TickDay(day);
                }

                events.Add(new DayStateChangeEvent("expeditions_caravans_ticked", "expeditions_caravans", null, null, day));
            }
        }

        /// <summary>
        /// Flagship XI (Plan 157) — psyops broadcast day: campaign resolution,
        /// loyalty-pressure routing to the faction authorities, intercept rolls.
        /// </summary>
        private sealed class PsyOpsDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public PsyOpsDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { /* no snapshot: campaign days are day-local */ }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupPsyOps();
                if (_m._psyops == null) return;

                int active = 0;
                var campaigns = _m._psyops.System.Campaigns;
                for (int i = 0; i < campaigns.Count; i++)
                    if (campaigns[i] != null && campaigns[i].status == (int)Ashfall.Core.Radio.PsyOpsCampaignStatus.Active)
                        active++;

                _m._psyops.System.TickCampaigns(day);
                events.Add(new DayStateChangeEvent("psyops_ticked", "psyops", null, null, active));
            }
        }

        /// <summary>
        /// Plan 173 Phase 2 — radio program production day: prep ticks and
        /// opportunistic delivery via existing schedule Resolve facts.
        /// </summary>
        private sealed class RadioProgramProductionDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public RadioProgramProductionDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { /* jobs are day-local; capture via save section */ }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                int before = _m._radioProgramProduction?.System.GetActiveJobs().Count ?? 0;
                _m.TickRadioProgramProduction(day);
                int after = _m._radioProgramProduction?.System.GetActiveJobs().Count ?? 0;
                events.Add(new DayStateChangeEvent("radio_program_production_ticked", "radio_program_production", null, null, after));
                if (before != after)
                    events.Add(new DayStateChangeEvent("radio_program_production_active_delta", "radio_program_production", null, null, after - before));
            }
        }

        /// <summary>
        /// Flagship XI (Plan 156) — underground day: oxygen, cave-in and flood
        /// hazards for every active underground sortie, claustrophobia morale
        /// through the needs authority, and forced retreats back through the
        /// expedition engine. Runs after expeditions (registration phase 4,
        /// ordinal after expeditions_caravans) and before world_evolution.
        /// </summary>
        /// <summary>
        /// Plan 211 — underworld market day owner (ownerId `underworld_market`,
        /// phase 4, after the debt-ledger tick). Refreshes discovered
        /// syndicates' stock snapshots from the black_market_stock RNG
        /// stream (fork-per-day, position-independent), then runs the debt
        /// due/overdue + heat tick. Emits underworld day events.
        /// </summary>
        private sealed class UnderworldMarketDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            private Ashfall.Core.Economy.BlackMarketState? _snapshot;
            public UnderworldMarketDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day)
            {
                _m.SetupBlackMarket();
                _snapshot = _m._blackMarket!.CaptureSave();
            }
            public void RestorePreDaySnapshot(int day)
            {
                if (_snapshot != null)
                    _m._blackMarket!.RestoreSave(_snapshot);
            }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupBlackMarket();
                var stockRng = _m._campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.BlackMarketStock, day, 0);
                _m._blackMarket!.TickDay(day, stockRng);
                if (_m._blackMarketDirty) _m.SaveBlackMarket();
                events.Add(new DayStateChangeEvent("underworld_ticked", "underworld_market", null, null,
                    _m._blackMarket.System.DiscoveredContacts.Count));
            }
        }

        private sealed class SubterraneanDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public SubterraneanDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { /* no snapshot: hazards are day-local */ }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupSubterranean();
                if (_m._subterranean == null) return;

                _m._subterranean.TickDay(day, requestRetreat: survivorId =>
                {
                    _m._expeditions?.Retreat(survivorId);
                });

                // Plan 49 flood bridge: rising subterranean water is the canonical
                // ingress for the excavation hazard sector of the same node.
                _m.ProjectSubterraneanFloodIntoExcavationHazards();

                int discovered = 0;
                var nodes = _m._subterranean.System.State.nodes;
                for (int i = 0; i < nodes.Count; i++)
                    if (nodes[i] != null && nodes[i].discovered) discovered++;

                events.Add(new DayStateChangeEvent("subterranean_ticked", "subterranean_network", null, null, discovered));
            }
        }

        /// <summary>
        /// Task 122 — the world changes because of time and player action:
        /// feeds live weather into landmark decay and location contamination,
        /// runs seeded wildlife migration, records expedition consequences on
        /// locations, bridges faction dominance into ownership, shifts market
        /// scarcity with wildlife pressure, and surfaces every major change
        /// through briefing events, journal lines, and radio intercepts.
        /// </summary>
        private sealed class EvolvingWorldDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            private LocationEvolutionSaveState? _locSnapshot;
            private WildlifeSaveState? _wildSnapshot;
            private LandmarkSaveState? _landSnapshot;
            private readonly HashSet<string> _processedExpeditions = new HashSet<string>();
            private string _lastDominantFaction = string.Empty;

            public EvolvingWorldDayOwner(Main m) => _m = m;

            public void CapturePreDaySnapshot(int day)
            {
                _m.SetupWorld();
                _locSnapshot = _m._world.LocationEvolution?.CaptureState();
                _wildSnapshot = _m._world.Wildlife?.CaptureState();
                _landSnapshot = _m._world.Landmarks?.CaptureState();
            }

            public void RestorePreDaySnapshot(int day)
            {
                if (_locSnapshot != null) _m._world.LocationEvolution?.RestoreState(_locSnapshot);
                if (_wildSnapshot != null) _m._world.Wildlife?.RestoreState(_wildSnapshot);
                if (_landSnapshot != null) _m._world.Landmarks?.RestoreState(_landSnapshot);
                _processedExpeditions.Clear();
            }

            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupWorld();
                var world = _m._world;

                var kind = world.Weather.Current;
                bool hazard = kind == WeatherKind.FalloutStorm || kind == WeatherKind.BlackRain;
                float ashfallMm = Main.AshfallMmFor(kind);

                // Pre-tick deltas we report on.
                var collapsedBefore = CollapsedSet(world);
                var sectorsBefore = SectorMap(world);
                var ownersBefore = OwnerMap(world);

                // ── The world moves ──
                world.Landmarks?.TickDay(day, ashfallMm);
                world.LocationEvolution?.TickDay(day,
                    new LocationEvolutionInputs(world.Weather.OutdoorRadModifier, hazard),
                    _m._campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.WorldEvolution, day, 0));
                world.Wildlife?.TickDay(day,
                    _m._campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.WorldEvolution, day, 1));

                // Plans 162-165 (Plan 165): the ecology layer ticks immediately
                // after the migration authority moved the packs — it reads and
                // mutates populations only through WildlifeMigrationSystem.
                _m.TickWildlifeEcosystemDay(day);

                // ── Landmark collapses → warning, journal ──
                if (world.Landmarks != null)
                {
                    foreach (var lm in world.Landmarks.State.landmarks)
                    {
                        if (lm == null || !lm.isCollapsed || lm.collapseDay != day) continue;
                        if (collapsedBefore.Contains(lm.landmarkId)) continue;
                        events.Add(new DayStateChangeEvent("hazard_warning", "world_evolution",
                            $"Landmark collapsed: {lm.landmarkId}", $"at {lm.locationId} (day {day})", lm.structuralIntegrity));
                        _m.SetupJournal();
                        _m._journal.TryAddRawEntry($"world_{lm.landmarkId}_collapse",
                            $"🔺 {lm.landmarkId} came down at {lm.locationId}. The skyline is poorer by one shape.",
                            null!, day);
                    }
                }

                // ── Pack migrations → radio intercepts ──
                if (world.Wildlife != null)
                {
                    int reported = 0;
                    var after = SectorMap(world);
                    foreach (var pack in world.Wildlife.State.packs)
                    {
                        if (pack == null || reported >= 3) continue;
                        if (sectorsBefore.TryGetValue(pack.packId, out var before)
                            && before != pack.currentSectorId)
                        {
                            // Plan 28: archetype-flavored coarse sighting; the
                            // generic move line stays for unremarkable species.
                            string notice = WildlifeSeasonalCalendar.MigrationNotice(
                                WildlifeSeasonalCalendar.ArchetypeOf(pack.speciesId),
                                pack.speciesId, before, pack.currentSectorId, day);
                            events.Add(new DayStateChangeEvent("radio_intercept", "world_evolution",
                                "wildlife net",
                                notice ?? $"{pack.packId} sighted moving {before} into {pack.currentSectorId}",
                                pack.population));
                            reported++;
                            // Plan 28 Phase 5: observation drives knowledge — a
                            // sighted species unlocks its field-guide teach
                            // entry (session knowledge; persistence = Plan 20A).
                            var teach = WildlifeSeasonalCalendar.FieldGuideEntryFor(pack.speciesId);
                            if (teach != null) _m.UnlockFieldGuideObservation(teach);
                        }
                        if (pack.isRabid && pack.lastThreatFiredDay == day)
                        {
                            events.Add(new DayStateChangeEvent("hazard_warning", "world_evolution",
                                $"Rabid {pack.speciesId}", $"{pack.packId} turned in {pack.currentSectorId}", pack.aggressionScore));
                        }
                    }
                }

                // ── Expedition consequences on locations ──
                _m.SetupExpeditions();
                var expeditions = _m._expeditions.Engine.CaptureState();
                if (expeditions != null)
                {
                    foreach (var ex in expeditions)
                    {
                        if (ex == null || string.IsNullOrEmpty(ex.expeditionId)) continue;
                        if (ex.phase != (int)ExpeditionPhase.Completed && ex.phase != (int)ExpeditionPhase.Failed) continue;
                        if (!_processedExpeditions.Add(ex.expeditionId)) continue;
                        if (string.IsNullOrEmpty(ex.locationId) || world.LocationEvolution == null) continue;

                        if (ex.phase == (int)ExpeditionPhase.Completed)
                        {
                            world.LocationEvolution.MarkCleared(ex.locationId, day);
                            events.Add(new DayStateChangeEvent("expedition_milestone", "world_evolution",
                                ex.locationId, "swept clean — salvage thins here for a while", 1));
                        }
                        else
                        {
                            world.LocationEvolution.MarkVisited(ex.locationId, day);
                            world.LocationEvolution.AddThreat(ex.locationId, LocationEvolutionSystem.ThreatSquatters);
                            events.Add(new DayStateChangeEvent("hazard_warning", "world_evolution",
                                ex.locationId, "sortie lost — stragglers now haunt the ground", 1));
                        }
                    }
                }

                // ── Faction dominance → location ownership ──
                _m.SetupYearOfAsh();
                string dominant = _m._yearOfAsh?.FactionWar?.DominantFactionId ?? string.Empty;
                if (!string.IsNullOrEmpty(dominant) && dominant != _lastDominantFaction)
                {
                    bool firstObservation = _lastDominantFaction.Length == 0;
                    _lastDominantFaction = dominant;
                    if (!firstObservation && world.Seeds?.location_seeds != null)
                    {
                        foreach (var seed in world.Seeds.location_seeds)
                        {
                            if (seed == null || seed.owner != dominant) continue;
                            var before = ownersBefore.TryGetValue(seed.location_id, out var o) ? o : null;
                            if (before == dominant) continue;
                            world.LocationEvolution?.SetLocationOwner(seed.location_id, dominant);
                            events.Add(new DayStateChangeEvent("hazard_warning", "world_evolution",
                                seed.location_id, $"control passes to {dominant}", 1));
                            _m.SetupJournal();
                            _m._journal.TryAddRawEntry($"world_{seed.location_id}_owner_{dominant}",
                                $"🔻 {seed.location_id} answer to {dominant} now. Flags change; the ground stays.",
                                null!, day);
                        }
                    }
                }

                // ── Wildlife pressure → market scarcity & trapping density ──
                _m.SetupEvolvingWorldInfluence();
                float ratio = world.Wildlife?.GetGlobalPopulationRatio() ?? 1f;
                var goods = EvolvingWorldSeeder.ScarcityGoods(world.Seeds);
                if (goods.Count > 0)
                {
                    float delta = ratio < 0.6f ? 0.02f : ratio < 0.85f ? 0.005f : ratio > 1.2f ? -0.005f : 0f;
                    if (Math.Abs(delta) > 0f)
                    {
                        _m.SetupEconomy();
                        // Plan 56 phase 5 — provenance-aware scarcity: goods with
                        // an active caravan supply line are buffered (0.5×);
                        // general goods track the market (1.0×); goods with no
                        // supply line escalate (1.5×). Active origin regions come
                        // from the caravan data authority.
                        var origins = new List<string>();
                        foreach (var cd in CaravanCatalogLoader.Load(_m._dataDir))
                            if (!string.IsNullOrEmpty(cd.origin_region) && !origins.Contains(cd.origin_region))
                                origins.Add(cd.origin_region);
                        foreach (var g in goods)
                        {
                            float scaled = delta * RegionalSupplyRouter.WorldShortageDemandScale(
                                _m._economy.Catalog, g, origins);
                            if (Math.Abs(scaled) > 0f)
                                _m._economy.Market.AdjustDemand(g, scaled);
                        }
                    }
                }

                // ── Plan 28 Phase 3: war-blocked corridors & collapse notice ──
                // Faction dominance projects onto the sector graph: sectors
                // holding dominant-faction ground close to wildlife movement
                // (stateless projection — the migration runtime never
                // persists blockage). Binding: seeds' location records carry
                // an optional sector_id.
                {
                    world.Wildlife?.ClearSectorBlockages();
                    if (!string.IsNullOrEmpty(dominant) && world.Seeds?.location_seeds != null)
                    {
                        foreach (var seed in world.Seeds.location_seeds)
                        {
                            if (seed == null || seed.owner != dominant) continue;
                            var sector = SectorOfLocation(world, seed.location_id);
                            if (!string.IsNullOrEmpty(sector)) world.Wildlife?.SetSectorBlocked(sector, true);
                        }
                    }
                }

                // ── Plan 28 Phase 4: ecological infestations ──
                _m.TickEcologicalInfestations(day, events);

                // ── Plans 46-49: Workshop, Radio, Social, Subterranean Hazards ──
                _m.TickPlans46_49(day, events);

                // ── Plans 198-201: CBRN Hazards, Comms Array, Ceremonies, Robotics ──
                _m.TickPlans198_201(day, events);

                // ── Plans 194-197: Naval & River, Item Degradation, Hobbies & Downtime, Winter Freeze ──
                _m.TickPlans194_197(day, events);

                // ── Plans 186-189: Radioactive Fallout, Desperation, Mercenary, Archaeology ──
                _m.TickPlans186_189(day, 24.0f);

                // ── Plan 176: anomaly hazard movement + approach warnings ──
                _m.TickAnomalyHazard(day);

                // ── Plan 174: companion care, bond/training, roles ──
                _m.TickCompanionDay(day);

                // ── Plan 177: bionics decay/power + anomaly disruption ──
                _m.TickBionicsDay(day);

                // ── Plan 175: ideological pressure, rituals, tension ──
                _m.TickZealotryDay(day);
                _m.TickSpiritualDay(day);

                // ── Plans 190-193: Infection & Amputation, Railways, Subterranean Fungi, Wasteland Justice ──
                _m.TickPlans190_193(day);

                // ── Plans 126-129: fermentation (drone/caster/lidar land in later waves) ──
                _m.TickPlans126_129(day);

                // ── Plan 147: contraband stash discovery rumors (pure reads + deduped journal) ──
                _m.TickContrabandStashDay(day);

                // ── Plan 147: shelter barter caravan schedule/arrivals (day-gated broker stock) ──
                _m.TickShelterBarterDay(day);

                // ── Plan 202: Plastic Pyrolysis (retort bay, grid-power projected) ──
                _m.TickPlasticPyrolysis(day);

                // ── Plan 205: airdrop descent/landing/interception + crate collection ──
                _m.TickCargoAirdrop(day);

                // ── Plan 203: perimeter weather wear + false alarms ──
                _m.TickPerimeterDefenseDaily(day);

                // ── Plans 178-181: Childhood Rearing, Prisoner Management, Mutation Trees, Stealth ──
                _m.TickPlans178_181(day);

                // ── Plans 182-185: Aviation, Forced Labor, Narcotics, Settlement Politics ──
                _m.TickPlans182_185(day);

                // ── Plan 28 Phase 3: collapse/scarcity notice (bounded) ──

                // ── Plan 28 Phase 3: collapse/scarcity notice (bounded) ──
                if (world.Wildlife != null && ratio <= 0.45f
                    && day - _lastCollapseNoticeDay >= CollapseNoticeCooldownDays)
                {
                    _lastCollapseNoticeDay = day;
                    events.Add(new DayStateChangeEvent("hazard_warning", "world_evolution",
                        "wildlife collapse",
                        "the land has gone quiet — snare lines and larders both", ratio));
                    _m.SetupJournal();
                    _m._journal.TryAddRawEntry($"world_wildlife_collapse_{day}",
                        "Something changed in the counts. The dogs range wider; the snares come back empty.",
                        null!, day);
                }

                events.Add(new DayStateChangeEvent("world_evolution_ticked", "world_evolution", null, null, day));
            }

            /// <summary>Collapse notices re-arm after this many days (anti-spam).</summary>
            private const int CollapseNoticeCooldownDays = 12;
            private int _lastCollapseNoticeDay = -30;

            /// <summary>
            /// Plan 28 Phase 3 — sector binding for war-blocked corridors: the
            /// seeds' location records carry an optional sector binding so the
            /// dominant faction's ground closes its representative sector to
            /// wildlife movement (stateless projection, never persisted).
            /// </summary>
            private static string? SectorOfLocation(WorldHostSession world, string locationId)
            {
                if (world.Seeds?.location_seeds == null) return null;
                foreach (var seed in world.Seeds.location_seeds)
                    if (seed != null && string.Equals(seed.location_id, locationId, StringComparison.Ordinal))
                        return string.IsNullOrEmpty(seed.sector_id) ? null : seed.sector_id;
                return null;
            }

            private static HashSet<string> CollapsedSet(WorldHostSession world)
            {
                var set = new HashSet<string>();
                foreach (var lm in world.Landmarks?.State.landmarks ?? new List<LandmarkStatusRecord>())
                    if (lm != null && lm.isCollapsed) set.Add(lm.landmarkId);
                return set;
            }

            private static Dictionary<string, string> SectorMap(WorldHostSession world)
            {
                var map = new Dictionary<string, string>();
                foreach (var p in world.Wildlife?.State.packs ?? new List<WildlifePackRecord>())
                    if (p != null) map[p.packId] = p.currentSectorId;
                return map;
            }

            private static Dictionary<string, string> OwnerMap(WorldHostSession world)
            {
                var map = new Dictionary<string, string>();
                foreach (var m in world.LocationEvolution?.State.mutations ?? new List<LocationMutationRecord>())
                    if (m != null) map[m.locationId] = m.currentOwner;
                return map;
            }
        }

        private sealed class NarrativeQuestsVerdictDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public NarrativeQuestsVerdictDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupMoralChoice();
                _m._moralChoice.Reconcile(day);
                _m.TickFactionBranchDay(day);
                _m.SetupCounterIntelligence();
                _m._counterIntelligence?.TickDay(day);

                _m.TickVerdict(day, _m.LivingDwellerCountEstimate());

                if (day >= 180)
                {
                    _m.SetupYearOfAsh();
                    _m._yearOfAsh.TickDay(day);
                }

                if (day >= 260)
                {
                    _m.SetupMuster();
                    _m._muster.Escalate(day);
                }

                _m.SetupExpansions();
                _m._expansions.TickCrossingQuests(day);

                _m.SetupExpansionQuests();
                _m._expansionQuests.TickDay(day);
                _m.SetupNpcArcs();

                // Plan 143: one deterministic daily draw from the shared
                // narrative stream. The selected event is persisted as a
                // pending modal; selecting it never applies consequences.
                _m.SetupNarrative();
                var arc = _m._narrative.SelectArcForDay(
                    day,
                    _m._campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Narrative, day, 0));
                if (arc != null)
                {
                    events.Add(new DayStateChangeEvent(
                        "narrative_arc_selected",
                        "narrative_quests_verdict",
                        arc.Id,
                        null,
                        day));
                }

                _m.SetupEchoes();
                var dueEchoConsequences = _m._echoes?.TickDay(day);
                if (dueEchoConsequences != null && dueEchoConsequences.Count > 0)
                {
                    events.Add(new DayStateChangeEvent(
                        "echo_consequence_due",
                        "narrative_quests_verdict",
                        dueEchoConsequences.Count.ToString(),
                        null,
                        day));
                }
                var echo = arc == null
                    ? _m._echoes?.SelectForDay(
                        day,
                        _m._campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Echo, day, 0))
                    : null;
                if (echo != null)
                {
                    events.Add(new DayStateChangeEvent(
                        "echo_surfaced",
                        "narrative_quests_verdict",
                        echo.Id,
                        null,
                        day));
                }

                events.Add(new DayStateChangeEvent("narrative_ticked", "narrative_quests_verdict", null, null, day));
            }
        }

        // ── Phase 5 Owners ───────────────────────────────────────────────

        private sealed class HostEventsDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public HostEventsDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupEventAdapter();
                bool hydroAudit = _m._muster?.HydroBarons?.AdminReform ?? false;
                bool hydroSeized = _m._muster?.HydroBarons?.PlantSeized ?? false;
                bool osteophageInquiry = (_m._yearOfAsh != null && _m._yearOfAsh.Timeline.CurrentDay >= 205) || day >= 205;
                bool coldCountBroadcast = _m._muster?.ColdCount?.BroadcastSent ?? false;
                _m._hostEventAdapter?.EvaluateTriggers(day, hydroAudit, hydroSeized, osteophageInquiry, coldCountBroadcast);

                events.Add(new DayStateChangeEvent("events_evaluated", "host_events", null, null, day));
            }
        }

        private sealed class MemorialDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public MemorialDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupMemorial();
                events.Add(new DayStateChangeEvent("memorial_checked", "memorial", null, null, day));
            }
        }

        /// <summary>Plan 29 Task 29A: once-daily room-history milestone pass.</summary>
        private sealed class ShelterRoomHistoryDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public ShelterRoomHistoryDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.TickShelterRoomHistoryMilestones(day);
            }
        }
    }
}
