//! Port of `tools/gotools/pkg/parser/parser.go`.
//!
//! Test-result parser used by the `parse-results [-exit-code N] [--json]`
//! subcommand. Reproduces the dotnet/xunit and pytest summary regexes, the
//! `[FAIL]` / `FAILED` failure-line extraction, the taxonomy ladder
//! (timeout -> build-break -> assert-fail -> unknown-failure -> pass), the
//! markdown report text, and the `--json` field order
//! (`runner`, `taxonomy`, `passed`, `failed`, `skipped`, `total`,
//! `duration_str`, `failures`, `raw_summary`) with `failures` omitted when empty.

use crate::jsonout;
use regex::Regex;
use serde::Serialize;
use std::sync::OnceLock;

pub const TAXONOMY_PASS: &str = "pass";
pub const TAXONOMY_ASSERT_FAIL: &str = "assert-fail";
pub const TAXONOMY_BUILD_BREAK: &str = "build-break";
pub const TAXONOMY_TIMEOUT: &str = "timeout";
#[allow(dead_code)] // Declared for parity with the Go taxonomy; never produced.
pub const TAXONOMY_QUARANTINED: &str = "quarantined";
pub const TAXONOMY_UNKNOWN: &str = "unknown-failure";

#[derive(Debug, Clone, Serialize, PartialEq, Eq)]
pub struct TestCaseFailure {
    pub name: String,
    pub message: String,
    #[serde(skip_serializing_if = "String::is_empty")]
    pub details: String,
}

#[derive(Debug, Clone, Serialize)]
pub struct ParsedResult {
    pub runner: String,
    pub taxonomy: String,
    pub passed: i64,
    pub failed: i64,
    pub skipped: i64,
    pub total: i64,
    pub duration_str: String,
    #[serde(skip_serializing_if = "Vec::is_empty")]
    pub failures: Vec<TestCaseFailure>,
    pub raw_summary: String,
}

impl Default for ParsedResult {
    fn default() -> Self {
        Self {
            runner: "generic".to_string(),
            taxonomy: TAXONOMY_PASS.to_string(),
            passed: 0,
            failed: 0,
            skipped: 0,
            total: 0,
            duration_str: String::new(),
            failures: Vec::new(),
            raw_summary: String::new(),
        }
    }
}

fn dotnet_summary_re() -> &'static Regex {
    static RE: OnceLock<Regex> = OnceLock::new();
    RE.get_or_init(|| {
        Regex::new(
            r"(?:Passed!|Failed!)\s+-\s+Failed:\s+(\d+),\s+Passed:\s+(\d+),\s+Skipped:\s+(\d+),\s+Total:\s+(\d+)(?:,\s+Duration:\s+([^-\n]+))?",
        )
        .expect("valid dotnet summary regex")
    })
}

fn dotnet_failed_re() -> &'static Regex {
    static RE: OnceLock<Regex> = OnceLock::new();
    RE.get_or_init(|| Regex::new(r"\[FAIL\]\s+(.+)").expect("valid [FAIL] regex"))
}

fn pytest_summary_re() -> &'static Regex {
    static RE: OnceLock<Regex> = OnceLock::new();
    RE.get_or_init(|| {
        Regex::new(r"=+\s+(?:(\d+)\s+passed)?(?:,\s*)?(?:(\d+)\s+failed)?(?:,\s*)?(?:(\d+)\s+skipped)?.*in\s+([0-9.]+s)\s+=+")
            .expect("valid pytest summary regex")
    })
}

fn pytest_failed_re() -> &'static Regex {
    static RE: OnceLock<Regex> = OnceLock::new();
    RE.get_or_init(|| Regex::new(r"FAILED\s+([^ -]+)").expect("valid FAILED regex"))
}

fn atoi(s: &str) -> i64 {
    s.parse::<i64>().unwrap_or(0)
}

