using Azure;
using Azure.AI.OpenAI;
using Azure.Identity;
using OllamaSharp.Models;
using OpenAI.Chat;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AI_PlugIn.DataStructures;

namespace AI_PlugIn.Classes
{
    internal class OpenAIWrapper
    {
        private string OpenAIapiKey { get; }
        private string AzureApiKey { get; }
        public OpenAIWrapper(string OpenAIapiKey = "", string Azureapikey = "")
        {
            this.OpenAIapiKey = OpenAIapiKey;
            this.AzureApiKey = Azureapikey;
        }

        public Result<string> AzureGenerateText(Uri endpoint, string deployment, string userPrompt, CancellationToken ct = default)
        {
            AzureOpenAIClient azureClient;
            if (AzureApiKey is null || AzureApiKey.Length < 0)
            {
                azureClient = new AzureOpenAIClient(endpoint, new DefaultAzureCredential());
            }
            else
            {
                azureClient = new AzureOpenAIClient(endpoint, new ApiKeyCredential(AzureApiKey));
            }

            IList<ChatMessage> messages = new List<ChatMessage>
            {
                new SystemChatMessage("You are a helpful assistant."),
                new UserChatMessage(userPrompt)
            };


            ChatClient chat = azureClient.GetChatClient(deployment);

            Result<string> result = new Result<string>();
            try
            {
                ClientResult<ChatCompletion> api_result = chat.CompleteChat(messages, cancellationToken: ct);
                result.Value = api_result.Value.Content[0].Text ?? string.Empty;
            }
            catch (ClientResultException e)
            {
                result.ErrorEncountered = true;
                result.Message = e.Message;
            }
            return result;
        }


        public static async Task AzureStreamWithAzureOpenAI(Uri endpoint, string deployment, string apiKeyOrNull, string userPrompt, Action<string> onDelta, CancellationToken ct = default)
        {
            AzureOpenAIClient azureClient;
            if (apiKeyOrNull is null || apiKeyOrNull.Length < 0)
            {
                azureClient = new AzureOpenAIClient(endpoint, new DefaultAzureCredential());
            }
            else
            {
                azureClient = new AzureOpenAIClient(endpoint, new ApiKeyCredential(apiKeyOrNull));
            }
               
            ChatClient chat = azureClient.GetChatClient(deployment);

            IList<ChatMessage> messages = new List<ChatMessage>
            {
                new SystemChatMessage("You are a helpful assistant."),
                new UserChatMessage(userPrompt)
            };

            var stream = chat.CompleteChatStreamingAsync(messages);
            var enumerator = stream.GetAsyncEnumerator(ct);

            try
            {
                while (await enumerator.MoveNextAsync())
                {
                    var update = enumerator.Current;

                    // Each update can contain multiple content parts
                    if (update?.ContentUpdate != null)
                    {
                        foreach (var part in update.ContentUpdate)
                        {
                            if (!string.IsNullOrEmpty(part.Text))
                                onDelta?.Invoke(part.Text);
                        }
                    }
                }
            }
            finally
            {
                await enumerator.DisposeAsync();
            }
        }

        public async Task OpenAIStream(string model, string userPrompt, Action<string> onDelta, CancellationToken ct = default)
        {
            var chat = new ChatClient(model: model, apiKey: OpenAIapiKey);

            IList<ChatMessage> messages = new List<ChatMessage>
            {
                new SystemChatMessage("You are a helpful assistant."),
                new UserChatMessage(userPrompt)
            };

            // Get the async enumerator from the stream
            var stream = chat.CompleteChatStreamingAsync(messages);
            var enumerator = stream.GetAsyncEnumerator(ct);

            try
            {
                while (await enumerator.MoveNextAsync())
                {
                    var update = enumerator.Current;

                    // Each update can contain multiple content parts
                    if (update?.ContentUpdate != null)
                    {
                        foreach (var part in update.ContentUpdate)
                        {
                            if (!string.IsNullOrEmpty(part.Text))
                                onDelta?.Invoke(part.Text);
                        }
                    }
                }
            }
            finally
            {
                await enumerator.DisposeAsync();
            }
        }

        public Result<string> OpenAIGenerateResponse(string model, string userPrompt, CancellationToken ct = default)
        {
            var chat = new ChatClient(model: model, apiKey: OpenAIapiKey);

            IList<ChatMessage> messages = new List<ChatMessage>
            {
                new SystemChatMessage("You are a helpful assistant."),
                new UserChatMessage(userPrompt)
            };

            Result<string> result = new Result<string>();
            try
            {
                ClientResult<ChatCompletion> api_result = chat.CompleteChat(messages, cancellationToken: ct);
                result.Value = api_result.Value.Content[0].Text ?? string.Empty;
            }
            catch(ClientResultException ex)
            {
                result.ErrorEncountered = true;
                result.Message = ex.Message;
            }
            return result;
        }
    }

}
