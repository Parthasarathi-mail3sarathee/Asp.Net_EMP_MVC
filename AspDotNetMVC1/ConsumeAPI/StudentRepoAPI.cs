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
        public string dateformatChange(string dateString)
        {
            string changedfromatdate = string.Empty;
            if (dateString.Contains("/"))
            {

            }
            return changedfromatdate;

        }
        public async Task<StudentList> GetStudents(string token)
        {
            StudentList stdlist = new StudentList();
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

        public StudentModel GetStudentByID(int id, string token)
        {
            StudentModel stdMdl = new StudentModel();
            Student std1;
            string status = string.Empty;
            string Baseurl = "https://localhost:44379/api/";
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = client.GetAsync("Student/GetByID/" + id).Result;
                //Checking the response is successful or not which is sent using HttpClient
                if (result.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var StdObjString = result.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    stdMdl.Student = JsonConvert.DeserializeObject<Student>(StdObjString);
                    stdMdl.status = "200";
                }
                else
                {
                    stdMdl.status = "401";
                }
                //returning the employee list to view
                return stdMdl;
            }
        }

        public async Task<string> AddStudent(Student std, string token)
        {
            Student std1;
            string status = string.Empty;
            string Baseurl = "https://localhost:44379/api/";
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var myContent = JsonConvert.SerializeObject(std);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                //Sending request to find web api REST service resource Gettoken using HttpClient
                var result = client.PostAsync("Student/AddStudent/", byteContent).Result;
                //Checking the response is successful or not which is sent using HttpClient
                if (result.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var tokenString = result.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    std1 = JsonConvert.DeserializeObject<Student>(tokenString);
                    status = "200";
                }
                else
                {
                    status = "401";
                }
                //returning the employee list to view
                return status;
            }
        }

        public async Task<string> UpdateStudent(Student std, string token)
        {
            Student std1;
            string status = string.Empty;
            string Baseurl = "https://localhost:44379/api/";
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var myContent = JsonConvert.SerializeObject(std);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                //Sending request to find web api REST service resource Gettoken using HttpClient
                var result = client.PutAsync("Student/UpdateStudent/", byteContent).Result;
                //Checking the response is successful or not which is sent using HttpClient
                if (result.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var tokenString = result.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    std1 = JsonConvert.DeserializeObject<Student>(tokenString);
                    status = "200";
                }
                else
                {
                    status = "401";
                }
                //returning the employee list to view
                return status;
            }

        }

        public async Task<string> DelStudent(int id, string token)
        {
            string status = string.Empty;
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
                var result = client.DeleteAsync("Student/DeleteStudent/" + id).Result;
                //Checking the response is successful or not which is sent using HttpClient
                if (result.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var tokenString = result.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list

                    status = "200";
                }
                else
                {
                    status = "401";
                }
                //returning the employee list to view
                return status;
            }
        }
    }
}
