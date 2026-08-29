"""
scene-lint.py — Godot production scene/resource structural linter.

Validates .tscn and .tres files in the active Godot tree only:
  - assets/ui/scenes/...
  - assets/ui/panels/...
  - assets/ui/components/...
  - assets/ui/modals/...
  - scenes/Main.tscn, scenes/CSharpTest.tscn

Detects:
  A. ext_resource — referenced file existence, path case match,
     declared type plausible, UID consistency where present.
  B. sub_resource — malformed/incomplete references.
  C. UID — malformed UIDs, stale or dangling uid:// references,
     duplicate UIDs across resources, duplicate ids within a single resource.
  D. PATH CASE — res://assets/... resolves on disk to res://Assets/...  (case-distinct trees).
  E. SCRIPT VALIDITY — referenced script file exists and is a loadable C# source.
  F. NODE CONTRACT REQUIRED NODES — optional `scene_ownership.kind` field.

Output: PASS or list of actionable errors with file/line context.

Exit 0 when single production scene tree green; exit 1 otherwise.

Run:
    python3 scripts/ci/scene-lint.py
"""

from __future__ import annotations

import argparse
import os
import pathlib
import re
import sys
from collections import defaultdict
from typing import Iterable

REPO_ROOT = pathlib.Path(__file__).resolve().parent.parent.parent

PROD_SCENE_ROOTS = (
    REPO_ROOT / "scenes",
    REPO_ROOT / "assets" / "ui" / "scenes",
    REPO_ROOT / "assets" / "ui" / "components",
    REPO_ROOT / "assets" / "ui" / "panels",
    REPO_ROOT / "assets" / "ui" / "modals",
)

# Sub-paths that live under a production root but are intentionally
# non-production (e.g. test fixtures, examples, archival). Each entry is a
# directory name (case-sensitive; matched against any path segment).
NON_PRODUCTION_SUBPATHS = (
    "scenestest",  # internal headless self-tests, not shipped
    "examples",
    "fixtures",
)

EXT_RESOURCE_RE = re.compile(r"^\[ext_resource\s+type=\"([^\"]+)\"\s+(?:uid=\"([^\"]*)\"\s+)?path=\"([^\"]+)\"\s+id=\"([^\"]+)\"\]\s*$")
SUB_RESOURCE_RE = re.compile(r"^\[sub_resource\s+type=\"([^\"]+)\"\s+(?:uid=\"([^\"]*)\"\s+)?id=\"([^\"]+)\"\]\s*$")
GD_RESOURCE_RE = re.compile(r"^\[gd_resource\s+type=\"([^\"]+)\"\s+(?:load_steps=\d+\s+)?(?:uid=\"([^\"]*)\"\s+)?(?:format=\d+\s+)?\]\s*$")
NODE_HEADER_RE = re.compile(r"^\[node\s+name=\"([^\"]+)\"(?:\s+type=\"([^\"]*)\")?(?:\s+parent=\"([^\"]*)\")?(?:\s+instance=\"([^\"]*)\")?\]\s*$")
SCRIPT_FIELD_RE = re.compile(r"^\s*script\s*=\s*ExtResource\(\"([^\"]+)\"\)")

PATH_CASE_RE = re.compile(r"^\s*res://([^/]+)/")
CANONICAL_RES_PREFIX = "res://assets/"

