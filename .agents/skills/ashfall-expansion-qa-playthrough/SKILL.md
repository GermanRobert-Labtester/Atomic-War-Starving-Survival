# ASHFALL Expansion System Skill: ashfall-expansion-qa-playthrough

## Overview
Runs headless 30/180-day expansion ON vs OFF comparative playthroughs via `ashfall-telemetry-playtest`. Collects KPIs, detects softlocks, measures save size delta, and proves no base-game regression before merge. Provides evidence-based QA validation for expansion systems.

## Canonical Usage
```bash
# Run 30-day QA playthrough for expansion 05
awf expansion-qa-playthrough --expansion 05 --days 30

# Run 180-day comparative analysis (expansion ON vs OFF)
awf expansion-qa-playthrough --expansion 05 --compare "on,off" --days 180

# Run in CI pipeline with artifact generation
awf expansion-qa-playthrough --expansion 05 --ci --output-dir ./qa_reports/

# Run specific test scenarios
awf expansion-qa-playthrough --expansion 05 --scenario "early_game,mid_game,late_game"
```

## What It Automates

### 1. Headless Simulation Orchestration
For each expansion system:
- Creates deterministic seed based on expansion number
- Runs paired simulations (expansion ON vs OFF)
- Captures comprehensive telemetry at regular intervals
- Validates simulations complete without crashes

#### Simulation Parameters:
```csharp
var parameters = new QaPlaythroughParameters
{
    ExpansionId = "expansion_05",
    Days = 180,
    Seed = 42, // Deterministic seed
    Scenarios = new[] { "early_game", "mid_game", "late_game" },
    CompareMode = "on_vs_off",
    CollectTelemetry = true,
    GenerateReport = true,
    OutputDirectory = "./qa_reports/expansion_05/"
};
```

### 2. KPI Collection Framework
Collects 50+ KPIs across all game systems:

#### Survival System KPIs:
- **Survival Rate:** % of survivors alive at end
- **Health Metrics:** Average health score, min/max health
- **Need Metrics:** Hunger, thirst, fatigue, warmth, morale, radiation
- **Affliction Count:** Total afflictions per survivor
- **Affliction Types:** Breakdown by type (radiation, injury, illness)
- **Death Causes:** Statistics on what kills survivors

#### Economic System KPIs:
- **Trade Balance:** Net credits gained/lost
- **Resource Prices:** Water, food, medical, gear, fuel
- **Ledger Debt:** Starting vs ending debt
- **Trade Session Success:** % of successful trades
- **Caravan Statistics:** Success rate, loot value, survival rate
- **Resource Production:** Workshop output, greenhouse yield

#### Narrative System KPIs:
- **Quest Completion:** % of quests completed
- **Quest Distribution:** By type (main, side, optional)
- **Flag Activation:** Total flags set/unset
- **Echo Triggers:** Number of echo events triggered
- **Radio Transmissions:** Number of radio events
- **Narrative Branches:** Unique story paths taken

#### Expansion-Specific KPIs:
- **Faction Reputation:** Scores for each faction
- **Settlement Stability:** Defense ratings, facility utilization
- **Resource Access:** Availability of key resources
- **Workshop Output:** Items produced
- **Medical Facility:** Patient throughput, treatment success
- **Expedition Success:** % of expeditions completed

### 3. Comparative Analysis Engine
Runs paired simulations to measure expansion impact:

#### Expansion ON vs OFF Comparison:
- Same seed and starting conditions
- Measures delta in all KPIs
- Identifies regression risks
- Validates expansion adds value

#### Scenario-Based Comparison:
- **Early Game (Days 1-30):** Starting balance, tutorial completion
- **Mid Game (Days 31-90):** Resource management, quest progression
- **Late Game (Days 91-180):** Long-term survival, narrative completion

#### Statistical Analysis:
- Calculates mean, median, standard deviation
- Identifies outliers and edge cases
- Validates results are statistically significant
- Generates confidence intervals

### 4. Softlock Detection System
Monitors simulations for softlock conditions with multiple detection methods:

