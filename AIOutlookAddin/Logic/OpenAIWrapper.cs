using OpenAI.Chat;

namespace BusinessLogic
{
    internal class OpenAIWrapper
    {
        public static async Task StreamWithOpenAI(string apiKey, string model, string userPrompt, Action<string> onDelta, CancellationToken ct = default)
        {
            ChatClient chat = new ChatClient(model: model, apiKey: apiKey);
            List<ChatMessage> messages = new()
            {
                new SystemChatMessage("You are a helpful assistant."),
                new UserChatMessage(userPrompt)
            };


            await foreach (var upd in chat.CompleteChatStreamingAsync(messages).WithCancellation(ct))
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
