# Hydrogeology Runtime Provenance & Authority Matrix (Plan 154)

**Document Reference:** `docs/content/HYDROGEOLOGY_RUNTIME_PROVENANCE_MATRIX.md`
**Subsystem:** Hydrogeology Science Discovery Layer (`HydroGeologyCatalog`)
**Data Authority:**
- `Assets/StreamingAssets/Data/narrative/artesian_well_contamination_logs.json` (8 records)
- `Assets/StreamingAssets/Data/narrative/cave_aquatic_biota_logs.json` (8 records)
- `Assets/StreamingAssets/Data/narrative/geothermal_steam_vent_diagnostics.json` (7 records)
- `Assets/StreamingAssets/Data/narrative/stalactite_mineral_assay_reports.json` (7 records)
**Total Records:** 30
**First-Pass Activated Records:** 20 (5 Well, 5 Biota, 5 Steam, 5 Mineral)
**Deferred Records:** 10 (3 Well, 3 Biota, 2 Steam, 2 Mineral)

---

## 1. Architectural Authority & Core Invariants

1. **Historical Record Independence:** Archival records reflect historical observations and measurements taken at specific sample times. They **never** overwrite or mutate live simulation state (such as current water safety, radiation dose rate, generator health, steam pressure, or room condition).
2. **No Direct Inventory Grants:** Mineral assay percentages (`primary_metal_assay_pct`) represent chemical assay analysis of geological samples, not harvestable ore deposits. Discovery of an assay record grants **zero** metal, ore, or crafting materials.
3. **No Direct Radiation Infliction:** Historical radiation values (e.g. Bq/L or µR/hr) describe contamination present at historical sampling dates. Inspecting or discovering a record inflicts **zero** radiation damage or dose on survivors.
4. **No Synthetic Entity Spawns:** Biological logs describing cave troglobites (e.g., blind trout, amphipods, snottites) do not spawn entities, create combat encounters, or generate food rations.
5. **No Mechanical State Overwrite:** Steam vent diagnostics record historical engineering failures and measurements. They do not alter current steam network pressure, turbine RPM, or boiler thermal output.
6. **Provenance Presentation Standard:** All measurements displayed to the player must explicitly indicate their provenance: *“Recorded at sample time [relative timestamp]”* to prevent confusion with real-time HUD telemetry.
7. **Idempotence & Save Safety:** Discovery of records is tracked by stable string ID. Repeated inspections are clean no-ops. Unknown IDs are tolerated and preserved inertly. Old saves load without error.

---

## 2. 30-Record Provenance & Mapping Matrix

### 2.1 Artesian Well Contamination Logs (8 Records)

