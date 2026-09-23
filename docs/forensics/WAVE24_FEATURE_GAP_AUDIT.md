# Wave 24 Feature Gap Audit

**Snapshot:** 2026-09-21; read-only review of current Core, host, UI, data, save registry, and integration/ownership references. **Scope:** five bounded feature opportunities used by Expansions 122–126. **Changes:** no game code or authored gameplay data changed.

## Findings

| Expansion | Classification | Current evidence | Honest gap or opportunity |
|---|---|---|---|
| 122 — The Trust They Can Withdraw | Code-backed route/persistence gap | ConfessionSecretSystem has catalog discovery, five resolution methods, delegated consequences, state capture and restore; confession_secrets.json exists. Bounded search of src and SaveSectionRegistry found no host call or confession-secret save row. The content-utilization scanner lists a static ConfessionPanel consumer label, but inspected source did not show a product panel or route. | Domain capability exists; inspected host reachability and persistence do not. Recheck indirect routing and name an existing save owner before implementation. No new save architecture is approved. |
| 123 — The Skill That Fell Quiet | Code-backed gameplay-effect gap | SkillAtrophySystem applies medical/crafting IDs after morale below 20 for 14 days and declares multiplier 0.5. SurvivorSocialCoordinator persists it in survivor_social; SurvivorRelationsPanel can show the read model. Bounded search found no medical/crafting action consumer of the multiplier outside the owner. | Atrophy is tracked and visible, but its declared factor was not found changing a canonical action. Map skill-bearing checks first; preserve permanence/value and do not invent an effect where no skill input exists. |
| 124 — A Name for What Came Back | Partial integration/authority convergence | TrophySystem reads trophies.json and contains exactly-once award/unlock state plus capture/restore. WildlifeTrappingSystem separately maps preserved species to recipe IDs and emits a trophy-ready event. Crafting, ShelterDecorSystem, its host session, and panel already support trophy crafting/display. Bounded search found no RecordQuarryPreserved call or dedicated trophy save row. | Trophy gameplay is live. The gap is a split between the dormant award ledger and live species-to-recipe path. Consolidate only after naming the save owner; do not add recipes or redo decor. |
| 125 — Five Days of Warning | Live-system extension | CrisisPredictor is deterministic/read-only. Main.BriefingCrisis assembles current owner inputs, and DailyBriefingReportBuilder.AppendCrisisWarnings is called from Main.Campaign. Neutral defaults intentionally avoid fabricating missing inputs. | Prediction and daily warnings already work. A preparation board can add provenance and links/previews to existing commands; no second predictor or automatic action. |
| 126 — The Line to Turn Back On | Live-system extension | ExpeditionSystem persists active sorties, supports explicit retreat and push-luck, and auto-returns non-pushing sorties at three looting ticks through MaybeAutoRetreat. ExpeditionHostSession and ExpeditionPanel provide the current route. | Basic retreat and auto-return are live. Optional per-sortie limits may extend that behavior while preserving current default, phase semantics, push-luck, and serialized state. This is not a missing-retreat fix. |

## Exclusion checks

- Expansions 117–121 are present under docs/expansions/wave23; this batch continues with 122–126 and does not rewrite those documents.
- The set does not claim prediction, daily crisis warnings, trophy recipes, trophy display, manual expedition retreat, or three-tick auto-return are absent.
- Confession prose in letters, radio, quests, HiddenAgendaSystem, and rumor systems is not proof of a route for ConfessionSecretSystem.
- Skill atrophy is not expanded into new progression, recovery, or morale systems.
- Trophy mount ownership is not duplicated with another species map or morale reward.

## Limits and follow-up

This is a bounded static source audit, not a runtime reachability audit or test run. Indirect signal wiring may exist outside literal searches; recheck exact route and save path before implementation. Expansions 122–124 describe evidence-backed wiring gaps; 125–126 are explicitly labeled extensions. Production work must follow current INTEGRATION_PLANS.md, WORKTREE_OWNERSHIP.md, TEST_POLICY.md, and the named integrator/foreman contract. These plans are not implementation, path claims, or permission to change a shared owner.

## Source paths inspected

- Assets/Ashfall.Core/Phantoms/ConfessionSecretSystem.cs
- Assets/Ashfall.Core/Phantoms/ConfessionSecretCatalog.cs
- Assets/StreamingAssets/Data/confession_secrets.json
- Assets/Ashfall.Core/Survivors/SkillAtrophySystem.cs
- Assets/Ashfall.Core/Survivors/SurvivorSocialCoordinator.cs
- Assets/Ashfall.Core/Shelter/TrophySystem.cs
- Assets/Ashfall.Core/WildlifeTrappingSystem.cs
- Assets/Ashfall.Core/Campaign/CrisisPredictionModel.cs
- Assets/Ashfall.Core/Campaign/DailyBriefingReportBuilder.cs
- src/Main.BriefingCrisis.cs
- Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs
- src/Host/ExpeditionHostSession.cs
- src/UI/ExpeditionPanel.cs
- Assets/Ashfall.Core/Save/SaveSectionRegistry.cs
- INTEGRATION_PLANS.md; WORKTREE_OWNERSHIP.md; KNOWN_DEBT.md
