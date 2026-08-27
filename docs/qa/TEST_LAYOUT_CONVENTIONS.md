# ASHFALL Test Suite Layout & Conventions

> **Canonical Test Suite:** `Ashfall.Core.Tests/` (`Ashfall.Core.Tests.csproj`, .NET 9.0 xUnit).
> **Master Directives:** [`AGENTS.md`](../../AGENTS.md).

---

## 1. Domain-Organized Directory Structure

All unit, integration, simulation, and contract test files are organized by domain under `Ashfall.Core.Tests/`:

```
Ashfall.Core.Tests/
├── AssemblyInfo.cs               # Assembly metadata and test configuration
├── CatalogTestBase.cs            # Shared fixture for data catalog validation
├── StubRng.cs                    # Deterministic seeded PRNG test stub
├── TestCategories.cs             # xUnit Category trait definitions
│
├── Core/                         # Simulation systems, survival logic, mechanics, timers
├── Host/                         # Host CLI, Godot adapters, summaries, bridge contracts
├── Save/                         # Save stores, codecs, checksums, wire format, persistence
├── UI/                           # UI panels, accessibility, focus/modal dismissal gates
├── Data/                         # JSON catalog loaders, schemas, and rule compliance
├── Narrative/                    # Quests, dialogues, moral choice arcs, factions, lore
├── Combat/                       # Ballistics, trauma, weapon condition, combat simulation
└── Tooling/                      # CI lint gates, forbidden APIs, docs/manifest checkers
```

---

## 2. Structural & Namespace Conventions

1. **Namespace Uniformity:** All test classes reside in the `Ashfall.Core.Tests` root namespace regardless of subfolder placement, preventing namespace churning when files are reorganized.
2. **Naming Convention:** Test classes end with `*Tests.cs` (e.g., `EconomySystemTests.cs`, `SaveWireContractTests.cs`).
3. **Engine Isolation (Invariant 1):** Tests under `Ashfall.Core.Tests/` test simulation and host contracts without requiring the live Godot graphical server.
4. **Deterministic PRNG (Invariant 4):** Tests use `ISeededRng` / `StubRng` / `CoreSeededRng`. Never `System.Random` or `Guid.NewGuid()`.

---

## 3. Test Traits & Filtered Execution

Tests use categorical trait attributes defined in [`TestCategories.cs`](../../Ashfall.Core.Tests/TestCategories.cs):

| Trait Attribute | Target Scope | Example Command |
|---|---|---|
| `[UnitTest]` | Fast isolated logic and unit contracts | `dotnet test --filter "Category=Unit"` |
| `[SaveTest]` | Save codecs, checksum envelopes, and persistence | `dotnet test --filter "Category=Save"` |
| `[DataTest]` | JSON catalogs, schema validity, and rules | `dotnet test --filter "Category=Data"` |
| `[IntegrationTest]` | Multi-system headless simulation flows | `dotnet test --filter "Category=Integration"` |

---

## 4. Execution Commands

```bash
# Run the entire test suite
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj

# Run tests by name / class
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~SaveWireContractTests"

# Run tests in a specific category
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "Category=Save"
```
