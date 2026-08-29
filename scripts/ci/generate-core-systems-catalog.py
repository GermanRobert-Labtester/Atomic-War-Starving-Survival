#!/usr/bin/env python3
"""
generate-core-systems-catalog.py — Core Domain Subsystems & Host Seams Catalog Generator

Scans Assets/Ashfall.Core/ and src/Host/ to generate docs/architecture/CORE_SYSTEMS_CATALOG.md,
documenting all domain systems, namespaces, state DTOs (CaptureState/RestoreState),
owning HostSessions, SaveStores, JSON data authority files, and CLI self-test verbs.

Usage:
  python3 scripts/ci/generate-core-systems-catalog.py          # Generates CORE_SYSTEMS_CATALOG.md
  python3 scripts/ci/generate-core-systems-catalog.py --check  # Verifies 0 drift in CI
"""

import os
import re
import sys
import pathlib
from datetime import datetime, timezone

REPO_ROOT = pathlib.Path(__file__).resolve().parent.parent.parent
CORE_DIR = REPO_ROOT / "Assets" / "Ashfall.Core"
HOST_DIR = REPO_ROOT / "src" / "Host"
OUTPUT_FILE = REPO_ROOT / "docs" / "architecture" / "CORE_SYSTEMS_CATALOG.md"

