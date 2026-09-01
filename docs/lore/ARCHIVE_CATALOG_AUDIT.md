# Plan 17 — Archive Catalog Audit

Audits the archive desk transcription system, ink catalog, and document discovery pipeline.

## Current State

### Archive Ink Catalog (archive_inks.json)

| Ink ID | Display Name | Legibility | Longevity | Fade Rate | Required Item | Amount |
|--------|-------------|:----------:|:---------:|:---------:|---------------|:------:|
| `ink_iron_gall` | Iron Gall Ink | 0.9 | 500 days | 0.002/day | `charcoal` | 2 |
| `ink_soot_lamp` | Soot Lamp Ink | 0.7 | 300 days | 0.003/day | `charcoal` | 1 |
| `ink_plant_dye` | Plant Dye Ink | 0.6 | 200 days | 0.005/day | `cloth` | 1 |

**Critical issue:** `charcoal` item is NOT defined in `items.json`. Two of three inks are uncraftable.

### Archive Desk System

- **Location:** `Assets/Ashfall.Core/ArchiveDeskSystem.cs` (196 lines)
- **Host:** `ArchiveDeskHostSession` (src/Host/)
- **Tests:** `ArchiveDeskSystemTests.cs` (10 tests)
- **Save:** `ArchiveDeskSaveStore` (checksummed envelope)

**Pipeline:** Evidence → Queue transcription → Consume ink → Tick day (8-hour work) → Create journal entry + unlock knowledge

### Transcription Economy

| Parameter | Value | Notes |
|-----------|-------|-------|
| Work day | 8 hours | Standard survivor shift |
| Ink consumption | Per job | Refunded on cancel |
| Legibility score | 0.6–0.9 | Affects transcription quality |
| Archival longevity | 200–500 days | Fade rate varies |
| Duplicate prevention | Yes | `IsEvidenceUnlocked` check |

## Plan 17F: Ink Expansion (3 → 12)

### Proposed New Inks

| Ink ID | Display Name | Material Origin | Legibility | Longevity | Required Item |
|--------|-------------|-----------------|:----------:|:---------:|---------------|
| `ink_lampblack` | Lampblack | Oil lamp soot | 0.65 | 250 days | `charcoal` |
| `ink_berry_juice` | Berry Juice | Crushed berries | 0.5 | 150 days | `berries` |
| `ink_chemical_marker` | Chemical Marker | Industrial chemical | 0.8 | 400 days | `chemical_solvent` |
| `ink_diluted_toner` | Diluted Toner | Salvaged copier toner | 0.75 | 350 days | `empty_toner_cartridge` |
| `ink_archival_carbon` | Archival Carbon | Pure carbon suspension | 0.95 | 600 days | `charcoal` |
| `ink_improvised_pigment` | Improvised Pigment | Ground minerals | 0.55 | 180 days | `mineral_chunk` |
| `ink_blood_emergency` | Blood (Emergency) | Human/animal blood | 0.4 | 100 days | `blood_sample` |
| `ink_sepia` | Sepia Wash | Cuttlefish/organic | 0.7 | 280 days | `organic_residue` |
| `ink_mineral_oxide` | Mineral Oxide | Rust/iron oxide | 0.6 | 220 days | `scrap_metal` |

### Balance Guardrails

- **Common inks** (lampblack, soot, plant dye): Readily obtainable, low cost
- **Rare inks** (archival carbon, chemical marker): Higher quality, harder to acquire
- **Emergency ink** (blood): Low quality, short longevity, treat as desperate measure
- **No mandatory lore gated behind rare inks** — common inks can transcribe anything
- **Quality affects flavor/legibility only** — not access

### Item Dependencies

New inks require these items (must be defined in `items.json`):
- `charcoal` — **ALREADY MISSING** (critical fix)
- `berries` — may exist in foraging system
- `chemical_solvent` — industrial material
- `empty_toner_cartridge` — office salvage
- `mineral_chunk` — geological salvage
- `blood_sample` — medical supply
- `organic_residue` — biological material
- `scrap_metal` — **EXISTS** (items.json)

## Document Discovery Pipeline

### Current State

- **No dedicated document discovery/loot system exists**
- Documents are referenced in narrative JSON but have no physical discovery path
- Archive desk transcribes "evidence IDs" but evidence acquisition is undefined

### Required Integration

1. **Document placement** — each document must have a discovery source:
   - Location loot table
   - Expedition reward
   - Quest completion
   - Faction reward
   - Archive cache
   - Trade goods

2. **Document → Archive flow:**
   ```
   Discover document (loot/quest/reward)
   → Add to inventory/evidence list
   → Queue at Archive Desk
   → Consume ink + time
   → Create journal entry
   → Unlock knowledge/codex
   ```

3. **Prevent exploits:**
   - No duplicate transcription rewards
   - No zero-cost repeat transcription
   - Unique documents cannot be accidentally consumed
   - Codex unlocks persist through save/load

## Verification

| Check | Status |
|-------|--------|
| Archive desk save/load | ✅ Tested (10 tests) |
| Ink consumption | ✅ Tested |
| Cancel/refund | ✅ Tested |
| Duplicate prevention | ✅ Tested |
| Charcoal item defined | ❌ MISSING |
| Document discovery system | ❌ NOT IMPLEMENTED |
| Document placement matrix | ❌ NOT CREATED |

## Next Steps

1. **Fix charcoal item** — add to items.json (blocking)
2. **Add 9 new inks** — expand archive_inks.json to 12
3. **Define missing item deps** — berries, chemical_solvent, etc.
4. **Create document discovery system** — or integrate with existing loot/expedition
5. **Create DOCUMENT_DISCOVERY_MATRIX.md** — map all documents to sources
6. **Add balance tests** — verify ink economy doesn't create grind
