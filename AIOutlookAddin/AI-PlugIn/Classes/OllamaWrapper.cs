using OllamaSharp;
using OllamaSharp.Models;
using OllamaSharp.Models.Chat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AI_PlugIn.DataStructures;

namespace AI_PlugIn.Classes
{
    internal class OllamaWrapper
    {
        public EventHandler<string> OnMessageReceived;
        private OllamaApiClient ollama { get; set; }
        public OllamaWrapper()
        {
            ollama = new OllamaApiClient("http://localhost:11434");
        }

        public async Task<IEnumerable<string>> GetAvailableModelsAsync()
        {
            var models = await ollama.ListLocalModelsAsync();
            return models.Select(m => m.Name);
        }

        public async Task<IEnumerable<Model>> GetRunningModelsAsync()
        {
            return await ollama.ListRunningModelsAsync();
        }

        public async Task<string> GetVersionAsync()
        {
            return await ollama.GetVersionAsync();
        }

        public async Task<bool> IsRunningAsync()
        {
            return await ollama.IsRunningAsync();
        }

        public async Task<Result<string>> ChatResponseAsync(string model, string userPrompt, bool stream = false, CancellationToken cancellationToken = default)
        {
            List<Message> messages = GetMessages(userPrompt);

            ChatRequest chatRequest = new ChatRequest
            {
                Stream = stream,
                Model = model,
                Messages = messages
            };

            Result<string> result = new Result<string>();
            IAsyncEnumerator<ChatResponseStream> responses;
            try
            {
                responses = ollama.ChatAsync(chatRequest, cancellationToken).GetAsyncEnumerator(cancellationToken);
            }
            catch(Exception ex)
            {
                result.ErrorEncountered = true;
                result.Message = ex.Message;
                return result;
            }

            StringBuilder sb = new StringBuilder();
            try
            {
                while (await responses.MoveNextAsync())
                {
                    var text = responses.Current.Message?.Content;
                    if (!string.IsNullOrEmpty(text)) { sb.Append(text); }
                }
            }
            finally
            {
                await responses.DisposeAsync();
            }

            result.Value = sb.ToString();
            return result;
        }

        public async Task StreamChatAsync(string model, string userPrompt, bool stream = true, CancellationToken cancellationToken = default)
        {
            List<Message> messages = GetMessages(userPrompt);

            ChatRequest chatRequest = new ChatRequest
            {   
                Stream = stream,
                Model = model,
                Messages = messages
            };

            var asyncEnum = ollama.ChatAsync(chatRequest, cancellationToken).GetAsyncEnumerator(cancellationToken);
            try
            {
                while (await asyncEnum.MoveNextAsync())
                {
                    var text = asyncEnum.Current.Message?.Content;
                    if (!string.IsNullOrEmpty(text))
                    {
                        OnMessageReceived?.Invoke(this, text);
                    }
                }
            }
            finally
            {
                await asyncEnum.DisposeAsync();
            }
        }

        private List<Message> GetMessages(string userPrompt)
        {
            return new List<Message>{
                    new Message(ChatRole.System, "You are a helpful assistant."),
                    new Message(ChatRole.User, userPrompt)
            };
        }
    }
}
