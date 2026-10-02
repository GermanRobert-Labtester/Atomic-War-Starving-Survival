//! Port of `tools/gotools/pkg/manifest/manifest.go`.
//!
//! Asset manifest builder used by the `build-manifest [--root R] [--out P]`
//! subcommand. Reproduces the category table, the `.import` / `.uid` exclusion,
//! SHA-256 per asset, the `id` = file base without extension, and the
//! `built_at` / `total_assets` / `total_bytes` / `assets` payload with
//! `width`/`height` omitted when zero and `assets: null` when empty.
//!
//! Image dimensions: Go registers only `image/png` with `image.DecodeConfig`,
//! which sniffs the *content* rather than the file name. PNG bytes therefore
//! yield dimensions whatever the extension, while `.jpg`/`.jpeg`/`.webp` assets
//! are still categorised `sprite`/`ui` but carry none. The port reads the same
//! PNG header directly and applies the same validity checks the Go png decoder
//! applies (signature, `IHDR` length, IHDR CRC-32, bit depth, colour type,
//! compression/filter/interlace method, and non-zero dimensions), so a file Go
//! would reject yields no dimensions here either.
//!
//! Ordering: Go appends from 8 goroutines, so its asset order is not
//! reproducible. The port walks `--root/assets` then `--root/Assets` in lexical
//! order, which is deterministic.

use crate::gotime;
use crate::gowalk;
use crate::jsonout;
use serde::Serialize;
use sha2::{Digest, Sha256};
use std::fs;
use std::path::Path;

#[derive(Debug, Clone, Serialize)]
pub struct AssetEntry {
    pub id: String,
    pub relative_path: String,
    pub category: String,
    #[serde(skip_serializing_if = "is_zero_i32")]
    pub width: i32,
    #[serde(skip_serializing_if = "is_zero_i32")]
    pub height: i32,
    pub size_bytes: i64,
    pub sha256: String,
}

fn is_zero_i32(v: &i32) -> bool {
    *v == 0
}

#[derive(Debug, Clone)]
pub struct AssetManifest {
    pub built_at: String,
    pub total_assets: usize,
    pub total_bytes: i64,
    pub assets: Vec<AssetEntry>,
}

/// Go nils `assets` out to `null`.
#[derive(Serialize)]
struct AssetManifestJson<'a> {
    built_at: &'a str,
    total_assets: usize,
    total_bytes: i64,
    assets: Option<&'a Vec<AssetEntry>>,
}

impl AssetManifest {
    /// `json.MarshalIndent(m, "", "  ")` equivalent (key order included).
    pub fn to_json_pretty(&self) -> String {
        let view = AssetManifestJson {
            built_at: &self.built_at,
            total_assets: self.total_assets,
            total_bytes: self.total_bytes,
            assets: if self.assets.is_empty() {
                None
            } else {
                Some(&self.assets)
            },
        };
        jsonout::to_pretty(&view)
    }

