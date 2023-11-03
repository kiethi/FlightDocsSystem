using Azure.Core;
using System;
using System.Text.RegularExpressions;

namespace FlightDocsSystem.Helpers
{
    public class RegexHelper
    {
        public static CheckResult CheckEmail(string email)
        {
            var regexEmail = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(email, regexEmail))
            {
                return new CheckResult(false, "Invalid email: incorrect email format");
            }
            if (!email.EndsWith("@vietjetair.com"))
            {
                return new CheckResult(false, "Invalid email: email extension is not @vietjetair.com");
            }

            return new CheckResult(true, "Valid email");
        }

        public static CheckResult CheckPassword(string password)
        {
            var regexPassword = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,40}$";
            if (!Regex.IsMatch(password, regexPassword))
            {
                return new CheckResult(false, "The password must be between 8 and 40 characters, containing at least 1 lowercase letter, 1 uppercase letter, and 1 numerical digit.");
            }
            return new CheckResult(true, "Valid password");
        }

        public static CheckResult CheckPhoneNumber(string phoneNumber)
        {
            var regexPhoneNumber = @"(((\+|)84)|0)(3|5|7|8|9)+([0-9]{8})\b";
            if (!Regex.IsMatch(phoneNumber, regexPhoneNumber))
            {
                return new CheckResult(false, "Invalid phone number");
            }
            return new CheckResult(true, "Valid phone number");
        }
    }
}
