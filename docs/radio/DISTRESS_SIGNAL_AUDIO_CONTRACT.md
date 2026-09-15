// SPDX-License-Identifier: MIT
# DISTRESS SIGNAL AUDIO CONTRACT (Tasks 9–12 Wave 4)

> Status: **SEALED — Wave 4.** Live authority for distress-signal audio.
> Baseline evidence: `docs/radio/DISTRESS_SIGNAL_TASKS_9_12_BASELINE.md` (PC-5).

## 1. Audio is a one-way presentation projection

```text
gameplay/radio state → audio
```

Never the reverse. Playback must never determine discovery, stage
progression, trust, quest state, follow-up scheduling, or encounter
outcomes. The text/transcript path is fully functional without audio — the
radio is never audio-only.

## 2. Cue schema (additive, plan Option A — stage overrides)

- `audio_cue` on the broadcast (signal default);
- `audio_cue` per `message_fragments` row (stage-level override);
- `audio_cue` per `follow_up_signals` row.

No clarity-band cue map exists (never create a second one). Empty = inherit
/ text-only and is legal everywhere.

## 3. Resolution (`DistressAudioCueResolver`)

Precedence: current stage's `audio_cue` override → signal default →
**empty string (text-only fallback)**. Deterministic — explicit IDs only,
no RNG, no time, no hash ordering. Side-effect free; safe for unknown
signals and out-of-range stage indices. The stage input comes from the
sole stage authority (`DistressStageResolver`) — no duplicate resolver.

## 4. Detection playback contract (host)

`RadioHostSession.TuneToFrequency` plays the resolved cue ONLY when
`RadioDistressSystem.Intercept` returned true — the Inactive → Intercepted
transition through the tuner flow. It never plays because the catalog
loaded, a signal became internally eligible, a signal sits in a candidate
pool, or a save contains an undiscovered signal.

**Invariant: undiscovered signal → no distress audio playback.**

Dedupe rides the existing persisted `playedBroadcastKeys` ledger with the
key `distress:{signalId}:{cueId}` — zero new save fields:

- an already-heard cue does not replay after a reload (persisted ledger);
- a stage transition to a NEW cue plays (new key);
- one-shot semantics match the existing broadcast voice-over behavior.

## 5. Follow-up audio

A fired follow-up plays its own `audio_cue` in the host's
`OnFollowUpFired` subscription. Follow-ups fire exactly once by scheduler
construction — no dedupe needed. Empty cue = text-only event line.

## 6. Missing-cue fallback policy (documented)

`AudioManager.PlayCue` resolves through `AudioCueCatalog`; an unknown cue
logs once (`LogMissingOnce`) and playback is skipped — the transmission
text continues and the game never crashes. **Structural** validation is in
the data-integrity gate (empty, or lowercase snake_case without
whitespace). **Semantic** resolution (does the ID exist in the cue
registry?) is verified by the host audio selftest once authored content
lands — the registry is host-side and Core must not reference it.

## 7. Save/load semantics

Nothing audio-related is persisted beyond the dedupe keys above: the same
signal at the same stage resolves to the same cue after load (pure
derivation). No clip positions, decoder buffers, or mixer state are ever
serialized.

## 8. Tests

`Ashfall.Core.Tests/Radio/DistressAudioCueTests.cs` (10 cases):
deterministic resolution, clarity/stage cue transitions, Option-A
precedence, text-only fallbacks (null/unknown/out-of-range), definition
JSON round-trip, follow-up cue binding, the single detection gate
(Intercept edge), no-mutation guarantee, structural validation (accept +
reject), and real-catalog cleanliness.

Host runtime playback is exercised through `--audio-selftest` (PASS) and
`--panel-bind-lifecycle-selftest` (PASS); headless `PlayCueDef` is a no-op
by design, so playback assertions are structural (gate + dedupe key),
matching the existing audio test pattern.
