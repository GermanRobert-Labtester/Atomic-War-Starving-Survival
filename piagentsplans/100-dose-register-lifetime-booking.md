# Plan 100 — Dose Register Lifetime Booking

> **Theme:** Switch the dose register's cumulative bookkeeping **100% to
> lifetime exposure** — the unclamped, untreatable accumulator — so the
> register records what it claims to record: the exposure a body has
> accumulated over a career in the zone. Under the current acute-dial
> bookkeeping, `RadiationSystem` caps the acute dial at 100 mSv, so ordinary
> play can never book past the register's Pale rung. Under lifetime booking,
> Amber, Red, Black and above become reachable in ordinary long play.
>
> **Owner decision (locked):** 100% lifetime. No dual bookkeeping, no acute
> fallback.

---

## 0. Two-line goal

Make `DoseLedgerSystem` book lifetime-exposure increments (with the existing
12-rung ladder and save compatibility), and wire one production booking seam
into the real day-advance so ordinary play climbs the register.

## 1. Files to touch / create

```
Assets/Ashfall.Core/DoseLedgerSystem.cs          (new API + DoseEntry fields)
Assets/Ashfall.Core/DoseLedgerSave.cs            (envelope v4 + frozen V3 shape)
src/Host/DoseLedgerHostSession.cs                (seam + baseline change)
src/Main.cs / day-advance coordinator            (end-of-day booking for tagged survivors)
src/Main.UiTests.RealCampaignJourney.cs          (register segment → lifetime expectations)
src/Host/HostCli.PanelTests.cs                   (dose-ledger-selftest traversal framing)
Ashfall.Core.Tests/DoseLedgerSystemTests.cs      (lifetime booking + migration tests)
Ashfall.Core.Tests/DoseQuestOwnershipTests.cs    (v4 version pins)
Ashfall.Core.Tests/Radiation/Plan81DoseLocationsExpansionTests.cs  (lifetime API)
docs/bodymind/DOSE_REGISTER_STATE_MODEL.md       (§3 bookkeeping note)
docs/medical/PLAN_100_DOSE_REGISTER_LIFETIME_CLOSEOUT.md
```

**Read-only:** `Assets/Ashfall.Core/Radiation/RadiationSystem.cs` (lifetime truth),
`Assets/StreamingAssets/Data/dose_registers.json` (unchanged), `items.json`.

---

## 2. Verified evidence (all current)

| Fact | Reference |
|---|---|
| Acute dial caps at 100: journey booked 26.8 mSv then 0.0 (dial saturated); status line renders "dose 73.2/100" | `SurvivorsHostSession.cs:310-319`, journey log |
| Lifetime is the unclamped, untreatable accumulator: "not reduced by any treatment", survives save/reload exactly | `SurvivorRadState.LifetimeRadiationExposure` (RadiationSystem.cs:17), journey checks |
| Lifetime accrual: `LifetimeRadiationExposure = Max(0, … + radsPerHour * hours * 0.1f)`; delta path; floor path; chronic-illness gate at `ChronicLifetimeThreshold` | `RadiationSystem.cs:248-249, 257, 307, 269` |
| An existing dosimeter registry already mirrors lifetime onto tags: `dosimeter.LifetimeDose = survivor.LifetimeRadiationExposure` | `RadiationSystem.cs:194` |
| Ledger books nominal increments with flux/shielding/anti-rad modifiers; `cumulativeMsv` drives `BandOf` | `DoseLedgerSystem.cs:113-171, 173+` |
| Envelope is at v3 (v1 frozen shape); `SaveChecksum` walks ALL public fields — new `DoseEntry` fields change legacy hashes | `DoseLedgerSave.cs`, `SaveChecksum.cs:163-182` |
| Ordinary-play defect motivating this plan: journey traversal `Green → Pale → Pale` (acute cap) | `--real-campaign-journey-selftest` log |
| Ladder is catalog-driven (Plan 90B); data unchanged by this plan | `DoseLedgerSystem.ConfigureLadder`, `dose_registers.json` |

---

## 3. Hard constraints

