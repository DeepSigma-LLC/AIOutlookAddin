using AI_PlugIn.DataStructures;
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
        private OpenAIWrapper OpenAI { get; set; }
        public AIService(AIProviderType providerType, string OpenAIapikey = "", string AzureApi = "") 
        {
            this.ProviderType = providerType;
            this.Ollama = new OllamaWrapper();
            this.OpenAI = new OpenAIWrapper(OpenAIapikey);
            this.Ollama.OnMessageReceived += Ollama_OnMessageReceived;
        }

        public AIService(string providerType, string OpenAIapikey = "", string AzureApi = "")
        {
            this.ProviderType = ConvertStringToEnum(providerType);
            this.Ollama = new OllamaWrapper();
            this.OpenAI = new OpenAIWrapper(OpenAIapikey);
            this.Ollama.OnMessageReceived += Ollama_OnMessageReceived;
        }

        private AIProviderType ConvertStringToEnum(string providerType)
        {
            if (Enum.TryParse(providerType, true, out AIProviderType result))
            {
                return result;
            }
            throw new ArgumentException($"Invalid provider type: {providerType}");
        }

        public async Task<string> GenerateText(string model, string userPrompt, Uri AzureEndpoint)
        {
            Result<string> result = new Result<string>();
            switch(ProviderType)
            {
                case AIProviderType.Ollama:
                    result = await Ollama.ChatResponseAsync(model, userPrompt, false);
                    break;
                case AIProviderType.OpenAI:
                    result = OpenAI.OpenAIGenerateResponse(model, userPrompt);
                    break;
                case AIProviderType.Azure:
                    result = OpenAI.AzureGenerateText(AzureEndpoint, model,  userPrompt);
                    break;
                default:
                    throw new NotImplementedException("Provider not implemented");
            }

            if (result.ErrorEncountered)
            {
                return result.Message;
            }
            return result.Value;
        }

        private void Ollama_OnMessageReceived(object sender, string e)
        {
            throw new NotImplementedException();
        }

    }
}
