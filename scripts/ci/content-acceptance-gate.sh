#!/usr/bin/env bash
# SPDX-License-Identifier: MIT
# Plan 45 / C1[14]: The Content Acceptance Pipeline Gate
# Evaluates content rungs in strict dependency order with fail-fast execution:
# 1. Integrity (JSON schema, ID prefixes, cross-catalog references)
# 2. Utilization (Catalog loaders, consumer systems, acceptance ladder)
# 3. Canon (Catalog registry drift & definition accounting)
# 4. Quality (Prose voice, tone, narrative standards)

set -euo pipefail

REPO_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
cd "${REPO_ROOT}"

echo "=== [Content Acceptance Pipeline] Rung 1: Integrity Gate ==="
if ! dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~CatalogIntegrity" --nologo; then
    echo "ERROR: Content Acceptance Pipeline failed at Rung 1 (Integrity Gate)." >&2
    exit 1
fi

echo "=== [Content Acceptance Pipeline] Rung 2: Utilization Gate ==="
if ! dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~ContentAcceptance" --nologo; then
    echo "ERROR: Content Acceptance Pipeline failed at Rung 2 (Utilization Gate)." >&2
    exit 1
fi

echo "=== [Content Acceptance Pipeline] Rung 3: Canon Gate ==="
if ! python3 scripts/ci/generate-catalog-registry.py --check; then
    echo "ERROR: Content Acceptance Pipeline failed at Rung 3 (Canon Gate)." >&2
    exit 1
fi

echo "=== [Content Acceptance Pipeline] Rung 4: Quality Gate ==="
if ! dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~CollectibleNarrativeQualityTests" --nologo; then
    echo "ERROR: Content Acceptance Pipeline failed at Rung 4 (Quality Gate)." >&2
    exit 1
fi

echo "CONTENT_ACCEPTANCE_PIPELINE PASS"
exit 0
