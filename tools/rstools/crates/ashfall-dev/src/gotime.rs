//! `time.Time` JSON formatting parity.
//!
//! Go marshals a `time.Time` as RFC 3339 with nanosecond precision and trailing
//! zeros in the fraction removed (`time.RFC3339Nano`), using `Z` for a
//! zero offset and the value's own zone offset otherwise. `IndexReport` and
//! `AssetManifest` embed `time.Now().UTC()`, and `index --json` embeds
//! `os.FileInfo.ModTime()`, which Go reports in the *local* zone.
//!
//! This module reproduces that text exactly from a Unix timestamp, so the port
//! does not need a date crate. The local offset is read from libc's
//! `localtime_r`, which is what Go's `time.Local` consults as well; on targets
//! where that is unavailable the offset falls back to UTC.

use std::time::{SystemTime, UNIX_EPOCH};

/// Convert Unix seconds to a civil UTC/zone-local date-time
/// (Howard Hinnant's `civil_from_days`).
fn civil_from_unix(secs: i64) -> (i64, u32, u32, u32, u32, u32) {
    let days = secs.div_euclid(86_400);
    let secs_of_day = secs.rem_euclid(86_400);
    let (hour, minute, second) = (
        (secs_of_day / 3_600) as u32,
        ((secs_of_day % 3_600) / 60) as u32,
        (secs_of_day % 60) as u32,
    );

    let z = days + 719_468;
    let era = z.div_euclid(146_097);
    let doe = z.rem_euclid(146_097); // [0, 146096]
    let yoe = (doe - doe / 1_460 + doe / 36_524 - doe / 146_096) / 365; // [0, 399]
    let year = yoe + era * 400;
    let doy = doe - (365 * yoe + yoe / 4 - yoe / 100); // [0, 365]
    let mp = (5 * doy + 2) / 153; // [0, 11]
    let day = (doy - (153 * mp + 2) / 5 + 1) as u32; // [1, 31]
    let month = if mp < 10 { mp + 3 } else { mp - 9 } as u32; // [1, 12]
    (
        year + i64::from(month <= 2),
        month,
        day,
        hour,
        minute,
        second,
    )
}

/// `time.Time` -> `RFC3339Nano` text.
pub fn format_rfc3339(epoch_secs: i64, nanos: u32, offset_secs: i64) -> String {
    let (year, month, day, hour, minute, second) = civil_from_unix(epoch_secs + offset_secs);
    let mut out = format!(
        "{:04}-{:02}-{:02}T{:02}:{:02}:{:02}",
        year, month, day, hour, minute, second
    );
    if nanos != 0 {
        out.push('.');
        out.push_str(format!("{:09}", nanos).trim_end_matches('0'));
    }
    if offset_secs == 0 {
        out.push('Z');
    } else {
        let sign = if offset_secs < 0 { '-' } else { '+' };
        let abs = offset_secs.abs();
        out.push_str(&format!("{}{:02}:{:02}", sign, abs / 3_600, (abs % 3_600) / 60));
    }
    out
}

/// Split a `SystemTime` into whole seconds since the Unix epoch and nanoseconds.
pub fn parts_from_system_time(t: SystemTime) -> (i64, u32) {
    match t.duration_since(UNIX_EPOCH) {
        Ok(d) => (d.as_secs() as i64, d.subsec_nanos()),
        Err(e) => {
            // Pre-epoch timestamps: floor to whole seconds.
            let d = e.duration();
            let secs = d.as_secs() as i64;
            let nanos = d.subsec_nanos();
            if nanos == 0 {
                (-secs, 0)
            } else {
                (-secs - 1, 1_000_000_000 - nanos)
            }
        }
    }
}

/// `time.Now().UTC()` as `RFC3339Nano`.
pub fn now_utc_rfc3339() -> String {
    let (secs, nanos) = parts_from_system_time(SystemTime::now());
    format_rfc3339(secs, nanos, 0)
}

/// `t.Local()` for a filesystem modification time, as `RFC3339Nano`.
pub fn system_time_local_rfc3339(t: SystemTime) -> String {
    let (secs, nanos) = parts_from_system_time(t);
    format_rfc3339(secs, nanos, local_offset_secs(secs))
}

