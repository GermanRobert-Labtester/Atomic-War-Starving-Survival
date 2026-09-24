# Subject Plan 179.20 — Loose Lips: branching-quest discovery prose

Lane: A · Subject family: moral-choice branching content · Status: PROPOSAL (single-record, data-first prose subject; implementation route evidence included, not integration approval)

## Bounded outcome

Review one field—`discovery`—for selector `quest_moral_chain_iron_07` in `Assets/StreamingAssets/Data/moral_choice_quests_branching.json`. Current depth: 111 words / 637 characters. The aim is to clarify the authored encounter that players see when this moral-choice quest is offered. The plan does not ask for a longer passage; retain the current text if a revision would add no useful information.

**Editorial question:** Keep the evidence, resource calculation, and authority pressure concrete. Do not make severity synonymous with competence or turn a hard policy into a promised result. Where consequences involve punishment, keep the prose humane and specific; do not make violence decorative or pre-commit the branch.

This is the Iron branch record **Loose Lips**. The branch identity and actual situation are source facts; do not use this pass to rewrite the larger quest arc.

## Verified premise and source evidence

- **VERIFIED (2026-09-24):** `quest_moral_chain_iron_07` is present exactly once in the current `schema_version: 1` `quests` array, which contains 100 entries.
- **VERIFIED:** the loader record shape is `id`, `display_name`, `category`, `trigger`, `discovery`, `location_id`, `min_day`, `max_day`, `choices`; the Core loader maps these to `MoralChoiceQuestDefinition` without changing the discovery text.
- **VERIFIED:** `Main.SetupMoralChoice` loads this branching catalog and appends its definitions to the same moral-choice catalog. `MoralChoiceModal.RefreshContent` renders `Trigger`, then `Discovery`, then the choice labels. This provides the existing data-only authoring route for the prose field.
- **VERIFIED:** the exact ID does not appear in the prior expansion indexes searched for this batch. Related-chain records are identified below for a semantic duplicate and continuity review.
- **MEASURED:** complete-catalog discovery text ranges from 42 to 143 whitespace-delimited words. Volume 56 of the local expansion authority measured the field shape and observed roughly 55 words in a head record; no plan-specific length target follows from that measurement.

**Trigger:** You learn someone in your group has been sharing information with outsiders.

**Place:** `loc_shelter_meeting` · **category:** `trust` · **day window:** 14–0 (0 max means unbounded in the current Core contract).

**Existing discovery prose (111 words / 637 characters):**

> It's not a spy — not exactly. It's Callum, one of your scouts, who's been talking to his sister. His sister lives in the Aldric settlement. They trade news the way families do — how he's eating, whether he's safe, what the weather's been like. Except some of what he's shared is operational: your patrol routes, your water rationing schedule, the fact that your grain bin is running low. He doesn't think it's sensitive. His sister doesn't think it's intelligence. But the Aldric leader is observant, and patterns are patterns. Callum is sitting by the fire when you confront him. He doesn't deny it. He says he didn't think it mattered.

**Existing choices, frozen for context:** 1. Expel him. Make it public. Set an example.; 2. Handle it privately. Reassign him to a role with less access.; 3. Use it. Feed Callum false information to pass to his sister — and through her, to Aldric.; 4. Let it continue. Family bonds aren't espionage. Adjust your operational security instead.

Adjacent chain records: predecessor `quest_moral_chain_iron_06`; successor `quest_moral_chain_iron_08`. Read them in the live catalog before a revision; do not borrow a neighboring record’s reveal, character knowledge, or outcome.

## Editorial treatment

Keep the evidence, resource calculation, and authority pressure concrete. Do not make severity synonymous with competence or turn a hard policy into a promised result. Where consequences involve punishment, keep the prose humane and specific; do not make violence decorative or pre-commit the branch. The discovery text should set the decision in motion, not answer it. Keep its relation to the separate `trigger` field clear: avoid restating the trigger verbatim when the current text already develops it, and avoid treating a UI heading as new diegetic speech. Use physical and social details supported by this record’s setting. Prefer one precise observation, action, or pressure point over a run of interchangeable bleak images.

Preserve point of view and information access. A character may know what they witnessed, remember, or were told; the narration must not know another person’s motive unless the record supports that knowledge. Keep allegations, uncertain identities, and disputed accounts explicitly uncertain. Maintain names, counts, timing, and causal relationships exactly as the live record states them. If a line needs a new fact to work, omit the line or route that separate idea to an independently evidenced subject.

