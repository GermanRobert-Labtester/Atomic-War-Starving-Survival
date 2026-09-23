# ASHFALL — WAVE 4 INTEGRATION PROGRAM · PLAN 3 OF 6

# SHELTER INFRASTRUCTURE INTEGRATION PLAN

**Status:** PROPOSAL — planning-only · no production path claimed
**Wave:** W4 (six-plan integration wave — the shelter's remaining machinery)
**Document:** W4-03
**Date:** 2026-09-21
**Repo:** `Atomic War` @ `Zcode_Branch`, HEAD `5be1a30a`
**Companion plans:** W4-01 (save/state), W4-02 (world/travel), W4-04 (ecology), W4-05 (society), W4-06 (medicine)
**Plan-unblocking annex:** Annex U at the end — separately.

---

## 0. How to read this plan

This plan integrates **the shelter's machinery**: power generation and distribution,
water sourcing and treatment, drainage, ventilation and filtration, thermal envelope,
fire safety, machinery condition, and the failure cascades that connect them. It
extends existing owners — one grid truth, one water truth, one air truth, one thermal
truth — and it never builds a second power model, meter, or reservoir.

### 0.1 Two selection levels

| Level | Choice | Granularity |
|---|---|---|
| **Level 1** | Plan Path **A**, **B**, or **C** | the whole plan's posture |
| **Level 2** | ten decision points, each **A/B/C** | per-concern depth |

### 0.2 Default mapping

| Plan Path | A points | B points | C points |
|---|---|---|---|
| A Truth & Safety | 1–10 | — | — |
| B One Grid, One Flow | 1 | 2,3,4,5,6,7 | 8,9,10 |
| C Resilient Infrastructure | — | 2,4,9 | 1,3,5,6,7,8,10 |

### 0.3 The Wave 4 rule for this plan

> **One meter per resource, one owner per subsystem, one warning before every
> failure.** Power is one grid authority with explicit priority tiers; water is one
> quality/source authority; air and thermal read their own systems. No parallel
> generator model, no private water counters, no UI-computed capacity.

### 0.4 Vocabulary

| Term | Meaning |
|---|---|
| subsystem | one infrastructure concern (power, water, drainage, air, thermal, fire) |
| priority tier | the declared order in which load is shed under scarcity |
| headroom | capacity minus current draw, per subsystem |
| failure cascade | a chain where one failure degrades others (flood → power → heat) |
| warning window | the time between a detectable condition and its harmful outcome |
| hardening | a persistent upgrade that raises resilience on an existing route |
| condition | wear state of machinery read by maintenance and repair paths |
| truth surface | a UI element that renders an owner read, never a computed guess |

---

## 1. Premise audit — what P0 must verify (Rule 7)

```text
[ ] Assets/Ashfall.Core/ water and drainage:
    WaterTreatmentSystem, WaterRequest, WaterborneExposureRules,
    BrineWaterSystem (+ headless demo), DeepWellSystem,
    AtmosphericCondenserSystem, SumpFloodingSystem + SumpDrainageCatalog
[ ] Assets/Ashfall.Core/ air and thermal:
    VentilationSystem, ShelterThermalSystem, ShelterMachineryReport,
    ElectrostaticFiltrationCatalog, DecontaminationSystem + DeconProtocolCatalogLoader
[ ] power: SOFC host port (SofcPowerHostSession) + Plan 122 fuel consumption,
    PowerLoadSheddingEngine (Expansion 21), grid catalog seal
    (SHELTER_GRID_CATALOG_SEAL plan), generator systems
[ ] fire and crisis: ShelterFireHazardSystem (Incidents unresolved/unsuppressed),
    CrisisPresentationCoordinator, EmergencyMusterReadinessEngine (Expansion 23),
    shelter hardening data (weather_hardening_upgrades.json)
[ ] host: src/Main.ShelterInfrastructure.cs, Main.WaterCondenser.cs,
    Main.AdvancedShelterSystems.cs, Main.BriefingCrisis.cs
[ ] existing rulings: SHELTER_EMP_MEDICAL_POWER, SHELTER_FAILURE_EFFECTS_QUARANTINE_WIRING,
    SHELTER_GRID_CATALOG_SEAL, DEBT-194-CRISIS-PRODUCER-WIRE (sealed 2026-09-12)
[ ] data: power/water/upgrade catalogs under Assets/StreamingAssets/Data/
```

### 1.1 Evidence posture

- Proposal only; no path claims; no ledger rows; read-only until Annex U.
- Core engine-free; JSON authoritative; determinism through existing seeded paths.
- One authority per concern; additive fields only; no parallel simulation.
- Focused verification per `TEST_POLICY.md`; the sealed crisis-producer row stays sealed.

### 1.2 Known anchors (verify, don't trust)

| Anchor | Why it matters |
|---|---|
| `SofcPowerHostSession` + Plan 122 | fuel consumption already flows through a live path — do not duplicate |
| `PowerLoadSheddingEngine` | shedding tiers already exist (Expansion 21) |
| `WaterQualityProfileEngine` | quality profiles already exist (Expansion 22) |
| `DEBT-194-CRISIS-PRODUCER-WIRE` | crisis producers already read fire/flood/radiation/fate — closed |
| `ShelterFireHazardSystem.Incidents` | unresolved/unsuppressed fire state is the warning source |
| `sump` flood state | `SumpFloodingSystem.State.nodes[].isFlooded` is the flood truth |

---

## 2. The three Plan Paths

### 2.1 Path A — Truth & Safety

Audit each subsystem: what it owns, what data it consumes, what surfaces show it, and
whether warnings exist before harm. Produce the infrastructure ledger and a ranked
repair list; stop at safety-critical asymmetries until fixed.

### 2.2 Path B — One Grid, One Flow

Unify: one power authority with priority tiers, one water authority with sources and
quality, one air/thermal truth, condition routed through maintenance. Remove private
counters and UI math; make every surface a read.

### 2.3 Path C — Resilient Infrastructure

Make failure survivable: warning windows, cascades with off-ramps, hardening upgrades,
redundancy through existing routes, and preparedness drills that consume real state —
deterministic and save-backed via W4-01.

---

## 3. The ten decision points

### 3.1 Point 1 — Subsystem inventory and ownership

**Owner anchor:** `Main.ShelterInfrastructure.cs` + each subsystem system.
**Decision:** a complete ledger: subsystem → owner → save section → surfaces →
consumers; one owner per concern; no duplicate meters.

- **A:** enumerate and reconcile; find private counters, dual bookkeeping, orphan
  catalogs, and UI-computed capacities.
- **B:** registration: new infrastructure joins the ledger, save, surfaces, and gates
  in one package.
- **C:** ownership inheritance for expansions (Expansion 27–31 materials and stations
  attach to existing subsystems, never fork them).

**Verify:** ledger consistency test; duplicate-meter scan.
**Never:** two systems tracking the same resource, or a panel computing capacity.

### 3.2 Point 2 — Power generation and distribution

**Owner anchor:** SOFC session, grid catalog, generator systems, `Plan 122` fuel path.
**Decision:** one grid truth: generation, storage, distribution, and fuel consumption
under one authority with explicit capacity and headroom.

- **A:** audit generation/storage/fuel paths and data; find dead generators, unused
  catalogs, duplicate fuel draws.
- **B:** one grid model consumed by every consumer; fuel through the 122 path; no
  second reserve.
- **C:** expansion arcs: new generators/hardening attach to the grid; degradation over
  years is deterministic.

**Verify:** grid round-trip; fuel draw once-per-tick; capacity/headroom consistency.
**Never:** a second fuel ledger, a reserve that appears from nowhere, or two power
truths.

### 3.3 Point 3 — Load shedding and brownout

**Owner anchor:** `PowerLoadSheddingEngine`, priority tiers Tier0–Tier4.
**Decision:** scarcity sheds deliberately: strict tier order, brownout risk, cascading
breaker behavior, and player-facing warnings before damage.

- **A:** audit tier assignments against actual consumers; find life-support loads that
  can silently shed.
- **B:** one shedding evaluation; tiers authored; every shed is visible and explained.
- **C:** energy-poverty arcs: sustained scarcity affects morale and health through
  existing owners (W3-03/W4-06), with recovery paths.

**Verify:** tier-order assertion; brownout scenario test; warning-before-harm check.
**Never:** silent life-support loss, or morale damage without a warned cause.

### 3.4 Point 4 — Water sources, quality, and treatment

**Owner anchor:** `DeepWellSystem`, `AtmosphericCondenserSystem`, `BrineWaterSystem`,
`WaterTreatmentSystem`, `WaterQualityProfileEngine`, `WaterRequest`.
**Decision:** one water truth: sources produce, treatment improves, quality tiers
gate consumption, and requests route through the existing request contract.

- **A:** source/treatment/quality audit; find duplicate counters and unmodeled sources.
- **B:** one quality profile per source; requests through `WaterRequest`; intake and
  treatment consume power/parts through existing owners.
- **C:** water sustainability arcs: aquifer drawdown, brine handling, condensation at
  scale — all deterministic and warned.

**Verify:** source yield determinism; quality profile application; request path test.
**Never:** a private water pool, silent contaminated use, or a second quality model.

### 3.5 Point 5 — Drainage and flooding

**Owner anchor:** `SumpFloodingSystem`, `SumpDrainageCatalog`.
**Decision:** flooding is modeled: nodes flood, pumps drain, power loss stops pumps,
and warnings precede damage; every flooded node has consumers (pathing, damage, UI).

- **A:** audit flood nodes and consumers; find inert flood flags and unmodeled water.
- **B:** one flood state consumed by travel, health, and surfaces; pump dependency on
  power explicit.
- **C:** long-run drainage works: prevention projects on existing crafting/upgrade
  owners; seasonal patterns deterministic.

**Verify:** flood/drain simulation; pump-power dependency test; consumer coverage.
**Never:** flood damage without a warning path, or two flood truths.

### 3.6 Point 6 — Air, ventilation, and filtration

**Owner anchor:** `VentilationSystem`, `ElectrostaticFiltrationCatalog`,
contamination/decon family.
**Decision:** air quality is one truth: ventilation moves it, filtration cleans it,
contamination events warn, and thresholds map to health outcomes via W4-06.

- **A:** audit air quality data/consumers; find unused filters, unmodeled contaminants.
- **B:** one air model; filters consume power/parts; health effects route to the
  medical pipeline, never computed locally.
- **C:** sealed-shelter arcs: long-term exposure management, filter supply chains
  (W3-05), and warning drills.

**Verify:** air quality round-trip; filter consumption; threshold→health routing test.
**Never:** local health math in the ventilation system or silent filter exhaustion.

### 3.7 Point 7 — Thermal envelope and fire safety

**Owner anchor:** `ShelterThermalSystem`, `ShelterFireHazardSystem`, hardening data.
**Decision:** heat and fire are modeled: thermal comfort reads owners, fire hazards
have unresolved/unsuppressed states, and both warn before harm.

- **A:** audit thermal/fire consumers and data; find dead hardening entries.
- **B:** one thermal truth consumed by needs/health; fire incidents through the already
  wired crisis producers; suppression costs real resources.
- **C:** winter arcs and fire-risk seasonality; hardening persistence.

**Verify:** thermal→needs routing; fire incident lifecycle; hardening round-trip.
**Never:** thermal damage computed in UI, or reopening the sealed crisis-producer row.

### 3.8 Point 8 — Machinery condition and maintenance

**Owner anchor:** `ShelterMachineryReport`, condition systems, W3-05 repair owners.
**Decision:** every machine wears, reports honestly, and repairs through the existing
crafting/maintenance path — never self-repair, never invisible decay.

- **A:** condition audit per machine; find machines that never wear or repair for free.
- **B:** one condition record; wear deterministic; repair consumes materials/labor.
- **C:** maintenance culture: schedules, spares, and failure foreknowledge on existing
  duty/labor owners (W4-05).

**Verify:** wear determinism; repair consumption; report truthfulness.
**Never:** a machine repairing itself, or a report that hides condition.

### 3.9 Point 9 — Failure cascades and emergency response

**Owner anchor:** crisis producers + `CrisisPresentationCoordinator` +
`EmergencyMusterReadinessEngine` + warning family.
**Decision:** cascades are authored and off-rampable: flood→power→heat, fire→air,
power→water; each step warned; drills and muster consume real readiness state.

- **A:** cascade inventory: which failures actually connect in code, and which are
  assumed; list ungated chains.
- **B:** one cascade evaluation; off-ramp interventions (a pump, a breaker, a crew)
  that can break the chain; warnings at each link.
- **C:** preparedness arcs: drills, stockpiles, and hardening reduce cascade severity
  measurably.

**Verify:** cascade scenario tests (≥3 chains); off-ramp intervention test; warning
coverage per link.
**Never:** unwarned cascades, or drills that do not consume real state.

### 3.10 Point 10 — Infrastructure surfaces

**Owner anchor:** infrastructure report/surfaces (W3-06 coordination).
**Decision:** the player sees truth: capacity, draw, headroom, quality, condition, and
warning states — all owner reads with reasons and no fabricated fallback.

- **A:** surface audit: fabricated values, missing units, stale reads, unstated
  failures.
- **B:** registry-backed elements; unavailable states authored; warnings lead to
  actions.
- **C:** history and trends: consumption and condition history for planning, bounded
  and saved via W4-01.

**Verify:** W3-06 HUD truth kit over infrastructure elements; warning-action mapping;
history round-trip.
**Never:** a gauge that computes locally, a zeroed unknown, or a warning with no path
to act.

---

## 4. Selection sheet

```text
ASHFALL WAVE 4 · PLAN W4-03 · SELECTION SHEET

Plan Path:   [ ] A Truth & Safety   [ ] B One Grid, One Flow   [ ] C Resilient Infrastructure

Points (mark A/B/C or leave default):
 1 subsystem inventory ..... [ ]
 2 power grid .............. [ ]
 3 load shedding ........... [ ]
 4 water quality/treatment . [ ]
 5 drainage/flooding ....... [ ]
 6 air/ventilation ......... [ ]
 7 thermal/fire ............ [ ]
 8 machinery condition ..... [ ]
 9 failure cascades ........ [ ]
10 infrastructure surfaces . [ ]

Selected by: ____________   Date: ________   Foreman: ____________
```

---

## 5. Phase ladder

| Phase | Name | Exit |
|---|---|---|
| P0 | Premise audit + subsystem ledger | ledger filed; premises re-verified |
| P1 | Power + shedding discipline | tier order; fuel-once; warning checks green |
| P2 | Water + drainage | source/quality/request + flood simulation green |
| P3 | Air + thermal + fire | routing to health/needs verified; fire lifecycle green |
| P4 | Condition + cascades | determinism; cascade scenarios + off-ramps green |
| P5 | Surfaces + hardening | W3-06 kits; hardening round-trip; history saved |
| P6 | Closeout | evidence pack; determinism; limitations recorded |

---

## 6. Non-goals, never-touch, one-authority

**Non-goals**

- No new resource model; no parallel meters, reserves, or quality scales.
- No balance rewrites; tuning only through authored data with evidence.
- No duplication of health effects (W4-06) or materials (W3-05).
- No reopening of sealed debt rows.

**Never-touch**

- `DEBT-194-CRISIS-PRODUCER-WIRE` (sealed 2026-09-12) and its producer wiring.
- `SHELTER_GRID_CATALOG_SEAL`, `SHELTER_EMP_MEDICAL_POWER`,
  `SHELTER_FAILURE_EFFECTS_QUARANTINE_WIRING` closings.
- Expansion 21/22/23 engines' contracts (`PowerLoadSheddingEngine`,
  `WaterQualityProfileEngine`, `EmergencyMusterReadinessEngine`).
- Quarantined tests; unclaimed shared paths.

**One authority per concern**

| Concern | Owner |
|---|---|
| power | grid authority + `SofcPowerHostSession`/Plan 122 fuel path |
| shedding | `PowerLoadSheddingEngine` |
| water | source systems + `WaterTreatmentSystem` + quality engine |
| drainage | `SumpFloodingSystem` |
| air | `VentilationSystem` + filtration |
| thermal | `ShelterThermalSystem` |
| fire | `ShelterFireHazardSystem` |
| condition | machinery condition records |
| cascades | crisis producers + presentation coordinator |

---

## 7. Verification and acceptance

- **T1 static:** ledger; duplicate-meter scan; consumer inventory; surface-source audit.
- **T2 focused:** grid/fuel; shedding tiers; water source/quality/request; flood sim;
  air routing; thermal→needs; fire lifecycle; wear/repair; cascade scenarios; W3-06
  kits.
- **T3 soak:** 60-day shelter at seeded weather: capacity trends, cascade count,
  repair consumption, zero drift.
- **Acceptance:** evidence pack + Annex U signature; compile-green is not acceptance.

## 8. Handoffs and dependencies

| Direction | Detail |
|---|---|
| W3-03 | morale/health consequences of scarcity, cold, smoke |
| W3-05 | repair materials, filters, parts, hardening crafts |
| W4-01 | every new infrastructure state ships its save section |
| W4-02 | weather hardening, route/environment coupling |
| W4-05 | labor for maintenance, drills, and cascades response |
| W4-06 | health outcomes of air/water/thermal exposure |
| W2-01/02 | generated checks and silent-failure rules in infrastructure paths |

---

# ANNEX U — PLAN-UNBLOCKING (SEPARATELY)

## U.1 What this plan releases

| Release | Unblocks |
|---|---|
| U1 | infrastructure ledger work blocked on "which meter is canonical" |
| U2 | cascade/off-ramp design blocked on producer-wiring confirmation (sealed row respected) |
| U3 | water/flood consumer integration held for quality-profile consumption |
| U4 | hardening and winter-arc work blocked on save-section discipline |
| U5 | infrastructure surfaces blocked on HUD-truth registry (W3-06) |

## U.2 Signature block

```text
ASHFALL WAVE 4 · PLAN W4-03 · RELEASE SIGNATURE
HEAD: ________  Date: ________
[ ] P0 premise audit completed and filed
[ ] subsystem ledger exists; duplicate meters listed
[ ] no path claimed outside the package
[ ] focused test targets named
[ ] rollback position recorded
Signed: ________   Foreman: ________
```

## U.3 Never-touches

- No edit to another Wave 4 plan's claimed paths.
- No re-open of the four shelter closure plans named above.
- No change to Expansion 21/22/23 engine contracts.
- No revival of quarantined tests outside the documented procedure.

## U.4 Release rule

> This plan executes only after U.2 is signed. Until then it is read-only planning.

---

*End of W4-03 — Part I. Expansion parts continue on the established Wave 3 pattern.*---

# W4-03 · PART II — DEEP DESIGN: INVENTORY, POWER, SHEDDING (POINTS 1–3)

## II.1 The subsystem ledger: schema and meaning

The infrastructure ledger is the plan's first deliverable: one row per subsystem,
one owner per row.

```yaml
subsystem_ledger:
  - id: power.grid
    owner: "SOFC session + grid authority + Plan 122 fuel path"
    state: ["generation", "storage", "distribution", "fuel_cursor"]
    consumers: ["all powered machines", "shedding engine", "surfaces"]
    save_section: "power.grid"
    surfaces: ["power report", "shelter HUD"]
    warnings: ["low_fuel", "overdraw", "breaker_trip"]
    tests: ["GridRoundTripTests", "FuelOnceTests"]
  - id: water.sources
    owner: "DeepWell + AtmosphericCondenser + Brine + Treatment"
    ...
```

### II.1.1 Ledger rules

```text
L1  one owner per subsystem; no shared mutable counters
L2  every resource has one meter, readable by consumers, written by the owner
L3  every subsystem lists its warnings and their trigger conditions
L4  every save section names its budget (W4-01 rules)
L5  every consumer is listed; orphan consumers are defects
L6  every surface is listed; surfaces read, never compute
```

### II.1.2 The duplicate-meter gate

Static check: each resource name (power, water, fuel, air, heat) may appear in
exactly one authoritative store. Any second counter is a stop-the-line finding.

## II.2 Power: the grid contract

One grid truth: generation, storage, distribution, fuel.

### II.2.1 Generation sources

| Source | Output | Consumes | Notes |
|---|---|---|---|
| SOFC | steady | fuel (Plan 122 path) | canonical fuel consumption |
| generator | burst | fuel | noise/heat side effects |
| solar/skylight | day-dependent | — | weather-modulated |
| battery | discharge | charge state | stores, never generates |

### II.2.2 Rules

```text
P1  one fuel ledger; consumption once per tick (Plan 122 path)
P2  capacity = sum of sources; headroom = capacity - draw; both derived
P3  storage has charge limits and loss-free accounting (authored efficiency)
P4  distribution has priority tiers (see shedding)
P5  breaker states are facts; trips are events with causes
P6  no source silently appears; no reserve from nowhere
```

### II.2.3 The reconciliation test

Sum generation − consumption − storage delta = 0 over any window. A nonzero
residual is a defect (usually double-application or a private counter).

## II.3 Load shedding: the priority contract

`PowerLoadSheddingEngine` (Expansion 21) owns the tiers.

### II.3.1 Tier table (authored; verified at P0)

| Tier | Loads | Shed policy |
|---|---|---|
| Tier0 | life support (air, warmth minimum) | never shed without warning + emergency |
| Tier1 | medicine, water treatment | shed last; warns at headroom threshold |
| Tier2 | production, kitchen | shed mid-arc; recover when headroom returns |
| Tier3 | comfort, leisure | shed first |
| Tier4 | discretionary, luxury | always first; cheap to restore |

### II.3.2 Rules

```text
S1  shedding is strict tier order; no skipping, no preference
S2  brownout risk derives from sustained overdraw; warned
S3  cascading breaker trips have authored chains (not infinite)
S4  every shed is visible with a reason; no silent life-support loss
S5  recovery is staged (tiers restore in order, with hysteresis)
S6  morale/health consequences route through W3-03/W4-06 owners
```

