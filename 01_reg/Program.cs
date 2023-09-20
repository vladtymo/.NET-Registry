using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_reg
{
    /*
        Registry.ClassesRoot
        Registry.CurrentConfig
        Registry.CurrentUser
        Registry.DynData
        Registry.Users
        Registry.PerformanceData
        Registry.LocalMachine
    */

    /*
        Name: повертає ім'я ключа реєстру
        Close (): закриває ключ
        CreateSubKey (): створює вкладений ключ, якщо він не існує
        DeleteSubKey (): видаляє вкладений ключ
        DeleteValue (): видаляє значення ключа
        GetSubKeyNames (): повертає колекцію імен вкладених 
        GetValue (): повертає значення 
        OpenSubKey (): відкриває вкладений 
        SetValue (): встановлює значення ключа
    */

    class Program
    {
        static void Main(string[] args)
        {
            RegistryKey currentUserKey = Registry.CurrentUser;

            /////Create key
            RegistryKey helloKey = currentUserKey.CreateSubKey("Inner Key");
            helloKey.SetValue("login", "Bob19");
            helloKey.SetValue("password", "12345");
            helloKey.Close();

            // Create subkey
            /*RegistryKey */ helloKey = currentUserKey.OpenSubKey("Inner Key", true);
            RegistryKey subHelloKey = helloKey.CreateSubKey("SubKey");
            subHelloKey.SetValue("value2", "777", RegistryValueKind.DWord);
            subHelloKey.Close();
            helloKey.Close();

            // short syntax
            Registry.CurrentUser
                    .OpenSubKey("Inner Key", true)
                    .OpenSubKey("SubKey", true)
                    .SetValue("value2", "888", RegistryValueKind.DWord);

            ///////Read data
            /*RegistryKey*/ helloKey = currentUserKey.OpenSubKey("Inner Key");

            string login = helloKey.GetValue("login").ToString();
            string password = helloKey.GetValue("password").ToString();
            helloKey.Close();

            Console.WriteLine(login);
            Console.WriteLine(password);

            ///////////Delete value and key
            /*RegistryKey*/ helloKey = currentUserKey.OpenSubKey("Inner Key", true);

            Console.ReadKey();
            // delete sub key
            helloKey.DeleteSubKey("SubKey");

            Console.ReadKey();
            // delete key value
            helloKey.DeleteValue("login");
            helloKey.Close();

            Console.ReadKey();
            // delete key
            currentUserKey.DeleteSubKey("Inner Key");

            ///////////////Get system theme
            int res = (int)Registry.GetValue(
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize",
                "AppsUseLightTheme", null);

            Console.WriteLine("UseLightTheme: " + res);

            Registry.SetValue(
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize",
                "AppsUseLightTheme", 0);
        }
    }
}
