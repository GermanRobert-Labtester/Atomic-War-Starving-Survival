// SPDX-License-Identifier: MIT
# DISTRESS SIGNAL PR3 CONTENT SEAL CLOSEOUT

> Status: **SEALED — CF-P1-DISTRESS-CONTENT-SEAL (2026-09-19).**
> Scope is verify-and-seal only: authored follow-up validation, population
> replay, host cue cross-reference, utilization verification, and governance.
> The scheduler, trust ledger, audio resolver, V6 save shape, and panel route
> were not changed.

## 1. Scope and validator rules

The permanent `CatalogIntegrityValidator` gate now applies three additive,
error-only rules after the existing follow-up structural checks:

| Rule | Contract | Fixture coverage |
|---|---|---:|
| `distress_followup_expired_requires_consequence` | `expired` requires a positive authored `deadline_days`/`deadlineDays` and a non-trap identity | V-01–V-09 (9) |
| `distress_followup_trap_only_on_lures` | `ambush_encountered` requires trap-class identity | V-10–V-14 (5) |
| `distress_followup_max_two` | no signal may author more than two follow-up entries | V-15–V-17 (3) |

V-18 confirms an invalid trigger produces only the existing grammar error;
semantic rules do not double-report malformed rows. V-19 pins both shipped
catalogs clean after remediation. The extended validator file passed **39/39**.
The rules emit no warnings, so the five primary-wins warnings in the dead-data
register remain unchanged.

## 2. P0 census and post-remediation population

The read-only P0 census found 48 catalog rows (25 primary, 23 expansion),
43 effective identities after the documented primary-wins merge, and 24
signals carrying follow-ups. Before remediation there were 43 entries:
17 `answered`, 16 `rescue_success`, 7 `ambush_encountered`, 3 `expired`,
and 0 `rescue_failed`; 21 entries had cues and 22 were text-only.

The sealed post-remediation census is 24 signals and 40 entries: 15 answered,
16 rescue-success, 7 ambush, 2 expired, 0 rescue-failed; 18 cued and 22
text-only. This is the complete replay table used by the drift guard:

