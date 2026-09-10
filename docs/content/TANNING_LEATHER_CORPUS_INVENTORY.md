# TANNING LEATHER CORPUS INVENTORY — Plan 159

> Evidence-based inventory of all 30 authored `TanningLeatherCatalog` records
> (source: `Assets/StreamingAssets/Data/narrative/*.json`, the data authority;
> loader: `Assets/Ashfall.Core/Narrative/TanningLeatherCatalog.cs`; runtime
> projection: `Assets/Ashfall.Core/Narrative/LeatherworkArchiveSystem.cs`).
> This document is read-only analysis; it authorizes no mechanical behavior.

## 1. Corpus summary

| Family | File | Count | ID prefix | Timestamps |
|---|---|---|---|---|
| Oak-bark vegetable tanning (pit logs) | `oak_bark_tanning_pit_logs.json` | 8 | `oak_bark_tan_` | `YEAR_02` → `YEAR_20` |
| Mineral tanning liquor assays | `chrome_alum_tanning_assays.json` | 8 | `mineral_tan_` | `YEAR_02` → `YEAR_20` |
| Rawhide deliming/bating failures | `rawhide_bating_failure_reports.json` | 7 | `rawhide_bate_` | `YEAR_03` → `YEAR_20` |
| Harness currying audits | `leather_harness_conditioning_audits.json` | 7 | `leather_harness_` | `YEAR_02` → `YEAR_20` |
| **Total** | 4 files | **30** | — | spans ~20 post-war years |

All four files carry top-level `schema_version` and pass
`--data-integrity-selftest` (299 catalogs, 0 errors). IDs are unique across
families (pinned by `LeatherworkArchiveTests.RecordIds_Unique_AcrossAllFamilies`).

## 2. Full record inventory (§7 Workstream A)

### Oak-bark vegetable tanning (pit logs) — `oak_bark_tanning_pit_logs.json` (8 records)

| # | record id | facility label | material/agent | measurements | timestamp | tags | prose topic |
|---|---|---|---|---|---|---|---|
| 1 | `oak_bark_tan_chestnut_liquor_density` | `LAY_AWAY_PIT_ROW_ALPHA_01` | `QUERCUS_PRINOIDES_CHESTNUT_OAK` | 48°Bk / 12 mo | `YEAR_02_TANNERY_LOG_01` | tanning, vegetable_tan, oak_bark, barkometer, sole_leather | Chestnut-oark bark layered between bull hides; 12-month cold steep yields dense water-resistant sole leather |
| 2 | `oak_bark_tan_pit_sour_fermentation_bloom` | `SOUR_FERMENTATION_HANDLER_03` | `QUERCUS_ROBUR_ENGLISH_OAK` | 22°Bk / 3 mo | `YEAR_04_TANNERY_LOG_03` | tanning, sour_liquor, fermentation, plumping, collagen | Wild *Lactobacillus* acidifies the liquor (pH 3.7), plumping collagen for faster tannin penetration |
| 3 | `oak_bark_tan_tannin_case_hardening_strike` | `FAST_TRACK_BARREL_DRUM_02` | `CONCENTRATED_QUEBRACHO_EXTRACT` | 85°Bk / 1 mo | `YEAR_07_TANNERY_LOG_02` | tanning, case_hardening, over_tanning, raw_core, leather_failure | Over-concentrated liquor case-hardens the grain; untanned core rots — a recorded process failure |
| 4 | `oak_bark_tan_iron_stain_black_rust_spot` | `WASH_POOL_VAT_NUMBER_05` | `WHITE_OAK_INNER_BARK_SHREDS` | 35°Bk / 6 mo | `YEAR_09_TANNERY_LOG_04` | tanning, iron_stain, ferric_tannate, blemish, rust_hook | Rusted hook + gallotannins precipitate black ferric tannate; thirty saddle skirts downgraded |
| 5 | `oak_bark_tan_sumac_leaf_light_saddlery_cure` | `FINE_PALE_LEATHER_VAT_02` | `RHUS_GLABRA_SMOOTH_SUMAC_LEAVES` | 28°Bk / 4 mo | `YEAR_12_TANNERY_LOG_01` | tanning, sumac, pale_leather, saddlery, scabbard | Sumac-leaf gallotannins give pale pliable leather for slings and holster linings |
| 6 | `oak_bark_tan_willow_bark_flexible_boot_upper` | `SUBTERRANEAN_RIVER_WILLOW_PIT` | `SALIX_ALBA_WHITE_WILLOW_BARK` | 18°Bk / 5 mo | `YEAR_15_TANNERY_LOG_03` | tanning, willow_bark, boot_leather, supple, footwear | Willow-bark broth yields low-stiffness, high-elongation leather for boot uppers |
| 7 | `oak_bark_tan_spent_tanbark_fuel_briquetting` | `SPENT_BARK_HYDRAULIC_PRESS` | `LEACHED_OAK_AND_HEMLOCK_MUSH` | 0°Bk / 0 mo | `YEAR_17_TANNERY_LOG_02` | tanning, spent_bark, fuel_briquette, recycling, kiln_fuel | Spent tannin-depleted bark pressed into smokeless kiln fuel burned in a foundry reverberatory furnace |
| 8 | `oak_bark_tan_tannery_effluent_oxygen_depletion` | `DRAINAGE_OUTFALL_SUMP_CANAL` | `MIXED_TANNERY_WASTE_SLUDGE` | 12°Bk / 0 mo | `YEAR_20_TANNERY_LOG_01` | tanning, effluent, bod_depletion, crayfish_kill, pollution | 4,000 mg/L BOD discharge strips dissolved oxygen; a cave-crayfish pool suffocates in six hours |

