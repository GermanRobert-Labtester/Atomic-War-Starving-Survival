#!/usr/bin/env python3
"""Deterministic Wave-1 localization drift gate.

The gate intentionally checks only the two pilot surfaces. Later UI waves are
inventory/roadmap work and must not be silently classified as localized.
"""

from __future__ import annotations

import csv
import json
import re
import sys
from pathlib import Path


ROOT = Path(__file__).resolve().parents[2]
CSV_PATH = ROOT / "assets" / "l10n" / "strings.csv"
ONBOARDING_CATALOG = ROOT / "Assets" / "Ashfall.Core" / "Onboarding" / "OnboardingJourney.cs"
ONBOARDING_HINT_PANEL = ROOT / "src" / "UI" / "OnboardingHintPanel.cs"
ACHIEVEMENTS_CATALOG = ROOT / "Assets" / "StreamingAssets" / "Data" / "achievements.json"
MICRO_LOCATIONS_CATALOG = ROOT / "Assets" / "StreamingAssets" / "Data" / "micro_locations.json"
PILOTS = [
    ROOT / "src" / "UI" / "ResearchPanel.cs",
    ROOT / "src" / "UI" / "OnboardingHintPanel.cs",
    ROOT / "src" / "Main.Onboarding.cs",
    # Survival-legibility HUD: after the P009 localization pass every Text
    # assignment routes through Tr/TrFormat, so a new hardcoded label must fail.
    ROOT / "src" / "UI" / "GameHudOverlay.cs",
]

# Later localization waves route player-facing strings through
# AshfallLocalization.Tr. Every key referenced by these surfaces must exist in
# strings.csv with German parity, so adding a new Tr(...) call without its row
# fails the gate instead of silently shipping an untranslated key.
LOCALIZED_SURFACES = [
    ROOT / "src" / "UI" / "WorkshopPanel.cs",
    ROOT / "src" / "UI" / "VisitorIntegrationPanel.cs",
    ROOT / "src" / "UI" / "ShelterHudPanel.cs",
    ROOT / "src" / "UI" / "DutyRosterPanel.cs",
    ROOT / "src" / "UI" / "SilentFoundryPanel.cs",
    ROOT / "src" / "UI" / "SkillMatrixPanel.cs",
    ROOT / "src" / "UI" / "TriangulationPanel.cs",
    ROOT / "src" / "UI" / "DifficultySettingsPanel.cs",
    # Survival-legibility surfaces: the needs glance row and the status panel's
    # threshold/drift sections route through AshfallLocalization.Tr; every key
    # must exist in strings.csv with German parity.
    ROOT / "src" / "UI" / "GameHudOverlay.cs",
    ROOT / "src" / "UI" / "StatusPanel.cs",
    # Cohort truth/localization wave 2026-10-02: SurvivalDetailPanel used to
    # hardcode every row in English and band against literals that contradicted
    # NeedsProfile. It now routes through AshfallUiText, so a new untranslated
    # row must fail the gate instead of shipping silently.
    ROOT / "src" / "UI" / "SurvivalDetailPanel.cs",
    # Ninth-wave surfaces 2026-10-02: the achievements/afflictions/medical panels
    # and the per-survivor detail panel now route their player-facing rows through
    # AshfallUiText; register them so a new untranslated key fails the gate.
    ROOT / "src" / "UI" / "AchievementsPanel.cs",
    ROOT / "src" / "UI" / "AfflictionsPanel.cs",
    ROOT / "src" / "UI" / "MedicalPanel.cs",
    ROOT / "src" / "UI" / "SurvivorDetailPanel.cs",
    # Eleventh-wave surfaces 2026-10-02: the cohort selector, dashboard, vigil
    # formatter, expedition panel, and survivors panel all resolve through
    # AshfallUiText; register them so a new untranslated key fails the gate.
    ROOT / "src" / "UI" / "ExpeditionPanel.cs",
    ROOT / "src" / "UI" / "GameDashboardPanel.cs",
    ROOT / "src" / "UI" / "MedicalVigilText.cs",
    ROOT / "src" / "UI" / "StartingCohortSetupPanel.cs",
    ROOT / "src" / "UI" / "SurvivorsPanel.cs",
    # Loop-2 (eleventh wave) — MedicalWardPanel now routes its chrome through
    # AshfallUiText; register it so a new untranslated key fails the gate.
    ROOT / "src" / "UI" / "MedicalWardPanel.cs",
    # Thirteenth-wave sweep 2026-10-02: the smallest remaining panels now route
    # their chrome through AshfallUiText; register them so a new untranslated
    # key fails the gate.
    ROOT / "src" / "UI" / "WeatherDetailPanel.cs",
    ROOT / "src" / "UI" / "TimeCapsulePanel.cs",
    ROOT / "src" / "UI" / "SurvivorRelationsPanel.cs",
    ROOT / "src" / "UI" / "KennelPanel.cs",
    ROOT / "src" / "UI" / "ExpansionsHubPanel.cs",
    ROOT / "src" / "UI" / "CyberneticsPanel.cs",
    ROOT / "src" / "UI" / "CryogenicPermafrostCorePanel.cs",
    ROOT / "src" / "UI" / "BeliefsPanel.cs",
    ROOT / "src" / "UI" / "AnomalyWatchPanel.cs",
    ROOT / "src" / "UI" / "WeatherForecastPanel.cs",
    ROOT / "src" / "UI" / "VinylMoralePanel.cs",
    ROOT / "src" / "UI" / "ShelterThermalPanel.cs",
    ROOT / "src" / "UI" / "SanitationPanel.cs",
    ROOT / "src" / "UI" / "RadioIntelligencePanel.cs",
    ROOT / "src" / "UI" / "GameOverPanel.cs",
    ROOT / "src" / "UI" / "ExpeditionCampPanel.cs",
    # Wave-9 sweep 2026-10-02: the three largest remaining raw-chrome panels now
    # route their chrome through AshfallUiText; register them so a new untranslated
    # key fails the gate.
    ROOT / "src" / "UI" / "FactionsPanel.cs",
    ROOT / "src" / "UI" / "GreenhousePanel.cs",
    ROOT / "src" / "UI" / "ShelterBarterPanel.cs",
    # Repo-wide 6-loop sweep round 2 (2026-10-02): small chrome-only panels
    # fully localized and registered so the zero-tolerance raw-chrome gate holds.
    ROOT / "src" / "UI" / "CraftingPanel.cs",
    ROOT / "src" / "UI" / "DynamicQuestlinePanel.cs",
    ROOT / "src" / "UI" / "InSarMappingPanel.cs",
    ROOT / "src" / "UI" / "InventoryDetailPanel.cs",
    ROOT / "src" / "UI" / "JournalDetailPanel.cs",
    ROOT / "src" / "UI" / "ShelterSchedulePanel.cs",
]


