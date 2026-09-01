#!/usr/bin/env python3
"""Safe ASHFALL ElevenLabs SFX planning, candidate generation, and acceptance.

The authenticated ElevenLabs MCP is the primary generation transport. This
script deliberately keeps network generation behind an explicit ``--execute``
flag as a direct-SDK fallback. Generated candidates go to a temporary review
directory and are never written into ``assets/audio`` automatically.

Accepted runtime assets must be mastered WAV or OGG files. Acceptance is an
explicit, non-overwriting operation that records provenance in the SFX
manifest. No command prints, stores, or writes an API key.
"""

from __future__ import annotations

import argparse
import hashlib
import json
import os
from pathlib import Path
import re
import shutil
import sys
import tempfile
import time
from dataclasses import dataclass
from datetime import datetime, timezone
from typing import Iterable, Sequence


REPO_ROOT = Path(__file__).resolve().parents[1]
RUNTIME_DIR = REPO_ROOT / "assets" / "audio" / "sfx"
MANIFEST_PATH = RUNTIME_DIR / "sfx_manifest.json"
COMBAT_CATALOG_PATH = REPO_ROOT / "Assets" / "StreamingAssets" / "Data" / "combat_catalog.json"
ITEMS_CATALOG_PATH = REPO_ROOT / "Assets" / "StreamingAssets" / "Data" / "items.json"
DEFAULT_CANDIDATE_DIR = Path(tempfile.gettempdir()) / "ashfall_elevenlabs_sfx_candidates"
DIRECT_API_OUTPUT_FORMAT = "mp3_44100_128"
ID_PATTERN = re.compile(r"^sfx_[a-z0-9_]+$")
RUNTIME_SUFFIXES = {".wav", ".ogg"}
NO_VOICE_SUFFIX = (
    " No voices, speech, music, or singing."
)


@dataclass(frozen=True)
class SfxSpec:
    id: str
    prompt: str
    duration_seconds: float
    category: str
    weapon_id: str | None = None
    role: str | None = None


@dataclass(frozen=True)
class WeaponSoundIdentity:
    weapon_id: str
    display_name: str
    construction: str
    mechanism: str
    acoustic_signature: str


