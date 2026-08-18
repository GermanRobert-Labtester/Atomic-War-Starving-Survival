#!/usr/bin/env python3
"""Phase 15.4 — DRY-RUN: list every byte-identical deprecated-ammo pair
and produce a defensible move plan.

This script DOES NOT execute `git mv`. It outputs a `quarantine_plan.json`
that a human reviewer can audit before the actual move.

A file is eligible for quarantine iff:
  1. It is in assets/art/ and matches the `ammo_deprecated_*` glob.
  2. There exists at least one other file in assets/art/ that is
     SHA256-byte-identical to it.
  3. The byte-identical partner is itself real caliber-specific art
     (not a generic placeholder stem like ammo_, ammo_box, item_ammo_ap).
  4. The .import companion file (if any) is moved alongside.
  5. No src/*.cs file references the deprecated stem in a live RPG
     runtime path (test fixtures are OK — `src/Host/AssetRegistrySelfTest.cs`
     contains probes that read deprecated stems directly).

The plan records:
   - hash evidence (SHA256 of deprecated and partner)
   - src/grep evidence for live-game references
   - the proposed destination under assets/_quarantine_legacy/
   - the canonical active counterpart that remains the surviving stem.
"""
import json
import hashlib
import re
from pathlib import Path

REPO = Path("/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War")
ART = REPO / "assets/art"
QUAR = REPO / "assets/_quarantine_legacy"
SRC = REPO / "src"

GENERIC_PARTNER_STEMS = {
    "ammo_", "ammo_pistol", "ammo_rifle", "ammo_shotgun",
    "ammo_box", "ammo_box_pistol", "ammo_box_rifle", "ammo_box_rifle_hq",
    "ammo_expended", "ammo_surrendered",
    "armor_piercing_556",
    "item_ammo_ap", "item_ammo_hp", "item_ammo_standard",
    "item_ammo_types", "item_ammo_ammotypes", "ammotypes_combatloot",
    "ammo_armor_piercing_556",
    "item_id", "item_id_prefix", "item_icon", "item_patterns", "item_type",
    "item_rarity_common", "item_rarity_rare", "item_rarity_uncommon",
    "item_rarity_unique",
    "item_worldcatalog_loot",
}

# Source files that are TESTS/selftest — these are allowed to mention
# the deprecated stem names without disqualifying quarantine.
TEST_SOURCE_REGEX = re.compile(r'(Tests|Test\.cs|Editor\/)', re.I)

def sha(path):
    return hashlib.sha256(path.read_bytes()).hexdigest()

# Build hash index
art_hashes = {}
for fp in ART.iterdir():
    if fp.is_file() and fp.suffix.lower() in ('.jpg', '.png', '.jpeg', '.import'):
        art_hashes.setdefault(sha(fp), []).append(fp)

# Walk src/* to see live-runtime references
live_refs_by_stem = {}
for cs in SRC.rglob("*.cs"):
    if not cs.is_file():
        continue
    if TEST_SOURCE_REGEX.search(str(cs)):
        continue  # skip test fixtures
    try:
        text = cs.read_text()
    except Exception:
        continue
    for m in re.finditer(r'(ammo_deprecated\w+)', text):
        live_refs_by_stem[m.group(1)] = live_refs_by_stem.get(m.group(1), 0) + 1

# Walk through deprecated artifacts
plan = []
for fp in sorted(ART.glob("ammo_deprecated_*")):
    sig = sha(fp)
    partners = [p for p in art_hashes.get(sig, []) if p != fp]
    if not partners:
        continue
    real_partner = None
    for p in partners:
        if p.suffix.lower() == ".import":
            continue
        if p.stem in GENERIC_PARTNER_STEMS:
            continue
        real_partner = p
        break
    if real_partner is None:
        continue
    depr_stem = fp.stem
    live_ref_count = live_refs_by_stem.get(depr_stem, 0)
    plan.append({
        "deprecated": str(fp.relative_to(REPO)),
        "deprecated_sha256": sig,
        "deprecated_size_bytes": fp.stat().st_size,
        "active_partner": str(real_partner.relative_to(REPO)),
        "active_partner_stem": real_partner.stem,
        "active_partner_sha256": sha(real_partner),
        "live_runtime_ref_count": live_ref_count,
        "decision": "QUARANTINE_OK" if live_ref_count == 0 else "REVIEW_LIVE_REF",
        "destination": str((QUAR / fp.name).relative_to(REPO)),
        "rationale": "byte-identical to a real active caliber art; no live-runtime references" if live_ref_count == 0 else "byte-identical to a real active caliber art BUT live runtime code references this stem — defer",
    })

QUAR.mkdir(exist_ok=True, parents=True)
(REPO / "docs/visual/_phase15_quarantine_plan.json").write_text(json.dumps(plan, indent=1))
print(f"→ wrote _phase15_quarantine_plan.json ({len(plan)} entries)")

# Print summary
reviewed = sum(1 for e in plan if e["decision"] == "REVIEW_LIVE_REF")
ok = len(plan) - reviewed
print(f"  QUARANTINE_OK (no live runtime refs): {ok}")
print(f"  REVIEW_LIVE_REF: {reviewed}")

# Build a defensive quarantine script — DO NOT execute
script_lines = [
    "#!/usr/bin/env bash",
    "set -euo pipefail",
    "# Phase 15.4 quarantine script — DRY-RUN target. Review _phase15_quarantine_plan.json",
    "# before invoking. Run with --apply to actually execute.",
    f"QUAR={QUAR}",
    f"ART={ART}",
    "mkdir -p \"$QUAR\"",
    "PLAN=\"docs/visual/_phase15_quarantine_plan.json\"",
]
if ok > 0:
    script_lines.append("# Eligible moves (no live runtime refs):")
    for e in plan:
        if e["decision"] == "QUARANTINE_OK":
            script_lines.append('# {0}'.format(e["deprecated"]))
    script_lines.append("")
    script_lines.append('# Apply only with --apply:')
    script_lines.append('if [ "${1:-dry-run}" = "--apply" ]; then')
    for e in plan:
        if e["decision"] == "QUARANTINE_OK":
            src_rel = e["deprecated"]
            dst_rel = e["destination"]
            script_lines.append(f'  if [ -f "{src_rel}" ]; then git mv "{src_rel}" "assets/_quarantine_legacy/{Path(dst_rel).name}" || mv "{src_rel}" "assets/_quarantine_legacy/{Path(dst_rel).name}"; fi')
            import_basename = Path(src_rel).name + ".import"
            import_src = f"assets/art/{import_basename}"
            script_lines.append(f'  if [ -f "{import_src}" ]; then mv "{import_src}" "assets/_quarantine_legacy/{import_basename}" || true; fi')
    script_lines.append('else')
    script_lines.append('  echo "[quarantine] DRY-RUN. invoke with --apply to execute."')
    script_lines.append('fi')

(REPO / "scripts/ci/quarantine_deprecated_ammo.sh").parent.mkdir(exist_ok=True, parents=True)
(REPO / "scripts/ci/quarantine_deprecated_ammo.sh").write_text("\n".join(script_lines))
print(f"→ wrote scripts/ci/quarantine_deprecated_ammo.sh ({ok} eligible)")

# Top 20 entries
print("\n=== Top 20 entries ===")
for e in plan[:20]:
    print(f"  [{e['decision']}] {e['deprecated']}")
    print(f"    ↔ {e['active_partner']}")
    print(f"    live-runtime refs: {e['live_runtime_ref_count']}")
