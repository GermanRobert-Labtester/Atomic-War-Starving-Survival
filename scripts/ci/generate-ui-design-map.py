#!/usr/bin/env python3
# SPDX-License-Identifier: MIT
"""
generate-ui-design-map.py — Programmatic UI Design Map Generator

Derives the ASHFALL UI design system directly from source:

  - Design tokens (colors, spacing, typography) from Ashfall.Core.UI.Theme
  - Layout system usage (AshfallDashboardShell, status rails, split bodies)
  - Information architecture (dashboard nav order/groups, panel registry routes)
  - Component library usage statistics (data grids, sidebars, option buttons…)
  - Per-surface design spec for every shell-based panel
  - Measured design invariants (close affordances, bind/unbind lifecycle, sizes)

Nothing is hand-authored: every row is parsed from src/UI, src/Main*.cs and the
Core theme. Output is both human (docs/ui/UI_DESIGN_MAP.md) and machine
(docs/ui/ui_design_map.json) readable, with --check drift verification.

Usage:
  python3 scripts/ci/generate-ui-design-map.py          # Regenerate the design map
  python3 scripts/ci/generate-ui-design-map.py --check  # Verify no drift in CI
"""

import importlib.util
import json
import pathlib
import re
import sys
from datetime import datetime, timezone

REPO_ROOT = pathlib.Path(__file__).resolve().parent.parent.parent
UI_DIR = REPO_ROOT / "src" / "UI"
OUT_MD = REPO_ROOT / "docs" / "ui" / "UI_DESIGN_MAP.md"
OUT_JSON = REPO_ROOT / "docs" / "ui" / "ui_design_map.json"

EXCLUDED = ("/obj/", "/bin/", "/.git/")


def load_graph():
    spec = importlib.util.spec_from_file_location(
        "amap", REPO_ROOT / "scripts" / "ci" / "generate-architecture-map.py")
    module = importlib.util.module_from_spec(spec)
    spec.loader.exec_module(module)
    return module.ARCHITECTURE_GRAPH


def parse_theme():
    theme = {"colors": [], "spacing": {}, "typography": {}}
    text = (REPO_ROOT / "Assets" / "Ashfall.Core" / "UI" / "Theme.cs").read_text(encoding="utf-8")
    for name, hexval in re.findall(r'public const string (\w+Hex) = "(\#[0-9A-Fa-f]+)"', text):
        theme["colors"].append({"token": name[:-3], "hex": hexval})
    for name, value in re.findall(r"public const int (Spacing\w+) = (\d+)", text):
        theme["spacing"][name] = int(value)
    for name, value in re.findall(r"public const int (FontSize\w+) = (\d+)", text):
        theme["typography"][name] = int(value)
    return theme


def parse_panels():
    panels = []
    scene_panels = {p.stem for p in (REPO_ROOT / "assets" / "ui" / "panels").glob("*.tscn")} \
        if (REPO_ROOT / "assets" / "ui" / "panels").exists() else set()

    for path in sorted(UI_DIR.glob("*.cs")):
        if path.name.startswith("Ashfall") or path.name == "DesignTheme.cs":
            continue
        text = path.read_text(encoding="utf-8", errors="ignore")

        m = re.search(r"public(?:\s+sealed)?(?:\s+partial)? class (\w+)", text)
        if not m:
            continue
        name = m.group(1)

        shell = re.search(
            r'new AshfallDashboardShell\("((?:[^"\\]|\\.)*)"'
            r'(?:,\s*minWidth:\s*(\d+))?(?:,\s*minHeight:\s*(\d+))?', text)
        bind = re.search(r"public void Bind\((\w+)\s+\w+\)", text)
        unbind = "public void Unbind(" in text

        rail_cards = re.findall(r'AddCard\("(\w+)",\s*"([^"]*)"', text)
        uses_status_rail = "SetStatusRail()" in text

        counts = {
            "scroll": len(re.findall(r"new ScrollContainer", text)),
            "option_buttons": len(re.findall(r"new OptionButton", text)),
            "buttons": len(re.findall(r"new Button\b", text)),
            "panel_cards": len(re.findall(r"new PanelContainer", text)),
            "separators": len(re.findall(r"MakeSeparator\(", text)),
            "hbox": len(re.findall(r"new HBoxContainer", text)),
            "vbox": len(re.findall(r"new VBoxContainer", text)),
            "datagrid": len(re.findall(r"AshfallDataGrid", text)),
            "sidebar": len(re.findall(r"AshfallSidebar", text)),
            "metric_card": len(re.findall(r"AshfallMetricCard", text)),
        }
        loc = text.count("\n") + 1

        if shell:
            pattern = "shell:split-body" if counts["hbox"] >= 1 and counts["vbox"] >= 2 else "shell:single-column"
        elif name in scene_panels or "GetNode<" in text or "_binder.Require<" in text:
            pattern = "scene-backed"
        else:
            pattern = "code-built"

        panels.append({
            "panel": name,
            "file": f"src/UI/{path.name}",
            "loc": loc,
            "bind_target": bind.group(1) if bind else None,
            "has_unbind": unbind,
            "bindable": bind is not None,
            "shell": bool(shell),
            "shell_title": shell.group(1) if shell else None,
            "min_width": int(shell.group(2)) if shell and shell.group(2) else 720,
            "min_height": int(shell.group(3)) if shell and shell.group(3) else 480,
            "rail_cards": [{"id": cid, "label": label} for cid, label in rail_cards],
            "uses_status_rail": uses_status_rail,
            "layout_pattern": pattern,
            "components": counts,
        })
    return panels


