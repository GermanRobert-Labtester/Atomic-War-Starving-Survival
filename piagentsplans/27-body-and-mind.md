# Plan 27 — The Body & Mind: Dose Justice, Death Inquiry & Deep Dread

## Goal

Make the human consequences of radiation, death, and deep contamination playable through registry
adjudication, consent-aware casework, and restrained psychological contamination. This plan does
not expand dose or autopsy catalogs.

## Scope boundary

- Plans 81, 90, 101, and 106 own dose location, band/plan, quest, and item catalogs.
- Plan 79 owns autopsy_procedures.json.
- This plan owns institutional decisions and case consequences that consume those definitions.
  It must not add dose entries, autopsy procedures, instruments, or a second mental-health meter.

## Task 27A — Dose-register adjudication and appeals

1. Use existing dose bands, care plans, items, locations, and quest definitions to stage triage,
   appeal, falsification-review, and work-clearance decisions.
2. Give the player visible evidence, a constrained authority, and consequences routed through the
   existing medical, duty, moral, and faction systems.
3. Persist decisions and test conflicting claims, a successful appeal, and the no-capacity case.

## Task 27B — Consent-aware death inquiry

1. Use Plan 79 procedures as evidence inputs to a casework flow: consent, findings review,
   next-of-kin communication, memorial consequences, and a possible suspicious-death lead.
2. Keep medical facts and procedure definitions in their owning systems; this plan records only
   the inquiry decision and resolved case facts.
3. Test refusal, incomplete evidence, a natural cause, and a suspicious case across save/load.

## Task 27C — Psychological contamination and deep-dive dread

1. Give the existing PsychologicalContaminationSystem carefully scoped sources, thresholds,
   recovery, and visible tells for deep/dark contamination.
2. Reuse existing trauma, flashback, guilt, and relationship effects; do not fork a sanity meter.
3. Test exposure, recovery, deterministic outcomes, and a trauma-system duplication guard.

## Definition of Done

- Dose and autopsy catalogs have one owner each.
- Registry and inquiry decisions are respectful, visible, persistent, and grounded in owned facts.
- Psychological contamination is an integration layer, not a parallel mental-health system.
