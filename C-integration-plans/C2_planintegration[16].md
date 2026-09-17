# C2 — Flagship Integration Plan [16]: Session Durability, Release Gates, Long-Run Soak, and Player-Safe Saves

> **Deliverable:** `C2_planintegration[16].md`
> **Source scope:** Plan 39 — *Session Durability: Saves, Slots, Soak, and the Release Gate*
> **Wave:** Continuity Wave 5 — *The Human Interface* (closing plan)
> **Primary objective:** convert existing save/session probes into mandatory artifact-level release gates, prove 200-hour-class campaign stability under deterministic long-run soak, and expose a player-facing save/slot/recovery model that makes save safety understandable and recoverable.
> **Required execution order:** **39A → 39B → 39C**
> **Hard dependency:** 39C must not expose recovery affordances until 39A proves those recovery paths actually work.
> **Cross-plan dependencies:** Plan 26B exported artifact smoke, Plan 26C performance budgets, Plan 36 wiring metrics, Plan 38A calendar metadata, Plan 34B difficulty metadata, Plan 31B briefing/status routing, Plan 37B keyboard accessibility, Plan 25A localization, Plan 27B/27C coverage/journeys.
> **Scope discipline:** no new save format, no cloud/Steam synchronization, no silent autosave-policy change, no new gate that has not been demonstrated to fail, no percentile claims from tiny samples, and no player-facing recovery option whose underlying recovery path is untested.

---

# 0. Executive Intent

ASHFALL’s save architecture is already stronger than its release discipline.

The project already has:

- checksummed generic save stores,
- atomic temp-write + rename,
- `.bak` rotation,
- a versioned campaign envelope,
- slot routing,
- dozens of generated save-store contracts,
- save/load UI failure selftests,
- deterministic multi-day smoke logic,
- runtime-scale instrumentation,
- session teardown infrastructure,
- export-smoke work from Plan 26.

The durability gap is that these pieces are not yet assembled into one release contract.

A real player does not care that `SaveStore<T>` is elegant if:

- the save fails when the disk fills,
- a quit occurs during the write,
- the newest slot is corrupt,
- a 200-hour campaign accumulates handlers/nodes forever,
- day advance gets slower every 50 days,
- the exported binary cannot find its data,
- Continue silently chooses an unsafe slot,
- the `.bak` exists but the player cannot recover from it.

C2[16] therefore treats durability as one end-to-end chain:

```text
campaign runtime
→ save/write
→ slot/envelope/checksum
→ quit/interruption behavior
→ load/recovery
→ long-session stability
→ exported artifact validation
→ player-facing save truth
→ one release command that can refuse the build
```

The flagship outcome is:

> **A release candidate is produced by one command that can say “no,” and a player can always answer: what was saved, where it was saved, whether it succeeded, what Continue will load, and what recovery path exists if the newest save is damaged.**

---

# 1. Source Diagnosis

The source plan identifies a particularly valuable situation:

- store mechanics are sound,
- coverage checks exist,
- a seven-day deterministic smoke already exists,
- the smoke is not a gate,
- performance has metrics but weak sampling/budgets,
- quit handling exists but durability is not tested,
- long-session leak risks are documented but mostly manual,
- export artifacts need real execution,
- player save/recovery presentation lags behind the underlying mechanisms.

The correct implementation strategy is therefore:

```text
promote proven probes into gates
→ extend them into durability matrices
→ quantify long-session stability
→ expose only proven recovery behavior to players
```

not:

```text
replace the save architecture
```

---

# 2. Program-Level Success Criteria

C2[16] closes only when all of the following are true.

## 2.1 Existing deterministic smoke becomes mandatory

A reduced multi-day smoke runs per push and the full seven-day variant runs nightly/release.

## 2.2 Performance sampling is statistically credible

Critical percentile/budget claims use enough warm samples.

## 2.3 Exported artifacts participate in durability validation

The exact Linux/Windows artifact boots, ticks, saves, reloads, and exits successfully.

## 2.4 Interrupted writes are recoverable

Previous primary and/or backup remain loadable according to the documented save policy.

## 2.5 Disk-full/read-only failures are explicit

No silent data loss.

## 2.6 Slot/profile isolation is proven

Interleaved saves cannot cross-contaminate slots.

## 2.7 Old save shapes migrate safely

Old formats load and never regress back to obsolete wire shapes.

## 2.8 Long sessions stay stable

Day 300 costs approximately what early days cost within a reviewed slope tolerance.

