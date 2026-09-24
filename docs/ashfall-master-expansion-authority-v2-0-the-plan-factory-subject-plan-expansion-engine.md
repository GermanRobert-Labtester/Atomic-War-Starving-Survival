# ASHFALL MASTER EXPANSION AUTHORITY v2.0 — THE PLAN FACTORY

**Repository:** `GermanRobert-Labtester/Atomic-War-Starving-Survival` (ASHFALL: Atomic War – Starving Survival)
**Document version:** 2.0.0 — compiled 2026-09-24, supersedes the v1.0 master world bible (compiled 2026-09-23) as an expansion scaffold.
**Document class:** SUBJECT-PLAN FACTORY. This document is not itself an integration plan. It is a repeatable generator: any future planning session can consume its matrices, backlog, and templates to produce an unbounded series of bounded subject plans, each of which names its own best integration route.
**Audit basis:** Live repository inspection performed 2026-09-24 (repository root listing, `Assets/StreamingAssets/Data/` listing at 342 entries, `docs/` listing, `docs/plans/` listing at 126 entries, `INTEGRATION_PLANS.md`, `SESSION_HANDOFF.md`, `AGENTS.md`, branch list). Every claim in the Drift Register (Part I) is labeled VERIFIED, HIGH CONFIDENCE, or UNVERIFIED.
**Authority order:** unchanged from v1.0 — live repository source and data first; then `AGENTS.md`; then this document; then the docs registry and atlas; then plan ledgers. Where this document and live source disagree, live source wins and this document must be corrected.

---

## PART 0 — WHAT CHANGED FROM v1.0 AND WHY A FACTORY IS NEEDED

The v1.0 master bible was a snapshot: a large, well-structured reference that a planner reads before drafting one plan. Its structural weakness, identified during the 2026-09-24 audit, is that it is a *library*, not a *machine*. It tells a planner what exists, but it does not encode the generative move — the repeatable transformation of (repository evidence × lane × subsystem) into a bounded subject plan with a recommended integration route.

v2.0 therefore adds four new organs on top of the preserved v1.0 body:

1. **The Drift Register (Part I):** a live-audit correction layer. The repository has moved since the v1.0 snapshot; every plan drafted against stale premises is wasted work. The register lists what changed, with evidence and confidence labels.
2. **The Factory Protocol (Part II):** the operating loop that converts evidence into subject plans. It is deterministic, like everything else in this project: same inputs, same plan shape, same verification demands.
3. **The Generator Matrices (Part III):** the combinatorial core. Ten expansion lanes × seventeen subsystem clusters, with per-cell opening archetypes. This is the mechanism by which one document yields hundreds of expansion plans without inventing duplicate systems.
4. **The Seeded Backlog (Part IV) and Templates (Part V):** audit-derived candidate expansions, each with a subject, evidence, confidence, and best integration route; plus the wave-charter, subject-plan, and verification templates the repository already uses, extended for factory output.

**Scale honesty clause.** The requested target for this expansion effort is two million characters. A single authoring pass cannot responsibly produce two million characters of *verified* planning content, and the repository's own constitution (Part 0.4 of v1.0; `AGENTS.md` rules 7–8) forbids manufacturing padded work. v2.0 therefore defines a Multi-Session Growth Protocol (Part VI): the factory is designed to be *appended* session by session, each session adding one or more verified volumes (expanded subsystem deep maps, prose spec libraries, backlog batches), until the corpus reaches the target size organically. The Part VI protocol is the only sanctioned path to the target; bulk generation of unverified prose is a NON-CANON act.

---

## PART I — LIVE-REPOSITORY AUDIT AND DRIFT REGISTER (2026-09-24)

### 1.1 Audit method

The audit inspected the live repository directly: root directory listing, `Assets/StreamingAssets/Data/` (342 entries), `docs/` (top-level documents and subdirectories), `docs/plans/` (126 entries), `INTEGRATION_PLANS.md` (32,793 characters, read head and tail), `SESSION_HANDOFF.md`, `AGENTS.md` (head), and the branch list. No working-tree clone was available in the audit environment; findings marked VERIFIED are directly demonstrated by these listings and file reads. Findings marked UNVERIFIED could not be confirmed in this pass and require a follow-up read before any plan relies on them.

### 1.2 Drift Register — repository facts that differ from, or extend, the v1.0 bible

**DR-01 — Root-level coordination artifacts absent from the v1.0 docs map. VERIFIED.**
The repository root now contains planning and coordination artifacts the bible's docs map (v1.0 Part 5.8) does not mention: `A1_BRIEFING_DEFERRED.md`, `A1_COORDINATION_RECORD.md`, `WAVE9_PART1_CLOSEOUT.md`, `Next-steps-plans/`, `piagentsplans/`, `Seal-steps/`, `semantic-review/`, `POTENTIALCLUTTER.md`, `sources.md`, `CRUSH.md`, `VIBE.md`, `MIMOCODE.md`, `OPENSETUP.md`, `Ashfall.slnx`, `Directory.Packages.props`, `global.json`, plus `tests/` and `snapshots/` at root. Consequence: a planner following the v1.0 docs map will miss active coordination surfaces and may duplicate decisions already recorded in them. Any expansion-planning session must now sweep the root-level `*.md` coordination files and the `Next-steps-plans/`, `piagentsplans/`, and `Seal-steps/` directories before drafting.

**DR-02 — The docs tree has substantially more subdirectories than the v1.0 map. VERIFIED.**
Live `docs/` subdirectories observed in the audit include (selection; the listing was long and partially truncated): `adr/`, `agents/`, `architecture/`, `archive/`, `balance/`, `bodymind/`, `campaign/`, `cartography/`, `ci/`, `cli/`, `collectibles/`, `combat/`, `content/`, `contracts/`, `crafting/`, `crossing/`, `culture/`, `decisions/`, `design/`, `discovery/`, `duty_roster/`, `ecology/`, `economy/`, `endgame/`, `expansions/`, `expeditions/`, `faction_war/`, `factions/`, `foreman/`, `forensics/`, `foundry/`, `gaps/`, `governance/`, `greenhouse/`, `health/`, `holdfast/`, `hygiene/`, `i18n/`, `implementation/`, `incidents/`, `integration/`, `journal/`, `lore/`, `maritime/`, `medical/`, `memorials/`, `mods/`, `moral/`, `moral_choice/`, `muster/`, `narrative/`, `onboarding/`, `orbital/`, `perf/`, `phantoms/`, `plans/`, `power/`, `process/`, `production/`, and a `player_surface_manifest.json`. Two of these — `gaps/` and `incidents/` — are first-class *expansion feedstock*: directories whose entire purpose is to record what is missing or broken. The Factory Protocol (Part II, step 2) now treats `docs/gaps/` and `docs/incidents/` as mandatory inputs.

**DR-03 — New top-level authority documents absent from the v1.0 docs map. VERIFIED.**
Observed live and not listed in v1.0 Part 5.8: `ECONOMY_FAIRNESS_AUDIT.md`, `ENGINE_SUPPORT_POLICY.md`, `GODOT_MIGRATION_STATUS.md`, `REPO_HISTORY_REWRITE.md`, `HUMAN_AUTHORSHIP.md`, `AI_DISCLOSURE.md`, `ASSET_MIGRATION_LEDGER.md`, `CODEX_SOURCE_MATRIX.md`, `ARCHIVE_INDEX.md`, `EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md`, `MEDICAL_DOSE_TREATMENT_MATRIX.md`, `MEDICAL_30_DAY_CAPACITY_REPORT.md`, `SHELTER_MAINTENANCE_MATRIX.md`, `SHELTER_30_DAY_MAINTENANCE_REPORT.md`, `L10N_WAVE2_ROADMAP.md`, `INPUT.md`, `RELEASE_EXPORT.md`, `ENGINE_SUPPORT_POLICY.md`. Of these, `ECONOMY_FAIRNESS_AUDIT.md`, `EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md`, `MEDICAL_DOSE_TREATMENT_MATRIX.md`, and `SHELTER_MAINTENANCE_MATRIX.md` are pre-computed balance baselines: they convert Lane C (economy and balance) planning from speculative to evidence-anchored. Subject plans in Lane C must cite these baselines instead of re-deriving numbers.

**DR-04 — The data catalog inventory has grown; several catalogs are absent from the v1.0 inventory. VERIFIED.**
`Assets/StreamingAssets/Data/` currently holds 342 entries. Catalogs observed live but not present in the v1.0 Part 5.4 inventory include: `dive_sites.json`, `hydroponic_crops.json`, `hydraulic_extrusion_catalog.json`, `metrology_standards_catalog.json`, `muster_camp_scenes.json`, `muster_epilogues.json`, `muster_faction_actions.json`, `muster_faction_culture.json`, `muster_witnesses.json`, `utility_actions.json`, `moral_choice_quests_branching.json`, `moral_choice_quests_distress.json`. Consequence: the duplication firewall (v1.0 Part 5) is stale in these domains; a planner could propose a "new" muster or moral-choice catalog that already exists. The ID-collision sweep in Factory Protocol step 1 must always run against the live listing, never against this document.

**DR-05 — A process script lives inside the data authority. VERIFIED (hygiene finding).**
`Assets/StreamingAssets/Data/` contains `rewrite.py` alongside the JSON catalogs. The data directory is canonically "the sole authored JSON data authority" (`AGENTS.md` rule 3); a Python rewrite script inside it is a process artifact in a content directory. Recommended handling: a Tooling-lane (Lane H) subject plan proposing relocation of the script to `scripts/` or `tools/` with a documented rationale, after verifying what the script rewrites and who calls it. Do not move it without call-site verification; it may be load-bearing for a historical catalog migration.

**DR-06 — Integration ledger state differs from the v1.0 queue snapshot. VERIFIED.**
Live `INTEGRATION_PLANS.md` (read 2026-09-24) shows, at minimum: the **XP Expansion W1** batch ACTIVE (difficulty authority package `XP-WAVE1-DIFFICULTY-AUTHORITY`, with a premise correction recorded against Plan 122 SOFC fuel); the **DISTRESS-SIGNALS-9-12** flagship COMPLETE and presented for acceptance; **Plan 24 CLOSED** (Wave 8, 2026-09-17, signatures resolved 2026-09-18, ward staffing sealed under option b, `DEBT-PLAN24-MEDICAL-WARD-STAFFING` RETIRED); **19A/19B/19C** waves closed with evidence (Endgame 84/84 PASS, focused suites 47 PASS, `verify-fast.sh` reported ALL 47 GATES PASSED); **C2[2] Plan 17** legibility executed with a documented not-executed list (Plan 31 semantic-kind authority, 17C audio phases, 17B deep test matrix remain open gaps); **PR 3 content seal SEALED 2026-09-19** (`CF-P1-DISTRESS-CONTENT-SEAL`); the **availability consumer RETIRED** (Wave 9 Part 2, Option B approved; `SignalTrustAvailability` retained as a pure-math specification pin). Consequence: subject plans in the radio/distress domain must treat the rescue-signal runtime as *sealed and closed*, not as an open expansion surface, unless they extend it through its recorded seams.

**DR-07 — Gate-count and test-total drift. HIGH CONFIDENCE.**
The v1.0 bible states 57 CI gates at v1.1.0 (53 fast + 3 full + 1 performance) and quotes both 11,098 and 11,697 full-suite totals from different handoffs. The live 19-wave closeout evidence in `INTEGRATION_PLANS.md` records `verify-fast.sh` ALL 47 GATES PASSED at that batch's close. These figures cannot all describe the same instant. Factory rule: any subject plan that names a gate count or test total must re-verify the number against the live gate inventory at drafting time and cite the closeout it came from. Never carry counts forward from this or any prior document.

**DR-08 — Wave directories beyond the v1.0 history. VERIFIED.**
`docs/plans/` (126 entries) contains wave directories `wave8_part2/`, `wave9_part2/`, `wave10_part1/`, `wave10_part2/`, `wave11_part1/`, `wave11_part2/`, `wave12_part1_1/`, `flagship_b5_b8/`, and `xp/`, plus `UNCLAIMED_CORPUS_CENSUS.md`, `UNBLOCKED_PLANS_AUDIT_2026-09-19.md`, and `WAVE10_MICRO_DEFERRAL_SWEEP.md`. Two of these are standing expansion inputs: `UNCLAIMED_CORPUS_CENSUS.md` (authored content no system consumes — a utilization-seam backlog) and the unblocked-plans audit. The Factory Protocol consumes both.

**DR-09 — Branch and agent sprawl. VERIFIED.**
The repository carries numerous agent- and CI-generated branches (`Zcode_Branch`, `bug_fixing_main`, multiple `chore/*` and `ci-autogen-*` branches) and a wide set of per-tool agent rulebooks at root (`CLAUDE.md`, `CODEX.md`, `CRUSH.md`, `GEMINI.md`, `GOOSE.md`, `MIMOCODE.md`, `QWEN.md`, `VIBE.md`, `.clinerules`, `.cursorrules`, `.windsurfrules`, `.zcode/`). Consequence: multi-agent discipline (worktree ownership, `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`) is not optional; every factory-generated plan must carry an ownership-claim step. No expansion plan may assume it is the only writer.

**DR-10 — v1.0 items the audit could not confirm in this pass. UNVERIFIED.**
Not confirmed in this audit pass (single-session, listing-level access): the 11,697 test total; the D1 seal state; the full 57-gate inventory; codec version pin values; the `ClaimPersonalBelonging` no-caller status; decision-blocked item states beyond those the ledger records as resolved. Each of these remains plausible but must be re-verified in live source before any plan depends on it. Factory rule: UNVERIFIED premises get a verification step inside the plan, never silent trust.

### 1.3 What the audit confirmed as stable (no change needed)

The following v1.0 structures were confirmed by the audit and remain authoritative: the four-tier architecture (Tier 1 data authority in `Assets/StreamingAssets/Data/`; Tier 2 engine-free Core; Tier 3 `src/Host` + `src/UI`; Tier 4 xUnit plus the `HostCli` selftest surface); the `AGENTS.md` non-negotiable rules (Godot authoritative, Core engine-free, JSON authoritative, one authority per concern, focused verification); the narrative corpus under `Assets/StreamingAssets/Data/narrative/` (present in the live listing); the faction, economy, weather, Year-of-Ash, moral-choice, muster, and verdict catalog families (all present live); and the plan-discipline artifacts (`INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`, `SESSION_HANDOFF.md`) at root.

---

## PART II — THE FACTORY PROTOCOL (HOW THIS DOCUMENT MAKES MANY PLANS)

This protocol is the heart of v2.0. It converts repository evidence into subject plans, deterministically, and it is designed to be executed by any future planning session (human or LLM) without re-deriving the method. One execution of the protocol yields one subject plan; the matrices in Part III provide the candidate space; the backlog in Part IV holds pre-audited candidates.

### 2.1 The seven-step factory loop

**Step 1 — Premise sweep (mandatory, every time).**
Before selecting any candidate, the session re-verifies premises against live source: the live `Assets/StreamingAssets/Data/` listing (duplication firewall, DR-04), `INTEGRATION_PLANS.md` current batch (DR-06), `WORKTREE_OWNERSHIP.md` claims (DR-09), `KNOWN_DEBT.md`, the root coordination files (DR-01), `docs/gaps/` and `docs/incidents/` (DR-02), and `docs/plans/UNCLAIMED_CORPUS_CENSUS.md` (DR-08). Output: a short premise sheet. A candidate whose premise fails the sweep is discarded, not patched.