/// Port of `ParseTestOutput`.
pub fn parse_test_output(output: &str, exit_code: i32) -> ParsedResult {
    let mut res = ParsedResult::default();

    if exit_code == 124 {
        res.taxonomy = TAXONOMY_TIMEOUT.to_string();
        res.raw_summary = "Execution timed out (exit code 124)".to_string();
        return res;
    }

    if output.contains("error CS") || output.contains("MSBUILD : error") {
        res.taxonomy = TAXONOMY_BUILD_BREAK.to_string();
        res.raw_summary = "Build compilation broken".to_string();
        return res;
    }

    if let Some(m) = dotnet_summary_re().captures(output) {
        res.runner = "dotnet/xunit".to_string();
        res.failed = atoi(&m[1]);
        res.passed = atoi(&m[2]);
        res.skipped = atoi(&m[3]);
        res.total = atoi(&m[4]);
        if let Some(d) = m.get(5) {
            res.duration_str = d.as_str().trim().to_string();
        }
        res.raw_summary = m.get(0).map(|m0| m0.as_str()).unwrap_or("").to_string();
    } else if let Some(m) = pytest_summary_re().captures(output) {
        res.runner = "pytest".to_string();
        if let Some(g) = m.get(1) {
            res.passed = atoi(g.as_str());
        }
        if let Some(g) = m.get(2) {
            res.failed = atoi(g.as_str());
        }
        if let Some(g) = m.get(3) {
            res.skipped = atoi(g.as_str());
        }
        res.total = res.passed + res.failed + res.skipped;
        if let Some(d) = m.get(4) {
            res.duration_str = d.as_str().to_string();
        }
        res.raw_summary = m.get(0).map(|m0| m0.as_str()).unwrap_or("").to_string();
    }

    for line in output.lines() {
        if let Some(m) = dotnet_failed_re().captures(line) {
            res.failures.push(TestCaseFailure {
                name: m[1].trim().to_string(),
                message: line.to_string(),
                details: String::new(),
            });
        } else if let Some(m) = pytest_failed_re().captures(line) {
            res.failures.push(TestCaseFailure {
                name: m[1].trim().to_string(),
                message: line.to_string(),
                details: String::new(),
            });
        }
    }

    if res.failed > 0 || exit_code != 0 {
        if res.failed > 0 {
            res.taxonomy = TAXONOMY_ASSERT_FAIL.to_string();
        } else if exit_code != 0 && res.taxonomy == TAXONOMY_PASS {
            res.taxonomy = TAXONOMY_UNKNOWN.to_string();
        }
    }

    res
}

impl ParsedResult {
    /// Port of `MarkdownReport`. Note the trailing newline: the CLI prints this
    /// with `println!`, exactly as Go's `fmt.Println(parsed.MarkdownReport())`.
    pub fn markdown_report(&self) -> String {
        let mut out = String::new();
        out.push_str("### Test Execution Result\n\n");
        out.push_str(&format!("- **Runner:** `{}`\n", self.runner));
        out.push_str(&format!("- **Status / Taxonomy:** `{}`\n", self.taxonomy));
        out.push_str(&format!(
            "- **Summary:** Passed: {}, Failed: {}, Skipped: {}, Total: {} (Duration: {})\n",
            self.passed, self.failed, self.skipped, self.total, self.duration_str
        ));
        if !self.failures.is_empty() {
            out.push_str("\n#### Failures:\n");
            for f in &self.failures {
                out.push_str(&format!("- ❌ `{}`: {}\n", f.name, f.message));
            }
        }
        out
    }

