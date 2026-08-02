using System;
using System.Collections.Generic;
using System.Reflection;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Radiation
{
    /// <summary>
    /// Environmental and protective context for a radiation exposure calculation tick:
    /// zone dose rate (0 outside), optional Shelter rad query callback, and gear currently worn.
    /// Pure data container; no MonoBehaviours.
    /// </summary>
    public class ExposureContext
    {
        /// <summary>Ambient radiation rate of the current location (rads/hr).</summary>
        public float ZoneRadLevel;

        /// <summary>Shelter shielding efficiency (0..1 fraction of rads blocked).</summary>
        public float ShelterShielding;

        /// <summary>Optional delegate returning interior rads/hr given ambient zone rads.</summary>
        public Func<float, float> ShelterRadQuery;

        private object _shelterObj;
        public object Shelter
        {
            get => _shelterObj;
            set
            {
                _shelterObj = value;
                if (value != null)
                {
                    var method = value.GetType().GetMethod("GetInteriorRadsPerHour", BindingFlags.Public | BindingFlags.Instance);
                    if (method != null)
                    {
                        ShelterRadQuery = zone => (float)method.Invoke(value, new object[] { zone });
                    }
                }
                else
                {
                    ShelterRadQuery = null;
                }
            }
        }

        /// <summary>Snapshot of protective gear items currently equipped on the survivor.</summary>
        public List<WornGear> WornGear = new List<WornGear>();
    }
}