| # | Catalog | Signal | Follow-up | Trigger | Delay | Cue |
|---:|:--:|---|---|---|---:|---|
| 1 | P | freq_distress_217_4 | fu_217_4_answered | answered | 2 | radio_distress_beacon |
| 2 | P | freq_distress_217_4 | fu_217_4_rescue_success | rescue_success | 4 | radio_distress_beacon |
| 3 | P | freq_distress_148_2 | fu_148_2_trap_fallen_for | ambush_encountered | 1 | radio_static |
| 4 | P | freq_distress_55_1 | fu_55_1_answered | answered | 2 | radio_vinyl_broadcast |
| 5 | P | freq_distress_401_9 | fu_401_9_answered | answered | 2 | radio_distress_beacon |
| 6 | P | freq_distress_401_9 | fu_401_9_rescue_success | rescue_success | 4 | radio_distress_beacon |
| 7 | P | freq_distress_88_3 | fu_88_3_answered | answered | 2 | text-only |
| 8 | P | freq_distress_88_3 | fu_88_3_rescue_success | rescue_success | 5 | text-only |
| 9 | P | freq_distress_156_8 | fu_156_8_answered | answered | 2 | text-only |
| 10 | P | freq_distress_156_8 | fu_156_8_rescue_success | rescue_success | 4 | text-only |
| 11 | P | freq_distress_203_1 | fu_203_1_answered | answered | 2 | radio_distress_beacon |
| 12 | P | freq_distress_203_1 | fu_203_1_rescue_success | rescue_success | 4 | radio_distress_beacon |
| 13 | P | freq_distress_311_5 | fu_311_5_answered | answered | 2 | radio_distress_beacon |
| 14 | P | freq_distress_311_5 | fu_311_5_rescue_success | rescue_success | 4 | radio_distress_beacon |
| 15 | P | freq_distress_445_2 | fu_445_2_answered | answered | 2 | text-only |
| 16 | P | freq_distress_445_2 | fu_445_2_rescue_success | rescue_success | 5 | text-only |
| 17 | P | freq_distress_192_4 | fu_192_4_trap_fallen_for | ambush_encountered | 1 | text-only |
| 18 | P | freq_distress_410_7 | fu_410_7_trap_fallen_for | ambush_encountered | 1 | radio_static |
| 19 | P | freq_distress_288_1 | fu_288_1_trap_fallen_for | ambush_encountered | 1 | radio_static |
| 20 | P | freq_distress_333_6 | fu_333_6_trap_fallen_for | ambush_encountered | 1 | radio_static |
| 21 | P | freq_distress_478_2 | fu_478_2_trap_fallen_for | ambush_encountered | 1 | radio_static |
| 22 | P | freq_distress_812_5 | fu_812_5_rescue_success | rescue_success | 4 | radio_distress_beacon |
| 23 | P | freq_distress_812_5 | fu_812_5_expired | expired | 3 | radio_static |
| 24 | P | freq_distress_867_9 | fu_867_9_rescue_success | rescue_success | 4 | radio_distress_beacon |
| 25 | P | freq_distress_867_9 | fu_867_9_expired | expired | 3 | radio_static |
| 26 | P | freq_distress_901_2 | fu_901_2_answered | answered | 2 | text-only |
| 27 | P | freq_distress_901_2 | fu_901_2_rescue_success | rescue_success | 5 | text-only |
| 28 | E | freq_distress_726_5 | fu_726_5_answered | answered | 2 | text-only |
| 29 | E | freq_distress_726_5 | fu_726_5_rescue_success | rescue_success | 4 | text-only |
| 30 | E | freq_distress_609_4 | fu_609_4_answered | answered | 2 | text-only |
| 31 | E | freq_distress_609_4 | fu_609_4_rescue_success | rescue_success | 5 | text-only |
| 32 | E | freq_distress_455_7 | fu_455_7_answered | answered | 2 | text-only |
| 33 | E | freq_distress_455_7 | fu_455_7_rescue_success | rescue_success | 4 | text-only |
| 34 | E | freq_distress_555_0 | fu_555_0_answered | answered | 2 | text-only |
| 35 | E | freq_distress_555_0 | fu_555_0_rescue_success | rescue_success | 4 | text-only |
| 36 | E | freq_distress_380_2 | fu_380_2_trap_fallen_for | ambush_encountered | 1 | text-only |
| 37 | E | freq_distress_318_0 | fu_318_0_answered | answered | 2 | text-only |
| 38 | E | freq_distress_318_0 | fu_318_0_rescue_success | rescue_success | 5 | text-only |
| 39 | E | freq_distress_269_3 | fu_269_3_answered | answered | 2 | text-only |
| 40 | E | freq_distress_269_3 | fu_269_3_rescue_success | rescue_success | 4 | text-only |

## 3. Population replay results

`DistressFollowUpPopulationReplayTests.cs` drives all 40 authored entries
through the real mission/scheduler lifecycle and `RadioSaveCodec` V6. Each
row asserts trigger transition, due day, exact payload text, authored cue or
explicit text-only state, trust result, persisted fired key, save-before-due
restore, and no refire after save-after-fire restore. Rescue rows correctly
allow their sibling answered event; the assertion keys the exactly-once check
to the specific follow-up under test.

The focused file passed **46/46**. It also proves:

- answered: trust 52 (`+2`), rescue success: trust 57 (`+2`, then `+5`),
  ambush: trust 47 (`+2`, then `−5`), and expiry: trust 48 (`−2`);
- consequence-bearing expiry applies the sender-death consequence once and
  still schedules its authored follow-up;
- same-day pending entries fire in deterministic `(dueDay, parent, id)` order;
- five undiscovered signals remain silent, unscheduled, and trust-neutral;
- a removed V6 pending id expires without presentation; and
- two complete population fingerprint runs are identical.

## 4. Audio registry verification

The new `--audio-selftest` section reads both authoritative distress catalogs
and checks signal, fragment, and follow-up `audio_cue` values against the
existing `AudioCueCatalog`. The result was **645/645**, with seven distinct
referenced IDs and no missing resources or fallback failures:

