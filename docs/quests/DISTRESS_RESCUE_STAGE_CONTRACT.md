# ASHFALL Distress Rescue Mission Stage Contract

> **Document Status:** Authoritative Lifecycle Contract & State Machine
> **Subsystem:** Radio Distress Missions / Quest Runtime Integration
> **Authority:** Flagship Integration Plan (Task 5)
> **Core Manager:** `Ashfall.Core.Radio.DistressRescueMissionManager`
> **Catalog Authority:** `Assets/StreamingAssets/Data/questline_master.json`

---

## 1. Objective

Provide an explicit, deterministic mission-discovery loop in which a player physically tunes to a signal, hears it through real propagation conditions, identifies the source and location, dispatches an expedition to the canonical destination, resolves the rescue or trap through established combat/expedition authorities, and receives the authored rewards and reputation **strictly once**.

---

## 2. Monotonic State Machine

```
[ None ]
   │
   │  Tuned on radio receiver
   ▼
[ Heard ]
   │
   │  Message fragments decoded / Direction-Finding confirmed
   ▼
[ Identified ]
   │
   │  Expedition sortie launched to canonical destination
   ▼
[ Dispatched ]
   │
   │  Expedition arrives at target coordinates
   ▼
[ Reached ] ─── If (currentDay > ExpiryDay) ──► [ TerminalFailed ]
   │
   ├─► If (IsTrap == true)  ──► [ TerminalAmbush ] ──► [ TerminalSurvived ]
   │
   └─► If (IsTrap == false) ──► [ TerminalRescued ]
```

### Stage Definitions

1. **`None` (0):** Unheard airwaves. Signal exists in catalog but tuner has not locked within tolerance.
2. **`Heard` (1):** Frequency intercepted on the tuner. Initial radio static replaced by carrier audio. Trace countdown begins (`InterceptedDay` recorded).
3. **`Identified` (2):** Sufficient message fragments decoded (or DF bearing recorded). Sender identity authenticated and canonical expedition destination resolved.
4. **`Dispatched` (3):** Expedition party assigned and launched from the shelter toward `DestinationId`. `ExpeditionId` correlated.
5. **`Reached` (4):** Party physically arrives at the target coordinates. Evaluated against `ExpiryDay = InterceptedDay + DeadlineDays`.
6. **`TerminalRescued` (5):** Genuine rescue succeeded in time. Survivor secured; authored item rewards and faction reputation eligible for claim.
7. **`TerminalFailed` (6):** Expiry day lapsed before expedition reached the site, or player failed to dispatch in time. Broadcast went silent; survivor succumbed.
8. **`TerminalAmbush` (7):** Signal was deceptive bait (`IsTrap == true`). Arrival triggers combat encounter.
9. **`TerminalSurvived` (8):** Ambush defeated or survived. Combat rewards secured; site cleared.

---

## 3. Idempotent Rewards & Consequence Ledger

To protect against duplicate claims on UI reopen, re-save, or double-interaction:

- Every claim is recorded in `_claimedReceipts` using the unique composite key:
  `$"{questId}:{signalId}"`
- `ClaimIdempotentRewards(questId)` verifies that `_claimedReceipts` does not already contain the key and `RewardClaimed` is false.
- Calling `ClaimIdempotentRewards` a second time returns empty rewards `([], 0)` without mutating shelter inventory or faction ledgers.

---

## 4. The Five Authoritative Flagship Missions

| Quest ID | Signal ID | Frequency (MHz) | Band | Destination ID | Trace (Days) | Deadline (Days) | Is Trap | Faction | Rewards |
|---|---|---|---|---|---|---|---|---|---|
| `quest_distress_trapped_mechanic` | `freq_distress_88_3` | 88.3 | `vhf_low` | `loc_recovery_yard` | 3 | 5 | False | `neutral` | `scrap_metal`, `mechanical_parts` (+5 Rep) |
| `quest_distress_injured_trader` | `freq_distress_156_8` | 156.8 | `vhf_high` | `rural_gas_station` | 2 | 3 | False | `neutral` | `bandage`, `battery`, `iodine_pills` (+3 Rep) |
| `quest_distress_family_shelter` | `freq_distress_445_2` | 445.2 | `uhf_low` | `family_bunker_backyard_shed` | 2 | 4 | False | `neutral` | `clean_water`, `canned_food` (+8 Rep) |
| `quest_distress_raider_trap` | `freq_distress_192_4` | 192.4 | `vhf_high` | `loc_denial_cut_substation` | 3 | 5 | True | `raiders` | Combat encounter (Ambush) |
| `quest_distress_military_patrol` | `freq_distress_901_2` | 901.2 | `uhf_high` | `checkpoint_kilo_armory` | 3 | 5 | False | `military` | `ammo_556`, `field_dressing_kit` (+10 Rep) |
