# Subject Plan 175.02 — A voice beneath the static
Lane: A · Cluster: C1 · Status: PROPOSAL (subject-level; serial queue; not an integration plan)

## Subject
Review the current `bodyText` for `incident_radio_interference` in `Assets/StreamingAssets/Data/incidents.json` and polish it only if the editorial pass identifies a material gap. Its register is **uncertainty without a new signal system**. Deepen the existing content rather than create a system or imply an unencoded outcome. “Text is sufficient” is an acceptable no-change closeout.

## Premise evidence
- **VERIFIED:** exact selector is live. Current title: `Radio Interference`. Current body: 26 whitespace-delimited words / 164 characters.
- **VERIFIED source text:**

> The comms array bursts with ear-splitting static. Just beneath the white noise, a cold, synthesized voice recites an unbroken sequence of numbers on a ghostly loop.

- **VERIFIED:** this record’s actual fields are `bodyText, id, minDay, title, weight`. Body text is the sole proposed prose surface; optional per-record fields must not be inferred from a different catalog record.
- **VERIFIED:** Registry maps incidents to ShelterEncounterSystem; v2.0 Volume 54 confirms that class-level consumer. A-42 names the measured five-record tranche DATA-ONLY.
- **HIGH CONFIDENCE:** The measured expansion-era band for these five legacy incidents is 55–80 words; this contract applies to this subset only.
- **UNKNOWN:** whether the exact selector has a player-facing trigger, whether a related content record owns the same scene, and whether the current unclaimed-content census already assigns this content.

## Why this and not something else
The compiled v2.0 authority supports a measured A-42 incident tranche and requires segmented census for large catalogs. A full local pass now covers all 240 event records, rather than extrapolating from the prior head fragment: IDs are unique; the current event array has multiple field shapes; and this body is among the low-length records selected for review. This plan applies to this exact object only and does not generalize one record’s optional shape to the catalog.

Editorial question: **Make the interference a bounded listening scene: distinguish what can actually be heard from what staff infer, and give the room one credible response to the synthesized voice. Preserve the source’s uncertain message and present encounter framing.** The output is a decision per field with a continuity reason. If the only benefit is greater length, retain the current text.

## What must not change
Do not resolve the voice’s origin, add coordinates or distress-signal content, create a radio puzzle, or add a transmission chain. Respect the sealed radio vocabulary boundary.

Preserve every non-prose field byte-for-byte in meaning: `id`="incident_radio_interference", `minDay`=8, `title`="Radio Interference", `weight`=1.0. Only `bodyText` may be considered; nested choices, costs, flags, deltas, conditions, gates, and order remain unchanged. Keep JSON valid and escape any new line breaks. Do not disturb encounter scheduling, event weights, day gates, deterministic selection, or sealed vocabulary boundaries. No new identifier, trigger contract, save state, route, or panel is in scope.

## Recommended integration route
**Tier:** DATA-ONLY, conditional on the current route. For this record: `EventsHostSession.LoadIncidents` → `ShelterEncounterSystem` weighted consumer. If the exact consumer/display path cannot be shown, record a DOCS-ONLY utilization finding and do not edit data to imply reachability.

**Seams:** exact JSON record → current parse/integrity gate → named selector consumer → existing display. No loader or host change is proposed. **Save impact:** NONE. **Determinism:** NONE for a bodyText-only edit. **Verification:** strict JSON parse; current catalog integrity; exact-selector reachability; structural diff proving `bodyText` is the sole changed value; canon/continuity review. For incidents, add A-42’s focused content-contract check. For events, use the smallest existing content/utilization gate after naming the caller.

**Ownership:** re-read `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `KNOWN_DEBT.md`, and current unclaimed-corpus census at scheduling. Claim the exact data path only if the integrator assigns it. This is a proposal, not an implementation claim.

## Continuity checklist result
- **Canon:** preserve facts, chronology, factions, locations, and people; reconcile any overlap with `docs/lore/04_ENCOUNTERS.md` before adding detail.
- **Catalog relation:** current census finds no exact ID overlap between `events.json` and `incidents.json`. Similar subject matter requires a manual fact check; never assume paired identity.
- **Mechanics:** no non-prose value, nested choice, condition, weight, or gate changes; body text cannot promise unencoded effects.
- **Duplication:** compare nearby events and earlier expansion prose for repeated scene turns. Exact selector is new relative to previous expansion-plan anchors.
- **Unclaimed content:** refresh the current census; route existing orphan content before proposing another source if its utilization is the real gap.
- **Epilogue:** none. **Seals:** incident subjects stay within A-42’s content-only five-field boundary; no closed one-shot conversion or distress-signal surface is reopened.

## Verification class
Before edit, re-read this object and current schema, trace the exact selector through runtime, refresh ownership/census, and compare adjacent records plus lore. After an approved body-only edit, strict-parse the whole JSON file, run the existing focused integrity/content gate, compare the full before/after objects to prove every non-body value stayed fixed, then review voice, factual fidelity, sensory clarity, duplication, and any branch/trigger implications. If reachability fails or existing prose is already sufficient, document the disposition without mutation.

## Open premises
1. Does runtime reach this exact selector? Weighted incident sampling is verified at class level; check any record-specific restrictions in the live sampler.
2. Does an authoritative document define this specific scene or character beat? Reconcile before adding facts.
3. Which current schema/integrity and exact-utilization commands validate this record, including its observed optional fields?
4. Does an active claim or the latest unclaimed-content census already own its content?
5. Is each suggested detail useful? If not, retain the source and close as sufficient.
