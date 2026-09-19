#!/usr/bin/env python3
# SPDX-License-Identifier: MIT
"""
generate-port-contract.py — Port Contracts, Host Wiring Manifest & CI Verification Gate

Authoritative generator and gate for Plan 36 (C2[13]):
- Validates docs/ci/port_contract_policy.json against Core seams and src/ callers.
- Enforces that all HOST_REQUIRED seams have observed callers in src/.
- Enforces that no DEFERRED seams are called in src/ without an upgrade.
- Enforces that no DEFERRED seams have expired.
- Enforces that all Core integration seams are tracked (0 unclassified seams).
- Generates docs/architecture/PORT_CONTRACT.md and docs/architecture/port-contract.json.

Usage:
  python3 scripts/ci/generate-port-contract.py          # Generate docs
  python3 scripts/ci/generate-port-contract.py --check  # Verify gate in CI
"""

import json
import os
import pathlib
import re
import sys
from datetime import datetime, timezone

REPO_ROOT = pathlib.Path(__file__).resolve().parent.parent.parent
POLICY_PATH = REPO_ROOT / "docs" / "ci" / "port_contract_policy.json"
CORE_DIR = REPO_ROOT / "Assets" / "Ashfall.Core"
SRC_DIR = REPO_ROOT / "src"
MD_OUTPUT_PATH = REPO_ROOT / "docs" / "architecture" / "PORT_CONTRACT.md"
JSON_OUTPUT_PATH = REPO_ROOT / "docs" / "architecture" / "port-contract.json"


def strip_comments_and_strings(text: str) -> str:
    def replacer(match):
        s = match.group(0)
        return " " if s.startswith("/") else s
    pattern = re.compile(
        r"//.*?$|/\*.*?\*/|\"(?:\\.|[^\\\"])*\"|'\\?(?:\\.|[^\\'])*'",
        re.DOTALL | re.MULTILINE
    )
    return re.sub(pattern, replacer, text)


def find_core_seams():
    """Scans Core for public Bind*, Wire*, Register*, Configure* methods."""
    token_pattern = re.compile(
        r"\b(?:class|struct|interface|record)\s+([A-Za-z0-9_]+)|(\{)|(\})|"
        r"public\s+(?:static\s+|override\s+|virtual\s+|async\s+)*(?:[\w<>\[\]?,]+\s+)+(Bind\w*|Wire\w*|Register\w*|Configure\w*)\s*\("
    )

    core_methods = {}
    for root, _, files in os.walk(CORE_DIR):
        for fn in files:
            if not fn.endswith(".cs"):
                continue
            fp = pathlib.Path(root) / fn
            try:
                raw = fp.read_text(encoding="utf-8", errors="ignore")
            except Exception:
                continue

            clean = strip_comments_and_strings(raw)
            class_stack = []
            for m in token_pattern.finditer(clean):
                if m.group(1):
                    class_stack.append([m.group(1), 0])
                elif m.group(2):
                    if class_stack:
                        class_stack[-1][1] += 1
                elif m.group(3):
                    if class_stack:
                        class_stack[-1][1] -= 1
                        if class_stack[-1][1] <= 0:
                            class_stack.pop()
                elif m.group(4):
                    mname = m.group(4)
                    cname = class_stack[-1][0] if class_stack else "Unknown"
                    rel_path = str(fp.relative_to(REPO_ROOT)).replace("\\", "/")
                    core_methods[f"{cname}.{mname}"] = {
                        "class_name": cname,
                        "method_name": mname,
                        "file_path": rel_path
                    }
    return core_methods


def load_src_texts():
    src_texts = {}
    for root, _, files in os.walk(SRC_DIR):
        for fn in files:
            if fn.endswith(".cs"):
                fp = pathlib.Path(root) / fn
                rel = str(fp.relative_to(REPO_ROOT)).replace("\\", "/")
                try:
                    src_texts[rel] = fp.read_text(encoding="utf-8", errors="ignore")
                except Exception:
                    pass
    return src_texts


def find_callers_in_src(method_name: str, src_texts: dict):
    pattern = re.compile(r"\b" + re.escape(method_name) + r"\s*\(")
    callers = []
    for rel_path, text in src_texts.items():
        if pattern.search(text):
            callers.append(rel_path)
    return sorted(callers)


