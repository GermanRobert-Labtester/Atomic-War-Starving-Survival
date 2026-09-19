#!/usr/bin/env bash
# scripts/release/hotfix.sh — Plan 48 / C2[21] Phase 5
# ======================================================
# Hotfix branch mechanics for ASHFALL.
# Enforces the iron rule: hotfixes must never alter save-schema constants.
#
# Usage:
#   bash scripts/release/hotfix.sh --version 1.1.1 --base-tag v1.1.0 [--classify] [--check]
#
# Flags:
#   --version    The hotfix version (e.g. 1.1.1)
#   --base-tag   The release tag this hotfix is based on (e.g. v1.1.0)
#   --classify   Run classification check only (no version writes)
#   --check      Validate current branch is eligible for hotfix (alias for --classify)
#
# Exit codes:
#   0  Eligible / PASS
#   1  Not eligible (iron rule violation or other issue)

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "${SCRIPT_DIR}/../.." && pwd)"

VERSION=""
BASE_TAG=""
CLASSIFY=false
CHECK=false

while [[ $# -gt 0 ]]; do
    case "$1" in
        --version)  VERSION="$2"; shift 2 ;;
        --base-tag) BASE_TAG="$2"; shift 2 ;;
        --classify) CLASSIFY=true; shift ;;
        --check)    CHECK=true; shift ;;
        *) echo "Unknown argument: $1"; exit 1 ;;
    esac
done

cd "$REPO_ROOT"

log()  { echo "[hotfix] $*"; }
fail() { echo "[hotfix] FAIL: $*" >&2; exit 1; }

if [[ -z "$BASE_TAG" ]]; then
    fail "--base-tag is required (e.g. --base-tag v1.1.0)"
fi

# ---------------------------------------------------------------------------
# Iron-rule check: no save-schema constant changes
# ---------------------------------------------------------------------------
log "Running iron-rule check: version-gate.py --hotfix --base-ref $BASE_TAG"
if ! python3 scripts/ci/version-gate.py --hotfix --base-ref "$BASE_TAG"; then
    echo ""
    echo "HOTFIX_GATE FAIL: save-schema constants changed relative to $BASE_TAG"
    echo "A hotfix MUST NOT alter any codec SchemaVersion constant."
    echo "Reclassify this as a minor or major release instead."
    exit 1
fi
log "Iron rule: PASS — no schema constant changes detected"

# ---------------------------------------------------------------------------
# Branch name check
# ---------------------------------------------------------------------------
BRANCH=$(git rev-parse --abbrev-ref HEAD 2>/dev/null || echo "unknown")
if [[ "$BRANCH" != hotfix/* ]]; then
    echo "[hotfix] WARNING: current branch '$BRANCH' does not match 'hotfix/*' pattern."
    echo "  Recommend: git checkout -b hotfix/v${VERSION:-X.Y.Z} $BASE_TAG"
fi

# ---------------------------------------------------------------------------
# Classification check mode — stops here
# ---------------------------------------------------------------------------
if $CLASSIFY || $CHECK; then
    echo ""
    echo "HOTFIX_CLASSIFY PASS"
    echo "  Base tag   : $BASE_TAG"
    echo "  Branch     : $BRANCH"
    echo "  Iron rule  : No schema constant changes detected"
    echo "  Eligible   : YES — this branch qualifies as a hotfix"
    if [[ -n "$VERSION" ]]; then
        echo "  Target ver : $VERSION"
        if ! echo "$VERSION" | grep -qE '^[0-9]+\.[0-9]+\.[0-9]+$'; then
            echo "  WARNING: '$VERSION' is not strict semver X.Y.Z"
        fi
    fi
    exit 0
fi

# ---------------------------------------------------------------------------
# Validate version argument for full mode
# ---------------------------------------------------------------------------
if [[ -z "$VERSION" ]]; then
    fail "--version is required in full mode (e.g. --version 1.1.1)"
fi
if ! echo "$VERSION" | grep -qE '^[0-9]+\.[0-9]+\.[0-9]+$'; then
    fail "Version '$VERSION' is not strict semver X.Y.Z"
fi

# ---------------------------------------------------------------------------
# Verify clean tree
# ---------------------------------------------------------------------------
if [[ -n "$(git status --porcelain)" ]]; then
    fail "Working tree is not clean. Commit fix before running hotfix.sh."
fi

# ---------------------------------------------------------------------------
# Full release gate (fast + release-specific)
# ---------------------------------------------------------------------------
log "Running release-gate.sh --skip-full (fast tier only for hotfix)..."
if ! bash scripts/ci/release-gate.sh --skip-full; then
    fail "Release gate failed. Fix the issues before tagging the hotfix."
fi

echo ""
echo "============================================================"
echo "  HOTFIX_GATE PASS — v${VERSION} is safe to tag"
echo "============================================================"
echo ""
echo "Tag and push:"
echo "  git tag -a v${VERSION} -m \"ASHFALL v${VERSION} (hotfix)\""
echo "  git push origin ${BRANCH} v${VERSION}"
echo ""
echo "Then merge back to main:"
echo "  git checkout main"
echo "  git merge --no-ff ${BRANCH} -m \"merge: hotfix v${VERSION} back to main\""
echo "  git push origin main"
echo ""
echo "Then run the historical corpus ceremony:"
echo "  bash scripts/release/prepare-release.sh --version ${VERSION} --base ${BASE_TAG}"
