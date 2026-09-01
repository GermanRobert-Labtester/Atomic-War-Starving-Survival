# Plan 09 — Medical & Disease Depth: Diagnosis, Detox & Palliative Care

## Goal

Turn the existing disease data into legible diagnosis and care: clinical tells, treatment
protocols, detox support, and dignified end-of-life choices. This plan does not expand the
disease catalog.

## Scope boundary

- Plan 112 owns disease_catalog.json and pathogen definitions.
- This plan consumes those diseases to define diagnostic and care flow. It must not add pathogen
  entries, transmission vectors, or a second disease loader.

## Task 9A — Diagnostic and outbreak-response protocols

1. Define data-backed diagnostic tells, test availability, isolation guidance, and treatment
   protocol references for Plan 112 disease ids.
2. Surface uncertainty honestly: suspected versus confirmed conditions and the cost of acting
   before confirmation.
3. Route spread/outbreak facts through the existing medical and event systems; do not recalculate
   disease statistics locally.
4. Test diagnostic progression, protocol validity, and old saves without a diagnosis record.

## Task 9B — Chemical-dependency and detox-clinic depth

1. Map the existing dependency system's tolerance, withdrawal, craving, and relapse hooks.
2. Define detox support, staffing, and relapse-response decisions using existing medical items
   wherever possible.
3. Test recovery, relapse, and save-round-trip behavior without creating a parallel affliction
   system.

## Task 9C — Palliative care, vigils, and end-of-life protocol

1. Define comfort-care choices, visitor/vigil capacity, and consent-aware consequences.
2. Reuse existing medical, relationship, memorial, and Plan 65 final-wish surfaces rather than
   duplicating their data.
3. Test care choices, grief effects, and no-loss restore behavior.

## Definition of Done

- Plan 112 remains the sole disease catalog owner.
- Diagnosis and care reference valid disease ids and existing treatment authorities.
- Detox and palliative choices are persistent, respectful, and tested.
