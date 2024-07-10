using CapsValidationProyect.Persistence.CapsValidationProyect.Domain.Models;
using CapsValidationProyect.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Polly.Retry;
using Polly;

namespace CapsValidationProyect.API
{
    public class AppContextSeed
    {

        public async Task SeedAsync(CapsTestContext context, IHostEnvironment env, IOptions<AppSettings> settings, ILogger<AppContextSeed> logger, UserManager<EmployeeUser> usuarioManager)
        {
            var policy = CreatePolicy(logger, nameof(CapsTestContext));

            await policy.ExecuteAsync(async () => {
                var useCustomizationData = settings.Value
                .UseCustomizationData;

                var contentRootPath = env.ContentRootPath;

                if (!usuarioManager.Users.Any())
                {
                    var usuario = new EmployeeUser { FullName = "Angel Loredo", UserName = "angelloredo", Email = "angelloredoo65@gmail.com", AddedDate = DateTime.UtcNow, LastUpdatedDate = DateTime.UtcNow, AddedBy = "Admin", LastUpdatedBy = "Admin" };
                    await usuarioManager.CreateAsync(usuario, "Password123$");
                }
            });
        }

      


        private string[] GetHeaders(string[] requiredHeaders, string csvfile)
        {
            string[] csvheaders = File.ReadLines(csvfile).First().ToLowerInvariant().Split(',');

            if (csvheaders.Count() != requiredHeaders.Count())
            {
                throw new Exception($"requiredHeader count '{requiredHeaders.Count()}' is different then read header '{csvheaders.Count()}'");
            }

            foreach (var requiredHeader in requiredHeaders)
            {
                if (!csvheaders.Contains(requiredHeader))
                {
                    throw new Exception($"does not contain required header '{requiredHeader}'");
                }
            }

            return csvheaders;
        }

        private AsyncRetryPolicy CreatePolicy(ILogger<AppContextSeed> logger, string prefix, int retries = 3)
        {
            return Policy.Handle<SqlException>().
                WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) => {
                        logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}", prefix, exception.GetType().Name, exception.Message, retry, retries);
                    }
                );
        }
    }
}
