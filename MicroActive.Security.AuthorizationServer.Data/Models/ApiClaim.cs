using System;
using System.Collections.Generic;

namespace MicroActive.Security.AuthorizationServer.Data.Models
{
    public partial class ApiClaim
    {
        public int Id { get; set; }
        public int ApiResourceId { get; set; }
        public string Type { get; set; }

        public virtual ApiResource ApiResource { get; set; }
    }
}
