# DAILY BRIEFING TAXONOMY & CADENCE MODEL
## Classification, Severity Tiers, Cadence Filtering, and Navigation Routes

**Document Version:** 1.0.0
**Scope:** Plan 75 (Daily Briefing & Dawn Intelligence Report)
**Status:** Canonical Briefing Taxonomy Specification

---

## 1. Taxonomy Classes

Every dawn report fact belongs to one of four strict taxonomy classes:

| Class | Ordinal | Severity Range | Description | Cadence Suppression Policy |
|---|---|---|---|---|
| `CRITICAL` | `0` | `80–100` | Immediate existential threats (starvation, lethal radiation, power failure). | Never silenced indefinitely; re-notifies every 3 days if unresolved. |
| `WARNING` | `1` | `40–79` | Deteriorating conditions (low rations, equipment wear, approaching storms). | Cadence filtered: silenced if unchanged. Re-reports only on severity change or resolution. |
| `INTEL` | `2` | `10–39` | Strategic opportunities (scouted ruins, radio decodes, faction movements). | One-time alert; never repeats automatically. |
| `FLAVOR` | `3` | `0–9` | Diegetic atmosphere (survivor morale remarks, bunker acoustic observations). | Cap 1–2 per day, non-urgent. |

---

## 2. Deep-Link Navigation Routes

Every tactical briefing entry exposes an optional deep-link route string that the UI uses to immediately navigate to the relevant problem or station:

- `panel:power_grid` → Focuses Electrical Generation / Battery Bank
- `panel:water_treatment` → Focuses Water Filtration / Desalination Sump
- `panel:medical` → Opens Infirmary / Quarantine Triage
- `panel:muster` → Opens Faction Warfare / Rally Board
- `panel:wasteland_map` → Centers World Map on target POI node (`panel:wasteland_map?node=loc_silo`)
- `panel:research` → Opens R&D Tech Tree
- `panel:subterranean` → Opens Metro Transit / Trench map

---

## 3. Cadence Filtering Mathematical Contract

A candidate fact $F$ with id $F_{id}$, class $C$, and severity $S_t$ on day $t$:

1. **New Fact ($F_{id} \notin \text{History}$):** Emit. Record $(S_t, t)$.
2. **Critical Repeat ($C == \text{CRITICAL}$):**
   $$\text{Emit if } t - t_{\text{last}} \ge 3 \lor |S_t - S_{\text{last}}| \ge 10$$
3. **Warning Change ($C == \text{WARNING}$):**
   $$\text{Emit if } |S_t - S_{\text{last}}| \ge 15 \lor (S_{\text{last}} > 0 \land S_t == 0 \text{ [Resolved]})$$
   $$\text{Suppress if unchanged: } |S_t - S_{\text{last}}| < 15$$
4. **Intel / Flavor ($C \in \{\text{INTEL}, \text{FLAVOR}\}$):**
   $$\text{Suppress if } t_{\text{last}} == t \text{ (deduplicate within dawn batch)}$$
