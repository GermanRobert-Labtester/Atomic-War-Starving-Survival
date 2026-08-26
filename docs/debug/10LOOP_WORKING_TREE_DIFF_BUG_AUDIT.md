# ASHFALL 10-Loop Bug Audit

## 1. Audit Target

The uncommitted working-tree diff at commit `de1c0a1feaba9904fe03e1bba35bcb4408bdfb29` —
95 modified C# files (13 in `Assets/Ashfall.Core/`, ~82 in `src/`), +866/−332 lines, plus
a WAV→OGG audio migration and one Core file deletion
(`Assets/Ashfall.Core/Radio/CensusBroadcastScheduler.cs`).

The diff is a mixed maintenance sweep of five themes:

- **Perf micro-caching**: cached `Enum.GetValues`/`Enum.GetNames` arrays
  (TacticalCombatSystem, TradeScreenPresenter, ItemDefinitions, WarlordDoctrineSystem,
  MusterHostSession, PowerGridPanel), cached reflection fields in `SaveChecksum`,
  font/texture caches in `AshfallUiHelpers`, resolution cache in `AssetRegistry`,
  static JSON caches in `FactionMatrixPanel`/`FactionsNarrativePanel`/`WastelandMapView`.
- **Tick-buffer reuse**: per-instance `List<string> _tickKeyBuffer` replacing per-call
  allocations in `ExpeditionSystem.TickHours`, `CaregivingSystem.Tick`,
  `SomaticFlashbackSystem.TickAll`.
- **Event hygiene**: `_ExitTree` unsubscribe overrides added to ~45 panels;
  unsubscribe-first `Bind` added to ~6 panels; lambda handlers replaced with named
  handlers in `DeepCoastPanel`, `DutyRosterPanel`, `QuestsPanel`, `QuestsAtlasPanel`.
- **`AshfallUiHelpers.EmptyChildren`**: new helper (RemoveChild + synchronous `Free()`)
  replacing 209 `QueueFree()` clear-loops, applied via regex script.
- **Save/IO**: `WorldSaveStore` gains `customPath` parameters **and a checksum-mismatch
  softening**; `DoseContentCatalog`/Muster catalogs replace bare `catch {}` with
  `CatalogDiagnostics.Warn`; `WarlordDoctrineCatalog.IndexIds` gains a wrapped-shape
  fallback probe.

## 2. Scope

Full diff read end-to-end (2,758 diff lines). Focused source reads of every
behaviorally-changed method. Cross-reference against all 17 sibling save stores, the
`SaveStoreChecksumSweepTests` suite, the audio asset tree, `.gitattributes`, and the
`AGENTS.md` checksum contract. Live headless runs of five selftest verbs.

Not in scope: the ~2,900 untracked files (skills, art LFS pointers, docs) except where
they intersect the diff (new `.ogg` binaries); the pre-existing codebase except as
context for regression analysis.

## 3. Baseline Verification

| Gate | Result |
|---|---|
| `dotnet build Ashfall.Core.Tests` | PASS (0 warnings, 0 errors) |
| `dotnet test Ashfall.Core.Tests` | PASS (2497/2497) |
| `dotnet build Ashfall.csproj` | PASS (0 warnings, 0 errors) |
| `godot --headless -- --data-integrity-selftest` | PASS (0 errors, 102 catalogs, 3600 ids) |
| `godot --headless -- --bridge-selftest` | PASS (exit 0) |
| `godot --headless -- --audio-selftest` | PASS (141/141; 49 cues, 0 fallback, 0 silent) |
| `godot --headless -- --ui-layout-selftest` | PASS (0 failures, all panels 1024x768→4K) |
| `godot --headless -- --player-panels-uitest` | PASS, but 33 ObjectDB instances + 6 CanvasItem RIDs leaked at exit |

## 4. Loop Completion Matrix

| Loop | Lens | Candidates examined | Confirmed | Rejected |
|---|---|---|---|---|
| 1 | Static sweep | 14 | 4 | 3 |
| 2 | Reachability | 6 | 1 | 2 |
| 3 | State transitions | 5 | 0 | 2 |
| 4 | Save/load | 7 | 1 | 3 |
| 5 | Determinism | 8 | 0 | 6 |
| 6 | Data/ID | 4 | 1 | 1 |
| 7 | Event/lifecycle | 11 | 2 | 4 |
| 8 | UI/player-facing | 6 | 0 | 3 |
| 9 | Test adversarial | 4 | 1 | 1 |
| 10 | Cross-system synthesis | all above | 2 clusters | — |

