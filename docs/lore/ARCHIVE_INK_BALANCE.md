# Plan 17 — Archive Ink Balance

Analysis of the archive ink economy, current state, and expansion plan from 3 → 12 inks.

## Current State (3 Inks)

| Ink ID | Display Name | Legibility | Longevity | Fade Rate | Required Item | Amount |
|--------|-------------|:----------:|:---------:|:---------:|---------------|:------:|
| `ink_iron_gall` | Iron Gall Ink | 0.9 | 500 days | 0.002/day | `charcoal` | 2 |
| `ink_soot_lamp` | Soot Lamp Ink | 0.7 | 300 days | 0.003/day | `charcoal` | 1 |
| `ink_plant_dye` | Plant Dye Ink | 0.6 | 200 days | 0.005/day | `cloth` | 1 |

**Critical fix applied:** `charcoal` item was missing from `items.json` — now added. All 3 inks are now craftable.

## Expansion Plan: 3 → 12 Inks

### Proposed New Inks (9 additions)

| Ink ID | Display Name | Material Origin | Legibility | Longevity | Required Item | Tier |
|--------|-------------|-----------------|:----------:|:---------:|---------------|:----:|
| `ink_lampblack` | Lampblack | Oil lamp soot | 0.65 | 250 days | `charcoal` | Common |
| `ink_berry_juice` | Berry Juice | Crushed berries | 0.5 | 150 days | `berries` | Common |
| `ink_chemical_marker` | Chemical Marker | Industrial chemical | 0.8 | 400 days | `chemical_solvent` | Uncommon |
| `ink_diluted_toner` | Diluted Toner | Salvaged copier toner | 0.75 | 350 days | `empty_toner_cartridge` | Uncommon |
| `ink_archival_carbon` | Archival Carbon | Pure carbon suspension | 0.95 | 600 days | `charcoal` | Rare |
| `ink_improvised_pigment` | Improvised Pigment | Ground minerals | 0.55 | 180 days | `mineral_chunk` | Common |
| `ink_blood_emergency` | Blood (Emergency) | Human/animal blood | 0.4 | 100 days | `blood_sample` | Emergency |
| `ink_sepia` | Sepia Wash | Cuttlefish/organic | 0.7 | 280 days | `organic_residue` | Common |
| `ink_mineral_oxide` | Mineral Oxide | Rust/iron oxide | 0.6 | 220 days | `scrap_metal` | Common |

### Tier Structure

| Tier | Inks | Availability | Use Case |
|------|------|-------------|----------|
| **Common** | Lampblack, Berry, Improvised, Sepia, Mineral Oxide | Readily obtainable | Everyday transcription |
| **Uncommon** | Chemical Marker, Diluted Toner | Moderate scarcity | Higher quality work |
| **Rare** | Archival Carbon, Iron Gall | Hard to acquire | Permanent records |
| **Emergency** | Blood | Desperate measure | When nothing else available |

### Balance Guardrails

1. **No mandatory lore gated behind rare inks** — common inks can transcribe anything
2. **Quality affects legibility/fade only** — not access to content
3. **Common inks are economically viable** — players shouldn't grind for basic transcription
4. **Emergency ink is clearly inferior** — low legibility, short longevity, moral weight
5. **Rare inks are for special cases** — permanent records, high-value documents

### Item Dependencies

New items required (must be added to `items.json`):

| Item | Category | Source | Priority |
|------|----------|--------|----------|
| `berries` | Consumable | Foraging/system | HIGH |
| `chemical_solvent` | Material | Industrial salvage | MEDIUM |
| `empty_toner_cartridge` | Component | Office salvage | MEDIUM |
| `mineral_chunk` | Material | Geological salvage | MEDIUM |
| `blood_sample` | Medical | Medical system | LOW (emergency only) |
| `organic_residue` | Material | Biological salvage | MEDIUM |

**Already exists:** `scrap_metal`, `charcoal`, `cloth`

## Transcription Economy

### Current Mechanics

| Parameter | Value | Notes |
|-----------|-------|-------|
| Work day | 8 hours | Standard survivor shift |
| Ink consumption | Per job | Refunded on cancel |
| Legibility score | 0.4–0.95 | Affects transcription quality |
| Archival longevity | 100–600 days | Fade rate varies by ink |
| Duplicate prevention | Yes | `IsEvidenceUnlocked` check |

### Cost Analysis

**Example:** Transcribing a 10-hour document

| Ink | Legibility | Fade/Day | Days to Fade | Quality |
|-----|:----------:|:--------:|:------------:|:-------:|
| Blood (emergency) | 0.4 | 0.010 | 100 | Poor |
| Berry Juice | 0.5 | 0.007 | 150 | Fair |
| Plant Dye | 0.6 | 0.005 | 200 | Acceptable |
| Soot Lamp | 0.7 | 0.003 | 300 | Good |
| Iron Gall | 0.9 | 0.002 | 500 | Excellent |
| Archival Carbon | 0.95 | 0.001 | 600 | Permanent |

**Player choice:** Fast/cheap (blood/berry) vs. slow/expensive (iron gall/archival) vs. balanced (soot/plant).

### Exploit Prevention

1. **No zero-cost repeat transcription** — ink consumed per job
2. **No duplicate codex unlocks** — `IsEvidenceUnlocked` check
3. **Cancel refunds ink** — but time is lost
4. **Unique documents cannot be accidentally consumed** — inventory transaction contract

## Verification

| Check | Status |
|-------|--------|
| Current 3 inks craftable | ✅ PASS (charcoal added) |
| Ink consumption works | ✅ PASS (10 tests) |
| Cancel/refund works | ✅ PASS |
| Duplicate prevention | ✅ PASS |
| Save/load round-trip | ✅ PASS |
| 9 new inks authored | ❌ NOT DONE |
| New item dependencies defined | ❌ NOT DONE |
| Balance tests created | ❌ NOT DONE |

## Next Steps

1. **Add 9 new ink definitions** to `archive_inks.json`
2. **Define missing item dependencies** (berries, chemical_solvent, etc.)
3. **Add balance tests** — verify economy doesn't create grind
4. **Test all 12 inks** — craftable, consumable, save/load
5. **Document tier structure** — player-facing guidance