| Cue id | Registry/resource result |
|---|---|
| `radio_static` | registered; resource resolves |
| `radio_distress_beacon` | registered; resource resolves |
| `radio_numbers_station` | registered; resource resolves |
| `radio_ebs_alert` | registered; resource resolves |
| `radio_morse` | registered; resource resolves |
| `radio_vinyl_broadcast` | registered; resource resolves |
| `radio_dead_hand_pulse` | registered; resource resolves |

Text-only summary: signals **43 cued / 5 text-only**, fragments **38 / 144**,
and follow-ups **18 cued / 22 text-only**. Missing cue behavior remains the
existing safety policy: log once, continue the text transmission, and never
mutate gameplay state.

## 5. Content-utilization result

`--content-utilization-selftest` passed its CI gate and deep-chain gate with
zero distress-catalog orphans/regressions. The generated report includes both
`radio_distress_signals.json` and `radio_distress_signals_expansion.json` as
consumed catalogs. The global inventory still reports nine unrelated orphaned
catalogs and 96 unresolved references; those pre-existing findings are outside
this package and are not a distress-layer failure.

## 6. Data fixes and before/after

The three violations were remediated in the primary JSON authority, with no
expansion-catalog or runtime-mechanism change:

| Removed entry | Before | Rule/reason | After |
|---|---|---|---|
| `fu_55_1_expired` | 43 entries; expired follow-up on `freq_distress_55_1` with no explicit positive response window | D1: expiry could not carry a declared consequence | removed; signal retains its answered follow-up |
| `fu_812_5_answered` | `freq_distress_812_5` authored 3 entries | D3: maximum two entries per signal | removed; rescue-success and expiry remain |
| `fu_867_9_answered` | `freq_distress_867_9` authored 3 entries | D3: maximum two entries per signal | removed; rescue-success and expiry remain |

After the edits, JSON parsing succeeds, the real-catalog validator pin is
clean, and `--data-integrity-selftest` reports **0 errors and exactly the
five pinned primary-wins warnings**. No warning count or dead-data register
entry changed.

## 7. Ledger row and ownership diff

`INTEGRATION_PLANS.md` now records `CF-P1-DISTRESS-CONTENT-SEAL` as sealed,
including the three rules, 40-entry replay, host cue cross-reference, and
focused/headless evidence. Its stale PR2 line now records that follow-up
payload and authored audio-cue content landed before this seal rather than
remaining deferred.

The claim `claim-cf-p1-distress-content-seal-2026-09-19` in
`WORKTREE_OWNERSHIP.md` is closed with the same verification summary. The
claim deliberately did not touch scheduler, save, trust, resolver, registry,
expansion-catalog, panel, or generated runtime architecture paths.

## 8. Limitations and deferred decisions

- After remediation, 18 of the 40 entries remain on signals without one of the
  12 authored runtime rescue missions. The replay injects harness missions
  through the existing public capture/restore seam to prove content/mechanism
  compatibility; it does not invent mission registration or claim runtime
  reachability. This remains a decision-needed content/reachability item.
- `freq_distress_445_2` declares `ignoreConsequence: sender_death` in JSON,
  while its authoritative mission fixture deliberately has no consequence
  tokens and uses legacy expiry. The fixture remains runtime authority; a
  future consistency rule needs an explicit decision.
- `freq_distress_88_9` has one legally optional stage hint omitted; no contract
  violation or content rewrite was made.
- Future follow-up/audio content tranches remain out of scope. No save-version
  bump, new save section, scheduler change, panel work, or second audio
  registry is part of this seal.

## Verification record

```text
bash scripts/run_test.sh Ashfall.Core.Tests/Radio/DistressFollowUpTests.cs
  PASS — 39/39
bash scripts/run_test.sh Ashfall.Core.Tests/Radio/DistressFollowUpPopulationReplayTests.cs
  PASS — 46/46
dotnet build Ashfall.csproj --no-restore
  PASS — 0 warnings, 0 errors
godot --headless --path . -- --data-integrity-selftest
  PASS — 0 errors, 5 pinned warnings
godot --headless --path . -- --audio-selftest
  PASS — 645/645
godot --headless --path . -- --content-utilization-selftest
  PASS — CI gate and deep-chain gate
```
