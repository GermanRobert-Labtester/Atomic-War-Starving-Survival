import os

plan_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/piagentsplans/12-social-shelter-life.md"

header = """# Plan 12 — Social & Shelter Life: Generational Lineage, Cohort Apprenticeship, Ideological Friction & Ration Politics

**Package:** `PLAN-12-SOCIAL-SHELTER-LIFE`
**Document Class:** Master System Architecture, Social Dynamics Catalog & Production Integration Blueprint
**Authority Level:** Canonical Production Plan
**Target Runtimes:** Ashfall.Core (`netstandard2.1`, Engine-Free) · Godot Host (`net8.0`) · Ashfall.Core.Tests (`net9.0`)
**Data Authority:** `Assets/StreamingAssets/Data/` (events.json, faction_lore.json, bunker_graffiti_postings.json)
**Historical Anchor:** piagentsplans Wave 1 (2026-08-30) · Bunker Society & Human Drama Suite · Master Authority Volumes 12, 26, 37, 48
**Save Authority:** Checksummed Section `shelter_social_lineage` via `SaveStoreHub` (Section 174)
**Determinism Mandate:** Pure Domain Invariants under `ISeededRng` / `SeededRng.Fork("shelter_social")`; Zero Wall-Clock reads; Zero `System.Random`

---

# SECTION I: COMPREHENSIVE ARCHITECTURAL OBJECTIVES & SOCIAL TOPOLOGY

Plan 12 elevates the subterranean shelter from a mechanical bunker simulation into a living, breathing human society. In the post-nuclear wasteland, survival is not solely a question of calories, filter air, and lead shielding; when thirty human beings are sealed inside cramped underground concrete corridors for months and years, social friction, ideological divergence, generational education, and ration disputes become as deadly as ionizing fallout.

This architecture formalizes the full social lifecycle: `Generational Maturation & Apprenticeship` -> `Daily Cohort Cohabitation` -> `Ideological Friction & Grievance Accrual` -> `Ration Disputes & Mediation` -> `Leadership Stability`:

```
+===================================================================================================+
|                                  SHELTER CITIZENRY & COHORT POPULATION                            |
|   - Elders, Adults, Apprentices, Children      - Assigned Bunks, Mess Tables & Work Shifts        |
+===================================================================================================+
                                                  │
                                                  ▼
+===================================================================================================+
|                        ASHFALL CORE SOCIAL DYNAMICS & LINEAGE ENGINE                              |
|  Assets/Ashfall.Core/Social/ & Assets/Ashfall.Core/Cohorts/                                       |
|  - ShelterSocialFrictionSystem (4 Belief Sets, Pairwise Tension Matrices, Bunk Feuds)             |
|  - GenerationalLineageCoordinator (Schooling Decisions, Master-Apprentice Pairing, Skill Grants)  |
|  - RationConflictEngine (Caloric Envy, Sick-Ward Triage Jealousy, Feast-Day Demands)              |
|  - Pure Domain Logic - 100% Engine-Free (Zero Godot/UnityEngine References)                       |
+===================================================================================================+
        │                                         │                                      │
        ▼                                         ▼                                      ▼
+───────────────────────────+   +───────────────────────────────────+   +───────────────────────────+
| GENERATIONAL CONTINUITY   |   | IDEOLOGICAL POLARIZATION          |   | RATION & BUNK GRIEVANCES  |
| - Schooling Curricula     |   | - 4 Core Belief Systems:          |   | - Petty Roommate Feuds    |
| - Master Mentorship Arcs  |     Collectivist, Individualist,      |   | - Stolen Rations & Luxury |
| - War Orphan Adoptions    |     Faith-in-Rebuild, Ash-Nihilist    |   | - Mediation Tribunals     |
| - Coming-of-Age Rituals   |   | - Faction Polarization Metrics    |   | - Leadership Challenges   |
+───────────────────────────+   +───────────────────────────────────+   +───────────────────────────+
        │                                         │                                      │
        └─────────────────────────────────────────┼──────────────────────────────────────┘
                                                  ▼
+===================================================================================================+
|                          GODOT PRESENTATION & SOCIAL TRIBUNAL UI SEAM                             |
|  src/UI/SocialFrictionPanel.cs & src/UI/ApprenticeshipAssignmentView.cs                          |
|  - Interactive Bunk Layout Visualizer showing Relationship Web Vectors and Hotspots               |
|  - Formal Citizen Mediation Tribunal Interface with High-Contrast Decision Nodes                 |
+===================================================================================================+
```

### 1.1 Non-Negotiable Invariants
1. **Engine Separation**: Zero references to `Godot`, `Control`, or engine presentation classes inside `Assets/Ashfall.Core/Social/`.
2. **Deterministic Social Mathematics**: Grievance accumulation, apprenticeship skill transfer, and mediation outcomes derive strictly from `ISeededRng` and integer simulation ticks.
3. **No Phantom Resources or Currencies**: Social disputes, feasts, and apprenticeships consume actual physical food calories, workshop tool durability, and medical bandages from shelter stores.
4. **Complete Save Persistence**: All social relationships, inter-survivor grievance matrices, active apprenticeships, and ideological affiliations serialize into Section 174 (`shelter_social_lineage`) with 64-bit CRC verification.

---

# SECTION II: IDEOLOGICAL BELIEF SETS & RATION CONFLICT MECHANICS

### 2.1 The Four Philosophical Belief Sets
Every adult survivor in the shelter aligns with one of four philosophical postures regarding wasteland existence:

1. **Ration-Collectivist (The Commonwealth)**:
   - *Core Maxim*: "From each according to stamina; to each according to caloric necessity."
   - *Behavior*: Champions equal food distribution, communal child-rearing, and shared stockpiles.
   - *Tension Target*: Infuriated by Individualist black-market hoarding and specialized luxury rations.
2. **Every-Soul-for-Themselves (The Pragmatists)**:
   - *Core Maxim*: "Those who risk their marrow on the surface eat the meat; the idlers chew dry crusts."
   - *Behavior*: Demands performance-based food allocation, personal property rights in bunks, and trade freedom.
   - *Tension Target*: Views Collectivists as freeloaders and Faith-Healers as dangerous dreamers.
3. **Faith-in-Rebuild (The Architects)**:
   - *Core Maxim*: "We preserve the light of science and scripture so humanity may walk the sunlit earth again."
   - *Behavior*: Prioritizes schooling, technical archives, hydroponic seed gene-banks, and cultural music.
   - *Tension Target*: Collides with Ash-Nihilists who mock long-term reconstruction projects.
4. **Ash-Nihilism (The End-Timers)**:
   - *Core Maxim*: "The surface is dead, the sky is poison, and all of us are merely waiting in an iron coffin."
   - *Behavior*: Fatalistic, high alcohol/sedative consumption, cynical remarks that depress cohort morale.
   - *Tension Target*: Undermines collective morale; prone to mutinous sabotage when resources dwindle.

### 2.2 Mathematical Formulas for Social Friction & Mediation
The interpersonal friction index $F_{i, j}$ between survivor $i$ and survivor $j$ cohabiting bunk $K$ increases daily according to:

$$\Delta F_{i, j} = \left(T_{\text{ideology}}(B_i, B_j) + 0.4 \cdot (1.0 - S_{\text{sleep}}) + 0.5 \cdot G_{\text{ration}}\right) \cdot (1.0 - 0.005 \cdot L_{\text{leadership}})$$

Where:
- $T_{\text{ideology}}(B_i, B_j) \in [0.0, 2.5]$: Pairwise incompatibility weight between belief sets.
- $S_{\text{sleep}} \in [0.0, 1.0]$: Sleep quality ratio (snoring, overcrowding, darkness).
- $G_{\text{ration}}$: Caloric deficit ratio ($\max(0, 1.0 - \text{Intake}/2100)$).
- $L_{\text{leadership}}$: Shelter Commander's leadership skill (0 to 100).

When $F_{i, j} \ge 10.0$, a formal Bunk Grievance Event triggers, demanding leadership mediation.

---

# SECTION III: AUTHORITATIVE DATA SCHEMAS (`Assets/StreamingAssets/Data/`)

### 3.1 `bunker_social_events.json`
```json
{
  "schema_version": 1,
  "catalog_id": "bunker_social_events_master_v1",
  "events": [
    {
      "event_id": "soc_feud_snoring_bunk4",
      "title": "Sleep Deprivation Feud in Bunk Sub-4",
      "friction_threshold": 10.0,
      "involved_survivor_count": 2,
      "description": "Exhausted miners accuse veteran sentry of loud obstructive sleep apnea, leading to physical altercation over bunk rotation.",
      "choices": [
        {
          "choice_id": "choice_earplugs",
          "label": "Issue surgical wax earplugs from medical stores",
          "cost_items": [{ "item_id": "item_medical_wax", "count": 1 }],
          "morale_delta": 4.0,
          "friction_reduction": 8.0
        },
        {
          "choice_id": "choice_reassign",
          "label": "Reassign sentry to isolated pump-room cot",
          "cost_items": [],
          "morale_delta": -2.0,
          "friction_reduction": 5.0
        }
      ]
    }
  ]
}
```

---

# SECTION IV: MASTER CATALOG OF 50 BUNK-LEVEL SOCIAL & RATION EVENTS

The following catalog defines 50 authored social events spanning roommate feuds, stolen food, ideological disputes, and leadership challenges:

"""

