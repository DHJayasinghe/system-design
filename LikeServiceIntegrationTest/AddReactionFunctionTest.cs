using LikeService;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LikeServiceIntegrationTest
{
    [TestClass]
    public class AddReactionFunctionTest
    {
        public IServiceProvider ServiceProvider { get; private set; }

        [TestInitialize]
        public void Initialize()
        {
            var host = new HostBuilder()
              .ConfigureWebJobs(builder => builder.UseWebJobsStartup(typeof(TestStartup), new WebJobsBuilderContext(), NullLoggerFactory.Instance))
              .Build();

            ServiceProvider = host.Services;
        }

        [TestMethod]
        public void Should_AddReactionEntryForPost_WhenUserDoInitialReactionForPost()
        {

        }
    }

    public class TestStartup : Startup
    {


    }
}