# Subject Plan 174.08 — Eleven Words across the Evening Count
Lane: A · Cluster: C10 · Status: PROPOSAL (subject-level, serial queue; not an integration plan)

## Subject
Review and, only if the census supports it, polish prose in `quest_transparent_truth_02` in `quests_massive_expansion_200.json`. The bounded subject is **truth as a public water notice**. Only `briefing`, `stages[].text`, and `choices[].text` are in scope; a field already doing its job stays unchanged. No new quest, branch, system, flag, or schema field is proposed.

## Premise evidence
- **VERIFIED:** `Assets/StreamingAssets/Data/quests_massive_expansion_200.json` exists with 200 records under `schema_version` / `quests`. This selector has `type=narrative_chain`, `prereq_quest_id=quest_transparent_truth_01`, `min_day=20`, 1 stage(s), and 2 choices.
- **VERIFIED:** Actual text fields are `briefing` (261 chars), `stages[].text` (78), and `choices[].text` (56); 395 prose characters combined. Choice IDs and `set_flag` values are separate live fields. This record has no `quest_hook` or `objective_text`.
- **VERIFIED source excerpt:** Briefing: “This time the truth is water. The aquifer report has been in the office drawer for a month; the proposal takes it out and puts two words on the air in the same sentence: aquifer, years. The sentence is eleven words long. It took the clerk an afternoon to write.” Stage: “At the evening count, the sentence goes out once. Cups stop halfway to mouths.” Choices: “Say it. Broadcast it.” / “Refuse. The word years is too much.”.
- **VERIFIED constraint:** `docs/data/CATALOG_REGISTRY.md` currently labels this catalog `UNRESOLVED`; no production consumer or player-facing route is established by this evidence. The v2.0 A-25 entry requires a census before authoring and corrects the premise that record count alone proves prose debt.
- **HIGH CONFIDENCE:** This short text is a review candidate, not a proven defect. Character counts prioritize inspection only.
- **UNKNOWN:** Exact current consumer, unclaimed-corpus status, ownership, adjacent-chain completeness, and whether existing text is already sufficient.

## Why this and not something else
The compiled v2.0 authority names this corpus as A-25 / C10 and directs census-first tranche work. This selector makes the audit tractable: one record, one editorial register, fixed choices. The concrete editorial question is **truth as a public water notice**. Deliverable is a field-by-field decision—polish, retain, or close as sufficient—with a brief continuity rationale. It is not an invitation to expand all 200 records or to make the prose longer for its own sake.

Editorial direction: Treat the broadcast sentence as public language: who chose it, who heard it, and what the pause at the cups means. Allow disagreement about preparedness without rewriting the forecast.

The reviewer should read the record together with its prerequisite and immediate successor (or adjacent opening records where the prerequisite is empty), then check whether each proposed line contributes a fact, character choice, or emotional turn not already supplied nearby. Keep briefing context, stage action, and choice language doing different jobs. The two options must remain understandable from the existing record; prose must not imply a mechanical consequence that the flags do not encode.

## What must not change
Do not change the “years” fact, invent a depletion date, or add reputation/broadcast mechanics. Abort is not permanent censorship.

Preserve `id=quest_transparent_truth_02`, `display_name='The Transparent Truth - Stage 2'`, `type=narrative_chain`, prerequisite `'quest_transparent_truth_01'`, Day 20, stage IDs/order, choice IDs/order, and both `set_flag` values exactly. No field widening, parallel quest state, save changes, route claims, or invented canon. New details remain proposals until checked against neighboring stages and the current world authority.

## Recommended integration route
**Tier: DOCS-ONLY until consumer and ownership are verified; conditional DATA-ONLY only if an existing reachable consumer is proven.**

**Seams:** fresh catalog classification → current schema/integrity authority → named loader/dispatch owner (must be found in source) → existing player-facing display (must be demonstrated). If the chain is absent, do not edit the orphan record to imply gameplay integration; document the utilization disposition or defer to the owning authority.

**Save impact:** NONE for prose-only edits on a consumed record. **Determinism:** NONE. **Verification class:** exact-ID consumer proof; integrity validation; neighboring-chain continuity; diff proving all non-prose data is unchanged. Then run only the smallest existing content gate. **Ownership:** re-read `INTEGRATION_PLANS.md` and `WORKTREE_OWNERSHIP.md` before selection and claim the exact catalog path only if assigned. This route is a recommendation, not authorization to integrate.

## Continuity checklist result
- **Canon:** retain established facts and chronology; proposed details are checked against current lore and neighboring records.
- **Duplication:** selector is unique in the live 200-record catalog. Before authoring, compare adjacent beats and related quest catalogs; use the current duplicate instrument if text overlap is suspected.
- **Mechanics:** IDs, prerequisites, gates, choice ordering and flags remain unchanged; choice text does not promise unencoded outcomes.
- **Unclaimed content:** refresh `docs/plans/UNCLAIMED_CORPUS_CENSUS.md`. If this corpus is listed as unused, utilization/consumer resolution takes priority over fresh prose.
- **Epilogue:** no permutation is touched. **Sealed/blocked:** no closed distress or decision-gated surface is reopened.
- **Tone/accessibility:** restrained fictional voice, plain choice contrast, no meaning dependent on visual-only cues.

## Verification class
Before any edit: prove a current consumer and display route for this exact catalog; refresh ownership, current batch, and census; validate field contract; compare prerequisite/successor. After an approved prose-only edit: JSON parse and current integrity gate; exact-ID utilization check if supported; mechanical diff of every non-prose value; human review for clarity, factual fidelity, voice, and branch meaning. If no route exists, the correct result is no mutation and a documented no-op/utilization finding. Do not substitute JSON presence for reachability or run broad tests for a documentation/content-only proposal.

## Open premises
1. Does a production consumer reach this exact catalog and selector? Registry says `UNRESOLVED`; verify by tracing live code end to end.
2. What is the current exact-record status in the unclaimed-content census?
3. What focused integrity/utilization command validates the actual schema and consumer, rather than a similarly named catalog?
4. Do adjacent records already complete this beat? If yes, retain the source text and close the proposal as sufficient.
