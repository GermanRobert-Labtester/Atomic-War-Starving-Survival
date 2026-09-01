# Plan 24 — Radio, Signals & the Airwaves World

## Goal

Make radio a coherent world layer through a unified schedule, rescue-mission lifecycle, and
number-station intelligence content. Distress signal definitions are intentionally delegated.

## Scope boundary

- Plan 107 owns radio_distress_signals.json entries and schema.
- Plan 50 owns player assessment and rescue-capacity triage.
- Plan 73 owns faction-radio corpus data.
- This plan owns schedule integration, mission creation/resolution, and number-station content.
  It must not add distress-signal rows or reimplement signal assessment.

## Task 24A — Unified broadcast schedule and programming grid

1. Inventory existing broadcasts and assign their station, frequency, day window, and schedule slot.
2. Create one schedule authority consumed by the tuner and player-authored programs (Plan 173).
3. Fill only genuine schedule gaps with ordinary broadcasts; do not copy faction or distress data.
4. Test tuner resolution, schedule coherence, and no frequency collisions.

## Task 24B — Distress-to-rescue mission lifecycle

1. Consume a validated Plan 107 signal and optional Plan 50 decision to create the existing rescue
   mission flow: intercept, triangulate, travel, resolve, and record outcome.
2. Support recruit, supplies, trap, and too-late outcomes through existing expedition, survivor,
   combat, and narrative authorities.
3. Persist in-progress missions and make resolution idempotent after reload.
4. Test the full lifecycle without authoring or duplicating any signal definitions.

## Task 24C — Number stations and signal intelligence

1. Author rotating number-station broadcasts, wiretap records, and cipher carriers through the
   existing intelligence/catalog pipeline.
2. Feed existing quest, cassette, and log consumers rather than adding a second decode system.
3. Validate signal references, narrative continuity, and deterministic log accrual.

## Definition of Done

- Plan 107 is the sole distress-signal catalog owner.
- Plan 24 is the sole distress-rescue mission creator.
- Schedule, faction corpus, player radio programs, and signal intelligence have explicit boundaries.
