using System;
using System.Collections.Generic;
using System.Text;

namespace Brightsoft.Authentication.Jwt
{
    public class JwtModel
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
