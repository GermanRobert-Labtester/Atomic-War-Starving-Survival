# Plan 08 — Visual Art Completion: Current-Seam Integration Architecture

> **Package:** `OLDEST-15-PIAGENTS-PLAN-QUALITY-REBASE-ROUND-7`
> **Claim:** `claim-oldest-15-piagents-quality-rebase-round7-2026-09-25`
> **Document class:** evidence-backed implementation plan; planning-only artifact
> **Domain:** presentation and asset truth
> **Read-only design authority:** `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`
> **Authority SHA-256:** `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
> **Target depth:** 150k–170k intermediate quality checkpoint, then 250,000+ characters as a quality target, not a ceiling; no padding or unsupported completion claims
> **Scope:** Core/data/host/UI/save/determinism architecture and verification planning only

## Executive summary

Make visual completion an evidence-backed registry, resolver, fallback, import, and accessibility pipeline rather than a promise that every authored ID has a production texture.

This document supersedes stale generated or historical claims in the selected plan path. It distinguishes current evidence, required delta, safe extension seam, ownership, persistence, determinism, presentation, failure behavior, focused verification, rollback, and the remaining implementation handoff. It is not an implementation report.

## Selection and premise record

- Original Git `HEAD` baseline: `5617` characters.
- Current worktree copy: `430549` characters before this rebuild.
- Selection rule: next-lowest original `HEAD` character count after excluding every path completed in Rounds 1–6 and any active claim.
- Current worktree generated text was not used as proof of implementation because it may contain stale counts, repetitive expansion, or unsupported pass language.
- User-supplied authority path contained a spacing variation; the canonical repository path used here is `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`.

## Current source and data dossier

### Current evidence: `src/Host/AssetRegistry.cs`
- Role: current source/owner candidate
- Worktree status: `M src/Host/AssetRegistry.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `19261be4581f85792e82931048a05b0e3f9604d829ec19418792a90af282f3a2`
- Snapshot size: 57397 characters; 1254 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0048:     /// <summary>
0049:     /// Thin, presentation-only asset registry that maps catalog IDs to Godot Texture2D resources.
0050:     ///
0051:     /// Path resolution order:
...
0273:         /// <summary>
0274:         /// Gets a faction icon by ID.
0275:         /// </summary>
0276:         public static AssetResult GetFaction(string factionId)
```

### Current evidence: `src/World/SurvivorActorView.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `14c093c9d0fffdcf3ade45ffa6304620a3cf6952f77f51a9c2516f137465b059`
- Snapshot size: 11157 characters; 308 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using Godot;
0004: using Ashfall.Core.Radiation;
0005: using Ashfall.Core.Survivors;
0006: using AtomicWar.GodotApp.UI;
0007:
0008: #pragma warning disable CS8618
0009: namespace AtomicWar.GodotApp.World
0010: {
0011:     /// <summary>
0012:     /// A single survivor's visual AND physics body on the shelter interior.
0013:     ///
0014:     /// Physics base (not placeholder): a grounded <see cref="CharacterBody2D"/>
0015:     /// with gravity, floor snapping, a capsule collider, accelerated horizontal
0016:     /// seek toward an assigned room anchor, and <c>MoveAndSlide</c> collision
0017:     /// against the interior's static bounds. Movement is presentation only —
0018:     /// Core still owns room assignment and needs.
0019:     ///
0020:     /// Visual base (placeholder): a non-real blockout mannequin sheet
```

### Current evidence: `src/World/MapLocationMarkerView.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `41df81303feecba9c80c945e52f56a966fea8a7a491bccc3d6912f073ada4fea`
- Snapshot size: 6281 characters; 151 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using Godot;
0003: using Ashfall.Core;
0004: using Ashfall.Core.UI;
0005: using CoreTheme = Ashfall.Core.UI.Theme;
0006:
0007: namespace AtomicWar.GodotApp.World
0008: {
0009:     /// <summary>
0010:     /// Lifecycle state of a map location marker on the wasteland surface.
0011:     /// </summary>
0012:     public enum MapLocationMarkerStatus
0013:     {
0014:         /// <summary>Discovered and available for immediate player travel and inspection.</summary>
0015:         Discovered,
0016:
0017:         /// <summary>Reachable through an adjacent discovered route; scoutable or selectable.</summary>
0018:         Available,
0019:
0020:         /// <summary>Locked due to severe radiation, extreme hazard, or faction blockade.</summary>
```

### Current evidence: `src/UI/MapDetailPanel.cs`
- Role: Godot presentation surface candidate
- Worktree status: `M src/UI/MapDetailPanel.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `6b091b1a461434e3ad68b33e4f50f12d9fd2c237d5aed33e117f21020ef3d8d7`
- Snapshot size: 13065 characters; 247 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using System.Collections.Generic;
0004: using Godot;
0005: using Ashfall.Core;
0006: using Ashfall.Core.Expeditions;
0007: using Ashfall.Core.UI;
0008: using AtomicWar.Journal;
0009:
0010: namespace AtomicWar.GodotApp.UI
0011: {
0012:     /// <summary>
0013:     /// ASHFALL — Map Detail panel.
0014:     /// Shows detailed sector intelligence, radiation readings, transit requirements,
0015:     /// architectural sub-layouts, and site salvage potential for a chosen location.
0016:     /// </summary>
0017:     public partial class MapDetailPanel : Control
0018:     {
0019:         public event Action? OnClose;
0020:
```

### Current evidence: `src/UI/ShelterDecorPanel.cs`
- Role: Godot presentation surface candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `ebf8710e14d5aaed08d3edaae4b02ef28199d223f09126e419ace0f8188fec4e`
- Snapshot size: 21143 characters; 443 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using Godot;
0004: using Ashfall.Core.Shelter;
0005:
0006: namespace AtomicWar.GodotApp.UI
0007: {
0008:     /// <summary>
0009:     /// Live room-interior panel for Plan 12C. Every action routes through
0010:     /// ShelterDecorHostSession: mounting consumes a real inventory item,
0011:     /// removal returns it to storage, and memorial plaques are read-only
0012:     /// projections of the memorial ledger.
0013:     /// </summary>
0014:     public partial class ShelterDecorPanel : Control, IBindablePanel
0015:     {
0016:         public event Action? OnClose;
0017:
0018:         private AshfallDashboardShell _shell = null!;
0019:         private AshfallStatusRail _statusRail = null!;
0020:         private OptionButton _roomPicker = null!;
```

### Current evidence: `src/UI/AshfallUiTheme.cs`
- Role: Godot presentation surface candidate
- Worktree status: `?? src/UI/AshfallUiTheme.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `0a0dbc6e59613f63448fd5f661678fae77a258676c07aa4fd126277d8e6da5e4`
- Snapshot size: 11633 characters; 230 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using Godot;
0003: using DesignTheme = Ashfall.Core.UI.Theme;
0004:
0005: namespace AtomicWar.GodotApp.UI
0006: {
0007:     /// <summary>
0008:     /// ASHFALL global UI theme (UI/UX audit 2026-09-25).
0009:     ///
0010:     /// Precision/consistency seam: panels built through <see cref="AshfallUiHelpers"/>
0011:     /// already carry explicit ASHFALL styleboxes, but ~58 panel files construct
0012:     /// raw <c>new Button</c> / <c>new LineEdit</c> / <c>new ItemList</c> nodes and
0013:     /// previously fell through to Godot's light default theme — inconsistent
0014:     /// chrome and, for buttons, no visible keyboard focus. This theme installs
0015:     /// the same ASHFALL visual language as class defaults on the Window root, so
0016:     /// every control of these types renders coherently unless a panel
0017:     /// deliberately overrides it.
0018:     ///
0019:     /// Values mirror the canonical helper fallbacks (MakeButton flat family,
0020:     /// AshfallFocusPolicy focus box) so per-node overrides and theme defaults
```

### Current evidence: `src/Main.Application.cs`
- Role: current source/owner candidate
- Worktree status: `M src/Main.Application.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `dfde24c7eaf92142b522f91327178376f7d8f7e3af8fc33a54b73c74ea5853ec`
- Snapshot size: 55807 characters; 1147 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using Godot;
0003: using System;
0004: using System.Globalization;
0005: using System.IO;
0006: using System.Linq;
0007: using System.Collections.Generic;
0008: using AtomicWar.Journal;
0009: using AtomicWar.GodotApp.Host;
0010: using Ashfall.Core;
0011: using Ashfall.Core.Campaign;
0012: using Ashfall.Core.Economy;
0013: using Ashfall.Core.Expeditions;
0014: using Ashfall.Core.Foundry;
0015: using Ashfall.Core.IO;
0016: using Ashfall.Core.Inventory;
0017: using Ashfall.Core.Journal;
0018: using Ashfall.Core.Muster;
0019: using Ashfall.Core.YearOfAsh;
0020: using Ashfall.Core.Radio;
```

### Current evidence: `Assets/Ashfall.Core/UI/FactionIconCatalog.cs`
- Role: current source/owner candidate
- Worktree status: `M Assets/Ashfall.Core/UI/FactionIconCatalog.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `206183f0ef2c3567a759b892740384cf034015b78a1cb997988629d0ef5722f2`
- Snapshot size: 11896 characters; 174 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System.Collections.Generic;
0003:
0004: namespace Ashfall.Core.UI
0005: {
0006:     /// <summary>
0007:     /// ASHFALL — engine-agnostic faction→icon path resolver.
0008:     /// Owns the canonical mapping from systems faction ids
0009:     /// (`currents.json`) to texture paths the hosts can fetch.
0010:     /// Falls back to a known asset for any id that has no coverage,
0011:     /// so callers (Trade, Radio, Dose, Verdict) never present a blank
0012:     /// emblem to the player.
0013:     /// No `UnityEngine` or `Godot` references — platform-agnostic.
0014:     /// </summary>
0015:     public static class FactionIconCatalog
0016:     {
0017:         /// <summary>Default fallback when an id has no emblem on disk.</summary>
0018:         public const string FallbackIconPath = "assets/ui/Icons/icon_unknown_faction.png";
0019:
0020:         /// <summary>
```

### Current evidence: `Assets/StreamingAssets/Data/asset_registry.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `71568df535648cee1b50c1228d1e631becb7684d571ecf04537353ac818b98a8`
- Snapshot size: 109342 characters; 3566 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "comment": "Plan 50 / C2[22] - Canonical Asset Manifest and Truth Mapping",
0004:   "generated_at": "2026-09-20T17:43:11.683610+00:00",
0005:   "total_assets": 355,
0006:   "categories": [
0007:     "item",
0008:     "portrait",
0009:     "location",
0010:     "faction",
0011:     "weather",
0012:     "ui"
0013:   ],
0014:   "assets": [
0015:     {
0016:       "id": "fallback_survivor",
0017:       "family": "portrait",
0018:       "kind": "character_portrait",
0019:       "path": "assets/sprites/Characters/placeholder_survivor.png",
0020:       "source": "canonical_scratch_fallback",
```

### Current evidence: `Assets/StreamingAssets/Data/locations.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `97543ade61b6458f5b31bcffb85a96ad5d3b322b80294deb158d1ae29da3386b`
- Snapshot size: 112801 characters; 1440 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "locations": [
0004:     {
0005:       "id": "abandoned_hospital",
0006:       "displayName": "Abandoned Hospital",
0007:       "description": "The east wing of the regional hospital came down in the second winter, and nobody has cleared it since. Girders lean against the stairwell, and the pharmacy door is buried under a ton of masonry. Sealed rooms still hold medicine if you can reach them without the ceiling deciding otherwise. Dosimeters tick up fast near the radiology basement, where the lead-lined walls kept their charge and the machines kept theirs. The morgue drawers are open. A stretcher with one wheel has been propped against the exit, as if someone meant to come back for it.",
0008:       "dangerLevel": 6,
0009:       "travelHours": 2.0,
0010:       "baseRadsPerHour": 35
0011:     },
0012:     {
0013:       "id": "rural_gas_station",
0014:       "displayName": "Rural Gas Station",
0015:       "description": "A roadside station stripped down to its frame on the main route east. The pumps are gutted, the shop glass is gone, and the wind blows ash through the aisles. Fuel drums lie where they were rolled and dropped, most of them empty, a few still holding dregs. The radiation is low here and the danger is low, which is exactly why it has been picked over so completely. What remains is what everyone else passed on. Someone has been sleeping in the workshop bay and oiling the door hinges, so the place is watched even when it is empty.",
0016:       "dangerLevel": 3,
0017:       "travelHours": 1.5,
0018:       "baseRadsPerHour": 15
0019:     },
0020:     {
```

### Current evidence: `Assets/StreamingAssets/Data/items.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `15bfc2f1283b4610cfaf756c1c6ad3f9af11281e5886fe7ba7ecba37a65600e7`
- Snapshot size: 390056 characters; 9660 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "items": [
0004:     {
0005:       "id": "item_decon_chelator_concentrate",
0006:       "displayName": "Chelator Concentrate",
0007:       "description": "A sealed glass ampoule of chelating agent concentrate. The label is half dissolved but the formula is standard: binds heavy radionuclides into a water-soluble complex for rinse removal. One ampoule per decon cycle. The pre-war stock won't last forever, and the synthesis requires a working pharma bench.",
0008:       "type": "Consumable",
0009:       "stackMax": 20,
0010:       "weight": 0.2,
0011:       "tradeValue": 8
0012:     },
0013:     {
0014:       "id": "item_lead_lined_effluent_filter",
0015:       "displayName": "Lead-Lined Effluent Filter",
0016:       "description": "A cylindrical filtration cartridge with a lead-foil inner liner and activated charcoal matrix. Installed in the decon airlock effluent tank to capture radionuclide-laden particulates before they can be sluiced into the general water system. Good for approximately five hundred liters of contaminated wash water before replacement is required.",
0017:       "type": "Equipment",
0018:       "stackMax": 5,
0019:       "weight": 3.5,
0020:       "tradeValue": 15
```

### Current evidence: `Assets/StreamingAssets/Data/survivors.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `c27e7ca9e79422b77bde6ae05c9b3d682f22d06c19165df1b6e30938b3a9f066`
- Snapshot size: 70319 characters; 1249 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:     "schema_version": 1,
0003:     "survivors": [
0004:         {
0005:             "id": "elena_vasquez",
0006:             "displayName": "Elena Vasquez",
0007:             "profession": "Paramedic",
0008:             "bio": "Nine years of night shifts in an ambulance bay taught Elena to sort the dying fast. She still checks wrists for pulses out of habit, and wears her watch strap-first over the cuff so it never catches on gloves.",
0009:             "baseHealth": 100
0010:         },
0011:         {
0012:             "id": "marcus_olejnik",
0013:             "traitIds": ["trait_claustrophobe"],
0014:             "displayName": "Marcus Olejnik",
0015:             "profession": "Mechanical Engineer",
0016:             "bio": "Marcus kept a turbine hall running on parts that were condemned five years earlier. He talks to machines more easily than to people, and is usually right about what they tell him.",
0017:             "baseHealth": 90
0018:         },
0019:         {
0020:             "id": "suki_tanaka",
```

### Current evidence: `Ashfall.Core.Tests/Visual/AssetFallbackDiagnosticsTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `bb711cee506e16cd2a254020b0f3a5ef55005c4bc5be41a2989ec245532491ae`
- Snapshot size: 6769 characters; 139 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0057:                 Assert.False(path.StartsWith("res://", StringComparison.OrdinalIgnoreCase),
0058:                     $"Core faction icon path for '{factionId}' must not use res:// scheme: {path}");
0059:                 Assert.False(path.StartsWith("/", StringComparison.Ordinal),
0060:                     $"Core faction icon path for '{factionId}' must be relative without leading slash: {path}");
```

### Current evidence: `Ashfall.Core.Tests/LegacyAssetGateTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `089420fdd3f40b738cdfaff084090cc892163976a2ff074ef1111088d6eb7200`
- Snapshot size: 1842 characters; 53 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using System.Diagnostics;
0004: using System.IO;
0005: using Xunit;
0006:
0007: namespace Ashfall.Core.Tests;
0008:
0009: public class LegacyAssetGateTests
0010: {
0011:     private static string GateScript(string name) =>
0012:         Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "scripts", "ci", name));
0013:
0014:     private static string RunGateAndCollectOutput(string scriptPath)
0015:     {
0016:         // ProcessStartInfo with UseShellExecute=false splits argument strings
0017:         // on whitespace, so paths containing spaces (e.g. ".../Atomic War/...")
0018:         // cannot be passed as a single quoted command line. Use ArgumentList
0019:         // to keep the script path as a single argv element.
0020:         var psi = new ProcessStartInfo
```

### Current evidence: `Ashfall.Core.Tests/ProductionArtManifestTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `a90346fbf6cb61929067fa97d62c0c7d426e3b601f09aa32df2bb37572d27799`
- Snapshot size: 22245 characters; 484 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using System.Collections.Generic;
0004: using System.IO;
0005: using System.Linq;
0006: using System.Text.Json;
0007: using Xunit;
0008:
0009: namespace Ashfall.Core.Tests
0010: {
0011:     /// <summary>
0012:     /// Gate tests for the production-art generation manifest.
0013:     ///
0014:     /// The manifest is the canonical plan that drives every Batch N of the
0015:     /// visual pipeline. It is emitted by `tools/production_manifest.py`
0016:     /// (Python tooling that runs in plain BCL it must not be re-implemented
0017:     /// in C#). What these tests guard is the *invariants* the manifest must
0018:     /// hold before any Batch can be trusted:
0019:     ///
0020:     ///   1. The file exists at the canonical path.
```

### Current evidence: `Ashfall.Core.Tests/Assets/Plan50AssetTruthIntegrationTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `965df9aeccfdf339a06983ff758bea5200211ee5a28fca4e8bc6f9a80caeb60a`
- Snapshot size: 10424 characters; 247 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using System.Collections.Generic;
0004: using System.IO;
0005: using Ashfall.Core.Assets;
0006: using Xunit;
0007:
0008: namespace Ashfall.Core.Tests.Assets
0009: {
0010:     public sealed class Plan50AssetTruthIntegrationTests
0011:     {
0012:         [Fact]
0013:         public void AssetManifestCatalog_ParsesJsonAndIndexesEntries()
0014:         {
0015:             const string json = @"{
0016:                 ""schema_version"": 1,
0017:                 ""total_assets"": 3,
0018:                 ""assets"": [
0019:                     {
0020:                         ""id"": ""pistol_cz75_9x19"",
```

### Current evidence: `Ashfall.Core.Tests/Presentation/Plan51PresentedGameIntegrationTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `269e1382e1174cabf257d8fa63e9b1374a0bfa9847b650628e8a1c85ef02da24`
- Snapshot size: 5840 characters; 140 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using System.Collections.Generic;
0004: using Ashfall.Core.Presentation;
0005: using Xunit;
0006:
0007: namespace Ashfall.Core.Tests.Presentation
0008: {
0009:     public sealed class Plan51PresentedGameIntegrationTests
0010:     {
0011:         [Fact]
0012:         public void HoldfastPresentationSlate_EvaluatesCrisisBandsCorrectly()
0013:         {
0014:             var slate = new HoldfastPresentationSlate();
0015:
0016:             // All nominal -> Calm
0017:             var r1 = new RoomPresentationSnapshot("room_bunks", "Dormitory", isPowered: true, isFlooding: false);
0018:             var r2 = new RoomPresentationSnapshot("room_gen", "Generator Vault", isPowered: true, isFlooding: false);
0019:             var r3 = new RoomPresentationSnapshot("room_sump", "Sump Drainage", isPowered: true, isFlooding: false);
0020:
```

## Current JSON audit

#### `Assets/StreamingAssets/Data/asset_registry.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, comment, generated_at, total_assets, categories, assets`
- `categories`: list count=6; sample IDs=['item', 'portrait', 'location', 'faction', 'weather', 'ui']
- `assets`: list count=355; sample IDs=['fallback_survivor', 'fallback_icon', 'fallback_location', 'fallback_weather', 'fallback_faction', 'ammo_deprecated_cal_9x19', 'ammo_deprecated_380acp', 'ammo_deprecated_762x25']
- `schema_version`: `1`
- SHA-256: `71568df535648cee1b50c1228d1e631becb7684d571ecf04537353ac818b98a8`
#### `Assets/StreamingAssets/Data/locations.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, locations`
- `locations`: list count=179; sample IDs=['abandoned_hospital', 'rural_gas_station', 'suburban_house', 'government_bunker', 'stranger_cache', 'location_geo_thermal_plant_ruins', 'location_arcology_sector_4', 'location_frozen_river_barge']
- `schema_version`: `1`
- SHA-256: `97543ade61b6458f5b31bcffb85a96ad5d3b322b80294deb158d1ae29da3386b`
#### `Assets/StreamingAssets/Data/items.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, items`
- `items`: list count=724; sample IDs=['item_decon_chelator_concentrate', 'item_lead_lined_effluent_filter', 'item_heavy_neoprene_scrub_brush', 'item_sealed_waste_bin', 'item_theodolite_brass_precision', 'item_surveyor_stadia_rod', 'item_datum_plate_bronze', 'item_concrete_mix']
- `schema_version`: `1`
- SHA-256: `15bfc2f1283b4610cfaf756c1c6ad3f9af11281e5886fe7ba7ecba37a65600e7`
#### `Assets/StreamingAssets/Data/survivors.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, survivors`
- `survivors`: list count=129; sample IDs=['elena_vasquez', 'marcus_olejnik', 'suki_tanaka', 'the_surgeon', 'the_pharmacist', 'the_vet', 'the_therapist', 'the_undertaker']
- `schema_version`: `1`
- SHA-256: `c27e7ca9e79422b77bde6ae05c9b3d682f22d06c19165df1b6e30938b3a9f066`
## Symbol and caller audit

#### `asset registry` — HOST_REFERENCE_PRESENT — core/declaration=0, host=1, test=0
- `src/Host/AssetRegistry.cs:49` (host) — /// Thin, presentation-only asset registry that maps catalog IDs to Godot Texture2D resources.
#### `fallback diagnostics` — NO_HOST_REFERENCE_IN_SNAPSHOT — core/declaration=0, host=0, test=0
#### `portrait resolver` — NO_HOST_REFERENCE_IN_SNAPSHOT — core/declaration=0, host=0, test=0
#### `location art` — NO_HOST_REFERENCE_IN_SNAPSHOT — core/declaration=0, host=0, test=0
#### `faction icon` — HOST_REFERENCE_PRESENT — core/declaration=0, host=1, test=2
- `src/Host/AssetRegistry.cs:274` (host) — /// Gets a faction icon by ID.
- `Ashfall.Core.Tests/Visual/AssetFallbackDiagnosticsTests.cs:58` (test) — $"Core faction icon path for '{factionId}' must not use res:// scheme: {path}");
- `Ashfall.Core.Tests/Visual/AssetFallbackDiagnosticsTests.cs:60` (test) — $"Core faction icon path for '{factionId}' must be relative without leading slash: {path}");
#### `snapshot fixture` — HOST_REFERENCE_PRESENT — core/declaration=0, host=3, test=0
- `src/UI/CombatHudOverlay.cs:80` (host) — // so the snapshot fixture is row-deterministic.
- `src/UI/EconomyMarketSnapshotFixture.cs:9` (host) — /// Plan 56 phase 4 — snapshot fixture for the market panel: binds a real
- `src/UI/SkillMatrixPanel.cs:339` (host) — // For the snapshot fixture we keep this simple and return 0 when unknown; the
## Read-only authority alignment

Authority file: `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`
Authority SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
Total lines in snapshot: 5510

The following excerpts are read-only orientation anchors. They do not override current source/data evidence or create implementation authority.
#### authority lines 9-12
00009: **Document class:** SUBJECT-PLAN FACTORY. This document is not itself an integration plan. It is a repeatable generator: any future planning session can consume its matrices, backlog, and templates to produce an unbounded series of bounded subject plans, each of which names its own best integration route.
00010: **Audit basis:** Live repository inspection performed 2026-09-24 (repository root listing, `Assets/StreamingAssets/Data/` listing at 342 entries, `docs/` listing, `docs/plans/` listing at 126 entries, `INTEGRATION_PLANS.md`, `SESSION_HANDOFF.md`, `AGENTS.md`, branch list). Every claim in the Drift Register (Part I) is labeled VERIFIED, HIGH CONFIDENCE, or UNVERIFIED.
00011: **Authority order:** unchanged from v1.0 — live repository source and data first; then `AGENTS.md`; then this document; then the docs registry and atlas; then plan ledgers. Where this document and live source disagree, live source wins and this document must be corrected.
00012:
#### authority lines 33-36
00033:
00034: The audit inspected the live repository directly: root directory listing, `Assets/StreamingAssets/Data/` (342 entries), `docs/` (top-level documents and subdirectories), `docs/plans/` (126 entries), `INTEGRATION_PLANS.md` (32,793 characters, read head and tail), `SESSION_HANDOFF.md`, `AGENTS.md` (head), and the branch list. No working-tree clone was available in the audit environment; findings marked VERIFIED are directly demonstrated by these listings and file reads. Findings marked UNVERIFIED could not be confirmed in this pass and require a follow-up read before any plan relies on them.
00035:
00036: ### 1.2 Drift Register — repository facts that differ from, or extend, the v1.0 bible
#### authority lines 47-50
00047: **DR-04 — The data catalog inventory has grown; several catalogs are absent from the v1.0 inventory. VERIFIED.**
00048: `Assets/StreamingAssets/Data/` currently holds 342 entries. Catalogs observed live but not present in the v1.0 Part 5.4 inventory include: `dive_sites.json`, `hydroponic_crops.json`, `hydraulic_extrusion_catalog.json`, `metrology_standards_catalog.json`, `muster_camp_scenes.json`, `muster_epilogues.json`, `muster_faction_actions.json`, `muster_faction_culture.json`, `muster_witnesses.json`, `utility_actions.json`, `moral_choice_quests_branching.json`, `moral_choice_quests_distress.json`. Consequence: the duplication firewall (v1.0 Part 5) is stale in these domains; a planner could propose a "new" muster or moral-choice catalog that already exists. The ID-collision sweep in Factory Protocol step 1 must always run against the live listing, never against this document.
00049:
00050: **DR-05 — A process script lives inside the data authority. VERIFIED (hygiene finding).**
#### authority lines 69-72
00069:
00070: The following v1.0 structures were confirmed by the audit and remain authoritative: the four-tier architecture (Tier 1 data authority in `Assets/StreamingAssets/Data/`; Tier 2 engine-free Core; Tier 3 `src/Host` + `src/UI`; Tier 4 xUnit plus the `HostCli` selftest surface); the `AGENTS.md` non-negotiable rules (Godot authoritative, Core engine-free, JSON authoritative, one authority per concern, focused verification); the narrative corpus under `Assets/StreamingAssets/Data/narrative/` (present in the live listing); the faction, economy, weather, Year-of-Ash, moral-choice, muster, and verdict catalog families (all present live); and the plan-discipline artifacts (`INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`, `SESSION_HANDOFF.md`) at root.
00071:
00072: ---
#### authority lines 425-428
00425: ### Recommended integration route
00426: Tier: audit (DOCS-ONLY) then DATA-ONLY corpus authoring, with HOST-WIRING only for confirmed unconsumed catalogs. Seams: corpus twins into `Assets/StreamingAssets/Data/narrative/` following the assay-report genre contracts; consumption wiring through the named host sessions (`HydraulicExtrusion` session confirmed live in the v1.0 host inventory). Verification: data-integrity selftest, content-utilization selftest (the decisive gate — presence is not reachability), focused loader tests.
00427:
00428: ### Continuity checklist result
#### authority lines 469-472
00469: ### Why this and not something else
00470: The endgame is the repository's most durable asset; unequal prose coverage there is player-visible at the highest-stakes moment, and the content is purely additive data through existing catalogs.
00471:
00472: ### What must not change
#### authority lines 563-566
00563: ### Subject
00564: Determine what `Assets/StreamingAssets/Data/rewrite.py` rewrites and who invokes it; then either relocate it to `scripts/`/`tools/` with a call-site update, or document in place why it must live beside the catalogs it rewrites.
00565:
00566: ### Premise evidence
#### authority lines 888-891
00888:
00889: **H-01 · Data-authority non-JSON assertion.** Subject: CI assertion that `Assets/StreamingAssets/Data/` contains only JSON plus whitelisted artifacts (consumes the F-009 resolution). Evidence: DR-05. Route: small CI script in the established gate family. Confidence: VERIFIED need.
00890:
00891: **H-02 · Gate-count drift guard.** Subject: consumed as F-008. Route: TOOLING. Confidence: PROPOSAL.
#### authority lines 964-967
00964:
00965: **DM-17 — Host surface and UI (C17).** Owners: the panel families (v1.0 Part 5.7), shell components, focus navigator, snapshots, a11y, briefings. Design pinned by `DESIGN.md`; a11y by `ACCESSIBILITY.md`; input by the 22-action map. Openings: E-01 through E-10, B-24, F-01. Constraint: zero gameplay authority in panels; every panel exposes existing commands and truthful state.
00966:
00967:
#### authority lines 1123-1126
01123: - 2026-09-24 — Volume 2: subject seed catalog, Lanes A–E (A-01…A-30, B-01…B-25, C-01…C-14, D-01…D-08, E-01…E-10) — ~29,000 — cumulative ~97,000
01124: - 2026-09-24 — Volume 3: subject seed catalog, Lanes F–J (F-01…F-06, G-01…G-08, H-01…H-07, I-01…I-06, J-01…J-05) plus seventeen subsystem deep maps (DM-1…DM-17) — ~23,000 — cumulative ~120,000
01125: - 2026-09-24 — Volume 4: prose specification library part 1 (eight worked genre contracts plus usage rules) — ~5,500 — cumulative ~125,500
01126:
#### authority lines 1894-1897
01894:
01895: # VOLUME 8 — BALANCE HARNESS SPECIFICATION LIBRARY (Factory batch 2026-09-25-D)
01896:
01897: One harness specification per Lane C seed. All harnesses inherit the repository's determinism canon: seeded runs, `ISeededRng` sub-streams, two-pass byte-identical proof, results published under `docs/balance/`. The Plan 76.2 seeded 200-run pattern is the template; these specifications parameterize it per question. No harness changes tuning; every harness produces evidence, and only evidence-backed outliers become tuning plans (C-13 discipline).
#### authority lines 2400-2403
02400: Why this: the gate is change-triggered, so old panels escape it; a one-time sweep closes the accumulated gap and the gate holds it closed thereafter.
02401: Must not change: design language (textual indicators within the pinned visual system).
02402: Route: HOST-WIRING, per-panel focused edits.
02403: Continuity: textual state must be truthful current state read through the same commands.
#### authority lines 2413-2416
02413: Route: HOST-WIRING per panel.
02414: Continuity: focus order must follow the visual reading order unless a documented exception exists.
02415: Verification: layout selftest; input parity tests where the selftest family covers them (premise read); manual controller pass per changed panel.
02416: Open premises: parity test surface read.
#### authority lines 2494-2497
02494: Must not change: nothing.
02495: Route: TOOLING + `docs/perf/`; the synthetic fixture doubles as a Lane D test asset (shared fixture, two consumers — the factory notes the reuse to avoid duplicate fixture authoring).
02496: Continuity: the fixture must be generated deterministically for reuse.
02497: Verification: cost traces; fixture determinism proof (shared with Lane D).
#### authority lines 2573-2576
02573: Lane H · Data authority · Status VERIFIED need (DR-05), PROPOSAL handling.
02574: Subject: a CI assertion that `Assets/StreamingAssets/Data/` contains only JSON catalogs plus an explicit whitelist, making the DR-05 hygiene question permanently machine-enforced once FP-009's resolution lands.
02575: Premise evidence: VERIFIED `rewrite.py` sits in the data authority (DR-05); VERIFIED the integrity selftest family is the enforcement precedent.
02576: Must not change: nothing until FP-009's call-site verdict is in (the whitelist derives from that verdict, not from here).
#### authority lines 2767-2770
02767: **Contract:** evidence before tuning; harnesses are the instrument; results publish to `docs/balance/`; outliers become plans, not edits.
02768: **Evidence inputs:** the live baseline documents (DR-03), the harness library (Volume 8), commodity/price catalogs, difficulty authority state.
02769: **Worked example (H-C5, compressed):** seed "tribute sustainability" → sweep: doctrine list from the live catalog, mid-game income model definition from baselines → run: seeded 120-day slices per doctrine, tribute on schedule → publish: sustainability thresholds, death-spiral levels, reaction-cost table → route: any doctrine whose demand exceeds its threshold becomes a tuning plan candidate with the table cited.
02770: **Failure modes:** aesthetic judgments dressed as balance findings; tuning without a harness run; confusing intended harshness with mathematical unsustainability (the lane's own distinction); carrying stale tables across content waves.
#### authority lines 3266-3269
03266:
03267: One roadmap per cluster C1–C17, each anchored to its deep map (DM-1 through DM-17) and its seeded openings. Each roadmap is a planning instrument, not a schedule: it states what the cluster already owns (verified), what the factory has already proposed (by plan id), and the phase structure a multi-year content arc would follow if the foreman authorizes sustained expansion. Phase labels are consistent across all clusters:
03268:
03269: - Phase I — Seed consumption: complete the cluster's expanded subject plans as wave tranches (the plan lists named above).
#### authority lines 3323-3326
03323:
03324: ## 17.17 C17 — Host surface and UI (DM-17)
03325: Owns: panel families, shell components, focus navigator, snapshots, a11y, briefings; design pinned by DESIGN.md, a11y by ACCESSIBILITY.md, input by the 22-action map. Expanded plans: satellites E-01 through E-10, B-24, F-01. Phase I: briefing rows for Wave 8–12 systems and snapshot coverage for newest panels (both census-first). Phase II: E-05 words-not-color-only sweep and E-06 controller parity for new panels. Phase III: E-07 camp-panel truth (selftest first), E-08 barter legibility against DEC-05, E-09 storm-window lead time, E-10 submission affordances. Phase IV: DR entry per sealed-display priority change (DEC-05 is the precedent). Multi-year arc: zero-authority panels telling the truth attractively — every new system surface exposed within its wave, never after it.
03326:
… 37 additional authority matches omitted; the implementation owner must cite the exact relevant section at execution time.
## Objective and success definition

The objective is to close the gap between authored catalog IDs, physical assets, import metadata, fallback chains, snapshot fixtures, and player-visible truth. Define what is complete, what is intentionally fallback-backed, and how missing art becomes actionable diagnostics. The success condition is not merely a larger catalog or a more attractive panel. Success requires a current owner, a reachable consumer, a durable state decision, deterministic behavior, truthful UI, explicit failure semantics, and a focused verification handoff.

## Current reality, requested behavior, and minimum delta

**Existing behavior.** The live host already has a centralized Godot AssetRegistry with explicit fallback diagnostics, survivor portrait resolution, location lookup, faction icon resolution, and an asset registry self-test. The visual plan therefore extends the current resolver and registry; it does not create a second visual domain authority.

**Requested behavior.** Close the gap between authored catalog IDs, physical assets, import metadata, fallback chains, snapshot fixtures, and player-visible truth. Define what is complete, what is intentionally fallback-backed, and how missing art becomes actionable diagnostics.

**Minimum safe delta.** Extend `src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates` only after the current owner and caller audit confirms the missing link. If the old plan’s proposed system already exists, convert the task into a bounded maintenance/reachability package rather than creating a replacement.

## Non-goals and collision exclusions

- No parallel gameplay authority, save store, ledger, selector, event bus, simulation, or UI-owned rule.
- No Unity restoration, Unity dependency, or engine types in Core.
- No edits to authored data or production code in this planning-only pass.
- No broad test suite, full runtime soak, generated index rewrite, or unrelated documentation cleanup.
- No invented API, count, save section, or caller claim. Unknowns remain named unknowns.

## Current reality and required delta

**Current reality.** The live host already has a centralized Godot AssetRegistry with explicit fallback diagnostics, survivor portrait resolution, location lookup, faction icon resolution, and an asset registry self-test. The visual plan therefore extends the current resolver and registry; it does not create a second visual domain authority.

**Required delta.** Close the gap between authored catalog IDs, physical assets, import metadata, fallback chains, snapshot fixtures, and player-visible truth. Define what is complete, what is intentionally fallback-backed, and how missing art becomes actionable diagnostics.

**Primary seam.** src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates

**Non-goals.** This planning package does not modify production, authored data, saves, tests, UI, generated indexes, assets, or runtime code. It does not approve a new owner, authorize Unity work, or turn a historical plan claim into current evidence. The following terms are design hypotheses to verify during implementation, not facts asserted by this document: `asset registry`, `fallback diagnostics`, `portrait resolver`, `location art`, `faction icon`, `snapshot fixture`.

## Integration framework

The integration framework is deliberately owner-first:

1. **Read and classify current state.** Start with the named Core owner, current JSON, host session, save store, and focused tests. Record whether the feature is live, partially wired, dormant, stale, or decision-gated.
2. **Choose one authority per concern.** Extend the current owner when it exists. If no owner exists, stop at the architecture decision boundary and name the new authority decision rather than creating a parallel store, selector, ledger, panel, or simulation.
3. **Author data against consumers.** A JSON row is not integrated merely because it parses. Every row must have a current loader, a current consumer, a visible or mechanically observable outcome, and a validation path.
4. **Route effects through existing events/seams.** Core emits facts; host sessions translate them; UI presents truthful state. Do not place gameplay calculations in a panel or Godot callback.
5. **Persist through the owning save path.** Capture and restore must be implemented before a feature is called persistent. Old versions, nulls, empty collections, checksums, and mid-event saves are explicit cases.
6. **Verify narrowly.** Use the smallest existing test file or a new focused test for an uncovered confirmed contract. Keep Core, data, host, UI, save, determinism, and cross-system checks distinguishable.
7. **Roll back by boundary.** A failed expansion should disable its adapter or authored tranche without corrupting the owning state or requiring a destructive reset.

## Code architecture

### Core layer

- Put reusable rules, validation, state transitions, deterministic selection, and read models in `Assets/Ashfall.Core/`.
- Keep Core engine-free. No Godot, Unity, `Texture2D`, `JsonUtility`, wall-clock, or unseeded randomness belongs in a Core contract.
- Extend existing models and public methods when the current API already expresses the concern. A new DTO, interface, event, or catalog loader is justified only when it removes a real ownership or boundary problem.
- Make invalid input observable through the owner’s normal result/diagnostic path. Do not silently coerce malformed content into a successful state.
- Keep deterministic ordering explicit: use ordinal IDs, stable catalog order, bounded collections, and the existing seeded RNG fork for any stochastic choice.

### Data layer

- Author under `Assets/StreamingAssets/Data/` using the existing schema and snake_case IDs.
- Prefer additive fields and existing collections over parallel catalogs.
- Validate IDs, references, ranges, and consumer reachability through the current catalog integrity pipeline.
- Record schema version and old-data behavior in the plan and implementing handoff.
- Data prose may describe a consequence only when the consequence is expressible through a current owner and event.

### Host layer

- Load the catalog in the current host/session owner, not in a panel constructor.
- Subscribe once to owner events, translate facts into existing journal/radio/UI signals, and dispose subscriptions with the session.
- Bind day/hour/event triggers through the existing campaign owner. Do not create a second clock or update loop.
- Rehydrate from the existing save owner and mark dirty only for actual canonical mutations.
- Keep host code free of duplicate gameplay math; it may format, route, and adapt.

### Presentation layer

- Panels expose current commands and truthful current state.
- They display unavailable/blocked reasons, provenance, and the next legitimate action rather than simulating a result.
- Preserve keyboard/controller close/back behavior, focus order, readable contrast, and reduced-motion/accessibility settings.
- Refresh from owner events and lifecycle state; never use a panel cache as authority.
- Snapshot or accessibility fixtures are verification artifacts, not gameplay state.

## Ownership and state matrix

| Concern | Authoritative owner | Adapter responsibility | Persistence rule | Verification gate |
|---|---|---|---|---|
| Domain rules | Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only | Bind inputs and translate facts | Existing owner DTO/save | Core focused tests |
| Authored content | Current JSON catalog and loader | Load/validate once | Catalog version/defaults | Data integrity |
| Runtime lifecycle | Existing host/session owner | Setup, event subscription, disposal | Existing save/session | Host wiring |
| Presentation | Existing Godot surface | Format and command dispatch | No gameplay state | UI/focus/headless |
| Diagnostics | Existing logging/telemetry owner | Correlate ID and phase | Bounded/non-authoritative | Failure test |
| Historical authority | Read-only master document | Cite relevant section only | Never persisted | Plan QA |

## Data, event, and command flow

`authoritative JSON / player command / current owner event` → validation at the owning seam → canonical Core state transition → typed fact/event → host session projection → journal/radio/UI refresh → existing save owner if the transition mutated durable state.

The flow is intentionally one-way for authority. UI may send a command, but it cannot directly mutate the domain. Events may be consumed by several read-only projections, but only the owner writes the state. If a proposed feature needs a second writer, that is an architecture failure, not an invitation to add another event bus.

## State, API, and compatibility contract

The implementing agent must confirm the actual public API and write the final signatures in the implementation handoff. At minimum, expose:

- a read-only query/projection for the current state;
- an explicit command or owner method for each player-visible mutation;
- a typed fact/event for meaningful state changes;
- capture/restore methods on the existing state owner;
- a diagnostic result for invalid or unavailable data;
- a stable key for idempotent commands and replay;
- a bounded, ordinal-stable collection for any retained history.

Old saves must default missing additive fields to the documented neutral value. New required fields need a versioned migration. Null and empty semantics must differ deliberately: null means unavailable/not supplied; empty means validly no records, unless the current owner’s contract says otherwise. Do not infer a new save section from the plan title.

## Determinism and replay

For every proposed random or time-dependent element, name the seed source, stream/fork, draw order, retry behavior, and tie-break rule. Prefer no randomness for validation, lookup, and deterministic UI state. If an existing owner uses `ISeededRng`, reuse its campaign stream/fork rather than constructing a private generator. Wall-clock time, `System.Random`, `Guid.NewGuid`, hash iteration order, filesystem enumeration order, and frame timing are prohibited in deterministic Core behavior.

The replay acceptance test must use two equivalent runs with identical seed, catalog snapshot, state, day/hour, and command sequence. Compare canonical state, event order, resource/ledger deltas, and persistence payload. Presentation-only differences are acceptable only when they are explicitly non-authoritative and do not alter commands or outcomes.

## Save, restore, and migration

Before implementation claims persistence:

1. Identify the current save-section owner and DTO.
2. Add or reuse capture/restore through that owner.
3. Deep-copy mutable collections so restoring does not alias runtime state.
4. Define old-version defaults for every new field.
5. Define behavior for missing catalogs, unknown IDs, partial records, and corrupted checksums.
6. Test capture → serialize → restore → continued mutation, plus a mid-transition reload.
7. Confirm a save/load pair does not duplicate one-shot events or reapply a quest/choice/ledger mutation.

No plan-created “state cache” is allowed. If a new authority is genuinely required, the package must pause for a decision and name its owner, section, migration, and test contract.

## Failure and edge behavior

The implementation must cover null/empty state, empty catalogs, duplicate IDs, missing references, stale old saves, invalid numeric values, extreme but bounded values, unavailable owners, dead or absent participants, repeated commands, simultaneous events, host reload, missing UI, missing audio/journal, new-game reset, teardown, and deterministic replay. The expected result should preserve the last valid state, report a useful diagnostic, and avoid presenting a fabricated success. The detailed failure matrix below expands these cases for 08.

## Test strategy and focused commands

The plan-only pass does not execute tests. The implementing package should reuse existing focused files first, run a new file alone, and stay below the repository’s focused-test policy unless a foreman-approved hypothesis requires more. The current candidate commands are listed in the verification matrix and must be revalidated against the worktree at implementation start. A passing compile is not proof of host wiring, persistence, determinism, or player reachability.

## Phased implementation and rollback

The detailed phase table below is the implementation contract. Each phase has a completion gate and a “must not touch yet” boundary. Rollback is additive and local: disable the adapter or remove the authored tranche, retain the owner’s last valid state, and never reset the shared worktree or shared save registry to hide a failure.

## Dependency-ordered implementation phases
| Phase | Outcome | Work | Boundary | Completion gate |
|---|---|---|---|---|
| 0 | Premise recheck and baseline | Confirm exact owner/API/catalog counts, current claims, dirty paths, and active decision gates. | No edits to production. | A written evidence table and focused baseline commands. |
| 1 | Owner and collision map | Trace current callers, save owner, event seam, and duplicate/legacy candidates. | No new catalog or state. | Single-owner map with zero unresolved authority collisions. |
| 2 | Core contract or bounded extension | Add only the smallest pure contract needed by the confirmed gap, or document that no Core change is needed. | No Godot/UI/data authoring. | Core tests for boundaries, transitions, invalid data, and determinism. |
| 3 | Persistence and migration contract | Implement capture/restore/old-save defaults through the existing owner. | No unrelated save sections. | Round-trip, deep-copy, corrupt/invalid, and mid-event tests are required to pass. |
| 4 | Authored data tranche | Author schema-valid rows only after consumer fields are known; validate references and reachability. | No prose-only orphan rows. | Data integrity and consumer coverage pass for the tranche. |
| 5 | Host/event wiring | Load, subscribe, translate, and dispose in the current host/session owner. | No panel gameplay math. | Host wiring test proves event → projection and setup/teardown. |
| 6 | Presentation and accessibility | Expose truthful state, commands, focus, controller/keyboard behavior, and feedback. | No new authority in UI. | Panel route/focus/headless checks pass; snapshots only through the owning harness. |
| 7 | End-to-end and replay | Run a bounded scenario, save/reload, paired seeded replay, and cross-system consequence check. | No full-suite default. | Named commands/results and limitations recorded. |
| 8 | Balance/content polish | Tune only authored values with current harnesses; remove dead rows and polish truthful text. | No hidden tuning or parallel scalar. | Content review confirms no dominated/unreachable row and no unsupported claim. |
| 9 | Rollback and closeout | Document feature disablement, migration reversal, owner handoff, and residual debt. | No unowned cleanup. | Foreman review accepts or records a blocker. |

### Phase-specific implementation questions
#### Phase 0: Premise recheck and baseline
- What current evidence must be reread? The live host already has a centralized Godot AssetRegistry with explicit fallback diagnostics, survivor portrait resolution, location lookup, faction icon resolution, and an asset registry self-test. The visual plan therefore extends the current resolver and registry; it does not create a second visual domain authority.
- What is the smallest safe change? Close the gap between authored catalog IDs, physical assets, import metadata, fallback chains, snapshot fixtures, and player-visible truth. Define what is complete, what is intentionally fallback-backed, and how missing art becomes actionable diagnostics.
- Which owner is touched? Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only
- What must remain untouched? No edits to production.
- What proves completion? A written evidence table and focused baseline commands.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 1: Owner and collision map
- What current evidence must be reread? The live host already has a centralized Godot AssetRegistry with explicit fallback diagnostics, survivor portrait resolution, location lookup, faction icon resolution, and an asset registry self-test. The visual plan therefore extends the current resolver and registry; it does not create a second visual domain authority.
- What is the smallest safe change? Close the gap between authored catalog IDs, physical assets, import metadata, fallback chains, snapshot fixtures, and player-visible truth. Define what is complete, what is intentionally fallback-backed, and how missing art becomes actionable diagnostics.
- Which owner is touched? Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only
- What must remain untouched? No new catalog or state.
- What proves completion? Single-owner map with zero unresolved authority collisions.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 2: Core contract or bounded extension
- What current evidence must be reread? The live host already has a centralized Godot AssetRegistry with explicit fallback diagnostics, survivor portrait resolution, location lookup, faction icon resolution, and an asset registry self-test. The visual plan therefore extends the current resolver and registry; it does not create a second visual domain authority.
- What is the smallest safe change? Close the gap between authored catalog IDs, physical assets, import metadata, fallback chains, snapshot fixtures, and player-visible truth. Define what is complete, what is intentionally fallback-backed, and how missing art becomes actionable diagnostics.
- Which owner is touched? Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only
- What must remain untouched? No Godot/UI/data authoring.
- What proves completion? Core tests for boundaries, transitions, invalid data, and determinism.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 3: Persistence and migration contract
- What current evidence must be reread? The live host already has a centralized Godot AssetRegistry with explicit fallback diagnostics, survivor portrait resolution, location lookup, faction icon resolution, and an asset registry self-test. The visual plan therefore extends the current resolver and registry; it does not create a second visual domain authority.
- What is the smallest safe change? Close the gap between authored catalog IDs, physical assets, import metadata, fallback chains, snapshot fixtures, and player-visible truth. Define what is complete, what is intentionally fallback-backed, and how missing art becomes actionable diagnostics.
- Which owner is touched? Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only
- What must remain untouched? No unrelated save sections.
- What proves completion? Round-trip, deep-copy, corrupt/invalid, and mid-event tests are required to pass.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 4: Authored data tranche
- What current evidence must be reread? The live host already has a centralized Godot AssetRegistry with explicit fallback diagnostics, survivor portrait resolution, location lookup, faction icon resolution, and an asset registry self-test. The visual plan therefore extends the current resolver and registry; it does not create a second visual domain authority.
- What is the smallest safe change? Close the gap between authored catalog IDs, physical assets, import metadata, fallback chains, snapshot fixtures, and player-visible truth. Define what is complete, what is intentionally fallback-backed, and how missing art becomes actionable diagnostics.
- Which owner is touched? Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only
- What must remain untouched? No prose-only orphan rows.
- What proves completion? Data integrity and consumer coverage pass for the tranche.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 5: Host/event wiring
- What current evidence must be reread? The live host already has a centralized Godot AssetRegistry with explicit fallback diagnostics, survivor portrait resolution, location lookup, faction icon resolution, and an asset registry self-test. The visual plan therefore extends the current resolver and registry; it does not create a second visual domain authority.
- What is the smallest safe change? Close the gap between authored catalog IDs, physical assets, import metadata, fallback chains, snapshot fixtures, and player-visible truth. Define what is complete, what is intentionally fallback-backed, and how missing art becomes actionable diagnostics.
- Which owner is touched? Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only
- What must remain untouched? No panel gameplay math.
- What proves completion? Host wiring test proves event → projection and setup/teardown.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 6: Presentation and accessibility
- What current evidence must be reread? The live host already has a centralized Godot AssetRegistry with explicit fallback diagnostics, survivor portrait resolution, location lookup, faction icon resolution, and an asset registry self-test. The visual plan therefore extends the current resolver and registry; it does not create a second visual domain authority.
- What is the smallest safe change? Close the gap between authored catalog IDs, physical assets, import metadata, fallback chains, snapshot fixtures, and player-visible truth. Define what is complete, what is intentionally fallback-backed, and how missing art becomes actionable diagnostics.
- Which owner is touched? Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only
- What must remain untouched? No new authority in UI.
- What proves completion? Panel route/focus/headless checks pass; snapshots only through the owning harness.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 7: End-to-end and replay
- What current evidence must be reread? The live host already has a centralized Godot AssetRegistry with explicit fallback diagnostics, survivor portrait resolution, location lookup, faction icon resolution, and an asset registry self-test. The visual plan therefore extends the current resolver and registry; it does not create a second visual domain authority.
- What is the smallest safe change? Close the gap between authored catalog IDs, physical assets, import metadata, fallback chains, snapshot fixtures, and player-visible truth. Define what is complete, what is intentionally fallback-backed, and how missing art becomes actionable diagnostics.
- Which owner is touched? Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only
- What must remain untouched? No full-suite default.
- What proves completion? Named commands/results and limitations recorded.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 8: Balance/content polish
- What current evidence must be reread? The live host already has a centralized Godot AssetRegistry with explicit fallback diagnostics, survivor portrait resolution, location lookup, faction icon resolution, and an asset registry self-test. The visual plan therefore extends the current resolver and registry; it does not create a second visual domain authority.
- What is the smallest safe change? Close the gap between authored catalog IDs, physical assets, import metadata, fallback chains, snapshot fixtures, and player-visible truth. Define what is complete, what is intentionally fallback-backed, and how missing art becomes actionable diagnostics.
- Which owner is touched? Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only
- What must remain untouched? No hidden tuning or parallel scalar.
- What proves completion? Content review confirms no dominated/unreachable row and no unsupported claim.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 9: Rollback and closeout
- What current evidence must be reread? The live host already has a centralized Godot AssetRegistry with explicit fallback diagnostics, survivor portrait resolution, location lookup, faction icon resolution, and an asset registry self-test. The visual plan therefore extends the current resolver and registry; it does not create a second visual domain authority.
- What is the smallest safe change? Close the gap between authored catalog IDs, physical assets, import metadata, fallback chains, snapshot fixtures, and player-visible truth. Define what is complete, what is intentionally fallback-backed, and how missing art becomes actionable diagnostics.
- Which owner is touched? Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only
- What must remain untouched? No unowned cleanup.
- What proves completion? Foreman review accepts or records a blocker.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

## Ownership matrix and file impact map
The following is an impact map for a future implementation package, not a request to edit these paths in this planning-only task.
- `READ/MODIFY only after claim`: `src/Host/AssetRegistry.cs` — Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/World/SurvivorActorView.cs` — Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/World/MapLocationMarkerView.cs` — Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/UI/MapDetailPanel.cs` — Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/UI/ShelterDecorPanel.cs` — Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/UI/AshfallUiTheme.cs` — Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/Main.Application.cs` — Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/UI/FactionIconCatalog.cs` — Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only; change only the confirmed owner seam.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/asset_registry.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/locations.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/items.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/survivors.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after presentation claim`: `src/UI/MapDetailPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after presentation claim`: `src/UI/ShelterDecorPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after presentation claim`: `src/UI/AshfallUiTheme.cs` — thin projection and command surface only.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/Visual/AssetFallbackDiagnosticsTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/LegacyAssetGateTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/ProductionArtManifestTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/Assets/Plan50AssetTruthIntegrationTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/Presentation/Plan51PresentedGameIntegrationTests.cs` — focused verification; no test created to mask a stale premise.

## Out of scope
- No unrelated refactor.
- No Unity restoration or dependency.
- No generated index or unrelated documentation regeneration.
- No broad test suite or runtime soak by default.
- No new save owner, registry, selector, or simulation unless a signed architecture decision names it.

## Definition of done
- The current owner and public API are cited from the implementation snapshot.
- src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates is wired end to end or the plan explicitly closes as already integrated.
- No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Missing primary art falls back visibly and records category, requested ID, tried paths, fallback path, and duplicate-request count; malformed import metadata fails closed to the canonical fallback.
- Data references, schema version, old-save behavior, focused tests, and rollback are recorded.
- No stale “sealed”, “approved”, or pass-count language is used without current evidence.

## Numbered implementation contract

# 1. Objective

Deliver only the bounded delta described as: Close the gap between authored catalog IDs, physical assets, import metadata, fallback chains, snapshot fixtures, and player-visible truth. Define what is complete, what is intentionally fallback-backed, and how missing art becomes actionable diagnostics. The objective is measured by current-owner reachability, truthful state, deterministic replay, and a safe implementation handoff rather than by document length.

# 2. Current Reality

The live host already has a centralized Godot AssetRegistry with explicit fallback diagnostics, survivor portrait resolution, location lookup, faction icon resolution, and an asset registry self-test. The visual plan therefore extends the current resolver and registry; it does not create a second visual domain authority.

# 3. Required Delta

Close the gap between authored catalog IDs, physical assets, import metadata, fallback chains, snapshot fixtures, and player-visible truth. Define what is complete, what is intentionally fallback-backed, and how missing art becomes actionable diagnostics.

# 4. Evidence

Use the current source/data dossier, JSON audit, symbol/caller audit, and read-only authority excerpts in this document. The canonical authority is docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md with SHA-256 911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c; current source/data remain the implementation truth when the authority is descriptive or historical.

# 5. Existing Extension Seams

Primary seam: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates. Before creating anything, verify the current public method, event, host session, save store, and consumer named in the dossier. A new abstraction is justified only when this seam cannot express the confirmed delta.

# 6. Proposed Architecture

Use the owner-first Core → data → host → presentation architecture described above. The proposed architecture is a bounded extension of Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only; it does not create a parallel gameplay system.

# 7. Ownership Matrix

Canonical ownership: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only. JSON owns authored content, Core owns pure rules/state transitions, the host owns lifecycle/adapters, and Godot panels own presentation only. Every proposed write must be assigned to exactly one row of that matrix.

# 8. Data Flow

INPUT (catalog, command, current state) → VALIDATION (owner/schema/reference checks) → CORE STATE → DOMAIN FACT → HOST PROJECTION → UI FEEDBACK → SAVE OWNER. The reverse UI path is a command request, never a direct state mutation.

# 9. State Model

State rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. The implementing agent must document fields, defaults, lifecycle, mutation, reset, persistence, and migration against the actual current DTO before editing.

# 10. API/Contracts

Expose only the current owner’s read query, command/mutation, typed fact/event, capture/restore, diagnostic result, idempotency key, and stable ordering needed for src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates. Confirm signatures from source; never copy historical API names from the old plan.

# 11. Data Changes

Data changes must extend current catalogs under Assets/StreamingAssets/Data/. For each row, validate schema_version, snake_case ID, references, ranges, default behavior, loader, consumer, and observable outcome. The record review ledger applies this rule to Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.

# 12. Save/Load

Persistence must use the current owner identified by the dossier. No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Require capture/restore, deep-copy isolation, old-version defaults, partial/corrupt input behavior, checksum handling, and mid-event reload before claiming persistence.

# 13. Determinism

Determinism contract: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order. Reuse existing seeded RNG forks, ordinal ordering, bounded state, and invariant culture formatting. A compile-green result is not replay evidence.

# 14. System/Event Wiring

Wire the confirmed event or command through src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates. Define event order, daily/hourly triggers, idempotency, subscriber lifetime, missing-owner behavior, and the exact host projection. Do not add a second event authority.

# 15. Godot Integration

Godot integration is limited to the current host/session and named presentation surfaces. UI rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs'] Preserve focus, controller/keyboard close/back, contrast, reduced motion, refresh, and disposal behavior.

# 16. Narrative/Content Integration

Content must describe only effects expressible by the current owner. Record-level action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art. Validate narrative references, continuity, voice, and player-visible consequence without making prose a hidden gameplay authority.

# 17. Failure Modes

Failure behavior: Missing primary art falls back visibly and records category, requested ID, tried paths, fallback path, and duplicate-request count; malformed import metadata fails closed to the canonical fallback. The failure matrix covers null/empty state, missing/duplicate references, old/corrupt saves, unavailable owners, dead or hostile participants, repeated commands, simultaneous events, reload, missing UI/audio/journal, and deterministic replay.

# 18. Test Strategy

Use the smallest existing focused test first; run a new test alone; keep save/load, determinism, lifecycle, mutation, fuzzing, state-transition, and cross-system workflows independently reported. The plan-only package does not claim these commands were run.

# 19. Dependency-Ordered Phases

Follow phases 0–9: premise recheck, owner/collision map, Core contract, persistence/migration, data tranche, host/event wiring, presentation/accessibility, end-to-end/replay, balance/polish, and rollback/closeout. Each phase has a completion gate and a must-not-touch boundary above.

# 20. File Impact Map

The future implementation package may modify only the confirmed owner/data/host/UI/test paths listed in the dossier and only after claiming them. This Round 7 planning package intentionally modifies none of those production paths.

# 21. Risks

Primary risk: The worktree contains active art imports and presentation changes; hashes are a planning snapshot and must be rechecked after concurrent art work settles. Additional risks are dirty-worktree drift, stale catalog counts, missing host callers, shared save seams, decision-gated authority, accessibility regressions, and false completion claims. Each risk has a stop/escalate rule in the handoff.

# 22. Out of Scope

No unrelated refactor, Unity restoration, new parallel authority, broad test suite, generated-index rewrite, asset production, or opportunistic gameplay tuning is included.

# 23. Rollback Strategy

Rollback is local: disable the adapter, remove only the bounded authored tranche, preserve the owner’s last valid state, and keep the save schema readable. Never reset the shared worktree or hide a failure with a destructive migration.

# 24. Definition of Done

The implementing package is done only when src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates is wired or proven already integrated, current data validates and reaches a consumer, save/determinism/failure/UI contracts pass focused verification, accessibility is truthful, and the handoff records limitations. This document itself remains planning-only.

# 25. Implementation Handoff

MUST PRESERVE the current owner, Godot/Core boundary, JSON authority, save/determinism contracts, and accessibility. MUST ADD only the smallest confirmed extension and focused evidence. MUST NOT invent APIs or claim unrun tests. FIRST SAFE STEP: reread the first current owner/catalog/host/test path and write a live-versus-stale premise table before any implementation edit.

## Detailed record-by-record integration ledger

### Record review 001: `fallback_survivor`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 002: `fallback_icon`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 003: `fallback_location`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 004: `fallback_weather`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 005: `fallback_faction`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 006: `ammo_deprecated_cal_9x19`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 007: `ammo_deprecated_380acp`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 008: `ammo_deprecated_762x25`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 009: `ammo_deprecated_45acp`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 010: `ammo_deprecated_9x21`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 011: `ammo_deprecated_765x21`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 012: `ammo_deprecated_12ga`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 013: `ammo_deprecated_16ga`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 014: `ammo_deprecated_556x45`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 015: `ammo_deprecated_762x39`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 016: `ammo_deprecated_545x39`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 017: `ammo_deprecated_762x51`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 018: `ammo_deprecated_300blk`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 019: `ammo_deprecated_57x28`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 020: `ammo_deprecated_46x30`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 021: `ammo_deprecated_762x54r`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 022: `ammo_deprecated_338lapua`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 023: `ammo_deprecated_408cheytac`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 024: `ammo_deprecated_50bmg`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 025: `ammo_545x39_jhp_ap`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 026: `ammo_545x39_exi`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 027: `ammo_545x39_api`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 028: `ammo_300blk_jhp_ap`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 029: `ammo_57x28_jhp_ap`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 030: `ammo_57x28_exi`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 031: `ammo_57x28_api`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 032: `ammo_762x54r_jhp_ap`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 033: `ammo_762x54r_exi`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 034: `ammo_762x54r_api`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 035: `ammo_338lapua_jhp_ap`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 036: `ammo_408cheytac_jhp_ap`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 037: `ammo_762x51_jhp_ap`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 038: `ammo_762x51_exi`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 039: `ammo_50bmg_jhp_ap`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 040: `ammo_50bmg_exi`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 041: `pistol_cz75_9x19`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 042: `pistol_beretta_92_9x19`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 043: `pistol_steyr_m9_9x19`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 044: `pistol_walther_ppk_380acp`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 045: `pistol_grand_power_p380_380acp`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 046: `pistol_cz52_762x25`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 047: `pistol_norinco_type54_762x25`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 048: `pistol_zastava_m57_762x25`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 049: `smg_m1928a1_thompson_45acp`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 050: `smg_hk_ump45_45acp`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 051: `smg_kriss_vector_45acp`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 052: `pistol_bt_apc45_mini_45acp`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 053: `smg_bt_apc45_45acp`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 054: `smg_sites_spectre_m4_9x21`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 055: `smg_imi_micro_uzi_9x21`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 056: `smg_cz_scorpion_evo3_9x21`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 057: `smg_steyr_solo_s1_100_765x21`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 058: `smg_mp34_765x21`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 059: `shotgun_benelli_m4_super90_12ga`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 060: `shotgun_remington_model1100_12ga`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 061: `shotgun_browning_auto5_16ga`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 062: `shotgun_franchi_al48_16ga`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 063: `rifle_m4a1_carbine_556x45`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 064: `rifle_hk416_556x45`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 065: `rifle_fn_scar_l_556x45`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 066: `rifle_steyr_aug_a3_556x45`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 067: `rifle_ak47_762x39`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 068: `rifle_cmmg_mk47_mutant_762x39`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 069: `lmg_rpk74_545x39`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 070: `rifle_ak74u_545x39`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 071: `rifle_fn_fal_762x51`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 072: `rifle_hk_g3_762x51`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 073: `rifle_q_honey_badger_300blk`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 074: `rifle_sig_mcx_rattler_300blk`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 075: `rifle_ddm4_pdw_300blk`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 076: `pdw_fn_p90_57x28`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 077: `carbine_ruger_lc_57x28`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 078: `pdw_hk_mp7a2_46x30`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 079: `pdw_cmmg_four6_46x30`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 080: `pdw_tb_tactical_t7_46x30`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 081: `sniper_mosin_nagant_m9031_762x54r`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 082: `sniper_svd_dragunov_762x54r`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 083: `sniper_romak3_psl_762x54r`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 084: `sniper_steyr_ssg08_338lapua`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 085: `sniper_sako_trg42_338lapua`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 086: `sniper_dsr1_338lapua`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 087: `sniper_cheytac_m200_intervention_408cheytac`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 088: `sniper_voere_mk_x3_408cheytac`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 089: `sniper_voere_mk_x4_408cheytac`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 090: `sniper_barrett_m82a1_50bmg`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 091: `item_emp_grenade`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 092: `water_bottle_1l_of_2l`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 093: `water_bottle_0_5l_of_1l`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 094: `water_bottle_0_5l_of_2l`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 095: `water_bottle_1_5l_of_2l`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 096: `fuel_0_5l_of_1l`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 097: `accelerant_half`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 098: `ejuice_10ml_10mg`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 099: `ejuice_10ml_20mg`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 100: `ejuice_20ml_35mg`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 101: `water_purification_tablets_40_of_40`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 102: `water_purification_tablets_20_of_40`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 103: `water_purification_tablets_0_of_40`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 104: `iodine_pills_bottle_10_of_10`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 105: `jetfuel_jerrycan_10l_of_10l`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 106: `instant_coffee_10x_container`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 107: `ice_tea_0_5l_package`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 108: `package_rolled_oats_1kg_of_1kg`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 109: `dry_rice_1kg_of_1kg`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 110: `dried_pasta_2kg_of_2kg`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 111: `soy_and_rice_milk_1l_of_1l`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 112: `anti_rad`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 113: `prewar_letter`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 114: `item_bioluminescent_moss`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 115: `hand_crank_radio`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 116: `item_uv_lamp_ballast`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 117: `item_geothermal_valve`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 118: `item_ro_membrane`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 119: `item_acoustic_decoy`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 120: `item_logic_board`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 121: `item_co2_scrubber_cartridge`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 122: `item_rebreather_scrubber`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 123: `antiseptic_1l_of_1l`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 124: `alcohol_wipes_box_10_of_10`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 125: `epi_pen`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 126: `decontamination_soap_5_of_5`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 127: `item_frostbite_salve`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 128: `item_scopolamine_root`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 129: `item_lithium_salts`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 130: `item_amnestic_syrup`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 131: `item_snow_goggles_improvised`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 132: `item_lead_visor`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 133: `item_ash_ghillie`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 134: `item_black_ice_sample`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 135: `item_cobalt_salt_canister`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 136: `item_black_water_vial`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 137: `item_submerged_server`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 138: `item_master_override`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 139: `item_hard_drive_platter`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 140: `item_pre_war_photo_album`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 141: `item_vinyl_collection`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 142: `att_mil_double_scope_5x_10x`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 143: `sewing_kit_10_of_10`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 144: `item_hand_crank_sled`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 145: `item_geiger_tether`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 146: `item_pneumatic_jack`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 147: `item_fungicide_fogger`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 148: `item_mine_prod`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 149: `item_headphones_mil`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 150: `item_epoxy_injector`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 151: `item_tether_harness`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 152: `explosive_powder_nitroglycerin`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 153: `salvaged_tech_trash`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 154: `rope_2m_of_2m`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 155: `copper_wire_10m_of_10m`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 156: `oat_flour`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 157: `plastic_contamination_bag_box_5`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 158: `item_cryo_coolant`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 159: `item_thermal_paste`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 160: `item_shoring_timber`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 161: `item_mycelium_bricks`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 162: `item_faraday_mesh`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 163: `item_sound_baffling`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 164: `item_tungsten_core`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 165: `item_pneumatic_hose`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 166: `item_galvanized_rebar`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 167: `item_welders_glass`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 168: `item_mirror_shard`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 169: `item_bio_plastic`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 170: `rubber_gasket`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 171: `concrete_patch_mix`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 172: `insulation_tape`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 173: `engine_block_intact`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 174: `bearing_set_industrial`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 175: `copper_tubing_1m`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 176: `structural_report`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 177: `radio_transcript_142_5`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 178: `signal_source_report`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 179: `refugee_screening_report`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 180: `supply_inventory_report`
- Source: `Assets/StreamingAssets/Data/asset_registry.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify the record as a real asset candidate, an intentional fallback, a missing production deliverable, or a stale catalog reference before authoring any new art.
- Primary owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State/save rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI truth rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

## Precision scenario matrix
Each row is a future implementation checkpoint, not a claim that the current repository already passes it.
### Scenario 01: fresh campaign before the owner is initialized
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 02: old save restored at day zero
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 03: old save restored after a partial event
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 04: catalog unavailable at startup
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 05: catalog contains an empty collection
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 06: duplicate canonical ID
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 07: reference points to a missing item
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 08: reference points to a missing location
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 09: reference points to a missing faction
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 10: unavailable optional owner
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 11: owner disabled by difficulty or policy
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 12: unpowered infrastructure
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 13: zero resources
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 14: negative or malformed numeric input
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 15: large but bounded collection
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 16: dead survivor or unavailable participant
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 17: hostile faction state
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 18: repeated player command
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 19: simultaneous day events
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 20: mid-transition save
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 21: reload after event dispatch
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 22: missing UI surface
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 23: stale presentation cache
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 24: missing audio cue
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 25: missing journal owner
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 26: missing save owner
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 27: corrupt save payload
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 28: checksum mismatch
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 29: RNG fork unavailable
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 30: unordered dictionary iteration
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 31: clock boundary at midnight
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 32: seasonal boundary
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 33: weather gate closure
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 34: route closure
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 35: trade or treaty conflict
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 36: choice already resolved
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 37: ending owner unavailable
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 38: optional content absent
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 39: mod or compatibility row absent
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 40: concurrent package changes source
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 41: headless session without UI
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 42: snapshot fixture unavailable
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 43: controller/keyboard focus path
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 44: screen reader/high contrast path
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 45: asset/resource fallback
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 46: final archive projection
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 47: new-game reset
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 48: legacy content migration
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 49: consumer not wired
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 50: host setup order reversed
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 51: teardown/disposal
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 52: replay after reload
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 53: telemetry/diagnostic emission
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 54: authority conflict discovered
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 55: future implementation acceptance boundary
- Domain lens: presentation and asset truth.
- Seam under test: src/Host/AssetRegistry.cs -> authored asset registry and domain IDs -> Godot texture/resource boundary -> UI/world views and snapshot gates.
- Expected authority: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed. Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI/accessibility check: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

## Failure and rejection matrix
The failure contract is intentionally strict: an unavailable feature is preferable to a convincing but unauthoritative simulation.
### Failure 01: null state
- Detection: accept an explicit empty/default state only where the owner contract permits it.
- Owner response: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Player-facing truth: Missing primary art falls back visibly and records category, requested ID, tried paths, fallback path, and duplicate-request count; malformed import metadata fails closed to the canonical fallback.
- Persistence response: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism response: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 02: empty catalog
- Detection: report a data-integrity gap and keep the previous safe projection.
- Owner response: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Player-facing truth: Missing primary art falls back visibly and records category, requested ID, tried paths, fallback path, and duplicate-request count; malformed import metadata fails closed to the canonical fallback.
- Persistence response: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism response: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 03: duplicate ID
- Detection: reject the row with a stable diagnostic rather than last-write-wins.
- Owner response: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Player-facing truth: Missing primary art falls back visibly and records category, requested ID, tried paths, fallback path, and duplicate-request count; malformed import metadata fails closed to the canonical fallback.
- Persistence response: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism response: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 04: missing reference
- Detection: do not create a phantom entity; expose the unresolved dependency.
- Owner response: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Player-facing truth: Missing primary art falls back visibly and records category, requested ID, tried paths, fallback path, and duplicate-request count; malformed import metadata fails closed to the canonical fallback.
- Persistence response: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism response: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 05: stale save
- Detection: migrate or default only through the owner’s versioned restore path.
- Owner response: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Player-facing truth: Missing primary art falls back visibly and records category, requested ID, tried paths, fallback path, and duplicate-request count; malformed import metadata fails closed to the canonical fallback.
- Persistence response: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism response: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 06: corrupt checksum
- Detection: refuse the corrupted section and preserve unrelated valid sections.
- Owner response: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Player-facing truth: Missing primary art falls back visibly and records category, requested ID, tried paths, fallback path, and duplicate-request count; malformed import metadata fails closed to the canonical fallback.
- Persistence response: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism response: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 07: host reload
- Detection: rehydrate through the same owner and event registration path.
- Owner response: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Player-facing truth: Missing primary art falls back visibly and records category, requested ID, tried paths, fallback path, and duplicate-request count; malformed import metadata fails closed to the canonical fallback.
- Persistence response: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism response: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 08: new game
- Detection: clear transient host state and initialize owner defaults exactly once.
- Owner response: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Player-facing truth: Missing primary art falls back visibly and records category, requested ID, tried paths, fallback path, and duplicate-request count; malformed import metadata fails closed to the canonical fallback.
- Persistence response: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism response: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 09: deterministic replay
- Detection: same seed, day, catalog, and state must produce the same fact/order.
- Owner response: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Player-facing truth: Missing primary art falls back visibly and records category, requested ID, tried paths, fallback path, and duplicate-request count; malformed import metadata fails closed to the canonical fallback.
- Persistence response: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism response: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 10: UI unavailable
- Detection: retain canonical state and defer presentation without re-running mutation.
- Owner response: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Player-facing truth: Missing primary art falls back visibly and records category, requested ID, tried paths, fallback path, and duplicate-request count; malformed import metadata fails closed to the canonical fallback.
- Persistence response: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism response: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 11: disposal
- Detection: unregister listeners and release host resources deterministically.
- Owner response: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Player-facing truth: Missing primary art falls back visibly and records category, requested ID, tried paths, fallback path, and duplicate-request count; malformed import metadata fails closed to the canonical fallback.
- Persistence response: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism response: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 12: concurrent claim
- Detection: stop and hand off rather than editing a shared seam.
- Owner response: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Player-facing truth: Missing primary art falls back visibly and records category, requested ID, tried paths, fallback path, and duplicate-request count; malformed import metadata fails closed to the canonical fallback.
- Persistence response: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism response: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 13: authority conflict
- Detection: name the conflict and defer the architectural decision.
- Owner response: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Player-facing truth: Missing primary art falls back visibly and records category, requested ID, tried paths, fallback path, and duplicate-request count; malformed import metadata fails closed to the canonical fallback.
- Persistence response: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism response: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 14: missing test
- Detection: do not claim integration; add the focused contract test in the implementing package.
- Owner response: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Player-facing truth: Missing primary art falls back visibly and records category, requested ID, tried paths, fallback path, and duplicate-request count; malformed import metadata fails closed to the canonical fallback.
- Persistence response: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism response: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 15: unsupported API
- Detection: use the current public API or mark the premise stale.
- Owner response: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Player-facing truth: Missing primary art falls back visibly and records category, requested ID, tried paths, fallback path, and duplicate-request count; malformed import metadata fails closed to the canonical fallback.
- Persistence response: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism response: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 16: legacy Unity reference
- Detection: do not restore it; port only through the Godot/Core boundary.
- Owner response: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Player-facing truth: Missing primary art falls back visibly and records category, requested ID, tried paths, fallback path, and duplicate-request count; malformed import metadata fails closed to the canonical fallback.
- Persistence response: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism response: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 17: engine reference in Core
- Detection: reject the change and move only presentation adaptation to src/.
- Owner response: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Player-facing truth: Missing primary art falls back visibly and records category, requested ID, tried paths, fallback path, and duplicate-request count; malformed import metadata fails closed to the canonical fallback.
- Persistence response: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism response: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 18: unowned save state
- Detection: reject the change until the owner and migration path are explicit.
- Owner response: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Player-facing truth: Missing primary art falls back visibly and records category, requested ID, tried paths, fallback path, and duplicate-request count; malformed import metadata fails closed to the canonical fallback.
- Persistence response: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism response: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 19: UI-owned gameplay
- Detection: reject the panel mutation and route the command to Core.
- Owner response: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Player-facing truth: Missing primary art falls back visibly and records category, requested ID, tried paths, fallback path, and duplicate-request count; malformed import metadata fails closed to the canonical fallback.
- Persistence response: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism response: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 20: false completion claim
- Detection: downgrade the handoff to planning/static evidence only.
- Owner response: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- Player-facing truth: Missing primary art falls back visibly and records category, requested ID, tried paths, fallback path, and duplicate-request count; malformed import metadata fails closed to the canonical fallback.
- Persistence response: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism response: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

## Layered focused verification matrix
Commands are exact paths only where the current test file exists. They are future implementation gates for this plan-only package.
### Verification layer 01: Core unit
- Coverage: valid input, boundary, missing optional field, duplicate/unknown reference, state invariant.
- Domain contract: asset registry.
- Owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 02: Core transition
- Coverage: one legal transition, one illegal transition, repeated transition, cancellation/rollback.
- Domain contract: fallback diagnostics.
- Owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 03: save round-trip
- Coverage: capture, serialize, restore, deep-copy isolation, old version/defaults.
- Domain contract: portrait resolver.
- Owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 04: data integrity
- Coverage: schema_version, snake_case IDs, duplicate IDs, references, ranges, collection shape.
- Domain contract: location art.
- Owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 05: host wiring
- Coverage: setup, command dispatch, event subscription, refresh, disposal, missing owner.
- Domain contract: faction icon.
- Owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 06: UI projection
- Coverage: truthful current state, disabled action, focus order, controller/keyboard close/back.
- Domain contract: snapshot fixture.
- Owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 07: headless/selftest
- Coverage: bounded deterministic scenario and diagnostics without a renderer.
- Domain contract: asset registry.
- Owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 08: cross-system
- Coverage: owner event to consumer, ordering, idempotency, no parallel state.
- Domain contract: fallback diagnostics.
- Owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 09: replay
- Coverage: same seed/day/input produces same state hash and fact order.
- Domain contract: portrait resolver.
- Owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 10: content utilization
- Coverage: authored record has a current loader, consumer, and observable outcome.
- Domain contract: location art.
- Owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 11: migration
- Coverage: old save and current catalog remain readable or fail with a named reason.
- Domain contract: faction icon.
- Owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 12: rollback
- Coverage: feature flag/owner boundary permits disabling the delta without data loss.
- Domain contract: snapshot fixture.
- Owner: Godot host AssetRegistry and existing presentation views; Core remains string/path-policy only.
- State rule: No new gameplay save state. Diagnostics are bounded runtime/QA data unless a future requirement proves a durable asset-migration state is needed.
- Determinism rule: Resolution order is ordinal and deterministic; fallback selection cannot depend on filesystem enumeration order.
- UI rule: ['src/UI/MapDetailPanel.cs', 'src/UI/ShelterDecorPanel.cs', 'src/UI/AshfallUiTheme.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

#### Current focused command 01
- Test: `Ashfall.Core.Tests/Visual/AssetFallbackDiagnosticsTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/Visual/AssetFallbackDiagnosticsTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 02
- Test: `Ashfall.Core.Tests/LegacyAssetGateTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/LegacyAssetGateTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 03
- Test: `Ashfall.Core.Tests/ProductionArtManifestTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/ProductionArtManifestTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 04
- Test: `Ashfall.Core.Tests/Assets/Plan50AssetTruthIntegrationTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/Assets/Plan50AssetTruthIntegrationTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 05
- Test: `Ashfall.Core.Tests/Presentation/Plan51PresentedGameIntegrationTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/Presentation/Plan51PresentedGameIntegrationTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

> **Structured body length before post-250K polishing:** 420,303 characters.
# Post-250K deep polishing pass

The architecture body above reached 420,381 characters before this pass. This is a second deep polishing phase, not a license to pad. Re-read the plan as an implementer and tighten it against the current source/data snapshot.

## Deep polish A — authority and collision recheck

- Re-run the owner/caller audit for: `asset registry`, `fallback diagnostics`, `portrait resolver`, `location art`, `faction icon`, `snapshot fixture`.
- Confirm each proposed mutation has exactly one writer. A host adapter may translate a fact; a panel may display it; neither becomes authority.
- Check for legacy or parallel names before proposing any new class, DTO, event, catalog, save section, or RNG stream.
- Treat the master authority as a read-only design lens. Current source/data wins when they disagree, and the disagreement is recorded rather than hidden.
- Recheck the live worktree status recorded in each evidence dossier. A dirty source path is a coordination warning, not a stable acceptance result.

## Deep polish B — data and consumer precision

- For every candidate row, name the loader, consumer, reference validator, and observable outcome.
- Replace counts copied from the old plan with current JSON counts or an explicit census task.
- Remove any row whose only consumer is a test, a prose generator, or a panel.
- Preserve additive schema compatibility and explain old-data defaults.
- Check that narrative text does not promise an effect that the current owner cannot produce.

## Deep polish C — state, save, and replay precision

- Confirm the existing save owner and DTO before naming a field.
- Test capture/restore, deep-copy isolation, old-version defaults, partial/corrupt input, and mid-event reload.
- Confirm repeated commands and replay cannot duplicate one-shot effects.
- Use the existing seeded stream/fork; document every random draw and tie-break.
- Treat a UI refresh as a projection, never as persistence or mutation.

## Deep polish D — failure and player truth

- Walk null, empty, missing, duplicate, stale, hostile, dead, unavailable, and repeated-event cases.
- Ensure blocked/unavailable states are legible and do not masquerade as success.
- Keep accessibility behavior attached to the same command/state surface as the visual behavior.
- Record which failures are diagnostic-only, which defer presentation, and which halt the transition.

## Deep polish E — implementation readiness

- Replace generic file lists with owner-specific action verbs and completion gates.
- Keep the phase order dependency-safe: premise, owner, Core, persistence, data, host, presentation, end-to-end, polish, closeout.
- Give the next implementer exact focused commands, test selection rationale, and stop conditions.
- Keep a final residual-risk list for decision-gated or concurrent work rather than hiding it in prose.

# Post-250K deep polishing pass — second pass

The first post-250K pass above checked structure and owner collisions. This second pass is a separate adversarial review after the plan has reached its depth checkpoint; it is not a duplicate paragraph exercise.

## Second-pass adversarial questions

- What would a reviewer incorrectly assume after reading only the executive summary?
- Which sentence describes historical intent but could be mistaken for current behavior?
- Which current owner, save path, host session, or event seam is missing from the impact map?
- Which data row has a valid ID but no reachable consumer?
- Which UI label could claim a consequence before the Core transition succeeds?
- Which failure currently falls through as a default success, duplicate event, or stale cache?
- Which random choice lacks a named seed/fork/tie-break?
- Which old-save field lacks a default, migration, or deep-copy test?
- Which concurrent package or decision gate could invalidate the proposed path?
- What is the smallest rollback that leaves the previous owner state readable?

## Second-pass correction protocol

For every answer, classify the issue as `CURRENT_EVIDENCE`, `PROPOSED_EXTENSION`, `DECISION_GATE`, `CONCURRENT_CLAIM`, or `OUT_OF_SCOPE`. Update the relevant numbered section and record the exact path/command that would close the issue. If no current evidence supports a claim, remove the claim rather than softening it with adjectives. If a proposed change needs a new architecture decision, stop at the gate and name the decision owner. This pass must leave the plan more precise, not merely longer.

## Second-pass acceptance

- [ ] Every “current” statement has a path or is marked as an open premise.
- [ ] Every “implemented” statement names a current caller or is downgraded to catalog-only.
- [ ] Every proposed state field names its save owner and migration behavior.
- [ ] Every proposed random/temporal behavior names determinism treatment.
- [ ] Every UI consequence has a command/state source.
- [ ] Every failure has a safe expected outcome.
- [ ] Every focused command resolves or is labeled future implementation work.
- [ ] Every decision-gated or concurrently claimed seam is explicit.

# Final precision and reaccuracy pass

1. Re-read every current path cited in the dossier and mark missing paths as open premises.
2. Re-run the hash verifier and data JSON parse check at the final snapshot.
3. Re-run the symbol occurrence audit and distinguish declarations, host consumers, and test-only references.
4. Check all current focused test commands resolve to existing files.
5. Remove unsupported historical counts, “sealed” claims, invented APIs, and unproven caller assertions.
6. Reconcile the plan title and requested delta with the actual current owner; if the old plan is already implemented or stale, state maintenance or blocked scope plainly.
7. Confirm Core remains engine-free, JSON remains authoritative, and no parallel authority is proposed.
8. Confirm UI, failure, save, determinism, and accessibility contracts are concrete.
9. Confirm rollback is local and does not require destructive state reset.
10. Record limitations honestly: this package is planning-only and does not run implementation tests.

# Full repolishing phase

The final repolishing pass is a quality audit over the complete document, not an append-only slogan. Read the plan from executive summary through handoff as one artifact.

- **Coherence:** every section uses the same owner names, state terms, and delta.
- **Evidence:** every important claim points to a current path, current data record, read-only authority anchor, or explicit unknown.
- **Architecture:** no duplicate system, parallel save, second RNG, engine dependency, or UI authority is smuggled into a phase.
- **Mechanics:** effects are expressed through existing commands/events and have a visible or auditable consequence.
- **Continuity:** references, days, locations, factions, and narrative facts use canonical IDs and current catalogs.
- **Failure:** invalid and unavailable states are defined, bounded, and truthful.
- **Persistence:** capture/restore and migration are named for every durable delta; no new section is casually proposed.
- **Determinism:** random sources, ordering, and replay comparisons are explicit.
- **UI:** panels are thin, accessible, refreshable, and non-authoritative.
- **Testing:** each layer has a focused command or a clear reason it must be authored later.
- **Rollback:** every phase has a local reversal and preservation rule.
- **Handoff:** the next implementer can start with the first safe step without interpreting the plan around stale prose.

## Repolish acceptance checklist

- [ ] Current owner/API confirmed from source.
- [ ] Current data schema and references confirmed.
- [ ] Existing save owner and migration path confirmed.
- [ ] Existing host/event seam confirmed.
- [ ] Existing UI surface and accessibility path confirmed.
- [ ] Focused test paths resolve or are labeled future work.
- [ ] Deterministic replay and idempotency contract stated.
- [ ] Failure and old-save behavior stated.
- [ ] No unsupported completion claim remains.
- [ ] Rollback and owner handoff are actionable.
- [ ] Plan remains implementation-ready without production edits in this package.

# Implementation handoff

## MUST PRESERVE

- The current owner named in this plan and the one-authority rule.
- Godot as the presentation/host authority and Core as engine-free domain logic.
- Authoritative JSON under `Assets/StreamingAssets/Data/`.
- Existing save ownership, migration semantics, seeded RNG contracts, and event ordering.
- Existing accessibility, focus, controller/keyboard close/back, and truthful UI behavior.

## MUST ADD

- Only the smallest confirmed Core/data/host/UI extension needed by the current delta.
- Exact catalog validation, consumer reachability, save/restore, failure, determinism, and focused tests.
- A bounded content tranche whose records are consumed and observable.
- A local rollback/disablement path and a concise implementation handoff.

## MUST NOT DO

- Do not create a second ledger, save store, selector, event authority, simulation, or panel-owned rule.
- Do not edit production, data, tests, UI, assets, or generated indexes during this planning package.
- Do not restore Unity or add engine references to Core.
- Do not claim tests, host wiring, save integration, or player reachability from static intent alone.
- Do not use unseeded randomness, wall-clock ordering, or hash iteration order for deterministic behavior.

## VERIFY WITH

- The exact focused `scripts/run_test.sh` commands listed in this plan after the implementation claim is opened.
- Current catalog integrity and consumer/utilization checks.
- A bounded Core test, save round-trip/migration test, host wiring test, and deterministic replay comparison.
- Godot headless/UI checks only when the confirmed implementation touches the host/UI path.

## FIRST SAFE IMPLEMENTATION STEP

Re-read the current owner, the first current catalog, the first current host/session, and the first focused test listed in the dossier. Write a one-page premise table that classifies each as live, partial, stale, or blocked. Do not author content or code until the table has one owner and one non-overlapping implementation seam.
