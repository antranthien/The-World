using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWorld2.Services
{
    public interface IMailService
    {
        bool SendMail(string to, string form, string subject, string body);
    }
}
