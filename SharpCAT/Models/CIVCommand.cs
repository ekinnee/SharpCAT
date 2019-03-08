using System;
using System.Collections.Generic;
using System.Text;

namespace SharpCATLib.Models
{
    class CIVCommand
    {
        string CmdToRadio = "FE FE 9A E0 Cn Sc Data area FD";
        string DataFromRadio = "FE FE E0 9A Cn Sc Data area FD";
        string OKFromRadio { get; set; }
        string NGFromRadio { get; set; }
    }
}
