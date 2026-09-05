# Verdict Witness Handoff Contract (Plan 84 Preparation)

> **Scope:** Preparing physical site evidence for downstream consumption by Plan 84 (Muster Witnesses & Testimony System) without inventing pre-mature runtime dependencies.

---

## 1. Witness Deposition Seams

Plan 82 establishes environmental facts that future witness depositions can corroborate, dispute, or clarify:

| Location | Physical Fact Established in Site Prose | Downstream Witness Handoff (Plan 84) |
|---|---|---|
| `loc_geological_core_vault` | Core box labeled "REF: VALE" with surveyor's plumb bob | Eden Vale can testify regarding her father's seismic survey work and the relocation of the drilling logs. |
| `loc_border_checkpoint_ruins` | Hidden logs of nocturnal transports stamped with Tempest anchor | Selya Saltmarsh can corroborate off-count census numbers from convoy manifests she was forbidden to audit. |
| `loc_sealed_marine_laboratory` | Specimen tags typed on linen shift charter material | Iaran Bell can confirm that laboratory personnel were issued standard Tempest maintenance vouchers. |
| `loc_network_fuse_bunker` | Solenoid clicking on automated schedule; swept dust path | Ferris Voss can explain why the fire-control pause order has never been rescinded. |
| `loc_minefield_observation_tower` | Watch logs recording seismic ground shock before sky flash | Observation post log corroborates that the Tempest Array triggered milliseconds prior to atmospheric burst. |

---

## 2. Invariant Rules for Witness Data

1. **No Speculative Dialogue in Location Data:** Location entries contain only physical evidence, physical anomalies, and concrete documents.
2. **Flag-Based Enrollment:** Discovering site evidence sets canonical flags (`flag_verdict_*`), which Plan 84 will check to unlock relevant testimony lines.
