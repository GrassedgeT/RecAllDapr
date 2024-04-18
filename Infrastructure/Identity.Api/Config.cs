using Duende.IdentityServer.Models;

namespace RecAll.Infrastructure.Identity.Api;

public class Config {
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource> { };

    public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope> { };

    public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource> { };

    public static IEnumerable<Client> Clients => new List<Client> { };
}