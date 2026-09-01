# Continuity Wave 4 — Audit Index (Plans 30–34): *The World Beyond the Gate*

**Snapshot audited:** `ccac926e` (branch `main`, 95 uncommitted paths) · **Date:** 2026-08-31
**Gates re-run this wave:** `dotnet build Ashfall.csproj` 0/0 · `dotnet test` **5303 passed / 0
failed** · `--data-integrity-selftest` **PASS 138 catalogs / 5563 ids / 0 errors** ·
`triad-drift-gate` PASS · `warning-baseline-gate` PASS (0 warnings) · `doc-link-gate` PASS ·
and still-red from Wave 3: `sync-agent-rulebooks` / `generate-docs-index` /
`generate-agent-skills-catalog` (`--check` all exit 1).

Previous waves: [W1 story](Wave1_Continuity_Audit_INDEX.md) ·
[W2 physics](Wave2_Continuity_Audit_INDEX.md) ·
[W3 ship](Wave3_Continuity_Audit_INDEX.md).

---

## ⚠️ Erratum — this wave disproved one of my own Wave-1 findings

**Plan 17 Task 17A** (`Plan_17_Legibility_Cause_Effect_Guidance.md`) states: *"the typed
`DayStateChangeEvent` channel has exactly one producer (`SurvivorFateSystem`), so 18 of 19
day-advance owners fall through to the hand-rolled fallback branch."*

**That is false.** Re-measured at `ccac926e`:

