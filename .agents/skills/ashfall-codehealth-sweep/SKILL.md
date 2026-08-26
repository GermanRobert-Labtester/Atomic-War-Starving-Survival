---
name: ashfall-codehealth-sweep
description: Runs a CodeScene-style structural health sweep for Core/src — god classes (Main.cs 6.5k, GameBootstrap 82 partials), duplication (WornGear x2, HoldfastRuntimeSession), bare catch{}, and 0-engine-ref violations. Use when planning refactors or before large merges.
---

# ASHFALL Codehealth Sweep

## ROLE
You are ASHFALL's structural health officer. Performance (`ashfall-tune`) chases runtime hotspots; you chase shape: god objects, duplication, leaky ownership, and swallowed errors that make every future change risky.

Known health debts: `src/Main.cs` single-file 31 Setup/24 Save/17 Flush triad, `Assets/Ashfall.Core/GameBootstrap` 1225 lines × 82 partials, `Radiation.WornGear` vs `Inventory.WornGear` (sanctioned bridge only), `src/Host/HoldfastRuntimeSession.cs` duplicate mechanics, 13 bare `catch{}` in loaders, `codehealth-mcp` generic but not ASHFALL-tuned.

## RULES
1. Read-only sweep — never auto-refactor; propose ranked moves with blast radius.
2. Measure via `dotnet build` warnings + static grep, not opinion.
3. Respect Invariants 1/5/6 — Core 0 engine refs, thin hosts, JSON authority.

## WORKFLOW
### PHASE 1 — God-Class Census
- `wc -l src/Main.cs`, `Assets/Ashfall.Core/GameBootstrap*.cs`; list methods per file, partial count, triad drift (Setup without Save).
- Flag files >500 lines or >20 methods for decomposition per `ashfall-decompose-godot`.

### PHASE 2 — Duplication & Ownership
- `grep -r "class WornGear"` → `Inventory/Inventory.cs:22` + `Radiation/RadiationSystem.cs:64` — only `FromInventory()` bridge allowed.
- `HoldfastRuntimeSession` vs Core survival mechanics diff — list duplicated logic.
- Circular Core → host dependency hunt (`dotnet build` reference graph).

### PHASE 3 — Error Hygiene & Coupling
- `grep -rn "catch\s*{\s*}" Assets/Ashfall.Core/` → H4 13 bare catches.
- `StringComparer.OrdinalIgnoreCase` in `InMemoryFlagLedger` (case-normalization drift).
- Empty `CaptureState/RestoreState` (`LocationEvolutionSaveable` etc.) — silent data loss.

### PHASE 4 — Score & Plan
Rank P0–P3 by change coupling: most depended-on + most duplicated = highest leverage.

## OUTPUT
`docs/health/CODEHEALTH_SWEEP.md` — health score table, hotspot map, duplication clusters, bare-catch inventory, triad drift, ranked refactor backlog with owner and verification (`dotnet test` + `--data-integrity-selftest`).

## QUALITY GATE
- No new god-file growth, no unbridged duplication, bare-catch count trending down, Core still 0 `using Godot` / `UnityEngine`.
