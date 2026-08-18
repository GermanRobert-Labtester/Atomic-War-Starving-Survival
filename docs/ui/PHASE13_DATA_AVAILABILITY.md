# Phase 13 — Data Availability Report

**Date:** this turn.
**Verified Phase 12 baseline:** 12/12 snapshots PASS; 1973/1973 tests; 0 W / 0 E; 41/41 bridge; 3588 IDs / 0 findings; 48/48 asset registry; 6 MATCH + 6 PARTIAL + 53 UNPAIRED.
**Scope:** authoritative source for every metric that Phase 13 may want to render. Classify `LIVE` / `DERIVED_DISPLAY_ONLY` / `FIXTURE_ONLY` / `UNAVAILABLE`. Apply the same gate to Tier-2 surfaces and the six remaining PARTIAL screens.

This document prevents future agents from inventing UI data. If a field is `UNAVAILABLE`, no Phase 13 widget renders it.

---

## Tier-2 — Tier-2 matrices / ledgers

### Faction Matrix

| Desired column | Authoritative API | Availability | Display transformation |
|---|---|---|---|
| Faction Id | `faction_lore.json` (`StreamingAssets/Data/faction_lore.json`) | LIVE | raw id upper-case + space |
| Faction Display | `faction_lore.json.displayName` | LIVE | direct |
| Stance | `IFactionStanceProvider.GetStance(factionId)` | LIVE | `Trade` / `Rob` / `HostileRaid` / `Refuse`; cell colour via `CellState` |
| Trust | `IFactionStanceProvider.GetEffectiveTrust(factionId)` | LIVE | rounded integer; threshold tiers map to `CellState` |
| Aggression | `IFactionStanceProvider.GetRaidAggression(factionId)` | LIVE | `0.00` to `1.00` numeric |
| Hostility threshold | `FactionStanceEngine.RegisterFaction` thresholds | LIVE | `(RobThreshold,RaidThreshold)` pair read via reflection if present |
| Lore one-liner | `faction_lore.json.shortDescription` | LIVE | first 80 chars |
| Faction mutual relations | UNAVAILABLE | No graph stored | — |
| Active wars | UNAVAILABLE | no `currentWarId` field in `FactionStanceEngine` | — |

**Verdict:** IMPLEMENT. Six columns, real data.

### Dose Ledger

| Desired column | Authoritative API | Availability | Display transformation |
|---|---|---|---|
| Survivor id | `DoseEntry.survivorId` | LIVE | raw |
| Cumulative dose (mSv) | `DoseLedgerSystem.GetCumulative(survivorId)` | LIVE | `0.0 mSv` |
| Dosimeter tag | `DoseEntry.assignedDosimeterTag` | LIVE | tag or `—` when unbound |
| Shielding factor | `DoseEntry.shieldingFactor` | LIVE | `0.00` |
| Baseline mSv | `DoseEntry.baselineMsv` | LIVE | `0.0` |
| Latest reading | `DoseEntry.readingsHistory` Last() | LIVE | day × source × booked mSv |
| Last anti-rad day | `DoseEntry.lastAntiRadDay` | LIVE | `D{n}` or `—` |
| Band | `DoseLedgerSystem.GetBand(...)` computed from cumulative vs Amber/Red/Black | LIVE | `Green` / `Amber` / `Red` / `Black` → `CellState` |
| Cause-of-exposure narrative | `DoseReading.source` | LIVE | string |

**Verdict:** IMPLEMENT. Eight columns, real data.

### Skill Matrix

| Desired column | Authoritative API | Availability | Display transformation |
|---|---|---|---|
| Survivor skill id | UNAVAILABLE | `Assets/_Game/Survivors/SkillProgressionSystem.cs` (Unity legacy) | never brought into Ashfall.Core |
| Skill tier | UNAVAILABLE | not ported | — |
| Skill level | UNAVAILABLE | not ported | — |
| Skill progress | UNAVAILABLE | not ported | — |
| Gain rate | UNAVAILABLE | not ported | — |
| Training state | UNAVAILABLE | not ported | — |

