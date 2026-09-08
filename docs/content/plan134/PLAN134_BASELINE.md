# Plan 134 Baseline

## Current authority

`Assets/StreamingAssets/Data/starting_supplies.json` was a schema-version 1
object containing one `starting_supplies` array. Each row used the DTO:

```text
itemId: string
amount: int
```

The legacy production list contained 15 distinct item IDs and 59 total units:

| Item | Amount |
|---|---:|
| `clean_water` | 12 |
| `canned_food` | 16 |
| `irradiated_water` | 4 |
| `item_air_filter_hepa` | 2 |
| `item_desal_membrane` | 1 |
| `iodine_pills` | 4 |
| `bandage` | 2 |
| `rad_away` | 1 |
| `item_dosimeter_pen` | 1 |
| `item_geiger_m3` | 1 |
| `gas_mask` | 1 |
| `hazmat_suit` | 1 |
| `battery` | 4 |
| `scrap_mechanical` | 6 |
| `scrap_electronic` | 3 |

The list order was preserved by the loader and is retained for the Standard
Holdfast profile. Inventory stack splitting is owned by `Inventory.Add`; the
starting-supply loader does not reinterpret quantities.

## Loader behavior before Plan 134

`ItemCatalogLoader.LoadStartingSuppliesDetailed`:

- locates `starting_supplies.json` under the shared data authority;
- accepts a wrapped list through `CatalogLocator.LoadWrappedList`;
- rejects missing files, empty files, parse failures, null/empty IDs, amounts
  less than or equal to zero, duplicate IDs, and unknown item IDs when an item
  catalog is supplied;
- rejects duplicate IDs case-insensitively;
- returns an empty list through the convenience overload when validation fails.

`InventoryHostSession.Create(dataDir)` first loads the inventory save. Only a
null save enters `LoadOrSeedStartingSupplies`. The host also contained a
hardcoded 15-stack `SeedStartingSupplies` fallback. Before this plan,
`Create` used the default `failClosed: true`, so the fallback was available to
explicit non-fail-closed callers but was not the normal missing-file behavior.

## New-game paths

- The production Godot path is `Main.StartNewGame` → `ComposeCampaign` →
  `SetupInventory` → `InventoryHostSession.Create`.
- `InventoryHostSession.Create` is also used by direct host tests and can be
  constructed without data for fixture/selftest paths.
- `StartingCohortSetupPanel` is the existing menu-only selection surface. It
  previously selected only survivor cohort IDs.
- `StartNewGame` already receives the cohort choice before `ComposeCampaign`;
  Plan 134 adds a separate starting-supplies profile ID at the same boundary.
- Continue/restore sets `CampaignInitializationMode.Restore` before rebuilding
  sessions. Restore must never request fresh inventory seeding.
- No existing save or settings contract needs to remember the origin after the
  inventory has been seeded. The inventory save remains the authority.

## Compatibility decision

Production data is converted to schema version 2 with six profiles. The Core
loader continues to read a schema-version 1 flat file for migration/tool
compatibility. Missing, empty, malformed, or unusable profile data resolves to
the same hardcoded Standard Holdfast 15-stack fallback. The default profile is
always `origin_standard_holdfast`.

## Baseline verification

The focused pre-change tests passed:

```text
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
  --filter "FullyQualifiedName~RuntimeJsonBootstrapParityTests|FullyQualifiedName~InventorySystemTests|FullyQualifiedName~StartingCohort"
PASS: 32/32
```
