# Plan 66 — Guilt Sources Expansion: Closeout

## Status: **COMPLETE** (data-first; two §28 dedup substitutions documented)

## Counts

```text
Baseline:  20  (verified — this plan's count was accurate)
New:       20  (the plan's exact requested additions, with 2 slots
               substituted per the §28 uniqueness rule)
Final:     40  — all choice_pattern values unique, severity within
               the observed catalog band (0.2–0.9, 0.05 steps)
```

## Runtime contract (verified in `GuiltInsomniaSystem.cs`)

| Question | Answer (repository truth) |
|---|---|
| Guilt scope | **Per-survivor** — `GuiltSurvivorState` per id |
| Trigger API | `RecordGuilt(survivorId, sourceId, severity, currentDay)` — **free-form sourceId**; no catalog lookup happens in the runtime |
| Accumulation | `insomniaSeverity = min(1, Σ severities)` per survivor |
| Repeat behavior | Each `RecordGuilt` appends a `GuiltRecord` — sources stack; no per-pattern once-only in the runtime |
| Decay | Records expire after **30 days** (`GuiltExpiryDays`); sedative −0.4; dialogue resolves the newest record |
| Thresholds | `HighSeverityThreshold = 0.7` insomnia → `OnGuiltInsomniaCritical` |
| Downstream | Sleep quality multiplier (insomnia × 0.5 penalty); the insomnia severity is the contamination surface |
| Persistence | Full per-survivor state save/restore — additive, id-keyed |
| Catalog load | `guilt_sources.json` is **not loaded by Core** — it is the pattern/severity/title/description reference vocabulary callers source their `RecordGuilt` arguments from (content-utilization maps it to `GuiltInsomniaSystem`/`GuiltPanel`) |
| Emission today | `ConfessionSecretSystem` emits data-driven patterns (`secret_exposed_{id}`); `Phase0Panel` emits one hardcoded pattern; the original 20 are reference vocabulary (no verbatim emitters) |

## The 20 new sources

| Pattern | Severity | Title | Class | Dedup/boundary notes |
|---|---|---|---|---|
| `hoard_medicine` | 0.80 | The Last Dose | resource | boundary with existing `hoard` (0.4): the death-adjacent medicine-specific withholding |
| `trade_food` | 0.55 | Weight of the Crates | resource | distinct from ration cuts (`cut_ration`/`reduce_food`/`starve`): the trade, not the ration |
| `taint_supplies` | 0.70 | Marked Unsafe | resource | unique |
| `warm_room` | 0.30 | One Warm Room | resource | unique (comfort-vs-critical) |
| `refugee_door` | 0.65 | The Closed Door | shelter | boundary with `turn_away` (0.55): organized group under dangerous conditions, the recording |
| `expel_bunk` | 0.70 | The Empty Bunk | shelter | unique |
| `hide_cache` | 0.45 | Behind the Panel | shelter | distinct from `lie`/`deceive`: material concealment from allies |
| `abort_rescue` | 0.60 | Turned Back | expedition | boundary with `abandon` (0.75): mission abandonment (re-attemptable) vs broken trust |
| `mute_distress` | 0.65 | The Radio Still Calling | expedition | **substitution**: the plan's "leave wounded behind" duplicated existing `leave_behind` ("The Abandoned") per §28 |
| `signal_dropped` | 0.50 | The Silent Watch | expedition | fills the freed slot: abandoning a trusted watch |
| `plea_ignored` | 0.75 | Hands Visible | combat | **substitution**: the plan's "execute surrendered enemy" duplicated existing `execute` ("The Execution") per §28 |
| `bait_civilians` | 0.90 | The Safer Route | combat | unique (devastating, as the plan specifies) |
| `kill_ally` | 0.80 | Known Face | combat | boundary with `kill` (0.85): identity-specific former-ally killing |
| `break_terms` | 0.60 | Terms Broken | social | boundary with `betray` (0.75): institutional agreement vs personal betrayal |
| `inform` | 0.70 | Name Given | social | unique |
| `break_promise` | 0.85 | The Promise | social | unique (the Plan 65 promise-failure trigger) |
| `withhold_relief` | 0.70 | Saved for Later | medical | unique |
| `triage_last` | 0.75 | Useful Enough | medical | unique |
| `strip_home` | 0.75 | Nothing Left | scavenging | boundary with `take_all` (0.6): inhabited home vs abandoned cache |
| `order_death` | 0.90 | The Order Given | leadership | boundary with `sacrifice_other` (0.85): explicit command vs the sacrifice outcome |