    /// Port of `SaveJSON`: pretty JSON, no trailing newline.
    pub fn save_json(&self, output_path: &str) -> std::io::Result<()> {
        fs::write(output_path, self.to_json_pretty())
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

/// Port of `categorizeAsset`.
fn categorize_asset(path: &str) -> &'static str {
    match go_ext_lower(path).as_str() {
        ".png" | ".jpg" | ".jpeg" | ".webp" => {
            if path.contains("ui") || path.contains("UI") {
                "ui"
            } else {
                "sprite"
            }
        }
        ".wav" | ".ogg" | ".mp3" => "audio",
        ".ttf" | ".otf" | ".woff" | ".woff2" => "font",
        ".tscn" | ".tres" => "godot_scene",
        ".gdshader" | ".shader" => "shader",
        _ => "other",
    }
}

const PNG_SIGNATURE: [u8; 8] = [0x89, b'P', b'N', b'G', 0x0D, 0x0A, 0x1A, 0x0A];

fn crc32_ieee(data: &[u8]) -> u32 {
    let mut crc = 0xFFFF_FFFFu32;
    for &byte in data {
        crc ^= byte as u32;
        for _ in 0..8 {
            crc = if crc & 1 != 0 {
                (crc >> 1) ^ 0xEDB8_8320
            } else {
                crc >> 1
            };
        }
    }
    !crc
}

/// The equivalent of Go's `png.DecodeConfig`: the image's dimensions if, and
/// only if, the PNG header is fully valid.
fn png_dimensions(bytes: &[u8]) -> Option<(i32, i32)> {
    // signature(8) + length(4) + type(4) + IHDR data(13) + CRC(4)
    if bytes.len() < 33 || bytes[..8] != PNG_SIGNATURE {
        return None;
    }
    let length = u32::from_be_bytes([bytes[8], bytes[9], bytes[10], bytes[11]]);
    if length != 13 || &bytes[12..16] != b"IHDR" {
        return None;
    }
    let body = &bytes[16..29];
    let stored_crc = u32::from_be_bytes([bytes[29], bytes[30], bytes[31], bytes[32]]);
    let mut crc_input = Vec::with_capacity(17);
    crc_input.extend_from_slice(b"IHDR");
    crc_input.extend_from_slice(body);
    if crc32_ieee(&crc_input) != stored_crc {
        return None;
    }

    let width = u32::from_be_bytes([body[0], body[1], body[2], body[3]]);
    let height = u32::from_be_bytes([body[4], body[5], body[6], body[7]]);
    let bit_depth = body[8];
    let color_type = body[9];
    let compression = body[10];
    let filter = body[11];
    let interlace = body[12];

    if !matches!(bit_depth, 1 | 2 | 4 | 8 | 16) {
        return None;
    }
    if !matches!(color_type, 0 | 2 | 3 | 4 | 6) {
        return None;
    }
    if color_type == 3 && bit_depth == 16 {
        return None;
    }
    if compression != 0 || filter != 0 || !matches!(interlace, 0 | 1) {
        return None;
    }
    if width == 0 || height == 0 {
        return None;
    }
    Some((width as i32, height as i32))
}

/// Port of `BuildManifest`.
pub fn build_manifest(repo_root: &str, asset_dirs: &[&str]) -> AssetManifest {
    let mut manifest = AssetManifest {
        built_at: gotime::now_utc_rfc3339(),
        total_assets: 0,
        total_bytes: 0,
        assets: Vec::new(),
    };

    let mut candidates: Vec<std::path::PathBuf> = Vec::new();
    for dir in asset_dirs {
        let full_dir = Path::new(repo_root).join(dir);
        if !full_dir.exists() {
            continue;
        }
        // No directory is pruned: Go's callback only filters `.import`/`.uid`.
        for entry in gowalk::walk_go(&full_dir, |_| true) {
            if entry.is_dir {
                continue;
            }
            let lower = entry
                .path
                .file_name()
                .map(|n| n.to_string_lossy().to_lowercase())
                .unwrap_or_default();
            if lower.ends_with(".import") || lower.ends_with(".uid") {
                continue;
            }
            candidates.push(entry.path);
        }
    }

    for path in candidates {
        let info = match fs::metadata(&path) {
            Ok(i) => i,
            Err(_) => continue,
        };
        let rel = path
            .strip_prefix(repo_root)
            .map(|p| p.to_string_lossy().into_owned())
            .unwrap_or_else(|_| path.to_string_lossy().into_owned());
        let rel_slash = rel.replace('\\', "/");
        let category = categorize_asset(&rel_slash);

        let file_name = path
            .file_name()
            .map(|n| n.to_string_lossy().into_owned())
            .unwrap_or_default();
        // Go: `strings.TrimSuffix(filepath.Base(p), filepath.Ext(p))`.
        let id = match file_name.rfind('.') {
            Some(i) => file_name[..i].to_string(),
            None => file_name.clone(),
        };

        let mut entry = AssetEntry {
            id,
            relative_path: rel_slash,
            category: category.to_string(),
            width: 0,
            height: 0,
            size_bytes: info.len() as i64,
            sha256: String::new(),
        };

        if let Ok(bytes) = fs::read(&path) {
            let hash = Sha256::digest(&bytes);
            entry.sha256 = hash.iter().map(|b| format!("{:02x}", b)).collect();
            if category == "sprite" || category == "ui" {
                if let Some((w, h)) = png_dimensions(&bytes) {
                    entry.width = w;
                    entry.height = h;
                }
            }
        }

        manifest.total_assets += 1;
        manifest.total_bytes += entry.size_bytes;
        manifest.assets.push(entry);
    }

    manifest
}

/// Read a whole file, mirroring `io.Copy(sha256.New(), f)`.
#[cfg(test)]
fn file_sha256(path: &Path) -> Option<String> {
    use std::io::Read;
    let mut f = fs::File::open(path).ok()?;
    let mut hasher = Sha256::new();
    let mut buf = [0u8; 64 * 1024];
    loop {
        let n = f.read(&mut buf).ok()?;
        if n == 0 {
            break;
        }
        hasher.update(&buf[..n]);
    }
    Some(hasher.finalize().iter().map(|b| format!("{:02x}", b)).collect())
}

#[cfg(test)]
mod tests {
    use super::*;
    use std::path::PathBuf;

