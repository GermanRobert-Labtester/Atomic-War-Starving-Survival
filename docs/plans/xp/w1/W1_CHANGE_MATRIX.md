# XP W1 Change Matrix

**Package:** `XP-WAVE1-DIFFICULTY-AUTHORITY`
**Status:** active

| Concern | Owner | Change |
|---|---|---|
| Authored presets | `difficulty_presets.json` | Four validated presets, default `difficulty_standard`, and strict starter-item references. |
| Core resolution | `DifficultyDirector` | Resolves catalog IDs; unknown explicit IDs fail closed; the unset provider remains all-one. |
| Campaign identity | `CampaignDaySave` | Adds checksummed `difficulty_preset_id`; v1 validates using its exact historical shape, then migrates to v2. |
| New Game | `Main.GameFlow` | Validates the preset before allocating the campaign slot and commits it before composition. |
| Inventory | `InventoryHostSession` through `Main.Inventory` | Grants selected starter items only while fresh initialization is active. |
| Slot restore | lifecycle registry | Resets `campaign_day`, forcing the newly selected slot's header to restore before dependent sessions. |
| Presentation | `StartingCohortSetupPanel` | Displays authored preset names and descriptions; emits only stable IDs. |

No scalar consumer, completion-history writer, or chronicle projection was
changed in this slice.
