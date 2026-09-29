# Subject Plan 180.05 — The Judge's Seat: branching-quest discovery prose

Lane: A · Subject family: moral-choice branching content · Status: PROPOSAL (single-record, data-first prose subject; implementation route evidence included, not integration approval)

## Bounded outcome

Review one field—`discovery`—for selector `quest_moral_chain_mercy_19` in `Assets/StreamingAssets/Data/moral_choice_quests_branching.json`. Current depth: 50 words / 273 characters. The aim is to sharpen the authored encounter that players see when this late-chain moral-choice quest is offered. The plan does not ask for a longer passage; retain the current text if a revision would add no useful information.

**Editorial question:** Report the killing in the fewest physical facts that make it undeniable—weapon dropped, man on his knees—and keep the community rendered as divided positions rather than a crowd, so the sentence the player hands down is the first verdict the scene contains.

This is the Mercy branch record **The Judge's Seat**, the chain's day-200 turn from dispensing mercy to defining its limits in law. The branch identity and actual situation are source facts; do not use this pass to rewrite the larger quest arc.

## Verified premise and source evidence

- **VERIFIED (2026-09-29):** `quest_moral_chain_mercy_19` is present exactly once in the current `schema_version: 1` `quests` array, which contains 100 entries (25 per branch: mercy, iron, listen, betray).
- **VERIFIED:** the loader record shape is `id`, `display_name`, `category`, `trigger`, `discovery`, `location_id`, `min_day`, `max_day`, `choices`; `MoralChoiceBranchQuestCatalogLoader.MapRecord` maps these to `MoralChoiceQuestDefinition` without changing the discovery text.
- **VERIFIED:** `Main.SetupMoralChoice` (`src/Main.MoralChoice.cs:33`) loads this branching catalog, appends its definitions to the moral-choice catalog, and initializes chain architecture from `moral_choice_chains.json`, whose gates reference this selector. `MoralChoiceModal.RefreshContent` renders the trigger, then the discovery, then the choice labels. This provides the existing data-only authoring route for the prose field.
- **VERIFIED:** the exact ID appears in no prior plan file or index (searched across `docs/expansions/` and `docs/plans/` on 2026-09-29); it enters the program with this wave.
- **MEASURED:** complete-catalog discovery text ranges from 42 to 143 whitespace-delimited words (re-measured 2026-09-29); no plan-specific length target follows from that measurement.

**Trigger:** Your community asks you to judge a murder case — a man killed a raider who surrendered.

**Place:** `loc_shelter_meeting` · **category:** `trust` · **day window:** 200–0 (`MaxDay <= 0` means unbounded in the current Core contract).

**Existing discovery prose (50 words / 273 characters):**

> Tomas killed a raider who had dropped his weapon. The raider was on his knees. Tomas put a bullet in him anyway. 'He killed my brother,' Tomas says. 'Last winter. On the east road.' The community wants justice. Some want blood. Some want mercy. They all want you to decide.

**Existing choices, frozen for context:** 1. Exile, not execution — we don't kill surrendered people; 2. Community service — he works off the debt to community trust; 3. Acquit — the raider killed his brother, it was justice; 4. Execute Tomas — murder is murder regardless of motive

Adjacent chain records: predecessor `quest_moral_chain_mercy_18`; successor `quest_moral_chain_mercy_20`. Read them in the live catalog before a revision; do not borrow a neighboring record's reveal, character knowledge, or outcome.

**Entity continuity (verified 2026-09-29):** **Tomas** appears in exactly two records of this catalog—this one and the later, still unplanned `quest_moral_chain_mercy_21`. Whatever this discovery establishes about him must remain consistent with that downstream return; a revision may not age him, wound him, or attach a fate the later record does not support.

## Editorial treatment

The killing is past tense and stays past tense; the discovery is not the crime, it is the account of the crime a community has to live beside. The three facts are already minimal and correctly ordered—weapon dropped, knees on the ground, the shot anyway—and their order is the moral architecture: surrender established before the act that violated it. A revision may adjust the telling of these facts but may not add gore, prolong the moment, or stage the scene cinematically; the record's restraint is what keeps the killing a fact instead of a spectacle, and the fourth choice's execution must not be prefigured by prose that has already made death casual.

