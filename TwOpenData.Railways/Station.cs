using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TwOpenData.Railways.Fares;

namespace TwOpenData.Railways {
    /// <summary>
    /// 車站資訊
    /// </summary>
    public class Station {
        /// <summary>
        /// 車站編號
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 三碼式車站編號
        /// </summary>
        public int ShortId { get; private set; }

        /// <summary>
        /// 車站中文名稱
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 車站英文名稱
        /// </summary>
        public string EnglishName { get; private set; }

        /// <summary>
        /// 車站中文地址
        /// </summary>
        public string Address { get; private set; }

        /// <summary>
        /// 車站英文地址
        /// </summary>
        public string EnglishAddress { get; private set; }

        /// <summary>
        /// 車站電話
        /// </summary>
        public string Phone { get; private set; }

        /// <summary>
        /// 車站經緯度位置
        /// </summary>
        public Position Position { get; private set; }
                
        public override bool Equals(object obj) {
            var Obj = obj as Station;
            return Obj != null && Obj.Id == this.Id;
        }

        public static bool operator ==(Station a, Station b) {
            if (Object.ReferenceEquals(a, b)) return true;
            if (Object.Equals(a, b)) return true;
            if (Object.Equals(a, null) && !Object.Equals(b, null)) {
                return b.Equals(a);
            }
            if (Object.Equals(b, null) && !Object.Equals(a, null)) {
                return a.Equals(b);
            }
            return a.Equals(b);
        }

        public static bool operator !=(Station a, Station b) {
            return !(a == b);
        }

        /// <summary>
        /// 透過車站編號非同步取得車站資訊
        /// </summary>
        /// <param name="id">車站編號</param>
        /// <returns>車站資訊</returns>
        public static async Task<Station> GetStationByIdAsync(int id) {
            if (Cache.StationDictionary == null || Cache.StationDictionary.Count == 0) {
                await Cache.LoadAsync();
            }
            if (!Cache.StationDictionary.ContainsKey(id)) {
                throw new KeyNotFoundException("找不到指定車站");
            }
            return Cache.StationDictionary[id];
        }

