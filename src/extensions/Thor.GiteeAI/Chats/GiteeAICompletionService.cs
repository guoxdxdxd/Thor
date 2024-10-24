﻿using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels;

namespace Thor.OpenAI.Chats;

public sealed class GiteeAICompletionService(IHttpClientFactory httpClientFactory) : IThorCompletionsService
{
    private const string baseUrl = "https://ai.gitee.com/api/serverless/{0}/completions";
    
    private string GetBaseUrl(string model)
    {
        return string.Format(baseUrl, model);
    }

    public async Task<CompletionCreateResponse> CompletionAsync(CompletionCreateRequest createCompletionModel,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory.CreateClient(GiteeAIPlatformOptions.PlatformCode);

        var url = GetBaseUrl(createCompletionModel.Model);
        
        var response = await client.PostJsonAsync(url,
            createCompletionModel, options.ApiKey);

        var result = await response.Content.ReadFromJsonAsync<CompletionCreateResponse>(
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return result;
    }
}