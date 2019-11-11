using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SharpCifs.Smb;

namespace AspNetCore.HealthChecks.SmbCifs
{
    public class SmbCifsStorageHealthCheck : IHealthCheck
    {
        private readonly string _domain;
        private readonly string _username;
        private readonly string _password;
        private readonly byte[] _challenge;
        private readonly byte[] _ansiHash;
        private readonly byte[] _unicodeHash;
        private readonly string _hostname;

        private NtlmPasswordAuthentication _auth;

        public SmbCifsStorageHealthCheck(string hostname, string domain, string username, string password)
        {
            if (hostname == null)
                throw new ArgumentNullException(nameof(hostname));
            if (domain == null)
                throw new ArgumentNullException(nameof(domain));
            if (username == null)
                throw new ArgumentNullException(nameof(username));
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            _hostname = hostname;
            _domain = domain;
            _username = username;
            _password = password;


        }

        public SmbCifsStorageHealthCheck(string hostname, string domain, string username, byte[] challenge, byte[] ansiHash, byte[] unicodeHash)
        {
            if (hostname == null)
                throw new ArgumentNullException(nameof(hostname));
            if (domain == null)
                throw new ArgumentNullException(nameof(domain));
            if (username == null)
                throw new ArgumentNullException(nameof(username));
            if (challenge == null)
                throw new ArgumentNullException(nameof(challenge));
            if (ansiHash == null)
                throw new ArgumentNullException(nameof(ansiHash));
            if (unicodeHash == null)
                throw new ArgumentNullException(nameof(unicodeHash));

            _hostname = hostname;
            _domain = domain;
            _username = username;
            _challenge = challenge;
            _ansiHash = ansiHash;
            _unicodeHash = unicodeHash;

        }


        public Task<HealthCheckResult> CheckHealthAsync(
          HealthCheckContext context,
          CancellationToken cancellationToken = default(CancellationToken))
        {

            try
            {
                CreateConnection();

                var folder = new SmbFile($"smb://{_hostname}/", _auth).List();

                return Task.FromResult<HealthCheckResult>(HealthCheckResult.Healthy((string)null, (IReadOnlyDictionary<string, object>)null));

            }
            catch (Exception ex)
            {
                return Task.FromResult<HealthCheckResult>(new HealthCheckResult(context.Registration.FailureStatus, (string)null, ex, (IReadOnlyDictionary<string, object>)null));
            }
        }

        private void CreateConnection()
        {
            _auth = (!string.IsNullOrWhiteSpace(_password))
                ? new NtlmPasswordAuthentication(_domain, _username, _password)
                : new NtlmPasswordAuthentication(_domain, _username, _challenge, _ansiHash, _unicodeHash);
        }
    }
}

