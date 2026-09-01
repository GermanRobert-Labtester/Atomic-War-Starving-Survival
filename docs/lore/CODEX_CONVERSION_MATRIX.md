# Plan 17 — Codex Conversion Matrix

Maps dev-lore documents to player-safe codex entries, with spoiler classification and gating requirements.

## Source Documents

| Source File | Lines | Player-Safe % | Codex Candidates | Spoiler Risk |
|-------------|-------|:-------------:|:----------------:|:------------:|
| `01_GAZETTEER.md` | 196 | ~75% | ~9 | LOW-MED |
| `03_LOCATIONS.md` | 446 | ~60% | ~60 | MEDIUM |
| `04_ENCOUNTERS.md` | 317 | ~40% | ~30 | HIGH |
| `05_FACTIONS.md` | 484 | ~50% | ~22 | MED-HIGH |
| `06_REBUILDERS_AND_BLACK_OPS.md` | 448 | ~25% | ~6 | HIGH |
| `02_THE_LIST.md` | 258 | ~10% | ~24 (gated) | **EXTREME** |
| `IntelBible.md` | 267 | ~60% | ~5 + 50 radio | LOW |
| **TOTAL** | **2,416** | | **~156** | |

## Conversion Priority

### Tier 1 — Safe to Convert Now (Low Spoiler Risk)

| Source | Entries | Content |
|--------|---------|---------|
| `01_GAZETTEER.md` | 5 | Sub-region descriptions (The Grid, The Verge, The Spine, The Toll, The Drown) |
| `01_GAZETTEER.md` | 4 | Military installations (redacted: remove "still has authority" late-game reveals) |
| `03_LOCATIONS.md` | 40 | Location descriptions (explicitly player-facing in source) |
| `IntelBible.md` | 5 | Radio broadcast lore fragments |
| **Subtotal** | **54** | |

### Tier 2 — Convert with Gating (Medium Spoiler Risk)

| Source | Entries | Content | Gating |
|--------|---------|---------|--------|
| `05_FACTIONS.md` | 14 | Faction descriptions, quotes, wants/offers | Discovery-gated (meet faction) |
| `04_ENCOUNTERS.md` | 10 | NPC descriptions and quotes | Discovery-gated (meet NPC) |
| `04_ENCOUNTERS.md` | 8 | Echo texts | Location-gated (visit site) |
| `06_REBUILDERS_AND_BLACK_OPS.md` | 2 | Faction descriptions (Rebuilders, D/9) | Discovery-gated |
| `06_REBUILDERS_AND_BLACK_OPS.md` | 2 | Character descriptions (Ottilie, Anneke) | Discovery-gated |
| **Subtotal** | **36** | | |

### Tier 3 — Heavily Gated (High Spoiler Risk)

| Source | Entries | Content | Gating |
|--------|---------|---------|--------|
| `02_THE_LIST.md` | 16 | World history entries | Day-gated + knowledge-gated |
| `02_THE_LIST.md` | 1 | Archivists faction | Deep-lore gated (Tier 3) |
| `02_THE_LIST.md` | 2 | Character entries (Margit Sole, Sela Renn) | Late-game gated (Tier 4) |
| `04_ENCOUNTERS.md` | 4 | Trust-reactive scenes | Trust-threshold gated |
| **Subtotal** | **23** | | |

### Never Player-Facing

| Source | Content | Reason |
|--------|---------|--------|
| `00_OVERVIEW.md` | Spine thesis, discovery ladder, mechanics | Core mystery structure |
| `02_THE_LIST.md` | Discovery ladder as structure, formula, branch outcomes | Spoilers entire mystery |
| All files | Code references, schema instructions, design commentary | Dev-only |
| All files | "See 02_THE_LIST.md" cross-references | Spoiler chain |
| All files | Spine-layer tags ("Spine layer 2/3", "Spine — critical") | Meta-structure |

## Codex Category Architecture

### Current State (JournalCodex)

5 tabs: Log, Items, People, Places, Events

### Proposed Expansion

| Category | Tab | Entry Count | Source |
|----------|-----|:-----------:|--------|
| Regions | Places | 5 | Gazetteer sub-regions |
| Locations | Places | 40+ | Location descriptions |
| Factions | People | 23+ | Faction lore |
| History | Events | 79+ | World history (gated) |
| Military Installations | Places | 4 | Gazetteer (redacted) |
| NPCs | People | 10+ | Encounter descriptions |
| Deep Lore | Places | 10 | Deep-lore locations |
| Documents | Log | 15+ | Transcribed documents |
| **Total** | | **~186** | |

### Category Navigation

- Keep existing 5 tabs for backward compatibility
- Add sub-categories within Places (Regions, Locations, Military, Deep Lore)
- Add sub-categories within People (Factions, NPCs)
- Add History as new top-level tab
- Add Documents as new top-level tab (transcribed archive content)

## Spoiler Tier Classification

| Tier | Definition | Unlock Condition | Examples |
|------|-----------|------------------|---------|
| 0 | Common knowledge | Available from start | Basic survival info, bunker systems |
| 1 | Ordinary discovery | Visit location, find document | Location descriptions, basic faction info |
| 2 | Faction/region restricted | Meet faction, explore region | Faction details, regional history |
| 3 | Deep lore | Reach deep-lore site, complete quest | Hidden faction history, restricted records |
| 4 | Late-game/endgame | Day threshold, story progress | Spine mystery reveals, twist identities |

## Conversion Rules

1. **Strip all dev commentary** — code refs, schema instructions, design rationale
2. **Strip all cross-file spoilers** — "See 02_THE_LIST.md", spine-layer tags
3. **Reframe through in-world voice** — "survivor's almanac" not "wiki entry"
4. **Preserve uncertainty** — where sources disagree, show disagreement
5. **Gate spoiler titles** — if a locked entry's title leaks a twist, generalize it
6. **Maintain provenance** — every entry should have an implied source
7. **No omniscient narrator** — present as collected knowledge, not absolute truth

## Voice Guidelines

**Good (in-world):**
> Known: The district handled refrigerated cargo before the Exchange.
> Recorded: Three evacuation manifests list the same eastbound convoy.
> Disputed: Foundry accounts insist the convoy arrived. No northern record confirms it.

**Bad (developer wiki):**
> The South Freight District was a pre-war logistics hub that played a key role in the evacuation.

## Verification

| Check | Status |
|-------|--------|
| 40+ player-safe entries authored | ❌ NOT DONE |
| No dev commentary exposed | Pending |
| Spoiler-sensitive facts gated | Pending |
| Entries read as in-world knowledge | Pending |
| Category navigation works | Pending |
| Locked titles don't leak spoilers | Pending |