def parse_nav():
    text = (REPO_ROOT / "src" / "UI" / "GameDashboardPanel.cs").read_text(encoding="utf-8")
    nav = []
    current_group = "Primary"
    token = re.compile(r'AddNavButton\(content,\s*"([^"]+)",\s*"([^"]+)"\)|MakeSectionHeader\("([^"]+)"\)')
    for label, route, header in token.findall(text):
        if header:
            current_group = header
            nav.append({"group": header, "buttons": []})
        else:
            if not nav or nav[-1]["group"] != current_group:
                nav.append({"group": current_group, "buttons": []})
            nav[-1]["buttons"].append({"label": label, "route": route})
    return [g for g in nav if g["buttons"]]


def parse_routes():
    surfaces = (REPO_ROOT / "src" / "Main.PlayerSurfaces.cs").read_text(encoding="utf-8")
    expanded = (REPO_ROOT / "src" / "Main.ExpandedShelterSystems.cs").read_text(encoding="utf-8")

    expanded_ids = []
    m = re.search(r"string\[\] expandedIds\s*=\s*\{(.*?)\};", surfaces, re.S)
    if m:
        expanded_ids = re.findall(r'"([\w]+)"', m.group(1))

    configured = re.findall(r'PanelRegistry\.ConfigureActions\("([\w]+)"', surfaces)
    open_cases = re.findall(r'case "([\w]+)":', expanded)
    return {
        "expanded_ids": expanded_ids,
        "panel_registry_ids": sorted(set(configured)),
        "open_expanded_cases": sorted(set(open_cases)),
    }


def build_route_map(graph):
    routes_by_panel = {}
    for section, entry in graph.items():
        for panel in entry.get("ui", []):
            if panel in ("Main", "GameDashboardPanel"):
                continue
            routes_by_panel.setdefault(panel, set()).update(entry.get("routes", []))
    return {k: sorted(v) for k, v in routes_by_panel.items()}


