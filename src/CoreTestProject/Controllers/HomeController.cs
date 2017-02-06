using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using CoreTestProject.Models;

namespace CoreTestProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetHash(string data)
        {
            var hashList = new List<HashType>();

            hashList.Add(new HashType
            {
                Hash = GetRfc2898(data),
                Name = "RFC2898"
            });

            hashList.Add(new HashType
            {
                Hash = GetMD5(data),
                Name = "MD5"
            });

            hashList.Add(new HashType
            {
                Hash = GetSha256(data),
                Name = "SHA256"
            });

            hashList.Add(new HashType
            {
                Hash = GetSha1(data),
                Name = "SHA1"
            });

            return Json(hashList);
        }


        private string GetMD5(string data)
        {
            using (var md5 = MD5.Create())
            {
                var hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(data));
                var sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        private string GetSha1(string data)
        {
            using (var sha = SHA1.Create())
            {
                var hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(data));
                var sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        private string GetSha256(string data)
        {
            using (var sha = SHA256.Create())
            {
                var hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(data));
                var sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        private string GetRfc2898(string data)
        {

            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] salt;
                rng.GetBytes(salt = new byte[16]);
                using (var pbkdf2 = new Rfc2898DeriveBytes(data, salt, 1000))
                {
                    var hash = pbkdf2.GetBytes(20);
                    var hashBytes = new byte[36];

                    Array.Copy(salt, 0, hashBytes, 0, 16);
                    Array.Copy(hash, 0, hashBytes, 16, 20);

                    return Convert.ToBase64String(hashBytes);
                }
            }
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
