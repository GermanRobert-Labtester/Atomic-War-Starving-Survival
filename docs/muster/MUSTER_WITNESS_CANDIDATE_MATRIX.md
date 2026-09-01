# Muster Witness Candidate Matrix (Plan 25 · 25B.3)

> Verified flag/ID availability 2026-09-01. **Rule: a witness exists only if they can testify to a concrete player decision via real campaign flags.** Candidates whose flags don't exist are substituted or their flags are authored at a real producer (never dead content, per plan §25G.12/§25G.14).

| # | Candidate | Source plan | Alive condition | Encounter/helped flag | Failed flag | Faction | Testimony theme | Binding status |
|---|---|---|---|---|---|---|---|---|
| W1 | The Messenger's Keeper (substitutes "spared warlord") | 10A adjacent | flag-gated (institutional) | `flag_messenger_kept` (exists, `MoralChoiceIds`) | `flag_become_warlord` (exists) | — (warlord orbit) | What the player's mercy/cruelty toward the collector's network cost | SOLID (substituted: no spared-warlord flag exists) |
| W2 | The Claimant Auditor | 18A | census/quest-gated | `quest_holdfast_census_claimant_audit` resolved + restitution action flag | `flag_grievance_hydro_claim_defrauded` | faction_hydro_barons | A corrected vs stolen claim | PARTIAL → action flag authored by A2/A3 |
| W3 | The Scavenger Claimant | 25A | guild active | `flag_favor_scavenger_arbitration_fair` (A2) | `flag_grievance_scavenger_claim_dispute` (A1) | faction_scavenger_guild | Fair arbitration vs ignored claim | SOLID (flags authored by 25A slice) |
| W4 | The Foundry Striker | 22C | foundry standing | `SilentFoundryIds.JournalStrike` + accord-respect flag | `flag_grievance_foundry_strike_broken` (authored at A-set or foundry action) | faction_silent_foundry | Labor respected vs coerced | PARTIAL → strike flag exists; respect flag authored |
| W5 | The Hydro Envoy | 25A | hydro active | `flag_favor_hydro_water_accord_honored` (A3/A4) | `flag_grievance_hydro_toll_defaulted` (A3) | faction_hydro_barons | Water leverage honored vs worsened | SOLID (25A flags) |
| W6 | The Raider Parley Survivor | 25A | raider active | `flag_favor_raider_parley_honored` (A5) | `flag_grievance_raider_parley_broken` (A5) | faction_iron_raiders | Terms kept vs betrayed under their code | SOLID (25A flags) |
| W7 | The Raised Child | 12A | lineage `childId` isActive, relationshipType adopted/mentor | lineage record + shelter survival | — (positive-only + neutral) | — (shelter) | Grew up under the player's policy | SOLID (existing `LineageRecord`) |
| W8 | The Palliative Nurse | 09 | survivor census | `SickListSystem.palliativePlan` assigned | triage-denial flag (authored at medical action if absent) | — (shelter) | How the dying were treated | SOLID (palliative exists; denial variant flags documented) |
| W9 | The Treaty Envoy | 16C | institutional | treaty ratified via read-model/system | `flag_treaty_*_violated` / `TreatyStatus.Violated` | varies | Accords honored vs enabled breach | SOLID (16-treaty corpus + system state) |
| W10 | The Camp Medic (Coalition) | 25B | camp formed | `CoalitionCampState.membersRallied` ≥ threshold + supply appeal (A8) | strategy D (`the_blood_price`) or lockout ≥ 60 | faction_deserter_coalition | Neutral ground kept vs sold | SOLID (existing camp state) |
| W11 | The Deserter Elder | 06C-adjacent | camp formed | `flag_peace_faction_forms` (E-R3) or rally record | `flag_war_requisition_refused` etc. | faction_deserter_coalition | Why the exhausted still came | SOLID (E-R3 produces flag) |
| W12 | The Shelter Dissenter | shelter social | living survivor | dissent-tolerated flag (authored at a real policy decision) | dissent-silenced flag (authored likewise) | — (internal) | The player's own house, testifying | PARTIAL → flags authored with producer decisions |

**Substituted archetypes (per plan §25B.5 suggestion list):**
- "Spared warlord" → W1 (no spared flag exists in `WarlordDoctrineState`; substitution documented instead of authoring a false producer).
- "Rescuee" (24B) → replaced by W2/W4/W12 (no rescuee ids/flags exist; plan §25G.12 forbids dead content).
- "Expedition survivor" → covered by W11/camp set (expedition flags exist in the expedition domain but no stable "left behind" testimony pair was verified; deferred rather than fabricated — recorded as a Plan 25 limitation in the closeout).

**Eligibility classes used by `WitnessSelector`:** alive/dead (subject census), encountered (flag), helped (flag), failed (flag), faction-present (system state), institutional exception (W5, W9, W10 may be summoned regardless of personal encounter — documented per plan §25B.9).

Every witness carries ≥2 testimony variants (helped / failed, plus complicated where authored). Variant flag map lives alongside the witness data in `muster_witnesses.json` v2 (`requires_*_flags`) — no prose-based inference.