| ID | Named Well / Facility | Aquifer Stratum | Contaminant Agent | Measurement (Bq/L) | Relative Timestamp | Canonical Producer | Status | Allowed Consumer | Scientific & Radiobiological Notes |
|---|---|---|---|---|---|---|---|---|---|
| `well_contam_tritium_percolation_spike` | `DEEP_ARTESIAN_WELL_03` | St. Peter Sandstone (400m) | Tritiated Water ($^3\text{H}_2\text{O}$ / HTO) | 4,500.0 | Year 02 Month 09 | `room_water_pump` | **Activated** (Day 1) | Codex, Journal | Tritium replaces hydrogen in water molecules; cannot be removed by charcoal/particle filtration; requires 5:1 dilution or isotopic separation. WHO drinking water guidance limit is 10,000 Bq/L. |
| `well_contam_strontium_90_limestone_leach` | `COMMISSARY_SUPPLY_WELL_01` | Karst Dolomite Formation | Strontium-90 Carbonate ($^{90}\text{SrCO}_3$) | 320.0 | Year 04 Month 11 | `room_filtration` | **Activated** (Day 5) | Codex, Journal | Alkaline earth bone-seeker that substitutes for calcium in hydroxyapatite lattices (teeth/bones). Cation-exchange softening with natural zeolites (clinoptilolite) is the standard treatment. |
| `well_contam_ferro_bacterial_iron_slime` | `HYDRO_POWER_RECIRC_WELL_04` | Glacial Outwash Gravel | *Leptothrix* iron bacteria slime | 15.0 | Year 06 Month 04 | `location_municipal_water_reservoir` | **Activated** (Day 10) | Codex, Journal | Biological biofouling rather than radiological poison; autotrophic iron-oxidizers precipitate thick ferric hydroxide gelatin, encrusting well screens and reducing flow from 600 to 45 GPM. |
| `well_contam_sub_surface_diesel_plume` | `SOUTH_WING_AUXILIARY_WELL_02` | Shallow Alluvial Gravel (80m) | Petroleum hydrocarbon plume | 0.0 | Year 08 Month 07 | `location_abandoned_desalination` | **Activated** (Day 15) | Codex, Journal | Pure chemical/hydrocarbon contamination with zero radiological activity (0.0 Bq/L). Demonstrates chemical vs. radiological hazard separation. Volatile organic compounds produce kerosene odors and flammable vapors. |
| `well_contam_radium_226_geothermal_upwelling` | `GEOTHERMAL_REINJECTION_WELL_07` | Precambrian Basal Granite | Radium-226 Chloride ($^{226}\text{RaCl}_2$) | 2,960.0 | Year 11 Month 02 | `location_geothermal_borehole_site` | **Activated** (Day 20) | Codex, Journal | Deep fracture upwelling dissolves ancient geothermal brine. Radium-226 decays to radioactive noble gas Radon-222, which degasses from warm cooling towers, creating an inhalation alpha hazard in enclosed spaces. |
| `well_contam_mine_acid_drainage_inrush` | `ABANDONED_DRIFT_MINE_INTAKE` | Pennsylvanian Coal Measures | Acid mine drainage (pyrite oxidation) | 85.0 | Year 13 Month 10 | `location_collapsed_salt_mine` | *Deferred* (Phase 2) | Codex | Pyrite ($FeS_2$) oxidation produces sulfuric acid leachate ($pH \approx 2.9$) and dissolved heavy copper, attacking plumbing joints and generating toxic copper verdigris. |
| `well_contam_caustic_lye_decon_runoff` | `SURFACE_PORTAL_SUMP_WELL_05` | Fractured Basalt Cap (50m) | Sodium hydroxide ($NaOH$) decon slurry | 1,200.0 | Year 16 Month 05 | `room_airlock` | *Deferred* (Phase 2) | Codex | Industrial chemical runoff from decontamination washing Cascades down casing annulus into groundwater. Demonstrates anthropogenic surface spill rather than natural geochemical migration. |
| `well_contam_cyanide_heap_leach_plume` | `WEST_RIDGE_DEEP_PRODUCTION_06` | Fractured Quartzite Aquifer | Free potassium cyanide ($KCN$) solution | 0.0 | Year 19 Month 08 | `location_chemical_plant` | *Deferred* (Phase 2) | Codex | Breached gold mining tailing dam releases highly toxic cyanide solution into hydraulic flow. 0.0 Bq/L activity demonstrates lethal acute chemical poison without radiation. |

---

### 2.2 Cave Aquatic Biota Logs (8 Records)