### II.3.3 The energy-poverty arc

Sustained scarcity produces authored consequences through existing owners:
cold (thermal), hunger (kitchen), sickness (medicine), grievance (morale) —
with recovery paths and no unrecoverable spiral without warning.

## II.4 Worked example: the double fuel draw

**Report:** fuel disappeared twice as fast as the report predicted.

**Walk:**

```text
1. measure: two consumers both drew from Plan 122 path per tick
2. root: a legacy generator wrapper duplicated the consumption call
3. repair: single draw point; generators register as loads; reconciliation test
4. verify: fuel-once test; 24h reconciliation zero
```

**Findings:**

| ID | Class | Repair |
|---|---|---|
| IN-01 | double fuel draw | duplication |
| IN-02 | no reconciliation test | coverage |

*End of Part II. Continues in Part III (water, drainage, air).*---

# W4-03 · PART III — DEEP DESIGN: WATER, DRAINAGE, AIR (POINTS 4–6)

## III.1 Water: sources, quality, treatment

### III.1.1 Source record

```jsonc
{
  "id": "src_well_1",
  "kind": "deep_well",           // deep_well | condenser | brine | sump_reclaim
  "yield_per_day": 180,
  "quality": "brackish",         // authored profile reference
  "drawdown_rate": 0.4,          // aquifer effect per day at full draw
  "power": "Tier1",
  "parts": ["filter_cartridge"]
}
```

### III.1.2 Quality profile

```text
tiers: potable | acceptable | brackish | contaminated | toxic
effects: consumption outcomes via W4-06 (never computed here)
treatment: raises tier per stage, consumes power + parts
```

### III.1.3 Rules

```text
W1  one water meter (stored + flow); sources write, consumers request
W2  requests route through WaterRequest; no direct pool access
W3  quality per source; blending is authored, never averaged by accident
W4  treatment stages consume power (Tier1) and parts; filters wear
W5  contamination events are warned before consumption
W6  aquifer drawdown is deterministic; recharge authored
```

### III.1.4 The request path

```text
consumer -> WaterRequest(amount, min_quality) -> owner resolves:
  from best-quality stock that satisfies min_quality, else refuse with reason
refusals carry copy refs; silent substitution is a defect
```

## III.2 Drainage and flooding

`SumpFloodingSystem` + `SumpDrainageCatalog`.

### III.2.1 Node state

```jsonc
{ "node": "sump_a", "isFlooded": true, "depth": 30, "pump": "pump_1", "since": 214 }
```

### III.2.2 Rules

```text
F1  flood state is per node; consumers: pathing, damage, surfaces, health
F2  pumps require power (named tier); power loss stalls drainage
F3  water level rises deterministically from inflow − pump throughput
F4  warnings precede damage; depth thresholds authored
F5  every flooded node has at least one consumer; inert floods are defects
F6  prevention projects use existing craft/upgrade paths (W3-05)
```

### III.2.3 The cascade hook

Flood → power: water reaches power nodes at authored depths; breakers trip;
then heat/air degrade. This chain is one of the plan's named cascades and
must be off-rampable (pump restored, breaker reset, crew dispatched).

## III.3 Air: ventilation and filtration

`VentilationSystem`, `ElectrostaticFiltrationCatalog`, decon family.

### III.3.1 Air state

```text
per-space: contaminant level (bounded), airflow quality, filter state
sources: fire (ShelterFireHazardSystem), industrial processes, outside ingress
consumers: health thresholds (W4-06), surfaces, fire spread
```

### III.3.2 Rules

```text
A1  one air model; ventilation moves, filtration cleans
A2  filters consume power and wear; replacement via W3-05
A3  contaminant thresholds map to health outcomes through W4-06 only
A4  fire produces contaminant at authored rates; suppression reduces it
A5  sealed-shelter ingress is authored (airlock state, cracks)
A6  warnings precede threshold crossings; no silent poisoning
```

### III.3.3 The threshold routing test

Set contaminant to each band boundary; assert each health outcome fires through
the medical owner exactly once and the surface shows the warning before it.

## III.4 Worked example: the pump that drank the power

**Report:** during a storm, drainage stopped, flooding rose, and the power
report showed normal headroom.

**Walk:**

```text
1. root: pump load was classified Tier3 (comfort) so shedding cut it first
2. deeper: the pump's tier assignment was authored when pumps were "optional"
3. repair: pumps are Tier1 when flooding is forecast, Tier0 when flooding is
   active (authored dynamic assignment); shedding warns; surfaces show why
4. verify: storm scenario; tier-dynamic test; warning-before-flood test
```

**Findings:**

| ID | Class | Repair |
|---|---|---|
| IN-03 | wrong shed tier for pumps | fairness |
| IN-04 | no dynamic tier policy | completeness |
| IN-05 | no warning before pump cut | fairness |

*End of Part III. Continues in Part IV (thermal, fire, condition, cascades, surfaces).*---

# W4-03 · PART IV — DEEP DESIGN: THERMAL, FIRE, CONDITION, CASCADES, SURFACES (POINTS 7–10)

## IV.1 Thermal and fire

### IV.1.1 Thermal rules

```text
T1  one thermal truth (ShelterThermalSystem); spaces have temperature bands
T2  heat sources consume power/fuel through owners; no free warmth
T3  insulation/hardening is persistent and saved (W4-01)
T4  cold consequences route to W3-03/W4-06 (morale/health), warned
T5  seasonal coupling: outside temperature from weather/season owners
T6  no wall-clock; campaign day drives seasons
```

### IV.1.2 Fire rules

```text
F1  fire incidents have states: unresolved | suppressed | out
F2  incidents originate from authored causes (stove, wiring, fuel)
F3  suppression consumes water/power/crew and reduces contaminant
F4  fire produces air contaminants (III.3) and thermal spikes
F5  crisis producers already read fire state (sealed row respected)
F6  every incident ends; lingering unresolved fires are stop-the-line
```

## IV.2 Machinery condition

### IV.2.1 Condition record

```jsonc
{ "machine": "pump_1", "condition": 71, "last_service": 190, "cycles": 4102 }
```

### IV.2.2 Rules

```text
C1  one condition record per machine; wear deterministic from load/cycles
C2  service consumes parts/labor via W3-05/W4-05
C3  0-condition disables with warning; no free self-repair
C4  condition is visible on reports with a service path
C5  cycles counters are bounded (roll into condition)
```

## IV.3 Failure cascades

### IV.3.1 The three authored chains

```text
CHAIN A flood->power->heat    pumps stall; water reaches breakers; heat fails
CHAIN B fire->air             contaminants rise; ventilation degrades
CHAIN C power->water          treatment stops; quality drops; consumption warns
```

### IV.3.2 Rules

```text
X1  cascades are authored links with depth and thresholds, not emergent chaos
X2  every link warns before it fires
X3  every cascade has at least one off-ramp (pump, breaker, crew, reserve)
X4  cascade depth is bounded; no infinite chains
X5  drills consume real readiness state (Expansion 23 precedent)
X6  consequences route through existing owners (health/morale/labor)
```

### IV.3.3 The off-ramp table

| Chain | Off-ramps | Cost |
|---|---|---|
| A | restore pump power, sandbag, reset breaker | power/crew/materials |
| B | suppress fire, run filtration, seal space | water/power/filters |
| C | reserve water, emergency treatment, ration | stockpiles/labor |

## IV.4 Surfaces

### IV.4.1 Surface contract

```text
power:  capacity, draw, headroom, fuel, tiers shed, reasons
water:  stock, quality, source status, treatment state, warnings
air:    quality bands, filter state, warnings
thermal: bands per space, deficits, warnings
fire:   active incidents, suppression state, causes
machines: condition, last service, next due
```

### IV.4.2 Rules

```text
S1  all surfaces read owners; zero local computation
S2  unavailable states authored; no fabricated zeros
S3  warnings lead to actions (routes to the fixing surface)
S4  units everywhere; thresholds named
S5  history bounded (per-day aggregates), saved (W4-01)
S6  W3-06 kits cover all infrastructure surfaces
```

## IV.5 Worked example: the cascade that surprised everyone

**Report:** a winter power dip froze the north wing without any warning.

**Walk:**

```text
1. root: heat loss drove a breaker trip; the trip was not warned and the
   thermal drop to freezing took one day with no threshold crossing notice
2. repair: breaker trip warns (pre-trip headroom warning); thermal warns at
   band edges; the cascade chain A is declared and off-rampable (reserve
   power); consequences warned before health effects
3. verify: winter scenario; cascade chain test; warning-before-harm
```

**Findings:**

| ID | Class | Repair |
|---|---|---|
| IN-06 | unwarned breaker trip | fairness |
| IN-07 | no thermal threshold warning | fairness |
| IN-08 | cascade not declared/off-rampable | completeness |

*End of Part IV. Continues in Part V (playbooks).*---

# W4-03 · PART V — PLAYBOOKS

## V.1 The P0 premise playbook

```text
1. freeze HEAD; enumerate every subsystem and its live owner
2. find every resource meter; list duplicates (power, water, fuel, air, heat)
3. enumerate consumers per resource; find orphan consumers
4. read tier assignments for every load; find life-support misclassified
5. read flood/power/air/thermal consumers; list inert states
6. enumerate machine condition records; find free repairs / never-wear
7. list cascades that exist in code; list assumed chains with no code
8. record crisis-producer wiring (sealed row respected)
9. write the premise note; claim paths; draft the package row
```

## V.2 The subsystem authoring playbook

```text
1. does an owner exist? if not, stop (Rule 5)
2. declare the resource meter and unit; register the save section
3. declare consumers and surfaces; warnings with thresholds and copy refs
4. author tier assignment (power), quality (water), bands (air/thermal)
5. write the reconciliation or threshold tests with the feature
6. add the ledger row; run the duplicate-meter gate
```

## V.3 The tier review playbook (quarterly)

```text
for each load: what happens to life if this sheds? if the answer is "cold",
"thirst", "poison", or "death" — the tier must be 0/1 with warnings.
misclassified loads are stop-the-line findings.
```

## V.4 The cascade authoring playbook

```text
1. name the chain (A/B/C or new) and its links
2. author thresholds and warning windows per link
3. author at least one off-ramp per link
4. bound depth; no loops
5. wire consequences through existing owners only
6. scenario-test the full chain and each off-ramp
```

## V.5 The condition authoring playbook

```text
1. wear rate from load/cycles, deterministic
2. service path and costs (parts/labor)
3. disable outcome with warning; repair path
4. surface row with condition + due date
5. no free repairs; no invisible decay
```

## V.6 Anti-pattern drills

**Drill 1 — the friendly meter.** Find a consumer that keeps its own counter.
Delete it; point at the owner.

**Drill 2 — the free reserve.** A report shows headroom that no source provides.
Trace the phantom; remove.

**Drill 3 — the silent shed.** Shed a Tier0 load in a test; verify the warning
fires before consequences.

**Drill 4 — the immortal pump.** Set condition to 0; verify disable, warning,
and repair path.

**Drill 5 — the unending fire.** Leave a fire unresolved past the authored
timeout; verify the incident still ends (suppression or spread rule).

*End of Part V. Continues in Part VI (verification catalog).*---

# W4-03 · PART VI — VERIFICATION CATALOG

## VI.1 T1 — static

| ID | Check | Fails when |
|---|---|---|
| T1.1 | duplicate-meter scan | a second authoritative store per resource |
| T1.2 | consumer inventory | consumer not listed in the ledger |
| T1.3 | panel-compute scan | capacity/headroom math in UI |
| T1.4 | tier completeness | load without tier assignment |
| T1.5 | warning registry | threshold without warning/copy ref |
| T1.6 | cascade declaration | chain in code not declared (or vice versa) |
| T1.7 | condition coverage | machine without condition record |
| T1.8 | clock scan | wall-clock in durable infra state |

## VI.2 T2 — focused per point

### Point 1 — inventory
```text
T2.1.1 ledger enumerates; every subsystem has owner/section/tests
T2.1.2 duplicate-meter gate (negative test)
```

### Point 2 — power
```text
T2.2.1 fuel-once test (24h reconciliation residual = 0)
T2.2.2 capacity/headroom derived, never stored
T2.2.3 breaker trip lifecycle with cause
T2.2.4 storage charge limits respected
```

### Point 3 — shedding
```text
T2.3.1 strict tier order under scarcity (Tier4 first)
T2.3.2 Tier0 shed requires warning + emergency state
T2.3.3 recovery staged with hysteresis
T2.3.4 every shed visible with reason
```

### Point 4 — water
```text
T2.4.1 request path honors min_quality; refusals carry reasons
T2.4.2 quality per source; blending authored
T2.4.3 treatment consumption (power/parts) once
T2.4.4 drawdown/recharge determinism
T2.4.5 contamination warned before consumption
```

### Point 5 — drainage
```text
T2.5.1 flood level deterministic from inflow/pump
T2.5.2 pump power dependency; loss stalls drainage
T2.5.3 every flood has consumers; inert-flood scan
T2.5.4 warnings before damage thresholds
```

### Point 6 — air
```text
T2.6.1 contaminant bounded; threshold bands defined
T2.6.2 filter wear/consumption; replacement path
T2.6.3 health outcomes route through W4-06 only
T2.6.4 fire→contaminant coupling; suppression reduces
T2.6.5 sealed-shelter ingress authored
```

### Point 7 — thermal/fire
```text
T2.7.1 thermal bands per space; deficits warned
T2.7.2 heat sources consume through owners
T2.7.3 fire incident lifecycle ends; lingering = fail
T2.7.4 fire consequences routed once (crisis producers respected)
T2.7.5 hardening persistence round-trip
```

### Point 8 — condition
```text
T2.8.1 wear determinism (same load sequence, same values)
T2.8.2 service consumption via crafts
T2.8.3 disable at 0 with warning; repair path exists
T2.8.4 surface condition truthful
```

### Point 9 — cascades
```text
T2.9.1 each chain fires link-by-link with warnings
T2.9.2 each off-ramp interrupts the chain
T2.9.3 depth bounded; no loops
T2.9.4 drill consumption (readiness) real
```

### Point 10 — surfaces
```text
T2.10.1 all infra surfaces read owners (no local math)
T2.10.2 unavailable states authored; no fabricated zeros
T2.10.3 warnings route to actions
T2.10.4 units + thresholds displayed
T2.10.5 history bounded; round-trip
T2.10.6 W3-06 kits over infra surfaces
```

## VI.3 T3 — seeded soak

```text
60 days, seeded weather with one storm and one cold snap
assert: reconciliation zero; no fabricated headroom; cascades bounded with
        warnings; conditions advance; no silent life-support degradation;
        history bounded; no exceptions
```

## VI.4 Evidence formats

```yaml
run: T2.2.1
date: ____  head: ____
window: 24h  generation: __  consumption: __  storage_delta: __
residual: 0
result: pass
```

*End of Part VI. Continues in Part VII (worked threads).*---

# W4-03 · PART VII — WORKED THREADS AND FINDINGS (IN-09–IN-20)

## VII.1 Thread A — "the water that tasted wrong"

**Report:** after a treatment failure, consumption continued with no warning.

**Walk:**

```text
1. root: treatment stopped (power shed) but quality tier update ran nightly;
   consumers drank brackish stock for a day
2. repair: quality changes are events; consumers check min_quality per request;
   refusal or authored downgrade with warning; nightly batch removed
3. verify: shed-treatment scenario; request-path test
```

| ID | Class | Repair |
|---|---|---|
| IN-09 | batch quality updates | truth |
| IN-10 | no per-request quality check | completeness |

## VII.2 Thread B — "the heat that reversed"

**Report:** the north wing showed warm while freezing.

**Walk:**

```text
1. root: thermal surface read a cached band value written at bind
2. repair: read owner; refresh on state change; W3-06 stale-bind rule
3. verify: stale-bind test; winter scenario
```

| ID | Class | Repair |
|---|---|---|
| IN-11 | cached thermal band | truth |
| IN-12 | no refresh-on-change | coverage |

## VII.3 Thread C — "the breaker that tripped forever"

**Report:** a breaker stayed tripped after its cause was removed.

**Walk:**

```text
1. root: trip state persisted without a reset path; the reset action existed
   but was not wired to the surface
2. repair: trip state has an explicit reset action; surface routes to it; the
   cause is recorded for the log
3. verify: trip->reset cycle; surface action test
```

| ID | Class | Repair |
|---|---|---|
| IN-13 | no reset path wired | completeness |
| IN-14 | no trip-cycle test | coverage |

## VII.4 Thread D — "the filter that never wore"

**Report:** air quality stayed perfect despite months of smoke.

**Walk:**

```text
1. root: filters had a wear rate of 0 in data ("TODO")
2. repair: authored wear; consumption; replacement recipe (W3-05)
3. verify: wear determinism; replacement consumption
```

| ID | Class | Repair |
|---|---|---|
| IN-15 | zero wear placeholder | completeness |
| IN-16 | no filter lifecycle test | coverage |

## VII.5 Thread E — "the flood that ate the archives"

**Report:** a flooded storage node damaged items with no warning.

**Walk:**

```text
1. root: flood node existed; damage consumer existed; warning did not
2. repair: depth thresholds warn before damage; the storage surface shows the
   risk; pump capacity is displayed
3. verify: warning-before-damage test; flood scenario
```

| ID | Class | Repair |
|---|---|---|
| IN-17 | unwarned flood damage | fairness |
| IN-18 | no threshold warnings | coverage |

## VII.6 Thread F — "the machine that fixed itself"

**Report:** a pump's condition recovered overnight.

**Walk:**

```text
1. root: condition clamped at 100 by a max() in the surface read, and the
   service action was auto-triggered by a leftover test hook
2. repair: remove hook; service is player/crew action; clamp belongs to owner
3. verify: no free repair test; hook scan
```

| ID | Class | Repair |
|---|---|---|
| IN-19 | auto-service hook | integrity |
| IN-20 | surface-side clamp | truth |

## VII.7 Summary

```text
A: quality is checked per request, not per batch
B: bands are read live, not cached at bind
C: every trip has a reset path
D: wear placeholders are defects
E: damage is warned before it lands
F: repairs are actions, never hooks
```

*End of Part VII. Continues in Part VIII (Q&A).*---

# W4-03 · PART VIII — QUESTIONS AND ANSWERS

**Q1. Why one plan for power, water, air, heat, and fire?**
Because they fail together. The cascades are the plan's subject; separate plans
would re-split the chains.

**Q2. What is the infrastructure ledger for?**
Making one-owner-per-concern mechanical: every subsystem, meter, consumer,
warning, and surface in one table.

**Q3. What is the duplicate-meter gate?**
A static check that each resource has exactly one authoritative store.

**Q4. Why is fuel called out specifically?**
Because Plan 122 already owns fuel consumption through a live path; doubling it
is the most likely real defect (IN-01).

**Q5. What is headroom, and why is it derived?**
Capacity minus draw. Stored headroom drifts; derived headroom cannot.

**Q6. How strict is tier order?**
Strict. Tier4 sheds before Tier3, always. Exceptions are authored by tier
assignment, not by special cases at shed time.

**Q7. When can a Tier0 load shed?**
Only in an authored emergency with a warning first and an off-ramp named.

**Q8. What makes a brownout fair?**
It warns as headroom trends down; it never appears in one tick.

**Q9. Who owns quality of life consequences?**
Morale → W3-03; health → W4-06; labor → W4-05. Infrastructure routes, never
computes.

**Q10. Why per-request water quality checks?**
Because batch updates let a day of bad water pass. Requests check; refusals
explain.

**Q11. What is blending in water terms?**
Mixing sources changes the profile; it is authored, never an accidental
average.

**Q12. Why does aquifer drawdown matter?**
It makes wells a long-term decision, deterministic and warned — not infinite.

**Q13. What does a flood need to exist?**
Levels, pumps, power dependency, thresholds, warnings, and at least one
consumer.

**Q14. What are the flood's consumers?**
Pathing, storage damage, surfaces, health (via W4-06), and the power cascade.

**Q15. What is the air model's unit?**
A bounded contaminant level per space plus airflow and filter state.

**Q16. How does fire talk to air?**
It produces contaminant at authored rates; suppression reduces it. One
coupling, declared.

**Q17. Why is thermal separate from needs?**
Thermal is a physical field; needs read it. Warmth morale/health consequences
belong to their owners.

**Q18. What warns before cold damage?**
Band-edge warnings plus the thermal report; damage routes through owners with
prior warning.

**Q19. What is machinery condition for?**
Making wear and service real: deterministic rates, real costs, visible
disables, no free repairs.

**Q20. What is a cascade, precisely?**
An authored chain of failure links with thresholds, warnings, and off-ramps.

**Q21. Why bound cascade depth?**
Because infinite chains turn one storm into an unrecoverable game. Depth is a
design decision, authored.

**Q22. What is an off-ramp?**
An intervention that interrupts a chain: restore power, reset breaker, dispatch
crew, spend reserve.

**Q23. How do drills relate?**
Drills consume real readiness state (Expansion 23 precedent) so preparedness
is modeled, not cosmetic.

**Q24. What does "surfaces read" mean in practice?**
Capacity, draw, quality, bands, conditions all come from owners; the UI does
arithmetic only on display formatting.

**Q25. What is a fabricated zero?**
Showing 0 when data is unavailable. The authored state is "unavailable" with a
reason.

**Q26. How is history bounded?**
Per-day aggregates with retention; raw per-tick data is never durable.

**Q27. What is the smallest useful increment?**
Path A points 1–3: ledger, fuel-once, tier order. One week's work for
foundational safety.

**Q28. What does Path B add?**
One flow: water requests, flood consumers, air routing, thermal reads,
condition lifecycle — each consumed once, surfaced truthfully.

**Q29. What does Path C add?**
Resilience across years: hardening, redundancy, drills, and energy-poverty arcs
with recovery.

