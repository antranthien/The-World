using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace TheWorld2.Services
{
    public class DebugMailService : IMailService
    {
        bool IMailService.SendMail(string to, string from, string subject, string body)
        {
            Debug.WriteLine($"Sending email: To: {to}, From: {from}, Subject: {subject}");

            return true;
        }
    }
}
