//! Port of `tools/gotools/pkg/scanner/scanner.go`.
//!
//! Save-file / save-store contract scanner, used by the `scan-saves [--root R]
//! [--save-dir D] [--json]` subcommand. Reproduces the three discovery regexes,
//! the `bin`/`obj` + `Test`-named-file exclusions, the `.json`-only save-file
//! walk, SHA-256 computation, the `schema_version` extraction (non-object
//! documents and non-numeric versions become 0 => `MISSING_SCHEMA_VERSION`), and
//! the exact `--json` shape:
//!
//! * `discovered_stores` and `scanned_files` are `null` (not `[]`/absent) when
//!   empty, matching Go's nil-slice marshalling;
//! * field order `class_name`, `source_file`, `section_name`, `save_path`,
//!   `has_checksum` and `file_path`, `section_name`, `schema_version`,
//!   `byte_size`, `checksum_valid`, `computed_sha256`, `status`.
//!
//! Upstream `ScanSaveDirectory` also returns an `error` that is never non-nil;
//! the port returns only the results.

use crate::gowalk;
use crate::jsonout;
use regex::Regex;
use serde::Serialize;
use sha2::{Digest, Sha256};
use std::fs;
use std::path::Path;
use std::sync::OnceLock;

#[derive(Debug, Clone, Serialize, PartialEq, Eq)]
pub struct SaveStoreDeclaration {
    pub class_name: String,
    pub source_file: String,
    pub section_name: String,
    pub save_path: String,
    pub has_checksum: bool,
}

#[derive(Debug, Clone, Serialize, PartialEq, Eq)]
pub struct SaveFileScanResult {
    pub file_path: String,
    pub section_name: String,
    pub schema_version: i64,
    pub byte_size: i64,
    pub checksum_valid: bool,
    pub computed_sha256: String,
    pub status: String,
}

#[derive(Debug, Clone)]
pub struct SaveAuditReport {
    pub discovered_stores: Vec<SaveStoreDeclaration>,
    pub scanned_files: Vec<SaveFileScanResult>,
    pub store_count: usize,
    pub file_count: usize,
}

/// Go nils slices out to `null`; `Option::None` reproduces that exactly.
#[derive(Serialize)]
struct SaveAuditReportJson<'a> {
    discovered_stores: Option<&'a Vec<SaveStoreDeclaration>>,
    scanned_files: Option<&'a Vec<SaveFileScanResult>>,
    store_count: usize,
    file_count: usize,
}

impl SaveAuditReport {
    /// `json.MarshalIndent(report, "", "  ")` equivalent.
    pub fn to_json_pretty(&self) -> String {
        let view = SaveAuditReportJson {
            discovered_stores: if self.discovered_stores.is_empty() {
                None
            } else {
                Some(&self.discovered_stores)
            },
            scanned_files: if self.scanned_files.is_empty() {
                None
            } else {
                Some(&self.scanned_files)
            },
            store_count: self.store_count,
            file_count: self.file_count,
        };
        jsonout::to_pretty(&view)
    }
}

fn class_re() -> &'static Regex {
    static RE: OnceLock<Regex> = OnceLock::new();
    RE.get_or_init(|| {
        Regex::new(r#"public\s+(?:static\s+|sealed\s+)?class\s+([A-Za-z0-9_]*SaveStore[A-Za-z0-9_]*|[A-Za-z0-9_]+)\b"#)
            .expect("valid class regex")
    })
}

fn section_re() -> &'static Regex {
    static RE: OnceLock<Regex> = OnceLock::new();
    RE.get_or_init(|| {
        Regex::new(r#"public\s+const\s+string\s+SectionName\s*=\s*"([^"]+)""#)
            .expect("valid SectionName regex")
    })
}

