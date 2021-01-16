using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XF_Login.View;

namespace XF_Login.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
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


        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Email"));
            }
        }
        

        private string nome;
        public string Nome
        {
            get { return nome; }
            set
            {
                nome = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Nome"));
            }
        }
        private string categoria;
        public string Categoria
        {
            get { return categoria; }
            set
            {
                categoria = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Categoria"));
            }
        }
        private string password;
        public event PropertyChangedEventHandler PropertyChanged;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Password"));
            }
        }
        public Command LoginCommand
        {
            get
            {
                return new Command(Login);
            }
        }
        public Command SignUp
        {
            get
            {
                return new Command(() => { App.Current.MainPage.Navigation.PushAsync(new XF_SignUpPage()); });
            }
        }
        private async void Login()
        {
            //null or empty field validation, check weather email and password is null or empty
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
                await App.Current.MainPage.DisplayAlert("Empty Values", "Please enter Email and Password", "OK");
            else
            {
                //call GetUser function which we define in Firebase helper class
                var user = await FirebaseHelper.GetUser(Email , Nome ,Categoria);
                //firebase return null valuse if user data not found in database
                if (user != null)
                    if (Email == user.Email && GetHashString(Password) == user.Password)
                    {
                        await App.Current.MainPage.DisplayAlert("Login Success", "", "Ok");
                        //Navigate to Wellcom page after successfuly login
                        //pass user email to welcom page

                        await App.Current.MainPage.Navigation.PushAsync(new WelcomPage( email, nome,categoria));
                        
                    }
                    else
                        await App.Current.MainPage.DisplayAlert("Login Fail", "Please enter correct Email and Password", "OK");
                else
                    await App.Current.MainPage.DisplayAlert("Login Fail", "User not found", "OK");
            }
        }
    }
}
