# Plan 49 — Depth Passes: Pouring the Dead Content Onto the New Rails

> **Wave:** Continuity Wave 7 — *Content on Rails & the Measurement Layer* (closing plan)
> **Depends on:** 45A/45B (the acceptance ladder and the cleared bucket), 40A/40B (identity + tags),
> 31A (event kinds), 42A/42B (voice delivery), 41A/41B (memory + place record), 15A/18A (choices and
> echoes), 46B (reachability proof).
>
> **Theme:** Waves 1–6 built rails; this wave puts the cargo on them — **452 definitions across 29
> catalogs that exist, are authored, and reach nobody.** No new systems are introduced. Every task is
> a wiring pass over content someone already wrote, validated by the acceptance ladder and the reach
> metrics rather than by a to-do list.

---

## Evidence Inventory — the cargo (from `artifacts/content-utilization.json`, re-derived at `ccac926e`)

| Cluster | Catalogs | Defs | Rails it needs (already planned) | Where it should land |
|---|---|---:|---|---|
| **Place & atmosphere** | `environmental_atmosphere_expansion.json` (152), `environmental_texts_expansion_05.json` (36) | **188** | 41B place record, 31A kinds, 42A voice triggers, 25C overlays | node inspect text, briefing atmosphere line, weather/season flavour tied to real state |
| **Medical & clinical** | `medical_texts.json` (83) | **83** | `DiagnosisKnowledgeStore`, `AutopsySystem`, `MedicalWardSystem`, 40B tags | diagnosis hints, autopsy findings, ward notes — knowledge the player *earns* |
| **Memory corpus** | `audio_logs_expansion_05.json` (30), `journal_entries_expansion_05.json` (28), `memorials_expansion_05.json` (27) | **85** | 41A memorial pipeline, `JournalSystem`, `DwellerHeirloomCatalog` (test-only today) | eulogies, found logs, memorial rows with authored variation |
| **Encounter & choice corpus** | `narrative_encounters_expansion.json` (29), `narrative_arc_events.json` (15), `moral_choice_quest_stubs.json` (10), `narrative_questlines.json` (8), `dynamic_questlines.json` (2) | **64** | 15A resolver, `DoorEncounterSystem`/`ExpeditionEncounterBridge`, 18A echo chain | playable choices — 215 moral-choice quests already await the same route |
| **Small trade/collection/ritual families** | `trade_texts.json` (4), `trade_screen_scenarios.json` (3), `cassette_sets.json`, `confession_secrets.json` (8), `final_wishes.json` (8), `damaged_map_zones.json` (3), `wall_carving_templates.json` (3), `guilt_sources.json` (20), `expansion_survivor_fields.json` (72), `expansion_item_tags.json` (67) | **~190 +** | 40A/40B, 42C pair events, 44B, 43A succession | negotiation tells, collectibles, confessions/grudges, final wishes, heirlooms |
| **Counted as zero because of root-array shape** | `cassette_sets`, `guilt_sources`, `confession_secrets`, `damaged_map_zones`, `final_wishes`, `*_survivor_fields` | invisible | **45A step 3** (counting fix) | can't be accepted until they're measurable |

**Context from earlier waves:** only **4 of 411** catalogs reach `EFFECT_PRODUCED`; `echoes.json` (23)
is exempted as "future content, no loader"; `AGENTS.md` still lists 56→196 narrative files as a
resolved tracking problem while 272 catalogs are policy-declared codex-only. Wave 1's 18A is the
pattern to repeat; Wave 7's 45A is the gate that makes repeating safe.

---

## Task 49A — Place and atmosphere: the wasteland gets a texture the systems agree on

**Goal:** land 188 authored atmosphere lines on the rails so a place reads differently by weather,
season, dose, territory, and what happened there — and the difference is *caused*, not rolled.