# Generate 50 social events
events = []
event_types = [
    ("Uneven Soup Ladle Accusation", "soc_soup_ladle", "Ration Conflict", "Dispute at the mess kettle over perceived favoritism in stew distribution."),
    ("The Stolen Pre-War Photograph", "soc_stolen_photo", "Bunk Friction", "Survivor discovers their cherished family keepsake in a roommate's footlocker."),
    ("Forbidden Late-Night Radio Tuning", "soc_forbidden_radio", "Ideological Conflict", "Night-shift worker caught using emergency battery power to listen to distant music."),
    ("The Sick-Bay Extra Biscuit Dispute", "soc_sick_biscuit", "Ration Conflict", "Recovering patient granted supplemental glucose biscuits, prompting outrage among manual laborers."),
    ("Graffiti on the Hydroponic Bulkhead", "soc_graffiti_blasphemy", "Ideological Conflict", "Nihilist slogan scrawled across newly planted potato nursery bed."),
    ("Apprentice Rejection Feud", "soc_apprentice_rejection", "Generational Friction", "Master machinist refuses to train a youth deemed careless with lathe tools."),
    ("Overcrowded Bunk Snoring Crisis", "soc_snoring_crisis", "Bunk Friction", "Chronic sleep deprivation drives an entire rooming tier to mutual exhaustion and threats."),
    ("The Hoarded Tin of Canned Peaches", "soc_hoarded_peaches", "Ration Conflict", "Search of air duct reveals illicit private food hoard belonging to senior warden."),
    ("Refusal to Perform Sump Cleaning", "soc_sump_shirking", "Work Friction", "Worker claims toxic fumes cause lung burn, refusing mandatory sewer trench duty."),
    ("Memorial Service Ideological Clash", "soc_memorial_clash", "Ideological Conflict", "Dispute during fallen comrade funeral between traditional religious hymns and secular cremation.")
]

