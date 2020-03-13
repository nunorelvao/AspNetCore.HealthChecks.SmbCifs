using FluentAssertions;
using FunctionalTests.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Threading.Tasks;
using AspNetCore.HealthChecks.SmbCifs.DependencyInjection;
using Xunit;

namespace FunctionalTests
{
    //[Collection("execution")]
    public class FunctionalTest
    {
        private readonly ExecutionFixture _fixture;


        //public FunctionalTest(ExecutionFixture fixture)
        //{
        //    _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        //}

        public FunctionalTest()
        {

        }

        [Fact]
        public async Task be_healthy_if_smbcifs_is_not_available()
        {

            var webHostBuilder = new WebHostBuilder()
                .UseStartup<DefaultStartup>()
                .ConfigureServices(services =>
                {

                    services.AddHealthChecks()
                        .AddSmbCifsBasicAuth(s =>
                        {
                            s.Hostname = "sambaalpine";
                            s.Domain = "workgroup";
                            s.Username = "";
                            s.UserPassword = "";
                        }, "smbcifshc", null, new string[] { "smbcifshc" });

                })
                .Configure(app =>
                {
                    app.UseHealthChecks("/health", new HealthCheckOptions()
                    {
                        Predicate = r => r.Tags.Contains("smbcifshc")
                    });
                });

            var server = new TestServer(webHostBuilder);

            var response = await server.CreateRequest($"/health")
                .GetAsync();

            response.StatusCode
                .Should().Be(HttpStatusCode.OK);

        }

        [Fact]
        public async Task be_unhealthy_if_smbcifs_is_not_available()
        {

            var webHostBuilder = new WebHostBuilder()
                .UseStartup<DefaultStartup>()
                .ConfigureServices(services =>
                {

                    services.AddHealthChecks()
                        .AddSmbCifsBasicAuth(s =>
                        {
                            s.Hostname = "ERRORSERVER";
                            s.Domain = "";
                            s.Username = "";
                            s.UserPassword = "";
                        }, "smbcifshc", null, new string[] { "smbcifshc" });

                })
                .Configure(app =>
                {
                    app.UseHealthChecks("/health", new HealthCheckOptions()
                    {
                        Predicate = r => r.Tags.Contains("smbcifshc")
                    });
                });

            var server = new TestServer(webHostBuilder);

            var response = await server.CreateRequest($"/health")
                .GetAsync();

            response.StatusCode
                .Should().Be(HttpStatusCode.ServiceUnavailable);

        }
    }
}