**Files:** `environmental_atmosphere_expansion.json`, `environmental_texts_expansion_05.json`,
new `Assets/Ashfall.Core/World/AtmosphereCatalogLoader.cs` + `AtmosphereTextSystem.cs`,
`LocationMemorySystem`, `src/UI/MapPanel.cs` / `map_detail` route, `src/Main.Campaign.cs`
(briefing), `42A` voice triggers, `src/Host/WorldHostSession.cs`, `docs/narrative/`,
`Ashfall.Core.Tests/AtmosphereContentTests.cs`.

### Substeps

1. **Re-read the actual data shape** before designing (field names, condition vocabulary, whether
   entries carry `minDay`/weather/zone keys) — Waves 1 and 6 both found that summarised schemas are
   wrong schemas.
2. **Load + validate** with the shared loader pattern (`SystemTextJsonSerializer`,
   `CatalogDiagnostics.Warn`), then extend the integrity tiers so every atmosphere entry's references
   (`loc_`, `zone_`, `sector_`, weather/state keys) resolve.
3. **Select on state, not on randomness alone**: atmosphere is keyed by
   (place, weather, season, place-memory flags, control state); the RNG breaks ties. Every line shown
   must be *explainable* — "ashfall + first winter + the depot collapsed here".
4. **Deliver to three surfaces, with budgets**: node inspect (map detail), the day briefing (one
   atmosphere line, 31A section), and place-linked voice (42A) — with the density limits 42B step 6
   defines, so texture never outruns information.
5. **Never let flavour carry a warning** (42B step 7): if an atmosphere line implies danger, the
   mechanical fact is also stated by the panel/briefing that owns it.
6. **Knowledge-gate it** (32C step 9): an unsurveyed place gets fewer, vaguer lines than a visited
   one — the ladder already exists.
7. **Text through 25A/25C**: atmosphere entries become overlay-able so translators and packs can
   reach them; nothing new lands inline in C#.
8. **Deduplicate against existing prose** (codex/journal/radio) — three content families describing
   the same place must not contradict each other: run `ashfall-narrative-continuity` on the merged
   set.
9. **Measure reachability**: with 46B's synthetic players, what share of atmosphere entries can a
   normal 60-day campaign see? Report the number; retarget the selection rules if it's <10 %.
10. **Delete the unplaceable**: entries that can never match a reachable state (bad zone id,
   impossible condition combo) are fixed or removed by 45A's ladder, not left as dead weight.
11. **Tests**: load/validate, selection determinism, state-explainability (each line has a traceable
    cause), knowledge gating, no-warning-only-by-flavour assertion, reachability report shape.
12. **Acceptance**: mark the two catalogs `SELECTED`/`EFFECT_PRODUCED` per 45A's ladder and remove
    their `exempt_no_source_evidence` entries.
13. **Run the checklist** + both content gates.

**DoD:** 188 written lines become place-conditional texture that a player can describe back to you.

---

## Task 49B — Clinical knowledge and the memory corpus: what the ward and the archive know

**Goal:** land 83 medical texts and 85 memory-corpus entries so diagnosis, autopsy, caregiving,
memorials, and journals vary by authored knowledge instead of by template.

**Files:** `medical_texts.json`, `audio_logs_expansion_05.json`, `journal_entries_expansion_05.json`,
`memorials_expansion_05.json`, `DiagnosisKnowledgeStore.cs`, `AutopsySystem.cs`,
`MedicalWardSystem.cs` + `MedicalTreatmentCatalog.cs`, `AfflictionContracts.cs`,
`JournalSystem`, `MemorialSystem`/`DeathQuality`/`ProceduralEulogyEngine` (41A),
`DwellerHeirloomCatalog` (41A), `caregiving` route,
`Ashfall.Core.Tests/ClinicalKnowledgeTests.cs`, `Ashfall.Core.Tests/MemoryCorpusTests.cs`.

### Substeps

1. **Split the medical corpus by role**: which texts are *player-facing knowledge* (diagnosis hints,
   manual entries), which are *procedural* (autopsy/ward notes), and which are *diegetic* (found
   documents) — three consumers, three acceptance targets, one file.
