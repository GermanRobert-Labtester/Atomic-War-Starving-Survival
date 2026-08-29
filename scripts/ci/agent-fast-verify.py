#!/usr/bin/env python3
"""
agent-fast-verify.py — Domain-Aware Fast Verification Runner for AI Agents & Developers

Enables fast, targeted pre-flight verification on specific subsystems (2–5 seconds)
with machine-readable JSON output and built-in Rule 7 timeout discipline.

Usage:
  python3 scripts/ci/agent-fast-verify.py --domain persistence
  python3 scripts/ci/agent-fast-verify.py --domain data
  python3 scripts/ci/agent-fast-verify.py --domain ui
  python3 scripts/ci/agent-fast-verify.py --domain core
  python3 scripts/ci/agent-fast-verify.py --domain docs
  python3 scripts/ci/agent-fast-verify.py --domain fast
  python3 scripts/ci/agent-fast-verify.py --domain all
  python3 scripts/ci/agent-fast-verify.py --domain persistence --json
"""

import sys
import time
import json
import pathlib
import subprocess
import argparse

REPO_ROOT = pathlib.Path(__file__).resolve().parent.parent.parent

# High-level domain targets mapped to concrete verification steps
DOMAIN_GATES = {
    "persistence": [
        {"name": "SaveStoreChecksumSelfTest", "cmd": ["godot", "--headless", "--path", ".", "--", "--save-store-checksum-selftest"]},
        {"name": "SaveLoadUiFailureSelfTest", "cmd": ["godot", "--headless", "--path", ".", "--", "--save-load-ui-failure-selftest"]},
        {"name": "SaveStoreCoverageTests", "cmd": ["dotnet", "test", "Ashfall.Core.Tests", "--filter", "FullyQualifiedName~SaveStore"]},
    ],
    "data": [
        {"name": "DataIntegritySelfTest", "cmd": ["godot", "--headless", "--path", ".", "--", "--data-integrity-selftest"]},
        {"name": "ContentUtilizationSelfTest", "cmd": ["godot", "--headless", "--path", ".", "--", "--content-utilization-selftest"]},
        {"name": "CatalogRegistryDrift", "cmd": ["python3", "scripts/ci/generate-catalog-registry.py", "--check"]},
    ],
    "ui": [
        {"name": "SceneBindingSelfTest", "cmd": ["godot", "--headless", "--path", ".", "--", "--scene-binding-selftest"]},
        {"name": "SceneLint", "cmd": ["python3", "scripts/ci/scene-lint.py"]},
        {"name": "UiAccessibilitySelfTest", "cmd": ["godot", "--headless", "--path", ".", "--", "--ui-accessibility-selftest"]},
        {"name": "UiPanelCatalogDrift", "cmd": ["python3", "scripts/ci/generate-ui-panel-catalog.py", "--check"]},
        {"name": "UiPanelContractTests", "cmd": ["dotnet", "test", "Ashfall.Core.Tests", "--filter", "UiPanelContractTests"]},
    ],
    "expansion": [
        {"name": "ExpansionsCompletenessSelfTest", "cmd": ["godot", "--headless", "--path", ".", "--", "--expansions-selftest"]},
        {"name": "DutyRosterSelfTest", "cmd": ["godot", "--headless", "--path", ".", "--", "--duty-roster-selftest"]},
        {"name": "VerdictSelfTest", "cmd": ["godot", "--headless", "--path", ".", "--", "--verdict-selftest"]},
        {"name": "BlackFlotillaSelfTest", "cmd": ["godot", "--headless", "--path", ".", "--", "--black-flotilla-selftest"]},
        {"name": "SilentFoundrySelfTest", "cmd": ["godot", "--headless", "--path", ".", "--", "--silent-foundry-selftest"]},
        {"name": "ExpansionsCatalogDrift", "cmd": ["python3", "scripts/ci/generate-expansions-catalog.py", "--check"]},
    ],
    "smoke": [
        {"name": "SevenDaySmokeSelfTest", "cmd": ["godot", "--headless", "--path", ".", "--", "--7-day-smoke-selftest"]},
        {"name": "PlayableShellSelfTest", "cmd": ["godot", "--headless", "--path", ".", "--", "--playable-shell-selftest"]},
        {"name": "Day1OnboardingSelfTest", "cmd": ["godot", "--headless", "--path", ".", "--", "--day1-selftest"]},
    ],
    "core": [
        {"name": "DotnetBuild", "cmd": ["dotnet", "build", "Ashfall.csproj"]},
        {"name": "CoreTestsSuite", "cmd": ["dotnet", "test", "Ashfall.Core.Tests"]},
        {"name": "CoreSystemsCatalogDrift", "cmd": ["python3", "scripts/ci/generate-core-systems-catalog.py", "--check"]},
    ],
    "docs": [
        {"name": "DocsIndexDrift", "cmd": ["python3", "scripts/ci/generate-docs-index.py", "--check"]},
        {"name": "DocLinkPortability", "cmd": ["python3", "scripts/ci/normalize-doc-links.py", "--check"]},
        {"name": "AgentRuleIntegrity", "cmd": ["dotnet", "test", "Ashfall.Core.Tests", "--filter", "AgentRuleIntegrityTests"]},
        {"name": "AgentRulebooksSync", "cmd": ["python3", "scripts/ci/sync-agent-rulebooks.py", "--check"]},
    ],
    "fast": [
        {"name": "SceneLint", "cmd": ["python3", "scripts/ci/scene-lint.py"]},
        {"name": "DocsIndexDrift", "cmd": ["python3", "scripts/ci/generate-docs-index.py", "--check"]},
        {"name": "DocLinkPortability", "cmd": ["python3", "scripts/ci/normalize-doc-links.py", "--check"]},
        {"name": "AgentRulebooksSync", "cmd": ["python3", "scripts/ci/sync-agent-rulebooks.py", "--check"]},
        {"name": "CoreSystemsCatalogDrift", "cmd": ["python3", "scripts/ci/generate-core-systems-catalog.py", "--check"]},
        {"name": "CatalogRegistryDrift", "cmd": ["python3", "scripts/ci/generate-catalog-registry.py", "--check"]},
        {"name": "UiPanelCatalogDrift", "cmd": ["python3", "scripts/ci/generate-ui-panel-catalog.py", "--check"]},
        {"name": "ExpansionsCatalogDrift", "cmd": ["python3", "scripts/ci/generate-expansions-catalog.py", "--check"]},
    ]
}