def validate_and_generate(check_mode: bool = False):
    if not POLICY_PATH.exists():
        print(f"[FAIL] Missing policy file at {POLICY_PATH}", file=sys.stderr)
        return 1

    try:
        policy = json.loads(POLICY_PATH.read_text(encoding="utf-8"))
    except Exception as ex:
        print(f"[FAIL] Malformed policy JSON: {ex}", file=sys.stderr)
        return 1

    ports = policy.get("ports", [])
    total_seams = policy.get("total_seams", len(ports))
    core_seams = find_core_seams()
    src_texts = load_src_texts()

    errors = []
    policy_by_key = {}

    for p in ports:
        cname = p.get("class_name", "")
        mname = p.get("method_name", "")
        key = f"{cname}.{mname}"

        if not cname or not mname:
            errors.append(f"Entry missing class_name or method_name: {p}")
            continue

        if key in policy_by_key:
            errors.append(f"Duplicate seam in policy: {key}")
        policy_by_key[key] = p

        if not p.get("owner"):
            errors.append(f"Seam '{key}' missing required 'owner'")
        if not p.get("reason"):
            errors.append(f"Seam '{key}' missing required 'reason'")

        classification = p.get("classification")
        valid_classes = {"HOST_REQUIRED", "OPTIONAL_HOST", "LIVE_VIA_CORE", "TEST_ONLY", "PURE_LIBRARY", "DEFERRED"}
        if classification not in valid_classes:
            errors.append(f"Seam '{key}' has invalid classification '{classification}'")

        callers = find_callers_in_src(mname, src_texts)
        p["observed_callers"] = callers

        if classification == "HOST_REQUIRED":
            if not callers:
                errors.append(f"HOST_REQUIRED seam '{key}' has NO callers in src/")
        elif classification == "DEFERRED":
            if callers:
                errors.append(f"DEFERRED seam '{key}' is called in src/ ({callers[0]}) — must upgrade to HOST_REQUIRED")
            if not p.get("activation_condition"):
                errors.append(f"DEFERRED seam '{key}' missing 'activation_condition'")
            expiry = p.get("expiry")
            if not expiry:
                errors.append(f"DEFERRED seam '{key}' missing 'expiry' date")
            else:
                try:
                    exp_dt = datetime.strptime(expiry, "%Y-%m-%d").replace(tzinfo=timezone.utc)
                    now = datetime.now(timezone.utc)
                    if now > exp_dt:
                        errors.append(f"DEFERRED seam '{key}' expired on {expiry}; must be wired or re-budgeted")
                except ValueError:
                    errors.append(f"DEFERRED seam '{key}' has unparseable expiry date '{expiry}'")

    # Check for unclassified Core seams
    untracked = set(core_seams.keys()) - set(policy_by_key.keys())
    for k in sorted(untracked):
        errors.append(f"Untracked Core seam '{k}' in {core_seams[k]['file_path']}")

    # Check for stale policy entries
    stale = set(policy_by_key.keys()) - set(core_seams.keys())
    for k in sorted(stale):
        errors.append(f"Stale policy entry '{k}' does not exist in Core")

    if errors:
        print(f"[FAIL] Port contract policy validation failed with {len(errors)} error(s):", file=sys.stderr)
        for err in errors:
            print(f"  - {err}", file=sys.stderr)
        return 1

    # Count categories
    counts = {
        "HOST_REQUIRED": sum(1 for p in ports if p["classification"] == "HOST_REQUIRED"),
        "OPTIONAL_HOST": sum(1 for p in ports if p["classification"] == "OPTIONAL_HOST"),
        "LIVE_VIA_CORE": sum(1 for p in ports if p["classification"] == "LIVE_VIA_CORE"),
        "TEST_ONLY": sum(1 for p in ports if p["classification"] == "TEST_ONLY"),
        "PURE_LIBRARY": sum(1 for p in ports if p["classification"] == "PURE_LIBRARY"),
        "DEFERRED": sum(1 for p in ports if p["classification"] == "DEFERRED"),
    }

    # Generate port-contract.json
    generated_json_data = {
        "schema_version": "1.0.0",
        "description": "Machine-readable manifest of all Core integration ports and host wiring contracts (Plan 36).",
        "generated_at": "2026-09-18",
        "total_seams": len(ports),
        "counts": counts,
        "ports": [
            {
                "port_id": f"{p.get('owner', 'core')}.{p['class_name'].lower()}.{p['method_name'].lower()}",
                "class_name": p["class_name"],
                "method_name": p["method_name"],
                "file_path": p["file_path"],
                "owner": p["owner"],
                "classification": p["classification"],
                "status": "BOUND" if p.get("observed_callers") else ("LIVE_CORE" if p["classification"] == "LIVE_VIA_CORE" else "EXEMPT"),
                "observed_caller_count": len(p.get("observed_callers", [])),
                "observed_callers": p.get("observed_callers", []),
                "reason": p["reason"],
                "diagnostic": p.get("diagnostic", False),
                "activation_condition": p.get("activation_condition"),
                "expiry": p.get("expiry")
            }
            for p in ports
        ]
    }
    generated_json_text = json.dumps(generated_json_data, indent=2) + "\n"

    # Generate PORT_CONTRACT.md
    md_lines = [
        "# Core Port Contracts and Host Wiring Manifest",
        "",
        "> **Plan 36 / C2[13] Authority:** Machine-declared integration seams, host wiring expectations, and CI gate.",
        "",
        "## Summary Metrics",
        "",
        f"- **Total integration seams:** {len(ports)}",
        f"- **Host-required (`HOST_REQUIRED`):** {counts['HOST_REQUIRED']} (all verified called from `src/`)",
        f"- **Optional host ports (`OPTIONAL_HOST`):** {counts['OPTIONAL_HOST']}",
        f"- **Live via Core (`LIVE_VIA_CORE`):** {counts['LIVE_VIA_CORE']}",
        f"- **Test/Diagnostic only (`TEST_ONLY`):** {counts['TEST_ONLY']}",
        f"- **Pure library utilities (`PURE_LIBRARY`):** {counts['PURE_LIBRARY']}",
        f"- **Deferred / Exemptions (`DEFERRED`):** {counts['DEFERRED']} (shrink-only ratchet with dated owner)",
        "- **Unbound production-required seams:** 0",
        "",
        "## Taxonomy & Classification Rules",
        "",
        "| Classification | Requirement | Verification Invariant |",
        "|---|---|---|",
        "| `HOST_REQUIRED` | Production-required | Must have >= 1 caller in `src/`. Missing caller fails fast CI gate. |",
        "| `OPTIONAL_HOST` | Host capability | Null-safe / optional adapter in host. |",
        "| `LIVE_VIA_CORE` | Domain call chain | Invoked internally within `Assets/Ashfall.Core`. |",
        "| `TEST_ONLY` | Test harness | Verified in `Ashfall.Core.Tests/` or diagnostic runner. |",
        "| `PURE_LIBRARY` | Engine-free math/utility | Pure functional or data transformation logic. |",
        "| `DEFERRED` | Formal debt ratchet | Explicit expiry date and activation condition. Caller in `src/` triggers upgrade failure. |",
        "",
        "## Active Seam Directory",
        "",
        "| Seam | Owner | Classification | Callers in `src/` | Status | Reason / Activation |",
        "|---|---|---|---:|---|---|"
    ]

    for p in ports:
        seam_name = f"`{p['class_name']}.{p['method_name']}`"
        caller_count = len(p.get("observed_callers", []))
        # Zero-caller status is classification-aware: optional/library seams are not tests.
        status = (
            "✅ BOUND" if caller_count > 0 else
            "🔹 CORE" if p["classification"] == "LIVE_VIA_CORE" else
            "🧩 OPTIONAL" if p["classification"] == "OPTIONAL_HOST" else
            "📚 LIBRARY" if p["classification"] == "PURE_LIBRARY" else
            "⏳ DEFERRED" if p["classification"] == "DEFERRED" else
            "🧪 TEST"
        )
        reason = p.get("activation_condition") if p["classification"] == "DEFERRED" else p["reason"]
        md_lines.append(f"| {seam_name} | {p['owner']} | `{p['classification']}` | {caller_count} | {status} | {reason} |")

    md_lines.append("")
    generated_md_text = "\n".join(md_lines)

    if check_mode:
        mismatches = []
        if not JSON_OUTPUT_PATH.exists() or JSON_OUTPUT_PATH.read_text(encoding="utf-8") != generated_json_text:
            mismatches.append(str(JSON_OUTPUT_PATH))
        if not MD_OUTPUT_PATH.exists() or MD_OUTPUT_PATH.read_text(encoding="utf-8") != generated_md_text:
            mismatches.append(str(MD_OUTPUT_PATH))

        if mismatches:
            print(f"[FAIL] Port contract artifacts out of date: {', '.join(mismatches)}", file=sys.stderr)
            print("Run 'python3 scripts/ci/generate-port-contract.py' to regenerate.", file=sys.stderr)
            return 1

        print(f"[PASS] Port contract verification gate: all {len(ports)} seams conform to policy (0 errors, {counts['HOST_REQUIRED']} host-required bound, {counts['DEFERRED']} deferred ratchet)")
        return 0

    JSON_OUTPUT_PATH.write_text(generated_json_text, encoding="utf-8")
    MD_OUTPUT_PATH.write_text(generated_md_text, encoding="utf-8")
    print(f"[OK] Generated {MD_OUTPUT_PATH} and {JSON_OUTPUT_PATH} ({len(ports)} seams).")
    return 0


if __name__ == "__main__":
    check = "--check" in sys.argv
    sys.exit(validate_and_generate(check_mode=check))
