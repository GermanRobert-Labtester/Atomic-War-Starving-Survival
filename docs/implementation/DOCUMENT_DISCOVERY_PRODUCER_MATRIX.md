# Document discovery producer matrix

Discovery is explicit and idempotent. A producer is a stable source-context token passed to `BureaucraticDocumentDiscoverySystem`; it is not a string command and is never resolved through reflection. The system checks the document ID, exact producer, current day, truth class and Journal knowledge ledger before writing one key.

| Producer | Current route | Records it can expose |
|---|---|---|
| `archive_desk` | Expanded Shelter → Archive Desk | Requisitions, inspections, maintenance chits and the filed theft report, plus the seed requisition |
| `medical_office` | Medical panel and Medical Ward | Medical requisition, patient transfer, injury report, mortality report and quarantine order |
| `shelter_records` | Shelter panel / shelter records surface | Loma and unidentified assignment slips, ration denials, meal notice and school notice |
| `duty_roster` | Duty Roster panel | Sentry and kitchen roster records |
| `quartermaster_desk` | Explicit source context reserved for a quartermaster handoff | Requisitions, ration/census records and theft report; no global unlock is run |
| `ration_board` | Explicit source context for a ration-board handoff | Ration denial and flour meal records |
| `admissions_desk` | Explicit source context for an admissions handoff | Assignment slips and census correction |
| `maintenance_office` | Explicit source context for a maintenance-office handoff | Requisitions, inspections, injury report and maintenance chits |
| `mess_hall_notice_board` | Explicit source context for a mess-hall notice handoff | Second-helping notice, flour notice and kitchen roster |
| `gate_house_notice_board` | Explicit source context for a gate-house handoff | Bram transfer, sentry roster, dam evacuation and weapons amnesty |
| `ward_b_notice_board` | Explicit source context for a Ward B notice handoff | Quarantine order and laundry notice |
| `school_corridor` | Explicit source context for a Ward C school-corridor handoff | School-hours notice |

The first four routes are wired to the existing Godot surfaces. The remaining tokens document safe handoff seams for physical/archive producers; they do not auto-unlock on day advance. A document may be visible as a locked row before discovery because the existing Journal Events tab supports that presentation, but its transcript is only rendered after its producer gate succeeds.

The vertical slice is therefore covered by real routes: ration (`shelter_records`), medical (`medical_office`), maintenance (`archive_desk`), assignment (`shelter_records`), roster (`duty_roster`) and institutional notice (`shelter_records`).
