// SPDX-License-Identifier: MIT
# SIGNAL TRUST CONTRACT (Tasks 9–12 Wave 2)

> Status: **SEALED — Wave 2.** Live authority for distress-signal trust.
> Baseline evidence: `docs/radio/DISTRESS_SIGNAL_TASKS_9_12_BASELINE.md` (PC-3).

## 1. What signal trust is

A **radio-specific credibility/history metric** — the shelter's accumulated
track record with distress calls. It is NOT a second reputation system:

- never faction standing (`FactionWarSystem` is faction-keyed only and keeps
  its own semantics; standing deltas from rescue/ignore outcomes are the
  pre-existing Plan 24 behavior and are unchanged);
- never NPC relationship, quest reputation, morality, or fame;
- never a global player-reputation aggregate.

Narrative frame: receiver/network credibility — the shelter's growing radio
competence and standing among legitimate callers.

## 2. Storage (decision gate outcome: fallback path B)

`SignalTrustLedger` (`Assets/Ashfall.Core/Radio/SignalTrustLedger.cs`):

- data-only, engine-free, deterministic, owned by the radio/distress runtime;
- integer score bounded **[0, 100]**, neutral default **50**;
- exactly-once per-signal event ledgers (first event wins — mirroring the
  `ClaimedReceipts` pattern), so overlapping paths can never double-count;
- persisted as `RadioSaveState.signalTrust` (V5, checksum-covered);
- old saves migrate to an explicit neutral default (V4→V5 frozen-shape
  migration; no reconstructed history from incomplete data).

## 3. Centralized delta policy (`SignalTrustPolicy`)

| Event | Delta | Authority |
|---|---|---|
| Answered (authoritative expedition dispatch) | +2 | `DistressRescueMissionManager.RecordExpeditionDispatched` |
| Rescue successful (arrival in time) | +5 | `RecordDestinationReached` → `TerminalRescued` |
| Ignored (actionable expiry or explicit decline) | −2 | `TickDaily` expiry paths / `RadioDistressSystem.ResolveMoralChoice` ignore branch |
| Trap fallen for (ambush actually encountered) | −5 | `RecordDestinationReached` → `TerminalAmbush` |

No event handler may scatter numeric trust changes; deltas exist only in
`SignalTrustPolicy`.

## 4. Event definitions (normative)

- **Answered** — the player performed the authoritative response action (an
  expedition dispatch). Tuning, panel opens, discovery, and moral-choice
  rescue declarations never count.
- **Ignored** — the signal was discovered and actionable AND the player
  explicitly declined OR the authored response window expired unanswered.
  Undiscovered signals never count. **Trap-class signals are excluded**:
  the authored design states "ignoring a lure is not a failure".
- **Trap fallen for** — the player dispatched into an authored trap and the
  arrival resolved the ambush (`TerminalAmbush`). Detecting or analyzing a
  trap never counts; surviving the ambush afterwards adds no second event.
- **Rescue successful** — the rescue authority resolved `TerminalRescued`
  (arrival in time). Late arrivals (`TerminalFailed`) count only as answered.

The ledger is bound inside `DistressRescueMissionManager` (optional ctor
parameter) and `RadioDistressSystem.ResolveMoralChoice` (optional parameter,
mirroring the `factionWar` pattern). `RadioHostSession` owns the ledger for
the default wiring and persists it in the radio save section.

## 5. Availability policy (`SignalTrustAvailability`)

- integer permille modifiers — no floating-point drift;
- genuine modifier = `500 + score×10` (neutral 1000‰, range [500,1500]);
- trap modifier = `1500 − score×10` (mirror, same bounds);
- **bounded**: no category can ever reach zero weight or dominance —
  genuine signals never become impossible, traps never become guaranteed;
- modifiers apply to FUTURE candidate weighting only; an already-active
  signal's authored authenticity is never changed retroactively;
- candidates must be sorted by stable signal ID before weighting (caller
  contract); `ModifyCandidates` preserves order and is side-effect free;
- **current architecture note**: the distress catalog is statically tunable —
  no dynamic candidate-selection pool exists in the runtime yet. This API is
  the tested policy the future selection consumer (Task 11+ availability
  wave) must call; no scan mechanic was invented.

## 6. Save contract

| State | Saved | Migration |
|---|---|---|
| Counters + score | yes (`signalTrust`) | old saves → neutral (score 50, zero counters) |
| Per-signal event ledgers | yes | old saves → empty |
| Faction standing | unchanged owner | unchanged |

`RadioSaveCodec` V4→V5: frozen `RadioSaveStateFrozenV4` shape validates the
old checksum (the trust field never participated in V4 hashes), then rebuilds
as V5 with an explicit neutral `SignalTrustSaveEntry`.

## 7. Tests

`Ashfall.Core.Tests/Radio/SignalTrustTests.cs` (21 cases): answered/ignored/
trap/rescue deltas, undiscovered exclusion, trap-ignore exclusion, late
arrival, exactly-once (repeat transitions, explicit+expiry overlap,
ambush-survival), clamping, determinism, ledger round-trip, V5 codec
round-trip, V4→V5 migration, availability bounds/monotonicity/ordering, and
the authenticity-immutability guard.
