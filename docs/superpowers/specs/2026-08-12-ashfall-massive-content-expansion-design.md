# ASHFALL — Massive Content Expansion (Expansion V) — Design Spec

> Status: Draft for review
> Author: Claude (this session)
> Supersedes as working brief: `ASHFALL_PROMPT_CATALOG_EXPANSION.md` Section 10/11 brainstorm items that turn out to already be implemented (see Section 1)

## 1. Context & Why This Shape

The user handed off a master brainstorm doc (originally authored by "antigravity", living at
`~/Desktop/ashfall_Brainstorm_prep.md`) asking for a massive creative expansion: locations, lore,
quests, questlines, choices, factions, character lore, illnesses, items, mechanics, morale choices,
and looting locations with encounters spanning "extreme danger" to "completely abandoned or shelled."

Before writing content, this session audited the brainstorm doc against the actual repo and found it
significantly stale:

- **Section 10 "structural theme" brainstorm is ~85% already built.** `System_BilgePumps.cs`,
  `Project_Minecart.cs`, `ShelterModule_Mortar.cs`, `HallucinationSystem.cs`,
  `ShelterModule_Confessional.cs`, `GraftRejectionSystem.cs`, `VehicleSystem.cs` +
  `VehicleMaintenanceSystem.cs`, `DeepAquiferProjectSystem.cs`, `PeaceTreatySystem.cs`,
  `CulturalPreservationSystem.cs`, `BunkerManifestoSystem.cs`, `FactionIntelligenceSystem.cs` all
  already exist. Even `Affliction_SporeLung.cs` — the doc's own "write this C# system from scratch"
  example prompt — already exists.
- **Confirmed genuine gaps**: `ShelterModule_SubBay` (the flooded room itself — only the pumps that
  would drain it exist), `FalloutForecastSystem`, and real depth behind the lean 104-line
  `VehicleSystem.cs`.
- **Baseline content is already large**: 419 items, 100 survivors, 67 locations (`locations.json` +
  `locations_expansion3.json`), 49 events, 15 echoes, 32 recipes, 5 factions, plus ~20 peripheral
  narrative/lore JSON catalogs (world history, faction lore, questlines, confessions, final wishes,
  trade specialties, etc.).
- **Illness/pathology roster is deep**: 27 named `Affliction_*.cs` classes already cover ARS, scurvy,
  frostbite, tetanus-adjacent, radiation blindness, hallucinations, TBI, phantom limb, thyroid cancer,
  sterility, zoonotic flu, the bends, spore lung, and more.
- **Endgame is deeper than documented**: 15 `Victory_*.cs` paths already exist, not the doc's claimed 8.

**Implication for this spec**: put expansion weight on content (additive by nature — a new named
location/survivor/item cannot collide with an existing system) and reserve new C# for confirmed gaps
plus a small number of well-justified fresh mechanics. Do not re-propose anything on the "already
exists" list above.

**Current blocker status**: a Unity compile-verification pass was attempted for the pre-existing
uncommitted Phase 11 UI work and blocked on a stale orphaned `AssetImportWorkerHW0` process (dead
parent PID, matches existing `mono_crash.mem.*` crash debris in the repo). At the user's direction this
is **deferred, not abandoned** — it will need to be resolved before any batch below that touches C#
(illnesses and new systems), since those need a real compile check per `AGENTS.md`'s verify-before-done
rule. Pure-JSON batches do not need Unity and are not blocked.

## 2. Goals / Non-Goals

**Goals:**
- Deliver a genuinely massive wave of new, non-duplicate content across the categories the user named.
- Keep every new id unique and snake_case; keep every new file additive (no edits to the in-flight,
  uncommitted Phase 11 files) so this work can proceed in parallel without merge conflicts.
- Match existing schemas, tone ("cold, exhausted, human, restrained", zero magic/aliens/sci-fi per
  `AGENTS.md`), and the established data pipeline (`JsonDataImporter` for the 6 core catalogs;
  dedicated lightweight loaders for peripheral catalogs, following the `PhantomTriggerCatalogLoader`
  pattern).
