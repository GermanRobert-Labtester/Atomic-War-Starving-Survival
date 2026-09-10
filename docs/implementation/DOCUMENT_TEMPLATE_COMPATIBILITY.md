# Document template compatibility

The audit found 11 structurally reusable forms: three requisitions, two inspection sheets, two rosters and three maintenance chits plus the seed requisition. They are classified `template_compatible_record` because a future live report could reuse their shape. Their authored transcripts are still archival snapshots.

Plan 149 does not implement generated reports. This keeps the schema simple and avoids a second document language. There are no placeholders, prose parsers, OCR paths, quantity extraction rules or state snapshots in the runtime.

| Candidate form family | Static authored record | Live projection decision |
|---|---|---|
| Requisition | Fuel, medicine, candles, seed potatoes | Deferred; live inventory/medical/greenhouse owners would need typed projection contracts |
| Inspection | Generator and air filters | Deferred; generator and ventilation systems remain the only state authorities |
| Maintenance chit | Lamp, pump and hatch seal | Deferred; maintenance queue and infrastructure systems remain the only state authorities |
| Shift schedule | Sentry Week 7 and Kitchen Week 9 | Deferred; duty roster remains the staffing authority |

If a generated form is added later, it must use a distinct generated-document definition with typed fields from the owning system, a `CURRENT GENERATED REPORT` provenance badge, and a declared snapshot time. It must never mutate or parameterize the static `transcript` in place. A compact owner state/reference is preferred for persistence; the full text should be recreated on read.