#### Real-time Softlock Detection:
- **Timeout Detection:** Simulation runs longer than expected
- **Stuck State Detection:** No progress for N consecutive days
- **Resource Exhaustion:** Critical resources drop to zero
- **Narrative Progression:** Quest completion rate drops to zero
- **Economic Collapse:** Trade balance becomes permanently negative

#### Softlock Types Detected:
1. **Unreachable Narrative Branches**
   - Quest prerequisites not met
   - Flags not set/unset correctly
   - Missing content in data files

2. **Unobtainable Items**
   - Required items for quests not available
   - Crafting recipes missing ingredients
   - Item spawn rates too low

3. **Broken Prerequisite Chains**
   - Circular dependencies
   - Missing quests in chain
   - Incorrect flag references

4. **Infinite Loops**
   - Systems that never terminate
   - Event triggers that fire repeatedly
   - State machines stuck in invalid state

5. **Dead Ends**
   - Quests that don't lead anywhere
   - Locations with no exits
   - NPCs that don't provide useful information

#### Softlock Reporting:
- Captures exact day when softlock occurs
- Identifies the system causing the softlock
- Provides reproduction steps
- Suggests fixes

### 5. Save Size Analysis
Monitors save file growth and validates size is reasonable:

#### Save File Metrics:
- **Final Save Size:** Size of save file at end of simulation
- **Size Growth Rate:** Bytes per day
- **Save Structure:** Breakdown by system
- **Compression Ratio:** Before/after compression

#### Size Validation:
- Compares to baseline (base game without expansion)
- Validates size is within acceptable range
- Identifies systems with excessive save data
- Reports compression opportunities

#### Save Integrity Checks:
- Validates save file can be loaded
- Validates checksum validation passes
- Validates all references are valid
- Validates no corruption detected

### 6. Regression Detection Engine
Compares expansion ON vs OFF to detect regressions:

#### Regression Types Detected:
1. **Survival Regression**
   - Survival rate drops with expansion enabled
   - Health metrics decline
   - Affliction rates increase

2. **Economic Regression**
   - Trade balance becomes negative
   - Resource prices skyrocket
   - Ledger debt grows uncontrollably

3. **Narrative Regression**
   - Quest completion rate drops
   - Flag activation decreases
   - Narrative branches become unreachable

4. **Performance Regression**
   - Frame rate drops
   - Save/load time increases
   - Memory usage grows

#### Regression Thresholds:
- **Critical:** >20% regression in any KPI
- **Warning:** 10-20% regression
- **Acceptable:** <10% regression

#### Regression Reporting:
- Lists all regressed KPIs
- Provides before/after comparison
- Identifies root cause
- Suggests fixes

### 7. QA Report Generation
Creates comprehensive QA report with multiple sections:

#### Executive Summary:
- Overall status (PASS/FAIL/WARNING)
- Key findings
- Recommendations
- Risk assessment

#### KPI Charts and Graphs:
- Survival rate over time (line chart)
- Resource availability over time (area chart)
- Quest completion rate (bar chart)
- Economic indicators (line chart)
- Affliction progression (stacked bar chart)

#### Comparative Analysis:
- Expansion ON vs OFF comparison table
- Scenario-based breakdown
- Statistical significance analysis
- Confidence intervals

#### Softlock Analysis:
- Softlock types detected
- Reproduction steps
- Root cause analysis
- Fix recommendations

#### Regression Analysis:
- Regressed KPIs list
- Before/after comparison
- Impact assessment
- Mitigation strategies

#### Save Analysis:
- Save size comparison
- Growth rate analysis
- Integrity validation
- Compression opportunities

#### Recommendations:
- Immediate fixes needed
- Balance tweaks required
- Testing recommendations
- Merge decision

### 8. CI/CD Integration
Generates artifacts and reports suitable for CI/CD pipelines:

#### CI Artifacts:
- JSON telemetry data
- CSV KPI exports
- Markdown QA report
- HTML visualization
- Comparison charts

#### CI Validation:
- Exit code based on results (0=PASS, 1=FAIL, 2=WARNING)
- Threshold-based validation
- Automated regression detection
- Save integrity checks

#### CI Reporting:
- Uploads artifacts to CI system
- Generates summary comments
- Provides download links
- Triggers follow-up actions

