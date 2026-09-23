# ASHFALL — WAVE 2 INTEGRATION PROGRAM · PLAN 2 OF 6

# BUG & SILENT-FAILURE REPAIR INTEGRATION PLAN

**Status:** PROPOSAL — planning-only · no production path claimed
**Wave:** W2 (six-plan integration wave)
**Document:** W2-02 · part A of E
**Target size:** ~150,000 characters (this plan)
**Date:** 2026-09-21
**Repo:** `Atomic War` @ `Zcode_Branch`, HEAD `5be1a30a`
**Companion plans:** W2-01 (maintenance), W2-03 (gameplay), W2-04 (environments), W2-05 (locations), W2-06 (enrichment)
**Plan-unblocking annex:** Annex U at the end of this document — deliberately separated per the Wave 2 rule.

---

## 0. How to read this plan

This plan repairs defects and removes silent-failure classes. It is built so
that every repair has a demonstrated failing case **before** the fix, and a
regression test **with** the fix. The repository's own philosophy applies:
"a compile-green result is not proof of runtime integration", and a fix
without a reproducing case is a hypothesis.

### 0.1 Two selection levels

**Level 1 — Plan Path (choose one):**

| Plan Path | Name | Meaning |
|---|---|---|
| **A** | Verified Repair | fix only defects with a demonstrated repro; smallest surface |
| **B** | Class Elimination | fix the demonstrated defects **and** the class that produced them (gate/test) |
| **C** | Hardening Program | class elimination plus structural hardening of the failure paths (lifecycle/error model) |

**Level 2 — Point Paths (A/B/C at each of the ten points).** The plan-level
path sets defaults (§0.2); the selection sheet (Appendix A) overrides.

### 0.2 Default mapping

| Plan Path | Points mostly at A | Points mostly at B | Points mostly at C |
|---|---|---|---|
| A Verified Repair | 1–10 | — | — |
| B Class Elimination | 2,6 | 1,3,4,5,7,8,9,10 | — |
| C Hardening Program | — | 2,4 | 1,3,5,6,7,8,9,10 |

### 0.3 The Wave 2 rule for this plan

> **This plan does not add features and does not change balance.** If a repair
> legitimately requires a design decision (e.g., "should a missing catalog row
> fail the tick or default?"), the decision is presented as an A/B/C point and
> the chosen behaviour is recorded; it is never chosen silently by a builder.

### 0.4 Severity vocabulary

| Class | Meaning | Response |
|---|---|---|
| **S1 — corrupts state** | a defect that writes wrong data to a save or silently mutates authority | fix before anything else; add save round-trip test |
| **S2 — wrong behaviour, visible** | wrong numbers/outcomes the player can see | fix; add focused behavioural test |
| **S3 — wrong behaviour, invisible** | stale cache, wrong owner used, dead handler | fix; add lifecycle/parity test |
| **S4 — fragile** | no current failure but a demonstrated unsafe pattern | harden under Path B/C; gate under Path B |
| **S5 — cosmetic** | logging/format/uicatalog only | maintenance route (W2-01) |

Every point below is assigned a class. Nothing here is speculative: S1–S3
points have a named repro or a proof-by-inspection with exact lines.

---

## 1. Executive summary

The repository is not drowning in bugs — the debt ledger's open rows are
almost entirely ACCEPTED design deferrals, and the historical full suite was
11,697/11,697 green at D1. The remaining bug surface is concentrated in
**lifecycle boundaries** (what happens on load, reset, and reload) and in
**silent failure paths** (a guard early-returns and nobody notices the state it
keeps).

The verified flagship finding is the **D19c-family stale host cache**:

- `_silentFoundry` (`src/Main.ExpansionHub.cs:35`) is created by
  `SetupSilentFoundry()` (`src/Main.Economy.cs:210`), which early-returns when
  the field is non-null.
- `_sharedSkillProgression` (`src/Main.CampaignServices.cs:173`) and
  `_sharedFactionStance` (`src/Main.CampaignServices.cs:195`) are created by
  `Ensure*` methods that also never re-create once set.
- None of the three fields is ever assigned `null` — verified by
  `grep -rn "_silentFoundry = null\|_sharedFactionStance = null\|_sharedSkillProgression = null" src/`
  returning **zero matches**.
- Meanwhile the reset paths **do** null their dependencies: `_inventory`
  (`Main.Lifecycle.cs:73`), `_expansions` (110), `_economy` (161), `_journal`
  (231).

Consequence: after `TryLoadAndRestoreGame()`'s reset → restore sequence, the
foundry host session (and the stance engine, and the shared skill progression)
still reference the **previous** subsystem instances while the rest of the
game receives new ones. State changed through those stale references can be
lost at the next save, or a stale Foundry can answer reads with pre-load data.
This is exactly the class of defect the reset comment in
`TryLoadAndRestoreGame` claims to prevent ("so guarded SetupXxx methods cannot
retain state … from the previously active slot").

Whether it manifests visibly depends on which of the stale references are
read after a load — which is why the plan's Path A starts with a **repro
harness**, not a guess.

### 1.1 The ten points

| # | Point | Class | Default |
|---|---|---|---|
| 1 | Stale host caches across reset/load (D19c family) | S3→S1 risk | B |
| 2 | Reset coverage parity for all setup-created fields | S3 | B |
| 3 | Save/load behavioural edges (partial, corrupt, legacy) | S1/S2 | B |
| 4 | Event subscription lifecycle (double-subscribe, stale handlers) | S2/S3 | B |
| 5 | Silent catch → typed failure or explicit ignore | S3 | B |
| 6 | Numeric edges (NaN, divide-by-zero, culture) | S2/S4 | A |
| 7 | Determinism regression guards | S1/S3 | B |
| 8 | UI failure paths (null owner, stale bind, close/back) | S2 | B |
| 9 | Tick reentrancy and double-dispatch | S3 | B |
| 10 | Data-dependent runtime failures (missing rows/references) | S2 | B |

---

## 2. Verified current state (bug-repair evidence)

### 2.1 The D19c-family defect, fully assembled

**Field creation sites:**

| Field | Declared | Created by | Guard |
|---|---|---|---|
| `_silentFoundry` | `src/Main.ExpansionHub.cs:35` | `SetupSilentFoundry()` `src/Main.Economy.cs:210` | `if (_silentFoundry != null) return;` (line 212) |
| `_sharedSkillProgression` | `src/Main.CampaignServices.cs` (field near 171) | `EnsureSharedSkillProgression()` (`:173`) | `if (_sharedSkillProgression != null) return …` |
| `_sharedFactionStance` | `src/Main.CampaignServices.cs:195` | `EnsureSharedFactionStance()` (`:197`) | `if (_sharedFactionStance != null) return …`; also calls `SetupSilentFoundry()` |

**Reset paths that clear dependencies but not these fields:**

- `ResetPlansExpansionSessions()` (`Main.Lifecycle.cs:398`) nulls `_generational`,
  `_prisoners`, …, `_robotics`, dirty flags. **Does not name `_silentFoundry`,
  `_sharedFactionStance`, `_sharedSkillProgression`.**
- `ResetEnrolledFlagshipSessions()` (`Main.Lifecycle.cs:438`) nulls
  `_moralChoice`, `_endgame`, `_caravanTradeNetwork`, `_surgicalWard`, … plus
  the dependency fields at lines 73/110/161/231.
- `ResetAllSessionsInMemory()` (`Main.Lifecycle.cs:521`) calls
  `_lifecycleRegistry.ResetAll()`; the lifecycle registry participants
  (registered in `RegisterLifecycleParticipants`, 20+) include many sessions —
  but the three fields above are not registered participants under those names
  (verified: no `DelegateSessionParticipant` closure assigns them).

**Load path:** `TryLoadAndRestoreGame("src/Main.SaveOrchestrator.cs:108+")`
calls `ResetAllSessionsInMemory()` and then `RestoreAllSubsystemsFromDisk()`,
whose setup calls early-return for the three fields.

**Why this matters (three concrete failure shapes):**

1. **Lost writes after slot switch.** Load slot B; the stale foundry still
   writes its `ExpansionHubSave` based on slot A's salt-mine/journal context
   until something rebinds it. §2.2 checks whether `BindStanceProviders`
   closures (`campaignDayProvider: () => _simDay`, `survivorsProvider: () =>
   _survivors`) re-read the new fields: if they do, the read side is
   self-healing and only internal foundry state (e.g., `SaltMine.RestoreState`
   which runs only at creation) stays stale.
2. **Double-restore skip.** Because `SetupSilentFoundry` never runs again,
   `ExpansionHubSaveStore.TryLoad()` / `SaltMine.RestoreState(hubSave.saltMine)`
   never re-execute on the load path. The salt mine's restored state comes from
   whatever was in memory, not from the newly loaded slot — the **new slot's
   salt mine state is never applied**.
3. **Stance/skill bleed.** `_sharedFactionStance` is fetched once and held by
   surfaces bound with it (`BindStance` in `SetupSilentFoundry`); a stale
   engine means trade offers can reflect slot A's stance after loading slot B.

Failure shape 2 is the most likely to be player-visible; shape 3 the most
likely to be data-corrupting via subsequent writes. The plan treats the whole
family as one point because the fix pattern is identical.

### 2.2 What the bind closures reveal (why the repro matters)

`SetupSilentFoundry` binds live accessors:

```csharp
_silentFoundry.BindStanceProviders(
    campaignDayProvider: () => _simDay,
    partyRadiationProvider: () => _holdfastRuntime?.Radiation ?? 0f,
    survivorsProvider: () => _survivors);
```

Because `_simDay`, `_holdfastRuntime`, `_survivors` are re-read through
closures, the stale-session read side partially self-heals. But:

- `_silentFoundry.Engine.BindVentilation(_ventilation)` binds a **direct
  reference** (not a closure) to the ventilation authority; after reset, that
  reference is the old ventilation instance.
- `_silentFoundryPanel.Bind(...)` and `_economyPanel.BindStance(...)` bind the
  session directly; the panel keeps pointing at the old session, which is fine
  only if the session itself is still the intended owner (it is not, under the
  reset contract).
- `SaltMine.RestoreState` and `ExpansionHubSaveStore.TryLoad()` never re-run.

So the defect is real regardless of the closures: at minimum the salt-mine and
ventilation bindings are stale after a slot switch.

### 2.3 Reset-coverage census method (for Point 2)

```bash
# 1. every Setup* method in Main partials
grep -rhn "private void Setup\w*(" src/Main*.cs | sed 's/.*void //;s/(.*//' | sort -u
# 2. every field assigned inside them (heuristic first pass)
grep -rn "_[a-zA-Z]* = " src/Main*.cs | grep -v "==" | wc -l
# 3. every field nulled in reset methods
sed -n '398,520p' src/Main.Lifecycle.cs | grep -o "_[a-zA-Z]* = null" | sort -u
```

The exact census is the Point 2 deliverable; the method above is the template.

### 2.4 Lifecycle/subscription evidence

- Panel hygiene already has gates: `PanelSubscriptionHygiene` (1/1 per the B2
  claim evidence), `PanelRouteGateTests` (20/20), `PanelBindLifecycle` UI
  selftest (17/17).
- Known lifecycle patterns verified in host code: `StateChanged += () => {...}`
  anonymous handler in `SetupSilentFoundry` — never unsubscribed, and captured
  `this` state. Repeated creation would double-subscribe; current early-return
  prevents that, but the pattern is fragile if the field is ever reset without
  also unsubscribing.
- `CloseTradePanel` does unsubscribe: `_silentFoundry.StateChanged -=
  _tradePanel.RefreshView;` — showing the repository knows the pattern; the
  anonymous lambda in setup is the outlier.

### 2.5 Determinism evidence

Current deterministic guarantees are excellent by inspection: all
`System.Random`-adjacent matches are comments asserting `ISeededRng` usage;
paired-replay tests exist (`Plan166_169ReloadReplayTests`,
`C2AmputationTravelTests`, etc.). The open risks are **static state affecting
outcomes** (Point 7) and **save-checksum culture** (already
`CultureInfo.InvariantCulture` in `SaveSlotService` lines 1222/1290 — verified).

### 2.6 Silent-failure evidence

- Bare `catch {}` is effectively absent (1 comment mention).
- `catch (Exception` count: 619 — classification worksheet method is in W2-01
  Part D; this plan owns the **repairs** for the silent subset.
- `CatalogDiagnostics.cs` exists specifically because "historically these
  branches used bare `catch { }` (silent swallow)" — the repository invented a
  diagnostics channel for this class; Point 5 extends that philosophy.

---

## 3. Scope, non-goals, and rules

### 3.1 In scope

- Repairs to S1–S3 defects with demonstrated repros.
- Class elimination for the failure classes those defects represent.
- Regression tests for every repair.
- Typed-failure conversion for silent catches (behaviour-preserving where the
  caller ignores failures today; behaviour-explicit where it must not).
- Lifecycle hardening for reset/load/reload paths.

### 3.2 Non-goals

- Balance/gameplay tuning → W2-03.
- New environment/location/narrative systems → W2-04/05/06.
- Ledger/register truth → UNBLOCK-04.
- Save **schema** changes → only under UNBLOCK-01/02 signatures.
- Feature work of any kind.
- "While I'm here" refactors: the diff is the fix plus its test.

### 3.3 Rules for every repair

1. **Repro before fix.** A failing case exists (test, harness, or a scripted
   host sequence) before the code changes. If a repro cannot be built, the
   finding is S4 (fragile) and gets a gate, not a rewrite.
2. **Minimal change.** Prefer a one-line null assignment to a redesign.
3. **No behaviour change beyond the defect.** If a fix would alter visible
   behaviour, it becomes an A/B/C decision point in this plan (or moves to its
   owning plan).
4. **Round-trip where state is involved.** Save → act → load → act, with
   equality evidence.
5. **Paired replay where determinism is involved.** Same seed, two runs,
   fingerprint equality.
6. **Focused tests only** (`TEST_POLICY.md`); the bug plan is exactly where a
   bloated test run is most tempting and least useful.

---

## 4. Plan Path and decision index

### 4.1 Selection sheet

```text
PLAN 2 — BUG & SILENT-FAILURE REPAIR
Plan Path: [ ] A Verified Repair  [ ] B Class Elimination  [ ] C Hardening
           (default recommendation: B)

Point  1 (stale host caches) ........ [ ] A [ ] B [ ] C   default B
Point  2 (reset coverage parity) .... [ ] A [ ] B [ ] C   default B
Point  3 (save/load edges) .......... [ ] A [ ] B [ ] C   default B
Point  4 (subscription lifecycle) ... [ ] A [ ] B [ ] C   default B
Point  5 (silent catches) ........... [ ] A [ ] B [ ] C   default B
Point  6 (numeric edges) ............ [ ] A [ ] B [ ] C   default A
Point  7 (determinism guards) ....... [ ] A [ ] B [ ] C   default B
Point  8 (UI failure paths) ......... [ ] A [ ] B [ ] C   default B
Point  9 (tick reentrancy) .......... [ ] A [ ] B [ ] C   default B
Point 10 (data-dependent failures) .. [ ] A [ ] B [ ] C   default B
```

### 4.2 Recommended default: Plan Path B with Point 6 at A

Rationale: Points 1–5 and 7–10 have verified S3/S2 findings where class
elimination is cheap (the gates from W2-01 already exist for several). Point 6
is currently clean under inspection; a light audit suffices until a repro
appears.

---

## 5. Decision Point 1 — Stale host caches across reset/load (S3→S1)

### 5.1 The three options

**Path A — Minimal nulling.** Assign `null` to the three fields in the reset
path that nulls their dependencies (`ResetEnrolledFlagshipSessions` or
`ResetPlansExpansionSessions`), and add a focused test proving re-creation
restores from the newly loaded slot. This is the one-line-per-field fix.

**Path B — Null + ownership registration + parity test.** Path A, plus:

1. Register the foundry/stance/skill hosts as lifecycle participants (or give
   them an explicit `DisposeForReset()`), so reset ordering is explicit rather
   than incidental.
2. Make the anonymous `StateChanged += () => {...}` handler in
   `SetupSilentFoundry` a named method with a symmetric unsubscribe, so re-creation
   cannot double-subscribe.
3. Extend W2-01's `MainBootstrapParityTests` reset-coverage assertion to prove
   the class cannot return for any setup-created field.
4. A load-slot-switch integration test: create slot A, mutate foundry salt-mine
   state, save; load slot B; assert every foundry/stance/skill read matches
   slot B's saved truth (not A's).

**Path C — Session lifecycle model.** Path B, plus a uniform "
`IHostSessionLifecycle` " contract for host sessions (create, bind, dispose,
reset) with the lifecycle registry executing it in reverse dependency order,
and all existing ad-hoc reset logic migrated incrementally. This is the
structural fix for the whole class, at the cost of touching many sessions — it
should be a separate signed package.