## 5. Executive Findings

The sweep is **mostly correct and net-positive**: it fixes real event-leak bugs across
~45 panels, closes a genuine warlord-validator blind spot, replaces swallowed catalog
exceptions with diagnostics, and the caching changes are semantically transparent
(tests + selftests green).

**One real regression was introduced**: `WorldSaveStore.TryLoadEnvelope` now loads
checksummed saves whose checksum *fails verification*, logging only a warning. This
breaks the documented integrity contract and makes WorldSaveStore the sole soft-loader
among 17 stores. The existing sweep tests cannot catch it because they test a private
DTO copy, and the new panel-test round-trip asserts presence, not rejection.

Two systemic fragilities were introduced or left behind: shared tick-key buffers that
are unsafe under reentrant event handlers, and a partial fix for double-subscribe-on-
rebind that covers ~6 panels while ~15 others keep the latent bug.

## 6. Critical Findings

None. (The WorldSaveStore regression is ranked HIGH, not CRITICAL: it corrupts silently
only when a save is already corrupt/tampered, and the rest of the envelope path —
missing-checksum rejection, legacy fallback — is intact.)

## 7. High Findings

### BUG-01 — WorldSaveStore loads saves that fail checksum verification

**Severity:** HIGH
**Confidence:** CONFIRMED
**Category:** SAVE BUG / SECURITY-ROBUSTNESS
**Active Runtime:** YES
**Player Impact:** A corrupted or tampered `world_save.json` is silently applied to
live weather, sky-armor, location-evolution, wildlife, and landmark state. Previously
the corrupt save was rejected and the session started clean; now the player plays on
invalid world state with only a console warning.
**Trigger:** Any `world_save.json` whose stored `Checksum` does not match
`SaveChecksum.Compute(envelope)` — bit rot, manual edit, or a foreign save.
**Expected:** Reject and return null, matching every sibling store
(`CaravanSaveStore:70`, `CombatSaveStore:72`, `ExpeditionSaveStore:73`,
`InventorySaveStore:67`, `MedicalSaveStore:72`, `NarrativeSaveStore:72`,
`MusterSaveStore:84`, `Phase0SaveStore:70`, `SurvivorsSaveStore:66`, and 7 more — all
`return null` on mismatch) and the AGENTS.md contract ("mutated-state changes hash"
must reject).
**Actual:** `src/Host/WorldSaveStore.cs:84-88` logs
`"[World] load warning: checksum mismatch (corrupt or version migration). Allowing fallback load."`
and falls through to `return envelope;`.
**Root Cause:** The diff replaced the hard reject with a soft warn, conflating two
different cases: (a) schema migration, which is handled by versioned codecs, not by
ignoring hashes; and (b) corruption, which must reject. There is no
`WorldHostSave`-codec migration path that would justify tolerating a bad hash.
**Evidence:** Diff hunk `src/Host/WorldSaveStore.cs` lines 84-88; comparison against 16
sibling stores (all reject); `SaveStoreChecksumSweepTests.cs:162-204` pins the contract
for a *private copy* of `WorldHostSave`, so the real store's behavior is unpinned.
**Affected Systems:** World weather, sky armor, location evolution, wildlife, landmarks;
`WorldHostSession.Create` (`src/Host/WorldHostSession.cs:62`) is the production caller.
**Save Impact:** Direct — integrity boundary removed for the world envelope.
**Determinism Impact:** Indirect — restored-but-invalid weather state changes seeded
simulation inputs.
**Regression Risk:** The fix (restore `return null`) is one line and zero-risk; the
regression risk is in *not* fixing it.
**Suggested Next Analysis:** Add a store-level (not DTO-level) test that mutates a
field in a written `world_save.json` and asserts `TryLoadEnvelope` returns null. Extend
`SaveStoreChecksumSweepTests` to exercise the real stores via temp paths — the new
`customPath` parameter makes this trivially testable now.

## 8. Medium Findings