# Existing accepted concepts. These entries are generation specifications, not
# permission to overwrite the shipped files bearing the same IDs.
SHIPPED_SFX_CATALOG: tuple[SfxSpec, ...] = (
    SfxSpec("sfx_bunker_door_open", "Heavy steel bunker door grinding open with hydraulic hiss, echoing in a concrete corridor, cold industrial post-apocalyptic mechanism", 3.0, "shelter"),
    SfxSpec("sfx_bunker_door_seal", "Airtight steel door sealing shut with a pneumatic clank and rubber gasket compression, muffled concrete echo", 2.0, "shelter"),
    SfxSpec("sfx_ventilation_fan", "Constant low industrial ventilation fan humming in an enclosed concrete space with a slight metallic rattle", 5.0, "shelter"),
    SfxSpec("sfx_generator_cough", "Diesel generator struggling to start, coughing and sputtering, then settling into an uneven mechanical rumble underground", 4.0, "shelter"),
    SfxSpec("sfx_pipe_clang", "Single metallic pipe clang echoing through an empty concrete bunker corridor, cold industrial reverb", 1.5, "shelter"),
    SfxSpec("sfx_water_drip_cave", "Slow water droplets falling into a shallow puddle in an underground concrete chamber, cold damp echoes", 4.0, "shelter"),
    SfxSpec("sfx_geiger_burst", "Intense Geiger counter crackling burst, rapid radioactive clicks escalating and fading, scientific instrument", 2.5, "radiation"),
    SfxSpec("sfx_radiation_alarm", "Harsh electronic radiation alarm pulsing with urgent industrial warning beeps in a bunker", 3.0, "radiation"),
    SfxSpec("sfx_contamination_warning", "Low ominous electronic hum building into an urgent contamination warning tone in a cold industrial facility", 2.5, "radiation"),
    SfxSpec("sfx_air_filter_degrade", "Air filtration system struggling, fan motor whining higher as air pressure drops, enclosed mechanical distress", 3.5, "radiation"),
    SfxSpec("sfx_wind_gust_harsh", "Harsh nuclear-winter wind gust howling past concrete structures while carrying grit and debris", 4.0, "weather"),
    SfxSpec("sfx_fallout_storm_approach", "Distant approaching fallout storm, low thunder mixed with abrasive wind and an ominous pressure change", 5.0, "weather"),
    SfxSpec("sfx_debris_impact", "Heavy debris striking concrete, rubble fragments falling through a dry dust cloud", 1.5, "danger"),
    SfxSpec("sfx_item_pickup_metal", "Small metal survival tool picked up and placed into a pack, concise metallic equipment clink", 0.8, "action"),
    SfxSpec("sfx_crafting_assemble", "Hands assembling an improvised device, small tools clicking and mismatched metal parts fitting together", 3.0, "action"),
    SfxSpec("sfx_repair_wrench", "Wrench turning a stubborn bolt, metal-on-metal tightening during industrial maintenance", 2.0, "action"),
    SfxSpec("sfx_trade_exchange", "Heavy packaged goods changing hands across a rough wooden counter, restrained barter foley", 1.5, "action"),
    SfxSpec("sfx_water_pour", "Clean water measured from a metal container into a cup, close dry-room perspective", 2.0, "action"),
    SfxSpec("sfx_pill_bottle", "Plastic pill bottle opened and tablets rattling briefly, close survival-medicine foley", 1.5, "medical"),
    SfxSpec("sfx_distant_explosion", "Very distant heavy explosion muffled by thick concrete walls, restrained low-frequency thump", 3.0, "danger"),
    SfxSpec("sfx_alarm_klaxon", "Emergency mechanical klaxon blaring in an underground bunker, urgent rotating industrial siren", 4.0, "danger"),
    SfxSpec("sfx_glass_break_small", "Small medical glass vial breaking on a concrete floor, sharp concise crystalline shatter", 1.0, "danger"),
    SfxSpec("sfx_radio_tune", "Old analog radio tuned through dry static, frequency sweep and mechanical dial movement", 3.0, "radio"),
    SfxSpec("sfx_radio_signal_lock", "Old radio finding a stable signal through static, noise narrowing into clear carrier tone and Morse pulses", 2.5, "radio"),
    SfxSpec("sfx_morse_key", "Morse telegraph key pressed in a rapid electrical clicking pattern on old communications equipment", 2.0, "radio"),
    SfxSpec("sfx_heartbeat_slow", "Slow heavy human heartbeat, low-frequency medical thumps from an exhausted body", 3.0, "medical"),
    SfxSpec("sfx_coughing_fit", "Short severe nonverbal coughing fit from respiratory illness, no words or vocal performance", 2.5, "medical"),
    SfxSpec("sfx_injection", "Medical syringe injection, small needle puncture and plunger pressing liquid", 1.5, "medical"),
)


# Every firearm must receive a distinct identity before a report is generated.
# DIY mechanisms remain acoustically separate from factory-made weapons even
# when their ammunition is similar.
WEAPON_SOUND_IDENTITIES: tuple[WeaponSoundIdentity, ...] = (
    WeaponSoundIdentity(
        "pistol_cz75_9x19", "CZ 75 Pistol", "factory",
        "steel locked-breech semi-automatic pistol in 9x19mm",
        "compact sharp crack, solid steel slide cycle, controlled short tail",
    ),
    WeaponSoundIdentity(
        "weapon_pipe_rifle", "Pipe Rifle", "diy",
        "hand-built single-shot pipe receiver firing .357 ammunition",
        "uneven high-pressure bark, loose tube resonance, crude latch slap and metallic after-rattle",
    ),
    WeaponSoundIdentity(
        "weapon_scrap_shotgun", "Scrap Shotgun", "diy",
        "hand-built break-action double-barrel smoothbore firing 12-gauge shells",
        "broad dirty boom, flexing sheet-metal resonance, loose chamber clatter and irregular decay",
    ),
    WeaponSoundIdentity(
        "weapon_bolt_rifle", "Held-Bolt Rifle", "factory",
        "worn military-surplus bolt-action rifle firing .308 ammunition",
        "dry powerful rifle crack, long outdoor tail, heavy machined bolt lift and positive lock",
    ),
    WeaponSoundIdentity(
        "weapon_assault_rifle", "Assault Rifle", "factory",
        "select-fire service rifle firing 5.56mm ammunition",
        "fast bright three-round report, compact gas action, tight consistent mechanical cadence",
    ),
    WeaponSoundIdentity(
        "weapon_lmg", "Light Machine Gun", "factory",
        "belt-or-box-fed light machine gun firing 7.62mm ammunition",
        "deep sustained five-round burst, heavy reciprocating action, slower authoritative cadence",
    ),
)