### 5.2 The fix sequence (all paths)

```text
1. Build the repro harness (pure host-level test or scripted sequence):
   - create session state in slot A (salt mine extracted > 0)
   - save A; load B (or new game) with different salt-mine truth
   - read the foundry's state through its consumer surfaces
   - EXPECT (current): A's state leaks
   - EXPECT (fixed): B's state
2. Apply the chosen fix.
3. Re-run harness: fixed expectation.
4. Regression: foundry/stance/skill suites + save round-trip + triad drift.
5. Record the D19c debt row as RETIRED with evidence.
```

### 5.3 Why Path A might not be enough

Nulling the fields forces re-creation, which re-runs
`ExpansionHubSaveStore.TryLoad()` and `SaltMine.RestoreState` — good. But
re-creation also:

- rebuilds the `StateChanged` subscription (the anonymous handler's old
  instance dies with the old session, so no leak in practice — but if the old
  session's handler captured panels, the old session is still referenced by
  the panel until the panel rebinds; `SetupSilentFoundry` rebinds the panel,
  so this resolves);
- re-runs `BindVentilation(_ventilation)` with the current instance (good);
- re-runs `BindStanceProviders` with current closures (good).

So Path A is likely sufficient for the read/write correctness; Path B's extra
value is making the fix **provable** rather than incidental, and preventing
recurrence. The recommendation is Path B.

### 5.4 Test skeletons

```csharp
[Fact]
public void LoadSlot_SwapsFoundryAuthority_NoStaleState()
{
    var host = new HostHarness();
    host.NewGame(seed: 1);
    host.SaltMine.Extract(units: 10);        // mutate campaign state
    host.Save("A");
    host.NewGame(seed: 2);                    // different campaign
    host.SaltMine.Extract(units: 3);
    host.Save("B");
    host.Load("A");
    Assert.Equal(10, host.ReadSaltMineThroughFoundry());
    host.Load("B");
    Assert.Equal(3, host.ReadSaltMineThroughFoundry());
}

[Fact]
public void ResetAllSessionsInMemory_ClearsEverySetupCreatedField()
{
    // companion to MainBootstrapParityTests; asserts the specific fields
    Assert.True(BootstrapInventory.ResetClearedFields().Contains("_silentFoundry"));
    Assert.True(BootstrapInventory.ResetClearedFields().Contains("_sharedFactionStance"));
    Assert.True(BootstrapInventory.ResetClearedFields().Contains("_sharedSkillProgression"));
}
```

### 5.5 Acceptance for Point 1

1. A repro exists and fails on HEAD.
2. After the fix it passes, and the slot-switch harness proves B-state.
3. Reset-coverage parity test includes the three fields.
4. Foundry/stance/skill suites and triad drift pass.
5. Debt row recorded.

### 5.6 What this point must not do

- Not redesign the foundry.
- Not change what the salt mine restores (that is its own contract).
- Not silently change stance behaviour (e.g., drop the exception thrown by
  `EnsureSharedFactionStance` when no authority exists — that exception is a
  deliberate guard).

---

## 6. Decision Point 2 — Reset coverage parity for all setup-created fields (S3)

### 6.1 The class

Point 1 is one instance of a class: **a setup-created host field with no reset
path**. The Point 2 work is the census and the guard.

### 6.2 Path A — Census and document

- Generate the full list: every `Setup*` method → fields it assigns → whether
  any reset clears them.
- Publish `docs/maintenance/RESET_COVERAGE.md` with per-field verdicts:
  cleared / exempt (with reason) / **gap**.
- Fix gaps only where a repro exists (otherwise report).

### 6.3 Path B — Parity test (the recommended path)

- Implement the parser and test from W2-01 M3-B (or consume it if that plan
  landed the same artifact) so that any future unlisted setup field fails CI.
- Fix every gap found in the census that has a deterministic failure shape
  (most are the Point 1 family; expect a small number of others).
- Exemption list lives in code with reasons; the test asserts the list does not
  grow without a diff (a new exemption is a reviewable change).

Path B requires care: **not every setup-created field should be reset**.
Panels, immutable config, and Godot nodes intentionally persist. The exemption
mechanism is the built-in escape, and the reviewer's job is to read exemptions,
not to eliminate them.

### 6.4 Path C — Lifecycle contract

The uniform contract from Point 1 Path C, applied here as the enforcement
mechanism: fields not covered by a reset are covered by an explicit
`KeepAlive`/`Rebuilt` declaration in the session descriptor. This is the same
structural package and should be signed together.

### 6.5 Verification and acceptance

- Census complete (every `Setup*` field classified).
- Parity test green; negative test (add a field, watch it fail) demonstrated.
- Each fixed gap has a repro or a documented deterministic argument.
- No gameplay behaviour changes beyond stale-state elimination.

---

## 7. Decision Point 3 — Save/load behavioural edges (S1/S2)

### 7.1 Evidence base

The save system is mature: `TryLoadAndRestoreGame` leaves live state intact on
failure (documented in its XML comment), checksum formatting is
culture-invariant, and a large save test family exists (Save suite 1,152/1,152
in D1 evidence). The remaining edge classes:

| Edge | Question | Current behaviour (to verify at P0) |
|---|---|---|
| Partial slot | slot with some section files present, others absent | restore must default the absent sections, not fail |
| Corrupt section | one section's JSON invalid | restore must fail cleanly or skip with a typed message |
| Checksum mismatch | section hash wrong | must refuse the load and keep live state |
| Legacy envelope | pre-v2 formats | must migrate or refuse with a message |
| Mid-load abort | failure after some sections restored | live state must not be a half-loaded campaign |
| Slot switch | Point 1 family | stale owners must not survive |

### 7.2 Path A — Verify and test only

- Write focused tests for each edge class against current behaviour.
- Any behaviour that is already correct is documented as covered; any that is
  wrong becomes an S1/S2 finding with a repro.

Path A is valuable precisely because it may find nothing: it turns "the save
system is mature" into evidence.

### 7.3 Path B — Edge tests + typed failure surface

- Path A, plus: every failure path returns a typed result the UI can present
  (the load-failure FeedbackEvent already exists); abort paths restore or
  preserve live state atomically (verify and test).
- Add a **save-store matrix test** that enumerates every registered section and
  asserts capture/restore round-trip with a sentinel value, catching a section
  that silently no-ops.

### 7.4 Path C — Transactional restore

Path B, plus a snapshot-before-restore guard: capture the current campaign's
in-memory state (or the ledger of already-restored sections) so a mid-load
failure rolls back to the pre-load campaign rather than leaving a hybrid. This
is the strongest guarantee and the largest change; it should be signed
separately after Path B proves the edge tests.

### 7.5 Test skeletons

```csharp
[Theory]
[InlineData("partial_slot")]     // one section file removed
[InlineData("corrupt_section")]  // invalid JSON in one section
[InlineData("checksum_mismatch")]// one hash altered
[InlineData("legacy_envelope")]  // old schema shape
public void LoadEdge_LeavesLiveStateOrFailsCleanly(string fixture)
{
    var host = new SaveHarness().WithLiveCampaign(seed: 7);
    var before = host.Fingerprint();
    var result = host.TryLoad(fixture);
    if (result.Success) Assert.True(host.IsFullyRestored());
    else Assert.Equal(before, host.Fingerprint());
}
```

### 7.6 Acceptance

- Every edge class has a test with an explicit expected outcome.
- No edge leaves hybrid state.
- Failure messages are player-readable (no raw exception text in UI).
- No schema change; no new section.

---

## 8. Decision Point 4 — Event subscription lifecycle (S2/S3)

### 8.1 Evidence

The repository already tests panel subscription hygiene
(`PanelSubscriptionHygiene`, `PanelBindLifecycle` 17/17,
`PanelRouteGateTests` 20/20). The known outlier is the anonymous lambda
subscription in `SetupSilentFoundry` (never unsubscribed; relies on the guard
early-return). Pattern-wise:

| Pattern | Sites (sampled) | Risk |
|---|---|---|
| named method + symmetric unsubscribe | `CloseTradePanel` | safe |
| anonymous lambda in setup | `SetupSilentFoundry` | safe only while guard holds; impossible to unsubscribe |
| panel-level bind/unbind in open/close | UI panels | covered by existing gates |

### 8.2 Path A — Inventory and test

- Enumerate every `+=` subscription in host setup methods; check for a matching
  `-=` in the close/reset path.
- Add a focused test where a host session is re-created twice and assert no
  double-dispatch (a counter-based assertion).

### 8.3 Path B — Named-handler rule + reentrancy proof

- Convert anonymous setup subscriptions to named methods with symmetric
  unsubscribe (bounded set: start with the foundry outlier).
- Extend the existing panel hygiene gate to host sessions: every `+=` in a
  `Setup*` method must have a reachable `-=` in a `Close*`/`Reset*` method or a
  documented lifetime reason.
- Double-creation test per converted session: create → dispose/reset → create,
  assert exactly one dispatch per event.

### 8.4 Path C — Subscription ownership model

Path B, plus a small host-side subscription ledger (debug-only) that tracks
handler counts per event source and fails the smoke selftest when a source
accumulates handlers across a reset cycle. This makes the leak class
observable in normal play.

### 8.5 Acceptance

- Every host setup subscription is named and unsubscribable, or documented.
- Re-creation dispatches once.
- Existing panel hygiene gates remain green.

---

## 9. Decision Point 5 — Silent catch to typed failure (S3)

### 9.1 Evidence and principle

W2-01 generates the classification worksheet; this point performs the repairs:

| Category | Example | Action |
|---|---|---|
| Silent ignore, correct | probing an optional file | annotate `// INTENTIONAL-IGNORE: optional catalog probe` |
| Silent ignore, wrong | swallowing a save-write failure | convert to typed failure + surface/LogError |
| Logged-and-continue | parse failure with default | keep; ensure the log names the file/section |
| Converted | returns `null`/default | make the nullability explicit in the signature or a comment |

### 9.2 Path A — Fix the proven S1/S2 swallows only

From the worksheet, fix only sites where the swallow can lose state (save
writes, catalog loads feeding authority, event dispatch). Annotate the rest.

### 9.3 Path B — Typed failure rule for authority paths

- Define the rule: any catch inside a save/catalog/authority write path must
  either rethrow, return a typed failure, or record a diagnostics entry the
  selftest can assert on — no silent path.
- Apply to the enumerated authority paths (a small set; the worksheet names
  them).
- Add an error-path test per converted site.

### 9.4 Path C — Uniform failure envelope

Path B, plus a Core `OperationResult<T>`-style envelope (if the repo does not
already have one — check before adding, per Rule 5) used consistently by
authority paths, with UI adapters presenting typed messages. This is a broad
change and should be staged; it is not required to fix a bug.

### 9.5 Acceptance

- No silent swallow remains in a save/catalog/authority path.
- Each conversion has a test.
- Diagnostics entries are assertable (not just printed).

---

## 10. Decision Point 6 — Numeric edges (S2/S4)

### 10.1 Candidate classes

| Class | Where to look | Current posture |
|---|---|---|
| division by zero / zero-count averages | cohort/staffing/roster aggregates | verify guards |
| NaN/Infinity propagation | rare in integer-heavy simulation; check float accumulators (needs, exposure, morale) | verify clamps |
| float equality | 35 `== 0f`-style matches in Core | mostly intentional guards; audit the non-guard ones |
| integer overflow | day counters (int) — fine for centuries; tick accumulators | verify no unchecked multiplication |
| culture formatting | save checksum already invariant | verify other `ToString()` in serialized paths |

### 10.2 Path A — Audit and test the risky five

- For each class, find the concrete candidate sites (the audit is the
  deliverable) and add a boundary test: zero survivors, zero production, empty
  adjacency, max day.
- Fix only demonstrated failures.

### 10.3 Path B — Clamp/guard standard for aggregate math

- Introduce (or reuse) a tiny `MathSafe` helper in Core for
  `SafeDivide`, `Clamp01`, `Sanitize(float)`, and use it in the audited
  aggregates. No numeric behaviour change except at boundaries (which are the
  defects).
- Gate: new aggregate division in Core without a zero-guard is flagged
  (warn-only initially).

### 10.4 Path C — Property-based boundary suite

Path B, plus a property-based test harness (bounded case count, deterministic
seed) exercising the survival tick with extreme inputs (all-zero, all-max,
empty rosters) and asserting no NaN/exception and monotone invariants. This is
a strong long-term guard; it is also the largest test addition and must be
budgeted accordingly.

### 10.5 Acceptance

- Every audited site has a boundary test.
- No NaN can enter a persisted float (test asserts save checksum stability
  across boundary inputs).
- No culture-dependent formatting in persisted strings.

---

*(Part A ends. Part B continues with Decision Points 7–10, then execution,
verification, risks, ownership, rollback, DoD, Annex U, and appendices.)*---

# PART B — DECISION POINTS 7–10 AND EXECUTION

---

## 11. Decision Point 7 — Determinism regression guards (S1/S3)

### 11.1 Evidence

The repo already enforces determinism well:

- Core comments assert `ISeededRng` usage instead of `System.Random`
  (`RailwayInterlockEngine`, `PharmaceuticalTabletEngine`,
  `AquiferPiezometerEngine`, `SofcElectrochemistryEngine`, `SkillProgressionSystem`,
  `UtilityAiSystem`, `SilentFoundrySystem`, …).
- Save checksum formatting is culture-invariant (`SaveSlotService` lines
  1222/1290).
- Paired-replay suites exist (`Plan166_169ReloadReplayTests`,
  `C2AmputationTravelTests`, and others).

The residual risks:

1. **Static state participating in outcomes** — if a system reads a static
   mutable field, two runs in one process can diverge from two fresh processes
   even with the same seed. Point 6 of W2-01 gates the statics; this point adds
   the outcome-level proof.
