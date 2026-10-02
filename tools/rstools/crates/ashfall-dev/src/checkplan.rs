//! Port of `tools/gotools/pkg/checkplan/checkplan.go`.
//!
//! `check-plan` verifies that a commit touching code (or authoritative game
//! data) also has an approved plan in `.ai/plans/`, `docs/plans/integrated/`,
//! or `.ai/plan.md`. It is read-only: it runs `git diff`/`git status` to learn
//! the changed set and reads candidate plan files.
//!
//! Parity notes:
//! * [`is_code_file`] reproduces the exact prefix/suffix/extension table.
//! * [`is_approved_plan`] reproduces `TrimSpace` then `Trim("*_`# ")` then the
//!   case-insensitive `STATUS: APPROVED BY USER` / `FULLY INTEGRATED` match.
//! * The default (`git status --porcelain`) branch reproduces Go's byte-slice
//!   parse of `line[3:]` **after** `TrimSpace` — including its over-shift on an
//!   unstaged ` M` status, exactly as the Go original does (see the Stage 1
//!   finding on `pkg/selector`; the same quirk lives here).
//! * Plan discovery walks `.ai/plans` and `docs/plans/integrated` via
//!   [`crate::gowalk`] (Go `filepath.Walk` order), collecting `.md` files whose
//!   basename is not `template.md`.

use crate::gowalk;
use std::path::{Component, Path, PathBuf};

/// `STATUS: APPROVED BY USER` marker constant (kept for parity with upstream).
#[allow(dead_code)]
pub const APPROVED_STATUS_MARKER: &str = "STATUS: APPROVED BY USER";

/// Port of `IsCodeFile`.
pub fn is_code_file(path: &str) -> bool {
    let norm = path.replace('\\', "/");

    if norm.starts_with(".ai/")
        || norm.starts_with("docs/")
        || norm.starts_with(".github/")
        || norm.starts_with(".git/")
        || norm.ends_with(".md")
        || norm.ends_with(".txt")
        || norm.ends_with(".png")
        || norm.ends_with(".jpg")
        || norm.ends_with(".svg")
        || norm.ends_with(".import")
        || norm.ends_with(".gitignore")
        || norm.ends_with(".gitattributes")
    {
        return false;
    }

    matches!(
        go_ext_lower(&norm).as_str(),
        ".cs" | ".gd" | ".py" | ".go" | ".tscn" | ".godot" | ".shader" | ".json"
    )
}

/// `filepath.Ext` lowercased.
fn go_ext_lower(path: &str) -> String {
    let name = path.rsplit('/').next().unwrap_or(path);
    match name.rfind('.') {
        Some(i) => name[i..].to_ascii_lowercase(),
        None => String::new(),
    }
}

/// Port of the private `isApprovedPlan`.
fn is_approved_plan(path: &str, content: &str) -> bool {
    let base = path.rsplit('/').next().unwrap_or(path);
    if base.eq_ignore_ascii_case("template.md") {
        return false;
    }
    for raw in content.lines() {
        let trimmed = raw.trim();
        let cleaned = trimmed
            .trim_matches(|c| matches!(c, '*' | '_' | '`' | '#' | ' '));
        let upper = cleaned.to_uppercase();
        if upper == "STATUS: APPROVED BY USER" || upper.contains("FULLY INTEGRATED") {
            return true;
        }
    }
    false
}

/// `filepath.Abs`-style path cleaning (join against CWD, then `Clean`).
fn go_abs(p: &str) -> PathBuf {
    let joined = if Path::new(p).is_absolute() {
        PathBuf::from(p)
    } else {
        std::env::current_dir()
            .unwrap_or_else(|_| PathBuf::from("."))
            .join(p)
    };
    let mut out = PathBuf::new();
    for component in joined.components() {
        match component {
            Component::CurDir => {}
            Component::ParentDir => {
                if !out.pop() && !out.has_root() {
                    out.push("..");
                }
            }
            other => out.push(other.as_os_str()),
        }
    }
    out
}

