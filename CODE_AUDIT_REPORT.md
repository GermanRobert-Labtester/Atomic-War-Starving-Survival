# ASHFALL EXPANSION CODE AUDIT — FINDINGS & FIX PLAN

> Date: Sweep of all 42 new systems, 12 wiring files, 19 UI widgets
> Critical: 12 (compile-breaking) | High: 2 (runtime leaks) | Medium: 3 (logic polish)

---

## CRITICAL — Compile Errors (Fix Immediately)

| # | File | Issue | Fix |
|---|------|-------|-----|
| C1 | `GameBootstrap.Phase3Wiring.cs` | References `AddictionSystem` property — actual property name is `Addiction` | Rename references |
| C2 | `GameBootstrap.Expansions3to4DeepLoreHud.cs` | `MapScreenUI.OnLocationSelected` doesn't exist | Remove/guard subscription |
| C3 | `GameBootstrap.DeepLoreWiring.cs`, `GameBootstrap.Expansions3to4Wiring.cs` | `EventRunner.TriggerEventById` doesn't exist | Use existing event queue API |
| C4 | `GameBootstrap.Expansions3to4DeepLoreHud.cs` | `EconomySystem.GetStanding()` doesn't exist — actual is `GetTrust(factionId)` | Rename call |
| C5 | `GameBootstrap.AntigravityWiring.cs` | `_locationCatalog.AllLocations` doesn't exist — actual is `locations` List | Rename property access |
| C6 | `GameBootstrap.AntigravityWiring.cs` | `WeatherKind.AshStorm` doesn't exist — actual enum value is `Ashfall` | Rename enum ref |
| C7 | `GameBootstrap.AntigravityWiring.cs`, `GameBootstrap.Phases9to10Wiring.cs` | `Shelter.GetAvailableBedCount()` doesn't exist | Remove/implement helper |
| C8 | `GameBootstrap.Phases9to10Wiring.cs` | `HatchDefenseSystem.ForceRaid()` doesn't exist | Remove call, log warning |
| C9 | `GameBootstrap.Phase0Expansion.cs` | `MedicalSystem.GrantChronicIllness()` doesn't exist | Remove callback binding |
| C10 | `GameBootstrap.AntigravityWiring.cs` | `MedicalSystem.DaysSinceLastTreatment()` doesn't exist | Return 0f stub |
| C11 | `GameBootstrap.AntigravityWiring.cs` | `Shelter.IsHatchLocked()`/`UnlockHatch()` don't exist | Remove hooks, return false/no-op |
| C12 | `GameBootstrap.Phases9to10Wiring.cs` | `SaveSystem.GetFactionStanding()` doesn't exist — only `GetWorldFlag` | Return 50f default |

## HIGH — Runtime Leaks & Silent Failures

| # | File | Issue | Fix |
|---|------|-------|-----|
| H1 | `GameBootstrap.Phases9to10Wiring.cs` | 4 lambda subscriptions not wrapped in `_subscriptions.Track()` — leak on scene teardown | Add Track() calls |
| H2 | `FinalWishSystem.OnPrognosisExpired()` | Doesn't apply `WishFailedMoralePenalty` — declared constant never used | Add penalty application |

## MEDIUM — Logic Polish

| # | File | Issue | Fix |
|---|------|-------|-----|
| M1 | `RadiationPhaseProgression.ResolveManifest()` | `deathChance` unclamped — could exceed 1.0 silently | Clamp to 0..1 |
| M2 | `MoralBranchingSystem.RegisterMoralChoice()` | Branch decision uses only LAST choice direction, not accumulated tally | Keep simple (by design) but document |
| M3 | `BunkerManifestoSystem.GetAdherenceMoraleModifier()` | Per-hour scaling inconsistent between bonus and penalty | Normalize both to per-hour |

---

## FIX ORDER

1. C1-C12: Compile fixes (rename refs, guard calls, remove dead hooks)
2. H1-H2: Leak + penalty fixes
3. M1-M3: Clamping + scaling polish
