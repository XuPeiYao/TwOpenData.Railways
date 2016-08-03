using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwOpenData.Railways {
    /// <summary>
    /// 列車資訊
    /// </summary>
    public class Train {
        /// <summary>
        /// 隱含時刻表
        /// </summary>
        internal Timetable Timetable { get; set; }

        /// <summary>
        /// 車次
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 等級
        /// </summary>
        public TrainLevels Level { get; private set; }

        /// <summary>
        /// 類型
        /// </summary>
        public TrainTypes Type { get; private set; }

        /// <summary>
        /// 路線
        /// </summary>
        public TrainLines Line { get; private set; }

        /// <summary>
        /// 方向
        /// </summary>
        public TrainDirection Direction { get; private set; }

        /// <summary>
        /// 是否為跨夜列車
        /// </summary>
        public bool IsOverNightStn { get; private set; }

        /// <summary>
        /// 是否有殘障車
        /// </summary>
        public bool HasCripple { get; private set; }

        /// <summary>
        /// 是否可辦理託運
        /// </summary>
        public bool CanPackage { get; private set; }

        /// <summary>
        /// 是否有餐車
        /// </summary>
        public bool HasDinning { get; private set; }

        /// <summary>
        /// 是否有哺乳室
        /// </summary>
        public bool HasBreastFeed { get; private set; }

        /// <summary>
        /// 是否輸送腳踏車
        /// </summary>
        public bool Bike { get; private set; }

        /// <summary>
        /// 註記
        /// </summary>
        public string Note { get; private set; }

        /// <summary>
        /// 英文註記
        /// </summary>
        public string EnglishNote { get; private set; }

        /// <summary>
        /// 所有停靠站點
        /// </summary>
        public TrainTimeInfo[] StoppingAt { get; private set; }

        /// <summary>
        /// 發車資訊
        /// </summary>
        public TrainTimeInfo Origin => StoppingAt.First();

        /// <summary>
        /// 終點資訊
        /// </summary>
        public TrainTimeInfo Destination => StoppingAt.Last();

        #region 比較
        public override bool Equals(object obj) {
            var Obj = obj as Train;
            return Obj != null && Obj.Id == this.Id;
        }

        public static bool operator ==(Train a, Train b) {
            if (Object.ReferenceEquals(a, b)) return true;
            if (Object.Equals(a, b)) return true;
            if (Object.Equals(a, null) && !Object.Equals(b,null)) {
                return b.Equals(a);
            }
            if (Object.Equals(b, null) && !Object.Equals(a, null)) {
                return a.Equals(b);
            }
            return a.Equals(b);
        }

        public static bool operator !=(Train a, Train b) {
            return !(a == b);
        }
        #endregion

        /// <summary>
        /// 自JSON資料轉換為列車資訊物件
        /// </summary>
        /// <param name="json">資料來源</param>
        /// <param name="date">日期</param>
        /// <returns>列車資訊物件</returns>
        internal static Train Parse(JObject json, DateTime date) {
            Train result = new Train();
            result.Id = int.Parse(json["Train"].Value<string>());
            result.Level = TrainLevelsConverter.Convert(int.Parse(json["CarClass"].Value<string>()));
            result.Type = (TrainTypes)int.Parse("0" + json["Type"].Value<string>());
            result.Line = (TrainLines)int.Parse("0" + json["Route"].Value<string>());
            result.Direction = (TrainDirection)int.Parse("0" + json["LineDir"].Value<string>());
            result.IsOverNightStn = json["OverNightStn"].Value<string>() != "0";
            result.HasCripple = json["Cripple"].Value<string>() == "Y";
            result.CanPackage = json["Package"].Value<string>() == "Y";
            result.HasDinning = json["Dinning"].Value<string>() == "Y";
            result.HasBreastFeed = json["BreastFeed"].Value<string>() == "Y";
            result.Bike = json["Bike"].Value<string>() == "Y";
            result.Note = json["Note"].Value<string>();
            result.EnglishNote = json["NoteEng"].Value<string>();

            var objectArray = json["TimeInfos"].Value<JArray>();

            List<TrainTimeInfo> infos = new List<TrainTimeInfo>();
            foreach (var item in objectArray) {
                var newItem = TrainTimeInfo.Parse(item.Value<JObject>(), date);
                if (result.IsOverNightStn && newItem.Arrival.Hour <= 12) {
                    newItem.Arrival = newItem.Arrival.AddDays(1);
                    newItem.Departure = newItem.Departure.AddDays(1);
                }
                infos.Add(newItem);
            }
            result.StoppingAt = infos.ToArray();

            return result;
        }
    }
}