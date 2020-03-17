using FluentAssertions;
using FunctionalTests.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using AspNetCore.HealthChecks.SmbCifs.DependencyInjection;
using SharpCifs.Netbios;
using Xunit;
using Xunit.Abstractions;

namespace FunctionalTests
{
    //[Collection("execution")]
    public class FunctionalTest
    {
        private readonly ITestOutputHelper _testOutputHelper;
        //private readonly ExecutionFixture _fixture;


        //public FunctionalTest(ExecutionFixture fixture)
        //{
        //    _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        //}

        private readonly string _sambaHostName;
        private readonly string _sambaWorkGroup;


        public FunctionalTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            string sambaHostIp;
            var sambaHostNameDsn = "sambaalpine";
            var sambaPortNumber = "8137";
            var sambaPubPortNumber = "8445";
            _sambaWorkGroup = "WORKGROUP";

            SharpCifs.Config.SetProperty("jcifs.smb.client.lport", sambaPortNumber);

            try
            {
                IPAddress addr = Dns.GetHostEntry(sambaHostNameDsn).AddressList.First(addr =>
                  addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                _testOutputHelper.WriteLine($"IP = {addr}");
                sambaHostIp = addr.ToString();
            }
            catch (Exception)
            {
                IPAddress addr = Dns.GetHostEntry("localhost").AddressList.First(addr =>
                  addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                _testOutputHelper.WriteLine($"IP = {addr}");
                sambaHostIp = addr.ToString();
            }



            SharpCifs.Config.SetProperty("jcifs.smb.client.laddr", sambaHostIp);

            SharpCifs.Config.Apply();


            _sambaHostName = $"{sambaHostIp}:{sambaPubPortNumber}";
        }

        [Fact]
        public async Task be_healthy_if_smbcifs_is_available()
        {

            _testOutputHelper.WriteLine($"SAMBAHOSTNAME = {_sambaHostName}");

            var webHostBuilder = new WebHostBuilder()
                .UseStartup<DefaultStartup>()
                .ConfigureServices(services =>
                {

                    services.AddHealthChecks()
                        .AddSmbCifsBasicAuth(s =>
                        {
                            s.Hostname = _sambaHostName;
                            s.Domain = _sambaWorkGroup;
                            s.Username = "guest";
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
                            s.Hostname = "0.0.0.0:0000";
                            s.Domain = "FAKEDOMAIN";
                            s.Username = "anyuser";
                            s.UserPassword = "anypassword";
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
