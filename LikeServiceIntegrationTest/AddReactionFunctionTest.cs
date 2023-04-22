using LikeService.API;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace LikeServiceIntegrationTest;

[TestClass]
public class AddReactionFunctionTest
{
    private HttpClient _httpClient = null;
    private const string BaseUrl = "http://localhost:8082/reactions";
    private const int One = 1;
    private const int Zero = 0;
    private const int Like = 0;
    private const int Heart = 1;

    [TestInitialize]
    public void Initialize()
    {
        var services = new ServiceCollection();
        services.AddHttpClient();
        var serviceProvider = services.BuildServiceProvider();

        _httpClient = serviceProvider.GetService<HttpClient>();
    }

    [TestMethod]
    public async Task Should_AddReactionEntryForPost_WhenUserDoReactionOnPost()
    {
        // Arrange
        var request = new AddReactionRequest
        {
            PostId = Guid.NewGuid().ToString(),
            UserId = "1",
            ReactionType = Like
        };

        // Act
        var reactionCountsBefore = await GetReactionCountAsync(request.PostId);
        var addReactionResponse = await AddReactionAsync(request);
        await DelayUntilEventHandlerCompletedAsync();
        var reactionCountsAfter = await GetReactionCountAsync(request.PostId);

        // Assert
        addReactionResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        reactionCountsBefore.Should().BeEmpty();
        reactionCountsAfter.First().LikeCount.Should().Be(One);
    }

    [TestMethod]
    public async Task Should_RemoveReactionEntryForPost_WhenUserUndoReactionMade()
    {
        var request = new AddReactionRequest
        {
            PostId = Guid.NewGuid().ToString(),
            UserId = "1",
            ReactionType = Like
        };

        await AddReactionAsync(request);
        await DelayUntilEventHandlerCompletedAsync();
        var reactionCountsBefore = await GetReactionCountAsync(request.PostId);
        var removeReactionResponse = await RemoveReactionAsync(request);
        await DelayUntilEventHandlerCompletedAsync();
        var reactionCountsAfter = await GetReactionCountAsync(request.PostId);

        removeReactionResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        reactionCountsBefore.First().LikeCount.Should().Be(One);
        reactionCountsAfter.First().LikeCount.Should().Be(Zero);
    }

    [TestMethod]
    public async Task Should_ChangeReactionEntryForPost_WhenUserChangeThenReactionMade()
    {
        var postId = Guid.NewGuid().ToString();
        var firstReaction = new AddReactionRequest
        {
            PostId = postId,
            UserId = "1",
            ReactionType = Like
        };
        var secondReaction = new AddReactionRequest
        {
            PostId = postId,
            UserId = "1",
            ReactionType = Heart
        };
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
    }

    [TestMethod]
    public async Task Should_MaintainCorrectReactionCountForPost_WhenMultipeUsersConcurrentlyDoReactions()
    {
        var postId = Guid.NewGuid().ToString();
        var reactions = Enumerable.Range(1, 10).ToList().Select(userId => new AddReactionRequest
        {
            PostId = postId,
            UserId = userId.ToString(),
            ReactionType = Like
        });

        var addReactionTasks = reactions.Select(reaction => AddReactionAsync(reaction));
        await Task.WhenAll(addReactionTasks);
        await DelayUntilEventHandlerCompletedAsync(30);

        var reactionCounts = await GetReactionCountAsync(postId);

        reactionCounts.First().LikeCount.Should().Be(reactions.Count());
    }


    private async Task<HttpResponseMessage> AddReactionAsync(AddReactionRequest request) => await _httpClient.PutAsync(BaseUrl, ArrangeJsonPayload(request));
    private async Task<HttpResponseMessage> RemoveReactionAsync(AddReactionRequest request)
    {
        var payload = ArrangeJsonPayload(request);
        return await _httpClient.SendAsync(
            new HttpRequestMessage
            {
                RequestUri = new Uri(BaseUrl),
                Content = payload,
                Method = new HttpMethod("DELETE")
            });
    }

    private static async Task DelayUntilEventHandlerCompletedAsync(int secondsDelay = 3) => await Task.Delay(secondsDelay * 1000);

    private async Task<List<ReactionCountDto>> GetReactionCountAsync(string postId)
    {
        var reactionCountResponse = await _httpClient.GetAsync($"{BaseUrl}?postId={postId}");
        return await reactionCountResponse.Content.ReadAsAsync<List<ReactionCountDto>>();
    }

    private static StringContent ArrangeJsonPayload(AddReactionRequest request) => new(JsonConvert.SerializeObject(request, new JsonSerializerSettings
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    }));
}