### BUG-02 — Shared `_tickKeyBuffer` is unsafe under reentrant event handlers

**Severity:** MEDIUM
**Confidence:** HIGH-CONFIDENCE
**Category:** CONCURRENCY/LIFECYCLE BUG
**Active Runtime:** YES (latent; no current handler re-enters)
**Player Impact:** None today; a future host that chains a tick from inside
`OnExpeditionCompleted`/`OnStateChanged`/`OnCaregivingEnded` will corrupt the outer
iteration — skipped or double-processed units, or `ArgumentOutOfRangeException`.
**Trigger:** Any event handler invoked from inside the tick loop calling
`TickHours`/`Tick`/`TickAll` on the same instance.
**Expected:** Tick iteration state is per-call.
**Actual:** Per-call key snapshots became per-instance fields:
`ExpeditionSystem.cs:180` (`TickHours` raises `OnExpeditionCompleted` and
`OnStateChanged` mid-loop at lines 220-226 while reading `_tickKeyBuffer[i]`);
`CaregivingSystem.cs:171` (`Tick` raises `OnCaregivingEnded` mid-loop);
`SomaticFlashbackSystem.cs:217` (`TickAll` → `Tick` raises `OnFlashbackEnded` and
`OnStateChanged`).
**Root Cause:** Allocation-avoidance refactor traded a real invariant (loop snapshot
isolation) for a micro-optimization. The buffer is cleared at method entry; a nested
call wipes the outer loop's array while the outer loop's index still advances.
**Evidence:** Source lines above; all three methods raise C# events inside the
buffered loop.
**Affected Systems:** Expeditions, caregiving, somatic flashbacks.
**Save Impact:** None.
**Determinism Impact:** If reentrancy ever occurs, tick processing order/content
diverges — a latent determinism hazard.
**Regression Risk:** Fix options are local-list (revert) or a reentrancy guard; both
cheap.
**Suggested Next Analysis:** Grep for any handler registration that could call back
into these methods; if none exists, add a one-line `_ticking` guard or document the
non-reentrancy precondition on the public methods.

### BUG-03 — Double-subscribe-on-rebind fixed for only ~6 of ~21 panels

**Severity:** MEDIUM
**Confidence:** CONFIRMED
**Category:** EVENT BUG
**Active Runtime:** YES (latent; depends on whether hosts re-Bind)
**Player Impact:** After a re-Bind with a different session, the panel refreshes twice
per state change and stays subscribed to the dead session — stale ghost updates and a
reference leak from the old session to the old panel.
**Trigger:** `Bind(...)` called twice on the same panel instance (session swap, test
harness rebinding, scene reuse).
**Expected:** Unsubscribe from the previous session before subscribing to the new one.
**Actual:** Fixed in `GreenhousePanel:46-50`, `CaravanBarterLedgerPanel:64-69`,
`MapAtlasPanel:51-55`, `PowerGridPanel:33-37`, `QuestsAtlasPanel:48-53`,
`QuestsPanel:40-53`, `ShelterPanel:54-67`. **Not** fixed in `DeepCoastPanel:47-57`
(subscribes `_deepCoast` and `_core.IceRoad` unconditionally), `MapPanel:50-63`,
`AirlockSecurityPanel:28-35`, `MaritimePanel`, `FactionsPanel:58-68`,
`ExpeditionPanel:66-69`, `RadioPanel`, and others. Worse, the new `_ExitTree`
unsubscribes only the *current* session reference, so a leaked subscription to a
*previous* session is now permanent (previously it was permanent too, but the sweep
creates the impression the class of bug is closed).
**Root Cause:** The hygiene pass was applied by pattern-matching rather than by
auditing every `Bind`; no checklist bound the fix to all subscribers.
**Evidence:** Side-by-side `Bind` bodies cited above.
**Affected Systems:** UI panels ↔ host sessions broadly.
**Save Impact:** None.
**Determinism Impact:** None.
**Regression Risk:** Completing the sweep is mechanical and safe.
**Suggested Next Analysis:** Decide whether hosts ever re-Bind. If yes, apply the
unsubscribe-first pattern to the remaining panels or centralize subscription management.

## 9. Low Findings

### BUG-04 — New `.ogg` audio binaries untracked in git

