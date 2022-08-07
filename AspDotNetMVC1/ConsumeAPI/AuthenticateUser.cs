using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using AspDotNetMVC1.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace AspDotNetMVC1.ConsumeAPI
{
    public interface IAuthenticateUserAPI
    {
        Task<Token> GetLogin(WebApplication_Shared_Services.Model.Login login);
    }
    public class AuthenticateUserAPI : IAuthenticateUserAPI
    {

        IConfiguration configuration;

        public AuthenticateUserAPI(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<Token> GetLogin(WebApplication_Shared_Services.Model.Login login)
        {
            Token token = null;
            UrlBase UrlBase = configuration.GetSection("UrlBase").Get<UrlBase>();
            ApiKey keys = configuration.GetSection("ApiKey").Get<ApiKey>();

            try
            {
                using (var client = new HttpClient())
                {
                    //Passing service base url
                    client.BaseAddress = new Uri(UrlBase.Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    //Define request data format
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add(keys.ClientKeyHeader, keys.ClientKey);


                    var myContent = JsonConvert.SerializeObject(login);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    //Sending request to find web api REST service resource Gettoken using HttpClient
                    var result = client.PostAsync("token", byteContent).Result;
                    //Checking the response is successful or not which is sent using HttpClient
                    if (result.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api
                        var tokenString = result.Content.ReadAsStringAsync().Result;
                        //Deserializing the response recieved from web api and storing into the Employee list
                        token = JsonConvert.DeserializeObject<Token>(tokenString);
                    }
                    //returning the employee list to view
                    return token;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
