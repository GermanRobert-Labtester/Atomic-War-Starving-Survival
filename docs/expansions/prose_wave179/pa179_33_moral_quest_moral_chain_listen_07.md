# Subject Plan 179.33 — The Trader's Ledger: branching-quest discovery prose

Lane: A · Subject family: moral-choice branching content · Status: PROPOSAL (single-record, data-first prose subject; implementation route evidence included, not integration approval)

## Bounded outcome

Review one field—`discovery`—for selector `quest_moral_chain_listen_07` in `Assets/StreamingAssets/Data/moral_choice_quests_branching.json`. Current depth: 82 words / 456 characters. The aim is to clarify the authored encounter that players see when this moral-choice quest is offered. The plan does not ask for a longer passage; retain the current text if a revision would add no useful information.

**Editorial question:** Make room for what the speaker is trying to communicate and what remains unsaid. Preserve the difference between testimony, memory, signal, and verified fact.

This is the Listen branch record **The Trader's Ledger**. The branch identity and actual situation are source facts; do not use this pass to rewrite the larger quest arc.

## Verified premise and source evidence

- **VERIFIED (2026-09-24):** `quest_moral_chain_listen_07` is present exactly once in the current `schema_version: 1` `quests` array, which contains 100 entries.
- **VERIFIED:** the loader record shape is `id`, `display_name`, `category`, `trigger`, `discovery`, `location_id`, `min_day`, `max_day`, `choices`; the Core loader maps these to `MoralChoiceQuestDefinition` without changing the discovery text.
- **VERIFIED:** `Main.SetupMoralChoice` loads this branching catalog and appends its definitions to the same moral-choice catalog. `MoralChoiceModal.RefreshContent` renders `Trigger`, then `Discovery`, then the choice labels. This provides the existing data-only authoring route for the prose field.
- **VERIFIED:** the exact ID does not appear in the prior expansion indexes searched for this batch. Related-chain records are identified below for a semantic duplicate and continuity review.
- **MEASURED:** complete-catalog discovery text ranges from 42 to 143 whitespace-delimited words. Volume 56 of the local expansion authority measured the field shape and observed roughly 55 words in a head record; no plan-specific length target follows from that measurement.

**Trigger:** A trader at the water station keeps a second ledger — one they don't show customers. Tonight, they leave it open on their table.

**Place:** `loc_water_station` · **category:** `listen` · **day window:** 14–0 (0 max means unbounded in the current Core contract).

**Existing discovery prose (82 words / 456 characters):**

> The trader is packing up for the night. Their trade ledger is locked in a case, but a smaller book lies open on the table — a personal account. Not of money. Of debts. Names and dates and small annotations: 'owed three filters — lost daughter to fever, couldn't charge,' 'gave away last antibiotics — mother, same cough as his,' 'accepted a drawing instead of payment — child, needed to feel useful.' The trader sees you reading and doesn't close the book.

**Existing choices, frozen for context:** 1. Read the ledger. Ask about the people in it.; 2. "You could have gone broke doing this."; 3. "You should hide that. Someone could use it against you."; 4. "Sentiment doesn't keep you alive."

Adjacent chain records: predecessor `quest_moral_chain_listen_06`; successor `quest_moral_chain_listen_08`. Read them in the live catalog before a revision; do not borrow a neighboring record’s reveal, character knowledge, or outcome.

## Editorial treatment

Make room for what the speaker is trying to communicate and what remains unsaid. Preserve the difference between testimony, memory, signal, and verified fact. The discovery text should set the decision in motion, not answer it. Keep its relation to the separate `trigger` field clear: avoid restating the trigger verbatim when the current text already develops it, and avoid treating a UI heading as new diegetic speech. Use physical and social details supported by this record’s setting. Prefer one precise observation, action, or pressure point over a run of interchangeable bleak images.

Preserve point of view and information access. A character may know what they witnessed, remember, or were told; the narration must not know another person’s motive unless the record supports that knowledge. Keep allegations, uncertain identities, and disputed accounts explicitly uncertain. Maintain names, counts, timing, and causal relationships exactly as the live record states them. If a line needs a new fact to work, omit the line or route that separate idea to an independently evidenced subject.