def main(check_only: bool) -> int:
    theme = parse_theme()
    panels = parse_panels()
    nav = parse_nav()
    routes = parse_routes()
    route_map = build_route_map(load_graph())

    for panel in panels:
        panel["routes"] = route_map.get(panel["panel"], [])

    shells = [p for p in panels if p["shell"]]
    bindable = [p for p in panels if p["bindable"]]

    component_totals = {}
    for p in panels:
        for k, v in p["components"].items():
            component_totals[k] = component_totals.get(k, 0) + v
    component_users = {
        k: sum(1 for p in panels if p["components"][k] > 0) for k in component_totals
    }

    min_sizes = sorted({(p["min_width"], p["min_height"]) for p in shells})
    missing_unbind = [p["panel"] for p in bindable if not p["has_unbind"]]
    # An expanded id is reachable when the OpenExpandedPanel switch handles it
    # OR PanelRegistry.ConfigureActions owns its open action (e.g. farming,
    # defense_grid, psychology_arcs, bestiary route through HandleX bindings).
    reachable_ids = set(routes["open_expanded_cases"]) | set(routes["panel_registry_ids"])
    orphan_routes = [r for r in routes["expanded_ids"] if r not in reachable_ids]
    rail_without_shell_rail = [p["panel"] for p in panels
                               if p["rail_cards"] and not p["uses_status_rail"]]

    invariants = [
        ("Every dashboard shell declares a min size ≥ 720×480",
         all(p["min_width"] >= 720 and p["min_height"] >= 480 for p in shells),
         f"{len(shells)} shell panels"),
        ("Every bindable panel exposes Unbind (lifecycle symmetry)",
         not missing_unbind,
         f"{len(bindable)} bindable panels"
         + (f"; missing: {', '.join(missing_unbind[:8])}" if missing_unbind else "")),
        ("Every expanded panel id has an open path (switch case or PanelRegistry binding)",
         not orphan_routes,
         f"{len(routes['expanded_ids'])} ids, {len(routes['open_expanded_cases'])} switch cases, "
         f"{len(routes['panel_registry_ids'])} registry bindings"
         + (f"; orphan: {', '.join(orphan_routes[:8])}" if orphan_routes else "")),
        ("Every surface with status-rail cards obtains the rail via shell.SetStatusRail()",
         not rail_without_shell_rail,
         f"{sum(1 for p in panels if p['rail_cards'])} surfaces with rails"
         + (f"; off-shell: {', '.join(rail_without_shell_rail[:8])}" if rail_without_shell_rail else "")),
    ]

    payload = {
        "schema_version": "1.0.0",
        "generated_at": datetime.now(timezone.utc).date().isoformat(),
        "canvas": {"resolution": "1920x1080", "scaling": "fixed"},
        "design_tokens": theme,
        "surface_counts": {
            "total_panels": len(panels),
            "shell_panels": len(shells),
            "bindable_panels": len(bindable),
            "scene_backed": sum(1 for p in panels if p["layout_pattern"] == "scene-backed"),
            "rail_surfaces": sum(1 for p in panels if p["rail_cards"]),
        },
        "layout_patterns": {
            k: sum(1 for p in panels if p["layout_pattern"] == k)
            for k in ("shell:split-body", "shell:single-column", "scene-backed", "code-built")
        },
        "component_totals": component_totals,
        "component_users": component_users,
        "shell_min_sizes": [f"{w}×{h}" for w, h in min_sizes],
        "nav": nav,
        "routes": routes,
        "invariants": [{"rule": r, "holds": ok, "evidence": ev} for r, ok, ev in invariants],
        "panels": panels,
    }

    md = render_md(payload)

    if check_only:
        if OUT_MD.exists() and OUT_MD.read_text(encoding="utf-8") == md:
            print(f"OK: {OUT_MD.name} in sync ({len(panels)} panels)")
            return 0
        print(f"FAIL: {OUT_MD.name} is out of date. Run generate-ui-design-map.py")
        return 1

    OUT_MD.parent.mkdir(parents=True, exist_ok=True)
    OUT_MD.write_text(md, encoding="utf-8")
    OUT_JSON.write_text(json.dumps(payload, indent=2) + "\n", encoding="utf-8")
    print(f"Wrote {OUT_MD} ({len(panels)} panels, {len(shells)} shells)")
    print(f"Wrote {OUT_JSON}")
    return 0


