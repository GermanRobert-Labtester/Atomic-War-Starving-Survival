//! Go-compatible JSON text emission.
//!
//! Go's `encoding/json` (`Marshal`, `MarshalIndent`, `Encoder.Encode`) escapes
//! `<`, `>`, `&`, U+2028, and U+2029 inside strings as `\u003c`, `\u003e`,
//! `\u0026`, `\u2028`, `\u2029` by default (`SetEscapeHTML(true)`).
//! `serde_json` does not. Any ported command whose `--json` payload must be
//! byte-identical to the Go original routes its serialized text through
//! [`escape_html_in_json`] so payloads that contain those characters still
//! match.
//!
//! The substitution is safe to apply to a whole JSON document: structural JSON
//! contains none of those characters outside string literals, and JSON escape
//! sequences (`\u003c`) never contain a literal `<`.

use serde::Serialize;

/// Apply Go's `encoding/json` HTML-safe escaping to already-serialized JSON.
pub fn escape_html_in_json(json: &str) -> String {
    if !json
        .bytes()
        .any(|b| b == b'<' || b == b'>' || b == b'&' || b >= 0x80)
    {
        // Fast path: nothing that could need escaping.
        return json.to_string();
    }
    let mut out = String::with_capacity(json.len() + 16);
    for ch in json.chars() {
        match ch {
            '<' => out.push_str("\\u003c"),
            '>' => out.push_str("\\u003e"),
            '&' => out.push_str("\\u0026"),
            '\u{2028}' => out.push_str("\\u2028"),
            '\u{2029}' => out.push_str("\\u2029"),
            other => out.push(other),
        }
    }
    out
}

/// `json.MarshalIndent(v, "", "  ")` equivalent, including Go's HTML escaping.
pub fn to_pretty<T: Serialize>(value: &T) -> String {
    escape_html_in_json(
        &serde_json::to_string_pretty(value).expect("portable types always serialize"),
    )
}

/// `json.Marshal(v)` equivalent, including Go's HTML escaping.
#[allow(dead_code)]
pub fn to_compact<T: Serialize>(value: &T) -> String {
    escape_html_in_json(&serde_json::to_string(value).expect("portable types always serialize"))
}

/// A `float64` serialized the way Go's `encoding/json` writes one.
///
/// Go's float encoder uses `strconv.FormatFloat(f, 'f', -1, 64)` for magnetudes
/// in `[1e-6, 1e21)`, so an integral value like `1.0` is emitted as `1`, not
/// `1.0`. `serde_json` always emits the latter, so any report field carrying a
/// Go `float64` (e.g. a monitor violation's `confidence`) routes through this
/// newtype to stay byte-identical.
#[derive(Debug, Clone, Copy, PartialEq)]
pub struct GoFloat(pub f64);

impl Serialize for GoFloat {
    fn serialize<S: serde::Serializer>(&self, serializer: S) -> Result<S::Ok, S::Error> {
        // Integral, comfortably within i64: Go's 'f'/-1 rendering drops the
        // fractional part entirely (`1.0` -> `1`).
        if self.0.fract() == 0.0 && self.0.abs() < 9.007_199_254_740_992e15 {
            serializer.serialize_i64(self.0 as i64)
        } else {
            serializer.serialize_f64(self.0)
        }
    }
}

#[cfg(test)]
mod tests {
    use super::*;
    use serde::Serialize;

    #[derive(Serialize)]
    struct Doc {
        text: String,
    }

    #[test]
    fn escapes_html_characters_like_go() {
        let d = Doc {
            text: "a < b && c > d".to_string(),
        };
        assert_eq!(
            to_compact(&d),
            r#"{"text":"a \u003c b \u0026\u0026 c \u003e d"}"#
        );
    }

    #[test]
    fn escapes_line_separators_like_go() {
        let d = Doc {
            text: "x\u{2028}y".to_string(),
        };
        assert_eq!(to_compact(&d), r#"{"text":"x\u2028y"}"#);
    }

    #[test]
    fn leaves_plain_payloads_untouched() {
        assert_eq!(to_pretty(&serde_json::json!({"a": 1})), "{\n  \"a\": 1\n}");
    }

    #[test]
    fn go_float_drops_integral_fraction_like_go() {
        assert_eq!(to_compact(&GoFloat(1.0)), "1");
        assert_eq!(to_compact(&GoFloat(0.8)), "0.8");
        assert_eq!(to_compact(&GoFloat(0.9)), "0.9");
        assert_eq!(to_compact(&GoFloat(0.0)), "0");
    }
}