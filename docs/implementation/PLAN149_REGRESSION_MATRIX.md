# Plan 149 regression matrix

| Area | Coverage | Expected result |
|---|---|---|
| Catalog parsing | 27 records, schema 1, required display fields | All records load; invalid schema/field/day fails closed |
| ID integrity | Duplicate authored ID and duplicate runtime-map ID fixtures | Load error; empty activation catalog |
| Truth safety | Unknown/missing truth mapping fixture | Unsafe record cannot be discovered |
| Provenance | 27 human-readable locations, zero unproven room IDs | Text preserved; no phantom room objects |
| Identity | Exact Suki/Elena catalog checks and unresolved short names | Display-only unless exact identity is proven |
| Related records | Explicit ID edges and missing-target fixture | Valid links navigate; missing edge is filtered with warning |
| Producer gating | Archive, medical, shelter records and duty roster routes | Only explicit producer records are considered |
| Day gating | Gate-house Day 1 and Day 4 fixture | Bram transfer first; evacuation becomes eligible at Day 4 |
| Idempotence | Repeated producer calls and repeated panel opens | One knowledge key and one codex increment |
| Save/restore | Journal capture and restore after discovery | Discovery survives; restore emits no unlock |
| Old saves | Journal save without Plan-149 keys | No fabricated discoveries |
| Simulation isolation | Static document read/discovery | No inventory, medical, population, staffing, room, quest, faction or infrastructure mutation |
| UI rebinding | Existing JournalBookUI / JournalCodex path | Rebinding only rebuilds rows; it does not discover records |
| Reader resilience | Long transcripts, line breaks and punctuation | Existing scrollable JournalBookUI body renders the source transcript |
| Content utilization | Static scanner, runtime collector and Journal surface | Both catalog files map to typed loader, registry, Journal systems and JournalPanel |
| Deterministic order | Catalog sort by `posted_day`, then ordinal `doc_id` | Same data and save state produce same rows |
| Template boundary | Candidate forms | No generated report path or static-to-live quantity transfer |

Negative fixtures cover unknown producer/document, invalid day, missing title/transcript, duplicate IDs, missing mapping, unknown related target and invalid truth class. The full verification result is recorded in `PLAN149_COMPLETION_REPORT.md`.
