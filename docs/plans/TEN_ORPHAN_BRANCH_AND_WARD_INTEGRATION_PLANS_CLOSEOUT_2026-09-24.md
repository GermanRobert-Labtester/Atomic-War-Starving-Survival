# Ten branch, transport, social, weather, labour, and Ward integration plans — documentation closeout

**Date:** 2026-09-24  
**Scope:** ten user-requested plans and dossiers, documentation only.  
**Source of expansion direction:** `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`. Its live-source-first, four-tier architecture, information-flow, and anti-duplication rules govern the addenda.  
**Status:** planning architecture and editorial pass complete; no production feature, host route, catalog row, or test has been added by this package.

## Deliverables and document roles

The historical contract, implementation log, closeout, authority map, design bible, or generated appendix at the head of each file is retained as dated evidence. A marked 2026-09-24 addendum gives the current premise, one bounded first package, explicit ownership and save route, phased integration sequence, a C# contract sketch, a reachability register where current JSON exists, candidate scene/edge-case reviews, and a source-specific editorial review. The matrix is **candidate planning material**, not newly approved game canon or a commitment to implement every case.

| Subject | Expanded file | Integration interpretation |
|---|---|---|
| Independent Branch | [baseline parity](../content/plan121/INDEPENDENT_BRANCH_8_BASELINE_PARITY.md) | Existing coordinator host and `weight_of_choices`; compare historical eight with current fifteen catalog rows. |
| Military Branch | [runtime contract](../factions/MILITARY_BRANCH_RUNTIME_CONTRACT.md) | Existing coordinator host; preserve branch exclusivity and separate standing owner. |
| Rebel Branch | [implementation log](PLAN_123_REBEL_BRANCH_IMPLEMENTATION_LOG.md) | Existing coordinator host; recheck old collision findings and information flow. |
| Draisine Recovery | [Plan 125 closeout](../expeditions/PLAN_125_AMPHIBIOUS_DRAISINE_CLOSEOUT.md) | Base rerailing system hosted; named armored subclass is indirect; save key `draisine_recovery`. |
| Trauma Bond | [relationship map](../survivors/PLAN_182_RELATIONSHIP_DRIFT_AUTHORITY_MAP.md) | Social coordinator owns trauma state; relationship `lastInteractionDay` map claim is superseded. |
| Aerial Recon Window | [transport dossier](EXPANSION_PROGRAM_WAVE3_2026-09-21/PLAN-TRANSPORT-EXPEDITION-30_APPENDIX-A_ORPHAN_DOSSIERS.md) | Pure evaluator; mission dispatch and aircraft owners must be identified; no evaluator save section. |
| Chit Purity Assay | [crime dossier](EXPANSION_PROGRAM_WAVE5_2026-09-21/PLAN-CRIME-SYNDICATES-44_APPENDIX-A_ORPHAN_DOSSIERS.md) | Pure evaluator; settle through canonical funds/trade/heat owners after exact funds-leg audit. |
| Cloud Seeding | [weather scaffold](EXPANSION_PROGRAM_WAVE13_2026-09-21/PLAN-WEATHER-SONDE-TRUTH-168_APPENDIX-A_SCAFFOLD.md) | Live coordinator and panel exist; route mutation through host with real inventory and campaign day. |
| Duty Roster Chart | [labour scaffold](EXPANSION_PROGRAM_WAVE6_2026-09-21/PLAN-LABOUR-PROFESSIONS-68_APPENDIX-A_SCAFFOLD.md) | Internal chart engine belongs to hosted `DutyRosterSystem`; no new chart section. |
| Lyophilization | [Ward plan](../expansions/wave6/expansion_38_the_ward_plan.md) | Compatibility subclass of hosted base system; extend `lyophilization` batch authority. |

## Finished planning architecture

```text
Authored JSON, campaign facts, canonical resources
    -> existing engine-free Core system or stateless evaluator
    -> existing host session / identified owner command
    -> existing save section for mutable state
    -> read-only Godot panel, journal, briefing, or ending projection
```