- Land each category as its own reviewable commit, in dependency order.

**Non-goals:**
- Not fixing or touching the in-flight Phase 11 UI/wiring files (separate, deferred workstream).
- Not re-implementing any system already confirmed to exist (Section 1 list).
- Not adding new top-level victory paths (15 already exist against the doc's assumed 8).
- Not padding the illness category to hit a round number if genuine non-overlapping pathologies run
  out — quality and distinctness over count.

## 3. Category Breakdown & Target Counts

All counts are additive to current totals. "Go bigger" scale per user direction. Subsections below are
ordered to match the table and the batch order in §5.

| # | Category | Target | Depends on | Touches C#? |
|---|---|---|---|---|
| 1 | Items | +100 | — | No (JSON, `items.json`) |
| 2 | Illnesses/pathologies | +10 (hard cap, see §3.2) | Items (treatment items) | **Yes** — one `Affliction_*` class per illness, matching existing pattern |
| 3 | New systems + minimal data | 5 systems + 1 content-only extension | Items | **Yes** |
| 4 | Locations + encounters | +50 locations, ~200 encounters | Items, illnesses, new systems | No (JSON) |
| 5 | Named survivors | +32 (10 with full 4-stage arcs) | Items, locations (backstory ties) | No (JSON) — arcs reuse `SurvivorNarrativeArcSystem`, no new C# |
| 6 | Faction depth | 15 sub-factions/leaders (3 per faction), 16 faction questlines | Survivors (named leaders) | No (JSON) |
| 7 | Moral dilemma events | +50 | All of the above (for flavor references) | No (JSON, `events.json`) |
| 8 | Questlines | +20 multi-stage | Survivors, factions, locations | No (JSON, reuses `QuestlineSO`/`DynamicQuestlineSystem`) |
| 9 | Echoes (relics) | +25 | — | No (JSON) |
| 10 | Radio broadcasts | +35 | Factions (propaganda), world state | No (JSON) |

Total: ~550 new content entries (100 items + 10 illnesses + 50 locations + ~200 encounters + 32
survivors + 31 faction entities/questlines + 50 moral events + 20 questlines + 25 echoes + 35 radio), 5
new C# systems, ~10 new affliction classes, 1 content-only vehicle extension.

### 3.1 Items (+100)

Weighted allocation (sums to 100):

| Sub-category | Count |
|---|---|
| SubBay/diving gear | 15 |
| Vehicle parts, fuel, tools | 15 |
| Livestock/agriculture tools | 10 |
| Forward Outpost / caravan-related gear | 10 |
| Faction-specific signature gear (one small set per faction) | 15 |
| Illness treatment items (for the new pathologies in §3.2) | 10 |
| Haunting relics / comfort items | 10 |
| General world-building fill (tools, materials, trade goods) | 15 |

### 3.2 Illnesses (+10, hard cap — highest duplication risk)

Starting hypothesis, **subject to a mandatory side-by-side check against all 27 existing
`Affliction_*.cs` classes before any are written** — some of these may get cut or merged if they turn
out to overlap:

1. Crush Syndrome (delayed-onset, rubble-clearing injury)
2. Silicosis / concrete-dust lung (chronic, clearing collapsed structures without a respirator)
3. Sub-pen decompression variant (pending check against existing `Affliction_TheBends.cs`)
4. Heavy-metal poisoning from scrap handling (pending check against existing `Affliction_LeadMadness.cs`)
5. Electrolyte crisis / salt deficiency (distinct from existing scurvy)
6. Fungal wound infection (wound-vector, distinct from existing respiratory `Affliction_SporeLung.cs`)
7. Welding/solar keratitis (acute eye damage, distinct from long-term radiation cataracts)
8. Zoonotic livestock fever (new strain tied to the new husbandry system in §3.3, distinct from existing
   `Affliction_ZoonoticFlu.cs`)
9. Non-freezing chronic cold injury (hands, repeated exposure — distinct from existing
   `Affliction_TrenchFoot.cs`)
10. "Road sickness" / caravan fatigue syndrome (motion + stress + poor sleep, ties to vehicle depth)

Each new illness is a small `Affliction_*.cs` class matching the existing pattern (see
`AfflictionSO.cs` base), not pure JSON — this batch touches C# and needs compile verification.

### 3.3 New Systems (the only new C# in this pass)

1. **`ShelterModule_SubBay`** (Shelter/Modules) — the flooded sub-pen bay room itself: water-depth
   state, structural pressure, seal-failure risk. Consumes bilge-pump throughput from the existing
   `System_BilgePumps.cs` rather than reimplementing pumping. Unlocks diving-based scavenging once
   drained/managed. ~100–150 lines, save-safe, event-driven, matches the existing `ShelterModule_*`
   pattern.
2. **`FalloutForecastSystem`** (Environment) — predictive rad/weather forecasting with uncertainty,
   hooks into the existing `WeatherSystem`, informs expedition planning risk. ~80–120 lines.
3. **Livestock/mutated-animal husbandry system** (new — e.g. `LivestockHusbandrySystem` in Shelter or
   Survivors) — breeding ash goats/mutant chickens for food+trade; needs a new `ShelterModule_Pen` (or
   similar) and feeds into the new zoonotic illness in §3.2. ~150–200 lines.
4. **Forward Outpost / surface camp system** — establish a small secondary camp at a chosen map node,
   cutting travel time to that region, requiring periodic resupply, vulnerable to raids. ~150–200
   lines.
5. **Traveling caravan / convoy encounters** — dynamic NPC merchant convoys on the map the player can
   intercept, escort, trade with, or rob; ties into the existing `GeneratedMap` node system,
   Encounters, and `DynamicEconomySystem` rather than building a new economy. ~150–200 lines.
6. **Vehicle depth (content-only, no new class)** — more vehicle-type entries in whatever catalog
   backs `VehicleSystem.cs`, plus fuel-siphoning and road-breakdown encounter content.

Before writing each system, re-grep for its name and close synonyms — this repo has repeatedly shown
things exist under names not in the brainstorm doc.

### 3.4 Locations + Encounters

Danger-tier bands (matching the existing `danger_level` field):

- **Tier 1 — Abandoned/Shelled, low danger** (`danger_level` 1–3): 12 locations. Picked-over ruins,
  low reward, safe scavenging, good early-game or desperate late-game runs.
- **Tier 2 — Contested/Moderate** (4–6): 16 locations.
- **Tier 3 — High Danger** (7–8): 14 locations.
- **Tier 4 — Extreme/Anomalous** (9–10+): 8 locations. Rare, endgame-tier, several tied to the new
  systems in §3.3 (e.g. the SubBay flood site, a Forward Outpost candidate site).

Each location gets 3–5 encounter entries spanning: pure loot, combat/ambush, environmental hazard, and
narrative/moral-dilemma-linked. Target ~200 encounters total.

### 3.5 Named Survivors (+32)

22 archetype-level entries (rich bio, profession, belief profile, latent trait — matching the
`survivors.json` baseline pattern) + 10 with full 4-stage narrative arcs (Discovery → Investigation →
Crisis → Resolution), reusing the existing `SurvivorNarrativeArcSystem` the same way
`aris_thorne`/`maya_lin`/`victor_vance`/`elena_rostov` already do. Professions chosen to not duplicate
the existing 100.

### 3.6 Faction Depth

For each of the 5 existing factions (Central Garrison Remnants, Upland Provincial Militia, Cultists of
the Glow, Scavenger Warlords, Rebuilders): 1 named leader + 3 named sub-factions/warbands/cells (loyal,
semi-independent, or splinter) = 15 new faction-adjacent entities. Plus 16 faction questlines
distributed across the 5 (weighted toward the ones with the least existing questline coverage) and
diplomacy/trade encounter content hooking into the existing `FactionIntelligenceSystem` /
`PeaceTreatySystem`. No new top-level factions — 5 is treated as canonical.

### 3.7 Moral Dilemma Events (+50)

Distributed across shelter-internal, expedition, faction, medical-triage, and resource-scarcity
dilemmas, matching the existing `events.json` schema (`id`, `title`, `min_day`, `weight`, `description`,
`choices[]` with `text` + `consequence_text`). Dilemmas with lasting character impact hook into the
existing `MoralBranchingSystem` the same way current entries do.

### 3.8 Questlines (+20)

Multi-stage, using the existing `QuestlineSO` / `DynamicQuestlineSystem` pattern, chaining new
survivors + factions + locations + items into coherent arcs rather than standalone content.

### 3.9 Echoes (+25) & Radio (+35)

Match existing `echoes.json` (haunting prose fragment) and `radio.json` (frequency/phase/text) schemas.
Radio distributed across civilian panic, military tactical, numbers stations, automated emergency
loops, and faction propaganda.

## 4. Data & Schema Conventions

- snake_case ids everywhere; never reuse an existing id (each batch starts with a grep against the
  relevant catalog for collisions).
- The 6 core catalogs (`items`, `recipes`, `survivors`, `locations`, `events`, `radio`) go through
  `JsonDataImporter` with strict schema + referential-integrity validation (e.g. recipe ingredients
  must resolve to real item ids) — run `Tools/ASHFALL/Validate Data (no import)` after each batch that
  touches these.
- Peripheral catalogs (faction lore, questlines, echoes extensions, illness treatment tags, etc.) get
  their own small loader if one doesn't already cover them, following the existing
  `PhantomTriggerCatalogLoader` pattern.
- New item/location/survivor/faction descriptions should stay compatible with the established AI-art
  prompt conventions in `docs/ai-art/PROMPT_RULES.md` / `CONSISTENCY_ANCHORS.md` (grounded realism,
  cold restrained palette, no text/logos/gore) so they can be prompted later without rework.

## 5. Batch Order (dependency-sequenced, each batch = one commit)

1. Items (+100)
2. Illnesses (+10) — needs Unity compile verification (stale-lock issue must be resolved first)
3. New systems (5) + vehicle depth extension — needs Unity compile verification
4. Locations + encounters (+50 / ~200)
5. Named survivors (+32)
6. Faction depth (15 entities + 16 questlines)
7. Moral dilemma events (+50)
8. Questlines (+20)
9. Echoes (+25)
10. Radio (+35)

Given the size, batches will be executed across multiple turns/sessions rather than one uninterrupted
burst — each is independently committable so nothing is lost if the session ends mid-sequence.

## 6. Testing & Verification

- Pure-JSON batches (1, 4–10): validate JSON syntax + referential integrity (via
  `JsonDataImporter.ValidateAll()` for the 6 core catalogs, or manual cross-reference checks for
  peripheral ones) before committing. No Unity Editor launch required.
- C#-touching batches (2, 3): resolve the stale Unity lock, then run a `-quit`-only compile pass (per
  the project's established `unity-test-cli` pattern) and fix any errors before committing. Add a small
  EditMode test per new system/affliction (construction, state transition, save/load round-trip),
  matching the existing `RadiationPhaseProgressionTests` / `ExpansionIntegrationTests` style. Full
  `-runTests` PlayMode runs are treated as periodic milestones, not a per-batch gate, given documented
  native-runtime instability with `-runTests` on this machine.

## 7. Risks & Mitigations

- **Duplication risk** (highest in illnesses, moderate in new systems): mandatory grep-before-write
  step at the start of each of those batches.
- **Stale Unity lock blocking C# batches**: flagged now; will need the user's go-ahead to clear it
  before batch 2 or 3 starts.
- **Merge conflicts with in-flight Phase 11 work**: mitigated by only adding new files / appending to
  data catalogs, never editing the currently-modified Phase 11 files listed in `git status`.
- **Scope fatigue over ~530 entries**: batches are independently valuable and committable, so partial
  completion across sessions is an acceptable outcome, not a failure state.
