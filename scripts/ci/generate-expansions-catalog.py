#!/usr/bin/env python3
"""
generate-expansions-catalog.py — Authoritative Expansions 01-11 Master Catalog Generator

Generates docs/expansions/EXPANSIONS_MASTER_CATALOG.md mapping all 11 expansions across:
  - Expansion Number and Working Title
  - Core Domain Namespace and Systems
  - Godot Host Session Class and Persistence Section Key in campaign.json
  - Authoritative StreamingAssets JSON Feeds
  - Headless Godot Self-Test CLI Verbs

Usage:
  python3 scripts/ci/generate-expansions-catalog.py          # Generates EXPANSIONS_MASTER_CATALOG.md
  python3 scripts/ci/generate-expansions-catalog.py --check  # Verifies 0 drift in CI
"""

import os
import sys
import pathlib
from datetime import datetime, timezone

REPO_ROOT = pathlib.Path(__file__).resolve().parent.parent.parent
OUTPUT_FILE = REPO_ROOT / "docs" / "expansions" / "EXPANSIONS_MASTER_CATALOG.md"

EXPANSIONS = [
    {
        "num": "01",
        "title": "The Holdfast & The Ice Road",
        "namespace": "Ashfall.Core.IceRoad / Ashfall.Core.Shelter",
        "systems": "IceRoadSystem, ClerkLedgerSystem, HoldfastCatalog, HoldfastTradeSession",
        "host_session": "HoldfastRuntimeSession",
        "save_section": "holdfast_s1",
        "data_feeds": "ice_road_catalog.json, holdfast_quests.json, items.json",
        "cli_verbs": "--holdfast-selftest, --ice-road-selftest, --ice-road-tick-demo",
        "summary": "Sub-zero survival loop, frozen supply road convoys, clerk requisition ledger, and starter survivor cohort."
    },
    {
        "num": "02",
        "title": "The Duty Roster",
        "namespace": "Ashfall.Core.DutyRoster / Ashfall.Core.Survivors",
        "systems": "DutyRosterSystem, WorkShiftSystem, FatigueAccumulator, AssignmentMatrix",
        "host_session": "DutyRosterHostSession",
        "save_section": "duty_roster",
        "data_feeds": "duty_roster.json, shifts.json, survivor_traits.json",
        "cli_verbs": "--duty-roster-selftest, --duty-roster-save-selftest",
        "summary": "24-hour work shift scheduling, fatigue drift, burnout risks, night-shift penalties, and critical post assignments."
    },
    {
        "num": "03",
        "title": "The Standing Record",
        "namespace": "Ashfall.Core.StandingRecord / Ashfall.Core.Factions",
        "systems": "StandingRecordSystem, TrustMomentumSystem, GrievanceLedger, TreatyMatrix",
        "host_session": "StandingRecordHostSession",
        "save_section": "standing_record",
        "data_feeds": "standing_record.json, factions.json, treaties.json",
        "cli_verbs": "--standing-record-selftest, --factions-selftest",
        "summary": "Faction reputation drift, historical grievance tracking, tribute pacts, regional border tensions, and ceasefire terms."
    },
    {
        "num": "04",
        "title": "Nobody's Charter & The Crossing",
        "namespace": "Ashfall.Core.Crossing / Ashfall.Core.Quests",
        "systems": "CrossingArbitrationSystem, NobodysCharterSystem, BorderControlMatrix",
        "host_session": "CrossingHostSession",
        "save_section": "crossing",
        "data_feeds": "crossing.json, arbitrations.json, contraband.json",
        "cli_verbs": "--crossing-selftest, --arbitration-selftest",
        "summary": "Neutral river crossing checkpoint, refugee arbitration disputes, contraband confiscation, and border security."
    },
    {
        "num": "05",
        "title": "The Year of Ash & The Greenhouse",
        "namespace": "Ashfall.Core.Greenhouse / Ashfall.Core.World",
        "systems": "GreenhouseSystem, YearOfAshSystem, AshContaminationSystem, SoilDegradationSystem",
        "host_session": "YearOfAshHostSession",
        "save_section": "greenhouse",
        "data_feeds": "greenhouse.json, crops.json, soil_nutrients.json",
        "cli_verbs": "--greenhouse-selftest, --year-of-ash-selftest",
        "summary": "Hydroponic crop cultivation, atmospheric ash deposition, soil decontamination, grow-light power draw, and food security."
    },
    {
        "num": "06",
        "title": "The Muster",
        "namespace": "Ashfall.Core.Muster / Ashfall.Core.Combat",
        "systems": "MusterSystem, MilitiaRosterSystem, FortificationSystem, DefenseWaveMatrix",
        "host_session": "MusterHostSession",
        "save_section": "muster",
        "data_feeds": "muster.json, defensive_structures.json, raid_doctrines.json",
        "cli_verbs": "--muster-selftest",
        "summary": "Shelter militia mobilization, automated turrets, perimeter barricade maintenance, and raider defense sieges."
    },
    {
        "num": "07",
        "title": "The Dose",
        "namespace": "Ashfall.Core.Medical / Ashfall.Core.Radiation",
        "systems": "DoseLedgerSystem, AcuteRadiationSicknessSystem, ChelationTherapySystem",
        "host_session": "DoseLedgerHostSession",
        "save_section": "dose_ledger",
        "data_feeds": "dose_ledger.json, radiation_treatments.json, dosimeters.json",
        "cli_verbs": "--dose-ledger-selftest, --radiation-selftest",
        "summary": "Cumulative radiation dosage tracking, bone marrow damage, chelation protocols, and thyroid saturation treatments."
    },
    {
        "num": "08",
        "title": "The Verdict",
        "namespace": "Ashfall.Core.Verdict / Ashfall.Core.Narrative",
        "systems": "VerdictSystem, ReckoningPhaseSystem, EvidenceLockerSystem, CensusAuditSystem",
        "host_session": "VerdictHostSession",
        "save_section": "verdict",
        "data_feeds": "verdict.json, evidence.json, tribunal_charges.json",
        "cli_verbs": "--verdict-selftest",
        "summary": "Community tribunal, evidence collection, pre-war guilt investigations, exile sentencing, and social cohesion shifts."
    },
    {
        "num": "09",
        "title": "The Black Flotilla",
        "namespace": "Ashfall.Core.Flotilla / Ashfall.Core.Maritime",
        "systems": "BlackFlotillaSystem, ScavengeDiveSystem, MarineContaminationSystem, VesselConditionSystem",
        "host_session": "BlackFlotillaHostSession",
        "save_section": "black_flotilla",
        "data_feeds": "black_flotilla.json, maritime_salvage.json, dive_zones.json",
        "cli_verbs": "--black-flotilla-selftest",
        "summary": "Sunken naval wreckage salvage, submarine diving suits, air supply management, underwater radiation, and marine loot."
    },
    {
        "num": "10",
        "title": "The Silent Foundry",
        "namespace": "Ashfall.Core.Foundry / Ashfall.Core.Crafting",
        "systems": "SilentFoundrySystem, RelicReverseEngineeringSystem, HeavyFabricationMatrix",
        "host_session": "SilentFoundryHostSession",
        "save_section": "silent_foundry",
        "data_feeds": "silent_foundry.json, foundry_recipes.json, metallurgical_alloys.json",
        "cli_verbs": "--silent-foundry-selftest",
        "summary": "Geothermal subterranean forge, military-grade alloy smelting, blueprint replication, and advanced machining."
    },
    {
        "num": "11",
        "title": "The Long Line",
        "namespace": "Ashfall.Core.LongLine / Ashfall.Core.Logistics",
        "systems": "LongLineLogisticsSystem, RelayStationSystem, LongRangeTelegraphSystem",
        "host_session": "LongLineHostSession",
        "save_section": "long_line",
        "data_feeds": "long_line.json, telegraph_cables.json, logistics_routes.json",
        "cli_verbs": "--long-line-selftest",
        "summary": "Trans-continental telegraph cable network, relay maintenance, long-distance signal routing, and weather interference."
    }
]