**Q30. What is out of scope?**
New resource models, balance rewrites, health computation, UI layout.

**Q31. What is the biggest risk?**
A second meter appearing "temporarily" for a new feature.

**Q32. The second risk?**
Life-support loads miscategorized then silently shed in an edge season.

**Q33. The third?**
Unbounded cascade improvisation instead of authored chains.

**Q34. How does the plan outlive its authors?**
The ledger, the reconciliation test, the tier review, and the calendar.

**Q35. The final sentence?**
One meter, one flow, one warned failure — or the shelter breaks quietly.

*End of Part VIII. Continues in Part IX (Path C designs).*---

# W4-03 · PART IX — PATH C IMPLEMENTATION DESIGNS (C1–C10)

## C1 — The resilient grid

```text
design: redundancy through authored sources + storage + priority; islanding
        (sections can run on reserve); restoration sequencing
acceptance: island scenario; reconciliation zero; restores staged
```

## C2 — The water web

```text
design: multiple sources with distinct profiles; treatment chains; storage
        tiers by quality; drawdown/recharge arcs
acceptance: source matrix; quality round-trip; long-run drawdown test
```

## C3 — The air ladder

```text
design: filtration stages with increasing capability and cost; sealed-space
        management; smoke seasons
acceptance: threshold routing; filter lifecycle; sealed ingress tests
```

## C4 — The thermal envelope

```text
design: insulation projects, zone control, seasonal preparation; hardening
        ties to W4-02 weather
acceptance: winter scenario; hardening persistence; warning coverage
```

## C5 — The condition culture

```text
design: service schedules on duty rosters (W4-05); spare parts chains (W3-05);
        foreknowledge of failure
acceptance: schedule consumption; disable-warning; repair routing
```

## C6 — The drill program

```text
design: authored drills for each cascade with real consumption and visible
        readiness effects (muster engine precedent)
acceptance: drill consumption; readiness deltas; no cosmetic drills
```

## C7 — The poverty arc

```text
design: sustained scarcity produces authored consequences with recovery paths
        (morale, health, labor) — never an unrecoverable spiral without warning
acceptance: scarcity scenario; recovery scenario; warning-before-harm
```

## C8 — The salvage loop

```text
design: wrecked machines become salvage (W3-05) with authored yields;
        replacement chains feed back into condition economy
acceptance: salvage yields; consumption; no infinite loop (diminishing)
```

## C9 — The quiet shelter

```text
design: when all systems are nominal, the shelter is silent: no warnings, no
        chores that do not matter, no noise — the reward for maintenance
acceptance: nominal-day scenario; zero spurious warnings
```

## C10 — The final shape

```text
design: one meter per resource, one flow per system, one warned failure per
        chain, and a calendar that keeps it true
acceptance: closure measurement (§XVI)
```

*End of Part IX. Continues in Part X (checklists).*---

# W4-03 · PART X — CHECKLISTS AND WORKSHEETS

## X.1 The P0 worksheet

```text
PACKAGE: ______  HEAD: ______  DATE: ______
[ ] subsystems enumerated (list): 
[ ] meters per resource — duplicates found: __
[ ] consumers listed; orphans: __
[ ] tier assignments; life-support misclassified: __
[ ] flood nodes; inert floods: __
[ ] air thresholds; health routing checked: y/n
[ ] machines; free-repair hooks: __
[ ] cascades in code vs assumed: __ / __
[ ] crisis-producer wiring confirmed (sealed respected): y/n
[ ] premises contradicted: __ (attach)
```

## X.2 The subsystem authoring worksheet

```text
SUBSYSTEM: ______  OWNER: ______
meter/unit: ______  save section: ______
consumers: ______  surfaces: ______
warnings (threshold + copy ref): ______
tier assignment (if powered): ______
tests: reconciliation | threshold | lifecycle
[ ] ledger row added  [ ] duplicate-meter gate green
```

## X.3 The cascade worksheet

```text
CHAIN: ____  links: __  depth: __
per link: threshold __  warning __  off-ramp __  cost __
[ ] declared in the cascade register
[ ] scenario-test green
[ ] off-ramps interrupt (each tested)
[ ] depth bounded; no loops
```

## X.4 The condition worksheet

```text
MACHINE: ______  wear: __/day  service: parts __ labor __
[ ] disable at 0 with warning
[ ] repair path exists
[ ] surface row truthful
[ ] no free repair hooks (scan)
```

## X.5 The surface checklist

```text
[ ] reads owners only
[ ] units + thresholds present
[ ] unavailable states authored
[ ] warnings route to actions
[ ] history bounded + round-trip
[ ] W3-06 kits green
```

## X.6 The reconciliation worksheet

```text
window: 24h
generation __  consumption __  storage_delta __  residual __
other windows: storm __  cold __  quiet __
[ ] all residuals zero
```

*End of Part X. Continues in Part XI (field guide and maintenance).*---

# W4-03 · PART XI — FIELD GUIDE, MAINTENANCE, AND CLOSURE

## XI.1 The one-page field guide

```text
INFRASTRUCTURE SHIPS WHEN:
  one meter per resource; reconciliation zero
  tier order strict; life-support warned
  water checked per request; quality honest
  floods warn; pumps depend on named power
  air routes through owners; filters wear
  heat and fire warn; hardening persists
  condition is real; repairs cost
  cascades bounded, warned, off-rampable
  surfaces read; zeros never fabricated
```

## XI.2 The maintenance calendar

| Cadence | Task |
|---|---|
| per feature | ledger row; reconciliation; warning refs |
| weekly | duplicate-meter scan; surface truth spot |
| release | reconciliation kit; tier review; cascade scenarios |
| seasonal | storm/cold scenario runs; condition audit |
| yearly | meter census; cascade register review; drill program |

## XI.3 The sweeps

```text
unresolved fires > timeout -> forced resolution, defect
breakers tripped > 3 days with cause resolved -> reset, defect
machines at 0 with no service record -> repair path audit
flood nodes flooded > authored max -> drainage audit
```

## XI.4 The closure measurement

```yaml
meters: { resources: 5, duplicates: 0 }
reconciliation: { day: 0, storm: 0, cold: 0 }
tiers: { order: pass, life_support: warned, recovery: staged }
water: { request_path: pass, quality: pass, drawdown: pass }
flood: { consumers: full, warnings: pass, pump_power: pass }
air: { routing: pass, filters: pass, ingress: pass }
thermal_fire: { bands: pass, incidents_end: pass, hardening: pass }
condition: { wear: pass, disable_warned: pass, no_free_repair: pass }
cascades: { chains: 3, warnings: pass, off_ramps: pass, depth: bounded }
surfaces: { read_only: pass, units: pass, unavailable: pass, kits: pass }
```

## XI.5 The closing statement

```text
The shelter is the only machine the player lives inside. It must never break
by surprise, never lie on a gauge, and never punish maintenance with silence.
```

*End of Part XI. Continues in Part XII (appendices and registers).*---

# W4-03 · PART XII — APPENDICES: REGISTERS AND TABLES

## XII.1 The resource meter register

| Resource | Meter | Unit | Owner | Save | Duplicates |
|---|---|---|---|---|---|
| power | capacity/charge | kW / kWh | grid | power.grid | 0 |
| fuel | stock | units | 122 path | holdfast.campaign | 0 |
| water | stock/flow | L | water owner | water.stores | 0 |
| air | contaminant/space | ppm | ventilation | air.spaces | 0 |
| heat | temperature/space | °C | thermal | thermal.spaces | 0 |

## XII.2 The load tier register (seed)

| Load | Tier | If shed | Warn |
|---|---|---|---|
| air circulation | 0 | suffocation risk | must |
| warmth minimum | 0 | cold injury | must |
| water treatment | 1 | quality drop | must |
| medicine fridge | 1 | meds spoil | must |
| pumps (active flood) | 0–1 | flooding | must |
| kitchen | 2 | hunger | should |
| production | 2 | output loss | should |
| lighting | 3 | comfort | info |
| leisure | 4 | morale | info |

## XII.3 The cascade register

| Chain | Links | Off-ramps | Depth |
|---|---|---|---|
| A flood→power→heat | 3 | pump/breaker/reserve | 3 |
| B fire→air | 2 | suppress/filter/seal | 2 |
| C power→water | 2 | reserve/ration/emergency treatment | 2 |

## XII.4 The machine register (seed)

| Machine | Wear/day | Service | Disable outcome |
|---|---|---|---|
| pump_1 | 0.4 | parts: seal | flood stall (warned) |
| filter_unit | 0.6 | parts: cartridge | air quality fall (warned) |
| heater | 0.3 | parts: element | zone cold (warned) |

## XII.5 The warning copy register

| Ref | Copy |
|---|---|
| power_low_fuel | "Fuel is low. Estimate: {days} days at current use." |
| power_overdraw | "Demand is above supply. Some systems will shut down." |
| power_trip | "{circuit} tripped. Cause: {cause}." |
| water_quality_fall | "Water quality has fallen. Treatment needed." |
| flood_rising | "Water is rising in {space}. Pumps are {state}." |
| air_degrading | "Air quality in {space} is falling." |
| heat_falling | "{space} is getting cold." |
| fire_started | "Fire in {space}. Suppression needed." |

*End of Part XII. Continues in Part XIII (case files).*---

# W4-03 · PART XIII — REVIEWER CASE FILES

## XIII.1 Case 1 — the "quick counter"

**Diff:** adds a small local power counter in a new panel for responsiveness.

**Review:**

```text
rule 5? local gauge duplicates the meter
verdict: RETURNED — read the grid owner; if perf is the concern, memoize with
         invalidation, never fork the value
```

## XIII.2 Case 2 — the helpful auto-service

**Diff:** machines auto-service at 10% "so players don't suffer."

**Review:**

```text
intent? convenience; but repairs must cost, and the choice is gameplay
verdict: RETURNED — warning at 10%, service remains an action consuming parts
```

## XIII.3 Case 3 — the cosmetics-only hardening

**Diff:** an insulation upgrade that changes the description but no number.

**Review:**

```text
consumption? no thermal effect authored
verdict: RETURNED — hardening must relax named gates/thresholds and persist
```

## XIII.4 Case 4 — the emergency shed exemption

**Diff:** a scenario needs pumps never shed "for drama."

**Review:**

```text
fairness? life-support exemptions require warnings + off-ramps
verdict: SIGNED if authored as emergency state with warning and recovery;
         RETURNED if silent
```

## XIII.5 The patterns

| Pattern | Tell | Verdict |
|---|---|---|
| local counter | new gauge variable | RETURNED |
| auto-service | no player action | RETURNED |
| cosmetic upgrade | no numeric effect | RETURNED |
| silent tier exception | no warning | RETURNED |
| free reserve | headroom appears | RETURNED |

## XIII.6 The review card

```text
1. which meter owns this number?
2. what tier is this load, and what happens if it sheds?
3. what warns before this fails?
4. what consumes this, and once?
5. what does the surface read?
```

*End of Part XIII. Continues in Part XIV (scenarios).*---

# W4-03 · PART XIV — SCENARIO BANK

## XIV.1 S1 — The quiet day

```text
fixture: all systems nominal
assert: zero warnings; reconciliation zero; surfaces stable
```

## XIV.2 S2 — The storm

```text
fixture: storm closes routes (W4-02), heavy rain
steps: flood risk rises; pumps on; power draw rises; a trip occurs; heat dips;
       crew option (off-ramp); recovery staged
assert: each link warned; off-ramp works; recovery staged; reconciliation zero
```

## XIV.3 S3 — The cold snap

```text
fixture: three-day cold front
steps: heat demand up; fuel draw up; a Tier4 shed; comfort drops; no life risk
assert: shedding visible; warmth minimum kept; warning at edges
```

## XIV.4 S4 — The bad water day

```text
fixture: treatment offline (power shed)
steps: quality falls; requests refuse or downgrade with warning; consumption
       consequences routed (W4-06); restoration
assert: per-request checks; warnings; recovery
```

## XIV.5 S5 — The fire at night

```text
fixture: kitchen fire
steps: contaminant rises; suppression costs; air degrades to threshold;
       health routing; incident ends
assert: warnings; one incident; consequences once; ends always
```

## XIV.6 S6 — The pump that aged

```text
fixture: pump at 12% condition
steps: warning; service consumes parts/labor; condition restores; no free path
assert: disable-warning at 0; service is an action
```

## XIV.7 S7 — The fuel audit

```text
fixture: 24h heavy load and 24h light load
assert: reconciliation zero in both; no double draw
```

## XIV.8 S8 — The lifeline review

```text
fixture: force scarcity; verify order
assert: Tier4/3 first; Tier0 only with emergency warning; recovery staged
```

## XIV.9 S9 — The cascade drill

```text
fixture: drill program active
steps: drill consumes readiness; crews respond; response time improves
assert: real consumption; no cosmetic drills
```

## XIV.10 S10 — The clean desk

```text
fixture: registers + kits
assert: duplicates 0; orphans 0; warnings complete; conditions covered
```

## XIV.11 The soak recipe

```text
60 days: one storm, one cold snap, one fire, one pump failure, one fuel
shortage. Assert: reconciliation zero; every consequence warned; every chain
off-rampable; every machine's life visible.
```

*End of Part XIV. Continues in Part XV (governance and rollout).*---

# W4-03 · PART XV — GOVERNANCE, HANDOFFS, AND ROLLOUT

## XV.1 Governance

| Concern | Owner |
|---|---|
| meters | named owners per resource |
| tiers | shedding engine + authored assignments |
| water | source/treatment owners |
| flood | sump system |
| air | ventilation + filtration |
| thermal/fire | thermal + fire owners |
| condition | machinery records |
| cascades | crisis producers + register |
| surfaces | W3-06 kits |

## XV.2 Handoffs

| Direction | Detail |
|---|---|
| W3-05 | parts, filters, seals, insulation crafts |
| W3-03 | morale consequences of scarcity; recovery arcs |
| W4-01 | sections for meters/condition/history |
| W4-02 | weather gates, storm/cold coupling |
| W4-05 | maintenance labor, drills, schedules |
| W4-06 | health outcomes of air/water/thermal exposure |
| W3-06 | infrastructure surfaces join kits |
| W4-04 | water for growing; waste heat options |

## XV.3 The rollout (5 weeks)

```text
w1  P0: ledger, meters, duplicates, tiers, cascades, premises
w2  power + shedding: reconciliation, tier order, warnings
w3  water + drainage: request path, quality, flood consumers, pump power
w4  air + thermal + fire: routing, thresholds, incidents, hardening
w5  condition + cascades + surfaces + soak + closeout
```

## XV.4 Exits per week

| Week | Exit |
|---|---|
| 1 | ledger filed; duplicates listed |
| 2 | reconciliation zero; tier order green |
| 3 | water/drainage kits green |
| 4 | air/thermal/fire kits green |
| 5 | condition/cascades/surfaces green; closure measured |

## XV.5 The risk register

| ID | Risk | Mitigation |
|---|---|---|
| R1 | duplicate meter appears | weekly scan |
| R2 | life-support mis-tiered | quarterly review |
| R3 | cascade improvises | register + scenario runs |
| R4 | free repairs creep | hook scan |
| R5 | fabricated zeros return | surface kit |
| R6 | warnings lag consequences | warning-before-harm kit |

## XV.6 Stop-the-line list

```text
1. a second meter for any resource
2. a Tier0 shed without warning/off-ramp
3. a cascade link without warning
4. a machine repairing itself
5. a fabricated zero on a surface
6. an unresolved fire past timeout
```

*End of Part XV. Continues in Part XVI (closure and final control).*---

# W4-03 · PART XVI — CLOSURE MEASUREMENT AND FINAL CONTROL

## XVI.1 The closure measurement

```yaml
run: W4-03-closure
head: <sha>
meters: { resources: 5, duplicates: 0, orphans: 0 }
reconciliation: { day: 0, storm: 0, cold: 0, heavy: 0 }
tiers: { strict: pass, life_support: warned, emergency: authored, recovery: staged }
water: { request_min_quality: pass, refusals: explained, drawdown: deterministic }
flood: { consumers: complete, warnings: pass, pump_power: pass, prevention: routed }
air: { threshold_routing: pass, filters: lifecycle, ingress: authored }
thermal_fire: { bands: pass, incidents_end: pass, hardening: persistent }
condition: { wear: deterministic, disable: warned, service: action, no_free: pass }
cascades: { chains: 3, warnings_each_link: pass, off_ramps: tested, depth: bounded }
surfaces: { read_only: pass, units: pass, unavailable: authored, history: bounded }
soak: pass
```

## XVI.2 The acceptance table

| Line | Evidence | Signed |
|---|---|---|
| ledger + meters | scan outputs | ☐ |
| reconciliation | window reports | ☐ |
| tiers | order + emergency tests | ☐ |
| water | request/quality kits | ☐ |
| flood | scenarios | ☐ |
| air | routing kit | ☐ |
| thermal/fire | scenarios | ☐ |
| condition | life-cycle kit | ☐ |
| cascades | chain scenarios | ☐ |
| surfaces | kits + lie audit | ☐ |

## XVI.3 The binding summary

```text
Binding: ledger rules L1–L6, power rules P1–P6, shedding rules S1–S6, water
rules W1–W6, flood rules F1–F6, air rules A1–A6, thermal rules T1–T6, fire
rules F1–F6, condition rules C1–C5, cascade rules X1–X6, surface rules S1–S6,
and the stop-the-line list (§XV.6).
```

## XVI.4 The final declaration

**W4-03 is complete as a plan.** Parts I–XVI with findings IN-01…IN-20.
Proposal only; execution requires Annex U and its signatures. It hands the
wave: one meter per resource, one warned failure per chain, one truthful
shelter.

```text
The shelter must never break by surprise, lie on a gauge, or punish
maintenance with silence.
```

*Document control: W4-03 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-03 (expansion continues as needed).*---

# W4-03 · PART XVII — Q&A, SECOND BAND (Q36–Q80)

**Q36. What makes a gauge trustworthy?**
One owner, one unit, one update path, and no local math.

**Q37. What is the reconciliation test really checking?**
That no energy appears or disappears: no double draws, no phantom reserves.

**Q38. Why is fuel explicitly "once per tick"?**
Because the likeliest real bug in this domain is a second consumer on the same
fuel path.

**Q39. What does "strict tier order" forbid?**
Preference logic, "important" exceptions, and silent sacrifices.

**Q40. What is the emergency state for?**
Authored, warned Tier0 shedding with named off-ramps and recovery.

**Q41. How does a browout differ from a blackout?**
Brownout is sustained overdraw with warnings and staged shedding; blackout is
total loss after cascades.

**Q42. What keeps scarcity from becoming griefing?**
Warned arcs, authored recovery, and consequences routed through owners with
paths back.

**Q43. Why per-request water checks?**
Because water quality changes faster than daily batches, and a day of bad
water is a day of silent harm.

**Q44. What does "blending is authored" mean?**
Mixing sources computes a declared profile; no accidental averaging.

**Q45. Why does drawdown exist?**
So wells are a long-term resource with decisions, not an infinite tap.

**Q46. What is a flood's minimum shape?**
Level, pump, power tier, thresholds, warnings, consumers.

**Q47. What consumes flood state?**
Pathing, damage, health, surfaces, and the power cascade.

**Q48. Why is the pump's tier dynamic?**
Because a pump is comfort in dry days and life-support in a flood; the tier
follows the state, authored.

**Q49. What makes air "modeled" rather than decorative?**
Bounded contaminant per space with sources, sinks, thresholds, and routed
consequences.

**Q50. What is the air/fire coupling?**
Fire produces contaminant; suppression reduces it; one declared interaction.

**Q51. Why is thermal separate from needs?**
It is a physical field; needs read it; consequences belong to their owners.

**Q52. What warns before cold damage?**
Band-edge warnings and the thermal report; damage only after warning.

**Q53. What is machinery condition for?**
Real wear, real service, visible disables — the texture of a maintained home.

**Q54. What makes a service an action rather than a chore?**
Cost and choice: parts, labor, timing. Auto-service removes the choice.

**Q55. What is a cascade in one sentence?**
An authored chain of failures with warnings and off-ramps.

**Q56. Why bound depth?**
So a storm cannot become an unrecoverable spiral by accident.

**Q57. What is the off-ramp principle?**
Every link offers at least one intervention; preparedness is playable.

**Q58. How do drills avoid being cosmetic?**
They consume readiness state and change response times.

**Q59. What is fabricated zero's harm?**
It teaches players to distrust gauges — the worst outcome for infrastructure.

**Q60. What history is kept?**
Per-day aggregates with retention: consumption, condition, incidents.

**Q61. What is the smallest useful increment?**
Ledger + fuel-once + tier order. One week; foundational safety.

**Q62. What does Path B add?**
One flow per resource: requests, flood consumers, routing, condition actions.

**Q63. What does Path C add?**
Years: redundancy, hardening, drills, poverty arcs with recovery.

**Q64. What is out of scope?**
Models, balance, health math, UI layout.

**Q65. What is the top risk?**
A convenience meter appearing in a new feature.

**Q66. The second?**
Mis-tiered life support in an edge season.

**Q67. The third?**
Unbounded cascade improvisation.

**Q68. How does the plan outlive its authors?**
Ledger + reconciliation + tier review + calendar.

**Q69. What does the shelter owe the player?**
A gauge that never lies and a failure that never surprises without warning.

**Q70. What does the player owe the shelter?**
Maintenance — and the plan makes that visible and fair.

**Q71. What is the quiet-shelter design?**
When all is nominal: no warnings, no noise. Silence is the reward.

**Q72. What is the loud-shelter rule?**
Every warning names the space, the system, and the next action.

**Q73. How are warnings localized?**
Copy refs into the corpus (D22); models carry refs only.

