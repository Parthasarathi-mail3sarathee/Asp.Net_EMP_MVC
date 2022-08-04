using AspDotNetMVC1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApplication_Shared_Services.Model;

namespace AspDotNetMVC1.ConsumeAPI
{
    public interface IStudentRepoAPI
    {
        UserRoleModel GetMyRole(string userid, string token);
        Pager GetStudentPageCount(Pager pager, string token);
        StudentList GetStudentsPerPage(Pager pager, string token);
        StudentList GetStudents(string token);
        StudentModel GetStudentByID(int id, string token);
        string AddStudent(Student std, string token);
        string AddStudentProfile(int id, IList<IFormFile> stdprofile, string token);
        string UpdateStudent(Student std, string token);
        string UpdateStudentProfile(int id, IList<IFormFile> stdprofile, string token);
        string DelStudent(int id, string token);

    }
    public class StudentRepoAPI : IStudentRepoAPI
    {
        IConfiguration configuration;

        public StudentRepoAPI(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public UserRoleModel GetMyRole(string userid, string token)
        {
            UserRoleModel userRolelist = new UserRoleModel();
            string Baseurl = "https://localhost:44379/api/";
            ApiKey keys = configuration.GetSection("ApiKey").Get<ApiKey>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Add(keys.ClientKeyHeader, keys.ClientKey);


                //Sending request to find web api REST service resource Gettoken using HttpClient
                var result = client.GetAsync("UserRole/GetMyRole/" + userid).Result;
                //Checking the response is successful or not which is sent using HttpClient
                if (result.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var getAllStudentString = result.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    userRolelist.userRole = JsonConvert.DeserializeObject<List<UserRole>>(getAllStudentString);
                    userRolelist.status = "200";
                }
                else
                {
                    userRolelist.userRole = null;
                    userRolelist.status = "401";
                }
                //returning the employee list to view
                return userRolelist;
            }
        }

        public Pager GetStudentPageCount(Pager pager, string token)
        {
            StudentList stdlist = new StudentList();
            string Baseurl = "https://localhost:44379/api/";
            ApiKey keys = configuration.GetSection("ApiKey").Get<ApiKey>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Add(keys.ClientKeyHeader, keys.ClientKey);


                //Sending request to find web api REST service resource Gettoken using HttpClient
                var result = client.GetAsync("Student/GetStudentPageCount/" + pager.pageSize).Result;
                //Checking the response is successful or not which is sent using HttpClient
                if (result.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var pageCountString = result.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    pager.pageCount = JsonConvert.DeserializeObject<int>(pageCountString);
                    stdlist.status = "200";
                }
                else
                {
                    stdlist.Students = null;
                    stdlist.status = "401";
                }
                //returning the employee list to view
                return pager;
            }
        }

        public StudentList GetStudentsPerPage(Pager pager, string token)
        {
            StudentList stdlist = new StudentList();
            string Baseurl = "https://localhost:44379/api/";
            ApiKey keys = configuration.GetSection("ApiKey").Get<ApiKey>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Add(keys.ClientKeyHeader, keys.ClientKey);


                //Sending request to find web api REST service resource Gettoken using HttpClient
                var result = client.GetAsync("Student/GetStudentPerPage/" + pager.currentPage + "/" + pager.pageSize).Result;
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

        public StudentList GetStudents(string token)
        {
            StudentList stdlist = new StudentList();
            string Baseurl = "https://localhost:44379/api/";
            ApiKey keys = configuration.GetSection("ApiKey").Get<ApiKey>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Add(keys.ClientKeyHeader, keys.ClientKey);


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
            ApiKey keys = configuration.GetSection("ApiKey").Get<ApiKey>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Add(keys.ClientKeyHeader, keys.ClientKey);


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
        public string AddStudentProfile(int id, IList<IFormFile> stdprofile, string token)
        {
            Student std1;
            string status = string.Empty;
            string Baseurl = "https://localhost:44379/api/";
            ApiKey keys = configuration.GetSection("ApiKey").Get<ApiKey>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Add(keys.ClientKeyHeader, keys.ClientKey);


                using (var content = new MultipartFormDataContent())
                {
                    ByteArrayContent byteArrayContent = null;
                    foreach (var filestd in stdprofile)
                    {
                        if (filestd.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                filestd.CopyTo(ms);
                                var fileBytes = ms.ToArray();
                                //string s = Convert.ToBase64String(fileBytes);
                                // act on the Base64 data
                                byteArrayContent = new ByteArrayContent(fileBytes);

                            }
                        }
                        content.Add(byteArrayContent, "profileFile", filestd.FileName);
                    }
                    var result = client.PostAsync("Student/AddStudentProfile/" + id, content).Result;
                    //Checking the response is successful or not which is sent using HttpClient
                    if (result.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api
                        var respString = result.Content.ReadAsStringAsync().Result;
                        //Deserializing the response recieved from web api and storing into the Employee list
                        var respstatus = JsonConvert.DeserializeObject<string>(respString);
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
        public string AddStudent(Student std, string token)
        {
            string status = string.Empty;
            string Baseurl = "https://localhost:44379/api/";
            ApiKey keys = configuration.GetSection("ApiKey").Get<ApiKey>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Add(keys.ClientKeyHeader, keys.ClientKey);


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
                    std = JsonConvert.DeserializeObject<Student>(tokenString);
                    status = "200:" + std.ID;
                }
                else
                {
                    status = "401";
                }
                //returning the employee list to view
                return status;
            }
        }
        public string UpdateStudentProfile(int id, IList<IFormFile> stdprofile, string token)
        {
            Student std1;
            string status = string.Empty;
            string Baseurl = "https://localhost:44379/api/";
            ApiKey keys = configuration.GetSection("ApiKey").Get<ApiKey>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Add(keys.ClientKeyHeader, keys.ClientKey);


                using (var content = new MultipartFormDataContent())
                {
                    ByteArrayContent byteArrayContent = null;
                    foreach (var filestd in stdprofile)
                    {
                        if (filestd.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                filestd.CopyTo(ms);
                                var fileBytes = ms.ToArray();
                                //string s = Convert.ToBase64String(fileBytes);
                                // act on the Base64 data
                                byteArrayContent = new ByteArrayContent(fileBytes);

                            }
                        }
                        content.Add(byteArrayContent, "profileFile", filestd.FileName);
                    }
                    var result = client.PostAsync("Student/UpdateStudentProfile/" + id, content).Result;
                    //Checking the response is successful or not which is sent using HttpClient
                    if (result.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api
                        var respString = result.Content.ReadAsStringAsync().Result;
                        //Deserializing the response recieved from web api and storing into the Employee list
                        var respstatus = JsonConvert.DeserializeObject<string>(respString);
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

        public string UpdateStudent(Student std, string token)
        {
            Student std1;
            string status = string.Empty;
            string Baseurl = "https://localhost:44379/api/";
            ApiKey keys = configuration.GetSection("ApiKey").Get<ApiKey>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Add(keys.ClientKeyHeader, keys.ClientKey);


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

        public string DelStudent(int id, string token)
        {
            string status = string.Empty;
            string Baseurl = "https://localhost:44379/api/";
            ApiKey keys = configuration.GetSection("ApiKey").Get<ApiKey>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Add(keys.ClientKeyHeader, keys.ClientKey);


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
