using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conseg.Administracao.Domain.Core
{
    public enum MessageType
    {
        Primary = 10,
        Info = 20,
        Success = 30,
        Warning = 40,
        Danger = 50,
        Error = 60,
        FatalError = 70
    }
}