    fn tmpdir(tag: &str) -> PathBuf {
        let base = std::env::temp_dir().join(format!(
            "ashfall-rstools-manifest-{}-{}",
            tag,
            std::process::id()
        ));
        let _ = fs::remove_dir_all(&base);
        fs::create_dir_all(&base).unwrap();
        base
    }

    /// Build a minimal valid 1x1 PNG with an explicit IHDR CRC.
    fn tiny_png(width: u32, height: u32) -> Vec<u8> {
        let mut out = Vec::new();
        out.extend_from_slice(&PNG_SIGNATURE);
        out.extend_from_slice(&13u32.to_be_bytes());
        out.extend_from_slice(b"IHDR");
        let mut body = Vec::new();
        body.extend_from_slice(&width.to_be_bytes());
        body.extend_from_slice(&height.to_be_bytes());
        body.extend_from_slice(&[8, 6, 0, 0, 0]); // depth, colour, comp, filter, interlace
        out.extend_from_slice(&body);
        let mut crc_input = Vec::new();
        crc_input.extend_from_slice(b"IHDR");
        crc_input.extend_from_slice(&body);
        out.extend_from_slice(&crc32_ieee(&crc_input).to_be_bytes());
        out
    }

    #[test]
    fn categorize_matches_go_table() {
        assert_eq!(categorize_asset("assets/ui/button.png"), "ui");
        assert_eq!(categorize_asset("assets/sprites/tree.png"), "sprite");
        assert_eq!(categorize_asset("assets/buildings/hut.png"), "ui"); // "b-ui-ldings"
        assert_eq!(categorize_asset("assets/audio/step.wav"), "audio");
        assert_eq!(categorize_asset("assets/fonts/mono.ttf"), "font");
        assert_eq!(categorize_asset("assets/scenes/main.tscn"), "godot_scene");
        assert_eq!(categorize_asset("assets/shaders/x.gdshader"), "shader");
        assert_eq!(categorize_asset("assets/readme.txt"), "other");
    }

    #[test]
    fn png_dimensions_require_a_valid_header() {
        assert_eq!(png_dimensions(&tiny_png(64, 32)), Some((64, 32)));
        assert_eq!(png_dimensions(b"not a png"), None);

        let mut corrupt = tiny_png(4, 4);
        let last = corrupt.len() - 1;
        corrupt[last] ^= 0xFF; // break the IHDR CRC
        assert_eq!(png_dimensions(&corrupt), None);

        // Zero dimensions are rejected (Go: "invalid image size").
        assert_eq!(png_dimensions(&tiny_png(0, 4)), None);
    }

