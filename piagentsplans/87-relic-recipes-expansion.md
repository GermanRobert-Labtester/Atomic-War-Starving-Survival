# Plan 87 — Relic Provenance, Display & Community Memory

## Goal

Make restored relics matter after their recipe completes: capture provenance, decide whether to
display, study, trade, or memorialize the object, and let that choice feed the shelter's shared
memory. This plan does not add or modify relic recipes.

## Scope boundary

- Plan 04 owns relic_recipes.json, restoration inputs, outputs, and final recipe count.
- Plan 47 owns general collectible content.
- Plan 162 owns the shelter-history archive.
- This plan owns post-restoration provenance and the player decision around a completed relic. It
  must not add recipe rows, component items, or a competing restoration system.

## Task 1 — Provenance contract

1. Define a RestoredRelicRecord keyed to an existing Plan 04 output: origin hint, restoration
   campaign/day, restorer, and optional discovery note.
2. Create a small provenance/display catalog that references existing relic ids only.
3. Capture one record when the existing restoration system succeeds; never recalculate recipe
   ingredients or completion.

## Task 2 — Display, study, trade, or memorialize

1. Offer mutually exclusive, reversible-where-appropriate outcomes: display for shelter identity,
   study for a research lead, trade for a concrete offer, or archive as a memorial artifact.
2. Route resulting effects through the owning systems: research via its unlock bridge, trade via
   the economy authority, and history via Plan 162.
3. Keep effects modest and explicitly sourced in UI so a relic is not a hidden permanent bonus.

## Task 3 — Validation

1. Validate all provenance entries against Plan 04 output ids and all effects against their owning
   catalogs.
2. Test restoration → record → choice → save/load, including legacy restored relics with no record.
3. Ensure the same relic cannot create duplicate provenance records after load/retry.

## Verification

    dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
    dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
    dotnet build Ashfall.csproj
    godot --headless --path . -- --data-integrity-selftest
    godot --headless --path . -- --bridge-selftest

## Definition of Done

- Plan 04 remains the only relic-recipe owner.
- Every recorded relic refers to one existing restored output.
- Post-restoration choices have visible, owned consequences rather than a second crafting loop.
