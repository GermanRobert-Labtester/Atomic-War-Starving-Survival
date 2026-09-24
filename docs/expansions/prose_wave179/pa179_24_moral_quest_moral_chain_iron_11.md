# Subject Plan 179.24 — The Strike: branching-quest discovery prose

Lane: A · Subject family: moral-choice branching content · Status: PROPOSAL (single-record, data-first prose subject; implementation route evidence included, not integration approval)

## Bounded outcome

Review one field—`discovery`—for selector `quest_moral_chain_iron_11` in `Assets/StreamingAssets/Data/moral_choice_quests_branching.json`. Current depth: 128 words / 765 characters. The aim is to clarify the authored encounter that players see when this moral-choice quest is offered. The plan does not ask for a longer passage; retain the current text if a revision would add no useful information.

**Editorial question:** Keep the evidence, resource calculation, and authority pressure concrete. Do not make severity synonymous with competence or turn a hard policy into a promised result.

This is the Iron branch record **The Strike**. The branch identity and actual situation are source facts; do not use this pass to rewrite the larger quest arc.

## Verified premise and source evidence

- **VERIFIED (2026-09-24):** `quest_moral_chain_iron_11` is present exactly once in the current `schema_version: 1` `quests` array, which contains 100 entries.
- **VERIFIED:** the loader record shape is `id`, `display_name`, `category`, `trigger`, `discovery`, `location_id`, `min_day`, `max_day`, `choices`; the Core loader maps these to `MoralChoiceQuestDefinition` without changing the discovery text.
- **VERIFIED:** `Main.SetupMoralChoice` loads this branching catalog and appends its definitions to the same moral-choice catalog. `MoralChoiceModal.RefreshContent` renders `Trigger`, then `Discovery`, then the choice labels. This provides the existing data-only authoring route for the prose field.
- **VERIFIED:** the exact ID does not appear in the prior expansion indexes searched for this batch. Related-chain records are identified below for a semantic duplicate and continuity review.
- **MEASURED:** complete-catalog discovery text ranges from 42 to 143 whitespace-delimited words. Volume 56 of the local expansion authority measured the field shape and observed roughly 55 words in a head record; no plan-specific length target follows from that measurement.

**Trigger:** Your workers are demanding better conditions — more food, shorter shifts, rest days.

**Place:** `loc_shelter_storage` · **category:** `trust` · **day window:** 22–0 (0 max means unbounded in the current Core contract).

**Existing discovery prose (128 words / 765 characters):**

> It starts with the carpentry team. Then the kitchen staff. Then the water collectors. By the third day, you have a list of demands written on a piece of cardboard and signed by nineteen people. The demands aren't unreasonable: one rest day per ten-day cycle, an extra half-ration for exterior workers, and a review of the medical supply allocation. The problem isn't the demands. The problem is the precedent. You've never had a collective negotiation before. The list is being presented by a man named Dex — not the Dex from the map, a different one, a former mechanic who's become the de facto representative for the labor force. He's calm, organized, and completely serious. He says they'll continue working while negotiations proceed. He doesn't say they won't.

**Existing choices, frozen for context:** 1. Accept all demands. The workers have a point.; 2. Negotiate. Accept some demands, reject others. Find the middle.; 3. Reject the demands. Remind them that survival isn't a negotiation.

Adjacent chain records: predecessor `quest_moral_chain_iron_10`; successor `quest_moral_chain_iron_12`. Read them in the live catalog before a revision; do not borrow a neighboring record’s reveal, character knowledge, or outcome.

## Editorial treatment

Keep the evidence, resource calculation, and authority pressure concrete. Do not make severity synonymous with competence or turn a hard policy into a promised result. The discovery text should set the decision in motion, not answer it. Keep its relation to the separate `trigger` field clear: avoid restating the trigger verbatim when the current text already develops it, and avoid treating a UI heading as new diegetic speech. Use physical and social details supported by this record’s setting. Prefer one precise observation, action, or pressure point over a run of interchangeable bleak images.

Preserve point of view and information access. A character may know what they witnessed, remember, or were told; the narration must not know another person’s motive unless the record supports that knowledge. Keep allegations, uncertain identities, and disputed accounts explicitly uncertain. Maintain names, counts, timing, and causal relationships exactly as the live record states them. If a line needs a new fact to work, omit the line or route that separate idea to an independently evidenced subject.