def run_gate(gate_info, timeout_sec=180):
    name = gate_info["name"]
    cmd = gate_info["cmd"]
    start = time.time()
    try:
        res = subprocess.run(cmd, cwd=REPO_ROOT, capture_output=True, text=True, timeout=timeout_sec)
        elapsed = time.time() - start
        return {
            "name": name,
            "cmd": " ".join(cmd),
            "exit_code": res.returncode,
            "status": "PASS" if res.returncode == 0 else "FAIL",
            "duration_sec": round(elapsed, 2),
            "stdout": res.stdout.strip(),
            "stderr": res.stderr.strip()
        }
    except subprocess.TimeoutExpired:
        elapsed = time.time() - start
        return {
            "name": name,
            "cmd": " ".join(cmd),
            "exit_code": 124,
            "status": "TIMEOUT",
            "duration_sec": round(elapsed, 2),
            "stdout": "",
            "stderr": f"Command timed out after {timeout_sec}s (Rule 7 escalation required)."
        }
    except Exception as e:
        elapsed = time.time() - start
        return {
            "name": name,
            "cmd": " ".join(cmd),
            "exit_code": 1,
            "status": "ERROR",
            "duration_sec": round(elapsed, 2),
            "stdout": "",
            "stderr": str(e)
        }

def write_failure_artifact(artifact_path, failed_results, total_count, domain):
    p = pathlib.Path(artifact_path)
    p.parent.mkdir(parents=True, exist_ok=True)
    report = {
        "domain": domain,
        "total_gates": total_count,
        "failed_count": len(failed_results),
        "failures": failed_results
    }
    p.write_text(json.dumps(report, indent=2), encoding="utf-8")

def main():
    parser = argparse.ArgumentParser(description="Domain-Aware Fast Verification Runner")
    parser.add_argument("--domain", choices=["persistence", "data", "ui", "expansion", "smoke", "core", "docs", "fast", "all"], default="fast", help="Target domain")
    parser.add_argument("--json", action="store_true", help="Output results in JSON format")
    parser.add_argument("--timeout", type=int, default=180, help="Default command timeout in seconds (default 180s)")
    parser.add_argument("--fail-artifact", type=str, default="", help="Path to write failure artifact JSON if any gate fails")
    args = parser.parse_args()

    targets = []
    if args.domain == "all":
        seen = set()
        for dom, gates in DOMAIN_GATES.items():
            if dom == "fast": continue
            for g in gates:
                if g["name"] not in seen:
                    targets.append(g)
                    seen.add(g["name"])
    else:
        targets = DOMAIN_GATES.get(args.domain, [])

    if not args.json:
        print(f"=== ASHFALL Agent Fast-Verify [{args.domain.upper()}] ===")
        print(f"Running {len(targets)} verification gates (timeout: {args.timeout}s)...\n")

    results = []
    overall_pass = True

    for gate in targets:
        if not args.json:
            print(f"▶ [{gate['name']}] ... ", end="", flush=True)
        res = run_gate(gate, timeout_sec=args.timeout)
        results.append(res)
        if res["status"] != "PASS":
            overall_pass = False
            if not args.json:
                print(f"FAILED ({res['duration_sec']}s)")
                if res['stderr']:
                    print(f"  Error: {res['stderr']}")
        else:
            if not args.json:
                print(f"PASS ({res['duration_sec']}s)")

    if not overall_pass and args.fail_artifact:
        failed = [r for r in results if r["status"] != "PASS"]
        write_failure_artifact(args.fail_artifact, failed, len(results), args.domain)

    if args.json:
        output = {
            "domain": args.domain,
            "total_gates": len(results),
            "passed": sum(1 for r in results if r["status"] == "PASS"),
            "failed": sum(1 for r in results if r["status"] != "PASS"),
            "overall_status": "PASS" if overall_pass else "FAIL",
            "results": results
        }
        print(json.dumps(output, indent=2))
    else:
        passed_cnt = sum(1 for r in results if r["status"] == "PASS")
        print(f"\nSummary: {passed_cnt}/{len(results)} gates passed. Overall: {'PASS' if overall_pass else 'FAIL'}")

    sys.exit(0 if overall_pass else 1)

if __name__ == "__main__":
    main()
