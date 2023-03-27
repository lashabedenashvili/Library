using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Library.Infrastructure.PropertyValidator
{
    public class PropertyValidators : IPropertyValidators
    {
        public string errNotValidCharacter { get; set; } = "Not Valid Character";
        public string errDateNotCorrext { get; set; } = "Date Format Or Emount Is Not Correct";
        public string errNotCorretFormat { get; set; } = "Not Correct Format";
        public string errIncorectEmail { get; set; } = "Incorect Email Format";
        public string errIncorectPassword { get; set; } = "Password Contains at least 8 character, included special symbol, number, capital letter";

        public bool EmailValidator(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;
            return Regex.IsMatch(email, @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z");
        }
        public bool PasswordValidator(string password)
        {
            if (string.IsNullOrEmpty(password)) return false;
            return Regex.IsMatch(password, "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
        }
        public bool OnlyLettersValidator(string letter)
        {
            if (string.IsNullOrEmpty(letter)) return false;
            return Regex.IsMatch(letter, "^[A-Za-z ]+$");
        }
        public bool OnlyNumbersValidator(string number)
        {
            if (string.IsNullOrEmpty(number)) return false;
            return Regex.IsMatch(number, "^[0-9]+$");
        }
        public bool BeAValidAge(DateTime? date)
        {
            if (date == null) return false;
            int currentYear = DateTime.Now.Year;
            int dobYear = date.Value.Year;
            return dobYear <= currentYear && dobYear > (currentYear - 120);
        }
        public bool PhoneNumbValidator(string number)
        {
            if (string.IsNullOrEmpty(number)) return false;
            return Regex.IsMatch(number, "\\+?[1-9][0-9]{7,14}");
        }
    }
}