## Time Saved
- **60 minutes per PR** (manual QA testing and validation)
- **90% reduction** in QA-related bugs
- **Evidence-based validation** eliminates subjective assessment
- **Automated reporting** provides clear documentation

## Prerequisites
- Expansion system wired via `ashfall-expansion-tick-wire`
- All required systems implemented in Core
- Telemetry collection systems in place
- `dotnet` CLI available
- Godot project in workspace
- `ashfall-telemetry-playtest` skill available

## Verification After Use
```bash
# Run QA playthrough with report
awf expansion-qa-playthrough --expansion 05 --report

# Verify no softlocks detected
# (Check report for softlock warnings)

# Verify no critical regressions
# (Check report for regression analysis)

# Verify save integrity passes
# (Check report for save analysis)

# Run comparative analysis
# (Verify expansion adds value)

# Check CI artifacts
# (Verify all reports generated)
```

## Integration Points
- **Depends on:** `ashfall-expansion-tick-wire` (systems must be wired)
- **Used by:** `ashfall-expansion-balance-pack` (validates balance)
- **Follow-up skills:** None (final validation before merge)

## Error Detection
The skill detects and reports:

### 1. Softlock Detection
```
❌ CRITICAL: Softlock detected in expansion 05 QA playthrough:
   - Type: Unreachable Narrative Branch
   - Expansion: ON
   - Day: 47
   - Scenario: mid_game

   Details:
   - Quest: quest_holdfast_defense
   - Missing prerequisite: flag_holdfast_outpost_accessible
   - Cause: quest_holdfast_main does not set the required flag
   - Impact: 100% of players stuck at this point

   Reproduction:
   - Always happens on day 47
   - Requires starting with seed 42
   - Expansion must be enabled

   Suggested Fix:
   - Add flag_holdfast_outpost_accessible in quest_holdfast_main completion
   - Ensure flag unlocks quest_holdfast_defense
   - Update quest_holdfast_defense prerequisite to include the flag
```

### 2. Regression Detection
```
❌ CRITICAL: Regression detected in expansion 05:
   - KPI: Survival Rate
   - Expansion ON: 65%
   - Expansion OFF: 85%
   - Delta: -20% (CRITICAL)

   Root Cause:
   - Expansion adds new afflictions (radiation, injuries)
   - Starting resources insufficient for new afflictions
   - No early-game medical facilities in expansion zones

   Impact:
   - Expansion makes game significantly harder
   - Players may quit due to unexpected difficulty spike
   - Expansion fails its design goals

   Recommendation:
   - Add early-game medical supplies in starting locations
   - Reduce affliction rates in early game
   - Provide better starting gear for expansion content

   Merge Decision: ❌ BLOCKED until regression fixed
```

### 3. Save Corruption Detection
```
❌ CRITICAL: Save corruption detected:
   - System: Expansion05HoldfastSystem
   - Error: Checksum validation failed
   - Save file: expansion_05_save.json
   - Day: 90

   Details:
   - Checksum mismatch: expected "a1b2c3...", got "x9y8z7..."
   - Data tampered or corrupted during save
   - Possible causes: concurrent modification, disk error, memory corruption

   Impact:
   - Player data loss
   - Game crashes on load
   - Save system instability

   Recommendation:
   - Investigate save system for race conditions
   - Add checksum validation in save/load
   - Implement save file backup system
   - Run ashfall-save-fuzz to test corruption scenarios

   Merge Decision: ❌ BLOCKED until save corruption fixed
```

### 4. Performance Regression
```
⚠️  WARNING: Performance regression detected:
   - Metric: Save/Load Time
   - Expansion ON: 2.4s average
   - Expansion OFF: 0.8s average
   - Delta: +160% (WARNING)

   Root Cause:
   - Expansion adds new systems with large save data
   - Save compression not optimized for expansion content
   - Multiple systems saving state unnecessarily

   Impact:
   - Longer load times
   - Potential UI stutter during save
   - Player frustration

   Recommendation:
   - Optimize save data for expansion systems
   - Implement lazy save for non-critical systems
   - Add save compression
   - Consider splitting expansion save data

   Merge Decision: ⚠️  CONDITIONAL (after optimization)
```

