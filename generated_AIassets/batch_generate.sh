#!/usr/bin/env bash
# Batch-generate 75 encounter + hazard icons via Bailian bl image generate
# Skips: hazard_surgicalbotch (duplicate), enc_a (already generated)
set -uo pipefail

ROOT="$(cd "$(dirname "$0")" && pwd)"
ENC_DIR="$ROOT/encounters"
HAZ_DIR="$ROOT/hazards"
LOG="$ROOT/batch_generate.log"

mkdir -p "$ENC_DIR" "$HAZ_DIR"
: > "$LOG"

log() { echo "[$(date '+%H:%M:%S')] $*" | tee -a "$LOG"; }

gen() {
  local prompt="$1" out_dir="$2" prefix="$3"
  bl image generate \
    --prompt "$prompt" \
    --out-dir "$out_dir" \
    --out-prefix "$prefix" \
    --watermark false \
    --size 1024*1024 \
    >> "$LOG" 2>&1
  return $?
}

# ── ENCOUNTERS (59, enc_a already done) ──────────────────────────────
declare -a ENC_IDS=(
  "enc_ambulance_gamble|ambulance gamble"
  "enc_apex_predator|apex predator"
  "enc_ash_quicksand|ash quicksand"
  "enc_b|b"
  "enc_black_rain_pocket|black rain pocket"
  "enc_blind_wanderer|blind wanderer"
  "enc_broken_drone|broken drone"
  "enc_cannibal_trap|cannibal trap"
  "enc_child_sniper|child sniper"
  "enc_collapsed_rubble|collapsed rubble"
  "enc_condensation_mold|condensation mold"
  "enc_cult_procession|cult procession"
  "enc_dead_letter_office|dead letter office"
  "enc_dead_radio_operator|dead radio operator"
  "enc_deserter_cache|deserter cache"
  "enc_deserters|deserters"
  "enc_deserters_stand|deserters stand"
  "enc_dog_pack|dog pack"
  "enc_dying_doctor|dying doctor"
  "enc_exile_vote|exile vote"
  "enc_faction_roadblock|faction roadblock"
  "enc_feral_dogs|feral dogs"
  "enc_flooded_crater|flooded crater"
  "enc_frozen_family|frozen family"
  "enc_geiger_spike|geiger spike"
  "enc_hanging_man|hanging man"
  "enc_hazmat_corpse|hazmat corpse"
  "enc_hostile_fauna|hostile fauna"
  "enc_mercy_request|mercy request"
  "enc_minefield|minefield"
  "enc_minefield_remnant|minefield remnant"
  "enc_mutated_bear|mutated bear"
  "enc_mutated_flora|mutated flora"
  "enc_overturned_ambulance|overturned ambulance"
  "enc_oxygen_thin|oxygen thin"
  "enc_paratrooper|paratrooper"
  "enc_pianist|pianist"
  "enc_pipe_burst|pipe burst"
  "enc_power_surge|power surge"
  "enc_rat_king|rat king"
  "enc_red_flare|red flare"
  "enc_rival_scavenger|rival scavenger"
  "enc_rival_trader|rival trader"
  "enc_safe_haven_ambush|safe haven ambush"
  "enc_safe_haven_empty_cache|safe haven empty cache"
  "enc_silent_death|silent death"
  "enc_sinking_mud|sinking mud"
  "enc_sleeping_ghoul|sleeping ghoul"
  "enc_tainted_stream|tainted stream"
  "enc_the_child_asks|the child asks"
  "enc_the_contraband_frequency|the contraband frequency"
  "enc_the_last_book|the last book"
  "enc_the_phantom_knock|the phantom knock"
  "enc_the_stroller|the stroller"
  "enc_the_wrong_birthday|the wrong birthday"
  "enc_vending_machine|vending machine"
  "enc_weather_station|weather station"
  "enc_who_eats|who eats"
  "enc_x|x"
)

# ── HAZARDS (15) ─────────────────────────────────────────────────────
declare -a HAZ_IDS=(
  "hazard_asbestos|asbestos"
  "hazard_cook_off|cook off"
  "hazard_drowning|drowning"
  "hazard_explosive_crafting|explosive crafting"
  "hazard_flammable_gas|flammable gas"
  "hazard_friendly_fire|friendly fire"
  "hazard_heavy|heavy"
  "hazard_id|id"
  "hazard_methane|methane"
  "hazard_mimic_crate|mimic crate"
  "hazard_sinkhole_collapse|sinkhole collapse"
  "hazard_surgical_botch|surgical botch"
  "hazard_trench_foot|trench foot"
  "hazard_weapon_burst|weapon burst"
  "hazard_zoonotic_flu|zoonotic flu"
)

# ── GENERATE ─────────────────────────────────────────────────────────
FAIL=0
DONE=0
TOTAL=$(( ${#ENC_IDS[@]} + ${#HAZ_IDS[@]} ))

log "═══════════════════════════════════════"
log "Starting batch: $TOTAL images"
log "═══════════════════════════════════════"

for entry in "${ENC_IDS[@]}"; do
  IFS='|' read -r prefix display <<< "$entry"
  prompt="2D illustration of a wasteland encounter with ${display}. A tense post-apocalyptic survival scenario in a ruined nuclear wasteland. 2D game icon art, post-apocalyptic atomic survival theme, gritty chiaroscuro dark lighting, cold desaturated colors, heavy ink outlines, detailed painterly texture, isolated on dark background."
  (( DONE++ )) || true
  log "[$DONE/$TOTAL] Generating encounter: $prefix"
  if gen "$prompt" "$ENC_DIR" "$prefix"; then
    log "  ✓ $prefix done"
  else
    (( FAIL++ )) || true
    log "  ✗ $prefix FAILED"
  fi
done

for entry in "${HAZ_IDS[@]}"; do
  IFS='|' read -r prefix display <<< "$entry"
  prompt="Icon for ${display} hazard. A dangerous environmental status symbol in a post-apocalyptic fallout shelter. 2D game icon art, post-apocalyptic atomic survival theme, gritty chiaroscuro dark lighting, cold desaturated colors, heavy ink outlines, detailed painterly texture, isolated on dark background."
  (( DONE++ )) || true
  log "[$DONE/$TOTAL] Generating hazard: $prefix"
  if gen "$prompt" "$HAZ_DIR" "$prefix"; then
    log "  ✓ $prefix done"
  else
    (( FAIL++ )) || true
    log "  ✗ $prefix FAILED"
  fi
done

log "═══════════════════════════════════════"
log "Batch complete: $DONE/$TOTAL processed, $FAIL failed"
log "Encounters: $ENC_DIR/"
log "Hazards:    $HAZ_DIR/"
log "═══════════════════════════════════════"
