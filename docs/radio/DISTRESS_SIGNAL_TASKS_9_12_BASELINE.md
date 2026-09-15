# DISTRESS SIGNAL TASKS 9–12 — WAVE 0 FORENSIC BASELINE

> Status: **Wave 0 complete (read-only).** No production code or data changed.
> Recorded 2026-09-13 against commit `87b199b2` + active dirty worktree (untouched).
> This document records current evidence and **premise corrections** to the
> Tasks 9–12 flagship plan. Implementation waves must consume these corrections.

---

## 1. Baseline verification (frozen)

| Check | Command | Result |
|---|---|---|
| Build | `dotnet build Ashfall.csproj` | **0 errors, 0 warnings** |
| Data integrity | `godot --headless --path . -- --data-integrity-selftest` | **PASS — 0 errors, 0 warnings, 325 catalogs, 12,890 authored ids** |
| Focused radio suite | `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/` | **249/249 passed** |
| Full suite | recorded in INTEGRATION_PLANS.md (same batch, 2026-09-13) | 11,098/11,098 — re-run deferred to closeout per TEST_POLICY |

---

## 2. PREMISE CORRECTIONS (plan vs current source truth)

### PC-1 — Signal count is 48, not 25

Plan §0/§23 assume a "25-signal distress catalog". Current evidence:

| Source | Count | Loader |
|---|---|---|
| `radio_distress_signals.json` | 25 broadcasts | `RadioDistressSystem.LoadFromJson` |
| `radio_distress_signals_expansion.json` | 23 broadcasts | `src/Host/RadioHostSession.cs:145` |
| `RegisterBuiltinCanonicalSignals()` | 8 hardcoded (IDs overlap the base JSON; `RegisterSignal` overwrites by ID — they are fallback defaults, not additional signals) | constructor |

**Corrected scope: 48 catalog rows, 43 unique effective signal identities,
47 total runtime-registered signals.** (25 base + 23 expansion − 5 IDs present
in both files = 43 JSON identities; plus 4 builtin-only fragment-less fallback
signals — `freq_distress_108_9`, `freq_distress_134_5`, `freq_distress_162_1`,
`freq_distress_124_7` — which exist ONLY in `RegisterBuiltinCanonicalSignals()`
and resolve via the legacy SourceName fallback.) The 5 duplicate IDs
(`freq_distress_148_2`, `freq_distress_392_7`, `freq_distress_401_9`,
`freq_distress_217_4`, `freq_distress_55_1`) are an intentional, documented
primary-wins pattern: `RadioHostSession` loads the expansion layer FIRST and the
primary Plan 50 authority LAST, so the base definitions are canonical and the 5
expansion rows are overridden (see host comment at `src/Host/RadioHostSession.cs`).
All "25/25" acceptance lines in the plan read **43/43 unique identities**.
Stage-contract validation must treat within-file duplicate IDs as errors and
cross-file duplicates as the documented primary-wins override (warning-level
visibility, never a gate failure).

### PC-2 — Task 9's stage model already exists as `message_fragments` (Level 1)

The plan's central audit question is already answered by source:

- `DistressMessageFragment { day, clarity, text }` — `Assets/Ashfall.Core/Radio/RadioDistressSystem.cs`
- **All 25 base signals have 2–5 fragments** (distribution: 2×6, 3×10, 4×5, 5×4),
  each with ascending `day` thresholds and ascending `clarity`.
- Fragment `day` semantics are **absolute campaign day**, proven at both consumers:
  - `RadioTuner.EvaluateFrequency` (RadioTuner.cs:162–176): selects the highest
    fragment with `frag.Day <= day`; presentation-only; recomputed per tune.
  - `RadioPropagation.Evaluate` (RadioPropagation.cs:192–210): same selection rule,
    plus weather attenuation `clarity = clamp(frag.Clarity × attenuation, 0.1, frag.Clarity)`
    and `AudibleFragmentIndex` output.
- Stage selection is **purely derivable from current campaign day** — no stage index
  is persisted, none is needed. Skipping days advances directly to the correct
  stage; regression is impossible (day is monotonic). `HighestClarity` on
  `ActiveDistressSignal` is persisted and monotonic via
  `UpdateMonotonicClarity` (RadioPropagation.cs:233).