Tomas's motive arrives only in his own quoted words, with his own dating: 'Last winter. On the east road.' This is exactly the right provenance—the player learns the revenge through Tomas's claim, not through narration, and the claim is unfalsifiable within the scene. A revision must not confirm the brother, name him, or lend the east road any detail that reads as corroboration; the record keeps what Tomas says and what is known in separate ledgers, and the choice space depends on that separation, since acquittal rests on accepting his account while exile rests on declining to.

The community is rendered as positions, not personalities: 'Some want blood. Some want mercy.' A revision may give the division one physical expression—the meeting room's silence, who stands where—but must not individualize the factions into named spokespeople, because the decision the player makes must answer a community, not a cast. The closing sentence is the record's one demand and its cleanest: they all want you to decide. It transfers the case without a recommendation, and it should remain the last thing the prose does.

Note the record files itself under `trust`, not `justice`: what is being judged is whether the community's lines mean anything, which is why every outcome text is about the line and the watching, not about the raider. A revision tuned to this will keep the prose asking what a rule costs, not what Tomas deserves.

Preserve point of view and information access. The narration knows what witnesses could report; it does not know what Tomas felt pulling the trigger, whether the brother existed, or which side is larger. Maintain names, counts, timing, and causal relationships exactly as the live record states them. If a line needs a new fact to work, omit the line or route that separate idea to an independently evidenced subject.

Do not rewrite choices, outcomes, epitaphs, morality deltas, empathy deltas, flags, availability, or the consequence grammar. The prose may frame the decision but cannot imply a choice has already been made or that its outcome occurred. Where consequences involve punishment, keep the prose humane and specific; do not make violence decorative or pre-commit the branch. Avoid explaining the moral score: the Core contract keeps the score invisible, and the world and recorded consequences carry the meaning.

## Frozen data contract

Only this record's top-level `discovery` string is in scope. Every other value is immutable. This exact JSON snapshot is the complete record with `discovery` removed; compare it against the live object immediately before any future edit:

```json
{
  "category": "trust",
  "choices": [
    {
      "empathy_delta": 2,
      "epitaph": "Exiled Tomas for killing a surrendered man. The community drew a line. We don't kill the helpless.",
      "label": "Exile, not execution — we don't kill surrendered people",
      "moral_delta": 12,
      "outcome_text": "You exile Tomas. He accepts it — barely. The community exhales. A line has been drawn: we don't kill the helpless, even when we understand why someone would. Tomas walks east. He doesn't look back."
    },
    {
      "empathy_delta": 1,
      "epitaph": "Sentenced Tomas to six months of hard community labor. Most forgave by month four. The line held.",
      "label": "Community service — he works off the debt to community trust",
      "moral_delta": 8,
      "outcome_text": "Six months of heavy labor: wall repair, latrine duty, the jobs nobody wants. Tomas accepts. The community watches. By month four, most have forgiven. The raider's friends haven't. But the line holds.",
      "set_flag": "flag_honored_debt"
    },
    {
      "empathy_delta": 0,
      "epitaph": "Acquitted Tomas. Called it justice. Half the community nodded. The other half noticed the line blur.",
      "label": "Acquit — the raider killed his brother, it was justice",
      "moral_delta": -4,
      "outcome_text": "You acquit him. Half the community nods. The other half goes quiet. The line — whatever it was — blurs. The next time a surrendered enemy appears, the hesitation is gone from people's hands."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Executed Tomas at dawn. Nobody kills a surrendered person here anymore. Nobody trusts the judge's seat either.",
      "label": "Execute Tomas — murder is murder regardless of motive",
      "moral_delta": -8,
      "outcome_text": "You order the execution. It happens at dawn. Half the community watches. Half doesn't. The line is drawn in the hardest ink possible. Nobody kills a surrendered person again. Nobody quite trusts the judge's seat again either."
    }
  ],
  "display_name": "The Judge's Seat",
  "id": "quest_moral_chain_mercy_19",
  "location_id": "loc_shelter_meeting",
  "max_day": 0,
  "min_day": 200,
  "trigger": "Your community asks you to judge a murder case — a man killed a raider who surrendered."
}
```