**Severity:** LOW (MEDIUM if committed without them)
**Confidence:** CONFIRMED
**Category:** DATA/MIGRATION BUG (repo state)
**Active Runtime:** NO (files exist locally; the hazard is fresh clones)
**Player Impact:** None on this machine. A fresh clone of a commit that includes the
catalog changes but not the binaries would fail `AudioSelfTest` (18 assets checked) and
ship silent ambience/music.
**Trigger:** `git add` of the C# catalog changes without `assets/audio/**/*.ogg`.
**Expected:** The WAV→OGG migration commits atomically: deleted `.wav`, added `.ogg`,
updated `AudioCueCatalog`/`AudioSelfTest`.
**Actual:** `.wav` files are staged-deleted; `bunker_ambience.ogg`,
`surface_ambience.ogg`, `main_menu.ogg`, `gameplay_underscore.ogg` are untracked (`??`
in git status). `.ogg` is deliberately plain binary per `.gitattributes:186-188` —
correct — but must still be `git add`ed. No `.import` files exist yet (first import
generates them; they should be committed for reproducible import settings per
AGENTS.md asset rules).
**Evidence:** `git status --short` (` D` wavs, `??` oggs); `ls assets/audio/`.
**Suggested Next Analysis:** Commit the four `.ogg` files together with the catalog
diff; run one Godot import and commit the generated `.ogg.import` files.

### BUG-05 — Regex-sweep artifacts: merged statements and stray migration scripts

**Severity:** LOW
**Confidence:** CONFIRMED
**Category:** CODE QUALITY / reviewability defect (not a runtime bug)
**Active Runtime:** N/A
**Player Impact:** None.
**Actual:** The `EmptyChildren` regex pass merged following statements onto the same
line in 8 places, e.g. `EconomyMarketPanel.cs:98`
(`EmptyChildren(_goodsList);_lblSummary.Text =`),
`DoorEncounterModal.cs:134` and `:154`, `ApproachSelectionModal.cs:58`,
`CurrentsRosterWidget.cs:66`, `JournalWitnessPanel.cs:64`,
`FactionRadioHudPanel.cs:359`, `UtilityAiPanel.cs:78`. Compiles clean, harms review
and future diffs. The migration scripts themselves (`fix_queuefree.py`,
`fix_queuefree.sh`, `fix_syntax.py`, `fix_using.py`, `safe_fix.py`, `test_parse.py`,
`files_to_check.txt`, `summaries.md`, `summaries/`) are untracked at repo root — they
must not be committed; add to `.gitignore` or delete.
**Evidence:** Line citations above; `git status` untracked list.
**Suggested Next Analysis:** Re-split the merged lines before commit; remove or
gitignore the scripts.

## 10. Suspected / Needs Reproduction

### SUSP-01 — `EmptyChildren(this)` synchronous free of Main's entire subtree in the UI-test quit path

`src/Main.UiHandlers.cs:342` replaced `foreach … child.QueueFree()` with
`AshfallUiHelpers.EmptyChildren(this)`, which `Free()`s every child of the root
synchronously between two `ProcessFrame` awaits. If any child's `_ExitTree` emits a
signal touching a sibling freed microseconds earlier, teardown crashes with a freed-
instance access. The old deferred path was immune. `--player-panels-uitest` exits 0
with this path but still reports **33 ObjectDB + 6 CanvasItem RID leaks at exit**; a
pre-change baseline run was attempted but blocked by a pre-existing broken symlink
(`.codex/skills/ashfall-wire/SKILL.md` — "beyond a symbolic link" — which also breaks
`git stash`). Whether the 33 leaks are an improvement, a regression, or pre-existing
cannot be established without the baseline. Confidence: SUSPECTED. Next step: repair or
remove the broken symlink so stash/baseline comparisons work, then re-run both ways.

### SUSP-02 — Unsorted key snapshots in `CaregivingSystem.Tick` / `SomaticFlashbackSystem.TickAll`

Unlike `ExpeditionSystem.TickHours` (which sorts `string.CompareOrdinal`), the two
survivor-system buffers iterate dictionary insertion order. Insertion order changes
after save/load restore (restore re-inserts in save-file order). Neither method consumes
RNG per item, so no divergence is provable today; but any future per-item RNG or
order-dependent event side effect becomes a silent determinism drift. SUSPECTED/LOW.

