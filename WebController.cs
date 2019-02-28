using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using System.Text.RegularExpressions;
using System.Globalization;

namespace AutoRegGmail
{
    class WebController
    {
        private string reg_gmail_url = "https://accounts.google.com/embedded/setup/v2/androidtv";
        private Random gen = new Random();

        // Random Day
        DateTime start = new DateTime(1980, 1, 1);
        DateTime end = new DateTime(2000, 1, 1);
        DateTime RandomDay()
        {
            int range = (end - start).Days;
            return start.AddDays(gen.Next(range));
        }

        public WebController()
        {

            //End Init
            Console.WriteLine("Start Web Server");

            UserInfo userInfo = do_fakeuser();
            this.Start(userInfo);
        }

        public UserInfo do_fakeuser()
        {
            FakeName fakeName = new FakeName();

            //for (int i = 0; i < 1; i++)
            //{
                var client = fakeName.get_random_client();

                //Get User_Info
                UserInfo userInfo = new UserInfo();
                userInfo.first_name = client.first_name;
                userInfo.last_name = client.last_name;
                userInfo.user_name = client.user_name;
                userInfo.dob = this.RandomDay();
                userInfo.gender = (gen.Next(0, 2) == 0);

                string user_name = (string)client.first_name + (string)client.last_name + gen.Next(10000000, 99999999);
                userInfo.user_name = this.convert_username(user_name);

                Console.WriteLine(client.full_name + "  " + userInfo.dob.ToString("dd/MM/yyyy") + " " + (userInfo.gender ? "Male":"Female") + "  " + userInfo.user_name);

            //Thread.Sleep(2);
            //}

            return userInfo;
        }

        public string convert_username(string s)
        {
            string result = RemoveDiacritics(s);
            result = result.Replace(" ", "");

            return result.ToLower();
        }

        static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }



        public void Start(UserInfo userInfo)
        {

            FirefoxOptions options = new FirefoxOptions();

            options.AddArgument("-private");
            options.AddArgument("--user-agent=Mozilla/5.0 (Linux; Android 6.0.1; SM-G920V Build/MMB29K) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.98 Mobile Safari/537.36");

            IWebDriver driver = new FirefoxDriver(options);
            driver.Url = reg_gmail_url;

            Thread.Sleep(100);
            driver.FindElement(By.XPath("//div[@class='daaWTb']//span[@class='RveJvd snByac']")).Click();
            Thread.Sleep(100);
            driver.FindElement(By.Id("lastName")).SendKeys(userInfo.last_name);
            driver.FindElement(By.Id("firstName")).SendKeys(userInfo.first_name);
            Thread.Sleep(100);
            driver.FindElement(By.XPath("//span[@class='RveJvd snByac']")).Click();
            Thread.Sleep(100);
            driver.FindElement(By.Name("day")).SendKeys(userInfo.dob.ToString("d"));

            driver.FindElement(By.Name("day")).SendKeys(userInfo.dob.ToString("d"));



            driver.FindElement(By.Name("day")).SendKeys(userInfo.first_name);
        }
    }
}
