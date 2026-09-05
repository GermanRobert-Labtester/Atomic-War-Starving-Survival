# Plan 92 — Faction War Dialogue Baseline & Forensics

> **Catalog Authority:** `Assets/StreamingAssets/Data/faction_war_dialogue.json`
> **Core Loader & Indexer:** `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs`
> **Narrative Layer Authority:** `docs/narrative/NARRATIVE_NEEDS.md` (Faction War Arc, Days 480–605+)

---

## 1. Verified Baseline Reconnaissance

### 1.1 Catalog State
Prior to Plan 92, `faction_war_dialogue.json` contained **18 verified dialogue snippets** (`schema_version: 1`).
Each snippet record has the shape:
- `id`: string (`dlg_d<day>_<semantic_descriptor>`)
- `locationId`: string (references `loc_*` canonical identities)
- `minDay`: integer (day threshold for availability)
- `speakerTag`: string (compact 1-sentence role/action description)
- `body`: string (in-media-res quoted dialogue)

### 1.2 Core Loader & Query Contract
`FactionWarContentCatalogLoader` loads `faction_war_dialogue.json` into `FactionWarDialogueRoot`:
```csharp
[Serializable]
public sealed class FactionWarDialogueSnippet
{
    public string id = string.Empty;
    public string locationId = string.Empty;
    public int minDay;
    public string speakerTag = string.Empty;
    public string body = string.Empty;
}
```

The selector function in `FactionWarContentCatalog` is:
```csharp
public List<FactionWarDialogueSnippet> GetDialogueForLocation(string locationId, int day)
{
    var result = new List<FactionWarDialogueSnippet>();
    for (int i = 0; i < _dialogueSnippets.Count; i++)
    {
        var s = _dialogueSnippets[i];
        if (s != null && s.minDay <= day &&
            string.Equals(s.locationId, locationId, StringComparison.Ordinal))
            result.Add(s);
    }
    return result;
}
```

### 1.3 Selector Semantics
1. **Filtering:** Strict location matching (`StringComparison.Ordinal`) and day threshold (`s.minDay <= day`).
2. **State:** Completely stateless in Core. No seen-state tracking, no cooldown timers, no mutation of faction standing.
3. **Ordering:** Preserves deserialization order in the list.
4. **Empty Pool:** Returns an empty `List<FactionWarDialogueSnippet>` safely when no snippets match location or day.
5. **Day 0 / Negative Day:** Gracefully handled by standard integer comparison (`minDay <= day`).

---

## 2. Campaign Horizon Evidence

- The planning brief referenced a generic "300+ day campaign."
- Repository forensics across `NARRATIVE_NEEDS.md`, `faction_war_events.json`, `faction_war_radio.json`, `faction_war_journal.json`, and `faction_war_communiques.json` confirm that the **Faction War narrative arc specifically spans Days 480 to 605+** (Year of Ash, Year 2 of the campaign).
- All 18 existing baseline snippets were authored with `minDay` in the range **482 to 591**.
- Plan 92 aligns all 22 new snippets to this exact operational horizon (Days 485–576).

---

## 3. Original 18 Baseline Snippets Roster

| # | Snippet ID | Location ID | minDay | Context | Speaker Tag | Lines |
|---|---|---|---|---|---|---|
| 1 | `dlg_d482_checkpoint_quartermasters` | `loc_garrison_checkpoint_gamma` | 482 | Garrison | Two Garrison quartermasters, reconciling manifests | 4 |
| 2 | `dlg_d483_exchange_lean_pool` | `loc_grain_silo` | 483 | Exchange | Two traders under the leaning silo | 5 |
| 3 | `dlg_d488_understory_relay_move` | `loc_understory_transmitter` | 488 | Understory | Two Understory relay hands, striking the mast for the move | 6 |
| 4 | `dlg_d490_switchback_pilgrims` | `loc_ash_sign_shrine` | 490 | Civilian/Pilgrim | Two pilgrims on the switchback trail | 5 |
| 5 | `dlg_d493_weighbridge_toll_grumble` | `loc_weighbridge` | 493 | Independent/Toll | Two travelers waiting at the toll booth | 4 |
| 6 | `dlg_d497_scavengers_clean_crater` | `loc_railway_span_44_alpha` | 497 | Scavenger | Two scavengers picking through the crater | 4 |
| 7 | `dlg_d505_conscription_office_clerks` | `loc_conscription_office` | 505 | Garrison | Two clerks covering the empty counter | 4 |
| 8 | `dlg_d512_weighbridge_reroute` | `loc_weighbridge` | 512 | Exchange/Toll | Two Weighbridge booth staff, watching an empty road | 6 |
| 9 | `dlg_d526_exchange_roster_kid` | `loc_grain_silo` | 526 | Exchange/Roster | A young roster member and an older trader | 4 |
| 10 | `dlg_d538_checkpoint_awkward_small_talk` | `loc_grain_silo` | 538 | Garrison/Exchange | A Garrison inspector and an Exchange trader, at the new post | 6 |
| 11 | `dlg_d552_deserter_hunters` | `loc_garrison_checkpoint_gamma` | 552 | Garrison | Two Garrison patrolmen off duty | 3 |
| 12 | `dlg_d549_children_after_the_plaza` | `loc_ration_queue_plaza` | 549 | Civilian | Two children playing near the plaza's edge | 5 |
| 13 | `dlg_d580_shrine_keepers_doubt` | `loc_ash_sign_shrine` | 580 | Shrine/Faith | Two shrine-keepers, after the anomaly | 4 |
| 14 | `dlg_d568_toll_syndicate_cynicism` | `loc_weighbridge` | 568 | Exchange/Toll | Two Weighbridge booth staff | 4 |
| 15 | `dlg_d571_forward_roster_checkpoint` | `loc_forward_roster_camp` | 571 | Forward Roster | Two Forward Roster checkpoint watch | 4 |
| 16 | `dlg_d573_forward_roster_identity` | `loc_forward_roster_camp` | 573 | Forward Roster | Sella Krenn and a Forward Roster veteran, off the toll line | 6 |
| 17 | `dlg_d584_d9_cell_debate` | `loc_d9_cache_bunker_delta` | 584 | D/9 Cell | Two D/9 cell operators, over the manifest | 6 |
| 18 | `dlg_d591_switchback_waystation_doubt` | `loc_shrine_switchback_waystation` | 591 | Civilian/Pilgrim | Two pilgrims resting at the switchback waystation | 6 |

All 18 original entries remain byte-preserved with unmodified IDs, locations, and bodies.
