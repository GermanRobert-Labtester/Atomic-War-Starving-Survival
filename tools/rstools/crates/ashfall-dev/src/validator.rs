//! Port of `tools/gotools/pkg/validator/validator.go`.
//!
//! Fast JSON/YAML catalog validator used by the `validate-json` subcommand.
//! Contract reproduced exactly:
//!
//! * violation `rule` identifiers and message wording (`readable`, `utf8_encoding`,
//!   `non_empty`, `object_root`, `valid_json`, `schema_version_present`,
//!   `schema_version_integer`, `schema_version_type`, `valid_yaml`);
//! * `--json` payload shape: `total_checked`, `valid_count`, `invalid_count`,
//!   `duration_ms`, and `violations` **omitted entirely** when there are none
//!   (Go `json:"violations,omitempty"` — not `null`);
//! * exit codes: 0 = clean, 1 = at least one violation (set by the CLI).
//!
//! Documented deltas (unavoidable cross-language differences):
//!
//! * OS read-error text uses the Rust `std::io::Error` rendering, not Go's
//!   `*fs.PathError` rendering (`open <path>: no such file or directory`).
//! * A JSON *syntax* error uses `serde_json`'s position message rather than
//!   Go's `invalid character …` message. The `rule` and exit code are identical.
//! * Violations are collected in lexical walk order. Go fans the files out over
//!   16 goroutines, so its violation order is scheduler-dependent and not
//!   reproducible run-to-run; the port is deterministic.

use crate::gowalk;
use crate::jsonout;
use serde::Serialize;
use std::fs;
use std::path::Path;
use std::time::Instant;

/// Copied from Go: directory names whose subtrees are never validated.
const SKIPPED_DIRS: [&str; 3] = [".git", "bin", "obj"];

#[derive(Debug, Clone, Serialize, PartialEq, Eq)]
pub struct ValidationViolation {
    pub file_path: String,
    pub rule: String,
    pub message: String,
}

#[derive(Debug, Clone, Serialize)]
pub struct ValidationSummary {
    pub total_checked: usize,
    pub valid_count: usize,
    pub invalid_count: usize,
    pub duration_ms: i64,
    /// Go: `json:"violations,omitempty"` — absent (not `null`) when empty.
    #[serde(skip_serializing_if = "Vec::is_empty")]
    pub violations: Vec<ValidationViolation>,
}

impl ValidationSummary {
    /// `json.MarshalIndent(summary, "", "  ")` equivalent, key order included.
    pub fn to_json_pretty(&self) -> String {
        jsonout::to_pretty(self)
    }
}

fn violation(file_path: &str, rule: &str, message: impl Into<String>) -> ValidationViolation {
    ValidationViolation {
        file_path: file_path.to_string(),
        rule: rule.to_string(),
        message: message.into(),
    }
}

/// `filepath.Ext` lowercased: the suffix from the final dot of the file name.
fn go_ext_lower(name: &str) -> String {
    match name.rfind('.') {
        Some(i) => name[i..].to_ascii_lowercase(),
        None => String::new(),
    }
}

/// Go's `%v` for a `float64` (`strconv.FormatFloat(v, 'g', -1, 64)`).
fn go_fmt_f64(v: f64) -> String {
    if v.is_nan() {
        return "NaN".to_string();
    }
    if v.is_infinite() {
        return if v > 0.0 { "+Inf".into() } else { "-Inf".into() };
    }
    let abs = v.abs();
    if v != 0.0 && (abs < 1e-4 || abs >= 1e21) {
        // Go switches to scientific notation here; Rust's `Display` never does.
        if let Some((mantissa, exp)) = format!("{:e}", v).split_once('e') {
            let (sign, digits) = match exp.strip_prefix('-') {
                Some(d) => ("-", d),
                None => ("+", exp.strip_prefix('+').unwrap_or(exp)),
            };
            return format!("{}e{}{:0>2}", mantissa, sign, digits);
        }
    }
    format!("{}", v)
}

/// The JSON token name Go's decoder uses in `cannot unmarshal …` errors.
fn go_json_kind(value: &serde_json::Value) -> &'static str {
    match value {
        serde_json::Value::Null => "null",
        serde_json::Value::Bool(_) => "bool",
        serde_json::Value::Number(_) => "number",
        serde_json::Value::String(_) => "string",
        serde_json::Value::Array(_) => "array",
        serde_json::Value::Object(_) => "object",
    }
}

