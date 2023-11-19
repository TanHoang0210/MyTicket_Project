using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.AuthModule.Dtos.UserDto
{
    public class CurrentCustomerDto
    {

        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; } 
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public int Country { get; set; }

        public int Nationality { get; set; }

        public string? Address { get; set; }

        public int Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }
    }
}