    /// Port of `ToJSON`: `json.MarshalIndent(r, "", "  ")`.
    pub fn to_json(&self) -> String {
        jsonout::to_pretty(self)
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn parses_dotnet_summary() {
        let out = "
  Ashfall.Core -> /bin/Ashfall.Core.dll
  Ashfall.Core.Tests -> /bin/Ashfall.Core.Tests.dll
Test run for Ashfall.Core.Tests.dll (.NETCoreApp,Version=v9.0)
Passed!  - Failed:     0, Passed:    32, Skipped:     0, Total:    32, Duration: 49 ms - Ashfall.Core.Tests.dll (net9.0)
";
        let res = parse_test_output(out, 0);
        assert_eq!(res.runner, "dotnet/xunit");
        assert_eq!(res.passed, 32);
        assert_eq!(res.failed, 0);
        assert_eq!(res.skipped, 0);
        assert_eq!(res.total, 32);
        assert_eq!(res.duration_str, "49 ms");
        assert_eq!(res.taxonomy, TAXONOMY_PASS);
        assert!(res.failures.is_empty());
    }

    #[test]
    fn parses_pytest_summary() {
        let out = "
tests/test_audio_pipeline.py::TestAudioPipeline::test_presets_ceiling PASSED [ 80%]
tests/test_audio_pipeline.py::TestAudioPipeline::test_reproducibility PASSED [100%]
============================== 5 passed in 0.58s ===============================
";
        let res = parse_test_output(out, 0);
        assert_eq!(res.runner, "pytest");
        assert_eq!(res.passed, 5);
        assert_eq!(res.failed, 0);
        assert_eq!(res.total, 5);
        assert_eq!(res.duration_str, "0.58s");
    }

    #[test]
    fn pytest_mixed_counts_and_failure_lines() {
        let out = "FAILED tests/test_x.py::test_y - AssertionError: nope\n==== 3 passed, 1 failed, 2 skipped in 1.20s ====\n";
        let res = parse_test_output(out, 1);
        assert_eq!(res.runner, "pytest");
        assert_eq!((res.passed, res.failed, res.skipped, res.total), (3, 1, 2, 6));
        assert_eq!(res.taxonomy, TAXONOMY_ASSERT_FAIL);
        assert_eq!(res.failures.len(), 1);
        assert_eq!(res.failures[0].name, "tests/test_x.py::test_y");
        assert_eq!(res.failures[0].message, "FAILED tests/test_x.py::test_y - AssertionError: nope");
    }

    #[test]
    fn dotnet_fail_line_is_collected() {
        let out = "[FAIL] Ashfall.Core.Tests.Needs.NeedsSystemTests.Decay\nFailed!  - Failed:     1, Passed:    31, Skipped:     0, Total:    32, Duration: 51 ms\n";
        let res = parse_test_output(out, 1);
        assert_eq!(res.runner, "dotnet/xunit");
        assert_eq!(res.failed, 1);
        assert_eq!(res.taxonomy, TAXONOMY_ASSERT_FAIL);
        assert_eq!(
            res.failures[0].name,
            "Ashfall.Core.Tests.Needs.NeedsSystemTests.Decay"
        );
    }

    #[test]
    fn exit_code_124_is_timeout() {
        let res = parse_test_output("whatever", 124);
        assert_eq!(res.taxonomy, TAXONOMY_TIMEOUT);
        assert_eq!(res.raw_summary, "Execution timed out (exit code 124)");
        assert_eq!(res.runner, "generic");
        assert_eq!(res.total, 0);
    }

    #[test]
    fn compile_errors_are_build_breaks() {
        let res = parse_test_output("Foo.cs(3,4): error CS0103: bad\n", 1);
        assert_eq!(res.taxonomy, TAXONOMY_BUILD_BREAK);
        assert_eq!(res.raw_summary, "Build compilation broken");

        let res = parse_test_output("\nMSBUILD : error MSB1009: nope\n", 1);
        assert_eq!(res.taxonomy, TAXONOMY_BUILD_BREAK);
    }

    #[test]
    fn nonzero_exit_without_counts_is_unknown_failure() {
        let res = parse_test_output("no recognizable summary", 1);
        assert_eq!(res.taxonomy, TAXONOMY_UNKNOWN);
        assert_eq!(res.runner, "generic");
        assert_eq!(res.raw_summary, "");
    }

    #[test]
    fn markdown_report_matches_go_layout() {
        let res = parse_test_output("Passed!  - Failed:     0, Passed:     2, Skipped:     0, Total:     2, Duration: 5 ms\n", 0);
        assert_eq!(
            res.markdown_report(),
            "### Test Execution Result\n\n- **Runner:** `dotnet/xunit`\n- **Status / Taxonomy:** `pass`\n- **Summary:** Passed: 2, Failed: 0, Skipped: 0, Total: 2 (Duration: 5 ms)\n"
        );
    }

    #[test]
    fn json_shape_and_key_order_without_failures() {
        let res = parse_test_output("Passed!  - Failed:     0, Passed:     2, Skipped:     0, Total:     2, Duration: 5 ms\n", 0);
        let json = res.to_json();
        assert_eq!(
            json,
            "{\n  \"runner\": \"dotnet/xunit\",\n  \"taxonomy\": \"pass\",\n  \"passed\": 2,\n  \"failed\": 0,\n  \"skipped\": 0,\n  \"total\": 2,\n  \"duration_str\": \"5 ms\",\n  \"raw_summary\": \"Passed!  - Failed:     0, Passed:     2, Skipped:     0, Total:     2, Duration: 5 ms\"\n}"
        );
        assert!(!json.contains("failures"));
    }

    #[test]
    fn json_html_escapes_like_go() {
        let res = parse_test_output("[FAIL] t <A> & b\nFailed!  - Failed:     1, Passed:     0, Skipped:     0, Total:     1, Duration: 1 ms\n", 1);
        let json = res.to_json();
        assert!(json.contains(r"\u003cA\u003e"), "got {json}");
        assert!(json.contains(r"\u0026"), "got {json}");
        assert!(!json.contains("<A>"));
    }
}