def generate_catalog():
    today = datetime.now(timezone.utc).strftime("%Y-%m-%d")
    lines = [
        "# ASHFALL Expansions 01–11 Master Systems & Integration Atlas",
        "",
        f"**Authoritative Expansion Catalog** | **Generated:** {today} | **Total Expansions:** {len(EXPANSIONS)}",
        "",
        "> [!IMPORTANT]",
        "> **EXPANSION INTEGRATION RULES:**",
        "> 1. **Core Gameplay Logic**: Lives strictly in `Assets/Ashfall.Core/<Domain>/` with 0 Godot/Unity dependencies.",
        "> 2. **Persistence Boundary**: Each expansion must persist via its own section in `campaign.json` utilizing `SaveStoreHub` or Core codecs.",
        "> 3. **Data Feeds**: Authoritative JSON catalogs live in `Assets/StreamingAssets/Data/` with integer `schema_version`.",
        "> 4. **Headless Verification**: Every expansion maintains at least one headless Godot CLI verification verb.",
        "",
        "---",
        "",
        "## Master Expansions Summary Matrix",
        "",
        "| Exp # | Expansion Title | Domain Systems | Host Session | Save Section Key | Self-Test Verbs |",
        "|---|---|---|---|---|---|"
    ]

    for exp in EXPANSIONS:
        lines.append(f"| **{exp['num']}** | {exp['title']} | `{exp['namespace']}` | `{exp['host_session']}` | `{exp['save_section']}` | `{exp['cli_verbs']}` |")

    lines.extend([
        "",
        "---",
        "",
        "## Detailed Subsystem & Data Seams",
        ""
    ])

    for exp in EXPANSIONS:
        lines.extend([
            f"### Expansion {exp['num']}: {exp['title']}",
            "",
            f"- **Overview:** {exp['summary']}",
            f"- **Core Namespace:** `{exp['namespace']}`",
            f"- **Core Systems:** `{exp['systems']}`",
            f"- **Godot Host Session:** `{exp['host_session']}`",
            f"- **Campaign Save Section:** `{exp['save_section']}` in `campaign.json`",
            f"- **Authoritative JSON Feeds:** `{exp['data_feeds']}`",
            f"- **Headless CLI Verbs:** `{exp['cli_verbs']}`",
            ""
        ])

    OUTPUT_FILE.parent.mkdir(parents=True, exist_ok=True)
    OUTPUT_FILE.write_text("\n".join(lines), encoding="utf-8")
    print(f"Wrote {OUTPUT_FILE} ({len(EXPANSIONS)} expansions mapped).")

if __name__ == "__main__":
    check_mode = "--check" in sys.argv
    if check_mode and OUTPUT_FILE.exists():
        curr = OUTPUT_FILE.read_text(encoding="utf-8")
        generate_catalog()
        new = OUTPUT_FILE.read_text(encoding="utf-8")
        if curr != new:
            print("❌ Error: EXPANSIONS_MASTER_CATALOG.md drifted from generator.", file=sys.stderr)
            sys.exit(1)
        print("OK: EXPANSIONS_MASTER_CATALOG.md is in sync.")
        sys.exit(0)
    generate_catalog()
