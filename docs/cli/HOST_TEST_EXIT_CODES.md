# ASHFALL — Host Self-Test Exit Codes & Output Protocol

**Date:** 2026-08-27
**Scope:** Defines the standard exit codes, output lines, and machine-readable protocols emitted by all headless Godot host self-tests (`--*-selftest`, `--*-uitest`, `--*-demo`).

---

## 1. Exit Code Conventions

All headless Godot host actions, verification gates, and diagnostic self-tests follow standard POSIX exit status semantics:

| Exit Code | Status | Meaning |
|---|---|---|
| `0` | 🟢 **PASS / Success** | All test assertions, save round-trips, schema validations, and catalog checks passed completely with zero errors. |
| `1` (or >0) | 🔴 **FAIL / Error** | One or more verification gates failed, assertion violated, data integrity mismatch, or unhandled exception encountered. |

---

## 2. Standardized Summary Line Protocol

Every host test verb executed via `godot --headless --path . -- --<action>` emits standardized summary lines formatted by [`HostTestSummary`](../../Assets/Ashfall.Core/HostTestSummary.cs):

### A. Banner Line (`[HOST_SELFTEST]`)

High-level summary token parsed by lightweight log scanners and test aggregators.

```text
[HOST_SELFTEST] <normalized_test_id> PASS
[HOST_SELFTEST] <normalized_test_id> FAIL
```

**Examples:**
```text
[HOST_SELFTEST] data_integrity_selftest PASS
[HOST_SELFTEST] save_load_ui_failure_selftest PASS
[HOST_SELFTEST] ui_accessibility_selftest PASS
```

---

### B. Key-Value Summary Line (`[HOST_SELFTEST_SUMMARY]`)

Structured key-value line detailing test counts, status, exit code, and failure descriptions.

```text
[HOST_SELFTEST_SUMMARY] test=<test_id> status=<PASS|FAIL> exit_code=<0|1> passed=<N> failed=<M> total=<K> details="<message>"
```

**Examples:**
```text
[HOST_SELFTEST_SUMMARY] test=ui_accessibility_selftest status=PASS exit_code=0 passed=5 failed=0 total=5 details="ALL 5 ACCESSIBILITY GATES GREEN"
[HOST_SELFTEST_SUMMARY] test=data_integrity_selftest status=PASS exit_code=0 passed=129 failed=0 total=129 details="All 129 catalogs validated with 0 errors"
[HOST_SELFTEST_SUMMARY] test=save_stores_failure_selftest status=FAIL exit_code=1 passed=11 failed=1 total=12 details="Corrupted header was not rejected"
```

---

### C. JSON Summary Line (`[HOST_SELFTEST_JSON]`)

Full JSON payload suitable for automated CI aggregation, telemetry dashboards, and test reporting.

```text
[HOST_SELFTEST_JSON] {"test":"<test_id>","status":"<PASS|FAIL>","exit_code":<int>,"passed":<int>,"failed":<int>,"total":<int>,"details":"<string>"}
```

**Examples:**
```json
[HOST_SELFTEST_JSON] {"test":"ui_accessibility_selftest","status":"PASS","exit_code":0,"passed":5,"failed":0,"total":5,"details":"ALL 5 ACCESSIBILITY GATES GREEN"}
[HOST_SELFTEST_JSON] {"test":"inventory_save_selftest","status":"PASS","exit_code":0,"passed":4,"failed":0,"total":4,"details":"Save, reload, modify, and checksum verified"}
```

---

### D. Standard & Legacy Tokens

For backwards compatibility with historical test scanners:

```text
SELFTEST PASS: <test_id>
<TEST_ID_UPPER> PASS
```

**Examples:**
```text
SELFTEST PASS: data_integrity_selftest
UI_ACCESSIBILITY_SELFTEST PASS
```

---

## 3. Invocation Examples & CI Verification

To run an individual host self-test and inspect its exit code:

```bash
# Execute headless self-test:
godot --headless --path . -- --ui-accessibility-selftest

# Check exit code:
echo $?
# 0 = PASS, 1 = FAIL
```

To run all fast-tier tests under automated gate enforcement:

```bash
bash scripts/ci/verify-fast.sh
```
