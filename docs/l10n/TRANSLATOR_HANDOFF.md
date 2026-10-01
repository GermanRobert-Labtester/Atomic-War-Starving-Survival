# Translator handoff

## Source of truth

`assets/l10n/strings.csv` is the authored localization catalog. The `key`, `en`,
`de`, and `source` columns are authoritative; `assets/l10n/template.pot` is a
generated gettext handoff artifact and must not be edited by hand.

## Refresh and verify

After changing the CSV, regenerate and check the template:

```sh
python3 scripts/ci/generate_pot_template.py
python3 scripts/ci/generate_pot_template.py --check
```

The generated POT uses localization keys as `msgid`, English text as
`msgstr`, and preserves CSV source references as `#:` comments. Its header
includes the source-row count. The fast `pot_template_drift` CI gate runs the
same check.

## Review notes

- Keep placeholders such as `{count}` identical in every locale.
- Keep product names, control labels, and established abbreviations consistent.
- German is the second supported locale; do not leave its field blank.
- Review context in the `source` column before translating short labels.
- Add new runtime-composed key families to `DYNAMIC_FAMILIES` in
  `scripts/ci/l10n_drift_gate.py` so missing catalog rows fail the drift gate.
