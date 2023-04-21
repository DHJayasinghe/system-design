using LikeService;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
***REMOVED***

namespace LikeServiceIntegrationTest
***REMOVED***
    [TestClass]
    public class AddReactionFunctionTest
    ***REMOVED***
        public IServiceProvider ServiceProvider ***REMOVED*** get; private set; ***REMOVED***

        [TestInitialize]
        public void Initialize()
        ***REMOVED***
            var host = new HostBuilder()
              .ConfigureWebJobs(builder => builder.UseWebJobsStartup(typeof(TestStartup), new WebJobsBuilderContext(), NullLoggerFactory.Instance))
              .Build();

            ServiceProvider = host.Services;
    ***REMOVED***

        [TestMethod]
        public void Should_AddReactionEn***REMOVED***ForPost_WhenUserDoInitialReactionForPost()
        ***REMOVED***

    ***REMOVED***
***REMOVED***

    public class TestStartup : Startup
    ***REMOVED***


***REMOVED***
***REMOVED***