Do not add or remove records or properties. Do not alter `id`, `display_name`, `category`, `trigger`, `location_id`, `min_day`, `max_day`, choice count/order/labels, outcome text, epitaph, flag, moral or empathy delta, or any nested field. The `set_flag` on the second choice is existing behavior; this proposal does not touch it. Do not introduce another state variant or a parallel narrative authority. The active integration ledger and current ownership ledger remain controlling for any later implementation package.

## Route, ownership, and save boundary

The data-first route is a one-field edit to the existing JSON entry, after rechecking its current schema and path ownership. The proven route is: `MoralChoiceBranchQuestCatalogLoader.Load` → `Main.SetupMoralChoice` definition registration and chain-gate initialization from `moral_choice_chains.json` → the availability checks in `GetAvailableMoralChoices` (unresolved, `IsAvailableOnDay`, `IsChainQuestAccessible`) → `MoralChoiceModal` display of trigger and discovery text. Exact display is still conditional on the existing day, chain, and resolved-state gates; this proposal does not change eligibility or guarantee that the record is offered in every campaign.

A top-level prose-only change has no save, RNG, or state impact. If it appears to require new host behavior, code, UI, flags, save data, mechanics, or a changed choice contract, stop and create a separately scoped, current-evidence integration plan. The subject plan authorizes no such work.

## Canon, continuity, and duplicate checks

1. Compare the record and both adjacent chain entries named above; verify the trigger-to-discovery handoff, reveal order, time, and unresolved questions. Then read the downstream `quest_moral_chain_mercy_21` before finalizing anything that touches Tomas's standing or his history on the east road.
2. Review the relevant current lore and moral-choice canon. Use Volumes 56–57 of `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md` as discovery context, while treating shipped JSON and current code as implementation truth. The Verdict institution owns separate tribunal fiction; confirm this community-level judgement does not collide with it.
3. Search the existing moral-choice, event, echo, and encounter catalogs for duplicate scene beats and recycled phrasing—summary justice after a surrender is a recurring beat; check existing judgement, execution, and blood-debt records (including the chain's own earlier `quest_moral_chain_mercy_02`) before reusing any image of a kneeling enemy. ID novelty alone is not content novelty.
4. Confirm each person, faction, place, resource quantity, and fact in the prose against existing authority; the dead brother exists only as Tomas's claim in this record and must remain uncorroborated. Do not add named entities or canon events by inference.
5. Keep the scene fictional, restrained, and playable. Avoid real-world references, borrowed phrasing, melodrama, moral instruction, and decorative suffering.

## Focused verification for a future edit

- Strict-parse the complete source catalog and confirm the schema version and 100-record count remain valid.
- Compare before/after JSON, proving only `quest_moral_chain_mercy_19.discovery` changed; assert all IDs, array order, gates, trigger/category/place, choice payloads, flags, outcomes, and score deltas are unchanged.
- Run the existing data-integrity and moral-choice catalog checks that own this file; report the focused command selected under the then-current `TEST_POLICY.md`.
- In a current route-level check, verify the record remains correctly mapped and that the modal presents trigger, revised discovery, and every unchanged choice label in that order. Do not alter or invent a branch result to demonstrate the prose.
- Review voice, clarity, duplicate imagery, canon facts, and word count as a descriptive measurement only. A retained-text disposition passes if no edit improves the scene.

## Disposition

Recommended scope: one selector, one prose field, one bounded review. Accept a revision only when it makes the facts of the killing and the division of the community more exact, without corroborating Tomas's claim or anticipating any verdict. The live discovery string can remain untouched. This proposal has an evidenced existing route and can inform a future data-only package; it is not a path claim, approval to edit, or a substitute for current foreman selection and ownership checks.
