#!/usr/bin/env python3
"""Rebuild the 2026-09-24 ten-document integration expansion layer.

Run after the original appendix generators. The original material is preserved
up to the expansion marker. This generator writes documentation only; it makes
no claim about production integration or test success.
"""

from __future__ import annotations

import json
from itertools import product
from pathlib import Path
from textwrap import dedent

ROOT = Path(__file__).resolve().parents[5]
MARKER = "# 2026-09-24 evidence-grounded integration expansion"
MASTER = "docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md"

INTERRUPTIONS = [
    ("first encounter", "the owner has no prior record for this subject", "default state and catalog bootstrap"),
    ("legacy restore", "the save predates an additive field", "old-save default and checksum boundary"),
    ("duplicate request", "the command is submitted twice before a day tick", "idempotent transition or typed refusal"),
    ("late day", "the action arrives at the boundary between two campaign days", "day source and tick order"),
    ("unknown authored ID", "a referenced catalog ID is absent", "loader validation and safe fallback"),
    ("missing participant", "the actor or target is no longer available", "living roster or canonical entity lookup"),
    ("scarce input", "the canonical owner reports less stock than the preview", "atomic cost validation"),
    ("interrupted power", "the operating facility loses available power", "power owner projection"),
    ("route not opened", "the player has not unlocked the required route", "reachability and authorization"),
    ("panel reopened", "the player closes and reopens the relevant surface", "binding, refresh, and disposal"),
    ("same seed replay", "two campaigns use identical seed and input order", "deterministic output and event order"),
    ("neighboring event", "another owner publishes a fact in the same day phase", "one-way adapter ordering"),
    ("past story window", "the player reaches the scene after its intended campaign band", "explicit availability rule"),
    ("empty roster", "no living eligible participant is present", "zero-count behavior"),
    ("dense roster", "many valid participants expose iteration order", "stable sorted processing"),
    ("conflicting command", "another command requests an incompatible transition", "single authority and refusal reason"),
    ("bad identifier", "the request contains an empty or wrong-domain ID", "input guard and no-op"),
    ("failed capture", "save capture fails after a successful Core action", "dirty state and retry semantics"),
    ("restore after fact", "the campaign reloads just after publishing an event", "exactly-once consumer behavior"),
    ("controller path", "the same operation uses controller focus and back", "accessible route parity"),
]

