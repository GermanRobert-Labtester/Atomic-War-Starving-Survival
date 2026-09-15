# Echo Runtime Wiring Pattern

## Authority

`Assets/StreamingAssets/Data/echoes.json` is the authored authority. The
engine-free runtime owner is `Ashfall.Core.Narrative.EchoSystem`, loaded by
`EchoCatalogLoader`. `EchoSystem` owns:

- day and world-flag eligibility;
- deterministic weighted selection on `CampaignStreamIds.Echo`;
- one-time resolution;
- pending modal identity;
- delayed-consequence due days;
- capture and restore of all echo state.

The Godot host does not reimplement those rules. `EchoHostSession` loads the
catalog and the `echoes` save section. `Main.Echoes.cs` applies typed effects
through the existing inventory, survivor-needs, consequence-ledger, and
journal owners.

## Daily ordering

`NarrativeQuestsVerdictDayOwner` performs the following sequence:

1. tick due delayed consequences;
2. emit the due-consequence day event;
3. select an Echo only when no narrative arc was selected;
4. persist the pending Echo through the normal save orchestrator.

Selecting an Echo has no gameplay effect beyond persisting its pending ID.
The existing `NarrativeArcModal` presents both narrative arcs and Echoes.
Choice dispatch remains on the existing narrative route.

## Delayed consequences

Resolving a choice applies its immediate effects and persists
`PendingConsequences` as `(echo_id, choice_id, due_day)`. On or after the due
day, `EchoSystem.TickDay` reconstructs the authored payload by stable IDs,
removes it before emitting `OnDelayedConsequenceDue`, and therefore cannot
replay it after a save/restore or repeated day tick.

The host then applies each delayed effect through the same owners as immediate
effects. Scheduled and completed journal entries use different keys.

## Save and lifecycle ownership

The dedicated `echoes` registry row uses `echoes_save.json`. Older payloads
that omit `PendingConsequences` normalize to an empty list. Lifecycle reset
disposes the host session and clears the Main reference, while audio binding
is removed by `AudioEventBridge` disposal.

## C2[3] ownership claim

| Concern | Owner | Boundary |
|---|---|---|
| Authored Echo records | `echoes.json` | JSON only |
| Selection, pending state, resolution, delays | `EchoSystem` | Core |
| Catalog load and save envelope | `EchoHostSession` / `EchoSaveStore` | Host |
| Daily selection and due processing | `NarrativeQuestsVerdictDayOwner` | Host day-owner |
| Items, needs, flags, journal | Existing canonical owners | Host adapters |
| Surfaced audio | `AudioEventBridge` | Presentation |

No second Echo scheduler, inventory ledger, needs store, or modal route is
introduced.

## Verification

Focused contracts:

- `Ashfall.Core.Tests/Narrative/EchoCatalogTests.cs`
- `Ashfall.Core.Tests/Narrative/EchoSystemTests.cs`
- `Ashfall.Core.Tests/Economy/RegionalSupplyPriceIntegrationTests.cs`

The data-integrity selftest passes. The content-utilization selftest currently
reports eleven pre-existing orphaned catalogs whose inferred loaders have no
runtime source evidence; these remain an explicit follow-up rather than being
reclassified as optional.
