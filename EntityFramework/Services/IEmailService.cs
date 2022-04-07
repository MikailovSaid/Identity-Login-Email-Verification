using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFramework.Services
{
    public interface IEmailService
    {   
        Task SendEmai(string emailAddress, string url);
    }
}
