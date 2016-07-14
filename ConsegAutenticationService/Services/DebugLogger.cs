using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConsegAutenticationService.Services
{
    public class DebugLogger : Interfaces.ILogger
    {
        public void Log(string message)
        {
           string mess = message;
        }
    }
}