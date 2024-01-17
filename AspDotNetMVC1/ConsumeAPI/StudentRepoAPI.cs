using AspDotNetMVC1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using WebApplication_Shared_Services.Model;

namespace AspDotNetMVC1.ConsumeAPI
{
    public interface IEmployeeRepoAPI
    {
        UserRoleModel GetMyRole(string userid, string token);
        Pager GetEmployeePageCount(Pager pager, string token);
        EmployeeList GetEmployeesPerPage(Pager pager, string token);
        EmployeeList GetEmployees(string token);
        byte[] GetthisEmployeeFile(int studid, string fileName, string token);
        List<string> GetEmployeeFileListByID(int studid, string token);
        EmployeeModel GetEmployeeByID(int id, string token);
        string AddEmployee(Employee empl, string token);
        string AddEmployeeProfile(int id, IList<IFormFile> emplprofile, string token);
        string UpdateEmployee(Employee empl, string token);
        string UpdateEmployeeProfile(int id, IList<IFormFile> emplprofile, string token);
        string DelEmployee(int id, string token);

    }
    public class EmployeeRepoAPI : IEmployeeRepoAPI
    {
        IConfiguration configuration;

        public EmployeeRepoAPI(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public UserRoleModel GetMyRole(string userid, string token)
        {
            UserRoleModel userRolelist = new UserRoleModel();
            UrlBase UrlBase = configuration.GetSection("UrlBase").Get<UrlBase>();
            ApiKey keys = configuration.GetSection("ApiKey").Get<ApiKey>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(UrlBase.Baseurl);
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
                    var getAllEmployeeString = result.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    userRolelist.userRole = JsonConvert.DeserializeObject<List<UserRole>>(getAllEmployeeString);
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

        public Pager GetEmployeePageCount(Pager pager, string token)
        {
            EmployeeList empllist = new EmployeeList();
            UrlBase UrlBase = configuration.GetSection("UrlBase").Get<UrlBase>();
            ApiKey keys = configuration.GetSection("ApiKey").Get<ApiKey>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(UrlBase.Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Add(keys.ClientKeyHeader, keys.ClientKey);


                //Sending request to find web api REST service resource Gettoken using HttpClient
                var result = client.GetAsync("Employee/GetEmployeePageCount/" + pager.pageSize).Result;
                //Checking the response is successful or not which is sent using HttpClient
                if (result.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var pageCountString = result.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    pager.pageCount = JsonConvert.DeserializeObject<int>(pageCountString);
                    empllist.status = "200";
                }
                else
                {
                    empllist.Employees = null;
                    empllist.status = "401";
                }
                //returning the employee list to view
                return pager;
            }
        }

        public byte[] GetthisEmployeeFile(int studid, string fileName, string token)
        {
            Stream stream1 = null;
            try
            {
                byte[] emplfile = null;
                UrlBase UrlBase = configuration.GetSection("UrlBase").Get<UrlBase>();
                ApiKey keys = configuration.GetSection("ApiKey").Get<ApiKey>();
                using (var client = new HttpClient())
                {
                    //Passing service base url
                    client.BaseAddress = new Uri(UrlBase.Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    //Define request data format
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    client.DefaultRequestHeaders.Add(keys.ClientKeyHeader, keys.ClientKey);
                    var result = client.GetAsync("Employee/GetEmployeeFileByIDAndName/" + studid + "/" + fileName).Result;


                    using (MemoryStream ms = (MemoryStream)result.Content.ReadAsStreamAsync().Result)
                    {
                        var bytearray = streamToByteArray(ms);

                        //SaveByteArrayToFileWithFileStream(bytearray, "d:\\test1\\" + fileName);

                        return bytearray;
                    }

                }
            }
            catch (Exception ex)
            {
                return null;
            }
            //byte[] buffer = new byte[16 * 1024];
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    int read;
            //    while ((read = resultStream.Read(buffer, 0, buffer.Length)) > 0)
            //    {
            //        ms.Write(buffer, 0, read);
            //    }
            //    emplfile = ms.ToArray();
            //    return emplfile;
            //}

        }

        public static void SaveByteArrayToFileWithFileStream(byte[] data, string filePath)
        {
            using (var stream = File.Create(filePath))
            {
                stream.Write(data, 0, data.Length);
            }
        }

        private static byte[] streamToByteArray(Stream input) //Eliminated
        {
            MemoryStream ms = new MemoryStream();
            input.CopyTo(ms);
            return ms.ToArray();
        }

        private static MemoryStream streamToMemoryStream(Stream input)//Eliminated
        {
            MemoryStream ms = new MemoryStream();
            input.CopyTo(ms);
            return ms;
        }

        private static byte[] streamWriteToFileAndSendArray(MemoryStream input, string fileName)
        {
            using (var file = new FileStream("d:\\test1\\" + fileName, FileMode.Create, FileAccess.Write))
            {
                ;
                input.WriteTo(file);
                return input.ToArray();
                // return ms;
            }
        }

        public List<string> GetEmployeeFileListByID(int studid, string token)
        {
            List<string> emplfilelist = new List<string>();
            UrlBase UrlBase = configuration.GetSection("UrlBase").Get<UrlBase>();
            ApiKey keys = configuration.GetSection("ApiKey").Get<ApiKey>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(UrlBase.Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Add(keys.ClientKeyHeader, keys.ClientKey);
                var result = client.GetAsync("Employee/GetEmployeeFileListID/" + studid).Result;

                if (result.IsSuccessStatusCode)
                {
                    var getAllEmployeeFileString = result.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    emplfilelist = JsonConvert.DeserializeObject<List<string>>(getAllEmployeeFileString);
                }
                else
                {
                    emplfilelist = null;
                }
                //returning the employee list to view
                return emplfilelist;
            }
        }
        public EmployeeList GetEmployeesPerPage(Pager pager, string token)
        {
            EmployeeList empllist = new EmployeeList();
            UrlBase UrlBase = configuration.GetSection("UrlBase").Get<UrlBase>();
            ApiKey keys = configuration.GetSection("ApiKey").Get<ApiKey>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(UrlBase.Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Add(keys.ClientKeyHeader, keys.ClientKey);


                //Sending request to find web api REST service resource Gettoken using HttpClient
                var result = client.GetAsync("Employee/GetEmployeePerPage/" + pager.currentPage + "/" + pager.pageSize).Result;
                //Checking the response is successful or not which is sent using HttpClient
                if (result.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var getAllEmployeeString = result.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    empllist.Employees = JsonConvert.DeserializeObject<List<Employee>>(getAllEmployeeString);
                    empllist.status = "200";
                }
                else
                {
                    empllist.Employees = null;
                    empllist.status = "401";
                }
                //returning the employee list to view
                return empllist;
            }
        }

        public EmployeeList GetEmployees(string token)
        {
            EmployeeList empllist = new EmployeeList();
            UrlBase UrlBase = configuration.GetSection("UrlBase").Get<UrlBase>();
            ApiKey keys = configuration.GetSection("ApiKey").Get<ApiKey>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(UrlBase.Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Add(keys.ClientKeyHeader, keys.ClientKey);


                //Sending request to find web api REST service resource Gettoken using HttpClient
                var result = client.GetAsync("Employee/Getall").Result;
                //Checking the response is successful or not which is sent using HttpClient
                if (result.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var getAllEmployeeString = result.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    empllist.Employees = JsonConvert.DeserializeObject<List<Employee>>(getAllEmployeeString);
                    empllist.status = "200";
                }
                else
                {
                    empllist.Employees = null;
                    empllist.status = "401";
                }
                //returning the employee list to view
                return empllist;
            }
        }

        public static bool ValidateServerCertificate(object sender,X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            Console.WriteLine("Validating certificate {0}", certificate.Issuer);
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            Console.WriteLine("Certificate error: {0}", sslPolicyErrors);

            // Do not allow this client to communicate with unauthenticated servers.
            return false;
        }

        public EmployeeModel GetEmployeeByID(int id, string token)
        {
            EmployeeModel emplMdl = new EmployeeModel();
            Employee empl1;
            string status = string.Empty;
            UrlBase UrlBase = configuration.GetSection("UrlBase").Get<UrlBase>();
            ApiKey keys = configuration.GetSection("ApiKey").Get<ApiKey>();

            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(UrlBase.Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Add(keys.ClientKeyHeader, keys.ClientKey);


                var result = client.GetAsync("Employee/GetByID/" + id).Result;
                //Checking the response is successful or not which is sent using HttpClient
                if (result.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var emplObjString = result.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    emplMdl.Employee = JsonConvert.DeserializeObject<Employee>(emplObjString);
                    emplMdl.status = "200";
                }
                else
                {
                    emplMdl.status = "401";
                }
                //returning the employee list to view
                return emplMdl;
            }
        }
        public string AddEmployeeProfile(int id, IList<IFormFile> emplprofile, string token)
        {
            Employee empl1;
            string status = string.Empty;
            UrlBase UrlBase = configuration.GetSection("UrlBase").Get<UrlBase>();
            ApiKey keys = configuration.GetSection("ApiKey").Get<ApiKey>();

            /// 
            ///
            //X509Certificate2 certificate;
            //var handler = new HttpClientHandler
            //{
            //    ClientCertificateOptions = ClientCertificateOption.Manual,
            //    SslProtocols = SslProtocols.Tls12
            //};
            //handler.ClientCertificates.Add(certificate);
            //handler.CheckCertificateRevocationList = false;
            //// this is required to get around self-signed certs
            //handler.ServerCertificateCustomValidationCallback =
            //    (httpRequestMessage, cert, cetChain, policyErrors) => {
            //        return true;
            //    };
            //var client = new HttpClient(handler);
            //requestMessage.Headers.Add("X-ARR-ClientCert", certificate.GetRawCertDataString());

            ////public IServiceProvider ConfigureServices(IServiceCollection services)
            //{
            //    services.AddCertificateForwarding(options => {
            //        options.CertificateHeader = "X-ARR-ClientCert";
            //        options.HeaderConverter = (headerValue) => {
            //            X509Certificate2 clientCertificate = null;
            //            try
            //            {
            //                if (!string.IsNullOrWhiteSpace(headerValue))
            //                {
            //                    var bytes = ConvertHexToBytes(headerValue);
            //                    clientCertificate = new X509Certificate2(bytes);
            //                }
            //            }
            //            catch (Exception)
            //            {
            //                // invalid certificate
            //            }

            //            return clientCertificate;
            //        };
            //    });
            //}

            //using (var client = new HttpClient(new HttpClientHandler
            //{
            //    ClientCertificateOptions = ClientCertificateOption.Manual,
            //    SslProtocols = SslProtocols.Tls12,
            //    ClientCertificates = { new X509Certificate2(@"C:\kambiDev.pfx") }
            //}))

            //var handler = new HttpClientHandler();
            //handler.ClientCertificates.Add(new X509Certificate2("cert.crt"));
            // using (var client = new HttpClient(handler))

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase.Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Add(keys.ClientKeyHeader, keys.ClientKey);


                using (var content = new MultipartFormDataContent())
                {
                    ByteArrayContent byteArrayContent = null;
                    foreach (var fileempl in emplprofile)
                    {
                        if (fileempl.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                fileempl.CopyTo(ms);
                                var fileBytes = ms.ToArray();
                                //string s = Convert.ToBase64String(fileBytes);
                                // act on the Base64 data
                                byteArrayContent = new ByteArrayContent(fileBytes);

                            }
                        }
                        content.Add(byteArrayContent, "profileFile", fileempl.FileName);
                    }
                    var result = client.PostAsync("Employee/AddEmployeeProfile/" + id, content).Result;
                    //Checking the response is successful or not which is sent using HttpClient
                    if (result.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api
                        var respString = result.Content.ReadAsStringAsync().Result;
                        //Deserializing the response recieved from web api and storing into the Employee list
                        var respstatus = JsonConvert.DeserializeObject<object>(respString);
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
        public string AddEmployee(Employee emp, string token)
        {
            string status = string.Empty;
            UrlBase UrlBase = configuration.GetSection("UrlBase").Get<UrlBase>();
            ApiKey keys = configuration.GetSection("ApiKey").Get<ApiKey>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(UrlBase.Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Add(keys.ClientKeyHeader, keys.ClientKey);


                var myContent = JsonConvert.SerializeObject(emp);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                //Sending request to find web api REST service resource Gettoken using HttpClient
                var result = client.PostAsync("Employee/AddEmployee/", byteContent).Result;
                //Checking the response is successful or not which is sent using HttpClient
                if (result.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var tokenString = result.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    emp = JsonConvert.DeserializeObject<Employee>(tokenString);
                    status = "200:" + emp.ID;
                }
                else
                {
                    status = "401";
                }
                //returning the employee list to view
                return status;
            }
        }
        public string UpdateEmployeeProfile(int id, IList<IFormFile> emplprofile, string token)
        {
            Employee empl1;
            string status = string.Empty;
            UrlBase UrlBase = configuration.GetSection("UrlBase").Get<UrlBase>();
            ApiKey keys = configuration.GetSection("ApiKey").Get<ApiKey>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase.Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Add(keys.ClientKeyHeader, keys.ClientKey);


                using (var content = new MultipartFormDataContent())
                {
                    ByteArrayContent byteArrayContent = null;
                    foreach (var fileempl in emplprofile)
                    {
                        if (fileempl.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                fileempl.CopyTo(ms);
                                var fileBytes = ms.ToArray();
                                //string s = Convert.ToBase64String(fileBytes);
                                // act on the Base64 data
                                byteArrayContent = new ByteArrayContent(fileBytes);

                            }
                        }
                        content.Add(byteArrayContent, "profileFile", fileempl.FileName);
                    }
                    var result = client.PostAsync("Employee/UpdateEmployeeProfile/" + id, content).Result;
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

        public string UpdateEmployee(Employee empl, string token)
        {
            Employee empl1;
            string status = string.Empty;
            UrlBase UrlBase = configuration.GetSection("UrlBase").Get<UrlBase>();
            ApiKey keys = configuration.GetSection("ApiKey").Get<ApiKey>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(UrlBase.Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Add(keys.ClientKeyHeader, keys.ClientKey);


                var myContent = JsonConvert.SerializeObject(empl);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                //Sending request to find web api REST service resource Gettoken using HttpClient
                var result = client.PutAsync("Employee/UpdateEmployee/" + empl.ID, byteContent).Result;
                //Checking the response is successful or not which is sent using HttpClient
                if (result.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var tokenString = result.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    empl1 = JsonConvert.DeserializeObject<Employee>(tokenString);
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

        public string DelEmployee(int id, string token)
        {
            string status = string.Empty;
            UrlBase UrlBase = configuration.GetSection("UrlBase").Get<UrlBase>();
            ApiKey keys = configuration.GetSection("ApiKey").Get<ApiKey>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(UrlBase.Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Add(keys.ClientKeyHeader, keys.ClientKey);


                //Sending request to find web api REST service resource Gettoken using HttpClient
                var result = client.DeleteAsync("Employee/DeleteEmployee/" + id).Result;
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
