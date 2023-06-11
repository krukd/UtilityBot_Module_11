using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityBot_Module_11.Services
{
    internal interface ITextMessageHandler
    {
        string Process(string message, string mode);
    }
}