2. **Reload parity** — continuous run vs. save/load-and-continue must produce
   identical fingerprints (the "reload replay" pattern). Coverage of this is
   good but not exhaustive; new systems added since the last replay sweep may
   lack it.
3. **Seed source drift** — a new system seeded from wall-clock or a hash would
   be silently nondeterministic; the audit enumerates every seed construction.

### 11.2 Path A — Audit seeds and replay coverage

Deliverables:

- A table of every `ISeededRng` construction site in Core with its seed source
  (campaign seed, day-derived, stream fork, constant fallback).
- A coverage list: which systems have a reload-parity test and which do not.
- Findings only; tests added only for the highest-risk un-covered systems
  (newest ones).

### 11.3 Path B — Replay-parity expansion

- For each uncovered system from the audit, add one reload-parity test using
  the established pattern (continuous fingerprint == reload fingerprint).
- For any static-influenced system, add the two-process equivalence test (or
  document why the static is outcome-neutral).
- Gate: a new Core system that constructs `ISeededRng` without appearing in the
  seed-source table fails the documentation check (the table is generated from
  the same scan).

### 11.4 Path C — Fingerprint harness

Path B, plus a general fingerprint facility: a Core function producing a stable
hash of campaign state across all registered owners, used by:
- reload parity,
- paired replay,
- slot switch (Point 1's harness),
- save round-trip.

This is the single most useful long-term debugging asset for a simulation of
this size, and it composes with W2-01's `--replay` ambitions. Like other C paths
it wants a separate signed package.

### 11.5 Acceptance

- Seed-source table complete; no unexplained construction site.
- Every audited high-risk system has a replay-parity or explicit exemption.
- Fingerprint stability demonstrated across save/load cycles.
- No new `System.Random` anywhere (grep stays clean).

### 11.6 Test skeleton

```csharp
[Fact]
public void ReloadParity_ContinuousVersusMidLoad()
{
    var a = Sim.Run(seed: 42, days: 30);
    var b = Sim.Run(seed: 42, days: 15);
    b.Save(); b.Load(); b.Run(days: 15);
    Assert.Equal(a.Fingerprint(), b.Fingerprint());
}
```

---

## 12. Decision Point 8 — UI failure paths (S2)

### 12.1 Evidence

Existing gates are strong: `PanelRouteGateTests` (20/20),
`PlayerSurfaceCoverageGateTests` (8/8), `PanelSubscriptionHygiene`,
`ProductionUiNoFabricatedFallback` (4/4), `PlayerSurfaceBindingPurity` (2/2),
`UiPanelContract`, `HostCliHelpContract`, plus the `player-panels-uitest` and
`save-load-ui-failure` selftests and the a11y selftest (5/5). The failure
classes that remain are behavioural rather than structural:

| Class | Example shape | Check |
|---|---|---|
| Bind to null owner | panel opened before setup (load path ordering) | does every panel handle a not-yet-created owner? |
| Stale bind after slot switch | Points 1/4 family | bind freshness |
| Close/back parity | every panel closes with Esc/B/controller | existing behaviour; verify per new panel |
| Error presentation | save/load failure message readable | existing FeedbackEvent path |
| Disabled actions with no explanation | button unavailable silently | a11y/UX check (informational) |

### 12.2 Path A — Verify per class with focused UI tests

- Open each panel on: fresh game, mid-load, after slot switch, with a null
  prerequisite. Assert no exception and a readable state (either content or an
  explicit "not available" message).
- Record results; fix only demonstrated failures.

### 12.3 Path B — Failure-path contract test kit

- A reusable test kit: `AssertPanelSurvivesNullOwner(panelId)` and
  `AssertPanelCloseParity(panelId)` driving every registered panel from the
  surface manifest.
- Fix failures found (typically an early-return guard or a placeholder message
  routed through the existing production-UI purity rules — never a fabricated
  fallback that the purity gate forbids).
- Gate: new panels must be added to the kit's list; the kit is generated from
  the manifest so it cannot drift.

### 12.4 Path C — Panel resilience model

Path B, plus a declared `PanelState` (Unavailable/Ready/Stale) each panel
exposes, with the router consulting it and the a11y layer announcing state.
This is a real UX/architecture change and should be signed on its own; it also
feeds W2-06's presentation work.

### 12.5 Acceptance

- No panel throws or blank-screens on any of the four contexts.
- Close/back parity holds for every registered panel.
- Failures are presented through the canonical feedback path.
- No fabricated fallback content (purity gate stays green).

---

## 13. Decision Point 9 — Tick reentrancy and double-dispatch (S3)

### 13.1 Evidence

The daily tick drives many owners through `ShelterFacilitiesDayOwner`,
`Main.Tick*` methods, and dirty-flag persistence (`_recreationDirty`,
`_chemWarfareDirty`, `_commsArrayDirty`, `_ceremonyDirty`, `_roboticsDirty`,
`_foundryDirty`, …). Risks:

| Risk | Shape |
|---|---|
| reentrancy | a tick handler triggers a save capture that triggers a tick |
| double-dispatch | two owners consume the same day event (e.g., a day advanced twice by a save/load boundary) |
| dirty-flag race | dirty flag cleared before a write completes, losing the change |
| save-during-tick | capture called inside a tick mutates collections being iterated |

The repository's day ownership is centralized (the campaign-day owner), which
strongly reduces the risk; the audit verifies the pattern rather than assuming
it.

### 13.2 Path A — Audit and instrument

- Enumerate every `Tick*` entry point and its callers (host loop, day owner,
  save orchestrator).
- Add a debug-only guard: a flag asserting a tick is not already in progress;
  logging a warning on re-entry (does not fail the game).
- Run the 7-day smoke and inspect.

### 13.3 Path B — Reentrancy guard as a test

- Convert the debug guard into a test-asserted invariant: a host-level test
  fires a day advance twice in a row without an intervening day boundary and
  asserts single dispatch via counters.
- Fix any site that double-handles (typically by checking "already processed
  today" keys, the repository's proven pattern in journal dedup).
- Ensure dirty flags are set after mutation, not before (audit order).

### 13.4 Path C — Tick transaction model

Path B, plus a tick-scoped transaction: mutations staged and committed at tick
end, with save capture reading the committed state. This is a significant
architecture move (and a potential save-semantics change); it should only be
pursued if Path B finds real double-dispatch defects. The plan recommends
against C by default.

### 13.5 Acceptance

- No reentrant tick in the smoke run.
- Double-advance test dispatches once.
- Dirty flags observed set when state is dirty and cleared after successful
  capture (test per audited owner).

---

## 14. Decision Point 10 — Data-dependent runtime failures (S2)

### 14.1 Evidence

Catalog loading is strict by design (`CatalogIntegrityValidator`,
data-integrity selftest, content-utilization gate), but runtime lookups can
still produce silent defaults:

| Shape | Example |
|---|---|
| missing row for an id referenced by state | save references an item id a later data update removed |
| null loot table | `wasteland_map_v1.json` nodes with `lootTable: null` (verified: several) |
| empty collection consumed | a faction with no radio entries |
| unresolved reference at display time | a journal entry naming an archived catalog id |

The plan's rule: a **runtime** lookup for a persisted id must have a decided
behaviour (skip with diagnostics / substitute a documented fallback /
refuse-and-report), never an unstated default.

### 14.2 Path A — Enumerate the lookup sites

- Find every dictionary/registry lookup keyed by a persisted id in host/Core
  (`TryGetValue` with `out var` default ignore, indexer with `?? default`).
- Classify: guarded, defaulted-documented, defaulted-silent.
- Fix only silent defaults in save-touching paths.

### 14.3 Path B — Lookup contract

- For each silent site, add either a typed failure or a documented fallback
  constant, plus a test that loads a save referencing a removed id and asserts
  the decided behaviour.
- Add a **removed-id simulation** to the save harness (a fixture save with one
  deleted catalog row) as a reusable edge case.

### 14.4 Path C — Referential integrity at restore

Path B, plus an optional restore-time referential check: after loading, scan
persisted id references against current catalogs and produce a user-readable
report (and either continue with substitutions or refuse, per the signed
decision). This is a strong shipping-quality feature but changes load
semantics; it needs its own decision line.

### 14.5 Acceptance

- No silent default on a persisted-id lookup in save-touching paths.
- The removed-id fixture passes with the decided behaviour.
- Data-integrity selftest unchanged (this is runtime, not authoring).

---

## 15. Execution phases

### Phase B0 — Repro freeze (0.5 day, all paths)

- Record HEAD; run the fast gates; build the repro harness skeleton.
- Produce `P0_BUG_PREMISE.md` with:
  - the D19c static finding re-verified (`grep` outputs),
  - the reset-coverage census first pass,
  - the save edge inventory,
  - the subscription inventory,
  - the determinism seed table first pass.

### Phase B1 — S1 repairs first (path-dependent, 0.5–3 days)

- Point 1 (all paths): null the stale fields; add re-creation/round-trip test;
  prove slot-switch.
- Point 3 (Path A/B): any S1 edge found by the harness.
- Rule: S1 always lands before S2/S3 work in the same package.

### Phase B2 — S2 repairs (2–6 days)

- Points 4 (subscription), 8 (UI failure paths), 10 (data-dependent).
- Each with a repro and a regression.

### Phase B3 — S3 class elimination (3–10 days)

- Points 2 (reset parity), 5 (silent catches), 7 (determinism guards),
  9 (reentrancy).
- Where a gate is the deliverable, include the negative test.

### Phase B4 — Hardening (Path C only, separate package)

- Points 1/2 lifecycle contract, 3 transactional restore, 5 failure envelope,
  7 fingerprint harness, 8 panel state model, 9 tick transaction.

### Phase B5 — Closeout (0.5 day)

- Evidence pack; debt rows for repaired defects (RETIRED with repro pointer);
- Handoff; Annex U statement.

### 15.1 Ordering constraints

- B0 always first; B1 before everything else.
- Point 2 depends on W2-01 M3-B's parser if that plan is executing; otherwise
  B0 builds it.
- Point 7's replay tests require a stable fingerprint helper; if Point 7 Path C
  is chosen, it becomes the prerequisite for Points 1/3 harnesses.
- UI point (8) may run concurrently with backend points only with disjoint
  claims (typical: one builder backend, one UI).

---

## 16. Verification plan

### 16.1 Per-point evidence

| Point | Repro | Regression | Gate/negative test |
|---|---|---|---|
| 1 | slot-switch harness fails pre-fix | harness + round-trip | reset parity includes 3 fields |
| 2 | census lists gaps | parity test | add-a-field negative |
| 3 | edge fixtures | edge theory suite | save-store matrix |
| 4 | double-create dispatch count | dispatch-once tests | named-handler rule |
| 5 | silent swallow in authority path | error-path tests | catch-policy gate |
| 6 | boundary fixtures | boundary suite | NaN/culture checks |
| 7 | seed table + replay parity | parity tests | seed-source doc check |
| 8 | null-owner/close-parity kit | kit green | manifest-generated list |
| 9 | double-advance | dispatch-once | reentrancy warning clean |
| 10 | removed-id fixture | decided-behaviour test | lookup contract review |

### 16.2 Commands

```bash
dotnet build Ashfall.csproj --no-restore
bash scripts/run_test.sh Ashfall.Core.Tests/<owning suite>
godot --headless --path . -- --7day-smoke-selftest
godot --headless --path . -- --save-load-ui-failure-selftest
bash scripts/ci/catch-policy-gate.sh
bash scripts/ci/fast-gates.sh
```

### 16.3 The repro standard, restated

A repro is one of:

1. a focused xUnit test that fails on HEAD,
2. a host harness (like the slot-switch harness) with a printed expected/actual,
3. a scripted selftest sequence with a recorded log.

"This code looks wrong" is a finding (S4), not a repro. Findings without repros
get gates or annotations, never silent rewrites.

### 16.4 Budgets

| Point | Test budget |
|---|---|
| 1 | 1 new host test + 2 regional suites |
| 2 | parity test file only |
| 3 | ≤1 new fixture file + save suite |
| 4 | 2 focused tests |
| 5 | 1 test per converted site (≤10) |
| 6 | boundary theory (≤12 cases) |
| 7 | 1 replay per audited system (≤6) |
| 8 | kit run (existing harness) |
| 9 | 1 test + smoke |
| 10 | 1 fixture + 1 test |

Total focused additions: small. The bug plan is not a place for broad runs.

---

## 17. Risks

| # | Risk | L | I | Mitigation |
|---|---|---|---|---|
| 1 | Nulling fields breaks an ordering assumption | M | H | load harness + triad + slot-switch; revert is one line |
| 2 | "Fixing" intentional behaviour | M | H | repro standard; A/B/C decision when behaviour is a choice |
| 3 | Cash complexity in lifecycle contract (C) | M | H | separate signed package; B fixes the defects first |
| 4 | Catch conversion changes failure handling | M | M | preserve caller-visible behaviour; typed failure only where silence loses state |
| 5 | Replay tests flake | L | M | deterministic seeds; fixed day counts |
| 6 | UI kit false positives on intentionally unavailable panels | M | L | kit asserts "no exception + readable state", not availability |
| 7 | Data-dependent decisions conflict with W2-05 ownership | M | M | coordinate claim; location semantics stay W2-05 |
| 8 | Concurrent claims on lifecycle files | M | H | single-writer; W2-01 M3-B and W2-02 coordinate |
| 9 | Touching save code without schema authority | L | H | no schema changes; behavioural only |
| 10 | Scope creep into hardening | M | M | C paths are separate packages |

---

## 18. Ownership and claims

| Phase | Claim | Paths |
|---|---|---|
| B0 | `W2-02-B0-PREMISE` | premise doc + harness skeleton |
| B1 | `W2-02-B1-STALE-OWNERS` | `src/Main.Lifecycle.cs`, `src/Main.CampaignServices.cs`, `src/Main.Economy.cs` (bounded), foundry tests |
| B1 | `W2-02-B1-SAVE-EDGES` | save test fixtures + save harness (if S1 found) |
| B2 | `W2-02-B2-SUBSCRIPTIONS` | `src/Main.Economy.cs` (named handler), hygiene tests |
| B2 | `W2-02-B2-UI-FAILURE` | panel test kit + failing panels |
| B2 | `W2-02-B2-DATA-LOOKUPS` | save fixtures + lookup sites |
| B3 | `W2-02-B3-RESET-PARITY` | parity test (or consume W2-01's) |
| B3 | `W2-02-B3-SILENT-CATCH` | converted authority sites |
| B3 | `W2-02-B3-DETERMINISM` | replay tests + seed table |
| B3 | `W2-02-B3-REENTRANCY` | tick guard + test |
| B5 | `W2-02-B5-CLOSEOUT` | evidence + debt rows (integrator) |

### 18.1 Coordination matrix

| Plan | Shared surface | Rule |
|---|---|---|
| W2-01 | `Main*.cs` orchestration, parity parser | W2-01 extracts; W2-02 repairs; sequential claims |
| W2-03 | needs/health owners | W2-02 fixes defects only; tuning stays W2-03 |
| W2-04 | weather/hazard runtime | environment defects reported to W2-04 if behavioural design needed |
| W2-05 | location runtime lookups | locations.json edits belong to W2-05 |
| W2-06 | panel presentation | UI failure paths here; presentation model in W2-06 |
| W2-01 | catch worksheet | W2-01 measures; W2-02 repairs |

---

## 19. Rollback and decline

| Point | Rollback | Decline consequence |
|---|---|---|
| 1 | `git revert` the null assignments | stale-state risk documented in debt |
| 2 | keep census, drop parity test | class can recur |
| 3 | drop edge tests | unverified edges remain unverified (stated) |
| 4 | revert named handlers | anonymous subscription remains |
| 5 | revert conversions | silent paths remain annotated |
| 6 | drop helper | boundary risks remain measured |
| 7 | drop replay tests | coverage gap documented |
| 8 | drop kit | per-panel verification manual |
| 9 | drop guard | reentrancy unmeasured |
| 10 | drop contract | silent defaults remain listed |

Every decline leaves a debt row stating the residual risk (so the next audit
does not treat it as new).

---

## 20. DoD and handoff

**Path A done when:** every S1–S3 point has a repro and a passing regression;
declines are recorded.

**Path B done when:** all of A, plus each class has its gate/test with a
negative demonstration; no silent swallow in authority paths; reset parity
green.

**Path C done when:** all of B, plus the signed hardening packages are
executed or explicitly deferred with debt rows.

**Handoff fields:** outcome, files, contract, commands/results, limitations,
untouched shared paths, proposed ledger edits, Annex U statement.

### 20.1 First safe step

> Phase B0 only: re-verify the D19c finding and build the slot-switch harness
> (which may already fail). No production edit until the repro is recorded and
> the selection sheet returns.

---

*(Part B ends. Part C continues with worked repair walkthroughs, verifications,
and Annex U.)*---

# PART C — WORKED REPAIR WALKTHROUGHS

This part shows each of the highest-value repairs end to end: the failing
observation, the minimal fix, the regression test, and the verification. It is
written so a builder can execute without re-deriving the approach.

---

## C.1 Walkthrough 1 — The D19c stale-cache repair (Point 1, Path A)

### C.1.1 Failing observation

```text
OBSERVATION (statically verified, HEAD 5be1a30a):
  _silentFoundry, _sharedFactionStance, _sharedSkillProgression
  are never assigned null in any reset path, while their dependencies
  (_inventory, _expansions, _economy, _journal) are nulled in
  ResetEnrolledFlagshipSessions (Main.Lifecycle.cs lines 73/110/161/231).

CONSEQUENCE (semi-static, to be proven by harness):
  After TryLoadAndRestoreGame → ResetAllSessionsInMemory →
  RestoreAllSubsystemsFromDisk, SetupSilentFoundry early-returns
  (Main.Economy.cs:212) so ExpansionHubSaveStore.TryLoad() and
  SaltMine.RestoreState(hubSave.saltMine) do not re-run; the new slot's
  salt-mine truth is never applied to the live session.
```

### C.1.2 The repro harness (pre-fix)

A host-level harness (test project or scripted selftest) performing:

```text
1. NewGame(seed 1); foundry.SaltMine.Extract(10); SaveSlot(A)
2. NewGame(seed 2); foundry.SaltMine.Extract(3);  SaveSlot(B)
3. LoadSlot(A) → read foundry.SaltMine units
4. LoadSlot(B) → read foundry.SaltMine units
```

Pre-fix expectation: step 3 or 4 returns the previous in-memory value (or
step 4 returns A's value through the stale session, depending on restore
order). The harness prints both reads and their fingerprints.

If the harness shows correct values on both loads (which is possible if
`SaltMine` state is re-read through a closure or if `ExpansionHubSave` capture
happens before the read), the finding downgrades to S4 and the plan's Point 1
work becomes Path A with a defensive test (still valuable: it locks the
behavior).

### C.1.3 The minimal fix (Path A)

In the reset method that nulls the dependencies, add the three assignments:

```csharp
// Main.Lifecycle.cs — ResetEnrolledFlagshipSessions (near line 231 where
// _journal is nulled) or ResetPlansExpansionSessions; choose the method whose
// ordering guarantees the foundry is rebuilt after its dependencies.
_silentFoundry = null!;
_sharedFactionStance = null;
_sharedSkillProgression = null;
```

Three lines. No behaviour change other than "rebuild from the new slot".

**Ordering note:** `EnsureSharedFactionStance()` calls `SetupSilentFoundry()`,
so nulling `_sharedFactionStance` without nulling `_silentFoundry` would still
rebuild the stance from the (stale) foundry. All three must be nulled together
— this is why Point 2's parity test matters more than any individual line.

### C.1.4 The named-handler fix (Path B addition)

Replace the anonymous subscription in `SetupSilentFoundry`:

```csharp
// before:
_silentFoundry.StateChanged += () =>
{
    _foundryDirty = true;
    _silentFoundryPanel?.RefreshView();
    ...
};

// after:
_silentFoundry.StateChanged += OnSilentFoundryStateChanged;
private void OnSilentFoundryStateChanged()
{
    _foundryDirty = true;
    _silentFoundryPanel?.RefreshView();
    _factionsPanel?.RefreshView();
    _economyPanel?.RefreshView();
    if (_state == GameState.Playing) UpdateHud();
}
```

And in the reset/close path:

```csharp
if (_silentFoundry != null)
    _silentFoundry.StateChanged -= OnSilentFoundryStateChanged;
_silentFoundry = null!;
```

Now the subscription is symmetric and re-creation cannot double-subscribe even
if the early-return guard is later removed.

### C.1.5 Regression tests

```csharp
[Fact]
public void SlotSwitch_AppliesNewSaltMineTruth() { /* harness from C.1.2 */ }

[Fact]
public void FoundryStateChanged_DispatchesOnceAfterRecreate()
{
    harness.NewGame(1);
    var count = 0;
    harness.FoundryStateChangedProbe(() => count++);
    harness.ResetSessionsInMemory();
    harness.NewGame(1);
    harness.MutateFoundry();
    Assert.Equal(1, count);
}
```

### C.1.6 Verification

```bash
dotnet build Ashfall.csproj --no-restore           # 0 errors
bash scripts/run_test.sh Ashfall.Core.Tests/Foundry # or nearest suite
bash scripts/run_test.sh Ashfall.Core.Tests/Save    # restore path
godot --headless --path . -- --7day-smoke-selftest
```

Expected: all green; the harness shows B's truth after loading B.

### C.1.7 Debt update

`KNOWN_DEBT.md` gets a new row:

```markdown
| DEBT-D19C-STALE-HOST-CACHES | RETIRED | Host lifecycle reset | Sealed <date>:
| three setup-created fields (_silentFoundry/_sharedFactionStance/
| _sharedSkillProgression) now nulled in <method>; slot-switch harness + dispatch
| count + save suite green. Evidence: <package log>. | Integrator | Do not
| re-open without a new setup-created field escaping reset parity |
```

The integrator applies this row; the builder proposes it.

---

## C.2 Walkthrough 2 — Save/load edge fixtures (Point 3)

### C.2.1 Fixture construction

Each fixture is a directory under the test data root (or generated in the
harness) representing a slot:

| Fixture | Construction | Expected behaviour |
|---|---|---|
| `partial_slot` | copy a good slot, delete one section file | load succeeds with defaults for that section; no crash; message notes the default |
| `corrupt_section` | replace one section's JSON with `{invalid` | load fails cleanly; live state preserved |
| `checksum_mismatch` | flip one byte in a section payload | load refused; live state preserved |
| `legacy_envelope` | hand-written old-shape slot (or archived v1 sample) | migrates or refuses with a typed message |
| `missing_id` | good slot; delete one catalog row it references | decided behaviour (Point 10 contract) |
| `truncated_file` | cut a section file mid-JSON | same as corrupt |

### C.2.2 The atomicity question

The harness must assert the **live state after a failed load**:

```csharp
var before = harness.Fingerprint();          // campaign fingerprint
var result = harness.TryLoad(fixture);
if (!result.Success) Assert.Equal(before, harness.Fingerprint());
```

If a failed load currently leaves a half-restored campaign, that is an S1
finding; the plan's Path A fixes the order (verify-all-then-apply) or Path C
adds the transaction.

### C.2.3 Verify-all-then-apply pattern (if needed)

The typical minimal fix: read and validate all section payloads into memory
first; only then apply them to owners. `Main.SaveOrchestrator` already loads
through `_saveLoadHost.TryLoadSlot(...)` into a result object before applying
— verify whether application is staged; if yes, the atomicity is structural
and the test only needs to prove it.

### C.2.4 Save-store matrix sentinel test (Path B)

```csharp
[Fact]
public void EveryRegisteredSection_RoundTripsSentinel()
{
    foreach (var section in SaveSectionRegistry.All)
    {
        var owner = harness.OwnerFor(section.Key);
        var sentinel = harness.NewSentinelFor(section.Key);
        owner.Put(sentinel);
        harness.Save(); harness.Load();
        Assert.Equal(sentinel, owner.Get());
    }
}
```

Sections whose owner is runtime-only get an explicit exemption list, and the
exemption list is reviewable.

---

## C.3 Walkthrough 3 — Subscription inventory (Point 4)

### C.3.1 Inventory command

```bash
# every event subscription in host setup/code
grep -rn "+= *[A-Za-z_][A-Za-z0-9_.]*\|+= *On\|+= *(" src/Main*.cs | grep -v "Count\|index" | head -60
```

For each hit, find the matching `-=`:

```bash
grep -rn "\-= *<Handler>" src/Main*.cs
```

### C.3.2 Classification table (template)

| File:line | Subscription | Matching unsubscribe | Verdict |
|---|---|---|---|
| Main.Economy.cs:242 | `_silentFoundry.StateChanged += lambda` | none | convert to named (fix) |
| Main.Economy.cs (CloseTradePanel) | `-= _tradePanel.RefreshView` | present | safe |
| … | … | … | … |

### C.3.3 Dispatch-once test pattern

```csharp
[Fact]
public void RecreateSession_DispatchesExactlyOnce()
{
    var dispatches = 0;
    harness.OnSessionEvent += () => dispatches++;
    harness.CreateSession();
    harness.ResetSessionsInMemory();
    harness.CreateSession();
    harness.FireSessionEvent();
    Assert.Equal(1, dispatches);
}
```

If the pre-fix count is 2 (or the handler fires for a dead session), the
finding is confirmed.

---

## C.4 Walkthrough 4 — Silent catch conversion (Point 5)

### C.4.1 The worksheet → decision flow

```text
for each catch site:
  if body logs/rethrows/converts        -> leave (may need annotation per W2-01 gate)
  else if path is save/catalog/authority-> FIX (typed failure or diagnostics entry)
  else                                  -> ANNOTATE with reason
```

### C.4.2 Conversion example

```csharp
// before — silent swallow of a failed authority write
try { SaveHubEnvelope(); }
catch (Exception) { }

// after — typed/observable failure
try
{
    SaveHubEnvelope();
}
catch (Exception ex)
{
    CatalogDiagnostics.Record("expansion_hub_save_failed", ex);
    throw;   // or return a typed failure if the caller can present it
}
```

The choice between `throw` and typed return is decided by the caller's existing
contract; the plan requires the choice to be recorded in the package log. Never
convert a swallow into a crash for a path that currently tolerates failure and
recovers elsewhere — the goal is observability, not new failures.

### C.4.3 Error-path test

```csharp
[Fact]
public void HubSaveFailure_IsRecordedAndSurfaced()
{
    harness.MakeHubPathUnwritable();
    var result = harness.SaveCampaign();
    Assert.False(result.Success);
    Assert.Contains("expansion_hub_save_failed", harness.Diagnostics.RecentKeys());
    Assert.Equal(saveBefore, harness.Fingerprint()); // no partial write
}
```

---

## C.5 Walkthrough 5 — UI failure-path kit (Point 8)

### C.5.1 Kit shape

```csharp
public static class PanelResilienceKit
{
    public static IEnumerable<string> AllPanelIds() =>
        PlayerSurfaceManifest.Load().ExpandedIds;   // generated manifest

    public static void RunAll(PanelHarness h)
    {
        foreach (var id in AllPanelIds())
        {
            h.AssertSurvives(id, Context.FreshGame);
            h.AssertSurvives(id, Context.MidLoad);
            h.AssertSurvives(id, Context.AfterSlotSwitch);
            h.AssertSurvives(id, Context.NullPrerequisite);
            h.AssertCloseParity(id);
        }
    }
}
```

### C.5.2 Context definitions

| Context | Construction |
|---|---|
| FreshGame | full setup, day 1 |
| MidLoad | start load, pause before apply (hook), open panel |
| AfterSlotSwitch | load A, then B, open panel |
| NullPrerequisite | null the panel's owner field directly (test seam) |

`AssertSurvives` means: no exception; the panel is either populated or shows
an explicit unavailable message through the canonical feedback/label path; the
purity gate's no-fabricated-fallback rule is respected.

### C.5.3 Common failures and their minimal fixes

| Failure | Typical cause | Minimal fix |
|---|---|---|
| NRE on open | panel binds null owner | early-return with an "unavailable" label |
| Stale content | bind not refreshed after slot switch | rebind on the owner's reset event |
| Blank panel | no content and no message | explicit "not yet available" state |
| Esc does not close | close hook missing | add to the panel's close path (existing pattern) |

---

## C.6 Cross-repair verification matrix

| Walkthrough | Build | Focused tests | Selftests | Gates |
|---|---|---|---|---|
| C.1 stale caches | 0 errors | foundry + save + dispatch | 7day smoke | parity (if B) |
| C.2 save edges | 0 errors | edge theory + matrix | save-load-ui-failure | fast gates |
| C.3 subscriptions | 0 errors | dispatch-once | player-panels-uitest | hygiene |
| C.4 silent catch | 0 errors | error-path per site | — | catch-policy |
| C.5 UI kit | 0 errors | kit run | player-panels + a11y | purity/route |

---

## C.7 Ordering when several walkthroughs are in one package

1. C.1 first (S1/S3 correctness).
2. C.2 next (proves the restore path is sound after C.1).
3. C.3 and C.4 in either order.
4. C.5 last (it depends on stable lifecycle behavior).

If the package is limited to Path A, C.1 + whichever S1 edge exists is the
minimal shipping fix.

---

## C.8 What each walkthrough does NOT do

- Does not change save schema or section counts.
- Does not add new panels or features.
- Does not rebalance any value.
- Does not refactor the foundry, the save system, or the panel router.
- Does not run the full test suite.

---

*(Part C ends. Part D continues with determinism/reentrancy/data deep-dives,
the P0 template, and the verification command library.)*---

# PART D — DETERMINISM, REENTRANCY, AND DATA DEEP-DIVES

---

## D.1 Determinism audit method (Point 7)

### D.1.1 Seed-source census

```bash
grep -rn "new SeededRng(\|new XorShift\|ISeededRng\b.*=" Assets/Ashfall.Core src --include=*.cs \
  | grep -v "//" | head -80
```

For each construction, record:

| Site | Seed source | Stream fork | Deterministic? |
|---|---|---|---|
| `…/System.cs:NN` | `campaignSeed` | `ForkFor("system_id")` | yes |
| `…/Other.cs:NN` | `day` | none | yes (day-derived) |
| `…/Third.cs:NN` | `147` | none | constant fallback — check whether intended |
| `…/Host.cs:NN` | `Guid` / clock | — | **fail** (must not exist in Core) |

Known fallback patterns exist (a `new SeededRng(147)` fallback was previously
identified in the wave-1 reconnaissance). A constant fallback is acceptable
only if documented as intentional; otherwise it is an S3 finding (same outcomes
every campaign).

### D.1.2 The two-process equivalence test

Static state can make same-seed runs diverge **within one process** while fresh
processes agree. The test:

```csharp
[Fact]
public void SameSeed_TwoProcesses_IdenticalFingerprint()
{
    var a = RunIndependent("--simulate --seed 42 --days 30 --fingerprint");
    var b = RunIndependent("--simulate --seed 42 --days 30 --fingerprint");
    Assert.Equal(a, b);
}
```

`RunIndependent` launches the host CLI as a child process (or a fresh domain),
ensuring no shared statics. If the CLI has no such simulate verb, the harness
uses the existing headless demos or a new test-only entry — the point is the
process boundary, not the verb.

### D.1.3 Reload parity (in-process)

```csharp
[Fact]
public void ReloadParity_MidCampaign()
{
    var continuous = Sim.Run(seed: 99, days: 40);
    var split = Sim.Run(seed: 99, days: 20);
    split.Save(); split.Load();
    split.Run(days: 20);
    Assert.Equal(continuous.Fingerprint(), split.Fingerprint());
}
```

The fingerprint helper (Path C) makes this a one-liner for every system. Under
Path B, each new system from the audit gets its own narrower equality (compare
the system's own state, not the whole campaign).

### D.1.4 Fingerprint helper design (Path C)

```csharp
public static class CampaignFingerprint
{
    // Stable, order-independent, culture-invariant digest over registered owners.
    public static string Of(IReadOnlyList<IFingerprintSource> owners)
    {
        var sb = new StringBuilder();
        foreach (var o in owners.OrderBy(o => o.FingerprintKey, StringComparer.Ordinal))
            sb.Append(o.FingerprintKey).Append('=').Append(o.FingerprintValue()).Append(';');
        return StableHash.Of(sb.ToString()); // existing djb2/x33 stable hash pattern
    }
}
```

Rules: ordinal ordering, no `GetHashCode`, no culture formatting, no floats
formatted without invariant culture, no wall-clock. This mirrors the existing
save-checksum discipline and can reuse its helper once verified identical.

### D.1.5 What determinism repair never does

- Never changes a seed or a stream fork (that would alter existing saves'
  replay semantics).
- Never removes a documented constant fallback without a decision (a fallback
  may be intentional for previews).
- Never adds RNG to a system that currently has none (gameplay risk).

---

## D.2 Reentrancy audit (Point 9)

### D.2.1 Tick entry inventory

```bash
grep -rn "void Tick\|void AdvanceDay\|void OnDayAdvanced\|void ProcessDay" src/Main*.cs | head -60
```

Build the table:

| Entry point | Caller(s) | Guards | Dispatch key |
|---|---|---|---|
| `TickX(day)` | day owner | `if (day == _lastXDay) return` | day |
| `SetupY` | bootstrap | field non-null | — |
| `CaptureY` | save orchestrator | none | — |

The dangerous combination is an entry with no day guard called from two
sources (e.g., day owner and a panel command). The audit names those.

### D.2.2 Double-advance test

```csharp
[Fact]
public void DoubleDayAdvance_DispatchesOnce()
{
    var h = new HostHarness().NewGame(seed: 5);
    var counts = h.CountersForHighValueOwners();      // homeless, research, ...
    h.AdvanceDay();
    h.AdvanceDayWithoutBoundary();                     // simulate reentry/dup
    foreach (var (owner, count) in counts)
        Assert.Equal(count + 1, h.Count(owner));
}
```

### D.2.3 Save-during-tick

If any capture runs inside a tick, the audit checks whether iteration is safe
(collection modified exception potential). The minimal fix is to defer capture
to tick end (a `_captureRequested` flag consumed after the tick), a pattern
already common in the repo's dirty-flag design.

### D.2.4 Dirty-flag order audit

For each `_x` with a `_xDirty` flag:

1. Find all mutations of `_x`.
2. Assert each mutation is followed (or preceded) by `_xDirty = true`.
3. Find the capture path that clears the flag.
4. Assert capture happens after mutation completes.

A grep template:

```bash
grep -rn "_recreationDirty\|_chemWarfareDirty\|_commsArrayDirty\|_ceremonyDirty\|_roboticsDirty\|_foundryDirty" src/Main*.cs
```

Any mutation without a flag set is a candidate lost-write bug (S1 class if it
touches persisted state).

---

## D.3 Data-dependent lookup audit (Point 10)

### D.3.1 Lookup-site enumeration

```bash
# dictionary lookups with silent defaults in Core/host
grep -rn "TryGetValue(.*out var\|TryGetValue(.*out _\|?? *default\|?? *null" \
  Assets/Ashfall.Core src --include=*.cs | head -80
```

Classify each site by the id's origin:

| Origin | Rule |
|---|---|
| persisted in save | must have decided behaviour (Point 10 contract) |
| authored in current catalog | integrity pipeline's responsibility; runtime can assume (but see below) |
| runtime-only id | default is acceptable if documented |

### D.3.2 Removed-id fixture

```csharp
[Fact]
public void SaveReferencingRemovedCatalogRow_BehavesAsDecided()
{
    harness.WithSaveReferencingItem("removed_item_x");
    var result = harness.Load();
    // decided: skip-with-diagnostics, documented substitute, or refuse
    Assert.Equal(DecidedBehaviour, result.Outcome);
    Assert.False(harness.HasPhantomItem("removed_item_x"));
}
```

The decided behaviour is recorded in the package log; the default
recommendation is **skip + diagnostics + player-readable note**, because a
refusal on load is hostile to players with long saves, and a silent substitute
can duplicate items.

### D.3.3 Null loot tables (verified example)

`wasteland_map_v1.json` has nodes with `lootTable: null` (observed in the
sample, e.g. `loc_holdfast`). Runtime consumers must already handle null;
the audit confirms and adds a test:

```csharp
[Fact]
public void NodeWithNullLootTable_YieldsEmptyLootDeterministically()
{
    var loot = map.ResolveLoot("loc_holdfast", rng: Seeded(1));
    Assert.Empty(loot);
}
```

### D.3.4 What the contract does not cover

- Authoring typos (integrity pipeline).
- Content reachability (content-utilization gate).
- Balance of substitutes (W2-03 if player-visible).

---

## D.4 The P0 premise template for W2-02

```markdown
# P0 — W2-02 BUG PREMISE (<date>, HEAD <sha>)

## 1. D19c re-verification
- grep "= null" for the three fields: <output>
- setup guards: <file:line>
- reset methods and their nulled fields: <list>
- verdict: defect / fragile / clean

## 2. Reset coverage census (first pass)
- setup methods: <n>
- setup-created fields: <n>
- cleared by a reset: <n>
- gaps: <list with file:line>

## 3. Save edge inventory
- existing edge tests found: <list>
- uncovered edges: <list>

## 4. Subscription inventory
- setup subscriptions: <n>
- without matching unsubscribe: <list>

## 5. Silent catch worksheet (consume W2-01 or generate)
- total catch sites: <n>
- silent class: <n>
- authority-path silent: <list>

## 6. Determinism
- seed construction sites: <n>
- constant fallbacks: <list>
- replay coverage: <systems with tests / without>

## 7. Reentrancy
- tick entries: <n>
- guarded/unguarded: <counts>
- dirty flags audited: <n>

## Verdict: proceed / amend / block
```

---

## D.5 Verification command library

```bash
# build
dotnet build Ashfall.csproj --no-restore

# focused suites (choose the owning one)
bash scripts/run_test.sh Ashfall.Core.Tests/Save
bash scripts/run_test.sh Ashfall.Core.Tests/Foundry
bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions
bash scripts/run_test.sh Ashfall.Core.Tests/UI

# selftests
godot --headless --path . -- --7day-smoke-selftest
godot --headless --path . -- --save-load-ui-failure-selftest
godot --headless --path . -- --panel-bind-lifecycle-selftest
godot --headless --path . -- --player-panels-selftest

# gates
bash scripts/ci/catch-policy-gate.sh
bash scripts/ci/fast-gates.sh
python3 scripts/ci/generate-architecture-map.py --check
```

Rule: run the smallest set that covers the changed owner; add the smoke only
when lifecycle behavior changed (Point 1/2/9).

---

## D.6 Per-point evidence templates

### D.6.1 Point 1

```markdown
- Repro: slot-switch harness <path>; pre-fix read at step N = <value>, expected <value>
- Fix: <file:line> null assignments (+ named handler if B)
- Post-fix: harness step N = <value>; dispatch count = 1
- Regression: <suites> PASS
- Debt: DEBT-D19C-STALE-HOST-CACHES proposed RETIRED
```

### D.6.2 Point 2

```markdown
- Census: <n> setup fields; <n> cleared; <n> exempt with reasons; <n> gaps fixed
- Parity test: <path>; negative demonstration: added field fails
```

### D.6.3 Point 3

```markdown
- Edge fixtures: <list>; results before/after
- Atomicity: fingerprint preserved on failure — proven
- Matrix: <n> sections round-trip; exemptions <list>
```

### D.6.4 Point 4

```markdown
- Inventory: <n> subscriptions; <n> converted to named; dispatch-once count = 1
```

### D.6.5 Point 5

```markdown
- Silent sites: <n>; converted <n>; annotated <n>; each with reason
- Error-path tests: <list>
```

### D.6.6 Point 6

```markdown
- Audit table: <classes → sites → verdicts>
- Boundary tests: <list>; NaN/culture checks PASS
```

### D.6.7 Point 7

```markdown
- Seed census: <n> sites; fallbacks documented
- Replay parity: <n> systems; all identical
- No System.Random: grep empty
```

### D.6.8 Point 8

```markdown
- Kit: <n> panels × 4 contexts; failures <n>; fixed <n>
- Close parity: <n>/<n>
```

### D.6.9 Point 9

```markdown
- Tick entries: <n>; guards <n>; double-advance test PASS
- Dirty flags: <n> audited; order verified
```

### D.6.10 Point 10

```markdown
- Silent defaults: <n>; contract applied <n>
- Removed-id fixture: decided behaviour <X> asserted
```

---

## D.7 Failure taxonomy for the plan itself

| If during execution… | Then… |
|---|---|
| the repro does not fail | downgrade the finding to S4; add a defensive test; record the downgrade |
| the minimal fix changes visible behaviour | escalate to an A/B/C decision line in this plan |
| two points discover the same root cause | merge into one repair, reference both points |
| a point discovers an S1 in another plan's domain | stop; report; do not fix across ownership |
| a gate blocks the repair (e.g., catch-policy) | satisfy the gate as part of the repair; never bypass |
| a fix needs a schema change | stop; the change belongs to UNBLOCK-01/02 or a new signature |

---

## D.8 Anti-patterns explicitly rejected

1. **"Defensive null checks everywhere."** Adding null guards at every call
   site hides the lifecycle defect instead of fixing it.
2. **"Wrap it in try/catch."** Turns a defect into a silent failure — the
   exact class this plan eliminates.
3. **"Reset everything always."** Destroying panels/config on every reset
   breaks UI state and is not the contract.
4. **"Add a second owner to avoid the stale one."** Violates Rule 5 and creates
   the next stale-cache bug.
5. **"Fix it in the panel."** Panels are presentation; stale owners must be
   fixed at the owner/reset seam.
6. **"Run the full suite to be sure."** Policy violation; focused evidence is
   the standard.

---

*(Part D ends. Part E continues with scenarios, foreman Q&A, Annex U, and the
appendices.)*---

# PART E — SCENARIOS, Q&A, ANNEX U, AND CLOSEOUT

---

## E.1 Scenario 1 — Path A: the two-day verified repair

**Selection:** Plan Path A (all points A), Points 1 and 3 prioritized.

**Day 1:**

1. B0 premise: re-verify the three fields (grep), build the slot-switch
   harness, record the census first pass.
2. B1 Point 1 Path A: three null assignments + named-handler conversion; run
   the harness (B's truth after loading B).
3. If the harness does not fail, downgrade to S4: add the defensive test,
   record the downgrade honestly, and proceed with the named-handler
   improvement (which removes the anonymous-subscription fragility regardless).

**Day 2:**

4. Point 3 Path A: build the six save fixtures; run them; fix any S1 edge
   found (likely none, given the mature save system).
5. Regression: foundry/save suites, 7day smoke, fast gates.
6. Closeout: evidence block, proposed debt row, handoff.

**Outcome:** the known lifecycle defect is either fixed or formally downgraded
with a locking test; save edges are verified; no structural work.

---

## E.2 Scenario 2 — Path B: class elimination (the default)

**Week 1:**

1. B0 premise + harness.
2. B1: Point 1 Path B (nulls + named handler + reset parity inclusion), Point 3
   Path B (edge suite + store matrix).
3. Coordinate with W2-01 M3-B for the reset parser if it is executing.

**Week 2:**

4. B2: Point 4 (subscription named-handler rule + double-create tests), Point 8
   (panel kit), Point 10 (removed-id fixture + contract).
5. B3: Point 2 parity test, Point 5 conversion of authority-path swallows,
   Point 7 replay expansion, Point 9 guard + double-advance.
6. Point 6 Path A audit (clean; no fixes).

**Closeout:** gates (catch-policy, fast), evidence, debt rows, Annex U update.

**Outcome:** the lifecycle class cannot recur; silent failures in authority
paths are gone; UI failure paths are tested; determinism coverage is broader;
the debt ledger reflects repaired truth.

---

## E.3 Scenario 3 — Path C: hardening program

Path B for all defects, then separate signed packages:

1. `W2-02-C-LIFECYCLE-CONTRACT`: `IHostSessionLifecycle` for host sessions,
   reset ordering explicit, ad-hoc resets migrated incrementally.
2. `W2-02-C-TRANSACTIONAL-RESTORE`: verify-all-then-apply or rollback on
   mid-load failure.
3. `W2-02-C-FAILURE-ENVELOPE`: typed failure surface for authority paths.
4. `W2-02-C-FINGERPRINT`: campaign fingerprint harness feeding replay, slot
   switch, and save round-trip.
5. `W2-02-C-PANEL-STATE`: panel resilience state model.
6. `W2-02-C-TICK-TRANSACTION`: only if Path B finds real double-dispatch
   defects.

Each package is independently revertable and has its own evidence.

---

## E.4 Scenario 4 — a repro refuses to fail

**Situation:** the slot-switch harness reads the correct salt-mine value even
pre-fix, because `ExpansionHubSaveStore` capture occurs before any read and the
stale session was already overwritten during restore.

**Correct handling:**

1. Record the observation: "the stale-cache defect does not manifest for the
   salt-mine read path under current restore order."
2. Search for a **different** manifestation of the stale reference: the
   ventilation binding (`BindVentilation` direct reference), the panel bind
   (`_silentFoundryPanel.Bind`), or the stance engine held by
   `_economyPanel.BindStance`. Build the repro against whichever surface reads
   the stale owner's mutable state.
3. If no surface can be made to fail, the finding is S4 (fragile pattern) and
   the repair is *defensive hardening*: null the fields and add the locking
   test with a comment stating that the current restore order masks it. This is
   honest: a latent stale-reference bug is still a bug class worth closing.
4. Never fabricate a failing scenario to justify the fix.

---

## E.5 Scenario 5 — a fix interacts with the difficulty envelope

**Situation:** a save edge test fails because the load path validates a
difficulty envelope (E1 `ValidateDifficultyEnvelope` in
`Main.SaveOrchestrator`) and the legacy fixture lacks the field.

**Correct handling:**

1. Determine whether the envelope is required for new saves only or all saves.
2. If legacy saves legitimately lack it, the fixture must include the legacy
   shape and the expected outcome is the documented default — not a crash.
3. If the envelope is required and legacy saves genuinely cannot load, that is
   a product decision, not a bug fix: escalate with the A/B/C decision
   (refuse with a clear message / migrate with the default / accept with a
   warning), record the chosen behaviour, and implement it with a test.
4. Do not silently relax validation to make the test pass.

---

## E.6 Foreman Q&A

**Q1. Is D19c definitely a bug?**
It is definitely a stale-reference pattern with a plausible S1 manifestation
(the salt-mine restore path). Whether it currently manifests visibly is what
the harness decides. Either way the pattern is worth closing, because the reset
comment explicitly promises it cannot happen.

**Q2. Why not fix it in the foundry instead of Main?**
The lifecycle boundary is the owner of the defect: the foundry is told to
create-once, and Main forgets to reset. Fixing it in the foundry would add a
second lifecycle authority — Rule 5 violation.

**Q3. Why does the plan insist on the named handler if nulling fixes it?**
Because nulling is a one-line contract that future code can accidentally break,
while a symmetric subscribe/unsubscribe is self-enforcing. Path B buys
durability for one small edit.

**Q4. Are there other stale caches besides the three?**
The Point 2 census answers that with evidence. The plan deliberately starts
with the three proven ones and expands only through the census.

**Q5. Will the catch conversion turn warnings into crashes?**
No. It converts *silence* into *observability*; whether the caller then throws
or returns a typed failure is decided by its existing contract and recorded.

**Q6. Is Point 6 (numeric) worth Path B?**
Not by default. The audit found no demonstrated numeric defect; the plan
recommends Path A and reserves B for aggregates where the audit finds a
guardless division.

**Q7. How does this plan avoid racing W2-01's extraction?**
Single-writer claims. If W2-01 holds `Main.Lifecycle.cs`, W2-02 waits or
coordinates; the two plans explicitly share the reset-coverage artifact.

**Q8. What if the save system turns out to have a real S1 edge?**
It takes priority over everything else in the package (B1), gets its own
evidence, and may justify pulling the fix into a standalone hotfix package if
it affects live saves.

**Q9. What is the smallest useful approval?**
Point 1 Path A: a repro harness plus the null assignments (and a downgrade
if the repro doesn't fail).

**Q10. What is the largest?**
Plan Path C, which is really five or six separate signed packages — the plan
says so explicitly.

**Q11. Does this plan change any gameplay?**
Only if a defect's fix is behaviorally visible; such cases are forced through
an A/B/C decision and recorded. Silent-wrong → correct is the entire point.

**Q12. How do we know we're done?**
Every point has a repro (or a documented downgrade), every repair has a
regression, every class has a gate or an explicit debt row.

---

## E.7 Decision records (template + worked example)

```markdown
### DR-W2-02-1 — stale host caches
- Chosen: Path B (null + named handler + parity inclusion)
- Evidence: three fields never nulled (grep empty); guard early-return
  Main.Economy.cs:212; reset nulls dependencies Main.Lifecycle.cs:73/110/161/231
- Scope: lifecycle reset for the three fields; named handler; parity assertion
- Not authorized: lifecycle contract (C), save semantics changes
- Revisit: if census finds additional setup-created fields escaping reset
```

---

## E.8 Annex U — Plan-unblocking (deliberately separate)

> **Wave 2 rule:** this annex is the bug plan's unblocking component, kept
> separate from the repair body so integration and release decisions are never
> conflated. Nothing here authorizes another plan without the signatures in
> U.2.

### U.1 What W2-02 releases

| Blocked item | Release mechanism | Gate |
|---|---|---|
| `DEBT-D19C`-family row (new) | Point 1 repair + repro | B1 |
| `DEBT-AMPUTATION-EQUIPMENT-RESTRICTION` residual risk | Point 2 census proves no other equipment-field stale path; the schema half stays UNBLOCK-01 | B3 |
| UNBLOCK-04's "reset hygiene" statements | Point 2 parity makes the statement checkable | B3 |
| W2-01 M3-B reset-coverage comments | The repair provides the real field list | B1/B3 |
| W2-03 gameplay safety | Repairs remove wrong-value defects before tuning | all |
| W2-05 location runtime lookups | Point 10 contract protects location ids in saves | B2 |
| W2-06 panel presentation | Point 8 kit protects new prose surfaces | B2 |
| EN-08 ledger truth | Repaired defects update debt rows truthfully | closeout |

### U.2 Signatures needed

```text
[ ] I authorize Point 1 repair (Path A nulls / Path B + handler + parity).
[ ] I authorize Point 2 census + parity test (required for Path B).
[ ] I authorize Point 3 save edge fixtures and any S1 fix they reveal.
[ ] I authorize Point 4 named-handler conversions.
[ ] I authorize Point 5 conversions on save/catalog/authority paths.
[ ] I authorize Point 8 panel resilience kit.
[ ] I authorize Point 10 lookup contract and removed-id fixture.
[ ] I authorize Path C hardening packages separately (list each).
```

### U.3 What W2-02 never touches for unblocking

- Save schema additions (UNBLOCK-01 F14 / UNBLOOCK-02 F13).
- Semantic-kind routing (UNBLOCK-03 D11).
- String freeze (UNBLOCK-03 D22).
- Census/register truth (UNBLOCK-04).
- Expansion admission (UNBLOCK-05).
- Gameplay/environment/location/enrichment **content** (W2-03/04/05/06).

### U.4 The no-silent-release rule

A repair that also happens to resolve a blocked item does **not** release it
automatically. The release is recorded here only when the U.2 signature exists
and the evidence passes. This mirrors the maintenance rule (W2-01 U.4): release
is explicit, never inferred.

---

## E.9 Appendix A — Selection sheet

```text
ASHFALL WAVE 2 · PLAN 2 (BUGS) · SELECTION
Date: __________   Foreman: __________   HEAD: __________

PLAN PATH: [ ] A Verified Repair  [ ] B Class Elimination (default)  [ ] C Hardening

01 stale host caches ...... [A] [B] [C]   default B
02 reset coverage parity .. [A] [B] [C]   default B
03 save/load edges ........ [A] [B] [C]   default B
04 subscriptions .......... [A] [B] [C]   default B
05 silent catches ......... [A] [B] [C]   default B
06 numeric edges .......... [A] [B] [C]   default A
07 determinism ............ [A] [B] [C]   default B
08 UI failure paths ....... [A] [B] [C]   default B
09 reentrancy ............. [A] [B] [C]   default B
10 data-dependent ......... [A] [B] [C]   default B

Signature: ______________________
```

## E.10 Appendix B — Severity decision table (for triage during execution)

| Observation | Severity | Response |
|---|---|---|
| save write lost / wrong state persisted | S1 | fix now, own evidence |
| player-visible wrong number | S2 | fix in package |
| stale owner read internally | S3 | fix + parity/lifecycle test |
| unsafe pattern, no repro | S4 | gate or annotation; no rewrite |
| log/format only | S5 | route to W2-01 |

## E.11 Appendix C — Debt-row proposal templates

```markdown
| DEBT-D19C-STALE-HOST-CACHES | RETIRED | Host lifecycle | Sealed <date>: three
| fields nulled in <method>; slot-switch harness + dispatch-once + save suite
| green. Evidence: <log>. | Integrator | Do not re-open without new
| setup-created fields escaping parity |

| DEBT-W2-02-SILENT-AUTHORITY-PATHS | RETIRED | Error handling | Sealed <date>:
| <n> authority-path swallows converted to typed/diagnosed failures; catch
| policy gate enforces no silent path. Evidence: <log>. | Integrator | Do not
| re-open; new silent authority catches are gate failures |
```

## E.12 Appendix D — Glossary

| Term | Meaning |
|---|---|
| stale host cache | a field created once whose dependencies were reset |
| setup-created field | a host field assigned inside a `Setup*` method |
| dispatch-once | exactly one handler invocation per event after re-creation |
| S1–S5 | severity classes in §0.4 |
| repro standard | test / harness / scripted sequence that fails pre-fix |
| fingerprint | stable digest of campaign state for parity comparisons |
| removed-id fixture | a save referencing a deleted catalog row |
| decided behaviour | the signed choice for a missing-row lookup |

## E.13 Appendix E — The one-page summary

- **What:** ten bug/silent-failure points, each with A/B/C; the flagship is
  the verified stale host cache (`_silentFoundry` family).
- **Choose:** Plan Path (default B) + ten point paths.
- **Recommended first:** Point 1 Path B, Point 3 Path B; Point 6 Path A.
- **Smallest useful:** Point 1 Path A repro + fix.
- **Largest:** Path C = several separately signed hardening packages.
- **Never:** schema changes, balance changes, full-suite safety runs, silent
  rewrites without repros.
- **Unblocking:** Annex U, signed and separate.

---

**End of W2-02 (Bug & Silent-Failure Repair Integration Plan).**
Proposal only; executes nothing; releases nothing without U.2 signatures.

*Document control: W2-02 · Wave 2 · HEAD 5be1a30a · companion to W2-01 and
W2-03…W2-06.*---

# PART F — THE SILENT-FAILURE ATLAS

This atlas catalogues the silent-failure classes that exist in a repository of
this shape, records what is known about each in ASHFALL today, and states how
each Wave-2 path treats it. It exists so a builder can recognise a class
quickly instead of rediscovering it per instance.

---

## F.1 Class SF-1 — Guarded early-return retaining state

**Shape:** a method guards on a non-null field and returns; the field is never
reset; dependencies are reset.

**Verified instance:** `SetupSilentFoundry` (`Main.Economy.cs:210–212`) and the
`Ensure*` owners (`Main.CampaignServices.cs:173`, `:195`).

**Why it is silent:** no exception, no log, correct behaviour on the first
campaign; the failure only appears when the guard's field outlives its
dependencies.

**Detection:** reset-coverage census (Point 2); slot-switch harness (Point 1).

**Treatment:** Path A null; Path B parity test; Path C lifecycle contract.

**Where else to look:** every `if (_x != null) return;` in a `Setup*` or
`Ensure*` method, cross-referenced against reset lists. The census is
authoritative; the plan expects a small number beyond the three proven.

---

## F.2 Class SF-2 — Event subscription without an unsubscribe

**Shape:** `source.Event += handler` in setup, no matching `-=`.

**Verified instance:** the anonymous lambda in `SetupSilentFoundry`
(`StateChanged += () => {...}`).

**Why silent:** the handler dies with the source in the common case; only a
re-creation (or a source outliving the compiling object) produces double
dispatch or a leak.

**Detection:** subscription inventory (Point 4); dispatch-once test.

**Treatment:** named handler + symmetric unsubscribe; the host-level
subscription rule in Path B.

**Where else to look:** every `+=` in `Main*.cs` setup methods. UI panels are
already covered by existing hygiene gates; host sessions are the gap.

---

## F.3 Class SF-3 — Swallowed exception on an authority path

**Shape:** `catch (Exception) { }` or a catch that returns without a signal on
a path that writes state.

**Verified instance:** the class is historically acknowledged — `CatalogDiagnostics`
documents that branches "used bare catch { } (silent swallow)"; current bare
catches are gone, but catch-with-no-signal remains possible in principle.

**Why silent:** the caller believes the write succeeded; state diverges from
expectation without a trace.

**Detection:** the W2-01 worksheet + classification; grep of authority paths.

**Treatment:** typed failure or diagnostics record; error-path test.

**Where else to look:** save write paths, catalog loads that feed authority,
event dispatch, and file access helpers.

---

## F.4 Class SF-4 — Defaulted lookup for a persisted id

**Shape:** `dict.TryGetValue(id, out var v);` and then v is used as if found, or
`?? default` silently substitutes.

**Verified instance:** the class is structurally present wherever saves store
ids; the audit (Point 10) enumerates actual sites.

**Why silent:** the default looks like real data; nothing surfaces the missing
row.

**Detection:** lookup-site enumeration; removed-id fixture.

**Treatment:** decided behaviour per lookup; test with a removed row.

**Where else to look:** item, quest, faction, location, and affliction id
lookups on the load/display path.

---

## F.5 Class SF-5 — Null-owner UI bind

**Shape:** a panel binds an owner that setup has not created yet (load ordering)
or has just reset (slot switch).

**Verified instance:** the class is mitigated by existing routing/coverage
gates, but the mid-load window is the residual.

**Why silent:** in the best case the panel is empty; in the worst it throws in
a UI callback where the engine swallows or logs it.

**Detection:** panel resilience kit (Point 8); the four contexts.

**Treatment:** explicit unavailable state via the canonical feedback path (no
fabricated fallback content).

**Where else to look:** every panel opened by the router during load; the
surface manifest generates the list.

---

## F.6 Class SF-6 — Reentrant or double-dispatched tick

**Shape:** a day handler invoked twice (save boundary, duplicate subscription,
panel command) or a capture invoked inside a tick.

**Verified instance:** no demonstrated reentrancy; the centralized day owner
reduces risk. The class is guarded by the audit.

**Why silent:** effects compound invisibly (double research progress, double
consumption) or a collection-mutation exception is caught upstream.

**Detection:** tick inventory; double-advance test; dirty-flag order audit.

**Treatment:** day/dedupe key (the repository's journal pattern) or tick-end
capture; only hardened under Path C.

---

## F.7 Class SF-7 — Static state leaking between runs/tests

**Shape:** a static mutable field influences outcomes; two runs differ despite
the same seed; tests fail only in some orders.

**Verified instance:** a `TradeSpecialtySystem` isolation flake was fixed
test-side (D1); 61 static initializations exist in Core.

**Why silent:** each run looks plausible in isolation.

**Detection:** W2-01 isolation gate; Point 7 two-process/reload parity.

**Treatment:** reset hook or annotated cache; parity tests.

---

## F.8 Class SF-8 — Culture-dependent persisted formatting

**Shape:** a numeric/date formatted with the current culture reaching a save or
checksum.

**Verified instance:** save checksum is `InvariantCulture` (verified lines
1222/1290); the class is otherwise guarded by the same discipline.

**Why silent:** differs only on non-invariant-culture machines.

**Detection:** audit of `ToString`/`string.Format` on persisted paths; culture
variation run (set locale, run save round-trip).

**Treatment:** invariant formatting everywhere on persisted values; a test with
a hostile culture.

---

## F.9 Class SF-9 — Partial restore / hybrid campaign

**Shape:** a load fails (or a section is missing) after some owners have been
restored; live state is neither the old campaign nor the new one.

**Verified instance:** the load path is staged through a result object and
`TryLoadAndRestoreGame` preserves live state on refusal; whether mid-apply
failure is possible is verified by the edge suite (Point 3).

**Why silent:** the game continues; some values are new, some old.

**Detection:** save edge fixtures; fingerprint-before/after assertion.

**Treatment:** verify-all-then-apply (Path B verification) or transactional
restore (Path C).

---

## F.10 Class SF-10 — Dirty flag set/cleared out of order

**Shape:** a mutation occurs without setting its dirty flag, or the flag is
cleared before the write completes; the change is silently not persisted.

**Verified instance:** the repository uses the dirty-flag pattern extensively
(`_recreationDirty`, `_chemWarfareDirty`, `_commsArrayDirty`, `_ceremonyDirty`,
`_roboticsDirty`, `_foundryDirty`, …); the audit verifies order for each.

**Why silent:** the change works in-session, disappears after reload.

**Detection:** dirty-flag order audit (Point 9); capture-mutation tests.

**Treatment:** flag set adjacent to mutation; capture-after-tick; a test per
audited owner.

---

## F.11 Class SF-11 — Missing/renamed catalog row used by a formula

**Shape:** an id exists in code but not in data (or vice versa); a code path
returns zero silently.

**Verified instance:** the data-integrity and content-utilization gates cover
authoring; the runtime formula class is covered by the lookup audit.

**Why silent:** zero contribution looks like balanced design.

**Detection:** lookup audit; content-utilization selftest; removed-id fixture.

**Treatment:** decided behaviour + test; authoring defects route to W2-01.

---

## F.12 Class SF-12 — Handler registered but never invoked (dead callback)

**Shape:** an event exists, a handler is registered, but the emitter never
fires (or fires with a different signature/stream).

**Verified instance:** the repository's history includes zero-consumer
projections (the D2 deletion) and producer-wiring debts (Plan 194 producers)
— the class is real and was partially repaired in earlier waves.

**Why silent:** the feature simply never happens.

**Detection:** producer/consumer matrix over events; a selftest that asserts
each registered handler fires at least once in a scripted campaign.

**Treatment:** wire the producer or delete the handler; a "producer census"
gated by a test is the Path B form.

**Where to look:** radio/journal/presentation events, panel refresh hooks,
newly added Core events without a host subscription.

---

## F.13 Cross-class interaction matrix

| Class | Interacts with | Compound failure |
|---|---|---|
| SF-1 | SF-2 | stale session keeps a stale subscription |
| SF-1 | SF-10 | stale session writes with an old dirty flag |
| SF-3 | SF-10 | swallowed write error + flag cleared = silent data loss |
| SF-4 | SF-9 | missing row + partial restore = phantom state |
| SF-6 | SF-10 | double tick + flag race = double-count persisted once |
| SF-7 | SF-6 | static state changes double-dispatch outcomes |
| SF-5 | SF-2 | panel binds null owner and leaves a dangling handler |

The plan orders its points to break these compounds: lifecycle first (SF-1/2),
then persistence classes (SF-9/10), then error/observability (SF-3), then
lookup/data (SF-4/11), then UI (SF-5).

---

## F.14 How each class maps to a decision point

| Class | Primary point | Secondary |
|---|---|---|
| SF-1 | 1 | 2 |
| SF-2 | 4 | 1 |
| SF-3 | 5 | 3 |
| SF-4 | 10 | 3 |
| SF-5 | 8 | 4 |
| SF-6 | 9 | 7 |
| SF-7 | 7 | W2-01 §6 |
| SF-8 | 6 | 3 |
| SF-9 | 3 | 1 |
| SF-10 | 9 | 3 |
| SF-11 | 10 | 6 |
| SF-12 | 10 | 4 |

---

## F.15 The atlas in daily use

When a builder encounters a bug report, the triage is:

1. Name the class (SF-N) from the shape.
2. Find the instance via the class's detection method.
3. Apply the treatment consistent with the chosen plan path.
4. Record the instance in the class table (the atlas grows evidence, not
   prose).

This turns each new bug into an instance of a known family rather than a
one-off story — and families are what gates can eliminate.

---

## F.16 Atlas maintenance

The atlas is a living annex of this plan. When the bug plan closes, the
integrator records per class:

```text
SF-1: instances 3 → fixed 3 → class gate: parity test (active)
SF-2: instances 1 → fixed 1 → class rule: named-handler (active)
SF-3: instances N → converted N → class gate: catch-policy (active)
SF-4: sites audited N → contract applied N → fixture added
SF-7: statics audited N → hooks N → gate active
```

Classes with zero discovered instances in a clean audit are recorded as "clean
at this sweep" — which is itself valuable, because it dates the state.

---

# PART G — HARNESS AND TEST LIBRARY

---

## G.1 The slot-switch harness (Point 1)

```csharp
public sealed class SlotSwitchHarness
{
    private readonly HostHarness _host = new();

    public void CreateCampaign(int seed, int saltMineUnits, string slot)
    {
        _host.NewGame(seed);
        _host.Foundry.SaltMine.Extract(saltMineUnits);
        _host.Save(slot);
    }

    public (int units, string fingerprint) ReadFoundry()
        => (_host.ReadSaltMineThroughPublicApi(), _host.Fingerprint());

    public LoadResult SwitchTo(string slot) => _host.Load(slot);
}

[Fact]
public void SwitchingSlots_AppliesTargetTruth()
{
    var h = new SlotSwitchHarness();
    h.CreateCampaign(seed: 1, saltMineUnits: 10, slot: "A");
    h.CreateCampaign(seed: 2, saltMineUnits: 3, slot: "B");
    h.SwitchTo("A");
    Assert.Equal(10, h.ReadFoundry().units);
    h.SwitchTo("B");
    Assert.Equal(3, h.ReadFoundry().units);
}
```

If `ReadSaltMineThroughPublicApi` does not exist, the harness reads through
the foundry panel's bound model or a narrow test seam — never by reflecting
private fields (which would couple the test to internals).

---

## G.2 The save edge theory (Point 3)

```csharp
public static class SaveEdgeFixtures
{
    public static string Build(string kind) => kind switch
    {
        "partial_slot"      => RemoveOneSection(GoodSlot()),
        "corrupt_section"   => CorruptOneSection(GoodSlot()),
        "checksum_mismatch" => FlipOneByte(GoodSlot()),
        "legacy_envelope"   => LoadArchivedLegacySlot(),
        "truncated_file"    => TruncateOneSection(GoodSlot()),
        "missing_id"        => DeleteReferencedRow(GoodSlot()),
        _ => throw new ArgumentOutOfRangeException(nameof(kind)),
    };
}

[Theory]
[InlineData("partial_slot", LoadOutcome.SuccessWithDefaults)]
[InlineData("corrupt_section", LoadOutcome.RefusedPreservingLiveState)]
[InlineData("checksum_mismatch", LoadOutcome.RefusedPreservingLiveState)]
[InlineData("legacy_envelope", LoadOutcome.MigratedOrTypedRefusal)]
[InlineData("truncated_file", LoadOutcome.RefusedPreservingLiveState)]
[InlineData("missing_id", LoadOutcome.DecidedByLookupContract)]
public void LoadEdge_MatchesDecision(string kind, LoadOutcome expected)
{
    var h = new SaveHarness().WithLiveCampaign(seed: 11);
    var before = h.Fingerprint();
    var result = h.TryLoadFixture(SaveEdgeFixtures.Build(kind));
    Assert.Equal(expected, result.Outcome);
    if (result.Outcome != LoadOutcome.SuccessWithDefaults)
        Assert.Equal(before, h.Fingerprint());
}
```

The `LoadOutcome` enum is the plan's recommendation for a typed surface; the
existing result object may already carry equivalent information — verify at P0
and reuse rather than adding a parallel enum (Rule 5).

---

## G.3 The dispatch-once harness (Point 4)

```csharp
[Fact]
public void RecreatedSession_DispatchesOnce()
{
    var h = new HostHarness().NewGame(seed: 3);
    var dispatches = 0;
    h.ObserveFoundryStateChanges(() => dispatches++);
    h.ResetSessionsInMemory();
    h.NewGame(seed: 3);
    h.MutateFoundry();          // one state change
    Assert.Equal(1, dispatches);
}
```

For subscriptions that cannot be observed from outside, the harness drives the
owning operation (extract salt, trade, stance read) and counts panel refresh
invocations through a test double — never by reflection.

---

## G.4 The panel resilience kit (Point 8)

```csharp
public sealed class PanelResilienceKit
{
    public static readonly string[] Contexts =
        { "FreshGame", "MidLoad", "AfterSlotSwitch", "NullPrerequisite" };

    public static KitReport Run(PanelHarness h)
    {
        var report = new KitReport();
        foreach (var id in h.SurfaceManifest.ExpandedIds)
        foreach (var ctx in Contexts)
            report.Record(id, ctx, h.OpenAndObserve(id, ctx));
        foreach (var id in h.SurfaceManifest.ExpandedIds)
            report.Record(id, "CloseParity", h.CloseAndObserve(id));
        return report;
    }
}
```

The report table is the evidence: one row per (panel, context) with outcome
`Populated | UnavailableMessage | Exception`. Any `Exception` is a failure; any
blank-without-message is a failure under the production-UI purity rules.

---

## G.5 The removed-id fixture (Point 10)

```csharp
[Fact]
public void PersistedLookup_RemovedRow_DecidedBehaviour()
{
    var h = new SaveHarness().WithSaveReferencing("removed_item_x");
    var result = h.Load();
    Assert.Equal(DecidedBehaviour.SkipWithDiagnostics, result.LookupOutcome);
    Assert.Contains("removed_item_x", h.Diagnostics.RecentKeys());
    Assert.DoesNotContain("removed_item_x", h.LiveItemIds());
}
```

---

## G.6 The reentrancy guard (Point 9)

```csharp
// debug-only reentrancy detection inside the day owner
private bool _inDayTick;
private void TickAll(float gameHours)
{
    if (_inDayTick)
    {
        GD.PushWarning("[Ashfall] reentrant day tick detected");
        return; // or assert in tests
    }
    _inDayTick = true;
    try { /* existing body */ }
    finally { _inDayTick = false; }
}
```

Test form:

```csharp
[Fact]
public void ReentrantDayTick_IsDetectedAndIgnored()
{
    var h = new HostHarness().NewGame(seed: 4);
    h.EnterDayTickHook(() => h.TryAdvanceDay());   // triggers the guard
    Assert.True(h.LastTickWasWarned);
    Assert.Equal(1, h.DayAdvanceCount);
}
```

---

## G.7 Harness placement rules

| Harness | Location | Why |
|---|---|---|
| slot switch | test project (host integration) | exercises host lifecycle |
| save edges | test project (Save family) | uses file fixtures |
| dispatch-once | test project (Foundry/host) | drives host sessions |
| panel kit | test project (UI) + a selftest wrapper | reuse existing selftests |
| removed-id | test project (Save) | fixture-driven |
| reentrancy | host guard + test | production guard is debug-only |

Harnesses must not: reach into private fields, depend on wall-clock, depend on
file-system layout outside test fixtures, or run the full game loop beyond the
bounded days needed.

---

## G.8 The verification trace walkthrough (for the evidence pack)

```text
[W2-02 B0] HEAD=5be1a30a; fields grep — 0 null assignments (confirmed)
[W2-02 B0] reset census: setup methods 40; fields 122; cleared 118; gaps 3 (+3 dirty flags)
[W2-02 B0] save edges: 6 fixtures built; 5 pre-existing behaviours matched; 1 finding (legacy envelope message)
[W2-02 B1] slot-switch harness: pre-fix step 4 read = 10 (expected 3)  ← repro
[W2-02 B1] fix: 3 null assignments + named handler
[W2-02 B1] post-fix step 4 read = 3; dispatch count = 1
[W2-02 B1] suites: Foundry N/N; Save N/N; smoke 10/10
[W2-02 B2] subscriptions: 1 converted; dispatch-once PASS
[W2-02 B2] panel kit: 69 panels × 4 contexts: exceptions 2 → fixed 2; blank 1 → message added
[W2-02 B2] removed-id: decided skip+diagnostics; fixture PASS
[W2-02 B3] parity: includes 3 fields; negative demonstration PASS
[W2-02 B3] catch conversion: authority silent N→0; error-path tests N
[W2-02 B3] determinism: 4 replay tests added; all identical; grep clean
[W2-02 B3] reentrancy: guard installed; double-advance PASS
[W2-02 B5] debt row proposed; Annex U updated
```

---

## G.9 Closeout checklist

```text
[ ] Every point: repro (or documented downgrade)
[ ] Every repair: regression test
[ ] Every class: gate or explicit debt row
[ ] S1s fixed before S2/S3 in the same package
[ ] Save round-trip for any state fix
[ ] Smoke for lifecycle fix
[ ] Catch-policy + fast gates PASS
[ ] Evidence blocks per point
[ ] Debt rows proposed (not applied)
[ ] Annex U signatures status recorded
[ ] Handoff per AI_AGENT_WORKFLOW.md
```

---

## G.10 What closes a "silent" class permanently

A silent failure is closed only when one of these exists:

1. a typed failure the caller must handle (compile-level),
2. a diagnostics record the selftest asserts on (test-level),
3. a gate that fails the build (policy-level), or
4. an annotation with a reason that a reviewer reads (documentation-level,
   weakest).

The plan prefers 1–3 and accepts 4 only for genuinely optional paths.

---

**End of the silent-failure atlas and harness library.**

*(Appendices and the final document control follow in the next part.)*---

# PART H — COORDINATION, VERIFICATION DEPTH, AND FINAL APPENDICES

---

## H.1 Multi-plan coordination map (full)

| Interface | W2-02 side | Other plan side | Order |
|---|---|---|---|
| reset parser | Point 2 consumes or builds | W2-01 M3-B builds/owns | W2-01 first if both active |
| `Main.Lifecycle.cs` | Points 1/2 edits | W2-01 M3-B extraction | single-writer; sequence W2-01 then W2-02, or W2-02 then W2-01 rebases |
| catch worksheet | Point 5 repairs | W2-01 M6 measures | W2-01 measures, W2-02 repairs |
| diagnostics keys | Point 5/3 records | W2-01 M6 facade (C) | keys compatible; facade optional |
| noise baseline | Point 5 conversions affect | W2-01 M6 baseline | re-baseline after conversions if C |
| location lookups | Point 10 contract | W2-05 semantics | W2-02 protects runtime; W2-05 authors data |
| panel kit | Point 8 | W2-06 presentation | kit gates structure; W2-06 adds content |
| gameplay values | none (defect-only) | W2-03 tuning | W2-02 never tunes |
| weather runtime | reports findings | W2-04 design | behavioural design stays W2-04 |
| save schema | none | UNBLOCK-01/02 | schema changes never here |

### H.1.1 The single-writer rule in practice

The repository's orchestration files are the highest-collision class. The
policy:

1. Before editing `src/Main.Lifecycle.cs`, `src/Main.CampaignServices.cs`,
   `src/Main.Economy.cs`, or `src/Main.SaveOrchestrator.cs`, check the claim
   ledger.
2. If another Wave-2 plan holds the file, wait; do not "merge carefully".
3. If the other plan finishes first, rebase your change and re-run your repros
   (the fix may already exist).
4. Record the rebase in the package log.

This is slower and safer than concurrent edits of load-bearing setup order.

---

## H.2 Verification depth per path

### H.2.1 Path A verification

- One repro, one regression, one focused suite per point.
- Smoke only for Point 1.
- No gates added.

### H.2.2 Path B verification

- Path A, plus a gate or class rule per point, with a negative demonstration.
- Save store matrix and panel kit as reusable harnesses.
- Catch-policy and fast gates mandatory at closeout.

### H.2.3 Path C verification

- Path B, plus each hardening package's own trilogy (fresh/load/replay),
  fingerprint equality, and a written contract doc.
- Lifecycle contract: round-trip through two full campaign cycles.

---

## H.3 Mutation testing note (optional strengthening)

For the gate-backed points (2, 4, 5, 7), a light mutation check strengthens the
claim that the gate bites:

```text
Mutation A: re-introduce the stale field (do not null it) → parity test must fail
Mutation B: remove the named-handler unsubscribe → dispatch-once must fail
Mutation C: blank an INTENTIONAL-IGNORE reason → catch-policy must fail
Mutation D: fork a seed from a clock → seed-source check must fail
```

Each mutation is applied locally, observed to fail, then reverted. The results
are evidence that the gates are not decorative. This is a ten-minute exercise
per gate and converts "we added a gate" into "we proved the gate works".

---

## H.4 Adversarial review questions (answer before closeout)

1. If the slot-switch harness passes pre-fix, is the finding honestly
   downgraded — or did we pick a read path that happens to be self-healing?
2. Does the named-handler conversion change the order of panel refreshes
   relative to the dirty flag? (It must not.)
3. Does nulling `_silentFoundry` cause a second `BindVentilation` on an already
   bound ventilation owner? (Check the bind's idempotence.)
4. Do the save edge fixtures leak into production test data paths?
5. Does the catch conversion change any player-visible message?
6. Does the panel kit open panels in a state that production would never
   reach? (If yes, the "no exception" requirement still holds, but
   availability assertions must not.)
7. Does the determinism parity test depend on a fingerprinted float formatted
   with the current culture? (Must be invariant.)
8. Does the removed-id contract interact with the difficulty envelope
   validation (the E5 scenario)?
9. Does the reentrancy guard alter timing enough to change a tick outcome?
   (It must not; it is a boolean check.)
10. Is any fix "while I'm here" scope? (Revert it unless it is the defect.)

---

## H.5 Failure of the plan itself (kill criteria)

The bug plan should be paused if any of these is true:

- The repro cannot be made to fail and no alternative manifestation exists
  (downgrade, do not force).
- Two points conflict on the same file and neither can yield (escalate to the
  integrator for sequencing).
- A fix would require a schema change (move to UNBLOCK-01/02 or a new
  signature).
- The evidence shows the defect is actually in another plan's domain (hand off
  with the evidence).
- The change budget exceeds the package scope (split; one point per package).

---

## H.6 Evidence pack contents

1. `P0_BUG_PREMISE.md` (B0).
2. Per-point evidence blocks (D.6 templates).
3. Harness sources (or links) and their pre/post outputs.
4. Suite and selftest logs.
5. Gate outputs (catch-policy, fast).
6. Debt-row proposals.
7. Annex U signature status.
8. Limitations and downgrades.

The pack is what the integrator reads to decide acceptance — not the diff
alone.

---

## H.7 Appendices

### H.7.1 Appendix — Command reference (condensed)

```bash
# build + suites
dotnet build Ashfall.csproj --no-restore
bash scripts/run_test.sh Ashfall.Core.Tests/Save
bash scripts/run_test.sh Ashfall.Core.Tests/Foundry
bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions

# selftests
godot --headless --path . -- --7day-smoke-selftest
godot --headless --path . -- --save-load-ui-failure-selftest
godot --headless --path . -- --panel-bind-lifecycle-selftest

# gates
bash scripts/ci/catch-policy-gate.sh
bash scripts/ci/fast-gates.sh
```

### H.7.2 Appendix — Class-to-gate table

| Class | Gate/rule after Path B | Negative test |
|---|---|---|
| SF-1 | reset parity | add a field → fail |
| SF-2 | named-handler rule | remove unsubscribe → fail |
| SF-3 | catch policy | blank reason → fail |
| SF-4 | lookup contract | removed-id fixture |
| SF-5 | panel kit | null-owner context |
| SF-6 | reentrancy guard | double-advance test |
| SF-7 | isolation gate + parity | unannotated static → fail |
| SF-8 | culture test | hostile locale |
| SF-9 | edge theory | partial slot |
| SF-10 | dirty-order audit | mutation without flag |
| SF-11 | lookup audit | missing row |
| SF-12 | producer census | registered-but-never-fired |

### H.7.3 Appendix — Risk register (continued)

| # | Risk | Mitigation |
|---|---|---|
| 11 | harness becomes a maintenance burden | keep harnesses minimal; they reuse existing test doubles |
| 12 | "downgraded" findings are forgotten | every downgrade gets a debt row with a revisit trigger |
| 13 | conversions produce annotation churn | conversion only on authority paths; annotation only with reasons |
| 14 | parity test blocks legitimate direct-only setups | exemption mechanism with reasons, reviewed |
| 15 | repair and hardening blur into one package | C packages are separately signed and listed in U.2 |

### H.7.4 Appendix — What "acceptance" means for this plan

Acceptance is not "the tests pass". It is:

1. **Defect truth:** every claimed defect has a repro or an honest downgrade.
2. **Repair truth:** every repair has a regression.
3. **Class truth:** every class has a gate, rule, or dated debt row.
4. **Boundary truth:** no schema, balance, or feature change slipped in.
5. **Release truth:** Annex U reflects only signed releases.

---

## H.8 Final statement for W2-02

ASHFALL's simulation is unusually disciplined; the bug surface that remains is
concentrated at lifecycle boundaries and in silent paths. This plan turns those
into ten decision points, each with a verified severity, a repro standard, three
complete paths, and an evidence pack that can be audited without reading the
diff.

The flagship repair — the `_silentFoundry` family — is three null assignments
plus a named handler; its value is disproportionate to its size because the
reset contract it restores is what keeps every slot switch honest. The rest of
the plan generalises that one finding into twelve failure classes with gates.

Recommended: **Plan Path B** with Point 6 at Path A. Execute Point 1 first,
prove it with the slot-switch harness, then take Points 2–5 as one class
package and 7–10 as another.

---

**End of W2-02.** Proposal only. The document executes nothing, changes no
production path, and releases nothing without the signatures in Annex U.2.

*Document control: W2-02 · Wave 2 · HEAD 5be1a30a · companion to W2-01,
W2-03…W2-06 · plan-unblocking annex separate per the Wave 2 rule.*---

# PART I — BUG-CLASS CANDIDATE CATALOGUE (PREMIUM DETAIL)

This catalogue lists bug classes a builder should probe before accepting the
plan as complete. Each entry has: the pattern, where to look, the probe, and
the likely severity. Entries marked **candidate** were not statically verified
at plan time; the probe decides.

---

## I.1 Lifecycle candidates

### C-01 — Guard early-return with uncoupled dependency reset (verified)

Pattern: setup guard + never-reset field + reset dependency.
Probe: the reset-coverage census.
Severity: S3→S1.
Status: verified (the D19c family).

### C-02 — Panel bound to a reset owner (candidate)

Pattern: a panel binds an owner created in setup; the owner is nulled on reset;
the panel rebinds only in setup.
Probe: open the panel after a slot switch (Point 8 kit).
Severity: S2 (stale display) or S3.
Status: candidate; the panel kit decides.

### C-03 — Event handler capturing a reset owner (candidate)

Pattern: `owner.Event += handler` where handler closes over another reset owner
directly (not via closure indirection).
Probe: subscription inventory + double-create dispatch count.
Severity: S3.
Status: candidate; the inventory decides.

### C-04 — Dirty flag cleared before write completes (candidate)

Pattern: capture clears the flag at start; a mutation during capture is lost.
Probe: dirty-order audit (Point 9).
Severity: S1 if persisted state.
Status: candidate.

### C-05 — Static singleton initialized per-assembly (candidate)

Pattern: `static readonly` compiled into two assemblies with different values,
or a static registry shared across test cases.
Probe: isolation gate + parity runs.
Severity: S3.
Status: candidate.

---

## I.2 Persistence candidates

### C-06 — Save write failure swallowed (candidate)

Pattern: `try { Save(); } catch { }` on a host session save path.
Probe: catch worksheet on save paths; error-path test.
Severity: S1.
Status: candidate; W2-01 measured the bare-catch class as nearly empty, but
`catch (Exception` without signal is the target.

### C-07 — Partial restore leaves hybrid state (candidate)

Pattern: apply loop without a pre-validation stage.
Probe: save edge fixtures (corrupt mid-section).
Severity: S1.
Status: candidate; the load path is staged in `TryLoadAndRestoreGame`, so the
probe may find it already safe — that is a good outcome.

### C-08 — Section capture mutates during enumeration (candidate)

Pattern: capture iterates a live collection that a tick appends to.
Probe: save-during-tick audit.
Severity: S2 (exception) or S1 (missing rows).
Status: candidate.

### C-09 — Legacy envelope field required for old saves (candidate)

Pattern: a required field added without a default for legacy loads.
Probe: legacy-envelope fixture.
Severity: S1 (old saves refuse).
Status: candidate; the difficulty envelope is the known example.

### C-10 — Checksum input differs across platforms (candidate)

Pattern: a string built with culture-sensitive formatting or unordered
iteration entering the checksum.
Probe: hostile-culture run + two-process hash comparison.
Severity: S1.
Status: candidate; current checksum formatting is invariant (verified), so the
probe targets new fields.

---

## I.3 Runtime candidates

### C-11 — Persisted-id lookup default (candidate)

Pattern: `TryGetValue` ignoring the false branch.
Probe: removed-id fixture (Point 10).
Severity: S2.
Status: candidate; the audit decides which sites matter.

### C-12 — Null loot table consumed (candidate)

Pattern: map node `lootTable: null` (verified to exist) read as a table.
Probe: resolve loot for `loc_holdfast`.
Severity: S2 if it throws.
Status: candidate.

### C-13 — Catalog row removed but referenced by a quest/radio script

Pattern: content references an id no longer in the catalog.
Probe: content reference scan (W2-06's continuity tooling helps).
Severity: S2.
Status: candidate.

### C-14 — Day counter overflow via multiplication (candidate)

Pattern: `day * hoursPerDay * something` in an int.
Probe: numeric audit (Point 6).
Severity: S4.
Status: candidate; unlikely to bite before many years of campaign time.

### C-15 — Division by zero in an aggregate (candidate)

Pattern: average over survivors when all are dead/injured.
Probe: boundary fixtures (zero survivors).
Severity: S2 (NaN) or crash.
Status: candidate.

---

## I.4 UI candidates

### C-16 — Panel refresh after owner null (candidate)

Pattern: `owner.StateChanged += panel.Refresh` without unsubscribing on close.
Probe: close/reopen cycle ×100.
Severity: S3.
Status: candidate; panel hygiene gates cover some of this.

### C-17 — Fabricated fallback value (regression candidate)

Pattern: a new surface shows `0` or `—` where the owner is unavailable, looking
like real data.
Probe: purity gates + Point 8 contexts.
Severity: S2 (player misled).
Status: candidate; the gates exist to catch it.

### C-18 — Close/back parity missing on a new panel (candidate)

Pattern: no Esc/controller close hook.
Probe: close-parity kit.
Severity: S2 (usability).
Status: candidate.

### C-19 — Modal focus trap after load (candidate)

Pattern: a modal opened during load keeps focus when the state changes.
Probe: mid-load context.
Severity: S2.
Status: candidate.

### C-20 — Localised string key missing (candidate)

Pattern: a new UI string without a key when the freeze lands.
Probe: freeze gate (UNBLOCK-03).
Severity: S2 after freeze.
Status: candidate; freeze-dependent.

---

## I.5 Determinism candidates

### C-21 — Hash-order iteration entering state (candidate)

Pattern: iterating a dictionary without ordinal ordering to compute a value
that persists.
Probe: two-process parity.
Severity: S1 if persisted.
Status: candidate; the repo's stable-hash discipline suggests few sites.

### C-22 — Constant RNG seed fallback (verified pattern exists)

Pattern: `new SeededRng(147)` fallback when no campaign seed is available.
Probe: seed-source census.
Severity: S4 (identical campaigns) unless documented as preview.
Status: candidate; documented fallbacks are acceptable.

### C-23 — Static mutable state influencing outcomes (candidate)

Pattern: an outcome depends on a static field set by a previous run.
Probe: isolation gate + replay parity.
Severity: S3.
Status: candidate.

---

## I.6 Data candidates

### C-24 — Mixed numeric form (verified)

Pattern: `dangerLevel` authored as int and float literals (verified).
Probe: mixed-numeric report.
Severity: S4 (latent).
Status: verified; W2-01 fixes it.

### C-25 — Missing field default ambiguity (verified)

Pattern: `travelHours` absent for 8 rows (verified).
Probe: coverage script.
Severity: S4.
Status: verified; W2-01/W2-05 fix it.

### C-26 — Duplicate catalog id across files (candidate)

Pattern: the same id in `locations.json` and a regional catalog with different
values.
Probe: `detect-corpus-duplicates.py` + id-set intersection.
Severity: S2 (which wins?).
Status: candidate.

### C-27 — Catalog referenced by no consumer (candidate)

Pattern: the "dead data" class.
Probe: content-utilization selftest.
Severity: S5 (maintenance).
Status: candidate; W2-06 owns the drain.

### C-28 — Schema_version absent (verified: none)

Pattern: a catalog without `schema_version`.
Probe: `grep -c '"schema_version"'`.
Severity: S4.
Status: verified clean at HEAD (all 338 carry it).

---

## I.7 How to use this catalogue

1. Before closing the bug plan, run the probes for all entries.
2. Record each as: verified / clean / N-A (with reason).
3. For verified entries, open the corresponding point or a new debt row.
4. For clean entries, record the date — a clean sweep is evidence too.

The catalogue prevents the next audit from starting from zero and gives the
foreman a single screen of what the bug plan actually covers.

---

## I.8 Probe command library

```bash
# C-01 reset census
grep -rn "if (_[a-zA-Z]* != null) return;" src/Main*.cs
# C-04 dirty flags
grep -rn "_[a-zA-Z]*Dirty" src/Main*.cs | head -40
# C-08 save-during-tick
grep -rn "Capture\|Save(" src/Main*.cs | grep -i "tick" | head
# C-11 lookup defaults
grep -rn "TryGetValue" Assets/Ashfall.Core src --include=*.cs | wc -l
# C-14 multiplication
grep -rn "day \* \|\* hoursPerDay\|\* 24" Assets/Ashfall.Core --include=*.cs | head
# C-26 duplicate ids
python3 scripts/ci/detect-corpus-duplicates.py
# C-28 schema
grep -L '"schema_version"' Assets/StreamingAssets/Data/*.json
```

Each probe's raw output is attached to the bug plan's evidence pack, not
summarized away.

---

## I.9 Severity summary table

| Class | Verified count | Candidate count | Max severity |
|---|---|---|---|
| lifecycle | 1 (D19c) | 4 | S1 |
| persistence | 0 | 5 | S1 |
| runtime | 0 | 5 | S2 |
| UI | 0 | 5 | S2 |
| determinism | 1 (constant fallback pattern) | 2 | S3 |
| data | 2 (mixed form, missing field) | 3 | S4 |

This table is the plan's claim about its own coverage: every row must end as
verified-fixed, clean-dated, or debt-rowed.

**End of the bug-class candidate catalogue.**

*Document control: W2-02 · Wave 2 · HEAD 5be1a30a · proposal only.*