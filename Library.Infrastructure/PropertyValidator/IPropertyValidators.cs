using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.PropertyValidator
{
    public interface IPropertyValidators
    {
        public string errNotValidCharacter { get; set; }
        public string errDateNotCorrext { get; set; }
        public string errNotCorretFormat { get; set; }
        public string errIncorectEmail { get; set; }
        public string errIncorectPassword { get; set; }
        public bool PasswordValidator(string password);
        public bool EmailValidator(string email);
        public bool OnlyLettersValidator(string letter);
        public bool OnlyNumbersValidator(string number);
        public bool BeAValidAge(DateTime? date);
        public bool PhoneNumbValidator(string number);
    }
}
