using UnityEngine;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// On-screen dosimeter readout: cumulative dose and current dose rate.
    /// Presents a Radiation.Dosimeter snapshot; refreshed by the HUD. Thin MonoBehaviour.
    /// </summary>
    public class DosimeterHUD : MonoBehaviour
    {
        [SerializeField] private RectTransform _readoutRoot;

        /// <summary>Update the readout from a dosimeter snapshot.</summary>
        public void SetReading(float cumulativeDose, float currentRate) => throw new System.NotImplementedException();
    }
}