| ID | Species Designation | Cavern Location | Bioluminescence | Ecological Niche | Relative Timestamp | Canonical Producer | Status | Allowed Consumer | Ecological & Evolutionary Notes |
|---|---|---|---|---|---|---|---|---|---|
| `biota_cave_eyeless_albino_trout` | *Salvelinus subterraneus alba* | Abyssal Siphon Pool 03 | None (Lateral line sensory) | Apex hydro predator | Year 03 Bio Survey 01 | `location_flooded_subway_depot` | **Activated** (Day 12) | Codex, Journal | Obligate troglobite char. Complete eye regression beneath skin with hypertrophied neuromast vibration pits along lateral line. Survives in oligotrophic subterranean waters. |
| `biota_cave_bioluminescent_blue_amphipod` | *Niphargus lucens azurea* | Calcite Cascade Terrace B | Luciferin pulsed cold blue (470nm) | Detritivore swarm grazer | Year 05 Bio Survey 04 | `location_drainage_network` | **Activated** (Day 8) | Codex, Journal | Swarming benthic crustacean. Mechanical disturbance stimulates luciferase-mediated 470nm blue emission, startling potential predators in rimstone pools. |
| `biota_cave_sulfur_oxidizing_mucilage_veil` | *Thiobacillus snottita acidica* | Sulfur Vent Grotto Sector 5 | None (Acidic mucilage) | Chemolithoautotroph mat | Year 07 Bio Survey 02 | `location_geo_thermal_plant_ruins` | **Activated** (Day 18) | Codex, Journal | Snottite extremophile biofilm hanging like mucous stalactites. Oxidizes volcanic $H_2S$ gas to sulfuric acid, creating dripping droplets of pH 0.8. |
| `biota_cave_blind_troglobitic_crayfish` | *Orconectes subterraneus ghost* | Gravel Bed Runoff Chute 01 | None (Tactile elongation) | Benthic scavenger | Year 09 Bio Survey 03 | `location_metro_tunnel` | **Activated** (Day 14) | Codex, Journal | Pigmentless decapod with 20cm antennae. Extreme longevity (up to 60 years) adapted to low caloric input and steady 10°C water temperatures. |
| `biota_cave_methanotrophic_algal_mat` | *Methylococcus roseus troglo* | Coal Seam Seep Basin 04 | Weak infrared emission | Methane-oxidizing bio trap | Year 12 Bio Survey 01 | `location_drainage_network` | *Deferred* (Phase 2) | Codex | Pink methanotrophic bacterial mat oxidizing coal-seam firedamp ($CH_4$). Consumes gas at seep interfaces. |
| `biota_cave_phosphorescent_cave_sponge` | *Spongilla radioactiva amber* | Lower Aquifer Siphon Pass | Sustained amber phosphorescence | Mineral bio-accumulator | Year 15 Bio Survey 02 | `location_collapsed_salt_mine` | **Activated** (Day 22) | Codex, Journal | Freshwater demosponge bio-sequestering heavy metals and radium in siliceous spicule lattices. Emits persistent 20-minute amber glow via mineral trapping. |
| `biota_cave_giant_segmented_leech` | *Haemopis gigas cavernicola* | Warm Sump Drainage Canal 06 | None (Thermal seeking) | Opportunistic parasite | Year 17 Bio Survey 03 | `location_municipal_water_reservoir` | *Deferred* (Phase 2) | Codex | 30cm hirudinean inhabiting 30°C thermal discharge channels. Possesses pit organs sensitive to infrared thermal gradients. |
| `biota_cave_sub_glacial_ice_worm_cluster` | *Mesenchytraeus solifugus sub* | Frozen Summit Dam Crevasse | None (Cryoprotective) | Psychrophile cryopredator | Year 20 Bio Survey 01 | `location_frozen_wetland` | *Deferred* (Phase 2) | Codex | Enchytraeid oligochaete living inside solid sub-glacial blue ice at -3°C. Body loaded with glycerol antifreeze; thermal shock from human touch lyses cells. |

---

### 2.3 Geothermal Steam Vent Diagnostics (7 Records)

| ID | Vent Manifold ID | Temp (°C) | Pressure (bar) | Failure Diagnostic | Relative Timestamp | Canonical Producer | Status | Allowed Consumer | Thermodynamic & Mechanical Notes |
|---|---|---|---|---|---|---|---|---|---|
| `steam_vent_superheated_nozzle_erosion` | `PRIMARY_GEOTHERMAL_HEADER_01` | 285.0 | 42.0 | Stainless nozzle throat erosion | Year 03 Steam Diag 01 | `location_geo_thermal_plant_ruins` | **Activated** (Day 15) | Codex, Journal | 285°C dry superheated steam at 42 bar (saturation temp ~253°C => 32°C superheat). Quartz particulate erosion widened 316-SS throttle from 25mm to 38mm, inducing turbine governor instability. |
| `steam_vent_hydrogen_sulfide_stress_cracking` | `BOREHOLE_CASING_FLANGE_SECTOR_6` | 210.0 | 35.0 | Sulfide stress corrosion cracking | Year 06 Steam Diag 03 | `location_geothermal_borehole_site` | **Activated** (Day 20) | Codex, Journal | High $H_2S$ (650 ppm) induced atomic hydrogen diffusion into high-strength Grade B7 casing studs, causing catastrophic brittle fracture under thermal cycling. |
| `steam_vent_condensate_slug_hammer_shock` | `MAIN_TRANSMISSION_RISER_LEVEL_3` | 175.0 | 28.0 | Two-phase water hammer shock | Year 09 Steam Diag 02 | `loc_cluster_steam_substation` | **Activated** (Day 10) | Codex, Journal | Failed inverted-bucket steam trap allowed 50L subcooled condensate pool. Steam flow propelled water slug at 60 mph into 90° elbow, shearing structural pipe hangers. |
| `steam_vent_silica_scaling_throttle_constriction` | `FLASH_VESSEL_DISCHARGE_NOZZLE` | 140.0 | 12.5 | Amorphous silica glass scale | Year 12 Steam Diag 04 | `room_main` | **Activated** (Day 6) | Codex, Journal | Pressure drop across cyclone flash separator lowered silica solubility, precipitating vitreous amorphous quartz crust that constricted nozzle ring orifice by 60%. |
| `steam_vent_geothermal_wellhead_subsidence` | `PRODUCTION_WELL_CLUSTER_B` | 240.0 | 38.0 | Surface wellhead tilt subsidence | Year 15 Steam Diag 01 | `location_geo_thermal_plant_ruins` | **Activated** (Day 25) | Codex, Journal | Fluid depressurization of geothermal reservoir caused 15cm ground subsidence, tilting master wellhead pad by 4° and inducing bending strain on expansion bellows. |
| `steam_vent_acoustic_jet_screamer_attenuation` | `EMERGENCY_ATMOSPHERIC_BLOWDOWN_D` | 260.0 | 45.0 | Acoustic jet overpressure hazard | Year 17 Steam Diag 02 | `location_geo_thermal_plant_ruins` | *Deferred* (Phase 2) | Codex | Choked supersonic sonic jet expansion at 45 bar relief blowdown produced 138 dB acoustic noise field capable of tympanic membrane rupture at 70 yards. |
| `steam_vent_non_condensable_gas_vent_fire` | `VACUUM_EJECTOR_EXHAUST_STACK` | 95.0 | 1.2 | Methane-hydrogen deflagration | Year 20 Steam Diag 01 | `loc_cluster_steam_substation` | *Deferred* (Phase 2) | Codex | Stripped geothermal non-condensable off-gas (8% $CH_4$, 4% $H_2$, 85% $CO_2$) ignited by static spark on copper shroud, producing 3m atmospheric flare jet. |

