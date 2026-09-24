# Subject Plan 179.38 — The Last Library: branching-quest discovery prose

Lane: A · Subject family: moral-choice branching content · Status: PROPOSAL (single-record, data-first prose subject; implementation route evidence included, not integration approval)

## Bounded outcome

Review one field—`discovery`—for selector `quest_moral_chain_listen_12` in `Assets/StreamingAssets/Data/moral_choice_quests_branching.json`. Current depth: 77 words / 491 characters. The aim is to clarify the authored encounter that players see when this moral-choice quest is offered. The plan does not ask for a longer passage; retain the current text if a revision would add no useful information.

**Editorial question:** Make room for what the speaker is trying to communicate and what remains unsaid. Preserve the difference between testimony, memory, signal, and verified fact.

This is the Listen branch record **The Last Library**. The branch identity and actual situation are source facts; do not use this pass to rewrite the larger quest arc.

## Verified premise and source evidence

- **VERIFIED (2026-09-24):** `quest_moral_chain_listen_12` is present exactly once in the current `schema_version: 1` `quests` array, which contains 100 entries.
- **VERIFIED:** the loader record shape is `id`, `display_name`, `category`, `trigger`, `discovery`, `location_id`, `min_day`, `max_day`, `choices`; the Core loader maps these to `MoralChoiceQuestDefinition` without changing the discovery text.
- **VERIFIED:** `Main.SetupMoralChoice` loads this branching catalog and appends its definitions to the same moral-choice catalog. `MoralChoiceModal.RefreshContent` renders `Trigger`, then `Discovery`, then the choice labels. This provides the existing data-only authoring route for the prose field.
- **VERIFIED:** the exact ID does not appear in the prior expansion indexes searched for this batch. Related-chain records are identified below for a semantic duplicate and continuity review.
- **MEASURED:** complete-catalog discovery text ranges from 42 to 143 whitespace-delimited words. Volume 56 of the local expansion authority measured the field shape and observed roughly 55 words in a head record; no plan-specific length target follows from that measurement.

**Trigger:** The knowledge keeper is packing books into waterproof cases. They've been collecting them from the ruins. They want someone to know what's being saved — and what's being lost.

**Place:** `loc_municipal_archive` · **category:** `listen` · **day window:** 25–0 (0 max means unbounded in the current Core contract).

**Existing discovery prose (77 words / 491 characters):**

> The knowledge keeper's collection fills three rooms of the camp. Books stacked floor to ceiling — not just technical manuals, but novels, poetry collections, children's picture books, histories. They're cataloging each one on index cards, cross-referencing by subject and condition. 'We're saving the knowledge,' they say. Then they pause. 'But I need someone to tell me: which parts of the knowledge matter? I can't save everything. The water's rising in the lower rooms. I have to choose.'

**Existing choices, frozen for context:** 1. Help them sort. Listen to what each book means to them.; 2. "Save the practical books first. Stories can wait."; 3. Ask what they would save if they could only take one book.; 4. "It's just paper. Focus on surviving."

Adjacent chain records: predecessor `quest_moral_chain_listen_11`; successor `quest_moral_chain_listen_13`. Read them in the live catalog before a revision; do not borrow a neighboring record’s reveal, character knowledge, or outcome.

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
      "epitaph": "Saved books from rising water. A keeper chose poetry over comfort. I understood why.",
      "label": "Help them sort. Listen to what each book means to them.",
      "moral_delta": 14,
      "outcome_text": "You spend the night sorting. The knowledge keeper tells you about each book as they decide. The medical textbook — essential. The farming guide — essential. The poetry collection — they hold it longest. 'This one has a poem about rain,' they say. 'Real rain, before the ash. If we lose the technical books, people die. If we lose this, people forget what they were surviving for.' They save both. The water rises. Some books get wet. The knowledge keeper carries the poetry collection on their head to keep it dry."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Saved the practical books. Lost a poem about rain. The keeper dried the pages. Nothing came back.",
      "label": "\"Save the practical books first. Stories can wait.\"",
      "moral_delta": 3,
      "outcome_text": "The knowledge keeper nods and starts with the medical texts. Efficient. Rational. The poetry collection goes into the last case, on top, where the water reaches it first. They don't notice until it's too late. The technical knowledge survives. The poem about rain dissolves into pulp. You were right about priorities. The knowledge keeper doesn't say so. They just carry the wet case out and set it in the sun, as if drying might restore something that wasn't just ink."
    },
    {
      "empathy_delta": 3,
      "epitaph": "Asked which book mattered most. The keeper couldn't choose. That was the right answer.",
      "label": "Ask what they would save if they could only take one book.",
      "moral_delta": 10,
      "outcome_text": "The question stops them. They walk through the collection, touching spines. Finally, they pull out a small, water-stained book. A children's story. 'This was my daughter's,' they say. 'It's not important to anyone else. But it's the book I read to her every night before the exchange.' They put it back. 'I can't choose one. That's the point. Knowledge isn't a single book. It's the whole shelf.' They start carrying cases to higher ground, one by one, saving everything they can."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Called books just paper. The keeper stopped talking to me. Some losses aren't measured in calories.",
      "label": "\"It's just paper. Focus on surviving.\"",
      "moral_delta": -5,
      "outcome_text": "The knowledge keeper looks at you the way a doctor looks at someone who doesn't believe in germs. 'Paper,' they repeat. They return to their cataloging. You've drawn a line between survival and meaning, and the person who's dedicated their life to preserving meaning has decided you're on the wrong side of it. The water rises. The books get saved — or they don't. Either way, you're not part of it anymore."
    }
  ],
  "display_name": "The Last Library",
  "id": "quest_moral_chain_listen_12",
  "location_id": "loc_municipal_archive",
  "max_day": 0,
  "min_day": 25,
  "trigger": "The knowledge keeper is packing books into waterproof cases. They've been collecting them from the ruins. They want someone to know what's being saved — and what's being lost."
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
- Compare before/after JSON, proving only `quest_moral_chain_listen_12.discovery` changed; assert all IDs, array order, gates, trigger/category/place, choice payloads, flags, outcomes, and score deltas are unchanged.
- Run the existing data-integrity and moral-choice catalog checks that own this file; report the focused command selected under the then-current `TEST_POLICY.md`.
- In a current route-level check, verify the record remains correctly mapped and that the modal presents trigger, revised discovery, and every unchanged choice label in that order. Do not alter or invent a branch result to demonstrate the prose.
- Review voice, clarity, duplicate imagery, canon facts, and word count as a descriptive measurement only. A retained-text disposition passes if no edit improves the scene.

## Disposition

Recommended scope: one selector, one prose field, one bounded review. Accept a revision only when it makes this encounter clearer or more particular without changing its decision space. The live discovery string can remain untouched. This proposal has an evidenced existing route and can inform a future data-only package; it is not a path claim, approval to edit, or a substitute for current foreman selection and ownership checks.
