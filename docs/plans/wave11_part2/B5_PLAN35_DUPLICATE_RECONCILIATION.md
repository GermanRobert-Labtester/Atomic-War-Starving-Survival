# WAVE 11 PART 2 — TASK B5 DUPLICATE RECONCILIATION

## C1[10] and C2[14] — Plan 35 “Goods Must Arrive”

### Terminal state: RECONCILED-DUPLICATE

## Evidence

| Contract area | C1[10] | C2[14] | Verdict |
|---|---|---|---|
| Source baseline | Plan 35 — *Goods Must Arrive: The Production-to-Provisioning Chain* | Same | Identical lineage |
| Mandatory order | 35A → 36A → 35B → 35C | Same | Identical |
| Mission | Producer output reaches an authoritative sink or yields typed refusal | Same delivery-chain mission | Identical |
| Dependencies | Consume, labour, power, condition, semantic events, fidelity, Plan 36 interlock | Same dependencies expressed in the original source format | No unique executable dependency |
| Guardrails | No rival currency/producer/bill/panel framework; no silent output loss | Same | Identical |
| Current implementation evidence | Wave 10 Part 2 B5: `Plan35ProductionDeliveryTests.cs` 4/4; C1[10] census SEALED | No independent implementation/log | C1[10] is current authority |

## Authority decision

**Class:** TRUE DUPLICATE\
**Authoritative current contract:** `C-integration-plans/C1_planintegration[10].md`\
**Historical sibling:** `C-integration-plans/C2_planintegration[14].md`\

C1[10] is selected because it is the current integration adaptation cited by the live Wave 10 B5 implementation log and census seal. C2[14] retains its Plan 35 body intact but now carries a `SUPERSEDED-BY C1[10]` provenance banner.

## Unique-clause review

Textual forms differ (C1[10] is the more detailed integration adaptation), but no C2[14] clause creates a distinct unimplemented delivery-chain authority. Its Plan 36A interlock remains governed by C2[13] Plan 36, whose current status is separately `PARTIALLY-SEALED`; this does not reopen or duplicate the completed Plan 35 delivery package.

## Corpus-wide scan

`python3 scripts/ci/detect-corpus-duplicates.py --check` reports one exact shared source baseline: **Plan 35**, the pair reconciled here, and verifies that the census contains a reconciliation registry. Historical source-number collisions with different subjects are not duplicates and remain subject to evidence-based reconciliation. The detector reports candidates only; semantic authority remains a human/governance decision.

## Verification

| Command | Result |
|---|---|
| `python3 scripts/ci/detect-corpus-duplicates.py --check` | PASS — one C1[10] / C2[14] source-Plan-35 candidate; census reconciliation registry present. |

## Governance changes

- C2[14] census status: `AUDIT-PENDING` → `RECONCILED-DUPLICATE`.
- C2[14] plan header: provenance-only supersession banner.
- Census duplicate registry: added C1[10] ↔ C2[14] authority row.
- Production diff: zero; this task modified only plan/governance documentation.

## Queue effect

C2[14] is no longer an executable queue node. Wave 10 Part 2 B5 remains the sole Plan 35 delivery-chain implementation owner.