    #[test]
    fn builds_manifest_with_hashes_dimensions_and_skips_import_sidecars() {
        let repo = tmpdir("build");
        let assets = repo.join("assets");
        fs::create_dir_all(assets.join("sprites")).unwrap();
        fs::write(assets.join("sprites").join("tree.png"), tiny_png(16, 8)).unwrap();
        fs::write(assets.join("sprites").join("tree.png.import"), b"meta").unwrap();
        // A .jpg is categorised as a sprite, but its bytes are not PNG so Go's
        // content sniffing finds no registered decoder and reports no dimensions.
        fs::write(
            assets.join("sprites").join("photo.jpg"),
            b"\xff\xd8\xff\xe0not-a-real-jpeg",
        )
        .unwrap();
        fs::write(assets.join("notes.txt"), b"hello").unwrap();

        let m = build_manifest(&repo.to_string_lossy(), &["assets", "Assets"]);
        assert_eq!(m.total_assets, 3);
        assert_eq!(m.total_bytes, 33 + 19 + 5);
        assert!(m.built_at.ends_with('Z'));

        let tree = m
            .assets
            .iter()
            .find(|a| a.relative_path == "assets/sprites/tree.png")
            .unwrap();
        assert_eq!(tree.id, "tree");
        assert_eq!(tree.category, "sprite");
        assert_eq!((tree.width, tree.height), (16, 8));
        assert_eq!(tree.sha256.len(), 64);
        assert_eq!(
            tree.sha256,
            file_sha256(&assets.join("sprites").join("tree.png")).unwrap()
        );

        let photo = m
            .assets
            .iter()
            .find(|a| a.relative_path == "assets/sprites/photo.jpg")
            .unwrap();
        assert_eq!(photo.category, "sprite");
        assert_eq!((photo.width, photo.height), (0, 0));

        let notes = m.assets.iter().find(|a| a.relative_path == "assets/notes.txt").unwrap();
        assert_eq!(notes.category, "other");
        assert_eq!((notes.width, notes.height), (0, 0));
        // width/height are omitempty in Go, so zero-valued entries carry no keys.
        let json = m.to_json_pretty();
        assert!(!json.contains("\"width\": 0"), "got {json}");
        assert!(!json.contains("\"height\": 0"), "got {json}");
        assert_eq!(json.matches("\"width\": 16").count(), 1, "got {json}");
        assert!(json.find("\"built_at\"").unwrap() < json.find("\"total_assets\"").unwrap());
        assert!(json.find("\"total_bytes\"").unwrap() < json.find("\"assets\"").unwrap());
        let _ = fs::remove_dir_all(&repo);
    }

    #[test]
    fn missing_asset_dirs_yield_null_assets() {
        let repo = tmpdir("empty");
        let m = build_manifest(&repo.to_string_lossy(), &["assets", "Assets"]);
        assert_eq!(m.total_assets, 0);
        let json = m.to_json_pretty();
        assert!(json.contains("\"assets\": null"), "got {json}");
        assert!(json.contains("\"total_assets\": 0"));
        let _ = fs::remove_dir_all(&repo);
    }

    #[test]
    fn save_json_writes_without_trailing_newline() {
        let repo = tmpdir("save");
        fs::create_dir_all(repo.join("assets")).unwrap();
        fs::write(repo.join("assets").join("a.txt"), b"x").unwrap();
        let m = build_manifest(&repo.to_string_lossy(), &["assets"]);
        let out = repo.join("out").join("m.json");
        fs::create_dir_all(out.parent().unwrap()).unwrap();
        m.save_json(&out.to_string_lossy()).unwrap();
        let written = fs::read(&out).unwrap();
        assert!(!written.ends_with(b"\n"));
        assert_eq!(String::from_utf8(written).unwrap(), m.to_json_pretty());
        let _ = fs::remove_dir_all(&repo);
    }
}