**Step 2 — Select exactly one lane and one subsystem cluster.**
From Part III. The selection rule is lane rotation discipline (v1.0 Part 11): at most one plan per lane per wave; data-first lanes (A, C, J) precede wiring lanes (B, D, E) within the same domain. The session states the lane and cluster in the plan header.

**Step 3 — Pull the cell's opening archetype and instantiate it.**
Each matrix cell names an archetype (the *kind* of expansion that cell supports, with its owning seams). The session instantiates the archetype against current evidence: which catalog, which loader, which host session, which save family, which panel. If the archetype's seams no longer exist as described, the cell is stale — record the correction in the Drift Register and pick again.

**Step 4 — Draft the subject plan in the v2.0 subject-plan format (Part V, Template S).**
A subject plan is not an integration plan. It states WHAT should expand, WHY (with evidence), WHAT MUST NOT CHANGE, and WHICH INTEGRATION ROUTE the repository should prefer — but it does not prescribe line-level implementation. The integration route recommendation (Part V, Template R) names the tier (data-only / host wiring / Core extension), the seams, the save impact class, and the verification class. This preserves the repo's own separation: subject plans propose; integration plans (drafted later, against the live tree, in an owning session) commit.

**Step 5 — Run the continuity and anti-duplication checklist.**
The v1.0 checklist (Part 13.2) applies in full, plus two factory additions: (a) duplication firewall — prove the candidate does not duplicate any live catalog, system, or `docs/` authority map; (b) unclaimed-content check — if the candidate's content domain appears in `UNCLAIMED_CORPUS_CENSUS.md`, the plan must wire the unclaimed content first or explain why new content outranks it.

**Step 6 — Label every claim.**
CANON / VERIFIED / HIGH CONFIDENCE / INFERENCE / PROPOSAL / UNKNOWN, per v1.0 Part 0.3. Plans containing UNVERIFIED premises must carry an explicit verification step as their first integration step.

**Step 7 — Emit the output bundle.**
The session delivers: the subject plan; facts used; new facts introduced; continuity risks; verification steps; the recommended integration route; and a backlog delta (which Part IV candidates were consumed, corrected, or added).

### 2.2 Factory invariants

- One plan = one bounded outcome riding existing seams (v1.0 Part 10). The factory never widens a plan to reach a size target.
- No plan may create a parallel authority. Every state change names its owning system.
- Data-first preference: if an expansion can be authored as JSON through an existing loader, it must be, and the plan must say so.
- The factory never drafts against decision-blocked items (the current list must be re-read from `INTEGRATION_PLANS.md` each session — DR-06 shows signatures resolve over time).
- Subject plans do not edit files. Implementation happens only in an owning session after plan selection (v1.0 approval-based workflow).
- Every generated plan must state its position relative to each epilogue permutation it touches (v1.0 Part 6.6).

---

## PART III — GENERATOR MATRICES (LANES × SUBSYSTEM CLUSTERS)

Ten lanes (A–J, from v1.0 Part 11) against seventeen subsystem clusters distilled from the live Core inventory (v1.0 Parts 5.1–5.2 and 16, confirmed live). Each cell names an opening archetype. Confidence labels reflect the audit state as of 2026-09-24 and must be re-checked at drafting time. This matrix is the combinatorial engine: 170 cells, each capable of yielding multiple subject plans over time as content lands and seams mature. Not every cell is currently open; cells marked SEALED are closed by evidence (e.g., the distress-signal content seal, DR-06) and may not be opened without new evidence and foreman signature.

### Cluster definitions

C1 Shelter operations (rooms, thermal, schedules, fire, decor, barter, noise, prisoners, sanitation, airlock, decon, atmosphere) · C2 Medical pipeline (disease, dose ledger, ARS, surgery, autopsy, pharma, diagnostics, therapies, dependency, crises) · C3 Water, food, agriculture (treatment, condensers, wells, brine, nutrition, kitchen, preservation, grain, greenhouse, crops, aquaponics, apiculture) · C4 Power and industry (grid, SOFC, solar, kinetic, geothermal, foundry, CVD diamond, coatings, optics, powder metallurgy, pyrolysis, Fischer-Tropsch, chlor-alkali, acids, fermentation, ethanol, air separation, metrology) · C5 Expeditions and travel (destinations, scavenging tables, vehicles, waystations, caravans, routes, travel encounters, micro-locations) · C6 Map and geography (wasteland map, damaged zones, fog, route gates, cartography, survey instruments) · C7 Factions and war (stance, doctrines, war chains, tributes, treaties, embargoes, espionage, psyops, infiltration, musters, labor camps, bounties) · C8 Radio and information (stations, programs, intercepts, distress signals, rumors, sound ranging, direction finding, NVIS, heliograph) · C9 Survivors and interiority (needs, skills, traits, arcs, trauma, guilt, therapies, relations, caregiving, beliefs, rituals, memorials, final wishes, lineage, cohorts, apprenticeships) · C10 Quests and moral choice (questline master, dynamic questlines, personal quests, NPC arcs, moral-choice chains/flags/gossip, branching, bureaucratic morality, expansion quests) · C11 Economy (market, baselines, regional prices, shocks, rumors, black market, debt ledger, tributes, trade screens, tell lines) · C12 Weather and Year of Ash (weather system, seasons, effects, gates, hardening, storm windows, Year-of-Ash families, epilogue pressure) · C13 Endgame and epilogue (Reckoning, verdict, epilogue matrix, chronicle, muster epilogues, standing records, census) · C14 Ecology and wildlife (migration, trapping, ecosystem, bestiary, flora, infestations, contagion, pathogens, crop genomes) · C15 Defense and security (perimeter, defense grid, sky defense, ordnance, chemical defense, orbital harrow, interlocks, EMP effects) · C16 Progression and meta (skills, research, collectibles, trophies, achievements, difficulty presets, XP wave, codex, field guide, bestiary, L10N, mods, settings, input) · C17 Host surface and UI (panels, shell, focus navigation, snapshots, a11y, briefings, dashboards).

### 3.1 Lane A — Narrative and prose (all types and kinds)

| Cluster | Opening archetype | Confidence |
|---|---|---|
| C1 | Bureaucratic texture for under-documented rooms: shift notices, maintenance glitch reports, load-shed amendments for rooms lacking corpus coverage | HIGH CONFIDENCE |
| C2 | Casebook and therapy-note expansion for affliction states with thin prose coverage; dose-treatment narrative pairing against `MEDICAL_DOSE_TREATMENT_MATRIX.md` | HIGH CONFIDENCE |
| C3 | Assay/log corpus for preservation and processing chains that have catalogs but no narrative corpus twin (v1.0 Part 16.4 pattern: every process ships technical + prose) | HIGH CONFIDENCE |
| C4 | Same pattern for the newest industrial catalogs confirmed live in DR-04 (`hydraulic_extrusion`, `metrology_standards`): audit records, calibration logs | HIGH CONFIDENCE |
| C5 | Expedition field reports and waypoint notes for destinations with sparse `arrival_description`/`revisit_description` coverage; route-waypoint batches | HIGH CONFIDENCE |
| C6 | Gazetteer entries and damaged-zone survey prose; cartographic marginalia | INFERENCE — verify current coverage |
| C7 | Communiqué, directive, and verdict-corpus expansion for factions with thin public/private language separation | HIGH CONFIDENCE |
| C8 | Radio rundown/transcript batches for stations with thin programming; numbers-station and cipher follow-ups | HIGH CONFIDENCE — but distress-signal content is SEALED under `CF-P1-DISTRESS-CONTENT-SEAL` (DR-06); do not add signal scenarios |
| C9 | Delayed moral-choice callbacks (~100-day returns) via `IFlagLedger` flags; phantom-memory triggers tied to surviving cohorts | HIGH CONFIDENCE (v1.0 Part 7 gap 2) |
| C10 | Quest prose fields (`quest_hook`, `objective_text`, outcome texts) for quest records with skeleton prose; follow Part 9 contracts exactly | HIGH CONFIDENCE |
| C11 | Ledger, statement, and debt-template prose; rumor batches within deterministic bands | HIGH CONFIDENCE |
| C12 | Mid-winter slump pressure (Days 90–180) story arcs; storm-window almanac entries | HIGH CONFIDENCE (v1.0 Part 7 gap 1) |
| C13 | Epilogue-chronicle depth for under-served permutations of the 32-permutation matrix | HIGH CONFIDENCE |
| C14 | Bestiary and natural-history corpus extension; mutated-botanical and limnology follow-on batches | HIGH CONFIDENCE |
| C15 | Defense-log and ordnance-manifest prose; orbital-harrow telemetry transcripts | INFERENCE — verify coverage |
| C16 | Codex and field-guide entries for systems that gained content since the last codex wave | HIGH CONFIDENCE |
| C17 | Ambient environmental text and atmosphere cues for panels rendering newer systems with sparse surface prose | INFERENCE — verify via `--ui-layout-selftest` and snapshot coverage |

### 3.2 Lane B — Mechanics and systems functionality

| Cluster | Opening archetype | Confidence |
|---|---|---|
| C1 | Room-level effect extensions routed through `IsRoomPowered`; shelter-failure follow-ons building on the quarantined failure-effects wiring logs observed in `docs/plans/` | HIGH CONFIDENCE |
| C2 | Ward-staffing and recovery-ramp follow-ons are CLOSED (Plan 24, DR-06); open instead: cross-links between medical and cohort/lineage (child health), and between dose ledger and Year-of-Ash fallout windows | PROPOSAL — premise sweep required |
| C3 | Zoonosis-style bridges: kitchen/preservation × disease; cellar-rot × greenhouse economics; apiculture × morale | PROPOSAL |
| C4 | Bind the newest industrial catalogs (DR-04) into consumption/production ledgers through the existing power-grid and foundry seams | PROPOSAL — needs live loader verification |
| C5 | Per-destination scavenging-table parity for destinations beyond the 49-table coverage; vehicle-breakdown consequences into medical and dose ledgers | HIGH CONFIDENCE |
| C6 | Flooded-route topology tags and authored map edges (foreman-flagged open decision — needs the named signature first) | BLOCKED — decision-gated |
| C7 | FactionWar per-strike emitter extension (foreman-flagged open decision — needs signature) | BLOCKED — decision-gated |
| C8 | Radio-signal follow-up chaining is SEALED (DISTRESS-SIGNALS-9-12 COMPLETE, DR-06); open instead: market-rumor band extension and intercept-driven journal depth | HIGH CONFIDENCE |
| C9 | Survivor interiority bridges: belief movements × faction stance; memorial rites × epilogue evidence; chemical dependency × medical ward | PROPOSAL |
| C10 | Quest state reopening after new discoveries (failure-recovery grammar, v1.0 Part 6.7); moral-choice flag consumers beyond the flag ledger | HIGH CONFIDENCE |
| C11 | Black-market funds/goods legs remain decision-gated (canonical funds authority); merchant restock priority is SEALED by DEC-05 (DR-06) | BLOCKED / SEALED |
| C12 | Year-of-Ash tick-window extensions (180–360) for systems not yet producing winter pressure | PROPOSAL |
| C13 | Reckoning evidence enrollment for systems added since the last endgame wave (19A/19B/19C closed, DR-06) | HIGH CONFIDENCE |
| C14 | Trapping→disease zoonosis bridge exists; open: migration × expedition route encounters; infestation × crop economy | PROPOSAL |
| C15 | EMP effects exist (shelter EMP/medical power logs observed); open: defense grid × warlord siege math; sky-armor × orbital harrow telemetry | PROPOSAL |
| C16 | XP Expansion W1 is ACTIVE (DR-06): difficulty-authority consumer binding is the sanctioned open seam in this cluster — extend it, do not parallel it | HIGH CONFIDENCE |
| C17 | Panels rendering stale or missing data for newer systems; verify against `--ui-layout-selftest` before claiming | HIGH CONFIDENCE |

### 3.3 Lane C — Economy and balance

Anchored by the live baselines (DR-03): `ECONOMY_FAIRNESS_AUDIT.md`, `ECONOMY_PRICE_FACTOR_MATRIX.md`, `EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md`, `docs/balance/`.

| Cluster | Opening archetype | Confidence |
|---|---|---|
| C4 | Fuel and feedstock income-versus-expenditure audits for each industrial chain; dominated-process analysis (do any catalogs produce strictly dominated outputs?) | HIGH CONFIDENCE |
| C5 | Scavenging E[value] re-runs after any loot authoring (Plan 76.2 harness pattern); vehicle dominance follow-ups against the live dominance table | HIGH CONFIDENCE |
| C7 | Tribute-cycle sustainability (7-day cadence) versus mid-game income; embargo economic pressure | HIGH CONFIDENCE |
| C11 | Price-shock and rumor-band systemic outcomes; debt-interest runaway analysis; black-market pricing tiers | HIGH CONFIDENCE |
| C12 | Winter resource compression (Days 90–180): calories, fuel, filters, morale — sustainability-day math per difficulty preset | HIGH CONFIDENCE |
| C14 | Trapping yield versus equipment degradation cost; zoonosis risk premium on uncooked yield | HIGH CONFIDENCE |
| All others | Balance audits only where numbers exist; never invent tuning targets without an intended design statement | — |

### 3.4 Lane D — Save, state, and compatibility

| Cluster | Opening archetype | Confidence |
|---|---|---|
| Any stateful extension | Codec bump + migration + round-trip per v1.0 Part 12.3; old-save→new-build fixtures through the `SaveSupportWindowTests` corpus | CANON process |
| C9 | Lineage/cohort long-horizon state (3-year simulation exists per 19B closeout, DR-06): verify horizon coverage before extending | HIGH CONFIDENCE |
| C13 | Epilogue evidence persistence: which Day-360+ facts survive into the Day-3650 window | HIGH CONFIDENCE |
| Cross-cutting | Mid-event and mid-combat save round-trips for exactly-once effect classes beyond the rescue-signal runtime (which models the pattern) | PROPOSAL |

### 3.5 Lane E — UI, UX, and accessibility

| Cluster | Opening archetype | Confidence |
|---|---|---|
| C17 | Per-system: panels exposing existing commands + truthful state for systems that gained data since their panel last shipped; a11y words-not-color-only gating; controller parity | HIGH CONFIDENCE |
| C8 | Rescue-signals strip is shipped (DR-06); extension only through existing strip seams | SEALED surface, additive only |
| C16 | Difficulty/XP binding surfaces once W1 lands (DR-06) — coordinate, do not parallel | HIGH CONFIDENCE |
| All | Snapshot coverage and `--ui-a11y-selftest` gates apply to every panel change; `DESIGN.md` and `ACCESSIBILITY.md` are pinned | CANON process |

### 3.6 Lane F — Performance

| Cluster | Opening archetype | Confidence |
|---|---|---|
| C17 | High-frequency UI rebuild audits (metric cards, data grids) — measure first via the CI performance gate | Potential hotspot — profile before rewrite |
| C12 | Year-of-Ash tick-window cost concentration (Days 180–360): per-day work spikes during storm windows | Potential hotspot — requires measurement |
| C13 | Epilogue-matrix evaluation cost at Day 360 — one-shot, likely fine; measure only if reported | HYPOTHESIS |
| All | No optimization plan without before/after numbers in `docs/perf/` | CANON process |

### 3.7 Lane G — Testing and verification

| Cluster | Opening archetype | Confidence |
|---|---|---|
| C2 | Dose-treatment matrix paired tests against `MEDICAL_DOSE_TREATMENT_MATRIX.md` | HIGH CONFIDENCE |
| C11 | Debt-ledger consequence dispatcher coverage; rumor-band determinism pins | HIGH CONFIDENCE |
| C10 | Moral-choice flag consumer coverage for newly added consumers | HIGH CONFIDENCE |
| C13 | Epilogue permutation reachability tests for under-served permutations | HIGH CONFIDENCE |
| Cross | Determinism two-pass proofs for every new simulation; TEST-AGGREGATION metadata for catalog checks | CANON process |

