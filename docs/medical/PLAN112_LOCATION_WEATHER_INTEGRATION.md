# Plan 112 location and weather integration

## Current contract

The active disease DTO has no `location_id`, weather condition, season,
hazard, or outbreak-weight field. The live location and weather systems do not
consume disease rows directly. Adding those fields would create inert data,
not integration.

## Existing seams

| Existing seam | Current disease contract | Plan 112 action |
|---|---|---|
| wildlife butchery | `disease_zoonotic_flu` via `TryExpose` | preserved |
| autopsy pathogen | `disease_zoonotic_flu` via `TryExpose` | preserved |
| sump flooding | `disease_silt_jaundice` source contract | preserved |
| excavation | `disease_deep_excavation_mold_lung` source contract | preserved |
| ecological infestation | spore blight / fungal respiratory source contract | preserved |
| generic direct exposure | any catalog ID through `TryExpose` / `Infect` | used by all four additions |

The four additions are therefore runtime-reachable through the existing
catalog/runtime path, but are not falsely claimed as weather- or
location-specific triggers.

## Deferred follow-up

A future location/weather plan may add a typed source registry or source
weights. It must define ownership, deterministic roll order, save behavior,
and source contracts before new disease rows are attached to it.
