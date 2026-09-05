using System;

namespace Ashfall.Core.Endgame
{
    /// <summary>
    /// Factory for creating EpilogueEvaluationContext and CampaignOutcomeSnapshot
    /// from live campaign authorities (Plan 19 / FX-01).
    /// </summary>
    public static class EpilogueContextFactory
    {
        public static CampaignOutcomeSnapshot CreateSnapshot(CampaignOutcomeEvaluationInput input)
            => CampaignOutcomeEvaluator.Evaluate(input);

        public static EpilogueEvaluationContext CreateContext(CampaignOutcomeEvaluationInput input)
            => CampaignOutcomeEvaluator.Evaluate(input).ToContext();

        public static EpilogueEvaluationContext CreateContext(CampaignOutcomeSnapshot snapshot)
        {
            if (snapshot == null) throw new ArgumentNullException(nameof(snapshot));
            return snapshot.ToContext();
        }
    }
}
