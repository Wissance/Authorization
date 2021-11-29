# Wissance.Authorization Project
C# class library that could be used in any type of project (Web, Desktop & so on)
An unofficial project to use **KeyCloak** and possibly others OpenIdServers **as Authentication and Authorization server**

## Functionality
* Easily integrate **KeyCloak as authorization server** into application'
* Protect **swagger with KeyCloak** Authorization

## Example of usage
### 1. Authentication & Authorization on Keycloak

don't forget to add this usage:
```c#
using Wissance.Authorization.Config;
using Wissance.Authorization.Extensions;
```

In my Startup.cs i have _ConfigureService_ method that calls ConfigureWeb:
```C#
public void ConfigureServices(IServiceCollection services)
{
     // Configure subsystems before ...
     ConfigureWeb(services);
     // Configure subsystems after ...
}

// ...

private void ConfigureWeb(IServiceCollection services)
{
     // ...
     // Authorization, here we need only config which is very simple, see KeyCloakAuthenticator tests
     KeyCloakServerConfig authConfig = BuildKeyCloakConfig();
     services.AddKeyCloak(authConfig);
     // ...
}

// don't forget to add Authentication & Authorization in Configure function, like this:

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // ...
    app.UseAuthentication();
    // app.UseHttpsRedirection();

    app.UseRouting();

    app.UseAuthorization();

    app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    //...
}

```

if you would like to just `Restrict` access to you controllers to only Autenticated users (without Claims check) your could add following (i suppose that Controllers configuretion is implemented in upper mentioned `ConfigureWeb(IServiceCollection services)` method:

```c#
services.AddControllers(config =>
{
    AuthorizationPolicy policy = new AuthorizationPolicyBuilder()
                                    .RequireAuthenticatedUser()
                                    .Build();
    config.Filters.Add(new AuthorizeFilter(policy));
});
```
this requires to add a couple of using:

```c#
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
```

If you would like to use Role-based acces to controllers (we form Roles property (propper mapper have to be configured on a KeyCloak side)) use `[Authorize]` attribute on controllers, i.e.
```
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Authorize(Roles = "user")]
 public class MyController : ControllerBase
{
    // ...
}
```

### 2. Use swagger under Authorization

See the structure of Startup class in part related to Keycloak, so to configure Swagger with Keycloak add following line to you `ConfigureWeb(IServiceCollection services` method:

```c#
IDictionary<string, string> scopes = _authConfig.Scopes.Select(s => s).ToDictionary(k => k, v => v);
services.AddSwaggerWithKeyCloakPasswordAuthentication(authConfig, scopes);
```

and to `Configure(IApplicationBuilder app, IWebHostEnvironment env)` method:

```c#
   app.UseSwaggerWithKeyCloakAuthentication("Wissance.BusinessTools", BuildKeyCloakConfig(), _authConfig.Scopes);
```

`_authConfig.Scopes` is array of strings (public string[] Scopes { get; set; }), by default _**Keycloak**_ works with _**profile**_ scope.
var Scopes = new string[]{"profile"};

!!! DON'T forget to add * or your app pase URI i.e. http://localhost:8421/* to **WebOrigin of Keycloak clietn settings** (subscribe to our medium because we are writing interesting articles and in particular about Authorization and Keycloak usage aspects: https://m-ushakov.medium.com/)

_Additional docs with images with examples will be soon_.

## Nuget package
https://www.nuget.org/packages/Wissance.Authorization/
