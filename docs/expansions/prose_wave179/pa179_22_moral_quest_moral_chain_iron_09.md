# Subject Plan 179.22 — The Example: branching-quest discovery prose

Lane: A · Subject family: moral-choice branching content · Status: PROPOSAL (single-record, data-first prose subject; implementation route evidence included, not integration approval)

## Bounded outcome

Review one field—`discovery`—for selector `quest_moral_chain_iron_09` in `Assets/StreamingAssets/Data/moral_choice_quests_branching.json`. Current depth: 122 words / 733 characters. The aim is to clarify the authored encounter that players see when this moral-choice quest is offered. The plan does not ask for a longer passage; retain the current text if a revision would add no useful information.

**Editorial question:** Keep the evidence, resource calculation, and authority pressure concrete. Do not make severity synonymous with competence or turn a hard policy into a promised result.

This is the Iron branch record **The Example**. The branch identity and actual situation are source facts; do not use this pass to rewrite the larger quest arc.

## Verified premise and source evidence

- **VERIFIED (2026-09-24):** `quest_moral_chain_iron_09` is present exactly once in the current `schema_version: 1` `quests` array, which contains 100 entries.
- **VERIFIED:** the loader record shape is `id`, `display_name`, `category`, `trigger`, `discovery`, `location_id`, `min_day`, `max_day`, `choices`; the Core loader maps these to `MoralChoiceQuestDefinition` without changing the discovery text.
- **VERIFIED:** `Main.SetupMoralChoice` loads this branching catalog and appends its definitions to the same moral-choice catalog. `MoralChoiceModal.RefreshContent` renders `Trigger`, then `Discovery`, then the choice labels. This provides the existing data-only authoring route for the prose field.
- **VERIFIED:** the exact ID does not appear in the prior expansion indexes searched for this batch. Related-chain records are identified below for a semantic duplicate and continuity review.
- **MEASURED:** complete-catalog discovery text ranges from 42 to 143 whitespace-delimited words. Volume 56 of the local expansion authority measured the field shape and observed roughly 55 words in a head record; no plan-specific length target follows from that measurement.

**Trigger:** Someone in your group has been stealing — small amounts, but consistently.

**Place:** `loc_shelter_storage` · **category:** `dead` · **day window:** 18–0 (0 max means unbounded in the current Core contract).

**Existing discovery prose (122 words / 733 characters):**

> The inventory doesn't lie, even when people do. Three cans of beans missing here. A knife that wasn't where it should be there. A half-roll of medical tape that everyone said was 'probably just misplaced.' The pattern takes two weeks to become clear. The thief is Harlan — one of your original group, someone who helped build the shelter's east wall, who shared his last cigarette during the first winter. He's been taking small amounts and distributing them to a family that isn't part of your settlement: his daughter, her husband, two grandchildren. They live six kilometers south. They're struggling. Harlan hasn't asked for help because asking means admitting the group can't take care of its own. So he's been stealing instead.

**Existing choices, frozen for context:** 1. Confront him publicly. Make the theft known. Let the group decide.; 2. Handle it privately. Offer to help his family through official channels.; 3. Let it continue. He's stealing for family. You'd do the same.

Adjacent chain records: predecessor `quest_moral_chain_iron_08`; successor `quest_moral_chain_iron_10`. Read them in the live catalog before a revision; do not borrow a neighboring record’s reveal, character knowledge, or outcome.

## Editorial treatment

Keep the evidence, resource calculation, and authority pressure concrete. Do not make severity synonymous with competence or turn a hard policy into a promised result. The discovery text should set the decision in motion, not answer it. Keep its relation to the separate `trigger` field clear: avoid restating the trigger verbatim when the current text already develops it, and avoid treating a UI heading as new diegetic speech. Use physical and social details supported by this record’s setting. Prefer one precise observation, action, or pressure point over a run of interchangeable bleak images.

Preserve point of view and information access. A character may know what they witnessed, remember, or were told; the narration must not know another person’s motive unless the record supports that knowledge. Keep allegations, uncertain identities, and disputed accounts explicitly uncertain. Maintain names, counts, timing, and causal relationships exactly as the live record states them. If a line needs a new fact to work, omit the line or route that separate idea to an independently evidenced subject.