### 3.8 Lane H — Tooling and developer experience

| Cluster | Opening archetype | Confidence |
|---|---|---|
| Data authority | `rewrite.py` relocation/justification (DR-05) | VERIFIED finding, PROPOSAL handling |
| Content | Content-utilization reporting extensions driven by `UNCLAIMED_CORPUS_CENSUS.md` (DR-08): a census-to-plan feed | HIGH CONFIDENCE |
| Docs | Index drift gate extensions covering the new root-level coordination files (DR-01) | HIGH CONFIDENCE |
| CI | Gate-count drift detection (DR-07): a script or test that fails when documented gate counts diverge from the live inventory | PROPOSAL |

### 3.9 Lane I — Documentation and conventions

| Cluster | Opening archetype | Confidence |
|---|---|---|
| Cross | Authority maps for domains that gained systems since their last map (the live `docs/` map in DR-02 shows which domains have directories — a directory without an authority map is a candidate) | HIGH CONFIDENCE |
| C16 | L10N wave roadmap continuation respecting string freeze (re-check freeze state first — signatures resolve over time, DR-06) | HIGH CONFIDENCE |
| Root | Register the root-level agent rulebooks and coordination files in the docs map so planners stop missing them (DR-01, DR-09) | VERIFIED need |

### 3.10 Lane J — Onboarding and player experience

| Cluster | Opening archetype | Confidence |
|---|---|---|
| C16 | Difficulty preset scalar consumers (CF-XP01 line, reinforced by active W1, DR-06) | HIGH CONFIDENCE |
| C17 | Daily-briefing surface for newly landed systems; onboarding flow waves | HIGH CONFIDENCE |
| All | Manual playthrough checklists per wave (pattern exists: HoldfastManualPlaytest, expedition playtest report) | CANON process |

---

## PART IV — SEEDED EXPANSION BACKLOG (AUDIT-DERIVED CANDIDATES)

Each candidate is a subject-plan seed: consume it through the Factory Protocol. Ordering within the backlog is by evidence strength, not by preference. None of these has been claimed; all require the Step 1 premise sweep before drafting.

**SB-01 — Delayed moral-choice callbacks (Lane A/C10).** Evidence: v1.0 Part 7 gap 2; `moral_choice_flags.json`, `IFlagLedger`, `DoorEncounterSystem`, `MoralChoiceSaveStore` all confirmed live. Subject: ~100-day delayed visitor/letter/radio/journal returns keyed on persisted flags. Integration route: data-first new catalog through the moral-choice loader family; dispatch through the daily-tick seam; possibly no codec bump if per-flag records already persist. Verification: integrity + utilization selftests, determinism replay, exactly-once dispatch test. Confidence: HIGH CONFIDENCE.

**SB-02 — Mid-winter slump pressure campaign (Lane A/C12).** Evidence: v1.0 Part 7 gap 1 (Days 90–180). Subject: a bounded story-pressure wave (blight, cave-in, levy arc) authored through existing catalogs. Integration route: data-first; each pressure rides its owning system (ecology for blight, subterranean/excavation for cave-ins, warlord doctrines for levies). Confidence: HIGH CONFIDENCE.

**SB-03 — Newest industrial catalogs: corpus twins + consumption wiring (Lanes A and B/C4).** Evidence: DR-04 (`hydraulic_extraction_catalog`, `metrology_standards_catalog` live, absent from v1.0 inventory). Subject: (a) assay-log narrative twins per the Part 16.4 pattern; (b) bind catalogs into consumption/production ledgers via power-grid/foundry seams if not yet consumed — check `UNCLAIMED_CORPUS_CENSUS.md` first (DR-08). Confidence: HIGH CONFIDENCE that content exists; UNVERIFIED whether systems consume them.

**SB-04 — Muster domain deep expansion (Lanes A and B/C7).** Evidence: DR-04 — five muster catalogs live (`muster_camp_scenes`, `muster_epilogues`, `muster_faction_actions`, `muster_faction_culture`, `muster_witnesses`). Subject: muster-camp encounter depth, witness-driven epilogue evidence, culture-conditioned actions. Integration route: data-first through muster loaders; epilogue touch must be declared. Confidence: HIGH CONFIDENCE.

**SB-05 — Epilogue permutation coverage campaign (Lanes A and C/C13).** Evidence: 19A/19B/19C closed (DR-06); matrix is 32 permutations. Subject: audit which permutations are under-served in chronicle prose and evidence enrollment; author chronicle depth for the weakest permutations. Integration route: data-first into epilogue chronicle catalogs; Reckoning enrollment through endgame owners. Confidence: HIGH CONFIDENCE.

**SB-06 — Balance baseline refresh wave (Lane C).** Evidence: DR-03 live baselines. Subject: re-run the deterministic simulation harness after the most recent content waves (muster, moral-choice distress additions, industrial catalogs) and publish deltas in `docs/balance/`. Integration route: no product code; harness runs + reports. Confidence: HIGH CONFIDENCE.

**SB-07 — Root coordination surface registration (Lane I).** Evidence: DR-01, DR-09. Subject: register root-level coordination files and agent rulebooks in the docs map; define which are active vs historical. Integration route: docs-only. Confidence: VERIFIED need.

**SB-08 — Gate-count drift guard (Lane H).** Evidence: DR-07. Subject: a check that fails when a documented gate count diverges from the live inventory, ending manual count drift between bibles, closeouts, and CI. Integration route: small script/test in `scripts/ci/` family, mirrors existing gates. Confidence: PROPOSAL (design needs the live gate inventory as input).

**SB-09 — `rewrite.py` data-authority hygiene (Lane H).** Evidence: DR-05. Subject: verify the script's role; relocate or document in place. Integration route: tooling-only; requires call-site verification first. Confidence: VERIFIED finding, PROPOSAL handling.

**SB-10 — Unclaimed-content census feed (Lanes A and H).** Evidence: DR-08 `UNCLAIMED_CORPUS_CENSUS.md`. Subject: convert the census into a standing factory input — every unclaimed content row is a wiring candidate (Lane B) with utilization-gated acceptance. Integration route: process/canon — no new system; extend the utilization selftest surface where needed. Confidence: HIGH CONFIDENCE.

**SB-11 — C2 open-gap package: Plan 31 semantic-kind authority, 17C audio phases, 17B deep test matrix (Lanes B, F, G).** Evidence: DR-06 not-executed lists. Subject: three bounded follow-ons the ledger itself records as real gaps. Integration route: per existing C2 plan documentation. Confidence: VERIFIED as open; scope per item needs the plan docs.

**SB-12 — Dive-site and hydroponic domain expansion (Lanes A and B/C3, C5).** Evidence: DR-04 — `dive_sites.json`, `hydroponic_crops.json` live but absent from v1.0's inventory. Subject: premise-sweep these domains for unexploited seams (dive oxygen drain is a canon hourly system; hydroponics may lack narrative corpus and economy legs). Integration route: data-first + existing host sessions. Confidence: INFERENCE pending sweep.

---

## PART V — TEMPLATES

### Template S — Subject plan (the factory's output unit)

```text
# Subject Plan [FACTORY-BATCH-N] — [outcome]
Lane: [A–J] · Cluster: [C1–C17] · Status: PROPOSAL (subject-level, not an integration plan)
## Subject
[What expands, in one paragraph.]
## Premise evidence
[Live-source facts verified this session, each labeled. Name exact files/catalogs/systems.]
## Why this and not something else
[Why the evidence ranks this above other open cells.]
## What must not change
[Canon constraints, sealed surfaces, decision-blocked items respected.]
## Recommended integration route
[Filled using Template R.]
## Continuity checklist result
[Part 13.2 of v1.0 + factory additions; epilogue permutations touched.]
## Verification class
[Which gates/selftests/focused tests will prove the eventual implementation.]
## Open premises
[Anything UNVERIFIED that the integration plan must confirm first.]
```

### Template R — Integration route recommendation

```text
Tier: DATA-ONLY | HOST-WIRING | CORE-EXTENSION | DOCS-ONLY | TOOLING
Seams (in order of use): [catalog file → loader/integrity → Core system → host session →
  save section/codec → event routing → UI panel → selftest]
Save impact class: NONE | EXISTING-SECTION | CODEC-BUMP-AND-MIGRATE
Determinism impact: NONE | NEW-RNG-SUBSTREAM | NEW-SIMULATION (two-pass proof required)
Verification class: integrity-selftest | utilization-selftest | focused-xUnit | headless selftest
  flags | determinism replay | balance harness | snapshot/a11y gates
Ownership: paths to claim in WORKTREE_OWNERSHIP.md; conflicts to check in INTEGRATION_PLANS.md
Route notes: [why this route beats alternatives; smaller-tier preference stated]
```

### Template W — Wave charter (for multi-plan batches)

```text
# Wave Charter — [domain] (Factory batch [N])
## Domain and non-goals
## Lane allocation (max one plan per lane; data-first before wiring)
## Flagship + satellites (never more than five concurrent plans on shared seams)
## Verification matrix (per plan: verification class + acceptance)
## Closeout discipline (docs/plans/PLAN[x]_CLOSEOUT.md; changelog generated-region update)
```

---

## PART VI — MULTI-SESSION GROWTH PROTOCOL (THE HONEST PATH TO 2,000,000 CHARACTERS)

The v1.0 bible is roughly 86,000 characters of verified reference. A two-million-character corpus is roughly twenty-three times that volume. That volume is reachable, but only as accumulated *verified* content, because the repository's constitution forbids manufactured padding and this document inherits that rule. The protocol:

1. **Volume unit.** A volume is one appended Part to this document (or one of its companion canvases) produced in a single session, typically 15,000–60,000 characters, always evidence-grounded against the live repository.
2. **Volume types, in rotation:** (a) subsystem deep-map volumes (one per cluster C1–C17: full catalog inventories, prose-coverage gaps, seam maps); (b) prose specification libraries (expanded Part 9 field contracts with worked examples per document genre); (c) backlog replenishment volumes (fresh premise sweeps converting new Drift Register entries into SB-candidates); (d) lane deep guides (one per lane: full archetype playbooks with worked subject plans); (e) audit volumes (periodic re-audits refreshing the Drift Register).
3. **Session checklist.** Each session: run the Step 1 premise sweep; execute the Factory Protocol or append a volume; update the Drift Register for anything that moved; record the character count and volume index in the growth ledger below.
4. **Growth ledger.** v2.0 base: approximately 25,000 characters (this document). Target: 2,000,000. Every appended volume appends one ledger line: `[date] Volume [n] ([type]) — [chars] — cumulative [total]`.
5. **Quality gates that never relax:** every substantive statement carries a fact status; every volume names its verification surface; no volume may open a sealed surface (DR-06) or a decision-blocked item without the named signature; no volume may duplicate a live catalog or system.
6. **Anti-padding rule.** If a session cannot find verified content for the next volume, it records "no warranted volume" and stops. Zero-volume sessions are acceptable outcomes under the repository's own zero-plans doctrine.

---

## APPENDIX A — UPDATED VERIFICATION COMMAND SURFACE

Unchanged from v1.0 Part 14.1, with two audit corrections:
- Gate counts and test totals are DRIFT-PRONE (DR-07). Re-verify against the live gate inventory; never quote this document's numbers.
- All commands assume the live repository tree; the audit environment for this document had listing-level access only, so every command result claimed in any future volume must come from an actual owning session against the working tree.

## APPENDIX B — GLOSSARY ADDITIONS (v2.0)

- **Factory** — this document's generative protocol (Part II).
- **Volume** — one session's appended, verified content unit (Part VI).
- **Drift Register** — the live-audit correction layer (Part I), the first thing any session reads.
- **Subject plan** — an expansion proposal that names its subject, evidence, and recommended integration route but commits no file changes.
- **Sealed surface** — a domain closed by evidence and signature (e.g., distress-signal content, DR-06); openable only with new evidence and foreman signature.
- **Unclaimed content** — authored catalog content with no consuming system, tracked in `docs/plans/UNCLAIMED_CORPUS_CENSUS.md` (DR-08).

*End of v2.0 core. Authority remains with live repository source; correct this document when source disagrees. Growth ledger starts at zero volumes; the next session appends Volume 1.*
---

# VOLUME 1 — TWELVE COMPLETE SUBJECT PLANS (Factory batch 2026-09-24-B)

Premise sweep executed 2026-09-24: live data listing (342 entries), live docs listing, `INTEGRATION_PLANS.md` (32,793 chars, head and tail), `SESSION_HANDOFF.md`, `AGENTS.md`, branch list, `docs/plans/` (126 entries). Every premise below that depends on file-internal state not readable in this pass is labeled with its open verification step. These are subject plans: they commit no file changes.

## Subject Plan F-001 — Delayed Moral-Choice Callbacks

Lane A · Cluster C10 · Status PROPOSAL.

### Subject
Roughly one hundred days after an early door-encounter or moral-choice resolution, the world returns: a visitor, a letter, a radio strip item, a journal prompt, or a rumor, keyed on the already-persisted choice flag. The player recognizes the anchor; the consequence lands in the mid-game rather than evaporating at Day 10.

### Premise evidence
VERIFIED: `moral_choice_flags.json`, `moral_choice_quests_distress.json`, `moral_choice_chains.json`, and the wider moral-choice catalog family exist live in the data authority. VERIFIED: v1.0 Part 5.6 documents the flags/ledger seam and the weight_of_choices epilogue codec (v2). VERIFIED (drift-corrected): the rescue-signal content wave added `moral_choice_quests_distress.json`, so the moral-choice loader family already consumes multiple split catalogs — the pattern for adding one more split catalog exists. HIGH CONFIDENCE: no current consumer re-reads door-choice flags after the near-term window (v1.0 Part 7 gap 2); the integration plan must re-grep flag consumers before implementation.

### Why this and not something else
The repository's own atlas names this the highest-confidence pacing gap (v1.0 Part 7 gap 2), and it converts already-authored, already-persisted state into new play with zero new save schema — the cheapest large narrative yield available.

### What must not change
Choice resolution behavior, flag ids, the weight_of_choices codec semantics, the sealed distress-signal content class (no callback may add a new distress scenario; callbacks arrive through journal, radio strip, visitor, or rumor seams, not the signal catalog).

### Recommended integration route (Template R)
Tier: DATA-ONLY plus one host-wiring step. Seams: new `moral_choice_delayed_callbacks.json` → moral-choice loader family integrity rules → daily-tick dispatch in the moral-choice host session (named RNG sub-stream `delayed_callback`) → journal / radio strip / relationship delta / rumor routing through existing owners → exactly-once guard keyed on flag id + day. Save impact class: EXISTING-SECTION if per-flag records persist in the moral-choice section (verify); CODEC-BUMP-AND-MIGRATE otherwise. Determinism impact: NEW-RNG-SUBSTREAM. Verification class: data-integrity selftest, content-utilization selftest, focused xUnit (loader gate with per-row failure output, exactly-once dispatch test, two-pass determinism replay, one cross-system consequence test).

### Continuity checklist result
Callback targets must reference existing survivor/location/faction ids only. Information-flow legality: the returning party must plausibly know the player's choice through a modeled channel (they were present, a rumor traveled, a courier carried word). Epilogue permutations: callbacks may adjust relationship deltas and epilogue weight only through the existing moral-choice weight seam; declare which permutations shift.

### Open premises
1. Verify whether the moral-choice save section already persists per-flag records sufficient for an exactly-once guard without codec bump. 2. Grep current flag consumers to confirm the long-horizon gap still exists.

