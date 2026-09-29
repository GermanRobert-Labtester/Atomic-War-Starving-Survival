# Subject Plan 180.01 — The Weight of Reputation: branching-quest discovery prose

Lane: A · Subject family: moral-choice branching content · Status: PROPOSAL (single-record, data-first prose subject; implementation route evidence included, not integration approval)

## Bounded outcome

Review one field—`discovery`—for selector `quest_moral_chain_mercy_15` in `Assets/StreamingAssets/Data/moral_choice_quests_branching.json`. Current depth: 54 words / 314 characters. The aim is to sharpen the authored encounter that players see when this late-chain moral-choice quest is offered. The plan does not ask for a longer passage; retain the current text if a revision would add no useful information.

**Editorial question:** Let the shelter's reputation arrive as reported speech and acted-on hearsay—what strangers repeated, what they traveled toward—never as narration that flatters the player; keep the alliance's material terms (the trade route you need) standing beside its appeal.

This is the Mercy branch record **The Weight of Reputation**, the chain's day-155 turn from giving to being known for giving. The branch identity and actual situation are source facts; do not use this pass to rewrite the larger quest arc.

## Verified premise and source evidence

- **VERIFIED (2026-09-29):** `quest_moral_chain_mercy_15` is present exactly once in the current `schema_version: 1` `quests` array, which contains 100 entries (25 per branch: mercy, iron, listen, betray).
- **VERIFIED:** the loader record shape is `id`, `display_name`, `category`, `trigger`, `discovery`, `location_id`, `min_day`, `max_day`, `choices`; `MoralChoiceBranchQuestCatalogLoader.MapRecord` maps these to `MoralChoiceQuestDefinition` without changing the discovery text.
- **VERIFIED:** `Main.SetupMoralChoice` (`src/Main.MoralChoice.cs:33`) loads this branching catalog, appends its definitions to the moral-choice catalog, and initializes chain architecture from `moral_choice_chains.json`, whose gates reference this selector. `MoralChoiceModal.RefreshContent` renders the trigger, then the discovery, then the choice labels. This provides the existing data-only authoring route for the prose field.
- **VERIFIED:** the exact ID appears in no prior plan file or index (searched across `docs/expansions/` and `docs/plans/` on 2026-09-29); it enters the program with this wave as the next unused selector in catalog order after `prose_wave179`'s fifty.
- **MEASURED:** complete-catalog discovery text ranges from 42 to 143 whitespace-delimited words (re-measured 2026-09-29); no plan-specific length target follows from that measurement.

**Trigger:** A faction you've never met offers an alliance — based entirely on the stories about you.

**Place:** `loc_shelter_meeting` · **category:** `trust` · **day window:** 155–0 (`MaxDay <= 0` means unbounded in the current Core contract; `IsAvailableOnDay` requires only `day >= MinDay`).

**Existing discovery prose (54 words / 314 characters):**

> A delegation from the Crossroads Collective arrives. You've never met them. They've heard of you — the open gate, the water sharing, the patrol pact, the plague quarantine. 'We want to be part of what you're building,' their leader says. 'Not above it. Part of it.' They control a trade route you desperately need.

**Existing choices, frozen for context:** 1. Accept the alliance on equal terms; 2. Accept with verification — trust but verify; 3. Decline — alliances based on reputation are fragile; 4. Exploit the opportunity — demand favorable trade terms

Adjacent chain records: predecessor `quest_moral_chain_mercy_14`; successor `quest_moral_chain_mercy_16`. Read them in the live catalog before a revision; do not borrow a neighboring record's reveal, character knowledge, or outcome.

**Entity continuity (verified 2026-09-29):** the name **Crossroads Collective** occurs in exactly one record in the whole authored data directory—this one. A revision inherits the obligation that single-record naming carries: the prose may not give the Collective a capital, a territory, a doctrine, or a second implied appearance that a later subject would then have to honor or contradict.

## Editorial treatment

The record's engine is the gap between what the delegation knows and what the player remembers doing. The four deeds they list—the open gate, the water sharing, the patrol pact, the plague quarantine—are the branch's own earlier stations; the discovery is the chain reading the player's mercy back to them. A revision may sharpen that echo, letting one deed return with a detail a player will recognize from the earlier record, but may not add deeds: an invented fifth act of kindness is a canon event created by prose, and this plan authorizes none. Four is also simply the right number for reported speech; a delegation that recites five good works sounds like a dossier, and a dossier sounds like a trap the prose has not earned.

Keep the leader's quoted position exactly as practical as it is. 'Not above it. Part of it.' is a negotiated term arriving in speech, not a creed arriving in rhetoric; a revision should resist the pull to make the delegation eloquent, because eloquence would spend trust the scene is supposed to be about. What the leader wants is legibility—the sentence says what joining means and what it excludes. The leader is otherwise undescribed, and the prose gains nothing by giving them a face; a delegation is a decision arriving in the plural.

The closing sentence carries the material stake: they control a trade route you desperately need. A revision may ground that need in the corridor geography the neighboring records already use, but must keep the need concrete and the desperation proportionate—soften it and the choice collapses into pure sentiment; inflate it and the exploit option becomes the obviously correct read. The choice space spans acceptance, verification, refusal, and leverage; the discovery must leave all four livable, which means the narration may report the delegation's words and conduct without certifying their sincerity and without labeling them either.

Preserve point of view and information access. The narration knows what the player sees and what the delegation says; it does not know whether the stories are accurate, whether the leader means the words, or what verification would find. Maintain names, counts, timing, and causal relationships exactly as the live record states them. If a line needs a new fact to work, omit the line or route that separate idea to an independently evidenced subject.

