# Plan 76 — Expedition Route Dossiers & Readiness Briefings

## Goal

Give every already-wired expedition destination a concise, trustworthy route dossier: what is
known, what is uncertain, what preparation matters, and what changes after a visit. This plan
does not add destinations or alter the destination catalog.

## Scope boundary

- Plan 32 owns the expeditions.json destination wiring and final destination count.
- Plan 21 owns equipment-condition warnings and projected gear risk.
- Plan 133 owns persistent world consequences after discovery.
- This plan owns the player-facing dossier and readiness evidence that joins those facts. It must
  not add locations, duplicate route geometry, or reimplement expedition dispatch.

## Task 1 — Dossier contract

1. Create a data-backed route dossier keyed by an existing destination id: known hazards, required
   capabilities, recommended supplies, uncertainty level, and discovery status.
2. Derive live readiness from the existing expedition, vehicle, weather, and gear authorities.
   Static dossier data must never copy their mutable values.
3. Record what the player learned from a visit as compact discovery facts for later briefings.

## Task 2 — Decision support

1. Show a pre-departure briefing that distinguishes confirmed facts, estimates, and unknowns.
2. Highlight missing requirements and competing commitments without blocking a valid risky choice.
3. Update the dossier after a completed expedition using Plan 133 consequence facts where present.

## Task 3 — Validation

1. Validate every dossier key against Plan 32's destination catalog.
2. Test stable rendering for undiscovered, partially discovered, and changed destinations.
3. Test old saves without dossiers and ensure readiness derives only from current authoritative state.

## Verification

    dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
    dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
    dotnet build Ashfall.csproj
    godot --headless --path . -- --data-integrity-selftest
    godot --headless --path . -- --bridge-selftest

## Definition of Done

- Plan 32 remains the sole destination/catalog owner.
- A departure briefing clearly separates facts, estimates, and unknowns.
- Route dossiers remain valid after persistent world changes.