SPECS = [
    dict(path="docs/content/plan121/INDEPENDENT_BRANCH_8_BASELINE_PARITY.md", name="Independent Branch", core="FactionBranchCoordinator.Independent / IndependentBranchSystem", host="FactionBranchHostSession and Main.FactionBranch.cs", save="weight_of_choices", catalog="independent_faction_branch.json", key="branches", api="IndependentBranchSystem.CommitBranch, LockPointOfNoReturn, ResolveEnding, CaptureState, RestoreState", status="The eight-baseline parity note is historical. The current catalog has fifteen branches; the first eight and their endings require fresh parity proof. Host integration is indirect through FactionBranchCoordinator.", first="one existing Independent branch from Factions/Quests panel through commitment, flag, save, and ending preview", boundary="Military and Rebel remain mutually exclusive branch authorities; standing is not a private Independent ledger", cluster="C7 and C13", object="an unendorsed form", scenes=["entry-band preview","uncommitted meeting","neutral vote","Military standing report","Rebel standing report","moral flag review","branch commitment","point of no return","faction pressure","route refusal","ending preview","old save return","final audience","unclaimed testimony","quiet withdrawal","chronicle entry"]),
    dict(path="docs/factions/MILITARY_BRANCH_RUNTIME_CONTRACT.md", name="Military Branch", core="FactionBranchCoordinator.Military / MilitaryBranchSystem", host="FactionBranchHostSession and Main.FactionBranch.cs", save="weight_of_choices", catalog="military_faction_branch.json", key="branches", api="MilitaryBranchSystem.CommitBranch, ShiftFactionAlignment, LockPointOfNoReturn, ResolveEnding, CaptureState", status="The current catalog has fifteen Military branches, and the host commits through the coordinator. Preserve the historical runtime contract while checking current predicates and panel reachability.", first="one Military commitment with a lawful moral flag and visible faction branch state", boundary="FactionBranchCoordinator owns branch exclusivity; FactionStanceEngine owns external standing", cluster="C7 and C13", object="a stamped order", scenes=["recruitment desk","branch briefing","alignment hearing","supply request","chain of command","competing Rebel request","civilian objection","mission refusal","point of no return","interrupted orders","rank dispute","field return","ending evidence","archive hearing","public communiqué","final muster"]),
    dict(path="docs/plans/PLAN_123_REBEL_BRANCH_IMPLEMENTATION_LOG.md", name="Rebel Branch", core="FactionBranchCoordinator.Rebel / RebelBranchSystem", host="FactionBranchHostSession and Main.FactionBranch.cs", save="weight_of_choices", catalog="rebel_faction_branch.json", key="branches", api="RebelBranchSystem.CommitBranch, ShiftFactionAlignment, LockPointOfNoReturn, ResolveEnding, CaptureState", status="The implementation log records dated work; current source has RebelBranchSystem and coordinator persistence. The log's old collision observations must be rechecked before being treated as live defects.", first="one Rebel branch commitment from a current player route to persisted flag and visible ending eligibility", boundary="No parallel rebel standing ledger or duplicate Defector identity; coordinator remains the branch gate", cluster="C7 and C13", object="a folded petition", scenes=["cell meeting","courier arrival","supply appeal","defector interview","Militia dispute","covert signal","branch briefing","commitment vote","point of no return","exposed safehouse","civilian shelter","counteroffer","ending testimony","leader absence","night departure","chronicle page"]),
    dict(path="docs/expeditions/PLAN_125_AMPHIBIOUS_DRAISINE_CLOSEOUT.md", name="Draisine Recovery", core="DraisineRerailingSystem; ArmoredDraisineRecoverySystem is a subclass", host="DraisineRerailingHostSession and Main.Plans130_133.cs", save="draisine_recovery", catalog="rerailing_equipment_catalog.json", key="equipment", api="DraisineRerailingSystem.StartRecovery, TickDay, Abandon, CaptureState, RestoreState", status="The Plan 125 closeout concerns amphibious crossing. Recovery is a separate base system with a hosted save; the named ArmoredDraisineRecoverySystem subclass has no direct host reference. Do not collapse crossing and rerailing state.", first="one armored-draisine recovery request through the base recovery host with equipment, power, duration, and rail state reconciled", boundary="RailwaySystem owns track integrity; inventory owns parts; vehicle owner owns condition; amphibious kit owner is separate", cluster="C5 and C6", object="a rail jack", scenes=["derailment notice","equipment inspection","track survey","armored consist","power availability","parts issue","crew assignment","first recovery day","weather interruption","rail alignment","failed lift","abandonment","vehicle return","route reopening","crossing distinction","maintenance record"]),
    dict(path="docs/survivors/PLAN_182_RELATIONSHIP_DRIFT_AUTHORITY_MAP.md", name="Trauma Bond", core="SurvivorSocialCoordinator.TraumaBond / TraumaBondSystem", host="SurvivorSocial host and Main.SurvivorSocial.cs", save="survivor_social", catalog="none; hazard facts and survivor IDs are current inputs", key="", api="TraumaBondSystem.OnSharedHazardEndured, Tick, GetBondStrength, GetCoShiftEfficiencyBonus, CaptureState", status="The signed 2026-09-12 authority map predates the later lastInteractionDay seal. TraumaBondSystem is already owned by SurvivorSocialCoordinator and stored in survivor_social; do not reopen a second affinity or decay system.", first="one real shared hazard fact reaches the social coordinator and produces a visible bond without fabricating an affinity event", boundary="SurvivorRelationsSystem owns pair affinity; TraumaBondSystem owns shared-hazard bond strength", cluster="C9", object="two names on a hazard roll", scenes=["shared storm shelter","raid aftermath","collapsed passage","starvation day","infirmary vigil","same shift","different shift","bond formation","affinity publication","daily bond decay","survivor death","new assignment","relationship panel","memorial return","old save","quiet reconciliation"]),
    dict(path="docs/plans/EXPANSION_PROGRAM_WAVE3_2026-09-21/PLAN-TRANSPORT-EXPEDITION-30_APPENDIX-A_ORPHAN_DOSSIERS.md", name="Aerial Recon Window", core="AerialReconWindowEngine.Evaluate (stateless)", host="existing expedition/aviation mission dispatch, to be identified at promotion", save="none for the pure evaluation; mission/airworthiness state belongs to current aircraft and expedition owners", catalog="none by exact name; flight and weather inputs come from current owners", key="", api="AerialReconWindowEngine.Evaluate and FlightWindowEvaluationResult", status="The 2026-09-21 orphan dossier reports no host reference for this exact static evaluator. It has no mutable state and does not need its own save or day owner.", first="one launch preview that samples current weather and aircraft state, runs Evaluate once, and blocks grounded sorties", boundary="Weather owns wind/visibility/temperature; aircraft owns airworthiness and payload; expedition owner commits launch", cluster="C5 and C12", object="a grease-pencil flight card", scenes=["clear launch","crosswind","ash haze","icing band","dry cold","heavy payload","worn airframe","grounded runway","marginal range","sensor run","airdrop drift","route return","forecast change","aborted sortie","late signal","pilot briefing"]),
    dict(path="docs/plans/EXPANSION_PROGRAM_WAVE5_2026-09-21/PLAN-CRIME-SYNDICATES-44_APPENDIX-A_ORPHAN_DOSSIERS.md", name="Chit Purity Assay", core="ChitPurityAssayEngine.EvaluateAssay and CertifyDilutedScrap (stateless)", host="existing trade settlement and funds authority, to be selected after a funds-leg audit", save="none for the pure assay; trade/funds/heat state remains with existing owners", catalog="no exact assay catalog; purity tiers are current Core enum values", key="", api="ChitPurityAssayEngine.GetTierParameters, EvaluateAssay, CertifyDilutedScrap", status="The orphan dossier flags an uncalled static engine. Its outputs propose accepted, confiscated, trust and heat quantities; no host application of those outputs was found. F13 funds opt-in and FundsLedger now exist, but the exact assay settlement leg still needs a current owner audit before wiring.", first="one assay preview on a real transaction with an explicit settlement decision and no shadow currency store", boundary="FundsLedger owns money; trade settlement owns exchange; BlackMarketHeatAttentionEngine and standing owners apply only their respective effects", cluster="C11", object="a scored alloy chit", scenes=["mint-pristine tender","standard alloy","diluted scrap","counterfeit lead","merchant assay","quiet rejection","discounted payment","confiscation claim","assay office","certification fee","small denomination","large payment","trust response","heat report","receipt correction","return to market"]),
    dict(path="docs/plans/EXPANSION_PROGRAM_WAVE13_2026-09-21/PLAN-WEATHER-SONDE-TRUTH-168_APPENDIX-A_SCAFFOLD.md", name="Cloud Seeding", core="WeatherIntelligenceCoordinator.CloudSeeding / CloudSeedingSystem", host="WorldHostSession plus WeatherForecastPanel; route must move mutation through host command", save="WorldSaveStore weather intelligence state already contains cloudSeeding", catalog="no name-matched catalog; weather and research/inventory owners supply inputs", key="", api="CloudSeedingSystem.Install, PreflightDeploy, Deploy, TickDay, CaptureState, RestoreState", status="The generated scaffold's no-host-candidate premise is stale: WorldHostSession owns WeatherIntelligenceCoordinator, and WeatherForecastPanel directly calls cloud-seeding methods. The current coordinator save includes cloudSeeding state.", first="one host-owned preflight/deploy command with real material cost and a truthful forecast panel refresh", boundary="WeatherSystem owns weather; inventory owns canisters; research owns knowledge; panel does not commit gameplay", cluster="C12", object="a sealed seeding canister", scenes=["station calibration","device installation","research gate","storm forecast","canister inventory","deployment order","failed dispersal","partial protection","seven-day cooldown","weather update","night forecast","stale preview","panel return","save restore","target day","storm aftermath"]),
    dict(path="docs/plans/EXPANSION_PROGRAM_WAVE6_2026-09-21/PLAN-LABOUR-PROFESSIONS-68_APPENDIX-A_SCAFFOLD.md", name="Duty Roster Chart", core="DutyRosterSystem and its internal DutyRosterChartEngine", host="DutyRosterHostSession and Main.DutyRoster.cs", save="duty_roster", catalog="duty_roster_quests.json and duty_roster_seasons.json", key="quests", api="DutyRosterSystem.WriteName, EraseName, TickMorning, ResolveChartChoice, ResolveLadleChoice, ResolveInkEnding", status="The generated scaffold proposes new Setup/Save method names, but Main.DutyRoster.cs already owns those methods. DutyRosterChartEngine is internal to DutyRosterSystem and has no independent host/save authority.", first="one chart write/erase choice with a real roster row, morning tick, and truthful panel feedback", boundary="DutyRosterSystem owns chart rows and role assignment; survivor roster owns living identity; no new chart save section", cluster="C9 and C1", object="a scored paper chart", scenes=["blank row","first written name","erased name","morning readout","status annotation","scripted row","ladle choice","ink ending","burned chart","visitor interruption","overflow notice","second winter","fitness warning","empty shelter","public roster","archived slate"]),
    dict(path="docs/expansions/wave6/expansion_38_the_ward_plan.md", name="Lyophilization", core="LyophilizationSystem; LyophilizationEngine is a compatibility subclass", host="LyophilizationHostSession and Main.Plans130_133.cs", save="lyophilization", catalog="lyophilization_catalog.json", key="recipes", api="LyophilizationSystem.StartBatch, TickDay, CanUseBatch, TryUseBatch, RegisterMedicalProtocol, CaptureState", status="The Ward design bible is pre-integration, but the base LyophilizationSystem already has a host, catalog, panel and save. The named LyophilizationEngine subclass has no direct host reference. Ward integration should consume the base owner rather than create a second batch ledger.", first="one existing recipe from inventory input to drying, expiry, and ward protocol use", boundary="Inventory owns physical items; power owns supply; medical pipeline owns patient care; LyophilizationSystem owns batch viability", cluster="C2 and C3", object="a dated glass vial", scenes=["recipe selection","input issue","container check","vacuum start","power loss","drying day","batch completion","viability assay","cold shelf","expiry review","ward request","protocol registration","partial use","spoiled lot","restored batch","patient chart"]),
]


