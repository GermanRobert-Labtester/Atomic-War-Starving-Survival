#!/usr/bin/env python3
"""
Regenerate detail-panel .tscn scenes with names that match the C# binder
field names declared in src/UI/<Name>.cs. Replaces the previously-generated
scenes whose generic *List names did not align with concrete field names
like _currentData, _stagesContainer, etc.

The output is a normal Godot 4.7 format=3 .tscn resource. Each section
header label and content VBoxContainer uses unique_name_in_owner=true so
the C# SceneBinder can resolve them via %NodeName lookups.
"""

from __future__ import annotations
import pathlib

REPO_ROOT = pathlib.Path(__file__).resolve().parent.parent.parent
SCRIPT_REL = "src/UI"
OUT_DIR = REPO_ROOT / "assets" / "ui" / "panels"

# (ClassName, Title, [(SectionHeaderUniqueName, SectionContentUniqueName, HeaderText)])
PANELS = [
    ("QuestDetailPanel", "OPERATION DOSSIER & DIRECTIVE", [
        ("ObjectivesHeader", "ObjectivesList", "OBJECTIVES"),
        ("PrerequisitesHeader", "PrerequisitesList", "PREREQUISITES"),
        ("RewardsHeader", "RewardsList", "REWARDS"),
        ("NarrativeHeader", "NarrativeText", "NARRATIVE"),
        ("StagesHeader", "StagesContainer", "STAGES"),
        ("ChoicesHeader", "ChoicesContainer", "CHOICES"),
    ]),

    ("MapDetailPanel", "SECTOR INTELLIGENCE DOSSIER", [
        ("LocationHeader", "InfoContainer", "LOCATION INFO"),
        ("HazardsHeader", "HazardsContainer", "THREATS"),
        ("LayoutsHeader", "LayoutsContainer", "LAYOUTS"),
        ("SalvageHeader", "SalvageContainer", "RESOURCES"),
    ]),

    ("RadiationDetailPanel", "RADIATION DETAIL", [
        ("CurrentHeader", "CurrentData", "CURRENT RADIATION"),
        ("DosimeterHeader", "DosimeterData", "DOSIMETER STATUS"),
        ("ProtectionHeader", "ProtectionData", "PROTECTION LEVELS"),
        ("EventsHeader", "EventsList", "RADIATION EVENTS"),
    ]),

    ("EconomyDetailPanel", "ECONOMY DETAIL", [
        ("ResourcesHeader", "ResourcesList", "CATALOG RESOURCES"),
        ("TradeHeader", "TradeList", "TRADE LEDGER"),
        ("MarketHeader", "MarketList", "MARKET DEMAND"),
        ("DebtHeader", "DebtList", "DEBT & CREDIT"),
    ]),

    ("CombatDetailPanel", "COMBAT DETAIL", [
        ("BattleHeader", "BattleInfo", "BATTLE INFORMATION"),
        ("TacticsHeader", "TacticsData", "BATTLE TACTICS"),
        ("LossesHeader", "CasualtyData", "CASUALTIES & LOSSES"),
        ("OutcomesHeader", "OutcomesData", "BATTLE OUTCOMES"),
    ]),

    ("FactionDetailPanel", "FACTION DIPLOMATIC DOSSIER", [
        ("InfoHeader", "InfoContainer", "FACTION PROFILE"),
        ("DiplomacyHeader", "DiplomacyContainer", "DIPLOMATIC STANDING"),
        ("TradeHeader", "TradeContainer", "TRADE COMMODITIES"),
        ("EventsHeader", "EventsContainer", "INTELLIGENCE LOGS"),
    ]),

    ("JournalDetailPanel", "JOURNAL DETAIL", [
        ("EntriesHeader", "EntriesList", "RECENT ENTRIES"),
        ("CodexHeader", "CodexList", "CODEX UNLOCKS"),
        ("TabsHeader", "TabsList", "TAB STATE"),
    ]),

    ("EventDetailPanel", "EVENT DETAIL", [
        ("EventInfoHeader", "EventInfoList", "RECENT EVENTS"),
        ("HistoryHeader", "HistoryList", "INCIDENTS LOG"),
        ("NarrativeHeader", "NarrativeList", "NARRATIVE PROGRESSION"),
    ]),

    ("DutyRosterDetailPanel", "DUTY ROSTER DETAIL", [
        ("AssignmentsHeader", "AssignmentsList", "CURRENT ASSIGNMENTS"),
        ("ShiftsHeader", "ShiftsList", "SHIFT SCHEDULE"),
        ("PerformanceHeader", "PerformanceList", "WORKER PERFORMANCE"),
    ]),

    ("SurvivalDetailPanel", "SURVIVAL DETAIL", [
        ("HealthHeader", "HealthData", "ROSTER HEALTH"),
        ("NeedsHeader", "NeedsData", "AVERAGE NEEDS"),
        ("RadiationHeader", "RadiationData", "RADIATION OVERVIEW"),
        ("StatusHeader", "StatusData", "STATUS SUMMARY"),
    ]),
]


