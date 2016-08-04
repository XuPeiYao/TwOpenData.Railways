using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TwOpenData.Railways.Extensions {
    public static class StationWifiExtension {

        private static string iTaiwanData;

        /// <summary>
        /// 非同步確認車站是否有iTaiwan熱點
        /// </summary>
        /// <param name="THIS"></param>
        /// <returns>是否有熱點</returns>
        public static async Task<bool> HasITaiwanWifiAsync(this Station THIS) {
            if (iTaiwanData == null) await LoadITaiwan();
            return iTaiwanData.IndexOf("臺鐵局" + THIS.Name + "站") > -1;
        }

        /// <summary>
        /// 確認車站是否有iTaiwan熱點
        /// </summary>
        /// <param name="THIS"></param>
        /// <returns>是否有熱點</returns>
        public static bool HasITaiwanWifi(this Station THIS) {
            return HasITaiwanWifiAsync(THIS).GetAwaiter().GetResult();
        }

        private static async Task LoadITaiwan() {
            var url = new Uri("http://www.railway.gov.tw/Upload/UserFiles/%E4%BA%A4%E9%80%9A%E9%83%A8%E8%87%BA%E7%81%A3%E9%90%B5%E8%B7%AF%E7%AE%A1%E7%90%86%E5%B1%80itaiwan%E7%86%B1%E9%BB%9E.csv");
            HttpClient client = new HttpClient();

            iTaiwanData = await client.GetStringAsync(url);
        }
    }
}
