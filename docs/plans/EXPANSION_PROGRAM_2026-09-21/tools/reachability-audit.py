#!/usr/bin/env python3
"""Read-only ASHFALL host-reachability audit (proposal artifact, not production tooling).

Computes which Core authority files are unreachable from the Godot host,
eliminating the false positives of a plain grep scan (Core-composed systems).

Usage (from the repository root):
    python3 docs/plans/EXPANSION_PROGRAM_2026-09-21/tools/reachability-audit.py

Output: a plain-text report on stdout. No files are written.
This script must not be wired into CI until PLAN-INTEGRATION-KIT-02 claims the
production version.
"""

import os
import re
import sys
from collections import defaultdict, deque

AUTHORITY_SUFFIXES = ("System", "Engine", "Coordinator", "Manager")


def repo_root() -> str:
    here = os.path.dirname(os.path.abspath(__file__))
    return os.path.abspath(os.path.join(here, "..", "..", "..", ".."))


def cs_files(root: str, rel: str):
    out = []
    for base, _dirs, files in os.walk(os.path.join(root, rel)):
        for f in files:
            if f.endswith(".cs"):
                out.append(os.path.join(base, f))
    return out


def read(path: str) -> str:
    try:
        with open(path, encoding="utf-8", errors="ignore") as fh:
            return fh.read()
    except OSError:
        return ""


def main() -> int:
    root = repo_root()
    core = cs_files(root, os.path.join("Assets", "Ashfall.Core"))
    host = cs_files(root, "src")
    tests = cs_files(root, os.path.join("Ashfall.Core.Tests"))

    type_re = re.compile(
        r"^\s*(?:public|internal)\s+"
        r"(?:sealed\s+|abstract\s+|static\s+|partial\s+|readonly\s+|ref\s+)*"
        r"\b(?:class|record|struct|interface|enum|delegate)\s+([A-Za-z_]\w*)",
        re.M,
    )
    token_re = re.compile(r"\b[A-Za-z_]\w*\b")

    file_types = defaultdict(set)
    token_cache = {}
    core_text = {}
    for path in core:
        text = read(path)
        core_text[path] = text
        token_cache[path] = set(token_re.findall(text))
        for m in type_re.finditer(text):
            file_types[m.group(1)].add(path)

    host_tokens = set()
    for path in host:
        host_tokens |= set(token_re.findall(read(path)))

    roots = {name for name, files in file_types.items() if name in host_tokens}

    reachable = set()
    queue = deque(roots)
    while queue:
        name = queue.popleft()
        for path in file_types.get(name, ()):
            if path in reachable:
                continue
            reachable.add(path)
            for tok in token_cache[path]:
                if tok in file_types:
                    queue.append(tok)

    test_tokens = set()
    for path in tests:
        test_tokens |= set(token_re.findall(read(path)))

    print("ASHFALL host-reachability audit")
    print(f"core files: {len(core)}  host files: {len(host)}  test files: {len(tests)}")
    print(f"roots (Core type names seen in host): {len(roots)}")

    # Name tokens used anywhere outside a type's own defining file.
    # core token sets already exist; host and test token sets are union sets,
    # which is sufficient for a "referenced anywhere else" check at type level.
    other_files_tokens = set(host_tokens) | set(test_tokens)

    orphan_files = []
    dead_types = []
    for path in sorted(core):
        if path in reachable:
            continue
        authorities = sorted(
            n for n, fs in file_types.items()
            if path in fs and n.endswith(AUTHORITY_SUFFIXES)
        )
        if not authorities:
            continue
        tested = [a for a in authorities if a in test_tokens]
        rel = os.path.relpath(path, root)
        orphan_files.append((rel, authorities, len(tested)))
        for a in authorities:
            # "fully dead" = the type name is referenced by no other file at
            # all: not the host, not the tests, and not another Core file.
            referenced_in_core = any(
                a in token_cache[p] for p in core if p != path
            )
            if a not in other_files_tokens and not referenced_in_core:
                dead_types.append((rel, a))

    print(f"\nhost-unreachable authority files: {len(orphan_files)}")
    print(f"  of which fully dead authority types: {len(dead_types)}")
    print()
    print(f"{'authority':52} {'tests':>5}  file")
    for rel, authorities, tested in orphan_files:
        print(f"{authorities[0]:52} {tested:>5}  {rel}")

    if dead_types:
        print("\nFULLY DEAD (type referenced by no other file):")
        for rel, a in dead_types:
            print(f"  {a}  ({rel})")

    # Type-level dead scan across ALL Core authority types, including those in
    # otherwise-reachable files (e.g. an unused engine beside a live loader).
    print("\nType-level dead authority scan (all Core authority types):")
    host_and_test = set(host_tokens) | set(test_tokens)
    dead_all = []
    for name, defining_set in sorted(file_types.items()):
        if not name.endswith(AUTHORITY_SUFFIXES):
            continue
        if name in host_and_test:
            continue
        defining = sorted(defining_set)[0]
        referenced_in_core = any(
            name in token_cache[p] for p in core if p not in defining_set
        )
        if not referenced_in_core:
            dead_all.append((name, os.path.relpath(defining, root)))
    print(f"  fully dead authority types: {len(dead_all)}")
    for name, rel in dead_all:
        print(f"  {name}  ({rel})")

    return 0


if __name__ == "__main__":
    sys.exit(main())
