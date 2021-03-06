﻿using System;
using System.Linq;
using System.Reflection;
using AspNetCore.HealthChecks.SmbCifs;
using AspNetCore.HealthChecks.SmbCifs.DependencyInjection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Xunit;

namespace Tests
{
    public class SmbCifsUnitTest
    {
        [Fact]
        public void add_health_check_when_properly_configured_with_basic_auth()
        {

            var services = new ServiceCollection();
            services.AddHealthChecks()
                .AddSmbCifsBasicAuth(s =>
                {
                    s.Hostname = "myhostmachinename or ip";
                    s.Domain = "mydomain.local";
                    s.Username = "mydomainusername";
                    s.UserPassword = "mydomainuserpassword";
                });



            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<HealthCheckServiceOptions>>();

            var registration = options.Value.Registrations.First();

            var check = registration.Factory(serviceProvider);



            registration.Name.Should().Be("smbcifs");

            check.GetType().Should().Be(typeof(SmbCifsStorageHealthCheck));

            FieldInfo fi = registration.Factory.Target.GetType().GetField("options");
            Assert.True(fi?.FieldType == typeof(SmbCifsBasicOptions));
        }
        [Fact]
        public void add_named_health_check_when_properly_configured_with_basic_auth()
        {
            var services = new ServiceCollection();

            services.AddHealthChecks()
                .AddSmbCifsBasicAuth(s =>
                {
                    s.Hostname = "myhostmachinename or ip";
                    s.Domain = "mydomain.local";
                    s.Username = "mydomainusername";
                    s.UserPassword = "mydomainuserpassword";
                }, name: "my-smbcifs-group");

            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<HealthCheckServiceOptions>>();

            var registration = options.Value.Registrations.First();
            var check = registration.Factory(serviceProvider);

            registration.Name.Should().Be("my-smbcifs-group");
            check.GetType().Should().Be(typeof(SmbCifsStorageHealthCheck));

            FieldInfo fi = registration.Factory.Target.GetType().GetField("options");
            Assert.True(fi?.FieldType == typeof(SmbCifsBasicOptions));
        }

        [Fact]
        public void add_health_check_when_properly_configured_with_extended_auth()
        {

            var services = new ServiceCollection();
            services.AddHealthChecks()
                .AddSmbCifsExtendedAuth(s =>
                {
                    s.Hostname = "myhostmachinename or ip";
                    s.Domain = "mydomain.local";
                    s.Username = "mydomainusername";
                    s.Challenge = Convert.FromBase64CharArray("xxxx".ToCharArray(), 0, "xxxx".Length);
                    s.AnsiHash = Convert.FromBase64CharArray("xxxx".ToCharArray(), 0, "xxxx".Length);
                    s.UnicodeHash = Convert.FromBase64CharArray("xxxx".ToCharArray(), 0, "xxxx".Length);
                });



            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<HealthCheckServiceOptions>>();

            var registration = options.Value.Registrations.First();
            var check = registration.Factory(serviceProvider);

            registration.Name.Should().Be("smbcifs");
            check.GetType().Should().Be(typeof(SmbCifsStorageHealthCheck));

            FieldInfo fi = registration.Factory.Target.GetType().GetField("options");
            Assert.True(fi?.FieldType == typeof(SmbCifsExtendedOptions));
        }
        [Fact]
        public void add_named_health_check_when_properly_configured_with_extended_auth()
        {
            var services = new ServiceCollection();

            services.AddHealthChecks()
                .AddSmbCifsExtendedAuth(s =>
                {
                    s.Hostname = "myhostmachinename or ip";
                    s.Domain = "mydomain.local";
                    s.Username = "mydomainusername";
                    s.Challenge = Convert.FromBase64CharArray("xxxx".ToCharArray(), 0, "xxxx".Length);
                    s.AnsiHash = Convert.FromBase64CharArray("xxxx".ToCharArray(), 0, "xxxx".Length);
                    s.UnicodeHash = Convert.FromBase64CharArray("xxxx".ToCharArray(), 0, "xxxx".Length);
                }, name: "my-smbcifs-group");

            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<HealthCheckServiceOptions>>();

            var registration = options.Value.Registrations.First();
            var check = registration.Factory(serviceProvider);

            registration.Name.Should().Be("my-smbcifs-group");
            check.GetType().Should().Be(typeof(SmbCifsStorageHealthCheck));

            FieldInfo fi = registration.Factory.Target.GetType().GetField("options");
            Assert.True(fi?.FieldType == typeof(SmbCifsExtendedOptions));
        }
    }
}
