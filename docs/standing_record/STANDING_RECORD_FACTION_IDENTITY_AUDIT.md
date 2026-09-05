# Standing Record Faction Identity Audit

## 1. Global Identity Reconciliation Table

This audit reconciles all proposed Standing Record faction identities against the global repository faction namespace before authoring, fulfilling the requirements of Plan 98 Section 5.

| Intended Standing Record Concept | Proposed ID | Proposed Display Name | Global Exact Match | Semantic Match | Current Global Faction ID if Matched | Relationship / Projection Mechanism Available? | Final Standing Record ID | Final Display Name | Rationale |
|---|---|---|---|---|---|---|---|---|---|
| **Cadastral & Land Survey Bureau** | `faction_the_overlay` | The Overlay | Yes (`standing_record_factions.json`) | Yes | `faction_the_overlay` | Native Standing Record Authority | `faction_the_overlay` | The Overlay | Baseline record preserved byte-for-byte in position 0. Retains institutional identity as the unyielding cadastral survey authority. |
| **Water Utility & Metering Cartel** | `faction_the_scale` | The Scale | No | No (distinct from Holdfast Hydroponics) | None | New Expansion-Local Identity | `faction_the_scale` | The Scale | Distinct civil administration focused on pipeline measurement, flow rates, and sluice transit in the Industrial Belt. |
| **Deed Archive & Arbitration Registry** | `faction_the_compact` | The Compact | No | Fictional Meridian Compact (history lore) | None | New Expansion-Local Identity | `faction_the_compact` | The Compact | Civic registry in Dead Suburbs preserving pre-war boundary deeds and mediating property claims among desperate survivors. |
| **Logistics Insurer & Escort Syndicate** | `faction_the_underwrite` | The Underwrite | No | No | None | New Expansion-Local Identity | `faction_the_underwrite` | The Underwrite | Actuarial security contractor pricing transit risk and underwriting armed fuel convoys out of the Industrial Belt. |
| **Ice Road & Corridor Maintenance** | `faction_the_cutters` | The Cutters | No | No | None | New Expansion-Local Identity | `faction_the_cutters` | The Cutters | Heavy winter-road maintenance fraternity operating in The Cut; controls passability rather than territorial sovereignty. |
| **Working Harbor & Coastal Barge Union** | `faction_the_fleet` | The Fleet | No | Black Flotilla (`flotilla`, maritime raiders) | `faction_black_flotilla` | Autonomous Civilian Maritime Cooperative | `faction_the_fleet` | The Fleet | Distinct from the predatory Black Flotilla; represents working barge operators, dockworkers, and caulk-masters on the Deep Coast. |
| **Agricultural Reconstruction Bloc** | `faction_the_rebuilders` | The Rebuilders | No | Silo Commune (settlement gazetteer) | None | Regional Production Network | `faction_the_rebuilders` | The Rebuilders | Communal agrarian bloc in Ash Flats coordinating soil rotation, grain reserves, and seasonal harvests across rural communes. |
| **Checkpoint & Fortification Authority** | `faction_the_garrison` | The Garrison | Yes (`wasteland_settlement_gazetteer.json`) | Fort Karkov Garrison | `faction_the_garrison` | Canonical Global Projection | `faction_the_garrison` | The Garrison | Aligns directly with canonical Fort Karkov Garrison in Ash Flats; acts as border sentry and checkpoint gatekeeper without duplicate state. |

---

## 2. Collision Analysis & Disambiguation Details

### 2.1 The Garrison (`faction_the_garrison`)
- **Repository Search Findings:** `faction_the_garrison` is already declared in `Assets/StreamingAssets/Data/narrative/wasteland_settlement_gazetteer.json` as the governing authority of Fort Karkov in the Ash Flats.
- **Resolution:** Reconciled as a canonical projection. The Standing Record dossier defines the bureaucratic and trade interface for the Fort Karkov checkpoint authority. Zero duplicate IDs or competing organizations were created.

### 2.2 The Fleet (`faction_the_fleet`)
- **Repository Search Findings:** Plan 23 and narrative files reference the "Black Flotilla" (`faction_black_flotilla`), a hostile maritime raider syndicate.
- **Resolution:** Disambiguated. "The Fleet" in Standing Record is explicitly authored as a working dockworkers' and barge-operators' cooperative operating coastal transport and salvage tolls on the Deep Coast. Their signature quote and access rules highlight civilian cargo haulage and seam integrity, preventing any narrative confusion with the militarized raiders.