2. **Wire knowledge gating**: `DiagnosisKnowledgeStore` gates what the player is told; a text is
   unlocked by study/library/autopsy progression (35C step 7, `library_study`), so medical literacy is
   a path the player can feel walking.
3. **Autopsy as the discovery channel** (24C step 9): an autopsy can *produce* a clinical text —
   converting the dead into knowledge, which is the single best use of that catalog.
4. **Ward notes must vary by death quality**: `DeathQuality`/`MemorialOutcome` (landed `b48b4494`)
   select authored memorial/journal variants, so two deaths do not read identically.
5. **Journal and audio logs become findable objects**, not ambient text: delivered through the
   inventory/scavenge/exploration channels (20A, 32B, 33A) with a place and a day, so the archive is
   something the crew *has*.
6. **Bound the archive** (39B retention): journal/memorial growth under a 400-year run must be capped
   or rolled into the standing record (41B step 9).
7. **Continuity**: names, dates, and places referenced in the memory corpus must agree with the
   survivor identity layer (40A) and place records (41B) — run `ashfall-narrative-continuity` and
   treat failures as data bugs.
8. **Tone pass**: `ashfall-write`/`ashfall-narrative-check` on every merged family — cold, restrained,
   specific; no heroic eulogies, no expository journals.
9. **Reachability report** per family with 46B's synthetic players (a memorial line nobody can reach is
   a wasted authoring session).
10. **Tests**: per-family acceptance (loaded → registered → selected → effect), knowledge-gating
    progression, autopsy-produces-text, death-variant selection, bounded growth, determinism of a
    100-death memorial run.
11. **De-exempt** all four catalogs in `ContentExemption.cs` and update
    `docs/data/CATALOG_REGISTRY.md` by regeneration.
12. **Run the checklist** + narrative gates + `--data-integrity-selftest`.

**DoD:** the ward learns, the dead are remembered distinctly, and the archive is a place with a
capacity.

---

## Task 49C — Choices and collection: the encounter corpus, and closing Wave 7

**Goal:** land the 64-entry encounter/choice corpus onto the resolver paths, give the small
ritual/collection families one consumer each, and publish the wave's evidence.

**Files:** `narrative_encounters_expansion.json`, `narrative_arc_events.json`,
`moral_choice_quest_stubs.json`, `narrative_questlines.json`, `dynamic_questlines.json`,
`echoes.json` (18A), `confession_secrets.json`, `final_wishes.json`, `cassette_sets.json`,
`damaged_map_zones.json`, `wall_carving_templates.json`, `trade_texts.json`,
`DoorEncounterSystem.cs`, `ExpeditionEncounterBridge.cs`, `MoralChoiceSystem` + 15A route,
`QuestlineSystem`, `VinylMoraleSystem` (wired in `62009ddc`), `44B` pair events, `38C` commitments,
`docs/roadmap/WAVE_LEDGER.md` (29C).

### Substeps

1. **One resolver for all choice content**: encounters, arc events, echoes (18A), and moral-choice
   quests must land through the *existing* `ResolveChoice` + effect-applier idiom
   (`ExpeditionHostSession.cs:432`, `Main.YearOfAsh.cs:300`, `DutyRosterHostSession.cs:151`) —
   Wave 1's 18A step 5 rule, applied at corpus scale so choice plumbing doesn't fork again.
2. **Wire the 215 authored moral-choice quests** behind 15A's player route and measure: choices
   per 30-day session, and the share reachable in a normal campaign.
3. **Delete or adopt `moral_choice_quest_stubs.json`** (10 defs, no loader) — stubs without an owner
   are the seed of the next false affordance (45B step 3).