def scene_text(class_name: str, title_text: str, sections: list) -> str:
    safe_id = class_name.lower().replace(" ", "_")
    uid_path = f"uid://b1{safe_id[:24].rstrip('_')}_ashfall"
    ext_id = f"1_{safe_id[:20].rstrip('_')}"

    out: list[str] = []
    out.append(f"[gd_scene load_steps=2 format=3 uid=\"{uid_path}\"]\n\n")
    out.append(f"[ext_resource type=\"Script\" path=\"res://{SCRIPT_REL}/{class_name}.cs\" id=\"{ext_id}\"]\n\n")
    out.append(f"[node name=\"{class_name}\" type=\"Control\"]\n")
    out.append("visible = false\nlayout_mode = 3\nanchors_preset = 15\n")
    out.append("anchor_right = 1.0\nanchor_bottom = 1.0\n")
    out.append("grow_horizontal = 2\ngrow_vertical = 2\n")
    out.append(f"script = ExtResource(\"{ext_id}\")\n\n")

    out.append("[node name=\"Backdrop\" type=\"ColorRect\" parent=\".\"]\n")
    out.append("unique_name_in_owner = true\nlayout_mode = 1\nanchors_preset = 15\n")
    out.append("anchor_right = 1.0\nanchor_bottom = 1.0\n")
    out.append("grow_horizontal = 2\ngrow_vertical = 2\n")
    out.append("color = Color(0.05, 0.05, 0.05, 0.92)\n\n")

    out.append("[node name=\"Dialog\" type=\"CenterContainer\" parent=\".\"]\n")
    out.append("unique_name_in_owner = true\nlayout_mode = 1\nanchors_preset = 15\n")
    out.append("anchor_right = 1.0\nanchor_bottom = 1.0\n")
    out.append("grow_horizontal = 2\ngrow_vertical = 2\n\n")

    out.append("[node name=\"Frame\" type=\"PanelContainer\" parent=\"Dialog\"]\n")
    out.append("custom_minimum_size = Vector2(550, 0)\n")
    out.append("layout_mode = 2\n\n")

    out.append("[node name=\"Margin\" type=\"MarginContainer\" parent=\"Dialog/Frame\"]\n")
    out.append("layout_mode = 2\n")
    out.append("theme_override_constants/margin_left = 24\n")
    out.append("theme_override_constants/margin_top = 24\n")
    out.append("theme_override_constants/margin_right = 24\n")
    out.append("theme_override_constants/margin_bottom = 24\n\n")

    out.append("[node name=\"Content\" type=\"VBoxContainer\" parent=\"Dialog/Frame/Margin\"]\n")
    out.append("layout_mode = 2\n")
    out.append("theme_override_constants/separation = 16\n\n")

    out.append("[node name=\"Title\" type=\"Label\" parent=\"Dialog/Frame/Margin/Content\"]\n")
    out.append("unique_name_in_owner = true\n")
    out.append("layout_mode = 2\n")
    out.append(f"text = \"{title_text}\"\n")
    out.append("horizontal_alignment = 1\n\n")

    out.append("[node name=\"Sep1\" type=\"HSeparator\" parent=\"Dialog/Frame/Margin/Content\"]\nlayout_mode = 2\n\n")

    for idx, (header_node, content_node, header_text) in enumerate(sections, start=1):
        out.append(f"[node name=\"{header_node}\" type=\"Label\" parent=\"Dialog/Frame/Margin/Content\"]\n")
        out.append("unique_name_in_owner = true\n")
        out.append("layout_mode = 2\n")
        out.append(f"text = \"{header_text}\"\n\n")

        out.append(f"[node name=\"{content_node}\" type=\"VBoxContainer\" parent=\"Dialog/Frame/Margin/Content\"]\n")
        out.append("unique_name_in_owner = true\n")
        out.append("layout_mode = 2\n")
        out.append("custom_minimum_size = Vector2(450, 0)\n")
        out.append("theme_override_constants/separation = 8\n\n")

        if idx < len(sections):
            out.append(f"[node name=\"Sep{idx+1}\" type=\"HSeparator\" parent=\"Dialog/Frame/Margin/Content\"]\nlayout_mode = 2\n\n")

    next_sep = len(sections) + 2
    out.append(f"[node name=\"Sep{next_sep}\" type=\"HSeparator\" parent=\"Dialog/Frame/Margin/Content\"]\nlayout_mode = 2\n")
    out.append("[node name=\"CloseButton\" type=\"Button\" parent=\"Dialog/Frame/Margin/Content\"]\n")
    out.append("unique_name_in_owner = true\ncustom_minimum_size = Vector2(200, 40)\n")
    out.append("layout_mode = 2\nsize_flags_horizontal = 4\ntext = \"CLOSE [Esc]\"\n")
    return "".join(out)


def main() -> None:
    OUT_DIR.mkdir(parents=True, exist_ok=True)
    written: list[str] = []
    for class_name, title, sections in PANELS:
        out = OUT_DIR / f"{class_name}.tscn"
        out.write_text(scene_text(class_name, title, sections), encoding="utf-8")
        written.append(str(out.relative_to(REPO_ROOT)))
    print(f"wrote {len(written)} scenes")


if __name__ == "__main__":
    main()
