using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Common.Exceptions
{
    public class ConflictException : Exception
    {
        public ConflictException(string name, string message)
            : base($"{name}: {message}")
        {
        }
    }
}