Do not rewrite choices, outcomes, epitaphs, morality deltas, empathy deltas, flags, availability, or the consequence grammar. The prose may frame the decision but cannot imply a choice has already been made or that its outcome occurred. Avoid explaining the moral score: the Core contract says the score remains invisible, and the world and recorded consequences carry the meaning.

## Frozen data contract

Only this record’s top-level `discovery` string is in scope. Every other value is immutable. This exact JSON snapshot is the complete record with `discovery` removed; compare it against the live object immediately before any future edit:

```json
{
  "category": "listen",
  "choices": [
    {
      "empathy_delta": 4,
      "epitaph": "Read a trader's secret ledger. Every debt was a person. Every forgiveness was a choice.",
      "label": "Read the ledger. Ask about the people in it.",
      "moral_delta": 14,
      "outcome_text": "The trader sits back down. Each name has a story. The mother who traded her wedding ring for clean water — the trader gave it back. The child who brought a drawing — the trader still has it, pinned inside the ledger's back cover. The soldier who owed everything and died before paying — the trader crossed out the debt the day they heard. 'A ledger isn't about money,' the trader says. 'It's about who owed what to whom, and who forgave what when it didn't matter anymore.' You close the book gently. The trader nods."
    },
    {
      "empathy_delta": 1,
      "epitaph": "A trader went broke on day twelve. Kept going anyway. The real currency was always people.",
      "label": "\"You could have gone broke doing this.\"",
      "moral_delta": 6,
      "outcome_text": "The trader laughs — a dry, cracked sound. 'Went broke on day twelve. This is day four hundred and something.' They tap the ledger. 'This is the real accounting. The other one is just... keeping score with people who don't remember the rules.' They close the book and slide it into their pack. 'You're the first one who noticed,' they add, almost as an afterthought."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Gave practical advice. It was correct. It was also the least important thing in the room.",
      "label": "\"You should hide that. Someone could use it against you.\"",
      "moral_delta": 3,
      "outcome_text": "The trader considers this. Then they close the book and lock it away. 'Maybe,' they say. 'But if I start hiding who I am, what am I trading for?' The practical advice lands, but something else closes with the book. You've been useful. You've also been a reminder that the world is still dangerous, even at a water station, even at night, even to kindness."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Told a kind man sentiment was weakness. He pitied me for it.",
      "label": "\"Sentiment doesn't keep you alive.\"",
      "moral_delta": -5,
      "outcome_text": "The trader closes the ledger slowly. 'No,' they say. 'It doesn't. But it's what I'm alive for.' They pack up in silence. The conversation is over. You've drawn a line between survival and living, and the trader has chosen a side you don't understand yet."
    }
  ],
  "display_name": "The Trader's Ledger",
  "id": "quest_moral_chain_listen_07",
  "location_id": "loc_water_station",
  "max_day": 0,
  "min_day": 14,
  "trigger": "A trader at the water station keeps a second ledger — one they don't show customers. Tonight, they leave it open on their table."
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
- Compare before/after JSON, proving only `quest_moral_chain_listen_07.discovery` changed; assert all IDs, array order, gates, trigger/category/place, choice payloads, flags, outcomes, and score deltas are unchanged.
- Run the existing data-integrity and moral-choice catalog checks that own this file; report the focused command selected under the then-current `TEST_POLICY.md`.
- In a current route-level check, verify the record remains correctly mapped and that the modal presents trigger, revised discovery, and every unchanged choice label in that order. Do not alter or invent a branch result to demonstrate the prose.
- Review voice, clarity, duplicate imagery, canon facts, and word count as a descriptive measurement only. A retained-text disposition passes if no edit improves the scene.

## Disposition

Recommended scope: one selector, one prose field, one bounded review. Accept a revision only when it makes this encounter clearer or more particular without changing its decision space. The live discovery string can remain untouched. This proposal has an evidenced existing route and can inform a future data-only package; it is not a path claim, approval to edit, or a substitute for current foreman selection and ownership checks.
