# Plan 49 — Micro-Location Schema

## EncounterDefinition (used for micro-locations)

| Field | Type | Required | Description |
|---|---|---|---|
| id | string | yes | Stable micro-location ID (prefix: `micro_`) |
| title | string | yes | Player-facing title |
| description | string | yes | Environmental story (1-2 sentences) |
| category | string | yes | "Discovery", "Hazard", or "Social" |
| baseWeight | float | yes | Selection weight (0.1-0.8) |
| stealthWeightMultiplier | float | yes | Weight multiplier for Stealth stance |
| speedWeightMultiplier | float | yes | Weight multiplier for Speed stance |
| minDangerLevel | float | yes | Minimum danger for eligibility |
| requiredLocationId | string | no | Location-specific encounter (empty = any) |
| forceOnArrival | bool | no | Force on arrival at destination |
| choices | list | yes | Player choices (2-3) |

## EncounterChoiceDefinition (extended for micro-locations)

| Field | Type | Required | Description |
|---|---|---|---|
| choiceId | string | yes | Stable choice ID |
| text | string | yes | Player-facing choice text |
| moraleDelta | int | yes | Morale impact |
| guiltDelta | int | yes | Guilt impact |
| grantItemId | string | no | Item to grant on resolution |
| grantItemQuantity | int | no | Quantity of granted item |
| setWorldFlag | string | no | World flag to set |
| journalUnlockId | string | no | Journal/codex knowledge key |
| discoverLocationId | string | no | Location to discover |
| depletesOnResolve | bool | no | Whether choice depletes the micro-location |

## Rarity Tiers

| Tier | Weight | Examples |
|---|---|---|
| Common | 0.6-0.8 | memorial, grave, pipe, barricade, tent, shrine |
| Uncommon | 0.4-0.5 | truck, bus, bridge, greenhouse, clinic, radio tower |
| Rare | 0.1-0.3 | emergency cache, observation post, drone, fuel cache, supply drop |

## Category Distribution

| Category | Count | Purpose |
|---|---|---|
| Discovery | 20 | Environmental storytelling, loot, information |
| Hazard | 3 | Risk/reward decisions, contamination |
| Social | 2 | Ethical decisions, moral texture |