def placeholders(value: str) -> set[str]:
    return set(re.findall(r"\{([A-Za-z_][A-Za-z0-9_]*|\d+)(?::[^}]*)?\}", value))


def onboarding_stage_keys() -> set[str]:
    """Onboarding stage title/objective keys, derived from the authoritative
    catalog instead of a hardcoded list that can silently drift when a stage is
    added (the ae6e54387 "HINT: —" failure class).

    ``OnboardingCatalog.Order``/``FirstHour`` build their title/objective keys
    at runtime from each stage id.
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
    return keys


def onboarding_hint_keys() -> set[str]:
    """Per-stage hint keys, held as literals in the hint panel."""
    text = ONBOARDING_HINT_PANEL.read_text(encoding="utf-8")
    return set(re.findall(r'"(onboarding\.hint\.[A-Za-z0-9_]+)"', text))


def achievement_keys() -> set[str]:
    """``achievement.{id}.name/description`` derived from ``achievements.json``.

    ``AchievementsPanel`` composes these at runtime from each definition id, so
    an authored achievement with no rows used to ship untranslated (L01).
    """
    data = json.loads(ACHIEVEMENTS_CATALOG.read_text(encoding="utf-8"))
    entries = data.get("achievements") or []
    keys: set[str] = set()
    for entry in entries:
        aid = entry.get("id")
        if not aid:
            continue
        keys.add(f"achievement.{aid}.name")
        keys.add(f"achievement.{aid}.description")
    if not keys:
        raise AssertionError(
            f"no achievements enumerated from {ACHIEVEMENTS_CATALOG.relative_to(ROOT)}"
        )
    return keys


def micro_discovery_keys() -> set[str]:
    """``discovery.{id}.title/description/choice.{cid}`` for expedition
    micro-locations, derived from ``micro_locations.json``.

    ``ExpeditionPanel`` composes these at runtime from each encounter id, so a
    new micro-location with no rows used to ship English (L01).
    """
    data = json.loads(MICRO_LOCATIONS_CATALOG.read_text(encoding="utf-8"))
    entries = data.get("encounters") or []
    keys: set[str] = set()
    for entry in entries:
        mid = entry.get("id")
        if not mid:
            continue
        keys.add(f"discovery.{mid}.title")
        keys.add(f"discovery.{mid}.description")
        for choice in entry.get("choices") or []:
            cid = choice.get("choiceId") or choice.get("id")
            if cid:
                keys.add(f"discovery.{mid}.choice.{cid}")
    if not keys:
        raise AssertionError(
            f"no micro-locations enumerated from {MICRO_LOCATIONS_CATALOG.relative_to(ROOT)}"
        )
    return keys


# Declarative registry of every dynamically-constructed localization key family.
# Each entry is (family name, authoritative catalog, derivation). Adding a family
# here makes a new member without en/de rows fail the gate instead of silently
# shipping untranslated — the ae6e54387 "HINT: —" class, generalised to all
# families (L01). Extend this tuple when a new runtime-composed key family
# appears so the same drift cannot recur.
DYNAMIC_FAMILIES = (
    ("onboarding_stage", ONBOARDING_CATALOG, onboarding_stage_keys),
    ("onboarding_hint", ONBOARDING_HINT_PANEL, onboarding_hint_keys),
    ("achievement", ACHIEVEMENTS_CATALOG, achievement_keys),
    ("micro_discovery", MICRO_LOCATIONS_CATALOG, micro_discovery_keys),
)


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
        # Include every dynamically constructed key family, each derived from its
        # authoritative catalog. Adding a stage, achievement or micro-location
        # without its en/de rows now fails here instead of shipping the
        # ae6e54387 "HINT: —" class — all families, not just the stage one (L01).
        dynamic: dict[str, set[str]] = {}
        for family, catalog, derive in DYNAMIC_FAMILIES:
            try:
                dynamic[family] = derive()
            except (OSError, json.JSONDecodeError) as exc:
                raise AssertionError(
                    f"dynamic key family '{family}' could not be enumerated from "
                    f"{catalog.relative_to(ROOT)}: {exc}"
                ) from exc
            # A derivation that silently yields zero keys (e.g. a refactor
            # changed how the panel composes them) would drop the whole family's
            # coverage while the gate still reports PASS. Fail loudly instead —
            # a registered family must never enumerate nothing.
            if not dynamic[family]:
                raise AssertionError(
                    f"dynamic key family '{family}' enumerated zero keys from "
                    f"{catalog.relative_to(ROOT)}; coverage would silently vanish"
                )
        dynamic_count = sum(len(keys) for keys in dynamic.values())
        referenced.update(key for keys in dynamic.values() for key in keys)
        missing = sorted(key for key in referenced if key not in rows)
        if missing:
            raise AssertionError("pilot/dynamic keys missing from strings.csv: " + ", ".join(missing))

        missing_de = sorted(key for key in referenced if not rows[key][1])
        if missing_de:
            raise AssertionError(
                "pilot/dynamic keys missing German translations: " + ", ".join(missing_de)
            )

        mismatched = sorted(
            key
            for key, (english, german) in rows.items()
            if german and placeholders(english) != placeholders(german)
        )
        if mismatched:
            raise AssertionError("placeholder mismatch: " + ", ".join(mismatched))

        # Reference gate for the localized wave surfaces: every Tr/T/F key they
        # name must resolve with a German row.
        localized_refs: set[str] = set()
        for path in LOCALIZED_SURFACES:
            text = path.read_text(encoding="utf-8")
            localized_refs.update(
                re.findall(r'(?:AshfallLocalization\.)?(?:Tr|TrFormat|T|F)\(\s*"([^"]+)"', text)
            )
        missing_localized = sorted(key for key in localized_refs if key not in rows)
        if missing_localized:
            raise AssertionError(
                "localized-surface keys missing from strings.csv: " + ", ".join(missing_localized)
            )
        missing_localized_de = sorted(key for key in localized_refs if not rows[key][1])
        if missing_localized_de:
            raise AssertionError(
                "localized-surface keys missing German translations: " + ", ".join(missing_localized_de)
            )

        for path in PILOTS:
            text = path.read_text(encoding="utf-8")
            if re.search(r"\b(?:Text|TooltipText)\s*=\s*\"[^\"$]*\"", text):
                raise AssertionError(f"literal UI text assignment remains in pilot: {path.relative_to(ROOT)}")

        families = ", ".join(f"{name}={len(keys)}" for name, keys in dynamic.items())
        print(
            f"L10N_DRIFT_GATE PASS — {len(rows)} keys, "
            f"{len(referenced)} pilot+dynamic references, "
            f"{dynamic_count} dynamic-family keys ({families}), "
            f"{len(localized_refs)} localized-surface references, German parity verified"
        )
        return 0
    except (OSError, AssertionError) as exc:
        print(f"L10N_DRIFT_GATE FAIL — {exc}", file=sys.stderr)
        return 1


if __name__ == "__main__":
    raise SystemExit(main())
