# Subject Plan 177.08 — The Refusal of the Order: focused event prose review

Lane: A · Subject family: authored event prose · Status: PROPOSAL (single-record, serial queue; not an integration plan)

## Subject and editorial brief

Review the exact event `event_belief_leadership_collision` in `Assets/StreamingAssets/Data/events.json`. Its current `bodyText` is 73 whitespace-delimited words / 405 characters. This is a source-grounded review proposal, not a defect finding or target length. A no-change disposition is appropriate where the current prose already communicates its scene clearly.

Editorial question: **Render belief as a person's interpretation rather than confirmed supernatural fact. Keep the choice boundary and competing evidence intact.** Add specificity only when it sharpens the existing event's person, decision, evidence, or consequence. Keep the text restrained, grounded, and compatible with the ASHFALL encounter register.

## Live-source evidence

- **VERIFIED:** exact selector `event_belief_leadership_collision` exists once in the current 240-record `events` array.
- **VERIFIED:** title `The Refusal of the Order`; exact observed fields: `bodyText, choices, id, minDay, title, weight`.
- **Current source prose:**

> The emergency order has met a wall: movement members refuse to scrap the memorial lathe, or to work through the remembrance day, whichever it is this morning, because the order and the day have collided. They stand with linked arms before the workshop bulkhead, and the arms are not violent, and the order is not either, and the two face each other while the corridor holds its breath on whose side it is.

- **Frozen non-prose snapshot:** `{"choices": [{"choiceId": "choice_negotiate_compromise_shift", "effects": [{"setWorldFlag": "leadership_belief_compromise_reached", "worldFlagValue": true}], "moraleDelta": 1.5, "text": "Negotiate a compromise shift schedule that preserves the memorial artifact."}, {"choiceId": "choice_demand_strict_compliance", "effects": [{"setWorldFlag": "leadership_coercion_enforced", "worldFlagValue": true}], "moraleDelta": -3.5, "text": "Threaten ration deductions and demand immediate obedience to leadership."}], "id": "event_belief_leadership_collision", "minDay": 35, "title": "The Refusal of the Order", "weight": 0.7}`. This serializes every observed field except the single editable prose value. Re-read the whole live object before any future edit.
- **Freshness:** this exact ID is absent from all earlier `docs/expansions/` plans in the current sweep.

## Content treatment

The candidate edit surface is top-level `bodyText` only. Preserve the event's point of view, time and place, named people and groups, evidence, causal claims, and degree of certainty. Do not turn rumor or belief into objective fact. Keep a record's distinctive voice; do not borrow another character's history to make this one feel fuller.

Any added image should do work: locate the speaker, make a choice intelligible, show a concrete trace of the event, or preserve a human reaction that the source already entails. Avoid recapping known mechanics, moralizing, lore dumps, decorative hardship, and repeated closing beats. The text may remain exactly as written if a proposed detail would only add length.

Compare the body against the full event record and adjacent catalog entries before polishing. In particular, no prose may imply that a choice has been selected, a flag has been written, a person has changed sides, a hazard has ended, or a resource has moved when that result is absent from the record. Distinguish what is seen from what is inferred. Keep a usable ambiguity when the record does not resolve it.

## Immutable data contract

Only `bodyText` may change. Preserve all values in the frozen snapshot, including IDs, title, weight, day gate, optional fields, and nested structures. For choice-bearing entries, freeze each choice ID, display string, order, morale delta, cost, effect, and flag. For condition-bearing entries, preserve each condition exactly. If the record has a second-register/trust-sensitive text or threshold, this plan does not authorize editing it; a paired-voice pass needs its own explicit contract.

Do not add or remove fields, events, IDs, choices, flags, costs, conditions, gates, effects, state, routes, save data, UI, or code. Do not change event selection frequency or day eligibility. Prose cannot promise an outcome beyond the existing payload.

## Existing route and remaining premise

The authority documents `events.json` as an ID-addressed catalog, distinct from weighted `incidents.json`. The current host class `EventsHostSession` loads the file and exposes `TryGetEvent(string eventId, out EventData)` for host adapters. Current content-utilization mappings also name `EventsHostSession`/`WorldHostSession` as consumers. These are verified catalog and lookup seams.

The per-ID display/dispatch path remains a promotion premise. Before authoring, trace this exact ID through the current caller to a player-visible presentation and confirm its existing payload is used. Generic loading and lookup alone do not establish reachability. If no live caller is found, record DOCS-ONLY and do not edit data on the assumption that it will appear in play.

**Conditional route:** DATA-ONLY, `events.json` top-level `bodyText` change after exact-selector reachability and current ownership are verified. Save and determinism impact: none for a prose-only edit. No loader, event host, or UI change is proposed.

## Canon, duplicate, and ownership checks

1. Compare with `docs/lore/04_ENCOUNTERS.md`, the relevant current lore, and neighboring records. A matching theme or vocabulary does not prove shared identity or chronology.
2. Check `incidents.json` and other parallel event catalogs for semantic duplication. Preserve the authority's distinction between ID-addressed authored events and separately weighted incident records.
3. Preserve all nested branches and thresholds. Read each literal effect and flag before deciding whether a sentence is safe to polish.
4. Search prior prose by exact ID and by repeated scene image; the selector is new, but genre-level duplication can still occur.
5. At promotion, refresh `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `KNOWN_DEBT.md`, and the unclaimed-corpus census. This proposal claims no path and does not replace the active integration ledger.
6. Reconcile current repository evidence before following stale or broader authority suggestions. The compiled v2.0 A-12 seed, for example, names arrival/revisit fields that do not exist in the current `expeditions.json`; no such field is introduced here.

## Future verification

Strict-parse the complete `events.json`; run the current focused data-integrity and content-utilization gates; compare the before/after root and every record to prove that only this record's `bodyText` changed; verify ID uniqueness, array order, gates, weights, optional keys, and nested choice/condition values; then exercise the named selector through its confirmed player-facing route. Review the revised text for exact facts, voice, continuity, branch promises, duplicate imagery, and restrained tone.

Do not run a broad test suite for this body-only proposal. If the caller, schema, or mechanics must change to make the content visible, stop and re-scope under the current foreman rather than folding feature work into this prose plan.

## Acceptance and disposition

Accept an edit only if it makes this one event clearer or more particular while preserving the live contract and verified canon. Retaining the current copy is a valid editorial result. This proposal is not a runtime-reachability certification, an integration approval, or permission to author all 50 Lane A candidates in one wave.
