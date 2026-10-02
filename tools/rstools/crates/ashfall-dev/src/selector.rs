//! Port of `tools/gotools/pkg/selector/selector.go`.
//!
//! Behavioural parity contract: the seven mapping sections, candidate
//! ordering, fallback branches, and explanation strings must match the Go
//! original, because CI and `bin/run-scoped-tests` depend on this selection.
//!
//! Known upstream quirk (reproduced deliberately, NOT fixed here): the Go
//! parser does `strings.TrimSpace(line)[3:]` on each `git status --porcelain`
//! line. When the index status byte is a space (unstaged ` M`, ` D`, ...),
//! trimming shifts the slice by one and the returned path loses its first
//! character (e.g. `WORKTREE_OWNERSHIP.md` -> `ORKTREE_OWNERSHIP.md`). Fixing
//! that is a behaviour change and belongs to a separately approved follow-up,
//! not to this port.

use serde::{Deserialize, Serialize};
use std::collections::HashSet;
use std::path::Path;
use std::process::Command;

pub const KIND_XUNIT: &str = "xunit";
pub const KIND_PYTEST: &str = "pytest";
// Defined for API parity with the Go `TargetKind` enum; not produced by the
// current mapping (same as upstream).
#[allow(dead_code)]
pub const KIND_GODOT: &str = "godot";
#[allow(dead_code)]
pub const KIND_NONE: &str = "none";

const TIER_FAST: &str = "Fast (<30s)";
const NO_TARGETS: &str = "No affected test targets mapped for current changed files. Full test suite run suppressed per TEST_POLICY.md rule 5.";

#[derive(Debug, Clone, Serialize, Deserialize, PartialEq, Eq)]
pub struct SelectedTest {
    pub target_file: String,
    pub kind: String,
    pub command: String,
    pub tier: String,
    pub trigger_file: String,
}

#[derive(Debug, Clone, Default, Serialize, Deserialize)]
pub struct SelectionPlan {
    pub changed_files: Vec<String>,
    pub selected_tests: Vec<SelectedTest>,
    pub explanation: String,
}

/// Serialization view with Go's exact key order. Serializing this struct
/// directly (rather than through `serde_json::Value`, which sorts keys) keeps
/// `changed_files`, `selected_tests`, `explanation` in Go's declaration order.
#[derive(Serialize)]
struct PlanJson<'a> {
    changed_files: Option<&'a Vec<String>>,
    selected_tests: Option<&'a Vec<SelectedTest>>,
    explanation: &'a str,
}

impl SelectionPlan {
    /// Byte-for-byte compatible with Go's `json.MarshalIndent(plan, "", "  ")`:
    /// nil slices become `null`, keys stay in declaration order.
    pub fn to_json_pretty(&self) -> String {
        let view = PlanJson {
            changed_files: if self.changed_files.is_empty() {
                None
            } else {
                Some(&self.changed_files)
            },
            selected_tests: if self.selected_tests.is_empty() {
                None
            } else {
                Some(&self.selected_tests)
            },
            explanation: &self.explanation,
        };
        // Route through Go-compatible escaping so a path/command containing
        // `<`, `>`, or `&` still matches `encoding/json` byte-for-byte.
        crate::jsonout::to_pretty(&view)
    }
}

fn selected(target: &str, kind: &str, command: String, trigger: &str) -> SelectedTest {
    SelectedTest {
        target_file: target.to_string(),
        kind: kind.to_string(),
        command,
        tier: TIER_FAST.to_string(),
        trigger_file: trigger.to_string(),
    }
}

/// `filepath.ToSlash` (identity on Linux; normalises Windows separators).
fn to_slash(s: &str) -> String {
    s.replace('\\', "/")
}

fn path_exists(path: &str) -> bool {
    Path::new(path).exists()
}

/// `filepath.Dir` for slash-separated relative paths.
fn dir_of(s: &str) -> String {
    match s.rfind('/') {
        None => ".".to_string(),
        Some(0) => "/".to_string(),
        Some(i) => s[..i].to_string(),
    }
}

/// `filepath.Base`.
fn base_of(s: &str) -> &str {
    match s.rfind('/') {
        None => s,
        Some(i) => &s[i + 1..],
    }
}

