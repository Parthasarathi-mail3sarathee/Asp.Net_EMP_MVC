using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using AspDotNetMVC1.Models;
using System.Threading.Tasks;

namespace AspDotNetMVC1.ConsumeAPI
{
    public class AuthenticateUserAPI
    {
        public async Task<Token> GetLogin(WebApplication_Shared_Services.Model.Login login)
        {
            Token token = null;
            string Baseurl = "https://localhost:44379/api/";
            try
            {
                using (var client = new HttpClient())
                {
                    //Passing service base url
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    //Define request data format
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
