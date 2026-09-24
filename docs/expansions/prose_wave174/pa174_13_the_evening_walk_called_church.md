# Subject Plan 174.13 — The Evening Walk Called Church
Lane: A · Cluster: C10 · Status: PROPOSAL (subject-level, serial queue; not an integration plan)

## Subject
Review and, only if the census supports it, polish prose in `quest_surveillance_state_06` in `quests_massive_expansion_200.json`. The bounded subject is **privacy practiced beyond the wire**. Only `briefing`, `stages[].text`, and `choices[].text` are in scope; a field already doing its job stays unchanged. No new quest, branch, system, flag, or schema field is proposed.

## Premise evidence
- **VERIFIED:** `Assets/StreamingAssets/Data/quests_massive_expansion_200.json` exists with 200 records under `schema_version` / `quests`. This selector has `type=narrative_chain`, `prereq_quest_id=quest_surveillance_state_05`, `min_day=60`, 1 stage(s), and 2 choices.
- **VERIFIED:** Actual text fields are `briefing` (245 chars), `stages[].text` (123), and `choices[].text` (45); 413 prose characters combined. Choice IDs and `set_flag` values are separate live fields. This record has no `quest_hook` or `objective_text`.
- **VERIFIED source excerpt:** Briefing: “The new microphones are smaller. The housing register has been amended: the word private is struck from it entirely. Real talk has moved outside. Every evening now, pairs take the air past the wire line, out in the ash at the edge of the lights.” Stage: “The watch log has taken to calling the evening walks church. The pairs come back with gray boots and nothing on the record.” Choices: “Bug the bunks.” / “Burn the register's definition.”.
- **VERIFIED constraint:** `docs/data/CATALOG_REGISTRY.md` currently labels this catalog `UNRESOLVED`; no production consumer or player-facing route is established by this evidence. The v2.0 A-25 entry requires a census before authoring and corrects the premise that record count alone proves prose debt.
- **HIGH CONFIDENCE:** This short text is a review candidate, not a proven defect. Character counts prioritize inspection only.
- **UNKNOWN:** Exact current consumer, unclaimed-corpus status, ownership, adjacent-chain completeness, and whether existing text is already sufficient.

## Why this and not something else
The compiled v2.0 authority names this corpus as A-25 / C10 and directs census-first tranche work. This selector makes the audit tractable: one record, one editorial register, fixed choices. The concrete editorial question is **privacy practiced beyond the wire**. Deliverable is a field-by-field decision—polish, retain, or close as sufficient—with a brief continuity rationale. It is not an invitation to expand all 200 records or to make the prose longer for its own sake.

Editorial direction: Extend the pairs’ evening walk: show how they make room for private conversation while the watch log misnames it. Treat deletion of “private” as the state’s act, not truth.

The reviewer should read the record together with its prerequisite and immediate successor (or adjacent opening records where the prerequisite is empty), then check whether each proposed line contributes a fact, character choice, or emotional turn not already supplied nearby. Keep briefing context, stage action, and choice language doing different jobs. The two options must remain understandable from the existing record; prose must not imply a mechanical consequence that the flags do not encode.

## What must not change
No microphone item, meter, stealth check, or new excursion route. Preserve both choices and chain order.

Preserve `id=quest_surveillance_state_06`, `display_name='Surveillance State - Stage 6'`, `type=narrative_chain`, prerequisite `'quest_surveillance_state_05'`, Day 60, stage IDs/order, choice IDs/order, and both `set_flag` values exactly. No field widening, parallel quest state, save changes, route claims, or invented canon. New details remain proposals until checked against neighboring stages and the current world authority.

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
