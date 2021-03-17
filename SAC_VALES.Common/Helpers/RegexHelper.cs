using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace SAC_VALES.Common.Helpers
{
    public class RegexHelper : IRegexHelper
    {
        public bool IsValidEmail(string emailaddress)
        {
            try
            {
                new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

    }
}
