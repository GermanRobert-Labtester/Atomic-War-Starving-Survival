# Plan 38 — Sky-Layer Armor Catalog (system exists, no data)

## Goal (2 lines)
Create `sky_layer_armor_catalog.json` for `SkyLayerArmorSystem` — the system is fully
implemented and save-supported but has **no catalog** (verified: file missing). Add 6 armor
configurations and 10 orbital-threat events that make the shelter's sky layer a meaningful
defense against kinetic strikes (existing 19B orbital harrow).

## Why (P2)
- Verified: `SkyLayerArmorSystem.cs` exists in `Assets/Ashfall.Core/Shelter/`; no catalog file
  exists. The system is wired but has nothing to configure or defend against.
- Connects shelter defense (sky layer) to the orbital-strike threat (existing 19B) — without
  armor data, strikes have no counter-play.
- Pure DATA work — zero new Core code if a loader exists.

## Files to touch
- `Assets/StreamingAssets/Data/sky_layer_armor_catalog.json` (CREATE — 6 configs + 10 threats)
- Read-only: `Assets/Ashfall.Core/Shelter/SkyLayerArmorSystem.cs` (confirm schema: armor id,
  material composition, thickness, blast resistance, degradation rate, repair cost),
  `Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs` (confirm threat-event schema)
- Check loader: `grep -rn "sky_layer_armor\|SkyLayerArmor\|LoadSkyArmor" Assets/Ashfall.Core/`

## Content grammar (per armor config)
- snake_case `id` with prefix `armor_` or `sky_armor_` (confirm accepted prefix — do not invent).
- composition: list of `item_*` ids (steel plate, concrete, sandbags, salvaged hull plating).
- thickness: integer; higher = more blast resistance but more material cost.
- blast_resistance: value subtracted from incoming strike damage.
- degradation_rate: armor wears per strike; repair consumes materials.
- tier: improvised (scrap) / reinforced (steel + concrete) / military_grade (salvaged hull).

## Content grammar (per threat event)
- snake_case `id` with prefix `event_` or `harrow_` (confirm accepted prefix).
- strike_type: kinetic_rod / cluster / emp_burst / debris_fall (existing 19B taxonomy).
- damage: base damage before armor mitigation.
- warning_time: ticks of telemetry warning before impact (feeds OrbitalHarrowTelemetrySystem).
- consequence_on_breach: what happens if armor fails (shelter damage, radiation ingress,
  fire, structural collapse, survivor casualties).

## Steps
1. Read `SkyLayerArmorSystem.cs` end-to-end: confirm the armor schema, the degradation math,
   the repair mechanism, and the save DTO shape.
2. Read `OrbitalHarrowTelemetrySystem.cs`: confirm how threats feed into the armor system and
   whether threat events are data-driven or hardcoded.
3. Confirm loader status for both catalogs; if missing, add mechanical loaders.
4. Author 6 armor configurations: improvised_sandbag_layer, scrap_plate_overlay,
   reinforced_concrete_slab, steel_hull_plating, composite_military_grade,
   emergency_blast_canopy. Each with escalating material cost and blast resistance.
5. Author 10 threat events: 4 kinetic-rod strikes (varying damage + warning time), 2 cluster
   strikes (area damage), 2 EMP bursts (electronics disruption), 2 debris-fall events
   (post-strike secondary). Each with consequence-on-breach.
6. Wire 3 armor configs into shelter-crafting recipes (existing items as materials).
7. Validate: `--data-integrity-selftest`; confirm an armor → strike → mitigation →
   degradation → repair loop works in a headless boot; save round-trip for armor state.
8. xUnit: blast resistance subtracts correctly, degradation applies per strike, repair
   consumes materials, consequence-on-breach fires when armor fails, save round-trip green.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
MEDIUM — the armor↔threat integration (step 2) is the hazard: if threats are hardcoded in
`OrbitalHarrowTelemetrySystem`, externalizing them is a Core change. Confirm before authoring.

## Definition of Done
- `sky_layer_armor_catalog.json` exists with 6 armor configs + 10 threat events, all ids
  resolving, armor-strike loop works end-to-end, degradation + repair verified, save
  round-trip green, integrity + tests green.

## Follow-on
- Existing 19B (orbital strikes) — this plan provides the defense counter-play.
- Existing 29B (machine personality) — sky-layer armor as a shelter "character" element.
- Plan 39 (orbital harrow events) — telemetry events that warn of incoming strikes.
