# Subject Plan 179.45 — The Interception: branching-quest discovery prose

Lane: A · Subject family: moral-choice branching content · Status: PROPOSAL (single-record, data-first prose subject; implementation route evidence included, not integration approval)

## Bounded outcome

Review one field—`discovery`—for selector `quest_moral_chain_betray_07` in `Assets/StreamingAssets/Data/moral_choice_quests_branching.json`. Current depth: 63 words / 387 characters. The aim is to clarify the authored encounter that players see when this moral-choice quest is offered. The plan does not ask for a longer passage; retain the current text if a revision would add no useful information.

**Editorial question:** Keep the competing account and the evidence gaps legible. Do not convert suspicion into proof or settle responsibility before the player chooses. Where a choice concerns disclosure, do not reveal a fact earlier than the current discovery text permits.

This is the Betray branch record **The Interception**. The branch identity and actual situation are source facts; do not use this pass to rewrite the larger quest arc.

## Verified premise and source evidence

- **VERIFIED (2026-09-24):** `quest_moral_chain_betray_07` is present exactly once in the current `schema_version: 1` `quests` array, which contains 100 entries.
- **VERIFIED:** the loader record shape is `id`, `display_name`, `category`, `trigger`, `discovery`, `location_id`, `min_day`, `max_day`, `choices`; the Core loader maps these to `MoralChoiceQuestDefinition` without changing the discovery text.
- **VERIFIED:** `Main.SetupMoralChoice` loads this branching catalog and appends its definitions to the same moral-choice catalog. `MoralChoiceModal.RefreshContent` renders `Trigger`, then `Discovery`, then the choice labels. This provides the existing data-only authoring route for the prose field.
- **VERIFIED:** the exact ID does not appear in the prior expansion indexes searched for this batch. Related-chain records are identified below for a semantic duplicate and continuity review.
- **MEASURED:** complete-catalog discovery text ranges from 42 to 143 whitespace-delimited words. Volume 56 of the local expansion authority measured the field shape and observed roughly 55 words in a head record; no plan-specific length target follows from that measurement.

**Trigger:** You intercept a radio broadcast about a supply convoy. Others are counting on your silence.

**Place:** `loc_neutral_ground` · **category:** `trust` · **day window:** 58–0 (0 max means unbounded in the current Core contract).

**Existing discovery prose (63 words / 387 characters):**

> The radio crackles at 0300. A convoy — unmarked, heading north through sector seven — carrying medical supplies and MREs. The frequency is supposed to be secure. Only three people in the shelter know you monitor it. The peacekeeper outpost would pay well for this intelligence. The knowledge keepers would trade knowledge. Your own people are hungry. The convoy passes in eighteen hours.

**Existing choices, frozen for context:** 1. Share the intelligence with the whole shelter; 2. Sell the intelligence to the highest bidder; 3. Use the information for your shelter alone; 4. Ignore it — some things aren't worth the risk

Adjacent chain records: predecessor `quest_moral_chain_betray_06`; successor `quest_moral_chain_betray_08`. Read them in the live catalog before a revision; do not borrow a neighboring record’s reveal, character knowledge, or outcome.

## Editorial treatment

Keep the competing account and the evidence gaps legible. Do not convert suspicion into proof or settle responsibility before the player chooses. Where a choice concerns disclosure, do not reveal a fact earlier than the current discovery text permits. The discovery text should set the decision in motion, not answer it. Keep its relation to the separate `trigger` field clear: avoid restating the trigger verbatim when the current text already develops it, and avoid treating a UI heading as new diegetic speech. Use physical and social details supported by this record’s setting. Prefer one precise observation, action, or pressure point over a run of interchangeable bleak images.

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
      "epitaph": "Shared the frequency with everyone. We traded labor for medicine. But the secret is shared now.",
      "label": "Share the intelligence with the whole shelter",
      "moral_delta": 6,
      "outcome_text": "You call a meeting and play the recording. The shelter mobilizes — a team rides out at dawn and intercepts the convoy peacefully, trading labor for a share of the medical supplies. Everyone eats better that week. But the frequency is now known to more people, and next time, the convoy might not be so cooperative."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Sold to both sides. The convoy was hit by raiders within the hour. The frequency went dead.",
      "label": "Sell the intelligence to the highest bidder",
      "moral_delta": -12,
      "outcome_text": "The peacekeeper outpost pays in fuel and ammunition. The knowledge keepers offer a map of underground cisterns. You take both payments. The convoy is hit by a raider party within the hour — tipped off by someone else who was listening. Nobody from your shelter was involved. The radio goes silent after that. No more broadcasts on that frequency."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Took a small team. Traded tools for MREs. Our shelter ate well. Nobody else knew.",
      "label": "Use the information for your shelter alone",
      "moral_delta": -4,
      "outcome_text": "You assemble a small team — people you trust, people who owe you. The convoy is intercepted at the narrow pass. You trade tools for a crate of MREs. Your shelter eats well for a week. Nobody else knows. The radio operator at the other end wonders who was listening."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Turned off the radio. Heard the engines pass. Some doors are better left closed.",
      "label": "Ignore it — some things aren't worth the risk",
      "moral_delta": 2,
      "outcome_text": "You turn off the radio and go back to sleep. The convoy passes. Someone else intercepts it — you hear the distant engines, then silence. You'll never know what was in those trucks. Some doors are better left closed."
    }
  ],
  "display_name": "The Interception",
  "id": "quest_moral_chain_betray_07",
  "location_id": "loc_neutral_ground",
  "max_day": 0,
  "min_day": 58,
  "trigger": "You intercept a radio broadcast about a supply convoy. Others are counting on your silence."
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
- Compare before/after JSON, proving only `quest_moral_chain_betray_07.discovery` changed; assert all IDs, array order, gates, trigger/category/place, choice payloads, flags, outcomes, and score deltas are unchanged.
- Run the existing data-integrity and moral-choice catalog checks that own this file; report the focused command selected under the then-current `TEST_POLICY.md`.
- In a current route-level check, verify the record remains correctly mapped and that the modal presents trigger, revised discovery, and every unchanged choice label in that order. Do not alter or invent a branch result to demonstrate the prose.
- Review voice, clarity, duplicate imagery, canon facts, and word count as a descriptive measurement only. A retained-text disposition passes if no edit improves the scene.

## Disposition

Recommended scope: one selector, one prose field, one bounded review. Accept a revision only when it makes this encounter clearer or more particular without changing its decision space. The live discovery string can remain untouched. This proposal has an evidenced existing route and can inform a future data-only package; it is not a path claim, approval to edit, or a substitute for current foreman selection and ownership checks.
