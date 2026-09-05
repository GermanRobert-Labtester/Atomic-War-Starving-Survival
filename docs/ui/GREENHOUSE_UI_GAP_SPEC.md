# GREENHOUSE UI GAP SPEC — Google Stitch Handoff (Plan 22)

> **Purpose:** Antigravity (or any agent) can hand this document to
> **`google-stitch`** and generate the missing Greenhouse UI with no further
> code archaeology. Stitch output is a **design proposal** — implementation
> lands in Godot 4.7+ C# through the existing UI helper layer and must be
> reconciled with the tokens below (project MCP rule: Stitch output never
> replaces the runtime theme or data authority).

---

## 1. Hard constraints (apply to every screen)

| Constraint | Value |
|---|---|
| Engine | Godot 4.7+ C#, `gl_compatibility`, fixed **1920×1080** |
| Fonts | **BarlowCondensed** (headings/labels), **ShareTechMono** (values/numerals) |
| Mood | Cold, exhausted, phosphor-terminal post-atomic shelter. No glassmorphism, no rounded-friendly mobile patterns, no decorative gradients |
| Surfaces | Backdrop `#090B0C`, Surface `#050709`, Card `#090B0D`, Selected `#282319`, Hover `#1D2228` |
| Ink/palette | Primary accent `#D3AA62` (Warm), highlight `#F4C875` (Hot), text `#C7DCD0` (Pale), muted `#938F84`, dim `#7E827A` |
| Semantic | Critical `#FF4D4D`, Warning/Hazard `#FF6B35`, Success `#5CD670`, Info/Lethe `#6EA3A8`, Radiation `#D9A026`, Entropy `#C97B3A`, Amber `#D4A35A` |
| Components | Reuse existing helpers: `AshfallStatusRail` (metric cards), `AshfallDataGrid` (plot rows), `AshfallUiHelpers.MakeDataRow/MakeButton/MakeSectionHeader/MakeSmall/MakeSeparator` |
| Feedback | Every action surfaces its outcome line via `GreenhouseHostSession.LastEvent` (single event strip — no toasts) |
| Accessibility | State must never rely on color alone — cards pair color with text (`DRY`, `—`, `Nd`, `%`); hit targets ≥ 90×30; no raw item IDs in labels (displayName only) |

---

## 2. What already exists (do NOT regenerate)

`src/UI/GreenhousePanel.cs` (bound to `GreenhouseHostSession`):

- **Status rail (8 cards):** Active Beds · Plot Count · Harvests · Seed Vault (OPEN/SEALED) · Blighted Beds · Pest Control (`Nd`/`—`) · Drip Line (uses/`DRY`/`—`) · Glazing (`%`)
- **Plot grid:** `AshfallDataGrid` rows per plot, crop filter tabs (incl. apiary view)
- **Plot detail rows:** Status · Seed · Growth % · Fertility x/100 · Moisture x/100 · Soil mSv · Blight %
- **Action buttons (routed via `OnActionRequested(action, plotIndex)` → `Main.HandleGreenhouseAction`):** PLANT (hardcoded tuber seed) · WATER (fixed 50 clean) · TREAT · REPAIR · CLEAR · HARVEST · apiary INSPECT/FEED/HARVEST/INSTALL
- **Inventory detail** already renders any supply's displayName/type/description/trade value generically (`InventoryDetailPanel.cs`) — no per-supply UI needed there.

---

## 3. Gap register (generate these)

Each gap lists the live host API it must call (all exist and are tested —
Plan 22 phases A–E) and the state it surfaces. **No new Core or host code is
required for any gap** unless marked.

### GAP-1 · Plot action: PLANT — seed selection
- **Today:** PLANT hardcodes `item_seed_tuber` (`Main.World.cs` "plant" case). 12 cultivars exist; wheat is unlock-gated.
- **Stitch target:** seed-picker overlay anchored to the PLANT button — one row per cultivar: displayName, GrowthHoursToMature (from crop curve), base yield, blight resistance, water/day; locked rows show "SEED VAULT SEALED".
- **Route:** `plant <plotIndex>` must carry the chosen `seedItemId` (host `Plant(plotIndex, seedItemId, day)` already takes it).
- **State:** `GreenhouseExpansionCatalog.CropCatalog.All` (12 entries), `IsPreWarWheatUnlocked`.

### GAP-2 · Plot action: AMEND — soil amendments
- **Today:** `GreenhouseHostSession.AmendSoil(plotIndex, amendmentItemId)` is live (Phase A); no button.
- **Stitch target:** "AMEND" button beside CLEAR → picker with 3 supplies (Screened Compost / Wood-Ash Fertilizer / Fish Emulsion), each showing inventory count and effect line ("+25 fertility", "+10 fertility", "+15 fertility · growth surge"); disabled rows at 0 stock.
- **Route:** `amend <plotIndex>` carrying `amendmentItemId`.

