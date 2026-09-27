#!/usr/bin/env python3
"""Transitive-reachability audit for host-orphan Core systems (ASHFALL).

A Core file is truly orphaned only if NO type declared in it is transitively
reachable from src/. Reachability = the set of Core types appearing in src text,
expanded transitively over the type-reference graph (type T -> all types declared
in files that mention T). This resolves the transitive-consumer trap that a flat
name-grep produces, which caused duplicate-authority defects in earlier batches.
"""
import os, re, collections

ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
CORE = os.path.join(ROOT, "Assets", "Ashfall.Core")
SRC = os.path.join(ROOT, "src")
TESTS = os.path.join(ROOT, "Ashfall", "Core.Tests")

TYPE_RE = re.compile(
    r"\b(?:public|internal|sealed|static|partial|abstract)\s+(?:class|struct|interface|enum|record)\s+(\w+)"
)
NAME_RE = re.compile(r"\b([A-Z][A-Za-z0-9_]{3,})\b")


def read(p):
    try:
        with open(p, "r", encoding="utf-8", errors="ignore") as fh:
            return fh.read()
    except OSError:
        return ""


def py_files(base):
    for dirpath, _, names in os.walk(base):
        if "/obj/" in dirpath + "/" or "/bin/" in dirpath + "/":
            continue
        for n in names:
            if n.endswith(".cs"):
                yield os.path.join(dirpath, n)


core_files = {os.path.relpath(p, ROOT): p for p in py_files(CORE)}
src_files = {os.path.relpath(p, ROOT): p for p in py_files(SRC)}
test_files = {os.path.relpath(p, ROOT): p for p in py_files(TESTS)}

# --- Core type declarations
declared_by_file = collections.defaultdict(set)   # rel -> {type,...}
declaring_file = collections.defaultdict(set)     # type -> {rel,...}
for rel, path in core_files.items():
    for t in set(TYPE_RE.findall(read(path))):
        declared_by_file[rel].add(t)
        declaring_file[t].add(rel)
all_core_types = set(declaring_file)

# --- mentions per file (word-boundary, catches generics/typeof/property refs)
mentions = {}
for rel, path in {**core_files, **src_files, **test_files}.items():
    mentions[rel] = set(NAME_RE.findall(read(path)))

# --- consumers_of: type -> set of Core files that mention it
consumers_of = collections.defaultdict(set)
for rel in core_files:
    for t in mentions[rel]:
        consumers_of[t].add(rel)


def visible_from(texts):
    """Core types transitively reachable from the given texts.

    File-level closure: a Core file becomes reachable when a reachable file
    mentions ANY type it declares, and every type mentioned by a reachable file
    is itself reachable. A type-only expansion misses uses of a type whose own
    declaring file never mentions the reachable type back (e.g. an
    engine-agnostic state record consumed by a reachable partial class).
    """
    seen_types = set()
    seen_files = set(src_files)
    for txt in texts:
        for w in NAME_RE.findall(txt):
            if w in all_core_types:
                seen_types.add(w)
    for _ in range(80):
        new_files = set()
        for t in list(seen_types):
            for cfile in consumers_of.get(t, ()):
                if cfile not in seen_files:
                    new_files.add(cfile)
        for f in new_files:
            for w in mentions[f]:
                if w in all_core_types:
                    seen_types.add(w)
        seen_files |= new_files
        if not new_files:
            break
    return seen_types


srctext = "".join(read(p) for p in src_files.values())
testtext = "".join(read(p) for p in test_files.values())
src_visible = visible_from([srctext])

# --- candidates: Core files naming a subsystem authority pattern
CAND = re.compile(
    r"(System|Engine|Coordinator|Registry|Scheduler|Simulator|Orchestrator|Authority|Resolutor|Director)$"
)
candidates = []
for rel in core_files:
    base = os.path.basename(rel)[:-3]
    if not CAND.search(base):
        continue
    types = declared_by_file[rel]
    if not types:
        continue
    orphans = [t for t in types if t not in src_visible]
    if not orphans:            # some type already reachable -> wired
        continue
    if orphans != sorted(types):
        continue                # partially wired -> a duplicate-authority trap
    ntest = len([t for t in types if t in set(NAME_RE.findall(testtext))])
    candidates.append((rel, sorted(types), ntest))

print(f"### HOST-ORPHAN CANDIDATES (transitive gate): {len(candidates)}")
for rel, types, nt in sorted(candidates):
    print(f"  {rel}  types={','.join(types)[:100]}  test_refs={nt}")
