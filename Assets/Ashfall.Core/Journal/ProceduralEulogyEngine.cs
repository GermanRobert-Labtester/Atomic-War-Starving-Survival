using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using System.Text;

namespace Ashfall.Core.Journal
{
    [Serializable]
    public sealed class DwellerLifeRecord
    {
        public string dwellerId;
        public string dwellerName;
        public string preWarProfession;
        public int daysSurvived;
        public int mealsPrepared;
        public int shiftsCompleted;
        public int radDoseAbsorbedMsv;
        public string causeOfDeath;
        public string favoriteRelicName;
        public List<string> memorableBarkSnippets = new List<string>();
    }

    [Serializable]
    public sealed class EulogySaveState
    {
        public List<string> archivedEulogyTexts = new List<string>();
    }

    /// <summary>
    /// ASHFALL: THE SAGA CHRONICLE — Procedural Eulogy Engine.
    /// Synthesizes bespoke, literary-grade funeral eulogies from a dweller's lifetime logs,
    /// barks, and mechanical sacrifices.
    /// </summary>
    public sealed class ProceduralEulogyEngine
    {
        private readonly List<string> _archivedEulogies = new List<string>();
        public IReadOnlyList<string> ArchivedEulogies => _archivedEulogies;

        public event Action<string, string> OnEulogySpoken;

        public string ComposeEulogy(DwellerLifeRecord life)
        {
            if (life == null) return string.Empty;

            var sb = new StringBuilder();
            sb.AppendLine($"--- MEMORIAL INSCRIPTION: {life.dwellerName.ToUpperInvariant()} ---");
            sb.AppendLine($"Pre-War: {life.preWarProfession}. Survived {life.daysSurvived} days in the cellar.");
            sb.AppendLine();

            // Work record
            if (life.shiftsCompleted > 50)
            {
                sb.AppendLine($"They worked {life.shiftsCompleted} watches without dropping the lantern.");
            }
            else if (life.mealsPrepared > 30)
            {
                sb.AppendLine($"They boiled {life.mealsPrepared} pots of turnip mash and never took the first ladle for themselves.");
            }
            else
            {
                sb.AppendLine($"They took their turn at the air intake when the frost was thick.");
            }

            // Memorable bark quote
            if (life.memorableBarkSnippets != null && life.memorableBarkSnippets.Count > 0)
            {
                string quote = life.memorableBarkSnippets[life.memorableBarkSnippets.Count - 1];
                sb.AppendLine($"We remember what they said by the stove: \"{quote}\"");
            }

            // Physical keepsake & death
            if (!string.IsNullOrEmpty(life.favoriteRelicName))
            {
                sb.AppendLine($"They were buried with {life.favoriteRelicName} tucked into their coat lining.");
            }

            sb.AppendLine($"Cause of Departure: {life.causeOfDeath}.");
            sb.AppendLine("The granite holds the name. The shift goes on.");

            string eulogy = sb.ToString();
            _archivedEulogies.Add(eulogy);
            OnEulogySpoken?.Invoke(life.dwellerId, eulogy);
            return eulogy;
        }

        public EulogySaveState CaptureState()
        {
            return new EulogySaveState
            {
                archivedEulogyTexts = new List<string>(_archivedEulogies)
            };
        }

        public void RestoreState(EulogySaveState state)
        {
            _archivedEulogies.Clear();
            if (state?.archivedEulogyTexts != null)
            {
                _archivedEulogies.AddRange(state.archivedEulogyTexts);
            }
        }
    }
}
