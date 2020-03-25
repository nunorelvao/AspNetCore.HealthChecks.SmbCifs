# AspNetCore.HealthChecks.SmbCifs
Extension to be used in .Net core projects using Xabaril/AspNetCore.Diagnostics.HealthChecks

>Please visit the project at Xabaril/AspNetCore.Diagnostics.HealthChecks

more info at:
<a href="https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks/blob/master/README.md"> Xabaril/AspNetCore.Diagnostics.HealthChecks</a>

>NUGET INSTALL
``` PowerShell
Install-Package AspNetCore.HealthChecks.SmbCifs
```

>SAMPLE CODE USING SIMPLE CREDENTIALS
```csharp
//OLD VERSION (2.0.5 AND LOWER)
public void ConfigureServices(IServiceCollection services)
{
    services.AddHealthChecks()
                .AddSmbCifs(
                    hostname: "myhostname.domain.subdomain", //or IP "xxx.xxx.xxx.xxx",
                    domain: "domain",
                    username: "username",
                    password: "password",
                    name: "smbcifshc",
                    failureStatus: HealthStatus.Degraded,
                    tags: new string[] { "smbcifs-service" })
}

//NEW VERSION (2.0.6 AND UP)
 services.AddHealthChecks()
                .AddSmbCifsBasicAuth(s =>
                {
                    s.Hostname = "myhostname.domain.subdomain"; //or IP "xxx.xxx.xxx.xxx";
                    s.Domain = "domain";
                    s.Username = "username";
                    s.UserPassword = "password";
                }, "smbcifshc", null, new string[] { "smbcifs-service" });
```

>SAMPLE CODE USING ADVANCED CREDENTIALS
```csharp
//OLD VERSION (2.0.5 AND LOWER)
public void ConfigureServices(IServiceCollection services)
{
    services.AddHealthChecks()
                .AddSmbCifs(
                    hostname: "myhostname.domain.subdomain", //or IP "xxx.xxx.xxx.xxx",
                    domain: "domain",
                    username: "username",
                    challenge: "byte[] challenge",
                    ansiHash: "byte[] ansiHash",
                    unicodeHash: "byte[] unicodeHash",
                    name: "smbcifs",
                    failureStatus: HealthStatus.Degraded,
                    tags: new string[] { "smbcifs-service" })
}
//NEW VERSION (2.0.6 AND UP)
    services.AddHealthChecks()
                .AddSmbCifsExtendedAuth(s =>
                {
                    s.Hostname = "myhostname.domain.subdomain"; //or IP "xxx.xxx.xxx.xxx";
                    s.Domain = "domain";
                    s.Username = "username";
                    s.Challenge = Convert.FromBase64CharArray("xxxx".ToCharArray(), 0, "xxxx".Length);
                    s.AnsiHash = Convert.FromBase64CharArray("xxxx".ToCharArray(), 0, "xxxx".Length);
                    s.UnicodeHash = Convert.FromBase64CharArray("xxxx".ToCharArray(), 0, "xxxx".Length);
                }, "smbcifshc", null, new string[] { "smbcifs-service" });
```

>ABOUT THE AUTHOR

```comment
I am Nuno Relvão a passionate Senior .Net Developer, that already helped lead projects and teams to anchieve more. I am still learning the many paths of life and work, and will problably will continue so for a long time... :)
```
