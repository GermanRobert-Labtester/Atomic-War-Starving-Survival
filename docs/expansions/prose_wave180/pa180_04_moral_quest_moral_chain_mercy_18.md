# Subject Plan 180.04 — The Sacrifice Offer: branching-quest discovery prose

Lane: A · Subject family: moral-choice branching content · Status: PROPOSAL (single-record, data-first prose subject; implementation route evidence included, not integration approval)

## Bounded outcome

Review one field—`discovery`—for selector `quest_moral_chain_mercy_18` in `Assets/StreamingAssets/Data/moral_choice_quests_branching.json`. Current depth: 56 words / 288 characters. The aim is to sharpen the authored encounter that players see when this late-chain moral-choice quest is offered. The plan does not ask for a longer passage; retain the current text if a revision would add no useful information.

**Editorial question:** Let the offer stay in Joss's own terse grammar—east, loud, fast—and keep the final distinction between permission and acceptance intact, because that distinction is the entire decision; the narration must neither honor the sacrifice nor rescue him.

This is the Mercy branch record **The Sacrifice Offer**, the chain's day-185 arrival at the question the branch has been circling: what accepting help costs the helper. The branch identity and actual situation are source facts; do not use this pass to rewrite the larger quest arc.

## Verified premise and source evidence

- **VERIFIED (2026-09-29):** `quest_moral_chain_mercy_18` is present exactly once in the current `schema_version: 1` `quests` array, which contains 100 entries (25 per branch: mercy, iron, listen, betray).
- **VERIFIED:** the loader record shape is `id`, `display_name`, `category`, `trigger`, `discovery`, `location_id`, `min_day`, `max_day`, `choices`; `MoralChoiceBranchQuestCatalogLoader.MapRecord` maps these to `MoralChoiceQuestDefinition` without changing the discovery text.
- **VERIFIED:** `Main.SetupMoralChoice` (`src/Main.MoralChoice.cs:33`) loads this branching catalog, appends its definitions to the moral-choice catalog, and initializes chain architecture from `moral_choice_chains.json`, whose gates reference this selector. `MoralChoiceModal.RefreshContent` renders the trigger, then the discovery, then the choice labels. This provides the existing data-only authoring route for the prose field.
- **VERIFIED:** the exact ID appears in no prior plan file or index (searched across `docs/expansions/` and `docs/plans/` on 2026-09-29); it enters the program with this wave.
- **MEASURED:** complete-catalog discovery text ranges from 42 to 143 whitespace-delimited words (re-measured 2026-09-29); no plan-specific length target follows from that measurement.

**Trigger:** An ally offers to sacrifice themselves to draw raiders away from your community during an assault.

**Place:** `loc_shelter_meeting` · **category:** `trust` · **day window:** 185–0 (`MaxDay <= 0` means unbounded in the current Core contract).

**Existing discovery prose (56 words / 288 characters):**

> Joss — the scout whose leg you treated, the man who brought twelve people to your gate — stands in front of you with a plan. 'I go east, loud, fast. They follow me. You hit them from behind.' He knows what 'they follow me' means. He's not asking permission. He's asking if you'll let him.

**Existing choices, frozen for context:** 1. Refuse — find another way; 2. Accept — his choice, his right; 3. Go yourself instead — you won't ask others to die for you; 4. Negotiate with the raiders instead — maybe there's a deal

Adjacent chain records: predecessor `quest_moral_chain_mercy_17`; successor `quest_moral_chain_mercy_19`. Read them in the live catalog before a revision; do not borrow a neighboring record's reveal, character knowledge, or outcome.

**Entity continuity (verified 2026-09-29):** **Joss** appears in exactly two records of this catalog—this one and the later, still unplanned `quest_moral_chain_mercy_24`. A revision of this discovery writes the first half of a relationship whose second half is already authored downstream; it must leave Joss alive, recognizable, and uncontradicted for that return, and must not add history between him and the player beyond the two facts the record already states.

## Editorial treatment

