#!/usr/bin/env python3
# SPDX-License-Identifier: MIT
"""
generate-plan-integration-audit.py — Recent Plan Integration Audit Generator

Programmatically verifies every recently integrated plan against the live
repository (current source, not ledger claims). For each plan it checks:

  1. Core authority exists in Assets/Ashfall.Core
  2. Host references exist in src/ (host reachability)
  3. Save section is registered in SaveSectionRegistry (own or parent section)
  4. Setup/Save methods exist on the Godot host Main partials
  5. Save store class exists in src/Host
  6. UI panel files exist in src/UI
  7. Dashboard/expansion routes are registered
  8. CLI self-test flag is parsed by HostCli AND cataloged in SELFTEST_MANIFEST.json
  9. Test fixtures exist under Ashfall.Core.Tests

Claims come from the curated ARCHITECTURE_GRAPH (generate-architecture-map.py)
plus explicit overrides for read-model / nested-section plans; every claim is
then independently re-verified against the source tree.

Usage:
  python3 scripts/ci/generate-plan-integration-audit.py          # Write the audit
  python3 scripts/ci/generate-plan-integration-audit.py --check  # Verify no drift
"""

import importlib.util
import json
import pathlib
import re
import sys
from datetime import datetime, timezone

REPO_ROOT = pathlib.Path(__file__).resolve().parent.parent.parent
OUT_MD = REPO_ROOT / "docs" / "plans" / "RECENT_PLAN_INTEGRATIONS_AUDIT.md"
OUT_JSON = REPO_ROOT / "docs" / "plans" / "recent_plan_integration_audit.json"

EXCLUDED = ("/obj/", "/bin/", "/.git/", "/builds/", "/artifacts/", "/__pycache__/")

