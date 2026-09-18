# Wave 9 Part 2 C2 — SignalTrust Availability Consumer Decision

## Decision ID

`WAVE9-PART2-C2-SIGNALTRUST-AVAILABILITY`

## Header

- **Source blocker:** `SIGNAL_TRUST_CONTRACT.md` §5 / `INTEGRATION_PLANS.md`: "Availability consumer (deferred with evidence): no dynamic distress-signal selection pool exists in the runtime; `SignalTrustAvailability` is the tested policy awaiting that future seam."
- **Current HEAD:** Commit `HEAD` (`Assets/Ashfall.Core/Radio/SignalTrustAvailability.cs`, `RadioTuner.cs`, `RadioDistressSystem.cs`).
- **Current owner:** `RadioDistressSystem` / `RadioTuner` (signal frequency and lock authority); `RadioHostSession` (session & trust ledger owner).
- **Current measurement/behavior:** `RadioTuner.EvaluateFrequency` matches the player's dial frequency directly against authored signal frequencies in `radio_distress_signals.json` via `distress.FindSignalAtFrequency(frequencyMhz, 0.5f)`. All 43 signals reside on static, authored frequencies. No dynamic signal spawner, random candidate selector, or daily availability pool exists in ASHFALL runtime code. `SignalTrustAvailability` is a pure static helper tested in `SignalTrustTests.cs` with zero production call sites.

---

## Why a Decision is Required

`SignalTrustAvailability` defines an integer-permille weighting function (`GenuineModifierPermille` [500, 1500]‰, `TrapModifierPermille` [500, 1500]‰) designed to weight candidates in a selection pool.
However, in ASHFALL, radio discovery is not a procedural dice roll; it is an active player activity: the player tunes a physical dial across frequencies (e.g. 217.4 MHz, 148.2 MHz) to intercept transmissions.

We must decide whether to:
1. Invent a dynamic candidate selection pool that actively spawns/de-spawns distress signals based on trust (Option A); or
2. Formally retire/tombstone the dormant selection-pool policy from active blocker accounting, documenting `SignalTrustAvailability` as a reference model or dormant utility while keeping tests as contract pins (Option B).

---

## Options Analysis

### Option A — Build Trust-Weighted Availability Pool
- **Behavior:** Implement a dynamic signal activation manager that gates which distress frequencies broadcast on any given day, drawing candidates weighted by `SignalTrustAvailability.ModifyCandidates`.
- **Owner:** `RadioDistressSystem` / `RadioHostSession`.
- **Persistence:** None (derived per day/seed).
- **Determinism:** Requires a dedicated day-keyed RNG stream (`CampaignRngStream`).
- **UI/Content Impact:** Signals would appear and disappear from the spectrum dynamically. May confuse players who noted a frequency on day 3 but find it absent on day 4.
- **Compatibility:** Significant change to radio game loop.
- **Test Impact:** Requires extensive replay tests, continuous vs restored run proofs, and non-coupling proofs with `DistressFollowUpScheduler`.
- **Rollback Risk:** High architectural impact.

### Option B — Retire / Tombstone Dormant Availability Policy (RETIRED / DECIDED-DEFERRED)
- **Behavior:** Keep existing static-frequency continuous tuning behavior intact. Formally update `SIGNAL_TRUST_CONTRACT.md` §5 and `INTEGRATION_PLANS.md` to change status from "awaiting future seam" to "DORMANT / RETIRED (no dynamic pool in radio design)".
- **Owner:** Documentation / contract only.
- **Persistence:** None.
- **Determinism:** Unchanged.
- **UI/Content Impact:** None. Diegetic manual dial tuning remains authoritative.
- **Compatibility:** 100% compatible.
- **Test Impact:** Retain `SignalTrustTests.cs` (or mark as contract specification tests) to prevent regression on the pure math.
- **Rollback Risk:** Zero. Removes longstanding ambiguous ledger debt.

---

## Architecture-Safe Recommendation

**Option B (Retire / Tombstone Dormant Availability Policy)**.
The manual tuner dial (`RadioTuner`) and static frequency spectrum (`radio_distress_signals.json`) are core aesthetic pillars of ASHFALL's 1980s post-nuclear atmosphere. Synthesizing a hidden procedural spawn pool to justify a dormant weighting helper would undermine the diegetic tuning mechanic. Retiring the "awaiting future seam" blocker is the correct, architecture-preserving resolution.

---

## Foreman Signature Gate

- **Chosen Option:** [PENDING FOREMAN DECISION]
- **Signer:** [User / Foreman]
- **Date:** [YYYY-MM-DD]
- **Conditions:**
  1. If Option B: `SIGNAL_TRUST_CONTRACT.md` §5 updated to DORMANT/RETIRED; `INTEGRATION_PLANS.md` closed.
  2. If Option A: Selection pool must be derived purely from persisted trust + day/seed without new save sections, and must not mutate `DistressFollowUpScheduler` state.
