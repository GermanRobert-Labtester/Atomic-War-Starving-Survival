#!/usr/bin/env python3
"""
Rewrite each remaining *DetailPanel.cs to use SceneBinder for its _Ready
block instead of imperative VBoxContainer construction.

Strategy
--------
Each panel declares a small set of fields:
    private VBoxContainer _xxxData;
    private VBoxContainer _yyyList;
    private Label _lblXxxTitle;
    private CommandTextView _zzz;
and constructs the corresponding container hierarchy inside _Ready().
The corresponding .tscn already names these via unique_name_in_owner so
SceneBinder can resolve them via %Xxx.

For each panel, this script:
  1. Reads the panel file.
  2. Inspects the fields declared in the body that are VBoxContainer or
     Label with names matching the section header naming. Heuristically
     maps them to the unique-name keys already declared in the generated
     .tscn.
  3. Replaces the entire _Ready() body with the SceneBinder-driven
     equivalent so the field assignment lines remain untouched elsewhere
     in the file.

The script prints a diff summary per panel and is idempotent — running
twice on the same panel produces no further edits.
"""

from __future__ import annotations
import re
import pathlib

REPO_ROOT = pathlib.Path(__file__).resolve().parent.parent.parent
UI_DIR = REPO_ROOT / "src" / "UI"

# (ClassName, [(FieldName, UniqueName)])
PANEL_FIELDS = {
    "RadiationDetailPanel": [
        ("_currentData", "CurrentData"),
        ("_dosimeterData", "DosimeterData"),
        ("_protectionData", "ProtectionData"),
        ("_eventsList", "EventsList"),
    ],
    "EconomyDetailPanel": [
        ("_resourcesList", "ResourcesList"),
        ("_tradeList", "TradeList"),
        ("_marketList", "MarketList"),
        ("_debtList", "DebtList"),
    ],
    "CombatDetailPanel": [
        ("_battleInfo", "BattleInfo"),
        ("_tacticsData", "TacticsData"),
        ("_casualtyData", "CasualtyData"),
        ("_outcomesData", "OutcomesData"),
    ],
    "FactionDetailPanel": [
        ("_infoContainer", "InfoContainer"),
        ("_diplomacyContainer", "DiplomacyContainer"),
        ("_tradeContainer", "TradeContainer"),
        ("_eventsContainer", "EventsContainer"),
    ],
    "JournalDetailPanel": [
        ("_entriesList", "EntriesList"),
        ("_codexList", "CodexList"),
        ("_tabsList", "TabsList"),
    ],
    "EventDetailPanel": [
        ("_eventInfoList", "EventInfoList"),
        ("_historyList", "HistoryList"),
        ("_narrativeList", "NarrativeList"),
    ],
    "DutyRosterDetailPanel": [
        ("_assignmentsList", "AssignmentsList"),
        ("_shiftsList", "ShiftsList"),
        ("_performanceList", "PerformanceList"),
    ],
    "SurvivalDetailPanel": [
        ("_healthData", "HealthData"),
        ("_needsData", "NeedsData"),
        ("_radiationData", "RadiationData"),
        ("_statusData", "StatusData"),
    ],
    "QuestDetailPanel": [
        ("_infoContainer", "InfoContainer"),
        ("_stagesContainer", "StagesContainer"),
        ("_choicesContainer", "ChoicesContainer"),
        ("_rewardsContainer", "RewardsContainer"),
    ],
    "MapDetailPanel": [
        ("_infoContainer", "InfoContainer"),
        ("_hazardsContainer", "HazardsContainer"),
        ("_layoutsContainer", "LayoutsContainer"),
        ("_salvageContainer", "SalvageContainer"),
    ],
}