## 2.9 Nodes/handlers return to baseline

Repeated panel/session churn does not grow the tree or subscriptions.

## 2.10 Save growth is bounded

Unbounded persisted histories have retention policies.

## 2.11 Player save state is legible

Slot metadata, autosave status, Continue status, backup recovery, migrations, and failures are visible.

## 2.12 Release is one command

One `release-gate` produces a machine- and human-readable release report.

---

# 3. Architectural Invariants

## 3.1 Existing save format remains authority

Do not invent a new save envelope.

## 3.2 Durability tests operate on real save APIs

No fake “save-like” harness that bypasses `SaveLoadHostSession`/`SaveStoreHub`.

## 3.3 Export validation operates on shipped artifacts

Source-tree success does not imply artifact success.

## 3.4 Gates must fail loudly

A command that exits 0 without the expected success summary is considered failed/skipped.

## 3.5 Performance budgets have one canonical source

Do not duplicate threshold values across scripts/docs.

## 3.6 Soak measures slope, not vanity speed

Long-run stability is primarily about growth over time.

## 3.7 Retention changes are schema-aware

If persisted data is capped/rolled, save schema/versioning must acknowledge it where wire shape changes.

## 3.8 Player recovery never mutates history silently

Loading a backup or migrating a save must be visible.

## 3.9 Continue is honest

If newest candidate is corrupt/incompatible, Continue is disabled or explicitly routes to a proven recovery path.

## 3.10 Save UI uses existing accessibility/localization systems

No bespoke inaccessible save screen.

---

# 4. Dependency Graph

```text
26B exported artifact smoke ───────────────► 39A artifact durability
26C perf budgets ──────────────────────────► 39A/39B timing budgets
36 port contract ──────────────────────────► 39A release report metrics
38A calendar ──────────────────────────────► 39C slot metadata
34B difficulty ────────────────────────────► 39C slot metadata / soak matrix
31B briefing/status ───────────────────────► 39C save success/failure/recovery messaging
37B keyboard navigation ───────────────────► 39C save UI accessibility
25A localization ──────────────────────────► 39C player-facing strings
27B/27C journeys/coverage ─────────────────► 39A tier design

39A — release gates
 │
 ▼
39B — long-session soak
 │
 ▼
39C — player-facing save model
```

Required order:

```text
39A → 39B → 39C
```

---

# 5. Baseline Capture

Before changes record:

- current CI gate count/tiering,
- current 7-day smoke command/output,
- runtime-scale sample size,
- day-advance median/p95/max,
- save slot count/metadata,
- `.bak` behavior,
- quit behavior,
- current export smoke status,
- current node/handler diagnostic capabilities,
- current save envelope sizes.