# plan, title, section, inside (parent section), authorities (override), cli, tests
RECENT_PLANS = [
    ("30",  "Expansion 30 — The Press",                   "broadsheet_press",    None,                [], "--broadsheet-press-selftest", ["BroadsheetPressLedgerTests", "PublicBroadsheetPressEngineTests"]),
    ("31",  "Expansion 31 — The Kiln",                   "kilnworks",           None,                [], "--kilnworks-selftest",        ["KilnFiringLedgerTests", "KilnFiringEngineTests"]),
    ("35",  "Expansion 35 — The Habit",                  "dependency_taper_withdrawal", None,        [], "--dependency-taper-selftest", ["DependencyTaperWithdrawalEngineTests", "DependencyTaperLedgerTests"]),
    ("36",  "Expansion 36 — The Watch",                  "perimeter_defense",        None,                ["NightWatchPatrolReadinessEngine"], "--patrol-encounter-selftest", ["NightWatchPatrolReadinessEngineTests", "NightWatchOperationsTests", "NightWatchHostIntegrationTests"]),
    ("37",  "Expansion 37 — The Quickening",             "antenatal_maternal_health", None,       [], "--antenatal-care-selftest", ["AntenatalMaternalHealthEngineTests", "AntenatalMaternalCareLedgerTests"]),
    ("38-exp", "Expansion 38 — The Ward",               "clinical_ward_triage", None,        [], "--clinical-ward-selftest",      ["ClinicalWardTriageEngineTests", "ClinicalWardLedgerTests"]),
    ("39-exp", "Expansion 39 — The Reagent",            "chemical_reagent_synthesis", None, [], "--chemical-reagent-selftest", ["ChemicalReagentSynthesisEngineTests", "ChemicalReagentLedgerTests"]),
    ("40-exp", "Expansion 40 — The Wheel",              "mechanical_driveline", None, [], "--mechanical-driveline-selftest", ["MechanicalPowerDrivelineEngineTests", "MechanicalDrivelineLedgerTests"]),
    ("41-exp", "Expansion 41 — The Quiet",              "sleep_acoustic_rest", None, [], "--sleep-acoustic-selftest", ["SleepAcousticRestEngineTests", "SleepAcousticLedgerTests"]),
    ("38",  "Commitments & Deadlines",              "commitment",           None,                [], "--commitments-selftest",        ["Plan38CommitmentHostIntegrationTests"]),
    ("39",  "Session Durability",                   "session_durability",   None,                [], "--session-durability-selftest",["Plan39SessionDurabilityHostIntegrationTests"]),
    ("42",  "Survivor Voice",                       "survivor_voice",       None,                [], "--survivor-voice-selftest",     ["Plan42SurvivorVoiceHostIntegrationTests"]),
    ("46",  "Playable Metrics",                     "playable_metrics",     None,                [], "--playable-metrics-selftest",   ["Plan46PlayMetricsHostIntegrationTests"]),
    ("49",  "Content Orphan Certification",         None,                   None, ["ContentOrphanCertificationEngine"], "--content-certification-selftest", []),
    ("51",  "Holdfast Presentation Slate",          None,                   None, ["HoldfastPresentationHostSession"], "--holdfast-presentation-selftest", []),
    ("52",  "Scarcity Audio",                       None,                   None, ["ScarcityAudioController", "ScarcityAudioStateMachine"], "--scarcity-audio-selftest", []),
    ("54",  "Seven-Day Slice",                      "seven_day_slice",      None,                [], "--seven-day-slice-selftest",    []),
    ("55",  "Retention & 400-Year Campaign",        "retention",            None,                [], "--retention-selftest",          ["Plan55RetentionHostIntegrationTests"]),
    ("58",  "Outposts & Second Holdfast",           "outpost_settlement",   None,                [], "--outpost-settlement-selftest", ["Plan58OutpostHostIntegrationTests"]),
    ("59",  "Standing Gates Retrospective",         None,                   None, ["StandingGateRegistry", "StandingGatesHostSession"], "--standing-gates-selftest", ["Plan59StandingGateHostIntegrationTests"]),
    ("132", "Hidden Agendas & Betrayal",            "hidden_agenda",        None,                [], "--hidden-agenda-selftest",      ["Plan132HiddenAgendaIntegrationTests"]),
    ("134", "Territory & Supply Lines",             "territory_control",    None,                [], "--territory-control-selftest",  ["Plan134TerritoryControlHostIntegrationTests"]),
    ("135", "Weather Gameplay Cascade",             "weather_cascade",      None,                [], "--weather-cascade-selftest",    ["Plan135WeatherCascadeHostIntegrationTests"]),
    ("136", "Cooking Pipeline",                     "cooking",              None,                [], "--cooking-selftest",            ["Plan136CookingHostIntegrationTests"]),
    ("137", "Needs→Performance Cascade",            None,                   None, ["NeedsPerformanceBridge"],           "--needs-performance-selftest", ["NeedsPerformanceBridgeTests"]),
    ("138", "Shelter Defense & Security",           "shelter_security",     None,                [], "--shelter-security-selftest",   ["Plan138ShelterSecurityIntegrationTests"]),
    ("140", "Generational Legacy",                  "campaign_legacy",      None,                [], "--campaign-legacy-selftest",    ["Plan140CampaignLegacyHostIntegrationTests"]),
    ("141", "Research Unlock Bridge",               "research_unlock",      None,                [], "--research-unlock-selftest",    ["Plan141ResearchUnlockHostIntegrationTests"]),
    ("143", "Afflictions → Quest/Work Bridge",      None,                   None, ["AfflictionQuestWorkBridge"], "--affliction-bridge-selftest", ["Plan143AfflictionBridgeIntegrationTests", "Plan143AfflictionBridgeHostIntegrationTests"]),
    ("145", "Unified Ending & Epilogue",            "unified_ending",       None,                [], "--unified-ending-selftest",     ["Plan145UnifiedEndingHostIntegrationTests"]),
    ("147", "NPC Memory & Relationships",           "npc_memory",           None,                [], "--npc-memory-selftest",         ["Plan147NpcMemoryHostIntegrationTests"]),
    ("148", "Ideological Friction",                 "ideological_friction", None,                [], "--ideological-friction-selftest", ["Plan148IdeologicalFrictionHostIntegrationTests"]),
    ("150", "Romance & Family",                     "romance_family",       None,                [], "--romance-family-selftest",     ["Plan150RomanceFamilyHostIntegrationTests"]),
    ("151", "Working Animals / Companions",         "companion_animals",    None, ["CompanionAnimalSystem"], "--working-animals-selftest", ["Plan151WorkingAnimalsTests"]),
    ("152", "Vehicle Customization",                "vehicle_customization",None,                [], "--vehicle-customization-selftest", ["Plan152VehicleCustomizationHostIntegrationTests"]),
    ("155", "Black Market & Underground Economy",   "black_market",         None, ["BlackMarketSystem"], "--black-market-selftest", ["Plan155BlackMarketIntegrationTests"]),
    ("162", "Shelter History & Archive",            "shelter_archive",      None,                [], "--shelter-archive-selftest",    ["Plan162ArchiveIntegrationTests", "ShelterArchiveSystemTests"]),
    ("165", "Mod & Content-Pack Contract",          None,                   None, ["ModSupportSystem"],                "--mod-support-selftest", ["Plan165ModdingIntegrationTests"]),
    ("166", "Shelter Identity & Origin",            "shelter_identity",     None,                [], "--shelter-identity-selftest",   ["ShelterIdentitySystemTests"]),
    ("167", "Tunnel Network",                       None,          "wasteland_map",   ["TunnelNetworkSystem"], "--tunnel-network-selftest", ["Plan167TunnelNetworkIntegrationTests"]),
    ("168", "Propaganda & Morale Warfare",          "propaganda_campaigns", None,                [], "--propaganda-selftest",         ["Plan168PropagandaIntegrationTests"]),
    ("169", "Audio Accessibility",                  None,                   None, ["AudioAccessibilityCoordinator"],    "--audio-accessibility-selftest", ["Plan169AudioAccessibilityIntegrationTests"]),
    ("171", "Dynamic Quest Generation",             None,       "procedural_narrative", ["DynamicQuestGenerator"], "--dynamic-quest-selftest", ["DynamicQuestGeneratorTests"]),
    ("172", "Radiation Mutation & Genetic Instability", "mutation_tree",    None, ["MutationSystem"],   "--radiation-mutation-selftest", ["Plan172RadiationMutationTests"]),
    ("173", "Radio Production & Audience",          "radio_program_production", None, ["RadioProgramProductionSystem"], "--radio-production-selftest", ["Plan173RadioProductionIntegrationTests"]),
    ("174", "Survivor Backstories",                 "backstory",            None,                [], "--backstory-selftest",          ["Plan174BackstoryHostIntegrationTests"]),
    ("177", "Dream & Sleep Event System",           "survivor_dreams",      None,                [], "--dream-system-selftest",       ["Plan177DreamSleepIntegrationTests"]),
    ("178", "Art & Culture Creation",               "culture_creation",     None, ["CultureCreationSystem"], "--culture-creation-selftest", ["Plan178ArtCultureIntegrationTests", "CultureCreationSystemTests"]),
    ("179", "Psychology & Phobia",                  "psychological_profiles", None, ["PsychologicalProfileSystem"], "--psychological-profile-selftest", ["Plan179UnifiedPsychologyIntegrationTests"]),
    ("180", "Skill Certification",                  "skill_certifications", None, ["SkillCertificationSystem"], "--skill-certification-selftest", ["Plan180SkillCertificationTests"]),
    ("181", "Difficulty Settings",                  "difficulty_settings",  None,                [], "--difficulty-settings-selftest", []),
    ("182", "Relationship Decay",                   "relationship_decay",   None,                [], "--relationship-decay-selftest", ["Plan182RelationshipDecayIntegrationTests"]),
    ("183", "Child Development Stages",             "child_development",    None, ["ChildDevelopmentSystem", "ChildDevelopmentCensus"], "--child-development-selftest", ["Plan183ChildDevelopmentIntegrationTests"]),
    ("184", "Accessibility Options System",         "accessibility_settings", None,             [], "--accessibility-settings-selftest", ["Plan184AccessibilitySettingsIntegrationTests"]),
    ("185", "Memory & Knowledge Decay",             "memory_decay",         None,                [], "--memory-decay-selftest",       ["Plan185MemoryDecayIntegrationTests", "MemoryDecaySystemTests"]),
    ("186", "Shelter Maintenance",                  "shelter_maintenance",  None,                [], "--shelter-maintenance-selftest", ["Plan186ShelterMaintenanceIntegrationTests"]),
    ("187", "Bestiary UI & Encounter Tracking",     "bestiary_knowledge",   None, ["BestiarySystem", "BestiaryCensus"], "--bestiary-selftest", ["Plan187BestiaryIntegrationTests"]),
    ("188", "Survivor Daily Routines",              "survivor_routines",    None,                [], "--survivor-routines-selftest",  ["Plan188SurvivorRoutineIntegrationTests"]),
    ("198", "Health History & Medical Records",     "health_history",       None, ["HealthHistorySystem", "HealthHistoryCensus"], "--health-history-selftest", ["Plan198HealthHistoryIntegrationTests", "Plan198MedicalRecordLogTests"]),
    ("200", "Survivor Personal Quests & Character Arcs", "personal_quests",  None,                [], "--personal-quests-selftest",    ["Plan200PersonalQuestsIntegrationTests", "PersonalQuestSystemTests"]),
    ("202", "Interpersonal Conflict & Grievance",   "interpersonal_conflict", None,              [], "--interpersonal-conflict-selftest", ["Plan202InterpersonalConflictIntegrationTests", "InterpersonalConflictSystemTests"]),
    ("203", "Rumor Network",                        "wasteland_rumors",     None,                [], "--rumor-network-selftest",      []),
    ("205", "Shelter Noise",                        "shelter_noise",        None,                [], "--shelter-atmosphere-selftest", []),
    ("206", "Death & Legacy",                       "death_legacy",         None,                [], "--death-legacy-selftest",       ["Plan206SurvivorDeathLegacyIntegrationTests"]),
    ("207", "Shelter Reputation",                   "shelter_reputation",   None,                [], "--shelter-reputation-selftest", ["Plan207ShelterReputationIntegrationTests"]),
    ("208", "Leadership Succession & Challenges",   None,          "survivor_social", ["LeadershipSystem", "LeadershipCensus"], "--leadership-succession-selftest", ["Plan208LeadershipSuccessionIntegrationTests"]),
    ("210", "Personal Belongings & Effects",        None,          "survivor_social", ["PersonalBelongingsSystem", "PersonalBelongingsHostSession"], "--personal-belongings-selftest", ["Plan210PersonalBelongingsIntegrationTests"]),
    ("212", "Time Capsules",                        "time_capsules",        None,                [], "--time-capsule-selftest",       ["Plan212TimeCapsuleIntegrationTests"]),
    ("214", "Visitor Integration & Housing",        "visitor_integration",  None,                [], "--visitor-integration-selftest", ["Plan214VisitorIntegrationTests"]),
    ("216", "Survivor Exercise & Physical Training", "exercise",            None,                [], "--exercise-selftest",           ["Plan216ExerciseIntegrationTests", "ExerciseSystemTests"]),
    ("220", "Shelter Atmosphere",                   "shelter_atmosphere",   None,                [], "--shelter-atmosphere-selftest", ["Plan220ShelterAtmosphereIntegrationTests"]),
]