Do not rewrite choices, outcomes, epitaphs, morality deltas, empathy deltas, flags, availability, or the consequence grammar. The prose may frame the decision but cannot imply a choice has already been made or that its outcome occurred. Avoid explaining the moral score: the Core contract says the score remains invisible, and the world and recorded consequences carry the meaning.

## Frozen data contract

Only this record’s top-level `discovery` string is in scope. Every other value is immutable. This exact JSON snapshot is the complete record with `discovery` removed; compare it against the live object immediately before any future edit:

```json
{
  "category": "trust",
  "choices": [
    {
      "empathy_delta": 0,
      "epitaph": "He talked to his sister. I made an example of him. The group is quieter now. Quiet isn't the same as loyal.",
      "label": "Expel him. Make it public. Set an example.",
      "moral_delta": -9,
      "outcome_text": "You do it at the morning assembly. Callum's face goes through several colors before settling on white. You lay out what he shared, what it could cost, what the precedent means. He's gone within the hour — pack on his back, heading for a settlement that may or may not take him. The message lands: information security is not optional. Three people who were close to Callum stop meeting your eyes. They don't disagree. They're afraid. Fear and loyalty look identical from a distance. You'll take what you can get."
    },
    {
      "empathy_delta": 0,
      "epitaph": "He didn't mean it. That's what makes it dangerous. I moved him sideways and started reading his mail. I tell myself it's temporary.",
      "label": "Handle it privately. Reassign him to a role with less access.",
      "moral_delta": -2,
      "outcome_text": "You pull Callum aside. Explain what his 'family updates' actually reveal. His face changes when he understands. He didn't think — that's the problem, not malice. You move him to interior maintenance. No patrols, no route knowledge, no access to supply counts. He accepts it quietly. His sister writes to him and he writes back, but the letters go through your mail now. You read them. They're just family news. You tell yourself that's why you stopped checking after the third week. You almost believe it."
    },
    {
      "empathy_delta": 0,
      "epitaph": "I turned a brother into a broadcast tower. He talks to his sister. I talk through him. Aldric hears what I want them to hear. Nobody lied. That's the worst part.",
      "label": "Use it. Feed Callum false information to pass to his sister — and through her, to Aldric.",
      "moral_delta": -13,
      "outcome_text": "You sit down with Callum and explain the situation calmly. Then you explain the alternative: he can continue talking to his sister, but the information flows both ways now. You give him a version of your patrol schedule that's two days offset from reality. You tell him your grain reserves are higher than they are. Callum passes it along because he believes it — he has no reason not to. Three weeks later, Aldric makes a trade offer based on the false supply data. They overestimate your position. You get better terms. Callum never knows he's a conduit. His sister never knows either. The lie travels through love like a virus through blood."
    },
    {
      "empathy_delta": 1,
      "epitaph": "I changed the locks instead of changing the man. His sister's letters are probably just letters. Probably.",
      "label": "Let it continue. Family bonds aren't espionage. Adjust your operational security instead.",
      "moral_delta": 5,
      "outcome_text": "You change the things that matter — rotate patrol schedules more frequently, move sensitive supply counts to a need-to-know basis, stop discussing strategy where family members might overhear. Callum keeps writing to his sister. You don't read the letters. The Aldric leader is still observant, but there's less to observe. It costs you efficiency — more compartmentalization means slower decisions — but it costs you less than expelling a good scout over a conversation with his sister. Callum's sister writes back about their mother's garden. Nothing in the letter is intelligence. You're fairly sure."
    }
  ],
  "display_name": "Loose Lips",
  "id": "quest_moral_chain_iron_07",
  "location_id": "loc_shelter_meeting",
  "max_day": 0,
  "min_day": 14,
  "trigger": "You learn someone in your group has been sharing information with outsiders."
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
- Compare before/after JSON, proving only `quest_moral_chain_iron_07.discovery` changed; assert all IDs, array order, gates, trigger/category/place, choice payloads, flags, outcomes, and score deltas are unchanged.
- Run the existing data-integrity and moral-choice catalog checks that own this file; report the focused command selected under the then-current `TEST_POLICY.md`.
- In a current route-level check, verify the record remains correctly mapped and that the modal presents trigger, revised discovery, and every unchanged choice label in that order. Do not alter or invent a branch result to demonstrate the prose.
- Review voice, clarity, duplicate imagery, canon facts, and word count as a descriptive measurement only. A retained-text disposition passes if no edit improves the scene.

## Disposition

Recommended scope: one selector, one prose field, one bounded review. Accept a revision only when it makes this encounter clearer or more particular without changing its decision space. The live discovery string can remain untouched. This proposal has an evidenced existing route and can inform a future data-only package; it is not a path claim, approval to edit, or a substitute for current foreman selection and ownership checks.