/// The local zone's UTC offset (seconds east) at `epoch_secs`.
#[cfg(all(unix, not(target_env = "musl")))]
pub fn local_offset_secs(epoch_secs: i64) -> i64 {
    // SAFETY: `localtime_r` is reentrant and only writes to the out-parameter.
    unsafe {
        let t = epoch_secs as libc::time_t;
        let mut tm: libc::tm = std::mem::zeroed();
        if libc::localtime_r(&t, &mut tm).is_null() {
            return 0;
        }
        tm.tm_gmtoff as i64
    }
}

/// Fallback: no local zone information available, so emit UTC like
/// `time.Now().UTC()`.
#[cfg(any(not(unix), target_env = "musl"))]
pub fn local_offset_secs(_epoch_secs: i64) -> i64 {
    0
}

#[cfg(test)]
mod tests {
    use super::*;

    /// 2023-11-14T22:13:20Z — a widely used anchor value.
    const ANCHOR: i64 = 1_700_000_000;

    #[test]
    fn utc_without_fraction_matches_go() {
        assert_eq!(format_rfc3339(ANCHOR, 0, 0), "2023-11-14T22:13:20Z");
    }

    #[test]
    fn nanosecond_fraction_is_trimmed_like_go() {
        assert_eq!(
            format_rfc3339(ANCHOR, 123_456_789, 0),
            "2023-11-14T22:13:20.123456789Z"
        );
        assert_eq!(
            format_rfc3339(ANCHOR, 123_400_000, 0),
            "2023-11-14T22:13:20.1234Z"
        );
        assert_eq!(
            format_rfc3339(ANCHOR, 1_000_000, 0),
            "2023-11-14T22:13:20.001Z"
        );
    }

    #[test]
    fn non_zero_offset_renders_numeric_zone() {
        assert_eq!(
            format_rfc3339(ANCHOR, 0, 3 * 3_600),
            "2023-11-15T01:13:20+03:00"
        );
        assert_eq!(
            format_rfc3339(ANCHOR, 0, -(5 * 3_600 + 30 * 60)),
            "2023-11-14T16:43:20-05:30"
        );
    }

    #[test]
    fn epoch_and_pre_epoch_boundaries() {
        assert_eq!(format_rfc3339(0, 0, 0), "1970-01-01T00:00:00Z");
        assert_eq!(format_rfc3339(-1, 0, 0), "1969-12-31T23:59:59Z");
        // A leap day, to pin the civil-date conversion.
        assert_eq!(format_rfc3339(1_709_164_800, 0, 0), "2024-02-29T00:00:00Z");
    }

    #[test]
    fn parts_from_system_time_round_trips() {
        let t = UNIX_EPOCH + std::time::Duration::new(ANCHOR as u64, 500);
        assert_eq!(parts_from_system_time(t), (ANCHOR, 500));
        assert_eq!(
            format_rfc3339(ANCHOR, 500, 0),
            "2023-11-14T22:13:20.0000005Z"
        );
    }

    #[test]
    fn now_is_utc_and_well_formed() {
        let s = now_utc_rfc3339();
        assert!(s.ends_with('Z'), "got {s}");
        let body = &s[..s.len() - 1];
        let (secs, frac) = match body.split_once('.') {
            Some((a, b)) => (a, Some(b)),
            None => (body, None),
        };
        assert_eq!(secs.len(), 19, "got {s}");
        assert_eq!(&secs[4..5], "-");
        assert_eq!(&secs[7..8], "-");
        assert_eq!(&secs[10..11], "T");
        assert_eq!(&secs[13..14], ":");
        assert_eq!(&secs[16..17], ":");
        assert!(
            secs.chars().all(|c| c.is_ascii_digit() || "-T:".contains(c)),
            "got {s}"
        );
        // Go trims trailing zeros from the nanosecond fraction.
        if let Some(f) = frac {
            assert!((1..=9).contains(&f.len()), "got {s}");
            assert!(f.chars().all(|c| c.is_ascii_digit()), "got {s}");
            assert!(!f.ends_with('0'), "trailing zeros not trimmed: {s}");
        }
    }
}