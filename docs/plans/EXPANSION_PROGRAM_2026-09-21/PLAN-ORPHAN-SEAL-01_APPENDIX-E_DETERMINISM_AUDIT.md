# PLAN-ORPHAN-SEAL-01 — Appendix E: Orphan Determinism & Time Audit

**Generated:** 2026-09-21. Banned-source audit across all 99 host-unreachable
authorities for nondeterministic primitives versus the seeded contract.
**Findings:** **5 of 99 orphans touch a banned
nondeterministic source**; 17 already reference `ISeededRng` /
`CampaignRng` / `Fork(`.
**Rule:** every package that is stateful or rolling must fork from
`CampaignRngStream` (63 registered streams, see
`PLAN-DETERMINISM-REPLAY-13` Appendix A); wall-clock and `System.Random`
sources are retired at the seam, not merely hidden behind a host adapter.

## Banned-source findings (wiring blockers)

| Authority | File | System.Random | new Random | Guid.NewGuid | DateTime.Now/UtcNow | Offset | TickCount |
|---|---|---:|---:|---:|---:|---:|---:|
| `CommunicationsSystem` | `Communications/CommunicationsSystem.cs` | 0 | 0 | 2 | 0 | 0 | 0 |
| `SeasonalCelebrationSystem` | `Events/SeasonalCelebrationSystem.cs` | 0 | 0 | 1 | 0 | 0 | 0 |
| `SessionDurabilityManager` | `Save/SessionDurabilityManager.cs` | 0 | 0 | 0 | 2 | 0 | 0 |
| `DisasterResponseSystem` | `Shelter/DisasterResponseSystem.cs` | 0 | 0 | 1 | 0 | 0 | 0 |
| `ShelterExpansionSystem` | `Shelter/ShelterExpansionSystem.cs` | 0 | 0 | 3 | 0 | 0 | 0 |

## Seeded-contract references already present

| Authority | ISeededRng | CampaignRng | Fork( |
|---|---:|---:|---:|
| `CommunicationsSystem` | 2 | 0 | 0 |
| `CookingSystem` | 3 | 0 | 0 |
| `CultureCreationSystem` | 1 | 0 | 0 |
| `SurvivorEducationSystem` | 1 | 0 | 0 |
| `SeasonalCelebrationSystem` | 1 | 0 | 0 |
| `TerritoryControlSystem` | 3 | 0 | 0 |
| `CampaignLegacySystem` | 4 | 0 | 0 |
| `PsychologicalProfileSystem` | 1 | 0 | 0 |
| `OutpostSettlementSystem` | 1 | 0 | 0 |
| `CupolaFoundryEngine` | 3 | 0 | 0 |
| `DisasterResponseSystem` | 1 | 0 | 0 |
| `HobbySystem` | 1 | 0 | 0 |
| `SurvivorAutonomySystem` | 3 | 0 | 0 |
| `SurvivorVoiceSystem` | 1 | 0 | 0 |
| `NuclearWinterProgressionSystem` | 2 | 0 | 0 |
| `WeatherCascadeSystem` | 2 | 0 | 0 |
| `CascadeTargetSystem` | 1 | 0 | 0 |

## Clean authorities

No banned source and no seeded reference found in the remaining 77 authorities; each package still confirms its determinism class at the seam (EP-01).
