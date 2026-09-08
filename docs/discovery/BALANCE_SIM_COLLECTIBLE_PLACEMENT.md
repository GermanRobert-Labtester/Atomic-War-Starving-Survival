# Balance Sim — Collectible Placement Tables (Plan 86 Follow-Up)

> **System:** collectible scavenging placement (48 entries / 40 collectibles / 20 touched tables),
> find-rate funnel, merchant revenue mix.
> **Method:** deterministic headless xUnit harness, `ISeededRng` (xorshift64*) only; read-only
> over the data authority. No production data was modified for this report.
> **Skill:** `ashfall-balance-sim` (report filed to `docs/discovery/` at the requester's direction;
> the skill's default output path is `docs/balance/`).

---

## 1. Knobs & ownership

| Knob | Authority | Owner |
|---|---|---|
| Loot entry `weight` per table | `scavenging_tables.json` | data authority |
| Collectible `rarity` / `unique` | `collectibles.json` | data authority |
| Collectible `tradeValue` | `items.json` | data authority |
| Merchant collectible draw probability (0.08) | test harness constant (`CollectibleMerchantSimulationTests`) | test |
| Find-rate gate (0.02–0.50) | `CollectibleScavengingSimulationTests` assertion | test |

## 2. Seed manifest

| Part | Simulation | Seeds | Rolls/actions |
|---|---|---|---|
| A — per-table distribution | 10,000 × `RollLoot` per touched table | `SeededRng(860000 + tableIndex)`, tables in ordinal id order | 200,000 rolls |
| B — find-rate funnel | 100 actions (5 × 20 target tables) | `SeededRng(1..200)` | 20,000 rolls |
| C — merchant revenue mix | 1,000 transactions | `SeededRng(42, 43, 44)` (network seed 42) | 3,000 trades |

---

## 3. Part A — per-table 10,000-roll distribution audit (SEED VERDICT: all in-band)

Observed collectible share vs the expected weight share, 3σ binomial band. **20/20 tables inside
the band — the roll implementation tracks authored weights with no skew.**

| Table | Expected | Observed | 3σ band | Verdict |
|---|---:|---:|---|---|
| `table_loot_apartment_block` | 0.1121 | 0.1139 | [0.1026, 0.1215] | OK |
| `table_loot_military_depot` | 0.1053 | 0.1096 | [0.0961, 0.1145] | OK |
| `table_loot_police_station` | 0.0735 | 0.0746 | [0.0657, 0.0814] | OK |
| `table_loot_concert_hall` | 0.0680 | 0.0649 | [0.0605, 0.0756] | OK |
| `table_loot_school` | 0.0601 | 0.0622 | [0.0530, 0.0672] | OK |
| `table_loot_conscription_office` | 0.0496 | 0.0492 | [0.0431, 0.0561] | OK |
| `table_loot_fire_station` | 0.0495 | 0.0488 | [0.0430, 0.0560] | OK |
| `table_loot_pilgrim_hearth` | 0.0476 | 0.0463 | [0.0412, 0.0540] | OK |
| `table_loot_industrial_district` | 0.0461 | 0.0448 | [0.0398, 0.0524] | OK |
| `table_loot_clinic` | 0.0479 | 0.0454 | [0.0415, 0.0543] | OK |
| `table_loot_hospital` | 0.0394 | 0.0370 | [0.0336, 0.0452] | OK |
| `table_loot_metro_station` | 0.0312 | 0.0315 | [0.0260, 0.0365] | OK |
| `table_loot_checkpoint` | 0.0276 | 0.0278 | [0.0227, 0.0325] | OK |
| `table_loot_transit_depot` | 0.0270 | 0.0263 | [0.0222, 0.0319] | OK |
| `table_loot_printworks` | 0.0236 | 0.0249 | [0.0191, 0.0282] | OK |
| `table_loot_waterworks` | 0.0221 | 0.0222 | [0.0177, 0.0265] | OK |
| `table_loot_tinkers_notch` | 0.0217 | 0.0216 | [0.0174, 0.0261] | OK |
| `table_loot_relay_mast` | 0.0166 | 0.0152 | [0.0127, 0.0204] | OK |
| `table_loot_recovery_yard` | 0.0149 | 0.0144 | [0.0113, 0.0186] | OK |
| `table_loot_collapsed_structure` | 0.0105 | 0.0118 | [0.0075, 0.0136] | OK |

Hottest single items: `family_portrait` (329 hits/10k at apartment_block),
`unit_photograph` (213/10k, depot), `civil_defense_poster` (211/10k, police) — all
the plan-exact required placements, none crowding out survival loot (share ≤ 12% gate holds).

## 4. Part B — find-rate funnel (200 seeds × 100 actions)

100-action window = 5 actions × the 20 `CollectibleScavengingSimulationTests` target tables.

| Metric | Value |
|---|---|
| Mean find rate | **0.0268** (≈ 2.7 collectibles per 100 actions) |
| Median | 0.03 |
| p05 / p95 | **0.00** / 0.06 |
| Min / max | 0.00 / 0.08 |
| Seeds below the 0.02 CI gate | **51 / 200 (25.5%)** |
| Seeds above the 0.50 CI gate | 0 / 200 |

### Finding B-1 (medium): 12 of 20 funnel tables are collectible-barren

`power_substation`, `chemical_plant`, `shopping_center`, `ordnance_shoulder`,
`government_bunker`, `warehouse`, `municipal_archive`, `swimming_baths` have **zero**
collectible placements, diluting the funnel by 40%. Several are thematically strong
candidates (`municipal_archive` for newspapers/documents, `government_bunker` for
military documents, `shopping_center` for cultural goods).

### Finding B-2 (medium): the 0.02 find-rate gate is seed-fragile

The CI test pins seed 42 (deterministic PASS), but **25.5% of the seed space** falls
below the 0.02 floor of its own `Assert.InRange(0.02, 0.50)` — and p05 = 0.00 means
1 in 20 campaigns sees zero collectibles in a 100-action window. Not a defect for a
fixed-seed test, but the design intent ("collectibles exist") is not met for a
quarter of seed space at this sample size.

## 5. Part C — merchant revenue mix (1,000 transactions × 3 seeds)

| Seed | Total rev | Collectible rev | Share | Dominance | Distinct sold |
|---|---:|---:|---:|---:|---:|
| 42 | 3,438 | 351 | **10.22%** | 6.2% | 37/40 |
| 43 | 3,459 | 280 | 8.09% | 7.0% | 37/40 |
| 44 | 3,516 | 302 | 8.59% | 7.1% | 34/40 |

| Rarity | Revenue share of collectible sales (seed 42) |
|---|---:|
| rare | 64.3% |
| uncommon | 24.1% |
| common | 11.6% |

### Finding C-1 (healthy): revenue ceiling has real headroom

Share 8.1–10.2% against the 20% ceiling and the 50-transaction CI gate; no dominance
risk (max 7.1% vs the 35% gate). The Plan 86 rebalance (`water_treatment_handbook`
22→18) landed the economy in a comfortable band — no further trade-value tuning is
evidenced.

### Finding C-2 (note): rare-collectible revenue concentration

Rares deliver ~64% of collectible revenue at ~25% of catalog count. Coherent with the
rarity economy (rares also gate knowledge research), but means merchant collectible
economics are effectively "hunt rares". Watch if uncommon/common collectibles are
meant to matter as trade goods.

## 6. Proposals — APPLIED (P1+P2, 2026-09-08)

> **Executed as data-only changes** (see commit history, "plan86-balance"):
>
> - **P1 executed** — 8 theme-matched secondary placements (weight 3) into the
>   barren funnel tables: `municipal_archive`←local_newspaper,
>   `government_bunker`←deployment_order, `shopping_center`←team_pennant,
>   `power_substation`←trade_guild_patch, `chemical_plant`←dosimeter_guide,
>   `ordnance_shoulder`←topo_map, `warehouse`←folk_craft,
>   `swimming_baths`←match_program. All 20 CI target tables now yield.
> - **P2 executed** — `music_box` 2→3 (collapsed_structure),
>   `survivor_map` 3→4 (recovery_yard).
> - **Post-change gates verified:** 40/40 placed (56 entries, 28 touched
>   tables), max per-table collectible share 11.2% (≤12% gate), reachability,
>   no-dupes and unique-placement all hold.
> - **Expected funnel mean 0.0356** (analytic) vs 0.0268 pre-change; a
>   permanent 200-seed CI gate (`Funnel200_MeanFindRate_StaysAtOrAbove3Percent`
>   in `CollectibleScavengingSimulationTests`) now asserts the mean stays
>   ≥0.03. Finding B-2's seed-fragility is materially reduced (all 20 target
>   tables can yield), though sub-0.02 seeds can still occur by low-weight
>   sampling.