def render_md(p):
    lines = []
    a = lines.append
    a("# ASHFALL UI Design Map — Programmatic")
    a("")
    a(f"**Generated:** {p['generated_at']} (from source; regenerate with "
      "`python3 scripts/ci/generate-ui-design-map.py`)")
    a("")
    a(f"**Method:** static parse of `src/UI/*.cs`, `src/Main.PlayerSurfaces.cs`, "
      "`src/UI/GameDashboardPanel.cs`, `Ashfall.Core.UI.Theme`")
    a(f"**Canvas:** fixed {p['canvas']['resolution']} — full-rect modal shells")
    a("")
    c = p["surface_counts"]
    a("## 1. Surface inventory")
    a("")
    a(f"- **{c['total_panels']}** UI panels in `src/UI/` — "
      f"**{c['shell_panels']}** dashboard shells, **{c['bindable_panels']}** bindable, "
      f"**{c['scene_backed']}** scene-backed, **{c['rail_surfaces']}** with status rails")
    a(f"- Layout patterns: " + ", ".join(f"`{k}` × {v}" for k, v in p["layout_patterns"].items()))
    a(f"- Distinct shell minimum sizes: {', '.join(p['shell_min_sizes'])}")
    a("")
    a("## 2. Design tokens (Ashfall.Core.UI.Theme)")
    a("")
    a("### Colors")
    a("")
    a("| Token | Hex |")
    a("|---|---|")
    for color in p["design_tokens"]["colors"]:
        a(f"| `{color['token']}` | `{color['hex']}` |")
    a("")
    a("### Spacing scale")
    a("")
    a("| Token | px |")
    a("|---|---:|")
    for token, value in p["design_tokens"]["spacing"].items():
        a(f"| `{token}` | {value} |")
    a("")
    a("### Typography scale")
    a("")
    a("| Token | px |")
    a("|---|---:|")
    for token, value in p["design_tokens"]["typography"].items():
        a(f"| `{token}` | {value} |")
    a("")
    a("## 3. Layout system")
    a("")
    a("Every designed surface is an `AshfallDashboardShell` full-rect modal:")
    a("title bar → optional `AshfallStatusRail` (metric cards) → content stack "
      "(split-body or single-column) → header close button. Fixed canvas 1920×1080.")
    a("")
    a("### Component library usage")
    a("")
    a("| Component | Total instances | Panels using it |")
    a("|---|---:|---:|")
    for k in sorted(p["component_totals"], key=lambda k: -p["component_totals"][k]):
        a(f"| `{k}` | {p['component_totals'][k]} | {p['component_users'][k]} |")
    a("")
    a("## 4. Information architecture")
    a("")
    a("Dashboard navigation groups in declared order (label → route):")
    a("")
    for group in p["nav"]:
        a(f"### {group['group']}")
        a("")
        a("```")
        for b in group["buttons"]:
            a(f"{b['label']} -> {b['route']}")
        a("```")
        a("")
    r = p["routes"]
    a(f"- Panel-registry configured ids: **{len(r['panel_registry_ids'])}**")
    a(f"- Expanded-surface ids: **{len(r['expanded_ids'])}**")
    a(f"- `OpenExpandedPanel` cases: **{len(r['open_expanded_cases'])}**")
    a("")
    a("## 5. Measured design invariants")
    a("")
    a("| Rule | Holds | Evidence |")
    a("|---|---|---|")
    for inv in p["invariants"]:
        a(f"| {inv['rule']} | {'✅' if inv['holds'] else '❌'} | {inv['evidence']} |")
    a("")
    a("## 6. Shell surface design specs")
    a("")
    a("Per-surface spec derived from source (binding target, layout, metrics, actions):")
    a("")
    a("| Panel | Shell title | Binds | Layout | Min size | Rail cards | Scroll | Options | Buttons | Routes | LOC |")
    a("|---|---|---|---|---|---|---:|---:|---:|---|---:|")
    for panel in p["panels"]:
        if not panel["shell"]:
            continue
        comp = panel["components"]
        title = (panel["shell_title"] or "").replace("|", "/")
        a(f"| `{panel['panel']}` | {title} | `{panel['bind_target'] or '—'}` | "
          f"`{panel['layout_pattern']}` | {panel['min_width']}×{panel['min_height']} | "
          f"{len(panel['rail_cards'])} | {comp['scroll']} | {comp['option_buttons']} | "
          f"{comp['buttons']} | {', '.join(panel['routes']) or '—'} | {panel['loc']} |")
    a("")
    a("## 7. Non-shell bindable surfaces")
    a("")
    nonshell = [x for x in p["panels"] if not x["shell"] and x["bindable"]]
    a(f"{len(nonshell)} bindable panels render without the dashboard shell "
      "(scene-backed forms, sub-views, read-only rows):")
    a("")
    a("| Panel | Binds | Pattern | Routes | LOC |")
    a("|---|---|---|---|---:|")
    for panel in nonshell:
        a(f"| `{panel['panel']}` | `{panel['bind_target'] or '—'}` | "
          f"`{panel['layout_pattern']}` | {', '.join(panel['routes']) or '—'} | {panel['loc']} |")
    a("")
    a("## 8. Machine-readable output")
    a("")
    a(f"Full token/route/panel JSON: `docs/ui/ui_design_map.json` "
      f"({len(p['panels'])} panels, {len(p['nav'])} nav groups).")
    a("")
    return "\n".join(lines) + "\n"


if __name__ == "__main__":
    sys.exit(main("--check" in sys.argv))
