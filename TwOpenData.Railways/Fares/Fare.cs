using SharpCompress.Archive.Rar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TwOpenData.Railways.Fares {
    /// <summary>
    /// 票價資訊
    /// </summary>
    public class Fare {
        private int _Starting;
        private int _Arrival;


        private Station _Cache_Starting;
        /// <summary>
        /// 起始站
        /// </summary>
        public Station Starting {
            get {
                return _Cache_Starting ?? (_Cache_Starting = Station.GetStationByShortId(_Starting));
            }
        }

        private Station _Cache_Arrival;
        /// <summary>
        /// 到達站
        /// </summary>
        public Station Arrival {
            get {
                return _Cache_Arrival ?? (_Cache_Arrival = Station.GetStationByShortId(_Arrival));
            }
        }

        /// <summary>
        /// 列車方向
        /// </summary>
        public TrainDirection Direction { get; set; }

        /// <summary>
        /// 列車等級
        /// </summary>
        public TrainLevels TrainType { get; set; }

        /// <summary>
        /// 票價類型
        /// </summary>
        public FareTypes FareType { get; set; }

        /// <summary>
        /// 里程
        /// </summary>
        public double Mileage { get; private set; }

        /// <summary>
        /// 票價
        /// </summary>
        public int Price { get; private set; }

        /// <summary>
        /// 非同步取得兩站點間的票價
        /// </summary>
        /// <param name="starting">起始站</param>
        /// <param name="arrival">到達站</param>
        /// <param name="trainType">列車類型</param>
        /// <param name="fareType">票價類型</param>
        /// <returns>票價</returns>
        public static async Task<int> GetFaresPriceAsync(Station starting, Station arrival, TrainLevels trainType, FareTypes fareType) {
            return (await GetFaresAsync(starting, arrival))
                .Where(x => x.TrainType == TrainTypesConverter.Convert(trainType) && x.FareType == fareType)
                .First().Price;
        }

        /// <summary>
        /// 取得兩站點間的票價
        /// </summary>
        /// <param name="starting">起始站</param>
        /// <param name="arrival">到達站</param>
        /// <param name="trainType">列車類型</param>
        /// <param name="fareType">票價類型</param>
        /// <returns>票價</returns>
        public static int GetFaresPrice(Station starting, Station arrival, TrainLevels trainType, FareTypes fareType) {
            return GetFaresPriceAsync(starting, arrival, trainType, fareType).GetAwaiter().GetResult();
        }
        
        /// <summary>
        /// 非同步取得指定起始站與到達站的票價資訊
        /// </summary>
        /// <param name="starting">起始站</param>
        /// <param name="arrival">到達站</param>
        /// <returns>票價資訊</returns>
        public static async Task<Fare[]> GetFaresAsync(Station starting, Station arrival) {
            if (Cache.FaresData == null || Cache.FaresData.Count == 0) await Cache.LoadAsync();

            return Cache.FaresData.Where(x =>
                    x._Starting == starting.ShortId &&
                    x._Arrival == arrival.ShortId)
                .ToArray();
        }

        /// <summary>
        /// 非同步取得指定起始站與到達站的票價資訊
        /// </summary>
        /// <param name="starting">起始站</param>
        /// <param name="arrival">到達站</param>
        /// <param name="direction">列車行駛方向</param>
        /// <returns>票價資訊</returns>
        public static async Task<Fare[]> GetFaresAsync(Station starting, Station arrival, TrainDirection direction) {
            return (await GetFaresAsync(starting, arrival)).Where(x => x.Direction == direction).ToArray();
        }

        /// <summary>
        /// 取得指定起始站與到達站的票價資訊
        /// </summary>
        /// <param name="starting">起始站</param>
        /// <param name="arrival">到達站</param>
        /// <returns>票價資訊</returns>
        public static Fare[] GetFares(Station starting, Station arrival) {
            return GetFaresAsync(starting, arrival).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 取得指定起始站與到達站的票價資訊
        /// </summary>
        /// <param name="starting">起始站</param>
        /// <param name="arrival">到達站</param>
        /// <param name="direction">列車行駛方向</param>
        /// <returns>票價資訊</returns>
        public static Fare[] GetFares(Station starting, Station arrival, TrainDirection direction) {
            return GetFaresAsync(starting, arrival, direction).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 自台鐵票價資訊之CSV列轉換為票價資訊
        /// </summary>
        /// <param name="csvString">CSV列</param>
        /// <returns>票價資訊</returns>
        internal static Fare[] Parse(string csvString) {
            var splited = csvString.Split(new char[] { ',', '　' }, StringSplitOptions.RemoveEmptyEntries);
            var baseInfo = splited.Take(4).Select(x => double.Parse(x)).ToArray();
            List<string[]> fareInfo = new List<string[]>();
            for (int i = 4; i < splited.Length - 2; i += 5) {
                fareInfo.Add(splited.Skip(i).Take(3).ToArray());
            }

            List<Fare> result = new List<Fare>(); ;
            for (int i = 0; i < fareInfo.Count; i++) {
                for (int j = 0; j < 3; j++) {
                    var item = new Fare();
                    item._Starting = (int)baseInfo[0];
                    item._Arrival = (int)baseInfo[1];
                    item.Direction = (TrainDirection)(int)baseInfo[2];
                    item.Mileage = baseInfo[3];
                    item.TrainType = (TrainLevels)(i * 10);
                    item.FareType = (FareTypes)j;
                    item.Price = int.Parse(fareInfo[i][j]);

                    result.Add(item);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// 票價資訊快取類別
        /// </summary>
        internal static class Cache {
            /// <summary>
            /// 資料來源網址
            /// </summary>
            private static Uri DataSource = new Uri("http://www.railway.gov.tw/Upload/UserFiles/1050722%E7%A5%A8%E5%83%B9%E6%AA%94%E6%9B%B4%E6%96%B0.rar");

            /// <summary>
            /// 快取資料集合
            /// </summary>
            public static List<Fare> FaresData = null;

            /// <summary>
            /// 清除快取
            /// </summary>
            public static void ClearCache() {
                FaresData = new List<Fare>();
            }

            /// <summary>
            /// 自資料來源進行非同步資料讀取動作
            /// </summary>
            /// <returns></returns>
            public static async Task LoadAsync() {
                HttpClient client = new HttpClient();
                ClearCache();
                var rar = await client.GetByteArrayAsync(DataSource);
                using (MemoryStream stream = new MemoryStream(rar)) {
                    var archive = RarArchive.Open(stream);
                    var file = archive.Entries.Where(x => x.Key == "WK_FARE").First();
                    StreamReader reader = new StreamReader(file.OpenEntryStream());
                    while (!reader.EndOfStream) {
                        FaresData.AddRange(Fare.Parse(await reader.ReadLineAsync()));
                    }
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