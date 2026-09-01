# Plan 50 — Distress Signal Verification & Rescue Triage

## Goal

Turn received distress signals into a legible, costly decision: assess evidence, compare rescue
opportunities against expedition capacity, and record why a call was answered, deferred, or
rejected. This plan does not add another distress-signal catalog.

## Scope boundary

- Plan 107 owns the validated `radio_distress_signals.json` entries and their schema.
- Plan 24 owns interception, triangulation, rescue-mission creation, and mission outcomes.
- Plan 157 owns signal range, encryption, interference, and jamming.
- This plan owns the assessment/triage layer only. It must not add signal rows, frequencies,
  coordinates, or a second rescue generator.

## Task 1 — Assessment contract

1. Define a Core `DistressAssessment` from a received Plan 107 signal plus Plan 157 delivery
   facts: confidence band, stale/ambiguous warnings, estimated response cost, and unresolved
   evidence.
2. Keep authoritative signal metadata in Plan 107; the assessment records observations and
   player conclusions, not a copied signal definition.
3. Create `rescue_triage_rules.json` for transparent scoring thresholds and explanatory text.

## Task 2 — Capacity and choice

1. Compare credible calls with available expedition crew, vehicle/fuel readiness, medical need,
   and competing active missions supplied by existing systems.
2. Offer answer, defer, investigate further, relay, or decline actions. Each action produces a
   clear record and, where applicable, asks Plan 24 to create its existing mission type.
3. Surface consequences without revealing hidden outcome data: missed windows, reputation hooks,
   and recurring ethical pressure when calls exceed capacity.

## Task 3 — Validation

1. Test the same received signal yields the same assessment from the same known facts.
2. Test defer/decline persistence, capacity changes, old saves with no assessment state, and no
   duplicated signal definitions.
3. Validate every referenced signal, expedition, vehicle, and localized text id.

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
```

## Definition of Done

- Plan 107 remains the sole distress-signal catalog owner.
- Players can make an informed, persistent triage decision without seeing future outcomes.
- Plan 24 remains the only rescue-mission creator.