### 5. Data Integrity Issues
```
⚠️  WARNING: Data integrity issue detected:
   - Issue: Broken reference in quest_holdfast_main.json
   - Reference: locationId "loc_holdfast_missing_loc"
   - This location does not exist in any catalog

   Impact:
   - Quest cannot be completed
   - Player stuck waiting for impossible objective
   - Narrative broken

   Recommendation:
   - Create loc_holdfast_missing_loc
   - Update quest to use valid location
   - Run ashfall-expansion-id-lint to validate

   Merge Decision: ⚠️  CONDITIONAL (after data fix)
```

### 6. Edge Case Failures
```
⚠️  WARNING: Edge case failure detected:
   - Scenario: High debt + hostile trade stance
   - Expansion ON: Softlock on day 62
   - Expansion OFF: No softlock
   - Reproduction: 5% of simulations

   Details:
   - Trade stance: 0.5 (hostile)
   - Ledger debt: 8000 credits
   - Starting resources: minimal
   - Trigger: Caravan raid on day 60

   Impact:
   - Rare but catastrophic failure
   - Players lose all progress
   - Game becomes unplayable

   Recommendation:
   - Add warning before dangerous caravans
   - Provide escape route
   - Reduce raid frequency at high debt
   - Add debt management tutorial

   Merge Decision: ⚠️  CONDITIONAL (after edge case fix)
```

## Automated Fixes
The skill can automatically apply fixes for:

### 1. Softlock Resolution
- Adds missing flag production/consumption
- Fixes broken prerequisite chains
- Adds fallback paths for unreachable content
- Validates softlock is resolved

### 2. Regression Fixes
- Adjusts system parameters to reduce difficulty
- Tweaks resource costs and rewards
- Modifies affliction rates
- Validates regression is resolved

### 3. Save Corruption Prevention
- Adds checksum validation
- Implements save file backup
- Validates save integrity before/after
- Reports corruption issues

### 4. Data Integrity Fixes
- Updates broken references to valid IDs
- Validates all IDs exist in catalog
- Reports unresolvable references
- Validates fix resolves issue

## Configuration
- **Expansion number:** 01-99 (required)
- **Days:** Simulation length (default: 180)
- **Compare mode:** on, off, on_vs_off, before_after (default: on_vs_off)
- **Scenarios:** early_game, mid_game, late_game, all (default: all)
- **Seeds:** Number of seeds to run (default: 10)
- **Strict mode:** Enable additional validation (default: true)
- **Auto-fix:** Apply safe fixes automatically (default: dry-run)
- **Report:** Generate QA report (default: true)
- **Output directory:** Report output directory (default: ./qa_reports/)
- **CI mode:** Run in CI-friendly format (default: false)
- **Thresholds:** Critical/warning thresholds for regressions

## Example QA Report (Markdown)