# Batch combat_01: twelve review candidates, two per authoritative named gun.
# Reports and actions are separate generations so the model receives one clear
# sound event at a time and the runtime can later trigger them independently.
COMBAT_BATCH_1_SPECS: tuple[SfxSpec, ...] = (
    SfxSpec(
        "sfx_weapon_cz75_report",
        "Single close-miked CZ 75 9x19mm pistol shot, compact sharp crack, solid steel slide snap, controlled short outdoor tail",
        1.2,
        "combat_weapon",
        weapon_id="pistol_cz75_9x19",
        role="report",
    ),
    SfxSpec(
        "sfx_weapon_pipe_rifle_report",
        "Single hand-built .357 pipe rifle shot, uneven high-pressure bark, hollow loose steel tube ring, crude latch slap, irregular metallic after-rattle",
        1.6,
        "combat_weapon",
        weapon_id="weapon_pipe_rifle",
        role="report",
    ),
    SfxSpec(
        "sfx_weapon_scrap_shotgun_report",
        "Single handmade 12-gauge scrap double-barrel shotgun blast, broad dirty boom, flexing sheet-metal ring, loose chamber clatter, irregular decay",
        1.8,
        "combat_weapon",
        weapon_id="weapon_scrap_shotgun",
        role="report",
    ),
    SfxSpec(
        "sfx_weapon_bolt_rifle_report",
        "Single worn military-surplus .308 bolt-action rifle shot, dry powerful crack, deep body, long open-air tail, report only",
        1.6,
        "combat_weapon",
        weapon_id="weapon_bolt_rifle",
        role="report",
    ),
    SfxSpec(
        "sfx_weapon_assault_rifle_burst",
        "Three-shot 5.56 service rifle burst, fast bright reports, compact gas action, tight consistent cadence, dry outdoor range",
        2.2,
        "combat_weapon",
        weapon_id="weapon_assault_rifle",
        role="report",
    ),
    SfxSpec(
        "sfx_weapon_lmg_burst",
        "Five-shot 7.62 light machine gun burst, deep heavy reports, slower authoritative cadence, forceful reciprocating action, dry outdoor range",
        3.0,
        "combat_weapon",
        weapon_id="weapon_lmg",
        role="report",
    ),
    SfxSpec(
        "sfx_weapon_cz75_reload",
        "CZ 75 pistol reload, steel magazine released, fresh magazine seated, solid slide rack, precise machined clicks, close-miked",
        2.5,
        "combat_weapon",
        weapon_id="pistol_cz75_9x19",
        role="action",
    ),
    SfxSpec(
        "sfx_weapon_pipe_rifle_reload",
        "Hand-built .357 pipe rifle reload, crude latch opens, spent case pulled by hand, loose cartridge inserted, hollow tube closes with an irregular metal clack",
        3.2,
        "combat_weapon",
        weapon_id="weapon_pipe_rifle",
        role="action",
    ),
    SfxSpec(
        "sfx_weapon_scrap_shotgun_reload",
        "Handmade double-barrel scrap shotgun reload, stiff break-action hinge, two shells ejected, fresh shells seated, warped barrels clack shut",
        4.0,
        "combat_weapon",
        weapon_id="weapon_scrap_shotgun",
        role="action",
    ),
    SfxSpec(
        "sfx_weapon_bolt_rifle_cycle",
        "Worn .308 bolt-action rifle cycle, heavy machined bolt lift, long steel pull, brass case ejection, positive lock, dry close-miked mechanism",
        2.4,
        "combat_weapon",
        weapon_id="weapon_bolt_rifle",
        role="action",
    ),
    SfxSpec(
        "sfx_weapon_assault_rifle_reload",
        "5.56 service rifle reload, stamped magazine release, fresh magazine seated, charging handle snap, tight consistent machined action",
        3.0,
        "combat_weapon",
        weapon_id="weapon_assault_rifle",
        role="action",
    ),
    SfxSpec(
        "sfx_weapon_lmg_reload",
        "7.62 light machine gun reload, heavy feed cover opens, metal belt laid into tray, cover slams shut, charging handle pulled, weighty machined clacks",
        4.5,
        "combat_weapon",
        weapon_id="weapon_lmg",
        role="action",
    ),
)