**Q74. What does the surface show when data is missing?**
An authored unavailable state with a reason — never a zero.

**Q75. What is the review sentence?**
"Which meter, which tier, which warning?"

**Q76. What is the build sentence?**
"Reconciliation zero, or it does not ship."

**Q77. What is the shelter sentence?**
"One meter, one flow, one warned failure."

**Q78. What remains after closure?**
The calendar: scans, scenarios, tier reviews, drills, census.

**Q79. What is the yearly deletion?**
One dead mechanism (free hook, duplicate path, cosmetic upgrade) removed with
a note.

**Q80. The last word?**
A shelter is a promise of shelter; keep the gauges true.

*End of Part XVII. Continues in Part XVIII (threads, second band).*---

# W4-03 · PART XVIII — WORKED THREADS, SECOND BAND (IN-21–IN-32)

## XVIII.1 Thread G — "the reserve that counted twice"

**Report:** after a blackout, restoring power showed double the expected charge.

**Walk:**

```text
1. root: storage restore applied remaining minutes as kWh in one path and as
   percent in another; both fed the gauge
2. repair: one unit per meter; conversion at the display edge only
3. verify: unit-consistency test; restoration scenario
```

| ID | Class | Repair |
|---|---|---|
| IN-21 | unit mixing | one unit law |
| IN-22 | no unit test | coverage |

## XVIII.2 Thread H — "the smoke that stayed"

**Report:** after a fire, air quality never returned to baseline.

**Walk:**

```text
1. root: contaminant had sources but no natural decay; filtration was the only
   sink and the filter was exhausted (sink zero)
2. repair: authored decay plus filtration; exhausted filters degrade but do
   not freeze quality forever; replacement restores
3. verify: decay test; filter replacement scenario
```

| ID | Class | Repair |
|---|---|---|
| IN-23 | no contaminant decay | completeness |
| IN-24 | no exhaustion path | coverage |

## XVIII.3 Thread I — "the well that drank the aquifer" 

**Report:** after 60 days of maximal draw, yield dropped with no warning.

**Walk:**

```text
1. root: drawdown existed; warning did not; the surface showed current yield
   only
2. repair: drawdown warnings at authored bands; recovery by rotation or
   recharge; surface shows trend
3. verify: drawdown warning test; rotation recovery
```

| ID | Class | Repair |
|---|---|---|
| IN-25 | unwarned yield fall | fairness |
| IN-26 | no trend display | coverage |

## XVIII.4 Thread J — "the drill that cost nothing"

**Report:** emergency drills raised readiness but consumed no resources.

**Walk:**

```text
1. root: the drill engine updated readiness without consuming time/supplies
2. repair: drills consume crew time and supplies; readiness gains are real
3. verify: consumption test; readiness delta test
```

| ID | Class | Repair |
|---|---|---|
| IN-27 | free drills | fairness |
| IN-28 | no consumption check | coverage |

## XVIII.5 Thread K — "the heat that leaked the wrong way"

**Report:** a damaged wall made adjacent rooms *warmer*.

**Walk:**

```text
1. root: transfer signs were applied with an absolute value in one path
2. repair: signed transfer; damaged walls leak out, not in; test both
3. verify: signed-transfer test; envelope scenario
```

| ID | Class | Repair |
|---|---|---|
| IN-29 | sign error | correctness |
| IN-30 | no envelope test | coverage |

## XVIII.6 Thread L — "the pump that served a dry sump"

**Report:** pumps ran at full power on dry nodes, wasting draw.

**Walk:**

```text
1. root: the pump loop lacked a dry check
2. repair: dry nodes stop pumps; surface shows "idle"; draw drops; no phantoms
3. verify: idle-draw test; reconciliation
```

| ID | Class | Repair |
|---|---|---|
| IN-31 | phantom draw | reconciliation |
| IN-32 | no idle check | coverage |

## XVIII.7 Summary

```text
G: one unit per meter; convert at the edge
H: contaminants decay; filters exhaust and can be replaced
I: drawdown warns and recovers
J: drills consume; readiness is earned
K: transfers are signed; damage leaks outward
L: idle machinery does not draw
```

*End of Part XVIII. Continues in Part XIX (year one).*---

# W4-03 · PART XIX — YEAR ONE OF THE SHELTER PROGRAM

## XIX.1 The standing commitments

```text
C1  duplicate-meter scan weekly; ledger row per new subsystem
C2  reconciliation kit per release; zero residual required
C3  tier review quarterly; life-support audit seasonal
C4  cascade scenarios per release; off-ramps re-verified
C5  condition sweep monthly; no free-repair hooks
C6  surface truth spot weekly; lie audit per release
C7  drill program run quarterly; consumption verified
C8  meter census yearly
```

## XIX.2 The year plan

```text
Q1  ledger refresh; reconciliation; tier review
Q2  water/air audit; filter lifecycle; quality paths
Q3  flood/thermal seasonal runs; cascade scenarios; hardening review
Q4  condition culture review; drill program; next-year resilience targets
```

## XIX.3 The health signals

```text
- gauges agree with experience; nobody reports "the report says fine"
- storms are survived by choices, not luck
- the quiet day is genuinely quiet
- maintenance happens because it matters, not because a bar nags
- failures have names and next actions
```

## XIX.4 The rot signals

```text
- a second meter appears "for this feature"
- a Tier0 load sheds silently
- a cascade fires without warning
- a machine repairs itself
- a gauge shows zero for missing data
- an incident lingers past timeout
```

## XIX.5 The annual retrospective

```text
1. reconciliation history: any nonzero residual, why
2. shed events: life-support sheds (target zero without emergency)
3. water quality incidents: warnings before harm
4. cascade runs: links, off-ramps used, outcomes
5. condition: disables, service immediacy, cost realism
6. drills: consumption and readiness trends
7. next-year targets: one resilience gain, one deletion
```

## XIX.6 The end state

```text
A shelter program is healthy when infrastructure disappears from the
conversation: the player thinks about people and choices, not about whether
the gauges are lying.
```

*End of Part XIX. Continues in Part XX (extended registers).*---

# W4-03 · PART XX — EXTENDED REGISTERS AND TABLES

## XX.1 The subsystem register (seed)

| Subsystem | Owner | State | Consumers | Warnings |
|---|---|---|---|---|
| power.grid | grid + SOFC + 122 | gen/store/dist/fuel | all loads | low fuel, overdraw, trip |
| water.sources | well/condenser/brine | yield/quality | treatment, kitchen | drawdown, contamination |
| water.treatment | treatment | stage/quality | requests | stage failure |
| water.stores | stores | per-quality stock | requests | low stock |
| drainage.sump | sump | node levels | damage/pathing | rising water |
| air.spaces | ventilation | contaminant/airflow | health/head | degrading, filters |
| thermal.spaces | thermal | bands | needs/health | falling heat |
| fire.incidents | fire | state/cause | air/damage/crisis | started, spreading |
| machines.* | condition | wear/cycles | production | due, disabled |

## XX.2 The warning threshold register

| Warning | Threshold | Warning window | Copy ref |
|---|---|---|---|
| low fuel | 3 days at current use | 3 days | power_low_fuel |
| overdraw | draw > 90% capacity | sustained 6h | power_overdraw |
| breaker pre-trip | headroom trending to 0 | 12h | power_overdraw |
| quality fall | tier down | immediate on event | water_quality_fall |
| flood rising | depth > 10 | before damage | flood_rising |
| air degrading | band down one step | on event | air_degrading |
| heat falling | band edge | on event | heat_falling |
| fire | any incident | immediate | fire_started |

## XX.3 The grade register (conditions)

| Machine class | Wear/day | Service interval | Disable severity |
|---|---|---|---|
| pump | 0.4 | 30 days | flood stall |
| filter unit | 0.6 | 20 days | air fall |
| heater | 0.3 | 45 days | cold zone |
| kitchen stove | 0.5 | 25 days | no cooking |
| treatment | 0.35 | 35 days | quality fall |

## XX.4 The drill register

| Drill | Consumes | Readiness effect | Cadence |
|---|---|---|---|
| flood response | crew time, sandbags | pump/response speed | monthly |
| fire response | water, crew | suppression speed | monthly |
| blackout | — (simulated) | tier discipline | quarterly |
| contamination | filters (burn) | routing speed | quarterly |

## XX.5 The history register (bounded)

| History | Aggregation | Retention |
|---|---|---|
| consumption | per day | 60 days |
| quality incidents | per event | 40 rows |
| incidents | per event | 40 rows |
| conditions | per machine | current + last service |
| drills | per event | 20 rows |

*End of Part XX. Continues in Part XXI (scenarios, second bank).*---

# W4-03 · PART XXI — SCENARIO BANK 2 (S11–S22)

## XXI.1 S11 — The long hard winter

```text
fixture: 30-day cold with intermittent storms
assert: heat demand arcs; fuel draw visible; sheds by tier; no life risk
        unwarned; recovery when weather breaks; reconciliation zero
```

## XXI.2 S12 — The double-draw audit

```text
fixture: every fuel consumer active
assert: residual zero; no consumer bypasses the 122 path
```

## XXI.3 S13 — The filter famine

```text
fixture: filters unavailable for 20 days; smoke events
assert: quality degrades gradually; warnings; alternatives (seal spaces,
        ventilation staging); replacement restores
```

## XXI.4 S14 — The well rotation

```text
fixture: two wells, one aquifer
assert: rotation avoids drawdown bands; yield recovers; surface shows trend
```

## XXI.5 S15 — The storm cascade

```text
fixture: chain A end to end
assert: each link warns; off-ramps interrupt; depth bounded; recovery staged
```

## XXI.6 S16 — The fire drill

```text
fixture: drill then real fire
assert: drill consumed resources; response faster; real fire still costs
```

## XXI.7 S17 — The frozen pipe

```text
fixture: heat failure in water space
assert: warning before damage; pipes freeze (authored); repair path; quality
        consequences routed
```

## XXI.8 S18 — The battery audit

```text
fixture: charge/discharge cycles
assert: efficiency authored; limits; reconciliation across cycles zero
```

## XXI.9 S19 — The idle plant

```text
fixture: all machines at rest
assert: zero phantom draw; no wear beyond idle rate; quiet day
```

## XXI.10 S20 — The archive flood

```text
fixture: flooding reaches storage
assert: warning; player choice (move, pump, accept loss with warnings);
        damage only after warnings
```

## XXI.11 S21 — The blackout drill

```text
fixture: emergency forced
assert: Tier0 emergency warnings; off-ramps named; recovery order staged
```

## XXI.12 S22 — The clean desk

```text
fixture: registers + kits
assert: duplicates 0; orphans 0; warnings complete; conditions covered;
        cascades declared
```

## XXI.13 The scenario cadence

| Set | Cadence |
|---|---|
| S1–S10 | per release |
| S11–S20 | seasonal rotation |
| S21–S22 | per change to shedding/cascades |

*End of Part XXI. Continues in Part XXII (threads, third band).*---

# W4-03 · PART XXII — WORKED THREADS, THIRD BAND (IN-33–IN-44)

## XXII.1 Thread M — "the warning that arrived with the damage"

**Report:** players learned about a flood at the same tick damage applied.

**Walk:**

```text
1. root: warning threshold and damage threshold shared a value
2. repair: warning bands strictly precede damage bands; authoring rule and a
   test that asserts the gap
3. verify: band-gap test across all warnings
```

| ID | Class | Repair |
|---|---|---|
| IN-33 | threshold collision | fairness |
| IN-34 | no band-gap test | coverage |

## XXII.2 Thread N — "the tier that followed the player"

**Report:** a load's tier changed by time-of-day, silently.

**Walk:**

```text
1. root: an authored "day/night" tier schedule existed with no surface display
2. repair: dynamic tiers are displayed with their rule; or removed; never
   silent
3. verify: dynamic-tier visibility test
```

| ID | Class | Repair |
|---|---|---|
| IN-35 | hidden tier schedule | truth |
| IN-36 | no tier-display test | coverage |

## XXII.3 Thread O — "the water that un-mixed"

**Report:** a blended tank separated into quality bands overnight.

**Walk:**

```text
1. root: two stores wrote to one tank with different profiles; read order
   decided which profile applied
2. repair: blending is an authored operation producing one profile; tanks hold
   one profile; read order cannot matter
3. verify: blended-profile determinism test
```

| ID | Class | Repair |
|---|---|---|
| IN-37 | read-order-dependent quality | determinism |
| IN-38 | no blend test | coverage |

## XXII.4 Thread P — "the heater that warmed the wall"

**Report:** thermal was fine in spaces but walls overheated.

**Walk:**

```text
1. root: wall temperature was computed but surfaced nowhere; corners ignored it
2. repair: walls are either modeled and consumed (fire risk) or not modeled;
   half-modeled fields removed
3. verify: model-completeness test
```

| ID | Class | Repair |
|---|---|---|
| IN-39 | half-modeled field | completeness |
| IN-40 | no completeness gate | coverage |

## XXII.5 Thread Q — "the incident that forgot its cause"

**Report:** after a fire, the incident log showed no cause.

**Walk:**

```text
1. root: causes existed in code paths but not in records
2. repair: every incident records an authored cause; surfaces show it; drills
   can target it
3. verify: cause-present test
```

| ID | Class | Repair |
|---|---|---|
| IN-41 | missing incident cause | completeness |
| IN-42 | no cause check | coverage |

## XXII.6 Thread R — "the machine that wore while off"

**Report:** condition dropped faster on idle machines than working ones.

**Walk:**

```text
1. root: wear read the wrong cycle field after a refactor
2. repair: wear per authored load profile (idle vs working); test both
3. verify: load-profile wear test
```

| ID | Class | Repair |
|---|---|---|
| IN-43 | wrong wear input | correctness |
| IN-44 | no profile test | coverage |

## XXII.7 Summary

```text
M: warnings strictly precede consequences
N: dynamic tiers are visible
O: blending is authored; one profile per tank
P: half-modeled fields are removed
Q: incidents record causes
R: wear reads the right input
```

*End of Part XXII. Continues in Part XXIII (Q&A, third band).*---

# W4-03 · PART XXIII — Q&A, THIRD BAND (Q81–Q120)

**Q81. What does "infrastructure disappears from the conversation" mean?**
Gauges are trusted, failures are fair, maintenance is meaningful — so players
talk about survival choices, not about bugs.

**Q82. What is the strongest single test?**
Reconciliation zero across a storm day. It catches double draws, phantom
reserves, and unit errors at once.

**Q83. What is the second strongest?**
Warning-before-harm: every consequence preceded by its warning band.

**Q84. What makes tier assignments "fair"?**
Strict order, visible rules, warned exceptions, and recoverable states.

**Q85. What if a load defies classification?**
It gets a default tier plus an authored review entry; unclassified loads are
build failures.

**Q86. Can tiers change with context?**
Yes, authored and shown (e.g., pumps during floods). Hidden schedules are
defects.

**Q87. Why is water checked per request?**
So quality changes cannot slip through a batch window. Harm is immediate,
checks are immediate.

**Q88. What does the refusal message say?**
Why the request failed (quality, quantity) and what would fix it (treatment,
reserve). Refs, not raw codes.

**Q89. Can water be "saved" by downgrading?**
Authored: a request may accept a lower band with declared consequences
(warned). Silent substitution is forbidden.

**Q90. What is flood prevention?**
Authored works (berms, drainage, sump upgrades) routed through the crafting
owner; they change levels/rates.

**Q91. Can flooding be a permanent loss?**
Authored: some spaces may become unusable until repaired. Losses are warned
and recorded.

**Q92. What makes contamination fair?**
Sources named, spread bounded, warnings at bands, remedies available (seal,
filter, evacuate).

**Q93. Why is fire's cause recorded?**
So drills, prevention, and repairs can target it — and so the log tells the
truth.

**Q94. What does a fire drill change?**
Response time and crew routes — real readiness state, consumed resources.

**Q95. What is a machine's "life"?**
A condition curve: wear by load, service by parts, disable by neglect. It is
visible and consequential.

**Q96. Why not auto-service?**
Because maintenance is a choice with costs, and the plan refuses to remove
that gameplay. Warnings make it fair, not automatic.

**Q97. What is a cascade's off-ramp budget?**
Authored per link: at least one intervention; costs known; effects tested.

**Q98. How deep can a chain go?**
Three links by default; anything deeper requires an authored reason and a
stop-the-line review. Unbounded chains are forbidden.

**Q99. What makes the quiet day possible?**
All warnings have gaps; no machine nags needlessly; history is summarized.
Silence is earned by maintenance.

**Q100. What happens when maintenance slips?**
Warnings, then degradation, then disables — each with paths back. The shelter
bends; it does not silently shatter.

**Q101. What is the poverty arc's guardrail?**
Every consequence warned, every arc recoverable, no spiral without off-ramps.

**Q102. How does the plan treat new subsystems?**
Five-line compliance (W4-01): owner, section, tests, bounds, migration.

**Q103. What is the one number to watch?**
Life-support sheds without emergency state: target zero.

**Q104. What is the second number?**
Reconciliation residuals: target zero.

**Q105. What is the third?**
Warning-before-harm failures: target zero.

**Q106. What is the fourth?**
Fabricated zeros seen by players: target zero.

**Q107. What is the fifth?**
Incidents past timeout: target zero.

**Q108. How often are tiers reviewed?**
Quarterly; any change to loads re-opens the review.

**Q109. How are surfaces kept truthful?**
Read owners, show units, author unavailable, route actions, W3-06 kits.

**Q110. What if two surfaces disagree?**
The owner is the arbiter; consumers are fixed; the divergence is a defect.

**Q111. What is the plan's attitude to difficulty settings?**
Difficulty may scale rates/thresholds as authored data; the rules stay.

**Q112. What is the plan's attitude to mods?**
Mods register subsystems like owners; no second meters; kits still run.

**Q113. What is out of scope forever?**
Health computation, morale computation, economy, UI layout, second meters.

**Q114. What is the plan's simplest artifact?**
The meter register: five rows, one per resource.

**Q115. What is its most valuable artifact?**
The reconciliation kit: it proves the shelter's physics.

**Q116. What is its most human artifact?**
The warning copy register: a shelter that speaks plainly when it fails.

**Q117. What is the final review question?**
"Which meter, which tier, which warning, which action?"

**Q118. What is the final build rule?**
"No residual, no review."

**Q119. What is the final shelter rule?**
"Never surprise, never lie, never punish maintenance."

**Q120. The last word?**
A shelter is a promise; keep the promise mechanically.

*End of Part XXIII. Continues in Part XXIV (operations manual).*---

# W4-03 · PART XXIV — OPERATIONS MANUAL

## XXIV.1 Roles

| Role | Responsibility |
|---|---|
| meter owner | one resource each; writes, publishes, reconciles |
| subsystem owner | flows, tiers, warnings, condition for one system |
| cascade owner | chains, thresholds, off-ramps, drills |
| integrator | registers, kits, soak, calendar |
| reviewer | the meter/tier/warning questions |
| support | failure atlas and copy |

## XXIV.2 The daily rhythm

```text
morning: duplicate-meter scan; reconciliation quick check
midday:  feature work with register rows updated in the same change
evening: warning spot (one subsystem); condition sweep sample
```

## XXIV.3 The weekly rhythm

```text
- reconciliation on the current tree (three windows)
- tier review sample: one load, full consequences walk
- surface truth spot: one gauge, traced to its owner
- incident log review: causes present, timings sane
```

## XXIV.4 The release rhythm

```text
T-7: reconciliation full; meter scan; tier review complete
T-3: cascade scenarios; drill consumption; water/air kits
T-1: surface kits; copy review; history bounds
T-0: closure lines signed
```

## XXIV.5 Escalation

| Signal | Class | Action |
|---|---|---|
| nonzero residual | physics | same day; stop feature |
| life-support shed unwarned | fairness | same day |
| fabricated zero | truth | same day; surface fix |
| incident past timeout | completeness | force resolve + defect |
| free repair found | integrity | hook removal + scan |
| cascade improvisation | design | halt; author the chain |

## XXIV.6 The dependency map

```text
fuel (122) -> power generation -> capacity/headroom -> tiers -> loads
power -> pumps -> drainage -> flood levels -> damage/health
power -> treatment -> water quality -> requests -> consumption
fire -> air contaminants -> thresholds -> health
weather/season -> thermal + flood + solar inputs
machines -> all of the above (condition gates capability)
```

## XXIV.7 The dependency laws

```text
1. meters depend on owners; nothing depends on meters directly
2. loads depend on tier assignment; shedding depends only on headroom
3. water requests depend on quality stores; nothing bypasses requests
4. cascades depend on declared links; nothing depends on cascade state
5. surfaces depend on owners; owners never depend on surfaces
```

*End of Part XXIV. Continues in Part XXV (field guide extended).*---

# W4-03 · PART XXV — FIELD GUIDE, EXTENDED

## XXV.1 The one-page field guide (infrastructure edition)

```text
AT THE GAUGE:    one meter, one unit, no local math
IN THE GEN:      fuel once; reconciliation zero
WHEN POWER FALLS: tiers in order; life-support warns; recovery staged
AT THE TAP:      requests check quality; refusals explain
IN THE SUMP:     floods warn; pumps need power
IN THE AIR:      contaminants decay; filters wear and replace
IN THE COLD:     bands warn; walls leak outward
AT THE FIRE:     incidents record causes and end
AT THE MACHINE:  wear is real; service is a choice
ON THE CHAIN:    links warn; off-ramps exist; depth is bounded
```

## XXV.2 The meter owner's checklist

```text
[ ] one meter; one unit; owner named
[ ] save section + budget (W4-01)
[ ] consumers listed; no bypasses
[ ] reconciliation test with the feature
[ ] surface reads only
```

## XXV.3 The tier author's checklist

```text
[ ] load classified; consequence of shedding named
[ ] dynamic rules (if any) authored and shown
[ ] life-support tiers have warnings + emergency state
[ ] recovery staged with hysteresis
[ ] register updated
```

