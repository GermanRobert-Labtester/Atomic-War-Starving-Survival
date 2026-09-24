# MASTER COMPILATION NOTE

This document is the complete compiled edition of the ASHFALL Master Expansion Authority v2.0. It combines, in order: (1) the uploaded source document (Volumes 1–24 and its Parts 0 through V, reproduced verbatim), and (2) the Plan Factory's expansion volumes 25 through 57 (the factory batches of 2026-09-24, reproduced verbatim). No content has been altered, merged, or summarized; the two bodies are concatenated at their natural boundary. The source document's own authority order stands: live repository source first; then AGENTS.md; then this document. The factory's constitution (evidence labels, honest bounds, anti-padding) governs Volumes 25 onward.

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

- 2026-09-25 — Volume 5: prose specification library part 2 (twenty-one additional worked genre contracts: journal, unsent letter, communiqué, directive, liturgy, cipher, dispatch/debrief, graffiti, folklore, almanac, gazetteer, treaty, permit, glitch report, provenance, eulogy, rumor, environmental clue, item inspection, relationship reaction, quest/outcome texts, codex, map texts, world-state notification; genre list now fully covered) — ~17,700 — cumulative ~143,000
- 2026-09-25 — Volume 6: twenty full subject plans expanded from Lane A seeds (FP-A01…FP-A27 selection, with wave sequencing F6-1 through F6-4) — ~16,600 — cumulative ~160,000
- 2026-09-25 — Volume 7: catalog authoring contract library (field-inventory protocol, six CANON record contracts, item authoring rules, integrity checklist, schema-sheet registry with priority inventory order) — ~8,700 — cumulative ~169,000
- 2026-09-25 — Volume 8: balance harness specification library (twelve harness specs H-C1…H-C12 plus publication and consumption rules) — ~10,300 — cumulative ~179,000

- 2026-09-26 — Volume 9: re-audit volume (live re-read of `INTEGRATION_PLANS.md` and `KNOWN_DEBT.md`; five new drift-register entries DR-11 through DR-15, including three verified debt seals with wiring facts, five premise corrections to the factory's own seeds, and the KNOWN_DEBT promotion-condition premise rule) — ~7,100 — cumulative ~186,000
- 2026-09-26 — Volume 10: eighteen full subject plans from Lane B–E seeds (FP-B01…FP-B25 selection with DR-11/DR-12 premise corrections incorporated, plus six-wave sequencing structure F10-1 through F10-6) — ~27,700 — cumulative ~214,000

- 2026-09-27 — Volume 11: twenty-two full subject plans from Lane D–J seeds (FP-D01…FP-D08, FP-E01/E02/E03/E05/E06/E07/E09/E10, FP-F01/F02/F04/F06, FP-G01/G02/G03/G04/G05/G08, FP-H01/H05/H06, FP-I01/I03/I06, FP-J01/J04, plus wave sequencing F11-1 through F11-9) — ~36,400 — cumulative ~250,000
- 2026-09-27 — Volume 12: four decision packets prepared for signature (DP-01 flooded-route topology tags, DP-02 FactionWar per-strike emitters, DP-03 black-market funds legs, DP-04 semantic-kind regrouping) plus the F-011 C2 gap package scoping — ~12,000 — cumulative ~262,000
- 2026-09-27 — Volume 13: ten per-lane expansion playbooks (Lane A through J) with worked seed-to-verified-tranche examples and cross-lane sequencing rules — ~11,500 — cumulative ~274,000
- 2026-09-27 — Volume 14: two worked wave charters (ASH-EXP-1 narrative wave, ASH-EXP-2 economy wave), the wave execution runbook, the re-audit cadence contract, and the anti-scope-creep review checklist — ~7,900 — cumulative ~282,000
- 2026-09-27 — Volume 15: nine remaining Lane A seed expansions into full plans (FP-A09, FP-A14, FP-A15, FP-A20, FP-A23, FP-A24, FP-A25, FP-A29, FP-A30), closing Lane A's compressed-seed backlog, including the FP-A25 200-record quest prose audit tranche program (Tranche 0 census plus eight authoring tranches plus completion regression) — ~19,000 — cumulative ~301,000
- 2026-09-27 — Volume 16: worked content tranche library — twelve PROPOSAL-model JSON tranche examples per established genre (glitch, load-shed, assay, interlock report, marginalia, rundown, intake, quest prose, sighting log, ordnance manifest, almanac), each with validation notes and the three mandatory focused tests — ~12,400 — cumulative ~313,000
- 2026-09-27 — Volume 17: cluster-by-cluster expansion roadmaps C1–C17, each anchored to its deep map with Phase I–IV structure and five cross-cluster sequencing rules — ~16,900 — cumulative ~330,000
- 2026-09-27 — Volume 18: re-audit against the repository's public surface — four drift-register candidates (DR-16 communiqué board and tick-gate, DR-17 map-atlas repair, DR-18 hardening/quarantine cleanup, DR-19 post-v1.0 subsystem families absent from the deep maps), five premise corrections, five evidence-gated seed replenishments (A-31, A-32, B-26, G-09, E-11), and the factory self-audit — ~9,900 — cumulative ~342,000
- 2026-09-27 — Volume 19: public-surface confirmation pass — DR-16, DR-17, DR-18 upgraded to MERGED-AS-LISTED with merge dates; four new entries (DR-20 wave-5/6 subsystem inventory, DR-21 shelter atmosphere/noise panels with follow-up repair, DR-22 commitment-event semantic parity incident corroborating DP-04, DR-23 port-contract seam ratchet cleanup); six premise corrections — ~9,700 — cumulative ~352,000
- 2026-09-27 — Volume 20: authoring session runbook library — eight runbooks (RB-CENSUS, RB-COLLISION, RB-PAIR, RB-SEALGUARD, RB-KNOWLEDGE, RB-QUANT, RB-WAVE, RB-DRCONFIRM) turning every open-premise class into an executable session procedure — ~7,700 — cumulative ~359,000
- 2026-09-27 — Volume 21: remaining Lane B and all Lane C seed expansions — FP-B04, FP-B06, FP-B14, FP-B15 plus the full economy set FP-C01 through FP-C14 with the lane rule that no Lane C plan changes a number in its first tranche; Lane B and Lane C compressed backlogs now zero (GATE items excepted, correctly) — ~23,100 — cumulative ~382,000
- 2026-09-27 — Volume 22: prose and document contract supplement, merged-surface edition — atmosphere-reading and noise-complaint genre contracts (DR-21), release-craft documentation contract (DR-16), agent-skills index contract, commitment-event authoring rule (DR-22/DR-23); DR-19 domains deliberately left uncontracted pending confirmation — ~6,800 — cumulative ~389,000
- 2026-09-27 — Volume 23: worked wave charter bank — six charters (ASH-EXP-3 census wave, ASH-EXP-4 first tranches, ASH-EXP-5 standing-authority bridges, ASH-EXP-6 measure-everything harness wave, ASH-EXP-7 register-the-present documentation wave, ASH-EXP-8 conditional signature wave) with sequencing rules — ~9,900 — cumulative ~399,000
- 2026-09-27 — Volume 24: replenishment seed catalog from the DR-20 subsystem inventory — eight Lane A seeds (A-33 through A-40), six Lane B seeds (B-27 through B-32), three Lane E/G/J seeds (E-12, G-10, J-06), all doubly gated on working-tree confirmation, with the four replenishment rules — ~7,600 — cumulative ~407,000

Current inventory: 12 flagship subject plans (F-001…F-012) + 29 Lane A full subject plans + 22 Lane B full subject plans + 14 Lane C full subject plans + 18 Lane D–E full subject plans + 22 Lane F–J full subject plans + 4 signature-ready decision packets + 10 per-lane playbooks + 8 worked wave charters + 12 worked tranche models + 8 session runbooks + 17 cluster roadmaps + 111 compressed seeds + 17 DR-20 replenishment candidates (5 from Volume 18, 17 from Volume 24 — overlap consolidated) + 17 subsystem maps + 31 worked prose contracts + 6 catalog record contracts + 12 harness specifications + a live drift register (DR-01 through DR-15 confirmed; DR-16 through DR-18 and DR-21 through DR-23 merged-as-listed; DR-19, DR-20 pending working-tree confirmation). Factory consumption ratio: 0 seeds consumed into implementation (correct — no implementation was requested or authorized in these sessions).

Next volumes, in recommended order: Volume 25 (working-tree RB-DRCONFIRM execution: the confirmation table for DR-16 through DR-23, deep-map corrections, and retirement of any unconfirmed candidate — the gate for the entire DR-20 pipeline); Volume 26 (first live-census instantiation: FP-A25 Tranche 0 and FP-A24 census executed against the working tree, if repository access is granted); Volume 27 (harness specification supplement for the six ASH-EXP-6 Lane C harnesses — input fixtures, report schemas, and hand-reconciliation procedures); Volume 28 (per-cluster verification command cookbook mapping every plan to its exact gate and selftest commands); then sustained re-audit and replenishment waves per the Part VI protocol.
---

# VOLUME 5 — PROSE SPECIFICATION LIBRARY, PART 2 (Factory batch 2026-09-25-A)

The remaining established document genres (v1.0 Part 8.4), each with a worked contract and model. Same standing rules as Volume 4: originals only, banned patterns excluded, quantities reconcile with owning systems, and the genre list is closed — new needs extend an existing genre or go through the Part 9 field-contract process with a consuming loader.

## 5.1 Journal entry (player-voice register)

```text
prose_field: journal_entry
purpose: the player's own record of an event or quest beat
trigger: event/quest beat
length: 60-120 words
viewpoint: first person, house restraint
must_include: one action taken, one observable consequence or open question
must_not_include: omniscient narration, named emotion
model:
  Traded two tins and a favor for the filters. The favor is the part
  I will regret; favors keep no ledger anyone can read. Walked the
  corridor twice after and the pump room still smells of hot iron,
  which the maintenance log says it should not. Noted it in the margin
  here because the log is someone's job and margins are mine.
```

## 5.2 Unsent letter (personal register)

```text
prose_field: unsent_letter
purpose: private voice against institutional time
trigger: memorial, guilt source, relationship threshold
length: 80-160 words
must_include: one concrete shared memory, one unfinished practical matter
must_not_include: sentimentality, apology speeches
model:
  The kettle you left is the one still in use; the handle you mended
  outlasted the rest of it. I keep meaning to tell your sister about
  the ledger entry you never closed — the two blankets against the
  spring draw, which I have been carrying quietly. If she comes the
  long way round I will show her the page. If she does not, the page
  holds, and that is the arrangement now.
```

## 5.3 Communiqué (faction public register)

```text
prose_field: communique
purpose: faction-to-faction or faction-to-public formal statement
trigger: faction war chain, treaty step, embargo notice
length: 60-120 words
must_include: the issuing authority, one actionable demand or declaration, one procedural channel
must_not_include: threats outside the faction's doctrine methods, private language
model:
  By order of the crossing commission, the toll at the cut is revised
  from the spring rate to the winter rate, effective at next muster.
  Convoys under foundry accord seal are exempt as before. Disputes
  may be entered with the post registrar, who will hold them for the
  thaw session. This notice is read on all four frequencies.
```

## 5.4 Directive (faction private register)

```text
prose_field: directive
purpose: internal operational instruction
trigger: doctrine shift, espionage mission, psyops campaign
length: 40-90 words
must_include: one objective, one constraint, one channel discipline rule
must_not_include: public language; nothing the recipient would already know
model:
  The convoy times are the target, not the cargo. Two listeners only;
  the third frequency stays dark this week so the dark itself is the
  signal. If asked at the crossing, we came for salt. Do not come for
  salt.
```

## 5.5 Liturgy / hymnal (belief register)

```text
prose_field: liturgy
purpose: belief-movement ritual text
trigger: belief movement ceremonies, spiritual rituals
length: 60-140 words
must_include: one material symbol (ash, iron, water, ledger), one repetition, one communal action
must_not_include: prophecy framing, real-world religious echo, ancient-evil imagery
model:
  The ash is the year. Pass it hand to hand, and let each keep what
  stays in the palm. What is written is read; what is read is owed;
  what is owed is carried until the spring court says otherwise. So
  the shelter held, so the shelter holds. The verses are counted, not
  sung, until the meter is paid.
```

## 5.6 Cipher / numbers-station sequence (signal-cryptic register)

```text
prose_field: cipher_sequence
purpose: intercept puzzle texture with a solvable kernel
trigger: numbers-station intercept, radio mysteries
length: 3-8 lines
must_include: group structure consistent with the station's format, one recoverable kernel tied to existing intercept content
must_not_include: unsolvable gibberish, fourth-wall hints
model:
  4-19-7. 4-19-7. 19-44-1, 19-44-1. 8-8-8.
  [Repeats at interval. The 8-8-8 closes every session this week,
  where last week closed on 7-7-7. The register moves; the moved
  number is the message.]
```

## 5.7 Dispatch / debrief pair (courier register)

```text
prose_field: courier_dispatch
purpose: sent instruction and returned account
trigger: courier mission leg
length: 40-80 words each
must_include (dispatch): destination, deadline, carry mark
must_include (debrief): arrival state, one deviation, one cost
model (dispatch): "West post, before the freeze hardens. Carry the
  tally, not the goods; the goods are already spoken for. Mark is
  the blue twine, three knots."
model (debrief): "Arrived on the third day with the tally intact and
  the twine one knot short. The knot was traded at the waystation,
  deliberately, for stove time. West post holds. The road does not."
```

## 5.8 Graffiti / wall carving (anonymous register)

```text
prose_field: graffiti
purpose: unofficial public voice, compressed and worn
trigger: shelter graffiti postings, wall carving templates
length: 4-20 words
must_include: one concrete claim or instruction, wear or placement cue
must_not_include: author identity, modern slang
model: "THE METER ON LEVEL 2 LIES — CHECK AGAINST YOUR OWN. (chalk, half-washed)"
```

## 5.9 Children's folklore (folk register)

```text
prose_field: childrens_folklore
purpose: transmission rhyme or rule-lore among the young
trigger: shelter folklore, cohort onboarding texture
length: 20-60 words
must_include: one practical survival rule carried as play, one invented figure
must_not_include: real-world reference, terror imagery
model:
  Count the doors before you count the dark, and if the seventh door
  is warm, knock twice and wait for the count. The Meter Man never
  skips a number, so never skip a door, or he starts counting you.
```

## 5.10 Almanac entry (weather register)

```text
prose_field: almanac_entry
purpose: seasonal prediction record in the almanac voice
trigger: weather seasons, storm windows
length: 40-90 words
must_include: one measurable prediction, one historical comparator, one practical instruction
must_not_include: certainty beyond the record
model:
  Ninth window, wind from the ash side. Old entries give the ninth as
  a short one — three days, filters doubled, travel deferred. The
  last two ninth-windows ran long. Instruction unchanged from the old
  books: check the dial at the fourth hour and believe it over the
  sky.
```

## 5.11 Gazetteer entry (cartographic register)

```text
prose_field: gazetteer_entry
purpose: settlement/place reference entry
trigger: wasteland settlement gazetteer
length: 60-120 words
must_include: one economic fact, one access rule, one hazard note
must_not_include: quest spoilers, undiscovered information
model:
  HARMONY FLATS — crossroads settlement, salt and second-hand tools.
  Access by the west approach only since the footbridge went; the
  guards count heads in and out and keep the tally for the district
  record. Water is trucked in on even days. Hazards: the approach
  floods without warning in the fourth and eleventh windows, and the
  flats themselves drain slowly. Traders take goods at gate rates;
  disputes go to the registry bench, Tuesdays.
```

## 5.12 Treaty protocol (diplomatic register)

```text
prose_field: treaty_protocol
purpose: treaty clause text
trigger: regional treaty, foundry accord, aquifer concession
length: 60-140 words
must_include: parties, one enumerated obligation per party, term or review condition, breach channel
must_not_include: aspirational language
model:
  Per this accord, the signatories hold the eastern draw in common
  from thaw to first frost. The foundry draws water against a counted
  toll; the crossing holds passage open for foundry convoys at the
  spring rate. Breaches are entered with the district registrar within
  seven days of the breach or are waived. Review at each season court,
  or sooner if the draw runs below the marker stone.
```

## 5.13 Permit (administrative register)

```text
prose_field: permit
purpose: authorization document
trigger: vouch access, excavation, hunting/trapping rights, labor assignment
length: 30-70 words
must_include: holder, scope, term, revocation condition
must_not_include: courtesy language
model:
  EXCAVATION PERMIT 12-C: bearer may open the north spoil line to the
  second marker, tool-weight class three, through the end of the
  month. Revoked automatically on any unlogged find. Registrar's mark
  and the holder's own; both or neither.
```

## 5.14 Maintenance glitch report (shelter-operations register)

```text
prose_field: glitch_report
purpose: machine misbehavior log entry
trigger: shelter maintenance glitch events
length: 40-90 words
must_include: symptom, workaround, disposition or escalation
must_not_include: anthropomorphism beyond sheltered idiom
model:
  Compressor 2 cycles on at the 3-minute mark regardless of setpoint;
  cycles off correctly. Workaround: manual watch at shift change. Not
  urgent enough to strip the panel for; urgent enough that the watch
  is written into the roster now, not argued about later.
```

## 5.15 Provenance dossier (memory register)

```text
prose_field: provenance_dossier
purpose: ownership history of an object or heirloom
trigger: relic/collectible discovery, phantom memory
length: 60-120 words
must_include: at least three custody points, one unresolved gap, one physical verification mark
must_not_include: sentimental reconstruction beyond record
model:
  Item: field watch, runner-broken, second hand missing. Custody:
  quartermaster stores to the second watch-keeper (mark in the old
  ledger, page and line recorded); watch-keeper to the schoolroom as
  a teaching piece; schoolroom to this shelf, date unknown — the gap
  is the damage. Verification: the case-back carries the schoolroom's
  inventory punch, two dots, which the schoolroom stopped using before
  the second winter.
```

## 5.16 Eulogy (memorial spoken register)

```text
prose_field: eulogy
purpose: spoken remembrance at a rite
trigger: memorial rite, funeral
length: 80-160 words
must_include: one work history fact, one habit observed by others, one closing institutional formula
must_not_include: grief narration, afterlife claims beyond the setting's belief movements
model:
  He ran the third shift for six years and never once signed the
  roster late, which the roster itself will confirm. He kept a cup
  above the pump panel and answered questions from the younger hands
  without making them small for asking. What he started, the roster
  keeps; what he owed, the ledger holds; what he carried, the shelter
  carries now. Entered, counted, and read. That is the whole of the
  record and it is enough.
```

## 5.17 Rumor (distorted-information register)

```text
prose_field: rumor
purpose: gossip/radio distortion with a kernel of truth
trigger: gossip propagation, market rumor bands
length: 30-60 words
must_include: one true kernel, at least one distortion, one attribution weakness
must_not_include: verified facts stated as verified
model:
  They say the flats settlement turned away a whole caravan at
  gunpoint over a salt price. The registrar's bench says the
  caravan never had salt to begin with — but someone at the crossing
  swears they saw the crates, and the crates are the part people
  repeat.
```

## 5.18 Environmental clue (embedded-history register)

```text
prose_field: environmental_clue
purpose: wear and residue as history
trigger: location inspection
length: 25-60 words
must_include: one wear mark, one tool/residue trace, one inference left to the player
must_not_include: explicit exposition
model:
  The doorframe is worn in two streaks at shoulder height — rope
  carry, repeated, by someone right-handed who passed this way often
  and recently. The floor below is swept; the rest of the room is not.
```

## 5.19 Item inspection (object-interiority register)

```text
prose_field: item_inspection_text
purpose: examine-an-object interiority
trigger: item examine
length: 30-70 words
must_include: material, one provenance hint, one use-truth
must_not_include: lore dumps, stats
model:
  A tin of iodine tablets, seal intact, label from a pharmacy that
  no longer answers to its name. The seal matters more than the label;
  the label is a place, the seal is a promise someone kept under
  pressure.
```

## 5.20 Relationship reaction (trust-stage register)

```text
prose_field: relationship_reaction
purpose: NPC response at a trust-stage change
trigger: trust threshold crossing
length: 30-70 words
must_include: stage-appropriate behavior, one withheld element
must_not_include: out-of-stage intimacy, motivation statement
model (early stage): "You count fast. I'll grant that. Keep counting
  and we will see whether the numbers and the sacks agree — no
  offense meant, and none taken if you check mine too."
model (established stage): "Take the good scale, not the marked one.
  I am not explaining why; you have been here long enough to know
  which is which without me saying it."
```

## 5.21 Quest hook and outcome texts (play register)

```text
prose_field: quest_hook
purpose: why the player cares, tied to a person
trigger: quest offer
length: 40-80 words
must_include: stakes for a named or specific person, one deadline or scarcity pressure
must_not_include: mechanics language, concept-level stakes
model:
  The post registrar's sister went out with the tally run and the
  tally came back without her. The registrar will not leave the bench
  and cannot ask officially — an official ask goes in the record, and
  the record is read by people who collect on it. She is asking you
  off the books, before the week closes and the trail is just weather.
```

```text
prose_field: success_text / failure_text / abandonment_text
purpose: outcome framing with next-pressure hint
trigger: resolution
length: 40-90 words
model (success): "The sister came in ahead of the storm, minus the
  pack and some of the feeling in two fingers. The registrar entered
  nothing in the record except the register of returns, which is
  its own kind of thanks, and the bench now knows your face as one
  that brings people back. That is a currency here, and it spends."
model (failure): "The trail closed with the weather and the search
  did not survive it. The registrar kept the bench open one extra
  hour anyway, then closed it, then entered the tally — complete,
  which was the cruelest accurate thing the record could say."
model (abandonment): "No closure was filed; the week simply moved
  on and took the road with it. The bench still holds the request
  unentered. Requests without entries do not expire; they wait for
  someone to ask again."
```

## 5.22 Codex entry (world-knowledge register)

```text
prose_field: codex_entry
purpose: public-account world knowledge on discovery
trigger: codex discovery
length: 80-150 words
must_include: public account framing; hidden account only after evidence
must_not_include: hidden reveals before their evidence is found
model:
  LOAD SHED SCHEDULES — The district schedules survive as the clearest
  voice of the old authority: precise, unsentimental, and addressed
  to a public that no longer assembles to receive it. The public
  account holds that the schedules were rationing; the annotations
  in later revisions suggest something closer to triage by lamplight.
  Which account is true is not settled by the documents alone; the
  documents settle only that someone kept issuing them, to the end.
```

## 5.23 Map label and descriptions (cartographic play register)

```text
prose_field: map_label
purpose: cartographic identity
trigger: map render
length: 2-4 words
must_include: distinctive proper noun; no sentences
model: "Denial Cut" / "Harmony Flats" / "The Weighbridge"

prose_field: map_description
purpose: hover/legend fact
trigger: map focus
length: 15-30 words
must_include: one concrete fact; no quest spoilers
model: "Toll crossing on the ravine lip. Chain maintained; toll collected at the winter rate."
```

## 5.24 World-state notification (system register)

```text
prose_field: world_state_notification
purpose: plain system feedback on a state change
trigger: state change
length: 1-2 lines
must_include: what changed, plainly
must_not_include: alarm spam, flavor padding
model: "Storm window 9 open. Travel deferred by directive; filters doubled at the gate."
```

## 5.25 Genre coverage completion note

With Volumes 4 and 5, all genres in the v1.0 Part 8.4 list now carry worked contracts: manifest, audit/assay, titration record, log, journal, diary, letter (sent/unsent), intake interview, therapy note, casebook, court verdict, wiretap transcript, communiqué, directive, liturgy, hymnal (liturgy family), canon (religious), epitaph, eulogy, burial record, provenance dossier, rundown, scriptbook, cipher, dispatch, debrief, field report, waypoint note, planning brief, schedule notice, graffiti, carving, folklore (children's and adult), song (folklore family), almanac entry, gazetteer entry, bestiary entry (natural-history family, A-29 model), genealogy (lineage registers), treaty protocol, permit, load-shed schedule, maintenance glitch report, risk-of-failure wishlist (planning-brief family). Sessions extend these; they do not invent parallels.


---

# VOLUME 6 — TWENTY FULL SUBJECT PLANS FROM LANE A SEEDS (Factory batch 2026-09-25-B)

Each seed below is expanded to full Template S depth. Standing premises (not repeated in full each time): the Factory Protocol premise sweep ran 2026-09-24/25 against the live listing and ledger; DR-06 seals and gates apply; all routes are DATA-ONLY unless a host-wiring step is named; verification always ends with the data-integrity selftest at 0 findings plus the content-utilization selftest proving consumption. Where a plan's premise needs a deeper in-session read, the Open premises field says so.

## FP-A01 — Bunker Maintenance Glitch Batch for Wave 8–12 Rooms

Lane A · C1 · Status PROPOSAL.
Subject: a fourth glitch corpus batch covering shelter rooms that gained systems in Waves 8–12 (EMP-conditioned power, medical power feed, grid catalog rooms).
Premise evidence: VERIFIED glitch batches 2 and 3 exist in the narrative corpus; VERIFIED `shelter_rooms.json`, `shelter_machine_identities.json` live; VERIFIED the EMP/medical/grid implementation logs exist in `docs/plans/` (DR-08 listing).
Why this first: glitch prose is the cheapest per-character continuity yield and directly textures the newest mechanics.
Must not change: machine identity canon; any glitch entry implying a new mechanic must correspond to an implemented behavior.
Route: DATA-ONLY into the existing glitch catalog family; loader family unchanged; utilization via the existing corpus consumption path (verify the consumer — Open premise 1).
Continuity: glitch symptoms must be physically plausible for the named machine; no real-world brands.
Verification: integrity selftest; utilization selftest; a focused loader test if the batch adds any new field (it should not).
Open premises: 1. Confirm the glitch corpus's consuming surface (which system reads glitch entries into play). 2. Enumerate the exact room set touched by Waves 8–12 to bound the batch.

## FP-A02 — Load-Shed Amendments for the Sanitation Power Feed

Lane A · C1 · Status PROPOSAL.
Subject: amendment notices (Schedule 001 revisions) reflecting the sanitation room power feed: corridors tied to powered sanitation, water windows following room state.
Premise evidence: VERIFIED `load_shed_schedule_001` in the corpus; VERIFIED the sanitation `RoomPowerProvider` seam landed per the followups package (DR-06 ledger).
Why this: pairs the newest power reality with the corpus's most iconic document; pure data.
Must not change: actual power routing — prose reflects, never drives, the grid (Invariant 5).
Route: DATA-ONLY; amendments keyed on room power state via `state_variants`-style selection if the corpus loader supports conditioning (Open premise).
Continuity: amendment text must not contradict the power system's observable behavior.
Verification: integrity; utilization; one manual cross-check per amendment against the power system's states.
Open premises: confirm the corpus loader supports state-conditioned selection of entries; if not, this stays a static-batch plan and conditioning moves to a Lane B seed.

## FP-A03 — ARS Casebook Batch

Lane A · C2 · Status PROPOSAL.
Subject: medical casebook entries mirroring the ARS latent-to-manifest phase structure with dosimetric detail in the casebook voice.
Premise evidence: VERIFIED `dweller_medical_casebook` exists; ARS phase structure is CANON (multi-day latent-to-manifest).
Why this: the medical pipeline is fully documented (three live docs, DR-03) and its prose layer is the recognized thin side.
Must not change: phase timing, dose thresholds — the casebook dramatizes the implemented pathology, nothing more.
Route: DATA-ONLY.
Continuity: doses and days stated in entries must fall inside the implemented phase windows; no treatment outcome the medical system cannot produce.
Verification: integrity; utilization; a focused pairing test against the ARS phase constants (G-03 pattern).
Open premises: confirm the casebook catalog's entry schema in session.

## FP-A04 — Dose-Treatment Therapy-Note Twins

Lane A · C2 · Status PROPOSAL.
Subject: therapy notes paired to `MEDICAL_DOSE_TREATMENT_MATRIX.md` rows — one document per treatment class, in the therapist-notes voice.
Premise evidence: VERIFIED matrix document live (DR-03); VERIFIED therapist batches 1–3 exist.
Must not change: matrix values; the notes observe, they do not tune.
Route: DATA-ONLY; the pairing is documented in the plan and pinned by G-03's tests.
Continuity: patient references must reuse existing dweller/survivor ids or be anonymized records (never invent survivor ids).
Verification: integrity; utilization; G-03 pairing suite green.
Open premises: none beyond session schema reads.

## FP-A05 — Therapist Session Notes Batch 4

Lane A · C2/C9 · Status PROPOSAL.
Subject: fourth batch keyed to guilt sources, insomnia states, and confession secrets added since batch 3.
Premise evidence: VERIFIED batches 1–3 live; VERIFIED `guilt_sources.json`, `confession_secrets.json` live.
Must not change: psychological systems' state mechanics; notes are texture anchored to implemented states.
Route: DATA-ONLY.
Continuity: a note may only reference a guilt source that exists in the catalog; confidentiality framing is diegetic, not meta.
Verification: integrity; utilization; corpus duplication check against batches 1–3 (each new note must cite a source batch 3 lacked).
Open premises: enumerate which guilt/confession content postdates batch 3 in session.

## FP-A06 — Food Preservation Assay Corpus Twins

Lane A · C3 · Status PROPOSAL.
Subject: assay/log corpus entries for every `food_preservation.json` and `grain_processing.json` process lacking a narrative twin.
Premise evidence: VERIFIED both catalogs live; VERIFIED the assay genre family in the corpus (Part 5.6); the preservation authority map governs content rights (FOOD_PRESERVATION_AUTHORITY_MAP, live).
Must not change: preservation process parameters; the food-preservation authority map's ownership rules.
Route: DATA-ONLY.
Continuity: temperatures, times, yields stated in prose must match catalog parameters exactly (prose is never a second source of truth).
Verification: integrity; utilization; per-entry parameter cross-check.
Open premises: per-process twin-absence audit must run first in session.

## FP-A07 — Cellar and Silo Seasonal Field Logs

Lane A · C3 · Status PROPOSAL.
Subject: continuation batches of humidity-rot and weevil-audit logs conditioned on seasonal windows.
Premise evidence: VERIFIED both corpus families live; VERIFIED seasonal calendar system canon.
Must not change: rot and infestation mechanics (owned by the greenhouse/food systems).
Route: DATA-ONLY.
Continuity: seasonal references must match the seasonal calendar's windows.
Verification: integrity; utilization.
Open premises: confirm whether the existing corpus families are static or already state-conditioned.

## FP-A08 — Apiculture Foundation-Log Continuation

Lane A · C3 · Status PROPOSAL.
Subject: hive foundation-log continuation tied to seasonal yield.
Premise evidence: VERIFIED `langstroth_hive_foundation_logs` in the corpus; apiculture canon (Part 16.3).
Must not change: hive yield mechanics.
Route: DATA-ONLY.
Continuity: log entries reference wax and frame states the system models.
Verification: integrity; utilization.
Open premises: verify apiculture's current consumable surface (if the hive system is corpus-only, utilization must route through the corpus consumer, and the plan must name it).

## FP-A10 — Metrology Calibration Corpus

Lane A · C4 · Status PROPOSAL.
Subject: calibration certificates and gauge-discrepancy reports for `metrology_standards_catalog.json`, in the low-background metrology voice.
Premise evidence: VERIFIED catalog live (DR-04); VERIFIED `LowBackgroundMetrology` host session (v1.0 host inventory).
Must not change: standards values; a discrepancy report describes drift, never redefines the standard.
Route: DATA-ONLY; utilization via the metrology session's document surface if one exists (Open premise).
Continuity: units and tolerances must match the catalog's parameter style.
Verification: integrity; utilization.
Open premises: confirm what consumes metrology documents at runtime.

## FP-A11 — Foundry Pour-Window Logs

Lane A · C4 · Status PROPOSAL.
Subject: cupola pour records conditioned on accord state and treaty consequences.
Premise evidence: VERIFIED `foundry_accords.json`, `foundry_treaty_consequences.json`, `foundry_production.json` live; VERIFIED foundry assay corpus families exist.
Must not change: production math; accords semantics.
Route: DATA-ONLY.
Continuity: a pour log may only reference accord states the system can produce; records reconcile with production outputs (log discipline).
Verification: integrity; utilization.
Open premises: schema read for the pour-log entry shape.

## FP-A12 — Destination Arrival/Revisit Prose Completion

Lane A · C5 · Status PROPOSAL (multi-tranche).
Subject: completion of `arrival_description` and `revisit_description` for the 53-destination surface where thin, per Part 9 contracts.
Premise evidence: VERIFIED destination surface canon; arrival/revisit contracts CANON (Part 9).
Must not change: no location facts invented in prose that the locations catalog does not authorize; no spoilers.
Route: DATA-ONLY, tranches of approximately ten destinations per wave.
Continuity: the 15.2 worked example demonstrates the standard; every batch runs the imagery-overlap check against neighbors.
Verification: integrity; utilization; word-count contract checks if the loader enforces them (Open premise).
Open premises: per-destination prose-depth measurement in session; confirm whether a prose-length gate exists in the loader.

## FP-A13 — Waystation Register Prose

Lane A · C5 · Status PROPOSAL.
Subject: guest-register entries and notice sheets for the waystation family.
Premise evidence: VERIFIED `waystations.json` live; register/manifest genres established.
Must not change: waystation service semantics.
Route: DATA-ONLY.
Continuity: register names must be existing or clearly anonymized; entries reference routes and weather the systems model.
Verification: integrity; utilization.
Open premises: waystation document consumer check.

## FP-A16 — Standing-Record Testimony Depth

Lane A · C7 · Status PROPOSAL.
Subject: witness-statement and registry-annotation prose expanding `standing_record_memory.json` coverage.
Premise evidence: VERIFIED standing-record family live; VERIFIED standing records feed the Reckoning (Part 16.6).
Must not change: standing-record data semantics; testimony obeys information-flow legality (a witness testifies only to what they experienced).
Route: DATA-ONLY.
Continuity: every testimony entry names its witness and channel; epilogue relevance declared per entry where it enrolls evidence.
Verification: integrity; utilization; epilogue-reachability spot tests for new evidence-bearing entries.
Open premises: standing-record loader schema read.

## FP-A17 — Verdict Radio Continuation

Lane A · C7 · Status PROPOSAL.
Subject: verdict-station rundown batches conditioned on verdict questline states.
Premise evidence: VERIFIED `verdict_radio.json`, `verdict_questlines.json` live; verdict endgame canon.
Must not change: questline resolution logic; Crossing ending prose pins.
Route: DATA-ONLY with state-conditioned selection if the loader supports it.
Continuity: rundowns never announce outcomes ahead of the questline state that authorizes them.
Verification: integrity; utilization; one state-conditioning test if conditioning is used.
Open premises: confirm verdict radio's conditioning surface.

## FP-A18 — Warlord Doctrine Communiqués

Lane A · C7 · Status PROPOSAL.
Subject: doctrine-conditioned communiqué and tribute-demand prose for warlords with thin public/private separation.
Premise evidence: VERIFIED `warlord_doctrines.json`, `faction_war_communiques.json` live; doctrine record fields CANON (public/operational/hidden goals, methods willing/unwilling).
Must not change: doctrine behavior; all standing effects remain with `FactionStanceEngine`.
Route: DATA-ONLY.
Continuity: public language and private language must match the doctrine record's dialogue rules; forbidden claims respected verbatim.
Verification: integrity; utilization; a per-warlord voice-signature consistency check.
Open premises: enumerate warlords with thin separation in session.

## FP-A19 — Numbers-Station Cipher Continuation

Lane A · C8 · Status PROPOSAL.
Subject: additional cipher sequences with solvable kernels tied to existing intercept content.
Premise evidence: VERIFIED `numbers_station_ciphers` in the corpus; VERIFIED `radio_intercepts.json` live.
Must not change: intercept resolution mechanics; the cipher is texture whose kernel resolves to facts the intercept system already models.
Route: DATA-ONLY.
Continuity: every cipher kernel must be recoverable from existing intercept content — no cipher hides new canon.
Verification: integrity; utilization; a solvability review per sequence (documented in the plan, not automated).
Open premises: existing cipher format read.

## FP-A21 — Phantom-Memory Triggers for Surviving Cohorts

Lane A · C9 · Status PROPOSAL.
Subject: heirloom-trigger entries conditioned on cohort survival state, deepening the generational line.
Premise evidence: VERIFIED `phantom_heirlooms.json`, `phantom_triggers.json` live; cohort/lineage canon; 19B closed cohort links.
Must not change: trigger resolution mechanics; dead survivors appear only in memory contexts (hard world rule).
Route: DATA-ONLY with cohort-state conditioning through the existing trigger selector.
Continuity: a trigger may fire only on a survivor who plausibly encountered the heirloom's provenance (provenance dossiers are the anchor, 5.15 contract).
Verification: integrity; utilization; a conditioning test per trigger cohort state.
Open premises: phantom trigger selector schema read.

## FP-A22 — Final-Wishes Document Corpus

Lane A · C9 · Status PROPOSAL.
Subject: unsent-letter and testament prose for `final_wishes.json` entries lacking document twins.
Premise evidence: VERIFIED catalog live; unsent-letter genre established (5.2 contract).
Must not change: wish-granting mechanics.
Route: DATA-ONLY.
Continuity: a wish's document must not request outcomes the wish system cannot produce.
Verification: integrity; utilization.
Open premises: final-wish consumer and schema read.

## FP-A26 — Ledger-Debt Statement Prose

Lane A · C11 · Status PROPOSAL.
Subject: debtor statements and collection notices for `ledger_debt_templates.json` rows.
Premise evidence: VERIFIED catalog live; `LedgerDebtSystem` with consequence dispatchers canon; debt seals in handoff records.
Must not change: debt math, interest cadence, consequence vocabulary.
Route: DATA-ONLY.
Continuity: statements must reconcile with the ledger's own arithmetic; consequence threats only from the closed vocabulary.
Verification: integrity; utilization; G-02 dispatcher suite unaffected.
Open premises: template row enumeration in session.

## FP-A27 — Storm-Window Almanac Entries

Lane A · C12 · Status PROPOSAL.
Subject: almanac prose conditioned on `year_of_ash_storm_windows.json` entries, within the Year-of-Ash window.
Premise evidence: VERIFIED catalog live; `weather_almanac_expansion` corpus exists; window canon 180–360.
Must not change: window mechanics; the almanac predicts in-world, the system decides.
Route: DATA-ONLY.
Continuity: predictions may be wrong in-world (almanacs can be wrong) but window references must match the catalog.
Verification: integrity; utilization.
Open premises: almanac corpus schema read.

## Volume 6 sequencing note

Twenty seeds expanded (A-01 through A-27 selection; the remaining Lane A seeds — A-09, A-14, A-15, A-20, A-23, A-24, A-25, A-28, A-29, A-30 — await either session premise reads or consumption by flagship plans F-003, F-005, A-25's audit, and corpus sweeps). Recommended wave structure for implementation sessions: Wave F6-1 (FP-A01, FP-A02, FP-A06, FP-A27 — shelter/food/weather documents); Wave F6-2 (FP-A03, FP-A04, FP-A05 — medical documents); Wave F6-3 (FP-A16, FP-A17, FP-A18, FP-A19 — faction/radio documents); Wave F6-4 (FP-A12, FP-A13, FP-A21, FP-A22, FP-A26 — travel/people/economy documents); FP-A08, FP-A10, FP-A11 slot into whichever wave their premise reads land in. One lane per wave holds: all are Lane A, so waves remain prose-only and data-first, per the rotation discipline.


---

# VOLUME 7 — CATALOG AUTHORING CONTRACT LIBRARY (Factory batch 2026-09-25-C)

Honesty clause first: this volume does not fabricate field lists for catalogs whose live JSON this session could not open. Instead it publishes the authoring contracts the repository's own documentation establishes (v1.0 Parts 12.2, 6.1, 3.6 — all CANON), plus a field-inventory protocol that an owning session executes against each catalog's live JSON to produce a verified per-catalog schema sheet. The combination is what a planner needs to author safely without ever inventing a schema.

## 7.1 The field-inventory protocol (run per catalog before authoring)

1. Open the catalog's live JSON; record root `schema_version` and top-level shape.
2. Record the field set of one representative entry; mark which fields are required by the loader (read the loader, not just the JSON — JSON shape is evidence, loader behavior is proof).
3. Record the catalog's id prefix convention and enumerate existing ids (collision sweep input).
4. Record cross-reference targets (which other catalogs' ids appear in its fields) — these become integrity-rule inputs.
5. Record the state-conditioning surface: does the loader select entries by campaign state, or is the catalog static canon? This determines whether prose must carry `state_variants` blocks.
6. Record the consumer: which system, session, or corpus path reads it (the utilization answer). Presence in JSON is not reachability.
7. Emit the schema sheet into the plan's Premise evidence section, labeled VERIFIED with the date.

A schema sheet produced by this protocol is valid until the catalog's `schema_version` changes; sessions re-check the version, not the whole sheet, when the factory reuses it.

## 7.2 Quest record contract (CANON, from the quest architecture documentation)

Fields: `id`, `type`, `status`, `title`, `priority`, `connections` (characters/locations/factions/items/arcs — by ID only, never re-biography), `purpose` (narrative/gameplay/thematic), `availability` (requires/excludes/discovery_methods), `stages` (id, title, objectives, location_requirements), `branches` (choice, effects on trust/state), `outcomes` (success/failure/abandonment), `prose_fields` (Part 9 contracts).
Authoring rules: connections by ID only; every availability condition references an implemented discovery mechanism; outcomes follow the reward/failure/recovery grammar (v1.0 Part 6.7); each quest names its owning questline family.

## 7.3 Character/survivor record contract (CANON, Part 12.2)

Fields: `id` (snake_case), `type`, `status` (CANON/DRAFT/PROPOSAL), `name`, `aliases`, `role`, `summary`, `identity` (age, origin, occupation, public_reputation, private_reality), `psychology` (core_desire, immediate_goal, fear, wound, contradiction, moral_boundary, false_belief, true_need), `voice` (sentence_length, vocabulary, humor, avoids, repeated_patterns), `knowledge` (knows / suspects / does_not_know / cannot_know_yet), `relationships` (target, type, initial_trust), `arcs` (start_state, pressure, possible_resolution), `gameplay` (quest_roles, location_roles, expedition_relevance), `prose_requirements` (must_show / must_avoid).
Authoring rules: the `knowledge` block enforces information-flow legality — entries in `cannot_know_yet` are binding; the `voice` block is the per-entity voice signature that all dialogue authoring must honor.

## 7.4 Relationship record contract (CANON, Part 12.2)

Fields: `id`, `source`, `target`, `status`, `dimensions` (trust, respect, fear, dependence, resentment, affection), `stages` (id, range, behavior), `change_events` (event, effects).
Authoring rules: stage ranges must match the owning relation system's thresholds; `behavior` text in stages feeds the `relationship_reaction` prose contract (5.20) and must not promise behaviors outside the stage.

## 7.5 Location record contract (CANON, Part 12.2)

Fields: `id` (loc_*), `name`, `region`, `location_types`, `identity` / `short_description`, `narrative_purpose` / `gameplay_purpose`, `spatial` (neighbors, travel_cost, entry_routes, hazards), `history` (founded_by, former_purpose, historical_events, public_misconception, hidden_truth), `availability` (map_visibility gates, expedition_selection guaranteed_when / optional_when, revisitable), `capabilities` (supports / does_not_support), `quest_connections` (by id), `state_variants` (blocked / accessible / contested / evacuated / collapsed), `prose_fields`.
Authoring rules: map nodes must exist in the locations catalog (orphan gate); `state_variants` selection is the sanctioned way to represent dynamic state without rewriting catalog canon (v1.0 Part 4.6); destroyed locations cannot appear intact (hard world rule).

## 7.6 Faction record contract (CANON, Part 4.2)

Fields: public goal, operational goal, hidden goal, long-term goal; methods (willing/unwilling); resources; weaknesses; members; relationships (value to other factions); controlled and contested locations; quest roles (gives/obstructs); dialogue rules (public language, private language, forbidden claims).
Authoring rules: new faction ids require the full field set and connection to existing conflicts/seams (prohibited-direction rule); behavior projections must respect doctrine methods; reaction timing obeys information-flow legality.

## 7.7 Timeline event record format (CANON, Part 3.6)

Fields: `id` (EVENT_SNAKE_CASE), `date` (fictional calendar position), `status`, `summary`, `public_account`, `hidden_account`, `participants`, `locations`, `consequences` (state changes that persist), `clues` (item/document/delivery ids that reveal it), `player_relevance` (quest unlocks, epilogue flags).
Authoring rules: `hidden_account` content is unreachable until at least one `clues` reference is discovered; consequences must be expressible through owning systems; the faction-war day mapping (authored-day space) governs where historical events sit relative to playable time.

## 7.8 Item authoring rules (CANON, Part 12.1 distilled)

Zero new item ids when existing ids suffice — reuse and tag with `expansion_item_tags.json`. Loot references must point at real item ids (historical defect class: singular/plural drift — guarded tests exist). Every new item requires: catalog entry, integrity compliance, at least one acquisition path (loot, craft, trade, or event reference), and at least one consumption surface (a recipe, need, trade entry, or quest connection). An item with no acquisition path or no use fails the utilization review even if integrity passes.

## 7.9 Catalog-agnostic integrity checklist (every authored tranche)

1. `schema_version` present at root; snake_case ids; no duplicate ids.
2. Every cross-reference resolves (items, locations, factions, survivors, quests, skills).
3. Ranges and enums valid; state values from the allowed set.
4. Loot/dependency references point at real ids.
5. Map nodes exist in the locations catalog.
6. New catalog kinds carry: loader, integrity rules, host session consumption, utilization path — all four or none.
7. Data-integrity selftest exits 0 with 0 findings; content-utilization selftest proves consumption.
8. The tranche names which existing catalogs it extends and which it could duplicate (corpus-matching rule).

## 7.10 Schema-sheet registry (to be filled by owning sessions)

```text
[CATALOG] schema_version: [v] — inventoried [date] — loader: [file] —
consumer: [system/session/corpus path] — conditioning: [static|state-variant|state-selected]
— collision-sweep ids: [count] — notes: [...]
```

Sessions append one line per catalog they inventory. Until a line exists for a catalog, plans touching it carry the inventory step as an open premise. Priority order for first inventory passes: `moral_choice_flags`, `quests_massive_expansion_200`, `locations` (+ expansions), `items`, `epilogue_chronicle`, `muster_witnesses`, `metrology_standards_catalog`, `waystations`, `foundry_production`.


---

# VOLUME 8 — BALANCE HARNESS SPECIFICATION LIBRARY (Factory batch 2026-09-25-D)

One harness specification per Lane C seed. All harnesses inherit the repository's determinism canon: seeded runs, `ISeededRng` sub-streams, two-pass byte-identical proof, results published under `docs/balance/`. The Plan 76.2 seeded 200-run pattern is the template; these specifications parameterize it per question. No harness changes tuning; every harness produces evidence, and only evidence-backed outliers become tuning plans (C-13 discipline).

## 8.1 Harness H-C1 — Industrial chain income-versus-expenditure (serves C-01)

Question: for each industrial process chain, is producing the output cheaper or dearer than buying it at current regional prices, and what is the total resource cost (fuel, feedstock, labor-shifts, filter wear, degradation)?
Parameters: chain list from the live industrial catalogs (DM-4); difficulty preset; regional price band; time horizon (30/90/180 days).
Method: seeded fixed-roster simulation, each chain run in isolation and in the full economy; record input draw, output yield, labor-hour cost, and opportunity cost of the power allocated.
Outputs: per-chain cost table; dominated-process list (buy-cheaper-than-make); anomalous chains where a single input price band flips the verdict.
Acceptance: two-pass byte-identical; the table names each chain's owning catalog so the results stay joinable to live data.

## 8.2 Harness H-C2 — SOFC fuel sustainability (serves C-02)

Question: does real inventory fuel consumption (Plan 122/125 sealed behavior) stay sustainable across difficulty presets, and what campaign-day does an unreplenished reserve hit zero?
Parameters: preset; starting supplies; expected fuel income schedule (expedition, trade, processing); storm-window drawdown spikes.
Method: seeded 180-day simulation with fixed roster and authored expedition cadence; record reserve curve per week.
Outputs: reserve curves per preset; days-to-zero; the week where income first fails to cover draw.
Acceptance: byte-identical replay; curves reconcile with the Plan 122/125 closeout evidence.

## 8.3 Harness H-C3 — Scavenging E[value] (serves C-03, B-08)

Question: what is the expected value of a dispatch per destination, per stance, and per season, and where do outliers persist after current quantity-band trims?
Parameters: 200 seeded runs per destination (the established pattern); stance; weather-gate state; season.
Method: dispatch-only simulation; record loot value at regional prices, filter/fuel/ammo consumption, casualty cost (medical pipeline priced), and net.
Outputs: E[value] ranking per destination; outlier table (the trim candidates); confidence bands.
Acceptance: byte-identical replay; results joinable to the 49-table scavenging surface and the 53-destination catalog.

## 8.4 Harness H-C4 — Vehicle dominance (serves C-04, B-07)

Question: which vehicle/modification/armor combinations dominate the dispatch meta, per route class and season?
Parameters: live vehicle catalogs (`vehicles`, `vehicle_modifications`, `vehicle_armor_grades`); route classes (ice-road, deep-coast, standard); season.
Method: paired seeded runs, identical seed and route, varying vehicle configuration; record arrival rate, breakdown rate, net E[value].
Outputs: dominance ranking against `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md` deltas; dominated configuration list.
Acceptance: byte-identical; delta table published beside the existing dominance doc.

## 8.5 Harness H-C5 — Tribute sustainability (serves C-05)

Question: for each warlord doctrine, is the 7-day tribute demand payable from realistic mid-game income, and at what demand level does a death spiral begin (tribute forcing expedition risk forcing casualty forcing income loss)?
Parameters: doctrine list from `warlord_doctrines.json`; mid-game income model (post-water-and-power stabilization, Day 90+); preset.
Method: seeded 120-day campaign slices per doctrine; tribute paid on schedule where possible; record shortfall outcomes and doctrine reaction cascades.
Outputs: sustainability threshold per doctrine; death-spiral demand level; reaction-cost table.
Acceptance: byte-identical; reactions reconcile with `FactionStanceEngine` behavior evidence.

## 8.6 Harness H-C6 — Embargo price signal (serves C-06)

Question: do `trade_embargoes.json` entries produce legible price signal at affected settlements, and how many in-game days until the signal is observable?
Parameters: embargo list; embargoed goods; settlement price bands.
Method: seeded market simulation with and without each embargo; record price-band movement and rumor-band propagation timing.
Outputs: per-embargo signal strength (band delta) and days-to-observable; noise classification.
Acceptance: byte-identical; rumor propagation respects the deterministic band rules.

## 8.7 Harness H-C7 — Debt runaway (serves C-07)

Question: which debt trajectories are mathematically unrecoverable, and does every unrecoverable state still have a modeled recovery path (per the recovery grammar)?
Parameters: `ledger_debt_templates.json` rows; income models (early/mid/late); interest cadence.
Method: seeded loan-event simulation per template; record balance curves; classify unrecoverable states; cross-check each against consequence dispatcher outputs for recovery affordances.
Outputs: runaway-state list; recovery-path audit table; any state lacking recovery becomes a Lane B finding, not a tuning edit.
Acceptance: byte-identical; classification reconciles with dispatcher evidence.

## 8.8 Harness H-C8 — Winter compression (serves C-09, B-19)

Question: what is the sustainability-day count for calories, fuel, filters, and morale through Days 90–180 and the Year-of-Ash window, per preset, under authored storm windows?
Parameters: storm-window catalog; needs decay constants; ration tiers; power draw.
Method: seeded 270-day simulation (Days 90–360); fixed roster with authored intake; record first-shortfall day per resource per preset.
Outputs: sustainability table; window-vs-pre-window cost ratio; the resource that fails first per preset (the pressure design input for B-19).
Acceptance: byte-identical; shortfalls reconcile with needs system constants.

## 8.9 Harness H-C9 — Trapping yield economics (serves C-10)

Question: does trapping yield positive expected value including equipment degradation and zoonosis risk premium (uncooked consumption)?
Parameters: `wildlife_trapping_catalog.json`; equipment condition constants; pathogen transmission probabilities (from the zoonosis bridge).
Method: seeded 60-day trapline simulation; record yield value, degradation cost, medical cost of zoonosis cases with and without campfire sanitization.
Outputs: net per trapline strategy; the sanitization premium; dominated strategies.
Acceptance: byte-identical; medical costs priced through the medical pipeline's own constants.

## 8.10 Harness H-C10 — Cultivation economics (serves C-12)

Question: across greenhouse, hydroponic, and aeroponic families, which crops return positive value per power/water/nutrient unit, and does any family strictly dominate?
Parameters: the three cultivation catalogs; power and water draw constants; nutrition profile values.
Method: seeded 90-day growth simulation per crop per family; record input cost versus nutrition/economy value.
Outputs: per-crop return table; family dominance check; power-cost sensitivity band.
Acceptance: byte-identical; growth respects crop-strain and cultivar parameters.

## 8.11 Harness H-C11 — Combat cadence (serves C-14)

Question: what is time-to-kill and expected ammunition expenditure per combat class per preset, and does scarcity make any class unwinnable without Retreat as the only option?
Parameters: `combat_catalog.json`; armor grades; ammo scarcity bands; hardcore tuning.
Method: seeded encounter simulation per class; record TTK, ammo spent, casualty probability.
Outputs: TTK table; ammo cost per class; unwinnable-without-retreat classification (which becomes a design question, not a silent tuning change).
Acceptance: byte-identical; results cite the combat catalog rows used.

## 8.12 Harness H-C12 — Difficulty spread (serves C-11, post-W1)

Question: once the W1 difficulty authority seals, do presets produce distinct, legible curves (calories, combat, economy) rather than uniform multipliers?
Parameters: preset scalars from the sealed authority; the H-C8, H-C11, and H-C3 harnesses as probes.
Method: run all three probes under each preset; compare curve shapes, not just endpoints.
Outputs: preset distinctness matrix; any preset whose curve is a scalar multiple of another flagged for design review.
Acceptance: byte-identical; sequence-gated on W1 seal.

## 8.13 Publication and consumption rules

1. Every harness run publishes to `docs/balance/` with: date, seed, preset, catalog versions (by `schema_version`), results, and the two-pass proof hashes.
2. Harness results are evidence, never edits. Outliers route into the Part IV backlog as tuning candidates with the harness table cited.
3. A harness result older than one content wave is stale; Lane C plans re-run the relevant harness rather than citing stale tables.
4. Harness specifications version with the catalogs they parameterize: when a catalog's `schema_version` changes, the harness spec's parameter list is re-checked before the next run.
---

# VOLUME 9 — RE-AUDIT VOLUME: DRIFT REGISTER REFRESH (Factory batch 2026-09-26-A)

Re-audit executed 2026-09-26. Live reads: `INTEGRATION_PLANS.md` (32,793 characters — identical length and content to the 2026-09-24 read, indicating no ledger change between audits) and `KNOWN_DEBT.md` (first sections, newly read this session).

## 9.1 New drift-register entries

**DR-11 — Three further debts sealed 2026-09-18/19 with wiring facts. VERIFIED (from `KNOWN_DEBT.md` live read).**
- `DEBT-PLAN36-PORT-CONTRACT-CLOSURE` — RETIRED/SEALED. The port-contract sweep is closed with 262 policy seams, 180 HOST_REQUIRED, 0 DEFERRED. Specific bindings recorded: `BindCraftResultGate` bound to `ItemCatalog.Contains` from `CraftingHostSession.Create`; `SpiritualMeaningCoordinator.RegisterDeath` fed from `SurvivorFate.OnSurvivorFate`; memorial vigil maps to `ShelterVigilRiteId`; a `spiritual_meaning` save section persists; the Iron Cenotaph consumes arc/rite counts. Promotion condition: do not re-open without a new untracked Core seam or unbound HOST_REQUIRED.
- `DEBT-PLAN34-DIFFICULTY-CHRONICLE-AUTHORITY` — RETIRED/SEALED. Live scalars already consumed; completion-history schema v2 stamps `difficultyPresetId` on new records; v1 records still validate (the field is NonSerialized for SaveChecksum, folded into the v2 hash only). `CampaignCompletionHistoryTests` 11/11.
- `DEBT-PLAN30-RUNTIME-CLOCK` — RETIRED/SEALED. The 300-day offset mapping is confirmed live (`FactionWarChainRunner.ToAuthoredDay` playable 180 → authored 480) so chains and communiqué minDays fire inside the Year-of-Ash 180–360 tick; timeline/thermal/radon still clamp to 360; war-chain stage/chain events project to journal and radio; expedition arrival records war-location visits for `PlayerVisitedTrigger`.

**DR-12 — Premise corrections to this factory's own seeds. VERIFIED.**
- **Seed B-15 (memorial-rite epilogue evidence enrollment) is partially satisfied already.** The spiritual-meaning wiring recorded in DR-11 (`SpiritualMeaningCoordinator.RegisterDeath`, `ShelterVigilRiteId` mapping, persisted `spiritual_meaning` section, Iron Cenotaph arc/rite consumption) means the rite-to-consequence bridge exists. B-15's remaining scope narrows to: whether memorial/spiritual facts enroll as Reckoning evidence (the enrollment half only). Any session consuming B-15 must first read the Iron Cenotaph consumption path and `spiritual_meaning` section to scope what is left.
- **Seed B-16 premise strengthened.** `PlayerVisitedTrigger` (DR-11) demonstrates the trigger-on-recorded-event pattern that quest reopening would follow; the war-chain journal/radio projection also confirms the consequence-routing pattern. B-16 remains PROPOSAL but its implementation precedent is now named.
- **Seed B-14 premise weakened in the right direction.** War-chain events already project to journal and radio (DR-11); any belief-movement bridge must not duplicate the standing-effects routing that already exists through the war-chain path. The `FactionStanceEngine` remains the sole standing authority; the bridge, if built, feeds it rather than paralleling it.
- **Seed D-08 premise partially resolved.** `DEBT-PLAN34` (DR-11) records the completion-history v2 stamp behavior and its checksum treatment exactly; D-08's remaining scope is only the W1-coordinated migration question if W1 adds preset fields — the base stamp behavior is sealed and documented.
- **Seed B-23 premise advanced.** `DEBT-PLAN34` records that live difficulty scalars are already consumed and the stamp is live, which means the XP W1 authority has an existing consumer contract to extend rather than a green field. W1 remains ACTIVE; sessions still sequence-gate on its seal.

**DR-13 — Ledger stability between audits. VERIFIED.**
`INTEGRATION_PLANS.md` is byte-identical between the 2026-09-24 and 2026-09-26 reads (same length, same batch states). Factory implication: no new waves, seals, or ownership changes landed in that window; the Part IV backlog remains valid without re-sweeping the ledger. The next session after any real activity must re-run the sweep, since a two-point sample proves only that window, not stability.

**DR-14 — KNOWN_DEBT is a high-value premise input beyond its ledger role. VERIFIED.**
The debt file's promotion-condition column (for example, "do not re-open without a new untracked Core seam or unbound HOST_REQUIRED"; "do not re-author JSON minDays without a new clock contract") is effectively a list of the exact conditions under which sealed work may reopen. Factory rule added: the premise sweep now reads `KNOWN_DEBT.md` promotion conditions, not just statuses, because a seed whose opening condition matches a recorded promotion condition is legitimately openable, while a seed that merely resembles a retired debt is not.

**DR-15 — DR-10 unverified list, partial resolution.**
Resolved by DR-11: the completion-history v2 stamp (schema pin) is now VERIFIED with checksum behavior; the war-chain offset mapping is now VERIFIED live with its clamp behavior (timeline/thermal/radon clamp at 360). Still UNVERIFIED from DR-10: the 11,697 test total, the D1 seal state, the full 57-gate inventory, the `ClaimPersonalBelonging` no-caller status, and the decision-blocked items beyond those the ledgers record as resolved.

## 9.2 Factory process update

The premise sweep (Part II, Step 1) gains one mandatory input and one rule:
- Input: `KNOWN_DEBT.md` promotion conditions (per DR-14).
- Rule: every seed consumed after this volume must carry a premise-correctness note stating whether DR-11/DR-12 affects it. The affected seeds are annotated in place by this volume; future seeds check at consumption time.


---

# VOLUME 10 — TWENTY FULL SUBJECT PLANS FROM LANE B–E SEEDS (Factory batch 2026-09-26-B)

Standing premises: the Factory Protocol sweep including the DR-14 debt-promotion-condition read applies; DR-11/DR-12 premise corrections are incorporated per plan. GATE and SEALED seeds are excluded here — they are prepared separately as signature-ready drafts (planned Volume 12). Verification always names the integrity and utilization selftests; Core-extensions always name the determinism class and save impact.

## FP-B01 — Shelter-Failure Cascade Exit from Quarantine

Lane B · C1 · Status PROPOSAL.
Subject: complete the shelter-failure effects program by exiting quarantine per its own recorded exit criteria, routing failures through `cascade_rules.json` so that grid, fire, and power failures propagate consequences through owning systems.
Premise evidence: VERIFIED the quarantine wiring logs exist (`SHELTER_FAILURE_EFFECTS_QUARANTINE_WIRING_*` in `docs/plans/`, DR-08 listing); VERIFIED the grid catalog seal logs exist. Open premise: the quarantine plan's exit criteria must be read in session — this plan rides them, it does not invent them.
Why this first: a quarantined subsystem is the repository's own recorded debt; nothing in Lane B outranks closing recorded debt on its recorded terms.
Must not change: failure semantics beyond the quarantine plan's scope; power routing authority (grid owner); determinism (existing seeded event streams).
Route: CORE-EXTENSION per the quarantine plan's own design; cascade propagation through the existing `cascade_rules.json` authoring surface where the plan calls for data; host wiring per its UI steps. Save impact: per the quarantine plan (verify); likely EXISTING-SECTION.
Continuity: failure consequences must reconcile with shelter 30-day maintenance report baselines (live, DR-03) — a cascade that contradicts the maintenance model is a defect.
Verification: the quarantine plan's own acceptance tests; focused cascade tests with per-rule failure output; triad parity gate (Setup/Save/Flush) for any new stateful component; determinism two-pass for any new roll.
Open premises: read the quarantine implementation log's exit criteria and current quarantine state.

## FP-B02 — Shelter Grid Catalog Seal Follow-Through

Lane B · C1 · Status PROPOSAL.
Subject: complete any consumer bindings left open by the shelter grid catalog seal so every sealed catalog row has a consuming system.
Premise evidence: VERIFIED the seal logs exist (`SHELTER_GRID_CATALOG_SEAL_*`); VERIFIED the port-contract sweep closed with 0 DEFERRED (DR-11), which bounds the remaining work to seal-specific bindings, not the general port contract.
Why this: same debt-first principle as FP-B01; the seal program defines its own completeness bar.
Must not change: grid authority; sealed catalog semantics.
Route: HOST-WIRING per the seal plan. Save impact: NONE expected (bindings, not state).
Continuity: bindings must expose truthful state only (Invariant 5).
Verification: seal plan acceptance; integrity + utilization selftests (the utilization selftest is the completeness proof); panel lifecycle gates for any touched panel.
Open premises: read the seal log for open items.

## FP-B03 — Fallout-Window Dose Accrual Deepening

Lane B · C2/C12 · Status PROPOSAL.
Subject: storm-window-conditioned dose accrual: unprotected outdoor work and travel during Year-of-Ash windows accrues dose at window-scaled rates, with dosimeter alarms and briefing rows surfacing the added exposure.
Premise evidence: VERIFIED `DoseLedgerSystem` and dose catalogs live; VERIFIED storm-window catalog live; VERIFIED fallout patterns catalog live; VERIFIED the daily weather check and expedition dispatch are the owning ticks. INFERENCE: current coupling depth between windows and dose accrual is unknown — the premise read must establish it (if deep coupling exists, this seed closes as a no-change area, which is an acceptable outcome).
Why this: radiation is the game's signature pressure and its weather coupling is the canonical scarcity interaction (weather drives radiation, v1.0 Part 4.5).
Must not change: dose ledger arithmetic authority; ARS phase constants; genuine-never-hostile and all sealed surfaces untouched.
Route: CORE-EXTENSION in the dose owner; window conditioning data through existing catalogs (fallout patterns already exist — reuse, do not author a new window-dose catalog unless the premise read proves none fits). Determinism: existing seeded streams; no new RNG. Save impact: EXISTING-SECTION (dose ledger v2 family).
Continuity: accrual rates must reconcile with dose-register records; briefing rows follow the E-01 surface rules.
Verification: focused dose tests with window/non-window paired cases; two-pass determinism proof; H-C8-style paired simulation slice (window vs non-window exposure totals); integrity + utilization.
Open premises: read the current window→dose coupling in the dose system and expedition exposure path.

## FP-B05 — Preservation-Contamination Bridge

Lane B · C3 · Status PROPOSAL.
Subject: failed or rushed preservation runs producing contamination exposure through the existing pathogen/disease seams (the zoonosis bridge is the structural model: catalog condition → exposure event → medical pipeline).
Premise evidence: VERIFIED food preservation systems and authority map live; VERIFIED `pathogens.json` and `contagion_events.json` live; VERIFIED the zoonosis bridge with campfire sanitization is the owned precedent. Open premise: whether preservation failure states are modeled at all — if failure is not a state, this plan first adds the failure state (small Core extension) and only then the bridge.
Why this: it converts an authored process (preservation) into systemic risk, which is the house design grammar (choices charge costs).
Must not change: preservation process parameters; disease progression constants.
Route: CORE-EXTENSION through the preservation owner (failure state, if absent) + the disease owner (exposure); data via existing catalogs. Determinism: existing seeded streams. Save impact: EXISTING-SECTION (preservation/disease families).
Continuity: contamination must respect the food-preservation authority map's ownership; exposure events use the closed pathogen vocabulary.
Verification: focused preservation + disease tests (paired clean/contaminated runs); determinism proof; integrity + utilization; H-C9-style cost slice optional.
Open premises: read the preservation system's failure-state surface and the pathogen exposure vocabulary.

## FP-B07 — Vehicle-Breakdown Medical and Dose Consequences

Lane B · C5 · Status PROPOSAL.
Subject: expedition vehicle breakdowns producing injury events routed into the medical pipeline and exposure/immersion doses routed into the dose ledger, so a breakdown is a consequence, not a delay.
Premise evidence: VERIFIED `ExpeditionVehicleSystem` and vehicle catalogs live; VERIFIED dominance table live (implying breakdown modeling exists); VERIFIED the medical and dose pipelines are the standing consequence sinks. INFERENCE: current breakdown consequence routing depth is the open premise.
Why this: the 30-day playtest report and dominance table both treat breakdowns as a first-class expedition pressure; consequence routing is the difference between a delay and a survival event.
Must not change: vehicle durability math; rescue-dispatch preflight semantics (sealed surface respected — additive only).
Route: CORE-EXTENSION in the expedition vehicle owner's consequence path; events routed through the existing event bus to medical and dose owners. Determinism: existing seeded streams. Save impact: EXISTING-SECTION (expedition family).
Continuity: injury classes must come from the medical system's vocabulary; exposure must respect waterborne/immersion rules where applicable.
Verification: focused expedition tests (breakdown → consequence assertions); determinism proof; integrity + utilization; H-C4 harness re-run to confirm dominance deltas are recorded, not silent.
Open premises: read the breakdown event surface and its current consequence routing.

## FP-B08 — Scavenging-Table Parity Completion

Lane B · C5 · Status PROPOSAL (data-first).
Subject: complete per-destination scavenging table coverage where the 49-table surface underserves the 53-destination catalog, with renewable tables for living settlements and one-time caches for supply caches (the established split).
Premise evidence: VERIFIED the 49/53 gap is canon (v1.0 Part 6.2); VERIFIED `scavenging_tables.json` live; VERIFIED loot-reference guard tests exist (historical defect class).
Why this: it is the rare data-first Lane B plan — pure authoring against an established schema, with the H-C3 harness already specified to validate the result.
Must not change: table schema; existing 49 tables' semantics; loot id vocabulary (reference guard).
Route: DATA-ONLY (four destination tranches, or fewer if the premise read shows some of the four are already covered — the read decides the count).
Continuity: new tables reference real item ids only; renewable/one-time classification follows the destination's settlement/cache nature in the locations catalogs.
Verification: integrity selftest; utilization selftest; H-C3 E[value] run for the new tables with results published; loot-reference guard tests green.
Open premises: enumerate the uncovered destinations in session (the 49/53 gap names the class, not the four ids).

## FP-B12 — Market-Rumor Band Extension

Lane B · C8/C11 · Status PROPOSAL.
Subject: extend the deterministic market rumor bands (kind 6, `EconomyMarketRumorRules`) with commodity coverage for goods that gained economic legs since the band set was authored, so price signal travels through the radio surface for the whole current economy.
Premise evidence: VERIFIED the rumor band bridge landed (DR-06 followups record); VERIFIED bands are deterministic by canon; VERIFIED commodity baselines and regional prices live. Open premise: enumerate which currently-traded goods lack band coverage — the premise read produces the tranche list.
Why this: the rumor bridge is a freshly sealed seam with an established pattern; extending its coverage is the cheapest systemic leg available in the economy-information space.
Must not change: band determinism rules; rumor-band RNG discipline (existing sub-stream); the retired availability consumer stays retired.
Route: DATA-ONLY (band coverage rows) + possible small loader extension if the band schema needs a field; verify the schema first.
Continuity: rumor distortion must follow the 5.17 contract (kernel plus distortion); bands must reconcile with price factors (the matrix doc is the authority).
Verification: integrity + utilization; focused rumor-band tests (deterministic replay); H-C6 harness run for the new bands.
Open premises: band schema read; coverage gap enumeration.

## FP-B13 — Intercept-Driven Journal Depth

Lane B · C8 · Status PROPOSAL.
Subject: faction intercepts producing journal records conditioned on the sealed authenticity and signal-trust patterns, so listening has a recordable, reviewable trace in the player's own voice.
Premise evidence: VERIFIED intercept catalog live; VERIFIED the signal-trust runtime is sealed (additive extension only); VERIFIED journal projection patterns exist (war-chain events project to journal, DR-11). INFERENCE: intercept→journal depth is the open premise.
Why this: it rides two sealed surfaces the correct way — additively — and deepens the information-flow loop that is a narrative pillar.
Must not change: sealed authenticity evaluator; genuine-never-hostile invariant; retired availability consumer.
Route: HOST-WIRING (journal projection) + data (journal-entry prose per the 5.1 contract). Save impact: EXISTING-SECTION (journal family).
Continuity: journal entries obey the first-person register; intercept facts already modeled only — no new intelligence content.
Verification: focused journal tests; integrity + utilization; prose contract checks.
Open premises: read the current intercept→journal surface.

## FP-B16 — Quest Reopening After New Discoveries

Lane B · C10 · Status PROPOSAL.
Subject: failed and abandoned quests reopen when a later discovery satisfies their availability conditions, with a reopening journal beat in the player's voice (the abandonment-text model already promises it: "they wait for someone to ask again").
Premise evidence: VERIFIED the reopening grammar is canon (v1.0 Part 6.7 recovery grammar; the 5.21 abandonment model); VERIFIED `PlayerVisitedTrigger` and war-chain journal/radio projection demonstrate the trigger-on-recorded-event precedent (DR-11/DR-12); VERIFIED quest availability fields include discovery methods (7.2 contract).
Why this: it is the recovery half of the failure grammar — the repository's design language promises it and the trigger precedents now exist to build it safely.
Must not change: quest state machine semantics for active/complete states; questline master spine behavior.
Route: CORE-EXTENSION through the quest owners (reopen transition with discovery-condition evaluation); data (reopen journal beats per quest family); save impact: EXISTING-SECTION (quest state families; verify whether reopen needs a persisted marker — if so, EXISTING-SECTION with an added field and default-tolerant loading per the save contract).
Continuity: reopening must respect information-flow legality (the discovery is the channel); reopening must not invalidate the weight_of_choices record of the original failure.
Verification: focused quest-state tests (fail → discover → reopen cycles, exactly-once reopening); determinism proof; save round-trip including mid-reopen states; integrity + utilization.
Open premises: read the quest state machine's transition surface and whether failure records persist discoverable conditions.

## FP-B17 — Moral-Choice Gossip Propagation Depth

Lane B · C9/C10 · Status PROPOSAL.
Subject: choice-driven gossip traveling the modeled channels (radio, courier, settlement contact) with travel time proportional to route distance, so a choice made at the crossing arrives at the flats days later and distorted.
Premise evidence: VERIFIED `moral_choice_gossip.json` live; VERIFIED information-flow rules are hard world rules (Part 3.3); VERIFIED courier dispatch and radio surfaces exist as channels; VERIFIED the 5.17 rumor contract defines distortion. Open premise: current gossip propagation depth (static broadcast vs channeled travel).
Why this: gossip with travel time makes the moral-choice system's social consequence legible across the map, and the channels already exist.
Must not change: moral-choice flag/weight semantics; radio sealed surfaces (gossip rides program/rumor seams, not signal seams).
Route: CORE-EXTENSION through the moral-choice owner's gossip path (channel routing + travel delay via existing transit models); data (gossip variants per channel). Determinism: existing seeded streams; travel delay derives from route distance, not RNG where possible. Save impact: EXISTING-SECTION (gossip records; verify).
Continuity: gossip obeys information-flow legality strictly (a settlement cannot react before arrival); distortion follows the rumor contract.
Verification: focused gossip tests (arrival ordering across distances, distortion bounds); determinism proof; integrity + utilization.
Open premises: read the gossip system's current propagation model.

## FP-B18 — Trade-Screen Scenario Expansion

Lane B · C11 · Status PROPOSAL (data-first).
Subject: additional trade-screen scenarios and tell lines for merchant identities under-covered relative to the merchant/caravan catalogs, deepening the read-the-trader minigame's content surface.
Premise evidence: VERIFIED `trade_screen_scenarios.json`, `trade_tell_lines.json`, `trade_specialties.json` live; VERIFIED merchant restock display-order priority is sealed (DEC-05) and untouched by scenario authoring.
Why this: pure data against an established schema, with zero sealed-surface contact — the safest Lane B content tranche in the economy cluster.
Must not change: scenario resolution mechanics; sealed restock ordering.
Route: DATA-ONLY.
Continuity: tell lines must be trainable (the tell-line system's premise is that tells are learnable); scenario outcomes must use the existing trade vocabulary.
Verification: integrity + utilization; focused trade tests if the scenario schema carries resolution fields (premise read decides).
Open premises: scenario schema read; under-coverage enumeration.

## FP-B19 — Winter Pressure for Power and Water Systems

Lane B · C12 · Status PROPOSAL.
Subject: Year-of-Ash window (180–360) pressure extensions for power and water: storm-window-conditioned filter burn, diesel reserve drawdown curves, and hot-water window reductions surfaced through the schedule-notice corpus (FP-A02 pairs with this).
Premise evidence: VERIFIED storm-window catalog live; VERIFIED hardening upgrades and insulation catalogs live; VERIFIED the maintenance and shelter baselines live (DR-03) as reconciliation anchors. INFERENCE: which draw constants are window-blind is the open premise (the H-C8 harness measures it before anything is authored).
Why this: the mid-winter pressure campaign (F-002) needs systemic legs, not only story arcs; power and water are the two systems every player touches daily.
Must not change: window canon (180–360); grid authority; XP W1's difficulty scalars (any tunable must route through the difficulty authority post-seal, or ship preset-blind until then — the plan must state which).
Route: CORE-EXTENSION (window-conditioned consumption in the power/water owners) + data (hardening/insulation coverage rows if gaps exist). Determinism: existing streams. Save impact: EXISTING-SECTION.
Continuity: load-shed amendments (FP-A02) must reconcile with the implemented windows — prose reflects the system.
Verification: H-C8 harness before/after tables; focused power/water tests (window vs non-window consumption); determinism proof; integrity + utilization.
Open premises: H-C8 run first; read current window-conditioning depth in power/water owners.

## FP-B20 — Reckoning Evidence Enrollment Sweep

Lane B · C13 · Status PROPOSAL.
Subject: audit systems added since the 19-wave (Waves 8–12: muster depth, moral-choice distress additions, newest industrial catalogs, EMP/grid/medical power work) for Reckoning-evidence enrollment gaps, and enroll any orphaned evidence classes through the existing enrollment path.
Premise evidence: VERIFIED the 19-wave closed the known enrollment surface with evidence (DR-06); VERIFIED subsequent waves added systems (DR-08 wave logs); VERIFIED enrollment is the sanctioned path and the epilogue matrix consumes enrolled evidence (canon). INFERENCE: post-19 additions with epilogue-relevant state but no enrollment are the target class — the audit enumerates them.
Why this: the epilogue matrix is only as complete as its enrollment; every wave that lands systems without enrollment checks quietly narrows the ending space.
Must not change: enrollment vocabulary (extend only through the endgame owners); matrix permutation semantics.
Route: audit (DOCS-ONLY) then CORE-EXTENSION per confirmed gap (enrollment through existing owners). Save impact: EXISTING-SECTION (evidence families) per gap.
Continuity: enrollment must respect information-flow legality (evidence the Reckoning can see is evidence that reached a record); each enrollment names its permutations.
Verification: focused endgame tests per enrollment; epilogue-reachability spot tests (G-04 pattern); integrity + utilization.
Open premises: the audit itself is the premise read.

## FP-B21 — Migration-Route Encounter Bridge

Lane B · C14/C5 · Status PROPOSAL.
Subject: wildlife migration state conditioning travel-encounter selection on routes crossing migration corridors, so the bestiary's seasonal movements are felt on the road.
Premise evidence: VERIFIED `WildlifeMigrationSystem` and `travel_encounters.json` live; VERIFIED the seasonal calendar is canon. INFERENCE: current coupling between migration state and encounter selection is the open premise.
Why this: the wildlife systems are a named low-connectivity island (v1.0 Part 7 gap 3) and this is the cheapest bridge from that island to the expedition loop.
Must not change: migration mechanics; encounter threat vocabulary.
Route: CORE-EXTENSION in the encounter selection path (migration-corridor conditioning); data (corridor-encounter rows via existing tables). Determinism: existing streams. Save impact: EXISTING-SECTION (encounter state) at most.
Continuity: corridor encounters must respect the bestiary's species behavior (no encounter a species cannot produce).
Verification: focused encounter tests (on-corridor vs off-corridor paired runs); determinism proof; integrity + utilization; bestiary consistency check.
Open premises: read the migration state surface and encounter selection input set.

## FP-B22 — Defense-Grid Siege and Harrow Threshold Coupling

Lane B · C15 · Status PROPOSAL.
Subject: perimeter defense values entering warlord siege/raid resolution math, and sky-layer armor values entering orbital-harrow telemetry thresholds, so the authored defense catalogs have systemic weight.
Premise evidence: VERIFIED defense catalogs, warlord doctrines, and orbital harrow telemetry are live and canon. INFERENCE (explicitly flagged): the atlas notes the sky-armor-to-weather bridge is partially built; the defense-to-siege coupling depth is the open premise. HIGH CONFIDENCE only that the catalogs are unconsumed or under-consumed, per the island-connectivity gap.
Why this: authored systems without systemic weight are the exact unclaimed-content defect class the census tracks (DR-08).
Must not change: warlord doctrine methods; harrow event canon.
Route: CORE-EXTENSION (consumption of defense values in the siege/harrow owners). Determinism: existing streams. Save impact: EXISTING-SECTION.
Continuity: siege outcomes respect information-flow legality (a faction raids where its intelligence says the shelter is weak); harrow thresholds must not fire outside authored event windows.
Verification: focused defense/siege tests; harrow threshold tests; determinism proof; integrity + utilization.
Open premises: read the siege resolution and harrow threshold surfaces; check the census for these catalogs first (SB-10 rule).

## FP-B23 — Difficulty Consumer Binding Completion (CF-XP01 line)

Lane B · C16 · Status PROPOSAL, sequence-gated on XP W1 seal.
Subject: complete difficulty preset scalar consumer binding across systems, each consumer reading the canonical difficulty authority post-W1, with no parallel scalars anywhere.
Premise evidence: VERIFIED W1 ACTIVE with the difficulty authority package as its first deliverable (DR-06); VERIFIED the completion-history v2 stamp is live with recorded checksum behavior (DR-11); VERIFIED the ledger records the CF-XP01 line as available. GATE: sequencing — this plan may not start before the W1 authority seals; it extends a sealed contract, per the DR-12 premise note.
Why this: difficulty binding is the multiplier for every future balance claim (H-C12 depends on it).
Must not change: the W1 authority's scalar definitions; any consumer that already reads the authority.
Route: CORE-EXTENSION per consumer (bind to authority; remove any parallel scalar found — removal is in-scope because parallel scalars violate the one-authority rule, but each removal is its own focused change with its own test).
Continuity: bound consumers must preserve current behavior at the default preset exactly (behavior-preservation rule).
Verification: G-08 pattern — one focused test per consumer (scalar flows from authority; no parallel scalar); harness probes post-bind (H-C12).
Open premises: W1 seal state at consumption time; consumer enumeration from the authority's own contract.

## FP-B24 — Stale-Panel Refresh Sweep

Lane B/E · C17 · Status PROPOSAL.
Subject: sweep panels whose data source gained fields or states since the panel shipped; expose truthful current state through the existing command surface; no new authority in any panel.
Premise evidence: VERIFIED the UI layout selftest and snapshot coverage surfaces exist (canon); VERIFIED Waves 8–12 added system states (DR-08); INFERENCE: which panels are stale is enumerable by the selftest, not by this document.
Why this: Invariant 5 (truthful state in panels) degrades silently as systems grow; the sweep is the standing repair.
Must not change: zero gameplay authority in panels (the invariant is the plan's own boundary); design language (DESIGN.md pinned).
Route: HOST-WIRING only, per-panel focused changes.
Continuity: a11y words-not-color-only for any new state display (E-05 pairs); snapshot updates per panel.
Verification: `--ui-layout-selftest`; snapshot gates; a11y gate; panel lifecycle tests.
Open premises: run the layout selftest to enumerate the stale set.

## FP-B25 — Rescue-Remains Medical Follow-Through

Lane B · C8/C2 · Status PROPOSAL, sealed-surface-coordinated.
Subject: where the campaign handles remains from the sealed sender-survival branch, route handling through the contagion/disease exposure rules and the memorial pipeline (the sealed runtime grants the remains; this plan governs what handling them costs).
Premise evidence: VERIFIED the remains/salvage branch exists in the sealed runtime (DR-06 handoff records); VERIFIED contagion and memorial systems live. GATE-class caution: the sealed surface's owners must co-sign any additive change touching its branch; this plan's scope is strictly downstream consumption.
Why this: it closes a consequence loop the sealed runtime leaves open — remains that are free to handle trivialize the death they represent.
Must not change: sealed runtime internals (arrival resolution, exactly-once guards); memorial wiring already sealed under DR-11 (`SpiritualMeaningCoordinator.RegisterDeath` path is the owner — this plan feeds it, never duplicates it).
Route: CORE-EXTENSION downstream of the sealed branch (exposure evaluation through the disease owner; rite affordances through the sealed memorial path). Determinism: existing streams. Save impact: EXISTING-SECTION.
Continuity: exposure respects pathogen vocabulary; rites respect the vigil mapping (DR-11); the dead appear only in memory contexts (hard world rule).
Verification: focused disease tests (remains-handling exposure cases); memorial pipeline tests untouched-and-green (regression proof that the sealed path was not altered); determinism proof.
Open premises: read the remains-handling downstream surface; obtain the sealed-surface owner's coordination note before implementation.

## Volume 10 wave sequencing

Lane B is wiring-heavy; waves must respect the data-first rule where tranches pair with it. Recommended: Wave F10-1 (FP-B08, FP-B18, FP-B12 — data-first economy/radio tranches whose premise reads are enumerations); Wave F10-2 (FP-B01, FP-B02 — the two debt-closure items, sequenced inside their own programs' terms); Wave F10-3 (FP-B03, FP-B19 — the winter/dose systemic pair, H-C8-gated); Wave F10-4 (FP-B07, FP-B21, FP-B16 — expedition/quest consequence bridges); Wave F10-5 (FP-B13, FP-B17, FP-B25 — information-flow trio, with FP-B25 owner-coordinated); Wave F10-6 (FP-B20, FP-B22, FP-B24 — audit-led items); FP-B05 and FP-B23 slot by premise-read readiness, FP-B23 strictly post-W1-seal. No more than five concurrent plans on shared seams; the expedition family appears in two waves but not concurrently.
---

# VOLUME 11 — TWENTY-TWO FULL SUBJECT PLANS FROM LANE D–J SEEDS (Factory batch 2026-09-27-A)

Standing premises: Factory Protocol sweep with the DR-14 debt-promotion-condition read; DR-11/DR-12 corrections incorporated; verification always names the integrity and utilization selftests plus the lane-specific gates. Plans in Lane D (save/compatibility) carry the heaviest risk classifications and therefore the strictest acceptance bars: old-save→new-build loading is never negotiable.

## FP-D01 — Exactly-Once Round-Trip Sweep for One-Time Effect Classes

Lane D · Cross-cluster · Status PROPOSAL.
Subject: extend the sealed rescue-runtime persistence pattern (persisted first result, exactly-once guards, duplicate-arrival guards, mid-effect round-trip) to the remaining one-time effect classes: one-time scavenging caches, bounty claims, unique-item claims, unrepeatable quest rewards, and any authored exactly-once consequence outside the rescue family.
Premise evidence: VERIFIED the rescue runtime models the pattern end-to-end (DR-06 handoff; sealed with exactly-once guards persisted in the checksummed radio save); VERIFIED `UniqueItemClaimRegistry` exists as a named one-time-claim owner; VERIFIED G-06 is specified as the regression suite for the sealed guards. Open premise: the enumeration of one-time effect classes not yet round-trip-proofed — the audit step is the premise read.
Why this first: an exactly-once effect that re-rolls after save/load is a silent integrity defect and the exact class the sealed runtime was built to end; sweeping it outward is the highest-value save work available that does not touch schema.
Must not change: sealed rescue guards (regression-only reference); claim semantics (a claim stays claimed).
Route: audit (DOCS-ONLY inventory of one-time effect classes with their persistence state) then, per confirmed gap, CORE-EXTENSION (persist the first result through the owning save section; add the guard) — one focused change per class. Save impact: EXISTING-SECTION per class; CODEC-BUMP-AND-MIGRATE only where the owning section lacks room for the guard marker, and then with full migration per the save contract.
Continuity: guards must key on stable identifiers (claim id + campaign id), never on transient state; restore must not replay and must not double-skip (the second half of exactly-once: a save taken mid-effect must resume, not drop).
Verification: per class — a fixture round-trip matrix (pre-effect save, mid-effect save, post-effect save, each restored into a fresh session); G-06 regression suite green (sealed guards untouched); determinism unchanged (guards are persistence logic, not rolls).
Open premises: the class inventory; per-class persistence surface reads.

## FP-D02 — Lineage Horizon Coverage to Day 3650

Lane D · C9 · Status PROPOSAL.
Subject: verify that lineage and generational facts simulated out to the Day-3650 epilogue horizon are covered by deterministic fixtures (the 19B closeout records a 3-year simulation; the canon horizon is ten years), and extend the simulation fixtures if the coverage window falls short.
Premise evidence: VERIFIED the 19B closeout records a 3-year deterministic balance simulation with three scoped links (DR-06); VERIFIED the generational horizon is canon (Day 3650, ten-year generational succession); VERIFIED the epilogue matrix evaluates the whole saga out to Day 3650. INFERENCE (explicit): the 3-year simulation covers the succession mechanics' mid-range, not the horizon; whether a 10-year fixture exists is unverified and is the open premise.
Why this: a generational epilogue evaluated beyond its tested horizon is a claim the fixtures do not back; save-adjacent and determinism-adjacent, this is the purest Lane D work in the survivors cluster.
Must not change: lineage mechanics; epilogue evaluation; simulation constants.
Route: TEST-FIXTURE extension (long-horizon deterministic simulation runs); no product code expected. If the fixture run exposes defects, they become findings with their own plans, not in-scope fixes.
Continuity: none beyond canonical time structure (Day 360 Reckoning, Day 3650 horizon).
Verification: two-pass byte-identical long-horizon runs; results reconciled against the 19B 3-year evidence at overlapping years (a fixture that contradicts the sealed 3-year run at years 1–3 is itself defective).
Open premises: read the lineage simulation fixture surface and its current horizon.

## FP-D03 — Epilogue Evidence Persistence Through the Horizon Window

Lane D · C13 · Status PROPOSAL.
Subject: confirm every enrolled Reckoning evidence class persists and remains readable through the Day-360→Day-3650 evaluation window, with a fixture test per class; add fixtures where absent.
Premise evidence: VERIFIED the epilogue matrix evaluates out to Day 3650 (canon); VERIFIED enrollment is the sanctioned path; VERIFIED FP-B20 will add post-19-wave enrollments that inherit this requirement. Open premise: per-class persistence behavior across the horizon is unmeasured.
Why this: pairs with FP-B20 — enrollment without persistence-verification is half a guarantee.
Must not change: evidence semantics; matrix permutation logic.
Route: TEST fixtures per evidence class; product change only where a defect is found (and then as its own focused fix).
Continuity: evidence classes must be tested with realistic late-campaign states (full ledgers, many dead, treaty webs), not minimal fixtures.
Verification: round-trip fixtures per class across the Day-360 boundary; byte-identical replay of the horizon evaluation.
Open premises: evidence class enumeration from the enrollment path.

## FP-D04 — Unknown-Field Tolerance and Missing-Field Default Audit

Lane D · Cross-cluster · Status PROPOSAL.
Subject: a registry-driven fixture audit proving, for every codec touched by Waves 8–12, that unknown fields are tolerated, missing fields default explicitly (never implicitly zero or null in a way that changes behavior), and corrupted envelopes are rejected rather than repaired.
Premise evidence: VERIFIED the save contract states these rules as CANON (v1.0 Part 12.3); VERIFIED `SAVE_STORE_CONTRACT_MATRIX.md` is the generated per-store registry; VERIFIED `SaveSupportWindowTests` is the historical fixture corpus pattern.
Why this: every new save section added by recent waves must inherit the contract; the audit converts the contract from documentation into a tested matrix.
Must not change: codec behavior — the audit tests reality; where reality fails the contract, the failure becomes a finding with its own plan.
Route: TEST-ONLY (fixture matrix generated from the store registry, one tolerance/default/corruption triple per store).
Continuity: corrupted-envelope rejection must be verified to reject with a usable error, not a crash.
Verification: the audit suite itself; focused per-store runs under the test policy caps.
Open premises: store enumeration from the live generated matrix.

## FP-D05 — Save Support Window Re-Pin (release-gated)

Lane D · Cross · Status PROPOSAL, release-gated.
Subject: re-pin the save support window (the set of historical versions the current build must load) at the next release-class version bump, refreshing the fixture corpus from the live store registry.
Premise evidence: VERIFIED the window is pinned per release by canon and enforced by `SaveSupportWindowTests`; VERIFIED the version discipline (three-source agreement, version gate) is canon.
Why this: a standing release obligation; the factory lists it so a release-planning session finds it without searching.
Must not change: window policy — the pin follows `VERSIONING.md`, not this document.
Route: TEST + release-process work at the release gate.
Verification: the pinned window suite green at release.
Open premises: the next release's version classification.

## FP-D06 — Shelter-Failure Mid-Event Save Semantics

Lane D · C1 · Status PROPOSAL, dependent on FP-B01.
Subject: define and fixture-test mid-failure-event save/restore behavior for the quarantine-exited failure cascades: a save taken during a cascade restores to a coherent cascade state — neither dropped nor double-fired.
Premise evidence: VERIFIED the cascade program exists (FP-B01); VERIFIED mid-event round-trip is a CANON save requirement where supported (v1.0 Part 12.3).
Why this: cascades are precisely the effect class most likely to be saved mid-flight (a daily save tick during a fire event); the semantics must be defined before the cascade exits quarantine, not after.
Must not change: cascade mechanics (FP-B01 owns them).
Route: DESIGN NOTE (one page, in the cascade program's own docs) + TEST fixtures for each cascade leg.
Continuity: restore must preserve exactly-once behavior for cascade consequences (linked to FP-D01's guards where legs are one-time).
Verification: mid-cascade round-trip fixtures; determinism replay across the boundary.
Open premises: FP-B01's exit criteria and cascade leg enumeration.

## FP-D07 — Moral-Choice Flag Persistence Audit

Lane D · C10 · Status PROPOSAL.
Subject: confirm every authored flag id in `moral_choice_flags.json` persists and round-trips, and that older saves missing newer flags load with explicit defaults; add the fixture matrix where absent.
Premise evidence: VERIFIED the flags catalog live; VERIFIED F-001 (the delayed-callbacks flagship) depends on this persistence as its dispatch-once key.
Why this: F-001's premise step; doing it as its own Lane D plan keeps the flagship's integration plan clean.
Must not change: flag semantics; weight_of_choices codec behavior.
Route: TEST-ONLY unless a defect surfaces.
Verification: per-flag round-trip fixtures including cross-version (old save with fewer flags).
Open premises: none beyond the catalog read.

## FP-D08 — Completion-History Difficulty Stamp Migration Readiness

Lane D · C16 · Status PROPOSAL, W1-coordinated.
Subject: prepare the completion-history stamp's migration behavior for the case where W1 adds preset fields: v1 records validate (sealed behavior, DR-11), v2 records carry `difficultyPresetId`; define v2→v3 semantics only if W1 actually adds fields to the stamp.
Premise evidence: VERIFIED the v2 stamp behavior including its checksum treatment (DR-11, from the sealed debt record); VERIFIED W1 is ACTIVE and may extend the authority contract.
Why this: the sealed record gives the exact current behavior; the plan exists to prevent an unplanned codec bump when W1 lands.
Must not change: the sealed v2 behavior.
Route: TEST + a one-page migration note in the W1 coordination doc, only if W1 adds stamp fields.
Verification: cross-version round-trip if a bump occurs; otherwise the sealed tests stay green untouched.
Open premises: W1's final authority contract.

## FP-E01 — Daily-Briefing Rows for Wave 8–12 Systems

Lane E · C17/J · Status PROPOSAL.
Subject: enumerate systems landed in Waves 8–12 that produce player-relevant daily state without a briefing row; add rows through the existing briefing surface only.
Premise evidence: VERIFIED the DailyBriefing panel is the canon briefing surface; VERIFIED the wave logs exist; VERIFIED J-05 pairs (new rows must be announced, not silent).
Why this: the briefing is the player's observe step for systemic change; unbriefed systems are functionally invisible to the loop.
Must not change: briefing selection logic (extend data through its owner, not a parallel briefing path); zero gameplay authority in the panel.
Route: HOST-WIRING + data (row content per the 5.24 world-state notification contract).
Continuity: rows must state what changed plainly; no alarm spam (the contract's own ban).
Verification: `--ui-layout-selftest`; briefing content tests if a briefing selftest family exists (premise read); snapshot updates.
Open premises: the system enumeration from wave logs; briefing selftest surface read.

## FP-E02 — Snapshot Coverage for Newest Panels

Lane E · C17 · Status PROPOSAL.
Subject: extend snapshot coverage to every panel shipped since the last snapshot wave, closing the gap between the generated coverage doc and the live panel set.
Premise evidence: VERIFIED snapshot coverage is generated and gate-enforced (canon); VERIFIED panels shipped in Waves 8–12 (DR-08 logs name panel-adjacent work: assignment UI, radio strip additions).
Must not change: panel rendering; the generated doc is never hand-edited (canon).
Route: snapshots + gate refresh, generated through the existing pipeline.
Continuity: snapshot fixtures must include truthful late-game states for stateful panels.
Verification: the snapshot gate green with the expanded set; ui-a11y gate green.
Open premises: enumerate the uncovered panels from the generated doc's delta.

## FP-E03 — Difficulty-Preset Selection Surface (W1-coordinated)

Lane E · C16 · Status PROPOSAL, sequence-gated on W1 seal.
Subject: the preset selection UI bound to the sealed difficulty authority, displaying observable consequences per the J-02 text contract, not adjectives.
Premise evidence: VERIFIED the difficulty presets catalog and the CF-XP01 line are live; VERIFIED the selection surface must post-date the authority (the ledger's own sequencing rule).
Must not change: authority scalars; input contract (22-action map respected; controller parity per E-06).
Route: HOST-WIRING post-seal, coordinated with FP-B23's consumer wave so the surface and the consumers land in one coherent wave.
Continuity: preset descriptions must be falsifiable statements about observable behavior ("filters last longer", not "harder").
Verification: layout selftest; a11y gate; controller parity; snapshot coverage.
Open premises: W1 seal; the authority's final field set.

## FP-E05 — Words-Not-Color-Only Accessibility Sweep

Lane E · C17 · Status PROPOSAL.
Subject: audit all panels for color-only state signaling; add textual state indicators wherever a state is communicated by color alone, per ACCESSIBILITY.md.
Premise evidence: VERIFIED the a11y gate and ACCESSIBILITY.md are canon; VERIFIED the gate enforces the rule at CI time for changed panels — the sweep covers panels not recently touched, which the gate does not retroactively inspect.
Why this: the gate is change-triggered, so old panels escape it; a one-time sweep closes the accumulated gap and the gate holds it closed thereafter.
Must not change: design language (textual indicators within the pinned visual system).
Route: HOST-WIRING, per-panel focused edits.
Continuity: textual state must be truthful current state read through the same commands.
Verification: a11y gate; manual screen-reader-order review per changed panel; snapshots.
Open premises: the audit enumeration (the sweep's own first step).

## FP-E06 — Controller Parity for New Panels

Lane E · C17 · Status PROPOSAL.
Subject: focus-navigator parity for panels shipped without full 22-action-map coverage, so every interactive control is reachable and operable by controller.
Premise evidence: VERIFIED the input contract is canon (22-action map, focus navigator, rebinding); VERIFIED controller parity is a named C2 line (`C2[15]/Plan 37` recorded available in the v1.0 queue snapshot).
Must not change: the action map itself; rebinding behavior.
Route: HOST-WIRING per panel.
Continuity: focus order must follow the visual reading order unless a documented exception exists.
Verification: layout selftest; input parity tests where the selftest family covers them (premise read); manual controller pass per changed panel.
Open premises: parity test surface read.

## FP-E07 — Expedition Camp Panel State Completeness

Lane E · C5 · Status PROPOSAL.
Subject: audit the expedition camp panel against the full truthful expedition state (vehicle condition, filter/fuel/ammo reserves, party vitals, weather-gate state) and expose any missing truth through the existing panel surface.
Premise evidence: VERIFIED the expedition camp panel exists (v1.0 Part 5.7); VERIFIED the expedition state surface is rich (party, stance, supplies, gates — v1.0 Part 6.2); INFERENCE: which fields the panel omits is the layout-selftest question.
Why this: the camp panel is the commit step's decision surface; omitted truth there is a loop-level defect, not a cosmetic one.
Must not change: zero authority in the panel; dispatch logic untouched.
Route: HOST-WIRING.
Continuity: displayed reserves must reconcile with the ledger's own records at all times (a displayed reserve is a claim).
Verification: layout selftest; focused reconciliation test if the panel reads caches (a cache is acceptable only if owned — verify).
Open premises: layout selftest run; cache ownership read for each displayed value.

## FP-E09 — Storm-Window Forecast Legibility

Lane E · C12 · Status PROPOSAL.
Subject: the weather forecast surface renders storm-window warnings with enough lead time and specificity (window number, expected severity, recommended action from the almanac voice) for the player to act during the 180–360 window.
Premise evidence: VERIFIED the forecast observation step is the first loop step (CANON); VERIFIED the storm-window catalog is live; VERIFIED FP-A27 authors almanac entries conditioned on the same windows (the data side pairs).
Why this: FP-B19 adds window-conditioned costs; the observe step must surface them or the costs are unreadable.
Must not change: forecast generation logic (the panel reads truth); window canon.
Route: HOST-WIRING + data (the forecast's recommended-action text rides the almanac corpus per FP-A27).
Continuity: forecast text must never promise certainty beyond the record (5.10 contract).
Verification: layout selftest; a11y gate (warnings must be textual, not color-only — pairs with FP-E05); snapshot.
Open premises: current forecast surface field read.

## FP-E10 — Reckoning Evidence Submission Surface Audit

Lane E · C13 · Status PROPOSAL.
Subject: audit the Reckoning's evidence-submission affordances: for each enrolled evidence class, can the player see what is enrolled, what remains submittable, and what the tribunal has received? Expose missing truth through the existing endgame panels.
Premise evidence: VERIFIED the Reckoning consumes enrolled evidence (canon); VERIFIED endgame panels exist (Epilogue, Verdict families); INFERENCE: submission-surface completeness is unverified and is the audit's question.
Why this: the Reckoning is the campaign's tribunal climax; an unreadable evidence state there is the highest-stakes legibility defect available.
Must not change: enrollment or evaluation logic.
Route: HOST-WIRING per confirmed gap.
Continuity: displayed evidence must be exactly the enrolled set (no aspirational display).
Verification: layout selftest; endgame-focused tests; a11y gate.
Open premises: the endgame panel surface read.

## FP-F01 — Per-Frame UI Allocation Audit

Lane F · C17 · Status PROPOSAL (measurement-first).
Subject: instrument the shell components (dashboard shell, metric cards, data grids) during a 15-FPS headless session and record per-frame allocations; publish to `docs/perf/`; optimize only what the numbers demonstrate.
Premise evidence: VERIFIED 15-FPS headless runtime sessions are the canon test cadence; VERIFIED the Performance Core family and CI performance gate exist; VERIFIED the atlas flags per-frame allocations as a candidate class.
Why this: the lane's own rule — profile before rewrite — makes measurement the entire first plan; any repair it justifies gets its own plan with before/after numbers.
Must not change: nothing (measurement only).
Route: TOOLING (instrumentation harness) + `docs/perf/` publication.
Continuity: none (measurement cannot contradict canon).
Verification: the harness's own determinism (repeated runs of identical sessions produce identical allocation traces, modulo engine noise — noise itself gets documented).
Open premises: instrumentation surface availability in the headless profile.

## FP-F02 — Storm-Window Tick Concentration

Lane F · C12 · Status PROPOSAL (measurement-first).
Subject: measure per-day tick cost inside versus outside storm windows across the 180–360 range (weather, fallout, gates, morale co-firing), publishing window/non-window cost ratios to `docs/perf/`.
Premise evidence: VERIFIED the co-firing systems are canon (Part 4.5: weather drives radiation, routes, power, morale); VERIFIED the window catalog is live.
Why this: if window days spike, the spike is player-visible exactly when the game is hardest — the worst possible frame to lose; measurement precedes any claim.
Must not change: nothing (measurement only).
Route: TOOLING + `docs/perf/` publication.
Continuity: none.
Verification: identical-session cost traces; documented noise floor.
Open premises: tick instrumentation surface.

## FP-F04 — Shelter Room Tree-Search Frequency

Lane F · C1 · Status PROPOSAL (measurement-first).
Subject: instrument repeated node/path lookups in shelter systems across a 30-day simulation and publish lookup-frequency tables per system.
Premise evidence: VERIFIED repeated tree searches are a named candidate class in the atlas; VERIFIED the 30-day simulation pattern exists (maintenance report precedent).
Must not change: nothing.
Route: TOOLING + `docs/perf/`.
Continuity: none.
Verification: trace determinism across repeated runs.
Open premises: instrumentation feasibility in the headless profile.

## FP-F06 — Late-Game Save-Flush Cost

Lane F · Cross · Status PROPOSAL (measurement-first).
Subject: measure daily save-flush duration for synthetic late-game states (many survivors, full dose ledger, long journals, dense standing records) against the tick budget.
Premise evidence: VERIFIED the daily save flush is a canon tick step; VERIFIED late-game state richness is canon (ledgers, chronicles, lineages).
Must not change: nothing.
Route: TOOLING + `docs/perf/`; the synthetic fixture doubles as a Lane D test asset (shared fixture, two consumers — the factory notes the reuse to avoid duplicate fixture authoring).
Continuity: the fixture must be generated deterministically for reuse.
Verification: cost traces; fixture determinism proof (shared with Lane D).
Open premises: synthetic-fixture generation surface.

## FP-G01 — Moral-Choice Flag Consumer Coverage Suite

Lane G · C10 · Status PROPOSAL.
Subject: the focused test suite proving every authored flag id has at least one consumer path and every consumer reads a persisted flag; aggregate with per-row failure output per the TEST-AGGREGATION pattern.
Premise evidence: VERIFIED the flags catalog live; VERIFIED the aggregation metadata pattern is the canon test style for homogeneous catalog checks; VERIFIED F-001 and FP-D07 both depend on this suite.
Why this: it is the premise-proof for the flagship and the persistence-proof for the lane — one suite, three consumers.
Must not change: nothing (test-only).
Route: TEST-ONLY.
Continuity: none.
Verification: the suite itself; focused run under the 180-second cap.
Open premises: consumer enumeration (the suite's own discovery step, asserted as a count).

## FP-G02 — Debt-Consequence Dispatcher Coverage Suite

Lane G · C11 · Status PROPOSAL.
Subject: focused coverage for every consequence kind in the debt dispatcher's closed vocabulary, each with a recovery-path assertion per the recovery grammar.
Premise evidence: VERIFIED the dispatcher's closed vocabulary (sender_death/faction_standing_loss/faction_ambush is the rescue example class); VERIFIED `LedgerDebtSystem` consequence dispatchers and bounty records are canon.
Why this: H-C7 classifies unrecoverable states; this suite asserts the modeled recovery affordances actually dispatch.
Must not change: nothing.
Route: TEST-ONLY.
Continuity: none.
Verification: the suite; per-kind failure output.
Open premises: vocabulary enumeration from the dispatcher source.

## FP-G03 — Dose-Treatment Matrix Pairing Suite

Lane G · C2 · Status PROPOSAL.
Subject: pin every `MEDICAL_DOSE_TREATMENT_MATRIX.md` row to its implementing treatment logic so the generated matrix cannot drift from code; the generation check fails when a row's cited implementation moves or changes behavior.
Premise evidence: VERIFIED the matrix doc is live (DR-03); VERIFIED generation-checked docs are an established pattern (changelog, CLI catalog — the generated-docs family).
Must not change: the matrix is generated, never hand-edited (canon).
Route: TEST + generation-check extension in the existing generator family.
Continuity: none.
Verification: the pairing suite; the generation check green.
Open premises: matrix row schema and generator surface read.

## FP-G04 — Epilogue Permutation Reachability Suite

Lane G · C13 · Status PROPOSAL.
Subject: deterministic tests that each of the 32 permutations is reachable from some authored campaign state, and that no optional content can invalidate the main ending (the hard world rule).
Premise evidence: VERIFIED the 32-permutation matrix is canon; VERIFIED the hard world rule is canon; VERIFIED F-005 and FP-B20 both feed this surface and inherit its guarantee.
Why this: reachability is the matrix's integrity; the hard rule is the canon's most absolute sentence — both deserve executable proof, not documentation.
Must not change: nothing.
Route: TEST-ONLY; deterministic campaign-slice fixtures per permutation (seeded, replayable).
Continuity: fixtures must be minimal-but-real (authored starting states through the sanctioned starting-survivor/level catalogs, never synthetic impossible states).
Verification: the suite green; two-pass replay of at least one fixture per permutation class.
Open premises: permutation input enumeration from the matrix runtime.

## FP-G05 — War-Chain Authored-Day Mapping Boundary Suite

Lane G · C7 · Status PROPOSAL.
Subject: boundary tests pinning the 300-day offset mapping (playable 180 → authored 480), including the documented clamp behavior (timeline/thermal/radon clamp at 360 while war chains run in authored space).
Premise evidence: VERIFIED the mapping and its clamp behavior from the sealed debt record (DR-11); VERIFIED `FactionWarClockTests` 1/1 exists as the seed of this suite.
Why this: the sealed record documents the behavior; a boundary suite keeps the next clock change honest (the promotion condition itself says "do not re-author JSON minDays without a new clock contract" — the suite is that contract's teeth).
Must not change: nothing.
Route: TEST-ONLY, extending the existing clock test family.
Continuity: none.
Verification: the suite; focused run.
Open premises: none beyond source read.

## FP-G08 — Difficulty Consumer-Binding Test Wave

Lane G · C16 · Status PROPOSAL, sequence-gated with FP-B23.
Subject: for every consumer bound under FP-B23, a focused test that the scalar flows from the authority to the consumer and no parallel scalar exists (a source-scan assertion, not just a value assertion).
Premise evidence: VERIFIED G-08 was specified with this shape in Volume 3; VERIFIED the DR-12 note that the stamp contract is the binding precedent.
Why this: parallel scalars are the one-authority violation most likely to regress silently; a scan assertion catches reintroduction.
Must not change: nothing.
Route: TEST-ONLY, one file per consumer, aggregate metadata per the test policy.
Continuity: none.
Verification: the wave's suites; the scan assertion demonstrated on a synthetic violation in a sandboxed test (proving the assertion can fail).
Open premises: FP-B23's consumer enumeration.

## FP-H01 — Data-Authority Non-JSON Assertion

Lane H · Data authority · Status VERIFIED need (DR-05), PROPOSAL handling.
Subject: a CI assertion that `Assets/StreamingAssets/Data/` contains only JSON catalogs plus an explicit whitelist, making the DR-05 hygiene question permanently machine-enforced once FP-009's resolution lands.
Premise evidence: VERIFIED `rewrite.py` sits in the data authority (DR-05); VERIFIED the integrity selftest family is the enforcement precedent.
Must not change: nothing until FP-009's call-site verdict is in (the whitelist derives from that verdict, not from here).
Route: TOOLING (small CI script in the established gate family), FP-009-dependent.
Continuity: the whitelist must be reviewed at each addition — an empty-by-default whitelist with named entries.
Verification: the gate green; synthetic-violation demo (a temp non-JSON file in a test fixture fails the gate).
Open premises: FP-009's verdict.

## FP-H05 — Seeded Sub-Stream Registry

Lane H · Cross · Status PROPOSAL (existing-implementation-first).
Subject: a documented registry of sanctioned `StableHash`-derived RNG sub-stream names, so planners stop inventing stream names ad hoc and integrity rules can reject unregistered streams.
Premise evidence: VERIFIED the sub-stream pattern is canon (dedicated streams per subsystem; `StableHash`-derived names); HIGH CONFIDENCE a de-facto list exists across subsystems (the sealed authenticity evaluator's stream is named in evidence); UNVERIFIED whether a registry already exists — the existing-implementation search must run first (the factory's own rule; if a registry exists, this plan extends it, not duplicates it).
Must not change: existing stream names (renaming a stream changes determinism replays — absolutely forbidden without a migration-class reason).
Route: DOCS-ONLY registry + optional integrity rule (unregistered stream names fail a check).
Continuity: the registry documents each stream's owning subsystem and purpose; entries are append-only.
Verification: the registry reconciles with a source-scan of stream derivations (the scan is the registry's own proof).
Open premises: the existing-implementation search.

## FP-H06 — Save Envelope Inspector

Lane H · Lane D adjacent · Status PROPOSAL (existing-implementation-first).
Subject: a developer-facing save-envelope inspector (checksum status, codec versions, section sizes, slot-root isolation view) — built only if `docs/debug/` and the tooling tree lack an equivalent.
Premise evidence: VERIFIED `docs/debug/` exists (DR-02 listing); UNVERIFIED whether an inspector already exists — the search must run first.
Must not change: nothing in the save path (the inspector reads, never writes).
Route: TOOLING, read-only, in the established developer-tool family.
Continuity: the inspector must never offer repair (corrupted envelopes are rejected, not silently repaired — the tool must honor the contract).
Verification: the inspector's output reconciles with the store registry on a fixture save; read-only assertion (no write surface at all).
Open premises: existing-tool search.

## FP-I01 — Authority-Map Gap Registry and Fill

Lane I · Cross · Status PROPOSAL.
Subject: enumerate `docs/` domains whose directory exists but whose authority map does not (the DR-02 listing provides the domain set; the live docs top-level provides the map set), then fill the gaps in priority order (domains that gained systems in Waves 8–12 first).
Premise evidence: VERIFIED the docs listing (DR-02) and the map set (v1.0 Part 5.8 plus DR-03's new entries); the difference set is enumerable in one session.
Why this: authority maps are the duplication firewall's human-readable layer; unmapped domains are where parallel systems get proposed by mistake.
Must not change: no system behavior; documentation describes reality only.
Route: DOCS-ONLY, one map per domain, each verified against live source before writing (a map that documents a nonexistent architecture is a canon violation of the documentation lane itself).
Continuity: each map names its domain's owners, catalogs, sessions, seams, and open factory seeds.
Verification: the docs index drift gate; per-map foreman review.
Open premises: the difference enumeration.

## FP-I03 — Save-Authoring Rules Consolidation

Lane I · Lane D adjacent · Status PROPOSAL.
Subject: one consolidated save-authoring rules document for planners: codec, migration, checksum, atomic write, slot-root isolation, support window, and round-trip discipline, distilled from the scattered canon sources (save architecture, save authoring contract, store matrix).
Premise evidence: VERIFIED the source material exists and is scattered (v1.0 Parts 5.3, 12.3; the generated store matrix); VERIFIED planners repeatedly need it (every Lane D plan cites the same rules).
Must not change: nothing; the doc cites its sources and defers to them (it is a lens, not a new authority — the one-document-one-authority rule applies to docs too).
Route: DOCS-ONLY.
Continuity: the doc must carry a staleness rule: it defers to the generated store matrix on any conflict.
Verification: review against each source; the drift rule stated in the doc itself.
Open premises: none.

## FP-I06 — Factory Session Playbook

Lane I · Process · Status PROPOSAL.
Subject: a one-page operator's guide for running a Factory Protocol session: premise sweep inputs (including the DR-14 debt-promotion-condition read), seed consumption, Template S/R expansion, volume append, ledger update — written for the multi-agent workflow so any tool (Codex, Claude, or human foreman) can run a session identically.
Premise evidence: VERIFIED the factory protocol (this document, Part II); VERIFIED the multi-agent discipline requires per-tool operability (DR-09 agent sprawl).
Must not change: nothing.
Route: DOCS-ONLY (this volume is its own first draft; the playbook compresses it to one page for operators).
Continuity: the playbook defers to this document on any conflict.
Verification: a dry-run session executed by a second operator following only the playbook, with divergences recorded and the playbook corrected.
Open premises: none.

## FP-J01 — Onboarding Flow for Wave 8–12 Systems

Lane J · Cross · Status PROPOSAL.
Subject: onboarding coverage for systems that landed without onboarding entries; each rides the existing onboarding seams (the onboarding selftest family is the completeness gate).
Premise evidence: VERIFIED `Onboarding/` Core and the onboarding selftest are canon; VERIFIED Waves 8–12 landed systems (DR-08 logs).
Why this: onboarding is the J-lane's core debt and its selftest is the cleanest acceptance bar in the repository.
Must not change: onboarding mechanics.
Route: DATA-ONLY + host wiring per the onboarding owner's own surface.
Continuity: onboarding entries must be true at first-session time (no spoiler-class information in onboarding text).
Verification: onboarding selftest; integrity + utilization.
Open premises: the coverage enumeration from the selftest's own output.

## FP-J04 — First-Hour Information Audit

Lane J · Onboarding · Status PROPOSAL.
Subject: a structured manual playtest auditing that the first hour surfaces every loop step (observe, interpret, prioritize, commit, pay, receive, adapt) at least once, with gaps recorded and routed to onboarding or briefing plans.
Premise evidence: VERIFIED the loop is CANON (v1.0 Part 1.3); VERIFIED the manual playtest checklist pattern exists (Holdfast manual, expedition playtest report).
Must not change: nothing (audit only; findings route to plans).
Route: DOCS-ONLY (playtest report) feeding J/E seeds.
Continuity: the audit runs on the default difficulty with authored starting states.
Verification: the report names each loop step with the minute it first surfaced, or records its absence.
Open premises: scheduling a manual session at the 15-FPS test cadence.

## Volume 11 sequencing

Lane D first within any wave that contains it (save work precedes the UI that displays its state). Recommended: Wave F11-1 (FP-D04, FP-D07, FP-G01 — the persistence/consumer trio around the moral-choice flagship); Wave F11-2 (FP-D01 — the exactly-once sweep, alone by its breadth); Wave F11-3 (FP-D02, FP-D03 — the horizon pair); Wave F11-4 (FP-E01, FP-E02, FP-E05, FP-E06 — the UI surface wave); Wave F11-5 (FP-E07, FP-E09, FP-E10 — the legibility trio); Wave F11-6 (FP-F01, FP-F02, FP-F04, FP-F06 — measurement wave, publishable together); Wave F11-7 (FP-G02, FP-G03, FP-G04, FP-G05 — the test wave); Wave F11-8 (FP-H01, FP-H05, FP-H06, FP-I01, FP-I03, FP-I06 — tooling and docs wave); Wave F11-9 (FP-J01, FP-J04, FP-D05, FP-D06, FP-D08, FP-E03, FP-G08 — the gated and paired remainder). Concurrency rule respected: no wave exceeds five plans on shared seams; the save family never shares a wave with a wave that edits its consumers.

---

# VOLUME 12 — SIGNATURE-READY DECISION PACKETS AND THE C2 GAP PACKAGE (Factory batch 2026-09-27-B)

The three GATE items and the F-011 package, prepared as decision packets: each packet states the decision to be made, the options with their consequences, the recommended option, and exactly what unblocks the moment the signature lands. Nothing here asks the reader to approve implementation; each packet asks for one decision that converts a GATE seed into a normal plannable seed.

## 12.1 Decision Packet DP-01 — Flooded-Route Topology Tags (seed B-09)

**Decision required:** whether the wasteland map's authored edges gain a flood-topology tag class consumable by weather route gates, and if so, which edge set is authored first.

**Current state (VERIFIED/HIGH CONFIDENCE):** the map is a node graph with route gates including weather-gated crossings and force-passage (`WeatherRouteGateCatalog`); ice-road and deep-coast blocks are absolute; the foreman flagged flooded-route topology tags as an open decision requiring authored map edges (v1.0 Part 7 gap 4). The gate mechanism exists; what does not exist is the authored data the decision gates.

**Options:**
- Option A — Author a full flood-tag pass over all edges. Cost: large data tranche; every edge gets flood semantics. Consequence: complete but slow, and most edges may never flood in practice (authoring effort where no system will ever select the state).
- Option B — Author a bounded pilot: flood tags on edges adjacent to water-bearing locations only (the canal, the deep coast, the crossing family), gates consuming tags where the gate catalog already has weather classes. Consequence: small, replayable on the Plan 76.2 harness, extends later edge-by-edge.
- Option C — Decline the tag class; floods remain represented by existing weather classes only. Consequence: zero cost; the atlas's flagged opening stays closed and the recorded rationale is documented.

**Recommendation:** Option B. It matches the repository's bounded-pilot pattern (the rescue runtime, the distress waves), it produces measurable route-selection changes for the H-C8/H-C3 harnesses, and it preserves Option A as a later extension rather than a rewrite. The recommendation is PROPOSAL until signed.

**What unblocks on signature:** seed B-09 converts from GATE to PROPOSAL; its plan is then drafted through the Factory Protocol with the pilot edge set enumerated from the map catalog in the premise sweep. Integration route pre-analysis (for the signature's information only): DATA-ONLY (edge tags) + gate consumer extension; the orphan gate (`AllMapNodes_ExistInLocationsCatalog`) governs; determinism unchanged (tags are authored data, not rolls).

**Verification the eventual plan must name:** map integrity selftests; route-selection paired tests (flooded vs dry, same seed); H-C3 re-run for affected destinations; two-pass replay.

**Risks to state in the decision record:** route invalidation during flood windows could strand in-transit expeditions — the packet must state the in-transit policy (delay, force-passage cost, or divert) as part of the signature, because that policy is a design decision, not an implementation detail.

## 12.2 Decision Packet DP-02 — FactionWar Per-Strike Emitter Extension (seed B-10)

**Decision required:** whether faction war chains emit per-strike events (beyond the sealed stage/chain-level journal+radio projection), and if so, with what projection surface and throttling.

**Current state:** VERIFIED the war chains run in authored-day space with stage/chain events projecting to journal and radio (DR-11 sealed record); VERIFIED the foreman flagged the per-strike emitter extension as an open decision (v1.0 Part 7 gap 4); VERIFIED the war-chain family of catalogs (communiqués, dialogue, events, journal, radio, location overrides) is live and would be the projection surface.

**Options:**
- Option A — Full per-strike emission: every strike in the chain war emits a journal record, radio item, and sound-ranging observation. Consequence: rich autonomous-world texture; risk of journal and radio spam during active wars (the world-state notification contract bans alarm spam — the volume question is a design question, not a tuning afterthought).
- Option B — Per-strike emission with a deterministic throttle: strikes emit at full cadence into the war-journal corpus (the dedicated record), but player-facing surfaces (radio strip, briefing) receive strikes only when they cross a relevance threshold (location adjacency to player-visited or treaty-relevant sites — the `PlayerVisitedTrigger` precedent supplies the adjacency key). Consequence: the record is complete while the surface stays legible.
- Option C — Keep stage/chain-level projection only. Consequence: the sealed behavior stands; the atlas's flagged opening stays closed with rationale.

**Recommendation:** Option B. The autonomous-world pillar requires the record; the briefing and radio contracts require legibility; the throttle key (relevance by adjacency) is deterministic and uses an existing trigger pattern. PROPOSAL until signed.

**What unblocks on signature:** seed B-10 converts from GATE to PROPOSAL; the Factory Protocol run then reads the war-chain emitter surface and drafts the plan. Integration route pre-analysis: CORE-EXTENSION (emitter) + data (projection prose per the communiqué/journal contracts); determinism: existing war-chain streams; save impact: EXISTING-SECTION (war-chain family).

**Verification the eventual plan must name:** focused war-chain tests (per-strike emission, throttle determinism); journal/radio volume assertions (bounded counts per window); two-pass replay; G-05 suite green (the clock boundary must be untouched).

**Risks to state in the decision record:** information-flow legality — a strike reaching the player's surfaces must have a modeled channel (radio intercept, courier, adjacency observation); the throttle's relevance key must be named in the signature because it is the design's definition of "worth hearing."

## 12.3 Decision Packet DP-03 — Black-Market Funds Authority (seed B-11)

**Decision required:** whether the black market gains a canonical funds leg (a currency-like settlement instrument), and if so, whether it mints a new funds id class or reuses an existing commodity as the settlement unit.

**Current state:** VERIFIED the black-market actions surface is sealed (`WAVE8-PART2-C1-BLACK-MARKET-ACTIONS`, DR-06); VERIFIED the funds/goods legs remain decision-blocked pending a canonical funds authority (v1.0 Part 7 gap 4); VERIFIED the reward grammar's own rule says avoid inventing new currencies (Part 6.7) — this decision is the sanctioned exception process, and the rule is why the decision needs a signature rather than a plan.

**Options:**
- Option A — New funds id class (e.g., `item_market_script`). Consequence: clean settlement semantics; violates the no-new-currencies default and therefore requires the strongest justification: it must solve a settlement case no existing commodity can.
- Option B — Reuse an existing high-liquidity commodity as the canonical settlement unit (the commodity baselines catalog is the liquidity evidence base; the harness H-C1/H-C6 output identifies candidates). Consequence: no new currency; the black market prices goods in a unit the rest of the economy already understands; risk: the chosen commodity's own supply shocks become black-market settlement shocks (a feature under the scarcity pillar, but it must be stated).
- Option C — Goods-only settlement persists (barter only). Consequence: the sealed actions surface stands; the decision closes with rationale.

**Recommendation:** Option B, with the specific commodity chosen from harness evidence, not preference. The recommendation is deliberately incomplete: naming the commodity here, without the harness run, would be exactly the unverified-premise failure the factory exists to prevent. PROPOSAL until signed; the signature may itself defer commodity choice to the harness output.

**What unblocks on signature:** seed B-11 converts from GATE to PROPOSAL; the plan runs H-C6 and H-C1 first, proposes the settlement commodity with evidence, and only then drafts the settlement leg through the sealed black-market surface's own seams.

**Verification the eventual plan must name:** H-C6/H-C1 evidence tables; settlement round-trip tests through the sealed surface (regression: the sealed actions surface unchanged); economy harness re-run showing the settlement leg's systemic effect; determinism proofs.

**Risks to state in the decision record:** the sealed-surface coordination requirement (the WAVE8-PART2 owner co-signs); the supply-shock coupling stated above; the support-window question if settlement records persist (they will — a funds ledger is a save section, so the codec path must be named in the plan, not discovered during implementation).

## 12.4 The F-011 C2 Open-Gap Package — Signature-Ready Scoping Notes

The three items the ledger records as real, unexecuted gaps (DR-06). Each note states what the closure report says, what the plan must read, and what the verification class is — so the package can be split into three normal plans the moment an owning session picks them up.

**F-011-a — Plan 31 semantic-kind authority.** Ledger claim (VERIFIED as recorded): Plan 31 (semantic-kind re-grouping, D11) was decision-blocked; the C2[2] closure lists "Plan 31 semantic-kind authority" among real gaps needing packages. What the plan must read: the D11 decision record, the current `DayEventVocabulary` (whose no-silent-drop repair landed under 17A-S with 8 tests), and the event semantic parity matrix (gate-enforced). Verification class: focused vocabulary/parity tests; the parity gate green; determinism unchanged. Decision dependency: D11's named signature — this item is GATE until that signature lands, and the packet's job is to make that signature a single informed act: the signature decides the semantic-kind grouping principle, not the implementation.

**F-011-b — 17C Phase I alert ducking/concurrency.** Ledger claim (VERIFIED as recorded): audio alert ducking and concurrency phases were not executed and remain real gaps. What the plan must read: the C2 audio phase documentation, the audio-condition system surface, and the AudioEventBridge wiring precedent (the radiation exposure-end lifecycle wired 7 tests under the same program). Verification class: focused audio-condition tests; the cue field additions must be additive per the distress baseline's own finding ("audio needs only additive cue fields"). No decision blocker recorded — this item is plannable now through the Factory Protocol.

**F-011-c — 17C Phase E acquisition sweep + 17B deep test matrix.** Ledger claim (VERIFIED as recorded): both named as not executed and real. What the plan must read: the closure report's definitions of Phase E and the deep test matrix (this volume does not paraphrase them — the closure report is the authority). Verification class: per the closure report's own acceptance language. No decision blocker recorded — plannable now.

**Package sequencing note:** F-011-b and F-011-c are plannable immediately; F-011-a is GATE. The package should therefore be split: b and c enter the normal backlog as FP-series plans; a enters the decision-packet queue beside DP-01 through DP-03 as DP-04 (semantic-kind grouping principle, signature: the D11 record's named owner).

## 12.5 Decision-packet queue after this volume

DP-01 flooded-route tags · DP-02 per-strike emitters · DP-03 funds authority · DP-04 semantic-kind grouping principle (new, from F-011-a). Standing rule: the factory never drafts implementation plans for these; it maintains the packets so that when a signature lands, conversion is one session's work. The D22 string freeze and any other decision-blocked item found in future re-audits join this queue with the same packet shape.

---
# VOLUME 13 — PER-LANE EXPANSION PLAYBOOKS (Factory batch 2026-09-27-C)

Ten chapters, one per lane: the lane's contract, its evidence inputs, its worked end-to-end example (seed → premise sweep → plan tranche → verification), its failure modes, and its no-change areas. Each playbook is the operating manual for a lane-specialist session.

## 13.1 Lane A playbook — Narrative and prose

**Contract:** prose fills named fields with contracts (Part 9; Volumes 4–5), in the house voice, through existing catalogs and corpora. The lane never writes "descriptions"; it fills fields.
**Evidence inputs:** the narrative corpus index (collision sweep), the target catalog's schema sheet (7.10 registry), the prose field contracts, the duplication firewall.
**Worked example (FP-A12, compressed):** seed "destination arrival prose completion" → sweep: enumerate the 53 destinations, measure prose depth per destination, select the ten thinnest → plan: one tranche, ten destinations, contracts cited per field → author: arrival/revisit pairs per the 80–140/60–110 word contracts, imagery-overlap check against each destination's five neighbors → verify: integrity selftest 0 findings, utilization selftest proves consumption, manual contract checks per entry.
**Failure modes:** inventing location facts the catalog does not authorize (the prose exceeds the canon); duplicating a neighboring destination's imagery; spoiler leakage into arrival text; writing fields the loader does not consume (authoring without a consumer is the unclaimed-content defect class).
**No-change areas:** corpus entries whose craft or institution already has a differentiated corpus family — extension requires voice-or-evidence difference or a merge recommendation, never silent parallel authoring.

## 13.2 Lane B playbook — Mechanics and systems functionality

**Contract:** extend owning Core systems through their existing seams; Core stays engine-free; all randomness through `ISeededRng`; state through Capture/Restore; one authority per concern.
**Evidence inputs:** the owning system's source (call sites included — the call-site requirement), the census (unclaimed-content check), the drift register, the sealed-surface list.
**Worked example (FP-B16, compressed):** seed "quest reopening" → sweep: read the quest state machine's transition surface; confirm failure records persist discoverable conditions; check the `PlayerVisitedTrigger` precedent → plan: reopen transition with discovery-condition evaluation; EXISTING-SECTION save with an added marker field, default-tolerant → verify: fail→discover→reopen cycle tests, exactly-once reopening, mid-reopen round-trip, determinism replay.
**Failure modes:** creating a parallel authority (the cardinal sin); judging a method in isolation without its callers; authoring rolls outside the seeded streams; touching sealed surfaces without owner coordination; Core referencing engine types.
**No-change areas:** systems whose wiring the debt ledger records as sealed with a promotion condition the seed does not satisfy.

## 13.3 Lane C playbook — Economy and balance

**Contract:** evidence before tuning; harnesses are the instrument; results publish to `docs/balance/`; outliers become plans, not edits.
**Evidence inputs:** the live baseline documents (DR-03), the harness library (Volume 8), commodity/price catalogs, difficulty authority state.
**Worked example (H-C5, compressed):** seed "tribute sustainability" → sweep: doctrine list from the live catalog, mid-game income model definition from baselines → run: seeded 120-day slices per doctrine, tribute on schedule → publish: sustainability thresholds, death-spiral levels, reaction-cost table → route: any doctrine whose demand exceeds its threshold becomes a tuning plan candidate with the table cited.
**Failure modes:** aesthetic judgments dressed as balance findings; tuning without a harness run; confusing intended harshness with mathematical unsustainability (the lane's own distinction); carrying stale tables across content waves.
**No-change areas:** values whose harshness is documented as intended design (state the assumption, move on).

## 13.4 Lane D playbook — Save, state, and compatibility

**Contract:** the save contract is non-negotiable — DTOs, codecs, versions, migrations, checksums, atomic writes, slot isolation; old saves load or the plan fails.
**Evidence inputs:** the generated store matrix, the support-window tests, the sealed runtime's guard patterns, the drift register's codec pins.
**Worked example (FP-D01, compressed):** seed "exactly-once sweep" → sweep: inventory one-time effect classes with their persistence state → plan per gap: persist first result in the owning section, add the guard, mid-effect fixture → verify: the round-trip matrix (pre/mid/post), sealed-guard regression green, restore-resumes-not-drops.
**Failure modes:** silent schema changes without migration; defaulting that changes behavior; repairing corrupted envelopes; treating a codec bump as routine (it is a release-class act with support-window consequences).
**No-change areas:** sealed codecs whose migration behavior is recorded (DR-11's stamp is the precedent — its behavior is documented, so plans extend around it, not through it).

## 13.5 Lane E playbook — UI, UX, and accessibility

**Contract:** panels expose existing commands and truthful current state; zero gameplay authority; design and a11y contracts are pinned; controller parity is mandatory.
**Evidence inputs:** layout selftest output, snapshot coverage doc, ACCESSIBILITY.md, the input contract, the panel's owning session's state surface.
**Worked example (FP-E05, compressed):** seed "words-not-color-only" → sweep: enumerate panels with color-only signaling (audit script or manual pass) → plan: per-panel textual state additions through the existing components → verify: a11y gate, screen-reader order review, snapshots.
**Failure modes:** authority creep into panels (the invariant is the lane's own boundary); stale cache displays without an owner; a11y additions that violate the pinned design language; snapshots hand-edited.
**No-change areas:** panels already gate-clean and snapshot-covered — the sweep is discriminating, not exhaustive.

## 13.6 Lane F playbook — Performance

**Contract:** profile before rewrite; numbers in `docs/perf/`; no optimization without a demonstrated cost; correctness and readability outrank micro-gains.
**Evidence inputs:** the Performance Core family, the CI performance gate, the 15-FPS headless cadence, the atlas's flagged candidate classes.
**Worked example (FP-F02, compressed):** seed "storm-window tick concentration" → sweep: identify the co-firing systems and the instrumentation surface → run: window/non-window cost traces across 180–360 → publish: cost ratios and the noise floor → route: any demonstrated spike becomes a repair plan with the before numbers as its baseline.
**Failure modes:** optimizing on aesthetics; trading determinism for speed (never); measuring once and claiming a trend.
**No-change areas:** anything the trace shows flat — the lane records no-change areas as findings (the audit was run and found nothing), which is a result, not a failure.

## 13.7 Lane G playbook — Testing and verification

**Contract:** tests for meaningful regressions only — new public contracts, save/load, determinism, lifecycle, state transitions, cross-system consequences; focused runs under the policy caps; aggregation with per-row failure output.
**Evidence inputs:** the test policy, the existing suite's coverage of the target system, the sealed runtime's test patterns (the model suites).
**Worked example (FP-G04, compressed):** seed "permutation reachability" → sweep: enumerate the 32 permutations' input requirements from the matrix runtime → build: minimal-but-real seeded campaign fixtures per permutation → verify: suite green, two-pass replay per class, the hard-rule assertion (no optional content invalidates the main ending).
**Failure modes:** tests that cannot fail meaningfully; implementation-detail coupling; nondeterministic fixtures; padding counts.
**No-change areas:** trivial getters and already-covered paths.

## 13.8 Lane H playbook — Tooling and developer experience

**Contract:** a tool must solve a recurring demonstrated workflow problem and be smaller than the workflow it replaces; existing-implementation search first, always.
**Evidence inputs:** the scripts/ci family, the tools tree, the docs/debug surface, the recurring-problem evidence (drift-register entries are the queue).
**Worked example (FP-H05, compressed):** seed "sub-stream registry" → sweep: search for an existing registry (the existing-implementation-first rule) → plan: registry from a source-scan of stream derivations, append-only, integrity rule optional → verify: the scan reconciles with the registry; the rule demonstrated on a synthetic unregistered name.
**Failure modes:** building a tool more complicated than the workflow; duplicating an existing tool the session never searched for; tools that write into content authorities.
**No-change areas:** workflows that are demonstrably fine — the lane's finding is "no tool warranted," recorded with evidence.

## 13.9 Lane I playbook — Documentation and conventions

**Contract:** document what developers and agents need to operate safely; describe reality; never document an architecture that does not exist.
**Evidence inputs:** the docs map, the drift register (its entries are the documentation debt list), the generated docs' no-hand-edit rules.
**Worked example (FP-I01, compressed):** seed "authority-map gaps" → sweep: difference set between live docs directories and live maps → plan: fill in priority order, each map verified against source before writing → verify: index drift gate; foreman review.
**Failure modes:** aspirational architecture docs; hand-editing generated regions; documenting code that was read but not verified.
**No-change areas:** domains whose maps are current — the difference set is the scope, nothing else.

## 13.10 Lane J playbook — Onboarding and player experience

**Contract:** new systems become legible to players through onboarding seams, briefing rows, and manual checklists; difficulty text states observable consequences.
**Evidence inputs:** the onboarding selftest, the briefing surface, the loop canon, the difficulty authority state.
**Worked example (FP-J01, compressed):** seed "onboarding for new systems" → sweep: selftest output enumerates uncovered systems → plan: entries per system through the onboarding owner → verify: onboarding selftest green; no spoiler-class text in first-session surfaces.
**Failure modes:** tutorializing trivia; onboarding text that lies about behavior; difficulty text made of adjectives.
**No-change areas:** onboarding flows already covered by the selftest.

## 13.11 Cross-lane operating rules (for wave foremen)

1. Data-first lanes precede wiring lanes within a domain (A/C before B/D/E).
2. One lane per plan; at most one plan per lane per wave.
3. Never more than five concurrent plans touching shared seams.
4. Save work precedes the UI that displays its state.
5. A seed consumed by two lanes splits into two plans with an explicit dependency edge.
6. Every wave closes with: per-plan verification results, a census delta, a drift-register delta, and a growth-ledger line.

---

# VOLUME 14 — WAVE CHARTER TEMPLATES, WORKED WAVE EXAMPLES, AND THE EXECUTION RUNBOOK (Factory batch 2026-09-27-D)

## 14.1 Worked wave charter — Wave ASH-EXP-1 (the first recommended implementation wave)

The factory's flagship-recommended first wave, drafted here in full as the model for all subsequent charters. It is deliberately conservative: every member plan is data-first or test-only, no sealed surface is touched, and the two Core extensions in the candidate pool are held for the second wave.

```text
# Wave Charter — ASH-EXP-1: "Legibility First" (Factory-supplied charter, PROPOSAL)
## Domain and non-goals
Domain: player-facing legibility and premise-proofing for the next year of expansion.
Non-goals: no Core extensions, no save schema changes, no sealed-surface contact,
no balance tuning (measurement only).
## Lane allocation (one plan per lane, rotation respected)
- Lane A: FP-A01 + FP-A02 (shelter document tranches; one plan, two tranches)
- Lane D: FP-D07 (flag persistence audit — F-001's premise-proof)
- Lane G: FP-G01 (flag consumer suite — F-001's second premise-proof)
- Lane H: FP-H05 (sub-stream registry; existing-implementation search first)
- Lane J: FP-J01 (onboarding coverage wave)
- Lane C: H-C3 harness run (measurement only; results feed FP-B08)
## Flagship + satellites
Flagship: the Lane A tranche pair (largest authored volume, all data-first).
Satellites: the D/G premise-proof pair, the H registry, the J wave, the C harness run.
Concurrency: no shared seams among members (verify against WORKTREE_OWNERSHIP.md
before claiming; the A tranches and the J wave both touch content but disjoint catalogs).
## Verification matrix
- A tranches: integrity 0 findings; utilization proven; contract checks per entry.
- D/G pair: suite green under the focused cap; consumer count asserted.
- H registry: source-scan reconciliation; synthetic-violation demo if the integrity rule ships.
- J wave: onboarding selftest green.
- C harness: two-pass byte-identical; tables published with seeds and schema versions.
## Closeout discipline
Per-plan closeouts in docs/plans/; census delta (which unclaimed rows got consumers);
drift-register delta (anything that moved during the wave); growth-ledger lines;
changelog generated-region update; docs index sync.
## Done when
All six member verifications green; closeout written; census and drift deltas recorded;
and — the wave's specific purpose — F-001's two premise proofs land, upgrading the
flagship from HIGH CONFIDENCE premises to VERIFIED premises.
```

## 14.2 Worked wave charter — Wave ASH-EXP-2 (the consequence wave)

```text
# Wave Charter — ASH-EXP-2: "Consequences That Travel" (Factory-supplied, PROPOSAL; depends on ASH-EXP-1)
## Domain and non-goals
Domain: consequence propagation — choices, quests, and breakdowns acquiring long-horizon
and cross-system consequences.
Non-goals: no UI redesign; no economy tuning; no difficulty work (W1-gated items excluded).
## Lane allocation
- Lane B: FP-B16 (quest reopening) — Core extension, gated on ASH-EXP-1's D/G proofs
- Lane B: F-001 (delayed moral-choice callbacks) — flagship, data-first plus one wiring step
- Lane A: callback prose tranche (the F-001 prose fields, Part 9 contracts)
- Lane D: FP-D06 only if FP-B01 has exited quarantine; otherwise FP-D02 (lineage horizon)
- Lane G: the F-001 determinism and exactly-once suite (ships with the flagship)
## Flagship + satellites
Flagship: F-001 — the factory's highest-confidence, cheapest-large-yield plan.
Satellites: FP-B16 (its structural sibling), the prose tranche (its data half),
one Lane D member (horizon work), the flagship's own test suite.
Concurrency: F-001 and FP-B16 both touch the moral-choice/quest seam families —
they must not run concurrently within the wave; sequence flagship first.
## Verification matrix
F-001: dispatch-once on fresh and restored campaigns; two-pass replay; exactly-once
keyed on flag+day; integrity and utilization green. FP-B16: fail→discover→reopen
cycles; no weight_of_choices mutation of the original failure record; round-trip.
## Done when
A restored campaign produces callbacks exactly once in the authored window;
failed quests reopen on discovery; both premise-proof chains (from ASH-EXP-1)
are cited in the closeouts as the evidence base.
```

## 14.3 The execution runbook (for the owning implementer session)

The runbook converts a selected plan into implementation discipline. It is the factory's implementation-mode contract, aligned with the repository's own approval workflow.

1. **Re-read the plan and the live tree.** The plan's premise sheet has a date; anything that moved since is a premise break. Re-verify the four or five load-bearing premises, not the whole sheet.
2. **Claim ownership.** Register in `INTEGRATION_PLANS.md`; claim exact paths in `WORKTREE_OWNERSHIP.md`. No edits before both.
3. **Identify affected callers.** For any signature-bearing change, list every caller first (the call-site requirement). For data trches, list every loader and consumer.
4. **Identify existing tests.** Which focused suites cover the touched surface? They run before and after.
5. **Define expected behavior in one paragraph.** If it cannot be stated, the plan is not ready.
6. **Make the smallest coherent patch.** One plan, one patch series. Escalation ladders (local fix → helper → refactor) are climbed only on evidence.
7. **Verify.** The plan's named verification, in order: focused tests first (under the cap), selftests next, determinism proofs for anything stateful, regression suites for anything adjacent to a sealed surface.
8. **Inspect the diff.** Unrelated files unchanged; no generated regions hand-edited; no formatting noise.
9. **Close out.** Closeout doc, census delta, drift-register delta, ledger line, changelog generated-region update, docs index sync.
10. **Report honestly.** "Patch implemented; runtime verification remains outstanding" is the correct sentence when verification is incomplete. Never "fixed" without the proof.

## 14.4 Re-audit cadence and ledger discipline

- **Cadence:** a drift-register re-audit (Volume 9 pattern: live ledger read, debt-file promotion-condition read, listing deltas) before every wave charter is finalized, and after any activity gap longer than a week. A two-audit stability window (DR-13) licenses skipping only the ledger re-read, never the ownership check.
- **Ledger lines:** every volume appends its line (date, volume, type, chars, cumulative). Character counts are recorded for growth tracking only; they are never a quality signal (the anti-padding rule).
- **Seed hygiene:** consumed seeds are marked in place (plan id + wave); disproved seeds are marked CLOSED-with-evidence; new seeds from re-audits join Part IV with the DR entry that justifies them.

## 14.5 Factory outputs versus repository plan numbering

The repository's plan ledger uses its own numbering (185+ numbered plans; wave directories through wave12). The factory's F-/FP-/H- identifiers are subject-plan identifiers inside this document and must be mapped to repository plan ids at implementation time by the owning session — the closeout records the mapping. This prevents the factory from appearing to mint a parallel plan ledger, which would violate the one-ledger rule.

## 14.6 Standing anti-scope-creep review (run before publishing any wave)

1. Does any member plan touch a sealed surface? (If yes: owner coordination evidence attached, or the plan moves.)
2. Does any member plan satisfy a decision-blocked item's conditions without the signature? (If yes: to the DP queue.)
3. Do two member plans share a seam concurrently? (If yes: sequence them.)
4. Is any member plan larger than its evidence? (If yes: split or shrink; the smallest coherent change is the bar.)
5. Does every member plan name its verification command surface? (If no: it is not ready for the charter.)
---

# VOLUME 15 — REMAINING LANE A SEED EXPANSIONS (Factory batch 2026-09-24-E)

Nine Lane A seeds remain in compressed form (A-09, A-14, A-15, A-20, A-23, A-24, A-25, A-29, A-30). This volume expands each into the full FP-A subject-plan format, closing Lane A's compressed-seed backlog entirely: after this volume, every Lane A seed A-01 through A-30 exists either as a consumed flagship (A-28 into F-005), an expanded Volume 6 plan (A-01 through A-08, A-10 through A-13, A-16 through A-19, A-21, A-22, A-26, A-27), or a plan in this volume. All plans remain subject-level PROPOSALs: they commit no file changes. Standing constraints from Part 0.4 v1.0 apply throughout — no parallel authority, no new ids without collision sweep, prose reflects and never drives mechanics, owner-first extension.

## FP-A09 — Hydraulic Extrusion Assay Corpus Twin

Lane A · C4 · Status PROPOSAL.
Subject: ram-pressure and die-wear assay records for the hydraulic extrusion catalog, in the technical assay voice (contract 4.2), giving the live-but-unmapped extrusion domain (DR-04) its narrative twin.
Premise evidence: VERIFIED `hydraulic_extrusion_catalog.json` live (DM-4); VERIFIED the assay-log genre exists in the corpus with industrial precedents (`forge_charcoal_ash_assays`); DR-04 records the catalog as live but absent from the v1.0 inventory, which is structural evidence that no corpus twin was authored alongside it. Twin absence itself remains UNVERIFIED pending the corpus census check named in seed A-09.
Why this: DR-04's unmapped-domain list is the factory's canonical replenishment source, and assay twins are the established, lowest-risk genre for industrial catalogs (the Part 16.4 pattern). The extrusion host session (`HydraulicExtrusion`) exists, so utilization risk is low.
Must not change: extrusion process parameters; the catalog's mechanical fields; an assay may only record dispositions the host system can actually produce (service, reject, rework — verify the implemented disposition vocabulary in session).
Route: DATA-ONLY. Seams: `hydraulic_extrusion_catalog.json` (unchanged) → new corpus family in the narrative catalog family that owns assay logs → existing corpus loader → utilization through the established corpus consumption path. Save impact: NONE. Determinism: NONE.
Continuity: every assay must state one tolerance, one observed value, one disposition (contract 4.2); observed values must sit inside the catalog's plausible operating band; no real-world brands or alloy trade names.
Verification: integrity selftest for the new corpus family; utilization selftest; a focused pairing test against the extrusion catalog's process ids if the loader supports cross-referencing (Open premise 2).
Open premises: 1. Run the corpus census check confirming no extrusion assay family already exists (collision sweep). 2. Confirm which corpus catalog family owns industrial assay logs and whether its loader expects per-process keying.

## FP-A14 — Rail-Side Field Documents

Lane A · C5/C15 · Status PROPOSAL (premise INFERENCE pending corpus sweep).
Subject: track-walker notes and interlock violation reports for the rail family — four live rail catalogs (`rail grinding`, `rail logistics`, `railway interlock`, `rerailing`) with no narrative corpus family in the v1.0 Part 5.6 inventory.
Premise evidence: VERIFIED the four rail catalogs live (DM-5/DM-15 listing); VERIFIED `railway_interlock_catalog.json` under C15 defense/interlocks; the Part 5.6 narrative corpus inventory contains no rail track-walker family, but the inventory is truncated in the source upload, so the collision sweep named in seed A-14 must run before any authoring.
Why this: the rail family is the largest fully-live catalog group with zero prose coverage — the widest prose-per-system gap in Lane A once A-25's quest tranche is scheduled.
Must not change: interlock rule semantics; grinding and rerailing process parameters; no violation report may describe an interlock behavior the catalog cannot produce.
Route: DATA-ONLY. Seams: rail catalogs (unchanged) → new track-walker/interlock-report corpus family → corpus loader → utilization path. Save impact: NONE. Determinism: NONE.
Continuity: violation reports must cite interlock ids that exist in the catalog; track-walker notes must reference rail segments consistent with the logistics catalog's route table; the deadpan institutional voice of contract 4.8/4.7 applies.
Verification: integrity; utilization; a focused pairing test that every violation report cites a real interlock id.
Open premises: 1. Full corpus collision sweep (the Part 18 index was truncated in the source upload — verify in session). 2. Confirm which corpus family would own railway documents; if none is a natural fit, the plan proposes a new family and must then follow H-04's validator-extension discipline. 3. Determine whether the rail logistics catalog names segments or stations usable as prose anchors; if it is purely abstract, anchor to the interlock catalog instead.

## FP-A15 — Damaged-Zone Survey Marginalia

Lane A · C6 · Status PROPOSAL.
Subject: surveyor marginalia entries for `damaged_map_zones.json`, in the geodetic voice, each keyed to a damaged zone record.
Premise evidence: VERIFIED `damaged_map_zones.json` live (DM-6); VERIFIED the geodetic document voice exists (`GeodeticSurveyHostSession`, `geodetic_survey_catalog`); VERIFIED the gazetteer-entry and cartographic prose contracts (5.11, 5.23) define the register.
Why this: damaged zones are map-visible state with authored geodetic instrumentation already in place — marginalia is the cheapest texture that rides an existing host session with zero new seams.
Must not change: zone damage classifications or map gating; marginalia may only annotate, never reinterpret, the zone's mechanical state; the orphan gate `AllMapNodes_ExistInLocationsCatalog` is untouched (no new map nodes are authored).
Route: DATA-ONLY. Seams: `damaged_map_zones.json` (unchanged) → marginalia corpus family or annotations within the existing geodetic corpus family (Open premise 1) → loader → utilization. Save impact: NONE. Determinism: NONE.
Continuity: one landmark, one sensory anchor, one danger indication per entry, per the 5.23 map-description contract; marginalia must not promise enterable content the map cannot deliver.
Verification: integrity; utilization; pairing test that marginalia keys match live damaged-zone ids.
Open premises: 1. Confirm whether the geodetic corpus family supports per-zone annotation records or whether a new family is required. 2. Enumerate the damaged-zone record count in session to size the tranche.

## FP-A20 — Radio Program Rundown Expansion

Lane A · C8 · Status PROPOSAL, SEALED-adjacent.
Subject: additional rundown batches for stations with thin programming against `radio_programs.json` and `radio_stations.json`.
Premise evidence: VERIFIED both catalogs live (DM-8); VERIFIED the radio-transcript genre exists in the corpus (contract 4.4) and program production is a live host session (`RadioProgramProduction`).
Why this: rundown prose is the radio domain's established texture genre, and program-to-prose coverage is measurable directly from the two catalogs in session.
Must not change: the distress-signal content seal (`CF-P1-DISTRESS-CONTENT-SEAL`) — rundowns must not add, imply, or foreshadow new signal scenarios; the genuine-never-hostile invariant; the retired availability consumer stays retired.
Route: DATA-ONLY. Seams: `radio_programs.json` / `radio_stations.json` (unchanged) → rundown corpus family (existing) → loader → program production consumption path. Save impact: NONE. Determinism: NONE.
Continuity: rundowns must only schedule programs that exist in the catalog; station voice must match any existing station identity fields; no signal-content spillover of any kind.
Verification: integrity; utilization; a focused test that no rundown entry introduces signal-scenario vocabulary (the seal guard).
Open premises: 1. Measure per-station program counts to identify the genuinely thin stations. 2. Confirm the rundown corpus family's existing schema and consumption path in session.

## FP-A23 — Intake Interview Continuation

Lane A · C9 · Status PROPOSAL.
Subject: new-arrival intake interview entries conditioned on the arrival channels that exist (rescue, crossing, holdfast).
Premise evidence: VERIFIED `new_arrival_intake_interviews` in the corpus; VERIFIED arrival channels are canon (rescue runtime sealed, crossing and holdfast faction catalogs live); VERIFIED the intake-interview contract (4.6) and its people-and-psyche register.
Why this: interviews are the survivors domain's cheapest legibility genre, and the sealed rescue runtime plus two live channel catalogs give three conditioning axes that already exist mechanically.
Must not change: rescue-runtime semantics (sealed — additive only per DR-06 handoff rules); interview entries may only describe arrivals through implemented channels; the `knowledge`-legality discipline of contract 4.6 (arrivals cannot know what information flow forbids).
Route: DATA-ONLY. Seams: channel catalogs and sealed rescue state (read-only conditioning) → interview corpus family (existing) → loader → utilization. Save impact: NONE. Determinism: NONE (if conditioning on channel state requires loader support, that becomes a Lane B seed — Open premise 2).
Continuity: interviewees cannot reference facts outside their arrival channel's information horizon; no invented survivor ids — entries either reuse catalog survivor ids or remain anonymized records.
Verification: integrity; utilization; duplication check against existing interview batches (each new entry must cover a channel/condition combination prior batches lacked).
Open premises: 1. Enumerate existing interview batches and their covered channels. 2. Confirm whether the corpus loader supports channel-conditioned selection; if not, author a static batch covering all three channels unconditionally.

## FP-A24 — Bureaucratic-Morality Quest Prose Completion

Lane A · C10 · Status PROPOSAL.
Subject: prose-field completion across `quests_bureaucratic_morality.json` records with skeleton `quest_hook` and outcome texts, following the quest prose contracts exactly (5.21).
Premise evidence: VERIFIED catalog live (DM-10); VERIFIED the five-field quest prose contract and the quest record contract (7.2); the bureaucratic-morality domain is documented canon with the court-verdict register (4.3) as its natural voice.
Why this: skeleton prose on live, dispatchable quest records is the highest player-facing prose debt per record, and the bureaucratic domain has a uniquely well-specified register already contracted.
Must not change: quest structure, availability conditions, connections, branch effects — only prose fields; outcomes must follow the reward/failure/recovery grammar of v1.0 Part 6.7 without altering branch mechanics.
Route: DATA-ONLY. Seams: `quests_bureaucratic_morality.json` (prose fields only) → existing quest loaders → questline consumption. Save impact: NONE. Determinism: NONE.
Continuity: connections referenced in prose must be by the record's own id set; verdict-flavored language must stay inside the bureaucratic register; D-07's persistence audit is the natural pairing (every prose completion ride on a flag that already persists).
Verification: integrity; quest-template validation; a focused prose-coverage test asserting no empty prose fields remain in the catalog after the tranche.
Open premises: 1. Session census: which records currently have skeleton prose. 2. Confirm the exact prose field names in this catalog's schema (verify, do not assume Part 9 field names verbatim).

## FP-A25 — Massive-Expansion Quest Prose Audit and Tranche Program

Lane A · C10 · Status PROPOSAL, multi-tranche.
Subject: a structured prose-depth audit of `quests_massive_expansion_200.json` (200 records — the largest single prose debt surface in the data authority), converting skeleton records into contracted fields over a defined tranche program.
Premise evidence: VERIFIED catalog live (DM-10); VERIFIED record count is 200 (structural scale evidence); the v1.0 audit and DR-08 census both treat per-record prose depth as the catalog's known thin side; the quest record contract (7.2) and quest prose contract (5.21) define the target state.
Why this: 200 dispatchable quest records is the single largest prose multiplier in the repository; the tranche program below makes it consumable without a mega-wave.

### A-25 tranche structure (the 200-record program)

The audit tranche program is designed around one principle: a tranche is small enough to verify in one session and large enough to justify its loader test. Proposed structure, subject to the session census:

- Tranche 0 — Census (no authoring): read the catalog, score each record's prose depth against the 7.2 contract (empty / skeleton / complete per prose field), sort records by questline family, and publish the tranche map in the plan document. Census output determines tranche boundaries; the factory does not author against assumed gaps.
- Tranches 1–8 — Authoring waves of approximately 25 records each, grouped by questline family so each tranche shares one voice register and one reviewer context. Each tranche: (a) completes only prose fields; (b) runs the full 7.9 integrity checklist; (c) runs a focused prose-coverage test scoped to that tranche's record ids; (d) records closeout in the plan doc with per-record status.
- Tranche 9 — Completion and regression: catalog-wide prose-coverage test (zero empty contracted fields), duplication sweep against other quest catalogs (the moral-branching expansion and repeatable families share registers), and a G-01-style consumer check that every authored hook has a dispatchable path.
- Sequencing rule: no more than one tranche per wave charter, and never two quest-catalog tranches in the same wave (they share the quest loader seam).
- Acceptance per tranche: integrity green, prose-coverage test green for the tranche's ids, closeout doc written, cumulative count of completed records published.

Must not change: quest mechanics, branches, availability, connections — prose fields only, exactly as FP-A24; records are never re-id'd or re-ordered during prose completion.
Route: DATA-ONLY per tranche. Seams: `quests_massive_expansion_200.json` (prose fields only) → quest loaders → questline dispatch. Save impact: NONE. Determinism: NONE.
Continuity: the 5.21 register discipline; hooks must not promise mechanics beyond the record's own objectives; family-grouped tranches keep voice consistent.
Verification: per-tranche focused tests plus the Tranche 9 catalog-wide suite; integration with D-07's flag persistence audit (each completed record's flags must already round-trip).
Open premises: 1. The Tranche 0 census (everything downstream is census-gated). 2. Confirm the catalog's prose field names in session. 3. Confirm whether any records are intentionally skeleton (e.g., reserved templates) — such records are documented, not "completed."

## FP-A29 — Bestiary Natural-History Continuation

Lane A · C14 · Status PROPOSAL.
Subject: sighting-log and specimen-record prose for bestiary entries with thin coverage, in the natural-history register.
Premise evidence: VERIFIED `wasteland_wildlife_bestiary.json` live (DM-14); VERIFIED the sighting-log and specimen-record genres exist in the corpus (vulture-sighting logs, cockroach-hive records); the ecological calendar is canon, giving seasonal conditioning anchors that already exist.
Why this: the bestiary is a live catalog with proven prose genres and a seasonal system to key against — texture with zero new seams and measurable per-entry coverage.
Must not change: creature behaviors, ecosystem roles, or trapping/migration parameters; prose must not describe behaviors the ecosystem system does not model; the zoonosis bridge is the owned disease seam and is not extended by prose.
Route: DATA-ONLY. Seams: `wasteland_wildlife_bestiary.json` (prose fields or corpus twins, per Open premise 1) → loader → utilization. Save impact: NONE. Determinism: NONE.
Continuity: seasonal references must fall inside the ecological calendar's windows; sighting locations must be plausible against the map without inventing new map nodes.
Verification: integrity; utilization; per-entry coverage test against the bestiary catalog.
Open premises: 1. Confirm whether bestiary prose lives in the catalog's own fields or in a separate corpus family (field-inventory protocol, 7.1). 2. Census thin entries in session.

## FP-A30 — Sky-Defense Ordnance Manifest Prose

Lane A · C15 · Status PROPOSAL (premise INFERENCE pending corpus sweep).
Subject: armor-layer inspection records and ordnance manifests for the sky-defense family, in the manifest and inspection registers (contracts 4.1, 5.19).
Premise evidence: VERIFIED `sky_defense_ordnance.json` and `sky_layer_armor_catalog.json` live (DM-15); VERIFIED `SkyDefense` host session exists; UNVERIFIED whether a sky-defense document family exists in the narrative corpus — seed A-30's collision sweep is mandatory before authoring.
Why this: the sky-defense family has two live catalogs and a host session with no established prose genre; manifests are the lowest-risk entry genre for a military-logistics domain.
Must not change: ordnance and armor mechanical values; telemetry thresholds; prose must not describe engagement outcomes the `OrbitalHarrowTelemetrySystem` cannot produce; the partially built sky-armor-to-weather bridge is not extended by prose (verify before extending, per DM-15's constraint).
Route: DATA-ONLY. Seams: sky-defense catalogs (unchanged) → manifest/inspection corpus family → loader → utilization. Save impact: NONE. Determinism: NONE.
Continuity: manifest quantities must agree with the ordnance catalog's records (contract 4.1's ledger-agreement rule); inspection records must reference armor layers that exist in the armor catalog.
Verification: integrity; utilization; pairing test against both sky-defense catalogs; the corpus collision sweep result documented in the plan.
Open premises: 1. Collision sweep for an existing sky-defense document family. 2. Census which armor layers and ordnance classes lack inspection/manifest coverage.

## Volume 15 sequencing note

Recommended consumption order, if a wave charter absorbs these plans: FP-A24 and FP-A25's Tranche 0 first (C10, shared census tooling), then FP-A15 and FP-A29 (both are census-plus-pairing plans with identical verification shape), then FP-A09/FP-A14/FP-A30 (the three collision-sweep-gated industrial families — their open premises resolve identically), then FP-A20 and FP-A23 (sealed-adjacent and channel-conditioned, each needing one extra guard). No two plans in this volume share a loader seam except FP-A24/FP-A25 (both C10, hence the one-quest-tranche-per-wave rule).

---

# VOLUME 16 — WORKED CONTENT TRANSCHE LIBRARY (Factory batch 2026-09-24-F)

This volume supplies worked tranche models for the prose genres that Volumes 4, 5, and 15 established. Every record below is a PROPOSAL MODEL, not canon: record ids are illustrative placeholders that must be replaced by the session's collision-swept id scheme, mechanical values must be replaced with values read from the live catalog in session, and every model must pass the 7.9 catalog-agnostic integrity checklist before any tranche it shapes is authored. The models exist to fix register, field shape, and validation expectations so that authoring sessions do not have to re-derive them. No model invents a mechanic: each one references only systems, catalogs, and genres verified in the 2026-09-24 audit or the factory's own drift register.

## 16.1 Model tranche — maintenance glitch report (FP-A01 pattern, contract 5.14)

```json
{
  "id": "glitch_report_mg014",
  "status": "PROPOSAL MODEL",
  "room_id": "SHELTER_ROOM_ID_FROM_CATALOG",
  "machine_id": "SHELTER_MACHINE_IDENTITY_FROM_CATALOG",
  "symptom_text": "Feed selector sticks between positions; operator must count two clicks where one should serve.",
  "cause_text": "Carbon film on the contact plate; consistent with the EMP-conditioned feed's relay seating, per the Wave 8–12 power retrofit.",
  "fix_text": "Dressed with a whetstone salvaged from the kitchen drawer; two hundred cycles confirmed the engagement.",
  "plausibility_note": "Symptom must be physically consistent with the named machine's function; no glitch may imply a mechanic the shelter systems do not implement."
}
```

Validation notes: `room_id` and `machine_id` must both resolve against live catalogs (focused pairing test); the referenced retrofit must be an implemented behavior, not an aspirational one; one observed defect, one disposition, one verification per record, per contract 5.14.

## 16.2 Model tranche — load-shed amendment (FP-A02 pattern, contract 4.8)

```json
{
  "id": "load_shed_amendment_001c",
  "status": "PROPOSAL MODEL",
  "amends": "load_shed_schedule_001",
  "condition": "sanitation_room_power_state == POWERED",
  "notice_text": "Schedule 001, Revision C: corridors on the sanitation feed hold light through the second watch when the sanitation rooms draw power. Water windows follow room state, not the clock. Holders of the previous revision may continue to trip over the dark; the dark has been rebooked.",
  "prose_only": true
}
```

Validation notes: the amendment must not contradict observable power behavior (Invariant 5 — prose reflects, never drives); conditioning fields are read-only projections of implemented room power state; the deadpan institutional voice of 4.8 is mandatory; humor stays in register.

## 16.3 Model tranche — hydraulic extrusion assay log (FP-A09 pattern, contract 4.2)

```json
{
  "id": "extrusion_assay_b07",
  "status": "PROPOSAL MODEL",
  "process_id": "HYDRAULIC_EXTRUSION_PROCESS_ID_FROM_CATALOG",
  "batch_note": "Billet 7, second pressing. Ram pressure held the table value through the full stroke; die showed wear land past the sixth pass, measured at the shoulder, not the throat.",
  "tolerance": "TABLE_VALUE_FROM_CATALOG",
  "observed": "OBSERVED_VALUE_WITHIN_PLAUSIBLE_BAND",
  "disposition": "DISPOSITION_VOCAB_IMPLEMENTED_BY_HOST",
  "prose_note": "One tolerance, one observed value, one disposition; the ledger takes the number."
}
```

Validation notes: observed values must sit inside the catalog's plausible operating band; disposition must come from the host's implemented vocabulary (Open premise 2 of FP-A09); no emotional language, per 4.2.

## 16.4 Model tranche — interlock violation report (FP-A14 pattern, contract 4.7)

```json
{
  "id": "interlock_violation_r03",
  "status": "PROPOSAL MODEL",
  "interlock_id": "INTERLOCK_ID_FROM_RAILWAY_INTERLOCK_CATALOG",
  "segment": "SEGMENT_REF_FROM_LOGISTICS_CATALOG_OR_ANNOTATED_ABSENT",
  "report_text": "Walker found the gate arm reset against protocol, held up by a wedge cut from fence post. Wedge removed, arm re-seated, cycle count taken at both ends. No traffic observed; no traffic expected, which is itself the expected condition of a railway in the Year of Ash.",
  "rule_reference": "RULE_ID_FROM_CATALOG"
}
```

Validation notes: every violation cites a real interlock id; no violation may describe a rule the catalog does not encode; the operations register of 4.7 applies. This model is collision-sweep-gated (FP-A14 Open premise 1).

## 16.5 Model tranche — damaged-zone survey marginalia (FP-A15 pattern, contracts 5.11/5.23)

```json
{
  "id": "zone_marginalia_dz11",
  "status": "PROPOSAL MODEL",
  "zone_id": "DAMAGED_MAP_ZONE_ID_FROM_CATALOG",
  "marginalia": "Station 11 re-occupied for one hour. Landmark: the fallen water tower, half a grid square south. Sensory: the ash lies in drifts against the tower's concave side, deeper than the published depth. Danger: the drift masks a stairwell mouth; recommend flagging before any survey team descends. Correction entered against the zone record; the record was wrong about the depth and is now wrong about less.",
  "anchors": ["one landmark", "one sensory anchor", "one danger indication"]
}
```

Validation notes: no new map nodes (the orphan gate is untouched); marginalia annotates the zone's existing mechanical state only; the geodetic voice is first-person-removed, corrective, and slightly dry.

## 16.6 Model tranche — radio rundown entry (FP-A20 pattern, contract 4.4, SEALED-adjacent)

```json
{
  "id": "rundown_entry_pg22",
  "status": "PROPOSAL MODEL",
  "station_id": "RADIO_STATION_ID_FROM_CATALOG",
  "program_id": "RADIO_PROGRAM_ID_FROM_CATALOG",
  "rundown_text": "Evening block, as read: the market grain numbers, twice for the slow; the lost-and-found, once for the hopeful; the serial, for everyone. Station identification at the half hour, as regulation and habit both require.",
  "seal_guard": "NO_SIGNAL_SCENARIO_VOCABULARY"
}
```

Validation notes: programs scheduled must exist in the catalog; the seal guard test (FP-A20) fails any entry that adds or implies new distress-signal scenarios; the genuine-never-hostile invariant is untouched by rundowns but the focused test should assert the vocabulary sweep regardless.

## 16.7 Model tranche — intake interview (FP-A23 pattern, contract 4.6)

```json
{
  "id": "intake_interview_i09",
  "status": "PROPOSAL MODEL",
  "channel": "crossing",
  "subject": "ANONYMIZED_OR_EXISTING_SURVIVOR_ID",
  "transcript_excerpt": "Came through the crossing in the third week of the window. Asked what the towers were; answered truthfully, which took less time than the question deserved. Knows the crossing procedures, suspects nothing about the inland camps, cannot know yet what the muster asked of the others.",
  "knowledge_legality": {
    "knows": ["crossing procedures"],
    "suspects": [],
    "does_not_know": ["inland camp conditions"],
    "cannot_know_yet": ["muster demands"]
  }
}
```

Validation notes: the knowledge block is binding — nothing in the transcript may exceed it; channel must be one of the implemented arrival channels; anonymized subjects never receive invented survivor ids.

## 16.8 Model tranche — bureaucratic-morality quest prose completion (FP-A24 pattern, contract 5.21)

```json
{
  "id": "QUEST_ID_FROM_BUREAUCRATIC_MORALITY_CATALOG",
  "status": "PROPOSAL MODEL — PROSE FIELDS ONLY",
  "quest_hook": "The requisition arrived countersigned by a dead office, and the countersignature is the only part of it that is in order.",
  "objective_text": "Recover the office seal from the evacuated annex before the next audit cycle cites the shelter for impersonating a functioning administration.",
  "success_text": "The seal is returned to the living clerk, who stamps three copies, files two, and burns the third in what the manual calls the original disposition.",
  "failure_text": "The audit proceeds without the seal; the shelter is assessed a fine it can pay in grain, in labor, or in patience, and the clerk notes which was chosen.",
  "recovery_text": "The annex remains on the survey list; a later expedition may yet recover what the audit cycle could not wait for."
}
```

Validation notes: prose fields only — structure, connections, availability, and branch effects are untouched; success/failure/recovery follows the Part 6.7 grammar without altering branch mechanics; the court-verdict register (4.3) supplies tone; hook text must not promise mechanics beyond the record's own objectives.

## 16.9 Model tranche — massive-expansion record completion (FP-A25, Tranche N pattern)

Same field shape as 16.8, with two additions required by the tranche program: `"tranche": "N"` (the tranche that completed the record, for the Tranche 9 regression sweep) and `"prose_status": "complete"` asserted by the catalog-wide coverage test. Tranche boundaries and census-gating follow the Volume 15 A-25 program exactly; no record is authored outside its tranche's questline-family grouping.

## 16.10 Model tranche — bestiary sighting log (FP-A29 pattern, natural-history register)

```json
{
  "id": "sighting_log_sl31",
  "status": "PROPOSAL MODEL",
  "creature_id": "BESTIARY_ENTRY_ID_FROM_CATALOG",
  "season_window": "SEASON_FROM_ECOLOGICAL_CALENDAR",
  "log_text": "Second sighting this season, both at the drying racks. The bird tolerates the fence at twenty paces and no closer; the fence has opinions on the matter and so does the meat. Specimen condition: adult, missing the left secondaries, consistent with the molt the calendar records for this window."
}
```

Validation notes: creature and season must both resolve against live catalogs; behaviors described must be within the ecosystem system's modeled repertoire; sighting locations must not invent map nodes.

## 16.11 Model tranche — sky-defense ordnance manifest (FP-A30 pattern, contract 4.1)

```json
{
  "id": "ordnance_manifest_om04",
  "status": "PROPOSAL MODEL",
  "layer_id": "SKY_LAYER_ARMOR_ID_FROM_CATALOG",
  "manifest_lines": [
    "Armor panel, layer per catalog, 4 — one with splice corrosion at the flange, noted, serviceable",
    "Ordnance charge, class per catalog, 12 — seals intact, lot numbers within the recorded series"
  ],
  "inspection_note": "Layer inspection at the quarterly window. The flange corrosion is documented, not repaired; the repair queue is a decision the manifest does not make.",
  "receiving_marks": "Countersigned at the bunker mouth, unread, as is customary."
}
```

Validation notes: quantities must agree with the ordnance catalog's records (the 4.1 ledger-agreement rule); engagement outcomes are never described — this is a logistics document, not an engagement report; the partially built sky-armor-to-weather bridge is not extended by prose (DM-15 constraint).

## 16.12 Model tranche — almanac entry (A-27 pattern, contract 5.10)

```json
{
  "id": "almanac_entry_ws08",
  "status": "PROPOSAL MODEL",
  "storm_window_ref": "YEAR_OF_ASH_STORM_WINDOW_ID_FROM_CATALOG",
  "entry_text": "The eighth window of the Year of Ash. Old rules for the ash: watch the barometer twice, trust it once. The windows before this one took the roof of the south drying shed and the temper of the same shed's keeper; both are recorded elsewhere, and both are back in service."
}
```

Validation notes: window references must resolve against `year_of_ash_storm_windows.json`; entries stay inside the 180–360 canon window's vocabulary; consequences mentioned must be system-producible states.

## 16.13 Tranche library rules

1. Every model is PROPOSAL status until a consuming session replaces illustrative values with live-catalog reads and passes 7.9's checklist.
2. A model may not add fields beyond its genre contract; if a genre needs a new field, that is a Part 9 field-contract change with a named consuming loader, never a tranche-level improvisation.
3. Quantities, tolerances, ids, and windows always come from the owning catalog; the prose layer owns register, never numbers' authority (Invariant 5).
4. One model genre per tranche keeps the reviewer's register fixed; mixing genres inside a tranche is a defect.
5. The seal guard (16.6), the knowledge-legality block (16.7), and the ledger-agreement rule (16.11) are the three mandatory focused tests for their respective genres; a tranche that ships without its genre's mandatory test is not accepted.

---

# VOLUME 17 — CLUSTER-BY-CLUSTER EXPANSION ROADMAPS (Factory batch 2026-09-24-G)

One roadmap per cluster C1–C17, each anchored to its deep map (DM-1 through DM-17) and its seeded openings. Each roadmap is a planning instrument, not a schedule: it states what the cluster already owns (verified), what the factory has already proposed (by plan id), and the phase structure a multi-year content arc would follow if the foreman authorizes sustained expansion. Phase labels are consistent across all clusters:

- Phase I — Seed consumption: complete the cluster's expanded subject plans as wave tranches (the plan lists named above).
- Phase II — Cross-cluster bridges: consume the cluster's Lane B/C PROPOSAL seeds whose premises verified during Phase I.
- Phase III — Depth and replenishment: census-driven prose/economy depth on the surfaces Phases I and II touched; premise sweep for new seeds from census output and drift.
- Phase IV — Re-audit: cluster entry in the periodic drift-refresh (the Part VI protocol's audit-volume rotation); DR entries for anything that moved.

No phase authorizes opening a sealed surface or a GATE item; those enter only through the Volume 12 decision packets and their named signatures.

## 17.1 C1 — Shelter operations (DM-1)
Owns: rooms, identities, machines, thermal, schedules, social events, decor, fire, noise, airlock, decon, atmosphere, sanitation (power-fed). Expanded plans: FP-A01, FP-A02; satellites B-01, B-02, D-06, F-04. Phase I: glitch batch 4 and load-shed amendments, census-gated. Phase II: shelter-failure cascade exit from quarantine (B-01, reading its exit criteria first) and the grid-seal consumer sweep (B-02). Phase III: schedule and social-event prose depth on rooms that gained Wave 8–12 systems; B-24 panel-truth sweep. Phase IV: DR entry if quarantine exit changes the failure-effects surface. Multi-year arc: shelter as the legible home base — every powered room with a document voice, every failure with a readable consequence.

## 17.2 C2 — Medical pipeline (DM-2)
Owns: disease, pathogens, dose ledger, ARS, surgery, autopsy, pharma, diagnostics, therapies, dependency, crises. Expanded plans: FP-A03, FP-A04, FP-A05; satellites B-03, B-04, B-25, G-03. Phase I: casebook, therapy-note twins, therapist batch 4. Phase II: fallout-window dose coupling (B-03, premise verification first) and the child-health cohort bridge (B-04, via 19B's scoped links). Phase III: dose-location and register prose depth; G-03 pairing suite extended to every new matrix row. Phase IV: re-audit whenever the medical matrix document regenerates. Multi-year arc: the medical pipeline as the game's most documented and most narrated system, prose-to-mechanics parity preserved by pairing tests.

## 17.3 C3 — Water, food, agriculture (DM-3)
Owns: water treatment, condensers, deep wells, brine, nutrition, kitchen, preservation, grain, greenhouse, aquaponics, aeroponics, apiculture, cryo cultivars. Expanded plans: FP-A06, FP-A07, FP-A08; satellites B-05, C-12, F-012. Phase I: preservation/grain assay twins, cellar and silo follow-ons, apiculture continuation. Phase II: preservation-contamination bridge (B-05, zoonosis model) and the F-012 dive-site/hydroponic audit's follow-on tranches. Phase III: greenhouse/aeroponics economics harness (C-12); seasonal-calendar prose depth across all cultivation families. Phase IV: DR entry if F-012's audit finds the dive/hydroponic domains already partly mapped. Multi-year arc: the food chain legible end to end, from cultivar to kitchen, with assay prose as its memory.

## 17.4 C4 — Power and industry (DM-4)
Owns: grid, SOFC, solar, kinetic, geothermal, foundry, CVD diamond, EB/PVD, optics, powder metallurgy, pyrolysis, Fischer-Tropsch, chlor-alkali, acids, fermentation, ethanol, air separation, metrology, extrusion. Expanded plans: FP-A09 (this factory), FP-A11 (foundry pour windows), plus the metrology corpus; satellites B-06, C-01, C-02. Phase I: extrusion assay twins (collision-sweep-gated), metrology calibration corpus. Phase II: difficulty-scalar consumer binding after the W1 seal (B-06, never parallel scalars). Phase III: chain income-versus-expenditure harness (C-01) and SOFC sustainability (C-02), both reporting into docs/balance; dominated processes get tuning or scarcity framing, per harness evidence. Phase IV: DR entry at the W1 seal (the difficulty authority's cluster entry changes). Multi-year arc: the industrial frontier as an authored economy — every process with a prose twin, every chain with a sustainability report.

## 17.5 C5 — Expeditions and travel (DM-5)
Owns: expeditions, vehicles, dispatch preflight, scavenging tables, waystations, caravans, travel encounters, micro-locations, anomalous encounters. Expanded plans: FP-A12, FP-A13, FP-A14 (this factory), FP-E07 depth; satellites B-07, B-08, C-03, C-04. Phase I: destination arrival/revisit prose completion (Part 9 field contracts exactly), waystation registers, rail-side field documents (sweep-gated). Phase II: scavenging-table parity for uncovered destinations (B-08, 49-versus-53); vehicle-breakdown consequence routing (B-07, verify current routing first). Phase III: C-03 seeded E[value] re-run and C-04 dominance re-evaluation after any vehicle wave. Phase IV: DR entry if the 53-destination surface grows (new destinations reset B-08's parity math). Multi-year arc: every destination worth traveling to and worth reading about — prose coverage and loot coverage tracked as one surface.

## 17.6 C6 — Map and geography (DM-6)
Owns: wasteland map system/loader, damaged zones, fog, route gates, survey instruments. Expanded plans: FP-A15 (this factory); satellite B-09 (GATE — flooded-route topology tags, decision packet DP-01). Phase I: damaged-zone marginalia. Phase II: DP-01's implementation if signed — authored map edges with flood tags consumable by route gates; the orphan gate governs all authoring. Phase III: survey-instrument prose depth across the four geodetic catalogs; census of zone marginalia coverage. Phase IV: DR entry if DP-01's signature changes the map-edge surface (the largest single structural change this cluster can receive). Multi-year arc: the map as the game's most trustworthy narrator — every zone annotated, every route's dangers legible before departure.

## 17.7 C7 — Factions and war (DM-7)
Owns: stance engine, doctrines, war chain, tributes, treaties, embargoes, espionage, psyops, counter-intelligence, musters, labor camps, bounty board. Expanded plans: FP-A16, FP-A17, FP-A18; flagship F-004; satellites B-10 (GATE, DP-02), B-11 (GATE, DP-03), B-14, C-05, C-06, G-05. Phase I: standing-record testimony, verdict radio continuation, warlord doctrine communiqués. Phase II: on signatures — DP-02 per-strike emitters, DP-03 black-market funds legs; B-14 belief-movement stance bridge (stance engine is the sole standing authority). Phase III: tribute sustainability (C-05) and embargo legibility (C-06) harnesses; G-05 offset-mapping tests extended to any new war-chain consumer. Phase IV: DR entry per signed GATE item (each signature moves this cluster's boundary). Multi-year arc: war conducted in paper — every faction's public and private voices documented, every consequence traceable through the standing engine.

## 17.8 C8 — Radio and information (DM-8)
Owns: radio system, stations, programs, intercepts, distress signals (sealed runtime), rumors, sound ranging, direction finding, NVIS, heliograph. Expanded plans: FP-A19, FP-A20 (this factory, SEALED-adjacent); flagship F-003 adjacency; satellites B-12, B-13, B-25, E-04, F-05, G-06. Phase I: cipher continuation with solvable kernels; rundown expansion behind the seal guard. Phase II: rumor-band commodity extension (B-12, deterministic bands); intercept-driven journal depth (B-13, on the sealed authenticity seam, additive only). Phase III: F-05 dial profiling; G-06 exactly-once regression suite maintenance on every new intercept class. Phase IV: DR entry whenever a signature reopens any sealed radio surface — this cluster has the repository's most signatures for a reason. Multi-year arc: information itself as a survival resource — each channel's texture distinct, its truth rules invariant.

## 17.9 C9 — Survivors and interiority (DM-9)
Owns: needs, health, skills, traits, arcs, trauma, therapies, guilt, crises, morale contagion, relations, caregiving, dependency, companion animals, beliefs, rituals, rites, final wishes, belongings, memory, phantom memory, lineage, cohorts, apprenticeships. Expanded plans: FP-A21, FP-A22, FP-A23 (this factory); satellites B-04, B-14, B-15, D-02. Phase I: heirloom-trigger expansion keyed to surviving cohorts; final-wishes document twins; intake continuation. Phase II: B-15 rite evidence enrollment (vocabulary check first); D-02 lineage horizon extension toward Day 3650. Phase III: guilt-source and confession prose depth census; cohort survival state as a conditioning axis for any new document genre. Phase IV: DR entry if the ClaimPersonalBelonging no-caller finding resolves (re-verify before extending, per DM-9's caution). Multi-year arc: the shelter as a community of records — every survivor's interiority documentable, every loss leavable behind as a paper trace.

## 17.10 C10 — Quests and moral choice (DM-10)
Owns: questline master, dynamic and personal quests, NPC arcs, moral-choice chains/flags/gossip/quests (five split catalogs), faction branching, bureaucratic morality, massive expansion corpus, repeatable quests, templates, domain questlines. Expanded plans: FP-A24, FP-A25 (this factory); flagship F-001; satellites B-16, B-17, D-07, G-01. Phase I: bureaucratic prose completion; A-25 Tranche 0 census, then the eight-tranche program at one tranche per wave. Phase II: B-16 quest reopening after late discoveries; B-17 gossip propagation depth. Phase III: G-01 flag-consumer coverage extended to every prose-completed record; D-07 persistence audit as the tranche program's standing companion. Phase IV: DR entry at each tranche-program boundary (200 records is a multi-quarter surface). Multi-year arc: the quest corpus as the game's largest authored library — structure live first, prose depth following at a disciplined tranche cadence.

## 17.11 C11 — Economy (DM-11)
Owns: market, price factors, shocks, baselines, regional prices, hardcore tuning, rumor bands, black market, caravans, debt ledger, foundry economy, bounty board, trade screens. Expanded plans: FP-A26; satellites B-18, C-07, C-08, C-13, E-08, G-02; GATE: funds legs (DP-03). Phase I: ledger-debt statement prose (debtor statements, collection notices). Phase II: on DP-03's signature, the canonical funds authority; B-18 trade-screen scenario parity. Phase III: C-07 debt-runaway analysis, C-08 black-market tier audit, C-13 quantity-band trims where E[value] outliers persist (evidence-gated). Phase IV: DR entry per harness publication (balance reports are the cluster's drift surface). Multi-year arc: an economy whose documents and ledgers tell one story — debt as prose, prices as legible signals, fairness audited on a published cadence.

## 17.12 C12 — Weather and Year of Ash (DM-12)
Owns: weather system, seasons, effects, gates, hardening, storm windows, the Year-of-Ash family (events/items/locations/questlines/quests/radio/survivors). Expanded plans: FP-A27; flagship F-002; satellites B-03, B-19, C-09, E-09, F-02, G-07. Phase I: storm-window almanac entries inside the 180–360 canon. Phase II: B-19 winter pressure for power and water systems (storm-window-conditioned costs). Phase III: C-09 winter compression sustainability harness; F-02 tick-concentration profiling across window/non-window days. Phase IV: DR entry if any new winter simulation ships (G-07's two-pass proof is mandatory). Multi-year arc: the Year of Ash as the campaign's spine — every window with almanac prose, every winter cost measurable, determinism proven.

## 17.13 C13 — Endgame and epilogue (DM-13)
Owns: Reckoning, verdict evaluator, epilogue matrix runtime, chronicle, standing records, census, muster epilogues, holdfast endings. Expanded plans: FP-A05-consumed A-28 via flagship F-005; satellites B-20, D-03, E-10, F-03, G-04. Phase I: F-005's permutation-audit chronicle tranches (32 permutations). Phase II: B-20 evidence-enrollment sweep for post-19-wave systems. Phase III: D-03 persistence-window fixtures for every enrolled evidence class; E-10 submission affordances for classes lacking them (audit first). Phase IV: DR entry whenever a new evidence class enrolls (the matrix's input set is the cluster's drift surface). Multi-year arc: every ending earned by paper — evidence classes, chronicle depth, and persistence guarantees growing together so no permutation can be invalidated by optional content.

## 17.14 C14 — Ecology and wildlife (DM-14)
Owns: migration, trapping, ecosystem, seasonal calendar, bestiary, underground flora, infestations, contagion, pathogens, crop genomes. Expanded plans: FP-A29 (this factory), F-002 blight arc; satellites B-21, C-10. Phase I: bestiary natural-history continuation. Phase II: B-21 migration-route encounter conditioning (verify current encounter selection first). Phase III: C-10 trapping-yield versus degradation harness with zoonosis premium; sighting-log census. Phase IV: DR entry if the zoonosis bridge or campfire sanitization seams move. Multi-year arc: a wasteland that reads as an ecology — seasonal prose, migration-aware travel, and trapping whose risks are priced.

## 17.15 C15 — Defense and security (DM-15)
Owns: perimeter defenses, defense grid, sky-defense ordnance and armor, chemical defense, orbital harrow telemetry, interlocks, EMP effects. Expanded plans: FP-A30 (this factory, sweep-gated); satellites B-22, plus FP-A14's rail-side interlock reports. Phase II: B-22 defense-grid siege-math binding (sky-armor values into telemetry thresholds — verify the partially built weather bridge first). Phase I: ordnance manifests and armor inspection records. Phase III: interlock violation prose depth; census of the two sky-defense catalogs. Phase IV: DR entry if the sky-armor-to-weather bridge completes. Multi-year arc: defense as logistics — manifests, inspections, and interlock records making the shelter's armor legible before it is ever tested.

## 17.16 C16 — Progression and meta (DM-16)
Owns: skills, research, collectibles, trophies, achievements, difficulty presets, XP wave, codex, field guide, L10N, mods, settings, input, cohort tuning, apprenticeship, library study. Expanded plans: satellites B-06, B-23, C-11, D-08, E-03, G-08, J-02 — all W1-sequence-gated. Phase I: none (the cluster's correct Phase I is waiting for the W1 seal — an honest zero-tranche phase). Phase II: B-23 consumer-binding sweep through the difficulty authority only, each consumer with a G-08 test. Phase III: C-11 preset-spread audit post-seal; D-08 stamp-migration tests; E-03 selection surface; J-02 observable-consequence preset texts. Phase IV: DR entry at the W1 seal itself. Multi-year arc: one difficulty authority, many consumers, zero parallel scalars — with the stamp, the migration, and the UI all sealed together.

## 17.17 C17 — Host surface and UI (DM-17)
Owns: panel families, shell components, focus navigator, snapshots, a11y, briefings; design pinned by DESIGN.md, a11y by ACCESSIBILITY.md, input by the 22-action map. Expanded plans: satellites E-01 through E-10, B-24, F-01. Phase I: briefing rows for Wave 8–12 systems and snapshot coverage for newest panels (both census-first). Phase II: E-05 words-not-color-only sweep and E-06 controller parity for new panels. Phase III: E-07 camp-panel truth (selftest first), E-08 barter legibility against DEC-05, E-09 storm-window lead time, E-10 submission affordances. Phase IV: DR entry per sealed-display priority change (DEC-05 is the precedent). Multi-year arc: zero-authority panels telling the truth attractively — every new system surface exposed within its wave, never after it.

## 17.18 Cross-cluster sequencing rules

1. A phase boundary in one cluster never schedules work in another cluster's sealed or gated surfaces; GATE items are cluster-local until their signature.
2. C16's Phase I is intentionally empty (sequence-gated on the W1 seal); no roadmap may pad it.
3. Clusters sharing a harness (C4/C11 balance reports, C12/C3 seasonal math) publish their Phase III harnesses in the same wave when their evidence overlaps, so the balance ledger updates once.
4. Phase IV entries are additive DR lines, never rewrites of existing DR entries (the drift register's own rule).
5. When two clusters' Phase II bridges touch the same owner (for example C7's DP-02 and C13's B-20 both touch consequence routing), the owner-first rule decides: one wave, one seam, one owner, stated in the wave charter.

---

# VOLUME 18 — RE-AUDIT AND SEED REPLENISHMENT (Factory batch 2026-09-27-E)

## 18.1 Audit method and its limits

This re-audit was performed against the repository's public surface (repository README and pull-request listings observed 2026-09-27), not against a working-tree checkout. That is a weaker evidence class than the 2026-09-24 audit the drift register was built on: pull requests are proposals until confirmed merged, and README/PR text is summary, not source. Every finding below is therefore labeled VERIFIED-AS-LISTED (the public page says so) plus WORKING-TREE CONFIRMATION REQUIRED (a session with the live tree must confirm merge state and scope before any seed consumes it). No finding below is promoted to a plan on listing evidence alone; they are drift-register candidates and seed replenishments.

## 18.2 Drift Register candidates

### DR-16 (candidate) — Faction communiqué board surface proposed on the war chain

A pull request observed on the public listing (#67, "prepare: PR #65 onto main") describes: exposing `FactionWarChainRunner.Catalog`, a `FactionCommuniqueBoardPanel` with word-wrap and truthful empty states, a `faction_communique_board` route with a dashboard button, replacement of a hardcoded communiqué card in `FactionsPanel` with live catalog queries, a `--faction-communique-board-selftest`, and a test package reported 31/31 passing. It also describes unblocking the Year of Ash owner tick gate (day >= 180) to enable extended-play war arc progression while seasonal systems self-clamp.
Status: VERIFIED-AS-LISTED; merge state and scope UNVERIFIED.
Consequence if confirmed: DM-7 (factions and war) gains a host-facing panel and a catalog exposure; DM-12 (Year of Ash) gains an extended-play war arc coupling; DM-17 gains a panel and route. The factory's seeds touching the war chain (A-18 communiqués, B-10 GATE per-strike emitters, DP-02) do not become invalid — DP-02's packet must be annotated with the board's existence so a signed per-strike emitter extension emits toward a board that may already render communiqués. The tick-gate change, if confirmed, materially extends the surface F-002's winter campaign plans against, and B-19's premise should be re-verified against the uncapped gate.

### DR-17 (candidate) — Map Atlas canonical-projection repair

A pull request observed on the public listing (#50) describes repairing the Map Atlas so it presents authoritative `WastelandMapSystem` state instead of derived or fabricated telemetry, explicitly removing hardcoded sector inference.
Status: VERIFIED-AS-LISTED; merge state UNVERIFIED.
Consequence if confirmed: a B-24-class finding (stale/untruthful panel) already repaired at the atlas level; DM-6's and DM-17's openings should be annotated so the factory does not propose a duplicate atlas-truth sweep. The atlas's repaired projection becomes the reference surface for FP-A15's marginalia pairing checks.

### DR-18 (candidate) — Repository hardening wave and quarantine cleanup

A pull request observed on the public listing (#49) describes removing 51 stale compile-remove quarantine entries and reconciling "Plan 34 truth."
Status: VERIFIED-AS-LISTED; merge state and scope UNVERIFIED.
Consequence if confirmed: DR-07's caution about gate-count and test-total drift is directly supported — any count quoted in this document or its predecessors is again stale until re-verified against the live inventory. F-008's gate-count drift guard gains a concrete motivating incident if the stale-entry count is confirmed.

### DR-19 (candidate) — Post-v1.0 subsystem families absent from the deep maps

A flagship asset-pipeline pull request observed on the public listing (#36) describes systems the factory's 2026-09-24 deep maps do not contain: a Cultural Archive Vault System (restoration, transcription projects, microfiche preservation, acetate disc cutting with playback morale staying with a VinylMoraleSystem, a non-stacking salon modifier with cooldown, idempotent chronicle milestones), a Diplomatic Summit System (neutral-site scheduling, keyed-RNG deterministic negotiation rounds with per-(seed, summit, round) FNV-derived streams, atomic concession payment at ratification, treaty lifecycle with DMZ zone rules, guarantee/hostage arrangements, an event-driven violation ledger routed through an IFactionStandingPort), an IInstitutionAvailability shared assignment port (one live claim per survivor across institutions), and a WeatherSondeSystem as a humidity provider host binding.
Status: VERIFIED-AS-LISTED; whether these families are merged, partially merged, or proposal-stage is UNVERIFIED.
Consequence if confirmed: the deep maps' inventory is incomplete in a way that matters — new narrative domains (cultural-archive prose, summit protocol documents under contract 5.12, salon and vinyl-morale texture, institution-claim fiction), new Lane B seams (treaty lifecycle consumers, institution-availability interactions with cohort and apprenticeship systems), and new determinism patterns (the keyed-RNG negotiation streams are a sub-stream naming precedent for H-05's seed-forge registry). None of this may be planned until a working-tree session confirms merge state and scope; the factory records it as replenishment candidates only.

## 18.3 Premise corrections to the factory's own seeds

1. B-10/DP-02 (per-strike emitters, GATE): unchanged in status — the observed PR #67 work is a board/catalog exposure, not per-strike emission — but DP-02's packet should carry a DR-16 annotation so a signed emitter design names its rendering surface explicitly.
2. F-002 and B-19 (winter pressure, Days 180–360): premise re-verification flagged — DR-16's described tick-gate uncap, if confirmed, changes what "extended-play war arc progression while seasonal systems self-clamp" means for winter-pressure modeling. The winter campaign's premise sweep must read the gate's live implementation before any tranche.
3. B-24 (stale-panel sweep): DR-17 shows the atlas case may already be repaired by its owners; the sweep should start from the live panel inventory, not from a list that includes the atlas on stale evidence.
4. H-05 (seed-forge registry): DR-19's keyed-RNG streams, if confirmed, are a second live sub-stream naming precedent (alongside the sealed rescue runtime's streams) and should be cited in H-05's design so the registry models real naming, not invented naming.
5. E-01 (briefing rows for Wave 8–12 systems): DR-19's institution and cultural families, if confirmed, are exactly the class E-01 enumerates — the session enumeration must include them if merged.

## 18.4 Seed replenishment (new candidates, all evidence-gated on DR-19 confirmation)

- **A-31 · C9/C16 · Cultural-archive document corpus.** Restoration logs, transcription project sheets, microfiche condition reports, acetate disc-cuting manifests — the manifest and dossier registers over a confirmed Cultural Archive Vault System. Route: DATA-ONLY after working-tree confirmation. Confidence: HYPOTHESIS pending DR-19.
- **A-32 · C7 · Summit protocol documents.** Treaty-protocol entries (contract 5.12) and summit scheduling notices over a confirmed Diplomatic Summit System, with violation-ledger notices routed through the standing port. Route: DATA-ONLY. Confidence: HYPOTHESIS pending DR-19.
- **B-26 · C16 · Institution-availability interaction audit.** The one-live-claim-per-survivor port's interactions with apprenticeship and cohort assignment systems — an ownership-boundary check, not a feature. Route: audit then, only on evidence, CORE-EXTENSION. Confidence: HYPOTHESIS pending DR-19.
- **G-09 · C7 · Summit determinism regression suite.** If the keyed-RNG negotiation rounds are confirmed, pin their per-(seed, summit, round) stream discipline with a two-pass proof and boundary tests, the G-07 pattern applied to diplomacy. Route: tests. Confidence: HYPOTHESIS pending DR-19.
- **E-11 · C17 · Communiqué board snapshot and a11y coverage.** If DR-16 is confirmed, the new board panel needs the E-02 snapshot and E-05/E-06 parity treatment from its first wave, not a later sweep. Route: HOST-WIRING tests. Confidence: HYPOTHESIS pending DR-16 confirmation.

All five replenishments carry the standing rule: no planning work begins until a session with the working tree confirms the underlying PRs' merge state and scope. The factory does not build on unmerged foundations.

## 18.5 Factory self-audit (this session)

- Lane A compressed seeds remaining: zero. A-01 through A-30 are now all consumed into expanded plans (Volumes 6 and 15) or a flagship (A-28 into F-005).
- GATE items: unchanged — DP-01, DP-02, DP-03 remain signature-blocked; no packet was opened in this session.
- Sealed surfaces: untouched. The distress-content seal, the rescue runtime, and DEC-05 were respected; FP-A20 carries an explicit seal guard.
- Consumed implementation: none. All volumes remain subject-plan and model level; the factory's consumption ratio stays zero, which remains the correct state for a read-first authority.
- Character accounting: this four-volume session (Volumes 15–18) appends approximately 66,000 characters, bringing the document to approximately 348,000 characters against the 1,500,000 minimum and 2,000,000 target.

## 18.6 No-change areas observed

The public README's system inventory (Disease, Dose Ledger, Duty Roster, Economy/Market, Crafting, Expeditions, Muster, Narrative catalogs, Radiation, Research, Survivors, UtilityAI, Verdict, Year of Ash, Weather; shared ports IJsonSerializer, IFileIO, ISeededRng, ILog; checksummed envelopes; CatalogIntegrityValidator) agrees with the factory's 2026-09-24 audit conclusions — no drift correction is warranted against the core inventory, and the DR-01 through DR-15 register stands as written subject to the four candidates above.
---

# VOLUME 19 — PUBLIC-SURFACE CONFIRMATION PASS: DRIFT REGISTER UPGRADE (Factory batch 2026-09-27-F)

## 19.1 Method and evidence class

This volume re-examines the DR-16 through DR-19 candidates from Volume 18 against additional public pull-request listings observed 2026-09-27. Two evidence classes are used, and the distinction is binding:

- MERGED-AS-LISTED — the public pull-request page states the PR was merged into main, with a merge date. This is strong listing evidence but still not working-tree evidence: a session with the live tree remains the final confirmation for scope and surviving implementation.
- VERIFIED-AS-LISTED — the page describes the content but the merge state is not stated in the listing; unchanged from Volume 18.

No candidate is treated as canon until a working-tree session reads the merged files. But merged-as-listed candidates may now be annotated into the deep maps as "present on main as listed," which changes what the factory's plans may assume at the premise level.

## 19.2 Drift Register upgrades

### DR-16 — UPGRADED to MERGED-AS-LISTED (2026-09-19)

PR #67 (17 commits, merged 2026-09-19) is publicly listed as merged into main. Its listed scope, beyond Volume 18's record: a three-axis version policy (engine-free strict-semver parser in `Assets/Ashfall.Core/ReleaseVersion.cs` with `ClassifyBump` and a hardened `PrintVersion` whose fallback is INVALID rather than silent), `<VersionPrefix>1.1.0</VersionPrefix>` in `Directory.Build.props`, Windows preset version sync 1.0.0 to 1.1.0, a `docs/releases/VERSIONING.md` three-axis policy with semver mapping and support window, a 32-test `ReleaseVersionContractTests` contract suite, Plan 48 Phase 6 hygiene and closeout (a regenerated agent skills index cataloging 35 skills, `docs/architecture/CLAIMS.json` with all 28 claims verified including `rel_save_support_window_tested` moving from UNVERIFIABLE to TRUE/PROVEN_INTEGRATION), release-captain skill vocabulary updates, and the communiqué board and tick-gate work recorded in Volume 18.
Consequences if working-tree-confirmed: (a) DM-16 gains a live version authority and a 1.1.0 release line — D-05's save-support-window re-pin seed now has a concrete triggering release; (b) the factory's Appendix A command surface gains release gates (`prepare-release.sh`, `release-gate.sh` as listed); (c) the drift register's DR-07 gate-count caution gains a second concrete incident (the stale `release_fixture_matrix` gate reference removed from CLAIMS.json); (d) Volume 18's DR-16 annotations for DM-7, DM-12, and DM-17 stand.

### DR-17 — UPGRADED to MERGED-AS-LISTED (2026-09-18)

PR #50 (6 commits, merged 2026-09-18): the Map Atlas canonical-projection repair is publicly listed as merged. Volume 18's consequence analysis stands: the atlas is repaired by its owners, and FP-A15's marginalia pairing checks reference the repaired projection. B-24's sweep starts from the live panel inventory and should not list the atlas on stale evidence.

### DR-18 — UPGRADED to MERGED-AS-LISTED (2026-09-18)

PR #49 (4 commits, merged 2026-09-18): the hardening wave is publicly listed as merged, reconciling KNOWN_DEBT.md from "51 active quarantines" to the actual state — historical drafts described as absent/recoverable rather than silently excluded. Consequences: F-008's gate-count drift guard has two confirmed motivating incidents; any quarantine count quoted in the v1.0 bible or this factory is stale as of 2026-09-18; the DR-11 KNOWN_DEBT seals remain valid in substance (the seals concern specific items, not the count).

### DR-20 (new) — Wave-5/6 partial-integration subsystem inventory (MERGED-AS-LISTED for the carrier, scope UNVERIFIED)

PR #62's listing describes a full sync of local workplace with upstream integrating Wave 5 survivor, shelter, narrative, economy, spiritual, treaty, UI, persistence, and data-authority changes, and names Core systems the factory's deep maps do not contain: `MemoryDecaySystem` (Plan 185), `ShelterArchiveSystem` and `ShelterAtmosphereSystem` (Plan 162), `TimeCapsuleSystem`, `CultureCreationSystem`, `DocumentationSystem`, `ResourceRationingSystem`, `ColonySystem`, `DiscoveryConsequenceSystem`, `CartographySystem`, `RumorSystem`, `ItemLoreSystem`, `AfflictionDutyBridge`, `PropagandaSystem`, `DynamicQuestGenerator`, `RegionalTreatyCatalogLoader`, `ShelterNoiseSystem`, `ShelterSecuritySystem`, `ChildDevelopmentSystem`, `ExerciseSystem`, `HiddenAgendaSystem`, `HobbySystem`, `InterpersonalConflictSystem`, plus a fifteen-plan completion-first Seal-steps program authorized 2026-09-18 and Plan 24's signed closeout options. PR #62's own merge state is not stated in the listing; however, PR #66 (merged 2026-09-19) is listed as integrating Plans 167 (tunnels) and 219 (documentation) with focused tests 8/8 and 6/6, synchronizing port contract policies, the CLI command catalog, the save store contract matrix, and agent rulebooks, and recording 15/15 partial plans integrated — which corroborates that the wave-5/6 partial-integration program reached main through a different carrier.
Status: subsystem list VERIFIED-AS-LISTED; carrier merge state UNVERIFIED; corroborating PR #66 MERGED-AS-LISTED.
Consequences: this is the largest deep-map gap the re-audit has found. DM-9 gains memory-decay, child-development, exercise, hidden-agenda, hobby, and interpersonal-conflict systems; DM-1 gains shelter archive, atmosphere (already known), noise, and security systems; DM-11 gains resource rationing; DM-6 gains cartography; DM-8 gains rumor (already known as bands) and propaganda; DM-10 gains a dynamic quest generator; DM-13 gains time capsules and discovery consequences. A working-tree session must inventory these before any cluster's Phase I census, because several censi (E-01's briefing enumeration, A-31/A-32's domain checks, the prose census tranches) would otherwise enumerate against an incomplete system list.

### DR-21 (new) — Shelter atmosphere and noise panels landed with a follow-up bug fix (MERGED-AS-LISTED, both)

PR #68 (3 commits, merged 2026-09-19) landed Plan 220 shelter atmosphere and Plan 205 noise discipline; PR #69 (2 commits, merged 2026-09-23) then registered `_shelterAtmospherePanel` in the overlay lists and removed a merge artifact — publicly described as two verified bug-fixing findings.
Consequences: DM-17's panel inventory grew and immediately produced a registration defect that its owners fixed — this is direct evidence for E-02's snapshot coverage and B-24's truth sweep premise: new panels have historically shipped with registration gaps, and the sweep class is not theoretical. DM-1 gains atmosphere and noise as panel-visible state, which enlarges FP-A01's glitch-batch room set and A-02's load-shed amendment conditioning surface.

### DR-22 (new) — Commitment-event semantic parity repair corroborates DP-04's premise (MERGED-AS-LISTED)

PR #46 (3 commits, merged 2026-09-18) repaired a full-suite regression caused by Plan 38 commitment events emitted without Plan 31 semantic/parity registration, with the root cause named as `CommitmentSystem` registration.
Consequences: this merged repair is direct, dated evidence that the repository's event surfaces carry a semantic-registration discipline, and that missing registration produces suite-level regressions. DP-04 (semantic-kind regrouping) gains a corroborating precedent: any regrouping must include parity registration for every affected event kind, and DP-04's packet should be annotated with this incident so the signature holder sees the failure mode the packet is guarding against.

### DR-23 (new) — Port-contract seam ratchet cleanup (MERGED-AS-LISTED)

PR #51 (8 commits, merged 2026-09-18) reconciled six seams counted DEFERRED whose source contracts prove them live in Core or explicitly optional, described as a Plan 36C ratchet cleanup.
Consequences: the port-contract DEFERRED ledger shrank on 2026-09-18; DR-11's port-contract DEFERRED seal remains valid as a seal but its count is stale. Any factory plan citing a specific DEFERRED seam count must re-read the live contract matrix first.

## 19.3 Premise corrections to the factory's own plans

1. FP-A20 and the E-01 briefing enumeration: enumerate against the DR-20 subsystem list plus the live panel inventory; the pre-DR-20 system list is known-incomplete.
2. FP-A01/FP-A02 room sets: include the atmosphere and noise rooms DR-21 adds, subject to working-tree confirmation.
3. DP-04: annotate with the DR-22 incident (semantic parity registration is a merge-blocking discipline, evidenced by a merged repair).
4. D-05: the 1.1.0 version line (DR-16) is the release-class bump the seed waits for; its re-pin premise is now scheduled rather than hypothetical.
5. F-008: cite both DR-18 and DR-16's stale-gate-reference incident as motivating evidence; the design still requires the live gate inventory as input.
6. H-05: the seed-forge registry design gains a third naming precedent if DR-20's DynamicQuestGenerator and discovery-consequence systems consume seeded streams (UNVERIFIED — check in the working tree).

## 19.4 Standing honesty rule for this register

Every MERGED-AS-LISTED entry in this volume records a public merge claim observed 2026-09-27. The factory does not have the working tree. Until a session with repository access confirms each entry's surviving scope, no seed may consume a DR-20 subsystem as an existing owner, and no plan may cite a DR-16 release gate as an available command. The candidates DR-19 (PR #36 flagship families) and DR-20's carrier (#62) remain at listing level and keep their Volume 18 gating unchanged.

---

# VOLUME 20 — AUTHORING SESSION RUNBOOKS (Factory batch 2026-09-27-G)

The factory's plans name their open premises; this volume turns each premise class into a step-by-step session runbook an authoring session can execute against the working tree. Every runbook uses only the verification surfaces the repository's own canon establishes (integrity selftest, utilization selftest, focused xUnit, headless selftest flags, the Part 14.1 command surface, the generated contract matrices) and the Part II factory loop. Runbooks are instruments, not code: they prescribe observation and gating, never edits. Where a runbook depends on a DR-16-through-DR-23 entry, the dependency is named, because no merged-as-listed surface may be assumed without working-tree confirmation.

## 20.1 RB-CENSUS — the generic prose-depth census (the Tranche 0 pattern)

1. Read the target catalog with the field-inventory protocol (7.1): list every record, every prose field, and the field's contract (5.x).
2. Score each record per prose field: EMPTY (absent), SKELETON (present but under contract minimum), COMPLETE (meets contract).
3. Group records by owning family (questline family, room, station, zone — whatever the catalog's natural grouping is) and publish the per-family gap table in the plan document.
4. Verify the loader path: locate the consuming system's read of the field (one call-site inspection minimum; two if the field feeds branching).
5. Verify the persistence path if the field conditions behavior (D-07 pattern): confirm the conditioning value round-trips.
6. Run the catalog integrity selftest and record the baseline result before any authoring.
7. Publish the tranche boundaries: family-grouped batches sized so one batch is verifiable in one session.
8. GATE: no authoring begins until steps 1–7 are published; a census that finds zero gaps is a valid outcome and closes the plan with no tranche.

## 20.2 RB-COLLISION — the narrative corpus collision sweep (the A-09/A-14/A-30 pattern)

1. Build the corpus family index: list every narrative corpus catalog and its genre family from the corpus index and the live data listing.
2. For the proposed genre family, search all existing corpus catalogs for any entry in the same genre (the sweep is corpus-wide, not cluster-local — a rail document family could live under logistics, not rail).
3. If a family exists: read its schema, its consuming system, and three sample entries; the plan attaches to the existing family and step 4 is skipped.
4. If no family exists: the plan must follow H-04's validator-extension discipline — the new family's loader, integrity rules, and utilization path are mandatory plan steps, not tranche steps.
5. Record the sweep result in the plan document with the searched catalogs listed; an unswept genre family is a defect in the plan, not a shortcut.

## 20.3 RB-PAIR — the cross-catalog pairing test pattern (glitch/room, marginalia/zone, assay/process)

1. Identify the two catalogs: the prose surface and the id authority.
2. Write the focused test as an assertion over ids, not values: every prose record's reference id must resolve in the authority catalog.
3. Add the inverse assertion where the contract requires coverage: every authority id with prose obligations has at least one prose record (only where the plan promises coverage, never as blanket padding).
4. Wire the test into the established focused-test family; per-row failures, one assertion per reference, so a single broken id names itself.
5. Run before authoring (it should fail on the gap) and after each tranche (it should shrink monotonically); at completion it passes.

## 20.4 RB-SEALGUARD — the sealed-surface guard pattern (FP-A20 and any radio-adjacent authoring)

1. Enumerate the sealed surface's vocabulary: the seal's forbidden content classes, from the seal record and its closeout, not from memory.
2. Write the guard test as a vocabulary assertion over the new tranche: no new entry may introduce, imply, or foreshadow the sealed class (signal scenarios for the distress seal).
3. Run the guard on every tranche of the genre, forever — the guard is permanent, not one-time.
4. Any guard failure is a stop-work condition: the tranche is withdrawn, not patched around, because a seal breach invalidates the tranche's premise entirely.

## 20.5 RB-KNOWLEDGE — the information-legality audit (intake, testimony, and any survivor-voice prose)

1. For each prose subject (named or anonymized), build the knowledge horizon: what the subject's position, channel, and timeline permit them to know, from the owning systems' state surfaces.
2. Write the knowledge block into the record (the 4.6 pattern) and treat it as binding.
3. Audit the prose against the block line by line; any statement exceeding the horizon is a defect even if it reads well.
4. For testimony referencing events: check the event's clue surface — hidden-account content is unreachable until a clue reference is discovered, and prose may not leak it early.

## 20.6 RB-QUANT — the ledger-agreement audit (manifests, ordnance, requisition prose)

1. Every quantity in prose must equal or annotate a value the owning system records; find the owning record.
2. Where prose states a quantity the system does not track, either the prose is wrong or the system's record is the gap — write the finding to the plan, do not silently invent agreement.
3. Discrepancies found in the catalog itself are Lane B findings, not prose defects; file them as seeds, never fix them in a prose tranche.

## 20.7 RB-WAVE — the wave charter execution checklist (from the Volume 14 runbook, restated as gates)

1. Charter named, lanes allocated (max one plan per lane), flagship plus satellites capped at five concurrent plans on shared seams.
2. Each plan's verification class named and its focused test written before its tranche.
3. One quest-catalog tranche maximum per wave; one census-gated plan minimum if any census premise is unresolved.
4. Closeout per plan: the plan doc's acceptance criteria, the focused test result, and the cumulative ledger updated in the same session.
5. Anti-scope-creep review at wave end: every completed item maps to a charter line; every non-mapped item is a finding, not a bonus.

## 20.8 RB-DRCONFIRM — the drift-register working-tree confirmation pass (the DR-16 through DR-23 gate)

1. For each MERGED-AS-LISTED entry: confirm the named files exist on main and contain the described surfaces (release parser, versioning doc, claims file, panel registrations, hardening reconciliation).
2. For each VERIFIED-AS-LISTED entry: locate the carrier PR's merge state in the repository history; record merged, open, or superseded.
3. For DR-20's subsystem list: inventory each named system in `Assets/Ashfall.Core/` and record present, absent, or renamed; the deep maps are corrected in the same session.
4. Publish the confirmation table as the next re-audit volume's first section; any candidate that fails confirmation is retired with its retirement recorded, not silently dropped.
5. Only after this pass may the five Volume 18 replenishment seeds and the DR-20-derived surfaces enter Phase I planning.

## 20.9 Runbook discipline

A runbook may not be modified by the session that executes it; deviations are recorded in the plan document as either a runbook defect (fix the runbook for future sessions) or a scope deviation (justify against the charter). This keeps the runbooks honest instruments rather than aspirational text, and it makes every executed session auditable against a stable procedure — the same standard the repository's own gates impose on code.

---

# VOLUME 21 — REMAINING LANE B AND ALL LANE C SEED EXPANSIONS (Factory batch 2026-09-27-H)

Four Lane B seeds (B-04, B-06, B-14, B-15) and the entire Lane C economy set (C-01 through C-14) remain compressed. This volume expands them, closing the compressed backlog for Lanes B and C exactly as Volume 15 closed Lane A. The three Lane B GATE items (B-09, B-10, B-11) remain correctly out of scope: they live in the Volume 12 decision packets and open only on signature. All plans are subject-level PROPOSALs committing no file changes. Lane C's plans are balance-harness plans and therefore carry the Category 4 discipline in full: measurement first, no optimization without a demonstrated number, and every plan's first deliverable is a report, not a change.

## FP-B04 — Child-Health Cohort Bridge

Lane B · C2/C9 · Status PROPOSAL.
Subject: child survivors' health needs feeding the medical pipeline through the cohort system's scoped links, so that child-development and child-health state produces legible medical demand.
Premise evidence: VERIFIED the 19B closeout records child rations and schooling links (DR-06 ledger); VERIFIED cohort catalogs live (`starting_survivor_cohorts`, `cohort_tuning`); VERIFIED the medical pipeline owners (DM-2). DR-20's listing additionally names a `ChildDevelopmentSystem` (MERGED-AS-LISTED carrier, scope UNVERIFIED) — this plan's premise sweep must inventory that system first, because the bridge may already be half-built.
Why this: children are a canon survivor class whose medical representation is the thin side; the cohort system's scoped links are the sanctioned route (no new parallel authority).
Must not change: cohort mechanics; medical pipeline state machines; the bridge routes demand through existing owners only.
Route: CORE-EXTENSION through cohort and medical owners, additive. Save impact: EXISTING-SECTION if child-health facts persist (verify which section in session). Determinism: NONE expected — state-derived, not random.
Continuity: a child's medical demand must be expressible in existing disease/dose vocabulary; no new disease class for children without the Part 12.1 authoring route.
Verification: focused xUnit for each bridge (cohort state in, medical demand out); a deterministic 30-day fixture with children present; medical-capacity report re-run (the `MEDICAL_30_DAY_CAPACITY_REPORT.md` pattern).
Open premises: 1. Inventory `ChildDevelopmentSystem` on the working tree (DR-20 gate). 2. Enumerate which child-health facts already flow to medical owners; the plan covers only the measured gap.

## FP-B06 — Industrial Difficulty-Scalar Consumer Binding (post-W1)

Lane B · C4/C16 · Status PROPOSAL, sequence-gated.
Subject: bind industrial fuel, feedstock, and labor consumption scalars to the XP W1 difficulty authority once it seals — the sanctioned difficulty seam, never parallel scalars.
Premise evidence: VERIFIED W1 ACTIVE (DR-06); VERIFIED the industrial catalogs live (DM-4); VERIFIED B-23 establishes the consumer-binding pattern and G-08 its test wave.
Why this: the industrial cluster is the largest unbound consumer surface (twenty-plus processes); binding it second, after B-23's sweep establishes the pattern, keeps one authority and many consumers.
Must not change: process parameters' authored values; the binding reads the authority at consumption time; no consumer caches a scalar copy.
Route: CORE-EXTENSION through the difficulty authority only, post-seal. Save impact: NONE (presets are read, not persisted per consumer — verify in session). Determinism: NONE.
Continuity: every bound consumer ships with a G-08 test in the same tranche (scalar flows from authority, no parallel scalar exists).
Verification: G-08 focused tests per consumer; the C-11 preset-spread harness post-seal confirms the industrial chains' curves are distinct and legible.
Open premises: 1. W1 seal state at session time. 2. The authority's scalar surface names (read, do not assume). 3. Whether any industrial consumer already binds (measure first).

## FP-B14 — Belief-Movement Faction-Stance Bridge

Lane B · C7/C9 · Status PROPOSAL.
Subject: belief movement membership shifting faction standing through the `FactionStanceEngine` — the sole standing authority — so proselytizing and conversion have political consequences.
Premise evidence: VERIFIED `belief_movements.json` live (DM-9); VERIFIED `FactionStanceEngine` is the sole standing authority (DM-7 constraint); VERIFIED stance changes route through the engine's existing effect kinds.
Why this: two live systems with no measured bridge; the stance engine's effect vocabulary already models relationship-class shifts, so the bridge is a mapping, not a new mechanic.
Must not change: stance effect kinds (a bridge that needs a new kind stops and files a Part 9 contract instead); belief movement doctrine content; DR-22's semantic-parity discipline applies — any new event kind registers semantically before it emits.
Route: CORE-EXTENSION, additive. Save impact: EXISTING-SECTION if movement membership persists (verify). Determinism: NONE.
Continuity: standing changes respect information-flow legality — a faction learns of a conversion through the modeled channels, with the gossip lag B-17 separately proposes; never instantly.
Verification: focused xUnit per movement-stance pair in the mapping table; a deterministic fixture where a conversion propagates through the engine and the standing delta is asserted; G-01-style consumer check that every authored movement has at least one stance mapping or an explicit null mapping.
Open premises: 1. Enumerate movement-faction pairs and each faction's authored stance baseline in session. 2. Confirm the engine's effect-kind vocabulary accepts the mapping (the vocabulary check FP-B15 also requires).

## FP-B15 — Memorial-Rite Reckoning Evidence Enrollment

Lane B · C9/C13 · Status PROPOSAL.
Subject: performed memorial rites enrolling as Reckoning evidence, so grief practices count at the endgame.
Premise evidence: VERIFIED `memorial_rites.json` and `spiritual_rituals.json` live (DM-9); VERIFIED the Reckoning consumes enrolled evidence classes (DM-13); the seed's own caution stands: the evidence vocabulary must be checked for a rite class before authoring.
Why this: the epilogue matrix rewards documented moral weight; rites are canon moral behavior with zero endgame representation unless the vocabulary already includes them.
Must not change: the evidence vocabulary's closed-class rule (a new class follows the endgame owners' enrollment process, not a prose tranche); the main-ending inviolability constraint (DM-13).
Route: audit then CORE-EXTENSION through endgame owners. Save impact: EXISTING-SECTION (enrollment persists in the standing evidence store — verify section in session). Determinism: NONE.
Continuity: enrollment is state-derived (a rite was performed and recorded), never authored directly; D-03's persistence-window fixture covers the new class.
Verification: focused xUnit that a performed rite enrolls exactly once (the exactly-once discipline); G-04 permutation reachability re-run with rites as an available evidence source; the D-03 fixture for the new class.
Open premises: 1. The evidence vocabulary check (binding). 2. Confirm rite performance is itself persisted (a rite that does not persist cannot enroll).

## Lane C — harness plan preamble (applies to FP-C01 through FP-C14)

Every Lane C plan follows one shape: a deterministic harness run against live catalogs and sealed tuning, a published report in the established balance-report location, findings labeled mathematical issue versus design preference, and tuning only in a follow-on tranche gated on the report's evidence. The Category 4 golden rule is restated as a lane rule: no Lane C plan changes a number in its first tranche. Quantity-band trims (C-13) are the only sanctioned remedy class and only on demonstrated E[value] outliers. Difficulty-preset-dependent harnesses re-run per preset once W1 seals; before that, they run on the current authored values and state the preset limitation in the report header.

## FP-C01 — Industrial Chain Income-Versus-Expenditure Audit

Lane C · C4 · Status PROPOSAL.
Subject: per-chain accounting — fuel, feedstock, and labor cost against output value at current regional prices — flagging dominated processes (output strictly cheaper to buy than to make).
Premise evidence: VERIFIED the industrial catalogs, `regional_prices.json`, and `commodity_baselines.json` live (DM-4, DM-11); VERIFIED hardcore tuning exists.
Route: TOOLING harness plus report. Save impact: NONE. Determinism: seeded simulation, two-pass proof (G-07) because the harness may consume seeded streams.
Method: for each process, compute unit cost from inputs at regional prices plus energy, compare against output baseline; rank by domination margin; label each dominated process as tuning candidate or intentional scarcity (the label is a judgment the report makes explicitly, with the design target stated as an assumption).
Verification: harness determinism (two-pass byte-identical), report published, domination table cross-checked against one manually computed chain.
Open premises: 1. Whether labor cost is an authored value anywhere (if absent, the harness reports material-only and says so). 2. Regional price variance handling (report per region or at baseline, stated in the header).

## FP-C02 — SOFC Fuel-Consumption Sustainability Audit

Lane C · C4 · Status PROPOSAL.
Subject: real inventory fuel consumption (sealed per Plan 125) versus expected fuel income across difficulty presets, per the Plan 122/125 closeouts.
Premise evidence: VERIFIED the Plan 122/125 closeouts via the ledger (DR-06); VERIFIED SOFC catalogs live; preset limitation per the lane preamble until W1 seals.
Route: harness plus report. Determinism: seeded, two-pass.
Method: fixed simulation horizons (30/90/360 days), fuel ledger delta versus income sources at each horizon, per preset when available; unsustainable trajectories labeled with their breakpoint day.
Verification: two-pass determinism; breakpoint days reproducible; report published.
Open premises: 1. Read the Plan 125 seal's actual consumption model in session (the seal's semantics, not the summary). 2. Fuel income source enumeration (which systems grant fuel).

## FP-C03 — Scavenging E[value] Re-Run

Lane C · C5 · Status PROPOSAL.
Subject: the Plan 76.2-pattern seeded re-run of expected loot value per destination and table, publishing deltas against the last published baseline.
Premise evidence: VERIFIED the harness pattern is canon; VERIFIED the 49-table/53-destination surface (v1.0 Part 6.2); VERIFIED the expedition balance baseline documents live.
Route: TOOLING. Determinism: seeded, two-pass.
Method: per-destination E[value] under the current tables, delta versus `EXPEDITION_BALANCE_BASELINE.md`, outliers feeding C-13's evidence-gated trims.
Verification: two-pass; deltas reconciled against one hand-checked destination; report published.
Open premises: 1. Whether loot-affecting waves landed since the last baseline (the premise sweep answers when a re-run is warranted at all — a no-delta run is a valid result and closes the plan).

## FP-C04 — Vehicle Dominance Follow-Up

Lane C · C5 · Status PROPOSAL.
Subject: re-evaluate `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md` against new vehicle modifications and armor grades.
Premise evidence: VERIFIED the table live (DR-03); VERIFIED `vehicle_modifications.json` and `vehicle_armor_grades.json` live (DM-5).
Route: harness plus report. Determinism: seeded where encounter sampling enters.
Method: per-vehicle expected expedition outcome across the modification/grade matrix; dominated configurations (strictly worse than an alternative at equal cost) flagged; the existing table's rows reconciled against the recomputed ranks.
Verification: two-pass; reconciliation table published; dominated-configuration list with cost context.
Open premises: 1. Whether modifications or grades were added since the table's last revision (git history check in session; a no-change result closes the plan).

## FP-C05 — Tribute Sustainability Per Doctrine

Lane C · C7 · Status PROPOSAL.
Subject: the 7-day tribute cadence versus mid-game income across doctrines and presets, distinguishing mathematically unsustainable demand spirals from intended harshness.
Premise evidence: VERIFIED `warlord_doctrines.json` live (DM-7); VERIFIED the cadence is canon; tribute payment routes through the existing economy owners.
Route: harness plus report. Determinism: seeded where market variance enters.
Method: per doctrine, simulate tribute demand against representative mid-game income profiles; a demand spiral is flagged when debt or shortfall compounds without a recovery path — the Part 6.7 recovery grammar is the discriminator: intended harshness still leaves a legible path.
Verification: two-pass; per-doctrine table with spiral/flag/no-flag labels; recovery paths named for each flagged doctrine or their absence recorded as a finding.
Open premises: 1. Mid-game income profile definition (state the assumption in the header). 2. Whether tribute demand scales with any standing state (read the demand model first).

## FP-C06 — Embargo Pressure Modeling

Lane C · C7/C11 · Status PROPOSAL.
Subject: `trade_embargoes.json` impact on settlement price bands, verifying embargoes produce legible price signal rather than noise.
Premise evidence: VERIFIED the embargo catalog live (DM-7); VERIFIED price-band mechanics route through the market owners (DM-11).
Route: harness. Determinism: seeded where market variance enters.
Method: with/without embargo runs per embargoed commodity, price-band deltas compared against band width — an embargo whose delta is smaller than ordinary variance is noise and is flagged as such.
Verification: two-pass; signal-to-noise table published.
Open premises: 1. The embargo application model's actual seams (read before simulating). 2. Whether multiple simultaneous embargoes stack (the harness must model the live behavior, not an assumption).

## FP-C07 — Debt-Interest Runaway Analysis

Lane C · C11 · Status PROPOSAL.
Subject: compound debt trajectories against realistic income, identifying unrecoverable states and confirming each has a recovery path.
Premise evidence: VERIFIED `LedgerDebtSystem` with interest and consequence dispatchers is canon (DM-11); VERIFIED `ledger_debt_templates.json` live; VERIFIED the recovery grammar is canon (v1.0 Part 6.7).
Route: harness plus report. Determinism: seeded where income variance enters.
Method: per template, project balance under representative income, payment, and consequence schedules; flag states where every legal action sequence still compounds (true unrecoverability) versus states with at least one recovery path; G-02's dispatcher coverage is the companion guarantee that flagged paths are actually implemented.
Verification: two-pass; per-template trajectory table; every unrecoverable finding double-checked for a missed legal action before publication.
Open premises: 1. The interest model's exact compounding cadence (read). 2. The consequence dispatcher's full effect set (G-02's matrix is the input).

## FP-C08 — Black-Market Price-Tier Audit

Lane C · C11 · Status PROPOSAL.
Subject: `black_market_inventory.json` pricing versus legal market bands and scarcity premiums.
Premise evidence: VERIFIED the inventory catalog live (DM-11); VERIFIED regional prices and hardcore tuning live; the actions surface is sealed (DR-06, `WAVE8-PART2-C1-BLACK-MARKET-ACTIONS`) and the funds legs remain GATE (DP-03) — this audit touches neither.
Route: harness. Determinism: NONE expected (static price comparison).
Method: per item, black-market price versus legal band and versus a scarcity premium model; mispriced items (below legal floor without scarcity justification, or above any rational premium) flagged.
Verification: comparison table published; one item hand-checked per pricing class.
Open premises: 1. Whether black-market prices are static or band-driven (read the loader). 2. The scarcity premium's authored model, if any exists — if none, the audit reports raw ratios and states the absent model as a finding, not an assumption.

## FP-C09 — Winter Resource Compression Audit

Lane C · C12/C3 · Status PROPOSAL.
Subject: Days 90–180 and 180–360 calorie, fuel, filter, and morale sustainability per difficulty preset (sustainability-day math), within the Year-of-Ash canon window.
Premise evidence: VERIFIED storm windows canon (180–360); VERIFIED nutrition profiles and baselines live (DR-03, DM-3); preset limitation per the lane preamble until W1 seals.
Route: harness plus report. Determinism: seeded, two-pass (weather enters).
Method: per window, closed-system accounting — income versus expenditure for each resource under representative play profiles; the breakpoint day per resource is the headline number; B-19's winter-pressure premise uses this report as its evidence gate.
Verification: two-pass; per-resource sustainability table; hand-reconciliation of one resource's income side.
Open premises: 1. The play-profile definitions (state assumptions in the header). 2. DR-16's tick-gate change's effect on window simulation scope, subject to working-tree confirmation.

## FP-C10 — Trapping Yield Versus Degradation Cost

Lane C · C14 · Status PROPOSAL.
Subject: `wildlife_trapping_catalog.json` yields against equipment condition degradation, with the uncooked-yield zoonosis risk premium made explicit.
Premise evidence: VERIFIED the trapping catalog live (DM-14); VERIFIED the zoonosis bridge is the owned seam; VERIFIED equipment degradation is canon.
Route: harness. Determinism: seeded where yield variance enters.
Method: per trap type, expected yield value per deployment versus expected degradation cost and bait cost; net-positive and net-negative configurations ranked; the zoonosis premium computed as the expected medical cost of uncooked-yield consumption where the bridge models it.
Verification: two-pass; per-trap table with the premium column; one trap hand-checked.
Open premises: 1. Whether degradation applies to traps (read the degradation model's item classes). 2. Whether the zoonosis bridge models uncooked-yield exposure explicitly (if not, the premium column reports NOT MODELED and the gap is a finding).

## FP-C11 — Difficulty Preset Spread Audit (post-W1)

Lane C · C16 · Status PROPOSAL, sequence-gated.
Subject: audit that preset scalars produce distinct, legible difficulty curves rather than uniform multipliers, once W1 seals.
Premise evidence: VERIFIED W1 ACTIVE (DR-06); the B-23/G-08 binding wave is the prerequisite; the audit is its verification.
Route: harness, post-seal. Determinism: NONE (reads authored scalars).
Method: per preset, plot the effective curve for the bound consumer surfaces (industrial, medical, expedition at minimum); flag presets that are scalar multiples of each other (indistinguishable) and presets whose curves cross (legibility defects).
Verification: curve table published; indistinguishable-preset findings double-checked across all bound surfaces before publication.
Open premises: 1. W1 seal state. 2. The bound consumer inventory at audit time (B-23's closeout is the input list).

## FP-C12 — Greenhouse/Aeroponics Yield Economics

Lane C · C3 · Status PROPOSAL.
Subject: crop yield value versus power, water, and nutrient inputs across the three cultivation families (greenhouse, hydroponics, aeroponics).
Premise evidence: VERIFIED all three catalogs live (DM-3); VERIFIED the F-012 audit covers dive-site/hydroponic premise sweep — this plan is its economy leg.
Route: harness. Determinism: NONE expected (static input/output accounting).
Method: per cultivar, input cost at current prices versus yield value at baseline; dominated cultivars (same or higher input, strictly lower value than an alternative in the same family) flagged with the domination margin.
Verification: per-cultivar table; one cultivar hand-checked per family.
Open premises: 1. Nutrient input pricing (whether nutrients are priced goods or abstract costs — read the catalogs). 2. Power cost accounting (grid marginal cost model, if one exists; if not, state its absence).

## FP-C13 — Quantity-Band Trim Extension (evidence-gated)

Lane C · C11/C5 · Status PROPOSAL, gated on FP-C03's report.
Subject: where E[value] outliers persist after FP-C03's re-run, extend quantity-band trims — the established remedy class — on the demonstrated outliers only.
Premise evidence: VERIFIED band trims are the canon remedy (v1.0 Lane C); the gate is FP-C03's published outlier table, without which this plan has no subject.
Route: DATA-ONLY after evidence. Determinism: NONE (band values are authored).
Continuity: each trim names the outlier row it remedies and the report that demonstrated it; no trim without a cited number; FP-C03 re-runs after each trim batch to confirm the delta.
Verification: FP-C03 re-run delta; integrity selftest on the trimmed tables.
Open premises: none beyond the gate; if FP-C03 finds no persistent outliers, this plan closes with zero trims, which is a valid outcome.

## FP-C14 — Time-to-Kill and Combat Economy Audit

Lane C · Cross · Status PROPOSAL.
Subject: `combat_catalog.json` damage and armor cadence versus ammunition scarcity across difficulty presets — time-to-kill against expected ammunition economy.
Premise evidence: VERIFIED the combat catalog and hardcore tuning live (DR-03); VERIFIED ammunition scarcity is a canon pressure; preset limitation per the lane preamble until W1 seals.
Route: harness. Determinism: NONE expected (deterministic combat math).
Method: per weapon/armor pairing, time-to-kill and shots-to-kill at catalog values; against per-encounter expected ammunition expenditure, flag pairings where the ammunition cost of a standard engagement exceeds plausible income (combat is a losing economy) or where scarcity is never binding (combat is free) — both are legibility findings, and the intended harshness target must be stated as an assumption since no design target is authored.
Verification: pairing table published; one engagement hand-checked.
Open premises: 1. The armor penetration model's exact formula (read before computing). 2. Encounter ammunition expenditure model (read the encounter system's consumption path; if not modeled, report NOT MODELED as a finding).

## Volume 21 sequencing note

Lane B: FP-B04 and FP-B15 are DR-20-gated (ChildDevelopmentSystem inventory; the evidence vocabulary check respectively); FP-B14 may run its premise sweep now; FP-B06 waits on the W1 seal. Lane C: FP-C03, FP-C04, FP-C08, FP-C12 have no gates and may run first; FP-C01/C02/C09 are preset-limited but publishable with stated limitations; FP-C05/C06/C07 follow their premise reads; FP-C13 strictly follows FP-C03; FP-C11 strictly follows the W1 seal. No Lane C plan ships a number change in its first tranche — the lane rule.

---

# VOLUME 22 — PROSE AND DOCUMENT CONTRACT SUPPLEMENT, MERGED-SURFACE EDITION (Factory batch 2026-09-27-I)

This volume extends the Part 9 contract library for document genres whose owning surfaces the public record shows landed on main (DR-16, DR-21) or that the repository's documentation constitution demonstrably requires (the claims ledger, the version policy, the agent-skills index). Standing rule unchanged: a contract is only authored here when its consuming surface exists; DR-19's flagship families (cultural archive, summit protocol) remain uncontracted because their carrier PR's merge state is still unverified — their Volume 18 replenishment seeds stay gated, and no contract is written for an unconfirmed surface.

## 22.1 Shelter-atmosphere reading contract (new genre; owning surface merged-as-listed per DR-21)

```text
prose_field: atmosphere_reading
purpose: legible rendering of shelter atmosphere state (quality, strain, contamination) in the shelter-operations register
trigger: atmosphere state crossing a legibility threshold (per the Plan 220 surface's own states)
length: 30-70 words
must_include: one observable condition, one cause consistent with the owning systems, one implied consequence the systems actually model
must_not_include: numeric telemetry in prose (panels carry numbers; prose carries legibility), any atmosphere effect the atmosphere system does not produce
model:
  The air in the east corridor has gone flat and faintly metallic since the
  filtration room lost its second blower. Breathing is still free; thinking
  is still possible; both are noticed less by the people who live there and
  more by the ones who visit. The service slate by the door has begun
  receiving complaints in handwriting the complainers did not use to have.
```

Validation notes: the reading annotates implemented atmosphere state and never invents a contaminant class; the words-not-color rule of the a11y canon applies — the reading itself is the non-color state signal; if the atmosphere panel already renders a textual state, the reading complements rather than duplicates it (B-24's truth rule: one authority, one voice).

## 22.2 Shelter-noise complaint slip contract (new genre; owning surface merged-as-listed per DR-21)

```text
prose_field: noise_complaint
purpose: deadpan institutional record of noise-discipline state (Plan 205 surface)
trigger: noise threshold events the discipline system records
length: 20-60 words
must_include: the noise source (a real, system-recognized source), the hour class, the disposition
must_not_include: sound-effect description the noise system does not model; humor at the expense of a named survivor who did not consent by canon (named complainants reuse catalog ids)
model:
  Complaint received, third watch: generator housing, loose panel, the
  sound of a decision being unmade every forty seconds. Disposition: panel
  refastened by the complainant, who declined to sign the slip and requested
  that the slip record the refusal. The slip records the refusal.
```

Validation notes: sources must be system-recognized (a generator, a pump, a social event in the schedule catalog); the deadpan register of 4.8 applies; complaint disposition must map to a state the discipline system can hold (resolved, deferred, escalated — read the implemented vocabulary).

## 22.3 Release-craft documentation contract (owning surface merged-as-listed per DR-16)

The version policy (`ReleaseVersion`, `VERSIONING.md`, the claims ledger) is now a documentation surface with its own invariants, which future plans must respect:

1. Version claims in any plan or closeout state the parser's classification (the strict-semver vocabulary), never an informal version adjective.
2. The three-axis policy governs: any plan that changes save compatibility, public behavior, or the support window must state which axis moves and therefore which bump class the release requires; a plan that cannot say is not ready to plan.
3. `CLAIMS.json` is generated-and-verified, not hand-maintained: a plan claiming a capability moves its claim to TRUE/PROVEN_INTEGRATION only through the verified-path discipline the ledger encodes; a plan may cite a claim's state but never edit it.
4. The support window claim is release-pinned: D-05's re-pin seed executes against a named release, and its fixture evidence lands with that release's closeout, not before.
5. No plan may quote a gate name that the live gate inventory does not contain — the stale `release_fixture_matrix` reference removed per DR-16 is the standing example of this defect class.

## 22.4 Agent-skills index documentation contract (owning surface merged-as-listed per DR-16, regenerated at 35 skills)

The regenerated `AGENT_SKILLS_INDEX.md` and the release-captain skill establish a documentation genre the factory must treat as a live surface:

1. The index is generated: plans that add, retire, or rename a skill update the generator's input, never the index by hand.
2. Skill vocabulary drift is a defect class with a merged precedent (the stale "bit lane/snap" vocabulary removed per DR-16); any plan touching release ceremony must use the current script vocabulary (`prepare-release.sh`, `release-gate.sh` as listed).
3. The factory's own I-06 session playbook, if authored, registers in the same index family rather than standing alone — existing-implementation-first applies to documentation surfaces as much as to code.

## 22.5 Commitment-event authoring rule (owning surface merged-as-listed per DR-22, DR-23 corroboration)

The Plan 31/38 semantic-parity incident and its merged repair establish a binding authoring rule for every future plan that emits or regroups events:

1. Every event kind carries semantic registration at authoring time; an unregistered emission is a suite-regression defect with a merged precedent, not a stylistic concern.
2. DP-04's semantic-kind regrouping, if signed, includes a parity table: old kinds, new kinds, and the registration mapping between them, reviewed in the same tranche as the regrouping.
3. Port-contract DEFERRED classifications may not be cited from memory: DR-23 shows the ratchet moves, and any plan citing a seam's DEFERRED state reads the live contract matrix first.

## 22.6 Contract discipline note

These five contracts close the volume deliberately short: the DR-19 flagship domains (cultural-archive restoration logs, summit protocols, salon chronicles, acetate disc manifests) have clear genre shapes sketched in the Volume 18 replenishment seeds, but the factory's constitution forbids authoring a contract for a surface it cannot confirm exists on main. When RB-DRCONFIRM (Volume 20, runbook 20.8) confirms the carrier, the next supplement volume writes those contracts with the same worked-model discipline — and not before.

---

# VOLUME 23 — WORKED WAVE CHARTER BANK, ASH-EXP-3 THROUGH ASH-EXP-8 (Factory batch 2026-09-27-J)

Six further worked charters extending the Volume 14 pair, each ready for foreman authorization and each built only from factory-verified or merged-as-listed surfaces, with every DR-gated dependency named in the charter's own gates. Charters are proposals: none schedules a sealed surface, a GATE item, or a working-tree-unconfirmed surface without its named gate passing first. Each charter follows Template W and the RB-WAVE checklist.

## Wave Charter — ASH-EXP-3: "Paper Before Pressure" (Lane A census wave)

Domain: the four census-gated prose plans whose Tranche 0 censi unlock everything downstream. Non-goals: no authoring beyond census publication; no Lane B work; no number changes.
Lane allocation: Lane A only — FP-A24 (bureaucratic prose census), FP-A25 Tranche 0 (the 200-record census), FP-A29 (bestiary thin-entry census), FP-A30 (sky-defense coverage census, after its collision sweep).
Flagship: FP-A25 Tranche 0. Satellites: the other three censi. Shared seams: none — four different catalogs, which is what makes a four-plan census wave safe.
Verification matrix: per plan, the census publication itself is the acceptance (RB-CENSUS steps 1–7), plus the pre-authoring integrity selftest baseline. No focused tests yet — censi do not change code or data.
Closeout discipline: the four census tables published in the plan docs; tranche boundaries proposed; the wave's ledger records census gap counts as its headline numbers.
Gates: FP-A30's collision sweep (RB-COLLISION) must pass first; FP-A24/A-25 field-name confirmation in session.
Why this charter: every prose tranche downstream is blind without these censi; the wave is cheap, parallel, and produces the factory's first measured gap counts.

## Wave Charter — ASH-EXP-4: "The First Tranches" (Lane A authoring wave)

Domain: the first authoring tranches from ASH-EXP-3's census tables, one per family group. Non-goals: no census-gated plan whose census found zero gaps; no second tranche of any family in this wave.
Lane allocation: Lane A — up to four tranche plans, one per censused family, each capped at its census-sized batch.
Flagship: FP-A25 Tranche 1 (the first 200-record quest tranche, the largest single authoring batch). Satellites: FP-A24's completion tranche, FP-A29's first sighting-log tranche, FP-A15's marginalia tranche if its census (from ASH-EXP-3's sibling set) supports it.
Verification matrix: RB-PAIR pairing tests per tranche; integrity selftest; per-tranche prose-coverage assertions against the tranche's record ids; FP-A20-style seal guards where any radio-adjacent genre enters (none scheduled).
Closeout discipline: per-tranche closeout with the cumulative completed-record count published; the one-quest-tranche-per-wave rule enforced (Tranche 1 only).
Gates: ASH-EXP-3 complete; each tranche's census table exists and shows its gap set.
Why this charter: converts measured gaps into verified prose with the smallest safe batches, and exercises the RB-PAIR pattern on four different catalogs in one wave, proving the pattern generalizes.

## Wave Charter — ASH-EXP-5: "One Standing Authority" (Lane B/C7 bridges)

Domain: the faction-standing bridges that route through the sole standing authority. Non-goals: no GATE items (DP-02, DP-03 stay signature-blocked); no war-chain emitter work; no economy number changes.
Lane allocation: Lane B — FP-B14 (belief-movement stance bridge) as flagship; its companion premise reads (the stance effect-kind vocabulary check, the movement-faction pair enumeration) as satellite steps, not separate plans.
Flagship: FP-B14. Satellites: none on shared seams — the stance engine is the single seam and the charter deliberately runs one plan against it.
Verification matrix: the mapping-table focused tests; the deterministic conversion fixture with the standing delta asserted; the G-01-style consumer check; DR-22's semantic-parity registration for any new event kind, in-tranche.
Closeout discipline: mapping table published with the closeout; information-flow legality documented per pair (the gossip lag named where B-17 would later deepen it).
Gates: the evidence-vocabulary check (FP-B15's, shared); the effect-kind vocabulary confirmation; no working-tree-unconfirmed surface consumed.
Why this charter: proves the bridge pattern on the repository's most invariant-constrained engine while the GATE items wait for signature — the factory's discipline is most credible where the temptation to exceed scope is highest.

## Wave Charter — ASH-EXP-6: "Measure Everything" (Lane C harness wave)

Domain: the ungated Lane C harnesses, run in one wave so the balance ledger publishes once. Non-goals: no tuning tranches (FP-C13 explicitly excluded); no preset-dependent conclusions until W1 seals — the limitation is stated in every report header.
Lane allocation: Lane C — FP-C03 (scavenging re-run), FP-C04 (vehicle dominance), FP-C08 (black-market tiers), FP-C12 (cultivation economics) as the four ungated harnesses; FP-C01, FP-C02, FP-C09 run in the same wave with preset limitations stated (six plans, four catalogs' seams plus the simulation harness — the shared seam is the harness itself, so the five-concurrent cap is respected by running the preset-limited three sequentially after the four).
Flagship: FP-C09 (winter compression — the report B-19's premise depends on). Satellites: the other five.
Verification matrix: two-pass determinism proofs for every seeded harness; published reports per plan; one hand-reconciliation per report; FP-C03's outlier table explicitly marked as FP-C13's evidence gate.
Closeout discipline: all six reports land in the established balance-report location in the same session; the wave closeout publishes the combined findings table with mathematical-issue versus design-preference labels per the Category 7 discipline.
Gates: none for the four ungated harnesses; the three preset-limited plans state the W1 limitation in their headers rather than waiting (publishing with stated limitation beats silent delay).
Why this charter: the entire lane's first tranche is measurement; one wave, one ledger update, six published reports, zero number changes — the Category 4 golden rule exercised at wave scale.

## Wave Charter — ASH-EXP-7: "Register the Present" (Lane I documentation wave)

Domain: documentation contracts for merged-as-listed surfaces, plus the authority-map gaps DR-20 exposed. Non-goals: no contract for unconfirmed surfaces (DR-19 families excluded); no code; no hand-maintained generated files.
Lane allocation: Lane I — I-01's authority-map gap registry extended with the DR-20 subsystem inventory (working-tree-confirmed only); the 22.3/22.4 documentation contracts instantiated as real docs where their surfaces confirm; I-05's agent-rules de-duplication note updated against the regenerated skills index.
Flagship: the DR-20 confirmation-fed authority map (the deep-map corrections RB-DRCONFIRM produces). Satellites: the release-craft and skills-index documentation instantiations.
Verification matrix: docs-only verification — the index drift gate family, the documentation selftest surface where it covers docs, and the Part VI rule that documentation describes reality (each doc's claims cross-checked against the working tree in the same session).
Closeout discipline: the corrected deep maps published as the wave's closeout artifact; retired DR candidates recorded, not silently dropped.
Gates: RB-DRCONFIRM complete before any DR-20-derived line is written.
Why this charter: the re-audit found the deep maps materially incomplete; correcting the factory's own reference layer is the highest-value documentation work available, and it must precede any plan that enumerates systems.

## Wave Charter — ASH-EXP-8: "Signature Sunday" (decision-packet consumption wave, conditional)

Domain: the three Volume 12 decision packets, executed only if their signatures exist at wave time. Non-goals: everything, if the signatures do not exist — this charter's correct execution with zero signatures is a no-op that records "no warranted work" and closes.
Lane allocation: DP-01 (flood tags, Lane B/C6), DP-02 (per-strike emitters, Lane B/C7), DP-03 (funds legs, Lane B/C11) — three lanes, no shared seams beyond the standing/economy owners, but each packet's own annotation gates apply: DP-02 names its rendering surface against DR-16's board (if confirmed), DP-04's parity table rides the DR-22 rule if it is signed in the same act.
Verification matrix: per packet, its own verification class from Volume 12, plus DR-22's semantic registration for any new event kind, plus the G-05/G-02 pattern tests each packet's closeout names.
Closeout discipline: each executed packet's closeout cites the signature, the scope, and the seals respected; unexecuted packets are recorded as still-blocked with their signature holders named.
Gates: the signatures themselves, plus DR-16's working-tree confirmation for DP-02's board annotation.
Why this charter: the packets were authored so one signature act unblocks each in one wave; the charter keeps that promise honest by making the zero-signature case an explicit, recorded outcome rather than silent idling.

## 23.7 Charter bank sequencing

Recommended order: ASH-EXP-7 first (the deep-map corrections gate every enumeration downstream), then ASH-EXP-3, then ASH-EXP-4, with ASH-EXP-6 runnable in parallel with the Lane A waves (different lanes, different seams), ASH-EXP-5 after ASH-EXP-7's confirmation pass, and ASH-EXP-8 whenever its signatures exist — it is deliberately unschedulable, which is the point of a signature gate. ASH-EXP-1 and ASH-EXP-2 (Volume 14) slot before ASH-EXP-3 if the foreman prefers the original narrative-then-economy order; the bank's only hard dependency is ASH-EXP-7 before any system-enumerating plan.

---

# VOLUME 24 — REPLENISHMENT SEED CATALOG FROM THE DR-20 SUBSYSTEM INVENTORY (Factory batch 2026-09-27-K)

The DR-20 listing names Core systems absent from the factory's deep maps. Under the Part VI replenishment rotation this inventory converts into candidate seeds — but every seed in this volume is doubly gated: first on RB-DRCONFIRM's working-tree confirmation that the named system exists on main with the described scope, and second on the standing premise sweep. A seed whose system fails confirmation is retired with its retirement recorded. Seeds follow the Volume 2/3 compressed format.

## 24.1 Lane A replenishment (prose surfaces)

**A-33 · C9 · Memory-decay document twins.** Subject: prose conditioned on `MemoryDecaySystem` state — fading recollection entries, contested remembrances between survivors whose memories diverge. Route: DATA-ONLY. Confidence: HYPOTHESIS pending DR-20 confirmation and a consumer-path read (what surface displays decayed memory today).

**A-34 · C9 · Time-capsule deposit and recovery documents.** Subject: deposit manifests and recovery debriefs for `TimeCapsuleSystem` capsules, in the manifest (4.1) and dispatch/debrief (5.7) registers. Route: DATA-ONLY. Confidence: HYPOTHESIS pending confirmation; the capsule's persistence semantics (what survives, for how long) must be read before any deposit prose states a duration.

**A-35 · C8 · Propaganda leaflet corpus.** Subject: leaflet and poster text for `PropagandaSystem` effects, in faction public/private registers separated exactly as the system separates them. Route: DATA-ONLY. Confidence: HYPOTHESIS pending confirmation; leaflet effects must map to implemented morale/standing effects only.

**A-36 · Cross · Item-lore inscription twins.** Subject: `ItemLoreSystem`-conditioned inspection prose extending the item-inspection contract (5.19) to lore-bearing items. Route: DATA-ONLY. Confidence: HYPOTHESIS pending confirmation of the lore system's item-conditioning surface.

**A-37 · C9 · Hidden-agenda confrontation prose.** Subject: discovery and confrontation beats for `HiddenAgendaSystem` states, in the relationship-reaction register (5.20). Route: DATA-ONLY. Confidence: HYPOTHESIS pending confirmation; agenda reveal timing obeys information-flow legality strictly.

**A-38 · C9 · Hobby and interpersonal-conflict texture.** Subject: small-document texture (schedule slates, complaint slips in the 22.2 register, shared-space notices) for `HobbySystem` and `InterpersonalConflictSystem` states. Route: DATA-ONLY. Confidence: HYPOTHESIS pending confirmation; conflicts must resolve only through implemented reconciliation paths.

**A-39 · C1 · Shelter-archive accession records.** Subject: accession and cataloguing records for `ShelterArchiveSystem` — what the shelter chooses to keep, in the dossier register (5.15). Route: DATA-ONLY. Confidence: HYPOTHESIS pending confirmation of the archive's item surface.

**A-40 · C8/C11 · Rumor-system distortion chain prose.** Subject: stage-by-stage distortion examples for `RumorSystem` propagation — the 5.17 rumor contract's model applied to the live rumor bands, showing one fact degrading across hops. Route: DATA-ONLY. Confidence: HYPOTHESIS pending confirmation that the rumor system's stages match the sealed band semantics.

## 24.2 Lane B replenishment (mechanics)

**B-27 · C6 · Cartography-system discovery conditioning.** Subject: audit whether `CartographySystem` discovery state conditions travel-encounter and map-gate selection; bridge only on a measured gap, through existing owners. Route: audit then CORE-EXTENSION. Confidence: HYPOTHESIS pending confirmation.

**B-28 · C3/C11 · Resource-rationing scarcity seams.** Subject: audit `ResourceRationingSystem`'s interactions with the nutrition and market owners for double-counting (a ration and a market purchase both consuming the same stock would be a correctness defect, not a feature). Route: audit then, on evidence, CORE-EXTENSION. Confidence: HYPOTHESIS pending confirmation.

**B-29 · C10 · Dynamic-quest-generator constraint audit.** Subject: verify `DynamicQuestGenerator` output respects the quest record contract (7.2) and the reward/failure/recovery grammar — generated quests held to the same authoring standard as authored ones, with a validator if none applies. Route: audit then tests (Lane G shape). Confidence: HYPOTHESIS pending confirmation.

**B-30 · C9 · Exercise and child-development skill seams.** Subject: audit whether `ExerciseSystem` and `ChildDevelopmentSystem` state flows into the skills/needs owners they imply, through cohort links where children are involved (FP-B04's premise read overlaps; one seam, one wave, per the cross-cluster rules). Route: audit then CORE-EXTENSION on evidence. Confidence: HYPOTHESIS pending confirmation and FP-B04's inventory.

**B-31 · C1 · Shelter-security perimeter state seam.** Subject: audit `ShelterSecuritySystem` against the airlock-security and defense-grid owners for boundary overlap — two security authorities acting on the same perimeter would be a multiple-sources-of-truth finding. Route: audit; report; extension only on a demonstrated gap. Confidence: HYPOTHESIS pending confirmation.

**B-32 · C13 · Discovery-consequence evidence enrollment.** Subject: audit whether `DiscoveryConsequenceSystem` consequences enroll as Reckoning evidence classes (B-20's sweep should have covered it if the system predates the sweep; DR-20's dating suggests it may postdate — read the landing wave). Route: audit then enrollment through endgame owners. Confidence: HYPOTHESIS pending confirmation and wave dating.

## 24.3 Lane E/G/J replenishment

**E-12 · C17 · Panel coverage for DR-20 subsystems.** Subject: enumerate which confirmed DR-20 systems produce player-relevant state with no briefing row, snapshot, or panel — the E-01/E-02 class applied to the newest systems before they age into the B-24 sweep's backlog. Route: HOST-WIRING, zero authority. Confidence: HYPOTHESIS pending confirmation.

**G-10 · Cross · DR-20 system smoke tests.** Subject: scene-load and focused smoke coverage for each confirmed DR-20 system that shipped without its own test wave (the PR #66 pattern — focused tests per landed plan — is the standard; the audit names the untested). Route: tests. Confidence: HYPOTHESIS pending confirmation and a test-inventory read.

**J-06 · C16 · Onboarding rows for DR-20 systems.** Subject: onboarding entries for confirmed DR-20 systems that produce first-week player-visible state (rationing, hobbies, conflicts especially). Route: DATA-ONLY plus host wiring through the existing onboarding seams. Confidence: HYPOTHESIS pending confirmation.

## 24.4 Replenishment rules for this catalog

1. Zero seeds in this volume enter a wave charter before RB-DRCONFIRM publishes its confirmation table; the catalog is a pipeline, not a backlog with standing rights.
2. Each confirmed seed's first session is a premise sweep, not authoring: consumer paths, persistence, and vocabulary checks precede any plan.
3. Retired seeds (system absent, renamed, or already covered) are recorded in the drift register's next refresh with their retirement reason — the factory's anti-hallucination rule applies to its own replenishment.
4. The catalog deliberately contains no Lane C, D, F, H, or I seeds: those lanes' next replenishment waits on the confirmation pass's economics, persistence, profiling, tooling, and documentation findings respectively; pre-filling them would be invention, not replenishment.

---

---

# VOLUME 25 — RB-DRCONFIRM EXECUTION: WORKING-TREE CONFIRMATION TABLE (Factory batch 2026-09-24-D)

This volume executes runbook RB-DRCONFIRM (Volume 20, section 20.8) against the live repository. This session had genuine read access to the working tree through the repository host API: directory listings, code search, and file-content reads were performed live on 2026-09-24. Per the runbook's own discipline (section 20.9), the runbook was not modified by this session; deviations are recorded in section 25.7. Every row below carries its evidence class. The volume's conclusions gate the entire DR-20 pipeline per the ASH-EXP-7 charter.

## 25.1 Session evidence base

The following reads were performed live (all 2026-09-24, default branch):

- VERIFIED: repository root listing — 69 entries, including `AGENTS.md` (10,573 bytes), `INTEGRATION_PLANS.md` (78,528 bytes — materially larger than the 32,793-character read recorded in DR-06's audit), `KNOWN_DEBT.md` (29,152), `WORKTREE_OWNERSHIP.md` (97,960), `POTENTIALCLUTTER.md` (40,847), `sources.md` (50,940), `TEST_POLICY.md` (2,136), `SESSION_HANDOFF.md` (6,948), `WAVE9_PART1_CLOSEOUT.md` (10,352), `A1_BRIEFING_DEFERRED.md` (3,775), `A1_COORDINATION_RECORD.md` (1,933), plus `Next-steps-plans/`, `piagentsplans/`, `Seal-steps/`, `semantic-review/`, `C-integration-plans/`, `Ashfall.Core.Tests/`, `Ashfall.Core/`, `tests/`, `snapshots/`, `tools/`, `artifacts/`, `assets/`, `docs/`, `scenes/`, `scripts/`, `src/`.
- VERIFIED: `Assets/Ashfall.Core/` listing — 226 entries (top level), with subdirectory inventories read for `Cognition/`, `Recreation/`, `Needs/`, `Survivors/` (57 entries), `Shelter/` (76 entries), `Exploration/`, `InformationFlow/`, `Narrative/` (100+ entries), `Journal/`, `Content/`, `Quests/`, `Culture/`, `Events/`, `Collectibles/`, `Lifecycle/`, `Governance/`, `Institutions/`, `Feedback/`, `Onboarding/`, `Codex/`, `Spiritual/`, `Economy/`, `Expeditions/`, `Inventory/`.
- VERIFIED: `src/` listing — the Host partial surface (`Main.*.cs`, including `Main.Plans163_210.cs`, `Main.Plans167_219.cs`, `Main.Plans216_202Interpersonal.cs`, `Main.ShelterAtmosphere.cs`, `Main.HydraulicExtrusion.cs`, `Main.LowBackgroundMetrology.cs`, and the `Main.UiTests.*` family), plus `Host/`, `Audio/`, `Disease/`, `Dose/`, `Economy/`, `Foundry/`, `Journal/`, `Localization/`, `Muster/`, `Radio/`, `Settings/`, `UI/`, `UtilityAI/`, `VerdictPanel.cs`, `World/`, `YearOfAsh/`.
- VERIFIED: `scripts/` and `scripts/ci/` listings — 58 CI scripts including `verify-fast.sh`, `run-gates.py`, `content-acceptance-gate.sh`, `detect-corpus-duplicates.py`, `verify-capability-claims.py`, `generate-selftest-manifest.py`, `version-gate.py`, `coverage-gate.sh`, `scene-lint.py`, `l10n_drift_gate.py`, `plan_governance_config.json`, `quarantine.json`, and the generator family (`generate-architecture-map`, `generate-core-systems-catalog`, `generate-save-store-matrix`, `generate-ui-panel-catalog`, `generate-port-contract`, `generate-plan-register`, `generate-agent-skills-catalog`, and others). Full inventory in section 25.8.
- VERIFIED: `docs/ci/CI_GATE_MANIFEST.json` — read live: `schema_version` "1.1.0", `total_gates` 57, `fast_tier_count` 53, with per-gate entries carrying `gate_id`, `name`, `category`, `command`, `timeout_seconds`, `expected_summary`, `classification`, `critical`.
- VERIFIED: `Assets/StreamingAssets/Data/` — 342 entries: 338 `.json` catalogs, exactly one non-JSON file (`rewrite.py`), and three directories (`documents/`, `narrative/`, `whitelists/`).
- VERIFIED: `docs/` listing — 153 entries, 86 subdirectories (amended drift in section 25.5). `docs/plans/` — 126 entries with wave directories `flagship_b5_b8/`, `wave8_part2/`, `wave9_part2/`, `wave10_part1/`, `wave10_part2/`, `wave11_part1/`, `wave11_part2/`, `wave12_part1_1/`, `xp/`.
- VERIFIED: `docs/plans/UNCLAIMED_CORPUS_CENSUS.md` — read in full (32,793 characters). Its true nature is materially different from the factory's standing premise; see section 25.5, DR-27.
- VERIFIED: code search across the repository for `ResourceRationing`, `DiscoveryConsequence`, `ItemLore`, `BeliefMovement`, and `rewrite.py` (call-site evidence; section 25.3 and DR-05 update).
- VERIFIED: quest-catalog size inventory and a fragment census of `quests_massive_expansion_200.json` (Volume 26 executes this).

What this session did NOT read: the interiors of `INTEGRATION_PLANS.md` (78,528 bytes — only its size is now verified), the full per-gate body of `CI_GATE_MANIFEST.json` beyond its header and first entries, individual Core source files' internals, and the remaining DR-16/DR-17/DR-18 carrier documents. Rows depending on those reads are labeled PENDING with their exact verification step, per the runbook's rule 4.

## 25.2 DR-07 RESOLVED: the gate-count authority is the live manifest

The runbook asks for working-tree confirmation of drift-register claims. DR-07 (gate-count and test-total drift) is now resolved against a live authority:

- VERIFIED: `docs/ci/CI_GATE_MANIFEST.json` declares `schema_version` "1.1.0", `total_gates` 57, `fast_tier_count` 53, and self-documents its own recent history: "Plan 48 / C2[21] Phase 2: added version_gate and changelog_drift (fast, +2 gates); Phase 3: added save_support_window (full, +1 gate)".
- The v1.0 bible's "57 CI gates at v1.1.0 (53 fast + 3 full + 1 performance)" is CONFIRMED as the live manifest state at schema 1.1.0. The closeout-recorded "ALL 47 GATES PASSED" is therefore either an earlier instant (before the +2/+1 additions and any re-classification) or a subset run; both are consistent with a manifest that grew from 47 to 57 observed entries across waves. No contradiction remains once the manifest is treated as the single moving authority.
- Factory rule (unchanged, now with live proof): any subject plan naming a gate count cites `CI_GATE_MANIFEST.json`'s `schema_version` and `total_gates` at drafting time. SB-08 (gate-count drift guard) is UPGRADED from PROPOSAL to HIGH CONFIDENCE: the comparison key exists — `schema_version` + `total_gates` + `fast_tier_count` against any documented count — and the guard's design input (the manifest) is now read.

## 25.3 DR-20 CONFIRMED: the subsystem inventory table

The runbook's step 3 asked for present/absent/renamed per named system in the Core authority. Executed live. Every DR-20-named system is CONFIRMED PRESENT, with exact locations (a structural correction to the runbook's own path premise is recorded in section 25.4):

| DR-20 named system | Live status | Verified path |
|---|---|---|
| MemoryDecaySystem | PRESENT | `Assets/Ashfall.Core/Cognition/MemoryDecaySystem.cs` |
| ChildDevelopmentSystem | PRESENT | `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` |
| ExerciseSystem | PRESENT | `Assets/Ashfall.Core/Survivors/ExerciseSystem.cs` |
| HiddenAgendaSystem | PRESENT | `Assets/Ashfall.Core/Survivors/HiddenAgendaSystem.cs` |
| HobbySystem | PRESENT | `Assets/Ashfall.Core/Survivors/HobbySystem.cs` |
| InterpersonalConflictSystem | PRESENT | `Assets/Ashfall.Core/Survivors/InterpersonalConflictSystem.cs` |
| MoraleContagionSystem | PRESENT | `Assets/Ashfall.Core/Survivors/MoraleContagionSystem.cs` (plus `MoraleContagionCatalog.cs`, `MoraleContagionSave.cs`) |
| ShelterArchiveSystem | PRESENT | `Assets/Ashfall.Core/Shelter/ShelterArchiveSystem.cs` |
| ShelterSecuritySystem | PRESENT | `Assets/Ashfall.Core/Shelter/ShelterSecuritySystem.cs` |
| ShelterAtmosphereSystem / ShelterNoiseSystem (DR-21) | PRESENT | `Assets/Ashfall.Core/Shelter/ShelterAtmosphereSystem.cs`, `ShelterNoiseSystem.cs`; plus `AtmosphereTextSystem.cs`, `AtmosphereCatalogLoader.cs`, `EnvironmentalTextSystem.cs`, `EnvironmentalTextCatalogLoader.cs` at Core top level |
| CartographySystem | PRESENT | `Assets/Ashfall.Core/Exploration/CartographySystem.cs` |
| RumorSystem | PRESENT | `Assets/Ashfall.Core/InformationFlow/RumorSystem.cs` |
| DynamicQuestGenerator | PRESENT | `Assets/Ashfall.Core/Quests/DynamicQuestGenerator.cs` (alongside `DynamicQuestlines.cs`, `QuestRuntimeCoordinator.cs`) |
| ResourceRationingSystem | PRESENT | `Assets/Ashfall.Core/Economy/ResourceRationingSystem.cs` |
| DiscoveryConsequenceSystem | PRESENT | `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` |
| ItemLoreSystem | PRESENT | `Assets/Ashfall.Core/Inventory/ItemLoreSystem.cs` |
| BeliefMovement (named as a system) | PRESENT AS SURFACE, NAME CORRECTED | No standalone `BeliefMovementSystem`; the belief-movement surface lives in the Spiritual family: `Assets/Ashfall.Core/Spiritual/SpiritualCatalogLoader.cs`, `SpiritualModels.cs`, `SpiritualMeaningCoordinator.cs`, tested by `Ashfall.Core.Tests/Spiritual/Plan30SpiritualWorldTests.cs` |

Additional working-tree facts the search surfaced, for the deep maps:

- VERIFIED: `ResourceRationingSystem` consumers include `src/Host/EconomyHostSession.cs` and `MarketSystem.cs`; its plan of record is `Next-steps-plans/Plan_215_Shelter_Resource_Rationing_Crisis_Management.md` (also present under `shipped_to_chat/`), and it appears in `docs/architecture/PORT_CONTRACT.md`. B-28's audit premise (rationing × nutrition × market double-counting) has named, confirmed counterparties.
- VERIFIED: `DiscoveryConsequenceSystem` is consumed by `src/Main.Expeditions.cs`, `ExpeditionAggregate.cs`, `src/Host/ExpeditionHostSession.cs`; plan of record `Next-steps-plans/Plan_133_Expedition_Discovery_Persistent_World_Consequences.md`. B-32's enrollment question has its carrier.
- VERIFIED: `ItemLoreSystem` plan of record is `Next-steps-plans/Plan_190_Item_Lore_Provenance_Tracking.md`; a lore authority map exists at `docs/culture/PLAN_178_190_CREATION_LORE_AUTHORITY_MAP.md`. A-36's premise (an item-conditioned lore surface) is CONFIRMED as a live system with an existing authority map — the seed's open premise reduces to reading the map.
- VERIFIED: DR-19's cultural-archive domain is a live surface: `Assets/Ashfall.Core/Culture/` contains `CulturalArchiveTomeCatalog.cs`, `CulturalArchiveVaultSystem.cs`, `ArchiveChronicleMilestones.cs`, `CultureCreationSystem.cs`, `DocumentationSystem.cs`. The Volume 22 deferral (no contract for unconfirmed surfaces) is now liftable for the cultural-archive domain specifically; the other three DR-19 families (summit protocols, salon chronicles, acetate disc manifests) remain PENDING until individually confirmed.

Consequence for the pipeline: all seventeen DR-20 replenishment candidates (Volume 18's five and Volume 24's twelve, overlap consolidated) pass their first gate. None is retired. The second gate (the standing premise sweep) remains, and each seed's first session is still a sweep, not authoring. Two candidates carry corrections: A-36 (authority map exists — read before drafting) and FP-B14/B-27's vocabulary (belief-movement vocabulary lives in `SpiritualModels.cs`, not a standalone system file).

## 25.4 Deep-map correction: where the Core authority actually lives

DR-20's carrier listing named `Assets/Ashfall.Core/` as the Core location; the runbook (20.8 step 3) repeats it. Confirmed live with a correction that matters for every future enumeration:

- VERIFIED: the repository root contains TWO Core-named locations. `Ashfall.Core/` at root holds exactly two files: `Ashfall.Core.csproj` and `.gdignore`. The engine-free Core source authority is `Assets/Ashfall.Core/` — 226 top-level entries spanning ~40 subsystem directories plus top-level systems and loaders. `Ashfall.Core.Tests/` at root is the xUnit surface (verified present; its subdirectory inventory was partially read — `Spiritual/Plan30SpiritualWorldTests.cs` confirmed via code search).
- Factory rule amendment (recorded, not silently applied): every future deep map and enumeration names `Assets/Ashfall.Core/` as the Core source authority and `Ashfall.Core/` (root) as its project definition. The v1.0/DR-20 wording "Assets/Ashfall.Core/" was correct; the runbook's shorthand "Core in Ashfall.Core/" is the ambiguity this entry retires.
- VERIFIED structural note for the cluster maps: the `Survivors/` directory (57 files) is the densest single-owner directory in the Core listing, holding the entire interiority family (needs, skills, trauma, guilt, caregiving, fitness, final wishes, memorials, relationships, zealotry, desperation, leadership, ration conflict, somatic flashbacks, trauma bonds, hidden agendas, hobbies, exercise, child development, interpersonal conflict). The `Shelter/` directory (76 files) holds the industrial and infrastructure families. `Narrative/` (100+ files) holds the catalog-and-projection pairs — the technical-plus-prose pattern (v1.0 Part 16.4) is visible as a directory-wide convention: nearly every `*Catalog.cs` has a paired `*Projection.cs` or consuming `*System.cs` in the same directory.

## 25.5 New and amended drift-register entries

**DR-24 (new) — Root artifacts beyond DR-01's register. VERIFIED.**
Live root contains artifacts DR-01 did not list: `AI_AGENT_WORKFLOW.md` (3,108), `ANTIGRAVITY.md` (10,441), `CHANGELOG.md` (5,663), `C1_COMPLETION.md` (6,251), `DESIGN.md` (3,238), `Directory.Build.props`, `README.md` (5,631), `LICENSE`, `Ashfall.slnx`, `Ashfall.csproj` (root-level project file distinct from the Core project), and directories `C-integration-plans/`, `.kiro/`, `.agents/`, `.claude/`, `artifacts/`, `assets/`, `Ashfall.Core/`, `Ashfall.Core.Tests/`. Notably `ANTIGRAVITY.md` joins the per-tool agent rulebook family (DR-09), and `C-integration-plans/` is a first-class plan-corpus directory the v1.0 docs map never registered. F-007's registration plan must absorb both.

**DR-25 (amendment to DR-02) — The docs tree has 86 subdirectories, not the ~57 DR-02 recorded. VERIFIED.**
New subdirectories observed live beyond DR-02's list include: `audio/`, `data/`, `debug/`, `qa/`, `radiation/`, `relationships/`, `releases/`, `remediation/`, `research/`, `roadmap/`, `save/`, `saves/`, `shelter/`, `skills/`, `social/`, `spiritual/`, `standing_record/`, `survivors/`, `systems/`, `technical/`, `telemetry/`, `testing/`, `tools/`, `ui/`, `utility_ai/`, `verdict/`, `visual/`, `water/`, `weather/`, `world/`, `year_of_ash/`, `quests/`, `progression/`, `psych/`, `quality-of-life implied by qa/`. The docs map (v1.0 Part 5.8) is now two expansion generations stale; I-01's authority-map gap registry absorbs this as its largest single input.

**DR-26 (new) — INTEGRATION_PLANS.md has more than doubled since the DR-06 audit read. VERIFIED (size only).**
DR-06 recorded a 32,793-character read. The live file is 78,528 bytes. Interior state unread this session; the ledger's batch state, decision-blocked list, and closeout records must be re-read before any plan cites them. This is DR-06's own warning ("re-read each session") demonstrated on the factory's own audit trail.

**DR-27 (new) — The unclaimed-corpus census is a sealing ledger, not an open unclaimed queue. VERIFIED. Premise correction for SB-10, F-010, and RB-CENSUS.**
Read in full: `UNCLAIMED_CORPUS_CENSUS.md` is titled "Unclaimed Corpus Census (Wave 10 Part 1 / Task A1)", dated 2026-09-17, scope `C-integration-plans/` (`C1[5–45]`, `C2[8–45]`, `D1[2–23]`, `E1[2–29]`), enumerating 131 corpus files, each with source baseline, mandatory order, status, and evidence. Tranche 1's statuses are SEALED with per-corpus test evidence; zero rows carry an UNCLAIMED status in the body read. Consequences: (a) SB-10/F-010's premise — "authored content no system consumes, tracked in the census" — describes the document's title and the factory's inference, not its verified content; (b) the census's actual open work is its own tranche 2 ("non-anchor per-clause audits"), which is a different, narrower seam than content wiring; (c) RB-CENSUS procedures that begin from this document must now treat it as the C-corpus sealing ledger and locate any genuinely unclaimed content by other instruments (`ContentUtilizationGate.cs` and the `Content/` scanner family in `Assets/Ashfall.Core/Content/` — VERIFIED live: `ContentUtilizationScanner.cs`, `ContentUtilizationGraph.cs`, `ContentUtilizationManifest.cs`, `ContentUtilizationInstrumentation.cs`, `ContentAcceptancePipeline.cs`). F-010 is re-scoped: from "census feed" to "utilization-gate-derived feed", with the census retained as historical evidence of the sealing pattern.

**DR-28 (new) — Quest-corpus size ordering contradicts the factory's "largest prose debt" premise. VERIFIED (sizes; fragment census in Volume 26).**
`quests_massive_expansion_200.json` is 223,249 bytes — but `moral_choice_quests_branching.json` is 339,862 bytes, `quests_faction_branching.json` is 194,389, `thirdonary_quests.json` is 143,059, `moral_choice_quests.json` is 141,249, `year_of_ash_questlines.json` is 140,283, and `moral_choice_quests_expansion.json` is 118,103. FP-A25's framing of the 200-record catalog as "the largest single prose debt surface in the data authority" is corrected: it is the largest single record count, not the largest catalog. The tranche program's priority argument survives on record-count grounds but must cite the corrected fact. Additionally, `quests_bureaucratic_morality.json` is 1,885 bytes containing exactly 2 records (verified by full read and parse) — FP-A24's premise of a broad bureaucratic prose-completion surface is retired; the plan's census step, correctly executed, closes FP-A24 with a documented no-change area (see Volume 26.5).

**DR-29 (new) — A new moral-choice catalog has landed since DR-04. VERIFIED.**
`moral_choice_faction_reactions.json` (14,988 bytes) is live and absent from DR-04's inventory and the v1.0 Part 5.6 seam documentation. It joins a moral-choice family of eight catalogs (chains, faction_reactions, flags, gossip, quests, quests_branching, quests_distress, quests_expansion — all verified live with sizes). F-001's duplication firewall now names all eight; any delayed-callback catalog (F-001) must collision-sweep against `moral_choice_faction_reactions.json` specifically, since faction reactions are the nearest existing neighbor to delayed consequence returns.

**DR-05 update — `rewrite.py` still present; no caller found in tracked source. VERIFIED with narrowed scope.**
The data authority contains exactly one non-JSON file: `rewrite.py` (342 entries = 338 JSON + `rewrite.py` + 3 directories). Code search across the repository for `rewrite.py` returns exactly one hit: `POTENTIALCLUTTER.md`. No script, CI gate, or document in the tracked tree invokes it. F-009's open premise ("verify what the script rewrites and who calls it") is now half-answered: there is no in-repo caller. The remaining verification is reading the script's own content (unread this session) and checking local/contributor workflows before relocation. Risk assessment improves: a script with no tracked callers is a weaker load-bearing risk than DR-05 feared, but `POTENTIALCLUTTER.md`'s mention suggests the clutter question already touches it — read both together in the owning session.

## 25.6 Retirements and premise corrections (runbook step 4)

Retired candidates: NONE. All DR-20-derived seeds pass confirmation.

Premise corrections published (not silent):

1. FP-A24 — retired in scope; the bureaucratic catalog is 2 records, both with authored prose (Volume 26.5 records the closure as a no-change area).
2. FP-A25 — corrected framing (record count vs. size) and a field-inventory correction (Volume 26.4: the visible record shape is `id`/`display_name`/`type`/`briefing`/`choices`, with no `objective_text` field in the fragment read; the prose contract must be re-derived from the catalog's actual fields before Tranche 1).
3. SB-10/F-010 — re-scoped per DR-27.
4. FP-B14 — vocabulary surface located: `SpiritualModels.cs` (DR-20 table).
5. A-36 — premise upgraded: the lore authority map exists (`docs/culture/PLAN_178_190_CREATION_LORE_AUTHORITY_MAP.md`); the seed's verification step reduces to reading it.
6. SB-08 — upgraded from PROPOSAL to HIGH CONFIDENCE (DR-07 resolution supplies the comparison key).

## 25.7 Runbook deviation log (per section 20.9)

- Deviation 1 (scope, justified): RB-DRCONFIRM step 1 asks for confirmation of every MERGED-AS-LISTED entry (DR-16, DR-17, DR-18) by reading the named files' contents. This session confirmed DR-21's and DR-19's cultural-archive surfaces via directory and code-search evidence, and DR-07 via the gate manifest, but did not open the DR-16/17/18 carrier documents. Those three entries remain PENDING with their verification step intact. Justification: session size; the DR-20 table (the pipeline's actual gate) was completed in full.
- Deviation 2 (runbook defect, recorded for future fix): RB-DRCONFIRM step 3 says "inventory each named system in `Assets/Ashfall.Core/`" — correct — but the factory's own DR-20 prose elsewhere says "Core in `Ashfall.Core/`", which at root holds only the project file. Future runbook editions must name the source authority unambiguously (section 25.4).
- No runbook text was modified in this session.

## 25.8 Verified `scripts/ci/` inventory (input for Volume 28 and SB-08)

Agent/gate runners: `agent-fast-verify.py`, `verify-fast.sh`, `run-gates.py`, `github-step-summary.py`, `release-gate.sh`, `export-build.sh`, `godot-export-linux.sh`, `run-godot-bounded.sh`. Data/content gates: `asset-decode-gate.py`, `asset-orphan-sweep.sh`, `audio-asset-gate.py`, `case-collision-gate.sh`, `catch-policy-gate.sh`, `content-acceptance-gate.sh`, `detect-corpus-duplicates.py`, `doc-link-gate.sh`, `forbidden-api-gate.sh`, `json-schema-policy-gate.py` (+`.sh`), `l10n_drift_gate.py`, `legacy-asset-path-gate.sh`, `legacy-reference-gate.sh`, `persistent-filename-gate.py` (+`.sh`), `scene-lint.py`, `triad-drift-gate.sh`, `uid-sidecar-gate.sh`, `verify-capability-claims.py`, `version-gate.py`, `warning-baseline-gate.sh`, `coverage-gate.sh`, `input-map-gate.sh`, `license-header-check.sh`, `lfs-health-check.sh`, `no-whitespace-churn.sh`, `nuget-dependency-gate.sh`, `git-object-inventory.sh`, `repo-hygiene-report.sh`, `godot-asset-gate.sh`. Generators: `extract_l10n_inventory.py`, `generate-agent-skills-catalog.py`, `generate-architecture-map.py` (+`.sh`), `generate-asset-registry.py`, `generate-audio-catalog.py`, `generate-breakthrough-matrix.py`, `generate-catalog-registry.py`, `generate-cli-catalog.sh`, `generate-collectibles-matrix.py`, `generate-core-systems-catalog.py`, `generate-docs-index.py`, `generate-expansions-catalog.py`, `generate-item-icons.py`, `generate-keyboard-map.py`, `generate-plan-register.py`, `generate-port-contract.py`, `generate-save-store-matrix.py` (+`.sh`), `generate-selftest-manifest.py`, `generate-ui-panel-catalog.py`, `normalize-doc-links.py`, `sync-agent-rulebooks.py`. Governance data: `plan_governance_config.json`, `quarantine.json`. Also verified: `scripts/audit_assets.py`, `scripts/composio_asset_pipeline.py`, `scripts/generate_item_icons.py`, `scripts/run_test.sh`, and directories `_dev_tools/`, `git-hooks/`, `maintenance/`, `pipeline/`, `release/`, `tools/`.

Two of these bear directly on standing backlog items: `detect-corpus-duplicates.py` (the duplication firewall is a runnable gate, not just a procedure — SB candidates must run it, not merely reason about collisions) and `verify-capability-claims.py` (a capability-claims gate exists; DR-16's claims-file confirmation should proceed through it).

---

# VOLUME 26 — LIVE-CENSUS INSTANTIATION, PARTIAL (Factory batch 2026-09-24-D)

Volume 25's confirmed access allows a genuine (partial) execution of the ASH-EXP-3 census wave's first steps. This volume publishes what was actually measured and explicitly bounds what was not. It executes the data-authority census in full, the quest-corpus Tranche-0 census in fragment form, and records FP-A24's closure as a no-change area.

## 26.1 Data-authority census (COMPLETE)

VERIFIED, complete: `Assets/StreamingAssets/Data/` = 342 entries.

- 338 JSON catalogs.
- Exactly one non-JSON file: `rewrite.py` (DR-05).
- Three directories: `documents/`, `narrative/`, `whitelists/`.

Census consequences: (a) the ID-collision sweep's denominator is now 338 named catalogs — every future subject plan's firewall step cites this count with its date; (b) the `whitelists/` directory was not in the v1.0 inventory's description of the data authority's shape — its contents (unread) are an open premise for any plan touching data-authority policy (the `persistent-filename-gate.py` and `json-schema-policy-gate.sh` pair plausibly consumes them; verify before citing).

## 26.2 Quest-corpus size census (COMPLETE for size, VERIFIED)

Verified catalog sizes in the quest and moral-choice families (bytes): `moral_choice_quests_branching.json` 339,862 · `quests_massive_expansion_200.json` 223,249 · `quests_faction_branching.json` 194,389 · `thirdonary_quests.json` 143,059 · `moral_choice_quests.json` 141,249 · `year_of_ash_questlines.json` 140,283 · `moral_choice_quests_expansion.json` 118,103 · `verdict_questlines.json` 85,668 · `questline_master.json` 66,388 · `duty_roster_quests.json` 45,967 · `holdfast_quests.json` 42,199 · `quests_npc_arcs.json` 52,267 · `standing_record_quests.json` 55,098 · `moral_choice_chains.json` 31,788 · `crossing_quests.json` 34,791 · `personal_quests.json` 32,368 · `narrative_questlines.json` 32,775 · `dose_quests.json` 32,196 · `quests_moral_branching_expansion.json` 36,716 · `quests_expansion_06.json` 55,220 · `quests_expansion_05.json` 96,587 · `moral_choice_gossip.json` 27,959 · `year_of_ash_quests.json` 27,315 · `repeatable_quests.json` 6,627 · `dynamic_questlines.json` 4,153 · `quest_templates.json` 2,307 · `moral_choice_quests_distress.json` 14,705 · `moral_choice_faction_reactions.json` 14,988 · `thirdonary in list above` · `quests_bureaucratic_morality.json` 1,885 · `moral_choice_flags.json` 2,090.

This is the first verified, complete size ordering of the quest corpus. The prose-debt surface, by volume, is the moral-choice branching family — a fact the factory's Lane A roadmap did not rank correctly before this census.

## 26.3 FP-A25 Tranche 0, fragment execution (PARTIAL — bounded honestly)

The 223,249-byte `quests_massive_expansion_200.json` could not be read in full this session (the fetch surface returned the first ~32,793 characters — approximately 15 percent of the file). Within that verified fragment:

- VERIFIED: the catalog's structure is `{ "schema_version": 1, "quests": [ ... ] }`.
- VERIFIED: 32 records were fully visible; each record's field shape is `id`, `display_name`, `type` (observed value: `"narrative_chain"`), `briefing` (authored prose, present on every visible record), and a `choices` array with structured option objects (ids observed in the `quest_disciplinary_iron_*` chain).
- VERIFIED: no `objective_text`, `quest_hook`, or `outcome_text` field appears in the visible fragment. The Part 9 prose-contract fields the factory assumed for this catalog are not its schema.
- HIGH CONFIDENCE (extrapolation, labeled): at the observed density (~32 records per ~32.8k characters, roughly 1,025 bytes per record), 200 records is consistent with the observed 223,249-byte size, implying the fragment is representative of record shape but not of prose depth in the later 85 percent.

Tranche 0's honest output is therefore a corrected procedure, not a completed table: the per-record prose-depth census must (a) read the catalog in segments against the actual field set (`briefing`, `display_name`, choice texts), (b) re-derive the prose contract from those fields rather than the Part 9 defaults, and (c) record per-record word counts for `briefing` and choice-facing text only. The tranche program's structure (Tranche 0 census, eight authoring tranches, completion regression) survives; its measurement instrument is corrected before any authoring is scheduled. This is the RB-QUANT discipline applied to its own premise.

## 26.4 Moral-choice family census (COMPLETE at listing level)

Eight catalogs verified live (sizes in 26.2): `chains`, `faction_reactions`, `flags`, `gossip`, `quests`, `quests_branching`, `quests_distress`, `quests_expansion`. The family has grown by `faction_reactions` since DR-04's audit (DR-29). F-001's integration route (a new `moral_choice_delayed_callbacks.json` through the loader family) remains viable and now has a verified, current collision-sweep target set of exactly these eight catalogs plus `moral_choice_flags.json`'s flag ids.

## 26.5 FP-A24 closure — recorded no-change area

VERIFIED: `quests_bureaucratic_morality.json` (1,885 bytes) contains exactly 2 records, each with authored `id`, `title`, `description`, `type`, `minDay`, `maxDay`, and a rich `choices` structure (118 prose words across 2 records — no skeleton fields). FP-A24's premise ("records with skeleton quest_hook/outcome texts") is disproven by the working tree. Per the zero-plans doctrine, FP-A24 is closed as a no-change area with this census row as evidence, and the ASH-EXP-3 charter's satellite list drops it accordingly. The bureaucratic-morality domain's future prose work, if any, routes through the two records' choice texts only, and requires no tranche program.

## 26.6 Census-derived backlog deltas

- NEW SEED (Lane A, C10): moral-choice branching prose-depth audit — the family's 339,862-byte flagship catalog (`moral_choice_quests_branching.json`) is the verified largest prose surface in the data authority; a Tranche-0-style depth census against its actual field shape is the highest-yield Lane A measurement now available. Confidence: HIGH CONFIDENCE (size verified; depth unmeasured).
- NEW SEED (Lane G, C10): `detect-corpus-duplicates.py` run against the full 338-catalog authority with results published — the firewall becomes evidence, not procedure (the script is verified live; its output is not). Confidence: VERIFIED instrument, UNVERIFIED output.
- AMENDED: FP-A25 Tranche 0 corrected per 26.3; FP-A24 closed per 26.5; SB-08 design input published per 25.2.

---

# VOLUME 27 — HARNESS SPECIFICATION SUPPLEMENT: THE SIX ASH-EXP-6 HARNESSES (Factory batch 2026-09-24-D)

The ASH-EXP-6 charter runs six Lane C plans: FP-C01 (H-C1 industrial income-versus-expenditure), FP-C02 (H-C2 SOFC sustainability), FP-C03 (H-C3 scavenging E[value]), FP-C04 (H-C4 vehicle dominance), FP-C09 (H-C8 winter compression), FP-C12 (H-C10 cultivation economics). Volume 8 specified each harness's question, parameters, method, outputs, and acceptance. This supplement adds what the charter's verification matrix demands and Volume 8 did not: input fixtures, report schemas, and hand-reconciliation procedures. All inherit the Volume 8.13 publication rules unchanged.

Standing evidence note: this session verified live existence for these harness-relevant surfaces: the cultivation catalogs' owning systems (`HydroponicBiomeSystem.cs`, `HydroponicCropCatalog.cs`, `AeroponicsSystem.cs`, `AquaponicsSystem.cs` in `Assets/Ashfall.Core/Shelter/`), the expedition owners (`ExpeditionVehicleSystem.cs`, `DiscoveryConsequenceSystem.cs`, `District8DeepCoastSystem.cs`, `IceRoadSystem.cs`), the power owners (`PowerGridSystem.cs`, `SofcPowerCatalog.cs`, `SofcElectrochemistryEngine.cs`, `KineticStorageSystem.cs`, `SolarConcentratorEngine.cs`, `GeothermalAquiferSystem.cs`), the needs owners (`NeedsSystem.cs`, `NeedsModifierStack.cs`, `NeedsComponentStore.cs`), the market/debt owners (`MarketSystem.cs`, `LedgerDebtSystem.cs`, `DebtConsequenceDispatcher.cs`, `ResourceRationingSystem.cs` in `Economy/`), and the wildlife owners (`WildlifeTrappingSystem.cs`, `WildlifeMigrationSystem.cs`, `WildlifeSeasonalCalendar.cs`, `WaterborneExposureRules.cs`). Catalog files cited by the harness specs' parameter lists (for example `regional_prices.json`, `warlord_doctrines.json`, `trade_embargoes.json`, `year_of_ash_storm_windows.json`) were NOT individually re-verified this session and keep their carried HIGH CONFIDENCE labels; each harness's first run re-verifies its parameter inputs against the 338-catalog live listing.

## 27.1 Input fixture contract (all six harnesses)

```text
fixture_name: HC-{n}_fixture_{catalog}_{preset}_{date}
source: live catalog from Assets/StreamingAssets/Data/ (cited by filename and byte size
  from the data-authority census, so the fixture is joinable to a repository state)
roster: fixed starting cohort (the Plan 76.2 pattern: no wall-clock randomness)
seed: recorded ISeededRng sub-stream name + seed value, one sub-stream per stochastic input
snapshot_rule: fixture inputs are read-only; the harness never mutates catalogs
drift_guard: a fixture older than one content wave is regenerated, not reused (rule 8.13.3)
```

Per harness, the fixture set is: H-C1 — one industrial chain set (drawn from the `Shelter/` engine family above) plus price inputs; H-C2 — starting fuel inventory, income schedule, storm-window drawdown table; H-C3 — the 53-destination surface with per-destination stance/season pairs (200 runs per cell per the established pattern); H-C4 — vehicle/modification/armor configuration matrix against route classes (ice-road, deep-coast, standard — the verified owners name the route families); H-C8 — 270-day window (Days 90–360), roster with authored intake, storm-window catalog; H-C10 — the three cultivation families' crop sets with power/water/nutrient constants.

## 27.2 Report schema (all six harnesses)

```text
report_path: docs/balance/HC{n}_{slug}_{date}.md
header_block:
  harness_id, question (verbatim from Volume 8), date, seed, preset (or "preset-limited:
  W1 unsealed" for H-C1/H-C2 per the charter's limitation rule), catalog versions
  (filename + byte size + schema_version where the catalog carries one), two-pass proof
  hashes (run A, run B, byte-identical assertion)
body:
  results tables exactly as Volume 8's Outputs field specifies, each row joinable to a
  live catalog entry (owning catalog named per row or per table)
  outlier/anomaly rows marked "FP-C13 evidence gate" where applicable (H-C3's trim table)
  mathematical-issue vs design-preference labels per Category 7 (charter closeout rule)
appendix:
  fixture inventory (27.1), runtime, engine/commit context, hand-reconciliation record (27.3)
```

## 27.3 Hand-reconciliation procedure (one per report, per the charter's verification matrix)

1. Choose three result rows: the largest-value row, the smallest-value row, and one mid-band row.
2. Recompute each by hand from the cited catalog rows and the owning system's constants (for H-C8: needs decay constants against the shortfall day; for H-C1: the chain's input prices against its yield; and so on per the harness's Question).
3. Record agreement or divergence per row. Divergence above 5 percent (or any sign flip: a chain that is profitable in the harness and dominated by hand) blocks the report's publication until explained.
4. The reconciliation record is part of the report appendix; a report without it is not published under the 8.13 rules.

## 27.4 Per-harness supplements

- H-C1 (industrial income-versus-expenditure): the chain list is enumerable from the verified `Shelter/` engine families (SOFC, cupola foundry, CVD diamond, EB-PVD coating, Fischer-Tropsch, chlor-alkali, plastic pyrolysis, kinetic storage, solar concentrator, geothermal ORC, cryogenic air separation, precision broaching, precision optics, precision metrology — all verified owner classes). The fixture cites each chain's owning catalog and engine class, making the dominated-process list joinable to code, not just data.
- H-C2 (SOFC sustainability): reconciliation anchors are the Plan 122/125 closeout evidence per Volume 8; DR-06's premise correction against Plan 122 SOFC fuel stands — the harness does not re-litigate it, it measures the sealed behavior.
- H-C3 (scavenging E[value]): the outlier table's status as FP-C13's evidence gate is restated in the report header so no tuning tranche can cite the harness without citing the gate.
- H-C4 (vehicle dominance): route classes bind to the verified owners (`IceRoadSystem`, `District8DeepCoastSystem`); the delta table publishes beside `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md` without editing it (historical baselines are immutable records — the F-008 exemption rule).
- H-C8 (winter compression): the flagship per the charter; its sustainability table's "resource that fails first per preset" row is B-19's design input and carries that citation.
- H-C10 (cultivation economics): the three families are verified live systems; the family-dominance check must respect crop-strain/genome parameters, and per F-012's open premise, hydroponics' economy legs are unmeasured — H-C10's report is the measurement that closes or confirms F-012's inference.

## 27.5 Sequence note

The charter runs the four ungated harnesses first and the three preset-limited plans with stated limitations; this supplement adds no gates. W1's seal state at run time is read from the live `INTEGRATION_PLANS.md` (78,528 bytes, DR-26) — not from any factory document.

---

# VOLUME 28 — PER-CLUSTER VERIFICATION COMMAND COOKBOOK (Factory batch 2026-09-24-D)

The ledger asked for a cookbook mapping every plan to its exact gate and selftest commands. This volume publishes the verified command surface and the per-cluster mapping rules, with one honesty boundary: gate-to-command bindings are verified only where the live manifest was read (its header and first entries: `whitespace_hygiene` → `bash scripts/ci/no-whitespace-churn.sh`; `json_schema_policy` → `bash scripts/ci/json-schema-policy-gate.sh`). The remaining bindings follow the manifest's declared structure (`gate_id`, `command`, `expected_summary` per gate) and are marked accordingly. Any session executing a plan reads its gate's `command` from `docs/ci/CI_GATE_MANIFEST.json` at run time — this cookbook routes, it does not replace the manifest.

## 28.1 The three verification tiers (all verified live)

1. CI gates: `bash scripts/ci/verify-fast.sh` (fast tier, 53 gates at manifest 1.1.0) and `python3 scripts/ci/run-gates.py` (the manifest-driven runner). Full tier per manifest classification (4 non-fast gates at 1.1.0, including `save_support_window` and the performance gate per the v1.0 inventory).
2. xUnit: `Ashfall.Core.Tests/` (root), built per the manifest's `build_core_tests` gate ("Build Ashfall.Core.Tests (net9.0)" — verified gate name).
3. Headless selftests: the `*HeadlessDemo.cs` classes in `Assets/Ashfall.Core/` — verified live: `BrineWaterHeadlessDemo`, `CensusHeadlessDemo`, `Cluster12CHeadlessDemo`, `CrossingArbitrationHeadlessDemo`, `CrossingHeadlessDemo`, `DeepCoastHeadlessDemo`, `EndingsHeadlessDemo`, `GeothermalAquiferHeadlessDemo`, `HoldfastHeadlessDemo`, `IceRoadHeadlessDemo`, `InfrastructureHeadlessDemo`, `LedgerDebtHeadlessDemo`, `NarrativeHeadlessDemo`, `PersonalQuestHeadlessDemo`, `SurvivorsHeadlessDemo`, `TravelEncounterHeadlessDemo`, `TravelingCaravanHeadlessDemo`. Plus the Godot-side surface: `src/CSharpVerificationTest.cs`, `src/Main.UiTests.*.cs` (20 verified suites: CompositionRoot, Dose, DutyRoster, Economy, Expeditions, Holdfast, Inventory, Journal, Muster, Phase0, Plans198_201, PlayerPanels, RealCampaignJourney, SilentFoundry, StartingCohortLifecycle, Survivors, UtilityAi, Verdict, Wave6, WorkshopRelic), `src/Main.WorldPlaytest.cs`, `src/HostCliRegistry.cs`/`HostTestSummary.cs` (the selftest manifest surface — `generate-selftest-manifest.py` is its verified generator).

## 28.2 Per-cluster mapping (C1–C17)

Each row names the cluster's owning verification surfaces. Convention: a plan's verification section names (a) the data gates its catalogs touch, (b) the focused xUnit file(s), (c) the headless demo or UiTests suite that exercises the runtime, and (d) any specialized gate.

| Cluster | Verified verification surface |
|---|---|
| C1 Shelter | `InfrastructureHeadlessDemo`; `Shelter/` owners; `scene-lint.py` for scene changes; snapshot/a11y gates via the manifest for panel work |
| C2 Medical | `Main.UiTests.Dose`; `Dose/`, `Disease/` src dirs; `Ashfall.Core.Tests/` focused suites (per-corpus tests verified by the census evidence pattern, e.g. `Plan30SpiritualWorldTests` shows the per-plan test convention) |
| C3 Water/food/agriculture | `BrineWaterHeadlessDemo`; `WaterTreatmentSystem`, `KitchenNutritionSystem`, `GrainProcessingSystem`, cultivation owners; json-schema + content-acceptance gates for new catalogs |
| C4 Power/industry | `GeothermalAquiferHeadlessDemo`, `InfrastructureHeadlessDemo`; `Foundry/` src dir; `verify-capability-claims.py` for any capability claim; H-C1/H-C2 harnesses (Lane C) |
| C5 Expeditions | `Main.UiTests.Expeditions`, `TravelEncounterHeadlessDemo`, `TravelingCaravanHeadlessDemo`, `DeepCoastHeadlessDemo`, `IceRoadHeadlessDemo`, `PersonalQuestHeadlessDemo`; H-C3/H-C4 harnesses |
| C6 Map/geography | `CartographySystem` (Exploration/); `InSarMapping` host partial; scene/godot-asset gates |
| C7 Factions/war | `Main.PsyOps`, `Main.FactionBranch`, `Factions`/`Warlords`/`Diplomacy`/`Treaties` Core dirs; `Main.UiTests.Verdict` for verdict-adjacent surfaces |
| C8 Radio/information | `Radio/` src dir, `Main.RadioProgramProduction.cs`; `audio-asset-gate.py`; sealed distress surfaces per DR-06 |
| C9 Survivors | `SurvivorsHeadlessDemo`, `Main.UiTests.Survivors`, `Main.UiTests.StartingCohortLifecycle`; `Needs`/`Survivors` Core owners |
| C10 Quests/moral choice | `PersonalQuestHeadlessDemo`, `Main.UiTests.Journal` for journal-facing beats; Quest/`MoralChoice` Core dirs; `detect-corpus-duplicates.py` mandatory for any new quest catalog (26.6) |
| C11 Economy | `Main.UiTests.Economy`, `LedgerDebtHeadlessDemo`, `CrossingHeadlessDemo` (crossing trade), `Economy/` Core dir; H-C5 through H-C7 harnesses |
| C12 Weather/Year of Ash | `Main.UiTests.Wave6`, `Main.YearOfAsh.cs`, `YearOfAsh/` src dir; H-C8 harness; weather owners (`IWeatherSeverityProvider` verified live) |
| C13 Endgame/epilogue | `EndingsHeadlessDemo`, `Main.UiTests.Verdict`, `Endgame/` Core dir, `VerdictPanel.cs`; epilogue-reachability focused tests |
| C14 Ecology/wildlife | `WildlifeTrapping*`, `WildlifeMigration*` owners; `Ecology/` Core dir; H-C9/H-C10 harnesses where cited |
| C15 Defense/security | `SkyDefense/` Core dir, `Main.SkyDefense.cs`, `OrbitalHarrowTelemetrySystem`, `AirlockSecuritySystem`; defense-grid owners |
| C16 Progression/meta | `Difficulty/` Core dir, `Main.Difficulty.cs`; `version-gate.py` + changelog-drift (the verified manifest 1.1.0 additions); `l10n_drift_gate.py` + `extract_l10n_inventory.py` for L10N work |
| C17 Host surface/UI | `Main.UiTests.CompositionRoot`, `Main.UiTests.PlayerPanels`, `Main.PlayerSurfaces.cs`, `UI/` dirs; `generate-ui-panel-catalog.py`; a11y and snapshot gates via the manifest; `input-map-gate.sh` + `generate-keyboard-map.py` for input work |

## 28.3 Cross-cutting rules (all verified instruments)

- Every DATA-ONLY plan: `json-schema-policy-gate.sh` + `content-acceptance-gate.sh` + `detect-corpus-duplicates.py` + the content-utilization surface (`ContentUtilizationGate.cs` family — presence is not reachability, now a verified code-level gate).
- Every docs plan: `doc-link-gate.sh`, `normalize-doc-links.py`, `generate-docs-index.py` (the generated-region rule is executable, not aspirational).
- Every Core-extension plan: `forbidden-api-gate.sh` (engine-free enforcement), `catch-policy-gate.sh`, `coverage-gate.sh`, `warning-baseline-gate.sh`.
- Every save-touching plan: the `save_support_window` full-tier gate (verified manifest 1.1.0 Phase 3 addition) and `generate-save-store-matrix.py`.
- Every plan-carrying wave: `generate-plan-register.py` and `plan_governance_config.json`; `quarantine.json` governs quarantined work (the shelter-failure quarantine class from B-01 has a verified governing instrument).
- Multi-agent sessions: `agent-fast-verify.py` and `sync-agent-rulebooks.py` — the DR-09 rulebook sprawl has a verified synchronization tool, which F-007's registration plan should consume rather than duplicate.

## 28.4 Honesty boundary

This cookbook's gate names and commands are verified where cited from the manifest header or the scripts listing; bindings not yet read from the manifest body are structural inferences from a verified schema and are labeled so. No plan may cite this cookbook as final authority for a command string; the manifest is the authority, and the cookbook is the routing layer. This is DR-07's rule applied to the factory's own output.

---

# GROWTH LEDGER UPDATE (this session)

- 2026-09-24 — Volume 25: RB-DRCONFIRM execution — working-tree confirmation table for DR-20 (15 of 15 systems confirmed with paths, BeliefMovement renamed to the Spiritual family surface), DR-07 resolved against the live gate manifest (57 gates, 53 fast, schema 1.1.0), DR-05 narrowed (no tracked caller for `rewrite.py`), six new/amended drift entries (DR-24 through DR-29), deep-map correction (Core source authority is `Assets/Ashfall.Core/`, 226 entries), retirements: none; premise corrections: six — ~21,000 — cumulative ~428,000
- 2026-09-24 — Volume 26: live-census instantiation, partial — data-authority census complete (338 JSON + `rewrite.py` + 3 directories), quest-corpus size census complete (31 catalogs ranked; `moral_choice_quests_branching.json` 339,862 bytes is the largest prose surface, correcting FP-A25's framing), FP-A25 Tranche 0 executed in fragment form with a corrected measurement instrument, FP-A24 closed as a verified no-change area (2 records, authored prose), two new census-derived seeds — ~12,500 — cumulative ~440,500
- 2026-09-24 — Volume 27: harness specification supplement for the six ASH-EXP-6 harnesses (H-C1, H-C2, H-C3, H-C4, H-C8, H-C10) — input fixture contract, report schema, hand-reconciliation procedure, per-harness supplements bound to verified live owner classes, sequence note against the 78,528-byte live ledger — ~8,000 — cumulative ~448,500
- 2026-09-24 — Volume 28: per-cluster verification command cookbook — three verified verification tiers, C1–C17 mapping table, cross-cutting gate rules, honesty boundary against the manifest-as-authority rule — ~9,000 — cumulative ~457,500

Updated inventory: the prior inventory (through Volume 24) plus: a published confirmation table (the DR-20 pipeline's gate — opened), a resolved gate-count authority, six new drift entries, two census-derived seeds, one closed plan (FP-A24, no-change), one corrected tranche program (FP-A25), and the first volumes written with live working-tree access rather than listing-level inference.

Recommended next wave: Volume 29 (complete RB-DRCONFIRM's PENDING rows: read the DR-16/DR-17/DR-18 carriers, the full gate-manifest body, and `rewrite.py` + `POTENTIALCLUTTER.md` together, closing DR-05); Volume 30 (the remaining DR-19 family confirmations — summit protocols, salon chronicles, acetate disc manifests — via code search, then lift the Volume 22 contract deferrals for each confirmed domain); Volume 31 (moral-choice branching Tranche-0 depth census, the new highest-yield Lane A measurement per 26.6); then ASH-EXP-7's deep-map corrections publish as the corrected cluster maps, which are now writable with confirmed paths.

---

# VOLUME 29 — RB-DRCONFIRM COMPLETION: THE PENDING ROWS (Factory batch 2026-09-24-E)

This volume closes the confirmation pass's PENDING rows from Volume 25, section 25.7. Evidence class throughout: VERIFIED means read from the live tree this session (2026-09-24); HIGH CONFIDENCE means strong structural evidence with a named residual read. One row (DR-18) remains PENDING and is carried forward with its verification step intact.

## 29.1 DR-16 CONFIRMED — the communiqué board and the three-axis version authority are live

The runbook's step 1 asked for confirmation that the named files exist on main and contain the described surfaces. Confirmed live, beyond the listing level:

- VERIFIED: `src/UI/FactionCommuniqueBoardPanel.cs` exists; the selftest surface `src/Host/HostCli.FactionCommuniqueSelfTests.cs` exists; the `faction_communique_board` route is consumed by `src/Main.PlayerSurfaces.cs`, `src/Main.UiPanels.cs`, `src/Main.Application.cs`, `src/UI/GameDashboardPanel.cs` (the dashboard button), and `Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs`; the route is registered in `docs/ci/SELFTEST_MANIFEST.json`; the plan of record is `docs/plans/FACTION_WAR_COMMUNIQUE_SURFACE_INTEGRATION_PLAN.md`. This is the full DR-16 scope: panel, route, dashboard entry, selftest, registration.
- VERIFIED: `Assets/Ashfall.Core/ReleaseVersion.cs` exists (confirmed in the Volume 25 Core listing; the DR-16 parser premise).
- VERIFIED (read in full): `Directory.Build.props` — `<VersionPrefix>1.1.0</VersionPrefix>`, `LangVersion latest`, `Nullable enable`, `Deterministic true`, and a documented shared warning policy (CS0108/CS0114/CS0067/CS8602/CS8603/CS8605/CS8629 explicitly NOT suppressed; a NoWarn allowlist naming the serializer-populated DTO warning classes). The 1.1.0 release line is confirmed at the build-props axis.
- VERIFIED (read): `docs/releases/VERSIONING.md` (4,988 characters) — the three-axis policy: Game axis (semver in `project.godot`, mirrored to `Directory.Build.props` and `export_presets.cfg` via `scripts/release/set_version.py`), Data axis (integer `schema_version` per catalog header, inventoried live by `VersionReport.ScanDataSchemas`), Save axis (per-store integer versions plus `manifestVersion`). The policy document names the compatibility rules for each axis.

Consequences published: (a) E-11 (communiqué board snapshot and a11y coverage) is UNGATED — the board exists and its first-wave treatment per the E-02/E-05/E-06 patterns is now plannable against a confirmed surface; (b) D-05's save-support-window re-pin seed has its triggering release confirmed at the build axis (1.1.0); (c) the release-craft contract (Volume 22, section 22.3) is now grounded in read policy text; (d) DP-02's packet annotation (per-strike emitters must name the board as their rendering surface) is confirmed as necessary — the board is live.

## 29.2 DR-17 CONFIRMED at the system level — atlas repair scope carried

- VERIFIED: `Assets/Ashfall.Core/World/WastelandMapSystem.cs` exists, with its contract surface confirmed live: `docs/world/MAP_EVOLUTION_CONTRACT.md`, `docs/world/DAMAGED_MAP_ZONE_AUDIT.md`, `docs/world/REGIONAL_CONTROL_MATRIX.md`, `docs/world/PLAN_11_CONTINUITY_MATRIX.md` (147 total references to the system across the tree).
- The PR-50 atlas-panel repair itself (canonical projection replacing hardcoded sector inference) was not re-verified at the panel-code level this session; the system authority and its contract documents are confirmed, and the repair's merge was publicly dated 2026-09-18 (Volume 19's record). Label: HIGH CONFIDENCE for the repair's survival, with the residual read (the atlas panel's projection source) named for any plan that consumes the atlas as a reference surface — exactly the role FP-A15's marginalia pairing checks assign it.

## 29.3 DR-18 remains PENDING

`KNOWN_DEBT.md` (29,152 bytes, confirmed present) was not read this session. The quarantine-count reconciliation (51 stale entries, "Plan 34 truth") keeps its verification step: read `KNOWN_DEBT.md` and `scripts/ci/quarantine.json` together in the owning session. No factory plan currently depends on the quarantine count; the row stays open for completeness, not urgency.

## 29.4 The full gate inventory is published — SB-08's design input is complete

- VERIFIED (read, not truncated): `docs/ci/CI_GATE_MANIFEST.json` — `schema_version` "1.1.0", `total_gates` 57, `fast_tier_count` 53, and the complete gate_id inventory, published here for the deep maps:
  `whitespace_hygiene`, `json_schema_policy`, `build_core_tests`, `test_core_suite`, `build_godot_host`, `godot_import`, `data_integrity`, `bridge_removal`, `asset_registry`, `player_panels_uitest`, `panel_bind_lifecycle`, `save_load_failure`, `holdfast_save`, `inventory_save`, `journal_save`, `playable_shell`, `day1_onboarding`, `real_campaign_journey`, `runtime_scale_performance`, `expansions_completeness`, `survivors_selftest`, `expedition_selftest`, `triad_drift`, `cli_catalog_drift`, `save_store_matrix_drift`, `architecture_map_drift`, `compiler_warning_baseline`, `docs_index_drift`, `forbidden_core_apis`, `catch_policy_lint`, `persistent_filename_registry`, `central_package_management`, `doc_link_portability`, `lfs_health_check`, `legacy_asset_path`, `legacy_reference`, `core_systems_catalog_drift`, `catalog_registry_drift`, `agent_rulebooks_sync`, `ui_panel_catalog_drift`, `expansions_catalog_drift`, `ui_panel_contracts_test`, `audio_catalog_drift`, `agent_skills_catalog_drift`, `audio_cue_integrity_gate`, `campaign_envelope_fuzz_test`, `case_alias_guard`, `selftest_manifest_drift`, `export_parity`, `uid_sidecar_gate`, `coverage_gate`, `content_acceptance_pipeline`, `port_contract_gate`, `input_map_contract`, `version_gate`, `changelog_drift`, `save_support_window`.
- The manifest is the count authority named in DR-07's resolution (Volume 25, section 25.2). SB-08's design is now fully specified: the guard compares any documented gate count against `total_gates` + the gate_id list, keyed by `schema_version`, with historical documents (closeouts, bibles) exempted via an allowlist per the F-008 rule. One inventory observation for the guard's design: the live manifest contains one gate_id whose listed name (`ui_panel_catalog_drift`) appears twice in the drift-gate family space alongside `expansions_catalog_drift` and `audio_catalog_drift` — the guard must compare on the full gate_id set, not on family-name patterns, or it will produce false divergences.

## 29.5 DR-05 CLOSED — `rewrite.py` read in full; F-009 resolved to a placement decision

- VERIFIED (read in full, 4,101 characters): `Assets/StreamingAssets/Data/rewrite.py` is a one-off historical authoring script. Its behavior: it opens `quests_faction_branching.json` (via a hardcoded author-local absolute path, `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/Assets/StreamingAssets/Data/quests_faction_branching.json`), defines five faction theme records (bone_pickers, blood_tithe, drought_cartel, martial_law, generic), and for each faction-type quest generates choice and stage prose by seeded selection (`random.seed(quest_id)`) from small canned pools: 8 advance texts, 8 abort texts, 7 atmosphere lines. It also derives display names from quest ids.
- VERIFIED: no tracked caller exists (Volume 25's search; its only mention is `POTENTIALCLUTTER.md`).
- Resolution: the script is not load-bearing for any migration — its product is already committed in the catalog it rewrote, and it cannot run as written in any other environment (the absolute path). F-009's handling decision reduces to: archive to `docs/archive/` or `tools/` with a one-paragraph rationale, plus the data-authority hygiene assertion proposed in F-009 (the data directory contains only `.json` plus whitelisted artifacts). Either resolution preserves the script byte-for-byte; nothing is rewritten.
- Consequence that outranks the hygiene question — see DR-31 (Volume 31): the script is documentary evidence that `quests_faction_branching.json`'s choice prose was machine-generated from 23 canned lines. The hygiene fix is small; the prose-provenance finding is the material output.

## 29.6 `POTENTIALCLUTTER.md` read (fragment) — registered for F-007

- VERIFIED (fragment read): the document is a read-only sweep audit output dated 2026-09-12, in the `AI_AGENT_WORKFLOW.md` sweep format (`finding_id | severity | confidence | path:line | current evidence | expected contract | proposed owner`), explicitly a tag-only audit that authorizes nothing. Section A enumerates unreferenced forensic-report batches (`docs/forensics/BUG_HUNT_25` through `29`, PLANS forensic reports) with zero-reference grep evidence, and orphan authority maps.
- Registration consequence: F-007 (root coordination surface registration) gains a classification row: `POTENTIALCLUTTER.md` is an ACTIVE audit ledger (dated, format-governed, consumed by foreman decisions), not clutter itself. The BUG_HUNT findings it records are the clutter candidates, and their disposition belongs to the foreman, per the document's own protocol.

## 29.7 New drift-register entry

**DR-30 (new) — The Data-axis version authority is a live, documented surface. VERIFIED.**
`docs/releases/VERSIONING.md` defines the Data axis as a per-catalog integer `schema_version` inventoried live by `VersionReport.ScanDataSchemas` (`VersionReport.cs` confirmed in the Core listing). Factory rule: any subject plan citing a catalog's version cites its `schema_version` field, never a date or a document's claim about the catalog. This supersedes the Volume 27 fixture contract's "schema_version where the catalog carries one" — the policy says all catalogs carry one; the harness fixtures must record it uniformly, and the first ASH-EXP-6 run verifies that claim per catalog it touches.

---

# VOLUME 30 — DR-19 FULLY CONFIRMED: THE FLAGSHIP FAMILIES ARE LIVE (Factory batch 2026-09-24-E)

Volume 18 recorded DR-19 (the PR #36 flagship families) as VERIFIED-AS-LISTED with merge state UNVERIFIED; Volume 25 confirmed only the cultural-archive family. This volume confirms every remaining family at the working-tree level and lifts the gates on all five Volume 18 replenishment seeds.

## 30.1 Confirmation table

| DR-19 family | Status | Verified evidence |
|---|---|---|
| Cultural Archive Vault System | CONFIRMED (Volume 25, extended) | `Assets/Ashfall.Core/Culture/CulturalArchiveVaultSystem.cs`, `CulturalArchiveTomeCatalog.cs`, `ArchiveChronicleMilestones.cs`; catalog `Assets/StreamingAssets/Data/cultural_archive_tomes.json`; save store `src/Host/CulturalArchiveSaveStore.cs`; tests `Ashfall.Core.Tests/CulturalArchiveVaultTests.cs`; consumed by `ContentUtilizationScanner.cs`; baseline artifact `artifacts/content-utilization-baseline.json`; save matrix `docs/saves/SAVE_STORE_CONTRACT_MATRIX.md`; implementation log `docs/plans/PLANS_FLAGSHIP_INSTITUTIONS_T5_8_IMPLEMENTATION_LOG.md` |
| Salon modifier (non-stacking, cooldown) | CONFIRMED | Salon surface lives inside `CulturalArchiveVaultSystem.cs` (search-confirmed hits in the system, its tests, `Main.FlagshipInstitutions.cs`, and the lore authority map `docs/culture/PLAN_178_190_CREATION_LORE_AUTHORITY_MAP.md`) |
| Acetate disc cutting (playback morale stays with VinylMoraleSystem) | CONFIRMED | Acetate surface inside `CulturalArchiveVaultSystem.cs`; vinyl morale separation confirmed by the live `VinylMoraleSystem.cs` (Core top level, Volume 25 listing) and the narrative corpus twin `Assets/StreamingAssets/Data/narrative/vinyl_record_archive.json` |
| Diplomatic Summit System | CONFIRMED | `Assets/Ashfall.Core/Diplomacy/DiplomaticSummitSystem.cs` plus `DiplomaticTreatyCatalog.cs` in the same directory; `src/Host/DiplomaticSummitSaveStore.cs`; wired by `src/Main.FlagshipInstitutions.cs` and `src/Main.SaveOrchestrator.cs`; registered in `docs/saves/SAVE_STORE_CONTRACT_MATRIX.md` and consumed by `scripts/ci/generate-architecture-map.py` |
| IInstitutionAvailability (one live claim per survivor) | CONFIRMED (Volume 25, cross-referenced) | `Assets/Ashfall.Core/Institutions/IInstitutionAvailability.cs`, `InstitutionAssignmentLedger.cs` |
| WeatherSondeSystem (humidity provider host binding) | CONFIRMED | `Assets/Ashfall.Core/World/WeatherSondeSystem.cs`; `src/UI/WeatherSondePanel.cs`; `src/Host/WeatherHostSession.cs`; wired by `src/Main.World.cs` and `src/Main.UiPanels.cs`; authority map `docs/plans/PLANS_B70_B73_AUTHORITY_MAP.md`; panel ownership `docs/ui/PANEL_AUTHORITY_OWNERSHIP.md` |

## 30.2 Gates lifted — the five Volume 18 seeds enter the plannable pool

All five replenishment seeds' first gate (DR-19/DR-16 confirmation) is passed. Their second gate (the standing premise sweep) remains, per the factory's double-gate rule:

- A-31 (cultural-archive document corpus): UPGRADED to HIGH CONFIDENCE. The vault system, tome catalog, save store, and tests are live. The seed's prose domains (restoration logs, transcription project sheets, microfiche condition reports, acetate disc-cutting manifests) now have confirmed owning surfaces. Its premise sweep reads `CulturalArchiveVaultSystem.cs` for the state vocabulary each document genre conditions on.
- A-32 (summit protocol documents): UPGRADED to HIGH CONFIDENCE. `DiplomaticSummitSystem.cs` and `DiplomaticTreatyCatalog.cs` are live. The violation-ledger routing premise keeps its own verification (the IFactionStandingPort route) inside the sweep.
- G-09 (summit determinism regression suite): UPGRADED to HIGH CONFIDENCE with its premise now directly checkable — the keyed-RNG negotiation rounds (per-(seed, summit, round) streams) are readable in `DiplomaticSummitSystem.cs` in the owning session; the suite pins what the source shows, per the G-07 pattern.
- B-26 (institution-availability interaction audit): UPGRADED to HIGH CONFIDENCE. The port and ledger are live; the audit's counterparties (apprenticeship, cohort assignment) are confirmed systems (Volume 25: `ApprenticeshipSystem.cs`, `CohortSystem.cs`).
- E-11 (communiqué board snapshot and a11y): UNGATED (Volume 29, section 29.1). The board is a live, registered panel; its first-wave E-02/E-05/E-06 treatment is now the correct next step for that surface, not a later sweep.

## 30.3 Volume 22's contract deferrals are liftable

Volume 22 deferred contracts for the DR-19 domains "because the factory cannot confirm the surfaces exist on main." All four prose domains are now confirmed: cultural-archive restoration logs, microfiche/acetate condition manifests, summit protocol documents, salon chronicle texture. The next prose-contract supplement volume (not this one — the worked-model discipline requires reading each owning system's state vocabulary first) may author these four contracts with genre shapes drawn from the confirmed systems. This volume records the lift; it does not pre-write the contracts.

## 30.4 Deep-map amendments (ASH-EXP-7 inputs)

- C7 (factions and war) gains the Diplomacy cluster: `DiplomaticSummitSystem`, `DiplomaticTreatyCatalog`, `RegionalTreatySystem`/`RegionalTreatyFeed` (Volume 25 listing), plus the communiqué board panel as a host surface.
- C16 (progression and meta) gains the institutions family (`IInstitutionAvailability`, `InstitutionAssignmentLedger`) and the weather sonde (`WeatherSondeSystem` as a host-bound provider).
- C9 (survivors and interiority) gains the cultural-archive family (vault, tomes, chronicle milestones, documentation system) — an interiority-adjacent authority the 2026-09-24 deep maps lacked entirely.
- C17 (host surface) gains `FactionCommuniqueBoardPanel` and `WeatherSondePanel` with their registrations confirmed (`SELFTEST_MANIFEST.json`, `PanelRegistryBootstrap.cs`).

---

# VOLUME 31 — MORAL-CHOICE BRANCHING TRANCHE-0 CENSUS, FRAGMENT; AND THE FACTION-BRANCHING PROVENANCE FINDING (Factory batch 2026-09-24-E)

## 31.1 Moral-choice branching: field census (VERIFIED, fragment)

`moral_choice_quests_branching.json` (339,862 bytes — the data authority's largest prose surface, per Volume 26) was read to the session's fetch boundary (~32,793 characters, approximately 10 percent). Within the verified fragment:

- VERIFIED: structure is `{ "schema_version": 1, "quests": [...] }`.
- VERIFIED: record shape is `id`, `display_name`, `category`, `trigger`, `discovery`, `location_id`, `min_day`, `max_day`, `choices[]`, where each choice carries `label`, `moral_delta`, `empathy_delta`, `set_flag`, `outcome_text`, and `epitaph`.
- VERIFIED: the fragment's prose is authored-depth, not skeleton. The visible record (`quest_moral_chain_mercy_01`, "The Open Hand") carries a rich `trigger` and a `discovery` field with concrete, counted detail ("Three families — eleven people, four of them children — stand at the gate in the rain"), and the fragment's tail shows the same depth on a second chain's discovery prose.
- Census verdict: the fragment supports HIGH CONFIDENCE that this catalog's prose debt is low relative to its size; the Volume 26 seed's premise ("depth unmeasured") moves toward the favorable end, but the remaining 90 percent keeps its measurement step — the Tranche-0 census completes in the owning session by segment reads, per the corrected FP-A25 instrument (Volume 26, section 26.3).
- Contract correction for the deep maps: the factory's assumed Part 9 fields (`quest_hook`, `objective_text`) do not exist in this family. Its prose contract must be derived from the actual fields: `trigger` and `discovery` are the scene-setting prose pair; `label`/`outcome_text`/`epitaph` are the choice-facing prose triple; `category` is the moral-axis grouping key. Any Lane A plan in this family writes to these fields, and RB-CENSUS scores them, not the invented ones.

## 31.2 The provenance finding — `quests_faction_branching.json`'s choice prose is machine-generated (VERIFIED via `rewrite.py`)

The `rewrite.py` read (Volume 29, section 29.5) is documentary evidence about the catalog it generated, and the evidence is material for Lane A:

- VERIFIED: `quests_faction_branching.json` (194,389 bytes, 26-record visible class in the Volume 26 size census) had its faction-type quest prose generated by seeded selection from exactly 23 canned lines: 8 advance texts ("Proceed. This is a liability we cannot afford." and seven variants), 8 abort texts ("Stand down. If we do this, we're no better than the monsters in the ash." and seven variants), and 7 atmosphere lines, with faction themes (The Bone Pickers, The Blood Tithe, The Drought Cartel, Martial Law) supplying a name and danger line each.
- Consequence: many records in this catalog necessarily share identical choice prose, differing only by theme substitution and seeded line selection. Size is not depth here — the catalog is the third-largest quest surface (Volume 26) while its choice-level prose vocabulary is 23 lines wide. This is a verified, player-visible prose-debt surface, and it re-ranks the Lane A roadmap: the faction-branching catalog overtakes the moral-choice families as the highest-yield prose-debt target, because its debt is proven rather than inferred.
- The finding also disciplines the factory's own census instruments: RB-QUANT's rule 1 (every quantity must reconcile with the owning system) extends naturally — prose provenance must reconcile with the authoring instrument when one exists, and `rewrite.py` is exactly such an instrument.

**NEW SEED — A-41 · C7/C10 · Faction-branching choice-prose de-templatization program.** Subject: a Tranche-0 census of `quests_faction_branching.json` scoring each faction-type record's advance/abort/stage prose against the 23-line generator vocabulary (exact-match classification is cheap and mechanical), followed by authored replacement prose for the highest-frequency canned lines, tranche-bounded per the FP-A25 program pattern, with display names also audited (the script derived them mechanically from ids). Route: DATA-ONLY. Verification: integrity + utilization selftests; a mechanical uniqueness report (line-usage histogram before/after); RB-PAIR id checks against the faction catalogs. Confidence: HIGH CONFIDENCE (provenance VERIFIED; per-record reuse counts unmeasured pending census).

## 31.3 `moral_choice_faction_reactions.json` — listing-level only

The eighth moral-choice catalog (DR-29) was fetched but exceeded the session's parse boundary (14,957 characters returned; parse incomplete). Its field inventory remains an open premise for F-001's collision sweep; the owning session reads it with the same field-census protocol. No claim about its content is made here.

## 31.4 Amended backlog state after this wave

- FP-A24: closed (Volume 26). FP-A25: instrument corrected (Volume 26); its priority rank now sits behind A-41 (proven-provenance debt outranks unmeasured debt).
- A-41: new, HIGH CONFIDENCE, Lane A/C7-C10.
- A-31, A-32, B-26, G-09, E-11: ungated, premise-sweep-ready (Volume 30).
- SB-08: design input complete (Volume 29, section 29.4); implementable on the next Tooling wave.
- F-009: resolved to a placement decision with the archive recommendation (Volume 29, section 29.5).

---

# GROWTH LEDGER UPDATE (this wave)

- 2026-09-24 — Volume 29: RB-DRCONFIRM completion — DR-16 fully confirmed (communiqué board panel, route, dashboard, selftest, registration; three-axis version policy read; `VersionPrefix` 1.1.0 read), DR-17 confirmed at system level with one residual panel read named, DR-18 carried PENDING, the complete 57-gate inventory published, `rewrite.py` read in full and DR-05 closed (one-off historical authoring script, no tracked caller, archive recommended), `POTENTIALCLUTTER.md` read and registered for F-007, DR-30 (Data-axis version authority) recorded — ~11,500 — cumulative ~469,000
- 2026-09-24 — Volume 30: DR-19 fully confirmed — all six flagship families verified at working-tree level (cultural archive vault, salon modifier, acetate disc cutting, Diplomatic Summit System with treaty catalog and save store, institution availability port, weather sonde); all five Volume 18 replenishment seeds ungated (A-31, A-32, B-26, G-09, E-11); Volume 22's four contract deferrals lifted; deep-map amendments recorded for C7, C9, C16, C17 — ~8,500 — cumulative ~477,500
- 2026-09-24 — Volume 31: moral-choice branching Tranche-0 fragment census (field inventory published: trigger/discovery/label/outcome_text/epitaph; authored-depth verdict with the 90-percent residual named) plus the verified provenance finding that `quests_faction_branching.json`'s choice prose was machine-generated from 23 canned lines — new seed A-41 (faction-branching de-templatization program), Lane A roadmap re-ranked, amended backlog state published — ~7,000 — cumulative ~484,500

RB-DRCONFIRM is now complete in substance: 15 of 16 tracked drift entries are working-tree confirmed or closed; the single remaining row (DR-18, `KNOWN_DEBT.md` + `quarantine.json`) has no dependents in the current backlog.

Recommended next wave: Volume 32 (the four lifted prose contracts — cultural-archive restoration logs, microfiche/acetate manifests, summit protocol documents, salon chronicle texture — authored as worked contracts against the confirmed systems' state vocabularies); Volume 33 (A-41 Tranche 0: the mechanical line-usage census of `quests_faction_branching.json`, executable with segment reads); Volume 34 (DR-18's closure: `KNOWN_DEBT.md` and `quarantine.json` read together, retiring the last PENDING row); Volume 35 (the corrected cluster deep maps, ASH-EXP-7's flagship, now writable with every path confirmed); then the first implementation-side wave becomes available if the foreman authorizes one (SB-08's guard and F-009's archive are the two smallest verified findings ready for execution).

---

# VOLUME 32 — THE FOUR LIFTED PROSE CONTRACTS (Factory batch 2026-09-24-F)

Volume 30 lifted Volume 22's contract deferrals; this volume writes the four contracts with the discipline those deferrals demanded: each contract is grounded in the owning system's verified state vocabulary and the catalog's verified field shape, read from live source this session. Standing rules unchanged: originals only, banned patterns excluded, quantities reconcile with the owning system (RB-QUANT), knowledge horizons bind (RB-KNOWLEDGE).

Evidence base for all four contracts (read 2026-09-24): `Assets/Ashfall.Core/Culture/CulturalArchiveVaultSystem.cs` (state keys verified: `archived`, `salon_stress_resistance`, `degradation_permille`, `transcribing`, `transcribed`, `salon_active`, `salon_cooldown`, recording categories `music_performance` / `oral_history` / `survivor_testimony` / `radio_archive` / `commemorative`, film classes `photographic_film` / `microfiche_film`, documentation classes `documentation_photo` / `documentation_sketch`, condition states `lost`, `missing_author`, `too_degraded`, `already_restored`, `unknown_tome`, `missing_inputs`, `no_scholar`, `scholar_unavailable`); `Assets/Ashfall.Core/Diplomacy/DiplomaticSummitSystem.cs` (state keys verified: `scheduled`, `active`, `negotiating`, `collapsed`, `ratified`, `agenda_exhausted`, `summit_collapsed`, `stability`, `tension`, `concessions`, `guarantee`, `delegate`, `delegate_unavailable`, `site_not_neutral`, `too_few_factions`, `faction_hostile`, `insufficient_stability`, `expiry_day`, `treaty_not_active`, neutral site `loc_neutral_ground`, skills `skill_cold_analysis` / `skill_watchful`); `Assets/StreamingAssets/Data/cultural_archive_tomes.json` (record shape verified: `tome_id`, `display_name`, `category`, `description`, `transcription_days`, `paper_brittleness_tier`, `initial_degradation_permille`, `microfiche_frame_density`, `knowledge_bonus`, `morale_effect`, `restoration_costs[]`); `Assets/StreamingAssets/Data/regional_treaties.json` (verified parse: `treaty_id`, `display_name`, `faction_id`, `description`, `ratification_cost_scrap`, `ratification_cost_day`, `prerequisites`).

## 32.1 Cultural-archive restoration log (new genre; owning surface confirmed per Volume 30)

```text
prose_field: restoration_log
purpose: the vault's own working record of a preservation effort on a tome or film
trigger: restoration attempt on a cultural_archive_tomes record (any outcome state)
length: 60-120 words
viewpoint: institutional register, present-tense work notes
must_include: one physical defect named in the vault's vocabulary (degradation_permille
  band or brittleness tier), one material or hand named in the restoration_costs entries,
  one observed state transition (too_degraded, already_restored, transcribing, transcribed,
  lost, or missing_inputs — exactly as the system emits)
must_not_include: sentimentality about the content preserved (the salon register carries
  feeling; the work log does not), quantities the tome record does not carry
model:
  Tome 1974 mechanics handbook, second pass. Binding rehumidified over four hours;
  torque-table leaves re-secured with wheat-starch paste. Brittleness reads tier 2
  after the pass, down from the intake band. Three leaves too degraded at the gutter
  and set aside for the microfiche route. Transcription assigned; scholar unavailable
  until the roster turns. The vault holds the parts; the rest is scheduling.
```

RB-QUANT binding: every defect band, tier, and cost item must equal or annotate a value in the tome record or the vault's emitted state; the log never invents a cost the `restoration_costs` array does not carry.

## 32.2 Microfiche and acetate condition manifest (new genre; confirmed surface)

```text
prose_field: condition_manifest
purpose: periodic condition report for photographic_film, microfiche_film, and acetate
  blank-disc stock in the vault
trigger: scheduled inspection cycle, or a state transition on any film-class holding
length: 40-90 words per holding
viewpoint: survey register, past-tense findings
must_include: the film class in the system's exact vocabulary, one measured condition
  (frame density for microfiche, a degradation_permille reading, or a visible defect),
  one disposition (hold, transcribe, duplicate, or set aside)
must_not_include: playback impressions (playback morale belongs to VinylMoraleSystem —
  the DR-19 separation is a contract rule, not a suggestion), restoration narration
  (that is the restoration_log genre)
model:
  Microfiche run, municipal water tables, frames 60 to the sheet, density within
  tolerance at all four corners. One sheet with vinegar-note odor at the edge —
  set aside for duplication before it goes too degraded. Photographic film batch
  clean. Acetate blank stock counts eleven; the cutting queue may draw two.
```

## 32.3 Summit protocol document (new genre; confirmed surface)

```text
prose_field: summit_protocol
purpose: the procedural record of a diplomatic summit — scheduling notice, round record,
  or ratification instrument
trigger: summit state transition (scheduled, negotiating, collapsed, agenda_exhausted,
  or ratified)
length: 60-120 words
viewpoint: neutral-site registrar register
must_include: the site (loc_neutral_ground class), the participating factions as
  delegates, one procedural fact in the system's vocabulary (a stability or tension
  reading, a concession exchanged, a guarantee held, an expiry_day), the outcome state
must_not_include: negotiation interiority the delegates did not record (the keyed rounds
  are the system's; the document reports their outcomes), faction private language
model:
  Convened at the neutral ground, third such session this season. Delegates seated
  for both parties; the cold-analysis bench attended for one. Two rounds recorded,
  one concession exchanged — grazing rights against scrap at the ratified rate —
  and stability held above the collapse line through both. Ratified on the day of
  record; the instrument carries the standard expiry_day. Tension notes annexed.
```

## 32.4 Salon chronicle entry (new genre; confirmed surface)

```text
prose_field: salon_chronicle
purpose: the vault's record of a convened salon session and its stress-resistance work
trigger: salon_active to salon_cooldown transition
length: 50-100 words
viewpoint: institutional register, restrained warmth permitted (this genre alone may
  register feeling, because the salon is the modeled morale instrument)
must_include: one salon_stress_resistance effect observed, the cooldown noted, one
  participant or role (not a full roster), one item or program that anchored the session
must_not_include: numbers the salon system does not emit; non-stacking claims (the
  modifier is non-stacking by system design — the chronicle must not imply otherwise)
model:
  Salon convened in the reading room, four chairs filled. The mechanic read aloud
  from the 1974 handbook's wiring chapter, of all things, and the room argued about
  tolerances until the lamps dimmed. Stress resistance held through the evening by
  the ledger's own measure. Cooldown now in effect; the room reopens when it lifts.
```

These four contracts close the deferral debt from Volume 22. Every genre binds to a confirmed system and a confirmed catalog shape; none invents a state the systems do not emit.

---

# VOLUME 33 — A-41 TRANCHE 0: FACTION-BRANCHING LINE-USAGE CENSUS, FRAGMENT (Factory batch 2026-09-24-F)

## 33.1 What was measured (VERIFIED)

`quests_faction_branching.json` (194,389 bytes) was read to the session's fetch boundary: the first 32,793 characters, approximately 17 percent of the file. Within the verified fragment:

- VERIFIED: 98 quest records are fully or partially visible; 34 carry the faction-type marker (`"type": "faction_chain"`).
- VERIFIED: display names follow the script's mechanical pattern exactly — the fragment's first record is `quest_bone_pickers_01` / "Bone Pickers - Stage 1", which is precisely the naming `rewrite.py` derives (theme name from id, "Stage N" suffix). The provenance finding (Volume 31) is confirmed at the display-name axis across the visible set.
- VERIFIED: the fragment's `briefing` prose is authored-depth, not canned — "A lone wanderer is caught in our snare traps. His leg is shattered. The Guild demands we don't waste a bullet—just take his boots and leave him for the ash-hounds." No line in the fragment's briefings comes from the script's 23-line pool, and this is expected: `rewrite.py` never wrote briefings. The briefings were always separately authored; the script's product is the choice-level advance/abort/stage texts.
- VERIFIED (negative result, honestly reported): zero hits for any of the 23 canned lines within the readable fragment — and zero `label`-class choice fields visible, meaning the fragment ends before the choice arrays of the visible records. The canned lines, if they survive, live in the unread 83 percent.

## 33.2 Census verdict (labeled, not extrapolated)

The fragment supports three verified conclusions and one bounded unknown:

1. VERIFIED: display-name provenance — the mechanical naming survives in the live catalog.
2. VERIFIED: briefing prose is authored and was never generated; A-41's scope must exclude briefing fields (they are not templatized).
3. HIGH CONFIDENCE: the choice-level canned lines exist in the unread portion in the script's exact vocabulary, because the script's output was committed (the catalog it rewrote is the live 194,389-byte file) — but the per-line reuse histogram is UNMEASURED until segment reads cover the choice arrays.
4. The A-41 census therefore completes with segment reads targeting the choice fields specifically: score each faction-type record's advance/abort/stage text against the 23-line pool (exact match after theme substitution), publish the line-usage histogram, and rank tranches by reuse count. The mechanical instrument is unchanged; the fragment's contribution is the verified scope correction (briefings excluded, display names included).

## 33.3 Census output for the deep maps

The fragment census also fixes the record shape for this family: `id`, `display_name`, `type` (`faction_chain`), `briefing`, `prereq_quest_id`, `min_day`, and (beyond the fragment, per the script's outputs) the choice and stage text fields. The display-name de-templatization question joins the choice-prose question in A-41's scope: "Bone Pickers - Stage 1" is a functional label, not house prose, and the tranche program should decide per-record whether the mechanical name stays (legible stage labeling) or gains an authored display title. That is a design question for the foreman, not a default — recorded as A-41's open design premise.

---

# VOLUME 34 — DR-18 CLOSED: THE DEBT AND QUARANTINE LEDGERS READ (Factory batch 2026-09-24-F)

## 34.1 KNOWN_DEBT.md (read in full, 29,083 characters)

- VERIFIED: `KNOWN_DEBT.md` is a governed ledger with a five-state status vocabulary, defined in its own footer: ACCEPTED (known, intentionally deferred, not repeatedly rediscovered), BLOCKED (valid need lacks an API, content authority, decision, or dependency), QUARANTINED (preserved outside active compilation/runtime with evidence), RETIRED (historical behavior that must not be restored), PROMOTED (approved for a named active package). Every row carries an evidence pointer, owner role, and promotion condition.
- VERIFIED: the ledger holds at least 50 distinct DEBT-ids spanning the plan-debt families: `DEBT-PLAN24-MEDICAL-WARD-STAFFING` (DR-06's RETIRED record is consistent with the ledger's vocabulary), `DEBT-PLAN125-SOFC-INVENTORY-FUEL` (F-002's premise anchor), `DEBT-PLAN34-DIFFICULTY-CHRONICLE-AUTHORITY`, `DEBT-STANDING-FAILURES-2026-09-17`, `DEBT-TEST-QUARANTINE-*` (four test-quarantine families: HOLDFAST-NPC, UTILITYAI, ARCHIVEINKS, AUTOPSYPROCEDURES), `DEBT-UNITY-LEGACY`, `DEBT-PLAN-SPRAWL`, and the Plans 166–199 implementation-queue family (~15 rows).
- VERIFIED: the visible status distribution in the body read: RETIRED 49 mentions, SEALED 4, with the five-state vocabulary governing. The tail records the 2026-09-12 worktree-deletion archive: 2,506 files extracted from git HEAD and archived with full project-relative paths, SHA256 sums, and an ARCHIVE_NOTE under the Twin quarantine directory (verified 0 mismatches), plus 7 orphan forensics reports archived to `docs/archive/forensics/2026-09-12/`, SPDX headers added to 1,867 C# files, and `license-header-check.sh --strict` passing.
- DR-18 resolution: the PR #49 "51 stale quarantine entries / Plan 34 truth" cleanup is CONFIRMED as historical — the ledger as read is the post-cleanup governed state, and the v1.0 bible's "51 active quarantines" figure is stale exactly as DR-18 predicted. DR-11's KNOWN_DEBT seals remain valid in substance (they concern specific items, not the count). The row is CLOSED; no factory plan cites a quarantine count.

## 34.2 `scripts/ci/quarantine.json` (read in full)

- VERIFIED: the file is NOT the compile-remove quarantine list — it is the flake-quarantine registry (Plan VIII Task 24.7): a quarantined gate still RUNS; a quarantined failure is flagged QUARANTINED in the summary and JSON report and does not by itself fail the run. Rules: maximum duration 14 days from added date; every entry needs owner, reason, and tracking note.
- VERIFIED: `protected_gates` — Core-invariant gates that can never be quarantined, five listed: `build_core_tests`, `test_core_suite`, `build_godot_host`, and the remainder of the protected set. This is a structural CI invariant the factory's deep maps lacked: the three build/test gates are quarantine-proof by policy.

New drift entry recorded:

**DR-31 (new) — Two distinct quarantine concepts, both governed. VERIFIED.** The repository operates (a) `KNOWN_DEBT.md`'s QUARANTINED status for preserved-out-of-runtime work (with the Twin worktree archive as its evidence store) and (b) `quarantine.json`'s flake-quarantine registry for CI gates (14-day max, protected-gate exemption). Any factory plan that says "quarantine" must name which instrument it means. The B-01 shelter-failure quarantine (quarantined wiring) is instrument (a); any CI-flakiness mitigation is instrument (b). DR-05's `rewrite.py` archive recommendation routes through instrument (a)'s evidence discipline (paths + checksums + archive note).

---

# VOLUME 35 — CORRECTED CLUSTER DEEP MAPS (ASH-EXP-7 FLAGSHIP) (Factory batch 2026-09-24-F)

The ASH-EXP-7 charter's flagship: the deep maps, corrected with confirmed paths from Volumes 25 through 34. What follows is the corrected map layer the charter demanded — every entry confirmed against the live tree this session or in the prior two waves, with the source of confirmation named per cluster. This is the factory's new reference layer; the v1.0 maps it corrects are retired to historical status.

## 35.1 C1 — Shelter operations

Owners (VERIFIED, `Assets/Ashfall.Core/Shelter/` unless noted): `ShelterRoomCatalog.cs`, `ShelterThermalSystem.cs`, `ShelterScheduleSystem.cs` (+`CatalogLoader`), `ShelterFireHazardSystem.cs`, `ShelterDecorSystem.cs`, `ShelterPrisonerSystem.cs`, `SanitationSystem.cs` (+`SanitationFacilityCatalog`, `SanitationConsequenceRules`), `ShelterAtmosphereSystem.cs`/`ShelterNoiseSystem.cs` (DR-21), `AirlockSecuritySystem.cs` (Core top level), `DecontaminationSystem.cs` (+`DeconProtocolCatalogLoader`), `ShelterArchiveSystem.cs`, `ShelterSecuritySystem.cs`, `ShelterSocialDynamicsSystem.cs`, `ShelterWorkshopSystem.cs`, `ShelterAssignmentSystem.cs` (+`ShelterAssignmentSave`), `ShelterShieldingModel.cs`, `TrophySystem.cs`, `CryoVaultSystem.cs`, `CascadeCoordinator.cs` (+`CascadeRuleCatalog`), `PneumaticDispatchSystem.cs`, `FluidLogisticsSystem.cs`. Cascade and quarantine: B-01's quarantined failure-effects wiring is instrument-(a) quarantined (DR-31); `cascade_rules.json` is the data seam. Panels: atmosphere and noise are panel-visible (DR-21, Volume 30).

## 35.2 C2 — Medical pipeline

Owners (VERIFIED): `Disease/`, `Dose/` (Core dirs); `DoseLedgerSystem.cs` (+`DoseLedgerSave`, `DoseRegistersCatalog`, `DoseContentCatalog`, `DoseQuestMigration`), `AutopsySystem.cs` (+`AutopsyProcedureCatalogLoader`), `PharmaLabSystem.cs`, `MentalHealthCrisisSystem.cs`, `SickListSystem.cs`, `DwellerMedicalCatalog.cs` (Narrative/), `MedicalPathologyCatalog.cs` (Narrative/), `MedicalTriage` and `Medical` host partials (`src/Main.Medical.cs`, `src/Main.MedicalTriage.cs`), `WaterborneExposureRules.cs` (radiation/exposure class). Ward staffing: Plan 24 CLOSED, DEBT-PLAN24-MEDICAL-WARD-STAFFING RETIRED (confirmed in the ledger read, Volume 34).

## 35.3 C3 — Water, food, agriculture

Owners (VERIFIED): `WaterTreatmentSystem.cs`, `BrineWaterSystem.cs` (+`BrineWaterHeadlessDemo`), `AtmosphericCondenserSystem.cs` (condensers), `DeepWellSystem.cs` (+`DeepWell` host partial), `KitchenNutritionSystem.cs`, `GrainProcessingSystem.cs`, `FoodPreservationSystem.cs` (+`FoodPreservationCatalog`), cultivation families: `HydroponicBiomeSystem.cs` (+`HydroponicCropCatalog`), `AeroponicsSystem.cs`, `AquaponicsSystem.cs`, `Greenhouse/` (Core dir), apiculture: `ApicultureBeeCatalog.cs` (Narrative/). Data seams: `dive_sites.json`, `hydroponic_crops.json` (DR-04; F-012's sweep premise).

## 35.4 C4 — Power and industry

Owners (VERIFIED, the engine families of `Shelter/`): `PowerGridSystem.cs` (+`PowerGridSave`, `ShelterPowerGridCatalog`, `PowerDistributionSubgridSystem`, `PowerSubgridCatalog`), `SofcElectrochemistryEngine.cs` (+`SofcPowerCatalog`), `SolarConcentratorEngine.cs`, `KineticStorageSystem.cs`, `GeothermalAquiferSystem.cs` (+`GeothermalCatalog`, `GeothermalOrcSystem`, `GeothermalAquiferState`), `CupolaFoundryEngine.cs` (+`CupolaFoundryCatalog`, `CrucibleFoundryCatalog`), `CvdDiamondSynthesisEngine.cs` (+`CvdDiamondCatalog`), `EbPvdCoatingEngine.cs` (+`EbPvdCoatingCatalogLoader`), `FischerTropschSynthesisEngine.cs` (+`FischerTropschCatalog`), `ChlorAlkaliSynthesisEngine.cs`, `PlasticPyrolysisSystem.cs`, `CryoVaultSystem.cs`, `CryogenicAirSeparationSystem.cs`, `PrecisionBroachingCatalog.cs`, `PrecisionMetrologySystem.cs`, `PrecisionOpticsEngine.cs`, `BioFermentationEngine.cs`, `CarbonCompositeEngine.cs` (+`CarbonCompositeCatalog`), `MaterialShieldingSystem.cs`, `NuclearCoreLifecycleSystem.cs` (+`NuclearCoreCatalog`), `OrbitalHarrowCatalog.cs`. Metrology: `LowBackgroundMetrology` host partial (confirmed, Volume 24 listing era); hydraulic extrusion: `Main.HydraulicExtrusion.cs` host partial. Difficulty binding: FP-B06 waits on the W1 seal, per Volume 21.

## 35.5 C5 — Expeditions and travel

Owners (VERIFIED): `ExpeditionVehicleSystem.cs`, `DiscoveryConsequenceSystem.cs` (`Expeditions/`), `District8DeepCoastSystem.cs`, `IceRoadSystem.cs`, `WaystationSystem.cs`, `TravelingCaravanSystem.cs`, `TravelEncounterSystem.cs` (+`TravelEncounterCatalog`, `TravelEncounterSelectionContext`), `MicroLocationEncounterLoader.cs` (+`MicroLocationHazardRegistry`), `ExcavationSystem.cs` (+`Excavation/` dir), `SumpFloodingSystem.cs` (+`SumpDrainageCatalog`), `LandmarkDegradationSystem.cs`, `LocationEvolutionSystem.cs`. Host: `src/Main.Expeditions.cs`, `src/Host/ExpeditionHostSession.cs`; demos: DeepCoast, IceRoad, TravelEncounter, TravelingCaravan (all verified).

## 35.6 C6 — Map and geography

Owners (VERIFIED): `WastelandMapSystem.cs` (`World/`, Volume 29 section 29.2), `CartographySystem.cs` (`Exploration/`), `WastelandCartographyCatalog.cs` (Narrative/), `InSarMapping` host partial, `GeodeticSurveyHostSession` (host surface, carried from v1.0). Contract documents: `MAP_EVOLUTION_CONTRACT.md`, `DAMAGED_MAP_ZONE_AUDIT.md`, `REGIONAL_CONTROL_MATRIX.md` (all verified by search). Flooded-route tags remain GATE (DP-01).

## 35.7 C7 — Factions and war

Owners (VERIFIED): `Factions/`, `Warlords/`, `Diplomacy/` (this wave: `DiplomaticSummitSystem.cs` + `DiplomaticTreatyCatalog.cs`), `Treaties/`, `FactionEmbargoLedger.cs`, `RegionalTreatySystem.cs` (+`RegionalTreatyFeed`, `RegionalTreatyCatalogLoader`), `Propaganda/` (psyops), `Muster/` (Core dir + host partial + `Muster` src dir), `StandingRecord/` (Core dir), `CensusClaimSystem.cs`, `CrossingArbitrationSystem.cs` (+`CrossingCatalog`, `CrossingSession`). Communiqué board: `FactionCommuniqueBoardPanel.cs` live with route, dashboard entry, selftest, and registration (DR-16, Volume 29). War-chain emitters remain GATE (DP-02, annotated with the board). Summit protocol documents: contract 32.3; summit determinism: G-09 unblocked.

## 35.8 C8 — Radio and information

Owners (VERIFIED): `Radio/` (Core + src dirs), `RadioScriptbookCatalog.cs`, `SignalIntelligenceCatalog.cs`, `GhostTransmissionCatalog.cs`, `HeliographSystem.cs`, `RumorSystem.cs` (`InformationFlow/`), `WeatherStationSystem.cs`, `WeatherSondeSystem.cs` (`World/`, this wave) with `WeatherHostSession` and `WeatherSondePanel` (Volume 30). Distress-signal content remains SEALED (`CF-P1-DISTRESS-CONTENT-SEAL`); the rescue runtime remains sealed and closed (DR-06).

## 35.9 C9 — Survivors and interiority

Owners (VERIFIED, `Survivors/` unless noted): `NeedsSystem.cs` (+`NeedsComponentStore`, `NeedsModifierStack`, `NeedsComponentParity`), `SkillProgressionSystem.cs` (+`SkillCatalogLoader`, `SkillDef`), `SurvivorLifecycle.cs`, `CohortSystem.cs` (+`CohortTuning`, `StartingCohortCatalog`), `GenerationalSystem.cs` (+`GenerationalLineageExtension`), `CaregivingSystem.cs`, `ChildDevelopmentSystem.cs`, `ExerciseSystem.cs`, `HobbySystem.cs`, `InterpersonalConflictSystem.cs`, `HiddenAgendaSystem.cs`, `MoraleContagionSystem.cs` (+`Catalog`, `Save`), `RationConflictSystem.cs`, `DesperationSystem.cs`, `ZealotrySystem.cs`, `LeadershipSystem.cs` (+`PolicySystem` in Governance/), `PsychologicalArcSystem.cs`, `SomaticFlashbackSystem.cs`, `TraumaBondSystem.cs`, `CombatTraumaSystem.cs`, `GuiltInsomniaSystem.cs`, `RelationshipDecaySystem.cs`, `SurvivorRelationsSystem.cs`, `SurvivorSocialCoordinator.cs`, `Memorial/` (Core dir + `MemorialComponentStore`/`Adapter`/`Parity`), `FinalWishSystem.cs` (+`FinalWishCatalog`, `FinalWishCatalogLoader`), `SurvivorFateSystem.cs`, `SurvivorDeathLegacySystem.cs`, `IdeologicalFrictionSystem.cs`, `MoralBranchingSystem.cs`, `PersonalBelongingsSystem.cs`, `LatentExpertAwakeningSystem.cs`, `SkillAtrophySystem.cs`, `LaborProductivity.cs`, `FitnessForDutyModel.cs`. Interiority prose systems: `PhantomMemoryEngine.cs`, `MemoryDecaySystem.cs` (`Cognition/`), `SurvivorDowntimeSystem.cs` (`Recreation/`). Cultural archive (this wave): the `Culture/` family — `CulturalArchiveVaultSystem.cs`, `CulturalArchiveTomeCatalog.cs`, `ArchiveChronicleMilestones.cs`, with `cultural_archive_tomes.json` (field shape verified, Volume 32 evidence base) and `src/Host/CulturalArchiveSaveStore.cs`.

## 35.10 C10 — Quests and moral choice

Owners (VERIFIED): `Quests/` — `DynamicQuestGenerator.cs`, `DynamicQuestlines.cs`, `PersonalQuestSystem.cs`, `QuestRuntimeCoordinator.cs`, `QuestlineMasterCatalog.cs`, `NarrativeQuestlineSystem.cs` (+`NarrativeQuestlineCatalog`); `MoralChoice/` (Core dir), `MoralChoice` host partial; questline families in data: 31 catalogs (Volume 26 size census). Provenance: `quests_faction_branching.json` display names mechanically derived (Volume 33); choice-level canned-line histogram pending segment reads. Moral-choice branching field contract: `trigger`/`discovery`/`label`/`outcome_text`/`epitaph` (Volume 31). Faction-branching field contract: `id`/`display_name`/`type`/`briefing`/`prereq_quest_id`/`min_day` + choice fields (Volume 33).

## 35.11 C11 — Economy

Owners (VERIFIED, `Economy/` unless noted): `MarketSystem.cs`, `LedgerDebtSystem.cs` (+`DebtTemplateCatalog`, `DebtConsequenceDispatcher`, `DebtConsequenceHostBridge`, `DebtBountyRecord`), `ResourceRationingSystem.cs` (consumers: `EconomyHostSession`, `MarketSystem`; plan of record Plan 215 — Volume 25), `TradeSpecialtySystem.cs` (+`TradeSpecialtyCatalogLoader`), `ContractorRosterSystem.cs`, `VoluntaryRegisterSystem.cs`, `VouchAccessSystem.cs`. Black-market actions surface: sealed by `WAVE8-PART2-C1-BLACK-MARKET-ACTIONS`; funds legs remain GATE (DP-03). `unique_item_claim_registry` : `UniqueItemClaimRegistry.cs` (Core top level).

## 35.12 C12 — Weather and Year of Ash

Owners (VERIFIED): `YearOfAsh/` (Core + src dirs), `Main.YearOfAsh.cs` host partial, `WeatherKind.cs`, `IWeatherSeverityProvider.cs` (Core top level), `WeatherStationSystem.cs`, `WeatherSondeSystem.cs`, `WildlifeSeasonalCalendar.cs` (seasonal bridge), `VinylMoraleSystem.cs` (morale-adjacent, DR-19 separation noted). Year-of-Ash window: Days 180–360 (canon, unchanged); DR-16's tick-gate uncap concerns war-arc progression on the extended-play axis (Volume 29 section 29.1) — F-002 and B-19 premise sweeps read the gate's live implementation.

## 35.13 C13 — Endgame and epilogue

Owners (VERIFIED): `Endgame/` (Core dir), `EndingsHeadlessDemo.cs`, `HoldfastEndings.cs`, `Main.Endgame.cs` + `Main.Verdict.cs` host partials, `Verdict/` (Core + src dirs), `VerdictPanel.cs`, `SurvivorFateSystem.cs` (fate class), `CeremonySystem.cs` (Narrative/), muster epilogues (`Muster/` family + `muster_epilogues.json`), standing record family (`StandingRecord/` + `standing_record_*.json`), epilogue chronicle (`epilogue_chronicle.json`, carried), 32-permutation matrix (canon; F-005's audit premise).

## 35.14 C14 — Ecology and wildlife

Owners (VERIFIED): `WildlifeMigrationSystem.cs` (+`.Live` partial), `WildlifeTrappingSystem.cs` (+`WildlifeTrappingCatalog`, `WildlifeTrappingEvents`), `WildlifeSeasonalCalendar.cs`, `WaterborneExposureRules.cs`, `Ecology/` (Core dir), `PathogenStrains` host partial + `Disease/` (pathogen class), `FaunaEntomologyCatalog.cs` (Narrative/), `WastelandBestiaryCatalog.cs`. Zoonosis bridge: canon (unchanged); crop genomes: `Farming/` (Core dir).

## 35.15 C15 — Defense and security

Owners (VERIFIED): `SkyDefense/` (Core dir), `Main.SkyDefense.cs` host partial, `SkyLayerArmorSystem.cs` (+`SkyLayerArmorCatalog`), `OrbitalHarrowTelemetrySystem.cs` (+`OrbitalHarrowCatalog`), `AirlockSecuritySystem.cs`, `ShelterSecuritySystem.cs` (boundary audit: B-31), `MaterialShieldingSystem.cs`, `Defense/` (Core dir), EMP effects: shelter EMP feed (Waves 8–12 logs, carried).

## 35.16 C16 — Progression and meta

Owners (VERIFIED): `Difficulty/` (Core dir) + `Main.Difficulty.cs`, `Research/` (Core dir), `Collectibles/` (+`CollectibleCatalog.cs`, `CollectibleDiscoveryState.cs`, `CollectibleEffectDispatcher`, `CollectibleMapProjector`, `CollectibleTutorialTracker`), `Codex/` (+`CodexEntryCatalog`, `CodexProjectionBuilder`), `Localization/` (Core + src dirs), `Mods/` (Core dir), `Settings/`, `Institutions/` (this wave: `IInstitutionAvailability`, `InstitutionAssignmentLedger`), `ApprenticeshipSystem.cs`, `LibraryStudySystem.cs` (+`LibraryManualCatalogLoader`), `ArchiveDeskSystem.cs` (+`ArchiveInkCatalogLoader`), `Onboarding/` (+`OnboardingJourney`, `OnboardingSaveState`). Versioning: three-axis authority (`ReleaseVersion.cs`, `Directory.Build.props`, `VERSIONING.md` — DR-30). XP W1: difficulty authority package, status read from the live ledger each session (DR-26).

## 35.17 C17 — Host surface and UI

Owners (VERIFIED): `src/UI/` (incl. `FactionCommuniqueBoardPanel.cs`, `WeatherSondePanel.cs`, `GameDashboardPanel.cs`), `src/Main.UiPanels.cs` + `Main.PlayerSurfaces.cs` + `Main.PanelLifecycle.cs`, `Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs`, `Main.UiTests.*` (20 suites, Volume 28), `Feedback/` service family, `BriefingCrisis` host partial, `EnvironmentalTextSystem.cs` (+`AtmosphereTextSystem`), `HostCliRegistry.cs` + `HostTestSummary.cs`, selftest manifest `docs/ci/SELFTEST_MANIFEST.json`, `VerdictPanel.cs`. Panel inventory sources: `generate-ui-panel-catalog.py` (generated), `docs/ui/PANEL_AUTHORITY_OWNERSHIP.md` (verified by search). Registration discipline: DR-21's registration-gap incident is the standing evidence for E-02/B-24 premise classes.

## 35.18 Map-layer rules

1. Every entry above is a confirmed path or a confirmed instrument; where a carried label remains (v1.0-era claims not re-verified this session), the cluster text names it as carried.
2. These maps supersede the v1.0 Parts 5.1–5.2/16 inventories and the factory's Volume 18-era amendments for enumeration purposes; the ASH-EXP-7 closeout condition (corrected deep maps published as the wave artifact) is satisfied by this volume.
3. The maps are joinable to the verification cookbook (Volume 28): each cluster's owners name the demos, suites, and gates that verify work against them.
4. Drift discipline: any future session finding a divergence corrects the map in its own volume with a DR entry — the maps are maintained, not canonical.

---

# GROWTH LEDGER UPDATE (this wave)

- 2026-09-24 — Volume 32: the four lifted prose contracts authored against verified state vocabularies — cultural-archive restoration log, microfiche/acetate condition manifest, summit protocol document, salon chronicle entry — each grounded in the read state keys of `CulturalArchiveVaultSystem.cs` and `DiplomaticSummitSystem.cs` and the verified field shapes of `cultural_archive_tomes.json` and `regional_treaties.json`; RB-QUANT and RB-KNOWLEDGE bindings stated per contract — ~9,500 — cumulative ~494,000
- 2026-09-24 — Volume 33: A-41 Tranche 0 fragment census — 98 records visible, 34 faction-type; display-name mechanical provenance confirmed live; briefing prose verified authored (script never wrote briefings — scope corrected); zero canned-line hits in the readable 17 percent with the choice arrays beyond the boundary, so the line-usage histogram keeps its segment-read step; A-41's scope narrowed to choice-level texts plus the display-name design premise — ~5,000 — cumulative ~499,000
- 2026-09-24 — Volume 34: DR-18 closed — KNOWN_DEBT.md read in full (governed five-state ledger, 50+ debt ids, the 2026-09-12 worktree archive with 2,506 files and checksums, post-cleanup state confirmed); `quarantine.json` read in full (flake-quarantine registry, 14-day rule, five protected gates); DR-31 recorded (two distinct quarantine instruments); the last PENDING RB-DRCONFIRM row retired — ~5,500 — cumulative ~504,500
- 2026-09-24 — Volume 35: the corrected cluster deep maps, C1 through C17, published with confirmed paths from the three access sessions — the ASH-EXP-7 charter's flagship artifact, with map-layer rules, carried-label discipline, and cookbook joinability — ~15,500 — cumulative ~520,000

RB-DRCONFIRM is now complete with zero open rows. The ASH-EXP-7 charter's gate is satisfied. The factory's reference layer (deep maps, verification cookbook, prose contracts, harness specs, runbooks, drift register) is now fully working-tree-anchored.

Recommended next wave: Volume 36 (the prose-contract supplement's next tranche: the A-41 choice-field contracts for faction-branching, derived from the choice field shapes the segment reads reveal, plus the moral-choice-branching outcome/epitaph contract extension); Volume 37 (E-11 executed as a plan: communiqué board snapshot and a11y coverage, the first E-lane plan against a fully confirmed surface); Volume 38 (B-26 executed: the institution-availability interaction audit against `ApprenticeshipSystem` and `CohortSystem`); Volume 39 (the refreshed Part III matrices: cell confidences updated against the confirmed deep maps, SEALED/BLOCKED/GATE states re-checked against the live ledger); and if the foreman authorizes implementation, the two smallest verified findings (SB-08's gate-count drift guard; F-009's `rewrite.py` archive) are ready for execution waves.

---

# VOLUME 36 — PROSE-CONTRACT SUPPLEMENT: THE CHOICE-FACING CONTRACTS (Factory batch 2026-09-24-G)

This tranche extends the Part 9 library for the two quest families whose field shapes the census waves fixed: the moral-choice branching family (Volume 31's field contract) and the faction-branching family (Volume 33's shape, with the choice fields named from `rewrite.py`'s verified outputs). Standing rules unchanged; the RB-SEALGUARD applies to the distress-adjacent moral-choice family (no signal scenarios, no sealed vocabulary).

## 36.1 Moral-choice outcome_text contract (extension of the Volume 31 field contract)

```text
prose_field: outcome_text (moral_choice_quests_branching family)
purpose: the world's answer to a resolved moral choice — what the shelter, the survivor,
  or the faction record as having happened
trigger: choice resolution (each choice in choices[] carries one)
length: 50-110 words
viewpoint: third person, consequence register, house restraint
must_include: one concrete consequence (a ration count, a relationship change, a
  standing fact), one residual state the epilogue could reach (the outcome feeds the
  moral_delta and set_flag machinery — the prose must be consistent with both, per
  RB-QUANT), one open thread left deliberately unresolved
must_not_include: moral verdicts on the player (the deltas carry judgment; the prose
  carries the world), foreshadowing of events the flag ledger cannot yet support
model (for a mercy choice, consistent with flag_shared_rations-class flags):
  The eleven at the gate became nine by the second week; the rain took the rest.
  Rations ran thin enough that the kitchen posted the count publicly rather than
  argue it in private. Nobody says the word for what turning them away would have
  been, and nobody has said the word for what it cost to keep them either. The
  ledger's arithmetic is done. What the two of them remember is not.
```

## 36.2 Moral-choice epitaph contract

```text
prose_field: epitaph
purpose: the record-keeping line the death ledger might carry if the choice's flag
  resolves against a survivor — the shortest prose field in the family and the one
  the epilogue matrix is most sensitive to
trigger: choice resolution with lethal-risk flags
length: 15-35 words
viewpoint: memorial register, flat
must_include: one name or role, one fact from the outcome (never a new fact), the
  register's restraint
must_not_include: sentimentality (the memorial prose family carries grief; the epitaph
  carries fact), any information the pre-Reckoning window forbids
model:
  Kept the gate open in the third week of rain. The count at the kitchen says
  what that cost. The count is what remains.
```

## 36.3 Faction-branching choice-text contract (the A-41 target genre)

The field shape derives from `rewrite.py`'s verified outputs (advance text, abort text, stage text) and the fragment census (Volume 33); the per-record choice field names keep their verification step inside the segment read, and this contract binds regardless of the field naming because it contracts the prose class:

```text
prose_field: faction choice texts (advance / abort / stage, per faction-type record)
purpose: the command-giver's voice for the order and the countermand, and the scene
  between them
trigger: faction-chain choice presentation
length: advance 15-40 words; abort 15-40 words; stage 30-60 words
viewpoint: command voice for advance and abort (imperative, faction-flavored);
  scene voice for stage
must_include (advance): one operational justification grounded in the faction theme
  (the Bone Pickers' economy of waste, the Blood Tithe's arithmetic of deterrence —
  the themes are the four verified ones plus generic Command); 
must_include (abort): one cost the refusal names (the theme's danger line may supply
  it, but not verbatim — the canned pool's exact 23 lines are banned from new tranches
  by this contract, which is the A-41 acceptance criterion stated as an authoring rule)
must_include (stage): one sensory anchor and one squad-observation beat; the theme's
  expectation line paraphrased at most, never repeated verbatim
must_not_include: any of the 23 canned lines (the RB-QUANT provenance rule, made a
  contract clause), theme names outside the verified five
model (advance, Bone Pickers theme):
  Take the boots and log the salvage. The Guild counts waste twice — once in
  what we spend and once in what we leave behind — and it bills for both.
model (abort, same theme):
  Boots stay on the body. Walk him to the crossing instead. The Guild reads
  mercy as inventory mislaid, so mislay it in a direction they can't audit.
```

## 36.4 Contract-family note

These three join the Volume 32 set as the library's quest-facing tranche. The Part 9 genre list remains closed: new prose needs route through the field-contract process with a consuming loader, per the standing rule.

---

# VOLUME 37 — E-11 AS AN EXECUTED PLAN SPECIFICATION (Factory batch 2026-09-24-G)

Volume 30 ungated E-11 (communiqué board snapshot and a11y coverage). The premise reads are now done; this volume publishes the plan specification with verified premises in place of open ones.

## 37.1 Verified premise base (read this session)

- VERIFIED: the board's selftest is registered in the live selftest manifest (`docs/ci/SELFTEST_MANIFEST.json`, schema 1.0.0, `total_tests` 127, `headless_test_count` 125 — the manifest declares its own counts, which SB-08's guard should also target as a second countable authority).
- VERIFIED (material finding): `docs/ui/PANEL_AUTHORITY_OWNERSHIP.md` (11,270 characters, derived from revision `87b199b2`, dated 2026-09-15) does NOT list the communiqué board — the doc predates the board's merge (2026-09-19, DR-16). The board has a selftest registration but no authority-ownership registration. This is precisely the DR-21 registration-gap class (the atmosphere/noise panels shipped with the same gap and needed a follow-up repair), now verified as recurring on a second panel.
- VERIFIED: the ownership doc's own rule states the invariant this gap violates: "A player-routed panel may only read/write the same instance the campaign day loop ticks and the save system captures. Panels never construct campaign authorities at bind time." The board's authority line (FactionWarChainRunner.Catalog, per DR-16) is absent from the table that enforces this.
- VERIFIED: the doc carries a "Known flagged items (foreman decisions needed, not fixed here)" section — the sanctioned place unresolved ownership questions go.

## 37.2 Plan specification — E-11: communiqué board surface coverage

Tier: DOCS-ONLY registration first (the ownership-table row), then HOST-WIRING tests (snapshot and a11y).

Seams, in order: (1) the ownership-table row — board panel name, its authority (the war-chain catalog instance), its route (`faction_communique_board`), its save relationship (read-only projection, per the DR-16 design); (2) snapshot coverage — the board in empty, populated, and truncated-content states, through the established snapshot gate surface; (3) a11y coverage — words-not-color-only and controller parity through the established a11y selftest class; (4) the E-02-class briefing row if the briefing surface enumerates panels.

Save impact class: NONE (the board is a projection; no save section is added). Determinism impact: NONE (the panel reads catalog state; no RNG).

Verification class: the docs_index_drift gate after the ownership-table edit; the snapshot gate with the three board states captured; the a11y selftest; the existing board selftest remains green (regression).

Open premises: (1) whether the snapshot surface has an existing board fixture (read the snapshot directory in the owning session); (2) whether the briefing enumeration (E-01's surface) already absorbed the board — enumerate before adding a duplicate row.

## 37.3 Standing finding recorded — the registration-gap class is now a pattern

DR-21 (atmosphere/noise panels, registration gap, follow-up repair) and this volume's board finding establish a two-incident pattern: panels merged through the war-chain and shelter waves did not automatically update `PANEL_AUTHORITY_OWNERSHIP.md`. A Lane H candidate follows naturally and is recorded:

**NEW SEED — H-06 · C17 · Panel-ownership drift check.** Subject: a small check that every registered player-facing panel (from the generated panel catalog) has a row in `PANEL_AUTHORITY_OWNERSHIP.md`, failing on divergence — the E-11 finding is its first verified motivating incident, and DR-21 is its second. Route: TOOLING, mirroring the existing drift-gate family (`ui_panel_catalog_drift` is the established pattern; this check adds the ownership table as a compared surface). Confidence: HIGH CONFIDENCE (two verified incidents; the generated catalog and the ownership doc are both confirmed live).

---

# VOLUME 38 — B-26 EXECUTED: THE INSTITUTION-AVAILABILITY INTERACTION AUDIT (Factory batch 2026-09-24-G)

Volume 30 ungated B-26 with its counterparties named. The audit reads are done; the findings follow.

## 38.1 What was read (VERIFIED)

- `Assets/Ashfall.Core/Institutions/InstitutionAssignmentLedger.cs` — read in full (2,214 characters). Its entire public surface is three methods: `IsAvailable(...)`, `TryClaim(...)`, `Release(...)`. This is a deliberately minimal one-live-claim-per-survivor port implementation.
- `Assets/Ashfall.Core/ApprenticeshipSystem.cs` — read in full (21,126 characters). Public surface: `SystemId` "apprenticeship", catalog `apprenticeship_catalog.json`, actions `RegisterMentorship`, `StartPair`, `CancelPair`, `StartTranscription`, `RegisterWill`, `ExecuteWill`, `NotifyMentorDeath`, `TickDay`, state capture/restore; events `OnApprenticeshipCompleted`, `OnApprenticeshipChanged`, `OnManualTranscribed`, `OnWillExecuted`, `OnMentorLegacyInherited`. Its constructor dependencies: `ISeededRng`, `SkillProgressionSystem`, `DutyRosterSystem`, `SurvivorRelationsSystem`, `ShelterAssignmentSystem` (optional), `InventoryContainer` (optional).

## 38.2 Findings

**Finding 1 — Apprenticeship does not route through the institution ledger. VERIFIED (absence, from a full source read).**
`ApprenticeshipSystem` contains zero references to `IInstitutionAvailability` or the ledger. A mentor-apprentice pair consumes survivor time (it coordinates with `DutyRosterSystem` and `ShelterAssignmentSystem`), but the one-live-claim port is not consulted when a pair starts. Classification per the false-positive control: this is a measured structural fact, but whether it is a defect is UNVERIFIED — it is a design-intent question. If apprenticeship is meant to be an institution-class claim (competing with the cultural archive's scholar slots and other institution assignments), the absence is a multiple-sources-of-truth defect; if apprenticeship is deliberately outside the institution family, the architecture is coherent and the finding closes as a no-change area with a one-line documentation note. The disposition belongs to the foreman. Recorded as: HYPOTHESIS pending an intent ruling; the audit's contribution is that the question is now precisely posed with a full source read behind it.

**Finding 2 — The transcription seam between apprenticeship and the cultural archive is a confirmed cross-system surface. HIGH CONFIDENCE.**
`ApprenticeshipSystem` holds `transcriptionTasks` and exposes `StartTranscription`/`OnManualTranscribed` (survivorId, manualItemId). `CulturalArchiveVaultSystem` (Volume 32's evidence base) emits `transcribing`/`transcribed`/`no_scholar`/`scholar_unavailable` states for tome transcription. The two transcription surfaces are adjacent by vocabulary; whether they are the same pipeline (the vault routes transcription through apprenticeship's task list) or two parallel transcription authorities is UNVERIFIED — the vault's call sites were not opened this session. If parallel, this is a C9-family ownership-boundary question of exactly the class the factory exists to surface. A-31's premise sweep (Volume 30) now carries this check explicitly: read the vault's transcription call path before authoring restoration-log prose that assumes either pipeline.

**Finding 3 — The wills subsystem is a confirmed adjacent authority, not an institution. VERIFIED by structure.**
`ApprenticeshipSystem` also holds `registeredWills`/`executedWills` with `SurvivorWill` records (testator, primary and fallback beneficiaries, bequeathed item ids) — a succession authority living inside the apprenticeship system. The deep map (Volume 35, C9) gains this: wills and legacy traits (`legacyTraitsGranted`, `OnMentorLegacyInherited`) are apprenticeship-owned. No action proposed; the map correction is the output.

## 38.3 Audit disposition

B-26's plan shape closes as: one HYPOTHESIS routed to the foreman (Finding 1's intent ruling), one premise check added to A-31's sweep (Finding 2), one deep-map correction applied (Finding 3). This is the correct outcome for an audit-class plan: no code change proposed, three verified outputs, zero invention.

---

# GROWTH LEDGER UPDATE (this wave)

- 2026-09-24 — Volume 36: the choice-facing prose contracts — moral-choice outcome_text and epitaph (extending Volume 31's field contract with worked models consistent with the flag/delta machinery) and the faction-branching choice-text contract with the 23 canned lines banned as a contract clause (A-41's acceptance criterion stated as an authoring rule) — ~6,000 — cumulative ~526,000
- 2026-09-24 — Volume 37: E-11 executed as a plan specification with verified premises — the board's selftest registration confirmed in the live manifest (127 tests, 125 headless), the material finding that `PANEL_AUTHORITY_OWNERSHIP.md` (revision-dated 2026-09-15) predates and omits the board, the registration-gap class established as a two-incident pattern, new seed H-06 (panel-ownership drift check) recorded — ~5,500 — cumulative ~531,500
- 2026-09-24 — Volume 38: B-26 executed — `InstitutionAssignmentLedger` read in full (three-method minimal port), `ApprenticeshipSystem` read in full (zero institution references — the intent question precisely posed to the foreman), the transcription seam between apprenticeship and the cultural-archive vault confirmed as adjacent vocabulary and routed into A-31's sweep, the wills subsystem mapped to C9 — ~5,500 — cumulative ~537,000

Recommended next wave: Volume 39 (the refreshed Part III matrices — cell confidences re-labeled against the confirmed deep maps, the four new seeds slotted, SEALED/BLOCKED/GATE states re-checked against the live ledger); Volume 40 (the A-31 premise sweep: the vault's transcription call path read, closing Finding 2 of Volume 38); Volume 41 (the C9 ownership-boundary question pack for the foreman: the apprenticeship intent ruling, the display-name design premise from Volume 33, and any decision items the ledger's current batch names); then the implementation-side waves (SB-08, F-009, H-06, and E-11's docs-first tranche) remain available on foreman authorization.

---

# VOLUME 39 — THE REFRESHED GENERATOR MATRICES (Factory batch 2026-09-24-H)

The Part III matrices carried audit-era confidences; the deep maps are now confirmed (Volume 35), the confirmation runbook is closed (Volumes 25, 29, 34), and the live ledger's current batch has been read (this session: XP Expansion W1 ACTIVE, 2026-09-18, with the SOFC premise corrected by source evidence; DISTRESS-SIGNALS-9-12 COMPLETE and presented for acceptance; a "Next waves (planned, not yet claimed)" section present). This volume re-labels the matrix cells that moved and slots the new seeds. Cells unchanged in confidence are not restated; this is a delta volume, and the Part III originals remain the base until a full matrix reprint is warranted.

## 39.1 Lane-level state changes

- Lane A: five new seeds since the base matrices (A-31 cultural-archive corpus, A-32 summit protocol documents — both now HIGH CONFIDENCE with contracts authored in Volume 32; A-41 faction-branching de-templatization — HIGH CONFIDENCE with the Volume 33 scope correction). The C8 distress cell remains SEALED with its guard now encoded as a contract clause (Volume 36). The C10 cells re-rank: FP-A25 drops behind A-41 (proven-provenance debt outranks unmeasured debt), FP-A24 is closed (Volume 26).
- Lane B: FP-B14's premise surface is now located (`SpiritualModels.cs`, Volume 25); FP-B06 waits on the W1 seal, which is confirmed ACTIVE-not-sealed in the live ledger head; B-26 executed with its findings routed (Volume 38). The C6 and C7 GATE cells (DP-01, DP-02) remain signature-blocked; DP-02's annotation now names the live board as its rendering surface (Volume 29).
- Lane C: unchanged in gate structure; the six ASH-EXP-6 harnesses have their supplement (Volume 27) and their owner-class bindings (Volume 35). The lane rule stands: no number change in a first tranche.
- Lane D: D-05's triggering release (1.1.0) is confirmed at the build axis (Volume 29); the save-support-window gate is confirmed in the live manifest (`save_support_window`, Volume 29's inventory).
- Lane E: E-11 is executed as a specification (Volume 37) and is the lane's next plannable tranche; E-01's enumeration must include the institution and cultural families (Volume 30) plus the sanatorium (Volume 40).
- Lane F: unchanged; no profiling evidence has landed since the base matrices.
- Lane G: G-09 is ungated and plannable (Volume 30); G-10's DR-20 gate is passed, leaving only the test-inventory read.
- Lane H: SB-08's design input is complete (Volume 29); H-06 is new (Volume 37, two verified incidents); F-009 is resolved to a placement decision with its archive discipline identified (Volumes 29, 34 — instrument (a), the KNOWN_DEBT evidence store pattern).
- Lane I: the ASH-EXP-7 flagship is satisfied (Volume 35); F-007's registration gains the gaps/incidents classification (Volume 43) and the ANTIGRAVITY.md and C-integration-plans additions (DR-24).
- Lane J: unchanged except that onboarding rows for confirmed DR-20 systems (J-06) are ungated at their first gate.

## 39.2 Cell corrections requiring matrix annotation

| Cell | Base label | Corrected label | Evidence |
|---|---|---|---|
| A/C8 radio programming | HIGH CONFIDENCE | HIGH CONFIDENCE, SEALED-adjacent guard encoded | Volume 36 contract clause bans sealed vocabulary; `RB-SEALGUARD` permanent |
| A/C10 quest prose | HIGH CONFIDENCE (FP-A25 largest) | Corrected: record count ≠ size; moral-choice branching is the largest by bytes, faction-branching carries proven provenance debt | Volumes 26, 31, 33 |
| B/C16 difficulty binding | HIGH CONFIDENCE, sequence-gated | Unchanged; W1 confirmed ACTIVE (2026-09-18) in live ledger head with `XP-WAVE1-DIFFICULTY-AUTHORITY` as the first owned package and corrected SOFC premise | Ledger head read, this session |
| B/C1 shelter failure effects | HIGH CONFIDENCE | Unchanged; instrument named: the quarantine is concept (a) per DR-31; `cascade_rules.json` seam confirmed | Volumes 34, 35 |
| C/all balance | as specified | Harness fixtures must record `schema_version` uniformly per DR-30 | Volume 29 |
| E/C17 panels | HIGH CONFIDENCE | Strengthened: the registration-gap class has two verified incidents (DR-21; Volume 37's board finding) | Volumes 37 |
| H/CI gate counts | PROPOSAL | HIGH CONFIDENCE, implementable: manifest read in full, 57 gates, count authority named | Volume 29 |
| H/data authority | VERIFIED finding, PROPOSAL handling | RESOLVED: no tracked caller, archive recommended through instrument (a) discipline | Volumes 29, 34 |
| I/root registration | VERIFIED need | In execution: classifications accumulating per artifact (Volumes 29, 34, 43) | this volume |
| J/C16 onboarding | HIGH CONFIDENCE | J-06 ungated at gate one; enumeration includes sanatorium | Volumes 30, 40 |

## 39.3 New slots

A-41 (Volume 31), H-06 (Volume 37), and the Volume 43 gap-derived seeds slot into their cells. The matrix's combinatorial claim (170 cells, multiple plans per cell over time) is unchanged; the factory's live seed inventory after this volume: 12 flagship plans, 60+ expanded plans across Lanes A–J, 4 decision packets (3 signature-blocked, DP-04 corroborated), 8 runbooks, 8 charters, 17 cluster maps (corrected), 40+ prose and record contracts, 12 harness specifications plus the Volume 27 supplement, and 6 new seeds from Volumes 26 through 43.

---

# VOLUME 40 — A-31 PREMISE SWEEP EXECUTED: THE INSTITUTION FAMILY AND THE TRANSCRIPTION PIPELINE (Factory batch 2026-09-24-H)

## 40.1 Finding 2 of Volume 38 — CLOSED: the transcription pipeline is single, not parallel

- VERIFIED (code search, this session): `StartTranscription` has exactly five references repository-wide: `Assets/Ashfall.Core/ApprenticeshipSystem.cs` (definition), `Assets/Ashfall.Core/Culture/CulturalArchiveVaultSystem.cs` (caller), and three test files (`CampaignContinuityFlagship54_57Tests.cs`, `ShelterApprenticeshipAndWillPlan55Tests.cs`, `CulturalArchiveVaultTests.cs`). The vault calls apprenticeship's transcription; apprenticeship never calls the vault.
- Verdict: ONE pipeline. The cultural-archive vault's `transcribing`/`transcribed` states are the front of an apprenticeship-owned task engine. Volume 38's ownership-boundary question dissolves — there is no second transcription authority. A-31's restoration-log prose may reference transcription assignment through the apprenticeship surface with confidence, and the restoration-log contract's "scholar unavailable until the roster turns" model line is consistent with the verified state vocabulary.
- Residual premise for A-31: the vault's exact call contract (argument shapes, completion routing to `OnManualTranscribed`) — one call-site read in the owning session, standard depth.

## 40.2 Finding 1 of Volume 38 — evidence materially improved; the intent question narrows

- VERIFIED (code search): `IInstitutionAvailability` has exactly ten references: the port and ledger themselves, plus four consuming systems — `SkyDefense/SkyDefenseBatterySystem.cs`, `Diplomacy/DiplomaticSummitSystem.cs`, `Culture/CulturalArchiveVaultSystem.cs`, and `Sanatorium/PsychologicalSanatoriumSystem.cs` — each with its own test file, plus the flagship wiring in `src/Main.FlagshipInstitutions.cs` and the reconnaissance/plans documents.
- The institution family is therefore a named, coherent flagship-institutions group: cultural archive, summit, sky-defense battery, psychological sanatorium. Apprenticeship is not a member, and its constructor dependencies (roster, relations, skills, assignments, inventory) place it in the survivor-development family instead.
- The foreman's intent ruling (Volume 41) is now well-posed with evidence: either apprenticeship is deliberately non-institutional (coherent; closes as a no-change area with a documentation note), or pair-claim time should compete institutionally (a design change, not a defect repair). The factory's own recommendation, on this evidence: coherent as built; record, do not change.

## 40.3 New system confirmed — the psychological sanatorium

- VERIFIED: `Assets/Ashfall.Core/Sanatorium/` contains `PsychologicalSanatoriumSystem.cs` and `PsychologicalTherapyCatalog.cs`. The sanatorium is an institution-family member with a therapy catalog — a C2/C9 boundary system the deep maps lacked entirely.
- Deep-map correction (C9 and C2, amending Volume 35): the sanatorium joins the medical-adjacent interiority owners, consuming the institution ledger for one-live-claim semantics. The therapy catalog implies a data seam (`PsychologicalTherapyCatalog` — its JSON file was not individually confirmed this session; the owning session names it before any prose plan cites it).
- E-01/J-06 enumerations gain the sanatorium; the `MentalHealthCrisisSystem.cs` (C2, Volume 35) relationship to the sanatorium is a premise for any therapy-prose plan (crisis system versus therapy institution — likely producer and facility, but unverified).

## 40.4 Drift-register entries

**DR-32 (new) — The institution family is enumerated and closed-shaped. VERIFIED.** Four consuming systems plus the port and ledger; no fifth consumer exists in the tracked tree. Any future plan proposing an institution-class claim routes through `IInstitutionAvailability` and names its family membership.

**DR-33 (new) — The gaps directory is a superseded audit with a live open-gaps core. VERIFIED.** `docs/gaps/PARTIAL_PLANS_VERIFIED_AUDIT.md` (read in full this session, 8,948 characters) declares itself SUPERSEDED for its P1–P8 findings status ("the authoritative current status is KNOWN_DEBT.md") while remaining the only place the P1–P8 evidence chain (the `ashfall-scan` DECLARED → OBSERVED → PERSISTED method) is recorded in one table, plus decisions D1/D2. The factory's premise sweep treats it as a historical audit whose findings must be re-checked against `KNOWN_DEBT.md` — which Volume 34's read now enables. The re-check executes in Volume 43.

---

# VOLUME 41 — THE FOREMAN QUESTION PACK (Factory batch 2026-09-24-H)

Four questions, each with its evidence base verified, each requiring a foreman ruling the factory cannot make itself. Per the approval-based workflow, nothing below proceeds without its answer; per the factory's constitution, each question is posed with a recommendation, not a proposal to execute.

## Q-1 — Apprenticeship and the institution ledger

Question: is apprenticeship deliberately outside the institution family, or should a mentor-apprentice pair claim institutionally?
Evidence: Volume 40's enumeration — four institution members, apprenticeship's constructor family, zero ledger references in a full source read.
Factory recommendation: coherent as built; record the boundary in the institutions documentation; no code change. If the ruling is "institutional," the change is a design tranche with G-08-class consumer tests, not a defect fix.

## Q-2 — Faction-branching display names

Question: do the mechanically derived display names ("Bone Pickers - Stage 1") stay as functional stage labels, or gain authored display titles in A-41's tranches?
Evidence: Volume 33's provenance confirmation; the catalog's chain structure making stage legibility a real value.
Factory recommendation: keep mechanical names for chain stages (legibility and save/serialization stability), author the briefing and choice prose only. The de-templatization program's scope statement should encode whichever ruling holds.

## Q-3 — Incident scheduler and consequence wiring (Plan 57's open seams)

Question: does the incident surface remain a text-only read model (Case D, as sealed), or does a scheduler/consequence follow-on enter a wave?
Evidence (read this session, `docs/incidents/PLAN57_FINAL_REPORT.md`, in full): 25 incidents in `incidents.json` with schema `{ id, title, bodyText, weight, minDay }`, consumed by `src/Host/EventsHostSession.cs` as a text-only read model — no scheduler, no weighted selection, no RNG stream, no consequences, no history, and the DTO binds only the five fields, so authoring more would be dead data (forbidden). The weight field is authored as future-scheduler data.
Factory recommendation: the Case D seal is respected; the follow-on, if wanted, is a Lane B plan through the events host session with a new seeded sub-stream, and it must not casually expand runtime architecture (the report's own §4.3 discipline). The factory records the seam; it does not open it.

## Q-4 — XP W1 consumer-binding sequence

Question: which industrial consumers bind to the difficulty authority in which order, once W1 seals?
Evidence: W1 ACTIVE (2026-09-18, live ledger head), `XP-WAVE1-DIFFICULTY-AUTHORITY` as the first owned package, SOFC premise corrected (Plan 122 already consumes canonical inventory fuel through the existing port — no duplicate catalog). FP-B06 and B-23 hold the binding patterns; the `Shelter/` engine families (Volume 35's C4 map) are the consumer surface.
Factory recommendation: B-23's sweep first to establish the pattern, then FP-B06's industrial tranche ordered by the C4 map's family grouping (one engine family per tranche), each with its G-08 test in the same tranche.

---

# VOLUME 42 — IMPLEMENTATION-READINESS SPECIFICATIONS (Factory batch 2026-09-24-H)

The four smallest verified findings are now specified to the point where an authorized session executes without inventing structure. These are specifications, not implementations; no file changes are committed by the factory.

## 42.1 SB-08 — the gate-count drift guard

Instrument: a script in `scripts/ci/` mirroring the drift-gate family (`cli_catalog_drift`, `selftest_manifest_drift` are the named precedents in the live manifest).
Authority: `docs/ci/CI_GATE_MANIFEST.json` — key: `schema_version` + `total_gates` + `fast_tier_count` + the full `gate_id` set (published in Volume 29). Second authority: `docs/ci/SELFTEST_MANIFEST.json` (`total_tests` 127, `headless_test_count` 125 at schema 1.0.0 — read this session).
Behavior: scan the repository's live documents (bibles, handoffs, closeouts younger than an allowlist cutoff) for declared gate/test counts; on divergence from either manifest, fail with the divergent document, the declared number, and the manifest number.
Design constraints (from verified incidents): compare on full gate_id sets, never family-name patterns (Volume 29's false-divergence warning); historical documents are immutable records — the guard annotates or exempts them via allowlist, never forces edits (F-008's rule); the manifest's own `schema_version` history (the +2/+1 additions recorded in its header) is the model for how counts legitimately change.
Verification: inject a synthetic divergence into a scratch document and confirm failure; run green against the current tree; register in the manifest per the `selftest_manifest_drift` pattern (the guard becomes a gate that guards gate-counting).

## 42.2 F-009 — the `rewrite.py` archive

Instrument: the KNOWN_DEBT instrument-(a) evidence discipline (Volume 34): full project-relative paths + SHA256 sums + an ARCHIVE_NOTE, per the verified 2026-09-12 worktree archive pattern (2,506 files, 0 mismatches).
Steps: (1) compute the script's checksum; (2) move to `docs/archive/` or `tools/` with a one-paragraph rationale naming its product (`quests_faction_branching.json` choice prose, Volumes 29–33) and its defect (author-local absolute path, no tracked caller); (3) record the move in `KNOWN_DEBT.md` per its row rules (evidence pointer, owner, promotion condition); (4) add the data-authority hygiene assertion — `Assets/StreamingAssets/Data/` contains only `.json` plus whitelisted artifacts — as a small check beside `json-schema-policy-gate.sh`'s surface or in the existing repo-hygiene report.
Verification: the data-integrity selftest before and after; the new assertion green; the catalog the script generated unchanged byte-for-byte (the script's product is committed history).

## 42.3 H-06 — the panel-ownership drift check

Instrument: a check mirroring `ui_panel_catalog_drift` (live manifest gate), adding `docs/ui/PANEL_AUTHORITY_OWNERSHIP.md` as a compared surface.
Behavior: enumerate registered player-facing panels from the generated panel catalog; assert each has an ownership row; fail naming the missing panel.
Verified motivating incidents: DR-21 (atmosphere/noise, repaired) and Volume 37's board finding (the ownership doc, revision-dated 2026-09-15, omits the board merged 2026-09-19). The doc's own "Known flagged items" section is the sanctioned interim home for panels awaiting ruling — the check should treat a flagged entry as present-but-flagged, not missing.
First executions: the check's initial run is expected to fail on the board row; E-11's docs tranche (Volume 37's seam 1) fixes it in the same wave, and the pair closes the loop the two incidents opened.

## 42.4 E-11 — the docs-first tranche

Seam 1 of the Volume 37 specification, isolated as the first tranche: add the communiqué board's ownership row (panel name, authority: the war-chain catalog instance; route: `faction_communique_board`; save relationship: read-only projection), into the ownership table, with the derived-from revision line updated per the doc's own convention.
Verification: the `docs_index_drift` gate; H-06's check (once both land) green; no code changes.

---

# VOLUME 43 — THE GAPS AND INCIDENTS INPUTS CONSUMED (Factory batch 2026-09-24-H)

The Factory Protocol's step 1 made `docs/gaps/` and `docs/incidents/` mandatory inputs (DR-02); no prior factory session had read them. This volume consumes both, in full.

## 43.1 `docs/gaps/` — the P1–P8 re-check against the live debt ledger

`PARTIAL_PLANS_VERIFIED_AUDIT.md` (read in full) records, with the `ashfall-scan` evidence chain (DECLARED → COMPILED → CONSTRUCTED → REGISTERED → CALLED → OBSERVED → PERSISTED):

- P1 — `DEBT-189-INTAKE-ADVISORY-BRIDGE`: `WaterTreatmentSystem.RegisterContaminationAdvisory` has zero callers; `AquiferPiezometerEngine.BuildAdvisory()` never reaches it; the doc comment at the engine names the bridge. Status: UNWIRED, priority G1.
- P2 — `DEBT-176-CAMPAIGN-AGE-CLOCK`: no `CampaignAge`/`AgeClock` type exists. UNSTARTED, G2.
- P3 — `DEBT-177-SLEEP-EVENT-CONSUMER`: no `SleepEvent` type. UNSTARTED, G2.
- P4 — `DEBT-178-CREATION-TO-VAULT`: no creation authority; the vault exists only as archive. UNSTARTED, G2.
- P5 — `DEBT-182-LAST-INTERACTION-STAMP`: no survivor-relation interaction stamp. UNSTARTED, G2.
- P6 — `DEBT-188-SCHEDULE-HOUR-CONSUMER`: no `ScheduleHour` type. UNSTARTED, G3.
- P7 — `DEBT-194-CRISIS-PRODUCER-WIRE`: no `CrisisCoordinator` type; only the HUD exists. UNSTARTED, G3.
- P8 — `DEBT-198-PIPELINE-EVENT-LOG`: no `HealthHistory`/`MedicalRecord` type. UNSTARTED, G2.

Cross-check against Volume 34's `KNOWN_DEBT.md` read: all eight debt ids appear in the live ledger's id set (the Volume 34 list includes 189-INTAKE-ADVISORY-BRIDGE, 176-CAMPAIGN-AGE-CLOCK, 177-SLEEP-EVENT-CONSUMER, 178-CREATION-TO-VAULT, 182-LAST-INTERACTION-STAMP, 188-SCHEDULE-HOUR-CONSUMER, 194-CRISIS-PRODUCER-WIRE, 198-PIPELINE-EVENT-LOG). The audit's supersession note is therefore consistent: the ledger carries the items; the audit carries the evidence chain. No contradiction.

Standing-rule consequence: the audit's STALE section (verify-and-retire candidates, e.g. `DEBT-185-SKILL-DORMANCY-TICK` verified implemented via `SurvivorSocialCoordinator.TickDay` step 6) gives the factory retire-check candidates — each is one verification read in the owning session, and a retired debt id is recorded, not silently dropped.

Decisions registered from the audit: D1 — amputation/prosthetic visuals OUT OF SCOPE until a survivor-avatar owner exists (the factory respects this boundary in all C2 prose plans); D2 — `SurvivorInspectionHostSession` RETIRED and DELETED (2026-09-17) — a standing anti-hallucination note: no plan may cite that session.

## 43.2 The gap-derived seeds (all verified-unwired, all pre-admitted by the repository's own audit)

- **NEW SEED — B-33 · C3 · Intake-advisory bridge wiring (P1).** The one verified UNWIRED seam at priority G1: `AquiferPiezometerEngine.BuildAdvisory()` → `WaterTreatmentSystem.RegisterContaminationAdvisory`. Route: CORE-EXTENSION through the two named owners, additive; the doc comment is the design contract. Verification: focused test asserting the advisory reaches the water-treatment intake state; determinism unchanged. Confidence: VERIFIED finding (zero callers proven by the audit's method and consistent with the ledger), PROPOSAL handling.
- **NEW SEED — B-34 · C9 · The unstarted-type cluster (P2–P8).** Seven debt items whose types do not exist. This is not seven plans; it is a foreman-prioritization question: which of the seven maps to a wanted wave. The factory's role is to keep them visible in the backlog, not to promote them. Confidence: VERIFIED unstarted (0-file searches, audit method).

## 43.3 `docs/incidents/` — the Plan 57 Case D record

Read in full (7,051 characters): 25 incidents, text-only read model, weight field authored as future-scheduler data, five-field DTO where extra fields are dead data (forbidden by the report's §33). The report's own follow-on section holds choice-ready concepts, deliberately unwired.

- The incident surface's classification as Case D is a seal the factory respects: no scheduler, no consequence dispatch, no history schema except through a follow-on plan the foreman requests (Q-3 of Volume 41).
- **NEW SEED — A-42 · C1 · Incident prose depth continuation.** Within the sealed surface's own terms (the five DTO fields), incident prose (`bodyText`) is the authorable surface: a census of the 25 entries' depth against the incident genre's contract, then authored tranches if the census warrants. Route: DATA-ONLY. Verification: the Plan 57 test file (12 contract tests, verified) extended per tranche; `data_integrity` gate. Confidence: HIGH CONFIDENCE (surface verified; depth unmeasured).

## 43.4 Registration consequences for F-007

`docs/gaps/` classifies as: one superseded-but-evidentiary audit, one sealing plan (GAP-48-49 destination seams), and a logs directory of seal logs (four, spanning Plans 146–153) — active historical records, not clutter. `docs/incidents/` classifies as a plan closeout record. Both gain rows in F-007's registration table with these classifications.

---

# GROWTH LEDGER UPDATE (this wave)

- 2026-09-24 — Volume 39: the refreshed generator matrices — lane-level state changes, ten cell corrections with evidence, new slots (A-41, H-06, the Volume 43 seeds), live-ledger confirmation of W1 ACTIVE and the distress seal — ~6,500 — cumulative ~543,500
- 2026-09-24 — Volume 40: A-31 premise sweep executed — the transcription pipeline verified single (vault calls apprenticeship; five references repository-wide), Volume 38's Finding 2 closed, the institution family enumerated and closed-shaped (four members), the psychological sanatorium discovered and mapped (C2/C9), DR-32 and DR-33 recorded — ~5,500 — cumulative ~549,000
- 2026-09-24 — Volume 41: the foreman question pack — four questions with verified evidence bases and factory recommendations (apprenticeship intent, display names, incident scheduler, W1 consumer sequence) — ~3,500 — cumulative ~552,500
- 2026-09-24 — Volume 42: implementation-readiness specifications for the four smallest verified findings (SB-08's guard with its two count authorities and false-divergence constraint, F-009's archive through the instrument-(a) discipline, H-06's panel-ownership check with its expected-first-failure, E-11's docs tranche) — ~5,000 — cumulative ~557,500
- 2026-09-24 — Volume 43: the gaps and incidents inputs consumed in full — the P1–P8 evidence chain cross-checked against the live debt ledger (consistent; no contradiction), two new seeds (B-33 the verified-unwired intake-advisory bridge at priority G1; B-34 the unstarted-type cluster as a foreman-prioritization question), the Plan 57 Case D seal registered with new seed A-42 (incident prose within the five-field surface), D1/D2 decisions registered, F-007 classifications extended — ~7,500 — cumulative ~565,000

---

# VOLUME 44 — THE CANONICAL MATRIX REPRINT (Factory batch 2026-09-24-I)

The Part III base matrices (upload, Volumes 1–24 body) plus every correction recorded through Volume 39, merged into one canonical reference. Every cell below is either (a) carried from the base matrix unchanged, (b) annotated with a correction and its evidence volume, or (c) marked with its live state (SEALED / BLOCKED / GATE / CLOSED). Cells are the factory's candidate space; seeds name themselves against these cells. This reprint is synthesis only — no new repository reads were performed for it; its evidence citations are to the base matrices and the correcting volumes. The Part III originals are hereby superseded as the working reference; the Drift Register entries they depend on (DR-01 through DR-10, plus DR-16 through DR-33) remain the correction log.

## 44.1 Lane A — narrative and prose (canonical)

| Cell | Archetype (canonical) | State / correction |
|---|---|---|
| A/C1 | Bunker maintenance glitch batch N+1 (A-01); sanitation load-shed amendments (A-02) | HIGH CONFIDENCE; both seeded |
| A/C2 | ARS casebook phases (A-03); dose-treatment narrative pairing against the live matrix (A-04); therapist batch 4 (A-05) | HIGH CONFIDENCE; A-04 anchored to `MEDICAL_DOSE_TREATMENT_MATRIX.md` (DR-03) |
| A/C3 | Preservation/grain assay twins (A-06); cellar and silo seasonal logs (A-07); apiculture continuation (A-08) | HIGH CONFIDENCE |
| A/C4 | Hydraulic extrusion assay twin (A-09, census-gated); metrology calibration corpus (A-10); foundry pour-window logs (A-11) | A-09 catalog VERIFIED, twin absence UNVERIFIED; A-10 seam confirmed (`LowBackgroundMetrology` session) |
| A/C5 | Destination arrival/revisit completion (A-12, per-destination measurement required); waystation registers (A-13); rail-side field documents (A-14, corpus sweep pending) | A-12 contract: 80–140 / 60–110 words, one landmark, one sensory anchor, one danger indication |
| A/C6 | Damaged-zone survey marginalia (A-15) | HIGH CONFIDENCE; `GeodeticSurveyHostSession` confirmed |
| A/C7 | Standing-record testimony (A-16); verdict radio continuation (A-17); warlord communiqués (A-18) | HIGH CONFIDENCE |
| A/C8 | Numbers-station ciphers (A-19); radio rundowns (A-20) | SEALED-adjacent: `RB-SEALGUARD` clause bans distress-signal vocabulary permanently (Volume 36) |
| A/C9 | Phantom-memory triggers keyed to cohorts (A-21); final-wishes documents (A-22); intake interviews (A-23) | HIGH CONFIDENCE |
| A/C10 | Bureaucratic-morality prose (A-24); massive-expansion audit (A-25); faction-branching de-templatization (A-41) | Corrected ranking (Volume 39): A-41 first — proven provenance debt (machine-generated choice prose from 23 canned lines, Volume 31) outranks unmeasured debt; FP-A24 CLOSED (Volume 26); FP-A25 re-ranked behind A-41 |
| A/C11 | Ledger-debt statement prose (A-26) | HIGH CONFIDENCE |
| A/C12 | Storm-window almanac (A-27) | HIGH CONFIDENCE; Year-of-Ash window 180–360 canon respected |
| A/C13 | Epilogue chronicle depth (A-28, consumed by F-005) | HIGH CONFIDENCE; per-permutation measurement in session |
| A/C14 | Bestiary continuation (A-29) | HIGH CONFIDENCE |
| A/C15 | Sky-defense ordnance manifests (A-30) | INFERENCE pending corpus sweep |
| A/C16 | Codex refresh for post-wave systems | HIGH CONFIDENCE |
| A/C17 | Ambient surface prose for newer panels | INFERENCE; `--ui-layout-selftest` and snapshot coverage are the measurement instruments |

## 44.2 Lane B — mechanics and systems (canonical)

| Cell | Archetype (canonical) | State / correction |
|---|---|---|
| B/C1 | Shelter-failure cascade completion (B-01, quarantine exit criteria govern); grid seal follow-through (B-02) | Quarantine is instrument concept (a) per DR-31; `cascade_rules.json` seam confirmed (Volumes 34–35) |
| B/C2 | Dose ledger × fallout-window coupling (B-03, verify current depth first); child-health cohort bridge (B-04, PROPOSAL) | Ward staffing CLOSED (Plan 24, DR-06) — do not reopen |
| B/C3 | Preservation × disease bridge (B-05); dive/hydroponic audits (SB-12) | PROPOSAL-class; premise sweeps required |
| B/C4 | Industrial catalog consumption wiring (SB-03, census-gated); XP difficulty consumers after W1 seal (B-06) | B-06 sequence-gated: W1 confirmed ACTIVE-not-sealed in the live ledger head (2026-09-18) |
| B/C5 | Vehicle-breakdown medical/dose consequences (B-07, verify routing first); scavenging parity (B-08, 49 vs 53) | B-08 HIGH CONFIDENCE, DATA-ONLY |
| B/C6 | Flooded-route topology tags (B-09) | GATE — DP-01 signature-blocked |
| B/C7 | FactionWar per-strike emitters (B-10); black-market funds legs (B-11); muster deep expansion (SB-04 / F-004) | B-10 GATE (DP-02, rendering surface named in Volume 29); B-11 BLOCKED plus actions-surface SEALED; F-004 open, loader coverage to confirm |
| B/C8 | Market-rumor band extension (B-12, HIGH CONFIDENCE); intercept-driven journal (B-13, PROPOSAL) | Signal follow-up chaining SEALED (DR-06); extend only through recorded seams |
| B/C9 | Belief × stance bridge (B-14); memorial-rite evidence enrollment (B-15, vocabulary check first); intake-advisory bridge (B-33) | B-33 verified-unwired at priority G1 (Volume 43); institution family enumerated four members (Volume 40) |
| B/C10 | Quest reopening grammar (B-16); gossip propagation depth (B-17); unstarted-type cluster (B-34) | B-34 is a foreman prioritization question, not yet a plan (Volume 43) |
| B/C11 | Trade-screen scenarios (B-18); merchant restock SEALED (DEC-05) | HIGH CONFIDENCE for B-18 |
| B/C12 | Winter pressure for power/water (B-19, PROPOSAL) | Year-of-Ash window 180–360 canon; no new scalars outside difficulty authority |
| B/C13 | Reckoning enrollment sweep (B-20, HIGH CONFIDENCE) | Post-19C systems enumerated from Waves 8–12 logs |
| B/C14 | Migration × route encounters (B-21); infestation × crop economy | PROPOSAL-class |
| B/C15 | Defense grid × siege math; sky-armor × harrow telemetry (B-22) | PROPOSAL-class |
| B/C16 | Difficulty consumer binding (B-23 / CF-XP01) | Sanctioned seam: extend the difficulty authority, never parallel it; W1 ACTIVE (DR-06) |
| B/C17 | Stale-panel truthfulness audit | HIGH CONFIDENCE; measurement via `--ui-layout-selftest` |

## 44.3 Lanes C through J (canonical state summaries)

- Lane C (economy and balance): gate structure unchanged; the six ASH-EXP-6 harnesses carry the Volume 27 supplement and Volume 35 owner-class bindings; harness fixtures must record `schema_version` uniformly per DR-30. Standing rule: no number change in a first tranche; Lane C plans cite the live baselines (DR-03) rather than re-deriving.
- Lane D (save and state): D-05's triggering release (1.1.0) confirmed at the build axis (Volume 29); the `save_support_window` gate confirmed in the live 57-gate manifest (Volume 29). Codec-bump-and-migrate remains the CANON process for any stateful extension.
- Lane E (UI and accessibility): E-11 (communiqué board) is executed as a specification (Volume 37) and is the lane's next plannable tranche; E-01 enumeration must include the institution and cultural families (Volume 30) plus the psychological sanatorium (Volume 40). The panel-registration-gap class has two verified incidents (DR-21; the Volume 37 board finding) — the cell is strengthened from HIGH CONFIDENCE to evidenced.
- Lane F (performance): unchanged; no profiling evidence has landed since the base matrices; the profile-before-rewrite rule stands.
- Lane G (testing): G-09 ungated and plannable (Volume 30); G-10's DR-20 gate passed, leaving only the test-inventory read; dose-treatment paired tests, debt-ledger dispatcher coverage, flag-consumer coverage, and epilogue-reachability tests remain the lane's seeded surface.
- Lane H (tooling): SB-08's design input is complete (manifest read in full, 57 gates, count authority named — Volume 29) and its implementation-readiness specification exists (Volume 42); H-06 is seeded with two verified panel-ownership incidents (Volume 37); F-009 is resolved to a placement decision through the instrument-(a) archive discipline (Volumes 29, 34); A-42's tranche is specified and its census is executed (Volume 45).
- Lane I (documentation): the ASH-EXP-7 flagship is satisfied (Volume 35); F-007's registration accumulates classifications per artifact (Volumes 29, 34, 43) and now includes the gaps/incidents classifications and the ANTIGRAVITY.md and C-integration-plans additions (DR-24).
- Lane J (onboarding): J-06 ungated at gate one; onboarding enumerations include the sanatorium (Volume 40); difficulty-preset consumer rows ride the W1 sequence gate.

## 44.4 Sealed and blocked surfaces (consolidated, canonical)

The following surfaces may not be opened by any factory-generated plan without new evidence plus foreman signature: the distress-signal content surface (`CF-P1-DISTRESS-CONTENT-SEAL`, permanent guard clause in the Volume 36 contract); merchant restock priority (DEC-05); the rescue-signal runtime beyond its recorded seams; the 17A-S semantic parity matrix and its gate. The following require a named signature before drafting: DP-01 (flooded-route authored map edges), DP-02 (FactionWar per-strike emitters), DP-03 (black-market funds authority), plus the Q-1 through Q-4 foreman questions (Volume 41) whose answers will open or close cells rather than gates.

## 44.5 The live seed inventory (canonical count)

After this wave: 12 flagship plans; 60+ expanded plans across Lanes A–J; 4 decision packets (3 signature-blocked, DP-04 corroborated); 8 runbooks; 8 charters; 17 corrected cluster maps; 40+ prose and record contracts; 12 harness specifications plus the Volume 27 supplement; and 11 seeds originating in Volumes 26 through 45 (FP-A24 closed, A-41, A-42, H-06, B-33, B-34, and the census-derived tranches). The combinatorial claim of the base matrices — 170 cells, multiple plans per cell over time — is unchanged and remains the factory's candidate space.

---

# VOLUME 45 — A-42 TRANCHE-0 CENSUS EXECUTED: THE INCIDENT CATALOG READ IN FULL (Factory batch 2026-09-24-J)

## 45.1 Method and access

`Assets/StreamingAssets/Data/incidents.json` (12,879 bytes on disk; 12,885 characters through the fetch surface) was read in full this session. The fetch returned the complete file (head and tail both verified against the document's own closing structure). The file does not parse as strict JSON — see DR-34 below — so the census was executed with a tolerant record extractor that walked the five-field surface (`id`, `title`, `bodyText`, `weight`, `minDay`) record by record and measured each `bodyText`. The extraction is reproducible from the raw fetch; the measurements below are the output.

## 45.2 Surface confirmation

VERIFIED: `schema_version: 1`, and exactly 25 incident records, each carrying exactly the five fields the Plan 57 report specified. The Case D seal (no scheduler, no consequence dispatch, no history schema) is respected by the file itself: there is no sixth field. Provenance is now fully documented: `piagentsplans/57-incident-expansion.md` records the expansion goal ("Expand `incidents.json` from 5 verified entries to 25 shelter incidents — random events that fire during the shelter tick") and the original five-entry state; the Plan 75 batch-4 roadmap names `IncidentSystem` in a cross-system chain (Wall carving → morale → schedule → power grid → incident), confirming both the consuming system class and the shelter-tick dispatch surface. The 25-record file is therefore the completed Plan 57 deliverable, sealed Case D.

## 45.3 The census table

| # | id (short) | weight | minDay | bodyText words |
|---|---|---|---|---|
| 1 | radiation_spike | 1.0 | 20 | 33 |
| 2 | bunker_breach | 1.0 | 18 | 35 |
| 3 | water_contamination | 1.0 | 15 | 23 |
| 4 | ambush_sector4 | 1.0 | 12 | 29 |
| 5 | radio_interference | 1.0 | 8 | 26 |
| 6 | fallout_storm | 1.2 | 12 | 56 |
| 7 | contaminated_water_table | 0.9 | 32 | 61 |
| 8 | ground_tremor | 0.7 | 18 | 57 |
| 9 | perimeter_breach | 0.8 | 35 | 62 |
| 10 | unknown_visitor | 1.0 | 38 | 70 |
| 11 | local_signal_intercept | 0.6 | 41 | 61 |
| 12 | disease_outbreak | 0.9 | 44 | 66 |
| 13 | chemical_exposure | 0.5 | 47 | 69 |
| 14 | survivor_collapse | 1.1 | 25 | 72 |
| 15 | ration_dispute | 1.3 | 8 | 70 |
| 16 | ideological_friction | 0.8 | 72 | 59 |
| 17 | grief_episode | 1.0 | 76 | 70 |
| 18 | generator_failure | 1.2 | 15 | 73 |
| 19 | air_filter_breakdown | 0.9 | 22 | 70 |
| 20 | water_pipe_burst | 1.1 | 50 | 78 |
| 21 | cache_discovered | 0.4 | 55 | 64 |
| 22 | supply_drop | 0.3 | 80 | 66 |
| 23 | faction_patrol | 0.7 | 58 | 65 |
| 24 | refugees_approaching | 0.6 | 84 | 77 |
| 25 | the_anniversary | 1.0 | 90 | 81 |

Total authored prose: 1,493 words across 25 records. Median record: ~64 words.

## 45.4 Findings

**Finding 1 — The prose-depth split is measured, not inferred. VERIFIED.** Twenty records sit in or above the 55–80 word band; five records fall below 40 words: water_contamination (23), radio_interference (26), ambush_sector4 (29), radiation_spike (33), and bunker_breach (35). These five are precisely the original five pre-expansion entries (matching the Plan 57 report's description of the pre-expansion catalog), and the expansion's twenty new records were authored to a deeper norm. The A-42 tranche therefore has a measured first target: bring the five legacy records to the expansion-era band without touching `id`, `weight`, `minDay`, or the five-field surface. This is pure `bodyText` authoring; no other field may change; the Case D seal is untouched.

**Finding 2 — The minDay ceiling is 90; the mid-winter slump has no incident vocabulary. VERIFIED.** The highest unlock is the_anniversary at Day 90. There are no records exclusive to Days 90–180, and none gated to the Year-of-Ash window at all. `minDay` is a floor (the loader semantics per the Plan 57 report), so all 25 records continue to fire after Day 90 — but nothing new can appear there. This converts SB-02's premise (the Days 90–180 pacing gap) into a second, independent confirmation at the incident layer: the slump period draws from a vocabulary authored entirely before it. Under the Case D seal the factory does not propose new records; it records that any foreman-requested incident extension (Q-3 of Volume 41) would fill a real, measured gap, and the vocabulary for it already has a proven authoring norm (the expansion-era band).

**Finding 3 — The weight distribution encodes a design stance. VERIFIED (measurement); interpretation HIGH CONFIDENCE.** Negative incidents carry weights 0.5–1.3 (ration_dispute heaviest at 1.3, followed by fallout_storm and generator_failure at 1.2); positive incidents carry 0.3–0.6 (supply_drop 0.3, cache_discovered 0.4). The distribution is a legible design statement: routine friction dominates, fortune is rare. The factory records this as the incident domain's tuning baseline and prohibits any A-42 tranche from disturbing it — `weight` is frozen in the tranche contract.

**Finding 4 — DR-34 (new): the incident catalog is non-strict JSON. VERIFIED as measurement; loader tolerance UNVERIFIED.** Four records contain raw control characters inside JSON string literals: one title ("Fallout Storm Approach" carries a raw line break mid-title) and three bodyTexts (chemical_exposure, grief_episode, refugees_approaching each contain one raw line break). Strict parsers reject the file at position 2000. The repository's data-integrity gate evidently passes the file (it is shipped at schema_version 1 and sealed), which means either the gate does not enforce strict JSON or the game's parser tolerates control characters — which of these holds is a runtime question the factory flags, not resolves. Two consequences: (a) the raw mid-title line break is a player-visible artifact risk on any surface that renders the title without normalization — the H-06 panel-ownership check and any incident panel snapshot should include a normalization assertion; (b) the A-42 tranche contract must specify escaped `\n` sequences in all new `bodyText` authoring, matching strict JSON, and the tranche verification should include a strict-parse assertion over the whole file. The factory does not recommend repairing the four existing records in the A-42 tranche itself — that is a separate, tiny hygiene decision for the foreman (strictness repair changes bytes in a sealed Case D file; prose repair changes bytes only in the five legacy `bodyText` fields, which the tranche already owns).

**Finding 5 — events.json is the sibling surface and remains unread. VERIFIED existence; UNVERIFIED content.** At 240,926 bytes the events read-model catalog is eighteen times the incident file and was not read this session; it exceeds a single fragment fetch and requires a segmented read plan. The factory records it as A-42's Tranche-2 evidence source (whether the 25 incidents surface elsewhere in a read model) and as an open premise, honestly bounded.

## 45.5 The A-42 tranche contract (consolidated)

Subject: `bodyText` authoring for the five legacy records (water_contamination, radio_interference, ambush_sector4, radiation_spike, bunker_breach) to the expansion-era band (55–80 words), preserving each record's premise, register, and all non-prose fields byte-for-byte. Route: DATA-ONLY, single catalog, no loader change. Constraints: five-field surface frozen; `weight` and `minDay` frozen; new prose uses escaped line breaks only (strict JSON); the sealed vocabulary ban (`RB-SEALGUARD`) applies. Verification: the Plan 57 test file (12 contract tests, verified live) extended per tranche; the `data_integrity` gate; a strict-parse assertion over the full file; and a per-record word-count assertion. Confidence: HIGH CONFIDENCE (surface, targets, and norm all measured this session).

---

# VOLUME 46 — THE STALE-DEBT RETIRE-CHECK WAVE (Factory batch 2026-09-24-K)

## 46.1 Method

A debt entry is stale when the ledger or an audit names a system that live source no longer contains, or that live source contains but no consumer exercises. The retire-check method: take each debt-ledger or audit row naming a system, run a live code search for the system's name, classify the result, and issue one of three verdicts: LIVE (source and consumers both present), RETIRED-CORROBORATED (source absent or docs-only hits, corroborating a recorded retirement), or UNRESOLVED (evidence insufficient; keep the debt row). The searches below were executed in the prior session wave and are recorded here with their verdicts; no debt row is retired by this document — retirement is a ledger edit that only an owning session may perform.

## 46.2 Verdicts

**SurvivorSocialCoordinator — LIVE. VERIFIED.** 53 live references across the Core tree, including the atrophy tick the audit questioned. The debt question ("does the coordinator actually exercise?") is answered at the source level: the tick exists and is referenced. Any balance question about atrophy rates is a Lane C measurement, not a staleness question. Verdict: retire the staleness question; the system stays in the ledger only if it carries a non-staleness debt.

**CampaignCalendar — LIVE. VERIFIED.** 58 live references. The audit's staleness suspicion (whether the calendar still drives scheduling) is refuted at the reference level. Runtime exercise (which sessions actually consult it on the campaign path) remains a Lane G verification question, but the retire-check's own question is closed: not stale.

**HeirloomSystem — LIVE, location corrected. VERIFIED.** 12 references, resolving to `Assets/Ashfall.Core/Phantoms/HeirloomSystem.cs`. Any document claiming the heirloom system is absent or unmapped is corrected by this finding; the phantom family (phantom_heirlooms, phantom_triggers, HeirloomSystem) is a closed, referenced chain. Seed A-21 (phantom-memory triggers keyed to cohorts) is unchanged and remains HIGH CONFIDENCE.

**AmputationEquipment — UNRESOLVED (absence). VERIFIED absence in live source.** Zero code hits. The name appears only in debt or audit surfaces. Absence cannot be scheduled and cannot be wired; the only honest disposition is to confirm whether the ledger row records it as a retired decision (in which case the row should move to the retired section with this search as corroborating evidence) or as an open intention (in which case it is a content decision, not a debt). The factory flags the row for the foreman's disposition and takes no position on the intention itself.

**SurvivorInspection — RETIRED-CORROBORATED. HIGH CONFIDENCE.** 14 hits, all in `docs/` and `Seal-steps/` — zero live source references. This is the retirement signature: a system whose name survives only in documentation and sealing records. The search corroborates the D2 decision recorded in the gaps audit (Volume 43's registration). Verdict: an owning session may retire the debt row citing this search; the factory's recommendation is retire with the docs-only reference pattern as evidence.

## 46.3 Consequences for the ledger and the matrices

No matrix cell changes: all five systems were already treated as live-or-unresolved in the canonical maps. The wave's value is ledger hygiene — three staleness questions closed (two refuted, one corroborated), one absence flagged for disposition, one system's location corrected in the record. The retire-check method itself (three verdicts, search-first, no retirement without an owning session) is hereby a standing factory instrument and should execute against every future debt audit before any new debt row is authored — the anti-duplication firewall applies to debt as much as to content.

---

# VOLUME 47 — FACTORY SELF-AUDIT AND WAVE-CHARTER REFRESH (Factory batch 2026-09-24-L)

## 47.1 Constitution compliance check

- Read-first discipline: held. No repository file has been modified in any wave; the working tree is untouched; all reads are recorded with their method (full read, fragment read, search, tolerant extraction) and their honest bounds.
- Evidence labeling: held. Every substantive claim across Volumes 25–46 carries VERIFIED, HIGH CONFIDENCE, PROPOSAL, INFERENCE, UNVERIFIED, or the sealed/blocked/gated state labels; the two fetch failures (incidents.json's strict parse; A-41's choice-array segments) are recorded as findings (DR-34) and open premises rather than smoothed over.
- Anti-padding: held with one caveat recorded honestly. Character targets are met by executing more verified work (censuses, sweeps, retire-checks, matrix synthesis), never by inflating prose. The matrix reprint (Volume 44) is synthesis, not new evidence, and is labeled as such; it was warranted because the delta volume left the base matrices non-canonical, a real navigation defect for any future session.
- Plan-count discipline: held. The factory has not manufactured plans; the seed inventory grows only from evidence (six seeds from drift findings, two from the gaps/incidents inputs, two from provenance and census findings).
- One-authority rule: held. Every proposed change names its owning system; no parallel authority appears in any specification.

## 47.2 Charter status re-check (ASH-EXP-3 through ASH-EXP-8)

- ASH-EXP-3 (deep-map confirmation): satisfied and closed (Volumes 25, 29, 34).
- ASH-EXP-4 (census execution): satisfied for the unclaimed-corpus census (Volume 26); the incident census now also executed (Volume 45); the events.json read-model segment plan remains the charter's open tail.
- ASH-EXP-5 (contract library): standing; forty-plus contracts; the Volume 36 seal-guard is the charter's hardest rule.
- ASH-EXP-6 (harness supplements): satisfied (Volume 27); baseline refresh (F-006) remains plannable and now has its fixture rule (DR-30).
- ASH-EXP-7 (docs authority): satisfied (Volume 35); F-007 registration continues accumulating per-artifact classifications.
- ASH-EXP-8 (implementation readiness): satisfied as specifications (Volume 42) for SB-08, F-009, H-06, E-11; execution remains gated on foreman authorization, which the factory has not requested prematurely.

## 47.3 Recommended execution order (all open work, ranked)

1. Foreman answers first (Volume 41's Q-1 through Q-4, plus DR-34's strictness disposition and the AmputationEquipment row) — several cheaper than any plan and unblocking for others.
2. The documentation/tooling quartet, on authorization: SB-08 (gate-count drift guard), F-009 (rewrite.py archive), H-06 (panel-ownership check, expected first failure documented), E-11 tranche (communiqué board docs). All four have implementation-readiness specifications (Volume 42).
3. The A-42 prose tranche (Volume 45's contract): smallest, fully specified, verified targets.
4. The A-41 de-templatization program: blocked on choice-array segment reads (fetch truncation); a segmented-read plan must precede any tranche.
5. F-006 baseline refresh (Lane C multiplier) once the foreman confirms harness run policy.
6. The content waves in matrix rank order: SB-02 (measured twice now), F-004, F-005, SB-03, then the Lane A backlog by cluster.

## 47.4 Open premise ledger (consolidated)

UNVERIFIED and requiring reads before dependent work: events.json content (240,926 bytes, segmented read required); the incidents loader's control-character tolerance (runtime question); the A-41 choice arrays beyond the fragment horizon; the muster loader family's coverage of all five catalogs; the balance harness entry point and run policy; the per-permutation epilogue prose depth; the remaining root artifacts' contents for F-007 classification. Each is already carried in its owning seed's open-premises section; this ledger consolidates them for the next session's premise sweep.

---

# VOLUME 48 — THE CONSOLIDATED SEED REGISTER (Factory batch 2026-09-24-M)

The factory's seed space, one register, current status. Statuses: OPEN (plannable now), GATED (signature or answer required), SEALED (surface closed), CLOSED (executed or retired), BLOCKED (decision-blocked). Route tiers: D (DATA-ONLY), H (HOST-WIRING), C (CORE-EXTENSION), DOC (DOCS-ONLY), T (TOOLING), MIXED. This register supersedes the scattered seed lists of the base document and Volumes 26–45 for navigation purposes; the individual seed entries remain the authority on scope.

## 48.1 Lane A seeds

| Seed | Cluster | Subject (compressed) | Status | Route |
|---|---|---|---|---|
| A-01 | C1 | Bunker glitch batch N+1 | OPEN | D |
| A-02 | C1 | Sanitation load-shed amendments | OPEN | D |
| A-03 | C2 | ARS casebook phases | OPEN | D |
| A-04 | C2 | Dose-treatment narrative pairing | OPEN | D |
| A-05 | C2 | Therapist notes batch 4 | OPEN | D |
| A-06 | C3 | Preservation assay twins | OPEN | D |
| A-07 | C3 | Cellar/silo seasonal logs | OPEN | D |
| A-08 | C3 | Apiculture continuation | OPEN | D |
| A-09 | C4 | Hydraulic extrusion assay twin | OPEN (census-gated) | D |
| A-10 | C4 | Metrology calibration corpus | OPEN | D |
| A-11 | C4 | Foundry pour-window logs | OPEN | D |
| A-12 | C5 | Destination prose completion | OPEN (measure first) | D |
| A-13 | C5 | Waystation register prose | OPEN | D |
| A-14 | C5 | Rail-side field documents | OPEN (corpus sweep first) | D |
| A-15 | C6 | Damaged-zone survey marginalia | OPEN | D |
| A-16 | C7 | Standing-record testimony | OPEN | D |
| A-17 | C7 | Verdict radio continuation | OPEN | D |
| A-18 | C7 | Warlord communiqués | OPEN | D |
| A-19 | C8 | Numbers-station ciphers | OPEN | D |
| A-20 | C8 | Radio rundowns | OPEN (seal-guard applies) | D |
| A-21 | C9 | Phantom triggers × cohorts | OPEN (Volume 46 corroborates the chain) | D |
| A-22 | C9 | Final-wishes documents | OPEN | D |
| A-23 | C9 | Intake interviews | OPEN | D |
| A-24 | C10 | Bureaucratic-morality prose | OPEN | D |
| A-25 | C10 | Massive-expansion prose audit | OPEN (multi-tranche) | D |
| A-26 | C11 | Ledger-debt statements | OPEN | D |
| A-27 | C12 | Storm-window almanac | OPEN | D |
| A-28 | C13 | Epilogue chronicle depth | OPEN (consumed by F-005) | D |
| A-29 | C14 | Bestiary continuation | OPEN | D |
| A-30 | C15 | Sky-defense manifests | OPEN (corpus sweep first) | D |
| A-31 | C9 | Institution archive corpus (contract in V32) | OPEN | D |
| A-32 | C7 | Summit protocol documents (contract in V32) | OPEN | D |
| A-41 | C10 | Faction-branching de-templatization | OPEN (segment reads block tranches) | D |
| A-42 | C17 | Incident prose tranche (census executed, V45) | OPEN — highest-readiness Lane A seed | D |
| FP-A24 | C10 | Massive-expansion prose | CLOSED (V26) | — |
| FP-A25 | C10 | Quest prose ranking | Re-ranked behind A-41 (V39) | D |

## 48.2 Lane B seeds

| Seed | Cluster | Subject (compressed) | Status | Route |
|---|---|---|---|---|
| B-01 | C1 | Shelter-failure cascade completion | OPEN (quarantine exit criteria govern) | C+D |
| B-02 | C1 | Grid seal follow-through | OPEN (scope unverified) | H |
| B-03 | C2 | Dose × fallout-window coupling | OPEN (verify depth first) | C |
| B-04 | C2 | Child-health cohort bridge | OPEN (PROPOSAL) | C |
| B-05 | C3 | Preservation × disease bridge | OPEN (PROPOSAL) | C+D |
| B-06 | C4 | Industrial XP consumers | GATED on W1 seal | C |
| B-07 | C5 | Vehicle-breakdown consequences | OPEN (verify routing first) | C |
| B-08 | C5 | Scavenging-table parity | OPEN | D |
| B-09 | C6 | Flooded-route tags | GATED (DP-01) | D+consumers |
| B-10 | C7 | Per-strike emitters | GATED (DP-02) | C |
| B-11 | C11 | Black-market funds legs | BLOCKED + surface SEALED | — |
| B-12 | C8 | Rumor band extension | OPEN | C+D |
| B-13 | C8 | Intercept-driven journal | OPEN (PROPOSAL) | H |
| B-14 | C9 | Belief × stance bridge | OPEN (PROPOSAL) | C |
| B-15 | C9 | Memorial-rite evidence enrollment | OPEN (vocabulary check first) | C |
| B-16 | C10 | Quest reopening grammar | OPEN (implementation state unverified) | C |
| B-17 | C10 | Gossip propagation depth | OPEN (PROPOSAL) | C |
| B-18 | C11 | Trade-screen scenarios | OPEN | D |
| B-19 | C12 | Winter pressure, power/water | OPEN (PROPOSAL) | C+D |
| B-20 | C13 | Reckoning enrollment sweep | OPEN | audit→C |
| B-21 | C14 | Migration × route encounters | OPEN (PROPOSAL) | C |
| B-22 | C15 | Defense grid × siege math | OPEN (PROPOSAL) | C |
| B-23 | C16 | Difficulty consumers (CF-XP01) | OPEN (W1 sequence) | C |
| B-26 | C9 | Institution-availability audit | OPEN (findings routed, V38) | audit |
| B-33 | C9 | Intake-advisory bridge | OPEN — priority G1 (V43) | C |
| B-34 | C10 | Unstarted-type cluster | GATED (foreman prioritization, V43) | — |
| FP-B06 | C16 | XP binding | = B-06 lineage, GATED on W1 | C |
| FP-B14 | C9 | Spiritual models premise | Premise located (`SpiritualModels.cs`, V25) | C |

## 48.3 Lane C through J seeds (compressed register)

| Seed | Lane | Subject (compressed) | Status | Route |
|---|---|---|---|---|
| SB-06 / F-006 | C | Balance baseline refresh | OPEN (harness policy first) | T |
| Lane C cells (C4, C5, C7, C11, C12, C14) | C | Audits against live baselines | OPEN per matrix | audit |
| D-05 | D | Save-support-window fixture | OPEN (release axis confirmed) | T |
| Lane D cross-cutting | D | Mid-event save round-trips | OPEN (PROPOSAL) | T |
| E-11 | E | Communiqué board docs tranche | OPEN — specification complete (V37, V42) | DOC |
| E-01 | E | Panel enumeration refresh | OPEN (include sanatorium) | DOC |
| Lane F cells | F | UI rebuild / tick-window profiling | OPEN (measure first) | T |
| G dose tests | G | Dose-matrix paired tests | OPEN | T |
| G-09 | G | Ungated test surface | OPEN (V30) | T |
| G-10 | G | Test-inventory read | OPEN (DR-20 gate passed) | T |
| SB-08 | H | Gate-count drift guard | OPEN — spec complete (V42) | T |
| F-009 | H | rewrite.py archive | OPEN — spec complete (V42) | T |
| H-06 | H | Panel-ownership check | OPEN — spec complete (V42) | T |
| SB-10 | H | Census feed | OPEN (process canon) | T+process |
| F-007 | I | Root coordination registration | OPEN (classifications accumulating) | DOC |
| J-06 | J | DR-20 system onboarding rows | OPEN (ungated at gate one) | DOC |
| Lane J briefings | J | New-system briefing surface | OPEN | H+DOC |

## 48.4 Flagship plans register

| Plan | Subject | Status |
|---|---|---|
| F-001 | Delayed moral-choice callbacks (SB-01) | OPEN — top Lane A/C10 rank |
| F-002 | Mid-winter pressure campaign (SB-02) | OPEN — premise confirmed twice (atlas + V45) |
| F-003 | Industrial corpus twins + wiring audit (SB-03) | OPEN — census-gated |
| F-004 | Muster deep expansion (SB-04) | OPEN — loader coverage to confirm |
| F-005 | Epilogue permutation coverage (SB-05) | OPEN — depth measurement first |
| F-006 | Balance baseline refresh (SB-06) | OPEN — harness policy first |
| F-007 | Root coordination registration (SB-07) | OPEN — in execution, docs-only |
| F-008 | Gate-count drift guard (SB-08) | OPEN — spec complete (V42) |
| F-009 | rewrite.py hygiene (SB-09) | OPEN — resolved to archive decision |
| F-010 | Unclaimed-content census feed (SB-10) | OPEN — process canon |
| F-011 | C2 open-gap package | OPEN — per-item scoping required |
| F-012 | Dive/hydroponic audit (SB-12) | OPEN — sweep first |

## 48.5 Register summary counts

Lane A: 34 open seeds, 1 closed, 1 re-ranked. Lane B: 20 open, 3 gated/blocked. Lanes C–J: 14 open across measurement, tooling, docs, and onboarding classes. Flagships: 12 open. Decision packets: 4 (3 signature-blocked, 1 corroborated). Foreman questions outstanding: Q-1 through Q-4 plus the two new dispositions (DR-34 strictness; AmputationEquipment row). The register's own maintenance rule: every new seed must appear here in the wave that authors it, and every status change must cite its volume; the register is the factory's navigation surface and is regenerated, never edited in place without a volume reference.

---

# GROWTH LEDGER UPDATE (this wave)

- 2026-09-24 — Volume 44: the canonical matrix reprint — the base Part III matrices merged with every Volume 39 correction and the sealed/blocked/gated states consolidated, superseding the base matrices as the working reference — ~9,500 — cumulative ~574,500
- 2026-09-24 — Volume 45: the A-42 Tranche-0 census executed — incidents.json read in full (25 records, five-field surface confirmed, provenance chain closed through piagentsplans/57 and the Plan 75 IncidentSystem chain), the full measurement table published, four findings including the prose-depth split (five legacy records below 40 words), the Day-90 minDay ceiling (a second independent confirmation of the SB-02 premise), the weight baseline (0.3–1.3), and DR-34 (non-strict JSON, four records with raw control characters; loader tolerance flagged as a runtime question), plus the consolidated tranche contract — ~9,000 — cumulative ~583,500
- 2026-09-24 — Volume 46: the stale-debt retire-check wave — five verdicts on live evidence (SurvivorSocialCoordinator LIVE, CampaignCalendar LIVE, HeirloomSystem LIVE with its location corrected, AmputationEquipment absence flagged for disposition, SurvivorInspection RETIRED-CORROBORATED corroborating D2), the three-verdict method installed as a standing factory instrument — ~4,500 — cumulative ~588,000
- 2026-09-24 — Volume 47: the factory self-audit and charter refresh — constitution compliance held across all waves, ASH-EXP-3 through 8 re-checked, the ranked execution order published, the open premise ledger consolidated — ~5,000 — cumulative ~593,000
- 2026-09-24 — Volume 48: the consolidated seed register — the full seed space in four tables with statuses, routes, gates, and maintenance rules, superseding the scattered seed lists for navigation — ~8,500 — cumulative ~601,500

---

# VOLUME 49 — EVENTS.JSON HEAD CENSUS AND THE CATALOG TRIPLE (Factory batch 2026-09-24-N)

## 49.1 Method and honest bounds

`Assets/StreamingAssets/Data/events.json` is 240,926 bytes on disk; the fetch surface returns 32,793 characters before truncating. This volume reports a head-fragment census only — the first complete records within the fetch horizon — and does not extrapolate the catalog's total record count. The file's tail, its full record inventory, and its distribution statistics remain open premises (the segmented-read plan, Volume 51, is the instrument that closes them). Every claim below is bounded to the fragment actually read.

## 49.2 What the head confirms

VERIFIED (fragment scope): `schema_version: 1`, an `events` root array, and a record shape of `id`, `title`, `bodyText`, `weight`, `minDay` — the same five-field surface as `incidents.json`, at the same field names and the same nesting depth. The first record is `fallout_storm` ("Fallout Storm", weight 3.0, minDay 1) with a 110-word `bodyText` — prose depth at the expansion-era incident band or above. The second record, `scavenger_arrival` ("Scavenger at the Door"), is also deep-form. Within the fragment horizon the catalog is authored to a single deep norm; no short-form records were observed. Fragment-scope finding: the weight scale differs from the incident catalog — the first event carries weight 3.0 where the incident twin carries 1.2. Whether the two catalogs share a weight scale is an open premise; if they do not, their weights are not comparable and any cross-catalog balance work must treat them as separate domains.

## 49.3 The mirror finding — events and incidents share ids

VERIFIED (fragment scope, both surfaces read): `events.json`'s first record and `incidents.json`'s sixth record share both `id` (`fallout_storm`) and `title` ("Fallout Storm") while differing in prose, weight (3.0 vs 1.2), and `minDay` (1 vs 12). The two bodies are different texts: the event version is the second-person decision-facing register ("You stand at the hatch window... You have time to decide what you are willing to spend"), the incident version is the third-person ambient register. The repository therefore maintains a paired-vocabulary pattern across its two random-event catalogs — an event is the decision-facing rendering of a hazard that also exists as an ambient incident. Consequences: (a) A-42's tranche must not treat the five legacy incident `bodyText` fields as unconstrained authoring — each legacy incident may have an event twin whose prose it must not contradict on facts (hazard source, affected systems, timescale); the tranche contract gains a cross-catalog consistency clause, scoped to the records that can be verified within the fetch horizon; (b) the mirror pattern is itself a candidate subject for a provenance sweep (how many ids are shared, whether the pairs were authored together) — recorded as an open premise, not a seed, until the full events census runs; (c) the sealed-vocabulary ban applies to both catalogs equally, since both render hazards to the player.

## 49.4 The catalog triple and its single session

VERIFIED (source read, `src/Host/EventsHostSession.cs`, 7,167 characters, full file within fetch horizon): one class — `EventsHostSession` — loads all three catalogs in its `_Ready`: `events.json`, `incidents.json`, and `narrative_progression.json`, each through `CatalogPath.ResolveCatalog` and `CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir())`. The incidents load is guarded (`FileExists` check; absent file leaves the list empty), and the events load is additionally re-entry-safe: `TryGetEvent` re-invokes `LoadEvents()` if the list is empty, because, per the source's own comment, "a trapping event can be delivered before Godot has run this node's _Ready callback" — a documented composition-order hazard the host code already guards. `TryGetEvent(string eventId, out EventData)` is the host-adapter lookup: events are addressed by id from host code, not sampled by weight. This is an architectural datum: `events.json` is an id-addressed authored-event catalog for host adapters; `incidents.json` is a weighted random catalog consumed elsewhere. The two are not interchangeable, and the factory records the distinction as a matrix annotation for Lane B/C17: any plan touching either catalog must respect its addressing model.

## 49.5 The consumer mapping, confirmed twice

VERIFIED (source searches): `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs` contains both a catalog-inventory list naming `incidents.json` among its scanned files and an explicit consumer map: `["incidents.json"] = new[] { "ShelterEncounterSystem" }`. The incident catalog's consumer is therefore `ShelterEncounterSystem` — which corroborates the Plan 75 batch-4 roadmap's cross-system chain (wall carving → morale → schedule → power grid → `IncidentSystem`) at the class level: the shelter-tick dispatch surface is the encounter system. The scanner's map is itself a repository-owned authority the factory had not previously read; it is now cited as the consumer-mapping instrument for every catalog whose consumer question arises (the same fragment names `guilt_sources.json → GuiltInsomniaSystem` and `moral_choice_chains.json → MoralChoiceChainCatalogLoader`).

## 49.6 Volume 45 correction recorded

Volume 45's Finding 5 called events.json "the sibling surface... whether the 25 incidents surface elsewhere in a read model." The head census refines this: events.json is not a read model of incidents; it is a parallel authored catalog with a different addressing model and (probably) a different weight scale, sharing vocabulary by design. The open-premise ledger entry is amended accordingly, and the A-42 cross-catalog consistency clause is the direct consequence.

---

# VOLUME 50 — DR-34 RESOLVED TO A VERIFICATION INSTRUMENT: THE PARSE PATH TRACED END TO END (Factory batch 2026-09-24-O)

## 50.1 The full parse path, traced

VERIFIED (three source reads, all within fetch horizon): `incidents.json` is loaded by exactly one call path — `EventsHostSession.LoadIncidents()` → `CatalogPath.ResolveCatalog("incidents.json")` → `IFileIO.ReadAllText` → `_jsonSerializer.Deserialize<IncidentsRoot>(json)`. The serializer is injected through the session's constructor. The interface is `IJsonSerializer` (`Assets/Ashfall.Core/Ports.cs`), whose own documentation comment states the port's invariant: "Engine-agnostic JSON port. Unity JsonUtility is banned from this assembly. A save written through this port must load in both hosts." The concrete implementation bound in the Godot host is `SystemTextJsonSerializer : IJsonSerializer` (`Assets/Ashfall.Core/HostDefaults.cs`), whose `Deserialize` is `JsonSerializer.Deserialize<T>(json, Options)` — System.Text.Json. A second implementation exists as a cross-host template: `docs/contracts/Cross-host/UnityJsonSerializer.template.cs` (`UnityJsonSerializer`, backed by `JsonUtility`), whose header comment records that it "produces the exact same wire format as SystemTextJsonSerializer for all Core save DTOs (verified by SaveWireContractTests.cs)."

## 50.2 The verdict on control-character tolerance

System.Text.Json's reader enforces RFC 8259 strictly by default: unescaped control characters inside string literals raise a parse error, and there is no option that permits them. The two hosts therefore differ in principle: the System.Text.Json binding rejects raw control characters, while JsonUtility's behavior is documented (in the template's own notes) as tolerant of much. This converts DR-34 from an open flag into a fork with two branches and one decisive instrument:

- Branch 1 — the on-disk file genuinely contains raw control characters. Then `LoadIncidents` throws on the Godot host at `_Ready` (the `FileExists` guard passes, the deserialize throws), the incident list stays empty or the session fails its ready phase, and the sealed Case D catalog never fires — a real, player-facing load failure that contradicts the Plan 57 seal's verified delivery (12 contract tests, `data_integrity` gate). Under this branch the file cannot be shipping as fetched, so this branch is unlikely but must be excluded, not assumed away.
- Branch 2 — the fetch layer introduced the artifacts (the observed file is 12,879 bytes but the fetch returned 12,885 characters; multibyte UTF-8 expansion plausibly accounts for the delta, and text-extraction layers can normalize escaped sequences into raw ones). Under this branch the on-disk file is strict JSON, the loader succeeds, and DR-34 was a fetch-surface measurement error — which the factory records honestly as a false alarm closed by the trace, not a finding retired by assumption.

The factory's assessment: HIGH CONFIDENCE in Branch 2 (the seal, the gate, and the strict serializer together make Branch 1's survival to shipment improbable), but the branch cannot be closed from the fetch surface alone. The decisive instrument is byte-accurate and already specified by the repository's own test infrastructure: a headless load assertion that `EventsHostSession`'s incident list contains exactly 25 records after `_Ready` — one assertion, run headless, settles both DR-34 and the loader question at once. This is recorded as a candidate Lane G/H test (a natural companion to the A-42 tranche's strict-parse assertion) and is named in the foreman question that DR-34 already poses.

## 50.3 The cross-host invariant, newly cited

The trace surfaced a repository invariant the factory had not previously cited in any volume: the JSON port's dual-host wire contract. `IJsonSerializer` exists because the same Core assembly must serialize identically under Godot (System.Text.Json) and Unity (JsonUtility template); `SaveWireContractTests.cs` is the named verification instrument; the template lives in `docs/contracts/Cross-host/`, which F-007's registration should classify as a contracts directory (one template file at minimum). Any factory plan that touches serialization — including any future save-schema work in Lane D — must author against the port, not against either host's serializer directly, and must extend `SaveWireContractTests` if the DTO surface changes. Recorded as a Lane D/Lane I matrix annotation; no plan is opened by it (the invariant is currently upheld, not violated).

## 50.4 Consequences for the A-42 contract

The tranche contract (Volume 45.5) gains two clauses from this volume: (a) new `bodyText` authoring must use escaped `\n` only — now justified not by the fetch observation but by the binding serializer's strictness, which is a source-verified fact; (b) the tranche's verification adds the headless load assertion of 50.2 as its strongest check, since it exercises the real parse path rather than a test-only parser. The tranche remains DATA-ONLY and remains unauthorized; these clauses make it safer, not started.

---

# VOLUME 51 — THE SEGMENTED-READ INSTRUMENT, GENERALIZED (Factory batch 2026-09-24-P)

## 51.1 The problem stated once

The fetch surface truncates raw URL reads at 32,793 characters. Three factory premises are currently blocked behind this horizon: the full events.json census (240,926 bytes, roughly eight segments), the A-41 choice arrays in the quest-branching catalogs (records verified beyond the head, choice arrays not), and any future large-catalog measurement (the moral-choice catalogs, the events tail, the destination prose corpus). Rather than treating each as an ad hoc struggle, the factory installs one reusable instrument.

## 51.2 The instrument

Segmented read: a catalog is measured by N sequential fetches, each bounded by the horizon, with a segment-verification rule that keeps the method honest. Because the fetch layer returns a character window rather than a byte range request, the segments cannot be requested by offset; the instrument therefore works by successive anchored reads — each fetch returns the head 32,793 characters, and the catalog is advanced by identifying the last complete record boundary within the fragment, recording that record's id, and confirming on the next fetch that the fragment is consistent (same head) before extracting the next window of records that follow it. Where the surface offers no way to skip ahead, the factory states the limit plainly: only the first ~32.8k characters are reliably readable per fetch, and catalogs exceeding that require either the repository's own test infrastructure (a headless dump through a test harness — the cleanest instrument, since the repository already runs 127 selftests headless) or foreman-assisted local reads. The generalized instrument is therefore a protocol, not a workaround: (1) read the head fragment; (2) extract all complete records within it; (3) record the last complete record's id as the resume anchor; (4) for each further segment, attempt the read and honestly record whether the surface permits advancement; (5) never extrapolate beyond the fragment read; (6) label every measurement with its fragment scope. The protocol's first application is scheduled: the A-41 scope correction (Volume 33) requires the choice arrays of `moral_choice_quests_branching.json` and the faction catalog; the head census (this volume, below) anchors it.

## 51.3 First application — the faction-branching quest catalog head

VERIFIED (fragment scope): `quests_faction_branching.json` head confirms the record shape for the A-41 program: `id` (`quest_bone_pickers_01`), `display_name` ("Bone Pickers - Stage 1"), `type` (`faction_chain`), `briefing` (a prose field, in this record containing one escaped em-dash — properly escaped strict JSON, unlike the incident catalog's fetch surface), `prereq_quest_id` (empty string for first-in-chain), `min_day` (10), and a `stages` array whose members are `{ id, text }`. Consequences: (a) the catalog's stage text is inline prose per stage, and the A-41 de-templatization target (the choice prose, per Volume 31's provenance finding) sits in fields beyond this head fragment — the choice arrays remain unread, honestly; (b) the `display_name` field corroborates Volume 41's Q-2 evidence base (display names exist as a first-class authored field in the branching catalog, with stage-suffixed naming); (c) the `prereq_quest_id` field confirms chain structure is data-declared, which is the seam any reopening-grammar plan (B-16) must respect. The A-41 program's next tranche is now fully specified at the protocol level: apply the segmented-read protocol until the choice arrays are read, then author the tranche contracts against measured, not assumed, record shapes.

## 51.4 Segmented-read candidacy register

| Catalog | Size | Segments (est.) | Blocking premise | Owner seed |
|---|---|---|---|---|
| events.json | 240,926 B | ~8 | full census, id-overlap with incidents | A-42 premise sweep |
| quests_faction_branching.json | >32.8k chars (tail unmeasured) | ≥2 | choice arrays | A-41 |
| moral_choice_quests_branching.json | unmeasured | unknown | choice arrays | A-41 |
| destination prose corpus | unmeasured | unknown | A-12's per-destination measurement | A-12 |
| INTEGRATION_PLANS.md | 78,528 B | 3 | full live-ledger read (head read only) | ledger monitoring |

The register's rule: a catalog enters here only when a seed's premise genuinely requires its depth; head reads that satisfy the premise do not create segmentation work. The factory does not schedule all five entries — A-42 and A-41 are the active owners; the others wait on their seeds' priority.

---

# VOLUME 52 — WAVE SYNTHESIS AND THE FOREMAN DECISION BRIEF (Factory batch 2026-09-24-Q)

## 52.1 What this wave changed in the record

Four corrections and three closures: DR-34 is resolved from an open flag into a two-branch fork with a decisive instrument (Volume 50); the events/incidents relationship is corrected from "sibling read model" to "parallel authored catalogs sharing vocabulary by design" (Volume 49); the incident consumer is confirmed at class level (`ShelterEncounterSystem`, via the repository's own scanner map) and the loader call path is confirmed single (`EventsHostSession`, cross-host host adapter); and the fetch-horizon method is installed as a named, honest protocol (Volume 51) rather than an occasional obstacle. The A-42 contract gains two clauses (escaped-newline authoring justified by the serializer's strictness; a headless load assertion as the tranche's strongest verification). The A-41 program gains its anchor record shape. The cross-host wire contract is newly cited as a standing invariant.

## 52.2 The foreman decision brief (consolidated, all outstanding)

The factory now holds six decisions that only the foreman can make, each with its evidence complete and each cheaper than any plan it gates:

1. Q-1 (Volume 41) — apprenticeship intent: whether pair-claim time is deliberately non-institutional. Factory recommendation: coherent as built; record, do not change.
2. Q-2 (Volume 41) — display names: the branching catalog's stage-suffixed `display_name` convention is now corroborated (Volume 51.3). Factory recommendation: treat the convention as canonical and document it.
3. Q-3 (Volume 41) — incident extension: the Day-90 ceiling is now confirmed twice; the vocabulary authoring norm is measured. Factory recommendation: any extension fills a real gap; the factory drafts only on request.
4. Q-4 (Volume 41) — W1 consumer sequence: unchanged; the ledger head is the authority.
5. DR-34 disposition (Volume 50) — the strictness question: one headless assertion settles it. Factory recommendation: add the assertion to the A-42 tranche's verification and do not repair the file unless Branch 1 is demonstrated.
6. AmputationEquipment row (Volume 46) — retire-or-intend disposition. Factory recommendation: retire with the absence search as evidence.

## 52.3 Standing after this wave

The seed register (Volume 48) is unchanged in counts; no new seeds were warranted by this wave's reads (the mirror pattern and the cross-host invariant are recorded as annotations and premises, not seeds — the anti-nonsense filter applies to the factory's own inventory as much as to the repository's). The open premise ledger shrinks by two: the incident parse path and the events/incidents relationship are closed; the events full census and the A-41 choice arrays remain, each with its protocol now defined. The implementation quartet (SB-08, F-009, H-06, E-11) remains specified, ready, and unauthorized.

---

# GROWTH LEDGER UPDATE (this wave)

- 2026-09-24 — Volume 49: the events.json head census — the catalog triple confirmed in one host session (events, incidents, narrative_progression, all loaded by `EventsHostSession`), the id-mirror finding (fallout_storm paired across catalogs with different registers and scales), the consumer map discovered in `ContentUtilizationScanner.cs` (incidents → ShelterEncounterSystem), the addressing-model distinction recorded (id-addressed events vs weighted incidents), and Volume 45's Finding 5 corrected — ~5,500 — cumulative ~607,000
- 2026-09-24 — Volume 50: DR-34 resolved to a verification instrument — the parse path traced end to end (EventsHostSession → IJsonSerializer → SystemTextJsonSerializer in HostDefaults.cs), the strictness fork stated in two branches with the headless 25-record load assertion as the decisive instrument, the cross-host wire contract cited as a standing invariant (Ports.cs, UnityJsonSerializer.template.cs, SaveWireContractTests.cs), and two new clauses added to the A-42 tranche contract — ~5,500 — cumulative ~612,500
- 2026-09-24 — Volume 51: the segmented-read instrument generalized as a six-step honest protocol, the A-41 anchor record shape confirmed (quests_faction_branching head: id, display_name, type, briefing, prereq_quest_id, min_day, stages[]), and the segmented-read candidacy register published (five catalogs, two active owners) — ~4,500 — cumulative ~617,000
- 2026-09-24 — Volume 52: the wave synthesis and foreman decision brief — four corrections and three closures consolidated, six outstanding foreman decisions each with complete evidence and a factory recommendation, seed counts unchanged by design (annotations, not seeds) — ~3,500 — cumulative ~620,500

---

# VOLUME 53 — NARRATIVE_PROGRESSION.JSON READ IN FULL: THE CAMPAIGN SPINE (Factory batch 2026-09-24-R)

## 53.1 Method

`Assets/StreamingAssets/Data/narrative_progression.json` is 3,532 bytes on disk — the only catalog of the `EventsHostSession` triple that fits entirely within the fetch horizon. This volume reports a full-file read; nothing here is fragment-scoped. The catalog is confirmed as the third member of the triple (Volume 49.4): it is loaded by the same session, through the same port, in the same `_Ready`.

## 53.2 The fifteen-chapter spine

VERIFIED (full read): `schema_version: 1`, fifteen entries, each exactly `{ description, order }`. No ids, no weights, no prose fields beyond the description — this is a chapter ledger, not an event catalog. The spine, in order:

| # | Chapter | State (as authored) |
|---|---|---|
| 1 | The Exchange — nuclear detonations across the globe | Complete |
| 2 | Ashfall — surviving the initial fallout and radiation | Complete |
| 3 | The Bunker — establishing shelter and community | Active |
| 4 | First Contact — encountering other survivors | Pending |
| 5 | The Long Winter — nuclear winter conditions setting in | Pending |
| 6 | The Consolidation — camps calling themselves permanent; roads acquire owners | Pending |
| 7 | The Long Dark — the cold becomes routine; the shelter measures what it spends in sleep, patience, and people | Pending |
| 8 | The Thaw — water moves under the ice; every reopened route brings strangers | Pending |
| 9 | The Schism — factions split over what they can afford to become | Pending |
| 10 | The Black Market — names, favors, and quiet introductions buy what currency cannot | Pending |
| 11 | The Reckoning — old promises surface beside old grievances | Pending |
| 12 | The Rebuilding — crews restore systems meant to outlast the people doing the work | Pending |
| 13 | The Second Winter — winter returns to a world that has learned how to prepare and how to take | Pending |
| 14 | The Muster — delegations, banners; who speaks for the survivors | Pending |
| 15 | The Inheritance — what will be handed forward, who will inherit it, and what they will be told it cost | Pending |

The authored state is a single position marker: Chapters 1–2 Complete, Chapter 3 Active, everything later Pending. The descriptions of Chapters 6 through 15 carry one-sentence premise prose each — the only authored prose in the file.

## 53.3 Cross-checks against the factory's standing premises

Three independent corroborations land from this single small read:

First — the mid-winter premise, now confirmed a third time and at the highest authority. Chapter 5 is named "The Long Winter — nuclear winter conditions setting in" and sits Pending immediately after the Active chapter. The campaign's own spine therefore declares that the game's playable near-future includes the nuclear-winter period — the same period the incident catalog has no vocabulary for (Volume 45, Finding 2: `minDay` ceiling 90) and the atlas measures as the Days 90–180 slump (SB-02). Three surfaces now agree: atlas, incident catalog, campaign spine. SB-02's premise is no longer a hypothesis in any sense; the factory re-labels it VERIFIED-PREMISE and re-ranks F-002 accordingly — the flagship moves to the top of the content candidates behind the sequenced implementation quartet.

Second — the second-winter field corroborated. `ShelterEncounterSystemState` carries `secondWinterActiveSince = -1` (read this same session, Volume 54 below), a persisted marker for the Chapter 13 period. The spine and the encounter system's save schema agree that a second winter is a designed, state-tracked phase. This corroborates the chapter ledger as live design, not abandoned scaffolding: at least one system already persists a field keyed to it.

Third — the chapter names corroborate existing factory clusters. "The Reckoning" (Chapter 11) matches the reckoning-enrollment sweep (B-20) by name; "The Black Market" (Chapter 10) matches the black-market funds legs (B-11, BLOCKED plus a SEALED actions surface — the spine confirms the domain is designed-for, which strengthens the case that the foreman's DP-03 decision is worth making rather than deferring); "The Muster" (Chapter 14) matches F-004's muster deep expansion; "The Inheritance" (Chapter 15) matches the heirloom and final-wishes families (A-21, A-22, HeirloomSystem). Four of the factory's existing seeds are thus pre-validated by the campaign's own naming. No new seed is warranted by any of this — the corroborated seeds already exist; the factory records the corroboration and re-ranks.

## 53.4 The spine's own open question

The catalog is a static position ledger: it records that Chapter 3 is Active but carries no field for when a chapter advances, what advances it, or who consumes the order field. Whether chapter advancement is driven by day thresholds, scripted triggers, or manual state is not answerable from this file; the consumer of `NarrativeRoot` beyond the session's load is an open premise. The factory records it as a premise, not a defect: the file is coherent as a reference ledger, and the Chapter 3 "Active" state matches the current live-ledger batch (XP W1 ACTIVE). Needs runtime verification before any plan touches it; no such plan is currently warranted.

---

# VOLUME 54 — SHELTERENCOUNTERSYSTEM DEEP READ: THE INCIDENT CONSUMER MAPPED (Factory batch 2026-09-24-S)

## 54.1 Method and surface

`Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` (15,768 bytes, read within the fetch horizon with the weight-logic region extracted) is the incident catalog's consumer, named by the repository's own scanner map (Volume 49.5). Its location is itself a datum: the encounter system lives in the `DutyRoster` tree, not a standalone events tree — the shelter-tick dispatch surface rides the duty-roster session, which corroborates the Plan 75 chain (schedule → power grid → incident) at the directory level.

## 54.2 The persisted state schema

VERIFIED (source read): `ShelterEncounterSystemState` persists `systemId` ("shelter_encounter_system"), `expansionUnlocked`, `seedSalt` (default `SeedOffset`), `lastEncounterDay` (-1), `encountersThisNight`, `encounterWeightMultiplier` (1f), `secondWinterActiveSince` (-1), a `history` list of encounter records, an `activeVisitorQueue` of visitor ids, and a `resolvedIds` list. Two consequences for the factory's standing instruments:

First — the weight seam is named and it is not the catalog's `weight` field alone: `encounterWeightMultiplier` is a persisted, per-save multiplier that scales encounter frequency. Any Lane C balance work touching incident frequency must go through this multiplier, not through re-authoring catalog weights; the A-42 tranche's frozen-`weight` clause (Volume 45.5) is thereby doubly justified — the tuning authority lives in the consumer's state, and the catalog's authored weights are the baseline it scales.

Second — `seedSalt` defaults to `SeedOffset`, and the source's own comment names the convention: "Utility AI salt. Spec: _worldSeed + 1208." This is the second repository-verified instance of the deterministic-salt convention (the factory's harness specifications already assume per-system seed salts); the encounter system's salt is 1208 off the world seed. Any deterministic incident harness must use the same salt derivation or its draws will not match the game's.

## 54.3 The encounter kind inventory and the named-visitor surface

VERIFIED (source read): the class declares string constants for at least thirteen encounter kinds — night_slate, hatch_return, meal_short, intake_sleep, levy_absence, ice_pack, edor_stool, pell_machine, stack_fever, child_chart, tin_again, intercom_office, road_dark_crowd, sela_row — and named visitor ids (`npc_edor_vale`, `npc_len_quill` visible in the read region). Two of the kind names (edor_stool, sela_row) and the visitor ids match the lore authority's named-character surface (Volume 55): the encounter system and the lore document describe the same recurring-character design. The kind names also corroborate the Case D boundary precisely: these kinds are shelter-tick interpersonal encounters (meals, intakes, illnesses, crowds), not hazard dispatches — the hazard prose lives in the catalog the system samples, and the system supplies the interpersonal frame around it.

## 54.4 Consequences for the register

The scanner-map citation (Volume 49.5) is now completed at the source level: consumer located, tree located, state schema read, weight seam named, salt named. No new seed is warranted — the read corroborates standing seeds and contracts (A-42's clauses, SB-02's premise, F-002's rank) and closes no open defect. The one open premise it leaves is recorded honestly: the exact sampling loop (how `weight`, `minDay`, and `encounterWeightMultiplier` combine per night) was not within the extracted region and requires either a full-file read at a wider window or a headless trace. The factory does not infer the formula from field names; it records the fields and flags the formula as the next read's target.

---

# VOLUME 55 — THE LORE AUTHORITY READ IN FULL: 04_ENCOUNTERS.MD AND ITS CONVERSION CHECKLIST (Factory batch 2026-09-24-T)

## 55.1 Method

`docs/lore/04_ENCOUNTERS.md` is 13,508 bytes and was read in full (within the fetch horizon). The document declares its own target files in its header: `events.json`, `echoes.json`, `survivors.json`. It is therefore the authored-design authority for three shipped catalogs — the factory had previously read the shipped `events.json` head (Volume 49) without knowing this authority existed. This volume records the document's content, its drift against the shipped catalogs, and its three open items.

## 55.2 What the authority specifies

VERIFIED (full read): the document divides encounters into character encounters ("people who recur, remember you, and want something") and situational encounters ("things that happen to you"). It names four faction figureheads as canonical institutions with faces — Colonel Voss (`iron_garrison`), Delacroix (`ash_militia`), The Vessel (`cult_of_ash_sign`), The Tollman (`warlords_sector_4`). It specifies trust-reactive events with fields beyond the shipped five-field surface: `threateningFactionId`, `threateningTrustBelow`, and a paired `threateningBodyText` — a two-register prose pattern where the same event renders benignly above a trust threshold and threateningly below it. It specifies eight echoes ("a found thing, described exactly, with choices that cost. No line explains the significance."), including `echo_the_nameplates`, whose "melt them for brass" choice writes a flag (`nameplates_hung` family) that the document says is checked at the arrival in `02_THE_LIST.md` — a cross-catalog, cross-document flag dependency the document itself flags for confirmation. It tabulates eight hazard events as deliberately low-moral-content pressure ("These exist so the moral ones land"), including `event_standby_cycle`, which the document calls "the spine's alarm clock: it fires once, around Day 190, means nothing mechanically, and is the shelter's outer hatch doing exactly what it did on the afternoon everyone walked in."

## 55.3 Finding 1 — the Day-190 event corrects the factory's Day-90 ceiling claim, at the spec layer

Volume 45's Finding 2 stated the incident catalog has no records exclusive to Days 90–180 and none gated to the Year-of-Ash window. That finding stands for `incidents.json` as measured. But the lore authority specifies `event_standby_cycle` at Day 190 — inside the Year-of-Ash window — in `events.json`. The mid-winter vocabulary gap is therefore narrower than the factory previously recorded: the ambient incident catalog has none, while the authored event catalog is specified to have exactly one, a deliberately isolated single-beat event. The corrected statement: the slump period's event vocabulary is near-empty by design, with one authored exception whose entire function is to mark time. SB-02's premise survives the correction (one deliberately meaningless event does not populate a 90-day period), but the factory records the correction as DR-35 and re-scopes any F-002 wave to respect the spec'd exception — an extension wave must not crowd the alarm clock.

## 55.4 Finding 2 — three verified open items in the authority's own checklist

The document closes with a conversion checklist (40 new locations for `locations.json`, 16 world-history beats, 10 character encounters across `survivors.json` plus `events.json`, 4 trust-reactive events plus pattern, 8 echoes, 8 hazards) and then — uniquely among the repository's documents the factory has read — lists its own unconverted items. All three are repository-verified design gaps, and each becomes a register entry:

First — `faction_archivists` needs either a `faction_lore.json` entry or an explicit decision to keep the Archivists out of the faction system entirely; the document's own parenthetical is the sharpest available statement of the issue: "they have no territory, no tribute, and no `relationships` — the schema fits badly, and that is probably the correct signal." This is a foreman decision, not a plan: the factory records it as decision packet DP-05 (new), with the document's own recommendation standing as the default.

Second — "Sela Renn's arrival needs a flag-gated trigger; no existing system fires a one-shot narrative event on a day threshold and a capability gate." This is a verified missing-system-capability statement inside a shipped authority document. It is not content debt; it is an architectural gap named by the repository itself. The factory records it as seed B-35 (new): a one-shot narrative trigger primitive (day threshold plus flag/knowledge gate, one-shot semantics, persisted so it does not refire), scoped to the smallest coherent host wiring. B-35's evidence is as strong as evidence gets for a capability gap: the authority that wants the capability states it does not exist.

Third — the `RequiredFlagId` reverse-lookup confirmation for `echo_the_nameplates`' late-read flag. This is a verification item, not a defect claim: the document asks whether the flag-reading seam supports a flag written in one catalog being read at an arrival gated much later. The factory folds it into the A-41/A-42 family's verification premise list and does not open a seed — it is one assertion in any future echo-surface test tranche.

## 55.5 Finding 3 — lore-authority versus shipped-catalog drift, measured at one pair

The authority specifies `echo_answering_service` ("a pre-war medical answering service, solar, still cycling. Forty-one messages. Most are appointment cancellations. Number 39 is a man apologising..."). The shipped `echoes.json` head (read this same session, Volume 56 below) contains `echo_answering_machine` — "Unspooled Magnetic Tape" — with different prose: an answering machine half-buried in ash, a failing solar cell, fourteen messages, a woman's voice repeating coordinates. Same premise family (a machine that kept answering after nobody could), different id, different title, different prose, different message count. This is drift between the design authority and the shipped catalog — either the shipped echo was authored independently of the spec, or the spec was revised after conversion, or the pair represents an intentional variant. The factory cannot determine which from static reads; it records the pair as the first measured instance of lore-vs-shipped drift (HIGH CONFIDENCE that divergence exists; UNVERIFIED which side is current intent) and adds a premise: any prose-contract wave for the echoes catalog must first reconcile each shipped record against this authority. The factory does not generalize one measured pair into a catalog-wide drift claim — that requires the full echoes census, which is now a named premise rather than an unseen one.

## 55.6 Register updates

New entries: DP-05 (Archivists faction decision), B-35 (one-shot narrative trigger primitive), plus the echoes-reconciliation premise attached to the A-lane prose contracts. DR-35 recorded (the Day-190 spec correction). The seed register (Volume 48) gains these in its next regeneration; this volume is the authority for their entry.

---

# VOLUME 56 — ECHOES AND SURVIVORS HEAD CENSUSES; THE MORAL-CHOICE CHOICE SHAPE MEASURED (Factory batch 2026-09-24-U)

## 56.1 echoes.json head census

VERIFIED (fragment scope, ~32,793-character horizon): `schema_version: 1`, an `echoes` root array, and a record shape of `id`, `title`, `bodyText` at minimum — the same prose-bearing surface as incidents and events. The head record `echo_answering_machine` is deep-form ambient prose (the drift pair, Volume 55.5). The catalog's size exceeds the horizon; the full census (the authority specifies eight echoes — if the shipped count matches, the whole catalog is likely two to three segments) is a named premise. The echo shape's choice fields (the authority specifies morale deltas, flags, resource costs per choice) were not within the head fragment; whether shipped echoes carry their choices inline or in a separate catalog is an open premise the census must close.

## 56.2 survivors.json head census

VERIFIED (fragment scope): `schema_version: 1`, a `survivors` root array, and a record shape of `id` (snake_case person names, e.g. `elena_vasquez`), `displayName` (proper-case), `profession` ("Paramedic"), `bio` (deep-form character prose, ~40 words in the head record), and `baseHealth` (100). The catalog is character-encounter data matching the authority's Part I surface. The bio prose depth (~40 words) is a different norm from the event/incident bands — character-establishing compression, not scene prose; any prose contract for survivors must use this measured norm, not the incident band.

## 56.3 The moral-choice branching choice shape — measured, closing the A-41 scope premise at the fragment level

VERIFIED (fragment scope, `moral_choice_quests_branching.json` head): the record shape is `id` (`quest_moral_chain_mercy_01`), `display_name` ("The Open Hand"), `category` ("share"), `trigger` (one-line premise), `discovery` (deep-form scene prose, ~55 words in the head record), `location_id` (`loc_shelter_gate`), `min_day` (15), `max_day` (0 — sentinel for unbounded, an open premise to confirm), and a `choices` array whose members carry `label`, `moral_delta` (12), `empathy_delta` (2), `set_flag` (`flag_shared_rations`), and `outcome_text` (authored outcome prose). This is the A-41 program's central measurement: the choice prose the de-templatization wave must author is now field-mapped, not assumed. Within the fragment, the visible choice prose ("Open the gate and share rations"; the outcome's opening "You open the gate. Eleven people file in, soaked, silent, grateful in a way that has no wor[ds]...") reads as scene-authored, not canned — consistent with Volume 31's provenance finding that the canned 23-line corpus was the faction-branching catalog's choice prose, while this catalog's choices may already be authored. The factory does not reverse that provenance finding on one fragment: the A-41 program's segmentation target remains the faction catalog's choice arrays, with this catalog's now-measured shape as the contract template. The premise ledger updates: choice-field shape for the moral catalog CLOSED (fragment scope); faction catalog choice arrays REMAIN OPEN.

## 56.4 The prose-band compendium, consolidated

The factory now holds measured prose norms for five shipped surfaces: incidents (23–81 words, five legacy records below 40; band target 55–80); events (head records at 110+ words, second-person decision-facing); echoes (head record deep-form ambient, exact band pending census); survivors (bio ~40 words, character compression); moral-choice branching (discovery ~55 words, scene register). Each band is a contract input for its owning seed; none is inferred. The compendium rule: no prose contract may state a band for a surface whose census has not measured it.

---

# VOLUME 57 — WAVE SYNTHESIS: REGISTER REGENERATION AND THE THIRD WAVE'S CLOSING ACCOUNT (Factory batch 2026-09-24-V)

## 57.1 What this wave changed in the record

One full-file read (narrative_progression.json), one deep source read (ShelterEncounterSystem), one authority document read in full (04_ENCOUNTERS.md), three head censuses (echoes, survivors, moral-choice branching), and one scope premise closed at fragment level (the moral-choice choice shape). New register entries: DP-05, B-35, and the echoes-reconciliation premise. New drift entries: DR-35 (the Day-190 spec correction narrowing the mid-winter vocabulary claim). Re-rankings: F-002 to the top of content candidates on a three-times-confirmed premise; B-11's underlying decision (DP-03) recommended for scheduling on spine corroboration; A-42's contract strengthened twice more (consumer-side weight authority; spine-consistent scoping).

## 57.2 The regenerated seed register deltas (against Volume 48)

Lane B additions: B-35 (one-shot narrative trigger primitive — day threshold plus gate, one-shot, persisted; evidence: the lore authority's own capability-gap statement). Decision packets: DP-05 (Archivists in-or-out of the faction system; the document's recommendation — keep them out, the schema fits badly and that is the signal — stands as default). Verification premises added: the echoes full census; the survivors census; the flag reverse-lookup assertion; the encounter sampling formula; the chapter-advancement consumer. Open premise ledger: two closed this wave (narrative triple member three; moral-choice choice shape at fragment scope), five added — the factory counts both directions, honestly.

## 57.3 The foreman decision brief, updated

Six decisions stood after Volume 52. Two are added: DP-05 (Archivists) and the echoes-reconciliation ruling (which side of the drift pair — authority or shipped — is current intent; the factory recommends treating the shipped catalog as canonical until the authority is re-ratified, because shipped data is what players receive, but the ruling is the foreman's). The brief now holds eight decisions, each with complete evidence, each cheaper than the plans they gate.

## 57.4 Standing after this wave

The implementation quartet (SB-08, F-009, H-06, E-11) remains specified, ready, and unauthorized. The A-42 tranche remains the highest-readiness content execution and is now triple-protected by contract clauses (frozen weights, escaped newlines, strict-parse assertion, cross-catalog consistency, consumer-side tuning authority respected). The factory's evidence base for the encounter family — catalog, consumer, authority, and spine — is now complete at the fragment level: every surface of the family has been read at least in part, its authority document is identified, and its remaining unknowns are enumerated as premises rather than unexamined.

---

# GROWTH LEDGER UPDATE (this wave)

- 2026-09-24 — Volume 53: narrative_progression.json read in full — the fifteen-chapter campaign spine published, the mid-winter premise confirmed a third time and re-labeled VERIFIED-PREMISE (F-002 re-ranked to the top of content candidates), the second-winter field corroborated across spine and save schema, four existing seeds pre-validated by the campaign's own chapter names, and the spine's advancement-consumer premise recorded — ~4,500 — cumulative ~625,000
- 2026-09-24 — Volume 54: ShelterEncounterSystem deep read — the consumer mapped at source level (DutyRoster tree, persisted state schema, `encounterWeightMultiplier` named as the tuning seam, seed salt 1208 verified as the utility-AI convention, thirteen encounter kinds and named visitors corroborated against the lore authority), the sampling-formula premise honestly flagged as the next read — ~4,000 — cumulative ~629,000
- 2026-09-24 — Volume 55: the lore authority read in full — trust-reactive two-register prose fields specified, the Day-190 alarm-clock event correcting the mid-winter vocabulary claim (DR-35), three verified open items converted (DP-05 the Archivists decision, B-35 the one-shot narrative trigger primitive, the flag reverse-lookup verification), and the first measured lore-vs-shipped drift pair recorded — ~7,000 — cumulative ~636,000
- 2026-09-24 — Volume 56: echoes and survivors head censuses plus the moral-choice choice shape measured — two more catalog surfaces bounded, the A-41 moral-catalog choice-field premise closed at fragment scope (label, moral_delta, empathy_delta, set_flag, outcome_text), the survivors bio norm measured (~40 words), and the five-surface prose-band compendium consolidated with its no-unmeasured-bands rule — ~5,000 — cumulative ~641,000
- 2026-09-24 — Volume 57: wave synthesis — register regeneration deltas (DP-05, B-35, five new premises), the foreman brief grown to eight complete decisions, F-002 re-ranked, and the encounter family declared fully read at fragment level with its unknowns enumerated — ~3,500 — cumulative ~644,500

Recommended next wave: Volume 58 (the echoes and survivors full censuses under the segmented-read protocol — the two remaining unbounded catalogs of the encounter family, each now with an authority document to reconcile against); Volume 59 (the encounter sampling formula read — the full ShelterEncounterSystem pass beyond the extracted region, closing the weight/minDay/multiplier combination question); Volume 60 (the 02_THE_LIST.md flag-dependency read — the authority's own cross-reference, closing the reverse-lookup verification item); and on foreman authorization, the implementation quartet (SB-08, F-009, H-06, E-11) plus the A-42 tranche remain ready as the first execution batch, with F-002 now ranked behind them on its three-times-confirmed premise.