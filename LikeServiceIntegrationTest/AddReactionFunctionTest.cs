using LikeService.API;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
***REMOVED***
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
***REMOVED***
using System.Collections.Generic;
using System.Linq;
using System.Net;

[assembly: Parallelize(Workers = 0, Scope = ExecutionScope.MethodLevel)]
namespace LikeServiceIntegrationTest;

[TestClass]
public class AddReactionFunctionTest
***REMOVED***
    private HttpClient _httpClient = null;
    private const string BaseUrl = "http://localhost:8082/reactions";
    private const int One = 1;
    private const int Zero = 0;
    private const int Like = 0;
    private const int Heart = 1;

    [TestInitialize]
    public void Initialize()
***REMOVED***
        var ***REMOVED*** = new ServiceCollection();
        ***REMOVED***.AddHttpClient();
        var serviceProvider = ***REMOVED***.BuildServiceProvider();

        _httpClient = serviceProvider.GetService<HttpClient>();
    ***REMOVED***

    [TestMethod]
    public async Task Should_AddReactionEn***REMOVED***ForPost_WhenUserDoReactionOnPost()
***REMOVED***
        // Arrange
        var request = new AddReactionRequest
    ***REMOVED***
            PostId = Guid.NewGuid().ToString(),
            UserId = "1",
            ReactionType = Like
***REMOVED***

        // Act
        var reactionCountsBefore = await GetReactionCountAsync(request.PostId);
        var addReactionResponse = await AddReactionAsync(request);
        await DelayUntilEventHandlerCompletedAsync();
        var reactionCountsAfter = await GetReactionCountAsync(request.PostId);

        // Assert
        addReactionResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        reactionCountsBefore.Should().BeEmpty();
        reactionCountsAfter.First().LikeCount.Should().Be(One);
    ***REMOVED***

    [TestMethod]
    public async Task Should_RemoveReactionEn***REMOVED***ForPost_WhenUserUndoReactionMade()
***REMOVED***
        var request = new AddReactionRequest
    ***REMOVED***
            PostId = Guid.NewGuid().ToString(),
            UserId = "1",
            ReactionType = Like
***REMOVED***

        await AddReactionAsync(request);
        await DelayUntilEventHandlerCompletedAsync();
        var reactionCountsBefore = await GetReactionCountAsync(request.PostId);
        var removeReactionResponse = await RemoveReactionAsync(request);
        await DelayUntilEventHandlerCompletedAsync();
        var reactionCountsAfter = await GetReactionCountAsync(request.PostId);

        removeReactionResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        reactionCountsBefore.First().LikeCount.Should().Be(One);
        reactionCountsAfter.First().LikeCount.Should().Be(Zero);
    ***REMOVED***

    [TestMethod]
    public async Task Should_ChangeReactionEn***REMOVED***ForPost_WhenUserChangeThenReactionMade()
***REMOVED***
        var postId = Guid.NewGuid().ToString();
        var firstReaction = new AddReactionRequest
    ***REMOVED***
            PostId = postId,
            UserId = "1",
            ReactionType = Like
***REMOVED***
        var secondReaction = new AddReactionRequest
    ***REMOVED***
            PostId = postId,
            UserId = "1",
            ReactionType = Heart
***REMOVED***
        var firstReactionResponse = await AddReactionAsync(firstReaction);
        await DelayUntilEventHandlerCompletedAsync();
        var reactionCountsBefore = await GetReactionCountAsync(firstReaction.PostId);
        var secondReactionResponse = await AddReactionAsync(secondReaction);
        await DelayUntilEventHandlerCompletedAsync();
        var reactionCountsAfter = await GetReactionCountAsync(secondReaction.PostId);

        firstReactionResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        secondReactionResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        reactionCountsBefore.First().LikeCount.Should().Be(One);
        reactionCountsBefore.First().HeartCount.Should().Be(Zero);
        reactionCountsAfter.First().LikeCount.Should().Be(Zero);
        reactionCountsAfter.First().HeartCount.Should().Be(One);
    ***REMOVED***

    [TestMethod]
    public async Task Should_MaintainCorrectReactionCountForPost_WhenMultipeUsersConcurrentlyDoReactions()
***REMOVED***
        var postId = Guid.NewGuid().ToString();
        var reactions = Enumerable.Range(1, 5).ToList().Select(userId => new AddReactionRequest
    ***REMOVED***
            PostId = postId,
            UserId = userId.ToString(),
            ReactionType = Like
    ***REMOVED***;

        var addReactionTasks = reactions.Select(reaction => AddReactionAsync(reaction));
        await Task.WhenAll(addReactionTasks);
        await DelayUntilEventHandlerCompletedAsync(20);

        var reactionCounts = await GetReactionCountAsync(postId);

        reactionCounts.First().LikeCount.Should().Be(reactions.Count());
    ***REMOVED***


    private async Task<HttpResponseMessage> AddReactionAsync(AddReactionRequest request) => await _httpClient.PutAsync(BaseUrl, ArrangeJsonPayload(request));
    private async Task<HttpResponseMessage> RemoveReactionAsync(AddReactionRequest request)
***REMOVED***
        var payload = ArrangeJsonPayload(request);
        return await _httpClient.SendAsync(
            new HttpRequestMessage
        ***REMOVED***
                RequestUri = new Uri(BaseUrl),
                Content = payload,
                Method = new HttpMethod("DELETE")
        ***REMOVED***;
    ***REMOVED***

    private static async Task DelayUntilEventHandlerCompletedAsync(int secondsDelay = 3) => await Task.Delay(secondsDelay * 1000);

    private async Task<List<ReactionCountDto>> GetReactionCountAsync(string postId)
***REMOVED***
        var reactionCountResponse = await _httpClient.GetAsync($"***REMOVED***BaseUrl***REMOVED***?postId=***REMOVED***postId***REMOVED***");
        return await reactionCountResponse.Content.ReadAsAsync<List<ReactionCountDto>>();
    ***REMOVED***

    private static StringContent ArrangeJsonPayload(AddReactionRequest request) => new(JsonConvert.SerializeObject(request, new JsonSerializerSettings
***REMOVED***
        ContractResolver = new CamelCasePropertyNamesContractResolver()
***REMOVED***);
***REMOVED***
