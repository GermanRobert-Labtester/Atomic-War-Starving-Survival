#!/usr/bin/env python3
"""Deterministic Wave-1 localization drift gate.

The gate intentionally checks only the two pilot surfaces. Later UI waves are
inventory/roadmap work and must not be silently classified as localized.
"""

from __future__ import annotations

import csv
import re
import sys
from pathlib import Path


ROOT = Path(__file__).resolve().parents[2]
CSV_PATH = ROOT / "assets" / "l10n" / "strings.csv"
ONBOARDING_CATALOG = ROOT / "Assets" / "Ashfall.Core" / "Onboarding" / "OnboardingJourney.cs"
PILOTS = [
    ROOT / "src" / "UI" / "ResearchPanel.cs",
    ROOT / "src" / "UI" / "OnboardingHintPanel.cs",
    ROOT / "src" / "Main.Onboarding.cs",
]


def placeholders(value: str) -> set[str]:
    return set(re.findall(r"\{([A-Za-z_][A-Za-z0-9_]*|\d+)(?::[^}]*)?\}", value))


def stage_family_keys(pilot_source: str) -> set[str]:
    """Derive the dynamically constructed onboarding stage keys from the
    authoritative catalog instead of a hardcoded list that can silently drift
    when a stage is added (the ae6e54387 "HINT: —" failure class).

    ``OnboardingCatalog.Order``/``FirstHour`` build their title/objective keys
    at runtime from each stage id; ``OnboardingHintPanel.StageHintCopy`` holds
    the per-stage hint keys as literals. Both are enumerated here so a new
    stage without its localization rows fails the gate.
    """
    catalog = ONBOARDING_CATALOG.read_text(encoding="utf-8")
    stages = re.findall(
        r"new\s+OnboardingStageDef\(\s*OnboardingStage\.([A-Za-z0-9_]+)",
        catalog,
    )
    if not stages:
        raise AssertionError(
            "could not enumerate onboarding stages from "
            f"{ONBOARDING_CATALOG.relative_to(ROOT)}"
        )

    def normalize(stage: str) -> str:
        # Mirrors OnboardingHintPanel.StageLocalizationId.
        if stage == "InventoryUse":
            return "inventory_use"
        if stage == "DayAdvance":
            return "day_advance"
        return stage.lower()

    keys: set[str] = set()
    for stage in stages:
        stem = f"onboarding.{normalize(stage)}"
        keys.add(f"{stem}.title")
        keys.add(f"{stem}.objective")
    keys.update(re.findall(r'"(onboarding\.hint\.[A-Za-z0-9_]+)"', pilot_source))
    return keys


def load_rows() -> dict[str, tuple[str, str]]:
    rows: dict[str, tuple[str, str]] = {}
    with CSV_PATH.open(encoding="utf-8", newline="") as handle:
        for row in csv.DictReader(handle):
            key = (row.get("key") or "").strip()
            if not key:
                continue
            if key in rows:
                raise AssertionError(f"duplicate localization key: {key}")
            rows[key] = ((row.get("en") or ""), (row.get("de") or ""))
    return rows


def main() -> int:
    try:
        rows = load_rows()
        source = "\n".join(path.read_text(encoding="utf-8") for path in PILOTS)
        referenced = set(
            re.findall(
                r'(?:AshfallLocalization\.)?(?:Tr|TrFormat|T|F)\(\s*"([^"]+)"',
                source,
            )
        )
        # Include the dynamically constructed stage-key family. It is derived
        # from the authoritative OnboardingCatalog (legacy + first-hour) and
        # the panel's hint map, so adding a stage without its localization rows
        # fails this gate instead of shipping the ae6e54387 "HINT: —" class.
        referenced.update(stage_family_keys(source))
        missing = sorted(key for key in referenced if key not in rows)
        if missing:
            raise AssertionError("pilot keys missing from strings.csv: " + ", ".join(missing))

        missing_de = sorted(key for key in referenced if not rows[key][1])
        if missing_de:
            raise AssertionError("pilot keys missing German translations: " + ", ".join(missing_de))

        mismatched = sorted(
            key
            for key, (english, german) in rows.items()
            if german and placeholders(english) != placeholders(german)
        )
        if mismatched:
            raise AssertionError("placeholder mismatch: " + ", ".join(mismatched))

        for path in PILOTS:
            text = path.read_text(encoding="utf-8")
            if re.search(r"\b(?:Text|TooltipText)\s*=\s*\"[^\"$]*\"", text):
                raise AssertionError(f"literal UI text assignment remains in pilot: {path.relative_to(ROOT)}")

        print(
            f"L10N_DRIFT_GATE PASS — {len(rows)} keys, "
            f"{len(referenced)} pilot references, German parity verified"
        )
        return 0
    except (OSError, AssertionError) as exc:
        print(f"L10N_DRIFT_GATE FAIL — {exc}", file=sys.stderr)
        return 1


if __name__ == "__main__":
    raise SystemExit(main())