def authored_rows(spec: dict) -> list[dict]:
    if not spec["key"]:
        return []
    path = ROOT / "Assets/StreamingAssets/Data" / spec["catalog"]
    if not path.exists():
        return []
    data = json.loads(path.read_text())
    return data.get(spec["key"], []) if isinstance(data, dict) else []


def row_id(row: dict) -> str:
    for key in ("id", "branch_id", "equipment_id", "recipe_id"):
        if row.get(key):
            return str(row[key])
    return "unknown_row"


def intro(spec: dict, rows: list[dict]) -> str:
    persistence_arrow = ("caller-owned mission, trade, and funds state; no evaluator save/day owner"
                         if "pure" in spec["save"] else f"existing save and day owner ({spec['save']})")
    inventory = ""
    if rows:
        inventory = "\n## Authored-row reachability register\n\n"
        inventory += "This list is read from current JSON, not invented content. Each row needs a loader, a predicate, a player route, and an observable consequence before it counts as integrated.\n\n"
        inventory += "| Existing ID | Current data fields | Integration review |\n|---|---|---|\n"
        for row in rows:
            fields = ", ".join(f"`{k}`" for k in list(row)[:8])
            inventory += f"| `{row_id(row)}` | {fields} | Check exact loader, availability, ending or output consumer, and save round trip. |\n"
    return dedent(f"""

---

{MARKER}

> **Generator chain:** original dated document or appendix, then
> `docs/plans/EXPANSION_PROGRAM_2026-09-21/tools/generators/gen_requested_ten_integration_expansions.py`,
> then the separate 2026-09-24 editorial polish pass. A historical statement
> above is not revalidated merely because this addendum follows it.

## Outcome, evidence, and document role

**{spec['name']}** is a planning subject in the master's {spec['cluster']} cluster. The source of expansion ideas is `{MASTER}`: it asks for data-backed narrative depth, one existing Core authority, a host route, persistence where state changes, and player-observable outcomes. This document is a reviewable integration plan, not a production claim or a declaration of runtime completeness.

**Current premise:** {spec['status']}

**First bounded outcome:** {spec['first']}. This is the first package to promote after the current premise is rechecked. Other cases in the register are candidate acceptance probes and narrative directions; they are not simultaneous build requirements.

**One-authority boundary:** {spec['boundary']}. Core source: `{spec['core']}`. Candidate host: `{spec['host']}`. Persistence: `{spec['save']}`. Authored data: `{spec['catalog']}`. Any proposed additional field must be justified by a real save-round-trip failure, and any proposed new catalog must first pass a live ID collision and consumer audit.

## Integration architecture

```text
Current authored JSON / real owner inputs
    -> {spec['core']}
    -> {spec['host']}
    -> {persistence_arrow}
    -> read-only panel, journal, or campaign projection
```

The arrow order is contractual: the host asks an owner for current facts, validates one command, commits at most once, applies only cross-owner effects assigned to that adapter, captures the existing section, and refreshes the player view. A panel never owns a second gameplay counter. A catalog row does not become playable until an actual caller consumes it. A failed request cannot publish a success event, spend stock, change standing, or claim an ending. For a pure evaluator the “commit” stage remains in the caller's owner and the evaluator needs no save of its own.

## Current file and API checkpoints

| Layer | Source or symbol to inspect before implementation |
|---|---|
| Core owner | `{spec['core']}` |
| Public methods | `{spec['api']}` |
| Host | `{spec['host']}` |
| Save | `{spec['save']}` |
| Catalog or input | `{spec['catalog']}` |
| Master expansion source | `{MASTER}` |

These are checkpoints, not substitute citations for a future code change. Check exact signatures and current callers in the source file at promotion time. The master explicitly labels inferred seams and bars bulk unverified canon; therefore candidate prose below must be accepted only when its trigger and reader exist.

## Dependency-ordered package

**Phase 0 — prove the premise.** Search direct and indirect references, constructor and bind sites, save capture/restore, catalog load, day-owner registration, and UI callback. Compare the dated document above with current source. Resolve a stale premise in writing before claiming files. Identify any live owner whose state would be affected and do not begin if the ownership map is ambiguous.

**Phase 1 — specify one command or query.** Choose the first bounded outcome above. Define the exact input record ID, actor, current day, result type, refusal codes, and old-save fallback. For a static Core evaluator, identify the host or presentation caller and show that the result does not silently mutate other owners. For a stateful owner, require an idempotent transition and a capture path.

**Phase 2 — attach the host.** Route through the existing session where one exists. Sample canonical inputs once, publish a typed fact only on success, and send each resulting delta to its current owner. Do not let the panel invoke low-level Core mutation directly when a host command must validate inventory, flags, route access, or save dirty state.

**Phase 3 — make authored content reachable.** Use current IDs and loaders. Add small JSON cohorts only after a usage census. A candidate line needs a known trigger, a specific speaker or document genre, a visible destination, and a fallback. No old test count, historical closeout, or catalog presence alone proves this phase.

**Phase 4 — persist and present.** Restore before binding UI. If the owner is stateless, document that no new section is needed. If it is stateful, capture into the existing registered section, default old fields safely, and prevent duplicate day effects after reload. Show current status, eligibility, cost, and refusal reason with keyboard/controller focus and close/back parity.

**Phase 5 — focused acceptance.** A future implementation package uses the smallest relevant Core fixture under `TEST_POLICY.md`, one save round trip for changed state, paired seed replay where selection is random, a host-route observation, and a headless Godot check if the runtime surface changes. This documentation generator runs no tests and makes no test-pass claim.

## C# integration framework — contract sketch

```csharp
// Illustrative application contract. Bind to the current methods listed above;
// do not add these types unless Phase 0 proves the host needs them.
public readonly record struct FeatureRequest(
    string ContentId, string ActorId, int CampaignDay, string RequestKey);
public readonly record struct FeatureReadModel(
    bool Available, string RefusalCode, string CurrentState,
    string CostText, string ConsequenceText);
// Host: obtain current owner inputs -> validate -> call the existing Core API.
// Success: apply authorized effects once, capture the existing section, refresh.
// Refusal: no mutation, no cost, no success event, but show a reason.
// Core cannot reference Godot; UI cannot hold independent mutable authority.
```

`RequestKey` is a proposal for any route that genuinely needs duplicate-request defense. Prefer current state guards and persisted event keys. The sketch is coding language for the handoff, not a drop-in patch or a claim that the current owner exposes these signatures. Exact current APIs are in the checkpoint table and must be used by the implementation package.

## Prose and content discipline

The master expansion document's register is precise, restrained, and grounded in physical evidence. {spec['object'].capitalize()} may anchor a line, but the line must never imply an outcome the owning system did not report. Technical records should name a measurement or reason code; an intimate record should name what its speaker can know. One event should not grant the same reward through both a quest flag and an independent host callback. For every proposed paragraph, write its trigger, consumer, fallback, and continuity constraint before the prose itself. After acceptance, move only the approved text into the JSON authority; do not load Markdown at runtime.

The register below is organized as a matrix of scene and boundary conditions. Each card asks a distinct state question for one scene. The repeated review rubric is intentional so an implementer can compare a specific failure against a specific player route. The matrix is reference material, not a requirement to ship every permutation. An entry without a real trigger is deferred as prose direction.
""") + inventory


