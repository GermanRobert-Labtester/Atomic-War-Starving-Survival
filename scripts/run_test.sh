#!/usr/bin/env bash
# ASHFALL targeted xUnit runner.
#
# Usage: ./scripts/run_test.sh Ashfall.Core.Tests/SomeTestFile.cs
#        ./scripts/run_test.sh Ashfall.Core.Tests/SomeTestDirectory
#
# The project is xUnit/.NET, not GUT. This wrapper converts a file or focused
# directory into class-name filters, refuses the whole test project, and caps
# each run at the project-wide 180 second limit.
set -euo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
TEST_ROOT="$ROOT/Ashfall.Core.Tests"
TEST_PROJECT="$TEST_ROOT/Ashfall.Core.Tests.csproj"
MAX_SECONDS=180

TARGET="${1:-}"
if [[ -z "$TARGET" ]]; then
    echo "Usage: $0 <path_to_test_file_or_focused_directory>" >&2
    exit 2
fi

excluded_sources=()
while IFS= read -r excluded_source; do
    excluded_sources+=("$excluded_source")
done < <(sed -nE 's/^[[:space:]]*<Compile[[:space:]]+Remove="([^"]+)"[[:space:]]*\/>.*/\1/p' "$TEST_PROJECT")

if [[ "$TARGET" == /* ]]; then
    TARGET_CANDIDATE="$(realpath -m "$TARGET")"
else
    TARGET_CANDIDATE="$(realpath -m "$ROOT/$TARGET")"
fi
if [[ "$TARGET_CANDIDATE" == "$TEST_ROOT"/* ]]; then
    candidate_relative_to_tests="${TARGET_CANDIDATE#$TEST_ROOT/}"
    for excluded_source in "${excluded_sources[@]}"; do
        if [[ "$candidate_relative_to_tests" == "$excluded_source" ]]; then
            printf 'ERROR: requested source is excluded from %s:\n' "${TEST_PROJECT#$ROOT/}" >&2
            printf '  - %s\n' "${TARGET_CANDIDATE#$ROOT/}" >&2
            exit 1
        fi
    done
fi

if [[ -e "$TARGET" ]]; then
    TARGET_PATH="$(realpath "$TARGET")"
elif [[ -e "$ROOT/$TARGET" ]]; then
    TARGET_PATH="$(realpath "$ROOT/$TARGET")"
else
    echo "ERROR: target does not exist: $TARGET" >&2
    exit 2
fi

if [[ "$TARGET_PATH" == "$TEST_ROOT" ]]; then
    echo "ERROR: the complete Ashfall.Core.Tests project is not a targeted test path" >&2
    exit 2
fi

case "$TARGET_PATH" in
    "$TEST_ROOT"/*) ;;
    *)
        echo "ERROR: target must be inside $TEST_ROOT" >&2
        exit 2
        ;;
esac

files=()
if [[ -f "$TARGET_PATH" ]]; then
    [[ "$TARGET_PATH" == *.cs ]] || {
        echo "ERROR: targeted file must be a C# test source file (.cs)" >&2
        exit 2
    }
    files=("$TARGET_PATH")
elif [[ -d "$TARGET_PATH" ]]; then
    while IFS= read -r file; do files+=("$file"); done < <(
        rg --files "$TARGET_PATH" -g '*.cs' | sort
    )
else
    echo "ERROR: target does not exist: $TARGET_PATH" >&2
    exit 2
fi

if [[ "${#files[@]}" -eq 0 ]]; then
    echo "ERROR: no C# files found below $TARGET_PATH" >&2
    exit 2
fi

aggregate_source_rows=0
aggregate_cases=0
aggregate_saved_cases=0
aggregate_markers=0
for requested_file in "${files[@]}"; do
    while IFS= read -r aggregate_marker; do
        if [[ "$aggregate_marker" =~ source_rows=([0-9]+)[[:space:]]+aggregate_cases=([0-9]+)[[:space:]]+saved_cases=([0-9]+) ]]; then
            aggregate_source_rows=$((aggregate_source_rows + BASH_REMATCH[1]))
            aggregate_cases=$((aggregate_cases + BASH_REMATCH[2]))
            aggregate_saved_cases=$((aggregate_saved_cases + BASH_REMATCH[3]))
            aggregate_markers=$((aggregate_markers + 1))
        fi
    done < <(rg --no-filename '^[[:space:]]*// TEST-AGGREGATION:' "$requested_file" || true)
done

if (( aggregate_markers > 0 )); then
    printf '[ashfall-tests] aggregate-row-reduction source_rows=%d aggregate_cases=%d saved_cases=%d markers=%d\n' \
        "$aggregate_source_rows" "$aggregate_cases" "$aggregate_saved_cases" "$aggregate_markers"
else
    printf '[ashfall-tests] aggregate-row-reduction no metadata for requested source file(s)\n'
fi

class_names=()
while IFS= read -r class_name; do
    [[ -n "$class_name" ]] && class_names+=("$class_name")
done < <(
    # Test classes in this workspace use the Test/Tests suffix.  Anchoring the
    # declaration prevents nested helpers such as NodeInfo, Holder, and
    # fixture probes from becoming misleading filter terms.
    rg -o --no-filename \
        '^[[:space:]]*(public|internal|private|protected)?[[:space:]]*(sealed|abstract|partial|static)?[[:space:]]*class[[:space:]]+[A-Za-z_][A-Za-z0-9_]*Tests?\b' \
        "${files[@]}" \
        | sed -E 's/.*class[[:space:]]+([A-Za-z_][A-Za-z0-9_]*Tests?).*/\1/' \
        | sort -u
)

if [[ "${#class_names[@]}" -eq 0 ]]; then
    echo "ERROR: no class declarations found in targeted files" >&2
    exit 2
fi

filter_parts=()
for class_name in "${class_names[@]}"; do
    filter_parts+=("FullyQualifiedName~$class_name")
done
FILTER="$(IFS='|'; echo "${filter_parts[*]}")"

command -v timeout >/dev/null 2>&1 || {
    echo "ERROR: coreutils timeout is required for bounded test runs" >&2
    exit 127
}

echo "[ashfall-tests] target=$TARGET_PATH classes=${#class_names[@]} timeout=${MAX_SECONDS}s" >&2
output_file="$(mktemp "${TMPDIR:-/tmp}/ashfall-test-output.XXXXXX")"
cleanup_output() {
    rm -f "$output_file"
}
trap cleanup_output EXIT

set +e
timeout --foreground --signal=TERM --kill-after=5s "${MAX_SECONDS}s" \
    dotnet test "$TEST_PROJECT" --no-restore --nologo \
    --filter "$FILTER" --logger 'console;verbosity=minimal' 2>&1 \
    | tee "$output_file"
test_status=${PIPESTATUS[0]}
set -e

if [[ "$test_status" -ne 0 ]]; then
    exit "$test_status"
fi

# A project can otherwise return zero without invoking VSTest at all (for
# example, before its assets have been restored).  A successful targeted run
# must emit a test-host execution line.
if ! rg -q 'Test run for ' "$output_file"; then
    echo "ERROR: dotnet test produced no test-host execution report" >&2
    exit 1
fi

# `dotnet test` can return success when a class was removed from the project or
# the filter matched nothing.  Treat that as a failed targeted run so a stale
# or quarantined test can never produce a false green result.
if rg -qi 'No test matches.*testcase filter|Total tests:[[:space:]]*0' "$output_file"; then
    echo "ERROR: targeted filter matched no tests: $FILTER" >&2
    exit 1
fi