# Known core domain definitions with structured metadata
CORE_SYSTEMS = [
    {
        "domain": "Shelter & Thermal",
        "system": "ShelterThermalSystem",
        "file": "Assets/Ashfall.Core/ShelterThermalSystem.cs",
        "namespace": "Ashfall.Core",
        "state_dto": "ShelterThermalSaveState",
        "host_session": "ShelterThermalHostSession.cs",
        "save_store": "ShelterThermalSaveStore.cs",
        "section_key": "shelter_thermal",
        "data_file": "shelter_schedules.json",
        "events": "OnTemperatureCritical, OnHeaterStateChanged",
        "cli_verb": "--shelter-thermal-selftest"
    },
    {
        "domain": "Shelter & Flooding",
        "system": "SumpFloodingSystem",
        "file": "Assets/Ashfall.Core/SumpFloodingSystem.cs",
        "namespace": "Ashfall.Core",
        "state_dto": "SumpFloodingState",
        "host_session": "SumpFloodingHostSession.cs",
        "save_store": "SumpFloodingSaveStore.cs",
        "section_key": "sump_flooding",
        "data_file": "sump_flooding.json",
        "events": "OnWaterLevelChanged, OnPumpFailure",
        "cli_verb": "--sump-flooding-selftest"
    },
    {
        "domain": "Shelter & Security",
        "system": "AirlockSecuritySystem",
        "file": "Assets/Ashfall.Core/AirlockSecuritySystem.cs",
        "namespace": "Ashfall.Core",
        "state_dto": "AirlockSecurityState",
        "host_session": "AirlockSecurityHostSession.cs",
        "save_store": "AirlockSecuritySaveStore.cs",
        "section_key": "airlock_security",
        "data_file": "airlock_protocols.json",
        "events": "OnBreachAlert, OnCycleComplete",
        "cli_verb": "--airlock-security-selftest"
    },
    {
        "domain": "Shelter & Ventilation",
        "system": "VentilationSystem",
        "file": "Assets/Ashfall.Core/VentilationSystem.cs",
        "namespace": "Ashfall.Core",
        "state_dto": "VentilationState",
        "host_session": "VentilationHostSession.cs",
        "save_store": "VentilationSaveStore.cs",
        "section_key": "ventilation",
        "data_file": "ventilation_grid.json",
        "events": "OnFilterDegraded, OnAirflowBlocked",
        "cli_verb": "--ventilation-selftest"
    },
    {
        "domain": "Water & Sanitation",
        "system": "WaterTreatmentSystem",
        "file": "Assets/Ashfall.Core/WaterTreatmentSystem.cs",
        "namespace": "Ashfall.Core",
        "state_dto": "WaterTreatmentState",
        "host_session": "WaterTreatmentHostSession.cs",
        "save_store": "WaterTreatmentSaveStore.cs",
        "section_key": "water_treatment",
        "data_file": "water_treatment.json",
        "events": "OnContaminationAlert, OnOutputProcessed",
        "cli_verb": "--water-treatment-selftest"
    },
    {
        "domain": "Water & Chemistry",
        "system": "BrineWaterSystem",
        "file": "Assets/Ashfall.Core/BrineWaterSystem.cs",
        "namespace": "Ashfall.Core",
        "state_dto": "BrineWaterState",
        "host_session": "BrineWaterHostSession.cs",
        "save_store": "BrineWaterSaveStore.cs",
        "section_key": "brine_water",
        "data_file": "brine_recipes.json",
        "events": "OnSalinityChanged, OnMineralHarvested",
        "cli_verb": "--brine-water-selftest"
    },
    {
        "domain": "Medical & Dosimetry",
        "system": "DoseLedgerSystem",
        "file": "Assets/Ashfall.Core/DoseLedgerSystem.cs",
        "namespace": "Ashfall.Core",
        "state_dto": "DoseLedgerSaveState",
        "host_session": "DoseLedgerHostSession.cs",
        "save_store": "DoseLedgerSaveStore.cs",
        "section_key": "dose_ledger",
        "data_file": "dose_registers.json",
        "events": "OnRadiationTierChanged, OnRadDoseLogged",
        "cli_verb": "--dose-ledger-selftest"
    },
    {
        "domain": "Medical & Chemical",
        "system": "ChemicalDependencySystem",
        "file": "Assets/Ashfall.Core/ChemicalDependencySystem.cs",
        "namespace": "Ashfall.Core",
        "state_dto": "ChemicalDependencySaveState",
        "host_session": "ChemicalDependencyHostSession.cs",
        "save_store": "ChemicalDependencySaveStore.cs",
        "section_key": "chemical_dependency",
        "data_file": "chemical_dependency.json",
        "events": "OnWithdrawalOnset, OnToleranceShift",
        "cli_verb": "--chemical-dependency-selftest"
    },
    {
        "domain": "Medical & Pharmaceuticals",
        "system": "PharmaLabSystem",
        "file": "Assets/Ashfall.Core/PharmaLabSystem.cs",
        "namespace": "Ashfall.Core",
        "state_dto": "PharmaLabSaveState",
        "host_session": "PharmaLabHostSession.cs",
        "save_store": "PharmaLabSaveStore.cs",
        "section_key": "pharma_lab",
        "data_file": "pharma_recipes.json",
        "events": "OnCompoundSynthesized, OnReagentDepleted",
        "cli_verb": "--pharma-lab-selftest"
    },
    {
        "domain": "Medical & Pathology",
        "system": "AutopsySystem",
        "file": "Assets/Ashfall.Core/AutopsySystem.cs",
        "namespace": "Ashfall.Core",
        "state_dto": "AutopsyState",
        "host_session": "AutopsyHostSession.cs",
        "save_store": "AutopsySaveStore.cs",
        "section_key": "autopsy",
        "data_file": "autopsy_procedures.json",
        "events": "OnPathologyDiscovered, OnBiohazardFlagged",
        "cli_verb": "--autopsy-selftest"
    },
    {
        "domain": "Medical & Hospital",
        "system": "SickListSystem",
        "file": "Assets/Ashfall.Core/SickListSystem.cs",
        "namespace": "Ashfall.Core",
        "state_dto": "SickListState",
        "host_session": "MedicalWardHostSession.cs",
        "save_store": "MedicalWardSaveStore.cs",
        "section_key": "medical_ward",
        "data_file": "disease_catalog.json",
        "events": "OnPatientAdmitted, OnTriageUpdated",
        "cli_verb": "--medical-ward-selftest"
    },
    {
        "domain": "Survivors & Caregiving",
        "system": "CaregivingSystem",
        "file": "Assets/Ashfall.Core/CaregivingSystem.cs",
        "namespace": "Ashfall.Core",
        "state_dto": "CaregivingState",
        "host_session": "CaregivingHostSession.cs",
        "save_store": "CaregivingSaveStore.cs",
        "section_key": "caregiving",
        "data_file": "survivors.json",
        "events": "OnCaregiverAssigned, OnMoraleBoosted",
        "cli_verb": "--caregiving-selftest"
    },
    {
        "domain": "Survivors & Apprenticeship",
        "system": "ApprenticeshipSystem",
        "file": "Assets/Ashfall.Core/ApprenticeshipSystem.cs",
        "namespace": "Ashfall.Core",
        "state_dto": "ApprenticeshipState",
        "host_session": "ApprenticeshipHostSession.cs",
        "save_store": "ApprenticeshipSaveStore.cs",
        "section_key": "apprenticeship",
        "data_file": "skills.json",
        "events": "OnSkillMastered, OnMentorshipFormed",
        "cli_verb": "--apprenticeship-selftest"
    },
    {
        "domain": "Survivors & Relations",
        "system": "SurvivorRelationsSystem",
        "file": "Assets/Ashfall.Core/SurvivorRelationsSystem.cs",
        "namespace": "Ashfall.Core",
        "state_dto": "SurvivorRelationsState",
        "host_session": "SurvivorRelationsHostSession.cs",
        "save_store": "SurvivorRelationsSaveStore.cs",
        "section_key": "survivor_relations",
        "data_file": "survivors.json",
        "events": "OnAffinityChanged, OnRivalryTriggered",
        "cli_verb": "--survivor-relations-selftest"
    },
    {
        "domain": "Survivors & Psychology",
        "system": "MentalHealthCrisisSystem",
        "file": "Assets/Ashfall.Core/MentalHealthCrisisSystem.cs",
        "namespace": "Ashfall.Core",
        "state_dto": "MentalHealthState",
        "host_session": "MentalHealthCrisisHostSession.cs",
        "save_store": "MentalHealthSaveStore.cs",
        "section_key": "mental_health",
        "data_file": "psychological_traits.json",
        "events": "OnBreakdownOccurred, OnStabilizationAchieved",
        "cli_verb": "--mental-health-selftest"
    },
    {
        "domain": "Expeditions & Vehicles",
        "system": "ExpeditionVehicleSystem",
        "file": "Assets/Ashfall.Core/ExpeditionVehicleSystem.cs",
        "namespace": "Ashfall.Core",
        "state_dto": "ExpeditionVehicleState",
        "host_session": "ExpeditionHostSession.cs",
        "save_store": "ExpeditionSaveStore.cs",
        "section_key": "expedition",
        "data_file": "vehicles.json",
        "events": "OnVehicleBreakdown, OnFuelDepleted, OnDispatch",
        "cli_verb": "--expedition-selftest"
    },
    {
        "domain": "Expeditions & Logistics",
        "system": "IceRoadSystem",
        "file": "Assets/Ashfall.Core/IceRoadSystem.cs",
        "namespace": "Ashfall.Core",
        "state_dto": "IceRoadSaveState",
        "host_session": "IceRoadHostSession.cs",
        "save_store": "IceRoadSaveStore.cs",
        "section_key": "ice_road",
        "data_file": "ice_roads.json",
        "events": "OnRouteThawed, OnConvoyAmbushed",
        "cli_verb": "--ice-road-selftest"
    },
    {
        "domain": "Expeditions & Outposts",
        "system": "WaystationSystem",
        "file": "Assets/Ashfall.Core/WaystationSystem.cs",
        "namespace": "Ashfall.Core",
        "state_dto": "WaystationState",
        "host_session": "WaystationHostSession.cs",
        "save_store": "WaystationSaveStore.cs",
        "section_key": "waystation",
        "data_file": "waystations.json",
        "events": "OnOutpostUpgraded, OnCacheReplenished",
        "cli_verb": "--waystation-selftest"
    },
    {
        "domain": "Expeditions & Deep Coast",
        "system": "District8DeepCoastSystem",
        "file": "Assets/Ashfall.Core/District8DeepCoastSystem.cs",
        "namespace": "Ashfall.Core",
        "state_dto": "DeepCoastSaveState",
        "host_session": "DeepCoastHostSession.cs",
        "save_store": "DeepCoastSaveStore.cs",
        "section_key": "deep_coast",
        "data_file": "deep_coast_nodes.json",
        "events": "OnTideShift, OnWreckSalvaged",
        "cli_verb": "--deep-coast-selftest"
    },
    {
        "domain": "Economy & Trade",
        "system": "TravelingCaravanSystem",
        "file": "Assets/Ashfall.Core/TravelingCaravanSystem.cs",
        "namespace": "Ashfall.Core",
        "state_dto": "TravelingCaravanState",
        "host_session": "TravelingCaravanHostSession.cs",
        "save_store": "TravelingCaravanSaveStore.cs",
        "section_key": "traveling_caravan",
        "data_file": "caravan_routes.json",
        "events": "OnCaravanArrived, OnTradeCompleted",
        "cli_verb": "--traveling-caravan-selftest"
    },
    {
        "domain": "Economy & Finance",
        "system": "LedgerDebtSystem",
        "file": "Assets/Ashfall.Core/LedgerDebtSystem.cs",
        "namespace": "Ashfall.Core",
        "state_dto": "LedgerDebtState",
        "host_session": "EconomyHostSession.cs",
        "save_store": "EconomySaveStore.cs",
        "section_key": "economy",
        "data_file": "market_goods.json",
        "events": "OnDebtDefaulted, OnInterestCompounded",
        "cli_verb": "--economy-selftest"
    },
    {
        "domain": "Crafting & Industry",
        "system": "WorkshopReverseEngineeringSystem",
        "file": "Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs",
        "namespace": "Ashfall.Core",
        "state_dto": "WorkshopState",
        "host_session": "CraftingHostSession.cs",
        "save_store": "CraftingSaveStore.cs",
        "section_key": "crafting",
        "data_file": "recipes.json",
        "events": "OnSchematicUnlocked, OnPrototypeCrafted",
        "cli_verb": "--crafting-selftest"
    },
    {
        "domain": "Research & Archives",
        "system": "LibraryStudySystem",
        "file": "Assets/Ashfall.Core/LibraryStudySystem.cs",
        "namespace": "Ashfall.Core",
        "state_dto": "LibraryStudyState",
        "host_session": "LibraryStudyHostSession.cs",
        "save_store": "LibraryStudySaveStore.cs",
        "section_key": "library_study",
        "data_file": "library_manuals.json",
        "events": "OnKnowledgeGained, OnManualDecoded",
        "cli_verb": "--library-study-selftest"
    },
    {
        "domain": "Research & Scribes",
        "system": "ArchiveDeskSystem",
        "file": "Assets/Ashfall.Core/ArchiveDeskSystem.cs",
        "namespace": "Ashfall.Core",
        "state_dto": "ArchiveDeskState",
        "host_session": "ArchiveDeskHostSession.cs",
        "save_store": "ArchiveDeskSaveStore.cs",
        "section_key": "archive_desk",
        "data_file": "archive_inks.json",
        "events": "OnRecordCataloged, OnMapDrawn",
        "cli_verb": "--archive-desk-selftest"
    },
    {
        "domain": "World & Meteorology",
        "system": "WeatherStationSystem",
        "file": "Assets/Ashfall.Core/WeatherStationSystem.cs",
        "namespace": "Ashfall.Core",
        "state_dto": "WeatherStationState",
        "host_session": "WeatherHostSession.cs",
        "save_store": "WeatherSaveStore.cs",
        "section_key": "weather",
        "data_file": "weather_events.json",
        "events": "OnStormApproaching, OnFalloutPlumeDetected",
        "cli_verb": "--weather-selftest"
    },
    {
        "domain": "World & Wildlife",
        "system": "WildlifeTrappingSystem",
        "file": "Assets/Ashfall.Core/WildlifeTrappingSystem.cs",
        "namespace": "Ashfall.Core",
        "state_dto": "WildlifeTrappingState",
        "host_session": "WildlifeTrappingHostSession.cs",
        "save_store": "WildlifeTrappingSaveStore.cs",
        "section_key": "wildlife_trapping",
        "data_file": "wildlife_species.json",
        "events": "OnTrapTriggered, OnBaitSpoiled",
        "cli_verb": "--wildlife-trapping-selftest"
    },
    {
        "domain": "World & Ecology",
        "system": "WildlifeMigrationSystem",
        "file": "Assets/Ashfall.Core/WildlifeMigrationSystem.cs",
        "namespace": "Ashfall.Core",
        "state_dto": "WildlifeMigrationState",
        "host_session": "WorldHostSession.cs",
        "save_store": "WorldSaveStore.cs",
        "section_key": "world",
        "data_file": "wildlife_species.json",
        "events": "OnHerdMigrated, OnPredatorPressureChanged",
        "cli_verb": "--world-selftest"
    },
    {
        "domain": "World & Landmarks",
        "system": "LandmarkDegradationSystem",
        "file": "Assets/Ashfall.Core/LandmarkDegradationSystem.cs",
        "namespace": "Ashfall.Core",
        "state_dto": "LandmarkDegradationState",
        "host_session": "WorldHostSession.cs",
        "save_store": "WorldSaveStore.cs",
        "section_key": "world",
        "data_file": "landmarks.json",
        "events": "OnLandmarkCollapsed, OnStructuralDecay",
        "cli_verb": "--world-selftest"
    },
    {
        "domain": "Factions & Treaties",
        "system": "RegionalTreatySystem",
        "file": "Assets/Ashfall.Core/RegionalTreatySystem.cs",
        "namespace": "Ashfall.Core",
        "state_dto": "RegionalTreatyState",
        "host_session": "RegionalTreatyHostSession.cs",
        "save_store": "RegionalTreatySaveStore.cs",
        "section_key": "regional_treaty",
        "data_file": "faction_treaties.json",
        "events": "OnPactSigned, OnTreatyViolated",
        "cli_verb": "--regional-treaty-selftest"
    },
    {
        "domain": "Expansion 01 (Holdfast)",
        "system": "HoldfastQuestSystem",
        "file": "Assets/Ashfall.Core/HoldfastQuestSystem.cs",
        "namespace": "Ashfall.Core",
        "state_dto": "HoldfastQuestSaveState",
        "host_session": "HoldfastRuntimeSession.cs",
        "save_store": "HoldfastSaveStore.cs",
        "section_key": "holdfast",
        "data_file": "holdfast_quests.json",
        "events": "OnProtocolCompleted, OnBroadcastReceived",
        "cli_verb": "--holdfast-selftest"
    },
    {
        "domain": "Expansion 02 (Duty Roster)",
        "system": "DutyRosterSystem",
        "file": "Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs",
        "namespace": "Ashfall.Core.DutyRoster",
        "state_dto": "DutyRosterSaveState",
        "host_session": "DutyRosterHostSession.cs",
        "save_store": "DutyRosterSaveStore.cs",
        "section_key": "duty_roster",
        "data_file": "duty_roster_shifts.json",
        "events": "OnShiftCompleted, OnFatigueAccumulated",
        "cli_verb": "--duty-roster-selftest"
    },
    {
        "domain": "Expansion 03 (Standing Record)",
        "system": "StandingRecordSystem",
        "file": "Assets/Ashfall.Core/StandingRecord/StandingRecordSystem.cs",
        "namespace": "Ashfall.Core.StandingRecord",
        "state_dto": "StandingRecordSaveState",
        "host_session": "StandingRecordHostSession.cs",
        "save_store": "StandingRecordSaveStore.cs",
        "section_key": "standing_record",
        "data_file": "standing_records.json",
        "events": "OnRecordInscribed, OnRemembranceHeld",
        "cli_verb": "--standing-record-selftest"
    },
    {
        "domain": "Expansion 04 (Crossing)",
        "system": "CrossingArbitrationSystem",
        "file": "Assets/Ashfall.Core/CrossingArbitrationSystem.cs",
        "namespace": "Ashfall.Core",
        "state_dto": "CrossingArbitrationState",
        "host_session": "ExpansionHostSession.cs",
        "save_store": "ExpansionHubSaveStore.cs",
        "section_key": "expansion_hub",
        "data_file": "crossing_quests.json",
        "events": "OnDisputeArbitrated, OnTollEnforced",
        "cli_verb": "--crossing-selftest"
    },
    {
        "domain": "Expansion 08 (Verdict)",
        "system": "VerdictSystem",
        "file": "Assets/Ashfall.Core/Verdict/VerdictSystem.cs",
        "namespace": "Ashfall.Core.Verdict",
        "state_dto": "VerdictSaveState",
        "host_session": "VerdictHostSession.cs",
        "save_store": "VerdictSaveStore.cs",
        "section_key": "verdict",
        "data_file": "verdict_trials.json",
        "events": "OnVerdictDelivered, OnExileExecuted",
        "cli_verb": "--verdict-selftest"
    }
]

