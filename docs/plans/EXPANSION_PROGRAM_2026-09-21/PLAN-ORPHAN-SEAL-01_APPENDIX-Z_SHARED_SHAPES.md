# PLAN-ORPHAN-SEAL-01 — Appendix Z: Shared Interface Shapes

**Generated:** 2026-09-21. Public methods that appear with the **same name and
arity in four or more orphans** — the de-facto interfaces the unreachable set
already shares. 8 shapes qualify.
**Cross-check:** Appendix D counted 58 orphans with a capture/restore method
*or* registry knowledge; the stricter `CaptureState`/`RestoreState` pair alone
accounts for **56** of them — more than half the orphan set already implements
the save-state shape, so save integration for those members is mechanical.
**Use:** when sealing many members of one shape, a shared Core interface (e.g.
an `IDayStepped` for `TickDay`) can reduce adapter boilerplate — but only if the
package first proves the semantics match; identical signatures are not proof of
identical contracts. Appendix AA lists which orphans actually step on the day.

| Shape | Orphans | Examples |
|---|---:|---|
| `CaptureState` (0 args) | 56 | `AccessibilitySettingsSystem`, `AgingSystem`, `BackstorySystem`, `BestiarySystem` … |
| `RestoreState` (1 arg) | 56 | `AccessibilitySettingsSystem`, `AgingSystem`, `BackstorySystem`, `BestiarySystem` … |
| `LoadCatalog` (1 arg) | 34 | `AccessibilitySettingsSystem`, `AgingSystem`, `AudioAccessibilityCoordinator`, `BackstorySystem` … |
| `Clone` (0 args) | 19 | `AntenatalMaternalHealthEngine`, `ApprenticeshipCurriculumEngine`, `ChemicalPlumeDispersionEngine`, `ChemicalReagentSynthesisEngine` … |
| `TickDay` (1 arg) | 11 | `CascadeTargetSystem`, `ColonySystem`, `CupolaFoundryEngine`, `FactionDiplomacySystem` … |
| `GetTemplate` (1 arg) | 6 | `BackstorySystem`, `FactionDiplomacySystem`, `InternalCommunicationSystem`, `ShelterMuseumSystem` … |
| `LoadCatalog` (2 args) | 4 | `BestiarySystem`, `ShelterGovernanceEngine`, `ShelterIdentitySystem`, `SurvivorAutonomySystem` |
| `TickDay` (2 args) | 4 | `AgingSystem`, `CommitmentSystem`, `SeasonalHumanMigrationEngine`, `TerritoryControlSystem` |
