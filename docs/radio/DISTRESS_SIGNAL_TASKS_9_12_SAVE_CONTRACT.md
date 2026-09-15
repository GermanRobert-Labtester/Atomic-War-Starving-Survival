// SPDX-License-Identifier: MIT
# DISTRESS SIGNAL TASKS 9–12 SAVE CONTRACT

> Status: **SEALED — Wave 5.** Consolidated save audit for the Tasks 9–12
> lifecycle. Radio save codec is at **V6** (`RadioSaveCodec.CurrentSaveVersion`).

## 1. State ownership table (plan §12)

| State | Saved directly? | Derived? | Existing owner | Migration |
|---|---|---|---|---|
| First detected day | yes (`interceptedDay` on DistressSignalSaveEntry / mission `InterceptedDay`) | no | `RadioDistressSystem` / mission manager | pre-existing |
| Current stage | **no** | yes — pure function of `definition + campaignDay` | `DistressStageResolver` | n/a (never stored) |
| Stage text / clarity / hint | no | yes | `DistressStageResolver` | n/a |
| `HighestClarity` | yes (pre-existing field) | monotonic cache | `RadioDistressSystem` | pre-existing |
| Trust counters | yes (`SignalTrustSaveEntry`) | no | `SignalTrustLedger` | V4→V5: explicit neutral (score 50, zero counters) |
| Trust score | yes | no | `SignalTrustLedger` | V4→V5: neutral |
| Per-signal trust ledgers | yes (id lists) | no | `SignalTrustLedger` | V4→V5: empty |
| Pending follow-ups | yes (`SignalFollowUpSaveState.pending`, due = absolute campaign day) | no | `DistressFollowUpScheduler` | V5→V6: empty |
| Fired follow-up IDs | yes (`firedKeys`) | no | `DistressFollowUpScheduler` | V5→V6: empty |
| Current audio cue | **no** | yes — `definition + stage → cue` | `DistressAudioCueResolver` | n/a (never stored) |
| Detection-cue dedupe | yes (`playedBroadcastKeys`, key `distress:{id}:{cue}`) | no | `RadioHostSession` | pre-existing ledger |

## 2. Mandatory properties (verified by test)

- **Deterministic serialization order** — capture methods sort (signals by
  ID ordinal, pending by `(dueDay, parent, followUpId)`, fired keys ordinal);
  the checksum walks public fields in ordinal name order.
- **No duplicate follow-up after reload** — fired ledger persisted;
  `FiredFollowUpDoesNotRefireAfterReload` + Scenario B.
- **No duplicate trust event after reload** — exactly-once per-signal
  ledgers persisted; `TrustSurvivesSaveLoad`.
- **Stage does not regress** — stages are day-derived and campaign time is
  monotonic; nothing to regress.
- **Old saves remain loadable** — frozen `RadioSaveStateFrozenV4`/`V5`
  shapes validate the old checksums, then rebuild additively
  (`MigrateV4`, `MigrateV5`).
- **Checksum integrity** — every new field participates in the V6 checksum;
  older versions are validated against their own frozen shape before
  migration (tampered saves are rejected at every version).

## 3. Version history

| Version | Batch | Added |
|---|---|---|
| V4 | rescue-signal runtime | mission state, receipts, fingerprint |
| V5 | Tasks 9–12 Wave 2 | `signalTrust` (SignalTrustSaveEntry) |
| V6 | Tasks 9–12 Wave 3 | `signalFollowUps` (pending + firedKeys) |

Wave 4 added no save fields (audio cue is derived; dedupe rides the
pre-existing `playedBroadcastKeys` ledger).