## Subject Plan F-002 — Mid-Winter Slump Pressure Campaign (Days 90–180)

Lane A/C12 · Cluster C1, C7, C14 · Status PROPOSAL.

### Subject
A bounded campaign-window content wave that inserts authored pressure into Days 90–180: a crop blight epidemic arc (ecology), a deep-strata cave-in arc (subterranean/excavation), and a warlord conscription levy arc (doctrines/tribute), each delivered through existing catalogs and event systems, so the stabilized mid-game stays legible as triage rather than routine.

### Premise evidence
VERIFIED: the atlas names the mid-winter slump as the primary pacing gap (v1.0 Part 7 gap 1). VERIFIED live: `ecological_infestations.json`, `subterranean_zones.json`, `warlord_doctrines.json`, `year_of_ash_storm_windows.json`, `seasonal_events.json`, `cascade_rules.json` all exist. VERIFIED: Year-of-Ash tick window is Days 180–360, so Days 90–180 pressure must ride seasonal/event seams, not Year-of-Ash seams.

### Why this and not something else
The atlas flags it; the catalogs that would carry it all exist; and it is data-first across three different owning systems, demonstrating the factory's one-lane-many-cluster pattern.

### What must not change
Year-of-Ash canon (window 180–360), warlord tribute cadence (7 days), weather gate semantics, difficulty authority ownership (XP W1 is ACTIVE — no new difficulty scalars outside it).

### Recommended integration route
Tier: DATA-ONLY (three parallel authored tranches, one per owning system). Seams: ecology infestation catalog + crop strain catalog → existing infestation event dispatch; subterranean zones + excavation hazard mitigation catalogs → existing cave-in event path; warlord doctrines + tribute ledger → existing levy/tribute seams with `FactionStanceEngine` for reactions. Save impact class: NONE (events derive from catalogs and campaign state). Determinism: events must use existing seeded event streams — no new simulation. Verification: integrity + utilization selftests, one focused event-dispatch test per arc, balance harness re-run for the levy's economic pressure.

