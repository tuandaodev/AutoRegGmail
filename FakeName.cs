using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace AutoRegGmail
{
    class FakeName
    {
        private JArray jsonClientArray;
        private int count_client = 0;
        private string json_data_path = @"D:\raw_data\info.json";

        public FakeName()
        {
            //Init
            this.load_client_json();
        }

        private void load_client_json()
        {
            using (StreamReader reader = new StreamReader(json_data_path))
            {
                var json = reader.ReadToEnd();
                this.jsonClientArray = JArray.Parse(json);
                this.count_client = jsonClientArray.Count;
            }
        }

        public dynamic get_random_client()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            int random_int = rnd.Next(this.count_client);
            dynamic data = JObject.Parse(this.jsonClientArray[random_int].ToString());

            return data;
        }
    }
}