## 5.1 Baseline commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --save-load-ui-failure-selftest
godot --headless --path . -- --runtime-scale-selftest
bash scripts/ci/verify-fast.sh
```

Run the existing 7-day deterministic smoke verb and save its output as a baseline artifact.

---

# 6. Workstream 39A — Turn Existing Probes Into Release Gates

## 6.1 Objective

Every existing durability probe that matters becomes a versioned CI gate with expected output, intentional failure proof, and artifact-level execution.

---

# 7. 39A Phase A — Register the Multi-Day Smoke

Add two tiers:

### Fast push tier

Reduced deterministic smoke:

```text
3 in-game days
```

Purpose:

- campaign initialization,
- day advancement,
- save/load continuity,
- deterministic sanity.

### Nightly/release tier

Full:

```text
7day_smoke_selftest
```

Register both in CI manifest with:

- command,
- tier,
- expected summary,
- owner/docs.

---

# 8. 39A Phase B — Credible Performance Sampling

Replace tiny-sample percentile reporting.

Use:

```text
warmup
→ ≥25 warm iterations
```

or a documented time-based sampling budget.

Report:

- n,
- median,
- p90/p95 if valid,
- max,
- allocations.

Do not print `p95` for sample sizes that cannot support a meaningful percentile.

---

# 9. 39A Phase C — CI Tier Contract

Document exact tier purpose.

## Fast

- compile,
- unit/integration,
- data,
- deterministic short smoke,
- essential boot,
- critical wiring.

## Nightly

- full 7-day smoke,
- soak subset/full,
- performance budgets,
- export artifacts,
- deeper coverage.

## Release

- all required gates,
- Linux/Windows artifact boot/load,
- release checklist,
- report.

`verify-fast.sh --list` or equivalent must match docs.

---

# 10. 39A Phase D — Durability Matrix

Extend deterministic smoke with save/quit/load permutations.

At minimum:

```text
start
→ day 1 save
→ advance 40 days
→ save
→ open modal/panel
→ simulate WM close request
→ relaunch/load
→ verify state
```

Repeat at odd-day counts.

Include:

- day boundaries,
- mid-panel quit,
- pending UI state,
- session teardown/rebind.

---

# 11. 39A Phase E — Interrupted Write Injection

Create a controlled file-IO fault seam or test double.

Inject interruption:

```text
temp created
→ before final rename/replace
```

Assert:

- old valid primary remains valid or backup remains valid according to save policy,
- next boot reports interruption/recovery condition,
- corrupted/truncated candidate does not crash.

Do not kill arbitrary production processes if a deterministic IO fault injection can model the precise stage.

---

# 12. 39A Phase F — Truncated/Corrupt Primary

Create fixtures:

- truncated JSON,
- checksum mismatch,
- missing envelope section,
- malformed header.

Expected:

- clean validation failure,
- no crash,
- Continue behavior matches player model,
- `.bak` option exposed only if valid.

---

# 13. 39A Phase G — Disk-Full Failure

Inject:

- write denied due to simulated ENOSPC/IO failure.

Expected:

- no overwrite of last good save,
- clear structured error,
- autosave status marks failure,
- release/selftest reports it.

---

# 14. 39A Phase H — Read-Only Save Root

Test write against read-only path.

Expected:

- no silent success,
- no data loss,
- clear reason,
- existing readable saves remain readable.

---

# 15. 39A Phase I — Slot/Profile Isolation

Create:

```text
Profile A / Slot 1
Profile A / Slot 2
Profile B / Slot 1
Profile B / Slot 2
```

Interleave saves.

Assert:

- resolved paths unique,
- envelopes do not cross,
- checksums remain tied to correct slot,
- Continue resolves intended newest compatible slot.

---

# 16. 39A Phase J — Migration Direction

Test:

- legacy V1 filename-keyed envelope,
- pre-envelope bare state.

Flow:

```text
load old
→ migrate in memory
→ save current
```

Assert new save uses only current shape.

Never rewrite old format.

---

# 17. 39A Phase K — Exported Artifact Durability

For Linux and Windows artifacts:

1. build via canonical Plan 26B script,
2. boot headless,
3. resolve packaged data,
4. initialize campaign,
5. advance one day,
6. save,
7. reload,
8. verify checksum/state,
9. exit 0.

This is part of release gate, not optional QA.

---

# 18. 39A Phase L — Expected-Summary Enforcement

Every gate declares an exact summary marker.

Examples:

```text
SEVEN_DAY_SMOKE_OK
SAVE_DURABILITY_OK
EXPORT_SAVE_RELOAD_OK
```

Gate runner treats:

```text
exit 0 + missing summary
```

as failure.

This catches skipped/no-op command paths.

---

# 19. 39A Phase M — Release Report

Produce a single artifact such as:

```text
artifacts/release-report.json
artifacts/release-report.md
```

Include:

- game/build version,
- commit SHA,
- save schema,
- data catalog/id counts,
- gate pass/fail,
- test counts,
- runtime performance stats,
- snapshot set hash,
- unbound-port count,
- content effect coverage,
- artifact hashes/sizes,
- exported smoke results.

---

# 20. 39A Phase N — `release-gate.sh`

Create:

```bash
scripts/ci/release-gate.sh
```

Responsibilities:

- invoke required release-tier gates,
- collect summaries,
- stop on hard failure,
- build release report,
- return non-zero when unhealthy.

No manual “remember these seven commands.”

---

# 21. 39A Phase O — Gate Failure Proofs

Create deliberate failure fixtures:

- corrupt save,
- slow owner/perf regression,
- missing packaged JSON,
- interrupted write,
- missing summary line.

Assert each associated gate fails.

A gate unproven against failure is not considered complete.

---

# 22. 39A Definition of Done

- [ ] 3-day fast smoke registered,
- [ ] 7-day nightly smoke registered,
- [ ] performance sample size credible,
- [ ] CI tiers documented,
- [ ] quit-mid-panel durability matrix,
- [ ] interrupted write tested,
- [ ] corrupt/truncated primary tested,
- [ ] disk-full tested,
- [ ] read-only path tested,
- [ ] slot isolation tested,
- [ ] legacy migration direction tested,
- [ ] Linux exported save/reload smoke,
- [ ] Windows exported save/reload smoke,
- [ ] expected summaries enforced,
- [ ] release report generated,
- [ ] one `release-gate.sh`,
- [ ] every new gate proven to fail.

---

# 23. Workstream 39B — Sessions That Last

## 23.1 Objective

Prove a long campaign does not accumulate hidden runtime or persisted debt.

---

# 24. 39B Phase A — Define the 360-Day Soak

Scripted deterministic policy:

- advance days,
- dispatch expeditions,
- craft,
- treat,
- trade,
- open/close panels,
- save periodically,
- load periodically.

Target:

```text
360 in-game days
+ 20 New Game/Load cycles
```

Use a fixed seed and policy.

---

# 25. 39B Phase B — Measured Channels

Record per day/cycle:

- wall time,
- allocations,
- managed memory,
- Godot node count,
- event-handler counts,
- save size,
- user-data directory size,
- warning/error count,
- day-owner timing,
- panel open/close count.

---

# 26. 39B Phase C — Growth-Slope Budgets

Primary durability question:

```text
Does the cost grow with session age?
```

Fit/compare:

- early window,
- middle window,
- late window.

Define tolerance for:

- day time slope,
- allocations/day slope,
- node count,
- handler count,
- save-size slope.

Day 300 should be materially similar to early stable days.

---

# 27. 39B Phase D — Retention Inventory

Scan persisted collections that can grow indefinitely.

Examples:

- journal/history,
- meal serving logs,
- memorial rows,
- census claims,
- incident history,
- briefing/event history.

For each record:

```text
why persisted?
retention requirement?
cap/window?
archive/aggregate?
checksum implications?
```

---

# 28. 39B Phase E — Retention Policy

Choose per collection:

- fixed rolling window,
- bounded summary + recent detail,
- permanent because game-critical.

Do not cap blindly.

If save wire shape changes, bump schema/version appropriately.

---

# 29. 39B Phase F — Panel Node Baseline

For every live Plan 16A player surface:

```text
open
→ close
```

repeat 20 times.

Assert tree returns to baseline after each cycle or within documented framework noise.

No monotonic node growth.

---

# 30. 39B Phase G — Handler Baseline

Measure Core event subscription counts before/after repeated panel lifecycle.

Assert identical.

Integrate Plan 16C/36 wiring instrumentation.

No duplicate listeners after reopen/load.

---

# 31. 39B Phase H — Session-Swap Safety

Loop:

```text
New Game
→ Load
→ New Game
→ Load
```

Assert:

- old authorities not referenced,
- old nodes not focused,
- old handlers removed,
- new sessions validate ports,
- panel authority identity matches new campaign.

---

# 32. 39B Phase I — Allocation Hotspots

Instrument known candidates:

- worn-gear collection,
- modifier stacks,
- cascade evaluation,
- briefing assembly,
- route estimates,
- event aggregation.

Add per-owner allocation assertions where useful.

Reuse buffers only when correctness remains clear.

---

# 33. 39B Phase J — Save-Size Discipline

Track envelope/store sizes by day.

Fail if non-design data grows linearly without retention policy.

Separate legitimate growth:

- new persistent entities,
- narrative history designed to persist

from accidental duplication.

---

# 34. 39B Phase K — Long Idle

Run:

```text
60 seconds no input
```

Measure:

- CPU time,
- allocations,
- node churn,
- repeated logs,
- audio-loop behavior.

Manual day advancement means idle should be cheap.

---

# 35. 39B Phase L — Determinism Under Soak

Run same:

```text
seed + scripted policy
```

twice.

Compare final:

- campaign checksum,
- autonomous-world digest,
- completion/state digests.

Soak doubles as determinism torture test.

---

# 36. 39B Phase M — Low-End Target

Run at minimum supported configuration:

- compatibility renderer,
- low-end/iGPU class hardware where CI/lab allows,
- heaviest dashboard/panel.

Record:

- frame pacing,
- input responsiveness,
- day-advance latency.

Do not turn hardware-specific measurements into flaky per-push gates if infrastructure cannot stabilize them; use nightly/release evidence.

---

# 37. 39B Phase N — Soak Gate Script

Create:

```bash
scripts/ci/soak-gate.sh
```

Output:

```text
artifacts/soak-results.json
```

Include:

- seed,
- policy version,
- 360-day results,
- slope metrics,
- node/handler baselines,
- final checksum,
- pass/fail budget reasons.

Register Tier-2/nightly.

---

# 38. 39B Phase O — Trend Documentation

Update:

```text
docs/perf/README.md
```

with:

- current baseline,
- last several soak results,
- budget changes,
- rebaseline procedure.

---

# 39. 39B Tests

- retention caps,
- schema migration for capped structures,
- allocation budget assertions,
- node count baseline,
- handler count baseline,
- session-swap stale-reference test,
- save growth budget,
- idle budget,
- soak digest equality.

---

# 40. 39B Definition of Done

- [ ] 360-day deterministic soak,
- [ ] 20 session cycles,
- [ ] time/alloc/node/handler/save metrics,
- [ ] slope budgets,
- [ ] retention inventory,
- [ ] documented caps/windows,
- [ ] live-panel node baseline stable,
- [ ] handler baseline stable,
- [ ] session swap has no stale refs,
- [ ] hot-path allocations constrained,
- [ ] save size bounded,
- [ ] idle work bounded,
- [ ] same-seed soak digest identical,
- [ ] low-end run recorded,
- [ ] nightly soak gate,
- [ ] soak trend documented.

---

# 41. Workstream 39C — Player-Facing Save Durability

## 41.1 Objective

Expose a truthful, understandable save model built only on recovery behavior proven in 39A.

---

# 42. 39C Phase A — Player Save Model ADR

Create:

```text
docs/saves/PLAYER_SAVE_MODEL.md
```

Explain in plain product terms:

- what a profile is,
- what a slot is,
- when autosave runs,
- what Continue selects,
- what manual save does,
- what backup recovery means,
- when a save is incompatible,
- what happens when a write fails,
- what happens on quit.

Implementation must match this page.

---

# 43. 39C Phase B — Slot Metadata DTO

Expose read-only metadata:

```text
slot id
campaign day
season/chapter
survivors alive
last saved wall time
difficulty preset
completion/ending state
migration marker
primary health
backup health
```

Use already-persisted facts.

No full-save load merely to draw every slot if lightweight metadata can be safely derived/stored.

---

# 44. 39C Phase C — Continue Selection Policy

Define deterministic selection:

```text
newest compatible healthy slot
```

or current project policy.

If newest candidate is:

- corrupt,
- unsupported newer schema,
- incomplete

Continue must:

- disable with reason, or
- explicitly offer proven previous-save recovery.

Never silently choose a different slot while implying it loaded the newest.

---

# 45. 39C Phase D — Continue Disabled Reasons

Localized reasons such as:

- save damaged,
- save from newer version,
- no save found,
- migration required/failed.

Use Plan 25 keys.

---

# 46. 39C Phase E — Autosave Visibility

When autosave succeeds:

- use existing save-success feedback,
- show unobtrusive status/cue.

When autosave fails:

- use distinct warning,
- keep last safe save intact,
- present diagnostic/recovery guidance.

Do not bury failure in developer logs.

---

# 47. 39C Phase F — Manual Save Slots

Support explicit slot selection.

Before overwrite show target metadata:

```text
Day N
Season/Chapter
X alive
Difficulty
Last saved time
```

Require confirmation for destructive overwrite.

---

# 48. 39C Phase G — Backup Recovery

If primary checksum/parse fails and `.bak` validates:

Offer:

```text
Try previous save
```

Clearly state:

- primary failed,
- backup timestamp/day,
- loading backup may lose recent progress.

Do not silently replace primary with backup without player awareness.

---

# 49. 39C Phase H — Recovery Persistence

After successful backup load:

- preserve evidence/diagnostic of recovery,
- next manual/autosave writes current valid format,
- do not immediately destroy only backup before a new safe primary exists.

---

# 50. 39C Phase I — Save→Load→Save Idempotence

For unchanged state:

```text
save
→ load
→ save
```

should produce byte-stable/canonical-equivalent envelope according to serialization contract.

If wall timestamps are part of envelope, define comparison excluding intentionally volatile metadata or canonicalize appropriately.

---

# 51. 39C Phase J — Migration Messaging

When a legacy save is upgraded:

- show one localized notice,
- record current schema/migration result in slot metadata,
- do not repeatedly warn after successful current-format save.

---

# 52. 39C Phase K — Standard Diagnostic Path

Reuse an established diagnostic-message pattern.

Do not create a second error-notification framework just for saves.

Structured data should include:

- error kind,
- slot,
- recovery option,
- localization key.

---

# 53. 39C Phase L — Quit-During-Write UX

Confirm close behavior:

- active save operation completes safely or cancels before destructive transition,
- partial temp artifact cannot become trusted primary,
- next boot detects interrupted save if relevant,
- player gets clear status.

Do not hang indefinitely on shutdown.

---

# 54. 39C Phase M — Accessibility

Save UI must be:

- keyboard navigable,
- controller navigable if 37C supports settings/menu surfaces,
- text labeled,
- not color-only,
- localized,
- focus-safe.

Recovery buttons require explicit text.

---

# 55. 39C Phase N — Save UI Journey Tests

Scenarios:

1. healthy Continue,
2. no save,
3. corrupt newest primary,
4. valid `.bak`,
5. newer-schema save,
6. manual overwrite,
7. autosave success,
8. autosave failure,
9. migrated legacy save,
10. quit-after-save.

---

# 56. 39C Phase O — Save Fuzz

Before release run existing save fuzz workflow against:

- primary,
- backup,
- slot metadata,
- migration.

Attach report to release artifact/checklist where appropriate.

---

# 57. 39C Definition of Done

- [ ] player save model documented,
- [ ] rich slot metadata,
- [ ] Continue policy explicit,
- [ ] disabled reasons visible,
- [ ] autosave success visible,
- [ ] autosave failure visible,
- [ ] manual slots,
- [ ] overwrite confirmation,
- [ ] `.bak` recovery affordance,
- [ ] recovery does not destroy safety,
- [ ] save/load/save idempotence,
- [ ] migration messaging,
- [ ] standard diagnostic path,
- [ ] quit-during-write safe,
- [ ] accessible/localized save UI,
- [ ] save journey tests,
- [ ] save fuzz attached before release.

---

# 58. Integrated Release-Durability Pipeline

```text
source + data
   │
   ▼