**Decision gate outcome (plan §9A):** reuse `message_fragments` as the stage
authority. **Do NOT add a `stages` field.** The Level 1→2 gap is not persistence —
it is (a) missing `outcome_hint`, (b) missing formal validation, (c) missing a
single named resolver (selection logic is currently duplicated in two places).

### PC-3 — Task 10's event authorities already exist

`DistressRescueMissionManager` (`Assets/Ashfall.Core/Radio/DistressRescueMissionManager.cs`)
already owns the exact lifecycle events the trust system needs:

| Plan event | Existing authority evidence |
|---|---|
| answered | `RecordExpeditionDispatched` / `ResolveMoralChoice(choiceIndex=0)` → `Dispatched` |
| ignored | `ResolveMoralChoice(choiceIndex=1)` → `ResolvedIgnored` + `IgnoreConsequenceApplied` (exactly-once tokens `sender_death`/`faction_standing_loss`/`faction_ambush`) |
| trap fallen for | `IsTrap` + `AssessmentThreatDetected` / `ResolvedTrapDefeated` / `ResolvedTrapAvoided` statuses |
| rescue successful | `CompleteRescue` → `ResolvedRescued` (idempotent) |

Faction standing consequences already route through `FactionWarSystem.ModifyStanding`
(`Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs:88`) — the standing store is
**faction-keyed only** (`GetStanding(string factionId)`), with no category/scoped
mechanism. Reusing it for `signal_trust` would contaminate faction semantics and
its UI.

**Decision gate outcome (plan §10A):** fallback path B — a minimal, radio-owned,
data-only `SignalTrustState` persisted in the radio save section. Do NOT extend
FactionWarSystem and do NOT create a general reputation service.

### PC-4 — Task 11 exactly-once and ledger patterns already exist

- `DistressMissionSaveState.ClaimedReceipts` — exactly-once receipt ledger with a
  canonical fingerprint (`ComputeFingerprint`) and codec validation.
- `RadioSaveState.playedBroadcastKeys` — fired-broadcast dedupe ledger.
- Scheduling infrastructure exists: `RadioScheduleCoordinator`,
  `CensusBroadcastScheduler`, `RadioProgramProductionSystem` (Plan 173
  `ScheduledBroadcastResult` delivery pattern).
- No `followUpSignals` / `delay_days` field exists in either distress JSON.

**Decision gate outcome (plan §11A):** add the smallest radio-owned pending
follow-up queue inside the distress runtime (mirroring the ClaimedReceipts
pattern), scheduled on campaign day; reuse `RadioSaveState`/`RadioSave.cs` as the
save owner. Do NOT create a second campaign scheduler.

### PC-5 — Task 12's audio model already exists; distress lacks only the cue field

- `RadioBroadcastModels.cs:338` — `AudioCue` property on broadcast models.
- `RadioBroadcastCatalog.cs:326,377` — `audio_cue` parsed from station JSON.
- `RecordedCassetteEntry.audioCue` persists cue strings through `RadioSave.cs:64`.
- `src/Audio/AudioManager.cs` + `AudioCueCatalog` + `ShelterOperationsAudioBridge`
  already resolve radio cues (`RadioTuningHeterodyne`, `RadioSignalLock`,
  `RadioDecryptedBeep`).
- Distress signal definitions have **no audio field** → one additive
  `audio_cue` field (+ optional per-fragment override) is required.

**Decision gate outcome (plan §12B):** add `audio_cue` on the signal and optional
`audio_cue` per fragment (stage-level override, plan Option A). Do NOT create a
clarity-band cue map.

### PC-6 — Builtin/JSON dual-definition is pre-existing behavior

8 signals are defined both in `RegisterBuiltinCanonicalSignals()` (fallback
defaults, no fragments) and in `radio_distress_signals.json` (with fragments).
JSON load happens after construction, so JSON wins. Any stage work must keep the
builtin fallbacks load-safe (they currently have zero fragments — the resolvers
already handle `Count == 0` by falling back to `SourceName`).

---

## 3. Current schema grammar (as loaded)

