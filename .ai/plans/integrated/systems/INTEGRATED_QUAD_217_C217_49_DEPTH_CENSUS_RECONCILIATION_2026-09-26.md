# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

> **FULLY INTEGRATED 2026-09-26** — user-authorized ("find 4 plans to fully integrate, don't leave as partials"). Every constituent package was re-verified live this session (host sessions present, checksummed save sections registered where stateful, CLI self-test probes registered, orchestration wired). Marked, renamed `INTEGRATED_*`, and published to the canonical integrated archive `docs/plans/integrated/systems/`; the agent-side `.ai/plans/integrated/systems/` copy is retained. No commit.

# Quad Package — Plans 217 + C2[17] + Plan 49 Depth Pass + Census Reconciliation

**STATUS: APPROVED BY USER**
**Authorized by:** user directive 2026-09-26 ("Start integrating 4 plans concurrently without testing anything excessively").
**Claim:** `claim-quad-package-217-c2-17-plan49-reconciliation-2026-09-26`

## Package A — Plan 217 Survivor Genealogy (full host integration)
Core accessors (`LineageRecords`, `FamilyEventLog`) on `GenerationalLineageExtension`; new `src/Host/GenealogyHostSession.cs` + `GenealogySaveStore` (own checksummed `genealogy` section); `src/Main.Genealogy.cs` subscribes ONLY canonical facts (`OnFamilyUnitEstablishedSeam` → `SetSpouse` union; `OnChildWelcomedToFamilySeam` → per-parent `EstablishLineage`/`OnAdoption` exactly-once; `OnSurvivorFate` → death event); NO second family-unit ledger (bridge unit-forming path deliberately unused); read-only `KinshipProvider` row in `SurvivorDetailPanel`; `--genealogy-selftest` 10/10; tests 13/13.

## Package B — C2[17] Authored Identity (inference retired)
`src/Main.SurvivorSocial.cs`: authored `belief_profile_id` is the sole belief source; the trait-keyword `InferBeliefProfile` shadow deleted (it was already inert — the 53 unauthored definitions have empty traits). Item-tag half already live (`ItemTagCatalog` consumed by CraftingSystem/ItemDefinitions). Tests 2/2.

## Package C — Plan 49 (C1[16]) Depth Passes
All four orphaned catalogs bound through existing Core loaders into constructed owners: `phantom_heirlooms.json`→`HeirloomCatalog.Load`+`HeirloomSystem` (read-only; Plan 41 owns runtime state), `trade_screen_scenarios.json`→`TradeScreenScenarioLoader`, `audio_logs_expansion_05.json`→additive `AudioConditionSystem.LoadAudioLogCatalog`, `memorials_expansion_05.json`→additive `MemorialSystem.LoadMemorialTexts`. New `src/Host/Plan49DepthPassHostSession.cs` + `src/Main.Plan49DepthPass.cs` (setup-time binding, no save section). **Content-utilization selftest: Orphaned 4 → 0.** Plan 191 was evaluated and REJECTED: retired by signed C3 decision — never implement.

## Package D — Census truth reconciliation (marking hosted plans)
Plans 131 (rumor), 186 (shelter maintenance), 201 (sanitation), 214 (visitors), 220 (atmosphere) verified integrated via their existing probes (`--rumor-network-selftest` PASS, `--shelter-maintenance-selftest` 12/12, SanitationHostSession + panel, `--visitor-integration-selftest` PASS, `--shelter-atmosphere-selftest` PASS); census verdicts updated; plan files archived with FULLY INTEGRATED headers.

## Verification discipline (user-directed: no excessive testing)
Per package: one focused probe/test run only (done inline above). One combined final gate run at package-set completion.