---

### 2.4 Stalactite Mineral Assay Reports (7 Records)

| ID | Sample Specimen ID | Mineral Species | Primary Metal (%) | Radiation (µR/hr) | Relative Timestamp | Canonical Producer | Status | Allowed Consumer | Mineralogical & Crystallographic Notes |
|---|---|---|---|---|---|---|---|---|---|
| `stalactite_assay_uranophane_canary_crust` | `SPELEO-MINERAL-ASSAY-009` | Uranophane ($Ca(UO_2)_2SiO_3(OH)_2 \cdot 5H_2O$) | 52.4 | 8,400.0 | Year 04 Mineral Assay | `location_collapsed_salt_mine` | **Activated** (Day 16) | Codex, Journal | Secondary hydrated calcium uranyl silicate formed by supergene oxidation of uraninite by silica-bearing meteoric groundwater. 8.4 mR/hr emission; canary yellow fluorescence. |
| `stalactite_assay_malachite_copper_dripstone` | `SPELEO-MINERAL-ASSAY-023` | Malachite ($Cu_2CO_3(OH)_2$) | 57.5 | 12.0 | Year 07 Mineral Assay | `location_flooded_subway_depot` | **Activated** (Day 12) | Codex, Journal | Basic copper carbonate precipitation from seepage across melted electrical infrastructure. Theoretical copper content in pure malachite is exactly 57.48% (matches 57.5%). |
| `stalactite_assay_galena_lead_soda_straw` | `SPELEO-MINERAL-ASSAY-041` | Galena ($PbS$) concretion | 86.6 | 45.0 | Year 10 Mineral Assay | `location_drainage_network` | **Activated** (Day 14) | Codex, Journal | Tubular soda straw stalactites formed beneath battery adits. Theoretical lead content in stoichiometric $PbS$ is $207.2 / 239.26 = 86.60\%$ (matches 86.6% assay exactly). |
| `stalactite_assay_cinnabar_mercury_globule_seep` | `SPELEO-MINERAL-ASSAY-058` | Cinnabar ($HgS$) with native mercury | 82.0 | 8.0 | Year 13 Mineral Assay | `location_metro_tunnel` | **Activated** (Day 18) | Codex, Journal | Mercury(II) sulfide weeping free droplets of liquid elemental mercury. High vapor pressure in unventilated caverns creates neurotoxic inhalation hazard (erethism). |
| `stalactite_assay_arsenopyrite_garlic_exhalation` | `SPELEO-MINERAL-ASSAY-076` | Arsenopyrite ($FeAsS$) | 46.0 | 25.0 | Year 16 Mineral Assay | `location_collapsed_salt_mine` | *Deferred* (Phase 2) | Codex | Monoclinic sulfarsenide mineral. Impact or steam exposure causes thermal dissociation, venting volatile arsine gas ($AsH_3$) with diagnostic garlic odor. |
| `stalactite_assay_fluorite_purple_cubic_drusy` | `SPELEO-MINERAL-ASSAY-092` | Fluorite ($CaF_2$) | 51.3 | 180.0 | Year 18 Mineral Assay | `location_geothermal_borehole_site` | **Activated** (Day 24) | Codex, Journal | Calcium fluoride vug crystals. Theoretical Ca content is $40.08 / 78.08 = 51.33\%$ (matches 51.3% assay). Deep royal purple coloration caused by radiation-induced F-center color defects. |
| `stalactite_assay_autunite_green_fluorescent_blade` | `SPELEO-MINERAL-ASSAY-110` | Autunite ($Ca(UO_2)_2(PO_4)_2 \cdot 10-12H_2O$) | 48.2 | 14,200.0 | Year 20 Mineral Assay | `location_collapsed_salt_mine` | *Deferred* (Phase 2) | Codex | Hydrated calcium uranyl phosphate forming tabular green crystal aggregates. Intense 14.2 mR/hr radiation field; blinding green fluorescence under 365nm UV. |

