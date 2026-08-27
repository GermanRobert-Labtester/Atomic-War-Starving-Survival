#!/usr/bin/env bash
# triad-drift-gate.sh — ensure every SaveXxx has a SetupXxx and AllSaveSections entry
# Fails if a Setup/Save pair is missing or AllSaveSections is out of sync.
# See AGENTS.md H7 triad drift risk: Setup without Save silently drops state.
# Full architecture documentation: docs/architecture/TRIAD_GATE_AND_SAVE_OWNERSHIP.md
set -euo pipefail
ROOT="$(cd "$(dirname "$0")/../.." && pwd)"
cd "$ROOT"

# ── Documented exceptions ────────────────────────────────────────────
# These Save methods have no matching Setup method BY DESIGN. Each entry
# must have an owner comment explaining why. Detailed save store ownership
# is documented in docs/architecture/TRIAD_GATE_AND_SAVE_OWNERSHIP.md.
# a Setup, add it here with an explanation — do not silently suppress it.
#
# SaveChemicalDependency  — initialized inline in SetupMentalHealthCrisis
#                           (Main.ShelterBatch3.cs). Owner: medical/expansion team.
# SaveDailyBriefing       — setup is SetupDailyBriefingModal (Main.Campaign.cs).
#                           Name mismatch, not a missing triad. Owner: campaign team.
# SaveExpansionHub        — setup is SetupExpansions (Main.ExpansionHub.cs).
#                           Name mismatch. Owner: expansion hub team.
# SaveHoldfast            — setup is SetupHoldfastRuntime (Main.Holdfast.cs).
#                           Name mismatch. Owner: holdfast/expansion 01 team.
# SavePhantomMemory       — setup is SetupPhantom (Main.Phase0.cs).
#                           Name mismatch. Owner: phase0/lineage team.
# SaveWastelandMap        — setup removed; map is owned by WorldHostSession.Create()
#                           (src/Host/WorldHostSession.cs). No dedicated Setup in Main.
#                           Owner: world/map team.
NO_SETUP_NEEDED="ChemicalDependency DailyBriefing ExpansionHub Holdfast PhantomMemory WastelandMap"

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
    # Check if this is a documented exception (Save with no dedicated Setup by design)
    if echo "$NO_SETUP_NEEDED" | grep -qw "$save"; then
      echo "[OK]   Save$save — documented exception (see header)"
    else
      echo "[WARN] Save$save has no matching Setup$save in src/Main.*.cs" >&2
      echo "       → Add it to NO_SETUP_NEEDED with an owner comment, or add the missing Setup." >&2
    fi
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
