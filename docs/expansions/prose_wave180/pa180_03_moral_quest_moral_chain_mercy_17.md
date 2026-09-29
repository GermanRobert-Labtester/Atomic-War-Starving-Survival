# Subject Plan 180.03 — The Winter Ration: branching-quest discovery prose

Lane: A · Subject family: moral-choice branching content · Status: PROPOSAL (single-record, data-first prose subject; implementation route evidence included, not integration approval)

## Bounded outcome

Review one field—`discovery`—for selector `quest_moral_chain_mercy_17` in `Assets/StreamingAssets/Data/moral_choice_quests_branching.json`. Current depth: 49 words / 270 characters. The aim is to sharpen the authored encounter that players see when this late-chain moral-choice quest is offered. The plan does not ask for a longer passage; retain the current text if a revision would add no useful information.

**Editorial question:** Let the arithmetic be the drama—the count is precise, the winter is longer than the count, the allied camps are shorter still—and keep every number exactly as authored, because the outcomes re-spend those numbers and the prose has no authority to round them.

This is the Mercy branch record **The Winter Ration**, the chain's day-175 turn from sheltering individuals to feeding a region. The branch identity and actual situation are source facts; do not use this pass to rewrite the larger quest arc.

## Verified premise and source evidence

- **VERIFIED (2026-09-29):** `quest_moral_chain_mercy_17` is present exactly once in the current `schema_version: 1` `quests` array, which contains 100 entries (25 per branch: mercy, iron, listen, betray).
- **VERIFIED:** the loader record shape is `id`, `display_name`, `category`, `trigger`, `discovery`, `location_id`, `min_day`, `max_day`, `choices`; `MoralChoiceBranchQuestCatalogLoader.MapRecord` maps these to `MoralChoiceQuestDefinition` without changing the discovery text.
- **VERIFIED:** `Main.SetupMoralChoice` (`src/Main.MoralChoice.cs:33`) loads this branching catalog, appends its definitions to the moral-choice catalog, and initializes chain architecture from `moral_choice_chains.json`, whose gates reference this selector. `MoralChoiceModal.RefreshContent` renders the trigger, then the discovery, then the choice labels. This provides the existing data-only authoring route for the prose field.
- **VERIFIED:** the exact ID appears in no prior plan file or index (searched across `docs/expansions/` and `docs/plans/` on 2026-09-29); it enters the program with this wave.
- **MEASURED:** complete-catalog discovery text ranges from 42 to 143 whitespace-delimited words (re-measured 2026-09-29); no plan-specific length target follows from that measurement.

**Trigger:** Winter closes in. You have enough food for your people — barely. Three allied camps are short.

**Place:** `loc_shelter_storage` · **category:** `share` · **day window:** 175–0 (`MaxDay <= 0` means unbounded in the current Core contract).

**Existing discovery prose (49 words / 270 characters):**

> The count is precise: sixty-three days of food for your shelter at current population. Winter will last seventy. The three allied camps are worse — forty days, maybe less. You can stretch yours to cover the gap. Or you can protect your own and hope the thaw comes early.

**Existing choices, frozen for context:** 1. Redistribute evenly across all camps — everyone makes it or nobody does; 2. Share your surplus but keep your safety margin; 3. Organize a joint hunting/foraging expedition; 4. Keep your stores — they should have planned better

Adjacent chain records: predecessor `quest_moral_chain_mercy_16`; successor `quest_moral_chain_mercy_18`. Read them in the live catalog before a revision; do not borrow a neighboring record's reveal, character knowledge, or outcome.

**Numeric continuity (verified 2026-09-29):** the outcomes re-use the discovery's arithmetic—the second outcome shares 'the gap between sixty-three and seventy — seven days of food', and the fourth keeps 'your sixty-three days intact'. The numbers are contract facts shared across fields of this record, not decoration.

## Editorial treatment

This is the branch's arithmetic scene, and the prose should trust the count the way a quartermaster does. Sixty-three days at current population; a winter of seventy; allied camps at forty, maybe less. The 'maybe' is doing real work—it is the difference between a shortage and a funeral, and a revision may let that uncertainty breathe without converting it into a forecast. The scene's texture is a storage room: the tally, the sacks that are lighter than they look, the graphite column that has been corrected twice. A revision that adds one such material detail earns its place; a revision that adds sentiment about hunger does not, because the choice space already contains the sentiment as decisions.

The final two sentences lay out the fork in plain arithmetic terms—stretch or protect—and their flatness is the point. Resist any pull to moralize the second branch ('hope the thaw comes early' is exactly as far as the prose is allowed to lean, and it leans by stating a hope, not by scoring it). The fourth choice's epitaph will later say one camp did not make it to thaw; the discovery cannot know that future, cannot gesture at graves, and cannot soften the hoarding option into a straw man. All four options must remain livable readings of the same count.

Keep the camps plural and faceless as the record keeps them—'three allied camps', no named leaders, no individual hungry faces. Individualizing the camps would route the decision through sympathy the choice space deliberately does not organize on; this is a logistics decision with a moral interior, and its interior is the margin.

Preserve point of view and information access. The narration knows the shelter's count exactly because the shelter counted it; it knows the camps' figures as reported figures, which is why they carry 'maybe less'. Maintain names, counts, timing, and causal relationships exactly as the live record states them—sixty-three, seventy, forty, three camps, four choices. If a line needs a new fact to work, omit the line or route that separate idea to an independently evidenced subject.

Do not rewrite choices, outcomes, epitaphs, morality deltas, empathy deltas, flags, availability, or the consequence grammar. The prose may frame the decision but cannot imply a choice has already been made or that its outcome occurred. Avoid explaining the moral score: the Core contract keeps the score invisible, and the world and recorded consequences carry the meaning.

