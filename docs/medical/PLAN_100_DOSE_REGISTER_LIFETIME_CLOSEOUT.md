# PLAN 100 — DOSE REGISTER LIFETIME BOOKING — CLOSEOUT

**Final status: ✅ COMPLETE (batches 100A–100D).**

The dose register's cumulative bookkeeping is **100% lifetime exposure**: the
unclamped, untreatable accumulator. The acute dial (0–100 mSv, reduced by
anti-rad) is treatment triage — the register no longer tracks it, so Amber,
Red, Black and above are reachable in ordinary long play.

---

## 1. Before / after — the motivating defect, fixed

| | Acute-dial booking (pre-100) | Lifetime booking (100) |
|---|---|---|
| Journey traversal | `Green → Pale → Pale (26.8 mSv)` — plateaued at the acute 100 mSv cap; shift 2 booked 0.0 | `Orange → Rose (203.6 of 203.6 mSv lifetime)` — tagged at inherited burden 173.6 → Orange; one ordinary 30 mSv shift → Rose |
| Upper rungs | unreachable in ordinary play (dial caps at 100) | reachable; rungs climb with the career ledger |
| Selftest ladder | 12 rungs via nominal increments | 12 rungs via lifetime deltas (`--dose-ledger-selftest`, Green→Slate, identical boundary values) |

## 2. What shipped

### Batch 100A — Core (`DoseLedgerSystem.cs`)
- `DoseEntry.lastLifetimeMsv` / `lifetimeBookkeeping` (additive, round-tripped).
- `BookReadingFromLifetime(survivorId, day, lifetimeNowMsv, source, highEnergyEvent, rng)`:
  books the delta since the last lifetime mark; **flux-only** distortion
  (measurement drift — shielding is pre-applied physically, anti-rad doesn't
  touch lifetime); **first-booking reconciliation max-rule**
  (`cumulative = Max(cumulative, flux(lifetimeNow))` — never down-classifies);
  zero-delta → `NoEntry` (no phantom rows); full event/parity semantics.
- Legacy `BookReading` retained (doc-marked) for pure-nominal callers.

### Batch 100B — Envelope v4 (`DoseLedgerSave.cs`)
- `CurrentSaveVersion = 4`; frozen `DoseEntryV3` / `DoseLedgerSystemStateV3` /
  `DoseLedgerSaveV3` graphs (pre-100 field sets) so legacy checksum validation
  never sees the new fields; `DoseLedgerSaveV1`'s nested dose state moved onto
  the frozen graph (closing the v1 exposure 100A opened).
- Migrations: v1 → promote + band remap; v2 → promote + band remap; v3 →
  promote only (already 12-rank). Lifetime fields default; reconciliation is a
  runtime event (lifetime truth lives in the survivors section).

### Batch 100C — Host seams
- `DoseLedgerDayOwner` registered as `dose_ledger` (**phase 4** — after the
  phase-3 survivors tick): end-of-day, every tagged survivor books their
  lifetime delta (`daily_tick`, deterministic — no RNG on ambient readings).
  Delta-based → naturally idempotent, no snapshot needed.
- Journey: survivor tagged with `LifetimeRadiationExposure` as baseline
  (inherited burden → Orange); one ordinary `ExposeToZone` shift booked.
- `--dose-ledger-selftest` traversal → lifetime deltas.

### Batch 100D — Callers, docs, closeout
- Plan-81 location tests → `BookReadingFromLifetime` (location sources
  preserved; attribution + round-trip assertions unchanged).
- `ScribeReading` (and the register surface's book button through it) →
  lifetime path: the parameter is the dosimeter's lifetime total; the register
  books the increment since the last reading.
- `DOSE_REGISTER_STATE_MODEL.md` §3 — two-ledger diagram + lifetime booking
  rule, drift-only distortion, reconciliation, daily seam.

## 3. Guardrails honored (no false medicine)

Unchanged from Plan 90/90B: no KI prophylaxis, no generic chelation, no
decontamination, no isolation, no transfer — and no new radiation physics
(lifetime accrual stays 100% owned by `RadiationSystem`). The register remains
an honest ledger: it records exposure and commits care resources; it never
cures, never pronounces prognosis, never blocks harmless work.

## 4. Pacing (empirical, from the journey)

`ExposeToZone` books the **full** amount to lifetime (the 0.1× factor at
`RadiationSystem.cs:248` is the ambient/day-tick path). One ordinary 30 mSv
shift ≈ +30 lifetime → Amber ≈ 4 shifts, Red ≈ 10, Black ≈ 20, Slate ≈ 33 from
zero. Survivors tagged with pre-existing burden start at their earned rung
(journey: 173.6 → Orange). Rate tuning, if wanted, is a data/balance follow-up.

## 5. Verification (final)

| Gate | Result |
|---|---|
| `dotnet test Ashfall.Core.Tests` | **7094/7094** (lifetime unit battery, v1–v4 migration matrix, Plan-81 lifetime migration, journey/smoke coverage) |
| `dotnet build Ashfall.csproj` | 0 errors, 0 warnings |
| `--real-campaign-journey-selftest` | PASS — `Orange → Rose (203.6 mSv lifetime)` |
| `--dose-ledger-selftest` | PASS — 12-rung lifetime traversal + save battery |
| `--save-store-checksum-selftest` | PASS — 21/21 gates (frozen-graph legacy validation) |
| `--campaign-fuzz-selftest` | PASS |
| `--7-day-smoke-selftest` | PASS — day-owner through real day advances |
| `--data-integrity-selftest` | PASS — 208/208 catalogs, 0 errors |

*(One transient suite run showed 21 failures from concurrent Plan 91/94 work
landing mid-write in the shared tree; the immediate re-run was fully green.)*

## 6. Follow-up debt (non-blocking)

- `DOSE_INSTITUTION_CONSEQUENCE_MATRIX.md` rows are lifetime-consistent as
  written; if pacing feels hot/cold in playtests, tune `RadiationSystem`
  accrual factors — never the register.
- `DiseaseTriage.SickBandFor` still maps illness stages onto the anchor rungs
  (Ill→Amber, Terminal→Red, OutcomePending→Black) — fine-grained illness
  triage on the new rungs remains optional design work.
- `plan_morphine_tray` demo surface (`OnAssignMorphine`) is unchanged demo
  content; the sick-list auto-writer (`DiseaseTriage.PalliativePlanFor`) is
  untouched and now coexists with lifetime bookings without interaction.
