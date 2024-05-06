using Duende.IdentityServer.Models;
using IdentityModel;

namespace RecAll.Infrastructure.Identity.Api;

public class Config {
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource> {
            new IdentityResources.OpenId(), new IdentityResources.Profile()
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope> {
            new("List", "List") {
                UserClaims = {
                    JwtClaimTypes.Audience
                }
            },
            new("TextList", "Text List") {
                UserClaims = {
                    JwtClaimTypes.Audience
                }
            },
            new("MaskedTextItem", "Masked Text Item") {
                UserClaims = {
                    JwtClaimTypes.Audience
                }
            }
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource> {
            new("List", "List") {
                Scopes = {
                    "List"
                },
                UserClaims = {
                    JwtClaimTypes.Audience
                }
            },
            new("TextList", "Text List") {
                Scopes = {
                    "TextList"
                },
                UserClaims = {
                    JwtClaimTypes.Audience
                }
            },
            new("MaskedTextItem", "Masked Text Item") {
                Scopes = {
                    "MaskedTextItem"
                },
                UserClaims = {
                    JwtClaimTypes.Audience
                }
            }
        };

    public static IEnumerable<Client>
        GetClients(Dictionary<string, string> clientUrlDict) =>
        new List<Client> {
            new() {
                ClientId = "ListApiSwaggerUI",
                ClientName = "ListApiSwaggerUI",
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,
                RedirectUris = {
                    $"{clientUrlDict["ListApi"]}/swagger/oauth2-redirect.html"
                },
                PostLogoutRedirectUris = {
                    $"{clientUrlDict["ListApi"]}/swagger/"
                },
                AllowedScopes = {
                    "List", "TextList", "MaskedTextItem"
                }
            },
            new() {
                ClientId = "TextListApiSwaggerUI",
                ClientName = "TextListApiSwaggerUI",
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,
                RedirectUris = {
                    $"{clientUrlDict["TextListApi"]}/swagger/oauth2-redirect.html"
                },
                PostLogoutRedirectUris = {
                    $"{clientUrlDict["TextListApi"]}/swagger/"
                },
                AllowedScopes = {
                    "TextList"
                }
            },
            new(){
                ClientId = "MaskedTextItemApiSwaggerUI",
                ClientName = "MaskedTextItemApiSwaggerUI",
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,
                RedirectUris = {
                    $"{clientUrlDict["MaskedTextItemApi"]}/swagger/oauth2-redirect.html"
                },
                PostLogoutRedirectUris = {
                    $"{clientUrlDict["MaskedTextItemApi"]}/swagger/"
                },
                AllowedScopes = {
                    "MaskedTextItem"
                }
            }
        };
}