### SUSP-03 — Static mutable caches (`s_cachedMapData`, `s_cachedFactions` ×2)

Read-only after first successful load, so safe in a single run. In the editor or in
snapshot harnesses that hot-reload data, a mutated JSON would not be picked up on
second load and there is no invalidation hook. No current caller reloads data mid-run.
SUSPECTED/LOW.

## 11. Rejected False Positives

- **`SaveChecksum` reflection field cache** (`SaveChecksum.cs:153-170`): verified
  field set, exclusion of `[NonSerialized]`, ordinal sort, and root `Checksum` skip are
  unchanged; 2497 tests incl. `SaveChecksumTests`/`SaveWireContractTests` green.
  REJECTED.
- **Enum-value caches** (`s_allStances`, `s_allShockKinds`, `s_canonicalNames`,
  `s_allStrategicActions`, `s_allRiskBiasTraits`, `s_priorities`): `Enum.GetValues`
  order is declaration order and stable; caching cannot change iteration order.
  `NormalizeActionName` cache is a pure function memoization. REJECTED.
- **`AudioManager` cooldown refactor**: decay-then-remove semantics identical; the new
  code also avoids a write-back for already-expired keys, which is behaviorally neutral.
  REJECTED.
- **`CensusBroadcastScheduler.cs` deletion**: superseded by
  `Verdict/VerdictCensusBroadcast.cs`, which adopts its canon (comment at line 21); no
  live references remain; tests use `IWorldCensus` from the Verdict namespace. REJECTED.
- **`GameHudOverlay.SetProcess(false)`**: `_healthAnimating`/`_radAnimating` are
  write-only dead flags; `_Process` was a no-op. REJECTED.
- **`UiBackgroundCarousel` mouse parallax**: old code divided window position by
  viewport size (nonsensical magnitude); new code is a correct mouse-position parallax.
  Not a bug — a fix. REJECTED.
- **`WarlordDoctrineCatalog.IndexIds` wrapper fallback**: closes a real gap (wrapped
  `{"locations":[…]}` files previously yielded zero indexed IDs → false-positive
  validation errors). The nested double-try is ugly but correct. REJECTED.

## 12. Root-Cause Clusters

**Cluster A — The hygiene sweep was pattern-driven, not contract-driven.** The
regex origin of `EmptyChildren` (confirmed by the leftover scripts) explains: merged
statement lines (BUG-05), the QueueFree→Free semantic change applied uniformly without
checking each site's deferred-free assumptions (SUSP-01), and the event-unsubscribe fix
landing on some `Bind` methods but not others (BUG-03). One root cause: a mechanical
sweep without a completion checklist.

**Cluster B — Test surfaces pin DTOs, not stores.** `SaveStoreChecksumSweepTests`
duplicates `WorldHostSave` as a private nested class; `HostCli.PanelTests` asserts
round-trip *presence*. Neither can fail when a real store softens rejection (BUG-01).
One root cause: no store-level adversarial test harness, despite `customPath` now
making it trivial.

## 13. Cross-System Failure Chains

**Chain 1 (BUG-01 end-to-end):** bit-flip in `user://world_save.json` →
`WorldSaveStore.TryLoadEnvelope` warns and proceeds → `WorldHostSession.Create`
restores invalid weather/sky-armor/wildlife/landmark state →
`WeatherSystem` (seeded) advances from corrupted state → dose/shelter/needs
simulation diverges → next save writes a *valid checksum over corrupt state*,
laundering the corruption permanently. This is why the softening is worse than it
looks: the first corrupt load is detectable; every subsequent save makes it canonical.

**Chain 2 (BUG-02 latent):** future host wires `OnExpeditionCompleted` → auto-launch
follow-up → nested `TickHours` → `_tickKeyBuffer` cleared mid-iteration → expeditions
silently skipped for one tick (no exception if counts happen to align) → state drift
between two runs with identical seeds.

## 14. Test Coverage Gaps

1. **No store-level checksum-mutation test for any of the 17 host save stores.**
   The sweep suite tests private DTO copies. With `customPath` now on WorldSaveStore,
   this is a pattern worth replicating to all stores.