BATCH_CATALOG: dict[str, tuple[SfxSpec, ...]] = {
    "combat_01": COMBAT_BATCH_1_SPECS,
}
SFX_CATALOG: tuple[SfxSpec, ...] = SHIPPED_SFX_CATALOG + COMBAT_BATCH_1_SPECS


def sha256_file(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open("rb") as handle:
        for chunk in iter(lambda: handle.read(1024 * 1024), b""):
            digest.update(chunk)
    return digest.hexdigest()


def build_prompt(spec: SfxSpec) -> str:
    return spec.prompt.rstrip(". ") + "." + NO_VOICE_SUFFIX


def catalog_by_id() -> dict[str, SfxSpec]:
    return {spec.id: spec for spec in SFX_CATALOG}


def load_authoritative_weapons() -> tuple[dict[str, str], list[str]]:
    weapons: dict[str, str] = {}
    errors: list[str] = []
    for path, collection_key, name_key in (
        (COMBAT_CATALOG_PATH, "weapons", "display_name"),
        (ITEMS_CATALOG_PATH, "items", "displayName"),
    ):
        try:
            payload = json.loads(path.read_text(encoding="utf-8"))
        except (OSError, json.JSONDecodeError) as exc:
            errors.append(f"cannot read authoritative weapon catalog {path}: {exc}")
            continue

        entries = payload.get(collection_key, [])
        if not isinstance(entries, list):
            errors.append(f"authoritative weapon collection is not an array: {path}")
            continue
        for entry in entries:
            if not isinstance(entry, dict):
                continue
            if collection_key == "items" and str(entry.get("type", "")).casefold() != "weapon":
                continue
            weapon_id = str(entry.get("id", ""))
            display_name = str(entry.get(name_key, ""))
            if not weapon_id or not display_name:
                errors.append(f"weapon entry lacks id/display name: {path}")
                continue
            previous = weapons.get(weapon_id)
            if previous is not None and previous != display_name:
                errors.append(f"weapon display-name conflict for {weapon_id}: {previous!r} vs {display_name!r}")
            weapons[weapon_id] = display_name
    return weapons, errors


def validate_catalog() -> list[str]:
    errors: list[str] = []
    ids: set[str] = set()
    prompts: set[str] = set()
    for spec in SFX_CATALOG:
        if not ID_PATTERN.fullmatch(spec.id):
            errors.append(f"invalid SFX id: {spec.id}")
        if spec.id in ids:
            errors.append(f"duplicate SFX id: {spec.id}")
        ids.add(spec.id)
        normalized_prompt = build_prompt(spec).casefold()
        if normalized_prompt in prompts:
            errors.append(f"duplicate generation prompt: {spec.id}")
        prompts.add(normalized_prompt)
        if not 0.5 <= spec.duration_seconds <= 8.0:
            errors.append(f"duration outside 0.5-8.0 seconds: {spec.id}")

    authoritative_weapons, authority_errors = load_authoritative_weapons()
    errors.extend(authority_errors)
    weapon_ids: set[str] = set()
    signatures: set[str] = set()
    for identity in WEAPON_SOUND_IDENTITIES:
        if identity.weapon_id in weapon_ids:
            errors.append(f"duplicate weapon identity: {identity.weapon_id}")
        weapon_ids.add(identity.weapon_id)
        if identity.construction not in {"diy", "factory"}:
            errors.append(f"invalid weapon construction: {identity.weapon_id}")
        signature = identity.acoustic_signature.casefold()
        if signature in signatures:
            errors.append(f"weapon acoustic signature is not unique: {identity.weapon_id}")
        signatures.add(signature)
        if not identity.mechanism.strip() or not identity.acoustic_signature.strip():
            errors.append(f"incomplete weapon sound identity: {identity.weapon_id}")

        authoritative_name = authoritative_weapons.get(identity.weapon_id)
        if authoritative_name is not None and authoritative_name != identity.display_name:
            errors.append(
                f"weapon display-name drift for {identity.weapon_id}: "
                f"identity={identity.display_name!r} authority={authoritative_name!r}"
            )

    missing_identities = sorted(set(authoritative_weapons) - weapon_ids)
    if missing_identities:
        errors.append("named weapons missing sound identities: " + ", ".join(missing_identities))
    orphan_identities = sorted(weapon_ids - set(authoritative_weapons))
    if orphan_identities:
        errors.append("weapon sound identities absent from authority: " + ", ".join(orphan_identities))

    roles_by_weapon: dict[str, set[str]] = {weapon_id: set() for weapon_id in weapon_ids}
    constructions = {identity.weapon_id: identity.construction for identity in WEAPON_SOUND_IDENTITIES}
    for spec in COMBAT_BATCH_1_SPECS:
        if spec.weapon_id not in weapon_ids:
            errors.append(f"combat batch SFX has unknown weapon identity: {spec.id}")
            continue
        if spec.role not in {"report", "action"}:
            errors.append(f"combat batch SFX has invalid role: {spec.id}")
            continue
        roles_by_weapon[spec.weapon_id].add(spec.role)

        prompt = spec.prompt.casefold()
        is_diy_prompt = any(token in prompt for token in ("hand-built", "handmade", "crude", "scrap"))
        if constructions[spec.weapon_id] == "diy" and not is_diy_prompt:
            errors.append(f"DIY weapon prompt lacks handmade construction texture: {spec.id}")
        if constructions[spec.weapon_id] == "factory" and is_diy_prompt:
            errors.append(f"factory weapon prompt contains DIY construction texture: {spec.id}")

    for weapon_id, roles in roles_by_weapon.items():
        if roles != {"report", "action"}:
            errors.append(f"weapon lacks report/action SFX pair: {weapon_id}")
    return errors


def validate_manifest() -> list[str]:
    errors: list[str] = []
    if not MANIFEST_PATH.is_file():
        return [f"manifest missing: {MANIFEST_PATH}"]
    try:
        manifest = json.loads(MANIFEST_PATH.read_text(encoding="utf-8"))
    except (OSError, json.JSONDecodeError) as exc:
        return [f"manifest unreadable: {exc}"]

    assets = manifest.get("assets")
    if not isinstance(assets, list):
        return ["manifest assets must be an array"]
    if manifest.get("total") != len(assets):
        errors.append("manifest total does not match assets length")
    seen: set[str] = set()
    for item in assets:
        asset_id = item.get("id", "") if isinstance(item, dict) else ""
        file_name = item.get("file", "") if isinstance(item, dict) else ""
        if asset_id in seen:
            errors.append(f"manifest duplicate id: {asset_id}")
        seen.add(asset_id)
        if not (RUNTIME_DIR / file_name).is_file():
            errors.append(f"manifest file missing: {file_name}")
    return errors


def select_specs(ids: Sequence[str], select_all: bool, batch_id: str | None) -> list[SfxSpec]:
    if not ids and not select_all and batch_id is None:
        raise ValueError("select at least one --id, an explicit --batch, or pass --all")
    if batch_id is not None:
        return list(BATCH_CATALOG[batch_id])
    known = catalog_by_id()
    if select_all:
        return list(SFX_CATALOG)
    missing = sorted(set(ids) - set(known))
    if missing:
        raise ValueError("unknown SFX id(s): " + ", ".join(missing))
    selected: list[SfxSpec] = []
    added: set[str] = set()
    for asset_id in ids:
        if asset_id not in added:
            selected.append(known[asset_id])
            added.add(asset_id)
    return selected


def print_plan(specs: Iterable[SfxSpec], candidate_dir: Path) -> None:
    for spec in specs:
        output = candidate_dir / f"{spec.id}.mp3"
        state = "SKIP (candidate exists)" if output.exists() else "READY"
        print(f"{spec.id}\t{spec.duration_seconds:.1f}s\t{spec.category}\t{state}")
        print(f"  candidate: {output}")
        print(f"  prompt: {build_prompt(spec)}")


def generate_candidates(specs: Sequence[SfxSpec], candidate_dir: Path, force: bool) -> int:
    api_key = os.environ.get("ELEVENLABS_API_KEY", "")
    if not api_key:
        print("ERROR: direct fallback requires ELEVENLABS_API_KEY", file=sys.stderr)
        print("Use the authenticated ElevenLabs MCP when available.", file=sys.stderr)
        return 2
    try:
        from elevenlabs import ElevenLabs
    except ImportError:
        print("ERROR: elevenlabs SDK is not installed", file=sys.stderr)
        return 2

    candidate_dir.mkdir(parents=True, exist_ok=True)
    client = ElevenLabs(api_key=api_key)
    failures = 0
    for index, spec in enumerate(specs):
        output = candidate_dir / f"{spec.id}.mp3"
        if output.exists() and not force:
            print(f"SKIP: {spec.id} candidate already exists: {output}")
            continue
        print(f"GENERATE: {spec.id} ({spec.duration_seconds:.1f}s) -> {output}")
        try:
            chunks = client.text_to_sound_effects.convert(
                text=build_prompt(spec),
                duration_seconds=spec.duration_seconds,
                output_format=DIRECT_API_OUTPUT_FORMAT,
                model_id="eleven_text_to_sound_v2",
            )
            payload = b"".join(chunks)
            temporary = output.with_suffix(output.suffix + ".tmp")
            temporary.write_bytes(payload)
            os.replace(temporary, output)
            print(f"  saved candidate: bytes={len(payload)} sha256={sha256_file(output)}")
        except Exception as exc:  # SDK/network errors are reported per candidate.
            failures += 1
            print(f"  FAILED: {spec.id}: {exc}", file=sys.stderr)
        if index < len(specs) - 1:
            time.sleep(1.0)
    return 1 if failures else 0


def load_manifest() -> dict:
    try:
        return json.loads(MANIFEST_PATH.read_text(encoding="utf-8"))
    except (OSError, json.JSONDecodeError) as exc:
        raise ValueError(f"cannot load manifest: {exc}") from exc


def write_manifest_atomic(manifest: dict) -> None:
    temporary = MANIFEST_PATH.with_suffix(MANIFEST_PATH.suffix + ".tmp")
    temporary.write_text(json.dumps(manifest, indent=2, ensure_ascii=False) + "\n", encoding="utf-8")
    os.replace(temporary, MANIFEST_PATH)


def accept_candidate(
    asset_id: str,
    candidate: Path,
    runtime_name: str | None,
    replace: bool,
    transport: str,
) -> int:
    known = catalog_by_id()
    if asset_id not in known:
        print(f"ERROR: unknown SFX id: {asset_id}", file=sys.stderr)
        return 2
    candidate = candidate.resolve()
    if not candidate.is_file():
        print(f"ERROR: candidate does not exist: {candidate}", file=sys.stderr)
        return 2
    if candidate.suffix.casefold() not in RUNTIME_SUFFIXES:
        print("ERROR: accepted runtime assets must be mastered .wav or .ogg files", file=sys.stderr)
        return 2

    file_name = runtime_name or f"{asset_id}{candidate.suffix.casefold()}"
    destination = RUNTIME_DIR / file_name
    if destination.suffix.casefold() not in RUNTIME_SUFFIXES or destination.name != file_name:
        print("ERROR: runtime name must be a plain .wav or .ogg filename", file=sys.stderr)
        return 2
    if destination.exists() and not replace:
        print(f"ERROR: runtime asset exists; refusing overwrite: {destination}", file=sys.stderr)
        return 2

    manifest = load_manifest()
    accepted = manifest.setdefault("accepted_generations", [])
    prior = [entry for entry in accepted if entry.get("id") == asset_id]
    if prior and not replace:
        print(f"ERROR: manifest already has an accepted generation for {asset_id}", file=sys.stderr)
        return 2

    source_hash = sha256_file(candidate)
    temporary = destination.with_suffix(destination.suffix + ".tmp")
    RUNTIME_DIR.mkdir(parents=True, exist_ok=True)
    shutil.copyfile(candidate, temporary)
    os.replace(temporary, destination)
    runtime_hash = sha256_file(destination)

    accepted[:] = [entry for entry in accepted if entry.get("id") != asset_id]
    accepted.append({
        "id": asset_id,
        "file": destination.name,
        "transport": transport,
        "accepted_at_utc": datetime.now(timezone.utc).isoformat(),
        "prompt": build_prompt(known[asset_id]),
        "duration_target": known[asset_id].duration_seconds,
        "source_sha256": source_hash,
        "runtime_sha256": runtime_hash,
        "runtime_bytes": destination.stat().st_size,
    })
    accepted.sort(key=lambda entry: entry.get("id", ""))
    manifest["schema_version"] = 2
    write_manifest_atomic(manifest)
    print(f"ACCEPTED: {asset_id} -> {destination}")
    print(f"  sha256={runtime_hash}")
    return 0


def add_selection_arguments(parser: argparse.ArgumentParser) -> None:
    selection = parser.add_mutually_exclusive_group()
    selection.add_argument("--id", action="append", default=[], help="select one SFX ID; repeatable")
    selection.add_argument("--batch", choices=sorted(BATCH_CATALOG), help="select one reviewed batch specification")
    selection.add_argument("--all", action="store_true", help="select every catalog entry explicitly")
    parser.add_argument("--candidate-dir", type=Path, default=DEFAULT_CANDIDATE_DIR)


def build_parser() -> argparse.ArgumentParser:
    parser = argparse.ArgumentParser(description=__doc__)
    subparsers = parser.add_subparsers(dest="command", required=True)

    subparsers.add_parser("validate", help="validate catalog, weapon identities, manifest, and shipped files")
    list_parser = subparsers.add_parser("list", help="list SFX specifications or weapon identities")
    list_parser.add_argument("--weapons", action="store_true", help="list the named-weapon sound contract")

    plan_parser = subparsers.add_parser("plan", help="print selected prompts without network or file writes")
    add_selection_arguments(plan_parser)

    generate_parser = subparsers.add_parser("generate", help="direct-SDK fallback; dry unless --execute is supplied")
    add_selection_arguments(generate_parser)
    generate_parser.add_argument("--execute", action="store_true", help="allow direct ElevenLabs API requests")
    generate_parser.add_argument("--force-candidate", action="store_true", help="replace an existing temporary candidate")

    accept_parser = subparsers.add_parser("accept", help="copy one reviewed WAV/OGG candidate into runtime assets")
    accept_parser.add_argument("--id", required=True)
    accept_parser.add_argument("--candidate", required=True, type=Path)
    accept_parser.add_argument("--runtime-name")
    accept_parser.add_argument("--replace", action="store_true", help="replace an existing accepted runtime asset explicitly")
    accept_parser.add_argument("--transport", default="elevenlabs_mcp_oauth")
    return parser


def main(argv: Sequence[str] | None = None) -> int:
    args = build_parser().parse_args(argv)

    if args.command == "validate":
        errors = validate_catalog() + validate_manifest()
        if errors:
            for error in errors:
                print(f"ERROR: {error}", file=sys.stderr)
            return 1
        print(
            f"PASS: specs={len(SFX_CATALOG)} weapon_identities={len(WEAPON_SOUND_IDENTITIES)} "
            f"manifest={MANIFEST_PATH}"
        )
        return 0

    if args.command == "list":
        if args.weapons:
            for identity in WEAPON_SOUND_IDENTITIES:
                print(
                    f"{identity.weapon_id}\t{identity.display_name}\t{identity.construction}\n"
                    f"  mechanism: {identity.mechanism}\n"
                    f"  signature: {identity.acoustic_signature}"
                )
        else:
            for spec in SFX_CATALOG:
                print(f"{spec.id}\t{spec.duration_seconds:.1f}s\t{spec.category}")
        return 0

    if args.command in {"plan", "generate"}:
        try:
            selected = select_specs(args.id, args.all, args.batch)
        except ValueError as exc:
            print(f"ERROR: {exc}", file=sys.stderr)
            return 2
        candidate_dir = args.candidate_dir.resolve()
        if args.command == "plan" or not args.execute:
            print_plan(selected, candidate_dir)
            if args.command == "generate":
                print("DRY RUN: pass --execute to permit direct API requests")
            return 0
        return generate_candidates(selected, candidate_dir, args.force_candidate)

    if args.command == "accept":
        return accept_candidate(
            args.id,
            args.candidate,
            args.runtime_name,
            args.replace,
            args.transport,
        )

    return 2


if __name__ == "__main__":
    raise SystemExit(main())