### Mineral tanning liquor assays — `chrome_alum_tanning_assays.json` (8 records)

| # | record id | facility label | material/agent | measurements | timestamp | tags | prose topic |
|---|---|---|---|---|---|---|---|
| 9 | `mineral_tan_potassium_alum_white_tawing` | `WHITE_TAWING_DRUM_UNIT_01` | `POTASSIUM_ALUMINUM_SULFATE_ALUM` | pH 3.4 / shrink 68°C | `YEAR_02_MINERAL_LOG_01` | tanning, tawing, potassium_alum, white_leather, gas_mask_liner | Alum/salt/egg-yolk tawing gives snow-white kidskin tailored into gas-mask facepiece liners |
| 10 | `mineral_tan_basic_chromium_sulfate_shrink_temp` | `CHROME_TAN_REVERSIBLE_DRUM_04` | `BASIC_CHROMIUM_SULFATE_CR2_SO4_3` | pH 3.8 / shrink 105°C | `YEAR_05_MINERAL_LOG_03` | tanning, chrome_tan, shrink_temperature, hydrothermal, work_boot | Chrome cross-linking raises hydrothermal shrink temperature to 105°C ("wet blue" hide) |
| 11 | `mineral_tan_basification_sodium_bicarbonate_drawn_grain` | `PADDLE_WHEEL_DRUM_CHROME_02` | `CHROMIUM_LIQUOR_WITH_BICARB` | pH 4.6 / shrink 92°C | `YEAR_08_MINERAL_LOG_02` | tanning, basification, drawn_grain, bicarbonate, defect | Dry bicarbonate causes local pH spikes; premature fixation produces coarse "drawn grain" |
| 12 | `mineral_tan_iron_vitriol_tanning_brittleness` | `FERROUS_SULFATE_MUD_PIT` | `IRON_VITRIOL_FERROUS_SULFATE` | pH 2.2 / shrink 62°C | `YEAR_11_MINERAL_LOG_04` | tanning, iron_tan, vitriol, brittleness, acid_rot | Residual sulfuric acid hydrolyzes collagen in storage; belts snap like stale crackers |
| 13 | `mineral_tan_zirconium_sulfate_snow_white_armor` | `HEAVY_MINERAL_ARMOR_VAT` | `BASIC_ZIRCONIUM_SULFATE` | pH 1.8 / shrink 98°C | `YEAR_14_MINERAL_LOG_01` | tanning, zirconium, white_armor, fire_resistant, cuirass | Zirconium-tanned 5 mm bull hide, beeswax-hardened into flame-resistant sentry cuirasses |
| 14 | `mineral_tan_formaldehyde_synthetic_oil_tannage` | `CHAMOIS_FULLING_MILL_TROUGH` | `COD_LIVER_OIL_AND_FORMALIN` | pH 5.5 / shrink 74°C | `YEAR_16_MINERAL_LOG_03` | tanning, chamois, cod_liver_oil, fuel_filter, washable | Oil/formaldehyde tannage yields washable chamois that passes petroleum hydrocarbons (fuel filters) |
| 15 | `mineral_tan_hexavalent_chromium_toxic_rash` | `REDUCED_DICHROMATE_VAT_BAY` | `SODIUM_DICHROMATE_MOLASSES_REDUCTION` | pH 2.8 / shrink 88°C | `YEAR_18_MINERAL_LOG_02` | tanning, hexavalent_chromium, toxic_dermatitis, ulcers, chemical_burn | 8% unreduced Cr(VI) causes "chrome hole" ulcerated dermatitis in tannery workers |
| 16 | `mineral_tan_synthetic_syntan_naphthalene_substitute` | `COAL_TAR_SYNTAN_BLENDING_TANK` | `SULFONATED_NAPHTHALENE_FORMALDEHYDE` | pH 3.6 / shrink 80°C | `YEAR_20_MINERAL_LOG_01` | tanning, syntan, coal_tar, vegetable_extender, tannin_saver | Coal-tar syntans extend dwindling oak-bark reserves by 60% |