/// `filepath.Base` with the given extension stripped.
fn base_stem(s: &str, ext: &str) -> String {
    base_of(s).strip_suffix(ext).unwrap_or(base_of(s)).to_string()
}

fn git_output(repo_root: &str, args: &[&str]) -> std::io::Result<String> {
    let out = Command::new("git").args(args).current_dir(repo_root).output()?;
    if !out.status.success() {
        return Err(std::io::Error::new(
            std::io::ErrorKind::Other,
            format!("git {:?} failed", args),
        ));
    }
    Ok(String::from_utf8_lossy(&out.stdout).into_owned())
}

/// Port of `GetChangedFiles`. Preserves order and de-duplicates.
pub fn get_changed_files(repo_root: &str, compare_ref: &str) -> Vec<String> {
    let mut files: Vec<String> = Vec::new();
    let mut seen: HashSet<String> = HashSet::new();

    if let Ok(out_status) = git_output(repo_root, &["status", "--porcelain"]) {
        for raw in out_status.lines() {
            let line = raw.trim();
            if line.len() < 4 {
                continue;
            }
            // Faithful to Go: byte slice at index 3 (not char-aware).
            let mut path = line.get(3..).unwrap_or("").trim().to_string();
            if let Some(idx) = path.find("->") {
                path = path[idx + 2..].trim().to_string();
            }
            let path = path.trim_matches('"').to_string();
            if !seen.contains(&path) && !path.is_empty() {
                seen.insert(path.clone());
                files.push(path);
            }
        }
    }

    if !compare_ref.is_empty() {
        if let Ok(out_diff) = git_output(repo_root, &["diff", "--name-only", compare_ref]) {
            for raw in out_diff.lines() {
                let path = raw.trim().to_string();
                if !seen.contains(&path) && !path.is_empty() {
                    seen.insert(path.clone());
                    files.push(path);
                }
            }
        }
    }

    files
}

fn add_xunit_target(
    repo_root: &str,
    candidates: &[String],
    test_set: &mut HashSet<String>,
    plan: &mut SelectionPlan,
    trigger: &str,
) -> bool {
    for cand in candidates {
        if cand.is_empty() || cand == "Ashfall.Core.Tests/." {
            continue;
        }
        if !path_exists(&format!("{}/{}", repo_root, cand)) {
            continue;
        }
        if !test_set.contains(cand) {
            test_set.insert(cand.clone());
            plan.selected_tests.push(selected(
                cand,
                KIND_XUNIT,
                format!("bash scripts/run_test.sh {}", cand),
                trigger,
            ));
        }
        return true;
    }
    false
}