/// Go's `%T` for a value decoded into `interface{}` by `encoding/json`.
fn go_type_name(value: &serde_json::Value) -> &'static str {
    match value {
        serde_json::Value::Null => "<nil>",
        serde_json::Value::Bool(_) => "bool",
        serde_json::Value::Number(_) => "float64",
        serde_json::Value::String(_) => "string",
        serde_json::Value::Array(_) => "[]interface {}",
        serde_json::Value::Object(_) => "map[string]interface {}",
    }
}

/// Port of `ValidateJSONFile`.
pub fn validate_json_file(path: &str) -> Vec<ValidationViolation> {
    let data = match fs::read(path) {
        Ok(d) => d,
        Err(e) => {
            return vec![violation(
                path,
                "readable",
                format!("Failed to read file: {}", e),
            )]
        }
    };

    // 1. Valid UTF-8 check.
    let text = match std::str::from_utf8(&data) {
        Ok(t) => t,
        Err(_) => {
            return vec![violation(path, "utf8_encoding", "File is not valid UTF-8")];
        }
    };

    let trimmed = text.trim();
    if trimmed.is_empty() {
        return vec![violation(path, "non_empty", "File is empty")];
    }

    // 2. Root must be a JSON object, never a bare array.
    if trimmed.starts_with('[') {
        return vec![violation(
            path,
            "object_root",
            "Root JSON element is an array; must be an object {}",
        )];
    }

    // Go decodes into `map[string]interface{}`: `null` succeeds (nil map), any
    // other non-object scalar fails with a `cannot unmarshal …` error.
    let root: serde_json::Map<String, serde_json::Value> = match serde_json::from_slice(&data) {
        Ok(serde_json::Value::Object(map)) => map,
        Ok(serde_json::Value::Null) => serde_json::Map::new(),
        Ok(other) => {
            return vec![violation(
                path,
                "valid_json",
                format!(
                    "JSON parse error: json: cannot unmarshal {} into Go value of type map[string]interface {{}}",
                    go_json_kind(&other)
                ),
            )]
        }
        Err(e) => {
            return vec![violation(
                path,
                "valid_json",
                format!("JSON parse error: {}", e),
            )]
        }
    };

    // 3. Schema version must exist and be >= 1 for data catalogs.
    match root.get("schema_version") {
        None => vec![violation(
            path,
            "schema_version_present",
            "Missing mandatory 'schema_version' field",
        )],
        Some(serde_json::Value::Number(n)) => {
            let v = n.as_f64().unwrap_or(f64::NAN);
            if !(v >= 1.0) || v.fract() != 0.0 {
                vec![violation(
                    path,
                    "schema_version_integer",
                    format!("schema_version must be a positive integer >= 1, got {}", go_fmt_f64(v)),
                )]
            } else {
                Vec::new()
            }
        }
        Some(other) => vec![violation(
            path,
            "schema_version_type",
            format!("schema_version must be integer, got {}", go_type_name(other)),
        )],
    }
}

/// Port of `ValidateYAMLFile`.
pub fn validate_yaml_file(path: &str) -> Vec<ValidationViolation> {
    let data = match fs::read(path) {
        Ok(d) => d,
        Err(e) => {
            return vec![violation(
                path,
                "readable",
                format!("Failed to read file: {}", e),
            )]
        }
    };

    // The Go original only cares that the document parses; the decoded value is
    // discarded.
    match serde_yaml::from_slice::<serde_yaml::Value>(&data) {
        Ok(_) => Vec::new(),
        Err(e) => vec![violation(
            path,
            "valid_yaml",
            format!("YAML parse error: {}", e),
        )],
    }
}

