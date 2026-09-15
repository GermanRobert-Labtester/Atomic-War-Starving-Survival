// SPDX-License-Identifier: MIT
# DISTRESS SIGNAL PR 2 — OUTCOME HINT CONTENT TRANCHE

> Status: **COMPLETE — 2026-09-15.** Implements the deferred PR 2 content
> tranche from `INTEGRATION_PLANS.md` ("Next waves"): outcome_hint text for
> the 43 JSON-backed distress signals. User-authorized this session.
> Mechanism authority: `docs/radio/DISTRESS_SIGNAL_STAGE_CONTRACT.md` (SEALED,
> Wave 1). This tranche is content-only — zero code, zero schema changes.

## Scope

| Measure | Value |
|---|---|
| Files touched | `radio_distress_signals.json`, `radio_distress_signals_expansion.json` |
| Hints authored | 63 stage hints (37 primary + 26 expansion-effective) |
| Signals covered | 43/43 unique effective identities now fully hinted across all stages |
| Code changes | none (additive JSON string fields only) |
| Schema changes | none (`outcome_hint` was already a validated optional field) |

## Primary-wins override note (premise verified at source)

`radio_distress_signals_expansion.json` rows `freq_distress_55_1`,
`freq_distress_401_9`, `freq_distress_217_4`, `freq_distress_148_2`, and
`freq_distress_392_7` are overridden at runtime by the primary Plan 50
definitions (expansion loads first, primary loads last —
`src/Host/RadioHostSession.cs`; documented warning in the integrity gate).
Hints were therefore **not** authored into those 5 dead expansion rows
(28 stage positions left untouched on purpose). Authoring them would write
unreachable content behind the primary-wins override.

## Hint authoring rules followed

- Observational radio-intelligence lines in the sealed Wave 1 house voice:
  what the operator hears, not what the plot means.
- No hidden-truth exposure ahead of the stage text (trap authenticity,
  quest internals). Trap/false-flag hints at early stages are mild sensory
  anomalies only; the stage text itself performs the reveal.
- No RNG, no gameplay coupling — hints are presentation-only fields.
- Validator shape rule honored: a hint is either absent or non-empty
  (`ValidateDistressSignalStages` errors on present-but-empty).

## Verification (focused, per TEST_POLICY)

| Check | Result |
|---|---|
| `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/DistressStageResolverTests.cs` | 17/17 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/` | 323/323 PASS |
| `godot --headless --path . -- --data-integrity-selftest` | PASS — 0 errors, 5 documented primary-wins warnings (unchanged), 327 catalogs |
| JSON parse of both catalogs | OK |
| Hint read-back (63/63 exact-match) | OK |
| Stage counts after edit | primary 82 / expansion 100 (unchanged) |

## Environmental note (does not block this tranche)

During verification the shared worktree was transiently uncompilable from
concurrent unclaimed in-flight Economy work (`TradeEmbargoSystem.cs`,
`RegionalPriceAtlas.cs` + its test). Touched by nobody on this package; the
active writer self-repaired both before final verification was run. No
racing, no edits to unclaimed paths.

## Still deferred (unchanged, recorded for the foreman)

- Follow-up payload content (`follow_up_signals`) — mechanism live, no
  authored content (mechanism-first decision stands).
- Authored `audio_cue` content — authoring cue ids without cue assets would
  create dead references; the resolver's missing-cue policy logs once and
  continues. Needs the audio content decision first.
- Trust availability consumer — no dynamic selection pool exists in the
  runtime (stale per baseline PC-3; explicitly skipped this session).