---

## 3. Producer Allocation & Distribution

All 20 activated records are mapped to genuine canonical entities without inventing fictional site IDs:

| Canonical Producer ID | Location Type | Mapped Activated Records | Count |
|---|---|---|---|
| `room_water_pump` | Shelter Utility Wellhead | `well_contam_tritium_percolation_spike` | 1 |
| `room_filtration` | Shelter Life Support | `well_contam_strontium_90_limestone_leach` | 1 |
| `room_main` | Shelter Generation & Boiler | `steam_vent_silica_scaling_throttle_constriction` | 1 |
| `location_municipal_water_reservoir` | Surface Infrastructure | `well_contam_ferro_bacterial_iron_slime` | 1 |
| `location_abandoned_desalination` | Coastal Infrastructure | `well_contam_sub_surface_diesel_plume` | 1 |
| `loc_cluster_steam_substation` | Holdfast Substation | `steam_vent_condensate_slug_hammer_shock` | 1 |
| `location_drainage_network` | Subterranean Transit/Drainage | `biota_cave_bioluminescent_blue_amphipod`, `stalactite_assay_galena_lead_soda_straw` | 2 |
| `location_metro_tunnel` | Subterranean Transit | `biota_cave_blind_troglobitic_crayfish`, `stalactite_assay_cinnabar_mercury_globule_seep` | 2 |
| `location_flooded_subway_depot` | Subterranean Transit/Water | `biota_cave_eyeless_albino_trout`, `stalactite_assay_malachite_copper_dripstone` | 2 |
| `location_collapsed_salt_mine` | Deep Geological Mine | `biota_cave_phosphorescent_cave_sponge`, `stalactite_assay_uranophane_canary_crust` | 2 |
| `location_geo_thermal_plant_ruins` | Industrial Geothermal | `biota_cave_sulfur_oxidizing_mucilage_veil`, `steam_vent_superheated_nozzle_erosion`, `steam_vent_geothermal_wellhead_subsidence` | 3 |
| `location_geothermal_borehole_site` | Deep Borehole Site | `well_contam_radium_226_geothermal_upwelling`, `steam_vent_hydrogen_sulfide_stress_cracking`, `stalactite_assay_fluorite_purple_cubic_drusy` | 3 |

**Maximum records at any single producer:** 3 (well within the max-5 limit).

---

## 4. Separation of Scientific History vs. Live Simulation

```
[ HydroGeologyCatalog ] (Historical Science Records)
          │
          ▼
[ HydroGeologyProjection & DiscoverySystem ] (Provenance, Bookkeeping, Codex/Journal)
          │
          ├───► Read-Only Presentation ("Recorded at sample time")
          ├───► Codex & Journal Observations
          └───► Deterministic Cross-Record Scientific Relations

   ═══════════════════ FIREWALL: ZERO MUTATION ═══════════════════

[ Live Simulation Systems ] (Unmodified Authority)
  ├─ NeedsSystem / WaterTreatmentSystem  ── Water inventory & drinkability
  ├─ RadiationSystem / DoseLedger        ── Real-time dweller exposure & ambient rads
  ├─ PowerGridSystem / BoilerSimulation  ── Actual steam pressure, wattage, heat
  └─ InventoryContainer / ScavengeSystem ── Actual mineral/ore scavenging yields
```