## Frozen data contract

Only this record's top-level `discovery` string is in scope. Every other value is immutable. This exact JSON snapshot is the complete record with `discovery` removed; compare it against the live object immediately before any future edit:

```json
{
  "category": "share",
  "choices": [
    {
      "empathy_delta": 2,
      "epitaph": "Leveled the stores across all camps. Half-rations for two weeks. Everyone made it. Shared hunger built something.",
      "label": "Redistribute evenly across all camps — everyone makes it or nobody does",
      "moral_delta": 16,
      "outcome_text": "You level the stores. Everyone goes to half-rations for the last two weeks. Everyone makes it. The hunger is shared, and shared hunger builds something that full bellies never do."
    },
    {
      "empathy_delta": 1,
      "epitaph": "Shared seven days of surplus. Everyone lived. One camp came close to the edge.",
      "label": "Share your surplus but keep your safety margin",
      "moral_delta": 8,
      "outcome_text": "You share the gap between sixty-three and seventy — seven days of food. The camps stretch it. One comes close to the edge. But everyone lives."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Organized a joint hunt. The winter forest gave reluctantly. Enough to close the gap.",
      "label": "Organize a joint hunting/foraging expedition",
      "moral_delta": 6,
      "outcome_text": "You gather hunters from all four camps. The winter forest gives reluctantly, but it gives. Enough to close the gap for the weakest camp. Not charity — cooperation."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Kept my stores. One camp didn't make it to thaw. The alliance survived on paper. Trust didn't.",
      "label": "Keep your stores — they should have planned better",
      "moral_delta": -8,
      "outcome_text": "You keep your sixty-three days intact. One allied camp doesn't make it to thaw. The other two barely survive. The alliance survives on paper. The trust doesn't survive the memory."
    }
  ],
  "display_name": "The Winter Ration",
  "id": "quest_moral_chain_mercy_17",
  "location_id": "loc_shelter_storage",
  "max_day": 0,
  "min_day": 175,
  "trigger": "Winter closes in. You have enough food for your people — barely. Three allied camps are short."
}
```

Do not add or remove records or properties. Do not alter `id`, `display_name`, `category`, `trigger`, `location_id`, `min_day`, `max_day`, choice count/order/labels, outcome text, epitaph, flag, moral or empathy delta, or any nested field. Do not introduce another state variant or a parallel narrative authority. The active integration ledger and current ownership ledger remain controlling for any later implementation package.

## Route, ownership, and save boundary

The data-first route is a one-field edit to the existing JSON entry, after rechecking its current schema and path ownership. The proven route is: `MoralChoiceBranchQuestCatalogLoader.Load` → `Main.SetupMoralChoice` definition registration and chain-gate initialization from `moral_choice_chains.json` → the availability checks in `GetAvailableMoralChoices` (unresolved, `IsAvailableOnDay`, `IsChainQuestAccessible`) → `MoralChoiceModal` display of trigger and discovery text. Exact display is still conditional on the existing day, chain, and resolved-state gates; this proposal does not change eligibility or guarantee that the record is offered in every campaign.

A top-level prose-only change has no save, RNG, or state impact. If it appears to require new host behavior, code, UI, flags, save data, mechanics, or a changed choice contract, stop and create a separately scoped, current-evidence integration plan. The subject plan authorizes no such work.

## Canon, continuity, and duplicate checks

1. Compare the record and both adjacent chain entries named above; verify the trigger-to-discovery handoff, reveal order, time, and unresolved questions. The alliance of camps should be consistent with the allied-camp relationships the branch's middle records author.
2. Review the relevant current lore and moral-choice canon. Use Volumes 56–57 of `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md` as discovery context, while treating shipped JSON and current code as implementation truth.
3. Search the existing moral-choice, event, echo, and encounter catalogs for duplicate scene beats and recycled phrasing—shared-stores winter decisions recur across survival fiction and possibly across this corpus; check the ration, cache, and harvest records before reusing any image of counted food. ID novelty alone is not content novelty.
4. Confirm each person, faction, place, resource quantity, and fact in the prose against existing authority; the day counts here are this record's own facts and must not be reconciled against any other catalog's food math. Do not add named entities or canon events by inference.
5. Keep the scene fictional, restrained, and playable. Avoid real-world references, borrowed phrasing, melodrama, moral instruction, and decorative suffering.

## Focused verification for a future edit

- Strict-parse the complete source catalog and confirm the schema version and 100-record count remain valid.
- Compare before/after JSON, proving only `quest_moral_chain_mercy_17.discovery` changed; assert all IDs, array order, gates, trigger/category/place, choice payloads, flags, outcomes, and score deltas are unchanged.
- Run the existing data-integrity and moral-choice catalog checks that own this file; report the focused command selected under the then-current `TEST_POLICY.md`.
- In a current route-level check, verify the record remains correctly mapped and that the modal presents trigger, revised discovery, and every unchanged choice label in that order. Do not alter or invent a branch result to demonstrate the prose.
- Review voice, clarity, duplicate imagery, canon facts, and word count as a descriptive measurement only. A retained-text disposition passes if no edit improves the scene.

## Disposition

Recommended scope: one selector, one prose field, one bounded review. Accept a revision only when it makes the count more felt—one true material detail of the storage room—without disturbing a single number or tilting the fork. The live discovery string can remain untouched. This proposal has an evidenced existing route and can inform a future data-only package; it is not a path claim, approval to edit, or a substitute for current foreman selection and ownership checks.
