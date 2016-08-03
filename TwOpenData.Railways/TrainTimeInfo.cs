using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwOpenData.Railways {
    /// <summary>
    /// 列車到站時間資訊
    /// </summary>
    public class TrainTimeInfo {
        private int _Station;
        /// <summary>
        /// 出發時間
        /// </summary>
        public DateTime Departure { get; internal set; }

        /// <summary>
        /// 到站時間
        /// </summary>
        public DateTime Arrival { get; internal set; }

        private Station _Cache_Station;

        /// <summary>
        /// 車站資訊
        /// </summary>
        public Station Station {
            get {
                return _Cache_Station ?? (_Cache_Station = Station.GetStationById(_Station));
            }
        }

        /// <summary>
        /// 將JSON格式的資料轉換為列車到站時間資訊物件
        /// </summary>
        /// <param name="json">資料來源</param>
        /// <param name="date">日期</param>
        /// <returns>列車到站時間資訊物件</returns>
        internal static TrainTimeInfo Parse(JObject json , DateTime date) {
            TrainTimeInfo result = new TrainTimeInfo();
            result._Station = int.Parse(json["Station"].Value<string>());
            result.Departure = date.Date + TimeSpan.Parse(json["DepTime"].Value<string>());
            result.Arrival = date.Date + TimeSpan.Parse(json["ArrTime"].Value<string>());
            return result;
        }
    }
}
