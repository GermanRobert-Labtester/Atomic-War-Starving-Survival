//! Port of `tools/gotools/pkg/indexer/indexer.go`.
//!
//! Repository file indexer, used by the `index [--root R] [--hash] [--json]
//! [--list]` subcommand. Reproduces the ignored-directory set (including the
//! upstream `.vscode: false` entry, which means `.vscode` is *not* skipped), the
//! category table, `filepath.Abs`-style root resolution, SHA-256 on demand, and
//! the exact `--json` payload:
//!
//! * `root_path`, `indexed_at`, `total_files`, `total_bytes`, `duration_ms`,
//!   `category_counts` (go's map marshalling = sorted keys), `files`
//!   (omitted unless `--list`, and omitted — not `null` — when empty);
//! * `sha256` omitted per file unless `--hash`.
//!
//! Not ported: the resident indexer. Go's `--watch` (fsnotify) and `--serve`
//! (HTTP API) keep a long-running process alive; the port reports them as
//! unsupported and exits 2 rather than pretending to serve. See
//! [`RESIDENT_UNSUPPORTED`].
//!
//! Ordering and timing: Go indexes with 8 goroutines, so its `files` order and
//! `category_counts` iteration order are not reproducible; the port walks in
//! lexical order and prints categories sorted. `indexed_at` is wall-clock.

use crate::gotime;
use crate::gowalk;
use crate::jsonout;
use serde::Serialize;
use sha2::{Digest, Sha256};
use std::collections::BTreeMap;
use std::path::{Component, Path, PathBuf};
use std::time::Instant;

/// Exact text the CLI emits for `--watch` / `--serve`.
pub const RESIDENT_UNSUPPORTED: &str = "[ashfall-dev] index --watch/--serve is not supported in the Rust port yet (Stage 2); use tools/gotools for the resident indexer.";

/// Upstream `defaultIgnoredDirs`. `.vscode` maps to `false` there, so it is
/// deliberately absent from this skip list.
const IGNORED_DIRS: [&str; 7] = [
    ".git",
    ".godot",
    "bin",
    "obj",
    "node_modules",
    "__pycache__",
    ".pytest_cache",
];

#[derive(Debug, Clone, Serialize)]
pub struct FileRecord {
    pub path: String,
    pub size: i64,
    pub mod_time: String,
    #[serde(skip_serializing_if = "String::is_empty")]
    pub sha256: String,
    pub category: String,
}

#[derive(Debug, Clone)]
pub struct IndexReport {
    pub root_path: String,
    pub indexed_at: String,
    pub total_files: usize,
    pub total_bytes: i64,
    pub duration_ms: i64,
    pub category_counts: BTreeMap<String, usize>,
    pub files: Vec<FileRecord>,
}

/// Key order plus `files` omission semantics of Go's `IndexReport`.
#[derive(Serialize)]
struct IndexReportJson<'a> {
    root_path: &'a str,
    indexed_at: &'a str,
    total_files: usize,
    total_bytes: i64,
    duration_ms: i64,
    category_counts: &'a BTreeMap<String, usize>,
    #[serde(skip_serializing_if = "Option::is_none")]
    files: Option<&'a Vec<FileRecord>>,
}

impl IndexReport {
    fn to_view(&self) -> IndexReportJson<'_> {
        IndexReportJson {
            root_path: &self.root_path,
            indexed_at: &self.indexed_at,
            total_files: self.total_files,
            total_bytes: self.total_bytes,
            duration_ms: self.duration_ms,
            category_counts: &self.category_counts,
            // Go's `files,omitempty`: `--list` on an empty tree leaves the nil
            // slice omitted rather than `null`.
            files: if self.files.is_empty() {
                None
            } else {
                Some(&self.files)
            },
        }
    }

    /// Port of `ToJSON`: `json.MarshalIndent(r, "", "  ")`.
    pub fn to_json(&self, indent: bool) -> String {
        if indent {
            jsonout::to_pretty(&self.to_view())
        } else {
            jsonout::to_compact(&self.to_view())
        }
    }

    /// Port of `Summary`. Category order is sorted here; Go iterates a map, so
    /// its order varies between runs.
    pub fn summary(&self) -> String {
        let mut out = String::new();
        out.push_str(&format!("Repository Index Summary ({})\n", self.root_path));
        out.push_str(&format!(
            "Indexed in {} ms | Total Files: {} | Total Size: {:.2} MB\n",
            self.duration_ms,
            self.total_files,
            self.total_bytes as f64 / (1024.0 * 1024.0)
        ));
        out.push_str("File Distribution:\n");
        for (category, count) in &self.category_counts {
            out.push_str(&format!("  - {:<18}: {}\n", category, count));
        }
        out
    }
}

/// `filepath.Ext` lowercased.
fn go_ext_lower(path: &str) -> String {
    let name = path.rsplit('/').next().unwrap_or(path);
    match name.rfind('.') {
        Some(i) => name[i..].to_ascii_lowercase(),
        None => String::new(),
    }
}