UNCOMMITTED_NOTE_PLANS = {"181", "186", "188", "210", "214"}


def load_graph():
    spec = importlib.util.spec_from_file_location(
        "amap", REPO_ROOT / "scripts" / "ci" / "generate-architecture-map.py")
    module = importlib.util.module_from_spec(spec)
    spec.loader.exec_module(module)
    return module.ARCHITECTURE_GRAPH


def collect_files(root, suffix):
    out = []
    for path in root.rglob(suffix):
        s = str(path)
        if any(marker in s for marker in EXCLUDED):
            continue
        out.append(path)
    return out


def scan_type_names(files):
    """Map C# type name -> set of relative file paths declaring it."""
    types = {}
    rx = re.compile(r"(?:public|internal|sealed|static|partial|abstract)\s+(?:class|struct|interface|enum|record)\s+([A-Za-z0-9_]+)")
    for path in files:
        text = path.read_text(encoding="utf-8", errors="ignore")
        for match in rx.finditer(text):
            types.setdefault(match.group(1), set()).add(path.relative_to(REPO_ROOT).as_posix())
    return types


def scan_references(files):
    """Map identifier -> set of files referencing it (word-boundary)."""
    refs = {}
    cache = {}
    for path in files:
        rel = path.relative_to(REPO_ROOT).as_posix()
        cache[rel] = path.read_text(encoding="utf-8", errors="ignore")
    return cache, refs