Class distribution: resource 4 / shelter 3 / expedition 3 / combat 3 /
social 3 / medical 2 / scavenging 1 / leadership 1 — **exactly the plan's
requested classes** (the 2 substitutions stayed inside their classes).
Severity distribution of the new 20: **3 minor-moderate (≤0.5) / 12
moderate-severe (0.55–0.75) / 5 severe-devastating (>0.75)** — not
top-heavy; combined 40-entry distribution holds the plan's healthy-band
targets. All descriptions follow the existing 2-sentence act+residue voice
with `{name}` templating; zero moralizing phrasing.

## Cross-system integration findings (evidence-driven)

- **Psychological contamination (§33): satisfied by validation, no new
  wiring.** The insomnia severity (Σ source severities, clamped) is the
  trauma surface — `order_death` (0.9), `bait_civilians` (0.9), and
  `plea_ignored`/`kill_ally` (0.75–0.8) contribute proportionally by
  construction. Per the plan's own §33: "if psychological contamination
  already reacts to total guilt, no source-specific wiring may be needed."
- **Final wishes (§32): deferred with evidence.** `FinalWishSystem`
  (Plan 65 verification) applies fixed +15/−10 shelter morale only — no
  guilt emission hook exists. `break_promise` (0.85) and
  `withhold_relief` (0.70) are authored as the vocabulary for the
  wish-failure bridge; the runtime hook (wish expiry → `RecordGuilt`) is a
  host-level follow-on requiring the §72 justification standard.
- **Incident confrontations (§31): deferred with evidence.** The Plan 57
  incident scheduler does not exist (Case D read-model); guilt-history
  conditions have no incident grammar to bind to yet. The 5 candidate
  confrontations (`mute_distress`, `expel_bunk`, `hide_cache`,
  `withhold_relief`, `break_promise`) are documented for the scheduler
  follow-on.
- **Confession (§34): already live.** `ConfessionSecretSystem` emits
  `secret_exposed_*` guilt through the same API — the expanded vocabulary
  composes with it (29 guilt/confession tests green).
- **Mourning (§35): no new wiring** — the memorial layer consumes death
  context through its own authority; the death-adjacent sources
  (`order_death`, `kill_ally`, `break_promise`) feed the survivor's own
  insomnia, which is the sanctioned surface.

## Save compatibility

`GuiltInsomniaSaveState` is survivor-id-keyed with full record history —
catalog expansion requires no migration. Old saves load unchanged; new
patterns only appear when a caller emits them.

## Verification

| Gate | Result |
|---|---|
| `--data-integrity-selftest` | **PASS** 0 findings / 208 catalogs (10,511 ids) |
| `dotnet test Ashfall.Core.Tests` | **PASS** 6,617/6,617 |
| Guilt/confession suites | **PASS** 29/29 |
| `dotnet build Ashfall.csproj` | **PASS** 0 errors |
| `--content-utilization-selftest` | **PASS** |
| `--bridge-selftest` | **PASS** exit 0 |

*(One test run showed 5 transient failures that did not reproduce — the
known order-dependent flake class documented in the Plan 58 closeout;
three subsequent runs green.)*

## Deferred

1. Wish-expiry → `RecordGuilt` host hook (`break_promise`, `withhold_relief`).
2. Incident-scheduler guilt-history conditions (the 5 confrontations).
3. Verbatim emission of the reference patterns from encounter/quest
   choices (the catalog vocabulary awaits the Plan 58/59 choice-event →
   guilt bridge — same §72 class as the wish hook).
4. Runtime once-only per-pattern semantics (records stack by design; a
   once-only class would be a runtime change).