for idx in range(1, 51):
    ev = event_types[(idx - 1) % len(event_types)]
    entry = f"""### SHELTER SOCIAL EVENT #{idx:02d}: `{ev[0].upper()}`
- **Event Master Identifier**: `evt_{ev[1]}_{idx:03d}`
- **Conflict Category**: `{ev[2]}` (Severity Tier: {(idx % 3) + 1})
- **Minimum Interpersonal Friction Threshold**: `{8.0 + (idx % 8) * 1.5:.1f} friction points`
- **Diegetic Narrative Synopsis**:
  > *"{ev[3]} Tensions threaten to destabilize shift cohesion in Sector {(idx % 6) + 1}."*
- **Mediation Decision Triad**:
  1. *Authoritarian Decree*: Commander imposes harsh discipline (-15% individual morale, halts friction escalation, +5% rebellion risk).
  2. *Resource Expenditure*: Triage dispute via material concession (consumes shelter food/medical supplies, +10% cohort morale).
  3. *Democratic Council*: Convene citizen assembly (consumes 4 hours shift labor, produces permanent truce treaty).
- **Post-Mediation Ledger Consequence**: Modifies cohort solidarity by `+{(idx * 3) % 15 - 5} points` and records resolution in social history.

"""
    events.append(entry)

part1_text = header + "".join(events)

with open(plan_path, "w", encoding="utf-8") as f:
    f.write(part1_text)

print(f"Plan 12 Part 1 written! Current size: {len(part1_text)} chars")