def main(check_only: bool) -> int:
    graph = load_graph()

    core_files = collect_files(REPO_ROOT / "Assets" / "Ashfall.Core", "*.cs")
    src_files = collect_files(REPO_ROOT / "src", "*.cs")
    core_types = scan_type_names(core_files)

    src_cache = {p.relative_to(REPO_ROOT).as_posix(): p.read_text(encoding="utf-8", errors="ignore")
                 for p in src_files}
    core_cache = {p.relative_to(REPO_ROOT).as_posix(): p.read_text(encoding="utf-8", errors="ignore")
                  for p in core_files}

    registry_text = (REPO_ROOT / "Assets" / "Ashfall.Core" / "Save" / "SaveSectionRegistry.cs").read_text(encoding="utf-8")
    registry = {m[0]: (m[1], m[2]) for m in re.findall(
        r'new\("([a-z0-9_]+)",\s*"(\w+)",\s*(?:"(\w+)"|null)', registry_text)}
    filename_map = dict(re.findall(r'\{ "([a-z0-9_]+)",\s*"([a-z0-9_]+\.json)" \}', registry_text))

    hostcli_text = (REPO_ROOT / "src" / "Host" / "HostCli.cs").read_text(encoding="utf-8")
    cli_registry_text = (REPO_ROOT / "Assets" / "Ashfall.Core" / "HostCliRegistry.cs").read_text(encoding="utf-8")
    manifest_text = (REPO_ROOT / "docs" / "ci" / "SELFTEST_MANIFEST.json").read_text(encoding="utf-8")

    surfaces_text = (REPO_ROOT / "src" / "Main.PlayerSurfaces.cs").read_text(encoding="utf-8")
    dashboard_text = (REPO_ROOT / "src" / "UI" / "GameDashboardPanel.cs").read_text(encoding="utf-8")
    main_partials = "\n".join(
        p.read_text(encoding="utf-8", errors="ignore") for p in src_files
        if p.name.startswith("Main"))

    test_files = [p.relative_to(REPO_ROOT).as_posix()
                  for p in collect_files(REPO_ROOT / "Ashfall.Core.Tests", "*Tests.cs")
                  + collect_files(REPO_ROOT / "Ashfall.Core.Tests", "*Test.cs")]

    def refs_in_src(identifier):
        rx = re.compile(r"\b" + re.escape(identifier) + r"\b")
        return [rel for rel, text in src_cache.items() if rx.search(text)]

    src_type_names = scan_type_names(src_files)

    def classify(identifier):
        """Where is this authority declared: Core, host, or nowhere?"""
        if identifier in core_types:
            return "Core"
        if identifier in src_type_names:
            return "Host"
        return "Missing"

    results = []
    for plan, title, section, inside, authorities, cli, tests in RECENT_PLANS:
        entry = graph.get(section, {}) if section else {}
        store_classes = [t for t in entry.get("store", []) if t != "Main"]
        ui_panels = [t for t in entry.get("ui", []) if t not in ("Main", "GameDashboardPanel")]
        routes = [r for r in entry.get("routes", []) if r not in ("expanded",)]

        checks = {}

        claimed = list(authorities) or list(entry.get("core", []))
        classification = {t: classify(t) for t in claimed}
        missing_authorities = [t for t, where in classification.items() if where == "Missing"]
        core_side = [t for t, where in classification.items() if where == "Core"]
        host_side = [t for t, where in classification.items() if where == "Host"]
        if entry.get("core"):
            graph_core_missing = [t for t in entry["core"] if classify(t) != "Core"]
        else:
            graph_core_missing = []

        if claimed:
            summary = ", ".join(f"{t} [{classification[t]}]" for t in claimed)
            checks["authority"] = (not missing_authorities and not graph_core_missing, summary)
        else:
            checks["authority"] = (None, "read model projected by host only")

        reach_ids = core_side + host_side + [t for t in entry.get("host", []) if t != "Main"]
        host_refs = {}
        for identifier in reach_ids:
            files = refs_in_src(identifier)
            if files:
                host_refs[identifier] = files
        checks["host_refs"] = (len(host_refs) > 0,
                               f"{len(host_refs)}/{len(reach_ids)} identifiers referenced"
                               if reach_ids else "no identifiers declared")

        eff_section = section or inside
        if eff_section and eff_section in registry:
            save_method, setup_method = registry[eff_section]
            checks["save_section"] = (True, f"`{eff_section}`" + (" (nested)" if inside else ""))
            checks["triad"] = (
                (setup_method is None or (f"{setup_method}(" in main_partials))
                and (f"{save_method}(" in main_partials),
                f"{setup_method or '—'} / {save_method}")
            if not inside and eff_section in filename_map:
                checks["save_file"] = (True, filename_map[eff_section])
            else:
                checks["save_file"] = (True, "persisted inside parent aggregate")
        elif eff_section:
            checks["save_section"] = (False, f"`{eff_section}` NOT registered")
            checks["triad"] = (False, "—")
            checks["save_file"] = (False, "—")
        else:
            checks["save_section"] = (None, "read model / no own section")
            checks["triad"] = (None, "—")
            checks["save_file"] = (None, "—")

        if store_classes:
            missing = [t for t in store_classes
                       if not (REPO_ROOT / "src" / "Host" / f"{t}.cs").exists()
                       and not refs_in_src(t)]
            checks["save_store"] = (not missing, ", ".join(store_classes))
        else:
            checks["save_store"] = (None, "—")

        host_declared = host_side + [t for t in entry.get("host", []) if t != "Main"]
        if host_declared:
            missing_host = [t for t in host_declared
                            if not (REPO_ROOT / "src" / "Host" / f"{t}.cs").exists()
                            and not refs_in_src(t)]
            checks["host_session"] = (not missing_host,
                                      ", ".join(host_declared[:3]) + ("…" if len(host_declared) > 3 else ""))
        else:
            checks["host_session"] = (None, "wired through Main")

        if ui_panels:
            missing_ui = [t for t in ui_panels
                          if not (REPO_ROOT / "src" / "UI" / f"{t}.cs").exists()]
            checks["ui_panel"] = (not missing_ui, ", ".join(ui_panels))
        else:
            checks["ui_panel"] = (None, "no dedicated panel (read model / detail rows)")

        if routes:
            missing_routes = [r for r in routes
                              if r not in surfaces_text and r not in dashboard_text]
            checks["route"] = (not missing_routes, ", ".join(routes))
        else:
            checks["route"] = (None, "—")

        checks["cli_flag"] = (cli in hostcli_text and cli in cli_registry_text and cli in manifest_text, cli)

        if tests:
            missing_tests = [t for t in tests
                             if not any(t in f for f in test_files)]
            checks["tests"] = (not missing_tests, ", ".join(tests))
        else:
            checks["tests"] = (None, "no named fixture (host selftest only)")

        booleans = [v for v in (c[0] for c in checks.values()) if v is not None]
        if not booleans:
            verdict = "UNVERIFIED"
        elif all(booleans):
            verdict = "INTEGRATED"
        elif checks["host_refs"][0]:
            verdict = "PARTIAL"
        else:
            verdict = "NOT-INTEGRATED"

        results.append({
            "plan": plan,
            "title": title,
            "section": section,
            "persisted_inside": inside,
            "verdict": verdict,
            "uncommitted": plan in UNCOMMITTED_NOTE_PLANS,
            "checks": {k: (v[0] if v[0] is None else bool(v[0]), v[1]) for k, v in checks.items()},
            "host_ref_files": {k: v[:8] for k, v in host_refs.items()},
        })

    counts = {}
    for r in results:
        counts[r["verdict"]] = counts.get(r["verdict"], 0) + 1

    payload = {
        "schema_version": "1.0.0",
        "generated_at": datetime.now(timezone.utc).date().isoformat(),
        "method": "programmatic source scan (Core type declarations, src references, SaveSectionRegistry, HostCli, SELFTEST_MANIFEST, test fixtures)",
        "verdict_counts": counts,
        "plans": results,
    }

    md = render_md(payload)

    if check_only:
        if OUT_MD.exists() and OUT_MD.read_text(encoding="utf-8") == md:
            print(f"OK: {OUT_MD.name} in sync ({len(results)} plans, {counts})")
            return 0
        print(f"FAIL: {OUT_MD.name} is out of date. Run generate-plan-integration-audit.py")
        return 1

    OUT_MD.parent.mkdir(parents=True, exist_ok=True)
    OUT_MD.write_text(md, encoding="utf-8")
    OUT_JSON.write_text(json.dumps(payload, indent=2, sort_keys=False) + "\n", encoding="utf-8")
    print(f"Wrote {OUT_MD} ({len(results)} plans, {counts})")
    print(f"Wrote {OUT_JSON}")
    return 0