/// Port of `ValidateDirectory`.
///
/// Sequential and therefore deterministic; Go runs the same per-file checks on
/// 16 goroutines and appends violations in completion order.
pub fn validate_directory(dir: &str) -> Result<ValidationSummary, std::io::Error> {
    let start = Instant::now();

    // Go's `filepath.WalkDir` ignores walk errors (the callback returns nil), so
    // an unreadable subtree — or a missing root — yields an empty result rather
    // than an error.
    let mut files: Vec<String> = Vec::new();
    for entry in gowalk::walk_go(Path::new(dir), |name| !is_skipped_dir(name)) {
        if entry.is_dir {
            continue;
        }
        let ext = go_ext_lower(&entry.path.file_name().unwrap_or_default().to_string_lossy());
        if ext == ".json" || ext == ".yaml" || ext == ".yml" {
            files.push(entry.path.to_string_lossy().into_owned());
        }
    }

    let mut summary = ValidationSummary {
        total_checked: files.len(),
        valid_count: 0,
        invalid_count: 0,
        duration_ms: 0,
        violations: Vec::new(),
    };

    for file in &files {
        // Go dispatches on a case-sensitive ".json" suffix, even though the
        // inclusion filter above lowercased the extension.
        let violations = if file.ends_with(".json") {
            validate_json_file(file)
        } else {
            validate_yaml_file(file)
        };
        if violations.is_empty() {
            summary.valid_count += 1;
        } else {
            summary.invalid_count += 1;
            summary.violations.extend(violations);
        }
    }

    summary.duration_ms = start.elapsed().as_millis() as i64;
    Ok(summary)
}

fn is_skipped_dir(name: &std::ffi::OsStr) -> bool {
    SKIPPED_DIRS.iter().any(|d| name == *d)
}

#[cfg(test)]
mod tests {
    use super::*;
    use std::path::{Path, PathBuf};

    fn tmpdir(tag: &str) -> PathBuf {
        let base = std::env::temp_dir().join(format!(
            "ashfall-rstools-validator-{}-{}",
            tag,
            std::process::id()
        ));
        let _ = fs::remove_dir_all(&base);
        fs::create_dir_all(&base).unwrap();
        base
    }

    fn write(dir: &Path, name: &str, body: &[u8]) -> String {
        let p = dir.join(name);
        fs::write(&p, body).unwrap();
        p.to_string_lossy().into_owned()
    }

