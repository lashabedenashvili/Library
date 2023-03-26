using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Library.Data.Domein.Data.Enum;

namespace Library.Infrastructure.Dto.UserDto
{
    public class UserRegistrationDto
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string PrivateNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