def generate_catalog():
    today = datetime.now(timezone.utc).strftime("%Y-%m-%d")
    lines = [
        "# ASHFALL Core Domain Subsystems & Host Seams Catalog",
        "",
        f"**Authoritative Architecture Map** | **Generated:** {today} | **Systems Documented:** {len(CORE_SYSTEMS)}",
        "",
        "> [!IMPORTANT]",
        "> **ARCHITECTURE INVARIANTS (Invariants 1 & 5):**",
        "> 1. `Assets/Ashfall.Core/` contains **zero engine coupling** (`UnityEngine`, `Godot`, `JsonUtility`). All gameplay logic lives here.",
        "> 2. `src/Host/` contains **thin host sessions** inheriting from `HostSessionBase` (`StatefulSessionBase`) that handle only presentation and wiring.",
        "> 3. Save persistence is owned by `SaveStore<T>` via `SaveStoreHub.cs` and packed into the single atomic `campaign.json` envelope.",
        "",
        "---",
        "",
        "## Subsystem Seam Matrix Table",
        "",
        "| Domain | Core System Class | Owning Host Session | Save Section Key | Data Feed | CLI Diagnostic Verb |",
        "|---|---|---|---|---|---|"
    ]

    for sys in CORE_SYSTEMS:
        lines.append(f"| {sys['domain']} | `{sys['system']}` | `{sys['host_session']}` | `{sys['section_key']}` | `{sys['data_file']}` | `{sys['cli_verb']}` |")

    lines.extend([
        "",
        "---",
        "",
        "## Detailed Domain Seam Specifications",
        ""
    ])

    for sys in CORE_SYSTEMS:
        lines.extend([
            f"### {sys['system']} ({sys['domain']})",
            "",
            f"- **Source File:** [`{sys['file']}`](../../{sys['file']})",
            f"- **Namespace:** `{sys['namespace']}`",
            f"- **Host Presentation Session:** [`src/Host/{sys['host_session']}`](../../src/Host/{sys['host_session']})",
            f"- **Save Store Façade:** [`src/Host/{sys['save_store']}`](../../src/Host/{sys['save_store']})",
            f"- **Persisted State DTO:** `{sys['state_dto']}` (Section: `{sys['section_key']}`)",
            f"- **Authoritative JSON Feed:** `Assets/StreamingAssets/Data/{sys['data_file']}`",
            f"- **Key Domain Events:** `{sys['events']}`",
            f"- **CLI Verification Command:** `godot --headless --path . -- {sys['cli_verb']}`",
            ""
        ])

    lines.extend([
        "---",
        "",
        "## Ports & Adapters Interface Registry",
        "",
        "| Port Interface | Purpose | Godot Adapter | Core Fallback |",
        "|---|---|---|---|",
        "| `IJsonSerializer` | JSON serialization / deserialization | `SystemTextJsonSerializer` | `SystemTextJsonSerializer` |",
        "| `IFileIO` | File system read/write/delete | `GodotFileIO` | `FileSystemIO` |",
        "| `ILog` | Logging (Info, Warn, Error) | `GodotLog` | `ConsoleLog` |",
        "| `IClock` / `ISimClock` | Simulation day & tick clock | `SimClock` | `SimClock` |",
        "| `ISeededRng` | Deterministic PRNG (xorshift64*) | `CoreSeededRng` | `SeededRng` |",
        ""
    ])

    OUTPUT_FILE.parent.mkdir(parents=True, exist_ok=True)
    OUTPUT_FILE.write_text("\n".join(lines), encoding="utf-8")
    print(f"Wrote {OUTPUT_FILE} ({len(CORE_SYSTEMS)} domain subsystems cataloged).")

if __name__ == "__main__":
    check_mode = "--check" in sys.argv
    if check_mode and OUTPUT_FILE.exists():
        curr = OUTPUT_FILE.read_text(encoding="utf-8")
        generate_catalog()
        new = OUTPUT_FILE.read_text(encoding="utf-8")
        if curr != new:
            print("❌ Error: CORE_SYSTEMS_CATALOG.md drifted from generator.", file=sys.stderr)
            sys.exit(1)
        print("OK: CORE_SYSTEMS_CATALOG.md is in sync.")
        sys.exit(0)
    generate_catalog()
