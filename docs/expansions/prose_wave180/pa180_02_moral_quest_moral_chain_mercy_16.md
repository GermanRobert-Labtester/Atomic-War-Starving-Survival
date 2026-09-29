# Subject Plan 180.02 — The Prodigal Raider: branching-quest discovery prose

Lane: A · Subject family: moral-choice branching content · Status: PROPOSAL (single-record, data-first prose subject; implementation route evidence included, not integration approval)

## Bounded outcome

Review one field—`discovery`—for selector `quest_moral_chain_mercy_16` in `Assets/StreamingAssets/Data/moral_choice_quests_branching.json`. Current depth: 46 words / 258 characters. The aim is to sharpen the authored encounter that players see when this late-chain moral-choice quest is offered. The plan does not ask for a longer passage; retain the current text if a revision would add no useful information.

**Editorial question:** Let the defector's past be present as physical fact and self-reported speech—scars, empty hands, a stated price—without the narration ever ruling on whether the offer is sincere, because the fourth choice is that doubt and the prose must not settle it.

This is the Mercy branch record **The Prodigal Raider**, the chain's day-165 test of whether a place can be earned by someone who used to take places. The branch identity and actual situation are source facts; do not use this pass to rewrite the larger quest arc.

## Verified premise and source evidence

- **VERIFIED (2026-09-29):** `quest_moral_chain_mercy_16` is present exactly once in the current `schema_version: 1` `quests` array, which contains 100 entries (25 per branch: mercy, iron, listen, betray).
- **VERIFIED:** the loader record shape is `id`, `display_name`, `category`, `trigger`, `discovery`, `location_id`, `min_day`, `max_day`, `choices`; `MoralChoiceBranchQuestCatalogLoader.MapRecord` maps these to `MoralChoiceQuestDefinition` without changing the discovery text.
- **VERIFIED:** `Main.SetupMoralChoice` (`src/Main.MoralChoice.cs:33`) loads this branching catalog, appends its definitions to the moral-choice catalog, and initializes chain architecture from `moral_choice_chains.json`, whose gates reference this selector. `MoralChoiceModal.RefreshContent` renders the trigger, then the discovery, then the choice labels. This provides the existing data-only authoring route for the prose field.
- **VERIFIED:** the exact ID appears in no prior plan file or index (searched across `docs/expansions/` and `docs/plans/` on 2026-09-29); it enters the program with this wave.
- **MEASURED:** complete-catalog discovery text ranges from 42 to 143 whitespace-delimited words (re-measured 2026-09-29); no plan-specific length target follows from that measurement.

**Trigger:** A raider captain defects and asks to join your community — with intelligence about a planned attack on your corridor.

**Place:** `loc_shelter_gate` · **category:** `trust` · **day window:** 165–0 (`MaxDay <= 0` means unbounded in the current Core contract).

**Existing discovery prose (46 words / 258 characters):**

> He arrives alone, unarmed, hands visible. Former raider captain — you recognize the scars. 'I'm done with it,' he says. 'And I know what they're planning for your eastern corridor. I'll tell you everything. But I want a place here. A real place. Not a cell.'

**Existing choices, frozen for context:** 1. Accept him — everyone deserves a second chance; 2. Take the intelligence but house him outside the walls; 3. Take the intelligence and send him away; 4. Refuse — it's a trap

Adjacent chain records: predecessor `quest_moral_chain_mercy_15`; successor `quest_moral_chain_mercy_17`. Read them in the live catalog before a revision; do not borrow a neighboring record's reveal, character knowledge, or outcome.

**Entity continuity (verified 2026-09-29):** the man is unnamed, and the scene deliberately pairs him with the shelter gate—the same place the chain opened with strangers asking for entry. The echo is structural; a revision may rely on it but should not annotate it.

## Editorial treatment

The record runs on three physical facts and one exchange. Alone, unarmed, hands visible: the man arrives in the grammar of surrender, and the prose should keep that grammar material—what the guard sees from the wall, what the body does when it has decided to be seen. The scars do the remembering the dialogue refuses: 'Former raider captain — you recognize the scars' is recognition without biography, and a revision should not convert it into backstory. Where a past is carried on the skin, one detail is worth more than a paragraph, and the paragraph would only sound like justification.

His speech is a transaction stated plainly: information for a place. 'A real place. Not a cell.' is the record's sharpest line because it names the exact boundary the choice space then negotiates—choice 2 houses him outside the walls, choice 3 takes the goods and refuses the price. A revision may tighten the transaction but must not soften it into an apology or harden it into a threat; either would spend the ambiguity that the fourth choice ('Refuse — it's a trap') is made of. The narration cannot know whether Thursday's attack is real, whether the scars mean command or conscription, or whether 'done with it' is exhaustion or tactic. Everything uncertain must read as uncertain.

Keep the threat concrete and local: the eastern corridor, an attack being planned, intelligence he claims to hold in full. A revision may add the sensory reality of the gate in winter—breath, distance, the long pause before anyone opens anything—but the intelligence itself stays unverified and the man stays undescribed beyond what a guard could witness.

Preserve point of view and information access. A character may know what they witnessed, remember, or were told; the narration must not know another person's motive unless the record supports that knowledge. Keep allegations, uncertain identities, and disputed accounts explicitly uncertain. Maintain names, counts, timing, and causal relationships exactly as the live record states them. If a line needs a new fact to work, omit the line or route that separate idea to an independently evidenced subject.

Do not rewrite choices, outcomes, epitaphs, morality deltas, empathy deltas, flags, availability, or the consequence grammar. The prose may frame the decision but cannot imply a choice has already been made or that its outcome occurred. Where consequences involve violence, keep the prose humane and specific; do not make violence decorative or pre-commit the branch. Avoid explaining the moral score: the Core contract keeps the score invisible, and the world and recorded consequences carry the meaning.

