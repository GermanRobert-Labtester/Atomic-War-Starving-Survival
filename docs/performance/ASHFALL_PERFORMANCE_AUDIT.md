# ASHFALL Performance Audit — 2026-09-27

**Mode:** audit only (`/performance-audit`). No production code was changed.
**Git SHA:** `241a179fb` (branch `integration/all-latest-2026-09-24`; worktree has
other lanes' uncommitted edits, which were read but not measured).
**Environment:** Linux x86_64 VM, 6 GB RAM, Godot 4.7.1 Mono, Compatibility
renderer on Mesa llvmpipe (software GL), Python 3.12.

## 1. Evidence levels

Each finding is tagged:

- **MEASURED** — timed or sized in this audit.
- **CODE-VERIFIED** — the mechanism is confirmed in source; cost is not timed.
- **INFERRED** — plausible from code structure; needs a measurement first.

No game runtime profile was captured in this audit. Existing records it builds on:
`docs/perf/FRAME_BASELINE.md`, `FRAME_AFTER.md`, `STARTUP_BASELINE.md`,
`BUDGETS.md`, `RUNTIME_SCALE_AFTER.md`, `LINQ_CLOSURE_SWEEP_2026-09-27.md`.
Items already fixed there (five idle UI callbacks, coordinator owner view,
location/wildlife/roster closures, health-trend scan) are not re-reported.

## 2. Ranked backlog

| ID | Pri | Area | Finding | Evidence | Fix size |
|---|---|---|---|---|---|
| PERF-01 | P1 | Save / long run | Per-day save re-hashes and re-parses the campaign many times | CODE-VERIFIED | Medium |
| PERF-02 | P1 | UI / fan-out | Every survivor need change runs a section save, a log line, and hidden panel rebuilds | CODE-VERIFIED | Small–medium |
| PERF-03 | P1 | UI lifecycle | Trade panel adds a new `StateChanged` handler on every open | CODE-VERIFIED | One line |
| PERF-04 | P1 | Rendering | Hidden Shelter `SubViewport` renders every frame | CODE-VERIFIED | One line |
| PERF-05 | P1 | Measurement | Runtime-scale gate measures 5 synthetic owners and an empty save | CODE-VERIFIED | Medium |
| PERF-06 | P1 | Dev tooling | Docs-index hook reads 29 GB of Markdown twice per commit | MEASURED | Small–medium |
| PERF-07 | P2 | UI lifecycle | About 60 panels stay subscribed after close and rebuild while hidden | CODE-VERIFIED | Medium (shared guard) |
| PERF-08 | P2 | Per frame | `AudioManager` rebinds every frame; scarcity context rescans roster twice per frame | CODE-VERIFIED | Small |
| PERF-09 | P2 | Physics | Up to 12 shelter actors run `MoveAndSlide` at 60 Hz while the panel is closed | CODE-VERIFIED | Small |
| PERF-10 | P2 | Save size | Unbounded saved histories (consequence ledger, dose readings, voice, weather, relations) | CODE-VERIFIED | Per system |
| PERF-11 | P2 | Package | Data JSON packed twice; unreferenced 512 px placeholder pack ships | MEASURED | Export filter |
| PERF-12 | P2 | Startup | ~10–11.6 s to first frame is unattributed; the profiler covers ~4% of reads | MEASURED + INFERRED | Instrumentation |
| PERF-13 | P3 | Package / VRAM | Item/icon art stored at 512 px, shown at 40–64 px; JPEG sources stored lossless | MEASURED | Import settings |
| PERF-14 | P3 | Core loops | Morale contagion, relationship lookup, chronicle duplicate check are linear per call | CODE-VERIFIED | Small each |
| PERF-15 | P3 | UI rebuild | Data grid allocates a `StyleBoxFlat` per row; journal rebuilds all tabs per entry | CODE-VERIFIED | Small each |
| PERF-16 | P3 | Idle | `HoldfastTerminalPanel._Process` always scheduled; ~110 hidden `_UnhandledInput` handlers | CODE-VERIFIED | Small |

## 3. Findings

### PERF-01 — Per-day save redundancy (P1, save / long run)

**Hot path:** day advance → `CampaignDayPersistenceAdapter.PersistBeforeBriefing`
→ `SaveAll` (`src/Main.SaveOrchestrator.cs:85-89, 519-758`), once per day, ~250
section captures regardless of dirtiness.

- `SaveSlotService.cs:484-485` recomputes the aggregate checksum and then
  `ValidateAggregate` recomputes every section checksum, although
  `CampaignEnvelopeBuilder.Build` (`Save/CampaignEnvelopeBuilder.cs:105-119`)
  already computed them.
- Backup rotation (`SaveSlotService.cs:527-548`) re-reads and re-validates the
  primary and each backup; `SaveEnvelopeHelper.TryWriteAtomic:118-121` reads each
  written file back from disk.
- `SaveLoadHostSession.cs:603-612` loads and fully validates the previous save
  only to clone its manifest.
- About 30 Core systems deep-clone state by JSON round trip inside capture
  (e.g. `SurvivorRelationsSystem.cs:489-491`, `WaterTreatmentSystem.cs:705-715`,
  `ShelterThermalSystem.cs:1063-1065`), so each section becomes text 3–4 times.
- `SaveChecksum.cs:60-66, 145-159` builds the full canonical string plus one
  `StringBuilder` per collection before hashing.

**Scaling:** linear in total save size, multiplied by the redundancy constant,
every day. Save size grows with PERF-10.
**Candidate:** compute checksums once and validate once per save; stream the
canonical form into `IncrementalHash`; drop redundant clones only where no
concurrent mutation can occur in the capture window.
**Risks:** on-disk bytes and checksum values must stay identical; keep one full
validation per save. Gate with the save round-trip and checksum tests.
**Measure first:** PERF-05 is the prerequisite; the current gate cannot see this.

### PERF-02 — Survivor need-change fan-out (P1, UI / fan-out)

`SurvivorsHostSession.cs:131` raises `StateChanged` on every
`Needs.OnNeedChanged` (`NeedsSystem.cs:437`, per survivor per need kind). The
handler in `src/Main.Survivors.cs:118-125` runs, per event:

- `SaveSurvivors()` — full roster capture and serialization plus a
  `GD.Print` (`Main.Survivors.cs:302-306`), bypassing the existing
  `Flush*IfDirty` pattern in `Main._Process`;
- `RefreshView()` on the survivors overlay, medical panel, and shelter panel,
  none of which checks visibility;
- `UpdateHud()` — a full dashboard snapshot (`Main.GameFlow.cs:791-898`).

`ShelterPanel.RefreshView` (`ShelterPanel.cs:120-146`) re-initializes the 2D
interior, freeing and recreating up to 12 `CharacterBody2D` actors and all
hotspots. It runs twice per event because the panel also subscribes itself
(`ShelterPanel.cs:105`). `SurvivorsPanel.RefreshView` captures the roster save
twice per refresh (`SurvivorsPanel.cs:63-77, 124-126`).
`_inventory.StateChanged` (`src/Main.Inventory.cs:79-90`) follows the same shape
with five panel refreshes per item change.

**Frequency:** INFERRED ~40–100 events per day advance with an 8–12 roster
(`TickHour(24f)`, `Main.CampaignOwners.cs:1651`), plus every player action on
needs or items.
**Candidate:** mark survivors dirty and flush in `_Process` like other
sections; skip refresh while `!IsVisibleInTree()` and refresh on `Open()`
(which already calls `RefreshView`); remove the duplicate Main-side refresh
calls; coalesce `UpdateHud` to once per frame.
**Risks:** save timing moves from immediate to next frame; confirm crash-safety
expectations with the save owner. UI state is unchanged at open.

### PERF-03 — Trade panel subscription leak (P1, correctness-adjacent)

`TradeScreenGodotPanel.BindSession` (`src/Economy/TradeScreenGodotPanel.cs:501-504`)
does `_session.StateChanged += RefreshView` with no prior `-=`.
`OpenTradeScreen` (`src/Main.Economy.cs:368-377`) calls it on every open with
the same long-lived session, and `CloseTradePanel` (`Main.Economy.cs:304-309`)
removes only the `_silentFoundry` handler. After N opens, every economy change
runs N full trade-screen rebuilds, including while hidden.
**Candidate:** unsubscribe before subscribing, mirroring `BindViewModel` in the
same file. No behavior change besides removing duplicates.

### PERF-04 — Hidden Shelter SubViewport renders every frame (P1, rendering)

`src/UI/ShelterPanel.cs:309-313` creates a 760×420 `SubViewport` with
`UpdateMode.Always`. The panel is built at boot (`Main.UiPanels.cs:500-503`) and
hidden. `Always` renders the target every frame even when nothing displays it.
It is the only such viewport outside selftests.
**Candidate:** `UpdateMode.WhenVisible`. Expected to reduce idle and in-play
render cost on llvmpipe; confirm with a before/after `FRAME_*` capture.

### PERF-05 — The runtime-scale gate cannot see real cost (P1, measurement)

`PerformanceCampaignHarness.cs:199-268` registers five synthetic owners
(`perf_weather`, `perf_survivors`, …) versus ~50 in `src/Main.CampaignOwners.cs:24-109`.
`CaptureSavePayload` (`:300-331`) sets every section payload to `string.Empty`,
and `AdvanceDays` passes no persistence adapter (`:239`). The `BUDGETS.md` figures
(0.7 ms for 30 days, 46 ms save) therefore exclude real owners and the real
per-day save path.
**Candidate:** add a headless measurement that drives the real campaign-day
host session plus `SaveAll` for 30/180/360 days, reporting per-day time, save
bytes, and allocations. Measurement only, no gameplay change. This is Phase 0 for
PERF-01, PERF-02, and PERF-10.

### PERF-06 — Docs-index pre-commit hook (P1, developer tooling)

**MEASURED.** `.git/hooks/pre-commit:55-58` runs
`scripts/ci/generate-docs-index.py --check`; on drift it runs the generator
again. Each pass `rglob`s and fully decodes every Markdown file
(`generate-docs-index.py:36, 127`). On this machine:

| Measure | Value |
|---|---|
| Markdown indexed | 5,537 files, 29,039,477,534 bytes, all tracked in git |
| Enumeration (`rglob`, 5,806 files) | 7.1 s |
| Largest 20 files (229 MB): read / `upper()` / `splitlines()` | 7.2 s / 2.8 s / 1.8 s |
| Observed hook time for commit `241a179fb` | ~25 min check + ~25 min regenerate |

RAM is 6 GB, so the corpus cannot stay cached and every pass is disk-bound.
`classify_doc` computes `content.upper()` for the whole file and never uses it,
and title and summary extraction call `splitlines()` on the whole file twice.
`docs/plans/EXPANSION_PROGRAM_2026-09-21/EVIDENCE.md` is 11.4 MB, but only
1.8 MB of its lines are unique.

**Candidates, least to most behavior change:**

1. Drop the unused `upper()` and read title and summary from the first few KB
   (about −35% CPU per pass, identical output).
2. Have the hook regenerate once and compare, instead of check then regenerate
   (halves wall time, identical output).
3. Cache per-file `(size, mtime) → chars/title/summary` so unchanged files are
   not reread (identical output; requires a cache file).
4. Report byte size instead of the exact code-point count. This changes the
   index format that commit `d3b3d484e` added on purpose and needs a decision.

The corpus size itself (3,591 files ≥100 KB, heavy line repetition) is a
repository-hygiene matter for the foreman, not a tooling fix.
The Go-only tool policy (`AGENTS.md`) may apply if this becomes a rewritten
indexer; the edits in candidates 1–2 are small changes to an existing script.

### PERF-07 — Hidden panels stay subscribed (P2)

About 60 panels subscribe `StateChanged += RefreshView` in `Bind`, and
`PanelRegistry` close actions only hide (`src/Main.PlayerSurfaces.cs:195-500`).
No `RefreshView` checks visibility. Examples with multi-source fan-out:
`FactionsPanel.cs:84-93` (5 sources), `MapPanel.cs:64-74` (4), `QuestsPanel.cs:81-89`
(5), `DutyRosterPanel.cs:72` (raw `OnNeedChanged`), `RadioPanel` refreshed twice
per change (`Main.Narrative.cs:594` and `RadioPanel.cs:59-63`),
`VerdictPanel.cs:129-133`, `ChroniclePanel.cs:66-77`, `JournalPanel.cs:54-57`.
Rebuilds use `AshfallUiHelpers.EmptyChildren` (`AshfallUiHelpers.cs:946-957`),
which frees every child synchronously.
**Candidate:** one shared stale-while-hidden guard applied per panel, with the
existing `Open()` refresh as the catch-up path. Check each panel's `Open()` first.

### PERF-08 — AudioManager per-frame rebinding (P2)

`AudioManager._Process` (`src/Audio/AudioManager.cs:224-230`) calls
`RefreshDomainBindings()` every frame: about 18 provider reads and a dozen
guarded bind calls (`:319-363`). `ScarcityAudioController.BindPowerGrid` and
`BindRoster` (`ScarcityAudioController.cs:59-69`) have no reference-equality
guard, so `RefreshContext()` runs twice per frame. That call scans the roster,
reads each dosimeter, and pushes context into the scarcity state machine.
**Candidate:** add the same `ReferenceEquals` guards the sibling methods use
(smallest fix), then rebind from session setup and teardown instead of polling.
**Risk:** if scarcity context relies on this per-frame refresh to track
changing roster values, it needs an event or low-rate tick instead. Check before removing.

### PERF-09 — Shelter actors tick while hidden (P2)

`SurvivorActorView._PhysicsProcess` (`src/World/SurvivorActorView.cs:153-172`)
runs `MoveAndSlide` at 60 Hz. Actors are created on first shelter bind
(`HoldfastInteriorView.cs:606-623`) and remain after close.
**Candidate:** disable physics processing while the interior is not visible
in the tree.

### PERF-10 — Unbounded saved histories (P2)

Appended for the whole campaign, saved, and captured every day:
`Flags/CampaignConsequenceLedger.cs:89` (every flag/counter change; cleared only
on restore/reset at `:259, :296`), `DoseLedgerSystem.cs:159`,
`Weather/WeatherGameplayCascadeEngine.cs:306`, `Voice/SurvivorVoiceSystem.cs:248`,
`Reputation/ShelterReputationSystem.cs:233`, `Survivors/RelationshipDecaySystem.cs:316`,
`SurvivorRelationsSystem.cs:209` (one formatted string per affinity change),
`PlayerCommand/CampaignActionLog.cs:17-22`, and several slower growers.
**Candidate:** per-system retention caps following `JournalSystem.MaxEntries`.
**Risk:** some histories feed gameplay (e.g. `EvidenceHistory` feeds
`EvaluateTags`), so each needs an owner review. Checksums change on capped saves,
which is expected.

### PERF-11 — Package waste (P2)

**MEASURED** from the Linux PCK index (`builds/linux/ashfall.pck`, 222,148,800 bytes):

- 712 data JSON files appear under both `res://Assets/StreamingAssets/Data/` and
  `res://assets/StreamingAssets/Data/`: 11.9 MB duplicated. This matches the
  "1,423 catalogs" parity count in `STARTUP_BASELINE.md` against 711 JSON files
  on disk. The staging mechanism (case-insensitive include filter at
  `export_presets.cfg:9`) needs one verbose export to confirm.
- `assets/art/placeholders-512/` — 1,079 generated PNGs plus manifest,
  26.7 MB, no references in code, data, or scenes; shipped by
  `export_filter="all_resources"`.
- `addons/godot_mcp` ships in release (0.6 MB), and its editor-only scripts log
  parse errors at export boot.
- 76 sound effects ship as MP3 (4.0 MB) instead of compressed WAV samples.

**Candidate:** exclude filters for the placeholder pack and the MCP addon, and
one embedded data copy. About −41 MB (−18%) with no visible content change.
Keep `all_resources`, because `AssetRegistry` builds art paths dynamically.

### PERF-12 — Startup is unattributed (P2)

**MEASURED** (existing artifacts): 9.9–11.6 s from engine start to the first
`Main._Process`; process start adds only ~0.05–0.2 s.
`CatalogReadProfiler` (`Assets/Ashfall.Core/Performance/CatalogReadProfiler.cs:15-28`)
counts only its 143 explicit call sites, while most boot IO goes through
`FileSystemIO.ReadAllText` (`HostDefaults.cs:23`). The "16 reads / 2 ms" figure
is a coverage artifact.
**INFERRED contributors:** .NET runtime and 15–17 MB assembly load and JIT;
GL context on llvmpipe; eager `BuildUserInterface()` (~106 panels,
`src/Main.UiPanels.cs:242-1707`); 13 synchronous `Setup*` sessions
(`Main.Application.cs:1101-1124`). There is also a small double parse:
`CatalogBootValidator` parses 32 catalogs and discards the results
(`CatalogBootValidator.cs:135-166`), and Setup loaders reparse them.
**Candidate (Phase 0):** timestamp the MCP autoload `_ready` against
`Main._Ready` entry and exit, and wrap `BuildUserInterface` and each `Setup*`
in the existing `FrameStartupProfiler.MeasureProcess` scopes. Only then decide on
lazy panel construction.

### PERF-13 — Texture sizes and import modes (P3)

Item art (327 JPG + 94 PNG) and 145 UI icons are 512×512 but displayed at
40–64 px (`TradeScreenGodotPanel.cs:153`, `InventoryPanel.cs:100`);
4,282 of 4,452 textures have `process/size_limit=0`. About 413 JPEG-sourced
textures use lossless compression. Estimated −15–25 MB PCK and large VRAM
savings. Run a full consumer sweep before any bulk import change.

### PERF-14 — Linear Core lookups (P3)

- `MoraleContagionSystem.cs:309-327, 348-372` — sources × survivors with
  per-call array allocation and linear room/role lookups.
- `SurvivorRelationsSystem.cs:190-198` — `Find` with two string allocations
  per candidate pair per affinity change.
- `CulturalArchiveVaultSystem.cs:392-393` — `Any` over an unbounded chronicle
  per milestone.
- `CampaignDayCoordinator.cs:242-247` — one `Stopwatch` allocation per owner
  per day (`Stopwatch.GetTimestamp()` avoids it).

All are daily or event-rate, so they matter only at large rosters or long
campaigns. Measure with PERF-05 first; order-preserving fixes are determinism-safe.

### PERF-15 — UI rebuild allocation (P3)

`AshfallDataGrid.cs:132-167, 278-299` frees all rows and allocates a new
`StyleBoxFlat` per row per rebuild. `JournalPanel.cs:63-132, 176-186` rebuilds
64 entries, four sorted knowledge scans, and a static six-card manual on every
entry. Cache one style box per cell state; build the manual once.

### PERF-16 — Small idle costs (P3)

`HoldfastTerminalPanel._Process` (`src/Host/HoldfastTerminalPanel.cs:448-458`)
is scheduled from boot for a 3-second confirm timer. About 110 hidden panels
receive every unhandled input event and return on `!Visible`.
Separately (correctness, not performance): `HoldfastTerminalPanel.cs:461-467`
handles Esc before its visibility guard, so it can close and consume Esc while hidden.

## 4. Not worth optimizing (P4)

- Shaders, particles, 2D lights: none exist in scenes or `src/`.
- Duplicate art: 71 groups, ~0.4 MB total.
- `Main._Process`: per-frame work is a guarded vigil tick; flush checks run at 4 Hz.
- Hourly Core methods: all compressed into the day advance; no real-time hourly loop.
- The 42 draw calls on the idle menu are ordinary Control composition.
- Timers and tweens: none repeat forever.

## 5. Housekeeping observations (need approval; not performance changes)

- `builds/linux/ashfall.pck-*` — three orphaned export temp files, ~671 MB.
- `builds/windows/ashfall.pck` (129.7 MB, 2026-09-25) predates current content.

## 6. Recommended order

1. **Phase 0, measurement:** PERF-05 real-owner scale run and PERF-12 startup
   scopes. These are pure instrumentation and make every later change measurable.
2. **One-line fixes with clear before/after metrics:** PERF-03, PERF-04, and
   the PERF-08 guards.
3. **Fan-out:** PERF-02, then PERF-07 as a shared guard, measured by refresh
   counts per day advance.
4. **Save path:** PERF-01 under the save round-trip and checksum gates, then
   PERF-10 per owner review.
5. **Package:** PERF-11 export filters, re-export, parity gate; PERF-13 later.
6. **Tooling:** PERF-06 candidates 1–2 any time; candidate 4 needs a decision.

## 7. Behavior preservation contract for the follow-up work

Gameplay results, RNG consumption order, save bytes and checksums (except
deliberate history caps), event order where contractual, IDs, and data
authority stay identical. Panels must show current state at open. Each change
lands alone with a before/after measurement; a change that shows no improvement
is reverted.