    #[test]
    fn valid_catalog_has_no_violations() {
        let dir = tmpdir("ok");
        let f = write(&dir, "valid.json", br#"{"schema_version": 1, "data": "test"}"#);
        assert!(validate_json_file(&f).is_empty());
        let _ = fs::remove_dir_all(&dir);
    }

    #[test]
    fn bare_array_root_is_object_root_violation() {
        let dir = tmpdir("array");
        let f = write(&dir, "array.json", br#"[{"schema_version": 1}]"#);
        let v = validate_json_file(&f);
        assert_eq!(v.len(), 1);
        assert_eq!(v[0].rule, "object_root");
        let _ = fs::remove_dir_all(&dir);
    }

    #[test]
    fn missing_schema_version_is_reported() {
        let dir = tmpdir("nover");
        let f = write(&dir, "no_version.json", br#"{"id": "foo"}"#);
        let v = validate_json_file(&f);
        assert_eq!(v.len(), 1);
        assert_eq!(v[0].rule, "schema_version_present");
        let _ = fs::remove_dir_all(&dir);
    }

    #[test]
    fn empty_non_utf8_and_scalar_roots_are_classified() {
        let dir = tmpdir("misc");
        let empty = write(&dir, "empty.json", b"   \n");
        assert_eq!(validate_json_file(&empty)[0].rule, "non_empty");

        let bad = write(&dir, "bad.json", &[0xff, 0xfe, b'{', b'}']);
        assert_eq!(validate_json_file(&bad)[0].rule, "utf8_encoding");

        let scalar = write(&dir, "scalar.json", b"123");
        let v = validate_json_file(&scalar);
        assert_eq!(v[0].rule, "valid_json");
        assert_eq!(
            v[0].message,
            "JSON parse error: json: cannot unmarshal number into Go value of type map[string]interface {}"
        );

        let stringy = write(&dir, "stringy.json", br#""1""#);
        let v = validate_json_file(&stringy);
        assert_eq!(v[0].rule, "valid_json");
        assert!(v[0].message.ends_with("into Go value of type map[string]interface {}"));
        assert!(v[0].message.contains("unmarshal string"));

        let nul = write(&dir, "null.json", b"null");
        let v = validate_json_file(&nul);
        assert_eq!(v[0].rule, "schema_version_present");
        let _ = fs::remove_dir_all(&dir);
    }

    #[test]
    fn schema_version_type_and_integer_rules() {
        let dir = tmpdir("ver");
        let stringy = write(&dir, "s.json", br#"{"schema_version": "1"}"#);
        let v = validate_json_file(&stringy);
        assert_eq!(v[0].rule, "schema_version_type");
        assert_eq!(v[0].message, "schema_version must be integer, got string");

        let frac = write(&dir, "f.json", br#"{"schema_version": 1.5}"#);
        let v = validate_json_file(&frac);
        assert_eq!(v[0].rule, "schema_version_integer");
        assert_eq!(
            v[0].message,
            "schema_version must be a positive integer >= 1, got 1.5"
        );

        let zero = write(&dir, "z.json", br#"{"schema_version": 0}"#);
        let v = validate_json_file(&zero);
        assert_eq!(v[0].rule, "schema_version_integer");
        assert_eq!(v[0].message, "schema_version must be a positive integer >= 1, got 0");
        let _ = fs::remove_dir_all(&dir);
    }

    #[test]
    fn directory_walk_skips_git_bin_obj_and_counts_files() {
        let dir = tmpdir("dir");
        fs::create_dir_all(dir.join("bin")).unwrap();
        fs::create_dir_all(dir.join(".git")).unwrap();
        write(&dir, "good.json", br#"{"schema_version": 2}"#);
        write(&dir, "notes.yaml", b"a: 1\n");
        write(&dir.join("bin"), "ignored.json", b"{}");
        write(&dir.join(".git"), "ignored.json", b"{}");
        write(&dir, "readme.txt", b"nope");

        let s = validate_directory(&dir.to_string_lossy()).unwrap();
        assert_eq!(s.total_checked, 2);
        assert_eq!(s.valid_count, 2);
        assert_eq!(s.invalid_count, 0);
        // omitempty: the field is absent, not null.
        let json = s.to_json_pretty();
        assert!(!json.contains("violations"), "got: {json}");
        assert_eq!(
            json,
            format!(
                "{{\n  \"total_checked\": 2,\n  \"valid_count\": 2,\n  \"invalid_count\": 0,\n  \"duration_ms\": {}\n}}",
                s.duration_ms
            )
        );
        let _ = fs::remove_dir_all(&dir);
    }

    #[test]
    fn missing_directory_yields_empty_summary_not_error() {
        let missing = std::env::temp_dir().join("ashfall-rstools-validator-does-not-exist");
        let s = validate_directory(&missing.to_string_lossy()).unwrap();
        assert_eq!(s.total_checked, 0);
        assert_eq!(s.valid_count, 0);
        let _ = fs::remove_dir_all(&missing);
    }

    #[test]
    fn invalid_yaml_reports_valid_yaml_rule() {
        let dir = tmpdir("yaml");
        let f = write(&dir, "broken.yml", b"a: [1, 2\n");
        let v = validate_yaml_file(&f);
        assert_eq!(v.len(), 1);
        assert_eq!(v[0].rule, "valid_yaml");
        assert!(v[0].message.starts_with("YAML parse error: "));
        let _ = fs::remove_dir_all(&dir);
    }

    #[test]
    fn go_float_and_ext_helpers_match_go() {
        assert_eq!(go_fmt_f64(0.0), "0");
        assert_eq!(go_fmt_f64(2.0), "2");
        assert_eq!(go_fmt_f64(-3.0), "-3");
        assert_eq!(go_fmt_f64(1.5), "1.5");
        assert_eq!(go_fmt_f64(1e21), "1e+21");
        assert_eq!(go_fmt_f64(1e-7), "1e-07");
        assert_eq!(go_ext_lower("a.b.JSON"), ".json");
        assert_eq!(go_ext_lower(".gitignore"), ".gitignore");
        assert_eq!(go_ext_lower("noext"), "");
    }

    #[test]
    fn violations_keep_go_json_key_order() {
        let v = ValidationViolation {
            file_path: "a.json".into(),
            rule: "readable".into(),
            message: "m".into(),
        };
        assert_eq!(
            jsonout::to_compact(&v),
            r#"{"file_path":"a.json","rule":"readable","message":"m"}"#
        );
    }
}