## Frozen data contract

Only this record's top-level `discovery` string is in scope. Every other value is immutable. This exact JSON snapshot is the complete record with `discovery` removed; compare it against the live object immediately before any future edit:

```json
{
  "category": "trust",
  "choices": [
    {
      "empathy_delta": 2,
      "epitaph": "Accepted the defector. His intelligence saved the corridor. He watches his own hands for old habits.",
      "label": "Accept him — everyone deserves a second chance",
      "moral_delta": 12,
      "outcome_text": "He tells you everything. The attack is planned for next Thursday, eastern approach, twenty raiders. You prepare. The attack comes and breaks against your foreknowledge like water on stone. He earns his place in the weeks after — slowly, watching his own hands for the old habits."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Took his intelligence. Housed him outside the walls. Close enough to see us. Far enough to not quite belong.",
      "label": "Take the intelligence but house him outside the walls",
      "moral_delta": 6,
      "outcome_text": "He talks. You listen. The attack fails. He lives outside the walls in a small structure. Close enough to see the community. Far enough that the community can pretend not to see him. Liminal. Earned, but unfinished."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Used his intelligence. Sent him away. Transaction complete. No home for him in any of it.",
      "label": "Take the intelligence and send him away",
      "moral_delta": 0,
      "outcome_text": "He talks. You use it. You send him east with supplies and thanks. The attack fails. He vanishes into the wasteland with good information and no home. The transaction is complete."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Turned away the defector. The attack came Thursday. Three died. Paranoia cost three lives.",
      "label": "Refuse — it's a trap",
      "moral_delta": -4,
      "outcome_text": "You turn him away. The attack comes Thursday as he warned. You're unprepared. Three people die on the eastern wall. You'll wonder, for months, whether the paranoia was worth three lives."
    }
  ],
  "display_name": "The Prodigal Raider",
  "id": "quest_moral_chain_mercy_16",
  "location_id": "loc_shelter_gate",
  "max_day": 0,
  "min_day": 165,
  "trigger": "A raider captain defects and asks to join your community — with intelligence about a planned attack on your corridor."
}
```

Do not add or remove records or properties. Do not alter `id`, `display_name`, `category`, `trigger`, `location_id`, `min_day`, `max_day`, choice count/order/labels, outcome text, epitaph, flag, moral or empathy delta, or any nested field. Do not introduce another state variant or a parallel narrative authority. The active integration ledger and current ownership ledger remain controlling for any later implementation package.

## Route, ownership, and save boundary

The data-first route is a one-field edit to the existing JSON entry, after rechecking its current schema and path ownership. The proven route is: `MoralChoiceBranchQuestCatalogLoader.Load` → `Main.SetupMoralChoice` definition registration and chain-gate initialization from `moral_choice_chains.json` → the availability checks in `GetAvailableMoralChoices` (unresolved, `IsAvailableOnDay`, `IsChainQuestAccessible`) → `MoralChoiceModal` display of trigger and discovery text. Exact display is still conditional on the existing day, chain, and resolved-state gates; this proposal does not change eligibility or guarantee that the record is offered in every campaign.

A top-level prose-only change has no save, RNG, or state impact. If it appears to require new host behavior, code, UI, flags, save data, mechanics, or a changed choice contract, stop and create a separately scoped, current-evidence integration plan. The subject plan authorizes no such work.

## Canon, continuity, and duplicate checks

1. Compare the record and both adjacent chain entries named above; verify the trigger-to-discovery handoff, reveal order, time, and unresolved questions. Note that the record shares its gate setting with the chain's opening and check the two scenes do not blur.
2. Review the relevant current lore and moral-choice canon. Use Volumes 56–57 of `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md` as discovery context, while treating shipped JSON and current code as implementation truth.
3. Search the existing moral-choice, event, echo, and encounter catalogs for duplicate scene beats and recycled phrasing—defector-at-the-gate is a recurring post-apocalyptic template; this record must stay distinct from prisoner, spy, and ambush variants already authored. ID novelty alone is not content novelty.
4. Confirm each person, faction, place, resource quantity, and fact in the prose against existing authority. The raiders stay an unaffiliated threat here; do not attach them to a named faction or invent their leadership. Do not add named entities or canon events by inference.
5. Keep the scene fictional, restrained, and playable. Avoid real-world references, borrowed phrasing, melodrama, moral instruction, and decorative suffering.

## Focused verification for a future edit

- Strict-parse the complete source catalog and confirm the schema version and 100-record count remain valid.
- Compare before/after JSON, proving only `quest_moral_chain_mercy_16.discovery` changed; assert all IDs, array order, gates, trigger/category/place, choice payloads, flags, outcomes, and score deltas are unchanged.
- Run the existing data-integrity and moral-choice catalog checks that own this file; report the focused command selected under the then-current `TEST_POLICY.md`.
- In a current route-level check, verify the record remains correctly mapped and that the modal presents trigger, revised discovery, and every unchanged choice label in that order. Do not alter or invent a branch result to demonstrate the prose.
- Review voice, clarity, duplicate imagery, canon facts, and word count as a descriptive measurement only. A retained-text disposition passes if no edit improves the scene.

## Disposition

Recommended scope: one selector, one prose field, one bounded review. Accept a revision only when it makes the arrival more particular—the surrender more visible, the transaction more exact—without resolving the doubt the choices exist to negotiate. The live discovery string can remain untouched. This proposal has an evidenced existing route and can inform a future data-only package; it is not a path claim, approval to edit, or a substitute for current foreman selection and ownership checks.
