# Plan 110 — Moral Choice Gossip Line Expansion Closeout

## Summary

Expanded `moral_choice_gossip.json` from 106 valid lines in 20 usable pools to
exactly **420 lines in 21 pools**: three sections × seven bands × 20 lines.
Existing line order and content were preserved; the missing
`whisper_lines.slightly_positive` typed pool was added as a minimal verified
compatibility correction.

## Final count report

| Section | Band | Old valid | Added | Replaced | Final |
| --- | --- | ---: | ---: | ---: | ---: |
| camp_chatter | very_positive | 10 | 10 | 0 | 20 |
| camp_chatter | positive | 9 | 11 | 0 | 20 |
| camp_chatter | slightly_positive | 5 | 15 | 0 | 20 |
| camp_chatter | neutral | 5 | 15 | 0 | 20 |
| camp_chatter | slightly_evil | 5 | 15 | 0 | 20 |
| camp_chatter | evil | 8 | 12 | 0 | 20 |
| camp_chatter | very_evil | 10 | 10 | 0 | 20 |
| npc_greeting_shifts | very_positive | 5 | 15 | 0 | 20 |
| npc_greeting_shifts | positive | 4 | 16 | 0 | 20 |
| npc_greeting_shifts | slightly_positive | 3 | 17 | 0 | 20 |
| npc_greeting_shifts | neutral | 4 | 16 | 0 | 20 |
| npc_greeting_shifts | slightly_evil | 4 | 16 | 0 | 20 |
| npc_greeting_shifts | evil | 4 | 16 | 0 | 20 |
| npc_greeting_shifts | very_evil | 5 | 15 | 0 | 20 |
| whisper_lines | very_positive | 6 | 14 | 0 | 20 |
| whisper_lines | positive | 4 | 16 | 0 | 20 |
| whisper_lines | slightly_positive | 0 | 20 | 0 | 20 |
| whisper_lines | neutral | 3 | 17 | 0 | 20 |
| whisper_lines | slightly_evil | 3 | 17 | 0 | 20 |
| whisper_lines | evil | 4 | 16 | 0 | 20 |
| whisper_lines | very_evil | 5 | 15 | 0 | 20 |
| **Total** |  | **106** | **314** | **0** | **420** |

## Content quality

- Very positive: exceptional trust, costly mercy, protection, and unadvertised
  generosity.
- Positive: routine fairness, reliability, practical help, and ordinary warmth.
- Slightly positive: surprised or grudging approval, especially in the new
  whisper pool.
- Neutral: active ambiguity, conditional help, and inconsistent interpretation.
- Slightly evil: early wariness, leverage, narrowed choices, and requests for
  witnesses.
- Evil: social restriction, defensive preparation, condemnation, and guarded
  trade.
- Very evil: avoidance, relocation, hidden stores, escorts, and fear expressed
  through survival behavior rather than supernatural language.

Camp chatter remains overheard and observational. Greetings are direct-address
openings. Whispers remain short, private, and deniable. New prose uses concrete
shelter details rather than raw moral-band labels or internal quest IDs.

## Duplicate review

- Exact duplicates within pools: **0**.
- Normalized exact duplicates across the complete catalog: **0**.
- Near-duplicate review: completed during authoring; one semantically redundant
  positive greeting was rewritten before final validation.

## Cross-plan context

Context capability is **C2**. The runtime only selects by effective moral band
and section; it cannot prove a Plan 109 echo outcome or Plan 100 faction
reaction before selecting a line.

- Plan 109 event-specific lines implemented: **0/8**.
- Plan 100 event-specific lines implemented: **0/6**.
- Safe thematic atmosphere: included in ordinary band pools.
- Context-gated follow-on: deferred to the owning contextual systems.

## Persistence and determinism

The runtime is stateless and does not persist line indexes or recent history.
The expansion adds no save fields and no migration. Selection remains seeded;
same seed, catalog, band, and section produce the same sequence.

## Architectural deviation

The roadmap expected 21 arrays, but the repository shipped only 20 whisper
fields. A data-only addition would have been silently ignored by the live
serializer/loader and unreachable at runtime. The only code change is the
minimal typed `slightly_positive` whisper field, loader mapping, runtime lookup
arm, and focused validation tests. No new gossip system or metadata schema was
introduced.

## Verification

Targeted moral-choice tests after expansion: **148 passed, 0 failed**.

- `godot --headless --path . -- --data-integrity-selftest`: **PASS**, 298/298,
  0 errors, 0 warnings.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`: **PASS**,
  9881/9881, 0 failed, 0 skipped.
- `dotnet build Ashfall.csproj`: **PASS**, 0 warnings, 0 errors.
- `godot --headless --path . -- --content-utilization-selftest`: **PASS**,
  581 catalogs, 0 orphaned; the repository's existing unresolved count remains
  70.
- `godot --headless --path . -- --real-campaign-journey-selftest`: **PASS**.
  It emits existing renderer/resource leak diagnostics during shutdown, but
  the host self-test reports success.
- `python3 scripts/ci/run-gates.py --tier fast`: **BLOCKED** at gate 1 by the
  unrelated pre-existing blank line at EOF in
  `docs/narrative/PLAN_89_MUSTER_EPILOGUES_EXPANSION_CLOSEOUT.md:76`.

No Unity tooling was invoked. Scoped diff and whitespace checks for the Plan
110 files are clean.