fast gates
   │
   ├─ compile/tests
   ├─ data integrity
   ├─ short deterministic smoke
   └─ wiring/liveness
   │
   ▼
nightly gates
   │
   ├─ 7-day smoke
   ├─ perf budgets
   ├─ 360-day soak
   └─ export builds
   │
   ▼
release gate
   │
   ├─ Linux artifact boot/save/load
   ├─ Windows artifact boot/save/load
   ├─ save corruption/recovery matrix
   ├─ release report
   └─ checklist
   │
   ▼
shippable artifact
```

---

# 59. Save Integrity Matrix

At minimum test:

| Case | Primary | Backup | Expected |
|---|---|---|---|
| healthy | valid | valid/none | load primary |
| primary corrupt | invalid | valid | offer backup |
| both corrupt | invalid | invalid | clean error |
| interrupted temp | old valid | valid | retain old |
| disk full | old valid | valid | failure + retain |
| read-only | valid | valid | read works, write fails clearly |
| newer schema | valid but unsupported | any | refuse with reason |
| legacy schema | valid legacy | any | migrate in memory |

---

# 60. Slot Isolation Contract

For every store routed through `SaveSlotRoot`:

```text
profile id + slot id
→ unique base directory
```

Cross-slot writes are critical failures.

---

# 61. Atomic-Write Contract

Expected sequence:

```text
serialize
→ write temp
→ flush/close
→ preserve/rotate previous backup
→ atomic replace/rename
→ verify
```

Tests should inject failure at multiple boundaries if feasible:

- before temp,
- during temp,
- before rename,
- after backup rotation.

---

# 62. Backup Contract

Document exactly:

- when `.bak` is created,
- how many generations exist,
- whether backup is checksum-validated,
- when backup is overwritten.

Player-facing language must match reality.

---

# 63. Performance Budget Contract

Canonical budget source should include:

```text
day_advance median
day_advance p95
allocations/day
node growth
handler growth
save-size growth
```

Sampling policy is part of the contract.

---

# 64. Soak Stability Contract

Preferred assertions:

```text
late_day_cost / early_day_cost <= threshold
node_count_end == baseline
handler_count_end == baseline
save_growth_slope <= threshold
final_digest_repeatable == true
```

Exact thresholds must be based on re-measured current values.

---

# 65. Release Report Contract

One report should answer:

```text
What commit shipped?
What data shipped?
What save schema?
Which gates ran?
Did exported Linux boot/save/load?
Did exported Windows boot/save/load?
What were perf results?
What was soak result?
How many unbound ports?
How much runtime content produced effects?
What snapshot set was approved?
```

---

# 66. Gate Self-Validation Contract

Every critical gate owns at least one intentional failure fixture.

Examples:

- bad checksum,
- missing PCK JSON,
- slow day owner,
- missing summary,
- unbound port.

---

# 67. Session Lifecycle Contract

Repeated:

```text
New Game
Load
New Game
Load
```

must maintain:

- wiring completeness,
- focus safety,
- node baseline,
- handler baseline,
- save isolation.

---

# 68. Retention Policy Contract

Every persisted unbounded collection must declare:

```text
retention = permanent / bounded / aggregated
reason
schema impact
```

No silent truncation.

---

# 69. Save UI Truth Contract

Player-facing text may never claim:

```text
Saved
```

unless the write completed and validation policy considers it safe.

Similarly:

```text
Continue
```

must reflect the actual target and health.

---

# 70. Failure Modes and Corrective Actions

## 70.1 Seven-day smoke exists but remains ungated

Register it and pin summary.

## 70.2 p95 computed from tiny n

Increase warm samples and document n.

## 70.3 Exported artifact boots but save path uses repo state

Isolate user/data directories and prove artifact-local behavior.

## 70.4 Disk-full destroys newest good save

Critical; fix atomic sequence/fault handling.

## 70.5 `.bak` exists but UI cannot use it

Expose recovery only after 39A proof.

## 70.6 Day 300 is steadily slower

Find growing collection/cache/subscription; fix or bound.

## 70.7 Node count increases every panel cycle

Lifecycle leak; coordinate Plan 16C/37 focus teardown.

## 70.8 Save envelope grows linearly from logs

Add retention/versioning.

## 70.9 Continue silently falls back

Make fallback explicit.

## 70.10 Gate exits 0 without executing assertion

Expected-summary enforcement fails it.

---

# 71. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| new release gates expose many failures | High | Medium | intentional, classify before gating |
| CI runtime becomes excessive | Medium | Medium | tiered fast/nightly/release |
| soak becomes flaky | Medium | High | deterministic policy + slope budgets |
| perf budgets too tight | Medium | Medium | credible baseline/headroom |
| retention changes save shape | Medium | High | schema versioning |
| backup UX promises unsupported behavior | Medium | High | 39A before 39C |
| disk fault injection unrealistic | Low–Med | Medium | IO seam/test double |
| Windows artifact divergence | Medium | High | same contract as Linux |
| save UI complexity | Medium | Medium | one-page player model first |
| timestamp breaks byte stability | Medium | Low | canonical comparison policy |

---

# 72. Commit Strategy

## C2[16].1 — baseline + CI tier inventory

## C2[16].2 — 3-day/7-day gate registration

## C2[16].3 — credible perf sampling

## C2[16].4 — durability fault matrix

## C2[16].5 — slot/migration isolation tests

## C2[16].6 — exported save/reload smoke

## C2[16].7 — expected-summary enforcement

## C2[16].8 — release report + `release-gate.sh`

## C2[16].9 — gate failure fixtures

### Gate: 39A complete

## C2[16].10 — 360-day soak harness

## C2[16].11 — growth-slope budgets

## C2[16].12 — retention policies

## C2[16].13 — node/handler baseline

## C2[16].14 — session-swap + allocation hot paths

## C2[16].15 — save-size/idle/determinism

## C2[16].16 — soak gate + trend docs

### Gate: 39B complete

## C2[16].17 — player save model + metadata

## C2[16].18 — Continue health/reason logic

## C2[16].19 — autosave success/failure feedback

## C2[16].20 — manual slots + overwrite

## C2[16].21 — backup recovery + migration messaging

## C2[16].22 — quit/write safety + diagnostics

## C2[16].23 — accessibility + journey/fuzz tests

### Gate: 39C complete

## C2[16].24 — Wave-5 release closure

---

# 73. Verification Checklist

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --save-load-ui-failure-selftest
godot --headless --path . -- --runtime-scale-selftest
bash scripts/ci/soak-gate.sh
bash scripts/ci/export-smoke-boot.sh
bash scripts/ci/release-gate.sh
bash scripts/ci/verify-fast.sh
```

