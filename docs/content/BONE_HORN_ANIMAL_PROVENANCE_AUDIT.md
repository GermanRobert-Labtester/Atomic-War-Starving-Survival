# Bone, Horn & Antler Animal Provenance Audit

The source fields describe material provenance in authored logs. They do not identify a current carcass, living animal or companion. The adapter therefore emits a single display identity status for every record: animal/material/tool labels are historical and no live entity is inferred.

| Authored label | Classification | Canonical cross-reference | Death/continuity decision |
|---|---|---|---|
| dog | generic animal label | no exact wildlife ID; no matching survivor ID | historical anonymous material; never binds to a dog companion |
| rabbit | generic animal label | trapping data uses a generic rabbit label, but no exact source identity exists | no individual or death event inferred |
| rat | generic animal label | related species_blight_rat exists, but the source does not identify it | no wildlife state or harvest inferred |
| cat | generic animal label | no exact canonical wildlife or survivor identity | display-only provenance |
| mixed_bird | aggregate material label | no species or individual is declared | display-only provenance |
| deer_antler | material label | no exact canonical deer identity | antler_horn_001 explicitly says old shed; no killing or death inferred |
| goat_horn | material label | species_feral_goat is a semantic candidate only, not an authored identity match | no goat harvest or mortality inferred |
| cattle_horn | material label | no exact canonical cattle entry located | unresolved historical material; no world-state change |
| mixed_offcuts | processed material batch | no animal identity remains | display-only provenance |

The source corpus was scanned against survivors.json and canonical wildlife catalogs. None of the bone source labels or blank labels is an exact survivor ID. The dog records therefore cannot affect a living companion, and no epitaph/death, wildlife drop, population, disease or harvest event is emitted by discovery.

The records do contain ordinary survival language about grease, drying, leather and tool use. It is preserved as authored history. Nothing in the projection certifies sanitation, sterility, medical suitability or current resource availability. Antler is specifically presented as capable of being naturally shed where the authored log says so.
