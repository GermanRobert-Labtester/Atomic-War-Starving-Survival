# Crossing Encounter & Crisis Bindings

## 1. Plan 115 Encounter & Crisis Integrations

In `Assets/StreamingAssets/Data/crossing_encounters.json`, encounters and crises authored in Plan 115 integrate directly with the expanded Crossing faction roster:

### 1.1 The Water Committee (`faction_the_water_committee`)
- **Crisis Hook:** `crisis_the_water_claim` ("The Water Claim")
  - *Synopsis:* A maintenance bloc claims that keeping the Crossing's filtration sand beds operational grants it the right to restrict access and tax water draws.
  - *Phases:* Claim → Counter-Petition → Arbitration → Ruling.
  - *Resolution:* Settled through the Standing arbitration assembly: common access, maintenance concession, or metered quota.

### 1.2 The Granary Wardens (`faction_the_granary_wardens`)
- **Crisis Hook:** `crisis_the_grain_riot` ("The Grain Riot")
  - *Synopsis:* Grain deliveries fall behind ration promises, and warehouse inventory no longer matches the public ledger. An angry crowd demands the granary doors be broken open.
  - *Phases:* Shortage → Hoarding Accusation → Distribution → Calm or Collapse.
  - *Resolution:* Settled through arbitration or forfeit paths to distribute reserves, hold for audit, or enforce rationing.

### 1.3 The Quarantine Post (`faction_the_quarantine_post`)
- **Crisis Hook:** `crisis_the_quarantine_break` ("The Quarantine Break")
  - *Synopsis:* A traveler breaks out of the gate isolation ward and enters the market arcade during an active lung-rot alert.
  - *Phases:* Break → Exposure Trace → Containment → Reconciliation.
- **Encounter Hook:** `enc_nc_pestilence_quarantine_lockdown` ("Pestilence Quarantine Standoff")
  - *Target Location:* `loc_crossing_stallrow`
  - *Interaction:* Distribute respirator filters (`water_filter` / PPE) or force passage through the sanitary barrier.

### 1.4 The Smugglers' Court (`faction_the_smugglers_court`)
- **Encounter Hook:** `enc_nc_smuggler_checkpoint` ("Smuggler Checkpoint")
  - *Target Location:* `loc_crossing_watchtower`
  - *Interaction:* Pay an unrecorded road fee in salt (`item_crossing_traded_salt`), ask the Watchtower to test jurisdiction, or turn back.
- **Encounter Hook:** `enc_nc_syndicate_bribe_overture` ("The Under-the-Table Voucher")
  - *Target Location:* `loc_crossing_underwrite_hall`
  - *Interaction:* Reject or accept diesel fuel bribes to bypass cargo manifest inspection.

### 1.5 The Lamplighters (`faction_the_lamplighters`)
- **Encounter Hook:** `enc_nc_forfeit_witness` / `enc_nc_standing_ambush`
  - *Target Location:* `loc_crossing_nightfire` / `loc_crossing_stallrow`
  - *Interaction:* Lantern fuel allocation and night route safety affecting quorum attendance.