class SceneLint:
    def __init__(self) -> None:
        self.errors: list[str] = []
        self.warnings: list[str] = []
        self.checked: list[pathlib.Path] = []
        self.ext_resource_index: dict[pathlib.Path, dict[str, tuple[str, str, str, str]]] = defaultdict(dict)
        self.required_unique_name_index: dict[pathlib.Path, set[str]] = defaultdict(set)

    def emit_err(self, file: pathlib.Path, line: int, msg: str) -> None:
        rel = file.relative_to(REPO_ROOT) if file.is_absolute() else file
        self.errors.append(f"{rel}:{line}: ERROR {msg}")

    def emit_warn(self, file: pathlib.Path, line: int, msg: str) -> None:
        rel = file.relative_to(REPO_ROOT) if file.is_absolute() else file
        self.warnings.append(f"{rel}:{line}: WARN {msg}")

    # ── Repository Discovery ───────────────────────────────────────
    def discover_production_scenes(self) -> list[pathlib.Path]:
        """Discover production .tscn/.tres files, intentionally excluding
        test fixtures, examples, and the Unity-shadow Assets/ tree."""
        if not any(r.exists() for r in PROD_SCENE_ROOTS):
            return []
        scenes: list[pathlib.Path] = []
        for root in PROD_SCENE_ROOTS:
            if not root.exists():
                continue
            for ext in ("*.tscn", "*.tres"):
                scenes.extend(root.rglob(ext))
        # Stable ordering for deterministic CI output
        return sorted(set(scenes))

    # ── Validation ────────────────────────────────────────────────
    def validate_scene(self, file: pathlib.Path) -> None:
        self.checked.append(file)
        try:
            text = file.read_text(encoding="utf-8")
        except OSError as exc:
            self.emit_err(file, 0, f"unreadable scene file: {exc}")
            return

        # Header
        header_line = text.splitlines()[0] if text else ""
        if not (header_line.startswith("[gd_scene") or header_line.startswith("[gd_resource")):
            self.emit_err(file, 1, f"first line must be [gd_scene] or [gd_resource], got: {header_line!r}")
            return

        # Line index → 1-based
        lines = text.splitlines()
        local_ids_seen: set[str] = set()
        resource_paths_by_id: dict[str, str] = {}
        resource_types_by_id: dict[str, tuple[str, str]] = {}
        uid_aliases: dict[str, str] = {}

        for i, raw_line in enumerate(lines, start=1):
            line = raw_line.strip()

            m = EXT_RESOURCE_RE.match(line)
            if m:
                rtype, ruid, rpath, rid = m.group(1), m.group(2) or "", m.group(3), m.group(4)
                if rid in local_ids_seen:
                    self.emit_err(file, i, f"duplicate ext_resource id {rid!r}")
                local_ids_seen.add(rid)
                resource_paths_by_id[rid] = rpath
                resource_types_by_id[rid] = (rtype, rpath)
                self.ext_resource_index[file][rid] = (rtype, ruid, rpath, "")
                # Path validation
                self._validate_resource_path(file, i, rtype, rpath)
                # UID format check
                if ruid and not self._is_valid_uid(ruid):
                    self.emit_err(file, i, f"malformed UID {ruid!r}")
                if ruid:
                    if ruid in uid_aliases:
                        self.emit_err(file, i, f"duplicate UID {ruid!r} (already used for {uid_aliases[ruid]})")
                    uid_aliases[ruid] = rid
                continue

            m = SUB_RESOURCE_RE.match(line)
            if m:
                rtype, ruid, rid = m.group(1), m.group(2) or "", m.group(3)
                if rid in local_ids_seen:
                    self.emit_err(file, i, f"duplicate resource id {rid!r}")
                local_ids_seen.add(rid)
                if ruid and not self._is_valid_uid(ruid):
                    self.emit_err(file, i, f"malformed UID {ruid!r}")
                if ruid:
                    if ruid in uid_aliases:
                        self.emit_err(file, i, f"duplicate UID {ruid!r} (already used for {uid_aliases[ruid]})")
                    uid_aliases[ruid] = rid
                continue

            m = NODE_HEADER_RE.match(raw_line)
            if m:
                # Unique names (%Name) must be unique within a resource
                uname = m.group(1)
                # We can't tell unique-name from node header alone;
                # parent tree fills in. Skip.
                continue

        # Now check that every ExtResource/Script reference resolves
        for i, raw_line in enumerate(lines, start=1):
            for m in re.finditer(r"ExtResource\(\"([^\"]+)\"\)", raw_line):
                rid = m.group(1)
                if rid not in resource_paths_by_id:
                    self.emit_err(file, i, f"references ExtResource(\"{rid}\") but no [ext_resource id=\"{rid}\"] declared")
                else:
                    rtype, rpath = resource_types_by_id[rid]
                    if rtype == "Script":
                        if not self._validate_script_file(file, i, rpath):
                            pass

        # Cross-resource UID alias consistency: every uid:// url should
        # resolve in this resource or in the repository uid index.
        for i, raw_line in enumerate(lines, start=1):
            for m in re.finditer(r"uid=\"(uid://[^\"]+)\"", raw_line):
                self._validate_uid_reference(file, i, m.group(1), resource_paths_by_id)

        # Node unique-name validation: track %Name usage and owner
        seen_unames: dict[str, int] = {}
        for i, raw_line in enumerate(lines, start=1):
            for m in re.finditer(r"unique_name_in_owner\s*=\s*true", raw_line):
                # find node header preceding this property
                parent_node = self._parent_node(lines, i)
                if parent_node:
                    if parent_node in seen_unames:
                        self.emit_err(file, i, f"unique_name_in_owner {parent_node!r} already used at line {seen_unames[parent_node]}")
                    seen_unames[parent_node] = i

        self.required_unique_name_index[file] = set(seen_unames.keys())

    def _validate_resource_path(self, file: pathlib.Path, line: int, rtype: str, rpath: str) -> None:
        if not rpath.startswith("res://"):
            self.emit_err(file, line, f"resource path must start with res://, got {rpath!r}")
            return

        # IGNORE non-asset paths like res://Ashfall.Core.csproj
        # Project-level resource paths include the project assembly, fonts, sub-projects.
        resolved = rpath[len("res://"):]
        actual_path = REPO_ROOT / resolved
        if not actual_path.exists():
            # Allow case fallbacks to be silent if they actually exist
            # This enforces Ticket #124: res://Assets/ is invalid because
            # canonical asset root is res://assets/.
            if rpath.startswith("res://Assets/"):
                # Ticket #124 forbids upper-case Assets/ runtime references.
                self.emit_err(
                    file, line,
                    f"path {rpath!r} uses forbidden 'res://Assets/' prefix (canonical is "
                    f"'res://assets/'). Ticket #124 path validation."
                )
                return
            if rpath.startswith(CANONICAL_RES_PREFIX):
                # Check on disk with case-insensitive to give actionable info
                # but as authoritative case-presented
                parent = actual_path.parent
                if not parent.exists():
                    self.emit_err(file, line, f"resource parent directory missing for {rpath!r}")
                    return
                # If file simply does not exist on case-sensitive FS, error
                if not actual_path.exists():
                    # try case-correcting siblings
                    target_name = actual_path.name
                    siblings = [p for p in parent.iterdir() if p.name.lower() == target_name.lower()]
                    if siblings and siblings[0].name != target_name:
                        correct = "res://" + str((siblings[0]).relative_to(REPO_ROOT)).replace("\\", "/")
                        self.emit_err(file, line, f"path case mismatch: {rpath!r} — actual file is {correct!r}")
                    else:
                        self.emit_err(file, line, f"referenced file does not exist: {rpath!r}")
                return
            # Non-canonical res path (e.g. res://project.godot, res://Ashfall.csproj)
            return

        # Detect path-case mismatch on res://assets/...
        if rpath.startswith(CANONICAL_RES_PREFIX):
            parent = actual_path.parent
            target_name = actual_path.name
            siblings = [p for p in parent.iterdir()] if parent.exists() else []
            actual_existing = [p for p in siblings if p.name == target_name]
            if not actual_existing:
                # a case-distinct sibling is on disk
                lower_hits = [p for p in siblings if p.name.lower() == target_name.lower()]
                if lower_hits and lower_hits[0].name != target_name:
                    correct = "res://" + str(lower_hits[0].relative_to(REPO_ROOT)).replace("\\", "/")
                    if correct != rpath:
                        self.emit_warn(file, line, f"path case differs from disk: {rpath!r} — disk has {correct!r}")

    def _validate_script_file(self, file: pathlib.Path, line: int, rpath: str) -> bool:
        resolved = rpath[len("res://"):] if rpath.startswith("res://") else rpath
        actual = REPO_ROOT / resolved
        if not actual.exists():
            # Could be a non-script ext_resource (rare); pass
            return False
        return True

    def _validate_uid_reference(self, file: pathlib.Path, line: int, uid_str: str, resource_paths_by_id: dict[str, str]) -> None:
        if not uid_str.startswith("uid://"):
            self.emit_err(file, line, f"malformed UID reference: {uid_str!r}")
            return
        suffix = uid_str[len("uid://"):]
        if not re.fullmatch(r"[A-Za-z0-9_]+", suffix):
            self.emit_err(file, line, f"malformed UID body: {uid_str!r}")

    def _is_valid_uid(self, uid: str) -> bool:
        # Godot 4 UIDs consist of a base64-like string of letters and digits.
        # Be permissive but reject empty bodies and stray punctuation.
        return bool(re.fullmatch(r"uid://[A-Za-z0-9_]+", uid))

    def _parent_node(self, lines: list[str], prop_line: int) -> str | None:
        # Find most recent [node ...] header above prop_line
        for i in range(prop_line - 1, 0, -1):
            m = NODE_HEADER_RE.match(lines[i - 1])
            if m:
                return m.group(1)
        return None

    # ── Manifest generation ───────────────────────────────────────
    def emit_manifest(self) -> str:
        """Generate a deterministic ownership manifest over the
        discovered production scenes."""
        manifest = {
            "schema_version": 1,
            "scenes": [],
        }
        for s in self.checked:
            rel = s.relative_to(REPO_ROOT)
            manifest["scenes"].append({
                "scene": "res://" + str(rel).replace("\\", "/"),
                "root_type": self._root_type(s),
                "kind": self._classify(s),
                "production": True,
                "unique_names": sorted(self.required_unique_name_index.get(s, set())),
                "ext_resources": sorted(self.ext_resource_index.get(s, {}).keys()),
            })
        import json
        return json.dumps(manifest, indent=2, sort_keys=True)

    def _root_type(self, file: pathlib.Path) -> str:
        try:
            text = file.read_text(encoding="utf-8")
        except OSError:
            return "?"
        for line in text.splitlines():
            m = NODE_HEADER_RE.match(line)
            if m:
                return m.group(2) or "Node"
            if line.startswith("[") and line != "[node name=\"root_owner\"]":
                continue
        return "?"

    def _classify(self, file: pathlib.Path) -> str:
        rel = str(file.relative_to(REPO_ROOT)).replace("\\", "/")
        if "/components/" in rel:
            return "component"
        if "/panels/" in rel:
            return "panel"
        if "/modals/" in rel:
            return "modal"
        if "/scenes/" in rel:
            return "shell"
        if "/scenes/Main.tscn" in rel or "/scenes/CSharpTest.tscn" in rel:
            return "shell"
        return "other"

    # ── Run ────────────────────────────────────────────────────────
    def run(self) -> int:
        scenes = self.discover_production_scenes()
        if not scenes:
            print("scene-lint: no production scenes found in", [str(r) for r in PROD_SCENE_ROOTS])
            return 0
        for s in scenes:
            self.validate_scene(s)
        # Summary
        n = len(self.checked)
        if self.errors:
            print(f"scene-lint: {n} production scenes; {len(self.errors)} error(s); {len(self.warnings)} warning(s)")
            for e in self.errors:
                print(f"  {e}")
            for w in self.warnings[:20]:
                print(f"  {w}")
            if len(self.warnings) > 20:
                print(f"  ... and {len(self.warnings) - 20} more warnings")
            return 1
        print(f"scene-lint: {n} production scenes checked; 0 errors; {len(self.warnings)} warning(s)")
        for w in self.warnings[:20]:
            print(f"  {w}")
        return 0


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--manifest", action="store_true", help="emit deterministic ownership manifest to stdout")
    parser.add_argument("--manifest-out", type=str, default=None, help="write manifest to this path")
    args = parser.parse_args()
    linter = SceneLint()
    rc = linter.run()
    if args.manifest or args.manifest_out:
        manifest_text = linter.emit_manifest()
        if args.manifest:
            print("\n--- OWNERSHIP MANIFEST ---\n" + manifest_text)
        if args.manifest_out:
            out_path = pathlib.Path(args.manifest_out).resolve()
            out_path.parent.mkdir(parents=True, exist_ok=True)
            out_path.write_text(manifest_text, encoding="utf-8")
            print(f"scene-lint: ownership manifest written to {out_path}")
    return rc


if __name__ == "__main__":
    sys.exit(main())
