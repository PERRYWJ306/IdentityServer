using System;
using System.Collections.Generic;

namespace MicroActive.Security.AuthorizationServer.Data.Models
{
    public partial class Client
    {
        public Client()
        {
            ClientClaims = new List<ClientClaim>();
            ClientCorsOrigins = new List<ClientCorsOrigin>();
            ClientGrantTypes = new List<ClientGrantType>();
            ClientIdPrestrictions = new List<ClientIdPrestriction>();
            ClientPostLogoutRedirectUris = new List<ClientPostLogoutRedirectUri>();
            ClientRedirectUris = new List<ClientRedirectUri>();
            ClientScopes = new List<ClientScope>();
            ClientSecrets = new List<ClientSecret>();
        }

        public int Id { get; set; }
        public int AbsoluteRefreshTokenLifetime { get; set; }
        public int AccessTokenLifetime { get; set; }
        public int AccessTokenType { get; set; }
        public bool AllowAccessTokensViaBrowser { get; set; }
        public bool AllowOfflineAccess { get; set; }
        public bool AllowPlainTextPkce { get; set; }
        public bool AllowRememberConsent { get; set; }
        public bool AlwaysIncludeUserClaimsInIdToken { get; set; }
        public bool AlwaysSendClientClaims { get; set; }
        public int AuthorizationCodeLifetime { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientUri { get; set; }
        public bool EnableLocalLogin { get; set; }
        public bool Enabled { get; set; }
        public int IdentityTokenLifetime { get; set; }
        public bool IncludeJwtId { get; set; }
        public string LogoUri { get; set; }
        public bool LogoutSessionRequired { get; set; }
        public string LogoutUri { get; set; }
        public bool PrefixClientClaims { get; set; }
        public string ProtocolType { get; set; }
        public int RefreshTokenExpiration { get; set; }
        public int RefreshTokenUsage { get; set; }
        public bool RequireClientSecret { get; set; }
        public bool RequireConsent { get; set; }
        public bool RequirePkce { get; set; }
        public int SlidingRefreshTokenLifetime { get; set; }
        public bool UpdateAccessTokenClaimsOnRefresh { get; set; }

        public virtual List<ClientClaim> ClientClaims { get; set; }
        public virtual List<ClientCorsOrigin> ClientCorsOrigins { get; set; }
        public virtual List<ClientGrantType> ClientGrantTypes { get; set; }
        public virtual List<ClientIdPrestriction> ClientIdPrestrictions { get; set; }
        public virtual List<ClientPostLogoutRedirectUri> ClientPostLogoutRedirectUris { get; set; }
        public virtual List<ClientRedirectUri> ClientRedirectUris { get; set; }
        public virtual List<ClientScope> ClientScopes { get; set; }
        public virtual List<ClientSecret> ClientSecrets { get; set; }
    }
}
