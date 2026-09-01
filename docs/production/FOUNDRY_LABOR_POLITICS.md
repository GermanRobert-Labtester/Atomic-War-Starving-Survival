# Foundry Labor Politics & Division Canon

This document formalizes the internal divisions, factions, and 8 labor dispute scenarios governing industrial work in the Silent Foundry (`SilentFoundrySystem.TreatyLabor.cs` and `foundry_faction.json`).

---

## 1. Six Internal Divisions

1. **Cupola Masters (`division_cupola_masters`)**: Tenders of the stack, charging bell, and tap hole. Focus on furnace preservation, lining longevity, and refractory safety.
   - *Grievance*: Forcing rapid heats or low-grade flux that damages firebricks.
2. **Mold-Makers (`division_mold_makers`)**: Pattern cutters, green sand sievers, and core setters. Focus on surface finish, dimensional precision, and gating design.
   - *Grievance*: Reusing contaminated sand or demanding fast pours that wash out sprues.
3. **Stokers (`division_stokers`)**: Coal shovelers, slag pullers, and tuyere pokers. High respiratory exposure, heat exhaustion, and physical injury risk.
   - *Grievance*: Shift length exceeding 8 hours without clean drinking water rations.
4. **Assayers (`division_assayers`)**: Metal testers, weigh-hut clerks, and ledger keepers. Balance scrap purity, carbon content, and accord delivery quotas.
   - *Grievance*: Contaminated salvage batches forced into precision heats.
5. **Carters (`division_carters`)**: Ladle haulers, scrap sorters, and ingot transport crews. Connect the foundry floor to the ice road and shelter workshops.
   - *Grievance*: Overloaded ladles, ungreased crane bogeys, and icy transport ramps.
6. **Apprentices (`division_apprentices`)**: Watchers, floor sweepers, and junior tenders. The future generational continuity of the works.
   - *Grievance*: Unsafe tasks, bypassed safety shielding, and missed instruction hours.

---

## 2. Eight Authored Labor Disputes

| Dispute ID | Title | Involved Division | Trigger Condition | Production Consequence | Resolution Choice & Cost |
|---|---|---|---|---|---|
| `dispute_stoker_walkout` | The Boil Order Walkout | Stokers | Water ration deficit during 2+ consecutive heats | Shuts down heat preparation completely | Provide boiled clean water ration (+0.5L/shift) OR delay heat |
| `dispute_mold_grievance` | Washed Core Grievance | Mold-Makers | Quality target forced below 65 | -25% casting quality on next 3 pours | Grant 4 extra mold preparation hours OR accept rough scrap finish |
| `dispute_apprentice_injury`| Burn Box Negligence | Apprentices | Foundry safety incident during overwork | -1 worker availability for 6 days | Pay medical dispensary burn salve OR face safety audit slowdown |
| `dispute_assay_rejection` | Brittle Melt Impurity | Assayers | Unsorted scrap batch loaded into Band 3/4 melt | Blocks tap until chemical flux added | Spend +2 Limestone Flux OR risk brittle catastrophic failure |
| `dispute_carter_refusal` | The Slag Ramp Blockade | Carters | Crane hoist wear exceeds 40% | Blocks finished good transfer to inventory | Assign workshop machinist to lube/repair crane OR carry by hand (fatigue) |
| `dispute_shift_length` | Twelve-Hour Stack Strike | Stokers & Masters | 3 back-to-back heats in under 48 hours | Full strike warning; +30% incident chance | Enforce mandatory 24-hour cool-down OR pay double rations |
| `dispute_safety_shutdown` | Tuyere Gas Blowback | Cupola Masters | Air blower pressure drop below critical threshold| Automatic emergency blast cutoff | Replace bearing housing (`item_foundry_bearing_housing`) |
| `dispute_priority_order` | Sky Armor vs Ice Anchor | Accord vs Shelter | Simultaneous high-priority shelter breach and quota deadline | Splits workforce; -50% throughput on both | Choose primary pour; negotiate 3-day accord extension with Cutters |

---

## 3. Four Strike Escalation Scenarios

1. **Slowdown (`strike_stage_slowdown`)**: +50% labor hours per product; quality unaffected.
2. **Skilled Withdrawal (`strike_stage_skilled_withdrawal`)**: Masters/Assayers walk out; Band 3 and 4 products locked; failure rate +30%.
3. **Full Furnace Halt (`strike_stage_full_halt`)**: All active heats extinguished; no new heats can be prepared.
4. **Treaty-Backed Shutdown (`strike_stage_treaty_lockout`)**: District 8 signatories intervene; coal shipments halted; accord standing decays daily.
