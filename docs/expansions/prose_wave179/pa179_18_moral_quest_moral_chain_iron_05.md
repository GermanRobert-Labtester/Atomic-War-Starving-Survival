# Subject Plan 179.18 — The Road They Walk: branching-quest discovery prose

Lane: A · Subject family: moral-choice branching content · Status: PROPOSAL (single-record, data-first prose subject; implementation route evidence included, not integration approval)

## Bounded outcome

Review one field—`discovery`—for selector `quest_moral_chain_iron_05` in `Assets/StreamingAssets/Data/moral_choice_quests_branching.json`. Current depth: 86 words / 509 characters. The aim is to clarify the authored encounter that players see when this moral-choice quest is offered. The plan does not ask for a longer passage; retain the current text if a revision would add no useful information.

**Editorial question:** Keep the evidence, resource calculation, and authority pressure concrete. Do not make severity synonymous with competence or turn a hard policy into a promised result.

This is the Iron branch record **The Road They Walk**. The branch identity and actual situation are source facts; do not use this pass to rewrite the larger quest arc.

## Verified premise and source evidence

- **VERIFIED (2026-09-24):** `quest_moral_chain_iron_05` is present exactly once in the current `schema_version: 1` `quests` array, which contains 100 entries.
- **VERIFIED:** the loader record shape is `id`, `display_name`, `category`, `trigger`, `discovery`, `location_id`, `min_day`, `max_day`, `choices`; the Core loader maps these to `MoralChoiceQuestDefinition` without changing the discovery text.
- **VERIFIED:** `Main.SetupMoralChoice` loads this branching catalog and appends its definitions to the same moral-choice catalog. `MoralChoiceModal.RefreshContent` renders `Trigger`, then `Discovery`, then the choice labels. This provides the existing data-only authoring route for the prose field.
- **VERIFIED:** the exact ID does not appear in the prior expansion indexes searched for this batch. Related-chain records are identified below for a semantic duplicate and continuity review.
- **MEASURED:** complete-catalog discovery text ranges from 42 to 143 whitespace-delimited words. Volume 56 of the local expansion authority measured the field shape and observed roughly 55 words in a head record; no plan-specific length target follows from that measurement.

**Trigger:** You discover a rival group's supply route runs through territory you can control.

**Place:** `loc_eastern_road` · **category:** `dead` · **day window:** 10–0 (0 max means unbounded in the current Core contract).

**Existing discovery prose (86 words / 509 characters):**

> The trail is obvious once you know to look — boot prints worn into the same deer path, broken branches at head height, a cigarette butt crushed into the mud every forty meters like breadcrumbs. The Kessler group has been running supplies from the military outpost to their main camp through a narrow ravine that sits just inside your effective range. Three trips a week. Predictable. Undefended. You could end their supply line in an afternoon. Or you could do something more interesting with the information.

**Existing choices, frozen for context:** 1. Ambush the next run. Take the supplies. Send a message.; 2. Block the route quietly. Force them to come to you for passage.; 3. Sell the route information to a third party — let someone else profit from the chaos.; 4. Leave the route alone. Knowledge is leverage — use it later.

Adjacent chain records: predecessor `quest_moral_chain_iron_04`; successor `quest_moral_chain_iron_06`. Read them in the live catalog before a revision; do not borrow a neighboring record’s reveal, character knowledge, or outcome.

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
      "epitaph": "Four people. Two pack animals. Three boxes of peaches. One knife shared between them. I counted everything. I don't count their faces.",
      "label": "Ambush the next run. Take the supplies. Send a message.",
      "moral_delta": -11,
      "outcome_text": "You wait in the ravine with two people you trust. The Kessler supply team arrives at dawn — four people, two pack animals, armed but not expecting trouble. The confrontation lasts eleven seconds. You take everything: medical supplies, tools, three boxes of canned goods. You leave them with their clothes and one knife between them. The message arrives at Kessler's camp by nightfall. He doubles his guard. He also starts looking for a new route. You've made an enemy more careful, which is worse than an enemy who's merely angry."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Twenty percent. Every week. They call it a passage fee. I call it a business. The ravine doesn't call it anything. The ravine just has a trench now.",
      "label": "Block the route quietly. Force them to come to you for passage.",
      "moral_delta": -8,
      "outcome_text": "You don't ambush anyone. Instead, you collapse two sections of the ravine path with carefully placed debris and dig a trench across the flat approach. It takes three days of work. On the fourth day, a Kessler scout appears at your perimeter and asks — politely — about 'temporary passage arrangements.' You charge them twenty percent of every load. They pay. They pay every week after that. Kessler knows who's doing it. He can't prove it's you, and even if he could, the alternative is a longer route through the burn zone. You've built a toll road. The money is clean. The method isn't."
    },
    {
      "empathy_delta": 0,
      "epitaph": "I sold a road to the people who patrol roads. Everyone got what they wanted except Kessler. That was the point.",
      "label": "Sell the route information to a third party — let someone else profit from the chaos.",
      "moral_delta": -5,
      "outcome_text": "The peacekeepers at the outpost have been looking for reasons to expand their patrol zone. You sell them the route details — timing, numbers, armaments — framed as a 'security concern.' They intercept the next Kessler supply run with a show of force. No one dies, but Kessler loses two days of supplies to 'inspection.' Kessler blames the peacekeepers. The peacekeepers think they're doing good work. You're three steps removed and completely deniable. The peacekeepers pay you in trade credits. Kessler never suspects you."
    },
    {
      "empathy_delta": 0,
      "epitaph": "A bullet is worth one use. A secret is worth every use. I kept the secret. Kessler kept his supplies. We both kept our people.",
      "label": "Leave the route alone. Knowledge is leverage — use it later.",
      "moral_delta": 3,
      "outcome_text": "You memorize the pattern and walk away. The information sits in your head like a loaded weapon you haven't pointed at anyone yet. Three weeks later, when Kessler makes an aggressive move at the water station, you remind him — quietly, privately — that you know exactly how his supplies move. He goes very still. Then he makes a concession at the negotiating table that he wouldn't have made otherwise. No one had to bleed. The route was worth more unspent than spent. You keep the knowledge. It appreciates."
    }
  ],
  "display_name": "The Road They Walk",
  "id": "quest_moral_chain_iron_05",
  "location_id": "loc_eastern_road",
  "max_day": 0,
  "min_day": 10,
  "trigger": "You discover a rival group's supply route runs through territory you can control."
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
- Compare before/after JSON, proving only `quest_moral_chain_iron_05.discovery` changed; assert all IDs, array order, gates, trigger/category/place, choice payloads, flags, outcomes, and score deltas are unchanged.
- Run the existing data-integrity and moral-choice catalog checks that own this file; report the focused command selected under the then-current `TEST_POLICY.md`.
- In a current route-level check, verify the record remains correctly mapped and that the modal presents trigger, revised discovery, and every unchanged choice label in that order. Do not alter or invent a branch result to demonstrate the prose.
- Review voice, clarity, duplicate imagery, canon facts, and word count as a descriptive measurement only. A retained-text disposition passes if no edit improves the scene.

## Disposition

Recommended scope: one selector, one prose field, one bounded review. Accept a revision only when it makes this encounter clearer or more particular without changing its decision space. The live discovery string can remain untouched. This proposal has an evidenced existing route and can inform a future data-only package; it is not a path claim, approval to edit, or a substitute for current foreman selection and ownership checks.