/// `filepath.Rel(root, path)` rendered with forward slashes, falling back to
/// the raw path when a relative form cannot be computed (like Go's `relErr`).
fn rel_slash(root: &str, path: &Path) -> String {
    let abs_root = go_abs(root);
    let abs_path = if path.is_absolute() {
        path.to_path_buf()
    } else {
        go_abs(&path.to_string_lossy())
    };
    match abs_path.strip_prefix(&abs_root) {
        Ok(rel) => rel.to_string_lossy().replace('\\', "/"),
        Err(_) => path.to_string_lossy().replace('\\', "/"),
    }
}

/// Run git, returning trimmed stdout, or a Go-style `exit status N` error.
fn git_output(dir: &str, args: &[&str]) -> Result<String, String> {
    let out = std::process::Command::new("git")
        .args(args)
        .current_dir(dir)
        .output()
        .map_err(|e| e.to_string())?;
    if !out.status.success() {
        return Err(format!(
            "exit status {}",
            out.status.code().unwrap_or(-1)
        ));
    }
    Ok(String::from_utf8_lossy(&out.stdout).into_owned())
}

/// Parse one `git status --porcelain` line the way Go's `CheckApprovedPlan`
/// does (TrimSpace whole line, then take bytes `[3..]`).
fn porcelain_path(line_raw: &str) -> Option<String> {
    let line = line_raw.trim();
    let bytes = line.as_bytes();
    if bytes.len() < 4 {
        return None;
    }
    let mut path = String::from_utf8_lossy(&bytes[3..]).trim().to_string();
    if let Some(idx) = path.find("->") {
        path = path[idx + 2..].trim().to_string();
    }
    let path = path.trim_matches('"').to_string();
    if path.is_empty() {
        None
    } else {
        Some(path)
    }
}

fn split_nonempty_lines(text: &str) -> Vec<String> {
    text.lines()
        .map(|l| l.trim().to_string())
        .filter(|l| !l.is_empty())
        .collect()
}

/// Collect approved `.md` plans under a directory (Go `filepath.Walk` order).
fn collect_approved(root: &str, dir: &Path, out: &mut Vec<String>) {
    if !dir.exists() {
        return;
    }
    for entry in gowalk::walk_go(dir, |_| true) {
        if entry.is_dir {
            continue;
        }
        let name = entry
            .path
            .file_name()
            .map(|n| n.to_string_lossy().into_owned())
            .unwrap_or_default();
        if !name.ends_with(".md") || name.eq_ignore_ascii_case("template.md") {
            continue;
        }
        if let Ok(content) = std::fs::read_to_string(&entry.path) {
            let display = entry.path.to_string_lossy();
            if is_approved_plan(&display, &content) {
                out.push(rel_slash(root, &entry.path));
            }
        }
    }
}

/// Port of `CheckApprovedPlan`. Returns `(ok, message, error)`.
pub fn check_approved_plan(
    repo_root: &str,
    staged_only: bool,
    compare_ref: &str,
    explicit_files: &[String],
) -> Result<(bool, String), String> {
    let files: Vec<String> = if !explicit_files.is_empty() {
        explicit_files.to_vec()
    } else if staged_only {
        let out =
            git_output(repo_root, &["diff", "--cached", "--name-only"]).map_err(|e| {
                format!("git diff --cached failed: {}", e)
            })?;
        split_nonempty_lines(&out)
    } else if !compare_ref.is_empty() {
        let out = git_output(repo_root, &["diff", "--name-only", compare_ref])
            .map_err(|e| format!("git diff failed: {}", e))?;
        split_nonempty_lines(&out)
    } else {
        let out = git_output(repo_root, &["status", "--porcelain"])
            .map_err(|e| format!("git status failed: {}", e))?;
        out.lines().filter_map(porcelain_path).collect()
    };

    let code_files: Vec<String> = files.into_iter().filter(|f| is_code_file(f)).collect();

    if code_files.is_empty() {
        return Ok((true, "No code files changed; plan check skipped.".to_string()));
    }

    let mut approved_plans: Vec<String> = Vec::new();
    collect_approved(
        repo_root,
        &Path::new(repo_root).join(".ai").join("plans"),
        &mut approved_plans,
    );
    collect_approved(
        repo_root,
        &Path::new(repo_root)
            .join("docs")
            .join("plans")
            .join("integrated"),
        &mut approved_plans,
    );

    let root_plan = Path::new(repo_root).join(".ai").join("plan.md");
    if let Ok(content) = std::fs::read_to_string(&root_plan) {
        let display = root_plan.to_string_lossy();
        if is_approved_plan(&display, &content) {
            approved_plans.push(".ai/plan.md".to_string());
        }
    }

    if approved_plans.is_empty() {
        let msg = format!(
            "No approved plan found for changed files ({} code file(s) changed: {}). Create/update a plan in .ai/plans/ and set STATUS: APPROVED BY USER.",
            code_files.len(),
            code_files.join(", ")
        );
        return Ok((false, msg));
    }

    Ok((
        true,
        format!("Approved plan(s) verified: {}", approved_plans.join(", ")),
    ))
}