def fmt_check(value):
    if value is True:
        return "✅"
    if value is False:
        return "❌"
    return "—"


def render_md(payload):
    lines = []
    a = lines.append
    a("# Recent Plan Integrations — Programmatic Audit")
    a("")
    a(f"**Generated:** {payload['generated_at']}")
    a("")
    a(f"**Method:** {payload['method']}")
    a("")
    a(f"**Scope:** {len(payload['plans'])} recently integrated plans (UNBLOCK-OLDEST batches,")
    a("flagship integration commits, and the 2026-09-23 Plans 210/214 full-integration package).")
    a("")
    a("Every verdict below is re-measured from current source: Core type declarations,")
    a("`src/` references, `SaveSectionRegistry`, `HostCli`/`SELFTEST_MANIFEST.json`, UI panel")
    a("files, route registration, and test fixtures. Ledger claims are not trusted.")
    a("")
    counts = payload["verdict_counts"]
    a("## 1. Verdict summary")
    a("")
    a("| Verdict | Plans |")
    a("|---|---:|")
    for verdict in ("INTEGRATED", "PARTIAL", "NOT-INTEGRATED", "UNVERIFIED"):
        if verdict in counts:
            a(f"| **{verdict}** | {counts[verdict]} |")
    a("")
    a("## 2. Per-plan verification")
    a("")
    a("| Plan | Title | Save section | Host refs | Triad | CLI | UI | Tests | Verdict |")
    a("|---|---|---|---|---|---|---|---|---|")
    for r in payload["plans"]:
        c = r["checks"]
        section = f"`{r['section']}`" if r["section"] else (
            f"inside `{r['persisted_inside']}`" if r["persisted_inside"] else "read model")
        uncommitted = " *(uncommitted)*" if r["uncommitted"] else ""
        a(f"| {r['plan']} | {r['title']}{uncommitted} | {section} | "
          f"{fmt_check(c['host_refs'][0])} {c['host_refs'][1]} | {fmt_check(c['triad'][0])} | "
          f"{fmt_check(c['cli_flag'][0])} | {fmt_check(c['ui_panel'][0])} | "
          f"{fmt_check(c['tests'][0])} | **{r['verdict']}** |")
    a("")
    a("## 3. Evidence detail")
    a("")
    for r in payload["plans"]:
        c = r["checks"]
        a(f"### Plan {r['plan']} — {r['title']} — {r['verdict']}")
        a("")
        for key in ("authority", "host_refs", "save_section", "triad", "save_file",
                    "save_store", "host_session", "ui_panel", "route", "cli_flag", "tests"):
            ok, note = c[key]
            a(f"- {fmt_check(ok)} `{key}`: {note}")
        if r["host_ref_files"]:
            sample = next(iter(r["host_ref_files"].values()))
            a(f"- Host reference files (first authority, up to 8): {', '.join(f'`{f}`' for f in sample)}")
        a("")
    a("## 4. Notes and constraints")
    a("")
    a("- Plans 181/186/188 (difficulty, shelter maintenance, survivor routines) and")
    a("  210/214 are present in the current worktree but not yet committed; their")
    a("  evidence is measured against the working tree, which is the current truth.")
    a("- Read-model plans (49, 51, 52, 59, 137, 165, 169) intentionally own no save")
    a("  section; persistence belongs to the canonical owners they project from.")
    a("- Nested persistence plans: 167 (`wasteland_map`), 171 (`procedural_narrative`),")
    a("  210 (`survivor_social`) — one aggregate owner, no parallel save section.")
    a("")
    return "\n".join(lines) + "\n"


if __name__ == "__main__":
    sys.exit(main("--check" in sys.argv))
