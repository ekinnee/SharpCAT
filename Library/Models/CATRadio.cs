namespace SharpCAT.Models
{
    //Base Radio Model
    public partial class CATRadio : IRadio
    {
        private string RadioMfg { get; }
        private string RadioModel { get; }

        private string CmdPad { get; } = "00000000";

        private string VFOToggle { get; }
        private Lock Lock { get; }
        private Ptt Ptt { get; }
        private Clar Clar { get; }
        private Split Split { get; }
        private Power Power { get; }
        private ToneMode ToneMode { get; }
        private OpModes OpModes { get; }

        partial void LockOn();

        partial void LockOff();

        partial void PttOn();

        partial void PttOff();

        partial void ClarOn();

        partial void ClarOff();

        partial void SplitOn();

        partial void SplitOff();

        partial void PowerOn();

        partial void PowerOff();

        partial void SetFreq(double freq);

        partial void SetOpMode(OpModes opmode);

        partial void SwitchVFO();

        partial void SetToneMode(ToneMode mode);

        partial void GetRXStatus();

        partial void GetTXStatus();

        partial void GetFreqAndModeStatus();
    }

    public partial class Lock
    {
        public static readonly string ON;
        public static readonly string OFF;
    }

    public class Ptt
    {
        public static readonly string ON;
        public static readonly string OFF;
    }

    public class Clar
    {
        public static readonly string ON;
        public static readonly string OFF;
    }

    public class Split
    {
        public static readonly string ON;
        public static readonly string OFF;
    }

    public class Power
    {
        public static readonly string ON;
        public static readonly string OFF;
    }

    public class ToneMode
    {
        public static readonly string DCS;
        public static readonly string CTCSS;
        public static readonly string ENCODER;
        public static readonly string OFF;
    }

    public class OpModes
    {
        public static readonly string LSB;
        public static readonly string USB;
        public static readonly string CW;
        public static readonly string CWR;
        public static readonly string AM;
        public static readonly string FM;
        public static readonly string DIG;
        public static readonly string PKT;
    }
}