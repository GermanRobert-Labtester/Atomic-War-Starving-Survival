# Plan 68 — Wall Carving Templates Expansion: Closeout

## Status: **COMPLETE** (pure data; consumer-absent finding documented)

## Counts

```text
Baseline:  15  (5 per band — the plan's 60-target assumed a deeper start)
Final:     60  (20 high / 20 medium / 20 low — exactly per plan)
Existing templates preserved: 15/15 (all KEEP — no edits, no replacements)
New templates authored:       45
```

## Consumer finding (§66/§67)

`wall_carving_templates.json` is **data-present and consumer-absent**: no
Core, host, or UI code parses it. The content-utilization scanner maps it to
`MemorialSystem`/`MemorialPanel` aspirationally; `MemorialSystem` owns the
grief cascade and memorial plaques (ShelterDecorSystem), neither of which
references carvings.

Per §66 the plan stays pure data — no loader, no selection runtime, no save
DTO was added. Building selection/persistence/panel display is a separate
feature (the §67 gate forbids folding it into this data expansion). The 13
contract tests validate the JSON directly through a local probe DTO and
record the consumer-absent status for the future feature.

## Schema contract (verified from data)

```json
{ "schema_version": 1, "items": [ { "morale_band": "high|medium|low",
  "morale_min": int, "morale_max": int, "templates": [ "bare strings" ] } ] }
```

- Band windows: high 60–100, medium 30–59, low 0–29 — the plan's conceptual
  ranges matched exactly.
- Templates are **bare strings, third-person descriptions of physical
  marks**, with occasional quoted carved words (straight quotes) and
  em-dashes — matching the existing 15's conventions (no curly typography).
- No unsupported fields added (`weight`/`room_type`/`event_tag` etc.
  omitted per §1.5).

## Existing-template audit (§3.5)

All 15 existing templates classified **KEEP**: tonally correct (tally marks,
the sun drawing, `HOPE`/`STILL`, the imaginary cake recipe, the unanswered
date question, the scratched-out name, the tiny `I'm sorry`, the handless
clock, the desperate `WHY`), physically plausible, non-duplicative, and
band-accurate. Preserved in original order (deterministic-selection note,
§31).

## New-content tone profile

- **High (15 new):** handprints, the TOMATOES—SPRING patch, door-frame
  height marks, repair initials, the soup joke, the roster of the living,
  planting rows, the circled SPRING date, the card-game tally, the
  clean-water count rising, notes on a hand-ruled staff, the decorated
  FIRST HARVEST, the birthday cake, the grease-pencil map home, joined
  initials — solidarity, continuity, modest future orientation.
- **Medium (15 new):** `FILTER 2 — CHANGE AGAIN`, the twice-corrected
  ammunition count, the draft arrow, the carved duty roster, unbroken
  water-ration marks, the battery countdown, the lost 13 wrench, the valve
  reminder, the dented `HEAT 2-5 ONLY`, the bunk swap, the never-returned
  tool, the cleaning rota, `ASK BEFORE TAKING`, the `SOON` pipe joint,
  `RUN 6H MAX` — routine, logistics, dry irritation.
- **Low (15 new):** the half-finished name line, the door warning, the
  prayer worn to one word, twelve tallies then nothing, the chipped
  `SORRY`, the empty ruled roster, the burial count that stops, the
  crossed-out spring date, the rewritten family initials, the deep `COLD`,
  the mid-row dots, the small `DIDN'T MEAN TO`, the missing-person mark,
  the unfinished repair warning, the child-height handprint — grief, fear,
  exhaustion, failed counting, ritual, restraint.

## QA passes (§42–§46, automated where possible)

- Mechanical: JSON valid, counts, schema, grammar, lengths (all ≤140 chars
  — glance-readable), test-pinned.
- Band accuracy: blind-vocabulary spot checks automated (SPRING/harvest in
  high; FILTER/BATTERY/NIGHT SHIFT/WRENCH in medium; SORRY/DON'T SLEEP/COLD
  in low).
- Physicality: all 60 describe or quote a plausible surface mark; zero
  narrator-exposition lines.
- Repetition: zero exact duplicates within bands; zero exact duplicates
  across bands; motif spread verified (names ≤5 in low, single tally
  reference per band in the new corpus).
- Cliché gate: the §50 blocklist (last hope, darkness swallowed, against
  all odds, never give up, tomorrow will come, ashes of the old world…)
  enforced by test — zero hits.
- Modern-meme gate: no internet slang (spot-checked).
- Child-writing gate: one child-adjacent template (existing black-sky
  bunker drawing) — restrained, no invented misspellings.
- Prayer gate: one low-band prayer reference, worn to ambiguity — no
  invented religious canon.
- Names/dates gates: initials and generic names only; relative/campaign
  framing only (the one carved date is explicitly an intent mark).
- Room-specificity gate: all new templates are global-context-safe (filter,
  pipe, door-frame, bench references are shelter-wide concepts).
- Epitaph/folklore boundary: no grave-epitaph or folklore catalog text
  duplicated (formal memorials remain Plan 69/30A authorities).

## Deferred (follow-on work, documented)

1. **The carving consumer** — selection/persistence/display runtime
   (MemorialPanel or a shelter-wall surface). The catalog, band windows,
   and probe DTO define the shape; Plan 68 §75.5/75.6 event-conditioned and
   survivor-authored extensions wait behind it.
2. Room-aware selection (Plan 41), folklore cross-references (Plan 30A),
   grave-epitaph boundary enforcement in a consumer (Plan 69).
3. Localization extraction for the 60 strings (the general-series
   localization task owns the extraction pass).

## Verification

| Gate | Result |
|---|---|
| `--data-integrity-selftest` | **PASS** 0 findings / 208 catalogs (10,619 ids) |
| `dotnet test Ashfall.Core.Tests` | **PASS** 6,666/6,666 (13 new Plan-68 tests) |
| `dotnet build Ashfall.csproj` | **PASS** 0 errors |
| `--content-utilization-selftest` | **PASS** |
| `--bridge-selftest` | **PASS** exit 0 |
