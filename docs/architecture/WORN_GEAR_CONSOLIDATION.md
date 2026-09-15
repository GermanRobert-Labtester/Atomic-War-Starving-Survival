# WornGear consolidation

Status: resolved, 2026-09-10.

## Canonical shape

There is one definition: `Ashfall.Core.Inventory.WornGear` in
`Assets/Ashfall.Core/Inventory/Inventory.cs`. Its complete runtime shape is:

The baseline source had already removed the second radiation-local declaration;
this wave completed the consolidation by removing the residual inventory-to-
radiation conversion helpers and pinning the direct path below.

| Field | Type | Role |
|---|---|---|
| `SourceEquipped` | `EquippedItem?` | wear write-back target |
| `ConditionSink` | `IEquipmentConditionSink?` | canonical condition authority |
| `SourceItem` | `ItemDefinition?` | source item projection |
| `RadProtection` | `float` | protection coefficient |
| `MaxDurability` | `float` | durability denominator |
| `CurrentDurability` | `float` | current condition |
| `DegradeRate` | `float` | radiation wear rate |
| `OnDegraded` | `Action<float>?` | typed state-change notification |

`DurabilityFraction`, `EffectiveProtection`, and `Degrade` remain on this
canonical model. The old radiation-local shape had no additional persisted
fields; the historical wire-facing fields `RadProtection`,
`MaxDurability`, `CurrentDurability`, and `DegradeRate` are unchanged.

## Direct production path

```text
Inventory.FillWornGear(List<Inventory.WornGear>)
    -> SurvivorsHostSession.BuildExposure
    -> ExposureContext.WornGear
    -> RadiationSystem.ComputeGearProtection / DegradeWornGear
```

`RadiationSystem` uses a namespace alias to the canonical inventory type. The
host collects the same objects directly from the bound inventory; there is no
`Radiation.WornGear` duplicate and no `FromInventory` conversion bridge.

## Compatibility evidence

The old four-field JSON shape deserializes into the canonical type. Existing
`NeedsRadiationSystemTests`, protective-gear journey tests, and the
`--survivors-selftest` cover no gear, full protection, degraded protection,
multiple pieces, equipped write-back, save/restore, and host exposure wiring.
The source authority gate asserts exactly one type definition and zero bridge
references.

No radiation formula or checksum contract was changed. A save loaded with an
equipped gas mask or hazmat item continues through `FillWornGear` and produces
the same reduced dose after a fresh restore.