The record carries its whole relationship in two appositional facts—the leg you treated, the twelve people at your gate—and that economy is the correct one. This is the chain remembering on the player's behalf: Joss is not introduced, he is recalled, and the recall is the argument. A revision may let one of the two facts land with slightly more body (a limp that has settled in, a count that has faces in it) but may not add a third fact; a third debt would turn gratitude into a ledger, and the scene is precisely about refusing to let a ledger decide. The appositions are also all the biography Joss gets here—no age, no rank, no description—because the plan he brings is meant to outshine the person bringing it, in his own design as much as the prose's.

His speech is three sentences of tactics and no adjectives: 'I go east, loud, fast. They follow me. You hit them from behind.' Keep it that way. A man converting himself into a route description is the record's one tonal feat; embellishing the speech would break its discipline, and paraphrasing it would break its voice. The narration's single act of interpretation follows—'He knows what "they follow me" means'—and should stay as narrow as it is: the prose understands the cost to him, states that he understands it, and declines to feel it for him.

The final sentence is the hinge and must survive any revision intact in meaning: he is not asking permission, he is asking if you will let him. The distinction between authority and witness is the exact shape of the choice space—refusal overrides his agency, acceptance honors it, substitution refuses the terms while paying personally, negotiation dissolves the frame. The prose must hold all four open; it cannot crown the refusal (the outcome's six messier hours and two injuries are not the discovery's to promise), and it cannot frame acceptance as tragedy already suffered.

Preserve point of view and information access. The narration knows what Joss says and what the player remembers; it does not know whether the raiders will take the bait, whether Joss will come back, or what he is not saying. Maintain names, counts, timing, and causal relationships exactly as the live record states them. If a line needs a new fact to work, omit the line or route that separate idea to an independently evidenced subject.

Do not rewrite choices, outcomes, epitaphs, morality deltas, empathy deltas, flags, availability, or the consequence grammar. The prose may frame the decision but cannot imply a choice has already been made or that its outcome occurred. Where consequences involve death risk, keep the prose humane and specific; do not make sacrifice decorative or pre-commit the branch. Avoid explaining the moral score: the Core contract keeps the score invisible, and the world and recorded consequences carry the meaning.

## Frozen data contract

Only this record's top-level `discovery` string is in scope. Every other value is immutable. This exact JSON snapshot is the complete record with `discovery` removed; compare it against the live object immediately before any future edit:

```json
{
  "category": "trust",
  "choices": [
    {
      "empathy_delta": 3,
      "epitaph": "Refused the sacrifice. Found another way. Messier, longer, two injured. Nobody died. Joss looked at me like I gave him something.",
      "label": "Refuse — find another way",
      "moral_delta": 12,
      "outcome_text": "You refuse. It takes six more hours to plan a flanking approach that doesn't require bait. It's messier. Two people are injured. Nobody dies. Joss looks at you afterward like you gave him something he didn't know he needed."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Let Joss run the decoy. He made it back. Barely. This time.",
      "label": "Accept — his choice, his right",
      "moral_delta": 0,
      "outcome_text": "He goes east. They follow. You hit from behind. The assault breaks. Joss makes it back — barely, bleeding, crawling the last hundred meters. He lives. This time. The next plan like this might not end the same."
    },
    {
      "empathy_delta": 4,
      "epitaph": "Ran the decoy myself. East, loud, fast. Made it back at dawn. Joss was waiting with water.",
      "label": "Go yourself instead — you won't ask others to die for you",
      "moral_delta": 14,
      "outcome_text": "You take the decoy run. East, loud, fast. They follow. Your people hit from behind. You run until your lungs burn, then you run further. You make it back at dawn, and Joss is waiting at the gate with water and the look of a man whose faith was just confirmed."
    },
    {
      "empathy_delta": 1,
      "epitaph": "Negotiated with the raiders. Twenty percent of the harvest. No blood. The chain of the deal hangs in the fields.",
      "label": "Negotiate with the raiders instead — maybe there's a deal",
      "moral_delta": 4,
      "outcome_text": "You send a runner with terms. The raiders consider it. They want twenty percent of your next harvest. You agree. The assault never comes. The cost is measured in grain, not blood. Joss is relieved. You feel the chain of the deal every time you look at the fields."
    }
  ],
  "display_name": "The Sacrifice Offer",
  "id": "quest_moral_chain_mercy_18",
  "location_id": "loc_shelter_meeting",
  "max_day": 0,
  "min_day": 185,
  "trigger": "An ally offers to sacrifice themselves to draw raiders away from your community during an assault."
}
```

