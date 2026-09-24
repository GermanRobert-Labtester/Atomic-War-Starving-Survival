# Subject Plan 179.48 — The Turned Tide: branching-quest discovery prose

Lane: A · Subject family: moral-choice branching content · Status: PROPOSAL (single-record, data-first prose subject; implementation route evidence included, not integration approval)

## Bounded outcome

Review one field—`discovery`—for selector `quest_moral_chain_betray_10` in `Assets/StreamingAssets/Data/moral_choice_quests_branching.json`. Current depth: 59 words / 349 characters. The aim is to clarify the authored encounter that players see when this moral-choice quest is offered. The plan does not ask for a longer passage; retain the current text if a revision would add no useful information.

**Editorial question:** Keep the competing account and the evidence gaps legible. Do not convert suspicion into proof or settle responsibility before the player chooses.

This is the Betray branch record **The Turned Tide**. The branch identity and actual situation are source facts; do not use this pass to rewrite the larger quest arc.

## Verified premise and source evidence

- **VERIFIED (2026-09-24):** `quest_moral_chain_betray_10` is present exactly once in the current `schema_version: 1` `quests` array, which contains 100 entries.
- **VERIFIED:** the loader record shape is `id`, `display_name`, `category`, `trigger`, `discovery`, `location_id`, `min_day`, `max_day`, `choices`; the Core loader maps these to `MoralChoiceQuestDefinition` without changing the discovery text.
- **VERIFIED:** `Main.SetupMoralChoice` loads this branching catalog and appends its definitions to the same moral-choice catalog. `MoralChoiceModal.RefreshContent` renders `Trigger`, then `Discovery`, then the choice labels. This provides the existing data-only authoring route for the prose field.
- **VERIFIED:** the exact ID does not appear in the prior expansion indexes searched for this batch. Related-chain records are identified below for a semantic duplicate and continuity review.
- **MEASURED:** complete-catalog discovery text ranges from 42 to 143 whitespace-delimited words. Volume 56 of the local expansion authority measured the field shape and observed roughly 55 words in a head record; no plan-specific length target follows from that measurement.

**Trigger:** A small allied group has a leader you could undermine. Their people are already restless.

**Place:** `loc_ash_sign_shrine` · **category:** `trust` · **day window:** 80–0 (0 max means unbounded in the current Core contract).

**Existing discovery prose (59 words / 349 characters):**

> The burn zone camp has twelve members. Their leader, Cade, is competent but cold. You've noticed the restiveness — side conversations during meetings, eyes that drift when he speaks, a woman named Pell who challenges him at every turn. Cade asks you to speak with the dissenters. Pell asks you to listen. Both think you're neutral. Neither is right.

**Existing choices, frozen for context:** 1. Support Cade and help him hold the camp together; 2. Quietly encourage Pell to take over; 3. Play both sides until both depend on you; 4. Stay out of it — their politics aren't your problem

Adjacent chain records: predecessor `quest_moral_chain_betray_09`; successor `quest_moral_chain_betray_11`. Read them in the live catalog before a revision; do not borrow a neighboring record’s reveal, character knowledge, or outcome.

## Editorial treatment

Keep the competing account and the evidence gaps legible. Do not convert suspicion into proof or settle responsibility before the player chooses. The discovery text should set the decision in motion, not answer it. Keep its relation to the separate `trigger` field clear: avoid restating the trigger verbatim when the current text already develops it, and avoid treating a UI heading as new diegetic speech. Use physical and social details supported by this record’s setting. Prefer one precise observation, action, or pressure point over a run of interchangeable bleak images.

Preserve point of view and information access. A character may know what they witnessed, remember, or were told; the narration must not know another person’s motive unless the record supports that knowledge. Keep allegations, uncertain identities, and disputed accounts explicitly uncertain. Maintain names, counts, timing, and causal relationships exactly as the live record states them. If a line needs a new fact to work, omit the line or route that separate idea to an independently evidenced subject.

Do not rewrite choices, outcomes, epitaphs, morality deltas, empathy deltas, flags, availability, or the consequence grammar. The prose may frame the decision but cannot imply a choice has already been made or that its outcome occurred. Avoid explaining the moral score: the Core contract says the score remains invisible, and the world and recorded consequences carry the meaning.

## Frozen data contract

Only this record’s top-level `discovery` string is in scope. Every other value is immutable. This exact JSON snapshot is the complete record with `discovery` removed; compare it against the live object immediately before any future edit:

```json
{
  "category": "trust",
  "choices": [
    {
      "empathy_delta": 1,
      "epitaph": "Stood with Cade. The camp held. Stability has a price. Someone else paid it.",
      "label": "Support Cade and help him hold the camp together",
      "moral_delta": 5,
      "outcome_text": "You tell Cade what you've heard. He nods slowly — he already knew, but your confirmation matters. You stand with him at the next meeting. The dissenters quiet. The camp holds. Cade owes you, and the alliance between your shelters deepens. Stability has a price, but someone else is paying it."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Never said the word. Pell did the rest. The camp is mine through a proxy who thinks she's free.",
      "label": "Quietly encourage Pell to take over",
      "moral_delta": -12,
      "outcome_text": "You meet Pell at the water point. You don't say 'overthrow Cade.' You say things like 'the camp seems frustrated' and 'leadership should reflect the group.' Pell does the rest. The coup is bloodless. Cade leaves quietly. Pell is grateful — and dependent. The camp is yours now, through a proxy who thinks she's free."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Played both sides. Both weakened. Both dependent. The most elegant thing I've ever done.",
      "label": "Play both sides until both depend on you",
      "moral_delta": -8,
      "outcome_text": "You tell Cade you'll help him manage Pell. You tell Pell you'll help her manage Cade. Both pour their confidence into you. When the inevitable confrontation comes, you're the only person either side trusts to mediate. The compromise leaves both weakened and you indispensable. It's the most elegant thing you've ever done."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Stayed out. The camp fractured. Her mistakes, her camp. My hands were clean.",
      "label": "Stay out of it — their politics aren't your problem",
      "moral_delta": 1,
      "outcome_text": "You tell both Cade and Pell it's not your place. The camp fractures within a week. Cade leaves with three loyalists. Pell takes over and immediately makes three decisions you disagree with. But it's her camp, her mistakes. Your hands are clean."
    }
  ],
  "display_name": "The Turned Tide",
  "id": "quest_moral_chain_betray_10",
  "location_id": "loc_ash_sign_shrine",
  "max_day": 0,
  "min_day": 80,
  "trigger": "A small allied group has a leader you could undermine. Their people are already restless."
}
```

Do not add or remove records or properties. Do not alter `id`, `display_name`, `category`, `trigger`, `location_id`, `min_day`, `max_day`, choice count/order/labels, outcome text, epitaph, flag, moral or empathy delta, or any nested field. Do not introduce another state variant or a parallel narrative authority. The active integration ledger and current ownership ledger remain controlling for any later implementation package.

## Route, ownership, and save boundary

The data-first route is a one-field edit to the existing JSON entry, after rechecking its current schema and path ownership. The proven route is: `MoralChoiceBranchQuestCatalogLoader.Load` → `Main.SetupMoralChoice` definition registration → existing availability/chain-access checks in `GetAvailableMoralChoices` → `MoralChoiceModal` display of trigger and discovery text. Exact display is still conditional on the existing day, chain, and resolved-state gates; this proposal does not change eligibility or guarantee that the record is offered in every campaign.

A top-level prose-only change has no save, RNG, or state impact. If it appears to require new host behavior, code, UI, flags, save data, mechanics, or a changed choice contract, stop and create a separately scoped, current-evidence integration plan. The subject plan authorizes no such work.

## Canon, continuity, and duplicate checks

1. Compare the record and both adjacent chain entries named above; verify the trigger-to-discovery handoff, reveal order, time, and unresolved questions.
2. Review the relevant current lore and moral-choice canon. Use Volumes 56–57 of `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md` as discovery context, while treating shipped JSON and current code as implementation truth.
3. Search the existing moral-choice, event, echo, and encounter catalogs for duplicate scene beats and recycled phrasing. ID novelty alone is not content novelty.
4. Confirm each person, faction, place, resource quantity, and fact in the prose against existing authority. Do not add named entities or canon events by inference.
5. Keep the scene fictional, restrained, and playable. Avoid real-world references, borrowed phrasing, melodrama, moral instruction, and decorative suffering.

## Focused verification for a future edit

- Strict-parse the complete source catalog and confirm the schema version and 100-record count remain valid.
- Compare before/after JSON, proving only `quest_moral_chain_betray_10.discovery` changed; assert all IDs, array order, gates, trigger/category/place, choice payloads, flags, outcomes, and score deltas are unchanged.
- Run the existing data-integrity and moral-choice catalog checks that own this file; report the focused command selected under the then-current `TEST_POLICY.md`.
- In a current route-level check, verify the record remains correctly mapped and that the modal presents trigger, revised discovery, and every unchanged choice label in that order. Do not alter or invent a branch result to demonstrate the prose.
- Review voice, clarity, duplicate imagery, canon facts, and word count as a descriptive measurement only. A retained-text disposition passes if no edit improves the scene.

## Disposition

Recommended scope: one selector, one prose field, one bounded review. Accept a revision only when it makes this encounter clearer or more particular without changing its decision space. The live discovery string can remain untouched. This proposal has an evidenced existing route and can inform a future data-only package; it is not a path claim, approval to edit, or a substitute for current foreman selection and ownership checks.