### Rawhide deliming/bating failures — `rawhide_bating_failure_reports.json` (7 records)

| # | record id | facility label | material/agent | measurements (test status) | timestamp | tags | prose topic |
|---|---|---|---|---|---|---|---|
| 17 | `rawhide_bate_ammonium_sulfate_deliming_stall` | `DELIMING_WASHER_PADDLE_01` | `AMMONIUM_SULFATE_FERTILIZER_RUNOFF` | `STRONG_MAGENTA_ALKALINE_CORE` | `YEAR_03_BEAMHOUSE_01` | tanning, deliming, lime_blast, phenolphthalein, beamhouse | Phenolphthalein reveals trapped lime; dried hide shows lime-blast mottling |
| 18 | `rawhide_bate_pancreatic_trypsin_over_digestion` | `WARM_ENZYMATIC_BATING_TUB_03` | `PIG_PANCREAS_TRYPSIN_ENZYME` | `OVER_BATED_FIBER_DISSOLUTION` | `YEAR_05_BEAMHOUSE_03` | tanning, bating, trypsin, fiber_loss, soft_leather | 12 h (instead of 3 h) enzyme bath digests elastin into limp rag-like sheets |
| 19 | `rawhide_bate_sodium_sulfide_unhairing_burn` | `HAIR_DISSOLVING_PADDLE_VAT_02` | `SODIUM_SULFIDE_CONCENTRATED_SLURRY` | `GRAIN_KERATIN_CAUSTIC_MELT` | `YEAR_08_BEAMHOUSE_02` | tanning, unhairing, sodium_sulfide, caustic_burn, grain_damage | Sulfide overdose dissolves grain layer with the hair; cratered pitted surface |
| 20 | `rawhide_bate_acid_swelling_pickling_rupture` | `HIGH_SALT_PICKLE_DRUM_04` | `SULFURIC_ACID_WITHOUT_SODIUM_CHLORIDE` | `HYDRAULIC_ACID_SWELLING_BURST` | `YEAR_11_BEAMHOUSE_04` | tanning, pickling, acid_swelling, osmotic_rupture, collagen | Acid before brine triggers osmotic swelling; hide triples in thickness and ruptures |
| 21 | `rawhide_bate_fleshing_beam_knife_gouge` | `MANUAL_FLESHING_BEAM_STATION_B` | `MECHANICAL_TWO_HANDLED_BEAM_KNIFE` | `CORIUM_GOUGE_GRADE_DOWNGRADE` | `YEAR_14_BEAMHOUSE_01` | tanning, fleshing, beam_knife, gouge_defect, skiving | Chipped fleshing knife gouges the corium; weak zones bar heavy-load use |
| 22 | `rawhide_bate_salt_stain_calcium_phosphate_speck` | `RAW_CURING_SALT_STORE_BAY` | `CRUDE_UNREFINED_ROCK_SALT_CURE` | `CALCIUM_PHOSPHATE_STAIN_DEFECT` | `YEAR_17_BEAMHOUSE_03` | tanning, salt_stain, calcium_phosphate, curing_blemish, rawhide | Mineral-rich unwashed curing salt leaves phosphate specks that resist dye |
| 23 | `rawhide_bate_bacterial_putrefaction_hair_slip` | `WARM_TRANSIT_HOLDING_CELLAR` | `ANAEROBIC_BACTERIAL_PUTREFACTION` | `EPIDERMAL_SLIP_PUTRID_ROT` | `YEAR_20_BEAMHOUSE_01` | tanning, hair_slip, putrefaction, spoilage, hide_loss | Four unsalted days in a warm cellar; hair slips in rotten clumps, hide lost |