Do not rewrite choices, outcomes, epitaphs, morality deltas, empathy deltas, flags, availability, or the consequence grammar. The prose may frame the decision but cannot imply a choice has already been made or that its outcome occurred. Avoid explaining the moral score: the Core contract says the score remains invisible, and the world and recorded consequences carry the meaning.

## Frozen data contract

Only this record’s top-level `discovery` string is in scope. Every other value is immutable. This exact JSON snapshot is the complete record with `discovery` removed; compare it against the live object immediately before any future edit:

```json
{
  "category": "trust",
  "choices": [
    {
      "empathy_delta": 2,
      "epitaph": "Nineteen signatures. Three demands. I said yes to all of them. The margins shrank. The people didn't. That's the math I chose.",
      "label": "Accept all demands. The workers have a point.",
      "moral_delta": 8,
      "outcome_text": "You agree to the rest day, the extra rations, the medical review. Dex looks surprised — he was clearly prepared for a fight. The changes take effect immediately. Exterior workers get an extra half-ration. The rest day is awkward at first — people don't know what to do with unstructured time — but within a week, morale improves measurably. Productivity on working days goes up. The medical review finds three people who needed treatment they hadn't requested. Your resource margins shrink by about eight percent. The group is healthier, better rested, and slightly more loyal. The precedent is also set: collective negotiation works. You'll face it again."
    },
    {
      "empathy_delta": 1,
      "epitaph": "Four hours. Three demands. I said yes to two. The third became something else. Nobody was happy. Everyone accepted it. That's governance.",
      "label": "Negotiate. Accept some demands, reject others. Find the middle.",
      "moral_delta": 2,
      "outcome_text": "You sit down with Dex and the three team representatives. The negotiation takes four hours. You agree to the rest day and the medical review. You reject the extra rations but offer a different concession: exterior workers get first choice of shelter assignments when space opens up. Dex pushes back on the rations. You hold firm — the food supply can't support it. He accepts it with visible reluctance. The agreement is signed. It's not perfect. Nobody got everything. But the process itself is the precedent: negotiation, not confrontation, is how this group resolves disputes. The rest day feels earned. The ration denial feels fair. Mostly."
    },
    {
      "empathy_delta": 0,
      "epitaph": "I tore the cardboard. I said the words. Three people left. The carpentry team is short-handed. The people who stayed are quieter. Quiet isn't loyal.",
      "label": "Reject the demands. Remind them that survival isn't a negotiation.",
      "moral_delta": -10,
      "outcome_text": "You tear the cardboard in half. You say the words: 'This isn't a democracy. We survive because we work. If you can't work, there are people outside who will.' The room goes very quiet. Dex's face doesn't change, but his hands do — they clench, then release, then clench again. The workers go back to work. Nobody strikes. Nobody complains. Three people leave within the week — two to the eastern road, one to join Aldric. They take their skills with them. The carpentry team is short-handed for two months. The water collection schedule slips. The people who stayed are more obedient. They're also more resentful. You've won the argument. You've lost some of your best workers."
    }
  ],
  "display_name": "The Strike",
  "id": "quest_moral_chain_iron_11",
  "location_id": "loc_shelter_storage",
  "max_day": 0,
  "min_day": 22,
  "trigger": "Your workers are demanding better conditions — more food, shorter shifts, rest days."
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
- Compare before/after JSON, proving only `quest_moral_chain_iron_11.discovery` changed; assert all IDs, array order, gates, trigger/category/place, choice payloads, flags, outcomes, and score deltas are unchanged.
- Run the existing data-integrity and moral-choice catalog checks that own this file; report the focused command selected under the then-current `TEST_POLICY.md`.
- In a current route-level check, verify the record remains correctly mapped and that the modal presents trigger, revised discovery, and every unchanged choice label in that order. Do not alter or invent a branch result to demonstrate the prose.
- Review voice, clarity, duplicate imagery, canon facts, and word count as a descriptive measurement only. A retained-text disposition passes if no edit improves the scene.

## Disposition

Recommended scope: one selector, one prose field, one bounded review. Accept a revision only when it makes this encounter clearer or more particular without changing its decision space. The live discovery string can remain untouched. This proposal has an evidenced existing route and can inform a future data-only package; it is not a path claim, approval to edit, or a substitute for current foreman selection and ownership checks.
