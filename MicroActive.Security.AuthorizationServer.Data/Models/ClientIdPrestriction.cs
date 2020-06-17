using System;
using System.Collections.Generic;

namespace MicroActive.Security.AuthorizationServer.Data.Models
{
    public partial class ClientIdPrestriction
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Provider { get; set; }

        public virtual Client Client { get; set; }
    }
}
