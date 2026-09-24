# Subject Plan 179.15 — What the Dying Know: branching-quest discovery prose

Lane: A · Subject family: moral-choice branching content · Status: PROPOSAL (single-record, data-first prose subject; implementation route evidence included, not integration approval)

## Bounded outcome

Review one field—`discovery`—for selector `quest_moral_chain_iron_02` in `Assets/StreamingAssets/Data/moral_choice_quests_branching.json`. Current depth: 70 words / 375 characters. The aim is to clarify the authored encounter that players see when this moral-choice quest is offered. The plan does not ask for a longer passage; retain the current text if a revision would add no useful information.

**Editorial question:** Keep the evidence, resource calculation, and authority pressure concrete. Do not make severity synonymous with competence or turn a hard policy into a promised result.

This is the Iron branch record **What the Dying Know**. The branch identity and actual situation are source facts; do not use this pass to rewrite the larger quest arc.

## Verified premise and source evidence

- **VERIFIED (2026-09-24):** `quest_moral_chain_iron_02` is present exactly once in the current `schema_version: 1` `quests` array, which contains 100 entries.
- **VERIFIED:** the loader record shape is `id`, `display_name`, `category`, `trigger`, `discovery`, `location_id`, `min_day`, `max_day`, `choices`; the Core loader maps these to `MoralChoiceQuestDefinition` without changing the discovery text.
- **VERIFIED:** `Main.SetupMoralChoice` loads this branching catalog and appends its definitions to the same moral-choice catalog. `MoralChoiceModal.RefreshContent` renders `Trigger`, then `Discovery`, then the choice labels. This provides the existing data-only authoring route for the prose field.
- **VERIFIED:** the exact ID does not appear in the prior expansion indexes searched for this batch. Related-chain records are identified below for a semantic duplicate and continuity review.
- **MEASURED:** complete-catalog discovery text ranges from 42 to 143 whitespace-delimited words. Volume 56 of the local expansion authority measured the field shape and observed roughly 55 words in a head record; no plan-specific length target follows from that measurement.

**Trigger:** A wounded stranger collapses at your perimeter. They're carrying a map marked with supply caches.

**Place:** `loc_shelter_perimeter` · **category:** `listen` · **day window:** 3–0 (0 max means unbounded in the current Core contract).

**Existing discovery prose (70 words / 375 characters):**

> He bleeds out against the concrete barrier, a through-and-through in the abdomen that you can see breathing. His fingers keep pressing at the wound as if he could push himself back together. With his other hand, he holds a folded map — you can see the edges, marked in red pencil. Supply caches. At least four of them. His name is Dex. He doesn't offer it. He asks for water.

**Existing choices, frozen for context:** 1. Give him water. Take the map. Let him go.; 2. Tend the wound properly. Earn his trust. Learn where the caches really are.; 3. Take the map. He won't need it where he's going.

Adjacent chain records: predecessor `quest_moral_chain_iron_01`; successor `quest_moral_chain_iron_03`. Read them in the live catalog before a revision; do not borrow a neighboring record’s reveal, character knowledge, or outcome.

## Editorial treatment

Keep the evidence, resource calculation, and authority pressure concrete. Do not make severity synonymous with competence or turn a hard policy into a promised result. The discovery text should set the decision in motion, not answer it. Keep its relation to the separate `trigger` field clear: avoid restating the trigger verbatim when the current text already develops it, and avoid treating a UI heading as new diegetic speech. Use physical and social details supported by this record’s setting. Prefer one precise observation, action, or pressure point over a run of interchangeable bleak images.

Preserve point of view and information access. A character may know what they witnessed, remember, or were told; the narration must not know another person’s motive unless the record supports that knowledge. Keep allegations, uncertain identities, and disputed accounts explicitly uncertain. Maintain names, counts, timing, and causal relationships exactly as the live record states them. If a line needs a new fact to work, omit the line or route that separate idea to an independently evidenced subject.

Do not rewrite choices, outcomes, epitaphs, morality deltas, empathy deltas, flags, availability, or the consequence grammar. The prose may frame the decision but cannot imply a choice has already been made or that its outcome occurred. Avoid explaining the moral score: the Core contract says the score remains invisible, and the world and recorded consequences carry the meaning.

## Frozen data contract

Only this record’s top-level `discovery` string is in scope. Every other value is immutable. This exact JSON snapshot is the complete record with `discovery` removed; compare it against the live object immediately before any future edit:

```json
{
  "category": "listen",
  "choices": [
    {
      "empathy_delta": 0,
      "epitaph": "He drank. I took. He walked. I don't know if he made it. I don't go back to check.",
      "label": "Give him water. Take the map. Let him go.",
      "moral_delta": -3,
      "outcome_text": "You uncork your canteen and hold it while he drinks. His hands are steady enough for that much. When he's done, you take the map from his lap. He doesn't resist — he doesn't have the strength or the leverage. You bandage the wound poorly and point him east. He walks for twenty meters before he sits down. You have the map. He has a chance. Neither of you believes it's a fair trade."
    },
    {
      "empathy_delta": 1,
      "epitaph": "Antibiotic paste costs more than water. So does trust. Both were worth it.",
      "label": "Tend the wound properly. Earn his trust. Learn where the caches really are.",
      "moral_delta": 5,
      "outcome_text": "You clean the wound with boiled water and pack it with the last of your antibiotic paste. Dex talks because you gave him a reason to. The map has four caches marked, but he tells you about a fifth — one he didn't draw, kept for emergencies. You take him to the shelter. He recovers slowly. The fifth cache is real."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Four caches by sunrise. One body by moonrise. The math doesn't keep me up. The silence does.",
      "label": "Take the map. He won't need it where he's going.",
      "moral_delta": -14,
      "outcome_text": "You slide the map from under his fingers while he's drinking. He's too far gone to stop you and too dehydrated to shout. You leave him the rest of your canteen — not out of mercy, but because a corpse at your perimeter draws attention. He dies around midnight. You're already at the first cache by dawn. The red pencil marks are precise. He knew this terrain."
    }
  ],
  "display_name": "What the Dying Know",
  "id": "quest_moral_chain_iron_02",
  "location_id": "loc_shelter_perimeter",
  "max_day": 0,
  "min_day": 3,
  "trigger": "A wounded stranger collapses at your perimeter. They're carrying a map marked with supply caches."
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
- Compare before/after JSON, proving only `quest_moral_chain_iron_02.discovery` changed; assert all IDs, array order, gates, trigger/category/place, choice payloads, flags, outcomes, and score deltas are unchanged.
- Run the existing data-integrity and moral-choice catalog checks that own this file; report the focused command selected under the then-current `TEST_POLICY.md`.
- In a current route-level check, verify the record remains correctly mapped and that the modal presents trigger, revised discovery, and every unchanged choice label in that order. Do not alter or invent a branch result to demonstrate the prose.
- Review voice, clarity, duplicate imagery, canon facts, and word count as a descriptive measurement only. A retained-text disposition passes if no edit improves the scene.

## Disposition

Recommended scope: one selector, one prose field, one bounded review. Accept a revision only when it makes this encounter clearer or more particular without changing its decision space. The live discovery string can remain untouched. This proposal has an evidenced existing route and can inform a future data-only package; it is not a path claim, approval to edit, or a substitute for current foreman selection and ownership checks.
