﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Stock_LUIS
{
    public class LUISStockClient
    {
        public static async Task<StockLUIS> ParseUserInput(string strInput)
        {
            string strRet = string.Empty;
            string strEscaped = Uri.EscapeDataString(strInput);

            using (var client = new HttpClient())
            {
                string uri = "https://api.projectoxford.ai/luis/v1/application?id=6f3cb055-31a8-4810-8897-4c778482b339&subscription-key=c98c774b24d346459aee70c976247d11&q=" + strEscaped;
                HttpResponseMessage msg = await client.GetAsync(uri);

                if (msg.IsSuccessStatusCode)
                {
                    var jsonResponse = await msg.Content.ReadAsStringAsync();
                    var _Data = JsonConvert.DeserializeObject<StockLUIS>(jsonResponse);
                    return _Data;
                }
            }
            return null;
        }
    }

    public class StockLUIS
    {
        public string query { get; set; }
        public lIntent[] intents { get; set; }
        public lEntity[] entities { get; set; }
    }

    public class lIntent
    {
        public string intent { get; set; }
        public float score { get; set; }
    }

    public class lEntity
    {
        public string entity { get; set; }
        public string type { get; set; }
        public int startIndex { get; set; }
        public int endIndex { get; set; }
        public float score { get; set; }
    }

}
