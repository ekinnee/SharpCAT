namespace SharpCATLib.Models
{
    internal class CIVCommand
    {
        private string CmdToRadio = "FE FE 9A E0 Cn Sc Data area FD";
        private string DataFromRadio = "FE FE E0 9A Cn Sc Data area FD";
        private string OKFromRadio { get; set; }
        private string NGFromRadio { get; set; }
    }
}