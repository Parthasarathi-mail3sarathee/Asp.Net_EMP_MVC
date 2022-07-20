using AspDotNetMVC1.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApplication_Shared_Services.Model;

namespace AspDotNetMVC1.ConsumeAPI
{
    public class StudentRepoAPI
    {
        public async Task<StudentList> GetStudents(string token)
        {
            StudentList stdlist  = new StudentList();
            string Baseurl = "https://localhost:44379/api/";
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                //Sending request to find web api REST service resource Gettoken using HttpClient
                var result = client.GetAsync("Student/Getall").Result;
                //Checking the response is successful or not which is sent using HttpClient
                if (result.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var getAllStudentString = result.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    stdlist.Students = JsonConvert.DeserializeObject<List<Student>>(getAllStudentString);
                    stdlist.status = "200";
                }
                else
                {
                    stdlist.Students = null;
                    stdlist.status = "401";
                }
                //returning the employee list to view
                return stdlist;
            }
        }
    }
}