### Harness currying audits — `leather_harness_conditioning_audits.json` (7 records)

| # | record id | facility label | material/agent | measurements | timestamp | tags | prose topic |
|---|---|---|---|---|---|---|---|
| 24 | `leather_harness_neatsfoot_oil_cold_stuffing` | `HEAVY_HARNESS_CURRYING_BENCH_01` | `RENDERED_NEATSFOOT_OIL_AND_TALLOW` | 14.5% oil / 4200 PSI | `YEAR_02_CURRYING_LOG_01` | leatherworking, neatsfoot_oil, stuffing, harness, draft_gear | Warm neatsfoot/tallow stuffing keeps 6 mm harness traces from cold-tunnel stiffening |
| 25 | `leather_harness_spew_stearic_acid_white_bloom` | `SADDLERY_STORAGE_VAULT_04` | `HIGH_STEARIN_BEEF_SUET_GREASE` | 18% oil / 3800 PSI | `YEAR_05_CURRYING_LOG_03` | leatherworking, spew, fatty_spue, stearic_acid, white_bloom | High-stearin tallow migrates as white crystalline spew in 6°C storage; wiped with heat lamps |
| 26 | `leather_harness_sulfur_gas_red_rot_powdering` | `COAL_MINE_HAULAGE_DEPOT_02` | `VEGETABLE_TANNED_UNSTUFFED_STRAP` | 4% oil / 850 PSI | `YEAR_08_CURRYING_LOG_02` | leatherworking, red_rot, sulfur_dioxide, acid_hydrolysis, powdering | SO₂ from coal-fired steam generators + moisture → red rot; straps crumble under incline hauling |
| 27 | `leather_harness_brass_rivet_verdigris_corrosion` | `REPAIR_BENCH_EQUIPMENT_SHED` | `UNNEUTRALIZED_FATTY_ACID_PASTE` | 12% oil / 2900 PSI | `YEAR_11_CURRYING_LOG_04` | leatherworking, verdigris, brass_corrosion, rivet_rot, green_wax | Rancid-lard fatty acids + brass buckles → waxy green verdigris rots stitch holes |
| 28 | `leather_harness_wax_thread_linen_rot_failure` | `STITCHING_AND_AWL_WORKBENCH` | `BEESWAXED_IRISH_LINEN_THREAD` | 10% oil / 3400 PSI | `YEAR_14_CURRYING_LOG_01` | leatherworking, linen_thread, beeswax, saddle_stitch, rot | Beeswax/rosin-seamed linen thread survives underground moisture; un-waxed control stitches rot in 8 months |
| 29 | `leather_harness_currying_dubbin_waterproofing` | `BOOT_MAKING_FINISHING_BAY` | `DUBBIN_TALLOW_COD_OIL_PITCH` | 22% oil / 3600 PSI | `YEAR_17_CURRYING_LOG_03` | leatherworking, dubbin, waterproofing, boot_leather, currying | 50/40/10 tallow/cod-oil/pitch dubbin keeps socks dry through acidic geothermal mud |
| 30 | `leather_harness_rawhide_lace_tensile_braiding` | `POWER_TRANSMISSION_BELT_SHOP` | `UN-OILING_DRIED_RAWHIDE_STRIP` | 1% oil / 7800 PSI | `YEAR_20_CURRYING_LOG_01` | leatherworking, rawhide, braided_belt, winch_band, high_tensile | Braided rawhide drive belts (7,800 PSI) power the shelter air-filtration blower pulleys |

