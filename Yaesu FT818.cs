namespace SharpCAT.Yaesu
{
    internal class FT818
    {
        public struct LOCK
        {
            private static readonly string ON = "00";
            private static readonly string OFF = "80";
        }

        public struct PTT
        {
            private static readonly string ON = "08";
            private static readonly string OFF = "88";
        }

        public struct CLAR
        {
            private static readonly string ON = "05";
            private static readonly string OFF = "85";
        }

        public struct SPLIT
        {
            private static readonly string ON = "02";
            private static readonly string OFF = "82";
        }

        public struct POWER
        {
            private static readonly string ON = "0F";
            private static readonly string OFF = "8F";
        }

        public struct OpModes
        {
            private static readonly string LSB = "00";
            private static readonly string USB = "01";
            private static readonly string CW = "02";
            private static readonly string CWR = "03";
            private static readonly string AM = "04";
            private static readonly string FM = "08";
            private static readonly string DIG = "0A";
            private static readonly string PKT = "0C";
        }

        public void SetLock()
        {
        }

        public void SetPTT()
        {
        }

        public void SetCLAR()
        {
        }

        public void SetSPLIT()
        {
        }

        public void SetPOWER()
        {
        }

        public void SetFreq(double freq)
        {
        }

        public void SetOpMode(string opmode)
        {
        }

        public void SetVFO()
        {
        }
    }
}