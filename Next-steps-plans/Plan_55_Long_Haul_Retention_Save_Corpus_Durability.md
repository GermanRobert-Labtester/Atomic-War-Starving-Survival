# Plan 55 — The Long Haul: Retention, a Save Corpus, and the 400-Year Campaign

> **Wave:** Continuity Wave 9 — *Weight, Durability & the Shop Window* (Plans 55–59)
> (predecessors: [W1](Wave1_Continuity_Audit_INDEX.md)–[W8](Wave8_Continuity_Audit_INDEX.md))
> **Depends on:** 39B (soak harness), 48A (save-compat fixtures), 41C/38 (generations + calendar make
> long runs real), 31 (journals/briefings are the growth vector).
>
> **Theme:** the game now supports multi-year play (Wave 6's 41C matures children, Wave 4's 38 turns
> the calendar, 30A runs the war) — and **there is no retention policy anywhere in the codebase**.
> One grep for retention/trimming/caps returns ballistics `RicochetRetention` and the briefing's
> `maxEntriesPerSection`. Meanwhile 7 MB of *real* accumulated saves — dated `holdfast_archive_*`
> folders produced over weeks of development — live only on one developer's disk: the most valuable
> regression asset in the project is uncommitted, unversioned, and about to be lost by an `rm -rf`.

---

## Evidence Inventory (re-verified @ `ccac926e`)

