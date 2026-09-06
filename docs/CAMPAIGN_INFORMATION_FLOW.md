# CAMPAIGN INFORMATION FLOW
## Dawn Execution Order & Cross-Domain Strategic Information Pipelines

**Document Version:** 1.0.0
**Domain:** Cross-System Campaign Intelligence Spine
**Status:** Canonical Standard for Plans 72–75

---

## 1. Dawn Execution Order (The 6-Step Morning Sequence)

To guarantee that the Daily Briefing, Codex, Map, and Faction layers never observe half-updated simulation state or double-report transitions, every campaign day follows this strict chronological order:

```text
┌─────────────────────────────────────────────────────────────┐
│ 1. Overnight Phase Transitions                              │
│    • Water/power consumption, life-support tick             │
│    • Illness/wound progression, medical triage checks        │
│    • Weather tick, storm fronts, environmental shifts       │
│    • Expedition movement / combat steps                     │
│    • Faction tension, patrol clashes, muster recovery       │
└──────────────────────────────┬──────────────────────────────┘
                               │
                               ▼
┌─────────────────────────────────────────────────────────────┐
│ 2. System Finalization & State Invariants                   │
│    • Casualty confirmation (deaths routed to memorial)      │
│    • Resource pool normalization (no negative inventory)     │
│    • Tech completion flag set, blueprint unlocks            │
│    • Map discovery nodes updated (visited / surveyed)       │
└──────────────────────────────┬──────────────────────────────┘
                               │
                               ▼
┌─────────────────────────────────────────────────────────────┐
│ 3. Briefing Fact Collection                                 │
│    • Typed collectors invoke CollectBriefingFacts(day)      │
│    • Isolated read-only query; zero state mutation          │
│    • Facts tagged with stable FactId, taxonomy, severity    │
└──────────────────────────────┬──────────────────────────────┘
                               │
                               ▼
┌─────────────────────────────────────────────────────────────┐
│ 4. Briefing Assembly & Cadence Filtering                    │
│    • Cadence comparison against persisted CadenceState      │
│    • Suppression of unchanged persistent warnings           │
│    • Deduplication by stable FactId                         │
│    • Deterministic sort (CRITICAL > WARNING > INTEL > FLAVOR│
│      then severity desc, category ordinal, entity ID)       │
└──────────────────────────────┬──────────────────────────────┘
                               │
                               ▼
┌─────────────────────────────────────────────────────────────┐
│ 5. Dawn Presentation & Modal Display                        │
│    • DailyBriefingModal displays derived report             │
│    • Deep-links routed through PanelRegistry                │
│    • Player reviews situation and acknowledges report       │
└──────────────────────────────┬──────────────────────────────┘
                               │
                               ▼
┌─────────────────────────────────────────────────────────────┐
│ 6. Player Action Phase                                      │
│    • Shelter work orders, crafting, research queuing        │
│    • Expedition dispatch, waystation maintenance            │
│    • Muster mobilization, force commitment                  │
└─────────────────────────────────────────────────────────────┘
```

---

## 2. Cross-Domain Data Pipelines

### 2.1 Muster → Briefing
- When a Muster force mobilizes, engages, or suffers casualties, `MusterSystem` generates typed briefing facts:
  - `muster.mobilized`: INTEL / WARNING (depending on threat).
  - `muster.engagement_won` / `muster.engagement_lost`: INTEL or CRITICAL.
  - `muster.heavy_casualties`: CRITICAL alert deep-linking to the Memorial panel.
- Supply shortages preventing mobilization emit a WARNING deep-linking to Inventory / Muster.

### 2.2 Muster → Wasteland Map
- Active military rally or contested sector operations emit map intelligence:
  - Danger ratings of neighboring route edges increase.
  - Temporary contested zone markers are established via the Map authority.
  - Node status reflects active hostilities without leaking non-discovered ground truth.

### 2.3 Wasteland Map → Codex
- Node discovery does not automatically unlock Codex encyclopedia entries.
- Instead, visiting a node uncovers specific physical evidence, flora, or fauna that then register in the Field Guide / Journal, unlocking corresponding Codex entries.
- Map links inside the Codex are strictly navigational deep-links, not fog-of-war reveals.

### 2.4 Wasteland Map → Briefing
- When a radio broadcast is decrypted into a rumored location, or an expedition surveys a new sector:
  - An INTEL fact is emitted: `map.new_rumor` or `map.survey_complete`.
  - The briefing links directly to `WastelandMapView` focused on the target node.
  - Cadence ensures the discovery is reported exactly once on the dawn following revelation.

### 2.5 Codex → Map
- Viewing a Codex entry with a known habitat or location allows the player to inspect known map locations matching that entity.
- If a location is currently `Unknown`, the Codex displays "Location Uncharted in Settlement Cartography" rather than exposing coordinates.

### 2.6 Research / Field Guide → Briefing
- Technology breakthroughs emit an INTEL fact: `research.completed:{tech_id}`.
- New species observations emit: `field_guide.entry_unlocked:{entry_id}`.
- Deep-links route the player directly to the Research Atlas or Codex Atlas.