/// Port of `SelectTestsForFiles`.
pub fn select_tests_for_files(repo_root: &str, changed_files: &[String]) -> SelectionPlan {
    let mut plan = SelectionPlan {
        changed_files: changed_files.to_vec(),
        selected_tests: Vec::new(),
        explanation: String::new(),
    };
    let mut test_set: HashSet<String> = HashSet::new();

    for file in changed_files {
        let clean = to_slash(file);

        // 1. Direct test files changed.
        if clean.starts_with("Ashfall.Core.Tests/") && clean.ends_with(".cs") {
            let full_path = format!("{}/{}", repo_root, clean);
            if path_exists(&full_path) && !test_set.contains(&clean) {
                test_set.insert(clean.clone());
                plan.selected_tests.push(selected(
                    &clean,
                    KIND_XUNIT,
                    format!("bash scripts/run_test.sh {}", clean),
                    file,
                ));
            }
            continue;
        }

        if clean.starts_with("tests/") && clean.ends_with(".py") {
            if !test_set.contains(&clean) {
                test_set.insert(clean.clone());
                plan.selected_tests.push(selected(
                    &clean,
                    KIND_PYTEST,
                    format!("pytest -n 4 -m fast {}", clean),
                    file,
                ));
            }
            continue;
        }

        // 2. Domain / Core C# files: Assets/Ashfall.Core/<Subsystem>/<Name>.cs
        if clean.starts_with("Assets/Ashfall.Core/") && clean.ends_with(".cs") {
            let sub = &clean["Assets/Ashfall.Core/".len()..];
            let dir = dir_of(sub);
            let base = base_stem(sub, ".cs");

            let candidates = vec![
                format!("Ashfall.Core.Tests/{}/{}Tests.cs", dir, base),
                format!("Ashfall.Core.Tests/{}/{}Test.cs", dir, base),
                format!("Ashfall.Core.Tests/{}Tests.cs", base),
                format!("Ashfall.Core.Tests/{}", dir),
            ];

            let mut matched = false;
            for cand in &candidates {
                if cand == "Ashfall.Core.Tests/." {
                    continue;
                }
                if path_exists(&format!("{}/{}", repo_root, cand)) {
                    if !test_set.contains(cand) {
                        test_set.insert(cand.clone());
                        plan.selected_tests.push(selected(
                            cand,
                            KIND_XUNIT,
                            format!("bash scripts/run_test.sh {}", cand),
                            file,
                        ));
                    }
                    matched = true;
                    break;
                }
            }

            if !matched {
                plan.selected_tests.push(selected(
                    &format!("Ashfall.Core.Tests (Filter: {})", base),
                    KIND_XUNIT,
                    format!(
                        "dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter FullyQualifiedName~{}",
                        base
                    ),
                    file,
                ));
            }
            continue;
        }

        // 3. Python tools changed.
        if clean.starts_with("tools/") && clean.ends_with(".py") {
            let base = base_stem(&clean, ".py");
            let py_test_cand = format!("tests/test_{}.py", base);
            if path_exists(&format!("{}/{}", repo_root, py_test_cand)) && !test_set.contains(&py_test_cand) {
                test_set.insert(py_test_cand.clone());
                plan.selected_tests.push(selected(
                    &py_test_cand,
                    KIND_PYTEST,
                    format!("pytest -n 4 -m fast {}", py_test_cand),
                    file,
                ));
            }
            continue;
        }

        // 4. Godot host C# sources: src/<Dir>/<Name>.cs.
        if clean.starts_with("src/") && clean.ends_with(".cs") {
            let sub = &clean["src/".len()..];
            let dir = dir_of(sub);
            let base = base_stem(sub, ".cs");
            let mut candidates: Vec<String> = Vec::new();
            if dir != "." {
                candidates.push(format!("Ashfall.Core.Tests/{}/{}Tests.cs", dir, base));
                candidates.push(format!("Ashfall.Core.Tests/{}/{}Test.cs", dir, base));
                candidates.push(format!("Ashfall.Core.Tests/{}", dir));
            }
            candidates.push("Ashfall.Core.Tests/Tooling".to_string());
            if add_xunit_target(repo_root, &candidates, &mut test_set, &mut plan, file) {
                continue;
            }
        }

        // 5. Authored game data.
        if clean.starts_with("Assets/StreamingAssets/Data/") {
            let candidates = vec![
                "Ashfall.Core.Tests/Data".to_string(),
                "Ashfall.Core.Tests/Tooling".to_string(),
            ];
            if add_xunit_target(repo_root, &candidates, &mut test_set, &mut plan, file) {
                continue;
            }
        }

        // 6. Localization catalog.
        if clean.starts_with("assets/l10n/") {
            let candidates = vec![
                "Ashfall.Core.Tests/Localization".to_string(),
                "Ashfall.Core.Tests/Tooling".to_string(),
            ];
            if add_xunit_target(repo_root, &candidates, &mut test_set, &mut plan, file) {
                continue;
            }
        }

        // 7. Gate / tooling / CI config.
        if clean.starts_with("scripts/")
            || clean.starts_with("docs/ci/")
            || clean.starts_with(".github/")
            || clean.starts_with("tools/gotools/")
        {
            let candidates = vec!["Ashfall.Core.Tests/Tooling".to_string()];
            if add_xunit_target(repo_root, &candidates, &mut test_set, &mut plan, file) {
                continue;
            }
        }
    }

    if plan.selected_tests.is_empty() {
        plan.explanation = NO_TARGETS.to_string();
    } else {
        plan.explanation = format!(
            "Selected {} high-signal, targeted test runner(s) for {} changed files.",
            plan.selected_tests.len(),
            changed_files.len()
        );
    }

    plan
}

#[cfg(test)]
mod tests {
    use super::*;
    use std::fs;

    fn temp_repo(tag: &str) -> String {
        let base = std::env::temp_dir().join(format!(
            "ashfall-rstools-selector-{}-{}",
            tag,
            std::process::id()
        ));
        let _ = fs::remove_dir_all(&base);
        base.to_string_lossy().into_owned()
    }