**Verdict:** **DEFER — AUTHORITATIVE DATA SOURCE ABSENT.**

The closest analogues in the engine-agnostic `Ashfall.Core/Survivors/` namespace are:
- `LeadershipSystem` — single binary `IsDesignatedLeader` per survivor
- `GuiltInsomniaSystem` — single scalar `GetSleepQualityMultiplier`
- `RationConflictSystem` — adversarial scalars

None of these hold the multi-skill matrix required by Stitch #22 / #30.

Required additions to unblock Skill Matrix in a later phase:
1. Port `SkillProgressionSystem` from `Assets/_Game/Survivors` into `Ashfall.Core/Survivors/`,
   including `SkillDef`, `SkillTier`, `SkillProgression`, and the per-survivor dancer model.
2. Extend `SurvivorsHostSession.CaptureSave().survivors` to persist a `Dictionary<skillId, level>`
   per survivor slice.
3. Reauthor the seven-tier skill catalog.

Until that exists, the Skill Matrix widget will not be painted.

---

## Original PARTIAL surfaces

### Inventory (`InventoryPanel`, snapshot `inventory_default`)

| Desired column / surface | Authoritative API | Availability |
|---|---|---|
| Item display | `Inventory.Slots[].Item.displayName` | LIVE |
| Item icon | `AssetRegistry.GetItem`.Texture | LIVE |
| Item count | `Inventory.Slots[].Amount` | LIVE |
| Equip line | `InventoryHostSession.EquipLine()` | LIVE |
| Capacity (kg) | `Inventory.GetCurrentWeight()/MaxWeight` | LIVE |
| Slot count | `Inventory.Slots.Count` | LIVE |
| Per-item durability | UNAVAILABLE — no `durability` field in engine | — |
| Item filter / category | UNAVAILABLE — no Category field | — |

**Verdict:** IMPLEMENT lightweight HYBRID (sidebar, status rail, list). No DataGrid needed because the data is row-major and the current row layout already serves it.

### Survivors (`SurvivorsPanel`, snapshot `survivors_default`)

| Desired column | Authoritative API | Availability |
|---|---|---|
| Survivor id / name | `SurvivorsHostSession.RosterState[].Id` | LIVE |
| Living count | `RosterState.Count(IsAliveState)` | LIVE |
| Health | `slice.Health / slice.MaxHealthCap` | LIVE |
| Radiation | `slice.radiationDose` | LIVE |
| Hunger / Thirst | `slice.Hunger / Thirst` | LIVE |
| Has iodine resistance | `slice.hasRadResistance` | LIVE |
| Skill proficiency | UNAVAILABLE — see Skill Matrix above | — |

**Verdict:** IMPLEMENT lightweight HYBRID with survivor roster DataGrid + status rail + detail pane.

### Radio (`RadioPanel`, snapshot `radio_default`)

| Desired column | Authoritative API | Availability |
|---|---|---|
| Day | `RadioIntercept.Day` | LIVE |
| Frequency | `RadioIntercept.FrequencyMhz` | LIVE |
| Callsign | `RadioIntercept.Callsign` | LIVE |
| Faction | `RadioIntercept.FactionId` | LIVE |
| Signal strength | `RadioIntercept.SignalStrength/5` | LIVE |
| Message | `RadioIntercept.Message` | LIVE |
| Current frequency | `RadioHostSession.CurrentFrequency` | LIVE |
| Day counter | `RadioHostSession.Day` | LIVE |
| Monitored channels | `RadioHostSession.Engine.FactionCount` | LIVE |
| Spectrogram | UNAVAILABLE | — |
| Frequency tuning control | UNAVAILABLE | — |

