# Power Room Identity Model

> **Contract:** Formal characterization of power entry identity and topology across ASHFALL.

---

## 1. The Model: Hybrid Architectural Model (Model D)

Analysis of `Assets/StreamingAssets/Data/power_grid.json` and `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs` confirms that power consumers follow **Model D (Hybrid)**:

1. **Canonical Room Spaces:** Physical facilities that exist as specific rooms within the shelter topology:
   - `room_clinic`: Field Clinic (Medical triage and treatment)
   - `room_greenhouse`: Hydroponic cultivation bay
   - `room_foundry`: Silent Foundry cupola and casting floor
   - `room_workshop`: General fabrication and machine bench
   - `room_kitchen`: Galley kitchen ration preparation
   - `room_radio_tuner`: Radio communications transceiver alcove
   - `room_laboratory_research`: Scientific analysis and archival workstation
   - `room_armory_munitions`: Munitions storage, reloading press, and powered lockers
   - `room_storage_secure`: Reinforced armored vault and refrigerated stores
   - `room_common_mess_hall`: Communal dining and muster area
   - `room_bunks`: Standard dormitory and sleep quarters
   - `room_airlock`: Surface airlock and chemical decon arch
   - `room_ward_quarantine`: Negative-pressure isolation ward

2. **Canonical Electrical Services / Infrastructure Circuits:** Life-support distribution and structural utility networks spanning the bunker:
   - `room_air_filtration`: The main HEPA intake and blower stack circuit (powers ventilation across all bunker sectors)
   - `room_water_pump`: Deep wellhead motor and lift pump circuit
   - `room_water_treatment`: Chemical dosing, UV purification, and reverse-osmosis plant
   - `room_lighting_main`: Concourse and arterial corridor low-voltage lighting bus
   - `room_surveillance`: Perimeter sensor network, exterior low-light cameras, and trip lines

---

## 2. Multi-Instance vs. Aggregate Rule

- Built rooms register their power load as aggregate functional circuits.
- The shelter grid represents the electrical service connection for each distinct functional category.
- Unbuilt rooms do not consume active power in the live campaign; `PowerGridState.ClosedBreakers` or registration sets default states upon construction.
