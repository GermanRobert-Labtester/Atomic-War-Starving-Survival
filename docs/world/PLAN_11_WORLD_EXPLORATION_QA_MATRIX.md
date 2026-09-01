# Plan 11 World Exploration QA Matrix

> **Document Class:** Test & Validation Matrix
> **Target:** Plan 11 (Deep Strata, Cipher Hunts & Living Geography)
> **Harness:** `Ashfall.Core.Tests/World/Plan11ExplorationTests.cs`, `--world-exploration-selftest`

---

## 1. Test Matrix Overview

| Test Category | Target Subsystem | Verification Method | Pass Criteria |
|---|---|---|---|
| **Excavation Determinism** | `ExcavationSystem` | Paired seeded simulation (seed 1986 vs 1986) | Same daily progress, identical cave-in rolls |
| **Shoring Material Cost** | `ExcavationSystem` | Resource balance checks | Halves structural risk, boosts progress +20% |
| **Seeded Loot Profiles** | `ExcavationSystem` | Loot table resolution | Deterministic relic drops per chamber |
| **Cipher Decode Progression**| `CipherQuestChainEngine` | Multi-order sequence evaluation | Handles Heard $\rightarrow$ Key and Key $\rightarrow$ Heard |
| **Hidden Location Reveal** | `WastelandMapSystem` | Map visibility queries | Hidden prior to decode; revealed & pathable after |
| **Evolution Day Triggers** | `WorldEvolutionEngine` | Daily simulation tick | Fires once at threshold day; persists across loads |
| **Route Blockades & Pathing**| `WastelandMapSystem` | BFS route planning | Locked nodes avoided; detours chosen deterministically |
| **Location Memory Strata** | `LocationMemorySystem` | Description recast lookup | Returns appropriate recast prose by active flags |
| **Save/Load Round-Trip** | All World Stores | Atomic campaign save & restore | Byte-accurate round-trip with checksum verification |

---

## 2. Test Execution Commands

```bash
# Core xUnit suite
dotnet test Ashfall.Core.Tests --filter Plan11ExplorationTests

# Headless Godot Self-Tests
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --content-utilization-selftest
godot --headless --path . -- --world-exploration-selftest
```
