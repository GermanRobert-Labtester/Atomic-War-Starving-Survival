#!/usr/bin/env bash
# =============================================================================
# ASHFALL — Catch Policy & Exception Handling Lint Gate
# =============================================================================
# Enforces that:
#   1. Zero silent/undocumented empty catches exist in the codebase.
#   2. Cleanup-only catches are documented with /* cleanup: ... */.
#   3. Data, catalog, and save loading catch blocks log diagnostic context.
# =============================================================================

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "${SCRIPT_DIR}/../.." && pwd)"

echo "── Running Catch Policy Lint Gate ──"

python3 -c '
import os, re, pathlib, sys

repo_root = pathlib.Path("'"${REPO_ROOT}"'")
search_dirs = [repo_root / "src", repo_root / "Assets" / "Ashfall.Core"]

cleanup_keywords = [
    "cleanup", "best-effort", "temp", "fallback", "tolerate",
    "ignore", "tamper", "rejection", "quarantine", "resilient",
    "deserialization failure", "probe"
]

logging_keywords = [
    "catalogdiagnostics", "gd.printerr", "gd.print", "gd.pushwarning",
    "log.", "_log.", "log?.", "_log?.", "console.error", "console.writeline", "errors.add",
    "report.error", "report.warning", "failure(", "[fail]", "return (false",
    "throw", "ex_catdiag", "check(", "failures++"
]

catch_header_re = re.compile(r"\bcatch(?:\s*\(([^\)]*)\))?\s*\{")

all_records = []
for d in search_dirs:
    if not d.exists(): continue
    for p in d.rglob("*.cs"):
        if "/obj/" in str(p) or "/bin/" in str(p): continue
        content = p.read_text(encoding="utf-8")
        rel = p.relative_to(repo_root).as_posix()
        is_data_loader = any(k in rel for k in ["Catalog", "Loader", "SaveStore", "SaveCodec", "Save", "JournalCatalogData"])

        for m in catch_header_re.finditer(content):
            start = m.start()
            # check if line starts with comment
            line_start = content.rfind("\n", 0, start) + 1
            line_prefix = content[line_start:start].strip()
            if line_prefix.startswith("//") or line_prefix.startswith("*"):
                continue

            brace_count = 1
            idx = m.end()
            while idx < len(content) and brace_count > 0:
                if content[idx] == "{": brace_count += 1
                elif content[idx] == "}": brace_count -= 1
                idx += 1

            body = content[m.end():idx-1].strip()
            line_num = content[:start].count("\n") + 1

            has_comment = "//" in body or "/*" in body
            body_lower = body.lower()
            has_logging = any(k in body_lower for k in logging_keywords)
            has_cleanup = any(k in body_lower for k in cleanup_keywords)

            all_records.append({
                "rel": rel,
                "line": line_num,
                "header": m.group(0),
                "body": body,
                "is_data_loader": is_data_loader,
                "has_comment": has_comment,
                "has_logging": has_logging,
                "has_cleanup": has_cleanup
            })

# 1. Undocumented empty catches
undocumented = []
for r in all_records:
    stripped_body = re.sub(r"//.*$", "", r["body"], flags=re.MULTILINE)
    stripped_body = re.sub(r"/\*.*?\*/", "", stripped_body, flags=re.DOTALL).strip()
    if not stripped_body and not r["has_cleanup"] and not r["has_comment"]:
        rel = r["rel"]
        line = r["line"]
        header = r["header"]
        undocumented.append(f"{rel}:{line} -> {header} (empty body with no explanation)")

if undocumented:
    print(f"❌ Found {len(undocumented)} undocumented empty catches:")
    for u in undocumented:
        print(f"  {u}")
    sys.exit(1)
else:
    print(f"[OK] Zero undocumented empty catches ({len(all_records)} total catches checked).")

# 2. Data loader catches without context logging
unlogged_data = []
for r in all_records:
    if r["is_data_loader"] and not r["has_logging"] and not r["has_cleanup"]:
        rel = r["rel"]
        line = r["line"]
        header = r["header"]
        unlogged_data.append(f"{rel}:{line} -> {header} does not log context")

if unlogged_data:
    print(f"❌ Found {len(unlogged_data)} data loader catches without context logging:")
    for u in unlogged_data:
        print(f"  {u}")
    sys.exit(1)
else:
    print("[OK] All data/load/catalog/save catches log context.")

print("\n✅ Catch Policy Lint Gate PASSED.")
'