1. **No new radiation physics.** Lifetime accrual stays 100% owned by `RadiationSystem`. The ledger only *reads* it.
2. **No data changes.** Band IDs, thresholds, labels, plans, NPCs untouched.
3. **Determinism.** `ISeededRng` only; same save + same seed → identical register.
4. **Save compatibility.** v1/v2/v3 dose saves load with correct band standing; no unjustified down-classification.
5. **Core stays engine-agnostic**; new state ships `CaptureState/RestoreState`; additive DTO fields only.
6. **One system per task, small batches** (shared tree — concurrent Plan 91/101 edits observed; rebase between batches).

---

## 4. Design decisions (locked)

### 4.1 Semantic
`DoseEntry.cumulativeMsv` ≡ **the lifetime burden known to the register** —
"what this dosimeter's log says this body took, ever." Band = `BandOf(cumulativeMsv)`
(unchanged derivation against the catalog ladder). Anti-rad, healing, and
treatment never lower it (lifetime is untreatable by definition).

### 4.2 New Core API — the only production booking path
```csharp
public DoseBandResult BookReadingFromLifetime(
    string survivorId, int day, float lifetimeNowMsv,
    string source, bool highEnergyEvent, ISeededRng rng)
```
- `delta = max(0, lifetimeNowMsv − entry.lastLifetimeMsv)`; untagged → `NoEntry` (parity with `BookReading`).
- `booked = flux(delta)` — the existing high-energy ambiguity (0.85–1.15×) is the only modifier: it models **measurement drift of the dial**, nothing else. Shielding and anti-rad do **not** apply (shielding is already baked into the physical accrual; anti-rad does not touch lifetime by definition).
- **First booking per entry (reconciliation):** `cumulativeMsv = Max(cumulativeMsv, flux(lifetimeNowMsv))` — supersedes acute-era cumulative with the true lifetime total, never down-classifies. Sets `lifetimeBookkeeping = true`.
- Subsequent bookings: `cumulativeMsv += booked`; `lastLifetimeMsv = lifetimeNowMsv`.
- Records a `DoseReading` (nominal = delta, booked = flux(delta), source freeform or `loc_` id) and fires `OnDoseCorrected` / `OnBandReached` / `RaiseChanged` exactly like `BookReading`.

### 4.3 Legacy path
`BookReading` (acute nominal) is retained, doc-marked legacy, for the pure-nominal
callers that have no radiation system (headless selftest traversal, synthetic
tests). **Convention:** production callers never mix the two paths for one
entry; a test pins that a lifetime-booked entry ignores acute re-books' drift
(documented, not hard-blocked).

### 4.4 `DoseEntry` additive fields
`float lastLifetimeMsv` (default 0), `bool lifetimeBookkeeping` (default false).
Captured/restored; defaults keep pre-v4 loads semantically valid until the
first runtime booking reconciles them.

### 4.5 Tagging baseline
`AssignDosimeter(survivorId, tag, baselineMsv)` — production callers pass the
survivor's current `LifetimeRadiationExposure` (journey: 173.6 → the survivor
lands on Orange immediately: inherited burden, "never zeroed" — the field's
existing comment already says exactly this).

### 4.6 Production seams (host)
- **Day-advance coordinator (the one seam):** at end of `TickSimDay`, for every
  tagged survivor, book `BookReadingFromLifetime(..., lifetimeNow, "daily_tick", ...)`.
  Ordinary play then climbs the register with zero additional UI.
- **Voluntary completion:** stays as-is — its `doseIncurred` is register-internal;
  the lifetime delta from the exposure that earned it arrives via the day tick.
  **No double-booking.**
- **Expedition returns:** covered by the day tick (accrual happens during
  expedition days). No separate booking.