def card(spec: dict, scene: str, scene_index: int, interruption: tuple[str, str, str], boundary_index: int, rows: list[dict], ordinal: int) -> str:
    label, fact, contract = interruption
    row = rows[scene_index % len(rows)] if rows else None
    existing = f"`{row_id(row)}`" if row else "the current owner projection"
    object_detail = spec["object"]
    time = ("before the shift bell", "after a long wait", "when the light changes", "while the ledger is still open", "at the edge of the next day")[(scene_index * 2 + boundary_index) % 5]
    line = (
        f"{object_detail.capitalize()} is set beside the record. No one calls the entry final.",
        f"A hand pauses over {object_detail}; the reason is written before the name.",
        f"{object_detail.capitalize()} stays where both sides can see it. The count remains unchanged.",
        f"Someone turns {object_detail} over. The blank side is easier to read.",
        f"{object_detail.capitalize()} marks the place. It offers no answer by itself.",
    )[(scene_index + boundary_index * 3) % 5]
    routing = (
        "The preview and commit must use the same canonical snapshot or revalidate just before mutation.",
        "A failed transition must preserve the previous owner state and retain a visible reason code.",
        "A consumer may project this fact but cannot infer an additional reward from the prose.",
        "The save capture must follow the successful event and restore without publishing it again.",
    )[(scene_index + boundary_index) % 4]
    return f"""
### {spec['name']} case {ordinal:03d} — {scene} / {label}

**Scene and prose.** The player reaches the {scene} {time}. {label.capitalize()} matters here: {fact}. The record under review is {existing}. This is a candidate moment for a short physical observation, not a guaranteed quest beat: “{line}” If the source state cannot establish that observation, keep the sentence in the proposal layer. The player must see the actual current state and a reason to act or wait; the text cannot turn a failed request into a narrative success.

**Owner trace.** The query begins with `{spec['core']}` and its actual input owners. The player route, if one exists, passes through `{spec['host']}`. The boundary question for this card is {contract}. {routing} The recorded state belongs in `{spec['save']}` only where the current owner is stateful. Authored identity comes from `{spec['catalog']}` when applicable; otherwise this case uses current Core values and no new JSON authority. Never add a field, store, route, or mechanic because this card happens to name one.

**Integration decision.** Promote this case only after the implementer shows a real scene trigger, the exact live API call, the before/after owner state, and the visible destination. If the condition is false, show a typed refusal and preserve every neighboring owner. If it is true, publish the current Core fact once and let the authorized adapter carry any consequence. For {label}, inspect {contract} in both a fresh campaign and a restored campaign. The content editor then checks that the line's speaker could know the detail and that it does not duplicate a nearby catalog entry.

**Acceptance record.** Record (a) the input snapshot and exact ID, (b) eligibility or refusal code, (c) one owner transition or a proven no-op, (d) save behavior appropriate to `{spec['save']}`, (e) any deterministic ordering requirement, and (f) panel/journal focus and refresh. Include file:line evidence and a focused command in the promoted package. A code compilation result alone does not prove this route; a catalog row alone does not prove reachability. Classify this card as accepted, deferred, or rejected rather than silently broadening the package.
"""


