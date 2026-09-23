# PLAN-ORPHAN-SEAL-01 — Appendix U: Data-Reference Verification

**Generated:** 2026-09-21. String literals inside each orphan's source matched
against **every** JSON file anywhere under `Data/` (recursively). This shows
what a system actually loads, which is stronger than Appendix M's type-name
matching. Most orphans carry no hardcoded filename — their loaders receive a
path — so a missing row here is not a data gap; the four rows that do carry
literals are verified below.
**Use:** a package whose loader reads one of these files has its data dependency
proven from source. `exists in subdir` rows name the actual relative path.

### `CookingSystem`

| Literal | Resolves | Path |
|---|---|---|
| `recipes_cooking.json` | **yes** | `recipes_cooking.json` |

### `CampaignLegacySystem`

| Literal | Resolves | Path |
|---|---|---|
| `legacy_traits.json` | **yes** | `legacy_traits.json` |

### `ModSupportSystem`

| Literal | Resolves | Path |
|---|---|---|
| `colony_blueprints.json` | **yes** | `colony_blueprints.json` |
| `factions.json` | no | — |
| `hobby_definitions.json` | **yes** | `hobby_definitions.json` |
| `items.json` | **yes** | `items.json` |
| `locations.json` | **yes** | `locations.json` |
| `map_regions.json` | **yes** | `map_regions.json` |
| `nuclear_winter_phases.json` | **yes** | `nuclear_winter_phases.json` |
| `quests.json` | no | — |
| `weather_effects.json` | **yes** | `weather_effects.json` |

### `CascadeTargetSystem`

| Literal | Resolves | Path |
|---|---|---|
| `weather_gameplay_effects.json` | **yes** | `weather_gameplay_effects.json` |

