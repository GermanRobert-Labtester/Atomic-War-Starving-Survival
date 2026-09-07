# Plan 41 — Memory That Acts: Heirlooms, Eulogies, and What the Dead Leave Behind

> **Wave:** Continuity Wave 6 — *The People In It*
> **Depends on:** 40A (identity: keepsake/phantom fields), 36A (port contract), 24C (the survivor
> journey), 18A (echoes chain, Wave 1).
>
> **Theme:** the game has an eulogy engine that nothing references, a thirty-entry heirloom registry
> that only tests read, wall-carving and confession catalogs with no consumer, and a phantom-memory
> system that *is* live — but whose `phantom_background` input is authored data nobody loads. Grief
> itself has the same disease (`ApplyGrief` is test-only, Wave 5). So when a survivor dies, the
> machinery for remembering them exists in pieces across six files and no single path runs from
> "someone died" to "the shelter is different because of it".

---

## Evidence Inventory (re-verified @ `ccac926e`)

| # | Artefact | What it is | Host reference count | Status |
|---|---|---|---:|---|
| 1 | `Assets/Ashfall.Core/Journal/ProceduralEulogyEngine.cs` (103 lines) | procedural eulogy text | **0 in `src/`, 0 in Core, 0 in tests** | **referenced by nothing at all** — the most emotional beat in the game, dead code |
| 2 | `Narrative/DwellerHeirloomCatalog.cs` (101 lines) | *"The 30 Survivor Personal Keepsakes & Heirloom Registry"* | 0 in `src/`; 1 test file (`DwellerHeirloomCatalogTests.cs`) | **test-only** — heirlooms never enter play |
| 3 | `wall_carving_templates.json` (3 defs, morale-band gated carving templates) | environmental storytelling | 0 in `src/`, 1 in Core (scanner table); `exemptionId = exempt_no_source_evidence`, `consumers=[]` | **dead content** |
| 4 | `confession_secrets.json` (8 defs: archetype, forgiveness/grudge outcomes) | moral play | 0 in `src/`; `exempt_no_source_evidence` | **dead content** |
| 5 | `echoes.json` (23 defs with `choices`, `conditions`, `minDay`) | narrative echoes | `exempt_echoes_future`: *"No loader or consumer exists yet"* | planned by Wave 1's **18A** — this plan consumes it |
| 6 | `wasteland_grave_epitaphs.json` | epitaph lines | scanner names `MemorialSystem`; **0 `src/` references** | verify it reaches the player (Core-side only is not enough) |
| 7 | `phantom_triggers.json` | phantom-memory triggers | `PhantomMemoryHostSession` (live, 2 refs) | **live** — but its per-survivor `phantom_background` input comes from the unread enrichment file (40A) |
| 8 | `SurvivorSocialCoordinator` + `MemorialSystem` | relations/memorial (6 Core refs, 2 src refs) | live | healthy; `DeathQuality`/`MemorialOutcome`/`IGriefSink` landed at `b48b4494` |
| 9 | **`ApplyGrief` — only tests call it** | Wave 5's headline table | 1 Core ref (declaration), **3 test files** | grief is unit-proven, gameplay-absent |
| 10 | `LocationMemorySystem` | place memory (5 Core refs, 1 src ref) → `StandingRecordEngine` | atlas §11: *"Hidden State (Weak Feedback)"* | partially live, barely felt |
| 11 | `CohortSystem.TryMaturation(childId, day)` | children growing up | **0 non-test `src/` callers** | the API to mature a child is never called (Wave 2's 24B/24C premise; now pinpointed) |
| 12 | `ShelterDecorSystem.cs:231–262` | memorial bridge into decor | live, but identifies keepsakes by **parsing item-id strings** | real coupling done the wrong way (fixed by 40B) |

**Reading:** nothing in this plan is missing. Eulogies, heirlooms, carvings, confessions, echoes,
epitaphs, phantom triggers, memorial morale, grief sinks, and maturation all exist as code or data.
What's absent is the **path** from death and time to a changed shelter. That path is the wave.

---

## Task 41A — From death to consequence: one memorial pipeline

**Goal:** when someone dies, six already-built things happen — an eulogy, an epitaph, a kept
heirloom, a grief effect on the living, a carving option, and a morale shift — through one pipeline
that the port contract (36) can verify.

**Files:** `Journal/ProceduralEulogyEngine.cs`, `DwellerHeirloomCatalog.cs`,
`Assets/Ashfall.Core/Memorial/*`, `Survivors/SurvivorFateSystem.cs`, `Survivors/NeedsSystem.cs`
(`OnDied:79`), `src/Host/SurvivorsHostSession.cs:111`, `src/Main.SurvivorFate.cs`,
`Shelter/ShelterDecorSystem.cs`, `Radiation`/`GuiltInsomniaSystem`, `phantom_triggers.json`,
`wasteland_grave_epitaphs.json`, new `docs/systems/MEMORIAL_PIPELINE.md`.

### Substeps

1. **Draw the pipeline before coding** — `OnDied → fate record → memorial row → eulogy text →
   epitaph → heirloom disposition → grief applied to linked survivors → morale/friction update →
   journal + briefing + audio` — and mark which of the ten steps exist today, which are Core-only,
   and which are missing entirely. (Steps 4 and 7 are the two dead ones.)
2. **Instantiate `ProceduralEulogyEngine` in the host path** with its inputs assembled from
   `SurvivorFateSystem`'s record + `DeathQuality`/`MemorialOutcome` (both landed 2026-08-31) + the
   identity layer from 40A. The engine is 103 lines of already-reviewed intent — wire it, don't
   rewrite it.
3. **Bind `ApplyGrief` for real**: for each surviving relation above an affinity threshold (from
   `SurvivorRelationsSystem`) and for co-shift trauma-bonded pairs, apply grief to
   `GuiltInsomniaSystem`/needs morale — closing Wave 5's `TEST_ONLY` row and Wave 2's 24B step 6
   with the same call.
4. **Heirlooms become items with an owner**: on death, the authored keepsake
   (`DwellerHeirloomCatalog` + 40A's `keepsake` field) is *distributed* — to a named survivor, to
   the memorial wall via decor, or lost if the body is never recovered. Each outcome is a decision
   the player can make, not a random drop.
5. **Epitaphs and carvings reach the player**: verify `wasteland_grave_epitaphs.json` lines actually
   surface in the memorial surface; wire `wall_carving_templates.json`'s morale-band gating to the
   decor/shelter surfaces (it's morale-gated content with no consumer today).
6. **Grief changes behaviour, not just numbers**: a grieving survivor should fail 24A's fitness
   verdict differently (insomnia → fatigue → errors), which the modifier stack (24B) is the
   mechanism for — declare it as a `sourceId` so 31 can attribute it in the briefing.
7. **Phantom memory**: with `phantom_background` now loaded (40A), let `phantom_triggers.json` fire
   for the right survivor in the right place (`LocationMemorySystem`), reusing the existing host
   session — this is the one system already wired that was missing an input.
8. **The empty shelf is the signal**: decor/keepsake surfaces should *show* an unclaimed keepsake and
   an unfilled carving slot, so memory is a space the player notices rather than a log line.
9. **Cost and choice**: a funeral costs hours and materials (35C's labour model) and can be refused —
   refusing has consequences through the same grief/morale path, authored in data, never a
   judgemental caption.
10. **Persistence**: prove the whole chain survives save/load — eulogy text is generated, not stored
    (idempotent from state); memorial rows, heirloom ownership, and grief state are stored and
    checksummed.
11. **Tone discipline**: eulogies must never repeat or contradict (`ashfall-write` variation rules,
    `ashfall-narrative-check`), and the register stays cold and restrained — no inspirational
    obituaries in a game about exhaustion.
12. **Tests**: one test per pipeline step, an end-to-end death→memorial→grief→morale→fitness test,
    heirloom distribution idempotency (no duplicate keepsakes on reload), grief applied once per
    pair, a determinism test of the eulogy inputs, and a port-contract check that each step's sink is
    bound (36A).
13. **Docs**: `docs/systems/MEMORIAL_PIPELINE.md` with the diagram from step 1, file:line per stage.
14. **Run the checklist** + `--data-integrity-selftest` (epitaph/carving ids now resolve).

**DoD:** a death changes the shelter in at least six observable, persisted, attributable ways.

---

## Task 41B — Time that leaves marks: place memory, decay, and what the holdfast remembers

**Goal:** make *places* and *things* accumulate history the player can read — the atlas's
"Hidden State (Weak Feedback)" promoted to a first-class information channel.

**Files:** `LocationMemorySystem`, `LandmarkDegradationSystem`, `LocationEvolutionSystem` (all under
`world_evolution`), `standing_record_memory.json` (38 defs, consumed by `LocationMemorySystem`),
`ArchiveDeskSystem`, `LibraryManualCatalog`/`library_study`, `StandingRecordEngine`,
`MapPanel.cs`/`map_detail` route, `journal`/codex, `LocationStrata` inscription data.

### Substeps

1. **Define "a place remembers"**: what a location records (who died there, what was found, what
   collapsed, what was carved), how long it persists, and who can read it — then implement only what
   existing systems already emit.
2. **Surface strata/inscription data at the point of use**: inspecting a node (`map_detail`) shows its
   memory summary — the atlas lists this as written-by-`LocationMemorySystem`/read-by-
   `StandingRecordEngine` with visibility "Low (Inspect popups)"; raise that visibility instead of
   adding a system.
3. **Scars from the player's own acts**: an expedition lost at a node, a fire, a mass grave, a
   breached hatch — each leaves a readable trace, feeding 41A's memorial path and 30's territory
   overlay.
4. **Degradation with a witness**: `LandmarkDegradationSystem` already emits `hazard_warning`
   collapses (Wave 4 evidence); those events should *change the place record*, not just the log, so a
   collapsed landmark is missing from the map afterwards and someone remembers it standing.
5. **Knowledge decay and record-keeping**: `ArchiveDeskSystem`, `library_study`, and ink/manual
   catalogs exist — connect them so records can be lost (flood, fire, no archivist on duty) and
   irreplaceable. That converts "storage" into "institution".
6. **Standing Record as the ledger of persons**: `standing_record_memory.json` (38 defs) plus
   `VoluntaryRegisterSystem`/`CensusClaimSystem` are the game's bureaucracy of people; make sure a
   name entered in life appears in the record after death (41A), so paperwork becomes remembrance.
7. **Player-authored memory**: the wall-carving templates and epitaph selection give the player a way
   to *decide* what is remembered, morale-banded as authored — the cheapest grief mechanic in the
   set and it already has data.
8. **Information channels only** (Wave 4's rule): a place's history is learned by visiting, by
   radio, or by an archivist's report — never shown globally on the map.
9. **Persistence**: place-memory records live in the world section already saved by
   `world_evolution`; prove growth is bounded (Wave 5's 39B retention rules apply to a 400-year
   world record).
10. **Tests**: write→read per event type, degradation changes the record, record loss on archive
    failure, a name flowing from census → memorial → standing record, and a snapshot of a scarred vs
    pristine node.
11. **Content**: reuse authored prose; anything new goes through `ashfall-write` + the 25A/25C text
    layer, never inline in C#.
12. **Run the checklist** + `ashfall-tilemap-world-qa` (zone/sector/world-history authority).

**DoD:** the wasteland keeps receipts of the player's campaign, and reading them is a way to learn.

---

## Task 41C — Grow up: make the cohort mature, and the years mean people

**Goal:** close the loop Wave 2 opened — children age in `CohortSystem` and `TryMaturation(childId,
day)` has **0** game callers, so nobody grows up. This task makes generation turnover real, feeding
19A's `childrenSurvived` ending input and 38's calendar.

**Files:** `CohortSystem.cs`, `src/Host/DoseLedgerHostSession.cs:45`,
`GenerationalSuccessionEngine`, `DutyRosterSystem`/`ApprenticeshipSystem`, `KnowledgeSystem`/
`library_study`, rations (22B/35B), `EpilogueMatrixRuntime` (`childrenSurvived`),
`src/Dose/DoseRegisterSurface.cs` (remove the `"sv_cohort_demo"` literal),
`src/Main.CampaignOwners.cs`.

### Substeps

1. **Call `TryMaturation` from the day loop** (a `survivors_needs`/`survivor_social`/cohort hook),
   driven by the calendar (38A) rather than raw arithmetic, and prove a 500-day run produces
   maturation events.
2. **Age classes with mechanical meaning**: infant → child → adolescent → adult → elder, each with
   authored consumption share (22B), schooling/apprentice eligibility (40A `profession`/skill), duty
   eligibility (24A fitness), and dose susceptibility (children and radiation: the genre's cruelest
   and most underused fact).
3. **Birth and the cohort ledger**: `BookChild(childId, parentIds, guessBand, birthDay,
   moralityMemory)` already exists, including the *uncertainty band* on paternity/maternity — wire it
   into the romance/family content path (parallel 150) as the authority, and make the guess-band
   a legible human ambiguity rather than a bug.
4. **Remove the demo literal**: `src/Dose/DoseRegisterSurface.cs:319`
   `CorrectBaseline("sv_cohort_demo", "high")` — a real UI path mutating a hardcoded child id
   (Wave 1's 16B class, still open).
5. **Generational succession**: `GenerationalSuccessionEngine` chapters/years (currently cosmetic per
   Wave 4's 38A row 4) should advance on maturation/death, so "Chapter 3" means a generation turned
   over, not three page-flips.
6. **Death of a child is a different event**: memorial pipeline (41A) with its own eulogy register,
   morale weight, and ending flag — restrained prose, no special-cased UI.
7. **Elders and final wishes**: `final_wishes.json` (8 defs — archetype, buff, completion text,
   morale bonus, steps) has no meaningful consumer path today; wire it into 41A's death pipeline as
   an optional quest-like beat.
8. **The long-game economy of people**: population growth must collide with rations, berths, duty
   slots, and dose limits — that collision *is* the late-game difficulty curve; verify with
   `ashfall-telemetry-playtest` on multi-year runs.
9. **Retention**: cohort records over 400 years need the Wave 5 39B caps (roll up generations into
   the standing record rather than growing lists forever).
10. **Ending parity**: assert `childrenSurvived` (19A) becomes derivable from cohort state, and add
    the reachable-branch test that the `!debtLedgersBurned`/`childrenSurvived` paths are attainable.
11. **Tests**: maturation cadence, age-class transitions, consumption share, apprentice gating,
    birth/maternity ambiguity handling, succession chapter advance, save round-trip across a
    birthday, determinism of a 400-year soak.
12. **Docs**: `docs/systems/GENERATIONS.md`, and update atlas §11 to retire the
    *"Orphan State (Underconnected)"* classification for child maturation — with evidence.

**DoD:** children grow up, are apprenticed, get sick, are mourned, and inherit the shelter.

---

## Cross-Task Dependencies

```
40A (identity fields) ──► 41A steps 2,4,7 (eulogy inputs, heirlooms, phantom)   36A (port contract)
24C (journey)          ──► 41A step 1's pipeline skeleton                        ▲ │
24B (needs stack)      ──► 41A step 6 (grief → fatigue/morale)                   │ │
18A (echoes chain)     ──► 41A/41B consume echoes.json                           │ │
38A (calendar)         ──► 41C steps 1,5 (maturation, chapters)                  │ │
35B/22B (storage/rations) ► 41C step 2, 41A step 9                               │ │
19A (derived ending)   ──► 41C step 10 (childrenSurvived reachable)  ◄──────────┘ │
                     41B (place memory) ──► 30 (territory overlay), 32C (reveal) ─┘
```

**Execution order:** 36A → 40A → 41A → 41B → 41C. 41A is the wave's spine: 41B and 41C are the same
argument applied to places and to time. Do not author new memorial prose before 41A wires the engine
that consumes it (Wave 1's 18B rule).

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. godot --headless --path . -- --content-utilization-selftest   # carving/confession/echo: consumed
7. ashfall-seed-replay: 400-year generational soak digest-stable
8. ashfall-narrative-check + ashfall-narrative-continuity        # tone + graph reachability
9. ashfall-dialog-graph-lint                                     # echo_/flag_/quest_ closure
10. bash scripts/ci/verify-fast.sh
```

---

## Estimated Effort & Risk

| Task | Core | Host | Data | UI | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|---|---|
| 41A | 1–2 | 3–4 | verify 2 | 2 | 14–18 | Medium | LOW (mostly wiring existing pieces) |
| 41B | 2–3 | 2 | 0 | 2 | 10–14 | Medium | LOW–MED |
| 41C | 1–2 | 3 | 1 | 2 | 12–16 | **High** (multi-year balance) | MEDIUM (population curves) |

**Guardrails:** no new memorial system, no new grief stat, no procedurally generated names, no
romance/family content design here (that's parallel 150 — this plan owns the mechanics), no memorial
UI that isn't a surface of an existing system, and absolutely no death without a witness: if an
event can't be surfaced in the memorial pipeline, the player experienced nothing.