#[cfg(test)]
mod tests {
    use super::*;
    use std::fs;

    fn tmpdir(tag: &str) -> PathBuf {
        let base = std::env::temp_dir().join(format!(
            "ashfall-rstools-checkplan-{}-{}",
            tag,
            std::process::id()
        ));
        let _ = fs::remove_dir_all(&base);
        fs::create_dir_all(&base).unwrap();
        base
    }

    #[test]
    fn is_code_file_table_matches_go() {
        assert!(is_code_file("Assets/Ashfall.Core/Needs/NeedsSystem.cs"));
        assert!(is_code_file("src/Host/HostCli.cs"));
        assert!(is_code_file("tools/gotools/main.go"));
        assert!(is_code_file("tests/test_something.py"));
        assert!(is_code_file("Assets/StreamingAssets/Data/items.json"));
        assert!(!is_code_file(".ai/state.md"));
        assert!(!is_code_file(".ai/plans/my_plan.md"));
        assert!(!is_code_file("docs/INDEX.md"));
        assert!(!is_code_file(".gitignore"));
        assert!(!is_code_file("assets/sprites/player.png"));
    }

    #[test]
    fn approved_plan_marker_and_integrated_banner() {
        assert!(is_approved_plan("x.md", "# Feature Plan\nSTATUS: APPROVED BY USER\n"));
        assert!(is_approved_plan(
            "x.md",
            "# FULLY INTEGRATED — FULLY INTEGRATED\ntext"
        ));
        assert!(!is_approved_plan("template.md", "STATUS: APPROVED BY USER"));
        assert!(!is_approved_plan("x.md", "nothing here"));
    }

    #[test]
    fn non_code_files_skip_the_check() {
        let root = tmpdir("nocode");
        let (ok, msg) = check_approved_plan(
            &root.to_string_lossy(),
            false,
            "",
            &["docs/INDEX.md".to_string()],
        )
        .unwrap();
        assert!(ok, "{msg}");
        assert_eq!(msg, "No code files changed; plan check skipped.");
        let _ = fs::remove_dir_all(&root);
    }

    #[test]
    fn missing_plan_fails_then_integrated_subfolder_passes() {
        let root = tmpdir("plan");
        fs::create_dir_all(root.join(".ai").join("plans")).unwrap();
        let file = vec!["src/Host/HostCli.cs".to_string()];

        let (ok, msg) =
            check_approved_plan(&root.to_string_lossy(), false, "", &file).unwrap();
        assert!(!ok);
        assert!(msg.contains("No approved plan found"), "{msg}");

        fs::write(
            root.join(".ai").join("plans").join("feature_plan.md"),
            "# Feature Plan\nSTATUS: APPROVED BY USER\n",
        )
        .unwrap();
        let (ok, _) = check_approved_plan(&root.to_string_lossy(), false, "", &file).unwrap();
        assert!(ok);

        fs::remove_file(root.join(".ai").join("plans").join("feature_plan.md")).unwrap();
        let integrated = root.join(".ai").join("plans").join("integrated").join("systems");
        fs::create_dir_all(&integrated).unwrap();
        fs::write(
            integrated.join("feature_plan.md"),
            "# Feature Plan\nSTATUS: APPROVED BY USER\n",
        )
        .unwrap();
        let (ok, msg) = check_approved_plan(&root.to_string_lossy(), false, "", &file).unwrap();
        assert!(ok, "{msg}");
        assert!(msg.contains(".ai/plans/integrated/systems/feature_plan.md"), "{msg}");
        let _ = fs::remove_dir_all(&root);
    }
}