using System;
using System.Collections.Generic;

namespace RMISDemo.API.Models
{
    public partial class ApiUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
