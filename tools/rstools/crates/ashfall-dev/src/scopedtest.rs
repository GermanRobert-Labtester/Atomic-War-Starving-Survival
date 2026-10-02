//! Port of `tools/gotools/pkg/scopedtest/scopedtest.go`.
//!
//! Enforces TEST_POLICY.md: never run the full suite unless the user types
//! exactly `RUN FULL TESTS`. Selection delegates to [`crate::selector`].

use crate::selector;
use std::process::Command;
use std::time::{Duration, Instant};

pub const FULL_TEST_PASSPHRASE: &str = "RUN FULL TESTS";

#[derive(Debug, Clone, Default)]
pub struct RunOptions {
    pub repo_root: String,
    pub files: Vec<String>,
    pub compare_ref: String,
    pub dry_run: bool,
    pub run_full_tests: bool,
    pub user_override: String,
}

#[derive(Debug, Clone, Default)]
pub struct RunResult {
    pub total_run: usize,
    pub total_passed: usize,
    pub total_failed: usize,
    pub duration: Duration,
    pub failures: Vec<String>,
    // Part of the ported result contract (Go `RunResult.Advice`); the CLI does
    // not print it, so it is only read by tests/future callers.
    #[allow(dead_code)]
    pub advice: String,
}

fn full_test_violation() -> String {
    // Single logical line so the two-space indent on the numbered steps is
    // preserved (a Rust `\`-continuation would strip leading whitespace and
    // break parity with the Go message).
    format!(
        "VIOLATION: Full test suite requested without explicit authorization.\n- NEVER run the full test suite unless the user types exactly: {:?}.\nIf you feel the need to run the full suite, instead:\n  1. List which additional tests you think are relevant.\n  2. Ask: \"Should I run these extra tests now, or only the scoped ones?\"",
        FULL_TEST_PASSPHRASE
    )
}

pub fn run_scoped_tests(opts: &RunOptions) -> Result<RunResult, String> {
    // 1. Explicit ban check.
    if opts.run_full_tests {
        let env_override = std::env::var("ASHFALL_TEST_OVERRIDE").unwrap_or_default();
        if opts.user_override.trim() != FULL_TEST_PASSPHRASE
            && env_override.trim() != FULL_TEST_PASSPHRASE
        {
            return Err(full_test_violation());
        }
    }

    // 2. Identify target files.
    let files = if opts.files.is_empty() {
        selector::get_changed_files(&opts.repo_root, &opts.compare_ref)
    } else {
        opts.files.clone()
    };

    if files.is_empty() {
        println!("[run-scoped-tests] No changed files detected. No scoped tests to run.");
        return Ok(RunResult::default());
    }

    // 3. Map changed files to targeted tests.
    let plan = selector::select_tests_for_files(&opts.repo_root, &files);

    if plan.selected_tests.is_empty() {
        println!("[run-scoped-tests] {}", plan.explanation);
        return Ok(RunResult {
            advice: plan.explanation,
            ..RunResult::default()
        });
    }

    println!(
        "[run-scoped-tests] Found {} changed files -> Mapped {} targeted test target(s):",
        plan.changed_files.len(),
        plan.selected_tests.len()
    );
    for (i, t) in plan.selected_tests.iter().enumerate() {
        println!("  {}. [{}] {} -> {}", i + 1, t.tier, t.target_file, t.command);
    }

    if opts.dry_run {
        println!("\n[run-scoped-tests] (Dry-run mode: tests not executed)");
        return Ok(RunResult {
            total_run: plan.selected_tests.len(),
            ..RunResult::default()
        });
    }

    // 4. Execute scoped tests.
    let start_time = Instant::now();
    let mut res = RunResult {
        total_run: plan.selected_tests.len(),
        ..RunResult::default()
    };

    println!("\n==================== EXECUTING SCOPED TESTS ====================");

    for (i, t) in plan.selected_tests.iter().enumerate() {
        println!(
            "\n>>> [{}/{}] Running {} ({})...",
            i + 1,
            plan.selected_tests.len(),
            t.target_file,
            t.tier
        );
        let test_start = Instant::now();

        let status = Command::new("bash")
            .arg("-c")
            .arg(&t.command)
            .current_dir(&opts.repo_root)
            .status();

        let test_duration = test_start.elapsed();
        let ok = matches!(status, Ok(s) if s.success());

        if !ok {
            res.total_failed += 1;
            res.failures.push(format!("{} ({})", t.target_file, t.command));
            eprintln!(">>> FAILED [{}] in {:?}", t.target_file, test_duration);
        } else {
            res.total_passed += 1;
            println!(">>> PASSED [{}] in {:?}", t.target_file, test_duration);
        }
    }

    res.duration = start_time.elapsed();
    println!("================================================================");
    println!(
        "Scoped Test Summary: {} ran, {} passed, {} failed in {:?}",
        res.total_run, res.total_passed, res.total_failed, res.duration
    );

    if res.total_failed > 0 {
        eprintln!("\n[ALERT - TEST POLICY ENFORCEMENT]:");
        eprintln!("Maximum 10-15 test-edit steps per failing test.");
        eprintln!(
            "If you cannot resolve failing tests within 10-15 steps, DO NOT repeatedly hit tests with micro-edits!"
        );
        eprintln!("Auto-flag the issue in .ai/state.md for a bug validator to fix instead.");
    }

    Ok(res)
}

#[cfg(test)]
mod tests {
    use super::*;
    use std::fs;
    use std::path::Path;

    #[test]
    fn full_suite_is_refused_without_passphrase() {
        let opts = RunOptions {
            repo_root: ".".into(),
            run_full_tests: true,
            user_override: String::new(),
            ..RunOptions::default()
        };
        let err = run_scoped_tests(&opts).unwrap_err();
        assert!(err.contains("VIOLATION"));
        assert!(err.contains(FULL_TEST_PASSPHRASE));
    }

    #[test]
    fn no_changed_files_is_a_noop() {
        let opts = RunOptions {
            repo_root: ".".into(),
            files: vec!["definitely-absent-file-xyz.md".into()],
            dry_run: true,
            ..RunOptions::default()
        };
        // Explicit file that maps to nothing -> advice path, no execution.
        let res = run_scoped_tests(&opts).unwrap();
        assert_eq!(res.total_run, 0);
    }

    #[test]
    fn dry_run_does_not_execute() {
        let base = std::env::temp_dir().join(format!("ashfall-rstools-scoped-{}", std::process::id()));
        let _ = fs::remove_dir_all(&base);
        let tooling = base.join("Ashfall.Core.Tests/Tooling");
        fs::create_dir_all(&tooling).unwrap();
        fs::write(tooling.join("T.cs"), b"// stub\n").unwrap();

        let opts = RunOptions {
            repo_root: base.to_string_lossy().into_owned(),
            files: vec!["src/UI/FooPanel.cs".into()],
            dry_run: true,
            ..RunOptions::default()
        };
        let res = run_scoped_tests(&opts).unwrap();
        assert_eq!(res.total_run, 1);
        assert_eq!(res.total_passed, 0);
        assert_eq!(res.total_failed, 0);

        assert!(Path::new(&base).exists());
        let _ = fs::remove_dir_all(&base);
    }
}