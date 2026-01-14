//using System;
//using System.Collections.Generic;
//using System.Security.Cryptography;
//using System.Text;
//using System.Text.Json;

//namespace HospitalManagementCosmosDB.Application.Helpers
//{
//    public class RequestHashHelper
//    {
//        public static string ComputeHash(object body)
//        {
//            var json = JsonSerializer.Serialize(body);
//            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(json));
//            return Convert.ToBase64String(bytes);
//        }
//    }
//}


using System.Security.Cryptography;
using System.Text;

namespace HospitalManagementCosmosDB.Application.Helpers
{
    public static class RequestHashHelper
    {
        public static string ComputeHash(string body)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(body));
            return Convert.ToBase64String(bytes);
        }
    }
}