2. **No reentrancy test** for tick methods that raise events mid-loop.
3. **No re-Bind test** for panel/session double-subscription.
4. The 33-instance ObjectDB leak at `--player-panels-uitest` exit is ungated — no
   assertion fails on leaks, so the very thing this diff set out to fix has no
   regression gate.

## 15. Migration/Legacy Risks

- Broken symlink `.codex/skills/ashfall-wire/SKILL.md` (and likely siblings under
  `.cursor/`, `.qwen/`) blocks `git stash` and possibly other git operations — repo
  hygiene issue outside the code diff.
- The WAV deletion is staged; the OGG addition is not (BUG-04) — a split commit would
  break audio on any other checkout.
- `CensusBroadcastScheduler` deletion is clean; `VerdictCensusBroadcast` is the live
  successor.

## 16. Save/Determinism Findings

- BUG-01 (checksum softening) — see §7.
- SUSP-02 (unsorted tick buffers) — no RNG consumed per item today; latent.
- `SaveChecksum` caching verified behavior-preserving against the wire contract tests.
- All enum caches verified order-preserving.
- 2497/2497 core tests pass; `--data-integrity-selftest` 0 errors.

## 17. Recommended Investigation Order

1. **Revert the checksum-mismatch softening in `WorldSaveStore.TryLoadEnvelope`**
   (restore `return null`) and add a store-level mutation test via `customPath`.
   One line plus one test; highest value per effort.
2. **Extend the mutation test pattern to the other 16 save stores** (cluster B).
3. **Repair the broken `.codex/skills/ashfall-wire` symlink**, then establish the
   `--player-panels-uitest` leak baseline to classify SUSP-01.
4. **Add a reentrancy guard or documented precondition** on
   `ExpeditionSystem.TickHours`, `CaregivingSystem.Tick`,
   `SomaticFlashbackSystem.TickAll` (BUG-02).
5. **Complete the unsubscribe-first `Bind` sweep** on the remaining ~15 panels, or
   formally rule out re-Bind as a supported operation (BUG-03).
6. **Commit the `.ogg` binaries with the catalog changes; gitignore or delete the
   migration scripts; re-split the 8 merged statement lines** (BUG-04, BUG-05).

## 18. Evidence Index

| Finding | Primary evidence |
|---|---|
| BUG-01 | `src/Host/WorldSaveStore.cs:84-88`; 16 sibling stores' reject paths; `SaveStoreChecksumSweepTests.cs:162-204` |
| BUG-02 | `ExpeditionSystem.cs:180-226`; `CaregivingSystem.cs:171-215`; `SomaticFlashbackSystem.cs:217-228` |
| BUG-03 | `DeepCoastPanel.cs:47-57`; `MapPanel.cs:50-63`; contrast `GreenhousePanel.cs:46-50` |
| BUG-04 | `git status` (` D` wav / `??` ogg); `.gitattributes:186-188`; `AudioSelfTest` asset list |
| BUG-05 | `EconomyMarketPanel.cs:98` etc.; repo-root `fix_*.py`, `safe_fix.py`, `test_parse.py` |
| SUSP-01 | `src/Main.UiHandlers.cs:342`; uitest exit output (33 ObjectDB leaks) |
| SUSP-02 | `CaregivingSystem.cs:180-184` vs `ExpeditionSystem.cs:187-190` |
| Baseline | test/build/selftest outputs in §3 |

## 19. Audit Confidence

High on BUG-01 (source + contract + sibling comparison, trivially reproducible by
corrupting a world save). High on BUG-03 (direct source contrast). Medium on BUG-02
(latent; no live reentrant caller found). Medium on SUSP-01 (blocked baseline). The
green baseline (2497 tests, 5 selftest verbs) bounds the blast radius: nothing in this
diff breaks the currently-pinned behavior.

## 20. Audit Completion Statement

All ten loops executed against the working-tree diff at `de1c0a1`. Candidates were
revisited across loops; seven plausible findings were rejected with evidence; two
root-cause clusters and two cross-system chains identified; no production code was
modified. Confirmed findings: 1 HIGH, 2 MEDIUM, 2 LOW, 3 SUSPECTED. The single
must-fix-before-commit item is the `WorldSaveStore` checksum-mismatch softening.
