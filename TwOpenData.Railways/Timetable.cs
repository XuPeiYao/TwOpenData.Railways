using Newtonsoft.Json.Linq;
using SharpCompress.Archive.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TwOpenData.Railways {
    /// <summary>
    /// 時刻表
    /// </summary>
    public class Timetable {
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; private set; }

        public Train[] Trains { get; private set; }


        public static async Task<Timetable> GetTimetableByDateAsync(DateTime date) {
            await Cache.LoadAsync(date);
            var key = date.ToString("yyyyMMdd");
            return Cache.Timetables[key];
        }

        public static Timetable GetTimetableByDate(DateTime date) {
            return GetTimetableByDateAsync(date).GetAwaiter().GetResult();
        }

        public static async Task<Timetable> GetTimetableByDateAsync(int year, int month, int day) {
            return await GetTimetableByDateAsync(new DateTime(year, month, day));
        }

        public static Timetable GetTimetableByDate(int year,int month,int day) {
            return GetTimetableByDateAsync(year, month, day).GetAwaiter().GetResult();
        }
        
        internal static Timetable Parse(JObject json,DateTime date) {
            var result = new Timetable();

            List<Train> temp = new List<Train>();
            foreach(var item in json["TrainInfos"].Value<JArray>()) {
                var newItem = Train.Parse(item.Value<JObject>(), date);
                newItem.Timetable = result;            
                temp.Add(newItem);
            }
            result.Trains = temp.ToArray();
            
            return result;
        }

        /// <summary>
        /// 時刻表快取類別
        /// </summary>
        internal static class Cache {
            /// <summary>
            /// 資料來源
            /// </summary>
            private static Uri DataSource = new Uri("http://163.29.3.98/json/");

            /// <summary>
            /// 快取資料
            /// </summary>
            public static Dictionary<string, Timetable> Timetables { get; set; } = new Dictionary<string, Timetable>();

            public static void ClearCache(DateTime date) {
                var key = date.ToString("yyyyMMdd");
                Timetables.Remove(key);
            }

            public static async Task LoadAsync(DateTime date) {
                if (date.Date < DateTime.Now.Date) throw new ArgumentException("無法存取過去的時刻表");
                var key = date.ToString("yyyyMMdd");
                if (Timetables.ContainsKey(key)) {
                    ClearCache(date);
                }

                HttpClient client = new HttpClient();
                using (MemoryStream stream = new MemoryStream(
                    await client.GetByteArrayAsync(DataSource + key + ".zip")
                    )) {
                    var archive = ZipArchive.Open(stream);
                    var file = archive.Entries.Where(x => x.Key == key + ".json").First();
                    StreamReader reader = new StreamReader(file.OpenEntryStream());
                    var result = Timetable.Parse(JObject.Parse(await reader.ReadLineAsync()),date);
                    result.Date = date;
                    
                    Timetables[key] = result;
                }
            }

            public static void Load(DateTime date) {
                LoadAsync(date).GetAwaiter();
            }
        }
    }
}