| # | Fact | Evidence |
|---|---|---|
| 1 | No retention/cap policy in Core | `grep -rniE "Retention\|TrimOldest\|maxEntries\|RollingWindow\|Prune" Assets/Ashfall.Core src/` → only `TimberCarpentryCatalog.RetentionKgM3` (density), `BallisticsSystem.RicochetRetention`, and `DailyBriefingReportBuilder.cs:27,113–116` `maxEntriesPerSection` (rendering cap, not storage) |
| 2 | Unbounded logs are already in saves | `KitchenNutritionState.servingLog`, `List<string> enactedDecrees`, `totalArtilleryStrikesLogged`, `JournalSystem` entries, memorial rows, census claims, pair histories (Wave 6's 44B step 2 invented the need for a cap), `DoseLedgerSystem` records, `MachineLogSystem` reads |
| 3 | Perf tiers for long runs exist | `src/Host/PerformanceSelfTest.cs` builds 30/180/360-day sessions (`WorkloadProfile.Days30/Days180/Days360`), `artifacts/runtime-scale-results.json` (`day_advance_30d` median 0.609 s, n=5) |
| 4 | The soak harness exists, isn't a gate, and has no growth assertion | Wave 5's 39A step 1 registered the 7-day determinism verb as a gate proposal; no monotonic-growth assertion anywhere |
| 5 | **Real save corpus is uncommitted** | `~/.local/share/godot/app_userdata/ASHFALL- Atomic War - Starving Survival/` → **7 MB** across dated `holdfast_archive_20260815 → 20260819…` folders plus current slots — weeks of genuine campaign states, none in the repo |
| 6 | Save shape has a strong envelope but no size story | Initiative #42's single `campaign.json` per slot, `SaveChecksum`, 62-store matrix, `.bak` rotation — nothing asserts a size ceiling or growth rate |
| 7 | Multi-year content is now load-bearing | `CohortSystem` + `GenerationalSuccessionEngine` chapters/years (`TotalYearsElapsed`), `LocationMemorySystem`, `StandingRecordEngine`, `FactionWarSystemState` — all designed to run for decades and all with per-entity lists |
| 8 | Long-session leak checks are prose, not gates | `docs/ui/UI_NODE_DIAGNOSTICS_AND_LEAK_TRIAGE.md` (a contributor guide), Wave 1's 16C subscription defects (4 panels) fixed only under Plan 16 |
| 9 | Store expectations | a 400-year campaign is not a demo, but it *is* the review headline for a survival-management game — long-run stability is a commercial property, not a technical nicety |

---

## Task 55A — Decide what a campaign is allowed to remember

**Goal:** an explicit retention policy for every growing collection, so a 400-year run stays
bounded, readable, and deterministic.

**Files:** new `Assets/Ashfall.Core/Records/RetentionPolicy.cs` (+ `RollingLog<T>`),
`KitchenNutritionSystem.cs` (`servingLog`), `DoseLedgerSystem.cs`, `Journal/JournalSystem.cs`,
`Memorial/*`, `CensusClaimSystem.cs`, `VoluntaryRegisterSystem.cs`,
`LocationMemorySystem.cs`, `YearOfAsh/FactionWarSystem.cs` (`enactedDecrees`,
`totalArtilleryStrikesLogged`), `Verdict/MachineLogSystem.cs`, `Survivors/SurvivorRelationsSystem.cs`
(pair histories, 44B), `SaveSectionRegistry.cs`, `docs/saves/RETENTION.md`,
`Ashfall.Core.Tests/RetentionPolicyTests.cs`.

### Substeps

1. **Inventory every growing collection** in persisted state — file, field, growth rate per day, and
   what a player can lose by capping it. This table is the task's core deliverable; nothing gets
   capped before it exists.
2. **Write the policy per collection**, not one global N: `keep newest K`, `roll up into a summary`
   (e.g. 500 meals → "meals served 512, avg nutrition X"), or `keep all references, drop prose`
   (memorials keep the name, lose the paragraph).
3. **Roll-ups are Core, deterministic, and reversible-in-meaning** — a summary must be derivable
   from the same inputs every time and must never contradict what the player was told (31's
   attribution).
4. **Introduce `RollingLog<T>` + `RetentionPolicy` in Core** (engine-free, invariant-safe) and
   migrate collections one at a time with an equivalence test each — never a bulk rewrite of 15 files.
5. **Distinguish display cap from storage cap**: `maxEntriesPerSection` is display; storage is
   separate, or the player silently loses history when the UI gets tight.
6. **Size the save**: measure `campaign.json` per day for a 400-year soak (step 41C's harness) and
   state a ceiling; a save that grows 1 MB/decade is a design fact worth writing down.
7. **Version the policy**: caps change what's saved, which changes `SaveChecksum`-adjacent shape —
   bump store schema versions deliberately with a migration that reconstructs roll-ups for old saves
   rather than dropping them (48A's compatibility discipline).
8. **Never cap obligation state**: deadlines (38C), grief (41A), pair history (44B), and ending
   flags (19A) must be retained or explicitly summarised with their consequence intact — losing a
   promise is worse than losing prose.
9. **Refuse silent truncation on load**: an oversized legacy save loads, gets rolled up, and reports
   what was summarised (via the save diagnostics from 48C step 7).
10. **Add the growth assertion to the nightly soak** (39B step 2): day-400 cost ≈ day-10 cost, and
    save size within ceiling — failing on slope, not on absolute.
11. **Tests**: per-collection cap and roll-up, determinism of a roll-up, old-save migration,
    "obligations never dropped" negative test, ceiling breach detection.
12. **Docs**: `docs/saves/RETENTION.md` with the inventory table and the policy; cite it from the
    save-store contract matrix.
13. **Run the checklist** + the 400-year soak + `--save-load-ui-failure-selftest`.

**DoD:** nothing grows without a stated limit, and the 400-year save is a known size.

---

## Task 55B — Turn real saves into a corpus: fixtures, fuzzing, and the archaeology suite

**Goal:** commit the accumulated campaign saves as versioned test fixtures, then use them (plus
synthetic long runs) to make compatibility a tested property rather than a hope.

**Files:** new `Ashfall.Core.Tests/Fixtures/Saves/` (+ `MANIFEST.json`), the
`~/.local/share/godot/app_userdata/ASHFALL…/holdfast_archive_*` contents (exported, sanitised),
`ashfall-save-fuzz` skill, `Ashfall.Core.Tests/SaveCorpusTests.cs`,
`scripts/ci/export-save-corpus.sh` (new), `docs/saves/SAVE_CORPUS.md`,
`.gitattributes` (LFS for save blobs), 48A step 6's compatibility gate.

### Substeps

1. **Salvage the existing archives first** — dated `holdfast_archive_*` folders are pre-refactor
   formats; capture them *before* anyone cleans their machine, because the only test that proves the
   V1→V2→envelope migrations work against genuinely old data requires exactly this material.
2. **Sanitise, don't ship personal paths**: strip absolute paths/usernames, keep structure and
   checksums meaningful, and record provenance (build version if recoverable) per fixture in a
   manifest — 46B's privacy rules apply to committed saves too.
3. **Choose the fixture set deliberately**: oldest envelope, pre-envelope bare-state, one save per
   store-version boundary, a mid-campaign save from each expansion surface, a corrupted-but-recoverable
   one, and a maximal (400-year) one.
4. **Track them in LFS** (`.gitattributes` policy is already images/fonts; a save corpus is binary-ish
   and must not bloat the clone — decide before committing, not after).
5. **Load-every-fixture gate**: for each fixture, load → validate checksum → assert expected day/
   roster/section presence → re-save → assert digest stability (idempotence, 39C step 7).
6. **Cross-version matrix**: for every supported save-schema version (48A), assert load + migrate +
   continue-play for 30 more days without error.
7. **Fuzz on structure, not randomness**: mutate envelope fields (checksum, `State`,
   `manifestVersion`), truncate, duplicate sections, swap slot roots — and assert the documented
   clean-error behaviour (Wave 5's 39A steps 5–6), never a crash and never a silent wipe.
8. **Keep a "real play" regression class**: corpus-derived fixtures are the only inputs that contain
   the odd combinations nobody hand-writes (a store saved mid-migration, an empty roster, a save with
   60 memorial rows) — track coverage of fixture-vs-handwritten explicitly.
9. **Publish the size/time budget** for loading the largest fixture, so cold-start regression is
   visible in the same number players feel.
10. **Document each fixture**: what campaign it is, what it proves, when it was captured — otherwise
    it becomes the next unattributed `artifacts/balance/*.csv` (Wave 7's 46A lesson).
11. **Tests**: the corpus gate plus a self-proof that it fails on a deliberately truncated fixture.
12. **Register the corpus and cross-version gates** in `CI_GATE_MANIFEST.json` (fast: load-all;
    nightly: fuzz + 30-day continue).
13. **Run the checklist** + `--save-load-ui-failure-selftest` + `ashfall-save-fuzz`.

**DoD:** every save format the game has ever written is in the repo, loads, and is gated.

---

## Task 55C — Long-session robustness: leaks, focus, freed objects, and the resume story

**Goal:** the 200-hour session is a first-class scenario — node/handler stability, session-swap
safety, and a resume path that never leaves the player in a half-loaded shelter.

**Files:** `src/Main.Lifecycle.cs`, `src/Main.PanelLifecycle.cs`, `src/UI/*` (bind/unbind),
`PanelBindLifecycleSelfTest.cs`, `src/Host/SaveLoadHostSession.cs`,
`docs/ui/UI_NODE_DIAGNOSTICS_AND_LEAK_TRIAGE.md`, 16C's subscription bag, 39B harness,
`src/Host/SceneBindingSelfTest.cs`.

### Substeps

1. **Define the resume contract**: after a load, every open or re-openable surface shows campaign
   state (16B's `ReferenceEquals` checks), ambience reflects the loaded situation (52A), guidance
   resumes (17B), and no panel is left bound to a freed session — then test it.
2. **Cycle test at scale**: 50× open→close of every live panel (16A set) with node-count and handler
   assertions; the guide's telemetry (`UI_NODE_DIAGNOSTICS…`) becomes an instrument, not a doc.
3. **Freed-object safety**: assert no `Control` holds focus or a signal to a freed node after
   new-game/load (37B step 9 flagged this as a real Godot crash class).
4. **Session-swap rebind sweep**: one ordered pass (28C's lifecycle stages) so every subsystem and
   panel re-resolves; verify with the port contract's bound-port report (36B step 3).
5. **Idle and background behaviour**: window blur/minimise must not spin the day loop, stack audio
   layers, or double-save; `NotificationWMCloseRequest` path (48B/39A) covered.
6. **Memory ceiling**: assert managed + native (texture) growth across the 400-day soak stays under a
   stated budget, with a report of the top allocations so the fix is targeted (Wave 2's 21A step 10
   per-tick lists are the obvious first suspect).
7. **Save-corpus resume tests**: load each 55B fixture and advance 3 days without error —
   "resumability", not just parseability.
8. **Failure injection at load**: missing section, unknown id, oversize log, broken slot root —
   each must produce the documented recovery path and a clean error surface (48C step 7's triage kit).
9. **Instrument the soak**: reuse 55A step 10's growth assertions so durability gates share one
   harness rather than three.
10. **Playtest the long game**: extend Wave 8's 54B with one 3-hour session at day ~250 — the
    *experience* of a mature campaign is unmeasured by every check above (retention roll-ups can hide
    important history from the player, which is a design bug with a passing test).
11. **Docs**: `docs/qa/LONG_SESSION_CHECKLIST.md` replacing the manual rows that currently cite
    selftests, plus the resume contract page.
12. **Tests**: cycles, focus/ freed-node, rebind completeness, memory ceiling, resumability per
    fixture.
13. **Run the checklist** + nightly soak + release gate.

**DoD:** a 200-hour campaign is a tested scenario — stable, resumable, and still legible.

---

## Cross-Task Dependencies

```
41C/38 (generations, calendar) ──► makes long runs real   39B (soak harness) ──► 55A/55C assertions
48A (version policy) ◄──────────► 55B fixtures + cross-version matrix
36B (bound-port report) ────────► 55C step 4              16B/16C ─────────────► 55C steps 1–3
52A (ambience state) ───────────► 55C step 1              54B (playtests) ─────► 55C step 10, 55A step 5
        55A (policy) ──► 55B (corpus) ──► 55C (long-session robustness)
```

**Execution order:** 55A → 55B → 55C (55B's fixtures benefit from 55A's roll-ups to prove
migration). **Salvage step (55B step 1) is urgent and independent** — those 7 MB are on one disk and
have no backup in the repo.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. godot --headless --path . -- --save-load-ui-failure-selftest
7. save-corpus gate: load-all + cross-version + fuzz             # (55B)
8. 400-year soak: cost slope, save-size ceiling, memory ceiling  # (55A/55C)
9. 50× panel cycle: node + handler baselines restored
10. bash scripts/ci/lfs-health-check.sh                          # corpus tracked correctly
11. bash scripts/ci/verify-fast.sh
```

---

## Estimated Effort & Risk

| Task | Core | Host | Fixtures/Docs | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|---|
| 55A | 1 new + ~12 fields | 1 | 1 doc | 12–16 | Medium–High | **MEDIUM — save shape + schema bumps** |
| 55B | 0 | 1 export script | corpus + manifest | 8–12 | Medium | LOW (fixtures are additive) |
| 55C | 0 | 2–3 | 1 doc | 8–12 | Medium | LOW–MED (lifecycle edits can leak — 16C rules apply) |

**Guardrails:** no cap that silently deletes a promise, obligation, or ending flag; no roll-up that
contradicts what the player was told; no personal paths in committed saves; no un-LFS'd corpus; no
bulk rewrite of 15 collections in one commit; and no claim that a passing soak equals a good long
game — that's what the 55C step 10 session is for.
