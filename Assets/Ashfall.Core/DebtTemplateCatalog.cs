using System;
using System.Collections.Generic;

#pragma warning disable CS8618

namespace Ashfall.Core
{
    /// <summary>Debt template definition loaded from ledger_debt_templates.json.</summary>
    [Serializable]
    public class DebtTemplate
    {
        public string id;
        public string creditorId;
        public string principalItemId;
        public int principalQuantity;
        public int termDays;
        public float rate;
        public string forfeitDescription;
        public string consequenceId;
        public string displayName;
        public string description;
    }

    /// <summary>Default/forfeit consequence definition loaded from ledger_debt_templates.json.</summary>
    [Serializable]
    public class DebtConsequence
    {
        public string id;
        public string trigger;
        public string effectType;
        public string targetFactionId;
        public int standingDelta;
        public string embargoScope;
        public int embargoDurationDays;
        public string bountyLevel;
        public string collateralItemId;
        public int laborDays;
        public string escalationId;
        public string displayName;
        public string description;
    }

    /// <summary>Root JSON envelope for ledger_debt_templates.json.</summary>
    [Serializable]
    public class DebtTemplateFile
    {
        public int schema_version;
        public List<DebtTemplate> templates;
        public List<DebtConsequence> consequences;
    }

    /// <summary>Catalog load result for debt templates.</summary>
    public class DebtTemplateCatalog
    {
        public const string FileName = "ledger_debt_templates.json";
        public const int CurrentSchemaVersion = 1;

        public List<string> Errors { get; } = new List<string>();
        public List<DebtTemplate> Templates { get; } = new List<DebtTemplate>();
        public List<DebtConsequence> Consequences { get; } = new List<DebtConsequence>();

        public DebtTemplate? GetTemplate(string templateId)
        {
            if (string.IsNullOrEmpty(templateId)) return null;
            for (int i = 0; i < Templates.Count; i++)
            {
                if (Templates[i].id == templateId) return Templates[i];
            }
            return null;
        }

        public DebtConsequence? GetConsequence(string consequenceId)
        {
            if (string.IsNullOrEmpty(consequenceId)) return null;
            for (int i = 0; i < Consequences.Count; i++)
            {
                if (Consequences[i].id == consequenceId) return Consequences[i];
            }
            return null;
        }
    }

    /// <summary>Loads ledger_debt_templates.json from StreamingAssets/Data.</summary>
    public static class DebtTemplateCatalogLoader
    {
        public static DebtTemplateCatalog Load(string dataDirectory, IFileIO files, IJsonSerializer json)
        {
            var catalog = new DebtTemplateCatalog();
            string path = files.Combine(dataDirectory, DebtTemplateCatalog.FileName);

            if (!files.FileExists(path))
            {
                catalog.Errors.Add("missing " + DebtTemplateCatalog.FileName + " in " + dataDirectory);
                return catalog;
            }

            DebtTemplateFile file;
            try
            {
                file = json.Deserialize<DebtTemplateFile>(files.ReadAllText(path)!);
            }
            catch (Exception e)
            {
                catalog.Errors.Add(DebtTemplateCatalog.FileName + " parse failed: " + e.Message);
                return catalog;
            }

            if (file == null)
            {
                catalog.Errors.Add(DebtTemplateCatalog.FileName + " deserialized to null");
                return catalog;
            }

            if (file.schema_version > DebtTemplateCatalog.CurrentSchemaVersion)
            {
                catalog.Errors.Add(DebtTemplateCatalog.FileName + " schema_version " + file.schema_version + " > supported " + DebtTemplateCatalog.CurrentSchemaVersion);
                return catalog;
            }

            // Validate templates
            var templateIds = new HashSet<string>();
            if (file.templates != null)
            {
                for (int i = 0; i < file.templates.Count; i++)
                {
                    var t = file.templates[i];
                    if (t == null) { catalog.Errors.Add("template[" + i + "] is null"); continue; }
                    if (string.IsNullOrEmpty(t.id)) { catalog.Errors.Add("template[" + i + "] missing id"); continue; }
                    if (!templateIds.Add(t.id)) { catalog.Errors.Add("duplicate template id: " + t.id); continue; }
                    if (string.IsNullOrEmpty(t.creditorId)) { catalog.Errors.Add(t.id + ": missing creditorId"); }
                    if (string.IsNullOrEmpty(t.principalItemId)) { catalog.Errors.Add(t.id + ": missing principalItemId"); }
                    if (t.principalQuantity <= 0) { catalog.Errors.Add(t.id + ": principalQuantity must be > 0"); }
                    if (t.termDays <= 0) { catalog.Errors.Add(t.id + ": termDays must be > 0"); }
                    if (t.rate < 0f) { catalog.Errors.Add(t.id + ": rate must be >= 0"); }
                    if (string.IsNullOrEmpty(t.forfeitDescription)) { catalog.Errors.Add(t.id + ": missing forfeitDescription"); }
                    if (string.IsNullOrEmpty(t.consequenceId)) { catalog.Errors.Add(t.id + ": missing consequenceId"); }
                    catalog.Templates.Add(t);
                }
            }

            // Validate consequences
            var consequenceIds = new HashSet<string>();
            if (file.consequences != null)
            {
                for (int i = 0; i < file.consequences.Count; i++)
                {
                    var c = file.consequences[i];
                    if (c == null) { catalog.Errors.Add("consequence[" + i + "] is null"); continue; }
                    if (string.IsNullOrEmpty(c.id)) { catalog.Errors.Add("consequence[" + i + "] missing id"); continue; }
                    if (!consequenceIds.Add(c.id)) { catalog.Errors.Add("duplicate consequence id: " + c.id); continue; }
                    if (string.IsNullOrEmpty(c.trigger)) { catalog.Errors.Add(c.id + ": missing trigger"); }
                    if (string.IsNullOrEmpty(c.effectType)) { catalog.Errors.Add(c.id + ": missing effectType"); }
                    catalog.Consequences.Add(c);
                }
            }

            // Cross-reference: every template consequenceId must resolve
            for (int i = 0; i < catalog.Templates.Count; i++)
            {
                var t = catalog.Templates[i];
                if (!string.IsNullOrEmpty(t.consequenceId) && !consequenceIds.Contains(t.consequenceId))
                {
                    catalog.Errors.Add(t.id + ": consequenceId " + t.consequenceId + " not found in consequences");
                }
            }

            // Cross-reference: every consequence escalationId must resolve
            for (int i = 0; i < catalog.Consequences.Count; i++)
            {
                var c = catalog.Consequences[i];
                if (!string.IsNullOrEmpty(c.escalationId) && !consequenceIds.Contains(c.escalationId))
                {
                    catalog.Errors.Add(c.id + ": escalationId " + c.escalationId + " not found in consequences");
                }
            }

            return catalog;
        }
    }
}
