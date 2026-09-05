# Combat Loot Reference Matrix (Plan 54 §17, §43)

## Finding: loot authority is runtime-owned and single

There is **no per-combatant loot/drop schema** anywhere in the combat data
layer. Victory loot is resolved once, in code:

- `TacticalCombatSystem.GrantVictoryLoot()` (`TacticalCombatSystem.Damage.cs`)
  awards a fixed `scrap_metal ×3` + `ammo_556 ×6`, routing each entry through
  the `CombatHostPorts.GrantLoot` port (host inventory authority).
- No loot table exists in `combat_catalog.json`, and none was added —
  creating a second loot-drop resolver inside combat data would duplicate
  authority (constraint 1.17).

## Consequences for Plan 54 scope

| Question (§43/§44) | Current answer |
|---|---|
| Can enemies drop their equipped weapon? | No — enemies have no weapon/equipment field at all |
| Can loot be conditioned/randomized? | Not in the combat slice; the host `GrantLoot` port is the only extension seam |
| Do faction soldiers "drop pristine military gear"? | Not modeled — the fixed loot grant contains no weapons, so the §44 risk (pristine high-tier drops) cannot occur |
| Enemy loot references in data? | None exist; none were authored |

If per-archetype loot lands later (§79.6), it belongs as a new section of the
existing catalog routed through `GrantVictoryLoot`'s port — not a parallel
resolver. Until then, combat economy value is bounded by the fixed grant,
which keeps the §54 "fighting as farming" risk structurally low.
