# ASHFALL — CAMPAIGN ENDGAME & EPILOGUE SPECIFICATION (PLAN 84 / TASK B25)

**Classification:** Campaign Closure, Endings & Chronicle Authority
**Author:** AI Pair Programmer / Antigravity
**Status:** Implemented & CI-Gated
**Data Authority:** `Assets/StreamingAssets/Data/endings.json`
**Core System:** `Assets/Ashfall.Core/Endgame/EndgameSystem.cs`
**Host Session:** `src/Host/EndgameHostSession.cs`
**Enforcement Tests:** `Ashfall.Core.Tests/Endgame/EndgameSystemTests.cs`
**Host Gate:** `--endgame-v1-selftest`

---

## 1. Executive Mission & Background

ASHFALL campaigns represent harrowing struggles across harsh nuclear winter, radioactive fallout, psychological collapse, and faction hostility. Until Plan 84, endings were fragmented across individual sub-systems (e.g. Holdfast sprint endings, Muster military outcomes, Verdict tribunal verdicts).

Plan 84 unifies all campaign closure into an authoritative, immutable **Endgame System** (`EndgameSystem.cs`). When the campaign horizon is reached (Day 360 Year of Ash milestone) or when a catastrophic or triumphant victory condition is triggered, the system evaluates all historical campaign data to select an earned ending and generates a personalized multi-paragraph epilogue chronicle.

---

## 2. Endgame State Machine & One-Way Sealing

```
       [ Active Campaign ]
               │
               ▼ (Day >= 360 OR Decisive Trigger Condition Met)
     [ Triggered / Evaluating ]
               │
               ▼ (Evaluates Roster, Factions, Morale, Verdict, Muster)
        [ Epilogue Roll ]
               │
               ▼ (Player Reviews Chronicle in ChroniclePanel)
        [ Campaign Sealed ] ──► (Save Envelope Frozen, Read-Only Chronicle)
```

### 2.1 State Rules
1. **One-Way Sealing**: Once transitioned to `CampaignSealed`, simulation ticks halt. The save file retains the final `EndingRecord`, preventing overwriting or accidental state corruption.
2. **Context-Sensitive Epilogue Roll**: The epilogue does not merely display a static string. It compiles:
   - **Main Outcome**: The geopolitical and ecological fate of the bunker and region.
   - **Survivor Memorial Ledger**: Specific paragraphs commemorating fallen dwellers and honoring living veterans.
   - **Faction Legacy**: How the regional factions (Flotilla, Garrison, Warlords, Scavengers) remember the bunker's governance.
   - **Moral Verdict**: Summary of community trust and humanitarian vs. authoritarian choices.

---

## 3. Authored Endings (Data Authority: `endings.json`)

The canonical catalog defines 8 definitive endings across multiple philosophical and survival axes:

1. `ending_dawn_of_thaw`: **The Spring of Year Two (Triumphant Survival)** — Bunker survives 360 days with high population, operational greenhouse, and stable power grid. The nuclear clouds thin and first spring thaw begins.
2. `ending_iron_hegemony`: **The Iron Bastion (Authoritarian Hegemony)** — Militaristic domination with Garrison/Muster alliance, crushing all outside opposition at the cost of civil liberties.
3. `ending_exodus_to_sea`: **The Flotilla Fleet (Maritime Exodus)** — Abandoning the frozen terrestrial wasteland to board the restored naval fleet with the Black Flotilla toward equatorial waters.
4. `ending_silent_tombs`: **The Silent Vault (Bunker Extinction)** — Complete dweller mortality or total life-support collapse. The blast doors remain sealed forever.
5. `ending_the_reckoning`: **The Truth of the Strike (Verdict Revelation)** — The tribunal uncovers and broadcasts the unredacted truth of the pre-war missile strike, permanently altering regional ideology.
6. `ending_wasteland_sanctuary`: **The Open Haven (Humanitarian Sanctuary)** — Welcoming wasteland refugees, sharing medical supplies, and forming a thriving post-war community council.
7. `ending_frozen_silence`: **The Deep Freeze (Hypothermic Oblivion)** — Thermal insulation failure during peak Year of Ash blizzard reduces the bunker to an icy grave.
8. `ending_warlord_tribute`: **The Subjugated Hold (Vassalage)** — Survival achieved through permanent vassalage and tribute payment to the regional Warlord coalition.

---

## 4. UI & Chronicle Presentation

- **Chronicle Panel** (`src/UI/ChroniclePanel.cs`): A grand presentation screen rendering:
  - Ending title, badge, and tone color.
  - Sourced epilogue prose segments.
  - Campaign metrics: Days survived, survivors living vs deceased, total expeditions completed, morale average.
  - "SEAL CAMPAIGN" button committing the immutable ending record.
