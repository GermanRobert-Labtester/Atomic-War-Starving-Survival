# TANNING LEATHER MEASUREMENT DISPOSITION — Plan 159 (Workstreams A & J)

> For every mechanical-looking field in the 30-record corpus: is it
> **DESCRIPTIVE** (archival batch data, display-only) or **MECHANICAL**
> (consumed by a live system)? Verified against current source; each
> disposition names its owning system or the reason no consumer exists.

## 1. Field-by-field disposition

| Field | Values (range across 30 records) | Disposition | Rationale |
|---|---|---|---|
| `barkometer_density_degrees` | 0–85 °Bk | **DESCRIPTIVE** | Barkometer is a historical tannin hydrometer scale. No tannin-strength consumer exists. Never a yield/quality/timer coefficient. |
| `hide_steep_duration_months` | 0–12 months | **DESCRIPTIVE** | §5 Rule 2: not a crafting timer unless crafting explicitly models it — Plan 55 does not. Long historical tanning times must never read as current timers (Risk 12). |
| `liquor_ph_level` | 1.8–5.5 | **DESCRIPTIVE** | §5 Rule 3: no chemical-exposure mechanic is created from pH. Disease/medical authority untouched. |
| `hydrothermal_shrink_temp_celsius` | 62–105 °C | **DESCRIPTIVE** | §5 Rule 4: laboratory hydrothermal benchmark of a historical batch — not armor/heat resistance. Leather quality ≠ protection (Risk 18). |
| `oil_content_percentage` | 1–22% | **DESCRIPTIVE** | archival currying specification; no conditioning/durability consumer. |
| `tensile_strength_psi` | 850–7800 PSI | **DESCRIPTIVE** | §5 Rule 5: never an equipment durability stat. Historical batch spec, explicitly labelled "NOT THIS ITEM'S MEASURED QUALITY" in inspection UI. |
| `fatliquor_compound_formula` | raw labels (e.g. `DUBBIN_TALLOW_COD_OIL_PITCH`) | **DESCRIPTIVE** | §5 Rule 6: a batch label, never parsed into components, never a recipe (Plan 55 authority). |
| `deliming_chemical_agent` | raw labels | **DESCRIPTIVE** | chemical names are historical context only; no inventory/consumption/exposure. |
| `phenolphthalein_test_status` | status labels (7) | **DESCRIPTIVE** (failure flag for display) | drives `FailureSummary` presentation only; interpreted as process history, never as an active hazard. |
| `bark_source_botanical` | Latin binomials | **DESCRIPTIVE** | §5 Rule 7: no harvestable plant created; no gatherable bark item. |
| `tannery_vat_id` / `beamhouse_pit_id` / `currying_workshop_id` / `mineral_tan_liquor_id` | equipment labels | **DESCRIPTIVE** (provenance label) | §5 Rule 8: equipment labels until/unless mapped to canonical locations — the location crosswalk maps *context*, never promotes labels to `loc_*`. |
| `timestamp_relative` | `YEAR_02`–`YEAR_20` phase labels | **DESCRIPTIVE** | historical chronology; never campaign time (`Firewall_HistoricalTimestamps_AreNotCampaignTime`). |
| `tags` | topical snake_case | **DESCRIPTIVE** | display/filter vocabulary only. |
| `prose` | technical narrative | **DESCRIPTIVE** | journal + archive text; reviewed below. |

**Summary: 30/30 records, 100% of fields DESCRIPTIVE. No field in this
corpus is consumed by any mechanical system.** (`Firewall_ViewingRecords_MutatesNothingButDiscovery`.)

## 2. Chemical & occupational-safety review (Workstream J)

Each hazardous-chemistry mention was reviewed for technical accuracy and for
leakage into gameplay systems. **No chemical-exposure system is created; all
worker-harm content remains historical safety context.**

| Topic | Records | Accuracy review | Disposition |
|---|---|---|---|
| Chrome tanning (Cr III) | #10, #11 | "basic chromium sulfate", collagen carboxyl cross-linking, basification via bicarbonate, "wet blue" — technically correct | presented as historical process record; no toxicity modeling (Cr III is the *tanning* species) |
| Hexavalent chromium (Cr VI) | #15 | incomplete SO₂/molasses reduction leaving Cr(VI); "chrome holes" (painful skin ulcers from Cr(VI) dermatitis) — accurate and appropriately restrained | historical occupational-hazard narrative only. **No disease/affliction entry created** — Disease/Medical authority untouched (§16 rule). Tag vocabulary stays descriptive. |
| Sodium sulfide unhairing | #19 | sulfide dissolution of keratin + grain damage on overdose — correct | caustic-burn language is batch history; no player-facing hazard hook |
| Acid pickling | #20 | acid-before-salt osmotic swelling — correct | process failure history only |
| Deliming / phenolphthalein | #17 | phenolphthalein magenta in alkaline (limed) core — correct indicator logic | test status displayed verbatim; never a mechanic |
| Formaldehyde / formalin | #14 | oil-formaldehyde chamois tannage — historically attested | deferred record (no producer); formalin toxicity **not** converted into a poison mechanic |
| Ferrous sulfate ("iron vitriol") | #12 | residual acid hydrolysis → brittle leather — correct | failure history; no equipment-debuff |
| SO₂ red rot | #26 | coal-fired SO₂ + moisture → acid hydrolysis of vegetable-tanned leather ("red rot") — correct and well-documented industrial phenomenon | historical failure; no shelter-air or gear effect |
| Verdigris | #27 | copper oleate from fatty acids + brass — correct ("verdigris" strictly applies to copper acetate; the record already hedges as "copper oleate") | kept as-is; no corrosion mechanic |
| BOD / effluent | #8 | 4,000 mg/L BOD oxygen depletion, crayfish kill — plausible and tonally restrained | environmental storytelling only; no water-pollution mechanic added |
| Worker-health claims | #15 | only record with direct worker injury | historical context, per §16: "no poisoning/disease entries from process prose" |

## 3. Jargon-risk note (Risk 15)

Measurement summaries in UI surfaces use compact units (°Bk, pH, PSI, °C)
with the standing "(ARCHIVAL BATCH — NOT THIS ITEM'S MEASURED QUALITY)"
guard rather than exposing raw field dumps. The full prose (with terms like
"barkometer", "fatliquor") surfaces in the journal/archive text where a
player opt-in context is expected.
