# Subject Plan 179.41 — Stolen Credit: branching-quest discovery prose

Lane: A · Subject family: moral-choice branching content · Status: PROPOSAL (single-record, data-first prose subject; implementation route evidence included, not integration approval)

## Bounded outcome

Review one field—`discovery`—for selector `quest_moral_chain_betray_03` in `Assets/StreamingAssets/Data/moral_choice_quests_branching.json`. Current depth: 74 words / 405 characters. The aim is to clarify the authored encounter that players see when this moral-choice quest is offered. The plan does not ask for a longer passage; retain the current text if a revision would add no useful information.

**Editorial question:** Keep the competing account and the evidence gaps legible. Do not convert suspicion into proof or settle responsibility before the player chooses.

This is the Betray branch record **Stolen Credit**. The branch identity and actual situation are source facts; do not use this pass to rewrite the larger quest arc.

## Verified premise and source evidence

- **VERIFIED (2026-09-24):** `quest_moral_chain_betray_03` is present exactly once in the current `schema_version: 1` `quests` array, which contains 100 entries.
- **VERIFIED:** the loader record shape is `id`, `display_name`, `category`, `trigger`, `discovery`, `location_id`, `min_day`, `max_day`, `choices`; the Core loader maps these to `MoralChoiceQuestDefinition` without changing the discovery text.
- **VERIFIED:** `Main.SetupMoralChoice` loads this branching catalog and appends its definitions to the same moral-choice catalog. `MoralChoiceModal.RefreshContent` renders `Trigger`, then `Discovery`, then the choice labels. This provides the existing data-only authoring route for the prose field.
- **VERIFIED:** the exact ID does not appear in the prior expansion indexes searched for this batch. Related-chain records are identified below for a semantic duplicate and continuity review.
- **MEASURED:** complete-catalog discovery text ranges from 42 to 143 whitespace-delimited words. Volume 56 of the local expansion authority measured the field shape and observed roughly 55 words in a head record; no plan-specific length target follows from that measurement.

**Trigger:** A quiet engineer fixes the water recycler. You can claim the credit and the political capital that comes with it.

**Place:** `loc_shelter_storage` · **category:** `trust` · **day window:** 25–0 (0 max means unbounded in the current Core contract).

**Existing discovery prose (74 words / 405 characters):**

> The water recycler has been dead for eleven days. Rationing is brutal. Then Ise — quiet Ise, the former hydrologist who never speaks at meetings — fixes it overnight. She tells no one. By morning, the tanks are filling and people are weeping with relief. You saw her working. You know what she did. But nobody else does. The shelter is looking for a hero, and Ise is already back in her corner, invisible.

**Existing choices, frozen for context:** 1. Publicly credit Ise for the repair; 2. Claim the repair was yours; 3. Use the knowledge to recruit Ise as an ally; 4. Say nothing and let the mystery stand

Adjacent chain records: predecessor `quest_moral_chain_betray_02`; successor `quest_moral_chain_betray_04`. Read them in the live catalog before a revision; do not borrow a neighboring record’s reveal, character knowledge, or outcome.

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
      "empathy_delta": 2,
      "epitaph": "Said her name. She flinched at the applause. Preferred the invisible work. But the extra ration was real.",
      "label": "Publicly credit Ise for the repair",
      "moral_delta": 6,
      "outcome_text": "You find Ise at the morning briefing and say her name. The room turns. She flinches like she's been struck. They applaud. She doesn't smile. Later she tells you, quietly, that she preferred the invisibility. But her ration card gets upgraded, and that's real."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Took credit for Ise's work. She never contradicted me. She just stopped meeting my eyes.",
      "label": "Claim the repair was yours",
      "moral_delta": -10,
      "outcome_text": "You mention it casually at the briefing — 'I was up all night with the recycler.' People clap. Ise looks at you from across the room. She doesn't contradict you. She doesn't speak at all. After that, she fixes things in the dark, and you keep getting the credit. She stops meeting your eyes."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Made Ise an offer she couldn't refuse. Her skills, my platform. Sealed in silence.",
      "label": "Use the knowledge to recruit Ise as an ally",
      "moral_delta": -5,
      "outcome_text": "You find Ise privately. 'I know what you did,' you say. She waits. 'I won't tell anyone — if you help me with the next project. And the one after that.' She understands the transaction. Her skills, your platform. She nods once. The alliance is sealed in silence."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Said nothing. The water runs. The ghost in the machine room stays anonymous.",
      "label": "Say nothing and let the mystery stand",
      "moral_delta": 1,
      "outcome_text": "Nobody knows who fixed it. The water runs. People are grateful to the anonymous ghost in the machine room. Ise catches your eye once and looks away. She suspects you know. You'll never discuss it."
    }
  ],
  "display_name": "Stolen Credit",
  "id": "quest_moral_chain_betray_03",
  "location_id": "loc_shelter_storage",
  "max_day": 0,
  "min_day": 25,
  "trigger": "A quiet engineer fixes the water recycler. You can claim the credit and the political capital that comes with it."
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
- Compare before/after JSON, proving only `quest_moral_chain_betray_03.discovery` changed; assert all IDs, array order, gates, trigger/category/place, choice payloads, flags, outcomes, and score deltas are unchanged.
- Run the existing data-integrity and moral-choice catalog checks that own this file; report the focused command selected under the then-current `TEST_POLICY.md`.
- In a current route-level check, verify the record remains correctly mapped and that the modal presents trigger, revised discovery, and every unchanged choice label in that order. Do not alter or invent a branch result to demonstrate the prose.
- Review voice, clarity, duplicate imagery, canon facts, and word count as a descriptive measurement only. A retained-text disposition passes if no edit improves the scene.

## Disposition

Recommended scope: one selector, one prose field, one bounded review. Accept a revision only when it makes this encounter clearer or more particular without changing its decision space. The live discovery string can remain untouched. This proposal has an evidenced existing route and can inform a future data-only package; it is not a path claim, approval to edit, or a substitute for current foreman selection and ownership checks.