BOX_TYPE_HINT = {
    "_infoContainer": "VBoxContainer",
    "_stagesContainer": "VBoxContainer",
    "_choicesContainer": "VBoxContainer",
    "_rewardsContainer": "VBoxContainer",
    "_hazardsContainer": "VBoxContainer",
    "_layoutsContainer": "VBoxContainer",
    "_salvageContainer": "VBoxContainer",
    "_resourcesList": "VBoxContainer",
    "_tradeList": "VBoxContainer",
    "_marketList": "VBoxContainer",
    "_debtList": "VBoxContainer",
    "_battleInfo": "VBoxContainer",
    "_tacticsData": "VBoxContainer",
    "_casualtyData": "VBoxContainer",
    "_outcomesData": "VBoxContainer",
    "_infoContainer": "VBoxContainer",
    "_diplomacyContainer": "VBoxContainer",
    "_tradeContainer": "VBoxContainer",
    "_eventsContainer": "VBoxContainer",
    "_entriesList": "VBoxContainer",
    "_codexList": "VBoxContainer",
    "_tabsList": "VBoxContainer",
    "_eventInfoList": "VBoxContainer",
    "_historyList": "VBoxContainer",
    "_narrativeList": "VBoxContainer",
    "_assignmentsList": "VBoxContainer",
    "_shiftsList": "VBoxContainer",
    "_performanceList": "VBoxContainer",
    "_healthData": "VBoxContainer",
    "_needsData": "VBoxContainer",
    "_radiationData": "VBoxContainer",
    "_statusData": "VBoxContainer",
    "_currentData": "VBoxContainer",
    "_dosimeterData": "VBoxContainer",
    "_protectionData": "VBoxContainer",
    "_eventsList": "VBoxContainer",
}


def build_ready(class_name: str, fields: list[tuple[str, str]]) -> str:
    type_kind_map = {}
    for f, u in fields:
        type_kind_map[u] = BOX_TYPE_HINT[f]
    declares_binder = "            var binder = new SceneBinder(this, typeof(" + class_name + "));\n"
    reqs = []
    assigns = []
    for field, unique in fields:
        node_type = type_kind_map[unique]
        reqs.append(f"            binder.Require<{node_type}>(\"{unique}\");\n")
        assigns.append(f"            {field} = binder.Get<{node_type}>(\"{unique}\");\n")
    close = "            binder.Get<Button>(\"CloseButton\").Pressed += () => OnClose?.Invoke();\n\n"
    end = "            Visible = false;\n        }"
    return (
        "        public override void _Ready()\n"
        "        {\n"
        "            // Ticket #125: layout chrome owned by res://assets/ui/panels/"
        + class_name
        + ".tscn; SceneBinder resolves typed unique-name nodes once.\n"
        + "            // Sibling refresh code is unchanged.\n"
        + declares_binder
        + "".join(reqs)
        + "            binder.Require<Button>(\"CloseButton\");\n"
        + "".join(assigns)
        + close
        + end
    )


def rewrite(file_path: pathlib.Path, class_name: str, fields: list[tuple[str, str]]) -> bool:
    src = file_path.read_text(encoding="utf-8")
    pattern = re.compile(
        r"        public override void _Ready\(\)\s*\n        \{.*?\n        \}",
        re.DOTALL,
    )
    matches = list(pattern.finditer(src))
    if not matches:
        # Already migrated
        return False
    new_ready = build_ready(class_name, fields)
    # Take the last _Ready() block (the panel's, not any helper)
    match = matches[-1]
    new_src = src[: match.start()] + new_ready + src[match.end():]
    file_path.write_text(new_src, encoding="utf-8")
    return True


def main() -> None:
    for class_name, fields in PANEL_FIELDS.items():
        f = UI_DIR / f"{class_name}.cs"
        if not f.exists():
            print(f"SKIP: {f.name} not found")
            continue
        if "_contentVBox = null!;" not in f.read_text(encoding="utf-8"):
            # Already in SceneBinder form
            print(f"SKIP: {f.name} appears already migrated")
            continue
        ok = rewrite(f, class_name, fields)
        print(f"OK : {f.relative_to(REPO_ROOT)}" if ok else f"NOOP: {f.relative_to(REPO_ROOT)}")


if __name__ == "__main__":
    main()
