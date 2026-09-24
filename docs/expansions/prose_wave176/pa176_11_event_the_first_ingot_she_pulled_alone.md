# Subject Plan 176.11 — The First Ingot She Pulled Alone: focused event prose review

Lane: A · Cluster: catalog event prose (domain-specific; classify against live canon at promotion) · Status: PROPOSAL (subject-level, serial queue; not an integration plan)

## Subject and editorial brief

Review the exact event `apprenticeship_completion_workshop_sense` in `Assets/StreamingAssets/Data/events.json`. Its current `bodyText` is 59 whitespace-delimited words (338 characters). This is a review candidate from the residual low-length tranche, not a word-count target or a claim that the source is deficient.

Editorial question: **Show learning through a bounded observed action and the response already supported by the record. Do not turn the text into a new training step, skill award, or guaranteed competence claim.** Work from the current record's facts and voice. Seek a material improvement in clarity, specificity, tone, or continuity. If the only available change would increase length, preserve the current text.

## Live source evidence

- **VERIFIED:** exact selector exists once in the 240-record `events` array. Its title is `The First Ingot She Pulled Alone` and its exact current field shape is `bodyText, choices, conditions, id, minDay, title, weight`.
- **VERIFIED source prose:**

> The forge apprentice pulled the ingot at the right beat, with the master standing two metres back and not saying anything. The ingot went onto the cooling rail without the master touching the rail. The Forge daughter did not say anything either. The apprentices do not say anything. The forge is the kind of work that prefers quiet hands.

- **VERIFIED non-prose snapshot:** `{"choices": [{"choiceId": "shift_to_morning_forge", "effects": [{"setWorldFlag": "app_workshop_sense_complete", "worldFlagValue": true}], "moraleDelta": 5.0, "text": "Move the apprentice to the morning forge shift permanently."}], "conditions": {"RequireChildCohort": true}, "id": "apprenticeship_completion_workshop_sense", "minDay": 50, "title": "The First Ingot She Pulled Alone", "weight": 1.2}`. Re-read the whole object before any future edit; this captured shape exists to prevent optional fields from being inferred from another event.
- **CENSUS:** the full local catalog has 240 unique IDs, five exact field-shape variants, and a 71-word median `bodyText`. This record is at or below that median. The measure ranks a review queue; it does not prescribe a rewrite length.
- **FRESHNESS:** this exact ID does not occur in any earlier `docs/expansions/` wave at the time of this sweep.

## Content direction

Make the event's central beat easier to see without explaining what the player should think. Preserve each named person, group, place, time gate, material object, and causal claim exactly as the source establishes it. Add at most a small number of details that deepen the existing moment; every detail must be compatible with the adjacent catalog records and `docs/lore/04_ENCOUNTERS.md`.

Keep uncertainty where the event has uncertainty. Distinguish observation from rumor, interpretation, recollection, and proven fact. Avoid a narrator who knows private motives without evidence. Keep prose grounded and economical: sensory information should clarify a person, decision, or consequence already present, never act as filler. The source may already be complete; a concise disposition should say why no edit is warranted.

## Immutable data boundary

Only top-level `bodyText` may be considered. Preserve every other key and value from the snapshot byte-for-byte in meaning, including `id`, `title`, `weight`, `minDay`, and every observed optional field. If this record includes `choices`, preserve choice order, IDs, text, morale deltas, effects, flags, and costs. If it includes `conditions`, preserve each gate. Any trust-sensitive paired text or faction threshold is a sealed two-register contract and cannot be altered under this single-field proposal.

Do not add IDs, optional fields, choices, effects, flags, conditions, costs, schedules, weights, save state, or an event. Do not change who acts, when an event becomes eligible, what a choice does, or how selection is made. Narrative text cannot promise a mechanical outcome absent from the frozen object.

## Current route and open premise

**Catalog route verified:** the registry maps `events.json` through `EventsHostSession`; `EventsHostSession.LoadEvents` opens the catalog and `TryGetEvent` provides ID lookup. The runtime utilization map identifies `EventsHostSession` and `WorldHostSession` consumers. This supports a conditional DATA-ONLY prose edit in the current catalog.

**Exact-selector route remains open:** the generic catalog lookup does not itself prove this ID is selected by a live dispatch or displayed in a player-facing surface. Before authoring, trace the current selector into its active caller and visible presentation. If that exact path cannot be demonstrated, stop at DOCS-ONLY; do not treat loader registration as gameplay reachability.

No loader, host, save, deterministic state, UI, or mechanics change is proposed. There is no save impact and no determinism impact for a body-only data edit.

## Continuity and integration checks

1. Compare the event to neighboring records in `events.json` and relevant entries in `incidents.json`, `year_of_ash_events.json`, and the current lore. Similar topics are not proof of a shared scene or identity.
2. Preserve date/day gates and the current event's selection weight. Do not use event prose to tune frequency or explain selection.
3. Confirm every named entity and claimed event is supported by current canon. Mark inference as inference; do not present it as a witnessed fact.
4. For choice-bearing records, check that the body introduces no option, promise, moral verdict, or resource result absent from the exact choice payload.
5. Re-read `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `KNOWN_DEBT.md`, and the current unclaimed-corpus census at scheduling. This proposal claims no files.
6. Compare earlier prose in the repo by exact selector and by scene turn; avoid repeating the same image, revelation, or closing gesture across the corpus.

## Verification for a future approved edit

Strict-parse all of `events.json`; run the current focused catalog-integrity and content-utilization checks; prove the record count, ID set, order, schema version, and every non-`bodyText` value are unchanged; then exercise the named selector through its verified runtime presentation path. Review voice, continuity, factual fidelity, duplicate motifs, and whether the body appears as expected with any existing choices. Do not run a broad gameplay suite for a body-only edit unless an actual integration seam changes. Any required mechanics or route change must be re-scoped and assigned separately.

## Acceptance and disposition

Accept a prose change only if it improves this exact event while remaining inside the single-field boundary and preserving its current evidence and outcomes. A no-change closeout is acceptable. The plan itself does not certify a route, claim implementation, reopen sealed behavior, or authorize batching multiple Lane A edits into one integration wave.