def close(spec: dict, count: int) -> str:
    return f"""
## Finished architecture handoff for {spec['name']}

The documentary integration architecture is complete at the planning level: `{spec['catalog']}` or current owner inputs → `{spec['core']}` → `{spec['host']}` → `{spec['save']}` where mutable → the current player surface. The first package is **{spec['first']}**. The {count} cases above form a candidate review register; they do not establish that the user can already perform them. Historical status claims remain dated above this marker. Production integration requires an authorized package, exact path claims, current call-site checks, and focused verification.

**Promotion packet fields:** package ID; desired observed behavior; non-goals; current source/data citations; exact claimed files; authority and adapter map; JSON IDs; save migration class; day-order rule; player route; 3–6 acceptance assertions; focused verification commands. A foreman resolves collisions. The person implementing one slice does not convert this full matrix into parallel systems.

**Polishing checklist:** remove unsupported promises, align text with the present state and source ID, name actual objects and speakers, preserve chronology, make refusal copy humane and exact, check accessibility labels and refresh, trim any duplicate card that adds no state distinction, and validate that every added JSON row has a consumer. This plan's C# sketch is illustrative; the exact current methods named earlier remain authoritative.
"""


def main() -> None:
    for spec in SPECS:
        path = ROOT / spec["path"]
        original = path.read_text()
        if MARKER in original:
            original = original.split("\n---\n\n" + MARKER, 1)[0].rstrip() + "\n"
        rows = authored_rows(spec)
        expansion = intro(spec, rows)
        ordinal = 0
        for scene_index, boundary_index in product(range(len(spec["scenes"])), range(len(INTERRUPTIONS))):
            ordinal += 1
            expansion += card(spec, spec["scenes"][scene_index], scene_index, INTERRUPTIONS[boundary_index], boundary_index, rows, ordinal)
            if len(original) + len(expansion) + len(close(spec, ordinal)) >= 251000:
                break
        expansion += close(spec, ordinal)
        path.write_text(original.rstrip() + "\n" + expansion.rstrip() + "\n")
        print(f"{spec['path']}\t{len(path.read_text())}\t{ordinal} cases")


if __name__ == "__main__":
    main()
