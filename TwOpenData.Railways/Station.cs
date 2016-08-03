﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
        
        /*
        public static int GetFares(Station start, Station end, TrainTypes type = TrainTypes.區間) {

        }*/

        /// <summary>
        /// 透過車站編號非同步取得車站資訊
        /// </summary>
        /// <param name="id">車站編號</param>
        /// <returns></returns>
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
        /// <returns></returns>
        public static Station GetStationById(int id) {
            return GetStationByIdAsync(id).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 非同步取得所有車站資訊
        /// </summary>
        /// <returns>所有車站資訊</returns>
        public static async Task<Station[]> GetAllStationsAsync() {
            if (Cache.StationDictionary == null) await Cache.LoadAsync();
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
        /// 自JSON物件轉換為車站資訊物件
        /// </summary>
        /// <param name="json">JSON物件</param>
        /// <returns>車站資訊物件</returns>
        internal static Station Parse(JObject json) {
            Station result = new Station();
            result.Id = int.Parse("0" + json["Station_Code(4)"].Value<string>());
            result.Name = json["Station_Name"].Value<string>();
            result.EnglishName = json["Station_EName"].Value<string>();
            result.Address = json["住址"].Value<string>();
            result.EnglishAddress = json["EnglishAddress"].Value<string>();

            result.Phone = json["電話"].Value<string>();
            if (result.Phone == "無") result.Phone = null;

            Position position = null;
            Position.TryParse(json["gps"].Value<string>(),out position);
            result.Position = position;

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