### GAP-3 · Greenhouse-wide: SUPPLY RAIL (consumable stock strip)
- **Today:** the rail shows *system* state but not what the player holds; supplies are invisible until spent.
- **Stitch target:** second rail row (or popover) with 9 supply chips: Glass Pane, UV Sheeting, Shade Cloth, Sticky Traps, Insect Mesh, Drip Kit, Line Filter, Catchment Kit, Grow Medium, Blight Treatment — chip = count + 1-word label; count 0 renders dim.
- **State:** `InventoryHost.Inventory.CountById(...)` per chip.

### GAP-4 · Greenhouse-wide: maintenance actions row
- **Today:** `ApplyDripChainItem`, `ApplyPestProtection`, `ApplyShadeClothSupply` are live; no buttons.
- **Stitch target:** a "MAINTENANCE" section under the actions: INSTALL DRIP KIT · LOAD FILTER · HANG SHADE CLOTH · DEPLOY TRAPS · SET MESH — each shows current system state beside it (drip installed? filter uses? shade days? protection days?) and disables when the action is meaningless (e.g., LOAD FILTER with no kit, second kit).
- **Route:** `drip_install`, `filter_load`, `shade_hang`, `traps_deploy`, `mesh_deploy` (host methods exist; new switch cases only).

### GAP-5 · Plot action: STERILIZE (clear with grow medium)
- **Today:** `Clear(plotIndex, useGrowMedium: true)` scrubs residual contamination; the CLEAR button always passes `false`.
- **Stitch target:** CLEAR splits into CLEAR and STERILIZE (sterilize shows Grow Medium stock; disabled at 0 stock with "no grow medium" note).
- **Route:** `clear <plotIndex> sterilised=true` (overload exists).

### GAP-6 · Plot action: WATER — quantity + tainted choice
- **Today:** WATER hardcodes 50 clean units; tainted irrigation (worse crops, contamination risk) is unreachable from the UI.
- **Stitch target:** WATER split: CLEAN 25 / CLEAN 50 / TAINTED 50 (clearly hazard-orange, labeled "irradiated — crops remember"); each shows clean_water/irradiated_water stock.
- **Route:** `water <plotIndex> units tainted` (host `Water(plotIndex, units, tainted)` exists).

### GAP-7 · Harvest preview / readiness column
- **Today:** the grid shows stage via cell state; no time-to-maturity or water-need signal per row.
- **Stitch target:** grid columns "READY IN" (ticks remaining, from growth % and light/water assumptions) and "DRY" warning glyph + text for plots below the auto-irrigation threshold.
- **State:** plot growth %, stage, water, `CropCatalog` WaterPerDay.

### GAP-8 · Empty/degraded states copy
- **Today:** mechanical strings; several states (filter DRY, glazing < 30, no protection) lack a human line.
- **Stitch target:** one restrained sentence per state, tone: cold, practical, no melodrama. Examples provided in §5.

---

## 4. Stitch prompt skeletons (paste-ready)

> Replace `{GAP-n}` with the section above. Generate desktop 1920×1080,
> dark terminal aesthetic, condensed sans headings, mono numerals, phosphor
> green text on near-black, hazard orange only for warnings, thin 1px
> separators, no drop shadows.

- `{GAP-1}` — "Add a seed selection popover to this greenhouse management screen. 12 crop rows with four mono-numeral stats each; locked last row shows a sealed-vault state. Anchor bottom-left of the PLANT button."
- `{GAP-3}` — "Add a horizontal consumable stock strip of 10 small chips (count + label) directly beneath the existing 8 metric cards. Chips with zero count dim to 40%."
- `{GAP-4}` — "Add a maintenance actions section with 5 buttons, each paired with a small status readout to its right (installed/uses/days). Disabled buttons stay visible with a reason line."
- `{GAP-6}` — "Split the single WATER button into three watering options with stock readouts; mark the tainted option in hazard orange with a one-line consequence note."

---

## 5. Tone examples for state copy (share with Stitch as style anchors)

- Filter spent: "The drip line is installed but the filter is spent. Load a cartridge."
- Glazing degraded: "Glazing below 30% — the crops are growing at 60% light. Fit a pane."
- No pest protection: "Nothing between the seed stocks and the moths."
- Sterilize unavailable: "No grow medium for a sterile bed. Clearing only."

Forbidden: "Optimize your yield!", exclamation-mark hype, corporate UI voice, humor.

---

## 6. Reconciliation rules (implementation contract)

1. Stitch produces **layout/visual proposals only**; all values come from
   `GreenhouseHostSession`/`GreenhouseSystem` state at runtime.
2. Implement through `AshfallUiHelpers` + `AshfallStatusRail` +
   `AshfallDataGrid`; new nodes follow the SceneBinder pattern used by
   `InventoryDetailPanel`.
3. New action strings extend `Main.HandleGreenhouseAction`'s switch and call
   the existing host methods listed per gap — no new host business logic.
4. `LastEvent` remains the single feedback strip.
5. Run `ashfall-godot-scene-lint` + `ashfall-snapshot-guard` after wiring;
   `--greenhouse-selftest` must stay 89/89.