4. **Merge the questline families deliberately**: `narrative_questlines`, `dynamic_questlines`,
   `questline_master`, expansion quest files all describe questlines — publish one ownership table
   (29C's "one authority per fact") and retire the duplicates with a migration note, or the next
   author picks a file by coin flip.
5. **Confessions and grudges as pair events** (44B step 4): disclosure writes history, shifts band,
   and may forgive or entrench — 8 defs, one consumer, done.
6. **Final wishes as authored death beats** (41C step 7): 8 defs with steps/morale bonus feeding the
   memorial pipeline; restraint required.
7. **Collectibles stay collectible, not consumable-by-code**: `cassette_sets` (hidden caches) through
   the vinyl/collection surface already wired in `62009ddc`; `damaged_map_zones` through 32C's
   fragment reveal; both counted properly after 45A step 3.
8. **Trade tells**: `trade_texts` into `TradeTellEngine`'s stance × trust-band selection (a documented
   consumer already exists: "selection is seed-deterministic") — cheap, and it makes bargaining feel
   populated.
9. **Fix the invisible ones first**: the root-array families must be counted (45A step 3) before any
   of steps 5–8 can be accepted — otherwise the ladder reports success on zero definitions.
10. **Acceptance sweep**: every catalog in this plan's table moves from `exempt_no_source_evidence` /
    `DISCOVERED` to at least `SELECTED`, or is deleted with a record (45A's two honest exits).
11. **Reachability + report**: publish per-family reachability from 46B, and a Wave 7 evidence table
    (before/after defs wired, exemption count, `EFFECT_PRODUCED`, dead-core-class count) into
    `docs/roadmap/WAVE_LEDGER.md` (29C) and this wave's index.
12. **Docs**: `docs/content/ACCEPTANCE.md` gains its worked examples from these three tasks — the
    ergonomics loop (45C step 4).
13. **Run the checklist** + both content gates + narrative gates + the release gate (48B step 6).

**DoD:** the encounter, ritual, and collection corpora are playable, reachable, and reported — and
the dead-content bucket stays empty because the gate is now load-bearing.

---

## Cross-Task Dependencies

```
45A/45B (ladder + cleared bucket) ──► everything below
40A/40B (identity, tags) ──► 49B steps 2,5 · 49C steps 5,7
31A (kinds) ──► 49A step 4, 49C step 1        42A/42B (voice) ──► 49A step 4
41A/41B (memorial, place) ──► 49A step 3, 49B steps 4,5,7
15A/18A (choice route, echoes) ──► 49C steps 1,2      46B (reachability) ──► every step 9/11
44B (pair events) ──► 49C steps 5,7                   38C (commitments) ──► 49C step 6
```

**Wave 7 order:** 45A → 46A → 47A → 46B → 48A → **49A** → 45B → 47B → 48B → **49B** → 46C →
47C → 48C → **49C**. Content never leads the rails; it follows them by exactly one step.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors (+ new tiers)
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. bash scripts/ci/content-acceptance-gate.sh                    # per-family ladder status
7. godot --headless --path . -- --content-utilization-selftest   # DEFS WIRED must rise
8. 46B synthetic reachability report per family
9. ashfall-narrative-check + ashfall-narrative-continuity + ashfall-dialog-graph-lint
10. bash scripts/ci/generate-catalog-registry.py --check
11. bash scripts/ci/verify-fast.sh
```

---

## Estimated Effort & Risk

| Task | Core | Host | Data edits | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|---|
| 49A | 1–2 | 2 | 2 catalogs + ids | 8–12 | Medium | LOW (additive texture) |
| 49B | 2–3 | 2–3 | verify 4 | 10–14 | Medium | MEDIUM (knowledge gating changes ward difficulty) |
| 49C | 2–4 | 3 | **~12 catalogs**, 2 merges | 14–18 | **High** (volume + dedup) | MEDIUM (quest-file merges touch saves) |

**Guardrails:** no new system in this plan — if a family needs one, it becomes a Wave 8 plan; no
content authored to satisfy a metric (45A's whole lesson); no second choice resolver; no duplicate
questline authority left standing; no inline prose (25A gate); no family accepted without a
reachability number; and no claim in a wave report without a generated artifact behind it (29B).