Original proposals (for the record):

1. **P1 — give the 8 barren funnel tables placements** (B-1): 8 tables × 1–2 entries
   at weight 2–4 would lift the mean find rate to ≈ 0.035 and shrink the sub-gate seed
   fraction materially. Candidates: `municipal_archive` ← `local_newspaper` (secondary),
   `government_bunker` ← `deployment_order` (secondary), `shopping_center` ←
   `team_pennant`/`civic_token` (secondary). All secondary placements — provenance
   reachability unaffected.
2. **P2 — if the 0.02 gate is meant as a design floor, raise the two weakest required
   tables** (`collapsed_structure` 0.0105, `recovery_yard` 0.0149 — both already touched)
   by +1 weight on their single collectible. Evidence: the floor is breached by 25.5%
   of seed space at current weights (Part B).
3. **P3 — no merchant trade-value changes** (C-1): current values sit in-band with
   2× ceiling headroom.

## 7. Reproduction

Harness: ephemeral xUnit diagnostic (removed after the run), `Ashfall.Core.Tests`,
logic preserved verbatim in §2–§5 above: `ScavengingTableCatalog.RollLoot(tableId, rng)`
with `SeededRng(860000 + index)` per table (A); 5×20 action windows with `SeededRng(1..200)` (B);
`CaravanTradeNetworkSystem.CalculateItemSellPrice` with draw probability 0.08 and
`SeededRng(42/43/44)` (C). Raw results: `/tmp/balance_collectible.json`
(20 × 10k per-item histograms included).
