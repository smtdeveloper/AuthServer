﻿using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    // A  - - - - - -  B
    // Public Key and Private Key  --> Asenkron Şifreleme

    public static class SignService
    {
        public static SecurityKey GetSymmetricSecurityKey(string securityKey) 
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
        }

    }
}
