// SPDX-License-Identifier: MIT
# DISTRESS SIGNAL TASKS 9–12 CLOSEOUT REPORT

> Status: **COMPLETE — Waves 0–5.** Flagship integration delivered 2026-09-13.
> Plan: `DISTRESS SIGNAL TASKS 9–12` (user-authorized). Evidence chain:
> baseline → four wave contract docs → this closeout.

```text
Baseline signal count:            25 (plan premise)
Final signal count:               48 catalog rows / 43 unique effective
                                  identities / 47 runtime-registered
                                  (incl. 4 builtin-only fragment-less
                                  fallbacks) — scope correction recorded
                                  in the baseline doc (PC-1)

Stage authority:                  message_fragments (reused; no `stages`
                                  field added — Wave 0 decision gate PC-2)
Resolver:                         DistressStageResolver (sole selection
                                  authority; both legacy consumers delegate)

Signals with multi-stage stages:  43/43 JSON-backed signals carry 2–7
                                  ascending stages; 4 builtin fallbacks
                                  stay fragment-less by design
25 total audit count:             superseded by the 43-identity audit
                                  (legacy-parity oracle, days 0–60)
Stage validation result:          inside the permanent data-integrity gate —
                                  PASS (5 documented primary-wins warnings,
                                  0 errors)

Trust implementation:             minimal SignalTrustLedger (fallback path B
                                  per baseline PC-3 — FactionWarSystem is
                                  faction-keyed only; standing untouched)
Score range:                      [0, 100], integer, neutral 50, clamped
Event deltas:                     +2 answered / +5 rescue / −2 ignored /
                                  −5 trap (centralized in SignalTrustPolicy)
Availability policy:              integer permille, bounded [500,1500],
                                  monotonic, order-preserving; consumer
                                  deferred WITH EVIDENCE (no dynamic
                                  selection pool exists in the runtime)

Follow-up implementation:         DistressFollowUpScheduler (radio-owned)
scheduler owner:                  RadioHostSession (day-aware tick ordering)
pending count model:              absolute campaign-day due dates
dedupe model:                     parentSignalId + followUpId, exactly-once
                                  pending set + persisted fired ledger
cycle validation:                 structurally impossible (self-contained
                                  payloads; catalog-backed refs would
                                  require cycle validation first — documented)
Authored follow-up content:       none yet (mechanism-first per user's
                                  Wave 1 option-c decision)

Audio implementation:             DistressAudioCueResolver → existing
                                  AudioManager (no second audio system)
AudioManager path:                PlayCue via AudioCueCatalog (host-side)
cue field:                        audio_cue on definition + fragment +
                                  follow-up (additive, empty = text-only)
stage override model:             plan Option A (no clarity-band system)
missing-cue policy:               structural validation in the integrity
                                  gate; semantic resolution deferred to the
                                  audio selftest when content lands;
                                  missing cue logs once, text continues
Detection gate:                   Intercept edge only; undiscovered signals
                                  are silent; dedupe rides the existing
                                  persisted playedBroadcastKeys ledger
                                  (zero new save fields)

Save migration:                   V4→V5 (trust, neutral default) → V6
                                  (follow-ups, empty default); frozen-shape
old save behavior:                checksum-validated at each version;
                                  tampered saves rejected
new fields:                       signalTrust, signalFollowUps
round-trip result:                codec tests + Scenario B byte-for-byte

Determinism:
seed(s):                          fixture seed 42 (harness constant)
trace comparison:                 day-level field trace (stage, clarity,
                                  text/hint hashes, cue, trust, pending,
                                  fired) + full V6 save payload fingerprint
result:                           continuous == interrupted == rerun

Data integrity:                   PASS — 0 errors, 325 catalogs
dotnet test:                      PASS — 11,170/11,170 (full suite,
                                  dedicated Wave 5 window)
dotnet build:                     PASS — 0 errors, 0 warnings
content-utilization:              PASS — 0 orphans, CI gate PASS
panel lifecycle:                  PASS
audio selftest:                   PASS

Known limitations:
Deferred audio assets:            no authored audio_cue content yet; the
                                  resolver, validator, and detection gate
                                  are live and will gate authored content
Deferred content:                 outcome_hint text + follow-up payloads +
                                  audio cues for the 43 signals (PR 2
                                  tranche, user-approved deferral)
Deferred consumer:                trust availability weighting awaits a
                                  dynamic selection seam (documented in
                                  SIGNAL_TRUST_CONTRACT.md §5)
Not owned by this batch:          RadioPanel presentation of follow-ups
                                  (existing strip surfaces them via the
                                  session event line; a dedicated strip is
                                  a future presentation wave)
```

## Flagship Definition of Done (plan §23)

- **Multi-stage progression** — all boxes satisfied (message_fragments
  documented as the authority; no redundant stage model; deterministic
  resolver; save/load preserves progression via derivation; original
  signals retain compatibility — legacy-parity oracle).
- **Signal trust** — all boxes satisfied (existing reputation evaluated
  first and rejected with evidence; explicit event definitions; undiscovered
  signals cannot count; centralized bounded deltas; availability policy
  tested; deterministic replay passes).
- **Follow-up chaining** — all boxes satisfied (stage/follow-up separation
  tested; stable identities; validated grammar; campaign-day delays;
  exact-once schedule/fire; ignored/trap suppression proven; cycles
  structurally impossible; deterministic ordering).
- **Audio** — all boxes satisfied (existing AudioManager path reused; no
  second audio system; fallback policy documented; stage transitions change
  cues; same state → same cue; undiscovered silence; text path complete;
  audio cannot mutate gameplay).
- **Regression closure** — build 0/0, full suite 11,170/11,170,
  data-integrity 0 new errors, all 43 identities load, deterministic
  full-lifecycle harness passes, save migration documented, this closeout.
