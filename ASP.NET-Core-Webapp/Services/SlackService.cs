using ASP.NET_Core_Webapp.Helpers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ASP.NET_Core_Webapp.Services
{
    public class SlackService
    {
        private readonly IConfiguration configuration;

        public SlackService(IConfiguration config)
        {
            this.configuration = config;
        }

        public void SendEmail(string email, string messageToSend)
        {
            var Client = new HttpClient();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", configuration["SlackBotToken"]);
            
            //Create an email user lookup request:
            var emailLookupRequest = new HttpRequestMessage(HttpMethod.Post, "https://slack.com/api/users.lookupByEmail");

            var list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("email", email));

            emailLookupRequest.Content = new FormUrlEncodedContent(list);
            var response = Client.SendAsync(emailLookupRequest).Result;

            EmailLookupResponse responseObject = new EmailLookupResponse();

            //Deserialize the response (first create a string from the request's content, 
            //then deserialize it.
            responseObject = JsonConvert.DeserializeObject<EmailLookupResponse>(response.Content.ReadAsStringAsync().Result);

            //Create the post message request:
            var postMessageRequest = new HttpRequestMessage(HttpMethod.Post, "https://slack.com/api/chat.postMessage");

            var messageRequestBody = new List<KeyValuePair<string, string>>();
            messageRequestBody.Add(new KeyValuePair<string, string>("channel", responseObject.user.id));
            messageRequestBody.Add(new KeyValuePair<string, string>("text", messageToSend));
            postMessageRequest.Content = new FormUrlEncodedContent(messageRequestBody);

            Client.SendAsync(postMessageRequest);
        }
    }
}
