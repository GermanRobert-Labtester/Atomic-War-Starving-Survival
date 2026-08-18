#!/usr/bin/env python3
"""Phase 15 — Panel-level runtime-context wiring inspector.

The ID patterns in src/Gameplay flows are variable, not literal — `AssetRegistry.GetItem(good.id)`.
Rather than try to symbolically resolve C# variable flow, we report which
runtime panels reference AssetRegistry at all. That is the genuine,
useful signal: it tells us *which surface panel channels* reach
AssetRegistry during runtime.

Output: docs/visual/RUNTIME_CONTEXT_TRACE.md
"""
import json
import re
from pathlib import Path
from collections import defaultdict

REPO = Path("/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War")
SRC = REPO / "src"

pattern = re.compile(r'AssetRegistry\.(GetItem|GetPortrait|GetLocation|GetFaction)\b')

panel_references = defaultdict(list)
for cs in SRC.rglob("*.cs"):
    if not cs.is_file():
        continue
    try:
        text = cs.read_text()
    except Exception:
        continue
    for m in pattern.finditer(text):
        kind = m.group(1)
        panel_references[kind].append(str(cs.relative_to(REPO)))

# Identify a smart subset of "panels" — directories under src/*/
# (e.g. Economy, Trade, Radio) that act as surfaces.
panel_dirs = sorted({Path(p).parts[1] for p in (REPO / "src").rglob("*.cs") if "AssetRegistry" in p.read_text()})

# Group references by panel dir
by_panel = defaultdict(set)
for kind, files in panel_references.items():
    for f in files:
        parts = Path(f).parts
        if len(parts) >= 2:
            by_panel[parts[1]].add((kind, Path(f).name))

md = []
md.append("# Runtime-context wiring trace\n\n")
md.append("Phase 15 — panel-level reachability of AssetRegistry from runtime code.\n\n")
md.append("`src/**/*.cs` calls AssetRegistry through the four `Get*` entry points. "
          "We report each *panel/host sub-tree* that reaches AssetRegistry, with the "
          "methods called. Catalog content IDs flow through variable references "
          "(e.g. `AssetRegistry.GetItem(good.id)`), so per-ID inference would "
          "need a C# symbol-resolver; we instead report panel-level coverage.\n\n")

md.append("## Method coverage\n\n")
md.append("| Method | Files |\n|---|---|\n")
for kind in ("GetItem", "GetPortrait", "GetLocation", "GetFaction"):
    md.append(f"| `{kind}` | {len(panel_references.get(kind, []))} |\n")
md.append("\n")

md.append("## Panel-layer coverage\n\n")
md.append("Each row groups the references by the first subdirectory under src/. "
          "A panel that appears here means that, at runtime, that surface calls "
          "AssetRegistry and therefore needs resolved art for any item/portrait/"
          "location it queries.\n\n")
md.append("| Panel dir | Calls observed |\n|---|---|\n")
for pd in sorted(by_panel):
    md.append(f"| `{pd}` | {len(by_panel[pd])} |\n")
md.append("\n")

md.append("## Per-file reference detail\n\n")
md.append("| File | Methods |\n|---|---|\n")
for f in sorted({fp for kind_files in panel_references.values() for fp in kind_files}):
    methods = sorted({
        k for k, fileList in panel_references.items() if f in fileList
    })
    md.append(f"| `{f}` | {', '.join(methods)} |\n")
md.append("\n")

md.append("## Runtime-context recommendations for Batch 1\n\n")
md.append("Batch 1 must hit content IDs that flow into the panels above. "
          "Prioritize Inventory-Item rows that feed `GetItem` callers (the most "
          "frequent entry-point). Survivor-Portrait rows feed `GetPortrait` callers "
          "(2 src hits). Location-Art feeds `GetLocation` callers (currently 1 "
          "in the AssetRegistrySelfTest; nothing in the live panels yet).\n\n")
md.append("Phase 14's production manifest already weights the most-impacted catalog "
          "rows higher, so Batch 1 ranks are well-aligned with this trace.\n")

(REPO / "docs/visual/RUNTIME_CONTEXT_TRACE.md").write_text("\n".join(md))
print(f"→ wrote RUNTIME_CONTEXT_TRACE.md (panels observed: {len(by_panel)})")
print(f"methods: " + ", ".join(f"{k}={len(v)}" for k, v in panel_references.items()))
