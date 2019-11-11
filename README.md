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
public void ConfigureServices(IServiceCollection services)
{
    services.AddHealthChecks()
                .AddSmbCifs(
                    hostname: "myhostname.domain.subdomain" or IP "xxx.xxx.xxx.xxx",
                    domain: "domain",
                    username: "username",
                    password: "password",
                    name: "smbcifs",
                    failureStatus: HealthStatus.Degraded,
                    tags: new string[] { "smbcifs-service" })
}
```

>SAMPLE CODE USING ADVANCED CREDENTIALS
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddHealthChecks()
                .AddSmbCifs(
                    hostname: "myhostname.domain.subdomain" or IP "xxx.xxx.xxx.xxx",
                    domain: "domain",
                    username: "username",
                    challenge: "byte[] challenge",
                    ansiHash: "byte[] ansiHash",
                    unicodeHash: "byte[] unicodeHash",
                    name: "smbcifs",
                    failureStatus: HealthStatus.Degraded,
                    tags: new string[] { "smbcifs-service" })
}
```

>ABOUT THE AUTHOR

```comment
I am Nuno Relvão a passionate Senior .Net Developer, that already helped lead projects and teams to anchieve more. I am still learning the many paths of life and work, and will problably will continue so for a long time... :)
```
