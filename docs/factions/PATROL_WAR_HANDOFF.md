# ASHFALL — Patrol War-State Variants & Dynamics Contract

**Milestone:** Flagship Integration Plan VII (F11)
**Authority:** Engine-agnostic Core (`Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs`, `Assets/Ashfall.Core/Narrative/TravelEncounterSystem.cs`)
**Status:** Integrated & Verified

---

## 1. Executive Summary

Wasteland faction patrols react dynamically to geopolitical conflict. When war erupts between factions in the Year of Ash, patrol profiles shift from routine border inspections and trade escorts to aggressive reconnaissance, armed raiding parties, and heavy combat advance guards.

This system integrates `FactionWarSystem` directly into `TravelEncounterSystem` selection without duplicating encounter tables or desynchronizing cooldown state.

---

## 2. Schema Specification: `war_state` and `war_weight_multiplier`

Each travel encounter definition supports two war-sensitive fields in `Assets/StreamingAssets/Data/travel_encounters.json`:

```json
{
  "id": "enc_patrol_ash_sign_wartime_recon",
  "cooldown_group": "patrol_ash_sign_recon",
  "war_state": "wartime",
  "war_weight_multiplier": 1.5,
  ...
}
```

### Supported War States
1. **`any`** (default): Eligible during both peacetime and wartime.
2. **`peacetime`**: Strictly gated to peacetime (`!FactionWarSystem.IsAtWar`). Ineligible during active war.
3. **`wartime`**: Strictly gated to wartime (`FactionWarSystem.IsAtWar`). Ineligible during peacetime.

### Weight Multiplier Dynamics
- When wartime is active (`FactionWarSystem.IsAtWar == true`), the encounter's base weight is multiplied by `war_weight_multiplier` during selection weighting.
- Defaults to `1.0` if omitted or unauthored. Must be strictly positive (`> 0f`) as enforced by `PatrolEncounterValidator`.

---

## 3. Sibling Variant Families & Cooldown Sharing

To represent shifting tactical postures under war conditions while maintaining narrative continuity, sibling variants share a single `cooldown_group`:

| Cooldown Group | Peacetime / Any Variant | Wartime Variant | Shared Recurrence |
|---|---|---|---|
| `patrol_warlord_raid` | `enc_patrol_warlord_raid` (any) | `enc_patrol_warlord_advancing` (wartime) | 7 days |
| `patrol_ash_sign_recon` | `enc_patrol_ash_sign_scouts` (any) | `enc_patrol_ash_sign_wartime_recon` (wartime) | 5 days |

### Validation & Symmetry Invariant
`PatrolEncounterValidator` verifies that all encounters within a `cooldown_group`:
- Share identical `cooldown_days` and `patrol_archetype`.
- Share identical chain stage bounds.
- Share mechanically identical choices (identical `is_nonviolent`, `cost_items`, `faction_standing_delta`, `morale_delta`, `guilt_delta`, item requirements, and standing requirements).

When any variant is encountered and resolved, the entire cooldown group is placed on cooldown. Surviving an advance guard prevents an immediate raid encounter on the following day.

---

## 4. Live Query & Immediate Invalidation

- `TravelEncounterSystem` evaluates `_factionWar.IsAtWar` in real time during every encounter check.
- When `FactionWarSystem.SetWarActive(true)` or `(false)` is invoked (e.g., triggered by story events, faction tension thresholds, or ceasefire treaties), encounter candidate pools update immediately with zero cache invalidation lag.

---

## 5. Save & Replay Guarantees

- **State Persistence:** `FactionWarSystemState.isWarActive` is serialized and deserialized within the standard save envelope.
- **Determinism:** All encounter selection weighting obeys `ISeededRng`. Verified by `PatrolWarStateVariantTests` and `PatrolCampaignCrossSystemSmokeTests`.
