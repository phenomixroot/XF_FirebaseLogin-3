using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using XF_Login.Models;

namespace XF_Login.ViewModel
{

    public class FirebaseHelper
    {
        public static byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }
        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));
            return sb.ToString();
        }
        public static FirebaseClient firebase = new
   <bold>>>>>>>>>>>     FirebaseClient("mon adresse est cacher mais bien dans le code et les info sont bien ecrite sur firebase au moment de l'enregistrement de l'user ");
</bold>
        //Read All    
        public static async Task<List<Users>> GetAllUser()
        {
            try
            {
                var userlist = (await firebase
                .Child("Users")
                .OnceAsync<Users>()).Select(item =>
                new Users
                {
                    Email = item.Object.Email,
                    Password = item.Object.Password,
                    Nome = item.Object.Nome,
                    Categoria = item.Object.Categoria
                }).ToList();
                return userlist;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        //Read     
        public static async Task<Users> GetUser(string email, string nome ,string categoria)
        {
            try
            {
                var allUsers = await GetAllUser();
                await firebase
                .Child("Users")
                .OnceAsync<Users>();
                return allUsers.Where(a => a.Email == email).FirstOrDefault();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }


        //Insert a user    
        public static async Task<bool> AddUser(string email, string password, string nome, string categoria)
        {
            try
            {
                await firebase
                .Child("Users")
                .PostAsync(new Users(){ Email = email, Password = GetHashString(password), Nome = nome, Categoria = categoria });
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //Update     
        public static async Task<bool> UpdateUser(string email, string password, string nome, string categoria)
        {
            try
            {
                var toUpdateUser = (await firebase
                .Child("Users")
                .OnceAsync<Users>()).Where(a => a.Object.Email == email).FirstOrDefault();
                await firebase
                .Child("Users")
                .Child(toUpdateUser.Key)
                .PutAsync(new Users(){ Email = email, Password = GetHashString(password), Nome = nome, Categoria = categoria });
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //Delete User    
        public static async Task<bool> DeleteUser(string email)
        {
            try
            {
                var toDeletePerson = (await firebase
                .Child("Users")
                .OnceAsync<Users>()).Where(a => a.Object.Email == email).FirstOrDefault();
                await firebase.Child("Users").Child(toDeletePerson.Key).DeleteAsync();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

    }
}
