using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Library.Data.Domein.Data.Enum;

namespace Library.Data.Domein.Data
{
    public class UserPasswordHistory:IGloblaId
    {
        public int Id { get; set; }        
        public int UserId { get; set; }
        public User User { get; set; }       
        public byte[] PasswordHash { get; set; }        
        public byte[] PasswordSalt { get; set; }        
        public DateTime CreateTime { get; set; }        
        public bool IsActive { get; set; }
        public DateTime? UpdateTime { get; set; }

    }
    
}