## 3. Chronology (Workstream A1) — repeated facilities over time

The corpus describes **no single long-lived tannery**. Facility labels are
per-batch equipment designations; only three contexts recur as a *practice*
across years, and the corpus reads as a surviving community re-learning
leatherwork generation after generation:

| Practice thread | Records | Span |
|---|---|---|
| Vegetable-tanning discipline (pit density control) | #1 (48°Bk, 12 mo) → #3 (85°Bk shortcut fails) → #2 (sour-liquor accelerator) | YEAR_02 → YEAR_04 → YEAR_07 |
| Chromium tanning maturity | #10 (successful 105°C bath) → #11 (basification defect) → #12 (cheap iron-vitriol substitute fails) → #16 (syntan extends scarce bark) | YEAR_05 → YEAR_20 |
| Beamhouse failure accumulation | #17 → #20 → #23 (deliming, pickling, spoilage: each a distinct lost batch) | YEAR_03 → YEAR_20 |
| Harness care knowledge | #24 (correct stuffing) → #26 (red rot from industrial SO₂) → #29 (dubbin waterproofing) → #30 (rawhide belts for the shelter blower) | YEAR_02 → YEAR_20 |

**Disposition:** repeated failures are preserved as history, not flattened.
Each record projects independently; no record overwrites or supersedes
another; nothing aggregates into a single "tannery stat."

## 4. Material vocabulary normalization (Workstream A2)

Equivalence notes for cross-referencing prose against item authority. **No
item ID may be renamed on the basis of these synonyms** — the crosswalk in
`TANNING_LEATHER_ITEM_CROSSWALK.md` is the only canonical mapping surface.

| Corpus term | Meaning | Notes |
|---|---|---|
| rawhide / un-tanned hide | un-tanned dermis, dried or cured | distinct from tanned leather; `rawhide_bate_*` records are about pre-tanning prep failures |
| tanned hide / leather | collagen stabilized by tannin | vegetable (#1–#8) vs mineral (#9–#16) families |
| tawing | alum-based mineral treatment (no tannin) | #9 — historically distinct from true tanning; non-transformable in the game |
| currying | post-tanning oiling/fatliquoring/dressing | #24–#30 — conditioning, not tanning |
| fatliquor | oil-in-water lubricant emulsion for leather fibers | `fatliquor_compound_formula` is a label, not a craftable recipe |
| bark liquor / tannin steep | vegetable tannin bath, measured in Barkometer degrees | #1–#4 |
| mineral tan liquor | chromium/alum/zirconium bath, measured by pH + shrink temp | #9–#16 |
| bate / bating | enzymatic beamhouse step removing non-collagen proteins | #18 |
| deliming | neutralizing lime from unhairing | #17 |
| pickling | acid+salt stabilization before tanning | #20 |
| red rot | SO₂-acid hydrolysis of vegetable-tanned leather | #26 — irreversible; historical failure, not a gear-debuff |
| spew / fatty spue | stearin migration to the grain surface | #25 — cosmetic, wipeable |
| dubbin | tallow/fish-oil/pitch waterproofing paste | #29 — historical dressing; not a recipe output |
| chamois | oil-tanned split sheepskin | #14 — no canonical item exists; provenance-only |
| Barkometer (°Bk) | tannin-strength hydrometer scale | archival unit; displayed only |

## 5. Truth classes

| Class | Count | Records |
|---|---|---|
| Successful process log | 9 | #1, #2, #5, #6, #7, #9, #10, #16, #29, #30 (see below) |
| Process failure report | 13 | #3, #4, #11, #12, #15, #17–#23, #26, #27, #28 |
| Defect/cosmetic report | 2 | #11, #25 |
| Environmental incident | 1 | #8 |

(Overlaps are per-record primary classification; failure status is also
mechanically derivable from `phenolphthalein_test_status` and currying
low-oil outliers — see `TANNING_LEATHER_MEASUREMENT_DISPOSITION.md`.)