```text
radio_distress_signals.json / radio_distress_signals_expansion.json
schema_version: 1
radio_broadcasts[]:
  frequency_id        string  required  (signal identity; OrdinalIgnoreCase dict key)
  frequency_mhz       string  required  ("217.4" / "217.4MHz"; invariant-culture parse; fallback 100.0)
  source_name         string  required
  outcome_type        string  required  (survivor_* | bait_trap | knowledge | narrative |
                                            supply_cache | abandoned_cache | water_caravan_wreck | ...)
  authenticity        string  optional  (expansion only: genuine | trap | false_flag |
                                            bait_trap | stale | automated)
  days_to_trace       int     required  (default 4) — countdown after interception
  deadline_days       int?    optional  (snake or camel; falls back to days_to_trace)
  sender_survival_days int?   optional  (sender survival window)
  ignore_consequence  string? optional  (token list; validated against
                                            KnownConsequenceTokens)
  warning_text        string  optional
  revealed_location   string  required  (location id; integrity-checked)
  location_reference  string? optional  (falls back to revealed_location)
  revealed_knowledge  string  optional
  knowledge_points    int     optional
  revealed_items      list    optional  (item ids; integrity-checked)
  message_fragments[] required  (2–5 rows; see below)
  narrative_id        string  optional
  recruit_survivor_id string  optional
  sender_faction_id   string  optional
  deceptive_faction_id string optional
  moral_choice_id     string  optional (expansion)
  reputation_faction_id string optional
  reputation_delta    int     optional (default 15)
  npc_id              string  optional (Plan 52 arc suppression)
  resolve_quest_id    string  optional (Plan 52 arc quest)

message_fragments[] row:
  day      int     required — ABSOLUTE campaign day threshold (both consumers)
  clarity  float   required — 0..1 scale, ascending in all authored data
  text     string  required — player-facing transmission text
  (no outcome_hint, no audio field yet)
```

**Day anchor verdict (plan §2.1):** `day` = absolute campaign day, in both
consumers. Stage N becomes audible when `campaignDay >= fragments[N].day`.
This is the documented anchor; changing it would break all 48 signals.

---

## 4. Extension seams (Wave 0 exit gate)

| Concern | Owner | Seam to extend |
|---|---|---|
| Signal definitions + runtime state | `RadioDistressSystem` | additive DTO fields + resolver |
| Stage/text/clarity resolution | `RadioTuner` / `RadioPropagation` (duplicated selection) | extract one shared resolver, keep both call sites |
| Outcome hints | `DistressMessageFragment` | additive `outcome_hint` field |
| Response/ignore/trap/rescue events | `DistressRescueMissionManager` + `RadioDistressSystem` | subscribe existing events for trust deltas |
| Trust storage | NEW minimal `SignalTrustState` | rides radio save section (`RadioSave.cs`) |
| Follow-up scheduling | NEW pending queue in distress runtime | campaign-day math + ClaimedReceipts-style ledger |
| Audio cues | `AudioManager` / `AudioCueCatalog` | additive `audio_cue` fields; presentation-only bridge |
| Save | `RadioSave.cs` (radio section) | additive fields with legacy-safe defaults |
| Validation | data-integrity selftest pipeline | extend existing distress validation |
| Tests | `Ashfall.Core.Tests/Radio/` (249 cases) | add focused files, run via `scripts/run_test.sh` |

## 5. Risks confirmed against current source

- R1 (stage model duplication): **confirmed avoidable** — reuse `message_fragments` (PC-2).
- R2 (second reputation engine): **confirmed avoidable** — FactionWarSystem is faction-keyed only (PC-3); minimal scoped state instead.
- R11 (original 5 signals change unexpectedly): builtin fallbacks are fragment-less; JSON wins; parity fixtures required before any resolver consolidation.
- New R13: the two duplicated fragment-selection loops (RadioTuner / RadioPropagation) can drift; consolidation must preserve the propagation-specific clarity attenuation.
- New R14: expansion signals (23) have no builtin fallback and are loaded by the host session; stage contract tests must cover both catalogs.
- New R15 (found during Wave 1): cross-file duplicate IDs mean naive "duplicate signal ID" validation would fail the existing selftest; the primary-wins load order is the documented authority policy and must be preserved by any validation.
