#!/usr/bin/env python3
"""language-policy-gate.py — enforce ASHFALL's four-language policy.

Permitted: C#, Rust, Python, GDScript. Prohibited: Go, JavaScript, TypeScript,
C, C++, Java, Kotlin, Swift, Objective-C, Lua, Ruby, PHP, Dart, Zig, Haskell,
Perl, VB, and any other unlisted language (see docs/ci/LANGUAGE_POLICY.md).

This scans tracked files by extension and fails when a prohibited-language
source file is present outside the allowlist. The transitional Go toolchain
(tools/gotools/**), which is being ported to Rust, is allowlisted until Stage 4
of `.ai/plans/rust-port-gotools-2026-10-02.md` deletes it.

Usage:
  python3 scripts/ci/language-policy-gate.py            # human check
  python3 scripts/ci/language-policy-gate.py --json     # machine-readable

Exit codes: 0 = only permitted/allowlisted sources; 1 = prohibited source found.
"""

from __future__ import annotations

import argparse
import json
import subprocess
import sys
from pathlib import Path

REPO_ROOT = Path(__file__).resolve().parent.parent.parent

# Extension -> language, for the prohibited set only. Permitted languages
# (cs, rs, py/gd) and config/data formats are intentionally absent.
PROHIBITED = {
    "go": "Go",
    "js": "JavaScript", "mjs": "JavaScript", "cjs": "JavaScript", "jsx": "JavaScript",
    "ts": "TypeScript", "tsx": "TypeScript",
    "c": "C", "h": "C/C++ header",
    "cc": "C++", "cpp": "C++", "cxx": "C++", "hpp": "C++", "hxx": "C++",
    "java": "Java", "kt": "Kotlin", "kts": "Kotlin",
    "swift": "Swift", "m": "Objective-C", "mm": "Objective-C++",
    "lua": "Lua", "rb": "Ruby", "php": "PHP", "dart": "Dart", "zig": "Zig",
    "hs": "Haskell", "scala": "Scala", "pl": "Perl", "pm": "Perl",
    "vb": "Visual Basic",
}

# Transitional allowlist: Go being ported to Rust, deleted in Stage 4.
ALLOWED_PREFIXES = ("tools/gotools/",)
# Not application logic: precompiled/external-ish trees, generated output.
ALLOWED_ANYWHERE = ("node_modules/", ".godot/", "build/", "obj/", "bin/")


def tracked_files(root: Path) -> list:
    out = subprocess.run(
        ["git", "ls-files"], cwd=str(root),
        capture_output=True, text=True, timeout=60, check=True,
    ).stdout
    return [line for line in out.splitlines() if line]


def is_allowed(path: str) -> bool:
    if any(path.startswith(p) for p in ALLOWED_PREFIXES):
        return True
    if any(seg in path for seg in ALLOWED_ANYWHERE):
        return True
    return False


def scan(root: Path) -> list:
    findings = []
    for path in tracked_files(root):
        ext = path.rsplit(".", 1)[-1].lower() if "." in Path(path).name else ""
        if ext in PROHIBITED and not is_allowed(path):
            findings.append({"path": path, "language": PROHIBITED[ext]})
    return findings


def main() -> int:
    parser = argparse.ArgumentParser(description="ASHFALL language-policy gate.")
    parser.add_argument("--json", action="store_true", help="Machine-readable output")
    parser.add_argument("--root", default=str(REPO_ROOT), help="Repository root (default: this repo)")
    args = parser.parse_args()

    findings = scan(Path(args.root))

    if args.json:
        print(json.dumps({
            "permitted": ["C#", "Rust", "Python", "GDScript"],
            "allowlisted_prefixes": list(ALLOWED_PREFIXES),
            "violations": findings,
            "count": len(findings),
        }, indent=2))
    else:
        if findings:
            print(f"LANGUAGE_POLICY FAIL: {len(findings)} prohibited source file(s):")
            for f in findings:
                print(f"  ✗ [{f['language']}] {f['path']}")
            print("\nPermitted languages: C#, Rust, Python, GDScript. "
                  "See docs/ci/LANGUAGE_POLICY.md.")
        else:
            print("LANGUAGE_POLICY PASS: no prohibited-language source outside the "
                  "transitional allowlist (tools/gotools/**).")

    return 1 if findings else 0


if __name__ == "__main__":
    sys.exit(main())