using AI_PlugIn.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_PlugIn.Classes
{
    internal class AIService
    {
        private AIProviderType ProviderType { get; set; }
        private OllamaWrapper Ollama { get; set; }
        public AIService(AIProviderType providerType)
        {
            this.ProviderType = providerType;
            this.Ollama = new OllamaWrapper();
            this.Ollama.OnMessageReceived += Ollama_OnMessageReceived;
        }

        private void Ollama_OnMessageReceived(object sender, string e)
        {
            throw new NotImplementedException();
        }

        internal async Task<string> GenerateTextAsync()
        {
            return await Ollama.ChatResponseAsync("", null);
        }


    }
}
