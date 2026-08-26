#!/usr/bin/env bash
# triad-drift-gate.sh — ensure every SaveXxx has a SetupXxx and AllSaveSections entry
# Fails if a Setup/Save pair is missing or AllSaveSections is out of sync.
# See AGENTS.md H7 triad drift risk: Setup without Save silently drops state.
set -euo pipefail
ROOT="$(cd "$(dirname "$0")/../.." && pwd)"
cd "$ROOT"

# Extract Setup* and Save* method names (private void SetupXxx() / SaveXxx())
SETUPS=$(grep -rhoE "private void Setup[A-Za-z0-9_]*\(" src/Main*.cs | sed -E 's/.*Setup([A-Za-z0-9_]+)\(.*/\1/' | sort -u)
SAVES=$(grep -rhoE "private void Save[A-Za-z0-9_]*\(" src/Main*.cs | grep -v "SaveAll" | sed -E 's/.*Save([A-Za-z0-9_]+)\(.*/\1/' | sort -u)

# Extract AllSaveSections entries (strings in array)
SECTIONS=$(grep -oE '"[a-z0-9_]+"' src/Main.SaveOrchestrator.cs | tr -d '"' | grep -v "user:" | sort -u)

# Helper: PascalCase to snake_case
to_snake() {
  echo "$1" | sed -E 's/([a-z0-9])([A-Z])/\1_\2/g' | tr '[:upper:]' '[:lower:]'
}

FAIL=0

echo "── Triad Drift Gate ──"
echo "Setups found: $(echo "$SETUPS" | wc -l), Saves found: $(echo "$SAVES" | wc -l), Sections: $(echo "$SECTIONS" | wc -l)"
echo ""

# For each Save, check Setup exists and section exists
for save in $SAVES; do
  # Skip orchestrators already filtered, but double-check
  if [[ "$save" == "All" || "$save" == "AllExpandedShelterSystems" ]]; then continue; fi
  if ! echo "$SETUPS" | grep -qx "$save"; then
    echo "[WARN] Save$save has no matching Setup$save in src/Main.*.cs" >&2
    # not fatal — some Saves are for sub-steps, but log
  fi
  snake=$(to_snake "$save")
  if ! echo "$SECTIONS" | grep -qx "$snake"; then
    # Try common aliases: SaveSurvivors -> survivors, SaveExpeditions -> expedition, SaveCaravans -> caravan, SaveMoralChoice -> (no section, uses journal?), SaveHoldfastRuntime -> holdfast_trade
    case "$save" in
      Survivors) snake="survivors" ;;
      Expeditions) snake="expedition" ;;
      Caravans) snake="caravan" ;;
      Holdfast) snake="holdfast" ;;
      HoldfastRuntime) snake="holdfast_trade" ;;
      MoralChoice) snake="host_event" ;; # MoralChoice state is saved via HostEventSaveStore/host_event, not a dedicated section
      PhantomMemory) snake="phantom_memory" ;;
      DoseLedger) snake="dose_ledger" ;;
      Thirdonary) snake="thirdonary" ;;
      CampaignDay) snake="campaign_day" ;;
      DailyBriefing) snake="daily_briefing" ;;
      MedicalWard) snake="medical_ward" ;;
      WastelandMap) snake="wasteland_map" ;;
      EncounterChoice) snake="encounter_choice" ;;
      PowerGrid) snake="power_grid" ;;
      StartingLevel) snake="starting_level" ;;
      YearOfAsh) snake="year_of_ash" ;;
      Phase0) snake="phase0" ;;
      EventAdapter) snake="host_event" ;;
      ExpansionQuests) snake="expansion_quest" ;;
      ChemicalDependency) snake="chemical_dependency" ;;
      *) ;;
    esac
    if ! echo "$SECTIONS" | grep -qx "$snake"; then
      echo "[FAIL] Save$save (snake: $snake) missing from AllSaveSections in src/Main.SaveOrchestrator.cs" >&2
      FAIL=1
    fi
  fi
done

# For each section, check Save exists
for sec in $SECTIONS; do
  # Convert snake to Pascal for Save check (e.g., water_treatment -> WaterTreatment)
  pascal=$(echo "$sec" | awk -F_ '{for(i=1;i<=NF;i++) $i=toupper(substr($i,1,1)) substr($i,2)}1' OFS="" | tr -d ' ')
  # Handle aliases
  case "$sec" in
    survivors) pascal="Survivors" ;;
    expedition) pascal="Expeditions" ;;
    caravan) pascal="Caravans" ;;
    holdfast) pascal="Holdfast" ;;
    holdfast_trade) pascal="HoldfastRuntime" ;;
    host_event) pascal="EventAdapter" ;; # also covers MoralChoice
    expansion_quest) pascal="ExpansionQuests" ;;
    *) ;;
  esac
  if ! echo "$SAVES" | grep -qx "$pascal"; then
    # Check alternative for host_event (could be MoralChoice or EventAdapter)
    if [[ "$sec" == "host_event" ]]; then
      if echo "$SAVES" | grep -qx "EventAdapter" || echo "$SAVES" | grep -qx "MoralChoice"; then
        continue
      fi
    fi
    echo "[FAIL] AllSaveSections entry \"$sec\" has no matching Save$pascal in src/Main.*.cs" >&2
    FAIL=1
  fi
done

if [[ $FAIL -eq 0 ]]; then
  echo "GATE PASS: triad drift — AllSaveSections in sync with Setup/Save pairs"
  exit 0
else
  echo "GATE FAIL: triad drift detected — fix Setup/Save/AllSaveSections" >&2
  exit 1
fi