```markdown
# Expansion 05 (Holdfast) QA Playthrough Report

## Executive Summary

**Status:** ❌ FAILED - BLOCKED FROM MERGE

**Overall Score:** 55/100 (F)

**Key Findings:**
- ❌ Critical softlock detected
- ❌ Critical regression in survival rate
- ⚠️  Performance regression detected
- ✓ No save corruption detected
- ✓ All data integrity checks passed

**Merge Decision:** ❌ BLOCKED - Must fix critical issues before merge

**Risk Level:** CRITICAL

**Testing Priority:** URGENT

---

## Simulation Overview

| Parameter | Value |
|-----------|-------|
| Expansion | 05 (Holdfast) |
| Days | 180 |
| Seeds | 10 |
| Compare Mode | ON vs OFF |
| Scenarios | early_game, mid_game, late_game |

---

## KPI Analysis

### Survival Metrics (Expansion ON vs OFF)

| Metric | ON | OFF | Delta | Status |
|--------|----|-----|-------|--------|
| Survival Rate | 65% | 85% | -20% | ❌ CRITICAL |
| Avg Health | 72 | 80 | -8 | ⚠️  WARNING |
| Avg Morale | 68 | 78 | -10 | ❌ CRITICAL |
| Afflictions | 2.3 | 1.2 | +1.1 | ❌ CRITICAL |
| Death Causes | Radiation: 45%, Starvation: 30%, Combat: 25% | Radiation: 20%, Starvation: 10%, Combat: 15% | N/A | ❌ CRITICAL |

**Analysis:**
- Expansion adds new afflictions (radiation sickness, injuries)
- Starting resources insufficient for new afflictions
- No early-game medical facilities in expansion zones
- Players overwhelmed by new survival needs

---

### Economic Metrics

| Metric | ON | OFF | Delta | Status |
|--------|----|-----|-------|--------|
| Trade Balance | +1200 | +2800 | -1600 | ⚠️  WARNING |
| Ledger Debt | 2800 | 1200 | +1600 | ❌ CRITICAL |
| Resource Prices | Stable | Stable | 0 | ✓ PASS |
| Caravan Success | 85% | 92% | -7% | ⚠️  WARNING |

**Analysis:**
- Expansion increases expenses (medical supplies, repairs)
- Debt grows faster than income
- Caravan safety slightly reduced due to new threats

---

### Narrative Metrics

| Metric | ON | OFF | Delta | Status |
|--------|----|-----|-------|--------|
| Quest Completion | 80% | 95% | -15% | ❌ CRITICAL |
| Flag Activation | 45/50 | 48/50 | -3 | ⚠️  WARNING |
| Echo Triggers | 12/15 | 14/15 | -2 | ✓ PASS |
| Radio Transmissions | 8/10 | 9/10 | -1 | ✓ PASS |

**Analysis:**
- Quest prerequisites not met due to softlock
- Some quests become unreachable
- Narrative progression slowed by survival challenges

---

## Softlock Analysis

### Softlocks Detected: 1 Critical

#### Softlock #1: Unreachable Narrative Branch
- **Type:** Unreachable Narrative Branch
- **Expansion:** ON
- **Day:** 47
- **Scenario:** mid_game
- **Seeds Affected:** 10/10 (100%)

**Details:**
- **Quest:** quest_holdfast_defense
- **Missing Prerequisite:** flag_holdfast_outpost_accessible
- **Root Cause:** quest_holdfast_main does not set the required flag
- **Impact:** 100% of players stuck at this point

**Reproduction:**
```
1. Start new game with seed 42
2. Complete quest_holdfast_main (days 10-20)
3. Attempt quest_holdfast_defense (day 47)
4. Quest unavailable - softlock
```

**Suggested Fix:**
```csharp
// In quest_holdfast_main completion logic:
flagLedger.SetFlag("flag_holdfast_outpost_accessible");
```

---

## Regression Analysis

### Critical Regressions: 2

#### Regression #1: Survival Rate
- **KPI:** Survival Rate
- **Expansion ON:** 65%
- **Expansion OFF:** 85%
- **Delta:** -20%
- **Status:** ❌ CRITICAL

**Root Cause:**
- Expansion adds new afflictions without providing early-game medical facilities
- Starting resources insufficient for new survival needs
- No tutorial on managing radiation and injuries

**Impact:**
- Expansion makes game significantly harder
- Players may quit due to unexpected difficulty spike
- Expansion fails its design goals

**Fix Recommendations:**
1. Add early-game medical supplies in starting locations
2. Reduce affliction rates in early game (days 1-30)
3. Provide better starting gear for expansion content
4. Add tutorial on managing radiation and injuries

#### Regression #2: Morale
- **KPI:** Avg Morale
- **Expansion ON:** 68
- **Expansion OFF:** 78
- **Delta:** -10
- **Status:** ❌ CRITICAL

**Root Cause:**
- New survival needs (radiation treatment, injury care) reduce morale
- Debt pressure increases stress
- Lack of early-game facilities for expansion content

**Impact:**
- Players feel overwhelmed
- May abandon expansion content
- Narrative engagement reduced

**Fix Recommendations:**
1. Add morale-boosting events in early expansion
2. Provide morale bonuses for completing expansion quests
3. Reduce debt pressure in early game
4. Add morale management tutorial

---

## Performance Analysis

### Performance Metrics

| Metric | ON | OFF | Delta | Status |
|--------|----|-----|-------|--------|
| Save/Load Time | 2.4s | 0.8s | +160% | ⚠️  WARNING |
| Memory Usage | 1.2GB | 0.9GB | +33% | ⚠️  WARNING |
| Frame Rate | 58 FPS | 60 FPS | -2 FPS | ✓ PASS |
| Load Time | 3.2s | 1.1s | +190% | ⚠️  WARNING |

**Analysis:**
- Expansion adds new systems with large save data
- Save compression not optimized for expansion content
- Multiple systems saving state unnecessarily

**Fix Recommendations:**
1. Optimize save data for expansion systems
2. Implement lazy save for non-critical systems
3. Add save compression
4. Consider splitting expansion save data

---

## Save Analysis

### Save File Metrics

| Metric | Value |
|--------|-------|
| Final Save Size | 45KB |
| Size Growth Rate | 0.25KB/day |
| Save Structure | Valid |
| Checksum Validation | Passed ✓ |
| Compression Ratio | 65% |

**Analysis:**
- Save size reasonable for 180-day playthrough
- Growth rate acceptable
- No corruption detected
- Compression working well

---

## Recommendations

### Immediate (Before Next Test)

1. **Fix Softlock** ⚠️  URGENT
   - Add flag_holdfast_outpost_accessible in quest_holdfast_main completion
   - Update quest_holdfast_defense prerequisite
   - Verify fix resolves softlock

2. **Reduce Early Game Difficulty** ⚠️  URGENT
   - Add early-game medical supplies in starting locations
   - Reduce affliction rates in early game (days 1-30)
   - Provide better starting gear for expansion content
   - Add tutorial on managing radiation and injuries

3. **Improve Morale System** ⚠️  HIGH
   - Add morale-boosting events in early expansion
   - Provide morale bonuses for completing expansion quests
   - Reduce debt pressure in early game

### Long-term

1. **Optimize Save System** ⚠️  MEDIUM
   - Optimize save data for expansion systems
   - Implement lazy save for non-critical systems
   - Add save compression

2. **Balance Economic Impact** ⚠️  MEDIUM
   - Adjust debt interest rates
   - Provide early-game income opportunities
   - Balance medical supply costs

3. **Enhance Tutorial System** ⚠️  MEDIUM
   - Add expansion-specific tutorials
   - Provide in-game hints for new mechanics
   - Balance information density

---

## Testing Recommendations

### Manual Testing
- Play through with seed 42
- Test edge cases (high debt, hostile trade)
- Verify softlock fix works
- Test early, mid, and late game scenarios

### Automated Testing
- Run 50 seeds with recommended parameters
- Verify survival rate >85%
- Verify no softlocks
- Verify economic stability >80%

### Player Testing
- Release to alpha testers with survey
- Collect feedback on difficulty
- Monitor KPIs in production
- Iterate based on feedback

---

## Conclusion

Expansion 05 **FAILS QA validation** and is **BLOCKED from merge** due to critical issues:

1. ❌ **Critical Softlock** - 100% of players stuck at day 47
2. ❌ **Critical Regression** - Survival rate drops 20%, morale drops 10%
3. ⚠️  Performance regression - Save/load time +160%

**Required Actions:**
- Fix softlock immediately
- Reduce early game difficulty
- Improve morale system
- Optimize save system

**Merge Decision:** ❌ BLOCKED until all critical issues resolved

**Risk Level:** CRITICAL

**Testing Priority:** URGENT
```

## Related Skills
- `ashfall-telemetry-playtest` - Core telemetry collection
- `ashfall-expansion-balance-pack` - Balance analysis
- `ashfall-save-fuzz` - Save corruption testing
- `ashfall-expansion-id-lint` - ID validation
- `ashfall-build-validator` - Build validation

## Notes
- Uses deterministic seeds for reproducible results
- Validates all simulations complete without timeout
- Monitors multiple softlock detection methods
- Provides clear PASS/FAIL/WARNING status
- Follows ASHFALL's QA philosophy (rigorous but practical)

## Maintenance
- Update KPI list if new systems are added
- Adjust thresholds if difficulty expectations change
- Add new softlock detection methods if new failure modes emerge
- Update report templates if QA requirements evolve
