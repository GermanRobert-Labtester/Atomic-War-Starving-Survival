# Foundry Treaty Epilogue & Endgame Handoff Contract

**Target System:** `Assets/Ashfall.Core/Endgame/`, `Assets/Ashfall.Core/Narrative/JournalSystem.cs`
**Host Hook:** `src/Foundry/SilentFoundryHostSession.cs`

---

## 1. Endgame Slate Construction

The ASHFALL endgame epilogue evaluates the regional balance of power upon campaign completion (Day 365+ or completion of District 8 constitution).

The consequence state ledger (`SilentFoundryConsequenceState.applied`) provides the authoritative audit trail of:
1. Which treaties were consistently **Honored** (`met`).
2. Which treaties suffered **Logistical Defaults** (`missed`).
3. Which treaties suffered **Willful Breaches** (`violated`).

---

## 2. Epilogue Outcome Mapping

| Treaty Status at Day 365 | Epilogue Slide Text Category | Wasteland Post-War Legacy |
|---|---|---|
| All 3 District 8 Treaties `met` | `epilogue_cluster_constitution_ratified` | The Silent Foundry is recognized as a permanent civil works; the school bell rings daily; District 8 achieves cooperative self-governance. |
| District 8 Treaties `missed` / `violated` | `epilogue_foundry_scrap_feud` | The cupola runs cold for lack of coal; Cutters confiscate remaining tooling for rail ballast; workers disperse into scavenger bands. |
| `treaty_deep_coast_aquifer_protection_treaty` `met` | `epilogue_deep_coast_aquifer_secured` | Coastal marsh pump stations remain operational; desalination screen maintenance passes to a joint maritime council. |
| `treaty_deep_coast_aquifer_protection_treaty` `violated` | `epilogue_aquifer_salinated` | Bilge seepage penetrates the lower intake manifold; the Deep Coast marsh becomes a brackish wasteland, forcing mass migration inland. |
| `treaty_garrison_grain_tithe_compact` `met` | `epilogue_verge_granary_endures` | Checkpoint Gamma transitions from a fortress into a grain trading exchange; Eastern Arterial Road remains safe for trade. |
| `treaty_garrison_grain_tithe_compact` `violated` | `epilogue_verge_famine_blockade` | Checkpoint Gamma becomes an abandoned choke point strewn with burnt carts; Verge farming communities collapse into subsistence enclaves. |
| `treaty_switchback_fuel_and_passage_accord` `met` | `epilogue_switchback_pilgrim_way` | The mountain snowline cairns remain lit with lamp oil; pilgrims and couriers cross the High Scarp unimpeded. |
| `treaty_switchback_fuel_and_passage_accord` `violated` | `epilogue_switchback_frozen_divide` | Sealed passes leave mountain settlements isolated; winter freezes claim dozens each cycle along the abandoned switchback. |
