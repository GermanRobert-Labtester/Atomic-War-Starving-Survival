using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Ashfall.Core
{
    /// <summary>
    /// Host-independent integrity hash for save data.
    ///
    /// The previous scheme hashed the pretty-printed JSON *text*, which coupled save validity to
    /// serializer formatting: Unity's <c>JsonUtility</c> indents with 4 spaces and writes a null
    /// string as <c>""</c>, while System.Text.Json indents with 2 and writes <c>null</c>. Same
    /// state, different bytes, different SHA256 — so a save written by one host was hard-rejected
    /// as corrupt by the other, violating the "a save written by one host MUST load in the other"
    /// invariant.
    ///
    /// This hashes the *state* instead: a reflection walk over public instance fields in ordinal
    /// name order, with values written in a self-delimiting invariant-culture form. Nothing about
    /// the result depends on which serializer produced or parsed the object.
    ///
    /// Two normalizations exist specifically to absorb the serializer differences above, and must
    /// not be removed:
    /// <list type="bullet">
    /// <item>a null string hashes identically to <c>""</c></item>
    /// <item>a null collection hashes identically to an empty one</item>
    /// </list>
    /// Without them the *in-memory* objects still differ across hosts after parsing the same file.
    ///
    /// Scope: public instance fields only, matching the save DTOs, which are deliberately all
    /// plain public fields. Fields marked <c>[NonSerialized]</c> are excluded. A private
    /// <c>[SerializeField]</c> field would be serialized by Unity but not covered here.
    /// </summary>
    public static class SaveChecksum
    {
        /// <summary>Guards against a cyclic object graph turning into a stack overflow.</summary>
        public const int MaxDepth = 32;

        /// <summary>
        /// Field skipped at the root, so the hash never has to include (or clear) the slot the
        /// hash itself is written into. Callers do not need to blank it first.
        /// </summary>
        public const string ChecksumFieldName = "Checksum";

        /// <summary>Lowercase hex SHA256 of the canonical form of <paramref name="root"/>.</summary>
        public static string Compute(object root)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(Canonicalize(root));
            using (var sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(bytes);
                var sb = new StringBuilder(hash.Length * 2);
                for (int i = 0; i < hash.Length; i++)
                    sb.Append(hash[i].ToString("x2", CultureInfo.InvariantCulture));
                return sb.ToString();
            }
        }

        /// <summary>
        /// The canonical text the hash is taken over. Public so a failing integrity check can be
        /// diffed field by field instead of compared as two opaque hex strings.
        /// </summary>
        public static string Canonicalize(object root)
        {
            var sb = new StringBuilder(4096);
            WriteValue(sb, root, root == null ? typeof(object) : root.GetType(), 0, true);
            return sb.ToString();
        }

        private static void WriteValue(StringBuilder sb, object value, Type declaredType, int depth, bool isRoot)
        {
            if (value == null)
            {
                WriteNull(sb, declaredType);
                return;
            }

            if (depth > MaxDepth)
                throw new InvalidOperationException(
                    "SaveChecksum: object graph deeper than " + MaxDepth + " levels; this is almost " +
                    "certainly a reference cycle, which save DTOs must not contain.");

            // Order matters: string is IEnumerable, and enums are primitives-by-underlying-type.
            if (value is string s) { WriteString(sb, s); return; }

            Type type = value.GetType();
            if (type.IsEnum)
            {
                // Numeric, not the member name: matches how both serializers persist enums, and
                // means renaming a member without changing its value does not invalidate saves.
                sb.Append('e').Append(Convert.ToInt64(value, CultureInfo.InvariantCulture)
                    .ToString(CultureInfo.InvariantCulture));
                return;
            }

            if (value is bool b) { sb.Append(b ? "b1" : "b0"); return; }

            // G9 / G17 are the shortest round-trip-exact forms, and unlike "R" they are stable
            // across runtimes.
            if (value is float f) { sb.Append('f').Append(f.ToString("G9", CultureInfo.InvariantCulture)); return; }
            if (value is double d) { sb.Append('d').Append(d.ToString("G17", CultureInfo.InvariantCulture)); return; }
            if (value is decimal m) { sb.Append('m').Append(m.ToString(CultureInfo.InvariantCulture)); return; }

            if (type.IsPrimitive)
            {
                sb.Append('i').Append(Convert.ToString(value, CultureInfo.InvariantCulture));
                return;
            }

            if (value is IEnumerable sequence) { WriteSequence(sb, sequence, depth); return; }

            WriteObject(sb, value, type, depth, isRoot);
        }

        private static void WriteNull(StringBuilder sb, Type declaredType)
        {
            // See the class remarks: these two cases are what make the hash survive a round trip
            // through a different serializer.
            if (declaredType == typeof(string)) { WriteString(sb, string.Empty); return; }
            if (declaredType != typeof(object) && typeof(IEnumerable).IsAssignableFrom(declaredType))
            {
                sb.Append("[0:]");
                return;
            }

            sb.Append('~');
        }

        private static void WriteString(StringBuilder sb, string value)
        {
            // Length-prefixed so no string content can imitate the surrounding delimiters.
            sb.Append('s').Append(value.Length.ToString(CultureInfo.InvariantCulture)).Append(':').Append(value);
        }

        private static void WriteSequence(StringBuilder sb, IEnumerable sequence, int depth)
        {
            var items = new StringBuilder();
            int count = 0;
            foreach (object item in sequence)
            {
                items.Append(';');
                WriteValue(items, item, item == null ? typeof(object) : item.GetType(), depth + 1, false);
                count++;
            }

            sb.Append('[').Append(count.ToString(CultureInfo.InvariantCulture)).Append(':').Append(items).Append(']');
        }

        private static void WriteObject(StringBuilder sb, object value, Type type, int depth, bool isRoot)
        {
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            Array.Sort(fields, CompareByName);

            sb.Append('{');
            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                // Attribute lookup rather than FieldInfo.IsNotSerialized, which is obsolete from
                // .NET 5 (SYSLIB0050) because it is tied to formatter-based serialization.
                if (Attribute.IsDefined(field, typeof(NonSerializedAttribute))) continue;
                if (isRoot && field.Name == ChecksumFieldName) continue;

                // The name is part of the hash, so two fields swapping values is detected.
                sb.Append(field.Name).Append('=');
                WriteValue(sb, field.GetValue(value)!, field.FieldType, depth + 1, false);
                sb.Append(',');
            }

            sb.Append('}');
        }

        private static int CompareByName(FieldInfo a, FieldInfo b) =>
            string.CompareOrdinal(a.Name, b.Name);
    }
}