### 4.7 Envelope v4 (required — legacy hashes otherwise break)
- `CurrentSaveVersion = 4`; add frozen `DoseLedgerSaveV3` (today's exact shape) —
  legacy v3 payloads validate against it; v1 keeps `DoseLedgerSaveV1`.
- Migrations: v1/v2 → existing band remap **plus** new-field defaults; v3 →
  new-field defaults. Reconciliation happens at first runtime booking (lifetime
  state lives in the survivors section — unavailable to the codec).
- Campaign section registry: unchanged (inner `saveVersion` governs).

### 4.8 Pacing implication (documented, not tuned here)
Lifetime accrues at 0.1× the acute rate (`RadiationSystem.cs:248`): a 30 mSv/hr
shift ≈ +3 mSv lifetime. Amber (100) ≈ ~33 exposure-shift days, Red (300) ≈
~100, Black (600) ≈ ~200. That is the intended long-play arc: the register
becomes a career record. Rate tuning belongs to data/balance, not this plan.

### 4.9 Non-goals
No threshold/ID changes; no coupling to `ChronicLifetimeThreshold` (biological
gate — the register is administrative); no UI redesign (panel anchors now align
*better* — the acute cap no longer flattens the display); no new selftest verbs.

---

## 5. Tasks

### Task 100A — Core: lifetime booking API + state
`DoseEntry` fields; `BookReadingFromLifetime` (reconciliation max-rule, flux-only,
NoEntry parity, events); capture/restore round-trip. Tests: delta booking;
cap-immunity (survivor at acute 100 still books lifetime); first-booking
reconciliation (old cumulative 26.8 + lifetime 173.6 → Orange, never down);
flux-only modifiers (anti-rad flags absent by design); untagged NoEntry;
determinism (same inputs → same band).

### Task 100B — Envelope v4
Frozen `DoseLedgerSaveV3`; version bump; migrations (v1/v2 band remap preserved;
new-field defaults); tests: v3 round-trip, v1 genuine-payload migration,
v4 round-trip, tamper rejection on all versions.

### Task 100C — Host seams
Day-advance booking for tagged survivors; `SealDemoSurvivors`/journey baselines
= lifetime; journey register segment reworked: expect **Amber+ reachable in
ordinary play** (baseline reconciliation lands the survivor ≥ Orange; assert
monotonic climb + administrative-vs-career-run comment); `--dose-ledger-selftest`
traversal reframed (lifetime deltas — existing synthetic 15 mSv readings remain
valid via the legacy path, relabeled).

### Task 100D — Callers, docs, closeout
Plan-81 location tests → lifetime API (source ids preserved); `ScribeReading`
demo re-routed; `DOSE_REGISTER_STATE_MODEL.md` §3 (two-ledger diagram gains the
lifetime booking rule); closeout with before/after traversal evidence.

---

## 6. Verification

```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # full suite green
dotnet build Ashfall.csproj                                # 0 errors, 0 warnings
godot --headless --path . -- --data-integrity-selftest     # 0 errors
godot --headless --path . -- --dose-ledger-selftest        # traversal + save battery
godot --headless --path . -- --real-campaign-journey-selftest  # register segment: Amber+ in ordinary play
godot --headless --path . -- --save-store-checksum-selftest    # Gate A
godot --headless --path . -- --campaign-fuzz-selftest      # save fuzz across versions
godot --headless --path . -- --content-utilization-selftest
```

## 7. Risks

| Risk | Mitigation |
|---|---|
| Legacy saves double-count or down-classify | Reconciliation max-rule at first booking; envelope v4 with frozen V3 shape; never down-classify |
| Dual booking paths drift | Single production path (lifetime); legacy path doc-marked; test pins the convention |
| Pacing too slow/fast | 0.1× accrual documented; tuning is a data/balance follow-up, not code |
| Shared-tree churn (concurrent plans) | Small batches; rebuild + re-run gates between batches |
| Zero-delta bookings spam history | `delta ≤ 0` → NoEntry (no phantom rows) — parity with existing guard |

## 8. Definition of Done

- [ ] `BookReadingFromLifetime` is the only production booking path; reconciliation proven
- [ ] Survivor at the acute cap still climbs rungs from lifetime deltas
- [ ] Envelope v4 ships; v1/v2/v3 saves load with correct band standing
- [ ] Day-advance books tagged survivors; journey proves Amber+ in ordinary play
- [ ] Data file byte-identical to Plan 90B state; ladder untouched
- [ ] All §6 gates green; closeout written with traversal evidence
