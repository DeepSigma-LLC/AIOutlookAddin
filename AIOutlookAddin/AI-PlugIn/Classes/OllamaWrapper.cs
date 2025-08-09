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

        public async Task<IEnumerable<Model>> GetAvilableModelsAsync()
        {
            return await ollama.ListLocalModelsAsync();
        }

        public async Task<IEnumerable<Model>> GetRunningModelsAsync()
        {
            return await ollama.ListRunningModelsAsync();
        }

        public List<Message> GetMessages()
        {
            return new List<Message>{
                    new Message(ChatRole.System, "You are a helpful assistant."),
                    new Message(ChatRole.User, "Hello! Who won the World Cup in 2022?")
            };
        }

        public async Task<string> GetVersionAsync()
        {
            return await ollama.GetVersionAsync();
        }

        public async Task<bool> IsRunningAsync()
        {
            return await ollama.IsRunningAsync();
        }

        public async Task<string> ChatResponseAsync(string model, List<Message> messages, bool stream = false, CancellationToken cancellationToken = default)
        {
            ChatRequest chatRequest = new ChatRequest
            {
                Stream = stream,
                Model = model,
                Messages = messages
            };

            StringBuilder sb = new StringBuilder();
            IAsyncEnumerator<ChatResponseStream> responses = ollama.ChatAsync(chatRequest, cancellationToken).GetAsyncEnumerator(cancellationToken);

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

            return sb.ToString();
        }

        public async Task StreamChatAsync(string model, List<Message> messages, bool stream = true, CancellationToken cancellationToken = default)
        {
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
    }
}