### Continuity checklist result
Levy reactions must respect information-flow legality (the faction learns of the player's capacity through modeled channels). Cave-ins must not contradict subterranean zone states. Blight must respect crop-strain genome rules. Epilogue: blight and levy outcomes may feed standing/evidence through existing owners; declare permutations touched.

### Open premises
Verify the current infestation and subterranean event dispatch surface actually consumes the catalog fields the new tranches would author (loader field check).

## Subject Plan F-003 — Industrial Catalog Corpus Twins and Consumption Wiring Audit

Lane A/B · Cluster C4 · Status PROPOSAL (two-stage).

### Subject
Stage one: audit the newest industrial catalogs confirmed live by the audit (`hydraulic_extrusion_catalog.json`, `metrology_standards_catalog.json`, plus any others found in the unclaimed census) for narrative corpus coverage and system consumption. Stage two: author the missing assay-log corpus twins and, where a catalog is unconsumed, wire its consumption through the existing power-grid/foundry/workshop seams — never a parallel system.

### Premise evidence
VERIFIED: both catalogs exist live and are absent from the v1.0 inventory (DR-04). VERIFIED: `docs/plans/UNCLAIMED_CORPUS_CENSUS.md` exists (DR-08) and is the designed instrument for exactly this question. HIGH CONFIDENCE: the repository pattern "every industrial process ships a technical catalog plus a narrative assay/log corpus" (v1.0 Part 16.4) — the corpus families named in Part 5.6 demonstrate it across dozens of processes.

### Why this and not something else
DR-04 shows the duplication firewall is stale for these domains; left unaudited, they become the exact duplicate-or-orphan defect class the repository's gates exist to prevent. This plan closes a verified drift finding.

### What must not change
No new items where existing ids suffice; no new production authority outside the foundry/power-grid/workshop owners; reuse `expansion_item_tags.json` for tagging.

### Recommended integration route
Tier: audit (DOCS-ONLY) then DATA-ONLY corpus authoring, with HOST-WIRING only for confirmed unconsumed catalogs. Seams: corpus twins into `Assets/StreamingAssets/Data/narrative/` following the assay-report genre contracts; consumption wiring through the named host sessions (`HydraulicExtrusion` session confirmed live in the v1.0 host inventory). Verification: data-integrity selftest, content-utilization selftest (the decisive gate — presence is not reachability), focused loader tests.

### Continuity checklist result
Assay prose must use real tolerances and procedures consistent with the technical catalogs (the authentic low-tech chemistry voice). No fictional process may contradict its own catalog numbers.

### Open premises
Read `UNCLAIMED_CORPUS_CENSUS.md` content and the two catalogs' field inventories in an owning session; this pass had listing-level access only.

## Subject Plan F-004 — Muster Domain Deep Expansion

Lane A/B · Cluster C7 · Status PROPOSAL.

### Subject
A coordinated content wave across the five live muster catalogs (`muster_camp_scenes`, `muster_epilogues`, `muster_faction_actions`, `muster_faction_culture`, `muster_witnesses`): culture-conditioned faction actions, witness-driven epilogue evidence chains, and camp-scene encounter depth, so the muster subsystem reaches the same integration depth as the verdict and holdfast families.

### Premise evidence
VERIFIED: all five catalogs exist live (DR-04) and are absent from the v1.0 Part 5.4 inventory. VERIFIED: `MusterSystem` and the `Muster` host session exist (v1.0 Part 5.2/5.5); muster epilogues participate in the endgame. VERIFIED: muster witnesses are a named evidence class feeding the epilogue matrix (v1.0 Part 16.6).

### Why this and not something else
Five adjacent catalogs landing without a corresponding bible inventory entry is the strongest signal that a domain expanded faster than its planning scaffold. The factory's job is to catch exactly this.

### What must not change
`FactionStanceEngine` remains the sole standing-effects authority; muster epilogue weight flows through the existing epilogue owners; no new faction ids where existing branch catalogs suffice.

### Recommended integration route
Tier: DATA-ONLY with HOST-WIRING verification. Seams: muster catalog family → existing muster loaders (verify integrity rules cover the newer catalogs — if the loader family predates them, extend it) → `MusterSystem` action selection → witness evidence into the Reckoning enrollment path → muster epilogue evaluation. Save impact class: NONE expected (catalog-driven), unless witness state persists — then EXISTING-SECTION with verification. Verification: integrity + utilization selftests, focused muster tests, one epilogue-reachability test for the new witness chains.

### Continuity checklist result
Culture-conditioned actions must not contradict doctrine behavior (doctrines remain the operational-behavior authority). Witness testimony must obey information-flow legality (a witness can only testify to what they experienced). Epilogue permutations touched must be declared per witness chain.

### Open premises
Confirm the muster loader family consumes all five catalogs; confirm whether witness state persists between sessions.

## Subject Plan F-005 — Epilogue Permutation Coverage Campaign

Lane A/C · Cluster C13 · Status PROPOSAL.

### Subject
A systematic audit of all 32 epilogue permutations for chronicle prose depth and evidence enrollment, followed by authored chronicle depth for the weakest permutations, closing the gap between the mechanically complete epilogue matrix (19A/19B/19C closed) and its narrative coverage.

### Premise evidence
VERIFIED: the 19-wave closeouts in `INTEGRATION_PLANS.md` record the endgame work as complete with evidence (Endgame 84/84 PASS). VERIFIED: `epilogue_chronicle.json`, `campaign_epilogues.json`, `endings.json` exist live. HIGH CONFIDENCE: some permutations carry thinner chronicle prose than others (structural inference from any 32-cell matrix authored incrementally; the audit could not read per-permutation depth at listing level — verify in session).

### Why this and not something else
The endgame is the repository's most durable asset; unequal prose coverage there is player-visible at the highest-stakes moment, and the content is purely additive data through existing catalogs.

### What must not change
Permutation semantics, Reckoning evidence vocabulary, verdict evaluation logic, Crossing ending prose pins (house-voice wording is pinned).

### Recommended integration route
Tier: DOCS-ONLY audit then DATA-ONLY prose. Seams: audit report into `docs/endgame/`; chronicle entries into the epilogue chronicle catalog through its existing loader; enrollment through existing Reckoning owners only where an audit row shows a system whose evidence cannot reach any permutation. Verification: integrity selftest, utilization selftest, focused endgame tests, epilogue-reachability tests for touched permutations.

### Continuity checklist result
Chronicle prose must reveal only scene-appropriate information and must respect the two-pass information rules (post-Reckoning prose may reference Day-360 outcomes; pre-Reckoning prose may not). Every entry names its permutation explicitly.

### Open premises
Per-permutation prose depth must be measured in session before authoring targets are set.

## Subject Plan F-006 — Balance Baseline Refresh Wave

Lane C · Cluster C4, C5, C7, C11, C12, C14 · Status PROPOSAL.

### Subject
Re-run the deterministic economy/balance simulation harness across the content that landed since the last baseline (muster catalogs, moral-choice distress additions, the newest industrial catalogs, XP difficulty authority once W1 seals) and publish deltas in `docs/balance/`, converting the live baseline documents (DR-03) from point-in-time snapshots into a maintained series.

### Premise evidence
VERIFIED: baseline documents exist live (DR-03). VERIFIED: the Plan 76.2 seeded 200-run harness is the established pattern (v1.0 Part 4.3). VERIFIED: multiple content waves have landed since those baselines were generated (DR-06 ledger). DR-07 warns counts drift; baselines drift for the same reason.

### Why this and not something else
Every future Lane C plan is only as good as its baselines; refreshing them is the multiplier for all downstream balance work, and it requires no product-code change.

### What must not change
No tuning values change in this plan; it is measurement only. Any anomalies found become new backlog entries, not in-plan edits.

### Recommended integration route
Tier: TOOLING (harness runs + docs). Seams: existing balance harness; `docs/balance/` publication; anomaly rows feed the Part IV backlog. Verification: two-pass byte-identical determinism proof of each harness run; documented before/after comparison against the prior baseline documents.

### Continuity checklist result
None material; measurement cannot contradict canon.

### Open premises
Locate the harness entry point and its run policy in `TEST_POLICY.md`-governed execution (focused, capped).

## Subject Plan F-007 — Root Coordination Surface Registration

Lane I · Cross-cluster · Status PROPOSAL (docs-only, VERIFIED need).

### Subject
Register the root-level coordination and agent artifacts (DR-01, DR-09: `A1_BRIEFING_DEFERRED.md`, `A1_COORDINATION_RECORD.md`, `WAVE9_PART1_CLOSEOUT.md`, `POTENTIALCLUTTER.md`, `sources.md`, the per-tool rulebooks `CLAUDE.md`, `CODEX.md`, `CRUSH.md`, `GEMINI.md`, `GOOSE.md`, `MIMOCODE.md`, `QWEN.md`, `VIBE.md`, `.clinerules`, `.cursorrules`, `.windsurfrules`, `.zcode/`, plus `Next-steps-plans/`, `piagentsplans/`, `Seal-steps/`, `semantic-review/`) in the docs map, classifying each as active instruction, historical record, or clutter candidate, so planners stop missing live decision surfaces.

### Premise evidence
VERIFIED: all named files exist at root (DR-01, DR-09). VERIFIED: the v1.0 docs map omits them. VERIFIED: `POTENTIALCLUTTER.md` exists, implying an unresolved clutter question.

### Why this and not something else
Every future factory session's premise sweep depends on knowing which surfaces are authoritative; this is infrastructure for all subsequent plans.

### What must not change
No file is moved or deleted in this plan. Classification only; deletion/relocation is a separate decision-gated action.

### Recommended integration route
Tier: DOCS-ONLY. Seams: docs map entry; `docs/INDEX.md` consistency (mind the generated-region rule); a short authority note per artifact. Verification: docs index drift gate; manual review by foreman.

### Continuity checklist result
Classification must describe reality: read each artifact before classifying it; do not infer purpose from filename.

### Open premises
Content of most root artifacts unread in this pass — classification requires an owning session's reads.

## Subject Plan F-008 — Gate-Count Drift Guard

Lane H · Cluster CI/tooling · Status PROPOSAL.

### Subject
A small CI-checkable guard that fails when a documented gate count (in bibles, closeouts, or handoffs) diverges from the live gate inventory, ending the manual count drift demonstrated by DR-07 (47 vs 57 gates; 11,098 vs 11,697 test totals).

### Premise evidence
VERIFIED: the conflicting counts exist across documents (DR-07). VERIFIED: `scripts/ci/` and the version gate are established guard patterns (v1.0 Part 14.2).

### Why this and not something else
DR-07 is a recurring documented workflow problem (every handoff quotes counts by hand); the tool is smaller than the workflow it replaces.

### What must not change
No gate semantics change; the guard only reads and compares.

### Recommended integration route
Tier: TOOLING. Seams: `scripts/ci/` family; the live gate inventory as source of truth; documented counts as compared targets with an allowlist of historical documents exempted (closeouts are historical records and must not be retroactively edited). Verification: the guard's own test — inject a synthetic divergence and confirm failure; run in `verify-fast.sh` mirror.

### Continuity checklist result
Historical documents are immutable records; the guard must exempt or annotate them rather than force edits.

### Open premises
The live gate inventory format must be read in session to design the comparison key.

## Subject Plan F-009 — `rewrite.py` Data-Authority Hygiene

Lane H · Cluster data authority · Status VERIFIED finding, PROPOSAL handling.

### Subject
Determine what `Assets/StreamingAssets/Data/rewrite.py` rewrites and who invokes it; then either relocate it to `scripts/`/`tools/` with a call-site update, or document in place why it must live beside the catalogs it rewrites.

### Premise evidence
VERIFIED: the file exists inside the data authority (DR-05), which `AGENTS.md` rule 3 defines as the authored JSON data authority.

### Why this and not something else
A process script inside the content authority is a standing trap for every future content tool and for the asset gate; the repository's own hygiene standards demand resolution.

### What must not change
The script's rewrite behavior must be preserved byte-for-byte in effect; this is placement hygiene, not a rewrite of the rewriter.

### Recommended integration route
Tier: TOOLING. Seams: call-site search first (search_code across the repo for invocations); relocation with an entry in the tooling docs; a CI assertion that `Assets/StreamingAssets/Data/` contains only `.json` plus whitelisted non-JSON artifacts thereafter. Verification: data-integrity selftest before and after; the new CI assertion green.

### Continuity checklist result
None material beyond call-site honesty.

### Open premises
The script's content and callers are unread in this pass; do not move anything before reading them.

## Subject Plan F-010 — Unclaimed-Content Census Feed

Lane A/B/H · Cross-cluster · Status PROPOSAL (process canon).

### Subject
Formalize `docs/plans/UNCLAIMED_CORPUS_CENSUS.md` as a standing factory input: every census row naming authored content with no consuming system becomes a wiring candidate with a utilization-gated acceptance criterion, and the census is regenerated as part of each wave's closeout.

### Premise evidence
VERIFIED: the census document exists (DR-08). VERIFIED: the content-utilization selftest exists and encodes the principle "presence in JSON is not reachability" (v1.0 Invariant 6).

### Why this and not something else
The census is already the correct instrument; the plan only closes the loop between measurement and planning, which is the definition of a recurring workflow problem worth automating.

### What must not change
The selftest remains the enforcement authority; the census is its human-readable planning view.

### Recommended integration route
Tier: TOOLING + PROCESS. Seams: census regeneration hook in the wave closeout template; Part IV backlog ingestion rule (this document already consumes it in Factory Protocol step 1). Verification: after one wave, the census shrinks or every remaining row carries an owned reason.

### Continuity checklist result
Census rows must not be "resolved" by deletion; only by wiring or by a recorded decision.

### Open premises
Census content unread in this pass.

## Subject Plan F-011 — C2 Open-Gap Package

Lane B/F/G · Cluster C2, C8, events · Status VERIFIED as open; scope per item.

### Subject
Three bounded follow-ons that the integration ledger itself records as real, unexecuted gaps (DR-06): Plan 31 semantic-kind authority; 17C Phase I alert ducking/concurrency and Phase E acquisition sweep; 17B deep test matrix.

### Premise evidence
VERIFIED: `INTEGRATION_PLANS.md` records these under the C2[2] Plan 17 closure as "NOT executed (real gaps, need packages)".

### Why this and not something else
These are pre-admitted by the repository's own ledger — the factory adds nothing; it only surfaces the ledger's own to-do list in plan form.

### What must not change
The 17A-S semantic parity matrix and its gate; the already-built 17B route/visibility and most 17C audio phases (stale per recon — do not rebuild what exists).

### Recommended integration route
Tier: per item (Plan 31 is a Core authority question; 17C Phase I is host audio routing; 17B is testing). Seams per the C2 closure report `docs/plans/C2_PLANINTEGRATION_2_CLOSURE_REPORT.md`. Verification per item, focused.

### Continuity checklist result
Each item must re-verify its premise in live source before work begins (ledger statements are claims, not proof — `AGENTS.md` rule 7).

### Open premises
The three plan documents' internals are unread in this pass; scoping requires them.

## Subject Plan F-012 — Dive-Site and Hydroponic Domain Expansion Audit

Lane A/B · Cluster C3, C5 · Status INFERENCE pending sweep.

### Subject
A two-domain premise sweep converting DR-04's inventory findings into openings: `dive_sites.json` (against the canon hourly dive-oxygen-drain system and the deep-coast family) and `hydroponic_crops.json` (against the greenhouse/aquaponics/aeroponics families), each audited for narrative corpus coverage, economy legs, and cross-system bridges (dive → medical/ARS via immersion exposure rules; hydroponics → morale via fresh-food rules if such rules exist).

### Premise evidence
VERIFIED: both catalogs exist live and are absent from the v1.0 inventory (DR-04). VERIFIED: `District8DeepCoastSystem`, dive oxygen drain (hourly tick class), and the greenhouse host session exist (v1.0 Parts 3.2, 5.2, 5.5). INFERENCE: both domains are under-expanded relative to their neighbors — this is precisely what the sweep must confirm or refute.

### Why this and not something else
Both are verified inventory facts whose expansion state is unknown; the sweep is cheap and either yields two plan batches or two documented no-change areas (both are acceptable factory outcomes).

### What must not change
Deep-coast absolute route-block semantics; dive oxygen drain as an hourly system; no parallel crop authority outside the greenhouse owner.

### Recommended integration route
Tier: DOCS-ONLY audit, then DATA-ONLY per confirmed opening. Verification: sweep report; then integrity + utilization selftests for any authored tranche.

### Continuity checklist result
Dive content must respect waterborne exposure rules and deep-coast canon; hydroponic content must respect crop-strain genome rules.

### Open premises
Both catalogs' contents, loaders, and consumers are unread in this pass.


---

# VOLUME 2 — SUBJECT SEED CATALOG, Lanes A–E (Factory batch 2026-09-24-C)

Each seed is a compressed subject plan: subject, evidence anchor, and route tier. Seeds are consumed through the Factory Protocol (Part II): premise sweep, then Template S expansion, then Template R route. Evidence anchors cite the live repository (verified in the 2026-09-24 audit or carried from the v1.0 bible's verified inventories). Seeds marked GATE require a named signature before drafting; seeds marked SEALED sit on closed surfaces and open only with new evidence plus foreman signature. Every seed carries the standing constraints of Part 0.4 v1.0: no parallel authority, no new ids without collision sweep, no wall-clock randomness, owner-first extension.

## 2.1 Lane A — Narrative and prose seeds (A-01 … A-30)

**A-01 · C1 · Bunker maintenance glitch batch N+1.** Subject: additional `bunker_maintenance_glitches` batches for shelter rooms that gained systems in Waves 8–12 (EMP feed, medical power, grid catalog seal — logs observed in `docs/plans/`). Evidence: glitch batches 2 and 3 exist in the narrative corpus; room coverage is enumerable from `shelter_rooms.json`. Route: DATA-ONLY into the existing glitch corpus family. Confidence: HIGH CONFIDENCE.

**A-02 · C1 · Load-shed schedule amendments tied to the sanitation power-grid feed.** Subject: amendment notices following the sanitation `RoomPowerProvider` seam (sanitation power-grid feed landed per `FOLLOWUPS-210-213-THINSEAMS`). Evidence: `load_shed_schedule_001` exists in the corpus; the power feed seam is verified via the followups package. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.

**A-03 · C2 · Casebook expansion for ARS latent-to-manifest phases.** Subject: medical casebook entries mirroring the authored ARS phase structure (latent-to-manifest, multi-day cadence, v1.0 Part 3.2). Evidence: `dweller_medical_casebook` exists; ARS pathology systems are canon. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.

**A-04 · C2 · Dose-treatment narrative pairing.** Subject: therapy-note and casebook twins for each row of `MEDICAL_DOSE_TREATMENT_MATRIX.md` (live, DR-03), so every mechanical treatment has a clinical-document voice. Evidence: matrix document verified live. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.

**A-05 · C2 · Therapist session notes batch 4.** Subject: a fourth batch keyed to guilt sources and insomnia states added since batch 3. Evidence: therapist batches 1–3 exist in the corpus; guilt sources and guilt insomnia are canon systems. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.

**A-06 · C3 · Preservation and processing assay twins.** Subject: assay/log corpus entries for every `food_preservation.json` and `grain_processing.json` process lacking a narrative twin — the Part 16.4 pattern applied to the food domain. Evidence: both catalogs verified live. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.

**A-07 · C3 · Root-cellar and silo follow-on field logs.** Subject: additional humidity-rot and weevil-audit entries conditioned on seasonal windows. Evidence: `root_cellar_humidity_rot_reports` and `grain_silo_weevil_audits` exist in the corpus; seasonal calendar is a canon system. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.

**A-08 · C3 · Apiculture assay continuation.** Subject: Langstroth foundation-log continuation tied to seasonal yield and morale. Evidence: `langstroth_hive_foundation_logs` exists; apiculture is canon in Part 16.3. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.

**A-09 · C4 · Hydraulic extrusion assay corpus twin.** Subject: ram-pressure and die-wear assay records for the live-but-unmapped hydraulic extrusion catalog (DR-04). Evidence: catalog verified live; corpus twin status unverified. Route: DATA-ONLY after census check. Confidence: HIGH CONFIDENCE (catalog) / UNVERIFIED (twin absence).

**A-10 · C4 · Metrology standards calibration corpus.** Subject: calibration certificates and gauge-discrepancy reports for `metrology_standards_catalog.json` (DR-04), in the low-background metrology voice. Evidence: catalog verified live; `LowBackgroundMetrology` host session exists. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.

**A-11 · C4 · Foundry pour-window log continuation.** Subject: cupola pour records conditioned on accord state and treaty consequences. Evidence: foundry corpus exists (`forge_charcoal_ash_assays`, kiln records); `foundry_accords.json` and `foundry_treaty_consequences.json` verified live. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.

**A-12 · C5 · Destination arrival/revisit prose completion.** Subject: `arrival_description` and `revisit_description` completion for any of the 53 dispatchable destinations with sparse prose, following the Part 9 field contracts exactly (80–140 / 60–110 words, one landmark, one sensory anchor, one danger indication). Evidence: destination surface is canon (53 destinations, 263-id dispatch surface). Route: DATA-ONLY. Confidence: HIGH CONFIDENCE that gaps exist; measure per destination in session.

**A-13 · C5 · Waystation register prose.** Subject: guest-register entries and way-notice sheets for the waystation family. Evidence: `waystations.json` verified live. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.

**A-14 · C5 · Rail-side field documents.** Subject: track-walker notes and interlock violation reports for the rail family (grinding, logistics, interlock, rerailing catalogs all live). Evidence: four rail catalogs verified live; narrative corpus has no rail track-walker family in the Part 5.6 inventory — collision sweep required against the full corpus index (Part 18 of v1.0, truncated in the source upload — verify in session). Route: DATA-ONLY. Confidence: INFERENCE pending corpus sweep.

**A-15 · C6 · Damaged-zone survey marginalia.** Subject: surveyor marginalia for `damaged_map_zones.json` entries, in the geodetic voice. Evidence: catalog verified live; `GeodeticSurveyHostSession` exists. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.

**A-16 · C7 · Standing-record testimony depth.** Subject: witness-statement and registry-annotation prose expanding `standing_record_memory.json` coverage. Evidence: the standing-record family (factions, layouts, memory, quests) is verified live and is a canon epilogue evidence source. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.

**A-17 · C7 · Verdict radio continuation.** Subject: verdict-station rundown batches conditioned on verdict questline state. Evidence: `verdict_radio.json` verified live; verdict questlines are canon. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.

**A-18 · C7 · Warlord doctrine communiqués.** Subject: doctrine-conditioned communiqué and tribute-demand prose for warlords whose public/private language separation is thin. Evidence: `warlord_doctrines.json`, `faction_war_communiques.json` verified live. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.

**A-19 · C8 · Numbers-station cipher continuation.** Subject: additional cipher sequences with solvable kernels tied to existing intercept content. Evidence: `numbers_station_ciphers` exists in the corpus. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.

**A-20 · C8 · Radio program rundown expansion.** Subject: rundown batches for stations with thin programming against `radio_programs.json` and `radio_stations.json`. Evidence: both catalogs verified live. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE. Note: distress-signal content is SEALED (`CF-P1-DISTRESS-CONTENT-SEAL`); rundowns must not add signal scenarios.

**A-21 · C9 · Phantom-memory trigger expansion keyed to surviving cohorts.** Subject: heirloom-trigger entries conditioned on cohort survival state, deepening the generational line. Evidence: `phantom_heirlooms.json`, `phantom_triggers.json` verified live; cohort/lineage systems are canon. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.

**A-22 · C9 · Final-wishes document corpus.** Subject: unsent-letter and testament prose for `final_wishes.json` entries lacking document twins. Evidence: catalog verified live; `unsent_letters_batch_2` demonstrates the genre. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.

**A-23 · C9 · Intake interview continuation.** Subject: new-arrival intake interviews conditioned on the arrival channels that exist (rescue, crossing, holdfast). Evidence: `new_arrival_intake_interviews` exists; arrival channels are canon. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.

**A-24 · C10 · Bureaucratic-morality quest prose completion.** Subject: prose-field completion across `quests_bureaucratic_morality.json` records with skeleton `quest_hook`/outcome texts. Evidence: catalog verified live; Part 9 contracts define the fields. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.

**A-25 · C10 · Massive-expansion corpus prose audit.** Subject: a prose-depth audit of `quests_massive_expansion_200.json` (200 records — the largest single prose debt surface in the data authority), converting skeleton records into contracted fields over several tranches. Evidence: catalog verified live; scale is structural evidence of thin per-record prose. Route: DATA-ONLY, multi-tranche. Confidence: HIGH CONFIDENCE.

**A-26 · C11 · Ledger-debt statement prose.** Subject: debtor statements and collection notices for `ledger_debt_templates.json` rows. Evidence: catalog verified live; `LedgerDebtSystem` with consequence dispatchers is canon. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.

**A-27 · C12 · Storm-window almanac entries.** Subject: almanac prose conditioned on `year_of_ash_storm_windows.json` entries. Evidence: catalog verified live; `weather_almanac_expansion` exists in the corpus. Route: DATA-ONLY, within the Year-of-Ash window (180–360) canon. Confidence: HIGH CONFIDENCE.

**A-28 · C13 · Under-served epilogue chronicle depth.** Subject: consumed by F-005 after the permutation audit selects the weakest cells. Evidence: matrix is canon (32 permutations). Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.

**A-29 · C14 · Bestiary natural-history continuation.** Subject: sighting-log and specimen-record prose for bestiary entries with thin coverage. Evidence: `wasteland_wildlife_bestiary.json` verified live; vulture-sighting and cockroach-hive log genres exist. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.

**A-30 · C15 · Sky-defense ordnance manifest prose.** Subject: armor-layer inspection records and ordnance manifests for the sky-defense family. Evidence: `sky_defense_ordnance.json`, `sky_layer_armor_catalog.json` verified live; `SkyDefense` host session exists. Route: DATA-ONLY. Confidence: INFERENCE pending corpus sweep for an existing sky-defense document family.

## 2.2 Lane B — Mechanics and systems functionality seeds (B-01 … B-25)

**B-01 · C1 · Shelter-failure follow-on effects.** Subject: extend the quarantined shelter-failure-effects wiring (logs observed: `SHELTER_FAILURE_EFFECTS_QUARANTINE_WIRING_*`) from quarantine into full cascade coverage through `cascade_rules.json`. Evidence: implementation logs verified in `docs/plans/`. Route: CORE-EXTENSION + data, per the existing quarantine plan's own exit criteria. Confidence: HIGH CONFIDENCE that the quarantine exists; read its exit criteria in session.

**B-02 · C1 · Shelter grid catalog seal follow-through.** Subject: complete any consumer bindings left open by the shelter grid catalog seal (log observed: `SHELTER_GRID_CATALOG_SEAL_*`). Evidence: logs verified live. Route: HOST-WIRING per seal plan. Confidence: HIGH CONFIDENCE the seal exists; scope unverified.

**B-03 · C2 · Dose ledger ↔ Year-of-Ash fallout-window coupling.** Subject: fallout-window-conditioned dose accrual deepening, so storm windows (180–360) measurably raise exposure risk on unprotected travel and work. Evidence: `DoseLedgerSystem`, `fallout_patterns.json`, storm windows all canon. Route: CORE-EXTENSION (existing dose owner) + data. Determinism: existing seeded streams. Confidence: PROPOSAL — verify current coupling depth first.

**B-04 · C2 · Child-health cohort bridge.** Subject: child survivors' health needs feeding the medical pipeline through the cohort system's scoped links (19B closeout records child rations and schooling links). Evidence: 19B closeout verified via ledger (DR-06). Route: CORE-EXTENSION through cohort and medical owners. Confidence: PROPOSAL.

**B-05 · C3 · Preservation × disease contamination bridge.** Subject: failed or rushed preservation producing contamination exposure through the existing disease/pathogen seams (zoonosis bridge is the model). Evidence: food preservation authority map exists; zoonosis bridge is canon. Route: CORE-EXTENSION + data. Confidence: PROPOSAL.

**B-06 · C4 · XP difficulty consumer binding for industrial chains.** Subject: once XP W1's difficulty authority seals, bind industrial fuel/feedstock consumption scalars to it (the sanctioned difficulty seam — never parallel scalars). Evidence: W1 ACTIVE (DR-06). Route: CORE-EXTENSION after seal. Confidence: HIGH CONFIDENCE, sequence-gated on W1.

**B-07 · C5 · Vehicle-breakdown medical/dose consequences.** Subject: expedition vehicle breakdowns producing injury and exposure events routed into medical and dose ledgers (extending `ExpeditionVehicleSystem` consequence routing). Evidence: `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md` and vehicle armor grades verified live; dominance table implies breakdown modeling exists. Route: CORE-EXTENSION. Confidence: PROPOSAL — verify current breakdown consequence routing first.

**B-08 · C5 · Scavenging-table parity for uncovered destinations.** Subject: complete per-destination renewable/one-time table coverage where the 49-table surface underserves the 53-destination catalog. Evidence: 49 vs 53 is canon (v1.0 Part 6.2). Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.

**B-09 · C6 · Flooded-route topology tags.** Subject: authored map edges carrying flood tags consumable by route gates. Evidence: foreman-flagged open decision requiring authored map edges (v1.0 Part 7 gap 4). Status: GATE. Route: DATA-ONLY + gate consumers. Confidence: VERIFIED as open; blocked on signature.

**B-10 · C7 · FactionWar per-strike emitter extension.** Subject: per-strike event emission for the faction war chain. Evidence: foreman-flagged open decision (v1.0 Part 7 gap 4). Status: GATE. Route: CORE-EXTENSION. Confidence: VERIFIED as open; blocked on signature.

**B-11 · C7 · Black-market funds legs.** Subject: canonical funds authority for black-market settlement. Evidence: decision-blocked (needs canonical funds authority — v1.0 Part 7 gap 4); the actions surface itself was sealed by `WAVE8-PART2-C1-BLACK-MARKET-ACTIONS` (DR-06). Status: GATE. Confidence: VERIFIED as blocked.

**B-12 · C8 · Market-rumor band extension.** Subject: extend deterministic rumor bands (kind 6, `EconomyMarketRumorRules`) with new commodity coverage. Evidence: rumor bridge landed per the followups package (DR-06); bands are deterministic by canon. Route: CORE-EXTENSION + data. Confidence: HIGH CONFIDENCE.

**B-13 · C8 · Intercept-driven journal depth.** Subject: faction intercepts producing journal records conditioned on signal authenticity patterns already sealed. Evidence: intercept catalog verified live; sealed authenticity evaluator is the seam. Route: HOST-WIRING. Confidence: PROPOSAL.

**B-14 · C9 · Belief-movement ↔ faction-stance bridge.** Subject: belief movement membership shifting faction standing through `FactionStanceEngine`. Evidence: `belief_movements.json` verified live; stance engine is the sole standing authority. Route: CORE-EXTENSION. Confidence: PROPOSAL.

**B-15 · C9 · Memorial-rite epilogue evidence enrollment.** Subject: performed rites enrolling as Reckoning evidence (rites exist; evidence vocabulary must be checked for a rite class before authoring). Evidence: `memorial_rites.json`, `spiritual_rituals.json` verified live. Route: CORE-EXTENSION through endgame owners. Confidence: PROPOSAL — evidence vocabulary check first.

**B-16 · C10 · Quest reopening after new discoveries.** Subject: failed/abandoned quests reopening when discovery conditions later satisfy (the failure-recovery grammar of v1.0 Part 6.7). Evidence: abandoned-quest reopen is canon grammar; implementation state unverified. Route: CORE-EXTENSION through quest owners. Confidence: PROPOSAL.

**B-17 · C10 · Moral-choice gossip propagation depth.** Subject: choice-driven gossip traveling the modeled channels with time lag proportional to distance. Evidence: `moral_choice_gossip.json` verified live; information-flow rules are canon. Route: CORE-EXTENSION. Confidence: PROPOSAL.

**B-18 · C11 · Trade-screen scenario expansion.** Subject: additional scenarios and tell lines for under-covered merchant identities. Evidence: `trade_screen_scenarios.json`, `trade_tell_lines.json`, `trade_specialties.json` verified live. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.

**B-19 · C12 · Winter pressure for power and water systems.** Subject: Year-of-Ash window (180–360) pressure extensions for systems that currently produce no winter-specific cost (filter burn, diesel reserve drawdown curves conditioned on storm windows). Evidence: storm windows canon; hardening upgrades catalog live. Route: CORE-EXTENSION + data. Confidence: PROPOSAL.

**B-20 · C13 · Reckoning evidence enrollment sweep.** Subject: audit systems added since the 19-wave for evidence enrollment gaps; enroll through the existing Reckoning path only. Evidence: 19C closed (DR-06); subsequent waves exist (Waves 8–12 logs). Route: audit then CORE-EXTENSION. Confidence: HIGH CONFIDENCE.

**B-21 · C14 · Migration ↔ route-encounter bridge.** Subject: wildlife migration state conditioning travel-encounter selection on routes crossing migration corridors. Evidence: `WildlifeMigrationSystem`, `travel_encounters.json` both canon. Route: CORE-EXTENSION. Confidence: PROPOSAL.

**B-22 · C15 · Defense-grid ↔ siege math.** Subject: perimeter defense values entering warlord siege/raid resolution; sky-armor values entering orbital-harrow telemetry thresholds. Evidence: warlord siege math and orbital harrow telemetry are canon systems; catalogs live. Route: CORE-EXTENSION. Confidence: PROPOSAL.

**B-23 · C16 · Difficulty binding consumers (CF-XP01).** Subject: complete difficulty preset scalar consumer binding across systems — the ledger records this line as available and W1-reinforced. Evidence: DR-06. Route: CORE-EXTENSION through the difficulty authority only. Confidence: HIGH CONFIDENCE.

**B-24 · C17 · Stale-panel refresh sweep.** Subject: sweep panels whose data source gained fields since the panel shipped (verify via `--ui-layout-selftest` and snapshot coverage, then expose truthful current state). Evidence: UI selftest surface is canon. Route: HOST-WIRING only, zero gameplay authority. Confidence: HIGH CONFIDENCE that the class exists; per-panel verification required.

**B-25 · C8/C2 · Rescue-remains medical follow-through.** Subject: remains-recovery branch (sealed runtime) feeding disease/contagion exposure rules where the campaign handles remains. Evidence: sealed sender-survival branch includes remains/salvage (DR-06 handoff); contagion events catalog live. Route: CORE-EXTENSION on sealed seams — additive only. Status: coordinate with the sealed surface's owners. Confidence: PROPOSAL.

## 2.3 Lane C — Economy and balance seeds (C-01 … C-14)

**C-01 · C4 · Industrial chain income-versus-expenditure audit.** Subject: per-chain fuel/feedstock/labor cost versus output value at current regional prices, flagging dominated processes (any process whose output is strictly cheaper to buy than to make is a candidate for tuning or intentional scarcity framing). Evidence: all industrial catalogs + `regional_prices.json` + `commodity_baselines.json` verified live. Route: TOOLING (harness) + report. Confidence: HIGH CONFIDENCE.

**C-02 · C4 · SOFC fuel-consumption sustainability audit.** Subject: real inventory fuel consumption (sealed per Plan 125) versus expected fuel income across difficulty presets. Evidence: Plan 122/125 closeouts verified via ledger. Route: harness + report. Confidence: HIGH CONFIDENCE.

**C-03 · C5 · Scavenging E[value] re-run.** Subject: Plan 76.2-pattern seeded re-run after any loot-affecting wave; publish deltas. Evidence: harness pattern is canon. Route: TOOLING. Confidence: HIGH CONFIDENCE.

**C-04 · C5 · Vehicle dominance follow-up.** Subject: re-evaluate `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md` against new vehicle modifications/armor grades. Evidence: table verified live (DR-03). Route: harness + report. Confidence: HIGH CONFIDENCE.

**C-05 · C7 · Tribute sustainability per doctrine.** Subject: 7-day tribute cadence versus mid-game income across doctrines and difficulty presets; identify mathematically unsustainable demand spirals versus intended harshness. Evidence: doctrine catalog live; cadence canon. Route: harness + report. Confidence: HIGH CONFIDENCE.

**C-06 · C7 · Embargo pressure modeling.** Subject: `trade_embargoes.json` impact on settlement price bands; verify embargoes produce legible price signal, not noise. Evidence: embargo catalog verified live. Route: harness. Confidence: HIGH CONFIDENCE.

**C-07 · C11 · Debt-interest runaway analysis.** Subject: compound debt trajectories versus realistic income; identify unrecoverable debt states and confirm each has a recovery path (the recovery grammar of v1.0 Part 6.7). Evidence: `LedgerDebtSystem` with interest is canon. Route: harness + report. Confidence: HIGH CONFIDENCE.

**C-08 · C11 · Black-market price-tier audit.** Subject: black-market inventory pricing versus legal market bands and scarcity premiums. Evidence: `black_market_inventory.json` verified live. Route: harness. Confidence: PROPOSAL.

**C-09 · C12 · Winter resource compression audit.** Subject: Days 90–180 and 180–360 calorie/fuel/filter/morale sustainability per difficulty preset (sustainability-day math). Evidence: baselines live (DR-03). Route: harness. Confidence: HIGH CONFIDENCE.

**C-10 · C14 · Trapping yield versus degradation cost.** Subject: `wildlife_trapping_catalog.json` yields against equipment condition degradation; uncooked-yield zoonosis risk premium. Evidence: trapping and zoonosis bridge canon. Route: harness. Confidence: HIGH CONFIDENCE.

**C-11 · C16 · Difficulty preset spread audit.** Subject: once W1 seals, audit that preset scalars produce distinct, legible difficulty curves rather than uniform multipliers. Evidence: W1 ACTIVE (DR-06). Route: harness, post-seal. Confidence: PROPOSAL, sequence-gated.

**C-12 · C3 · Greenhouse/aeroponics yield economics.** Subject: crop yield value versus power/water/nutrient inputs across the three cultivation families. Evidence: all three catalogs verified live. Route: harness. Confidence: PROPOSAL.

**C-13 · C11 · Quantity-band trim extension.** Subject: where E[value] outliers persist, extend quantity-band trims (the established remedy class). Evidence: band trims are the canon remedy (v1.0 Lane C). Route: DATA-ONLY after C-01/C-03 evidence. Confidence: HIGH CONFIDENCE, evidence-gated.

**C-14 · Cross · Time-to-kill and combat economy audit.** Subject: `combat_catalog.json` damage/armor cadence versus ammunition scarcity across difficulty presets. Evidence: combat catalog and hardcore tuning live. Route: harness. Confidence: PROPOSAL.

## 2.4 Lane D — Save, state, and compatibility seeds (D-01 … D-08)

**D-01 · Cross · Mid-event round-trip sweep.** Subject: extend exactly-once round-trip guarantees (rescue-runtime pattern: persisted first-result, exactly-once guards, duplicate arrival guards) to other exactly-once effect classes (one-time caches, bounty claims, unique-item claims). Evidence: pattern sealed in the rescue runtime; `UniqueItemClaimRegistry` exists. Route: CODEC-BUMP-AND-MIGRATE where state must persist; test matrix per v1.0 Part 12.3. Confidence: PROPOSAL per class.

**D-02 · C9 · Lineage horizon coverage.** Subject: verify 3-year simulation coverage extends to the Day-3650 generational horizon for lineage facts; extend simulation fixtures if not. Evidence: 19B records a 3-year deterministic simulation (DR-06); canon horizon is 3650 days. Route: test-fixture extension. Confidence: PROPOSAL.

**D-03 · C13 · Epilogue evidence persistence window.** Subject: confirm Day-360+ enrolled evidence survives into the Day-3650 window evaluation; add fixture tests for each evidence class. Evidence: epilogue matrix canon. Route: tests + possible migration. Confidence: PROPOSAL.

**D-04 · Cross · Unknown-field tolerance audit.** Subject: fixture tests proving unknown fields are tolerated and missing fields defaulted explicitly for every codec touched by recent waves (registry-driven, per `SAVE_STORE_CONTRACT_MATRIX.md`). Evidence: contract matrix is generated and maintained. Route: tests. Confidence: HIGH CONFIDENCE.

**D-05 · Cross · Save support window re-pin.** Subject: re-pin the support window after the next release-class version bump. Evidence: window is pinned per release by canon. Route: tests + release process. Confidence: HIGH CONFIDENCE, release-gated.

**D-06 · C1 · Shelter failure mid-event save semantics.** Subject: define and test mid-failure-event save/restore behavior for the quarantine-exited failure cascades (B-01 dependency). Evidence: quarantine logs verified. Route: design + tests. Confidence: PROPOSAL.

**D-07 · C10 · Moral-choice flag persistence audit.** Subject: confirm every authored flag id persists and round-trips; default-tolerant missing-flag handling for older saves. Evidence: flags catalog live; F-001 depends on this. Route: tests. Confidence: HIGH CONFIDENCE as an audit; outcomes may be NONE.

**D-08 · C16 · Completion-history difficulty stamp integrity.** Subject: verify the difficulty-preset stamp (schema v2) migrates cleanly when W1 adds preset fields. Evidence: stamp is canon (v1.0 Part 16.7). Route: migration + tests, W1-coordinated. Confidence: PROPOSAL, sequence-gated.

## 2.5 Lane E — UI, UX, and accessibility seeds (E-01 … E-10)

**E-01 · C17 · Briefing surface for post-19-wave systems.** Subject: daily-briefing entries for systems landed in Waves 8–12 that produce player-relevant state but no briefing row. Evidence: briefing surface is canon (`DailyBriefing` panel). Route: HOST-WIRING only. Confidence: HIGH CONFIDENCE that the class exists; enumerate in session.

**E-02 · C17 · Snapshot coverage for newest panels.** Subject: extend snapshot coverage to panels shipped since the last snapshot wave. Evidence: snapshot coverage doc is generated and gate-enforced. Route: snapshots + gates. Confidence: HIGH CONFIDENCE.

**E-03 · C16 · Difficulty preset selection surface.** Subject: preset selection UI bound to the W1 difficulty authority (post-seal, coordinated). Evidence: W1 ACTIVE. Route: HOST-WIRING. Confidence: PROPOSAL, sequence-gated.

**E-04 · C8 · Radio strip extension points.** Subject: any new radio-conditioned content surfaces through the existing RESCUE SIGNALS strip seams — additive only, sealed surface respected. Evidence: strip shipped and sealed (DR-06). Route: HOST-WIRING. Status: SEALED-adjacent. Confidence: HIGH CONFIDENCE constraint.

**E-05 · C17 · A11y words-not-color-only sweep for newer panels.** Subject: audit newer panels for color-only state signaling; add textual state wherever found. Evidence: `ACCESSIBILITY.md` and the a11y gate are canon. Route: HOST-WIRING. Confidence: HIGH CONFIDENCE that the sweep is warranted.

**E-06 · C17 · Controller parity for new panels.** Subject: focus-navigator parity for panels shipped without full 22-action-map coverage. Evidence: input contract canon (22-action map). Route: HOST-WIRING. Confidence: HIGH CONFIDENCE.

**E-07 · C5 · Expedition camp panel depth.** Subject: expose truthful existing expedition state that the camp panel does not yet render (verify via layout selftest before claiming). Evidence: expedition camp panel exists (v1.0 Part 5.7). Route: HOST-WIRING. Confidence: INFERENCE pending selftest.

**E-08 · C11 · Caravan barter ledger legibility.** Subject: `CaravanBarterLedger` panel state completeness against the sealed merchant-restock display-order priority (DEC-05). Evidence: DEC-05 sealed (DR-06). Route: HOST-WIRING. Confidence: PROPOSAL.

**E-09 · C12 · Storm-window forecast legibility.** Subject: weather forecast surface rendering storm-window warnings with adequate lead time for the 180–360 window. Evidence: forecast observation is a canon loop step. Route: HOST-WIRING. Confidence: PROPOSAL.

**E-10 · C13 · Reckoning evidence submission surface.** Subject: submission UX for enrolled evidence classes that lack a clear submission affordance (audit first). Evidence: Reckoning consumes enrolled evidence by canon. Route: HOST-WIRING. Confidence: PROPOSAL.


---

# VOLUME 3 — SUBJECT SEED CATALOG, Lanes F–J, AND SUBSYSTEM DEEP MAPS (Factory batch 2026-09-24-D)

## 3.1 Lane F — Performance seeds (F-01 … F-06)

**F-01 · C17 · Per-frame UI allocation audit.** Subject: measure per-frame allocations in the shell components (dashboard shell, metric cards, data grids) during a 15-FPS headless session; only optimize what the profiler demonstrates. Evidence: 15-FPS runtime test sessions are canon (v1.0 Part 14.1); `Performance/` Core and the CI performance gate exist. Route: measurement harness, then targeted repair with before/after numbers in `docs/perf/`. Confidence: potential hotspot — requires profiling.

**F-02 · C12 · Storm-window tick concentration.** Subject: measure per-day tick cost spikes inside storm windows (Days 180–360) where weather, fallout, route gates, and morale effects co-fire. Evidence: window canon; co-firing systems canon. Route: profiler comparison across window/non-window days. Confidence: potential hotspot — requires profiling.

**F-03 · C13 · Epilogue-matrix evaluation cost.** Subject: one-shot Day-360 evaluation cost across 32 permutations plus the Day-3650 pass; likely negligible, measure only if reported slow. Evidence: matrix canon. Route: one measurement, likely a no-change area. Confidence: HYPOTHESIS.

**F-04 · C1 · Room tree-search frequency.** Subject: count repeated node/path lookups in shelter systems during a 30-day simulation; the atlas flags repeated tree searches as a candidate class. Evidence: 30-day simulation patterns exist (shelter maintenance report, expedition playtest). Route: instrumentation run. Confidence: potential hotspot — requires profiling.

**F-05 · C8 · Radio dial per-frame work.** Subject: measure SNR dial work at 15 FPS during active tuning; the dial is one of the canon real-time/frame surfaces. Evidence: real-time tier is canon (v1.0 Part 3.2). Route: profiling pass. Confidence: potential hotspot — requires profiling.

**F-06 · Cross · Save-flush cost at day tick.** Subject: measure daily save-flush duration against tick budget for large late-game states (many survivors, full dose ledger, long journals). Evidence: daily save flush is a canon tick step. Route: measurement with a synthetic late-game fixture. Confidence: potential hotspot — requires profiling.

## 3.2 Lane G — Testing seeds (G-01 … G-08)

**G-01 · C10 · Moral-choice flag consumer coverage.** Subject: tests proving every authored flag id has at least one consumer path and every consumer reads a persisted flag (supports F-001 and D-07). Evidence: flags catalog live. Route: focused xUnit, aggregate with per-row failures. Confidence: HIGH CONFIDENCE.

**G-02 · C11 · Debt-consequence dispatcher coverage.** Subject: dispatcher coverage for every consequence kind in the closed vocabulary, with recovery-path assertions. Evidence: dispatchers canon; recovery grammar canon. Route: focused xUnit. Confidence: HIGH CONFIDENCE.

**G-03 · C2 · Dose-treatment matrix pairing tests.** Subject: pin each matrix row to its implementing treatment logic so the generated matrix cannot drift from code. Evidence: matrix live (DR-03). Route: focused xUnit + generation check. Confidence: HIGH CONFIDENCE.

**G-04 · C13 · Epilogue permutation reachability suite.** Subject: deterministic tests that each permutation is reachable from some authored campaign state and that no optional content can invalidate the main ending (hard world rule). Evidence: matrix and rule canon. Route: deterministic simulation tests. Confidence: HIGH CONFIDENCE.

**G-05 · C7 · War-chain authored-day mapping tests.** Subject: pin the 300-day offset mapping (playable 180 → authored 480) with boundary tests. Evidence: mapping is canon (`FactionWarChainRunner.ToAuthoredDay`). Route: focused xUnit. Confidence: HIGH CONFIDENCE.

**G-06 · C8 · Exactly-once guard regression suite.** Subject: regression tests covering every sealed exactly-once guard class (ignore consequences, arrival resolution, salvage grants) against restore-mid-effect saves. Evidence: sealed runtime models the guards (DR-06). Route: focused xUnit + fixture saves. Confidence: HIGH CONFIDENCE.

**G-07 · C12 · Two-pass determinism proof for any new winter simulation.** Subject: any Lane B/C12 plan shipping new simulation logic must ship the byte-identical two-pass proof (the Plan 76.2 pattern). Evidence: pattern canon. Route: mandatory plan component. Confidence: CANON process.

**G-08 · C16 · XP W1 consumer-binding test wave.** Subject: for every consumer bound under B-23, a focused test that the scalar flows from the authority to the consumer and no parallel scalar exists. Evidence: W1 ACTIVE. Route: focused xUnit per consumer, post-binding. Confidence: HIGH CONFIDENCE, sequence-gated.

## 3.3 Lane H — Tooling seeds (H-01 … H-07)

**H-01 · Data-authority non-JSON assertion.** Subject: CI assertion that `Assets/StreamingAssets/Data/` contains only JSON plus whitelisted artifacts (consumes the F-009 resolution). Evidence: DR-05. Route: small CI script in the established gate family. Confidence: VERIFIED need.

**H-02 · Gate-count drift guard.** Subject: consumed as F-008. Route: TOOLING. Confidence: PROPOSAL.

**H-03 · Census-to-plan feed.** Subject: consumed as F-010. Route: TOOLING + PROCESS. Confidence: HIGH CONFIDENCE.

**H-04 · Content-validator extension per new catalog kind.** Subject: for each genuinely new catalog kind authored under this factory, the loader + integrity rules + utilization path are mandatory plan steps (v1.0 Part 12.1); no validator may be authored for a kind that already has one. Evidence: `CatalogIntegrityValidator` canon. Route: extension of existing validator. Confidence: CANON process.

**H-05 · Deterministic seed tooling surface.** Subject: a documented seed-forge utility listing sanctioned `StableHash` sub-stream names so planners stop inventing stream names ad hoc. Evidence: sub-stream pattern canon. Route: TOOLING + docs. Confidence: PROPOSAL — verify whether a registry of stream names already exists before creating anything.

**H-06 · Save inspector for development.** Subject: a developer-facing save-envelope inspector (checksum, codec versions, section sizes) if none exists; verify `docs/debug/` first. Evidence: checksummed envelopes canon; debug docs directory exists (DR-02 shows `docs/debug/`). Route: TOOLING, existing-implementation-first. Confidence: PROPOSAL pending existing-tool search.

**H-07 · Docs index drift gate extension.** Subject: extend the index drift gate to cover the root coordination files once F-007 classifies them. Evidence: docs index drift gate canon; DR-01 verified need. Route: TOOLING. Confidence: PROPOSAL, F-007-dependent.

## 3.4 Lane I — Documentation seeds (I-01 … I-06)

**I-01 · Authority-map gap registry.** Subject: enumerate `docs/` domains (DR-02 listing) whose directory exists but whose authority map does not, and fill them in priority order (domains that gained systems in Waves 8–12 first). Evidence: DR-02 listing verified. Route: DOCS-ONLY. Confidence: HIGH CONFIDENCE.

**I-02 · Bible-to-registry reconciliation.** Subject: a reconciliation pass between this document's Drift Register and `docs/ASHFALL_IMPLEMENTED_CANON_REGISTRY.md`, correcting whichever side is stale. Evidence: both exist; DR-04 proves drift occurs. Route: DOCS-ONLY. Confidence: HIGH CONFIDENCE.

**I-03 · Save-schema rules consolidation.** Subject: a single save-authoring rules doc for planners summarizing codec, migration, checksum, and window discipline from the scattered canon sources (v1.0 Parts 5.3, 12.3). Route: DOCS-ONLY, describing reality only. Confidence: HIGH CONFIDENCE.

**I-04 · L10N wave continuation.** Subject: continue the L10N wave roadmap respecting the string freeze — re-check freeze state first (signatures resolve over time, DR-06). Evidence: `L10N_CONTRACT.md`, `L10N_WAVE2_ROADMAP.md` verified live. Route: process + content. Confidence: HIGH CONFIDENCE, freeze-gated.

**I-05 · Agent-rules de-duplication note.** Subject: document the relationship between the per-tool rulebooks (DR-09) and `AGENTS.md` — which is generated, which is hand-maintained, and which tools read which. Evidence: files verified live. Route: DOCS-ONLY. Confidence: VERIFIED need.

**I-06 · Factory session playbook.** Subject: a one-page operator's guide for running the Factory Protocol (premise sweep, seed consumption, volume append), aimed at the multi-agent workflow so any tool can run a factory session. Route: DOCS-ONLY. Confidence: HIGH CONFIDENCE.

## 3.5 Lane J — Onboarding seeds (J-01 … J-05)

**J-01 · Onboarding flow for Wave 8–12 systems.** Subject: onboarding coverage for systems that landed without onboarding entries; each rides the existing onboarding seams. Evidence: `Onboarding/` Core and onboarding selftest canon. Route: DATA-ONLY + host wiring. Confidence: HIGH CONFIDENCE that the class exists; enumerate in session.

**J-02 · Difficulty-preset onboarding text.** Subject: preset descriptions that state observable consequences, not adjectives (post-W1). Evidence: W1 ACTIVE. Route: DATA-ONLY, sequence-gated. Confidence: PROPOSAL.

**J-03 · Manual playthrough checklist per wave.** Subject: a manual checklist for each factory wave, following the existing manual-playtest pattern (Holdfast manual, expedition playtest report — both verified live). Route: DOCS-ONLY. Confidence: CANON process.

**J-04 · First-hour information audit.** Subject: audit that the first hour surfaces every loop step (observe/interpret/prioritize/commit/pay/receive/adapt) at least once. Evidence: loop is CANON (v1.0 Part 1.3). Route: playtest + report. Confidence: PROPOSAL.

**J-05 · Daily-briefing new-content rows.** Subject: consumed by E-01; the onboarding half ensures new rows are announced rather than silently appearing. Route: HOST-WIRING. Confidence: HIGH CONFIDENCE.

## 3.6 Subsystem deep maps (D-01 through D-17; maps, not verdicts)

Each map lists the cluster's canon owners, live catalogs, host sessions, and current factory openings. These are planning instruments: a session picks a cluster, reads its map, and consumes its seeds. All catalog and system names below are carried from the verified v1.0 inventory and the live 2026-09-24 listings; per-field internals remain session-verify territory.

**DM-1 — Shelter operations (C1).** Owners: shelter rooms/identities/machines, thermal, schedules, social events, decor, fire, noise, airlock security, decon, atmosphere, sanitation (power-fed). Live catalogs: `shelter_rooms`, `shelter_room_identities`, `shelter_machine_identities`, `shelter_schedules`, `shelter_social_events`, `shelter_audio_cues`, `shelter_insulation_catalog`, `shelter_shielding`, `sanitation_facilities`, plus the sealed grid catalog. Hosts: ShelterAssignment, ShelterAtmosphere, ShelterDecor, ShelterFire, ShelterSchedule, ShelterThermal, Sanitation, AirlockSecurity, Decontamination, Ventilation. Openings: A-01, A-02, B-01, B-02, D-06, F-04. Notable constraint: room effects route through `IsRoomPowered`; shelter state persists through the holdfast/shelter save family.

**DM-2 — Medical pipeline (C2).** Owners: disease, pathogens, dose ledger, ARS, surgery, autopsy, pharma lab, diagnostics, therapies, dependency, crises. Live catalogs: `disease_catalog`, `pathogens`, `dose_items/locations/quests/registers`, `autopsy_procedures`, `surgical_procedures`, `pharma_recipes`, `microfluidic_diagnostic_catalog`, `medical_texts`, `psychological_therapies`, `chemical_dependency_items`. Hosts: MedicalWard, DoseLedger, PsychologyArc, MentalHealthCrisis. Docs: `MEDICAL_PIPELINE_JOURNEY.md`, `MEDICAL_DOSE_TREATMENT_MATRIX.md`, `MEDICAL_30_DAY_CAPACITY_REPORT.md` (all verified live). Openings: A-03, A-04, A-05, B-03, B-04, B-25, C-14 support, G-03.

**DM-3 — Water, food, agriculture (C3).** Owners: water treatment, condensers, deep wells, brine, nutrition, kitchen, preservation, grain, greenhouse, aquaponics, aeroponics, apiculture, cryo cultivars. Live catalogs: `water_treatment` family via systems, `fog_harvesting_catalog`, `deep_well` systems, `brine` systems, `nutrition_profiles`, `food_preservation`, `grain_processing`, `greenhouse_items`, `hydroponic_crops`, `aquaponics_system_catalog`, `aeroponics_nutrient_catalog`, `cryo_cultivars`, `crop_strains`, `dive_sites`. Hosts: Greenhouse, GrainProcessing, KitchenNutrition, FoodPreservation, DeepWell, Sanitation, DeepCoast. Openings: A-06, A-07, A-08, B-05, C-12, F-012 (dive/hydroponic audit consumed as F-012 above).

**DM-4 — Power and industry (C4).** Owners: power grid, SOFC, solar, kinetic, geothermal, foundry, CVD diamond, EB/PVD, optics, powder metallurgy, pyrolysis, Fischer-Tropsch, chlor-alkali, acids, fermentation, ethanol, air separation, metrology, extrusion. Live catalogs: `power_grid`, `power_subgrid_nodes`, `sofc_power_catalog`, `solar_concentrator_catalog`, `kinetic_flywheel_catalog`, `geothermal_strata_catalog`, `geothermal_drilling_depths`, `cupola_foundry_catalog`, `cvd_diamond_catalog`, `ebpvd_coating_catalog`, `precision_optics_catalog`, `precision_broaching_catalog`, `powder_metallurgy_catalog`, `plastic_pyrolysis_catalog`, `fischer_tropsch_catalog`, `chlor_alkali_synthesis_catalog`, `mineral_acid_synthesis_catalog`, `bio_fermentation_catalog`, `cellulosic_ethanol_catalog`, `cryogenic_air_separation`, `low_background_lead_catalog`, `metrology_standards_catalog`, `hydraulic_extrusion_catalog`. Hosts: SofcPower, SolarConcentrator, SilentFoundry, CvdDiamond, EbPvdCoating, PrecisionOptics, CryogenicAirSeparation, ChlorAlkali, BioFermentation, PlasticPyrolysis, HydraulicExtrusion, LowBackgroundMetrology, GeothermalAquifer. Openings: A-09, A-10, A-11, B-06, C-01, C-02. Constraint: XP W1 owns difficulty scalars for this cluster post-seal.

**DM-5 — Expeditions and travel (C5).** Owners: expedition system, vehicles, dispatch preflight, scavenging tables, waystations, caravans, travel encounters, micro-locations, anomalous encounters. Live catalogs: `expeditions`, `vehicles`, `vehicle_modifications`, `vehicle_armor_grades`, `scavenging_tables`, `waystations`, `caravans`, `merchant_caravans`, `caravan_trade_routes`, `travel_encounters`, `micro_locations`, `anomalous_expedition_encounters`. Hosts: Expedition, ExpeditionVehicle, TravelingCaravan, Waystation, RescueDispatchPreflight. Docs: `EXPEDITION_30_DAY_PLAYTEST_REPORT.md`, `EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md` (verified live). Openings: A-12, A-13, A-14, B-07, B-08, C-03, C-04, E-07.

**DM-6 — Map and geography (C6).** Owners: wasteland map system/loader, damaged zones, fog, route gates, survey instruments. Live catalogs: `wasteland_map_v1`, `damaged_map_zones`, `weather_route_gates`, `gpr_exploration_catalog`, `insar_geodesy_catalog`, `geodetic_survey_catalog`, `seismic_fault_catalog`, `piezometer_network_catalog`. Hosts: GeodeticSurvey, InSarMapping, Cartography selftest family. Openings: A-15, B-09 (GATE). Constraint: the orphan gate `AllMapNodes_ExistInLocationsCatalog` governs all map authoring.

**DM-7 — Factions and war (C7).** Owners: stance engine, doctrines, war system/chain runner, tributes, treaties, embargoes, espionage, psyops, counter-intelligence, musters, labor camps, bounty board. Live catalogs: `factions`, `faction_lore`, `faction_territory`, `faction_intelligence`, branch catalogs (independent/military/rebel), faction war family (communiques/dialogue/events/journal/radio/location_overrides), `warlord_doctrines`, `muster_*` family (five), `labor_camps`, `bounty_board`, `regional_treaties`, `trade_embargoes`, `foundry_accords`, `holdfast_factions`, `crossing_factions`. Hosts: Espionage, PsyOps, CounterIntelligence, Muster, FactionBranch, RegionalTreaty. Openings: A-16, A-17, A-18, B-10 (GATE), B-11 (GATE), C-05, C-06, G-05, plus the F-004 muster campaign. Constraint: all standing effects through `FactionStanceEngine`.

**DM-8 — Radio and information (C8).** Owners: radio system, stations, programs, intercepts, distress signals (sealed runtime), rumors, sound ranging, direction finding, NVIS, heliograph. Live catalogs: `radio`, `radio_stations`, `radio_programs`, `radio_intercepts`, `radio_distress_signals` (+ expansion), `comms_targets`, `sound_ranging_catalog`, `direction_finding_catalog`, `nvis_communications_catalog`, `heliograph`. Hosts: Radio, RadioProgramProduction, SoundRanging, Heliograph. Sealed: distress content (`CF-P1-DISTRESS-CONTENT-SEAL`); availability consumer retired. Openings: A-19, A-20, B-12, B-13, B-25 (coordinated), E-04, F-05, G-06. Constraint: genuine-never-hostile invariant; no new signal scenarios without signature.

**DM-9 — Survivors and interiority (C9).** Owners: needs, health, skills, traits, mental arcs, trauma, therapies, guilt, crises, morale contagion, relations, caregiving, dependency, companion animals, beliefs, spiritual rituals, memorial rites, final wishes, belongings, memory decay, phantom memory, lineage, cohorts, apprenticeships. Live catalogs: `survivors`, `skills`, `development_traits`, `mental_arcs`, `psychological_trauma`, `psychological_therapies`, `guilt_sources`, `confession_secrets`, `belief_movements`, `spiritual_rituals`, `memorial_rites`, `final_wishes`, `companion_animals`, `phantom_heirlooms`, `phantom_triggers`, `starting_survivors`, `starting_survivor_cohorts`, `expansion_survivor_fields`. Hosts: Survivors, SurvivorRelations, PsychologyArc, MentalHealthCrisis, Caregiving, Spiritual, PhantomMemory. Openings: A-21, A-22, A-23, B-04, B-14, B-15, D-02. Known caution: `ClaimPersonalBelonging` no-caller finding (unverified at runtime — re-verify before extending).

**DM-10 — Quests and moral choice (C10).** Owners: questline master, dynamic questlines, personal quests, NPC arcs, moral-choice chains/flags/gossip/quests (five split catalogs live), branching faction quests, bureaucratic morality, massive expansion corpus, repeatable quests, templates, domain questlines (dose, year-of-ash, holdfast, crossing, thirdonary, verdict, expansion). Live catalogs: `questline_master`, `dynamic_questlines`, `personal_quests`, `npc_arcs`, `quests_npc_arcs`, `moral_choice_chains/flags/gossip/quests/quests_branching/quests_distress/quests_expansion`, `quests_faction_branching`, `quests_bureaucratic_morality`, `quests_massive_expansion_200`, `quests_moral_branching_expansion`, `repeatable_quests`, `quest_templates`. Hosts: NarrativeQuestline, PersonalQuest, MoralChoice, DynamicQuestline, ExpansionQuest, NpcArc. Openings: A-24, A-25, B-16, B-17, D-07, G-01, plus the F-001 flagship.

**DM-11 — Economy (C11).** Owners: market, price factors, shocks, baselines, regional prices, hardcore tuning, rumor bands, black market, caravans, debt ledger, foundry economy, bounty board, trade screens. Live catalogs: `commodity_baselines`, `regional_prices`, `hardcore_economy_tuning`, `economy_goods`, `black_market_inventory`, `ledger_debt_templates`, `trade_screen_scenarios`, `trade_tell_lines`, `trade_specialties`, `trade_texts`, `bounty_board`. Hosts: Economy, BlackMarket, TravelingCaravan, SilentFoundry. Docs: `ECONOMY_FAIRNESS_AUDIT.md`, `ECONOMY_PRICE_FACTOR_MATRIX.md` (verified live). Sealed: merchant restock priority (DEC-05). Openings: A-26, B-18, C-07, C-08, C-13, E-08, G-02. GATE: black-market funds legs.

**DM-12 — Weather and Year of Ash (C12).** Owners: weather system, seasons, effects, gates, hardening, storm windows, Year-of-Ash family (events/items/locations/questlines/quests/radio/survivors/storm windows). Live catalogs: all of the above verified live. Hosts: WeatherHardening, YearOfAsh widgets, WeatherStationSystem. Openings: A-27, B-03, B-19, C-09, E-09, F-02, G-07, plus the F-002 campaign. Constraint: tick window 180–360 canon.

**DM-13 — Endgame and epilogue (C13).** Owners: Reckoning, verdict ending evaluator, epilogue matrix runtime, epilogue chronicle, standing records, census, muster epilogues, holdfast endings. Live catalogs: `endings`, `campaign_epilogues`, `epilogue_chronicle`, `verdict_data/items/locations/npcs/questlines/radio`, `standing_record_factions/layouts/memory/quests`, `muster_epilogues`. Hosts: Endgame, Verdict, StandingRecord. Openings: A-28, B-20, D-03, E-10, F-03, G-04, plus the F-005 campaign. Constraint: main ending cannot be invalidated by optional content.

**DM-14 — Ecology and wildlife (C14).** Owners: migration, trapping, ecosystem, seasonal calendar, bestiary, underground flora, infestations, contagion, pathogens, crop genomes. Live catalogs: `wildlife_ecosystem`, `wildlife_trapping_catalog`, `wasteland_wildlife_bestiary`, `underground_flora`, `ecological_infestations`, `contagion_events`, `pathogens`, `crop_strains`, `mutations`. Hosts: WildlifeEcosystem, WildlifeTrapping. Openings: A-29, B-21, C-10, plus the F-002 blight arc. Constraint: zoonosis bridge and campfire sanitization are the owned seams.

**DM-15 — Defense and security (C15).** Owners: perimeter defenses, defense grid, sky defense ordnance and armor, chemical defense, orbital harrow telemetry, interlocks, EMP effects. Live catalogs: `perimeter_defenses`, `defenses`, `sky_defense_ordnance`, `sky_layer_armor_catalog`, `chemical_weapons`, `orbital_harrow_events`, `railway_interlock_catalog`. Hosts: DefenseGrid, SkyDefense, ChemWarfareDefense, OrbitalHarrowTelemetrySystem. Openings: A-30, B-22. Constraint: sky-armor-to-weather bridge already partially built; verify before extending.

**DM-16 — Progression and meta (C16).** Owners: skills, research, collectibles, trophies, achievements, difficulty presets, XP wave, codex, field guide, L10N, mods, settings, input, cohort tuning, apprenticeship, library study. Live catalogs: `skills`, `research_knowledge`, `collectibles`, `trophies`, `difficulty_presets`, `cohort_tuning`, `apprenticeship_catalog`, `library_manuals`, `cultural_archive_tomes`, `codex_entries`, `field_guide`. Hosts: Codex, Research, Collectibles, Difficulty, Apprenticeship, LibraryStudy, Mods, Onboarding, StartingLevel. Openings: B-06, B-23, C-11, D-08, E-03, G-08, J-02. Constraint: XP W1 owns difficulty authority while ACTIVE.

**DM-17 — Host surface and UI (C17).** Owners: the panel families (v1.0 Part 5.7), shell components, focus navigator, snapshots, a11y, briefings. Design pinned by `DESIGN.md`; a11y by `ACCESSIBILITY.md`; input by the 22-action map. Openings: E-01 through E-10, B-24, F-01. Constraint: zero gameplay authority in panels; every panel exposes existing commands and truthful state.


---

# VOLUME 4 — PROSE SPECIFICATION LIBRARY, PART 1 (Factory batch 2026-09-24-E)

Worked field fills for the highest-traffic genres, drafted to the v1.0 Part 9 contracts and the v1.0 Part 15 example standard. Each entry is a model output: a planner extends the genre by instantiating these contracts, never by free-prose imitation. Every example is original to this document; entity references are to verified catalog families. Banned patterns from v1.0 Part 8.3 apply to all of them: no ancient-evil imagery, no prophecy framing, no modern idiom, no named emotion, no real-world references, no exposition dumps.

## 4.1 Manifest (institutional register)

```text
prose_field: manifest
purpose: diegetic inventory/shipping record
trigger: cargo, expedition loadout, caravan arrival
length: 4-10 line items + header
must_include: quantities, condition notes, receiving marks
must_not_include: narrative commentary
model:
  MANIFEST 44-C — VERITY MOTEL RUN
  Received of: shelter stores, per requisition 12
  Blankets, wool, 6 — two with seam failure, noted
  Iodine tablets, tin, 2 — seals intact
  Diesel, jerrycans, 3 — one weeping at the cap, decanted en route
  Signed: FENNE, convoy lead. Countersigned unread, as is customary.
```

Validation note: quantities must agree with the transactional reality the owning system records; a manifest that contradicts the inventory ledger is a continuity defect, not flavor.

## 4.2 Assay report (technical register)

```text
prose_field: assay_log
purpose: process-quality record in the low-tech chemistry voice
trigger: industrial batch completion or inspection
length: 60-120 words
must_include: one tolerance, one observed value, one disposition
must_not_include: emotional language, deferred explanations
model:
  Batch 31, rope, three-strand. Break load held to within a
  hand-width of the table value through the fifth wetting; the sixth
  showed slip at the splice, not the lay. Disposition: service for
  hoisting only, marked at both ends. The ledger takes the number;
  the splice keeps its own counsel.
```

## 4.3 Court verdict (bureaucratic register)

```text
prose_field: verdict_text
purpose: institutional judgment record
trigger: verdict questline resolution
length: 80-160 words
must_include: the charge, the finding, the penalty or release, the authority mark
must_not_include: moralizing, interior monologue
model:
  In the matter of the missing scales, the standing record finds the
  weight short and the explanation shorter. The accused is bound over
  to stores duty under watch for sixty days, after which the record
  closes if no further shortage is entered. The register notes, without
  comment, that the scales were returned the same evening they were
  asked for.
```

## 4.4 Radio transcript (signal register)

```text
prose_field: radio_transcript
purpose: broadcast texture with station identity
trigger: radio play
length: 60-150 words
must_include: station identity, signal framing, one concrete local fact
must_not_include: modern idiom (outside sanctioned satire), exposition
model:
  [SIGNAL FAIR, 19:40] You are hearing the register, and the register
  is short tonight, so here is what was counted: four degrees of frost
  on the east road, a caravan burning bad fuel at the crossing, and
  one frequency that still answers. Keep your filters dry. We will
  read the names again at the top of the hour.
```

## 4.5 Epitaph (memorial register)

```text
prose_field: epitaph
purpose: life compressed to record
trigger: burial/memorial rite
length: 6-20 words
must_include: one lived fact, one institutional mark
must_not_include: sentimentality
model: "She kept the second pump alive past its counting. Log 88, mark 2."
```

## 4.6 Intake interview (people-and-psyche register)

```text
prose_field: intake_interview
purpose: arrival record in question-and-answer form
trigger: new survivor arrival
length: 80-160 words
must_include: one verifiable claim, one unverifiable claim left unverified
must_not_include: the interviewer's conclusions
model:
  Asked how she crossed the canal, she says: before the freeze. Asked
  who traveled with her, she gives one name and declines the second
  question. Hands: rope wear, no frostbite loss. Belongings: a tin,
  contents not offered, not asked for yet. Entered as day-labor
  pending; the register takes what it can verify.
```

## 4.7 Field report (operations register)

```text
prose_field: field_report
purpose: expedition/patrol after-action record
trigger: expedition return
length: 80-150 words
must_include: route legs, one hazard observed, one resource consumed
must_not_include: heroics, foreshadowing
model:
  Out four days, back three. The rail cut is passable to the winch
  mount; beyond it the ballast is ice over void, and we did not argue
  with it. Fuel spent: one jerrycan and most of a second. One filter
  cracked at the membrane, logged. Saw no one; saw where someone had
  been, within the week, cooking small. We did not follow it. The
  pan tells the rest.
```

## 4.8 Load-shed notice (deadpan institutional register)

```text
prose_field: schedule_notice
purpose: hardship announcement in institutional voice
trigger: power shortfall event
length: 30-70 words
must_include: the affected window, the receiving register, one procedural instruction
must_not_include: apology beyond the procedural
model:
  Per Schedule 001, rev. 6: corridor lighting will be dark from the
  nineteenth hour until muster. Hot water follows the corridor. Entries
  of complaint may be made in the register and will be read in spring,
  in the order received.
```

## 4.9 Genre usage rules for factory sessions

1. Never invent a parallel genre; the established list (v1.0 Part 8.4) is closed for new kinds — new needs extend an existing genre or propose a genuinely new field through the Part 9 contract process with a consuming loader.
2. Two catalogs describing the same craft or institution must differ in voice or evidence, or be merged (v1.0 corpus-matching rule).
3. Every authored batch names which existing `narrative/*.json` catalogs it extends and which it could duplicate.
4. Quantities in diegetic documents must reconcile with the owning system's records; prose is never a second source of truth.
5. Voice signatures are per-entity (v1.0 Part 12.2 character records); institutional documents speak in register, not personality.

---

# GROWTH LEDGER

- 2026-09-24 — Base document v2.0 (drift register, factory protocol, matrices, backlog, templates, growth protocol) — ~32,000 — cumulative ~32,000
- 2026-09-24 — Volume 1: twelve complete subject plans (F-001 … F-012) — ~36,000 — cumulative ~68,000
- 2026-09-24 — Volume 2: subject seed catalog, Lanes A–E (A-01…A-30, B-01…B-25, C-01…C-14, D-01…D-08, E-01…E-10) — ~29,000 — cumulative ~97,000
- 2026-09-24 — Volume 3: subject seed catalog, Lanes F–J (F-01…F-06, G-01…G-08, H-01…H-07, I-01…I-06, J-01…J-05) plus seventeen subsystem deep maps (DM-1…DM-17) — ~23,000 — cumulative ~120,000
- 2026-09-24 — Volume 4: prose specification library part 1 (eight worked genre contracts plus usage rules) — ~5,500 — cumulative ~125,500

Current seed inventory: 12 full subject plans + 111 compressed seeds + 17 subsystem maps + 8 prose contracts. Factory consumption ratio: 0 seeds consumed into implementation (correct — no implementation was requested or authorized in this session).

Next volumes, in recommended order: Volume 5 (prose specification library part 2: remaining genres with worked models); Volume 6 (worked expansion of the twenty highest-confidence A-lane seeds into full Template S plans); Volume 7 (per-cluster catalog field inventories, built from in-session catalog reads); Volume 8 (balance harness specification library, one harness spec per Lane C seed); Volume 9 (re-audit volume refreshing the drift register); then wave-structured batches of full subject plans at five per wave, per the lane rotation discipline.