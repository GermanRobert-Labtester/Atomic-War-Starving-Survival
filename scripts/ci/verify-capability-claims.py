#!/usr/bin/env python3
"""
scripts/ci/verify-capability-claims.py
Authoritative capability claims verification gate for ASHFALL.

Validates docs/architecture/CLAIMS.json:
- Schema conformance and status taxonomy
- Existence of referenced source paths, test files, and doc files
- Validity of referenced CI gate identifiers against docs/ci/CI_GATE_MANIFEST.json
- Evidence requirement: every TRUE claim must cite valid source/doc and test/gate evidence
"""

import argparse
import json
import os
import sys

ALLOWED_STATUSES = {"TRUE", "PARTLY_TRUE", "STALE", "UNVERIFIABLE", "FALSE", "SUPERSEDED"}
ALLOWED_CONFIDENCE = {"PROVEN_RUNTIME", "PROVEN_INTEGRATION", "PROVEN_UNIT", "SOURCE_ONLY", "UNVERIFIED"}


def load_manifest_gates(repo_root: str) -> set:
    manifest_path = os.path.join(repo_root, "docs", "ci", "CI_GATE_MANIFEST.json")
    if not os.path.isfile(manifest_path):
        return set()
    try:
        with open(manifest_path, "r", encoding="utf-8") as f:
            data = json.load(f)
            return {g["gate_id"] for g in data.get("gates", [])}
    except Exception:
        return set()


def verify_claims(repo_root: str, claims_path: str) -> tuple[bool, str, dict]:
    if not os.path.isfile(claims_path):
        return False, f"Claims file not found: {claims_path}", {}

    try:
        with open(claims_path, "r", encoding="utf-8") as f:
            data = json.load(f)
    except json.JSONDecodeError as err:
        return False, f"JSON syntax error in {claims_path}: {err}", {}

    claims = data.get("claims", [])
    if not isinstance(claims, list) or len(claims) == 0:
        return False, "Claims file contains no claims array or is empty", {}

    manifest_gates = load_manifest_gates(repo_root)

    seen_ids = set()
    errors = []
    status_counts = {s: 0 for s in ALLOWED_STATUSES}
    confidence_counts = {c: 0 for c in ALLOWED_CONFIDENCE}

    for idx, claim in enumerate(claims):
        claim_id = claim.get("claim_id")
        if not claim_id or not isinstance(claim_id, str):
            errors.append(f"Claim #{idx} missing string claim_id")
            continue

        if claim_id in seen_ids:
            errors.append(f"Duplicate claim_id: {claim_id}")
        seen_ids.add(claim_id)

        status = claim.get("status")
        if status not in ALLOWED_STATUSES:
            errors.append(f"Claim '{claim_id}' invalid status: {status}")
        else:
            status_counts[status] += 1

        confidence = claim.get("confidence")
        if confidence not in ALLOWED_CONFIDENCE:
            errors.append(f"Claim '{claim_id}' invalid confidence: {confidence}")
        else:
            confidence_counts[confidence] += 1

        evidence = claim.get("evidence", [])
        if not isinstance(evidence, list):
            errors.append(f"Claim '{claim_id}' evidence must be an array")
            evidence = []

        # Validate evidence file existence
        for ev in evidence:
            ev_path = ev.get("path")
            if ev_path:
                full_ev_path = os.path.join(repo_root, ev_path)
                if not os.path.exists(full_ev_path):
                    errors.append(f"Claim '{claim_id}' evidence path not found on disk: {ev_path}")

        # Validate test file existence
        tests = claim.get("tests", [])
        if not isinstance(tests, list):
            errors.append(f"Claim '{claim_id}' tests must be an array")
            tests = []
        for t in tests:
            full_test_path = os.path.join(repo_root, t)
            if not os.path.exists(full_test_path):
                errors.append(f"Claim '{claim_id}' test path not found on disk: {t}")

        # Validate gate references
        gates = claim.get("gates", [])
        if not isinstance(gates, list):
            errors.append(f"Claim '{claim_id}' gates must be an array")
            gates = []
        for g in gates:
            if manifest_gates and g not in manifest_gates:
                errors.append(f"Claim '{claim_id}' references unknown CI gate: {g}")

        # For TRUE claims, enforce evidence requirements
        if status == "TRUE":
            if not evidence:
                errors.append(f"Claim '{claim_id}' is marked TRUE but has no evidence entries")
            if not tests and not gates:
                errors.append(f"Claim '{claim_id}' is marked TRUE but cites neither tests nor gates")

    metrics = {
        "total_claims": len(claims),
        "status_counts": status_counts,
        "confidence_counts": confidence_counts,
        "errors": errors,
    }

    if errors:
        msg = f"Claims verification FAILED with {len(errors)} error(s):\n" + "\n".join(f"  - {e}" for e in errors)
        return False, msg, metrics

    msg = f"All {len(claims)} capability claims successfully verified."
    return True, msg, metrics


def main():
    parser = argparse.ArgumentParser(description="Verify ASHFALL capability claims registry")
    parser.add_argument("--check", action="store_true", help="Run verification check and exit with status code")
    parser.add_argument("--claims-file", default=None, help="Explicit path to CLAIMS.json")
    args = parser.parse_args()

    repo_root = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", ".."))
    claims_file = args.claims_file or os.path.join(repo_root, "docs", "architecture", "CLAIMS.json")

    passed, message, metrics = verify_claims(repo_root, claims_file)

    print("==================================================")
    print("ASHFALL CAPABILITY CLAIMS VERIFICATION REPORT")
    print("==================================================")
    print(f"Registry: {claims_file}")
    if metrics:
        print(f"Total claims: {metrics.get('total_claims', 0)}")
        print("Status breakdown:")
        for k, v in metrics.get("status_counts", {}).items():
            print(f"  {k:15}: {v}")
        print("Confidence breakdown:")
        for k, v in metrics.get("confidence_counts", {}).items():
            print(f"  {k:20}: {v}")
    print("--------------------------------------------------")
    print(message)
    print("==================================================")

    if not passed and args.check:
        sys.exit(1)


if __name__ == "__main__":
    main()