Do not add or remove records or properties. Do not alter `id`, `display_name`, `category`, `trigger`, `location_id`, `min_day`, `max_day`, choice count/order/labels, outcome text, epitaph, flag, moral or empathy delta, or any nested field. Do not introduce another state variant or a parallel narrative authority. The active integration ledger and current ownership ledger remain controlling for any later implementation package.

## Route, ownership, and save boundary

The data-first route is a one-field edit to the existing JSON entry, after rechecking its current schema and path ownership. The proven route is: `MoralChoiceBranchQuestCatalogLoader.Load` → `Main.SetupMoralChoice` definition registration and chain-gate initialization from `moral_choice_chains.json` → the availability checks in `GetAvailableMoralChoices` (unresolved, `IsAvailableOnDay`, `IsChainQuestAccessible`) → `MoralChoiceModal` display of trigger and discovery text. Exact display is still conditional on the existing day, chain, and resolved-state gates; this proposal does not change eligibility or guarantee that the record is offered in every campaign.

A top-level prose-only change has no save, RNG, or state impact. If it appears to require new host behavior, code, UI, flags, save data, mechanics, or a changed choice contract, stop and create a separately scoped, current-evidence integration plan. The subject plan authorizes no such work.

## Canon, continuity, and duplicate checks

1. Compare the record and both adjacent chain entries named above; verify the trigger-to-discovery handoff, reveal order, time, and unresolved questions. Then read the downstream `quest_moral_chain_mercy_24` before finalizing anything that touches Joss's manner of speaking or his standing in the community.
2. Review the relevant current lore and moral-choice canon. Use Volumes 56–57 of `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md` as discovery context, while treating shipped JSON and current code as implementation truth.
3. Search the existing moral-choice, event, echo, and encounter catalogs for duplicate scene beats and recycled phrasing—the volunteer-decoy is a war-story staple; confirm this record does not echo an already-authored volunteer, martyrdom, or cult-sacrifice scene. ID novelty alone is not content novelty.
4. Confirm each person, faction, place, resource quantity, and fact in the prose against existing authority; the treated leg and the twelve people are this record's facts and must not be reconciled into any other quest's history. Do not add named entities or canon events by inference.
5. Keep the scene fictional, restrained, and playable. Avoid real-world references, borrowed phrasing, melodrama, moral instruction, and decorative suffering.

## Focused verification for a future edit

- Strict-parse the complete source catalog and confirm the schema version and 100-record count remain valid.
- Compare before/after JSON, proving only `quest_moral_chain_mercy_18.discovery` changed; assert all IDs, array order, gates, trigger/category/place, choice payloads, flags, outcomes, and score deltas are unchanged.
- Run the existing data-integrity and moral-choice catalog checks that own this file; report the focused command selected under the then-current `TEST_POLICY.md`.
- In a current route-level check, verify the record remains correctly mapped and that the modal presents trigger, revised discovery, and every unchanged choice label in that order. Do not alter or invent a branch result to demonstrate the prose.
- Review voice, clarity, duplicate imagery, canon facts, and word count as a descriptive measurement only. A retained-text disposition passes if no edit improves the scene.

## Disposition

Recommended scope: one selector, one prose field, one bounded review. Accept a revision only when it deepens the recall without inflating it—Joss more present, never more explained—and leaves the permission/acceptance hinge untouched. The live discovery string can remain untouched. This proposal has an evidenced existing route and can inform a future data-only package; it is not a path claim, approval to edit, or a substitute for current foreman selection and ownership checks.
