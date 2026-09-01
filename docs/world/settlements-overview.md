# Plan 20 — Wasteland Settlements

> **Data authority:** `Assets/StreamingAssets/Data/settlements.json` (settlements + NPCs), `Assets/StreamingAssets/Data/repeatable_quests.json`
> **Core system:** `Assets/Ashfall.Core/World/SettlementCatalog.cs`
> **Schema version:** 1

## Purpose

Six persistent wasteland settlements give the player recurring social anchors outside the shelter. Each settlement has named NPCs whose greetings change with the player's standing, unique repeatable side-work quests with cooldowns, and a route node in the wasteland map.

## Settlement Roster

| ID | Name | Region | Character |
|----|------|--------|-----------|
| `settlement_brine_pans` | Brine Pans | Coastal Shelf | Salt extraction cooperative. Trade in salt and dried fish. |
| `settlement_iron_siding` | Iron Siding | Industrial Belt | Scrap foundry. Trade in rebar, steel plate, and welding rod. |
| `settlement_cape_beacon` | Cape Beacon | Coastal Shelf | Lighthouse watchtower and salvage relay. Navigation intel and dive equipment. |
| `settlement_slate_hollow` | Slate Hollow Enclave | High Scarp | Quarry subterranean enclave. Building stone and precision-ground hones. |
| `settlement_pilgrim_hearth` | The Pilgrim's Hearth | High Scarp | Mountain sanctuary. Herbal remedies, hot broth, rest. |
| `settlement_tinkers_notch` | Tinker's Notch | Dead Suburbs | Electronics and chassis market. Copper wire, batteries, chips. |

## NPCs (18 named)

Each settlement has 3 NPCs. NPC greetings are standing-reactive (3 tiers: ally/neutral/cautious). NPC IDs use `npc_` prefix plus trade role descriptor, e.g. `npc_salt_factor`, `npc_lighthouse_keeper`.

## Repeatable Quests (6)

| Quest ID | Settlement | Cooldown |
|----------|-----------|---------|
| `quest_repeat_brine_salt_run` | Brine Pans | 7 days |
| `quest_repeat_iron_salvage_drive` | Iron Siding | 6 days |
| `quest_repeat_cape_signal_check` | Cape Beacon | 5 days |
| `quest_repeat_slate_mine_shoring` | Slate Hollow | 8 days |
| `quest_repeat_pilgrim_supply_carry` | Pilgrim's Hearth | 5 days |
| `quest_repeat_tinker_parts_hunt` | Tinker's Notch | 4 days |

Quest targets include registered location IDs (`location_quarry_overlook`, etc.) and award standing deltas + item rewards.

## Engine Model

```csharp
// Load
catalog.Load(dataDir, fileIO);
// Standing-reactive greeting
string greeting = catalog.GetNpcGreeting(npcId, standingScore);
// Quest eligibility
bool eligible = catalog.IsQuestEligible(questId, currentDay);
// Complete quest
catalog.CompleteQuest(questId, currentDay, out int standingDelta, out string rewardItemId);
// Save state
var state = catalog.CaptureState();
catalog.RestoreState(state);
```

## Standing Scale

| Score | Tier |
|-------|------|
| ≥ 75 | Ally — warm, insider information |
| 25–74 | Neutral — transactional, professional |
| < 25 | Cautious — watchful, guarded |
