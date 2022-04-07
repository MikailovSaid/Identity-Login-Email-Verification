using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFramework.Exceptions
{
    public class AppDbException : Exception
    {
        public string errorMessage = "Have Problem related to database";
         public AppDbException(string message) : base(message)
        {
          
        }
    }
}