        /// <summary>
        /// 透過車站編號取得車站資訊
        /// </summary>
        /// <param name="id"></param>
        /// <returns>車站資訊</returns>
        public static Station GetStationById(int id) {
            return GetStationByIdAsync(id).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 透過三碼車站編號非同步取得車站資訊
        /// </summary>
        /// <param name="id">車站編號</param>
        /// <returns>車站資訊</returns>
        public static async Task<Station> GetStationByShortIdAsync(int id) {
            if (Cache.StationDictionary == null || Cache.StationDictionary.Count == 0) {
                await Cache.LoadAsync();
            }

            Station result = null;
            result = Cache.StationDictionary.Values.Where(x => x.ShortId == id).FirstOrDefault();
            if (result == null) {
                throw new KeyNotFoundException("找不到指定車站");
            }
            return result;
        }

        /// <summary>
        /// 透過三碼車站編號取得車站資訊
        /// </summary>
        /// <param name="id">車站編號</param>
        /// <returns>車站資訊</returns>
        public static Station GetStationByShortId(int id) {
            return GetStationByShortIdAsync(id).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 非同步取得所有車站資訊
        /// </summary>
        /// <returns>所有車站資訊</returns>
        public static async Task<Station[]> GetAllStationsAsync() {
            if (Cache.StationDictionary == null || Cache.StationDictionary.Count == 0) await Cache.LoadAsync();
            return Cache.StationDictionary.Select(x => x.Value).ToArray();
        }

        /// <summary>
        /// 取得所有車站資訊
        /// </summary>
        /// <returns>所有車站資訊</returns>
        public static Station[] GetAllStations() {
            return GetAllStationsAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// 透過車站名稱非同步取得車站資訊
        /// </summary>
        /// <param name="id">車站編號</param>
        /// <returns>車站名稱</returns>
        public static async Task<Station> GetStationByNameAsync(string name) {
            var result = (await GetAllStationsAsync()).Where(x => x.Name == name).FirstOrDefault();
            if (result == null) throw new KeyNotFoundException("找不到指定車站");
            return result;
        }

        /// <summary>
        /// 透過車站名稱取得車站資訊
        /// </summary>
        /// <param name="id">車站編號</param>
        /// <returns>車站名稱</returns>
        public static Station GetStationByName(string name) {
            return GetStationByNameAsync(name).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 透過車站英文名稱非同步取得車站資訊
        /// </summary>
        /// <param name="id">車站名稱</param>
        /// <returns>車站資訊</returns>
        public static async Task<Station> GetStationByEnglishNameAsync(string name) {
            var result = (await GetAllStationsAsync()).Where(x => x.EnglishName == name).FirstOrDefault();
            if (result == null) throw new KeyNotFoundException("找不到指定車站");
            return result;
        }

        /// <summary>
        /// 透過車站英文名稱取得車站資訊
        /// </summary>
        /// <param name="id">車站名稱</param>
        /// <returns>車站資訊</returns>
        public static Station GetStationByEnglishName(string name) {
            return GetStationByEnglishNameAsync(name).GetAwaiter().GetResult();
        }


        /// <summary>
        /// 自JSON物件轉換為車站資訊物件
        /// </summary>
        /// <param name="json">JSON物件</param>
        /// <returns>車站資訊物件</returns>
        internal static Station Parse(JObject json) {
            Station result = new Station();
            result.Id = int.Parse("0" + json["Station_Code(4)"].Value<string>());
            result.ShortId = int.Parse("0" + json["Station_Code(3)"].Value<string>());

            result.Name = json["Station_Name"].Value<string>();
            result.EnglishName = json["Station_EName"].Value<string>();
            result.Address = json["住址"].Value<string>();
            result.EnglishAddress = json["EnglishAddress"].Value<string>();

            result.Phone = json["電話"].Value<string>();
            if (result.Phone == "無") result.Phone = null;
            
            Position position = null;
            Position.TryParse(json["gps"].Value<string>(),out position);
            result.Position = position;

            #region fix 三姓橋
            if(result.Name == "三姓橋") {
                result.Id = 1035;
            }
            #endregion

            return result;
        }

        /// <summary>
        /// 嘗試自JSON物件轉換為車站資訊物件
        /// </summary>
        /// <param name="json">JSON物件</param>
        /// <param name="result">車站資訊物件</param>
        /// <returns>轉換是否成功</returns>
        internal static bool TryParse(JObject json, Station result) {
            try {
                result = Parse(json);
                return true;
            } catch {
                result = null;
                return false;
            }
        }

        /// <summary>
        /// 車站資訊快取類別
        /// </summary>
        internal static class Cache {
            /// <summary>
            /// 資料來源網址
            /// </summary>
            private static Uri DataSource = new Uri("http://www.railway.gov.tw/Upload/UserFiles/%E8%BB%8A%E7%AB%99%E5%9F%BA%E6%9C%AC%E8%B3%87%E6%96%99.json");

            /// <summary>
            /// 快取資料集合
            /// </summary>
            public static Dictionary<int, Station> StationDictionary { get; set; }

            /// <summary>
            /// 清除快取資料
            /// </summary>
            public static void ClearCache() {
                StationDictionary = new Dictionary<int, Station>();
            }

            /// <summary>
            /// 自資料來源進行非同步資料讀取動作
            /// </summary>
            /// <returns></returns>
            public static async Task LoadAsync() {
                HttpClient client = new HttpClient();
                //取得結果並轉換為UTF8編碼
                //Encoding.get
                string response = Encoding.GetEncoding("Big5")
                    .GetString(await client.GetByteArrayAsync(DataSource));
                
                var stationList = JArray.Parse(response)
                    .Select(x => Station.Parse(x.Value<JObject>()))
                    .ToList();
                
                ClearCache();

                foreach(var station in stationList) {
                    StationDictionary[station.Id] = station;
                }                
            }

            /// <summary>
            /// 自資料來源進行資料讀取動作
            /// </summary>
            public static void Load() {
                LoadAsync().GetAwaiter();
            }
        }
    }
}
