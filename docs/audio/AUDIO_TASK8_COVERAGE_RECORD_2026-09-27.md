# Audio Coverage Record — Enhancement Task 8 (2026-09-27)

> **STATUS: COMPLETE — audit only. No code change was justified by current evidence.**

## Premise check

The enhancement plan described `AudioManager` as a small, two-reference
surface. That premise was stale. `src/Audio/` holds a full subsystem: 15 buses,
a 196-cue data catalog, domain/expansion/shelter-operations event bridges,
state and scarcity controllers, and `AudioSelfTest`. Day advance and
expedition completion already play cues.

## Candidate gaps

| Candidate | Evidence | Decision |
|---|---|---|
| Verdict resolution cue | Core has no verdict-resolution event. `VerdictNpcSystem.OnSpoken` is NPC dialogue only. | Not wired: adding one needs a new Core event, which is a design change |
| Rationing crisis cues | `ResourceRationingSystem.OnCrisisDeclared` / `OnCrisisResolved` exist, but nothing in `src/` or Core calls `DeclareCrisis` or `ResolveCrisis` | Not wired: a subscription would never play. Upstream gap recorded as `DEBT-RATIONING-CRISIS-NO-PRODUCER` |
| Volume persistence | Master/music/SFX/radio/ambience persist in `user://settings.json` and are projected through `AudioSettings.ApplyUnifiedMix`. `user://audio_settings.json` is loaded but never saved, and no player UI writes it | No bug: it is a read-only advanced override file |

## Why no cue was added

AGENTS.md counts a system as integrated only when its event path and its
observable outcome agree. Binding cues to events that never fire would look
like coverage in `AudioSelfTest` while players hear nothing. No new cue IDs or
assets were invented.

## To unblock the rationing cue

1. Approve a crisis producer, for example daily ration evaluation that calls
   `DeclareCrisis` when stores fall below the tier threshold.
2. Then add `ResourceRationingSystem? AudioRationing` to
   `IExpansionAudioProvider`, bind it in `ExpansionAudioBridge` to existing
   cues (`ui_warning` on declare; a resolve cue chosen from the catalog), and
   extend `AudioSelfTest` with a declare/resolve probe.