**Verdict:** IMPLEMENT HYBRID — current tuner log becomes a DataGrid, status rail carries frequency / day / monitored channels ; no fabricated waveform.

### Weather (`WeatherPanel`, snapshot `weather_default`)

| Desired column | Authoritative API | Availability |
|---|---|---|
| Day | `WeatherForecastEntry.Day` | LIVE |
| Outlook kind | `WeatherForecastEntry.Kind` (enum) | LIVE |
| Outdoor rad | `WeatherForecastEntry.OutdoorRad` | LIVE |
| Current weather | `WorldHostSession.Weather.Current` | LIVE |
| Outdoor rad modifier | `WorldHostSession.Weather.OutdoorRadModifier` | LIVE |
| Plume passing | UNAVAILABLE — `PriceShockKind.PlumePassing` is a Trade concept, not weather | — |
| Storm probability | UNAVAILABLE | — |
| Wind vector | UNAVAILABLE | — |
| Confidence interval on forecast | UNAVAILABLE | — |

**Verdict:** IMPLEMENT HYBRID — forecast DataGrid + status rail with current/forecast highlights ; no fabricated confidence.

### Verdict (`src/VerdictPanel.cs`, snapshot `verdict_default`)

| Desired column | Authoritative API | Availability |
|---|---|---|
| Final grade | `VerdictHostSession.FinalGrade` | LIVE |
| Cumulative score | `VerdictHostSession.CumulativeScore` | LIVE |
| Alignment | `VerdictHostSession.FinalAlignment` | LIVE |
| Days survived | `VerdictHostSession.DaysSurvived` | LIVE |
| Roster records | `VerdictHostSession.RosterRecords` | LIVE |
| Site records | `VerdictHostSession.SiteRecords` | LIVE |
| Transmission records | `VerdictHostSession.TransmissionRecords` | LIVE |

**Verdict:** IMPLEMENT HYBRID — DataGrid for roster/site/transmission records; status rail for grade/score/days/alignment.

### Trade (`TradeScreenGodotPanel`, snapshot `trade_default`)

| Desired column | Authoritative API | Availability |
|---|---|---|
| Faction stance | `IFactionStanceProvider.GetStance(factionId)` | LIVE |
| Trust / aggression / repels | LIVE | already wired |
| Player offers | `_playerOfferCounts` | LIVE |
| Faction asks | `_factionAskCounts` | LIVE |
| Arbitrator scale | LIVE | already wired |
| Grim drawer (Biological trade) | LIVE | already wired |

**Verdict:** Keep PARTIAL. This panel is the **child** of `CaravanBarterLedgerPanel` (already MATCH). Adding another dashboard shell would produce nested shells that double chrome, contradict the brief's instruction not to automatically wrap, and would regress an established PARTIAL that is intentionally lower-density.

Documented relationship: `TradeScreenGodotPanel` = focused trade interaction ; `CaravanBarterLedgerPanel` = dashboard view of the same backend. Both consume `IFactionStanceProvider`, `ITradeScreenViewModel`, and the same offer/ask mechanics. Consumers include `_tradePanel` in `Main.cs` and the new `CaravanBarterLedgerPanel`.

---

## Summary

- Faction Matrix → **IMPLEMENT**
- Dose Ledger → **IMPLEMENT**
- Skill Matrix → **DEFER** (documented; blocked on `SkillProgressionSystem` Port)
- Inventory → **IMPLEMENT lightweight HYBRID** (data exists; no DataGrid needed for row-major list)
- Survivors → **IMPLEMENT HYBRID** with DataGrid
- Radio → **IMPLEMENT HYBRID** with DataGrid
- Weather → **IMPLEMENT HYBRID** with DataGrid
- Verdict → **IMPLEMENT HYBRID** with DataGrid
- Trade → **KEEP PARTIAL** as contextual child of Caravan Barter Ledger

No fabricated `UNAVAILABLE` field renders anywhere.
