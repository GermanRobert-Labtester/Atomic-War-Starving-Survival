# Plans 146–149: Gameplay Assumptions & Architectural Invariants

**Plan IDs:** AF-146, AF-147, AF-148, AF-149
**Status:** Canonical Implementation Baseline
**Authority:** `Assets/Ashfall.Core/` (pure C#), `Assets/StreamingAssets/Data/` (JSON)

---

## 1. Plan 146 — EB-PVD Thermal Barrier Coatings

### Gameplay Assumptions
- **Power Coupling:** EB-PVD requires substantial electrical power (6.0 to 12.5 kW). The deposition process is sensitive to voltage stability. Brownouts or total grid loss automatically halt deposition to prevent defective microstructures, requiring manual or automated operator resumption once power stabilizes.
- **Physical Metallurgy:** Physical vapor deposition produces columnar grain microstructures (7YSZ, Pyrochlore, Alumina) that grant critical thermal resistance bonuses (+120°C to +250°C) to superalloy turbine blades, combustor liners, and heavy diesel injectors.
- **Maintenance Lifecycle:** High-vacuum operation relies on electron gun filaments (100h MTBF) and diffusion vacuum pump oil/seals (150h MTBF). Operating without maintenance risks vacuum breakdown, particulate contamination, and chamber arcing.

### Core Architecture Boundary
- `EbPvdCoatingEngine` lives strictly in `Assets/Ashfall.Core/Shelter/`.
- Zero engine dependencies. All power input is queried through `PowerSupplyContext`.
- Inventory transactions are atomic: input ceramics and bond powders are consumed immediately upon starting a cycle, and completed items are placed directly into output storage upon cycle completion.

---

## 2. Plan 147 — Mine-Clearing Flail Vehicle Module

### Gameplay Assumptions
- **Mechanical Clearance:** The demining flail uses a rotating drum with heavy forged steel chains spinning at 350–450 RPM to mechanically strike the ground ahead of an armored vehicle, detonating or neutralizing anti-personnel and anti-tank mines.
- **Attrition & Maintenance:** Blast detonations and mechanical soil impacts erode chain links and degrade the armored blast deflector. If intact chains drop below operational thresholds (<20/40), flail effectiveness collapses, and unexploded mine breach risk spikes.
- **Corridor Safety:** Clearing a minefield is an incremental process. A single breach creates a cleared lane (`ClearedFraction01`), systematically reducing expedition casualty and vehicular loss hazards from 85%+ down to under 5%.

### Core Architecture Boundary
- `MineClearingFlailEngine` lives in `Assets/Ashfall.Core/Expeditions/`.
- Mutates `RouteInfrastructureSystem` (`Assets/Ashfall.Core/World/RouteInfrastructureSystem.cs`) as the single authority on wasteland route conditions, clearance percentages, and hazard states.
- Replay-deterministic random outcomes for detonation events and chain link loss driven by `ISeededRng`.

---

## 3. Plan 148 — Microfluidic Diagnostics

### Gameplay Assumptions
- **Decoupled Diagnostic Assay:** Microfluidic diagnostic chips provide rapid, point-of-care detection of virulent pathogens (cholera, pneumonic plague, radiation-induced sepsis, marrow aplasia) in 15 to 45 minutes, compared to days for traditional microbial culture.
- **Preserved Encapsulation (No Disease Leakage):** The diagnostic engine never modifies or bypasses the underlying `DiseaseSystem`. It queries patient symptomatic/infection presence via an encapsulated read-only predicate, simulating diagnostic sensitivity (false negative rate) and specificity (false positive rate).
- **Soft-Lithography Cartridge Fabrication:** Consumable PDMS silicone elastomeric kits and lyophilized antibody/reagent packs are fabricated in-shelter, creating a tangible logistics loop between chemical stores and clinical triage.

### Core Architecture Boundary
- `MicrofluidicDiagnosticEngine` lives in `Assets/Ashfall.Core/Medical/`.
- Stores clinical records (`DiagnosticResultRecord`) capturing assay confidence, patient ID, and diagnostic classification (`Positive`, `Negative`, `Indeterminate`, `Invalid`).
- Thread-safe, stateless evaluation with deterministic PRNG handling probabilistic results.

---

## 4. Plan 149 — Rail Grinding & Track Corridor Rehabilitation

### Gameplay Assumptions
- **Track Geometry Degradation:** Post-apocalyptic railway corridors suffer severe railhead corrugation, micro-cracking, and roughness caused by seismic tremors, weathering, and unmaintained heavy freight. High roughness forces transit speed caps down to 15–25 km/h and escalates derailment hazards.
- **Grinding Reprofiling:** Rail-grinding draisine passes remove surface metal defects, restoring longitudinal rail profiles, lowering the roughness index, reducing rolling resistance, and restoring line speeds to 50–70 km/h.
- **Abrasive Stone Consumption:** Grinding wears down vitrified corundum grinding stones from 250mm down to 120mm scrap limit, requiring replacement stone inventory and water misting to suppress dry-season spark fires.

### Core Architecture Boundary
- `RailGrindingEngine` lives in `Assets/Ashfall.Core/Expeditions/`.
- Mutates `RouteInfrastructureSystem` rail segment condition records (`RoughnessIndex`, `SafeSpeedLimitKph`, `LastReprofiledDay`).
- Safe speeds and derailment probabilities are authoritative in Core and consumed by `RailwaySystem`.

---

## 5. Unified Save & Host Boundary

- Every system implements `CaptureState()` and `RestoreState()` producing checksummed envelopes managed by `SaveStoreHub` and `SaveStore<T>`.
- UI panels in `src/UI/` are thin presentation-only nodes that bind to host sessions (`EbPvdCoatingHostSession`, `MicrofluidicDiagnosticHostSession`, `MineClearingFlailHostSession`, `RailGrindingHostSession`).
- No UI code computes domain math, alters inventories directly, or bypasses host sessions.
