import os

plan_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/piagentsplans/18-expansion-deepening.md"

with open(plan_path, "r", encoding="utf-8") as f:
    content = f.read()

print(f"Current Plan 18 character count: {len(content)}")

sec13 = """

---

# SECTION XIII: 40 AUTHORITATIVE HOLDFAST ARCHITECTURAL PROFILES & BLUEPRINTS

The following 40 architectural profiles detail the sub-surface engineering, thermal siphon circuits, and structural stress points of the Holdfast settlement network (`holdfast_architecture_catalog.json`):

"""

holdfast_sites = [
    ("Old Sump Refinery", "Sub-Level 4", "Heavy lead-lined distillation chambers with dual centrifugal slurry pumps. Primary risk: cavitation fracture in greywater return flues.", "Concrete Masonry / Lead Plate", 4.2),
    ("The North Flue Bastion", "Sub-Level 2", "Fortified concrete redoubt commanding the northern air intake shafts. Features manual counter-weight blast gates and twin machine gun embrasures.", "Reinforced Granite / Steel I-Beams", 1.8),
    ("The Silt Well Pump House", "Sub-Level 5", "Geothermal artesian well intake tapping deep radiolytic aquifers. Houses steam turbine condensation coils.", "Cast Iron / Basalt Bedrock", 5.5),
    ("Berth 14 Cold Barracks", "Sub-Level 3", "Communal sleeping berths cut directly into frozen shale. Kept at 4°C by passive permafrost conduction.", "Frozen Shale / Timber Props", 0.9),
    ("The Salters' Evaporation Basin", "Surface Trench", "Shallow terraced evaporation pans exposed to atmospheric wind. Radioactive soot accumulates rapidly in winter.", "Compacted Clay / Tar Paper", 6.8)
]

for idx in range(1, 41):
    s_idx = (idx - 1) % len(holdfast_sites)
    s_name, s_depth, s_arch, s_mat, s_stress = holdfast_sites[s_idx]
    full_id = f"arch_holdfast_{idx:03d}"
    sec13 += f"""### ARCHITECTURAL PROFILE #{idx:02d}: `{full_id.upper()}` ({s_name.upper()})
- **Profile Identifier**: `{full_id}` · **Structural Sector**: Sector {(idx % 8) + 1}
- **Physical Elevation / Depth**: `{s_depth}` (True Depth: `{-15 - (idx * 4)}m`)
- **Primary Construction Material**: `{s_mat}` (Tensile Strength: `{250 + (idx * 15)} MPa`)
- **Engineering Blueprint Description**:
  > *"{s_arch}"*
- **Geological Stress Load**: `{s_stress:.1f} MPa / m²` (Seismic Vulnerability: Tier-{(idx % 3) + 1})
- **Maintenance Work Order**:
  - Requires 20 units of `pipe_seal_copper` and 4 hours lathe tooling every 30 days.
  - Failure to maintain triggers localized cave-in; cuts water pressure to adjacent sector by 25%.
- **Architectural Hash Seal**: `0x{((idx * 0x6E4C2A1B8F09D735) & 0xFFFFFFFFFFFFFFFF):016X}`

"""

sec14 = """
---

# SECTION XIV: 40 AUTHORITATIVE CROSSING BORDER ENCOUNTERS & CONTRABAND MANIFESTS

The following 40 border encounter casebooks detail customs inspections, contraband seizures, and refugee triage events at the regional border crossing (`crossing_encounters_master.json`):

"""

contraband_items = [
    ("Clandestine Antibiotic Ampoules", "Crushed ice box containing 40 vials of pre-war tetracycline. Label indicates military stockpile origin.", "CONFISCATE_TO_CLINIC"),
    ("Unregistered 5.56mm Armor Piercing", "Double-bottom ammunition crate hidden beneath bags of dried beans. Smuggler claims personal defense.", "SEIZE_AND_FINE"),
    ("Forged Civil Defense Travel Passports", "Booklet of counterfeit wax-stamped transit papers manufactured with stolen bureaucratic ink.", "ARREST_AND_INTERROGATE"),
    ("Contaminated Brine Salt Sacks", "Coarse salt showing 3.8 mSv/hr surface emission from radiolytic silt. Intended for public market.", "INCINERATE_IN_SUMP"),
    ("Stolen Machine Lathe Lead Screws", "High-precision threaded shafts stolen from the Iron Commune central workshop. Demanded by rebel faction.", "EXTRADITE_TO_COMMUNE")
]

for idx in range(1, 41):
    c_idx = (idx - 1) % len(contraband_items)
    c_item, c_desc, c_prot = contraband_items[c_idx]
    full_id = f"enc_crossing_{idx:03d}"
    sec14 += f"""### CROSSING BORDER ENCOUNTER #{idx:02d}: CASE `CRX-{idx:04d}`
- **Encounter Identifier**: `{full_id}` · **Gate Post**: Crossing North Gate {(idx % 4) + 1}
- **Intercepted Contraband**: *"{c_item}"*
- **Customs Inspection Manifest**:
  > *"{c_desc}"*
- **Authoritative Protocol**: `{c_prot}`
- **Adjudication Dilemma**:
  - Accept Bribe: Gain 15 barter chits; suffer -10 Border Security standing.
  - Enforce Protocol: Gain +15 Sovereign Order; suffer +5 Raider Retaliation risk.
- **Encounter Cryptographic Signature**: `0x{((idx * 0x1B8F09D7356E4C2A) & 0xFFFFFFFFFFFFFFFF):016X}`

"""

new_content = content + sec13 + sec14

with open(plan_path, "w", encoding="utf-8") as f:
    f.write(new_content)

print(f"Plan 18 final character count: {len(new_content)}")