fn save_path_re() -> &'static Regex {
    static RE: OnceLock<Regex> = OnceLock::new();
    RE.get_or_init(|| Regex::new(r#"(?:SavePath|FileName)\s*=\s*"([^"]+)""#).expect("valid SavePath regex"))
}

fn to_slash(s: &str) -> String {
    s.replace('\\', "/")
}

/// `filepath.Join`: concatenate and then `Clean`, so `.` segments disappear
/// (`Join(".", "saves") == "saves"`).
pub fn go_join(a: &str, b: &str) -> String {
    let joined = format!("{}/{}", a.trim_end_matches('/'), b);
    let mut parts: Vec<&str> = Vec::new();
    for segment in joined.split('/') {
        match segment {
            "" | "." => {}
            other => parts.push(other),
        }
    }
    let out = parts.join("/");
    if joined.starts_with('/') {
        format!("/{}", out)
    } else if out.is_empty() {
        ".".to_string()
    } else {
        out
    }
}

/// Port of `DiscoverSaveStores`.
pub fn discover_save_stores(repo_root: &str) -> Vec<SaveStoreDeclaration> {
    let mut stores: Vec<SaveStoreDeclaration> = Vec::new();

    let search_dirs = [
        Path::new(repo_root).join("src"),
        Path::new(repo_root).join("Assets").join("Ashfall.Core"),
    ];

    for dir in search_dirs {
        for entry in gowalk::walk_go(&dir, |name| !is_build_dir(name)) {
            if entry.is_dir {
                continue;
            }
            let name = entry
                .path
                .file_name()
                .map(|n| n.to_string_lossy().into_owned())
                .unwrap_or_default();
            if !name.ends_with(".cs") || name.contains("Test") {
                continue;
            }
            let content = match fs::read(&entry.path) {
                Ok(c) => c,
                Err(_) => continue,
            };
            let text = String::from_utf8_lossy(&content);
            if !text.contains("SaveStore") && !(text.contains("SectionName") && text.contains("SavePath")) {
                continue;
            }

            let rel = entry
                .path
                .strip_prefix(repo_root)
                .map(|p| p.to_string_lossy().into_owned())
                .unwrap_or_else(|_| entry.path.to_string_lossy().into_owned());

            let class_name = class_re()
                .captures(&text)
                .map(|c| c[1].to_string())
                .unwrap_or_default();
            let section_name = section_re()
                .captures(&text)
                .map(|c| c[1].to_string())
                .unwrap_or_default();
            let save_path = save_path_re()
                .captures(&text)
                .map(|c| c[1].to_string())
                .unwrap_or_default();

            if !class_name.is_empty()
                && (!section_name.is_empty()
                    || !save_path.is_empty()
                    || class_name.ends_with("SaveStore"))
            {
                stores.push(SaveStoreDeclaration {
                    class_name,
                    source_file: to_slash(&rel),
                    section_name,
                    save_path,
                    has_checksum: text.contains("Checksum") || text.contains("checksum"),
                });
            }
        }
    }

    stores
}

fn is_build_dir(name: &std::ffi::OsStr) -> bool {
    name == "bin" || name == "obj"
}

/// Port of `ScanSaveDirectory`.
pub fn scan_save_directory(save_dir: &str) -> Vec<SaveFileScanResult> {
    let mut results: Vec<SaveFileScanResult> = Vec::new();

    // `os.Stat` + `os.IsNotExist`: a missing directory is an empty scan, not an
    // error.
    if let Err(e) = fs::metadata(save_dir) {
        if e.kind() == std::io::ErrorKind::NotFound {
            return results;
        }
    }

    for entry in gowalk::walk_go(Path::new(save_dir), |_| true) {
        if entry.is_dir {
            continue;
        }
        let name = entry
            .path
            .file_name()
            .map(|n| n.to_string_lossy().into_owned())
            .unwrap_or_default();
        if !name.ends_with(".json") {
            continue;
        }
        let data = match fs::read(&entry.path) {
            Ok(d) => d,
            Err(_) => continue,
        };

        let hash = Sha256::digest(&data);
        let hash_hex: String = hash.iter().map(|b| format!("{:02x}", b)).collect();

        // Go decodes into `map[string]interface{}`; any failure leaves the map
        // nil and the version at 0.
        let mut schema_version: i64 = 0;
        if let Ok(serde_json::Value::Object(map)) = serde_json::from_slice::<serde_json::Value>(&data)
        {
            if let Some(serde_json::Value::Number(n)) = map.get("schema_version") {
                schema_version = n.as_f64().unwrap_or(0.0) as i64;
            }
        }

        let status = if schema_version == 0 {
            "MISSING_SCHEMA_VERSION"
        } else {
            "OK"
        };

        results.push(SaveFileScanResult {
            file_path: entry.path.to_string_lossy().into_owned(),
            section_name: String::new(),
            schema_version,
            byte_size: data.len() as i64,
            checksum_valid: false,
            computed_sha256: hash_hex,
            status: status.to_string(),
        });
    }

    results
}

#[cfg(test)]
mod tests {
    use super::*;
    use std::path::PathBuf;

    fn tmpdir(tag: &str) -> PathBuf {
        let base = std::env::temp_dir().join(format!(
            "ashfall-rstools-scanner-{}-{}",
            tag,
            std::process::id()
        ));
        let _ = fs::remove_dir_all(&base);
        fs::create_dir_all(&base).unwrap();
        base
    }

    #[test]
    fn discards_nil_slices_as_null() {
        let report = SaveAuditReport {
            discovered_stores: Vec::new(),
            scanned_files: Vec::new(),
            store_count: 0,
            file_count: 0,
        };
        assert_eq!(
            report.to_json_pretty(),
            "{\n  \"discovered_stores\": null,\n  \"scanned_files\": null,\n  \"store_count\": 0,\n  \"file_count\": 0\n}"
        );
    }

    #[test]
    fn save_file_result_keeps_go_key_order_and_null_style() {
        let r = SaveFileScanResult {
            file_path: "saves/a.json".into(),
            section_name: String::new(),
            schema_version: 3,
            byte_size: 10,
            checksum_valid: false,
            computed_sha256: "ab".repeat(32),
            status: "OK".into(),
        };
        let json = jsonout::to_compact(&r);
        assert!(json.starts_with(
            r#"{"file_path":"saves/a.json","section_name":"","schema_version":3,"byte_size":10,"checksum_valid":false,"computed_sha256":"#
        ));
        assert!(json.ends_with(r#","status":"OK"}"#));
    }

    #[test]
    fn scan_directory_reads_schema_version_and_hashes() {
        let dir = tmpdir("scan");
        fs::write(dir.join("slot1.json"), br#"{"schema_version": 2}"#).unwrap();
        fs::write(dir.join("slot2.json"), br#"{"other": 1}"#).unwrap();
        fs::write(dir.join("slot3.json"), b"not json").unwrap();
        fs::write(dir.join("notes.txt"), b"ignored").unwrap();

        let results = scan_save_directory(&dir.to_string_lossy());
        assert_eq!(results.len(), 3);
        // Walk is lexical: slot1, slot2, slot3.
        assert_eq!(results[0].schema_version, 2);
        assert_eq!(results[0].status, "OK");
        assert_eq!(results[0].byte_size, 21);
        assert_eq!(results[0].computed_sha256.len(), 64);
        assert!(!results[0].checksum_valid);
        assert_eq!(results[0].section_name, "");
        assert_eq!(results[1].status, "MISSING_SCHEMA_VERSION");
        assert_eq!(results[2].status, "MISSING_SCHEMA_VERSION");
        let _ = fs::remove_dir_all(&dir);
    }

    #[test]
    fn missing_save_directory_is_an_empty_scan() {
        let missing = std::env::temp_dir().join("ashfall-rstools-scanner-absent");
        assert!(scan_save_directory(&missing.to_string_lossy()).is_empty());
    }

    #[test]
    fn discovers_save_stores_and_skips_tests_and_build_dirs() {
        let repo = tmpdir("discover");
        let src = repo.join("src").join("Save");
        fs::create_dir_all(&src).unwrap();
        fs::write(
            src.join("InventorySaveStore.cs"),
            r#"
public sealed class InventorySaveStore {
    public const string SectionName = "inventory";
    public const string SavePath = "saves/inventory.json";
}
"#,
        )
        .unwrap();
        fs::write(
            src.join("ThingStore.cs"),
            r#"
public class ThingStore {
    public const string SectionName = "thing";
}
"#,
        )
        .unwrap();
        fs::write(src.join("SaveStoreTests.cs"), "public class SaveStoreTests {}\n").unwrap();
        let bin = repo.join("src").join("bin");
        fs::create_dir_all(&bin).unwrap();
        fs::write(
            bin.join("GeneratedSaveStore.cs"),
            "public class GeneratedSaveStore { }\n",
        )
        .unwrap();

        let stores = discover_save_stores(&repo.to_string_lossy());
        assert_eq!(stores.len(), 1, "got {stores:#?}");
        assert_eq!(stores[0].class_name, "InventorySaveStore");
        assert_eq!(stores[0].source_file, "src/Save/InventorySaveStore.cs");
        assert_eq!(stores[0].section_name, "inventory");
        assert_eq!(stores[0].save_path, "saves/inventory.json");
        assert!(!stores[0].has_checksum);
        let _ = fs::remove_dir_all(&repo);
    }

    #[test]
    fn checksum_flag_detects_checksum_mention() {
        let repo = tmpdir("checksum");
        let src = repo.join("src");
        fs::create_dir_all(&src).unwrap();
        fs::write(
            src.join("RosterSaveStore.cs"),
            "public class RosterSaveStore { public const string SectionName = \"roster\"; public int Checksum; }\n",
        )
        .unwrap();
        let stores = discover_save_stores(&repo.to_string_lossy());
        assert_eq!(stores.len(), 1);
        assert!(stores[0].has_checksum);
        let _ = fs::remove_dir_all(&repo);
    }
}