Do not rewrite choices, outcomes, epitaphs, morality deltas, empathy deltas, flags, availability, or the consequence grammar. The prose may frame the decision but cannot imply a choice has already been made or that its outcome occurred. Avoid explaining the moral score: the Core contract keeps the score invisible, and the world and recorded consequences carry the meaning.

## Frozen data contract

Only this record's top-level `discovery` string is in scope. Every other value is immutable. This exact JSON snapshot is the complete record with `discovery` removed; compare it against the live object immediately before any future edit:

```json
{
  "category": "trust",
  "choices": [
    {
      "empathy_delta": 2,
      "epitaph": "Allied with the Crossroads Collective. Their trade route opened. What I built might be a community now.",
      "label": "Accept the alliance on equal terms",
      "moral_delta": 10,
      "outcome_text": "The Crossroads Collective joins the pact. Their trade route opens. Your corridor stretches from the ridge to the southern water. For the first time, what you built feels like it might be called a community.",
      "set_flag": "flag_chosen_faction_side"
    },
    {
      "empathy_delta": 1,
      "epitaph": "Trial alliance for thirty days. By day fifteen, it was already real.",
      "label": "Accept with verification — trust but verify",
      "moral_delta": 7,
      "outcome_text": "You accept pending a thirty-day trial. They agree without offense. By day fifteen, their caravans run your route. By day thirty, the trial is a formality."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Declined the Collective's offer. Reputation isn't the same as knowing someone. The route stays closed.",
      "label": "Decline — alliances based on reputation are fragile",
      "moral_delta": 0,
      "outcome_text": "You decline politely. They leave without rancor. The trade route stays closed. Your corridor stays what it is. Sometimes refusing growth is a kind of honesty."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Leveraged my reputation into a better deal. Got the route. Lost the equity.",
      "label": "Exploit the opportunity — demand favorable trade terms",
      "moral_delta": -6,
      "outcome_text": "You leverage your reputation into a lopsided deal. They agree — barely. The alliance starts with a taste of resentment that never quite fades. You got the route. You lost the equal footing."
    }
  ],
  "display_name": "The Weight of Reputation",
  "id": "quest_moral_chain_mercy_15",
  "location_id": "loc_shelter_meeting",
  "max_day": 0,
  "min_day": 155,
  "trigger": "A faction you've never met offers an alliance — based entirely on the stories about you."
}
```

Do not add or remove records or properties. Do not alter `id`, `display_name`, `category`, `trigger`, `location_id`, `min_day`, `max_day`, choice count/order/labels, outcome text, epitaph, flag, moral or empathy delta, or any nested field. The `set_flag` on the first choice is existing behavior; this proposal does not touch it. Do not introduce another state variant or a parallel narrative authority. The active integration ledger and current ownership ledger remain controlling for any later implementation package.

## Route, ownership, and save boundary

The data-first route is a one-field edit to the existing JSON entry, after rechecking its current schema and path ownership. The proven route is: `MoralChoiceBranchQuestCatalogLoader.Load` → `Main.SetupMoralChoice` definition registration and chain-gate initialization from `moral_choice_chains.json` → the availability checks in `GetAvailableMoralChoices` (unresolved, `IsAvailableOnDay`, `IsChainQuestAccessible`) → `MoralChoiceModal` display of trigger and discovery text. Exact display is still conditional on the existing day, chain, and resolved-state gates; this proposal does not change eligibility or guarantee that the record is offered in every campaign.

A top-level prose-only change has no save, RNG, or state impact. If it appears to require new host behavior, code, UI, flags, save data, mechanics, or a changed choice contract, stop and create a separately scoped, current-evidence integration plan. The subject plan authorizes no such work.

## Canon, continuity, and duplicate checks

1. Compare the record and both adjacent chain entries named above; verify the trigger-to-discovery handoff, reveal order, time, and unresolved questions. Confirm the listed deeds correspond to stations the mercy branch actually authors.
2. Review the relevant current lore and moral-choice canon. Use Volumes 56–57 of `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md` as discovery context, while treating shipped JSON and current code as implementation truth.
3. Search the existing moral-choice, event, echo, and encounter catalogs for duplicate scene beats and recycled phrasing—strangers-arriving-knowing-your-name is a common beat; this record must remain distinct from ambush, spy, and cult-recruitment variants. ID novelty alone is not content novelty.
4. Confirm each person, faction, place, resource quantity, and fact in the prose against existing authority; the Crossroads Collective has no other authored appearance, so the prose may not grow the entity beyond this scene. Do not add named entities or canon events by inference.
5. Keep the scene fictional, restrained, and playable. Avoid real-world references, borrowed phrasing, melodrama, moral instruction, and decorative suffering.

## Focused verification for a future edit

- Strict-parse the complete source catalog and confirm the schema version and 100-record count remain valid.
- Compare before/after JSON, proving only `quest_moral_chain_mercy_15.discovery` changed; assert all IDs, array order, gates, trigger/category/place, choice payloads, flags, outcomes, and score deltas are unchanged.
- Run the existing data-integrity and moral-choice catalog checks that own this file; report the focused command selected under the then-current `TEST_POLICY.md`.
- In a current route-level check, verify the record remains correctly mapped and that the modal presents trigger, revised discovery, and every unchanged choice label in that order. Do not alter or invent a branch result to demonstrate the prose.
- Review voice, clarity, duplicate imagery, canon facts, and word count as a descriptive measurement only. A retained-text disposition passes if no edit improves the scene.

## Disposition

Recommended scope: one selector, one prose field, one bounded review. Accept a revision only when it makes the delegation's arrival more particular—their knowledge more traceable, the route's need more material—without changing the decision space. The live discovery string can remain untouched. This proposal has an evidenced existing route and can inform a future data-only package; it is not a path claim, approval to edit, or a substitute for current foreman selection and ownership checks.
