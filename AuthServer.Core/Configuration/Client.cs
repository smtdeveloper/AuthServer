﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Configuration
{
    public class Client
    {
        public string Id { get; set; } // ıd guid olarak tanımlanıyor.
        public string Secret { get; set; }

        //www.myapi1.com - www.myapi2.com
        public List<String> Audiences { get; set; } // hangi apileri görebileceği tutulduğu özellik.

    }
}