Also run repository-canonical equivalents of:

```text
7-day deterministic smoke
ashfall-save-fuzz
ashfall-seed-replay
ashfall-lfs-gate
```

---

# 74. Flagship Definition of Done

## 39A — Release gates

- [ ] 3-day fast smoke,
- [ ] 7-day nightly smoke,
- [ ] credible performance sampling,
- [ ] tier contract documented,
- [ ] durability matrix,
- [ ] interrupted write test,
- [ ] corrupt/truncated file test,
- [ ] disk-full test,
- [ ] read-only test,
- [ ] slot/profile isolation,
- [ ] migration direction,
- [ ] Linux artifact save/reload,
- [ ] Windows artifact save/reload,
- [ ] expected-summary enforcement,
- [ ] release report,
- [ ] one release command,
- [ ] deliberate failure proof for every new gate.

## 39B — Long-session durability

- [ ] 360-day soak,
- [ ] 20 session cycles,
- [ ] per-day timing,
- [ ] allocations,
- [ ] node baseline,
- [ ] handler baseline,
- [ ] save/user-dir growth,
- [ ] retention policies,
- [ ] session-swap safety,
- [ ] allocation hot paths bounded,
- [ ] idle budget,
- [ ] deterministic final digest,
- [ ] low-end evidence,
- [ ] nightly soak gate,
- [ ] trend history.

