﻿using System;
using System.Collections.Generic;

namespace MicroActive.Security.AuthorizationServer.Data.Models
{
    public partial class ClientClaim
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }

        public virtual Client Client { get; set; }
    }
}
