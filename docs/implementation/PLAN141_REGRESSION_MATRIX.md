# Plan 141 — Regression Risk Matrix

## 1. Risk Register & Mitigation Strategy

This matrix audits the 18 specific integration risks defined in Plan 141, documenting architectural safeguards, automated test gates, and residual risk levels.

| # | Risk Description | Severity | Mitigation Architecture | Automated Verification Gate | Residual Risk |
|---|---|---|---|---|---|
| 1 | Prose contradicts simulation (e.g. prognosis mismatch) | High | Live mechanical state outranks prose; unmapped or contradictory states omit prose. | Unit test: `DiagnosisSummary_Matches_Mechanical_Severity` | Low |
| 2 | Medical misinformation reaches player UI | High | Forensic audit; removal of bone healing myth and clarification of iodine/chelation limits. | Accuracy test: `Catalog_Contains_No_Medically_Debunked_Claims` | Negligible |
| 3 | Radiation semantics regress | High | Iodine labeled for thyroid protection only; chelation labeled for decorporation; true dose owned by DoseLedger. | Selftest: `--medical-selftest` + unit test assertions | Negligible |
| 4 | Decorative percentage shown as real chance | High | `success_chances` strictly hidden from UI; only pipeline previews calculate availability. | Unit test: `Prose_Never_Exposes_Decorative_Percentages` | Negligible |
| 5 | Required-item prose becomes second treatment gate | High | Item requirements are owned exclusively by `MedicalTreatmentCatalog`; prose `required_items` unconsumed. | Unit test: `Treatment_Execution_Unaffected_By_Prose_Catalog` | Negligible |
| 6 | Generic infection maps to transmissible disease incorrectly | Medium | `MedicalConditionResolver` restricts mapping; transmissible diseases use specific identities only. | Unit test: `Disease_Mappings_Are_Explicit_And_Non_Colliding` | Low |
| 7 | Duplicate condition identities | Medium | Normalized key deduplication; consolidated `medical_dehydration_severe` in JSON. | Data test: `Catalog_Has_Zero_Duplicate_Condition_Ids` | Negligible |
| 8 | Casebook record mutates live patient | High | Case entries are immutable lore DTOs; no mutation methods exist on casebook models. | Unit test: `Casebook_Query_Emits_No_Simulation_Side_Effects` | Negligible |
| 9 | UI becomes text-heavy / unreadable | Medium | 3-tier hierarchy: live stats on top, compact 1-line overview + 1 symptom line in detail. | UI visual inspection + `scene-lint.py` | Low |
| 10 | Panel reopen rerolls prose | Medium | Deterministic pseudo-hash selection keyed on `(survivorId, conditionId)`. No `System.Random`. | Determinism test: `Prose_Selection_Is_Deterministic_Across_Rebinds` | Negligible |
| 11 | Save stores static prose unnecessarily | Medium | Zero modifications to save DTOs; prose is resolved dynamically on load. | Save test: `SaveStoreChecksum_Unchanged_Across_Save_Load` | Negligible |
| 12 | Missing optional file breaks medical loop | High | Loader uses non-fatal fallback; missing files return empty catalog gracefully. | Unit test: `Missing_Catalog_File_Does_Not_Throw_Or_Halt` | Negligible |
| 13 | Catalog update changes mechanical behavior | High | Core simulation systems have zero references to `MedicalTextCatalog`. | Architectural guard: `CoreArchitecture_Zero_Engine_Coupling` | Negligible |
| 14 | Hidden fallback hardcodes competing prose | Medium | Fallback is clean `string.Empty` or `null`; no shadow prose dictionaries in UI code. | Code audit: no fallback prose strings in `src/UI/` | Low |
| 15 | Item ID mismatches | Low | All referenced item IDs cross-checked with `items.json` and canonical aliases. | Validation test: `Required_Item_References_Resolve_To_Known_Ids` | Low |
| 16 | Psychological records receive physical text | Medium | Separate mappings for combat trauma, somatic flashbacks, and guilt insomnia. | Unit test: `Psychology_Afflictions_Map_Only_To_Mental_Prose` | Low |
| 17 | Long-term effects promise mechanics not modeled | Medium | Long-term effects kept descriptive/flavor; no mechanical promises. | Unit test audit | Low |
| 18 | Content-utilization reports loaded without player reachability | Medium | End-to-end integration: `MedicalPanel` and `AfflictionsPanel` actively query and render prose. | Selftest: `--content-utilization-selftest` passes | Negligible |