## 39C — Player-facing saves

- [ ] player save-model doc,
- [ ] slot metadata,
- [ ] honest Continue,
- [ ] disabled reasons,
- [ ] autosave success signal,
- [ ] autosave failure warning,
- [ ] manual slots,
- [ ] overwrite confirmation,
- [ ] backup recovery,
- [ ] idempotent save/load/save,
- [ ] migration messaging,
- [ ] shared diagnostic path,
- [ ] quit-during-write safety,
- [ ] keyboard/controller accessibility,
- [ ] localization,
- [ ] scripted recovery journeys,
- [ ] save fuzz before release.

## Global

- [ ] no new save format,
- [ ] no cloud sync scope creep,
- [ ] no unproven recovery UI,
- [ ] no five-sample percentile claims,
- [ ] full release gate green.

---

# 75. Closure Report Template

```markdown
## C2[16] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:
- Release candidate version:

### 39A — Release Gates
- Fast smoke:
- 7-day smoke:
- Perf sample n:
- Median:
- p95:
- Durability matrix:
- Interrupted write:
- Corrupt save:
- Disk full:
- Read-only:
- Slot isolation:
- Legacy migration:
- Linux artifact:
- Windows artifact:
- Missing-summary regression:
- Release report:
- release-gate:
- Result:

### 39B — Soak
- Seed:
- Policy version:
- Days:
- Session cycles:
- Early day median:
- Late day median:
- Time slope:
- Allocation slope:
- Node baseline delta:
- Handler baseline delta:
- Save-size slope:
- Idle CPU/alloc:
- Final digest A:
- Final digest B:
- Low-end run:
- Result:

### 39C — Player Saves
- Player model:
- Slot metadata:
- Continue healthy:
- Continue corrupt:
- Continue newer schema:
- Autosave success:
- Autosave failure:
- Manual overwrite:
- Backup recovery:
- Save/load/save idempotence:
- Migration notice:
- Quit-mid-write:
- Accessibility:
- Localization:
- Save fuzz:
- Result:

### Release Summary
- Commit:
- Data catalog count:
- Data id count:
- Save schema:
- Tests:
- Gates:
- Unbound ports:
- EFFECT_PRODUCED coverage:
- Snapshot hash:
- Artifact hashes:
- Final release result:

### Remaining Debt
- Save:
- Performance:
- Soak:
- UI:
- Artifact:
```