Do not rewrite choices, outcomes, epitaphs, morality deltas, empathy deltas, flags, availability, or the consequence grammar. The prose may frame the decision but cannot imply a choice has already been made or that its outcome occurred. Avoid explaining the moral score: the Core contract says the score remains invisible, and the world and recorded consequences carry the meaning.

## Frozen data contract

Only this record’s top-level `discovery` string is in scope. Every other value is immutable. This exact JSON snapshot is the complete record with `discovery` removed; compare it against the live object immediately before any future edit:

```json
{
  "category": "dead",
  "choices": [
    {
      "empathy_delta": 0,
      "epitaph": "He stole for his grandchildren. I made his theft public. The group voted. The math worked. The feeling didn't.",
      "label": "Confront him publicly. Make the theft known. Let the group decide.",
      "moral_delta": -7,
      "outcome_text": "You lay it out at the evening meal. The cans, the knife, the tape. Harlan's face goes through surprise, then shame, then something harder — the expression of a man who's been found out and has decided he doesn't care. He explains about his daughter. Some people nod. Some don't. You put it to a vote. The group decides: expulsion, with a warning that return means worse. Harlan leaves that night. His daughter's family never gets the supplies. Three people who voted for expulsion don't meet your eyes the next morning. The thefts stop. The resentment doesn't."
    },
    {
      "empathy_delta": 2,
      "epitaph": "He stole because he couldn't ask. I listened because I could. His grandchildren eat now. The system bent. It didn't break.",
      "label": "Handle it privately. Offer to help his family through official channels.",
      "moral_delta": 6,
      "outcome_text": "You find Harlan alone. You don't accuse — you ask. He breaks before you finish the first question. His daughter's family is down to one meal a day. Her husband can't work after a fall. Harlan couldn't ask because the group's resources are tight and he knew the answer would be no. You tell him the answer is still no — the group can't support an outside family. But you can arrange a trade: Harlan's daughter can come to the settlement two days a week to work in exchange for a separate ration allocation. It's not charity. It's a program. Harlan stops stealing. His daughter comes. The grandchildren eat. The precedent is messy but humane."
    },
    {
      "empathy_delta": 1,
      "epitaph": "Three cans a week. A knife. Half a roll of tape. I called it compassion. I also called it insurance.",
      "label": "Let it continue. He's stealing for family. You'd do the same.",
      "moral_delta": 3,
      "outcome_text": "You close the inventory ledger and put it back on the shelf. The missing cans are a rounding error. The knife is replaceable. Harlan's grandchildren are eating because their grandfather is willing to risk expulsion for them. You've done worse for less. The thefts continue at the same small rate. Your inventory shrinks by a percent or two each month. Nobody else notices, or if they do, they don't say anything. You tell yourself it's compassion. Some of it is. Some of it is the knowledge that Harlan would do the same for you."
    }
  ],
  "display_name": "The Example",
  "id": "quest_moral_chain_iron_09",
  "location_id": "loc_shelter_storage",
  "max_day": 0,
  "min_day": 18,
  "trigger": "Someone in your group has been stealing — small amounts, but consistently."
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
- Compare before/after JSON, proving only `quest_moral_chain_iron_09.discovery` changed; assert all IDs, array order, gates, trigger/category/place, choice payloads, flags, outcomes, and score deltas are unchanged.
- Run the existing data-integrity and moral-choice catalog checks that own this file; report the focused command selected under the then-current `TEST_POLICY.md`.
- In a current route-level check, verify the record remains correctly mapped and that the modal presents trigger, revised discovery, and every unchanged choice label in that order. Do not alter or invent a branch result to demonstrate the prose.
- Review voice, clarity, duplicate imagery, canon facts, and word count as a descriptive measurement only. A retained-text disposition passes if no edit improves the scene.

## Disposition

Recommended scope: one selector, one prose field, one bounded review. Accept a revision only when it makes this encounter clearer or more particular without changing its decision space. The live discovery string can remain untouched. This proposal has an evidenced existing route and can inform a future data-only package; it is not a path claim, approval to edit, or a substitute for current foreman selection and ownership checks.