    fn touch(repo: &str, rel: &str) {
        let full = format!("{}/{}", repo, rel);
        let p = Path::new(&full);
        fs::create_dir_all(p.parent().unwrap()).unwrap();
        fs::write(p, b"// stub\n").unwrap();
    }

    #[test]
    fn host_file_maps_to_existing_tooling_suite() {
        let repo = temp_repo("host");
        touch(&repo, "Ashfall.Core.Tests/Tooling/SomeTests.cs");

        let plan = select_tests_for_files(&repo, &["src/UI/SomePanel.cs".to_string()]);
        assert_eq!(plan.selected_tests.len(), 1);
        assert_eq!(plan.selected_tests[0].target_file, "Ashfall.Core.Tests/Tooling");
        assert_eq!(plan.selected_tests[0].kind, KIND_XUNIT);
        assert_eq!(
            plan.selected_tests[0].command,
            "bash scripts/run_test.sh Ashfall.Core.Tests/Tooling"
        );
        assert_eq!(plan.selected_tests[0].trigger_file, "src/UI/SomePanel.cs");

        let _ = fs::remove_dir_all(&repo);
    }

    #[test]
    fn data_and_l10n_map_to_their_suites() {
        let repo = temp_repo("data");
        touch(&repo, "Ashfall.Core.Tests/Data/SomeTests.cs");
        touch(&repo, "Ashfall.Core.Tests/Localization/SomeTests.cs");

        let d = select_tests_for_files(&repo, &["Assets/StreamingAssets/Data/foo.json".to_string()]);
        assert_eq!(d.selected_tests[0].target_file, "Ashfall.Core.Tests/Data");

        let l = select_tests_for_files(&repo, &["assets/l10n/strings.csv".to_string()]);
        assert_eq!(l.selected_tests[0].target_file, "Ashfall.Core.Tests/Localization");

        let _ = fs::remove_dir_all(&repo);
    }

    #[test]
    fn unmapped_change_yields_no_targets_and_policy_explanation() {
        let repo = temp_repo("unmapped");
        let plan = select_tests_for_files(&repo, &["README.md".to_string()]);
        assert!(plan.selected_tests.is_empty());
        assert_eq!(plan.explanation, NO_TARGETS);
        // A nil selected list marshals to null, like Go, and key order matches.
        let json = plan.to_json_pretty();
        assert!(json.contains("\"selected_tests\": null"), "got: {}", json);
        assert!(
            json.find("\"changed_files\"").unwrap() < json.find("\"selected_tests\"").unwrap(),
            "Go key order: changed_files before selected_tests; got: {}",
            json
        );
        let _ = fs::remove_dir_all(&repo);
    }

    #[test]
    fn core_test_file_changed_is_selected_directly() {
        let repo = temp_repo("coretest");
        touch(&repo, "Ashfall.Core.Tests/Needs/NeedsSystemTests.cs");
        let plan = select_tests_for_files(
            &repo,
            &["Ashfall.Core.Tests/Needs/NeedsSystemTests.cs".to_string()],
        );
        assert_eq!(plan.selected_tests.len(), 1);
        assert_eq!(plan.selected_tests[0].kind, KIND_XUNIT);
        let _ = fs::remove_dir_all(&repo);
    }

    #[test]
    fn tooling_py_change_maps_to_pytest_when_present() {
        let repo = temp_repo("py");
        touch(&repo, "tests/test_helper.py");
        let plan = select_tests_for_files(&repo, &["tools/helper.py".to_string()]);
        assert_eq!(plan.selected_tests.len(), 1);
        assert_eq!(plan.selected_tests[0].kind, KIND_PYTEST);
        assert_eq!(plan.selected_tests[0].target_file, "tests/test_helper.py");
        let _ = fs::remove_dir_all(&repo);
    }

    #[test]
    fn dir_of_and_base_stem_match_filepath_semantics() {
        assert_eq!(dir_of("Foo.cs"), ".");
        assert_eq!(dir_of("UI/Foo.cs"), "UI");
        assert_eq!(dir_of("a/b/c.cs"), "a/b");
        assert_eq!(base_of("a/b/c.cs"), "c.cs");
        assert_eq!(base_stem("a/b/c.cs", ".cs"), "c");
        assert_eq!(base_stem("Weird", ".cs"), "Weird");
    }
}