---

# 76. Final Execution Directive

Execute Plan 39 as a release-durability closure, not as another save-system rewrite.

The critical sequence is:

```text
promote existing smoke probes into mandatory gates
→ make performance sampling credible
→ prove interrupted-write and corruption recovery
→ boot/save/load the actual artifacts
→ produce one release command/report
→ run a deterministic 360-day soak
→ eliminate long-session growth
→ only then expose player-facing recovery and save truth
```

Do not add a recovery button before the recovery path is proven.

Do not add a gate that has never been forced to fail.

Do not treat `exit 0` as proof if the expected success summary never appeared.

Do not claim long-session health from a seven-day run alone.

The strongest release rule is:

> **A release candidate is healthy only if the exact artifact that ships passes data, boot, save, reload, durability, wiring, performance, and soak contracts.**

The strongest long-session rule is:

> **The 360th day must cost approximately what an early stable day costs; nodes, handlers, allocations, and persisted history must not grow without an explicit retention policy.**

The strongest player-safety rule is:

> **A player must always be able to tell what was saved, whether it succeeded, what Continue will load, and which tested recovery path exists if the newest save is damaged.**

The flagship acceptance command is:

```bash
bash scripts/ci/release-gate.sh
```

and the flagship product criterion is simple:

> **One command, one release report, and a real ability to say no.**