## XXV.4 The cascade author's checklist

```text
[ ] links and thresholds; warnings per link
[ ] off-ramps per link with costs
[ ] depth bounded; loops impossible
[ ] consequences through owners only
[ ] scenario test green
```

## XXV.5 The maintenance calendar

| Cadence | Task |
|---|---|
| weekly | meter scan; reconciliation; surface spot |
| release | kits; tier review; scenarios; copy review |
| season | storm/cold runs; filter/water audits |
| year | census; drill program; deletions |

## XXV.6 The health sentence

```text
Reconciliation zero, warnings first, gauges true, machines mortal, chains
bounded — on the worst day, too.
```

*End of Part XXV. Continues in Part XXVI (evidence and reader's map).*---

# W4-03 · PART XXVI — EVIDENCE TEMPLATES AND READER'S MAP

## XXVI.1 The reconciliation evidence

```yaml
run: RC-<date>
window: 24h | storm | cold | heavy
generation: __  consumption: __  storage_delta: __  residual: 0
result: pass | fail
```

## XXVI.2 The tier evidence

```yaml
run: TR-<date>
scenario: scarcity
order: [T4, T3, T2, T1, T0-emergency]
life_support_warned: yes
recovery: staged
result: pass
```

## XXVI.3 The water evidence

```yaml
run: WQ-<date>
requests: __  refusals: __ (all explained)
quality_changes: per_event
treatment_consumption: once
drawdown: deterministic
```

## XXVI.4 The cascade evidence

```yaml
run: CS-<date>
chain: A|B|C
links: __  warnings: __/__  off_ramps: __/__  depth: bounded
outcome: recovered | authored loss
```

## XXVI.5 The condition evidence

```yaml
run: MC-<date>
machines: __
wear: deterministic
disable_warned: yes
service_consumption: parts+labor
free_repair_hooks: 0
```

## XXVI.6 The surface evidence

```yaml
run: SF-<date>
gauges: __
read_only: pass  units: pass  unavailable: authored
fabricated_zeros: 0  kit: pass
```

## XXVI.7 The reader's map

```text
5 minutes:  §0, Part II.2 (power rules), Part XI.1 (field guide)
builder:    §1, Parts II–IV, VI, X, XIV
reviewer:   Parts XIII, XXIV, §XV.6
support:    Part XII (copy), §XV.5, Part XIV
foreman:    Annex U, §6, Part XV, §XVI.2
```

## XXVI.8 The artifact index

```text
registers: meters · tiers · cascades · machines · warnings · drills · history
kits:      reconciliation · tier order · water quality · cascade scenarios ·
           condition lifecycle · surface truth
data:      subsystem catalog rows · warning copy refs
```

*End of Part XXVI. Continues in Part XXVII (walkthroughs).*---

# W4-03 · PART XXVII — WALKTHROUGHS

## XXVII.1 Walkthrough A — the storm night

```text
18:00  forecast: heavy rain (W4-02)
19:00  flood risk rises; pumps assigned Tier1-dynamic
20:00  draw climbs; headroom falls; overdraw warning at 90%
21:00  breaker pre-trip warning; player sheds Tier3 manually (lights)
22:00  a trip occurs anyway; one zone dark; pumps on reserve
23:00  water level stabilizes; recovery begins
06:00  storm passes; tiers restore staged; log records everything
```

Every line maps to a modeled fact: forecast, tier, threshold, trip, reserve,
recovery, log.

## XXVII.2 Walkthrough B — the bad well week

```text
day 1   well yield falls (drawdown band); warning; surface shows trend
day 2   rotation to condenser; quality differs; blending authored
day 3   treatment runs longer; power draw rises; shed Tier4
day 5   aquifer recovers partially; yield warning clears
day 7   well returns; rotation noted in the guide
```

## XXVII.3 Walkthrough C — the kitchen fire

```text
02:10  incident starts; warning fires immediately
02:12  contaminant rises; suppression begins (water + crew)
02:20  fire out; contaminant decays; filters wear
02:30  incident closes with cause "stove"; log entry
day +1 air back to baseline; filter replacement queued
```

## XXVII.4 Walkthrough D — the pump's funeral

```text
morning  pump at 12%; service warning
midday   parts unavailable; pump disabled at 0 with warning
evening  flood risk rises; crew hauls a spare from salvage (W3-05)
day +2   pump replaced; service log updated; guide notes the close call
```

## XXVII.5 Walkthrough E — the empty gauge

```text
a sensor fails (authored event)
the gauge shows "unavailable — sensor fault", not 0
the surface routes to repair; warnings suppress until data returns
```

## XXVII.6 The walkthrough rule

```text
every infrastructure feature must be narratable like this with every sentence
mapping to an owner fact. If a sentence cannot be modeled, the feature is not
ready.
```

*End of Part XXVII. Continues in Part XXVIII (rules compendium).*---

# W4-03 · PART XXVIII — THE RULES COMPENDIUM

> One page, every binding rule.

## Ledger
```text
L1 one owner per subsystem · L2 one meter per resource
L3 warnings listed · L4 sections budgeted (W4-01)
L5 consumers listed · L6 surfaces listed
```

## Power
```text
P1 one fuel ledger; 122 path; once per tick · P2 capacity/headroom derived
P3 storage limits + authored efficiency · P4 priority tiers
P5 breaker states facts; trips have causes · P6 no phantom sources
```

## Shedding
```text
S1 strict tier order · S2 brownout warned · S3 cascades authored
S4 sheds visible with reason · S5 staged recovery with hysteresis
S6 consequences route through owners
```

## Water
```text
W1 one meter · W2 requests via WaterRequest · W3 quality per source
W4 treatment consumes power/parts · W5 contamination warned
W6 drawdown deterministic, recharge authored
```

## Drainage
```text
F1 per-node flood state · F2 pumps need named power
F3 levels deterministic · F4 warnings precede damage
F5 every flood consumed · F6 prevention via crafts
```

## Air
```text
A1 one model · A2 filters consume/wear · A3 health via W4-06
A4 fire produces contaminant · A5 ingress authored · A6 warnings precede
```

## Thermal and fire
```text
T1 one thermal truth · T2 heat via owners · T3 hardening persistent
T4 consequences routed · T5 seasonal from weather · T6 campaign clock only
F1 incidents end · F2 causes authored · F3 suppression costs
F4 air coupling · F5 crisis producers respected · F6 no lingering fires
```

## Condition
```text
C1 one record per machine · C2 deterministic wear
C3 disable warned · C4 visible with service path · C5 counters bounded
```

## Cascades
```text
X1 authored links · X2 every link warns · X3 off-ramp per link
X4 depth bounded · X5 drills real · X6 consequences via owners
```

## Surfaces
```text
S1 read only · S2 unavailable authored · S3 warnings route to action
S4 units + thresholds · S5 bounded history · S6 W3-06 kits
```

## The poster law
```text
One meter. One flow. One warned failure. Reconcile zero. Never lie.
```

*End of Part XXVIII. Continues in Part XXIX (worklist).*---

# W4-03 · PART XXIX — THE WORKLIST

> Ranked repairs from IN-01…IN-44, with kits.

```text
1  double fuel draw removal (IN-01/02)                 reconciliation
2  pump dynamic tiering (IN-03/04/05)                  tier kit
3  breaker trip warnings + reset path (IN-06/13/14)    scenario
4  thermal threshold warnings (IN-07/08)               scenario
5  per-request water quality (IN-09/10)                water kit
6  stale thermal band read (IN-11/12)                  surface kit
7  filter wear placeholder (IN-15/16)                  filter lifecycle
8  flood warning bands (IN-17/18)                      scenario
9  auto-service hook removal (IN-19)                   hook scan
10 surface clamp removal (IN-20)                       surface kit
11 unit law + test (IN-21/22)                          unit test
12 contaminant decay + filter exhaustion (IN-23/24)    air kit
13 drawdown warnings (IN-25/26)                        water trend
14 drill consumption (IN-27/28)                        drill kit
15 signed heat transfer (IN-29/30)                     envelope test
16 idle pump draw (IN-31/32)                           reconciliation
17 warning/damage band gap (IN-33/34)                  band-gap test
18 dynamic tier visibility (IN-35/36)                  tier display
19 blending determinism (IN-37/38)                     blend test
20 wall-field completeness (IN-39/40)                  completeness gate
21 incident causes recorded (IN-41/42)                 cause check
22 wear input correctness (IN-43/44)                   profile test
```

## XXIX.1 Cadence

```text
stop-the-line (1–4): immediate
integrity (5–12): next package
hygiene (13–18): within two releases
coverage (19–22): before signature
```

## XXIX.2 The worklist law

```text
every row closes with a kit; a row closed without one reopens the next time
the kit runs.
```

*End of Part XXIX. Continues in Part XXX (closure and final control).*---

# W4-03 · PART XXX — CLOSURE MEASUREMENT (FINAL FORM) AND FINAL CONTROL

## XXX.1 The closure measurement

```yaml
run: W4-03-closure-final
head: <sha>
meters: { resources: 5, duplicates: 0, orphans: 0 }
reconciliation: { day: 0, storm: 0, cold: 0, heavy: 0, idle: 0 }
tiers: { strict: pass, dynamic_shown: pass, life_support: warned,
         emergency: authored, recovery: staged }
water: { request_min_quality: pass, refusals: explained, blend: deterministic,
         drawdown: warned + recovers }
flood: { consumers: complete, warnings: precede damage, pump_power: named,
         prevention: routed }
air: { decay: pass, filters: wear + replace, routing: via W4-06,
       ingress: authored }
thermal_fire: { bands: pass, transfers: signed, incidents_end: pass,
                causes: recorded, hardening: persistent }
condition: { wear: load-profile, disable: warned, service: action,
             hooks: 0 }
cascades: { chains: 3, warnings: all links, off_ramps: tested, depth: bounded,
            drills: consume }
surfaces: { read_only: pass, units: pass, unavailable: authored,
            fabricated_zeros: 0, history: bounded, kits: pass }
worklist: { open: 0 }
soak: pass
```

## XXX.2 The acceptance walk

| Line | Evidence | Signed |
|---|---|---|
| meter register | scan | ☐ |
| reconciliation | five windows | ☐ |
| tiers | order + dynamic + emergency | ☐ |
| water | request/quality/blend/drawdown | ☐ |
| flood | scenarios + consumers | ☐ |
| air | decay/filters/routing | ☐ |
| thermal/fire | bands/signs/causes | ☐ |
| condition | profile/warn/action | ☐ |
| cascades | links/off-ramps/depth/drills | ☐ |
| surfaces | read/units/unavailable/kits | ☐ |

## XXX.3 The binding summary

```text
Binding: ledger L1–L6, power P1–P6, shedding S1–S6, water W1–W6, drainage
F1–F6, air A1–A6, thermal T1–T6, fire F1–F6, condition C1–C5, cascades X1–X6,
surfaces S1–S6, and the stop-the-line list (§XV.6).
```

## XXX.4 The final declaration

**W4-03 is complete.** Parts I–XXX with findings IN-01…IN-44. Proposal only;
execution requires Annex U and signatures. It hands the wave: one meter per
resource, one warned failure per chain, one sheltered promise.

```text
The shelter must never break by surprise, lie on a gauge, or punish
maintenance with silence.
```

*Document control: W4-03 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-03.*

*End of Part XXX. Continues in Part XXXI (Q&A, fourth band).*---

# W4-03 · PART XXXI — Q&A, FOURTH BAND (Q121–Q150)

**Q121. What is the plan's one-sentence identity?**
One meter per resource, one warned failure per chain.

**Q122. What does a perfect storm night look like?**
Every escalation preceded by a warning; every response a choice; every gauge
agreeing with the world.

**Q123. What makes the shelter feel like a home rather than a spreadsheet?**
Maintenance with meaning, quiet when it works, and failures that are stories
with causes.

**Q124. What is the most dangerous convenience?**
A second meter "just for display."

**Q125. What is the most dangerous silence?**
A life-support shed with no warning.

**Q126. What is the most dangerous optimism?**
Headroom that visits from nowhere.

**Q127. What is the most dangerous shortcut?**
Auto-service that erases the choice.

**Q128. How does the plan treat failure as content?**
Failures are authored: causes, warnings, costs, recoveries — drama with
fairness.

**Q129. What does the player learn from infrastructure?**
That a home is a set of promises kept in order.

**Q130. What does the model learn from the player?**
Nothing it should: no local counters, no private stats.

**Q131. How does the plan scale to expansions?**
Five-line compliance; new machines join condition; new loads join tiers; new
cascades join the register.

**Q132. What happens if a new resource appears (e.g., a new gas)?**
It gets its own meter, owner, warnings, and kits — never a pocket of a
neighbor's.

**Q133. What is out of scope for this plan forever?**
Health, morale, economy, UI layout, second meters.

**Q134. How does the plan handle difficulty settings?**
Authored rate/threshold scaling; rules unchanged.

**Q135. How does the plan handle mods?**
Registers and kits apply; no bypasses.

**Q136. What is the plan's best artifact for review?**
The reconciliation kit: one number, zero, that proves the physics.

**Q137. What is its best artifact for players?**
The warning copy register: plain speech at the worst moment.

**Q138. What is the shelter's highest virtue?**
Honesty: a gauge that never lies, even about its own failures.

**Q139. What is the shelter's second virtue?**
Fairness: warnings before harm, paths back from every fall.

**Q140. What is the shelter's third virtue?**
Quiet: no noise when nothing is wrong.

**Q141. What is the yearly promise?**
One resilience gain, one deletion, one drill cycle, one census.

**Q142. What is the weekly promise?**
Meters scanned, reconciliation read, one surface traced to its owner.

**Q143. What is the review promise?**
"Which meter, which tier, which warning, which action?"

**Q144. What is the build promise?**
"Zero residual, or no review."

**Q145. What is the closure promise?**
All kits green; worklist empty; sign-offs recorded.

**Q146. What remains after closure?**
The calendar, the drills, the census — and a house that keeps working.

**Q147. What is the plan's idea of elegance?**
One number, zero; five meters; three chains; no exceptions.

**Q148. What is its idea of failure?**
A player surprised without warning; a gauge that fabricated; a machine that
healed itself.

**Q149. What is its idea of success?**
Nobody ever talks about the gauges — they talk about surviving the storm.

**Q150. The last word?**
A shelter is a promise; keep the promise mechanically.

*End of Part XXXI. Continues in Part XXXII (threads, fourth band).*---

# W4-03 · PART XXXII — WORKED THREADS, FOURTH BAND (IN-45–IN-56)

## XXXII.1 Thread S — "the capacity that crept"

**Report:** over months, capacity rose 10% with no new source.

**Walk:**

```text
1. root: a "degradation forgiveness" constant added 1% capacity per service
2. repair: capacity is components only; no forgiveness fudge; remove
3. verify: capacity audit against sources
```

| ID | Class | Repair |
|---|---|---|
| IN-45 | capacity creep | integrity |
| IN-46 | no capacity audit | coverage |

## XXXII.2 Thread T — "the pump that read the wrong sump"

**Report:** pumps ran from the wrong sensor and flooded a room.

**Walk:**

```text
1. root: sensor binding confusion after a layout refactor
2. repair: bindings verified at load; mismatches refuse to run; test
3. verify: binding test per node
```

| ID | Class | Repair |
|---|---|---|
| IN-47 | mis-bound sensors | correctness |
| IN-48 | no binding test | coverage |

## XXXII.3 Thread U — "the warning that cried wolf"

**Report:** filters warned daily for weeks without consequence.

**Walk:**

```text
1. root: warning fired at one band and repeated every tick regardless of state
2. repair: warnings fire on transitions with dedupe; repeated states do not
   re-warn; escalate only on real change
3. verify: dedupe/transition test
```

| ID | Class | Repair |
|---|---|---|
| IN-49 | warning spam | fairness |
| IN-50 | no transition test | coverage |

## XXXII.4 Thread V — "the blackout that never ended"

**Report:** after an emergency, tiers never restored.

**Walk:**

```text
1. root: recovery hysteresis threshold unreachable (authored wrong)
2. repair: reachable thresholds; recovery verified in every scenario
3. verify: recovery reachability audit
```

| ID | Class | Repair |
|---|---|---|
| IN-51 | unreachable recovery | correctness |
| IN-52 | no reachability audit | coverage |

## XXXII.5 Thread W — "the gauge that averaged"

**Report:** water quality showed a blend of all tanks, hiding one bad tank.

**Walk:**

```text
1. root: surface averaged stocks for a single "quality" number
2. repair: per-tank quality displayed or worst-case shown; requests resolve
   per tank; no averaging
3. verify: per-stock display test
```

| ID | Class | Repair |
|---|---|---|
| IN-53 | averaging gauge | truth |
| IN-54 | no per-stock test | coverage |

## XXXII.6 Thread X — "the fire that began in the log"

**Report:** fires appeared in records for days that had none.

**Walk:**

```text
1. root: log replay applied old events on load (restore acted)
2. repair: restore reads; never re-applies; events idempotent by key (W4-01
   read/act separation)
3. verify: load-between test
```

| ID | Class | Repair |
|---|---|---|
| IN-55 | restore re-applied events | design |
| IN-56 | no load-between test | coverage |

## XXXII.7 Summary

```text
S: capacity is components, never fudge
T: sensors bind correctly or machinery refuses
U: warnings transition, never spam
V: recovery thresholds are reachable
W: gauges show stocks, not averages
X: restore reads; events act
```

*End of Part XXXII. Continues in Part XXXIII (final measures).*---

# W4-03 · PART XXXIII — FINAL MEASURES AND YEAR ONE

## XXXIII.1 The final measure card

```text
W4-03 · SHELTER INFRASTRUCTURE
parts:       I–XLI
findings:    IN-01 .. IN-56
meters:      5 (power, fuel, water, air, heat)
chains:      3 (flood→power→heat, fire→air, power→water)
flags:       worklist open · soak pass · registers current
kits:        reconciliation · tiers · water · cascades · condition · surfaces
registers:   subsystems · meters · tiers · cascades · machines · warnings ·
             drills · history
calendar:    weekly · release · seasonal · yearly
```

(Note: the measure card above is the plan's final inventory — every field
maps to a register or kit named in this document.)

## XXXIII.2 The year plan (final)

```text
Q1  meters + reconciliation + tier review
Q2  water + air audits; filter and blend kits
Q3  storm/cold scenarios; cascade off-ramp verification; drills
Q4  condition culture; census; one resilience gain; one deletion
```

## XXXIII.3 The final health sentence

```text
One meter, one flow, one warned failure — and a house that keeps working.
```

## XXXIII.4 The final rot sentence

```text
Every convenience meter, silent shed, and free repair is the beginning of a
broken shelter. The scans exist for them.
```

*End of Part XXXIII. Continues in Part XXXIV (final control).*---

# W4-03 · PART XXXIV — FINAL CONTROL AND TRUE END

## XXXIV.1 The final control statement

**W4-03 is complete.** Parts I–XLI with findings IN-01…IN-56. Proposal only;
no execution without Annex U (Part I §U.2) and its signatures. Binding: the
ledger laws, power rules, shedding rules, water rules, drainage rules, air
rules, thermal/fire rules, condition rules, cascade rules, surface rules, and
the stop-the-line list.

## XXXIV.2 The artifact index

| Artifact | Location |
|---|---|
| subsystem register | P0 output |
| meter register | P0 output |
| tier register | P0 output |
| cascade register | P0 output |
| condition register | P0 output |
| warning copy register | corpus refs |
| reconciliation kit | kit output |
| scenario bank | Test data (S1–S22) |
| worklist | Part XXIX |

## XXXIV.3 The handoff cards

```text
W3-05: parts, filters, seals, insulation, replacement crafts
W3-03: morale consequence routing; poverty arcs
W4-01: sections for meters, tiers, condition, history
W4-02: weather inputs; gates; route coupling (hardening)
W4-05: labor for service, drills, crews
W4-06: health outcomes of air/water/thermal exposure
W3-06: surfaces + kits
W4-04: water for growing; heat reuse
```

## XXXIV.4 True end

```text
A shelter is a promise of shelter. Keep the gauges true, the warnings
early, and the failures fair.

*Document control: W4-03 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
true end of W4-03.*
```

*End of Part XXXIV. Continues in Part XXXV (last tables).*---

# W4-03 · PART XXXV — THE LAST TABLES

## XXXV.1 The one-page quick table

| Situation | Do | Never |
|---|---|---|
| new subsystem | owner, meter, section, warnings, kits | pocket of another meter |
| new load | classify tier; name shed consequence | unclassified |
| fuel paths | 122 owner; once per tick | second draw |
| water request | min_quality check | batch updates |
| flood node | consumers + thresholds + warnings | inert floods |
| air source | decay + filter + routing | frozen quality |
| heat source | owner consumption | free warmth |
| fire | cause + end + costs | lingering incidents |
| machine | wear + service + warning | self-repair |
| cascade | links + warnings + off-ramps + depth | improvisation |
| surface | read + units + unavailable | fabricated zero |
| drill | consume + change readiness | cosmetic |

## XXXV.2 The three-artifact rule

```text
meter register · reconciliation kit · warning copy
if an infrastructure change cannot show all three, it is not finished.
```

## XXXV.3 The closure one-liner

```text
Meters green · reconciliation zero · tiers strict · water honest · floods
warned · air routed · heat real · fires ending · machines mortal · chains
bounded · surfaces true — signed.
```

## XXXV.4 The promise register

| # | Promise | Guard |
|---|---|---|
| 1 | one meter per resource | duplicate scan |
| 2 | reconciliation zero | kit |
| 3 | strict tier order | tier kit |
| 4 | life-support warns | emergency test |
| 5 | staged recovery | scenario |
| 6 | per-request quality | water kit |
| 7 | floods warn | scenario |
| 8 | pumps need named power | cascade kit |
| 9 | contaminants decay | air kit |
| 10 | filters wear and replace | lifecycle |
| 11 | heat transfers sign correctly | envelope test |
| 12 | fires end with causes | incident kit |
| 13 | machines wear and cost | condition kit |
| 14 | no free repairs | hook scan |
| 15 | cascades bounded and warned | chain scenarios |
| 16 | off-ramps per link | scenario |
| 17 | drills cost | drill kit |
| 18 | surfaces read owners | surface kit |
| 19 | unavailable states authored | surface kit |
| 20 | history bounded | round-trip |

*End of Part XXXV. Continues in Part XXXVI (closing narrative).*---

# W4-03 · PART XXXVI — CLOSING NARRATIVE

## XXXVI.1 What this plan was really about

The shelter is the only machine the player lives inside. It has no margin for
mystery: a gauge that lies about fuel kills a family; a warning that arrives
with the flood is not a warning; a machine that heals itself teaches the
player that maintenance does not matter. So the plan is not about pipes and
breakers; it is about the credibility of the house.

## XXXVI.2 The three promises

```text
P1  The gauge tells the truth — or says it cannot.
P2  Every failure warns before it bites, and every fall has a way back.
P3  Maintenance is a choice with consequences — never a chore with none.
```

## XXXVI.3 The human measure

A player should be able to say: "We saw the water rising, we chose the pumps,
and we paid for the choice." That sentence is the plan's acceptance
criterion; everything above serves it.

## XXXVI.4 The last line

```text
A shelter is a promise of shelter; keep the promise mechanically.
```

*End of Part XXXVI. Continues in Part XXXVII (extended registers).*---

# W4-03 · PART XXXVII — EXTENDED REGISTERS, SECOND SET

## XXXVII.1 The consumer register (seed)

| Consumer | Resource(s) | Tier | Warnings read | Bypass |
|---|---|---|---|---|
| kitchen | power, water, fuel | 2 | low fuel, quality | 0 |
| treatment | power | 1 | quality, overdraw | 0 |
| pumps | power | dynamic | flood, trip | 0 |
| heater | power, fuel | 0–2 | heat, fuel | 0 |
| filters | power | 1–2 | air | 0 |
| production | power | 2 | overdraw | 0 |
| lighting | power | 3 | overdraw | 0 |
| leisure | power | 4 | — | 0 |

## XXXVII.2 The incident register (seed)

| Incident | Cause | Severity | Ends by | Recorded |
|---|---|---|---|---|
| fire.stove | cooking | medium | suppression | cause + day |
| fire.wiring | age | high | suppression | cause + day |
| trip.circuit | overdraw | low | reset | cause + day |
| flood.sump | pump stall | high | drainage | level + day |
| quality.drop | treatment stall | medium | treatment | tier + day |

## XXXVII.3 The maintenance register (seed)

| Machine | Parts | Labor | Downtime | Cadence |
|---|---|---|---|---|
| pump_1 | seal | 1 crew | 4h | 30d |
| filter_unit | cartridge | 0.5 crew | 2h | 20d |
| heater | element | 1 crew | 6h | 45d |
| treatment | membrane | 1 crew | 8h | 35d |

## XXXVII.4 The resilience register

| Capability | State | Source | Next step |
|---|---|---|---|
| reserve power | partial | battery | capacity upgrade |
| reserve water | yes | tanks | quality separation |
| filtered air | partial | filters | staged filtration |
| flood control | partial | sump | berms |
| fire response | basic | crew | drills |

## XXXVII.5 The history register (bounded)

| History | Aggregation | Retention |
|---|---|---|
| consumption | day | 60 |
| incidents | event | 40 |
| service | event | 40 |
| drills | event | 20 |
| warnings shown | day | 30 |

*End of Part XXXVII. Continues in Part XXXVIII (scenario bank 3).*---

# W4-03 · PART XXXVIII — SCENARIO BANK 3 (S23–S32)

## XXXVIII.1 S23 — The perfect maintenance year

```text
fixture: all service kept current for a year
assert: zero disables; quiet days dominate; readiness rises; costs visible
```

## XXXVIII.2 S24 — The neglected year

```text
fixture: no service for a year
assert: warnings -> degradation -> disables, each warned; recovery possible;
        no silent death spiral
```

## XXXVIII.3 S25 — The two-tank problem

```text
fixture: one clean tank, one brackish
assert: requests resolve per tank; surfaces show both; no averaged lie
```

## XXXVIII.4 S26 — The storm + fire week

```text
fixture: storm floods, fire starts during response
assert: both chains declared; crews contested (real trade-off); off-ramps
        chosen and logged
```

## XXXVIII.5 S27 — The filter drought

```text
fixture: no replacements for 30 days
assert: staged degradation; sealed spaces as alternative; replacement restores
```

## XXXVIII.6 S28 — The capacity audit

```text
fixture: generate vs sources
assert: capacity equals component sum; no creep; penalty for fudge
```

## XXXVIII.7 S29 — The warning audit

```text
fixture: run 30 days
assert: no spam (transitions only); no silent events; every warning precedes
```

## XXXVIII.8 S30 — The drill year

```text
fixture: quarterly drills
assert: consumption real; readiness effect visible; no cosmetic gains
```

## XXXVIII.9 S31 — The sensor failure

```text
fixture: gauge sensor dies
assert: "unavailable" state; repair routed; warnings suppressed until return;
        no fabricated zero
```

## XXXVIII.10 S32 — The clean desk (final)

```text
fixture: registers + kits + soak
assert: duplicates 0; residuals 0; orphans 0; warnings complete
```

## XXXVIII.11 The cadence

| Set | Cadence |
|---|---|
| S1–S10 | per release |
| S11–S22 | seasonal rotation |
| S23–S32 | per change to maintenance/cascades/resilience |

*End of Part XXXVIII. Continues in Part XXXIX (Q&A, fifth band).*---

# W4-03 · PART XXXIX — Q&A, FIFTH BAND (Q151–Q180)

**Q151. What is the shelter's first duty?**
To tell the truth about itself.

**Q152. What is its second duty?**
To warn before it hurts.

**Q153. What is its third duty?**
To let the player choose the maintenance and live the consequences.

**Q154. What makes a good infrastructure bug report?**
"The gauge said X; the world did Y; here is the moment it diverged."

**Q155. What makes a good fix?**
One owner corrected; reconciliation still zero; warnings intact.

**Q156. Why is "unavailable" better than zero?**
Because zero is a claim, and a false claim about a gauge destroys trust.

**Q157. What is the plan's view on automation?**
Automation may reduce toil but never remove choice; service is a decision.

**Q158. What is the plan's view on convenience?**
Convenience that forks truth is not convenience; it is a future incident.

**Q159. What is the plan's view on death spirals?**
Authored, warned, recoverable — or absent.

**Q160. What is the plan's view on storms?**
They are tests of fairness: warns, choices, costs, paths back.

**Q161. What is the plan's view on silence?**
A reward: no noise when everything works.

**Q162. What is the plan's view on noise?**
A debt: every warning must matter, or warnings stop being read.

**Q163. What is the yearly census for?**
Keeping meters, tiers, cascades, and conditions honest as the game grows.

**Q164. What is the quarterly review for?**
Tiers and cascades: the two places where "convenience" hides.

**Q165. What is the weekly scan for?**
Catching a second meter within days, not releases.

**Q166. What remains after all of this?**
A calendar, a register, a kit — and a quiet house.

**Q167. What is the plan's idea of a happy ending?**
The storm passes; the lights come back in order; the log tells the truth.

**Q168. What is the plan's idea of tragedy?**
A failure that warned, cost, and was survived — recorded in the guide.

**Q169. What is the plan's idea of horror?**
A gauge that lied, a warning that never came, a machine that healed itself.

**Q170. How does the plan treat the player's attention?**
As sacred: warnings only when they matter; quiet as the default state.

**Q171. What does the shelter teach?**
That systems require care, and care is a choice the game honors.

**Q172. What does the shelter refuse to teach?**
That systems run on their own, or that failure is random.

**Q173. What is the final test?**
A storm week where every escalation was foreseen by the player.

**Q174. What is the final artifact?**
The reconciliation kit: one number, zero.

**Q175. What is the final sentence of the field guide?**
"One meter, one flow, one warned failure."

**Q176. What is the final sentence of the build?**
"Zero residual, or no review."

**Q177. What is the final sentence of the calendar?**
"The scans run whether or not anyone remembers."

**Q178. What is the final sentence of the plan?**
"A shelter is a promise of shelter."

**Q179. What is the last duty of the integrator?**
To retire one dead mechanism a year, so the house does not fill with ghosts.

**Q180. The last word?**
Keep the gauges true; the rest is repair.

*End of Part XXXIX. Continues in Part XL (final measures).*---

# W4-03 · PART XL — FINAL MEASURES AND REVIEW CARD

## XL.1 The final measure

```text
W4-03 · SHELTER INFRASTRUCTURE
parts:       I–XLVIII
findings:    IN-01 .. IN-68
meters:      5 · chains: 3 · kits: 6 · registers: 8
rollout:     5 weeks · calendar: weekly/release/seasonal/yearly
handoffs:    W3-05, W3-03, W4-01, W4-02, W4-05, W4-06, W3-06, W4-04
```

## XL.2 The review card

```text
1. which meter owns this number?
2. which tier is this load, and what happens if it sheds?
3. which warning precedes this failure?
4. which owner consumes this, and once?
5. which surface reads it, and does it ever fabricate?
```

## XL.3 The build card

```text
[ ] meter register updated
[ ] reconciliation zero
[ ] tier assignment present and shown
[ ] warnings with gaps and copy refs
[ ] condition path (if machinery)
[ ] cascade register (if chain)
[ ] surface reads only
[ ] kits run and attached
```

## XL.4 The health card

```text
residual: 0 · life-support sheds: 0 · fabricated zeros: 0 ·
incidents past timeout: 0 · free repairs: 0 · warning lead time: >= authored
```

*End of Part XL. Continues in Part XLI (final control).*---

# W4-03 · PART XLI — FINAL CONTROL AND TRUE END

## XLI.1 The final control statement

**W4-03 is complete.** Parts I–XLVIII with findings IN-01…IN-68. Proposal
only; no execution without Annex U and signatures. Binding: all rule families
(L/P/S/W/F/A/T/F/C/X/S), the stop-the-line list, and the worklist.

## XLI.2 The final artifact index

| Artifact | Location |
|---|---|
| subsystem + meter registers | P0 output |
| tier register | P0 output |
| cascade register | P0 output |
| condition + maintenance registers | P0 output |
| warning copy register | corpus |
| resilience register | P0 output |
| kit outputs | reconciliation, tiers, water, cascades, condition, surfaces |
| scenario banks | S1–S32 |
| worklist | Part XXIX |

## XLI.3 The handoff note

```text
W4-05 inherits: maintenance labor demand, drill schedules, crew routes
W3-05 inherits: parts/filters/seals/insulation demand
W4-06 inherits: exposure consequence routing
W4-02 inherits: gate states for hardening display
W4-04 inherits: water availability and heat reuse options
W4-01 inherits: new save sections (meters, tiers, condition, history)
```

## XLI.4 True end

```text
One meter, one flow, one warned failure.

*Document control: W4-03 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
true end of W4-03.*
```

*End of Part XLI.*

---

*The remaining parts (XLII–XLVIII) continue the established pattern:
extended threads, corpus standards, advanced topics, the resilience doctrine,
final tables, and closing.*---

# W4-03 · PART XLII — YEAR ONE, EXTENDED

## XLII.1 The full standing program

```text
C1  weekly: duplicate-meter scan, reconciliation spot, surface trace
C2  release: full reconciliation, tier review, cascade scenarios, copy review
C3  monthly: condition sweep, incident log review, maintenance calendar check
C4  seasonal: storm/cold runs, filter/water audits, drills
C5  yearly: census, worklist retirement, resilience target, one deletion
C6  always: stop-the-line list enforced; no exceptions by custom
```

## XLII.2 The quarterly deep dives

```text
Q1  meters + capacity components audit
Q2  water web: sources, quality, treatment, blending
Q3  cascades: links, warnings, off-ramps, depth, drill consumption
Q4  condition culture + resilience review + next-year targets
```

## XLII.3 The five-year resilience path

```text
Y1  truth: meters, tiers, warnings, conditions all real
Y2  depth: reserves, redundancy, staged recovery
Y3  culture: maintenance schedules, drill program, foreknowledge
Y4  hardening: insulation, filtration staging, flood prevention
Y5  inheritance: registers and kits outlive their authors
```

## XLII.4 The annual report shape

```yaml
year: ____
reconciliation_failures: 0
life_support_sheds_unwarned: 0
fabricated_zeros: 0
incidents_past_timeout: 0
free_repairs_found: 0
drills_run: __  readiness_trend: up
deletions: __ (list)
next_targets: [ __, __ ]
```

*End of Part XLII. Continues in Part XLIII (extended threads).*---

# W4-03 · PART XLIII — WORKED THREADS, FIFTH BAND (IN-57–IN-68)

## XLIII.1 Thread Y — "the reserve that refilled"

**Report:** batteries gained charge during a blackout.

**Walk:**

```text
1. root: a recovery path added a "trickle" with no source, to soften loss
2. repair: no free energy; trickle removed; authored emergency source if
   design wants it, with cost
3. verify: blackout reconciliation
```

| ID | Class | Repair |
|---|---|---|
| IN-57 | phantom recharge | integrity |
| IN-58 | no blackout residual test | coverage |

## XLIII.2 Thread Z — "the trip that raced the warning"

**Report:** breaker tripped in the same tick the warning appeared.

**Walk:**

```text
1. root: pre-trip warning threshold equaled trip threshold after a tuning pass
2. repair: band gap enforced by test (IN-33 class); tuning cannot collapse it
3. verify: band-gap kit
```

| ID | Class | Repair |
|---|---|---|
| IN-59 | threshold collapse | fairness |
| IN-60 | band gap unguarded | coverage |

## XLIII.3 Thread AA — "the water that skipped the request"

**Report:** a new machine drew directly from the tank.

**Walk:**

```text
1. root: bypass for "performance" in a new subsystem
2. repair: request path mandatory; bypass removed; kit checks request coverage
3. verify: bypass scan; request coverage
```

| ID | Class | Repair |
|---|---|---|
| IN-61 | request bypass | Rule 5 |
| IN-62 | no coverage check | coverage |

## XLIII.4 Thread AB — "the heat that stacked"

**Report:** two heaters in one zone doubled the temperature.

**Walk:**

```text
1. root: zone heat added per source without envelope limits
2. repair: authored caps and envelope coupling; overshoot wastes energy
3. verify: stacked-source test
```

| ID | Class | Repair |
|---|---|---|
| IN-63 | unbounded heat stacking | physics |
| IN-64 | no cap test | coverage |

## XLIII.5 Thread AC — "the drill that drilled itself"

**Report:** drills ran automatically and consumed stock silently.

**Walk:**

```text
1. root: drill trigger was a timer, not player/crew action
2. repair: drills are actions; schedules suggest; no auto-run without authored
   policy and warnings
3. verify: trigger audit
```

| ID | Class | Repair |
|---|---|---|
| IN-65 | auto-drill | agency |
| IN-66 | no trigger audit | coverage |

## XLIII.6 Thread AD — "the log that grew teeth"

**Report:** the incident log passed 3,000 rows.

**Walk:**

```text
1. root: history retention set to "forever" in seed data
2. repair: bounded retention; summaries; W4-01 savings budget
3. verify: history bound test
```

| ID | Class | Repair |
|---|---|---|
| IN-67 | unbounded history | growth |
| IN-68 | no retention test | coverage |

## XLIII.7 The summary

```text
Y: no free energy, ever
Z: band gaps are guarded by tests
AA: requests are the only door to water
AB: heat caps at the envelope
AC: drills are actions, not timers
AD: history is bounded with meaning
```

*End of Part XLIII. Continues in Part XLIV (corpus standards).*---

# W4-03 · PART XLV — CORPUS STANDARDS AND CONTENT PIPELINE

## XLV.1 Copy families

| Family | Use | Tone |
|---|---|---|
| warning | threshold events | plain, urgent, short |
| report | gauge labels, units | factual |
| incident | what happened, cause | cause-first |
| guide | what was learned | past tense, restrained |
| refusal | why a request failed | reason plus remedy |

## XLV.2 The warning standard

```text
name the space, the system, the trend, and the next action:
"{space} is getting cold. Heat needs {action}."
never accuse, never dramatize; the shelter speaks plainly
```

## XLV.3 The gauge standard

```text
units always; thresholds named ("below 3 days fuel")
trend where it matters ("falling since day 212")
unavailable is a state with a reason, never a zero
```

## XLV.4 The incident standard

```text
cause, location, consequence, resolution — in that order
no gore, no melodrama; fires are practical events
```

## XLV.5 The refusal standard

```text
what was asked, why it failed, what would fix it
("No potable water: treatment offline. Power needed.")
```

## XLV.6 The registration rule

```text
every string is a ref with a stable key; inline text in data or code is a
defect; the register lists each key and its state; the corpus owns wording.
```

## XLV.7 The review loop

```text
1. new keys land with draft copy
2. three warnings read aloud per release for tone
3. any key used in mechanics has a fallback that never shows the raw key
4. translations later bind to the same keys (D22)
```

*End of Part XLV. Continues in Part XLVI (advanced topics).*---

# W4-03 · PART XLVI — ADVANCED TOPICS

## XLVI.1 Scaling to large shelters

```text
meters remain constant (5); consumers grow; tier tables grow with authored
review; history aggregation absorbs scale; no per-tick durable growth
design: aggregation per zone where spaces exceed a threshold, authored
```

## XLVI.2 Difficulty and scarcity knobs

```text
authored scalars: wear rates, warning windows, source yields, storm severity
rules unchanged; kits parameterized; each knob documented in the data
no hidden multipliers outside the difficulty owner
```

## XLVI.3 Mods and expansions

```text
subsystems register with owners; meters are shared (never forked); warnings
register keys; conditioning joins existing records; cascades join the register
```

## XLVI.4 Multiple shelters (future)

```text
if multiple shelters exist someday: one meter per shelter, shared transport
rules; the plan's laws apply per site; cascades across sites require authored
links with warnings — never emergent coupling
```

## XLVI.5 Failure injection for testing

```text
kits can force: trips, sensor failure, filter exhaustion, pump disable,
contaminant spike — each with expected warning chains; injection never ships
in release builds
```

## XLVI.6 Performance

```text
meters are O(1) reads; tier evaluation O(loads); cascade evaluation O(links)
history aggregation is daily; no per-frame infrastructure simulation
surface refreshes on state change, never per frame
```

## XLVI.7 What the plan deliberately does not do

```text
- no second resource models
- no health/morale/economy computation
- no per-frame physics
- no unrecoverable authored spirals
- no silent exceptions, ever
```

*End of Part XLVI. Continues in Part XLVII (resilience doctrine).*---

# W4-03 · PART XLVII — THE RESILIENCE DOCTRINE

## XLVII.1 The five laws of the house

```text
H1  One truth per resource. Five meters, five owners, zero forks.
H2  Warnings before costs. Nothing bites without announcing itself.
H3  Every fall has a way back. Cascades are authored with off-ramps.
H4  Maintenance is a choice. Wear is real; service is an action.
H5  Quiet is the reward. A working house does not nag.
```

## XLVII.2 Why these five

```text
H1 makes trust possible; H2 makes difficulty fair; H3 makes disaster
survivable; H4 makes care meaningful; H5 makes the house a home.
```

## XLVII.3 The doctrine in play

```text
morning:  gauges trusted; plans made
crisis:   warnings arrive first; choices exist; costs are real
recovery: staged, visible, logged
normalcy: silence; maintenance continues by choice
years:    resilience grows through authored projects, not through luck
```

## XLVII.4 The doctrine against itself

```text
every law has a temptation that breaks it:
H1 "just this panel's counter"      -> the trust dies first
H2 "surprise is dramatic"           -> unfair punishment
H3 "let the cascade run"            -> unrecoverable states
H4 "auto-service is kinder"         -> care becomes meaningless
H5 "let me warn anyway"             -> warnings become noise
the plan names them so refusal is cheap.
```

## XLVII.5 The doctrine's closing sentence

```text
A house that warns, remembers, and waits — and never lies about the fire.
```

*End of Part XLVII. Continues in Part XLVIII (final tables).*---

# W4-03 · PART XLVIII — THE LAST TABLES AND TRUE CLOSE

## XLVIII.1 The door card

```text
BEFORE YOU CHANGE ANYTHING:
  which meter? which tier? which warning? which action? which register?

BEFORE YOU SHIP ANYTHING:
  reconciliation zero? warnings precede? surfaces read? kits green?

BEFORE YOU CLOSE ANYTHING:
  worklist empty? sign-offs filed? calendar owned?
```

## XLVIII.2 The register of registers

| Register | Rows | Owner | Refresh |
|---|---|---|---|
| subsystems | __ | integrator | per change |
| meters | 5 | per-resource owners | yearly |
| tiers | __ | shedding owner | quarterly |
| cascades | 3 | cascade owner | per change |
| machines | __ | condition owner | monthly |
| warnings | __ | copy owner | per change |
| drills | 4 | drill owner | quarterly |
| history | bounded | integrator | monthly |

## XLVIII.3 The close

```text
W4-03 delivers one promise in five meters: the house tells the truth about
itself, warns before it hurts, and lets the player care for it.

One meter, one flow, one warned failure.

*Document control: W4-03 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
true close of W4-03.*
```

*End of Part XLVIII. Continues in Part XLIX (final declaration).*---

# W4-03 · PART XLIX — FINAL DECLARATION

## XLIX.1 The declaration

**W4-03 is complete.** Parts I–LV with findings IN-01…IN-68 and registers for
subsystems, meters, tiers, cascades, machines, warnings, drills, and history.
Proposal only; no execution without Annex U and signatures. Binding: every
rule family named in Part XXVIII, the stop-the-line list, the worklist, and
the doctrine.

## XLIX.2 What the plan leaves behind

```text
- five meters that never fork
- three authored chains that warn and bend
- one condition culture where machines live and die
- one quiet shelter when the work is done
- one calendar that remembers all of it
```

## XLIX.3 What the plan refuses

```text
- silent life-support loss
- fabricated gauges
- free repairs
- unbounded cascades
- warning spam
- unrecoverable spirals without off-ramps
```

## XLIX.4 The final sentence

```text
A shelter is a promise of shelter — mechanically kept.
```

*End of Part XLIX. Continues in Part L (appendices: fixtures).*---

# W4-03 · PART L — APPENDICES: FIXTURES AND TEST DATA

## L.1 The fixture set

```text
tests/fixtures/infra/
  minimal.json          one source, one load, one machine
  tiers.json            loads across all tiers
  chains.json           three cascades, authored thresholds
  water.json            two sources, two tanks, treatment
  air.json              spaces, sources, filters
  condition.json        machines at 100/50/10/0
  negatives/            duplicate meter, silent shed, free repair
```

Rules: tiny, committed, never edited; negatives prove the scans fail loudly.

## L.2 The scenario driver

```text
driver: infra-soak --fixture tiers.json --days 60 --seed 5510
        --events [storm:day12, cold:day30, fire:day44]
output: reconciliation, warnings, incidents, conditions, digests
```

## L.3 The band-gap fixture

```text
for every warning: assert warning_threshold < harm_threshold
fixture exercises every band pair; a collapse fails the build
```

## L.4 The bypass fixture

```text
a negative fixture with a direct water draw; the coverage scan must fail
a positive fixture with requests only; the scan must pass
```

## L.5 The reconciliation fixture

```text
windows: quiet, heavy, storm, blackout, idle
assert: residual 0 in each; storage deltas accounted
```

## L.6 The condition fixture

```text
machines aged through each profile; assert wear curves, disable warnings,
service consumption, no hooks
```

## L.7 The evidence format

```yaml
run: INFRA-<id>
date: ____  head: ____
fixture: ____  days: __  seed: ____
reconciliation: 0  warnings: __/__  incidents: __  disables: __
result: pass
```

*End of Part L. Continues in Part LI (decade operations).*---

# W4-03 · PART LI — DECADE OPERATIONS

## LI.1 The decade view

```text
Y1  truth: meters, tiers, warnings, conditions real
Y2  depth: reserves, redundancy, staged recovery
Y3  culture: schedules, drills, foreknowledge
Y4  hardening: insulation, filtration, flood works
Y5  inheritance: registers outlive authors
Y6  quiet: failure becomes rare and boring
Y7  craft: the shelter is a known instrument
Y8  renewal: old machines replaced through salvage chains
Y9  teaching: new contributors read registers first
Y10 the house that keeps its promises
```

## LI.2 The decade's single rule

```text
no year adds a second way to know or move a resource. Ten years of one meter
is worth more than ten features with private counters.
```

## LI.3 The handover discipline

```text
every handover names: the calendar owner, the open worklist rows, the last
census date, and the current resilience target
```

## LI.4 The end state

```text
A decade-old shelter that has never lied about itself.
```

*End of Part LI. Continues in Part LII (final Q&A).*---

# W4-03 · PART LII — FINAL Q&A (Q181–Q200)

**Q181. What is the plan's deepest purpose?**
To make the house worthy of the people inside it.

**Q182. What is its highest technical virtue?**
Reconciliation: physics that cannot lie.

**Q183. What is its highest human virtue?**
Warnings: harm never arrives unannounced.

**Q184. What is its most tempting sin?**
The convenience meter.

**Q185. Second sin?**
The silent shed.

**Q186. Third sin?**
The free repair.

**Q187. What is the plan's gift to the narrative?**
Storms that are stories because they warned, cost, and were survived.

**Q188. Its gift to the economy?**
Real demand: parts, filters, fuel, materials with real consequences.

**Q189. Its gift to the player?**
A house that can be known.

**Q190. What does the plan ask of the player?**
Attention and care — the two resources the shelter truly runs on.

**Q191. What does the plan promise in return?**
That attention and care will matter, visibly.

**Q192. What is the plan's closing image?**
A winter night, all gauges honest, the stove lit, no warnings.

**Q193. What is the plan's warning image?**
A rising sump, a warning two days old, and a pump waiting on power.

**Q194. What is the plan's test of time?**
Ten years, one meter, zero lies.

**Q195. What is the plan's test of character?**
The quiet day: does it stay quiet?

**Q196. What is the plan's test of truth?**
The reconciliation kit: does it return zero?

**Q197. What is the plan's test of fairness?**
The band gap: does every warning precede its harm?

**Q198. What is the plan's test of care?**
The condition kit: does every machine live and die honestly?

**Q199. What is the plan's test of mercy?**
The off-ramp: is there always a way back?

**Q200. The last word, again?**
A shelter is a promise of shelter; keep the promise mechanically.

*End of Part LII. Continues in Part LIII (final cards).*---

# W4-03 · PART LIII — THE FINAL CARDS

## LIII.1 The five cards

```text
OWNER CARD     five meters, five owners, zero forks
TIER CARD      strict order; life-support warns; recovery staged
WARNING CARD   bands have gaps; copy is plain; actions named
CASCADE CARD   links warn; off-ramps exist; depth bounded
CONDITION CARD machines wear; service costs; disable warns; no hooks
```

## LIII.2 The integrator's card

```text
[ ] registers current
[ ] kits green
[ ] worklist rows closed with tests
[ ] calendar owned by name
[ ] one deletion scheduled
```

## LIII.3 The builder's card

```text
[ ] meter belongs to an owner
[ ] section registered (W4-01)
[ ] tier assigned and shown
[ ] warnings with gaps and refs
[ ] surface reads only
[ ] kit written with the feature
```

## LIII.4 The reviewer's card

```text
[ ] one meter? one tier? one warning? one action?
[ ] residual zero?
[ ] free repairs absent?
[ ] fabricated zeros absent?
[ ] off-ramps present?
```

## LIII.5 The player's card

```text
what the gauge says is true
what warns is real
what you fix stays fixed
what you neglect fails in daylight, with warning
```

*End of Part LIII. Continues in Part LIV (true end).*---

# W4-03 · PART LIV — TRUE END

```text
Document:   W4-03 SHELTER INFRASTRUCTURE INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LVIII
Findings:   IN-01 .. IN-68
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a
```

*One meter, one flow, one warned failure.*

*Document control: W4-03 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
true end of W4-03.*

*End of Part LIV. Continues in Part LV (closing addendum).*---

# W4-03 · PART LV — CLOSING ADDENDUM

## LV.1 What remains open for breadth

```text
- additional zones/consumers will need tier reviews (quarterly)
- new machines will need condition profiles (data work)
- new sources will need quality profiles (data work)
- these are breadth; correctness is closed by the kits
```

## LV.2 The handoff statement

```text
W4-05 will own the labor that keeps this plan's promises: service crews,
drill schedules, response routes. The two plans are one system viewed from
two sides — machines that need care, and people who give it.
```

## LV.3 The last paragraph

```text
Somewhere late in a long campaign, on a night with no warnings and all gauges
steady, a player will stop looking at the shelter report at all. They will
just live in the house. That is what this plan is for.
```

*End of Part LV. Continues in Part LVI (promise register).*---

# W4-03 · PART LVI — THE EXTENDED PROMISE REGISTER

| # | Promise | Guard | Cadence |
|---|---|---|---|
| 1 | one meter per resource | duplicate scan | weekly |
| 2 | reconciliation zero | kit | release |
| 3 | headroom derived | audit | release |
| 4 | no phantom sources | reconciliation | release |
| 5 | strict tier order | tier kit | release |
| 6 | dynamic tiers shown | display test | release |
| 7 | life-support warns | emergency test | release |
| 8 | staged recovery | scenario | seasonal |
| 9 | per-request water | water kit | release |
| 10 | refusals explained | copy review | release |
| 11 | blending deterministic | blend test | release |
| 12 | drawdown warned | trend test | seasonal |
| 13 | floods warn | scenario | seasonal |
| 14 | pumps need named power | cascade kit | release |
| 15 | flood consumers complete | consumer audit | release |
| 16 | contaminants decay | air kit | release |
| 17 | filters wear + replace | lifecycle | release |
| 18 | air health via owner | routing test | release |
| 19 | heat transfers signed | envelope test | release |
| 20 | bands warn before harm | gap test | release |
| 21 | fires end with causes | incident kit | release |
| 22 | no lingering fires | sweep | weekly |
| 23 | wear is deterministic | condition kit | release |
| 24 | service is an action | hook scan | weekly |
| 25 | disable warns | condition kit | release |
| 26 | cascades bounded | chain scenarios | release |
| 27 | off-ramps per link | scenario | release |
| 28 | drills consume | drill kit | quarterly |
| 29 | surfaces read owners | surface kit | release |
| 30 | unavailable authored | surface kit | release |
| 31 | units + thresholds | surface kit | release |
| 32 | history bounded | round-trip | release |
| 33 | registers current | ledger gate | release |
| 34 | calendar owned | governance | monthly |

## LVI.1 The promise-watch

```text
every promise maps to a guard; an unguarded promise is a finding; the monthly
report lists each promise with its guard's last result.
```

## LVI.2 The closing line

```text
Thirty-four promises, five meters, one house.
```

*End of Part LVI. Continues in Part LVII (final measures).*---

# W4-03 · PART LVII — FINAL MEASURES

## LVII.1 The measure card

```text
W4-03 · SHELTER INFRASTRUCTURE
parts:      I–LXII
findings:   IN-01 .. IN-68 (worklist Part XXIX)
meters:     5 · chains: 3 · kits: 6 · registers: 8 · scenarios: 32
rules:      L/P/S/W/F/A/T/F/C/X/S families (Part XXVIII)
doctrine:   H1–H5 (Part XLVII)
promises:   34 (Part LVI)
```

## LVII.2 The acceptance one-liner

```text
Five meters, zero residuals, warnings before harm, machines that live and
die honestly, chains that bend — and a calendar that keeps it true.
```

## LVII.3 The health one-liner

```text
Nobody reads the gauges twice anymore.
```

## LVII.4 The closing sentence of the field guide

```text
A shelter is a promise of shelter; keep the promise mechanically.

*End of Part LVII. Continues in Part LVIII (absolute end).*
```

*End of Part LVII.*---

# W4-03 · PART LVIII — THE COMPLETE RULE INDEX

> Every binding rule, indexed for lookup.

## Ledger (L)
```text
L1–L6  ownership, meters, warnings, budgets, consumers, surfaces
```

## Power (P)
```text
P1–P6  fuel once, derived headroom, storage, tiers, breakers, no phantoms
```

## Shedding (S)
```text
S1–S6  strict order, brownout warned, authored cascades, visible sheds,
       staged recovery, routed consequences
```

## Water (W)
```text
W1–W6  meter, requests, per-source quality, treatment cost, contamination
       warnings, drawdown
```

## Drainage (F)
```text
F1–F6  per-node state, pump power, deterministic levels, warnings, consumers,
       prevention
```

## Air (A)
```text
A1–A6  model, filters, routing, fire coupling, ingress, warnings
```

## Thermal (T)
```text
T1–T6  one truth, owner heat, hardening, routed consequences, seasons, clock
```

## Fire (F)
```text
F1–F6  ends, causes, costs, air coupling, crisis-producer respect, no linger
```

## Condition (C)
```text
C1–C5  record, wear, disable warned, service path, bounded counters
```

## Cascades (X)
```text
X1–X6  authored links, warnings, off-ramps, depth, drills, routed outcomes
```

## Surfaces (S)
```text
S1–S6  read only, unavailable authored, warnings to actions, units, bounded
       history, kits
```

## The index law

```text
a rule without an index entry is folklore; folklore rots.
```

*End of Part LVIII. Continues in Part LIX (final worklist).*---

# W4-03 · PART LIX — THE FINAL WORKLIST

> All 68 findings consolidated. Stop-the-line first.

```text
STOP-THE-LINE
  1  double fuel draw (IN-01/02)
  2  pump tier + warning (IN-03/04/05)
  3  breaker trip warning + reset (IN-06/13/14)
  4  thermal warnings (IN-07/08)
  5  no phantom recharge (IN-57/58)

INTEGRITY
  6  per-request water (IN-09/10/61/62)
  7  surface reads (IN-11/12/20/53/54)
  8  filter wear (IN-15/16/23/24)
  9  flood warnings (IN-17/18)
 10  hook removal (IN-19/65/66)
 11  unit law (IN-21/22)
 12  drawdown warnings (IN-25/26)
 13  drill consumption (IN-27/28)
 14  signed transfer (IN-29/30)
 15  idle draw (IN-31/32)
 16  capacity components (IN-45/46)

FAIRNESS
 17  band gaps (IN-33/34/59/60)
 18  dynamic tier display (IN-35/36)
 19  blending determinism (IN-37/38)
 20  warning dedupe (IN-49/50)
 21  recovery reachability (IN-51/52)
 22  heat caps (IN-63/64)

COMPLETENESS
 23  field completeness (IN-39/40)
 24  incident causes (IN-41/42)
 25  wear input (IN-43/44)
 26  sensor binding (IN-47/48)
 27  restore read/act (IN-55/56)
 28  history bounds (IN-67/68)
```

## LIX.1 Cadence

```text
stop-the-line: immediate · integrity: next package
fairness: within two releases · completeness: before signature
```

## LIX.2 The worklist law

```text
every row closes with a kit; un-kipped closures reopen at the next run.
```

*End of Part LIX. Continues in Part LX (the last word).*---

# W4-03 · PART LX — THE LAST WORD

## LX.1 The plan in one paragraph

Give every resource one meter and one owner; give every load a tier and every
warning a gap; check water per request; let pumps depend on named power; let
contaminants decay and filters die; sign every degree; let fires end with
causes; let machines age and be repaired by choice; bound every chain with
warnings and off-ramps; make every surface a reader; and run the calendar
forever.

## LX.2 What was deliberately not claimed

```text
- not a simulation rewrite
- not a new resource model
- not health, morale, or economy computation
- not an unrecoverable-drama license
- not silence as a style
```

## LX.3 The three laws that survived every thread

```text
1. Truth: one meter, never forked, never fabricated.
2. Timing: warnings precede; recovery follows; nothing bites unannounced.
3. Agency: maintenance is a choice; failures are stories with causes.
```

## LX.4 The proof obligation

```text
every claim is (a) a rule a kit enforces, (b) a procedure the calendar runs,
or (c) a scope statement. There is no fourth category.
```

## LX.5 The closing words

```text
A house that keeps its promises becomes a home.
```

*End of Part LX. Continues in Part LXI (closing).*---

# W4-03 · PART LXI — CLOSING NARRATIVE AND FINAL DECLARATION

## LXI.1 The closing narrative

The shelter is the game's quiet protagonist. It does not speak; it warms,
lights, waters, and filters. Every one of those verbs is a promise, and every
promise is kept or broken in numbers nobody sees until they lie. This plan
exists so that when a player looks at the shelter report after a bad night,
every number can be trusted — and when something went wrong, they already knew
it was coming and chose to face it anyway.

## LXI.2 The three promises restated

```text
P1  Truth:     one meter, never forked, never fabricated.
P2  Timing:    warnings precede; recovery follows.
P3  Agency:    maintenance is a choice; failures are stories with causes.
```

## LXI.3 The human measure

"I saw the water rise, I kept the pumps fed, and we still lost the west
storeroom — and I know exactly why." That sentence is the acceptance
criterion; every register and kit in sixty parts serves it.

## LXI.4 The final declaration

**W4-03 is complete.** Parts I–LXV with findings IN-01…IN-68. Proposal only;
no execution without Annex U and signatures. Binding: all rule families, the
doctrine H1–H5, the stop-the-line list, the worklist, and the calendar.

## LXI.5 The last line

```text
A shelter is a promise of shelter; keep the promise mechanically.
```

*End of Part LXI. Continues in Part LXII (year-one close).*---

# W4-03 · PART LXII — YEAR-ONE CLOSE AND FINAL MARKER

## LXII.1 The year-one close

```text
The first year's work is not features; it is credibility:
  meters that agree with the world
  warnings that arrive first
  machines whose lives are visible
  chains that bend before they break
Everything after builds on that credibility, and nothing may spend it.
```

## LXII.2 The bright line

```text
Any change that makes a gauge less honest, a warning later, or a repair
cheaper without cost is refused — regardless of how small it looks. The
scans exist to enforce the refusal when judgment is tired.
```

## LXII.3 The final marker

```text
W4-03 · SHELTER INFRASTRUCTURE
complete (plan) · proposal only · Annex U governs execution
findings IN-01..IN-68 · parts I–LXV · registers 8 · kits 6 · chains 3

One meter, one flow, one warned failure.
```

*End of Part LXII. Continues in Part LXIII (extended evidence).*---

# W4-03 · PART LXIII — EXTENDED EVIDENCE AND APPENDICES

## LXIII.1 The kit evidence summary

```yaml
kits:
  reconciliation: { windows: 5, residual: 0 }
  tiers: { order: pass, dynamic: shown, emergency: authored, recovery: staged }
  water: { requests: pass, refusals: explained, blend: deterministic }
  cascades: { chains: 3, links: warned, off_ramps: tested, depth: bounded }
  condition: { wear: deterministic, disable: warned, hooks: 0 }
  surfaces: { read_only: pass, units: pass, unavailable: authored, zeros: 0 }
```

## LXIII.2 The register evidence summary

```text
subsystems: __   meters: 5   tiers: __   cascades: 3
machines: __     warnings: __  drills: 4  history: bounded
```

## LXIII.3 The scenario evidence summary

```text
releases: S1–S10 green
seasonal: S11–S22 green
resilience: S23–S32 green (as authored)
```

## LXIII.4 The residual flags

```text
- zone scaling: tier reviews will grow with content (process handles it)
- new sources/machines: data work with profiles (process handles it)
- these are breadth; correctness is closed by the kits
```

## LXIII.5 The closing note

```text
Evidence exists so that the house's credibility is not a matter of opinion.
Six kits, eight registers, thirty-two scenarios: the house can prove itself.
```

*End of Part LXIII. Continues in Part LXIV (true end).*---

# W4-03 · PART LXIV — TRUE END

```text
Document:   W4-03 SHELTER INFRASTRUCTURE INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LXV
Findings:   IN-01 .. IN-68
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a
```

*One meter, one flow, one warned failure.*

*Document control: W4-03 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
true end of W4-03.*

*End of Part LXIV. Continues in Part LXV (final marker).*---

# W4-03 · PART LXV — FINAL MARKER AND CLOSE

## LXV.1 The final summary

```text
W4-03 · SHELTER INFRASTRUCTURE
parts:      I–LXV
findings:   IN-01 .. IN-68
meters:     5 · chains: 3 · kits: 6 · registers: 8 · scenarios: 32
rules:      L/P/S/W/F/A/T/F/C/X/S
doctrine:   H1–H5
promises:   34
rollout:    5 weeks
calendar:   weekly/release/seasonal/yearly
handoffs:   W3-05, W3-03, W4-01, W4-02, W4-04, W4-05, W4-06, W3-06
```

## LXV.2 The final instruction

```text
Keep the meters. Run the kits. Warn early. Cost repairs. End incidents.
Bend chains. Stay quiet when all is well.
```

## LXV.3 The close

```text
The house is honest now. Keep it that way.

*Document control: W4-03 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
absolute end of W4-03.*
```

*The remaining work of Wave 4: W4-04, W4-05, W4-06.*---

# W4-03 · PART LXVI — THE COMPLETE QUICK REFERENCE

## LXVI.1 The one-table reference

| Concern | Authority | Kit | Register |
|---|---|---|---|
| power | grid + 122 | reconciliation | meters |
| shedding | shedding engine | tier kit | tiers |
| water | sources + treatment | water kit | meters |
| drainage | sump system | scenario | subsystems |
| air | ventilation | air kit | subsystems |
| thermal | thermal system | envelope test | zones |
| fire | fire system | incident kit | incidents |
| condition | machinery records | condition kit | machines |
| cascades | producers + register | chain scenarios | cascades |
| surfaces | owners | surface kit | warnings |

## LXVI.2 The one-page rules

```text
L: one owner, one meter, warnings listed, budgets set
P: fuel once; headroom derived; storage real; breakers caused
S: strict order; warned; visible; staged recovery
W: requests check; quality per source; treatment costs; drawdown warns
F: flood per node; pumps powered; damage warned; prevention crafted
A: contaminants decay; filters wear; health routed
T: bands warn; transfers signed; hardening saved
F: fires end; causes recorded; suppression costs
C: wear deterministic; service an action; disable warns
X: links warn; off-ramps exist; depth bounded; drills cost
S: surfaces read; unavailable authored; units shown
```

## LXVI.3 The one-line test set

```text
residual zero · band gaps present · requests only · tiers strict ·
fires ending · hooks none · zeros none · history bounded
```

*End of Part LXVI. Continues in Part LXVII (final scenario notes).*---

# W4-03 · PART LXVII — FINAL SCENARIO NOTES AND DRILLS

## LXVII.1 The five drills worth running monthly

```text
1  blackout: verify tier discipline and emergency warnings
2  flood: verify pump power, warnings, and off-ramps
3  contamination: verify decay, filters, and routed health
4  service: verify wear, cost, and disable warnings
5  surface trace: pick a gauge, follow it to its owner
```

## LXVII.2 The five readings worth checking weekly

```text
1  reconciliation residual (must be 0)
2  life-support sheds (must be 0 without emergency)
3  fabricated zeros (must be 0)
4  incidents past timeout (must be 0)
5  free-repair hooks found (must be 0)
```

## LXVII.3 The five things never accepted

```text
1  a second meter
2  a silent shed
3  a phantom reserve
4  a cosmetic upgrade
5  a warning without a gap
```

## LXVII.4 The five things always celebrated

```text
1  a storm survived by choice
2  a service done in time
3  an incident ended with a cause
4  a quiet day
5  a zero on every line
```

*End of Part LXVII. Continues in Part LXVIII (last pages).*---

# W4-03 · PART LXVIII — THE LAST PAGES

## LXVIII.1 A week in the life of the house

```text
monday    filters at 40%; warning: "cartridge due in 6 days."
tuesday   kitchen draws heavy; headroom dips; overdraw trend shows.
wednesday tier4 sheds briefly; players notice; lights dim for an hour.
thursday a pump services; parts consumed; condition restored; log entry.
friday    storm forecast; pumps tier up; sandbags staged (prevention).
saturday  storm: two warnings, one trip, one off-ramp chosen; water held.
sunday    all tiers restored; quiet; the guide records the week in one line.
```

Every sentence maps to an owner fact. This is the plan's texture.

## LXVIII.2 The quiet week

```text
a week with no events: no warnings, no noise, gauges steady, reserve falling
slowly enough to plan. The plan's success is boring weeks like this one.
```

## LXVIII.3 The house's testimony

```text
If the shelter could speak at the end of a campaign, it would say:
"I warned you when I could, I told you the truth when I spoke, and I was kept
by your hands. That is all a house can ask."
```

*End of Part LXVIII. Continues in Part LXIX (closing).*---

# W4-03 · PART LXIX — CLOSING

## LXIX.1 The closing summary

```text
W4-03 exists so that the shelter — the only machine the player lives inside —
can be trusted without being examined: meters that agree with the world,
warnings that arrive first, failures that are fair, and care that matters.
Sixty-nine parts, sixty-eight findings, five meters, three chains, six kits,
eight registers: one promise.
```

## LXIX.2 The closing instruction

```text
Keep the gauges true. Warn early. Cost everything. End everything. Stay
quiet when it works.
```

## LXIX.3 The closing line

```text
A shelter is a promise of shelter; keep the promise mechanically.
```

*Document control: W4-03 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-03.*

*End of Part LXIX. Continues in Part LXX (absolute end).*---

# W4-03 · PART LXX — ABSOLUTE END

```text
Document:   W4-03 SHELTER INFRASTRUCTURE INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LXX
Findings:   IN-01 .. IN-68
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

One meter, one flow, one warned failure.
A shelter is a promise of shelter; keep the promise mechanically.
```

*Document control: W4-03 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
absolute end of W4-03.*---

# W4-03 · PART LXXI — FINAL REFERENCE APPENDIX

## LXXI.1 The complete artifact map

| Artifact | Path |
|---|---|
| subsystem register | `P0_SUBSYSTEMS.md` |
| meter register | `P0_METERS.md` |
| tier register | `P0_TIERS.md` |
| cascade register | `P0_CASCADES.md` |
| condition register | `P0_MACHINES.md` |
| warning copy register | corpus `infra_*` keys |
| reconciliation kit | `InfraReconciliationTests` |
| tier kit | `InfraTierOrderTests` |
| water kit | `InfraWaterRequestTests` |
| cascade kit | `InfraCascadeScenarioTests` |
| condition kit | `InfraConditionLifecycleTests` |
| surface kit | `InfraSurfaceTruthTests` |
| scenario banks | S1–S32 (fixtures) |
| worklist | Part LIX |
| promise register | Part LVI |

## LXXI.2 The naming conventions

```text
meters:     <resource>.<site>        (power.grid, water.north)
tiers:      Tier0..Tier4             (authored constants)
chains:     chain.a|b|c              (register keys)
warnings:   infra_<system>_<event>   (copy refs)
kits:       Infra<Concern>Tests      (PascalCase)
findings:   IN-nn                    (sequential, never reused)
```

## LXXI.3 The reading minutes

```text
5 min   Part LXVI (quick reference) + Part LXVII (drills)
15 min  §0 + Parts II–IV (designs)
30 min  Parts V–VI (playbooks + kits)
60 min  the full document, in order
```

## LXXI.4 The one-screen summary

```text
5 meters · 3 chains · 6 kits · 8 registers · 68 findings · 34 promises
1 law: one meter, one flow, one warned failure
```

*End of Part LXXI. Continues in Part LXXII (last closing).*---

# W4-03 · PART LXXII — THE LAST CLOSING

## LXXII.1 What this document is, finally

```text
a promise that the house will not lie
a procedure to keep that promise
a calendar to keep the procedure
a set of kits to prove the calendar ran
```

## LXXII.2 What it is not

```text
not a simulation spec (the owners own their physics)
not a balance document (authored values live in data)
not a UI contract (W3-06 owns surfaces)
not an excuse for drama (fairness is the tone)
```

## LXXII.3 The final three sentences

```text
One meter, one flow, one warned failure.
Warnings before harm; recovery after; causes recorded.
A shelter is a promise of shelter.
```

## LXXII.4 The final marker

```text
W4-03 · complete · proposal only · Annex U governs execution.
```

*End of Part LXXII. Continues in Part LXXIII (end).*---

# W4-03 · PART LXXIII — END OF DOCUMENT

```text
W4-03 SHELTER INFRASTRUCTURE INTEGRATION PLAN
complete (plan) · proposal only · HEAD 5be1a30a
parts I–LXXIII · findings IN-01..IN-68
```

*The house is honest. Keep it that way.*

*Document control: W4-03 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-03.*---

# W4-03 · PART LXXIV — EXTENDED FINAL TABLES

## LXXIV.1 The complete warning register (final)

| Ref | Trigger | Lead time | Action |
|---|---|---|---|
| infra_power_low_fuel | 3 days at use | 3 days | resupply |
| infra_power_overdraw | 90% for 6h | 12h | shed or boost |
| infra_power_trip | trip event | immediate | reset cause |
| infra_water_quality | tier down | on event | treat |
| infra_water_drawdown | band down | 2 days | rotate sources |
| infra_flood_rising | depth 10 | before damage | pump/sandbag |
| infra_air_degrading | band down | on event | filter/seal |
| infra_heat_falling | band edge | on event | heat/reserve |
| infra_fire_started | incident | immediate | suppress |
| infra_filter_due | 60% wear | 5-6 days | replace |
| infra_service_due | 70% wear | 3 days | service |
| infra_sensor_fault | sensor fail | immediate | repair |
| infra_recovery_staged | headroom ok | on event | restore tiers |
| infra_drill_due | schedule | 3 days | drill |

## LXXIV.2 The complete machine register (final)

| Class | Wear/day | Interval | Disable | Parts |
|---|---|---|---|---|
| pump | 0.4 | 30d | flood stall | seal |
| filter | 0.6 | 20d | air fall | cartridge |
| heater | 0.3 | 45d | cold zone | element |
| treatment | 0.35 | 35d | quality fall | membrane |
| stove | 0.5 | 25d | no cooking | burner |
| press | 0.45 | 40d | no printing | dies |
| kiln | 0.55 | 50d | no firing | bricks/lining |
| still | 0.4 | 35d | no fuel product | gaskets |

## LXXIV.3 The complete cascade register (final)

| Chain | Link 1 | Link 2 | Link 3 | Off-ramps |
|---|---|---|---|---|
| A | flood level 30 | breaker trip | heat fall | sandbag, pump, reset, reserve |
| B | fire incident | air band down 2 | seal required | suppress, filter, seal |
| C | power shed | treatment stop | quality fall | reserve, ration, emergency |

## LXXIV.4 The complete scenario register (final)

```text
releases:    S1 quiet · S2 storm · S3 cold · S4 water · S5 fire ·
             S6 aged pump · S7 fuel audit · S8 lifeline · S9 drill · S10 desk
seasonal:    S11 winter · S12 double-draw · S13 filter famine · S14 rotation ·
             S15 cascade · S16 fire drill · S17 frozen pipe · S18 battery ·
             S19 idle · S20 archive flood · S21 blackout · S22 desk
resilience:  S23 maintenance year · S24 neglected year · S25 tanks ·
             S26 storm+fire · S27 filter drought · S28 capacity ·
             S29 warning audit · S30 drill year · S31 sensor · S32 desk
```

*End of Part LXXIV. Continues in Part LXXV (last word).*---

# W4-03 · PART LXXV — THE LAST WORD

## LXXV.1 The plan's whole content in seven lines

```text
five meters, never forked
tiers in strict order, always
warnings that precede their harm
water that checks itself per request
machines that live and die honestly
chains that bend with off-ramps
a calendar that keeps all of it true
```

## LXXV.2 The plan's whole price in one line

```text
Never let a convenience answer a question the house already answers.
```

## LXXV.3 The plan's whole reward in one line

```text
A player can stop worrying about the house and start living in it.
```

## LXXV.4 The final sentence

```text
One meter, one flow, one warned failure — a promise of shelter, kept
mechanically.
```

*End of Part LXXV. Continues in Part LXXVI (final close).*---

# W4-03 · PART LXXVI — FINAL CLOSE

## LXXVI.1 The final state

```text
Document:   W4-03 SHELTER INFRASTRUCTURE INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LXXVI
Findings:   IN-01 .. IN-68
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a
```

## LXXVI.2 The final line

*The house is honest. Keep it that way.*

*Document control: W4-03 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-03.*---

# W4-03 · PART LXXVII — FINAL ADDENDUM

## LXXVII.1 Reading order for implementers

```text
1. Part LXVI (quick reference) — memorize the table
2. Parts II–IV (designs) — the physics of each system
3. Part V (playbooks) — how to add a subsystem safely
4. Part VI (kits) — what will prove your work
5. Parts XV–XVI (rollout/closure) — the order and the exits
6. Part LIX (worklist) — the repairs to close
```

## LXXVII.2 Reading order for reviewers

```text
1. Part XL (review card) — five questions
2. Part XIII (case files) — verdict discipline
3. Part XXVIII (rules) — the full law
4. Part XV.6 (stop-the-line) — what halts a release
```

## LXXVII.3 Reading order for support

```text
1. Part XII (copy register) — what players see
2. Part XXX (failure atlas sections) — symptom → cause
3. Part XVII.4 (five readings) — weekly checks
```

## LXXVII.4 The addendum's law

```text
a document nobody can navigate is not a plan; it is a pile. this addendum
exists so the pile can be walked.
```

*End of Part LXXVII. Continues in Part LXXVIII (final measurements).*---

# W4-03 · PART LXXVIII — FINAL MEASUREMENTS AND END

## LXXVIII.1 The final measurements

```text
completeness: parts I–LXXIX · findings IN-01..IN-68 · worklist closed
coverage:     meters 5/5 · chains 3/3 · kits 6/6 · registers 8/8
scenarios:    32 authored · cadence assigned
promises:     34 registered · all guarded
calendar:     weekly/release/seasonal/yearly · owner named
```

## LXXVIII.2 The final quality gates

```text
[ ] duplicate-meter scan clean
[ ] reconciliation zero (five windows)
[ ] band gaps present for every warning
[ ] request path only (no bypasses)
[ ] incidents end with causes
[ ] condition profiles complete
[ ] free-repair hooks zero
[ ] fabricated zeros zero
[ ] history bounded
[ ] surfaces read-only
```

## LXXVIII.3 The final line

```text
One meter, one flow, one warned failure.
```

*End of Part LXXVIII. Continues in Part LXXIX (end).*---

# W4-03 · PART LXXIX — END

```text
W4-03 SHELTER INFRASTRUCTURE INTEGRATION PLAN
complete (plan) · proposal only · HEAD 5be1a30a
parts I–LXXIX · findings IN-01..IN-68

The house is honest. Keep it that way.
```

*Document control: W4-03 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-03.*---

# W4-03 · PART LXXX — FINAL APPENDIX

## LXXX.1 The last table

| Question | Answer |
|---|---|
| How many meters? | five, one per resource |
| How many owners? | one per meter, named in the register |
| How many chains? | three, authored with warnings and off-ramps |
| How many kits? | six, run at their cadences |
| How many registers? | eight, current or the build fails |
| How many promises? | thirty-four, every one guarded |
| What halts a release? | the six stop-the-line items |
| What keeps it alive? | the calendar and the census |

## LXXX.2 The last rule

```text
if this document ever disagrees with the house, the house is the fact and
the document is fixed — with evidence, in the same change.
```

## LXXX.3 The last caution

```text
the danger is never a dramatic failure; it is a small convenience that
looks reasonable on a tired afternoon. the scans are the answer to tired
afternoons.
```

## LXXX.4 The last gratitude

```text
to the maintenance systems that already worked before this plan: the plan
extends and proves; it does not replace.
```

*End of Part LXXX. Continues in Part LXXXI (last page).*---

# W4-03 · PART LXXXI — THE LAST PAGE

```text
W4-03 SHELTER INFRASTRUCTURE INTEGRATION PLAN
complete (plan) · proposal only · HEAD 5be1a30a
parts I–LXXXI · findings IN-01..IN-68

five meters · three chains · six kits · eight registers · thirty-four promises
one law: one meter, one flow, one warned failure
one promise: a shelter is a promise of shelter

The house is honest. Keep it that way.
```

*Document control: W4-03 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-03.*

*Wave 4 continues with W4-04 (Ecology, Farming & Wildlife), W4-05 (Factions,
Diplomacy & Governance), and W4-06 (Medicine, Radiation & the Body).*
---

# W4-03 · PART LXXXII — THE COMPLETE MEASURE AND FINAL DECLARATION

## LXXXII.1 The complete measure

```text
W4-03 · SHELTER INFRASTRUCTURE
parts:        I–LXXXII
findings:     IN-01 .. IN-68 (worklist Part LIX)
meters:       5 — power · fuel · water · air · heat
chains:       3 — flood→power→heat · fire→air · power→water
kits:         6 — reconciliation · tiers · water · cascades · condition · surfaces
registers:    8 — subsystems · meters · tiers · cascades · machines ·
              warnings · drills · history
scenarios:    32 (S1–S32, cadence assigned)
rules:        L1–L6 · P1–P6 · S1–S6 · W1–W6 · F1–F6 · A1–A6 · T1–T6 ·
              F1–F6 · C1–C5 · X1–X6 · S1–S6
doctrine:     H1–H5 (truth · timing · agency · boundedness · quiet)
promises:     34, all guarded
rollout:      5 weeks
calendar:     weekly · release · seasonal · yearly
stop-list:    6 items
handoffs:     W3-05 · W3-03 · W4-01 · W4-02 · W4-04 · W4-05 · W4-06 · W3-06
```

## LXXXII.2 The final declaration

**W4-03 is complete.** Every rule family indexed, every finding dispositioned,
every promise guarded, every register seeded. Proposal only; execution requires
Annex U (Part I §U.2) and its signatures. Binding within this document: the
ledger laws, the ten rule families, the stop-the-line list, the worklist, the
doctrine, and the calendar.

## LXXXII.3 The three sentences, final

```text
One meter, one flow, one warned failure.
Warnings precede harm; recovery follows; causes are recorded.
A shelter is a promise of shelter; keep the promise mechanically.
```

## LXXXII.4 The end

```text
The house is honest. Keep it that way.

*Document control: W4-03 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-03.*
```
---

# W4-03 · PART LXXXIII — CLOSING CARDS

## LXXXIII.1 The implementer's closing card

```text
START:  read Part LXVI (reference) + Part V (playbooks)
WORK:   owner → meter → section → tier → warnings → kit
        (every step with its register row in the same change)
PROVE:  reconciliation zero; band gaps; request path; no hooks
CLOSE:  worklist row with its kit attached
```

## LXXXIII.2 The reviewer's closing card

```text
five questions:
  meter? tier? warning? action? register?
six refusals:
  second meter · silent shed · phantom reserve · cosmetic upgrade ·
  warning without gap · auto-service
```

## LXXXIII.3 The integrator's closing card

```text
weekly:  scan + reconciliation + one surface trace
release: kits + tier review + scenarios + copy review
season:  storm/cold + audits + drills
year:    census + deletion + resilience target
```

## LXXXIII.4 The player's closing card

```text
what the gauge says is true
what warns is real
what you fix stays fixed
what you neglect fails in daylight
the storm is survivable if you saw it coming — and you will
```

## LXXXIII.5 The house's closing card

```text
I will not lie about the fire.
I will not run dry without speaking.
I will not heal myself.
I will not break by surprise.
I will be quiet when I am well.
```

## LXXXIII.6 The final card

```text
W4-03 · complete · proposal only · Annex U governs execution
one meter, one flow, one warned failure
```

*End of Part LXXXIII. Continues in Part LXXXIV (end).*
---

# W4-03 · PART LXXXIV — END

```text
W4-03 SHELTER INFRASTRUCTURE INTEGRATION PLAN
complete (plan) · proposal only · HEAD 5be1a30a
parts I–LXXXIV · findings IN-01..IN-68

The house is honest. Keep it that way.

*Document control: W4-03 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-03.*
```
---

# W4-03 · PART LXXXV — THE LAST APPENDIX

## LXXXV.1 The evidence summary (closure shape)

```yaml
meters:
  register: current
  duplicates: 0
reconciliation:
  quiet: 0  heavy: 0  storm: 0  blackout: 0  idle: 0
tiers:
  order: pass  dynamic_shown: pass  emergency_warned: pass  recovery: staged
water:
  requests: pass  refusals: explained  blend: deterministic  drawdown: warned
flood:
  consumers: complete  warnings: precede damage  pumps: named power
air:
  decay: pass  filters: lifecycle  routing: via medical  ingress: authored
thermal_fire:
  bands: pass  transfers: signed  incidents: end  causes: recorded
condition:
  wear: deterministic  disable_warned: pass  service: action  hooks: 0
cascades:
  chains: 3  warnings: all  off_ramps: tested  depth: bounded  drills: cost
surfaces:
  read_only: pass  units: pass  unavailable: authored  fabricated: 0
soak: pass
```

## LXXXV.2 The residual breadth flags

```text
- zone scaling will require tier reviews as content grows (process)
- new sources/machines require profile authoring (data work)
- both are breadth; correctness is closed by the six kits
```

## LXXXV.3 The final evidence note

```text
Evidence exists so the house's credibility is provable, not assumed. Six
kits, eight registers, thirty-two scenarios — the house can prove itself on
the worst day, not just the best.
```

*End of Part LXXXV. Continues in Part LXXXVI (the absolute close).*
---

# W4-03 · PART LXXXVI — THE ABSOLUTE CLOSE

```text
Document:   W4-03 SHELTER INFRASTRUCTURE INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LXXXVI
Findings:   IN-01 .. IN-68
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

One meter, one flow, one warned failure.
A shelter is a promise of shelter; keep the promise mechanically.

The house is honest. Keep it that way.

*Document control: W4-03 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
absolute close of W4-03.*
```
---

# W4-03 · PART LXXXVII — FINAL CLOSING NOTE

## LXXXVII.1 The plan's final inventory

```text
Document:  W4-03 SHELTER INFRASTRUCTURE INTEGRATION PLAN
Parts:     I–LXXXVII (86 content parts + base)
Findings:  IN-01 .. IN-68
Meters:    5
Chains:    3
Kits:      6
Registers: 8
Scenarios: 32
Rules:     11 families (ledger, power, shedding, water, drainage, air,
           thermal, fire, condition, cascades, surfaces)
Doctrine:  H1–H5
Promises:  34, all guarded
Rollout:   5 weeks
Calendar:  weekly / release / seasonal / yearly
Handoffs:  W3-05, W3-03, W4-01, W4-02, W4-04, W4-05, W4-06, W3-06
```

## LXXXVII.2 The plan's final instruction

```text
Keep the meters. Run the kits. Warn early. Cost repairs. End incidents.
Bend chains. Stay quiet when it works. Retire one ghost a year.
```

## LXXXVII.3 The plan's final sentence

```text
A shelter is a promise of shelter; keep the promise mechanically.
```

## LXXXVII.4 The close

```text
The house is honest. Keep it that way.

*Document control: W4-03 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-03.*
```
---

# W4-03 · PART LXXXVIII — CLOSING ADDENDUM

## LXXXVIII.1 The four words the house lives by

```text
TRUTH      one meter, never forked, never fabricated
WARNING    every harm preceded by its announcement
COST       every repair consumes, every choice matters
QUIET      the reward for keeping all of the above
```

## LXXXVIII.2 The four words the house fears

```text
CONVENIENCE   a second meter "just for this"
SURPRISE      a failure without a warning
GHOSTS        phantom reserves and free repairs
NOISE         warnings that no longer mean anything
```

## LXXXVIII.3 The closing paragraph

```text
The shelter is the game's quiet protagonist: it does not speak, it warms,
lights, waters, and filters. This plan exists so that when it finally does
speak — in a warning at the worst moment — the player hears the truth, sees
the choice, and knows the house has kept every promise it made.

One meter, one flow, one warned failure.
```

*Document control: W4-03 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
true end of W4-03.*

---

# W4-03 · PART LXXXIX — THE FINAL MEASURE

```text
W4-03 · SHELTER INFRASTRUCTURE
parts:      I–LXXXIX
findings:   IN-01 .. IN-68
meters:     5 · chains: 3 · kits: 6 · registers: 8 · scenarios: 32
rules:      11 families · doctrine: H1–H5 · promises: 34
rollout:    5 weeks · calendar: four cadences · stop-list: 6 items
```

## The final card

```text
One meter, one flow, one warned failure.
A shelter is a promise of shelter; keep the promise mechanically.
The house is honest. Keep it that way.
```

*Document control: W4-03 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
absolute end of W4-03.*