The three faction branches meet at `FactionBranchCoordinator`, `FactionBranchHostSession`, `Main.FactionBranch.cs`, and `weight_of_choices`. Each child branch retains its catalog and predicates, while the coordinator owns exclusivity. External faction standing remains with its own owner. Draisine rerailing, duty chart, and lyophilization already have base hosts and save routes; an exact-name subclass or internal engine with zero direct host references is not proof of absent gameplay. Trauma bond rides `SurvivorSocialCoordinator` and `survivor_social`, while pair affinity and its interaction stamp remain with `SurvivorRelationsSystem`. The aerial and chit engines are pure: callers own mission or transaction state, so adding a new save section would duplicate authority. Cloud seeding has a weather-intelligence capture path; the missing production work is command, canonical cost, current-day, and panel boundary integrity.

## Current-evidence corrections that affect promotion

1. `Main.SaveDraisineRerailing` captures `draisine_recovery`; the method name is not a save-section name. Amphibious crossing and rerailing stay distinct.
2. `RelationshipEntry.lastInteractionDay` now exists and is stamped in `SurvivorRelationsSystem`; the older authority map remains historical. That stamp alone does not implement neglect decay.
3. `WeatherForecastPanel` directly calls `CloudSeedingSystem`. `WeatherIntelligenceCoordinator` currently constructs the system with null inventory; Core skips material validation and removal when inventory is null. The panel uses forecast crisis day as action day. This plan identifies a concrete resource/clock seam for a future authorized build.
4. `DutyRosterChartEngine` is internal to `DutyRosterSystem`, which has an existing host and `duty_roster` save. The scaffold's proposed new setup method must not be promoted.
5. `LyophilizationEngine` and `ArmoredDraisineRecoverySystem` are subclasses of hosted base systems; their exact-name host count cannot be interpreted as an unbuilt base feature.
6. `AerialReconWindowEngine` permits some Hazardous launches; Grounded is the blocking result. Its output belongs in a mission preview/dispatch, not a new day owner.
7. `ChitPurityAssayEngine` returns suggested amounts and effects. Current F13 funds work and `FundsLedger` must be inspected before choosing an atomic settlement leg; the evaluator itself does not post money.

## Regeneration and polishing

`docs/plans/EXPANSION_PROGRAM_2026-09-21/tools/generators/gen_requested_ten_integration_expansions.py` creates the marked addenda from the dated source text and live catalog IDs. `polish_requested_ten_integration_expansions.py` then inserts the ten source-specific editorial reviews and fixes generated grammar. The weather and labour scaffold files declare themselves generated: run their original owning scaffold generator first when changing their base inventory, then rerun these two expansion scripts. The addenda preserve the historical material above the marker and are reproducible; do not hand-edit the generated layer.

The editorial pass corrected stale status claims, clarified the stateless evaluator boundary, separated literal save keys from method names, and gave each first package a concrete player route and acceptance observation. Candidate prose remains outside runtime JSON until its trigger, owner, reader, and fallback are proven. No authored catalog additions or fictional canon were silently promoted.

**Static closeout checks:** all ten expanded files are above 250,000 characters (252,687–255,258; 2,540,282 combined). Each has one expansion marker, one editorial review, and one architecture handoff. A repeated polish run produced identical hashes, the ten closeout links resolved, and scoped `git diff --check` reported no whitespace errors. No runtime tests were run because this package changes documentation and generators only.

## Remaining integration decisions

- Identify the active expedition/aviation dispatch caller for aerial window evaluation, then route one launch preview and one Grounded refusal through that owner.
- Select the canonical trade settlement leg for chit assay, with funds debit/credit, confiscation, fee, trust, heat, and duplicate-submit semantics resolved together.
- Give cloud seeding a host command with real inventory and campaign-day sources. Verify whether installation has a cost in the current Core contract before adding one.
- For the three indirectly hosted named types, establish whether the exact subclass/internal engine has a behavior absent from its base owner. If no behavior is missing, close the direct-reference orphan finding rather than spawning another host.
- Promote only a small content cohort at a time, with live IDs, predicate/reader evidence, one observable route, save behavior, and focused verification under `TEST_POLICY.md`.

This is the closeout for the **integration-plan architecture**. Runtime integration is intentionally a later, claimed implementation package under `INTEGRATION_PLANS.md` and `WORKTREE_OWNERSHIP.md`.
