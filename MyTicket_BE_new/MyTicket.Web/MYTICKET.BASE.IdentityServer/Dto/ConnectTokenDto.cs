using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.BASE.IdentityServer.Dto
{
    public class ConnectTokenDto
    {
        [FromForm(Name = "grant_type")]
        public string GrantType { get; set; }
        [FromForm(Name = "username")]
        public string Username { get; set; }
        [FromForm(Name = "password")]
        public string Password { get; set; }
        [FromForm(Name = "scope")]
        public string Scope { get; set; }
        [FromForm(Name = "client_id")]
        public string ClientId { get; set; }
        [FromForm(Name = "client_secret")]
        public string Client_Secret { get; set; }
    }
}
