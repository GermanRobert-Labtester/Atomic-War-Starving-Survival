#!/usr/bin/env bash
# =============================================================================
# coverage-gate.sh — Plan 27B / Wave 10 Save Round-Trip & Determinism Coverage Gate
# =============================================================================
# Enforces:
# 1. 100% of stateful Core systems have registered save round-trip test coverage.
# 2. 100% of registered campaign day owners have determinism replay test coverage.
# 3. Data catalog fidelity against shipped items.json.
# 4. No fresh campaign-owned system instantiation in selftests.
# 5. Golden save envelope & determinism digest validation.
# =============================================================================

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "${SCRIPT_DIR}/../.." && pwd)"
cd "${REPO_ROOT}"

echo "[coverage-gate] Running Save Round-Trip, Determinism & Fidelity Gates..."
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj \
    --filter "FullyQualifiedName~SaveStateRoundTripCoverageGateTests|FullyQualifiedName~CampaignDayOwnerDeterminismGateTests|FullyQualifiedName~DataAuthorityFidelityTests|FullyQualifiedName~NoFreshCampaignSystemGateTests|FullyQualifiedName~GoldenSaveFixtureTests" \
    --nologo

echo "[coverage-gate] Coverage gate passed: all stateful systems, day owners, fixtures, and fidelity gates green."
