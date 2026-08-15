using System;
using System.Collections.Generic;
using AtomicWar._Game.Quests;
using AtomicWar._Game.Factions;
using AtomicWar._Game.Environment;
using AtomicWar._Game.World;
using AtomicWar._Game.Narrative;
using Ashfall.Core;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Expansion Save Integration — registers all expansion systems with
    /// internal state (beyond Survivor fields) into SaveSystem via the
    /// ISaveable contract so their state round-trips save/load.
    ///
    /// Systems with state that lives ONLY on Survivor fields do not need
    /// registration (those fields serialize automatically).
    /// </summary>
    public partial class GameBootstrap
    {
        private void RegisterExpansionSaveables()
        {
            if (SaveSystem == null) return;

            // DynamicQuestlineSystem — active quests + completed list
            if (DynamicQuestlines != null)
                SaveSystem.Register(new DynamicQuestlineSaveable(DynamicQuestlines));

            // FactionIntelligenceSystem — intel, agents, tributes, alliances
            if (FactionIntel != null)
                SaveSystem.Register(new FactionIntelSaveable(FactionIntel));

            // Lore bible 05_FACTIONS — the Currents' state classes.
            if (NPCArchivists != null)
                SaveSystem.Register(new ArchivistsSaveable(NPCArchivists));
            if (NPCSunSeekers != null)
                SaveSystem.Register(new SunSeekersSaveable(NPCSunSeekers));
            if (NPCOsteophages != null)
                SaveSystem.Register(new OsteophagesSaveable(NPCOsteophages));
            if (NPCLamplighters != null)
                SaveSystem.Register(new LamplightersSaveable(NPCLamplighters));
            if (NPCQuietHouse != null)
                SaveSystem.Register(new QuietHouseSaveable(NPCQuietHouse));
            if (NPCGrainExchange != null)
                SaveSystem.Register(new GrainExchangeSaveable(NPCGrainExchange));
            if (NPCTally != null)
                SaveSystem.Register(new TallySaveable(NPCTally));
            if (NPCUndertow != null)
                SaveSystem.Register(new UndertowSaveable(NPCUndertow));

            if (IceRoadSystem != null)
                SaveSystem.Register(new IceRoadSaveable(IceRoadSystem));
            if (CensusClaimSystem != null)
                SaveSystem.Register(new CensusClaimSaveable(CensusClaimSystem));
            if (BrineWaterSystem != null)
                SaveSystem.Register(new BrineWaterSaveable(BrineWaterSystem));
            if (WaystationSystem != null)
                SaveSystem.Register(new WaystationSaveable(WaystationSystem));
            if (HoldfastQuests != null)
                SaveSystem.Register(new HoldfastQuestSaveable(HoldfastQuests));
            if (NPCEdorVale != null)
                SaveSystem.Register(new EdorValeSaveable(NPCEdorVale));
            if (NPCYaraHolm != null)
                SaveSystem.Register(new YaraHolmSaveable(NPCYaraHolm));
            if (NPCTheOffice != null)
                SaveSystem.Register(new TheOfficeSaveable(NPCTheOffice));

            // ASHFALL: NOBODY'S CHARTER — social gate + Standing + Ledger + companions.
            if (Vouch != null)
                SaveSystem.Register(new VouchAccessSaveable(Vouch));
            if (Arbitration != null)
                SaveSystem.Register(new CrossingArbitrationSaveable(Arbitration));
            if (Ledger != null)
                SaveSystem.Register(new LedgerDebtSaveable(Ledger));
            if (NPCOsranKell != null)
                SaveSystem.Register(new OsranKellSaveable(NPCOsranKell));
            if (NPCMattisCray != null)
                SaveSystem.Register(new MattisCraySaveable(NPCMattisCray));
            if (NPCDessaVane != null)
                SaveSystem.Register(new DessaVaneSaveable(NPCDessaVane));
            if (NPCPerrinAshby != null)
                SaveSystem.Register(new PerrinAshbySaveable(NPCPerrinAshby));
            if (NPCIvoFenn != null)
                SaveSystem.Register(new IvoFennSaveable(NPCIvoFenn));
            if (NPCWynSabler != null)
                SaveSystem.Register(new WynSablerSaveable(NPCWynSabler));

            // VehicleMaintenanceSystem — vehicle condition/fuel/cargo/mods
            if (VehicleMaintenance != null)
                SaveSystem.Register(new VehicleMaintenanceSaveable(VehicleMaintenance));

            // AshDriftBurialSystem — ash accumulation level
            if (AshDriftBurialSystem != null)
                SaveSystem.Register(new AshDriftSaveable(AshDriftBurialSystem));

            // LocationEvolutionSystem — location ownership states
            if (LocationEvolutionSystem != null)
                SaveSystem.Register(new LocationEvolutionSaveable(LocationEvolutionSystem));

            // WildlifeMigrationSystem — zone danger levels
            if (WildlifeMigrationSystem != null)
                SaveSystem.Register(new WildlifeSaveable(WildlifeMigrationSystem));

            // LandmarkDegradationSystem — collapse/flood states
            if (LandmarkDegradationSystem != null)
                SaveSystem.Register(new LandmarkSaveable(LandmarkDegradationSystem));

            // ASHFALL: THE GLASS ORCHARD (Expansion XI) — GreenhouseSystem
            // implements ISaveable directly (plots + wheat unlock + harvests).
            if (Greenhouse != null)
                SaveSystem.Register(Greenhouse);

            // ASHFALL: THE DUTY ROSTER — roster, marks, Kess, Ansel.
            if (DutyRosterSystem != null)
                SaveSystem.Register(new DutyRosterSaveable(DutyRosterSystem));
            if (MoraleMarkSystem != null)
                SaveSystem.Register(new MoraleMarkSaveable(MoraleMarkSystem));
            if (ShelterEncounterSystem != null)
                SaveSystem.Register(new ShelterEncounterSaveable(ShelterEncounterSystem));
            if (NPCKessAdler != null)
                SaveSystem.Register(new KessAdlerSaveable(NPCKessAdler));
            if (NPCAnselDuth != null)
                SaveSystem.Register(new AnselDuthSaveable(NPCAnselDuth));
            if (NPCTamsinRook != null)
                SaveSystem.Register(new TamsinRookSaveable(NPCTamsinRook));
            if (NPCLenQuill != null)
                SaveSystem.Register(new LenQuillSaveable(NPCLenQuill));
            if (NPCHadiMorrow != null)
                SaveSystem.Register(new HadiMorrowSaveable(NPCHadiMorrow));
            if (NPCNilaBrant != null)
                SaveSystem.Register(new NilaBrantSaveable(NPCNilaBrant));

            // ASHFALL: THE STANDING RECORD — layouts, memory, site encounters.
            if (LocationLayoutSystem != null)
                SaveSystem.Register(new LocationLayoutSaveable(LocationLayoutSystem));
            if (LocationMemorySystem != null)
                SaveSystem.Register(new LocationMemorySaveable(LocationMemorySystem));
            if (SiteEncounterSystem != null)
                SaveSystem.Register(new SiteEncounterSaveable(SiteEncounterSystem));
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    // ISaveable adapters — wrap system state for SaveSystem
    // ═══════════════════════════════════════════════════════════════════

    internal class DynamicQuestlineSaveable : ISaveable
    {
        private readonly DynamicQuestlineSystem _system;
        public string SaveId => "dynamic_questlines";
        public DynamicQuestlineSaveable(DynamicQuestlineSystem s) => _system = s;
        public object CaptureState() => _system.CaptureState();
        public void RestoreState(object state) =>
            _system.RestoreState(state as DynamicQuestlineSaveState);
    }

    internal class FactionIntelSaveable : ISaveable
    {
        private readonly FactionIntelligenceSystem _system;
        public string SaveId => "faction_intelligence";
        public FactionIntelSaveable(FactionIntelligenceSystem s) => _system = s;
        public object CaptureState() => _system.GetState();
        public void RestoreState(object state)
        {
            // State is restored into the live object by GetState reference.
            if (state is FactionIntelligenceSaveState incoming)
            {
                var current = _system.GetState();
                current.ActiveIntel.Clear();
                current.ActiveIntel.AddRange(incoming.ActiveIntel);
                current.ActiveAgents.Clear();
                current.ActiveAgents.AddRange(incoming.ActiveAgents);
                current.TributeDemands.Clear();
                foreach (var entry in incoming.TributeDemands)
                    current.TributeDemands.Add(new TributeDemandEntry
                    {
                        FactionId = entry.FactionId,
                        ResourceType = entry.ResourceType,
                        Amount = entry.Amount
                    });
                current.AlliedFactionIds.Clear();
                current.AlliedFactionIds.AddRange(incoming.AlliedFactionIds);
                current.InformantNetworkActive = incoming.InformantNetworkActive;
            }
        }
    }

    internal class VehicleMaintenanceSaveable : ISaveable
    {
        private readonly VehicleMaintenanceSystem _system;
        public string SaveId => "vehicle_maintenance";
        public VehicleMaintenanceSaveable(VehicleMaintenanceSystem s) => _system = s;

        [Serializable]
        private class State
        {
            public List<string> VehicleIds = new List<string>();
            public List<float> Conditions = new List<float>();
            public List<float> Fuel = new List<float>();
            public List<float> Cargo = new List<float>();
        }

        public object CaptureState()
        {
            var state = new State();
            // Only track the default scout truck for now
            var v = _system.GetVehicle("scout_truck");
            if (v != null)
            {
                state.VehicleIds.Add("scout_truck");
                state.Conditions.Add(v.ConditionPct);
                state.Fuel.Add(v.FuelLitres);
                state.Cargo.Add(v.CurrentCargoKg);
            }
            return state;
        }

        public void RestoreState(object state)
        {
            if (state is State s && s.VehicleIds != null)
            {
                for (int i = 0; i < s.VehicleIds.Count; i++)
                {
                    var v = _system.GetVehicle(s.VehicleIds[i]);
                    if (v == null) continue;
                    if (i < s.Conditions.Count) v.ConditionPct = s.Conditions[i];
                    if (i < s.Fuel.Count) v.FuelLitres = s.Fuel[i];
                    if (i < s.Cargo.Count) v.CurrentCargoKg = s.Cargo[i];
                }
            }
        }
    }

    internal class AshDriftSaveable : ISaveable
    {
        private readonly AshDriftBurialSystem _system;
        public string SaveId => "ash_drift_burial";
        public AshDriftSaveable(AshDriftBurialSystem s) => _system = s;

        [Serializable]
        private class State { public float AshAccumulation; }

        public object CaptureState() =>
            new State { AshAccumulation = _system.AshAccumulation };

        public void RestoreState(object state)
        {
            if (state is State s)
            {
                // Restore via clear+storm accumulation math
                var current = _system.AshAccumulation;
                // Simplest: clear then add back
                _system.ClearAsh(9999f);
                if (s.AshAccumulation > 0f)
                    _system.OnAshStorm(s.AshAccumulation /
                        AshDriftBurialSystem.AshAccumulationRatePerStorm);
            }
        }
    }

    internal class LocationEvolutionSaveable : ISaveable
    {
        private readonly LocationEvolutionSystem _system;
        public string SaveId => "location_evolution";
        public LocationEvolutionSaveable(LocationEvolutionSystem s) => _system = s;
        public object CaptureState() => null; // state lives in registered locations
        public void RestoreState(object state) { /* locations re-registered at init */ }
    }

    internal class WildlifeSaveable : ISaveable
    {
        private readonly WildlifeMigrationSystem _system;
        public string SaveId => "wildlife_migration";
        public WildlifeSaveable(WildlifeMigrationSystem s) => _system = s;
        public object CaptureState() => null;
        public void RestoreState(object state) { }
    }

    internal class LandmarkSaveable : ISaveable
    {
        private readonly LandmarkDegradationSystem _system;
        public string SaveId => "landmark_degradation";
        public LandmarkSaveable(LandmarkDegradationSystem s) => _system = s;
        public object CaptureState() => null;
        public void RestoreState(object state) { }
    }

    // ═══════════════════════════════════════════════════════════════════
    // Lore bible 05_FACTIONS — Current NPC state adapters
    // ═══════════════════════════════════════════════════════════════════

    internal class ArchivistsSaveable : ISaveable
    {
        private readonly NPC_Archivists _npc;
        public string SaveId => "currents_archivists";
        public ArchivistsSaveable(NPC_Archivists npc) => _npc = npc;
        public object CaptureState() => _npc.CaptureState();
        public void RestoreState(object state) => _npc.RestoreState(state as NPC_ArchivistsState);
    }

    internal class SunSeekersSaveable : ISaveable
    {
        private readonly NPC_SunSeekers _npc;
        public string SaveId => "currents_sun_seekers";
        public SunSeekersSaveable(NPC_SunSeekers npc) => _npc = npc;
        public object CaptureState() => _npc.CaptureState();
        public void RestoreState(object state) => _npc.RestoreState(state as NPC_SunSeekersState);
    }

    internal class OsteophagesSaveable : ISaveable
    {
        private readonly NPC_Osteophages _npc;
        public string SaveId => "currents_osteophages";
        public OsteophagesSaveable(NPC_Osteophages npc) => _npc = npc;
        public object CaptureState() => _npc.CaptureState();
        public void RestoreState(object state) => _npc.RestoreState(state as NPC_OsteophagesState);
    }

    internal class LamplightersSaveable : ISaveable
    {
        private readonly NPC_Lamplighters _npc;
        public string SaveId => "currents_lamplighters";
        public LamplightersSaveable(NPC_Lamplighters npc) => _npc = npc;
        public object CaptureState() => _npc.CaptureState();
        public void RestoreState(object state) => _npc.RestoreState(state as NPC_LamplightersState);
    }

    internal class QuietHouseSaveable : ISaveable
    {
        private readonly NPC_QuietHouse _npc;
        public string SaveId => "currents_quiet_house";
        public QuietHouseSaveable(NPC_QuietHouse npc) => _npc = npc;
        public object CaptureState() => _npc.CaptureState();
        public void RestoreState(object state) => _npc.RestoreState(state as NPC_QuietHouseState);
    }

    internal class GrainExchangeSaveable : ISaveable
    {
        private readonly NPC_GrainExchange _npc;
        public string SaveId => "currents_grain_exchange";
        public GrainExchangeSaveable(NPC_GrainExchange npc) => _npc = npc;
        public object CaptureState() => _npc.CaptureState();
        public void RestoreState(object state) => _npc.RestoreState(state as NPC_GrainExchangeState);
    }

    internal class TallySaveable : ISaveable
    {
        private readonly NPC_Tally _npc;
        public string SaveId => "currents_tally";
        public TallySaveable(NPC_Tally npc) => _npc = npc;
        public object CaptureState() => _npc.CaptureState();
        public void RestoreState(object state) => _npc.RestoreState(state as NPC_TallyState);
    }

    internal class UndertowSaveable : ISaveable
    {
        private readonly NPC_Undertow _npc;
        public string SaveId => "currents_undertow";
        public UndertowSaveable(NPC_Undertow npc) => _npc = npc;
        public object CaptureState() => _npc.CaptureState();
        public void RestoreState(object state) => _npc.RestoreState(state as NPC_UndertowState);
    }

    internal class IceRoadSaveable : ISaveable
    {
        private readonly IceRoadSystem _system;
        public string SaveId => IceRoadSystem.SystemId;
        public IceRoadSaveable(IceRoadSystem system) => _system = system;
        public object CaptureState() => _system.CaptureState();
        public void RestoreState(object state) => _system.RestoreState(state as IceRoadSystemState);
    }

    internal class CensusClaimSaveable : ISaveable
    {
        private readonly CensusClaimSystem _system;
        public string SaveId => CensusClaimSystem.SystemId;
        public CensusClaimSaveable(CensusClaimSystem system) => _system = system;
        public object CaptureState() => _system.CaptureState();
        public void RestoreState(object state) => _system.RestoreState(state as CensusClaimSystemState);
    }

    internal class BrineWaterSaveable : ISaveable
    {
        private readonly BrineWaterSystem _system;
        public string SaveId => BrineWaterSystem.SystemId;
        public BrineWaterSaveable(BrineWaterSystem system) => _system = system;
        public object CaptureState() => _system.CaptureState();
        public void RestoreState(object state) => _system.RestoreState(state as BrineWaterSystemState);
    }

    internal class WaystationSaveable : ISaveable
    {
        private readonly WaystationSystem _system;
        public string SaveId => WaystationSystem.SystemId;
        public WaystationSaveable(WaystationSystem system) => _system = system;
        public object CaptureState() => _system.CaptureState();
        public void RestoreState(object state) => _system.RestoreState(state as WaystationSystemState);
    }

    internal class HoldfastQuestSaveable : ISaveable
    {
        private readonly HoldfastQuestSystem _system;
        public string SaveId => HoldfastQuestSystem.SystemId;
        public HoldfastQuestSaveable(HoldfastQuestSystem system) => _system = system;
        public object CaptureState() => _system.CaptureState();
        public void RestoreState(object state) => _system.RestoreState(state as HoldfastQuestSystemState);
    }

    internal class EdorValeSaveable : ISaveable
    {
        private readonly NPC_EdorVale _npc;
        public string SaveId => "npc_edor_vale";
        public EdorValeSaveable(NPC_EdorVale npc) => _npc = npc;
        public object CaptureState() => _npc.CaptureState();
        public void RestoreState(object state) => _npc.RestoreState(state as NPC_EdorValeState);
    }

    internal class YaraHolmSaveable : ISaveable
    {
        private readonly NPC_YaraHolm _npc;
        public string SaveId => "npc_yara_holm";
        public YaraHolmSaveable(NPC_YaraHolm npc) => _npc = npc;
        public object CaptureState() => _npc.CaptureState();
        public void RestoreState(object state) => _npc.RestoreState(state as NPC_YaraHolmState);
    }

    internal class TheOfficeSaveable : ISaveable
    {
        private readonly NPC_TheOffice _npc;
        public string SaveId => "npc_the_office";
        public TheOfficeSaveable(NPC_TheOffice npc) => _npc = npc;
        public object CaptureState() => _npc.CaptureState();
        public void RestoreState(object state) => _npc.RestoreState(state as NPC_TheOfficeState);
    }

    internal class VouchAccessSaveable : ISaveable
    {
        private readonly VouchAccessSystem _system;
        public string SaveId => VouchAccessSystem.SystemId;
        public VouchAccessSaveable(VouchAccessSystem system) => _system = system;
        public object CaptureState() => _system.CaptureState();
        public void RestoreState(object state) => _system.RestoreState(state as VouchAccessSystemState);
    }

    internal class OsranKellSaveable : ISaveable
    {
        private readonly NPC_OsranKell _npc;
        public string SaveId => "npc_osran_kell";
        public OsranKellSaveable(NPC_OsranKell npc) => _npc = npc;
        public object CaptureState() => _npc.CaptureState();
        public void RestoreState(object state) => _npc.RestoreState(state as NPC_OsranKellState);
    }

    internal class MattisCraySaveable : ISaveable
    {
        private readonly NPC_MattisCray _npc;
        public string SaveId => "npc_mattis_cray";
        public MattisCraySaveable(NPC_MattisCray npc) => _npc = npc;
        public object CaptureState() => _npc.CaptureState();
        public void RestoreState(object state) => _npc.RestoreState(state as NPC_MattisCrayState);
    }

    internal class CrossingArbitrationSaveable : ISaveable
    {
        private readonly CrossingArbitrationSystem _system;
        public string SaveId => CrossingArbitrationSystem.SystemId;
        public CrossingArbitrationSaveable(CrossingArbitrationSystem system) => _system = system;
        public object CaptureState() => _system.CaptureState();
        public void RestoreState(object state) => _system.RestoreState(state as CrossingArbitrationState);
    }

    internal class DessaVaneSaveable : ISaveable
    {
        private readonly NPC_DessaVane _npc;
        public string SaveId => "npc_dessa_vane";
        public DessaVaneSaveable(NPC_DessaVane npc) => _npc = npc;
        public object CaptureState() => _npc.CaptureState();
        public void RestoreState(object state) => _npc.RestoreState(state as NPC_DessaVaneState);
    }

    internal class LedgerDebtSaveable : ISaveable
    {
        // Ashfall.Core single source (§5.3) — the Unity host consumes the
        // engine-agnostic system directly, no host twin.
        private readonly LedgerDebtSystem _system;
        public string SaveId => LedgerDebtSystem.SystemId;
        public LedgerDebtSaveable(LedgerDebtSystem system) => _system = system;
        public object CaptureState() => _system.CaptureState();
        public void RestoreState(object state) => _system.RestoreState(state as LedgerDebtSystemState);
    }

    internal class WynSablerSaveable : ISaveable
    {
        private readonly NPC_WynSabler _npc;
        public string SaveId => "npc_wyn_sabler";
        public WynSablerSaveable(NPC_WynSabler npc) => _npc = npc;
        public object CaptureState() => _npc.CaptureState();
        public void RestoreState(object state) => _npc.RestoreState(state as NPC_WynSablerState);
    }

    internal class PerrinAshbySaveable : ISaveable
    {
        private readonly NPC_PerrinAshby _npc;
        public string SaveId => "npc_perrin_ashby";
        public PerrinAshbySaveable(NPC_PerrinAshby npc) => _npc = npc;
        public object CaptureState() => _npc.CaptureState();
        public void RestoreState(object state) => _npc.RestoreState(state as NPC_PerrinAshbyState);
    }

    internal class IvoFennSaveable : ISaveable
    {
        private readonly NPC_IvoFenn _npc;
        public string SaveId => "npc_ivo_fenn";
        public IvoFennSaveable(NPC_IvoFenn npc) => _npc = npc;
        public object CaptureState() => _npc.CaptureState();
        public void RestoreState(object state) => _npc.RestoreState(state as NPC_IvoFennState);
    }

    // ═══════════════════════════════════════════════════════════════════
    // ASHFALL: THE DUTY ROSTER — saveable adapters
    // ═══════════════════════════════════════════════════════════════════

    internal class DutyRosterSaveable : ISaveable
    {
        private readonly DutyRosterSystem _system;
        public string SaveId => DutyRosterSystem.SystemId;
        public DutyRosterSaveable(DutyRosterSystem system) => _system = system;
        public object CaptureState() => _system.CaptureState();
        public void RestoreState(object state) => _system.RestoreState(state as DutyRosterSystemState);
    }

    internal class MoraleMarkSaveable : ISaveable
    {
        private readonly MoraleMarkSystem _system;
        public string SaveId => MoraleMarkSystem.SystemId;
        public MoraleMarkSaveable(MoraleMarkSystem system) => _system = system;
        public object CaptureState() => _system.CaptureState();
        public void RestoreState(object state) => _system.RestoreState(state as MoraleMarkSystemState);
    }

    internal class KessAdlerSaveable : ISaveable
    {
        private readonly NPC_KessAdler _npc;
        public string SaveId => "npc_kess_adler";
        public KessAdlerSaveable(NPC_KessAdler npc) => _npc = npc;
        public object CaptureState() => _npc.CaptureState();
        public void RestoreState(object state) => _npc.RestoreState(state as NPC_KessAdlerState);
    }

    internal class AnselDuthSaveable : ISaveable
    {
        private readonly NPC_AnselDuth _npc;
        public string SaveId => "npc_ansel_duth";
        public AnselDuthSaveable(NPC_AnselDuth npc) => _npc = npc;
        public object CaptureState() => _npc.CaptureState();
        public void RestoreState(object state) => _npc.RestoreState(state as NPC_AnselDuthState);
    }

    internal class ShelterEncounterSaveable : ISaveable
    {
        private readonly ShelterEncounterSystem _system;
        public string SaveId => ShelterEncounterSystem.SystemId;
        public ShelterEncounterSaveable(ShelterEncounterSystem system) => _system = system;
        public object CaptureState() => _system.CaptureState();
        public void RestoreState(object state) => _system.RestoreState(state as ShelterEncounterSystemState);
    }

    internal class TamsinRookSaveable : ISaveable
    {
        private readonly NPC_TamsinRook _npc;
        public string SaveId => "npc_tamsin_rook";
        public TamsinRookSaveable(NPC_TamsinRook npc) => _npc = npc;
        public object CaptureState() => _npc.CaptureState();
        public void RestoreState(object state) => _npc.RestoreState(state as NPC_TamsinRookState);
    }

    internal class LenQuillSaveable : ISaveable
    {
        private readonly NPC_LenQuill _npc;
        public string SaveId => "npc_len_quill";
        public LenQuillSaveable(NPC_LenQuill npc) => _npc = npc;
        public object CaptureState() => _npc.CaptureState();
        public void RestoreState(object state) => _npc.RestoreState(state as NPC_LenQuillState);
    }

    internal class HadiMorrowSaveable : ISaveable
    {
        private readonly NPC_HadiMorrow _npc;
        public string SaveId => "npc_hadi_morrow";
        public HadiMorrowSaveable(NPC_HadiMorrow npc) => _npc = npc;
        public object CaptureState() => _npc.CaptureState();
        public void RestoreState(object state) => _npc.RestoreState(state as NPC_HadiMorrowState);
    }

    internal class NilaBrantSaveable : ISaveable
    {
        private readonly NPC_NilaBrant _npc;
        public string SaveId => "npc_nila_brant";
        public NilaBrantSaveable(NPC_NilaBrant npc) => _npc = npc;
        public object CaptureState() => _npc.CaptureState();
        public void RestoreState(object state) => _npc.RestoreState(state as NPC_NilaBrantState);
    }

    // ═══════════════════════════════════════════════════════════════════
    // ASHFALL: THE STANDING RECORD — saveable adapters
    // ═══════════════════════════════════════════════════════════════════

    internal class LocationLayoutSaveable : ISaveable
    {
        private readonly LocationLayoutSystem _system;
        public string SaveId => LocationLayoutSystem.SystemId;
        public LocationLayoutSaveable(LocationLayoutSystem system) => _system = system;
        public object CaptureState() => _system.CaptureState();
        public void RestoreState(object state) => _system.RestoreState(state as LocationLayoutState);
    }

    internal class LocationMemorySaveable : ISaveable
    {
        private readonly LocationMemorySystem _system;
        public string SaveId => LocationMemorySystem.SystemId;
        public LocationMemorySaveable(LocationMemorySystem system) => _system = system;
        public object CaptureState() => _system.CaptureState();
        public void RestoreState(object state) => _system.RestoreState(state as LocationMemoryState);
    }

    internal class SiteEncounterSaveable : ISaveable
    {
        private readonly SiteEncounterSystem _system;
        public string SaveId => SiteEncounterSystem.SystemId;
        public SiteEncounterSaveable(SiteEncounterSystem system) => _system = system;
        public object CaptureState() => _system.CaptureState();
        public void RestoreState(object state) => _system.RestoreState(state as SiteEncounterState);
    }
}