/// Port of `categorizePath`.
fn categorize_path(path: &str) -> &'static str {
    match go_ext_lower(path).as_str() {
        ".cs" => {
            if path.contains("Tests") {
                "csharp_test"
            } else {
                "csharp_source"
            }
        }
        ".json" => "json_data",
        ".yaml" | ".yml" => "yaml_config",
        ".py" => "python_script",
        ".sh" => "shell_script",
        ".png" | ".jpg" | ".jpeg" | ".webp" | ".svg" => "image_asset",
        ".wav" | ".ogg" | ".mp3" => "audio_asset",
        ".tscn" | ".gd" => "godot_resource",
        ".md" => "documentation",
        _ => "other",
    }
}

/// `filepath.Abs`: join against the current directory, then `Clean`.
fn go_abs(root: &str) -> std::io::Result<PathBuf> {
    let joined = if Path::new(root).is_absolute() {
        PathBuf::from(root)
    } else {
        std::env::current_dir()?.join(root)
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
    Ok(out)
}

/// Port of `IndexRepository`.
pub fn index_repository(
    root: &str,
    compute_hashes: bool,
    include_file_list: bool,
) -> Result<IndexReport, std::io::Error> {
    let start = Instant::now();
    let clean_root = go_abs(root)?;
    let root_str = clean_root.to_string_lossy().into_owned();

    let mut report = IndexReport {
        root_path: root_str.clone(),
        indexed_at: gotime::now_utc_rfc3339(),
        total_files: 0,
        total_bytes: 0,
        duration_ms: 0,
        category_counts: BTreeMap::new(),
        files: Vec::new(),
    };

    for entry in gowalk::walk_go(&clean_root, |name| !is_ignored_dir(name)) {
        if entry.is_dir {
            continue;
        }
        let info = match std::fs::symlink_metadata(&entry.path) {
            Ok(i) => i,
            Err(_) => continue,
        };

        let rel = entry
            .path
            .strip_prefix(&clean_root)
            .map(|p| p.to_string_lossy().into_owned())
            .unwrap_or_else(|_| entry.path.to_string_lossy().into_owned());
        let rel_slash = rel.replace('\\', "/");

        let mut record = FileRecord {
            path: rel_slash,
            size: info.len() as i64,
            mod_time: info
                .modified()
                .map(gotime::system_time_local_rfc3339)
                .unwrap_or_default(),
            sha256: String::new(),
            category: String::new(),
        };
        record.category = categorize_path(&record.path).to_string();

        if compute_hashes {
            if let Ok(bytes) = std::fs::read(&entry.path) {
                record.sha256 = Sha256::digest(&bytes)
                    .iter()
                    .map(|b| format!("{:02x}", b))
                    .collect();
            }
        }

        report.total_files += 1;
        report.total_bytes += record.size;
        *report.category_counts.entry(record.category.clone()).or_insert(0) += 1;
        if include_file_list {
            report.files.push(record);
        }
    }

    report.duration_ms = start.elapsed().as_millis() as i64;
    Ok(report)
}

fn is_ignored_dir(name: &std::ffi::OsStr) -> bool {
    IGNORED_DIRS.iter().any(|d| name == *d)
}

#[cfg(test)]
mod tests {
    use super::*;

    fn tmpdir(tag: &str) -> PathBuf {
        let base = std::env::temp_dir().join(format!(
            "ashfall-rstools-indexer-{}-{}",
            tag,
            std::process::id()
        ));
        let _ = std::fs::remove_dir_all(&base);
        std::fs::create_dir_all(&base).unwrap();
        base
    }

    #[test]
    fn category_table_matches_go() {
        assert_eq!(categorize_path("src/Main.cs"), "csharp_source");
        assert_eq!(categorize_path("Ashfall.Core.Tests/NeedsTests.cs"), "csharp_test");
        assert_eq!(categorize_path("Assets/StreamingAssets/Data/x.json"), "json_data");
        assert_eq!(categorize_path("a.yml"), "yaml_config");
        assert_eq!(categorize_path("tools/x.py"), "python_script");
        assert_eq!(categorize_path("bin/x.sh"), "shell_script");
        assert_eq!(categorize_path("assets/a.svg"), "image_asset");
        assert_eq!(categorize_path("assets/a.ogg"), "audio_asset");
        assert_eq!(categorize_path("scenes/main.tscn"), "godot_resource");
        assert_eq!(categorize_path("src/thing.gd"), "godot_resource");
        assert_eq!(categorize_path("README.md"), "documentation");
        assert_eq!(categorize_path("LICENSE"), "other");
    }

    #[test]
    fn indexes_files_and_skips_ignored_dirs() {
        let root = tmpdir("walk");
        std::fs::create_dir_all(root.join("src")).unwrap();
        std::fs::create_dir_all(root.join(".git")).unwrap();
        std::fs::create_dir_all(root.join("node_modules")).unwrap();
        std::fs::create_dir_all(root.join(".vscode")).unwrap();
        std::fs::write(root.join("src").join("Main.cs"), b"class M {}").unwrap();
        std::fs::write(root.join(".git").join("HEAD"), b"ref: x").unwrap();
        std::fs::write(root.join("node_modules").join("dep.js"), b"x").unwrap();
        // Upstream maps `.vscode` to false, so it is walked, not skipped.
        std::fs::write(root.join(".vscode").join("settings.json"), b"{}").unwrap();
        std::fs::write(root.join("README.md"), b"# x").unwrap();

        let report = index_repository(&root.to_string_lossy(), false, true).unwrap();
        assert_eq!(report.total_files, 3, "{:#?}", report.files);
        let paths: Vec<&str> = report.files.iter().map(|f| f.path.as_str()).collect();
        assert_eq!(paths, vec![".vscode/settings.json", "README.md", "src/Main.cs"]);
        assert_eq!(report.category_counts.get("csharp_source"), Some(&1));
        assert_eq!(report.category_counts.get("json_data"), Some(&1));
        assert_eq!(report.category_counts.get("documentation"), Some(&1));
        assert!(report.files.iter().all(|f| f.sha256.is_empty()));
        assert_eq!(report.root_path, root.to_string_lossy().as_ref());
        let _ = std::fs::remove_dir_all(&root);
    }

    #[test]
    fn hashes_are_computed_only_when_requested() {
        let root = tmpdir("hash");
        std::fs::write(root.join("a.json"), b"{}").unwrap();
        let hashed = index_repository(&root.to_string_lossy(), true, true).unwrap();
        assert_eq!(hashed.files[0].sha256.len(), 64);
        let unhashed = index_repository(&root.to_string_lossy(), false, true).unwrap();
        assert!(unhashed.files[0].sha256.is_empty());
        // sha256 is omitempty in Go.
        assert!(!unhashed.to_json(true).contains("sha256"));
        assert!(hashed.to_json(true).contains("\"sha256\": \""));
        let _ = std::fs::remove_dir_all(&root);
    }

    #[test]
    fn files_are_omitted_without_list_and_json_keys_are_ordered() {
        let empty = tmpdir("json-empty");
        let bare = index_repository(&empty.to_string_lossy(), false, false).unwrap();
        let json = bare.to_json(true);
        assert!(!json.contains("\"files\""), "got {json}");
        // With no files and no nested object, every line is a top-level key.
        let keys: Vec<&str> = json
            .lines()
            .filter_map(|l| l.trim().strip_prefix('"').and_then(|r| r.split('"').next()))
            .collect();
        assert_eq!(
            keys,
            vec![
                "root_path",
                "indexed_at",
                "total_files",
                "total_bytes",
                "duration_ms",
                "category_counts"
            ],
            "got {json}"
        );
        assert!(json.contains("\"category_counts\": {}"), "got {json}");
        let _ = std::fs::remove_dir_all(&empty);

        let root = tmpdir("json");
        std::fs::write(root.join("a.json"), b"{}").unwrap();
        let listed = index_repository(&root.to_string_lossy(), false, true).unwrap();
        let json = listed.to_json(true);
        assert!(json.contains("\"files\""), "got {json}");
        assert!(json.contains("\"category_counts\": {"), "got {json}");
        assert!(json.contains("\"json_data\": 1"), "got {json}");
        assert!(json.contains("\"mod_time\": \""), "got {json}");
        let _ = std::fs::remove_dir_all(&root);
    }

    #[test]
    fn summary_reports_totals_and_sorted_categories() {
        let root = tmpdir("summary");
        std::fs::write(root.join("a.json"), b"{}").unwrap();
        std::fs::write(root.join("b.md"), b"x").unwrap();
        let report = index_repository(&root.to_string_lossy(), false, false).unwrap();
        let s = report.summary();
        assert!(s.starts_with(&format!("Repository Index Summary ({})\n", report.root_path)));
        assert!(s.contains("Total Files: 2 | Total Size: 0.00 MB"));
        assert!(s.contains("File Distribution:\n"));
        let doc = s.find("documentation").unwrap();
        let json = s.find("json_data").unwrap();
        assert!(doc < json, "sorted: {s}");
        let _ = std::fs::remove_dir_all(&root);
    }

    #[test]
    fn go_abs_cleans_like_filepath_abs() {
        let cwd = std::env::current_dir().unwrap();
        assert_eq!(go_abs(".").unwrap(), cwd);
        assert_eq!(go_abs("./x/../y").unwrap(), cwd.join("y"));
        assert_eq!(go_abs("/a/b/../c").unwrap(), PathBuf::from("/a/c"));
        assert_eq!(go_abs("/../a").unwrap(), PathBuf::from("/a"));
    }

    #[test]
    fn missing_root_indexes_nothing_without_error() {
        let missing = std::env::temp_dir().join("ashfall-rstools-indexer-absent");
        let report = index_repository(&missing.to_string_lossy(), false, false).unwrap();
        assert_eq!(report.total_files, 0);
        assert!(report.category_counts.is_empty());
        assert!(report.to_json(true).contains("\"category_counts\": {}"));
    }

    #[test]
    fn resident_mode_is_explicitly_unsupported() {
        assert!(RESIDENT_UNSUPPORTED.contains("not supported in the Rust port"));
    }
}