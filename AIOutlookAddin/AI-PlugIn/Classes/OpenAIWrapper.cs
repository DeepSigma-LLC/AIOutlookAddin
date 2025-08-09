using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenAI.Chat;

namespace AI_PlugIn.Classes
{
    internal class OpenAIWrapper
    {

        public string Test()
        {

        }

   

        public static async Task StreamWithOpenAI(string apiKey, string model, string userPrompt, Action<string> onDelta, CancellationToken ct = default)
            {
                ChatClient chat = new ChatClient(model: model, apiKey: apiKey);

                // Async streaming (token-by-token)
                await foreach (var upd in chat.CompleteChatStreamingAsync(
                    new[]
                    {
                    new SystemChatMessage("You are a helpful assistant."),
                    new UserChatMessage(userPrompt)
                    }).WithCancellation(ct))
                {
                
                // Each update can contain multiple content parts—append the text parts
                foreach (var part in upd.ContentUpdate)
                {
                    onDelta?.Invoke(part.Text);
                }
                        
                }
            }
    }
}
