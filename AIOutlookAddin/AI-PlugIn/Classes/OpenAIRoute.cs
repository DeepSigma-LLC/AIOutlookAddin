using OpenAI;
using OpenAI.Chat;
using OpenAI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AI_PlugIn
{
    internal class OpenAIRoute
    {
        private string key { get; }
        public OpenAIRoute(string key)
        {
            this.key = key;
        }

        public string GenerateText(string prompt, string model)
        {
            ChatClient client = new ChatClient(model: model, apiKey: key);
            ChatCompletion completion = client.CompleteChat(prompt);

            return completion.Content[0].Text;
        }

        public async Task TestConnection()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.openai.com/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", key);

                try
                {
                    var response = await client.GetAsync("v1/models");

                    Console.WriteLine($"Status Code: {response.StatusCode}");

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("✅ Connected to OpenAI API successfully.");
                        Console.WriteLine(content.Substring(0, Math.Min(content.Length, 500))); // Trim output
                    }
                    else
                    {
                        Console.WriteLine("❌ Failed to connect to OpenAI API.");
                        var error = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(error);
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine("❌ Network error:");
                    Console.WriteLine(ex.Message);
                }
            }

            
        }
    }
}
