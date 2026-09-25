import os

path = 'scripts/tools/build_batch227_script.py'
with open(path, 'r', encoding='utf-8') as f:
    c = f.read()

target = '''| **Heat Recuperation** | Integration with subterranean sCO2 thermal loops | Direct counter-flow PCHE cooling returning 600 C reject heat |
""")
\'\'\''''

addition = '''| **Heat Recuperation** | Integration with subterranean sCO2 thermal loops | Direct counter-flow PCHE cooling returning 600 C reject heat |

### 61.7 Subterranean Telemetry Protocol & Real-Time Sensor Telemetry Matrix

To enforce deterministic runtime visibility across holdfast supervisory networks, `{coord}` establishes an immutable,
high-frequency sensor telemetry packet structure emitted at 60 Hz over the internal CAN-FD bus. This telemetry packet
guarantees bit-level serialization parity between server simulation instances and Godot presentation nodes:

```csharp
namespace {ns}
{{{{
    /// <summary>
    /// Binary telemetry packet emitted by MHD generator instrumentation at 60 Hz.
    /// Strictly packed with explicit struct layout for zero-allocation UDP/CAN streaming.
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)]
    public readonly struct MhdTelemetryPacket
    {{{{
        public readonly uint TelemetryFrameIndex;
        public readonly ushort FluidVelocityMmPerSec;
        public readonly ushort TemperatureKelvin;
        public readonly ushort PressureCentibar;
        public readonly ushort TerminalMillivolts;
        public readonly uint OutputMilliamperes;
        public readonly uint EmpSurgeMillijoules;
        public readonly byte ChannelStateFlag;
        public readonly byte ReservedPadding;
        public readonly ulong TelemetryChecksum;

        public MhdTelemetryPacket(
            uint frameIndex,
            double velocityMPerS,
            double tempC,
            double pressureBar,
            double terminalVolts,
            double currentAmperes,
            double empJoules,
            MhdOperationalState state,
            ulong checksum)
        {{{{
            TelemetryFrameIndex = frameIndex;
            FluidVelocityMmPerSec = (ushort)System.Math.Clamp((int)(velocityMPerS * 1000.0), 0, 65535);
            TemperatureKelvin = (ushort)System.Math.Clamp((int)((tempC + 273.15) * 10.0), 0, 65535);
            PressureCentibar = (ushort)System.Math.Clamp((int)(pressureBar * 100.0), 0, 65535);
            TerminalMillivolts = (ushort)System.Math.Clamp((int)(terminalVolts * 1000.0), 0, 65535);
            OutputMilliamperes = (uint)System.Math.Clamp((long)(currentAmperes * 1000.0), 0L, 4294967295L);
            EmpSurgeMillijoules = (uint)System.Math.Clamp((long)(empJoules * 1000.0), 0L, 4294967295L);
            ChannelStateFlag = (byte)state;
            ReservedPadding = 0;
            TelemetryChecksum = checksum;
        }}}}
    }}}}
}}}}
```

```
[MHD SUBSYSTEM INTEGRATION STATE MACHINE]

                +---------------------------------+
                |      ColdStandby (T < 20 C)     |
                +---------------------------------+
                                |
                   (Auxiliary Trace Heating)
                                v
                +---------------------------------+
                |   NominalPumping (20C - 650C)   | <----------------+
                +---------------------------------+                  |
                   |                             |                   |
            (Load Switch Closed)          (HEMP Trigger)       (Surge Decay)
                   v                             v                   |
   +------------------------------+   +------------------------------+
   |   PowerHarvesting (15 kA DC) |   |   EmpBlastSurgeClamping      |
   +------------------------------+   +------------------------------+
                   |                             |
          (Overtemp > 740 C)             (Severe Ground Shock)
                   v                             v
   +------------------------------+   +------------------------------+
   |   ThermalBypassRecuperation  |   |   QuenchDumpSafeIsolation    |
   +------------------------------+   +------------------------------+
```

### 61.8 Long-Term Isotopic Wear & Magnetohydrodynamic Channel Degradation Model

During decade-long subterranean survival campaigns, high-velocity alkali metal flow and neutron irradiation from
adjacent reactor cores cause gradual magnetohydrodynamic duct erosion and contact degradation:

1. **SiC Duct Erosion Velocity Threshold:** Liquid NaK-78 flowing over sintered alpha-SiC ceramic displays zero chemical
   corrosion up to 750 C. However, turbulent boundary shear stresses at fluid velocities exceeding 22.0 m/s induce localized
   micrometer-scale cavitation erosion near the cathode boundary layer. The `{coord}` simulation maintains laminar Hartmann
   flow (`Ha > 450`) to suppress turbulent eddies and cap fluid velocity at 18.0 m/s.
2. **Contact Resistance Growth Curve:** Exposure to trace oxygen contaminants (`> 15 ppm`) forms insoluble potassium superoxide
   (`KO2`) and sodium monoxide (`Na2O`) scales on the tungsten electrode faces. Contact resistance increases according to
   `R_contact(t) = R_0 * (1.0 + 0.045 * (OxygenPpm / 10.0) * (HoursOperating / 1000.0))`.
   Automated zirconium hot-trap getters continuously purify the eutectic inventory to keep oxygen below `2.5 ppm`.
3. **Superconducting Magnet Quench Protection:** If ground shocks deform the cryostat vacuum jacket, liquid nitrogen/helium
   boil-off triggers immediate passive dumping of HTS coil stored energy (`E_mag = 0.5 * L * I^2 = 1.45 MJ`) into an external
   subterranean granite ballast resistor array within `85 milliseconds`, preventing permanent coil thermal destruction.
""")
\'\'\''''

if target in c:
    c = c.replace(target, addition, 1)
    with open(path, 'w', encoding='utf-8') as f:
        f.write(c)
    print('Updated build_batch227_script.py successfully!')
else:
    print('Target still not found!')
