using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CodexSharp
{
    public class GPT
    {

        private readonly string apiKey;

        public GPT(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public async Task<string> GenerateText(string prompt)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

                var requestBody = new
                {
                    prompt,
                    temperature = 0.5,
                    max_tokens = 100,
                    top_p = 1,
                    frequency_penalty = 0,
                    presence_penalty = 0
                };

                var json = JsonConvert.SerializeObject(requestBody);

                var response = await client.PostAsync("https://api.openai.com/v1/engine/davinci-codex/completions", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));

                var content = await response.Content.ReadAsStringAsync();

                var parsedContent = JObject.Parse(content);

                var choices = parsedContent["choices"];

                if (choices == null || choices.Count() == 0)
                {
                    return null;
                }

                var text = choices.First()["text"].ToString();

                return text;
            }
        }
    }

}