| Claim | Reality |
|---|---|
| "one producer" | **26 emission sites across all 19 owner classes** in `src/Main.CampaignOwners.cs` (plus Core's `SurvivorFateSystem.cs:129/348/359` and a perf harness) |
| "briefing runs on fallback" | `src/Main.Campaign.cs:225` — `if (args != null && args.AllEvents().Any())` is satisfied every day, so the **primary** path (`DailyBriefingReportBuilder.BuildFromDayEvents`) runs; the fallback is the exception branch |

**The real defect is different and cheaper:** the producer and consumer vocabularies diverged.

- **27** distinct kinds are emitted; `DailyBriefingReportBuilder.cs:49–115` handles **14** and has
  **no `default:`** → **20 emitted kinds are silently dropped** (`power_ticked`, `needs_ticked`,
  `market_ticked`, `duty_roster_ticked`, `maritime_ticked`, `phase0_ticked`, `memorial_checked`,
  `events_evaluated`, `journal_ticked`, `survivors_ticked`, `survivor_social_ticked`,
  `shelter_facilities_ticked`, `greenhouse_foundry_ticked`, `holdfast_ticked`, `narrative_ticked`,
  `medical_disease_ticked`, `world_ticked`, `world_evolution_ticked`, `expeditions_ticked`,
  `shelter_decor_morale`).
- **6 handled kinds are never emitted** — `survivor_condition`, `resource_delta`,
  `shelter_consequence`, `weather_condition`, `radio_transmission`, `crafting_production`: the
  builder was written for a semantic vocabulary the owners never adopted, so owners emit
  `<domain>_ticked` heartbeats instead.
- There is **no `DayEventKinds`** authority (`grep` → 0 hits): kinds are literals on both sides.
- Owner **failure** (`DayOwnerReport.FailureMessage` / `HasFailures`) is never surfaced — a skipped
  system reads as a quiet day.

**Consequence:** 17A should be executed as **[Plan 31](Plan_31_Event_Layer_Semantic_Kinds_No_Silent_Drops.md)**
instead — same goal, correct diagnosis, roughly a third of the work (the transport and the consumer
already exist). 17B (guidance) and 17C (audio) stand unchanged.

---

## Wave 4 findings: the 10 highest-impact outside-world gaps

| # | Gap | Category | Severity | Why it matters to the player | Smallest action | Deps | Timing |
|---|---|---|---|---|---|---|---|
| 1 | **The briefing silently discards 20 of 27 event kinds** — switch with no `default`, 6 handled kinds never produced | UX/feedback + technical | **critical** | The day's report reads as a few facts and a lot of calm; causes never appear | `DayEventKinds` in Core; owners emit transitions; `default:` surfaces unknowns | none | **before** 30/32/33 (they all report through this) |
| 2 | **The faction war never advances** — `FactionWarSystem.SimulateDailyFriction(day)` (`:87`) has **zero** game-path callers (tests only); `YearOfAshHostSession.TickDay` ticks timeline/deep-freeze/radon/warlord and skips `_factionWar` | core loop / system connection | **critical** | The world is a painting. Standing, tension, territory, decrees and artillery exist and are frozen | call it from the day loop with the campaign RNG; add `world_politics` owner | 31 | before next expansion |
| 3 | **`FactionWarChainRunner` is save-only** — declared in `YearOfAshSave.cs:34,113,148`, its `ResolveChoice` never constructed in `src/` | content / system connection | **important** | Authored multi-stage war chains exist and can never be played | instantiate + resolve through the existing choice idiom | 31 | during 30 |
| 4 | **The travel graph is 6 nodes with no distances** — `wasteland_map_v1.json`: 6 nodes / 7 routes, every `distance_km` empty though `MapRouteDef` declares it; registry claims "261 Nodes … terrain-modified travel vectors" | technical architecture / content | **important** | Route choice can't be a decision; the map is decoration over a list of names | one place-authority ADR + populate nodes/distances + graph integrity tier | 31 | before 30B/33 |
| 5 | **Expeditions don't use the graph** — no `WastelandMap`/`MapNode` reference in `Expeditions/*`; travel is `locationId` with `unknown_target` validation (`ExpeditionHostSession.cs:225,376`) | system connection | **important** | Fuel, dose, weather and risk are per-destination constants rather than a journey | `Estimate` as a sum over path edges | 32A, 20A | during |
| 6 | **Traceable distress signals are dead content** — `radio_distress_signals.json` (5 signals with `days_to_trace`, clarity-graded `message_fragments`, `outcome_type: survivor_community`) has **no loader**; `knowledge_points` has **0 consumers** | content / system connection | **important** | The one authored "we heard something, do we go?" loop can't happen | loader + `DistressSignalSystem` + outcome handlers into existing reveal/census/loot paths | 31, 32C | during |
| 7 | **Reveals unlock nothing; intel costs nothing** — `OnLocationRevealed` feeds only status text/journal/panel; listening draws no power, time, or operator | UX / balance | **important** | Discovery has no payoff and no price, so radio is a screensaver | gate expedition targeting on reveal state; charge power/time/battery | 32C, 23 | during |
| 8 | **`hardcore_economy_tuning.json` is applied as three empty arrays** — `src/Main.Economy.cs:194–199` passes `Array.Empty` for scarcity, faction preferences and price-shock rules; the loader is only called from a selftest (`HostCli.PanelTests.cs:1012`) | balance | **critical** | Every tuned scarcity/shock number in the file does nothing; balance work against it is theatre | load and apply the real bundle (+ a gate forbidding the empty call) | none | **immediately — one line** |
| 9 | **No difficulty axis; achievements are render-time strings** — `grep -i difficulty src/` → 0 hits, settings has no presets; `AchievementsPanel.cs:14` admits "An AchievementsHostSession does not exist yet", with 14 literal rows and milestones as `_simDay < 7 ? …` | progression / UX | **important** | No way to ask for a gentler or harsher game; nothing you did is recognised beyond a caption | presets in data + observed milestones from the event stream | 31, 34B step 1 | after 1–3 |
| 10 | **Nothing remembers a campaign** — all stores route through `SaveSlotRoot` per slot; no meta/legacy layer (`metaProgress\|NewGamePlus\|carryover\|inheritance` → 0 real hits) | progression / production | **later** | After 200 days and one death you were attached to, the menu knows nothing | a records-only `LegacyLedger` (no bonuses) written at the epilogue | 19A, 34A | after the world works |

---

## Plans in this wave

| Plan | Title | Closes | DoD in one line |
|---|---|---|---|
| [30](Plan_30_War_Runs_Without_You_Autonomous_World.md) | The War Runs Without You | 2, 3 | Day 90: the world moved, and the briefing says who did it. |
| [31](Plan_31_Event_Layer_Semantic_Kinds_No_Silent_Drops.md) | The Event Layer Speaks | 1 (+ Wave 1's 17A erratum) | 0 emitted kinds dropped, 0 handled kinds unproduced, every line names its cause. |
| [32](Plan_32_One_Map_Three_Notions_Of_Place.md) | One Map, Three Notions of Place | 4, 5 | One authority answers where/how far/how dangerous/who holds it — and the docs agree. |
| [33](Plan_33_Intel_Worth_Something_Radio_Traces_Secrets.md) | Intel Worth Something | 6, 7 | Five authored distress signals become five decisions; the radio can lie, and knowing costs. |
| [34](Plan_34_Long_Arc_Milestones_Difficulty_Legacy.md) | The Long Arc | 8, 9, 10 | Tuning actually tunes, milestones are observed, and the menu remembers you. |

---

## Cross-wave picture

| Wave | Question | Plans | One-line result |
|---|---|---|---|
| 1 — Story machine | Does choosing matter? | 15–19 | A choice, four consequences, an ending that reads the campaign. |
| 2 — Bunker machine | Does doing matter? | 20–24 | Dose from place, gear that dies, food that feeds, watts that bind, bodies that tire. |
| 3 — Ship it intact | Can we build/test/describe it? | 25–29 | Words translatable, artifact boots with its data, tests mean it, registration unforgett-able. |
| 4 — World beyond the gate | Is there anything *else* going on out there? | 30–34 | The war runs, the roads are real, intel is worth paying for, and the run is remembered. |

**Unifying root cause across all four waves:** measurement asks *"does it exist?"* and almost never
*"does it act?"* — Wave 1's liveness gate, Wave 2's single-authority principle, Wave 3's
runtime-evidence/fixture fidelity, and Wave 4's `DayEventKinds` + `default:` branch are the same
correction at four altitudes. This wave's #1 finding (20 kinds dropped by a missing `default`) is
that principle in its purest form: the transport, producers and consumer were each "present", and
74 % of the vocabulary evaporated quietly.

**Wave-level order:** 31A → **34B step 1** (one-line balance truth) → 30A → 32A → 33A → 30B →
32B → 34A → 33B → 34B → 30C → 32C → 33C → 34C. Three-task minimum: **31A, 30A, 34B-step-1** — a
report that doesn't throw information away, a world that moves on its own, and tuning numbers that
actually apply.

## Metrics to report at wave close

1. Emitted day-event kinds silently dropped: **20 → 0** (and 6 unproduced handled kinds → 0)
2. Game-path callers of `SimulateDailyFriction`: **0 → the day loop**; `FactionWarChainRunner`: **save-only → ticked**
3. Travel graph: **6 nodes / 7 routes / 0 distances → authored N nodes, every route with a distance**, graph-integrity tier in the selftest
4. Expeditions routing on the graph: **0 → every dispatch**, with per-edge dose/fuel/risk shown
5. Catalogs with no consumer (Wave 1 baseline `exempt_no_source_evidence` 26): **−5** (distress signals, damaged map zones, knowledge fields, 2 narrative clusters)
6. `hardcore_economy_tuning.json` rules applied in the live campaign: **0 → non-zero, gated against regression**
7. Achievements: **14 literals → 100 % data-driven predicates**; milestones observed from events, not `_simDay` comparisons
8. Cross-campaign record: **none → written once at the epilogue, read by the menu, zero stat bonuses**
9. Registry claims corrected with evidence pointers: `261 Nodes`, `PARTIAL/STUB` rows, plus Wave 3's four

## Deferred to Wave 5 → **now planned**

**[Continuity Wave 5 — Plans 35–39, *The Human Interface*](Wave5_Continuity_Audit_INDEX.md)** picked
these up and found the root cause of all four prior waves: **74 of 147 Core integration seams
(`Bind*/Set*/Wire*/Register*/Apply*`) have no caller in `src/`** — including `ApplyGrief` and
`ApplySedative`, which only unit tests ever invoke. Read that index before executing any plan here.

Original candidates:
* **Inner-world depth** — survivor voices, relationships, hidden agendas and autonomy (coordinate with the parallel plans 132/144/147/148/150/154, which add systems Wave 2's authority work must own first).
* **Shelter governance, defence and refugees as player-facing structures** (parallel 138/158/159/156) — sequence after 30B so outside pressure has a source.
* **Outposts, vehicles as mobile base, working animals, espionage** (parallel 151/152/153/160/155) — content-heavy expansions, each needing Wave 4's graph and intel layer to be meaningful.
* **Release-track polish** — first-hour funnel telemetry over 20+ seeds, accessibility conformance, export artifacts, store readiness (Wave 3's 26C/25C continue this).
