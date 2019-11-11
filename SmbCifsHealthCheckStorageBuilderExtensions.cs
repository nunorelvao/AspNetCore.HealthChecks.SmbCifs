using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AspNetCore.HealthChecks.SmbCifs
{
    public static class SmbCifsHealthCheckStorageBuilderExtensions
    {

        private const string NAME = "smbcifs";

        public static IHealthChecksBuilder AddSmbCifs(
            this IHealthChecksBuilder builder,
            string hostname,
            string domain,
            string username,
            string password,
            string name = null,
            HealthStatus? failureStatus = null,
            IEnumerable<string> tags = null)
        {
            return builder.Add(
                new HealthCheckRegistration(name ?? "smbcifs",
                    (Func<IServiceProvider, IHealthCheck>)(sp =>
                        (IHealthCheck)new SmbCifsStorageHealthCheck(hostname, domain, username, password)), failureStatus, tags));
        }

        public static IHealthChecksBuilder AddSmbCifs(
            this IHealthChecksBuilder builder,
            string hostname,
            string domain,
            string username,
            byte[] challenge,
            byte[] ansiHash,
            byte[] unicodeHash,
            string name = null,
            HealthStatus? failureStatus = null,
            IEnumerable<string> tags = null)
        {
            return builder.Add(
                new HealthCheckRegistration(name ?? "smbcifs",
                    (Func<IServiceProvider, IHealthCheck>)(sp =>
                        (IHealthCheck)new SmbCifsStorageHealthCheck(hostname, domain, username, challenge, ansiHash, unicodeHash)), failureStatus, tags));
        }
    }
}
