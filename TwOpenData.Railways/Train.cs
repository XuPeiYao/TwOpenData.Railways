using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwOpenData.Railways {
    /// <summary>
    /// 列車資訊
    /// </summary>
    public class Train {
        /// <summary>
        /// 車次
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 列車等級
        /// </summary>
        public TrainLevels Type { get; private set; }


        /// <summary>
        /// 所有停靠站點
        /// </summary>
        public Station[] StoppingAt { get; private set; }

        /// <summary>
        /// 起點站
        /// </summary>
        public Station Origin => StoppingAt.First();

        /// <summary>
        /// 終點站
        /// </summary>
        public Station Destination => StoppingAt.Last();
        /*
        /// <summary>
        /// 取得列車於指定站點發車時間
        /// </summary>
        /// <param name="target">指定站點</param>
        /// <returns>發車時間</returns>
        public DateTime GetDepartureTime(Station target) {
            if (Array.IndexOf(this.StoppingAt, target) == -1) {
                throw new ArgumentException("輸入站點並未包含在此列車路徑");
            }
        }*/
        /*
        /// <summary>
        /// 取得列車到達指定站點時間
        /// </summary>
        /// <param name="target">指定站點</param>
        /// <returns>到達時間</returns>
        public DateTime GetArrivalTime(Station target) {
            if (Array.IndexOf(this.StoppingAt, target) == -1) {
                throw new ArgumentException("輸入站點並未包含在此列車路徑");
            }
        }
        *//*
        /// <summary>
        /// 取得兩站點間票價
        /// </summary>
        /// <param name="start">起始站點</param>
        /// <param name="end">結束站點</param>
        /// <returns>票價</returns>
        public int GetFares(Station start, Station end) {
            int startIndex = Array.IndexOf(this.StoppingAt, start);
            int endIndex = Array.IndexOf(this.StoppingAt, end);
            if(startIndex == -1 || endIndex ==-1) throw new ArgumentException("輸入站點並未包含在此列車路徑");
            if(startIndex > endIndex) throw new ArgumentException("輸入站點與行徑方向不符合");
            return Station.GetFares(start, end, Type);
        }
        */
        public Train() {

        }
        /*
        /// <summary>
        /// 使用車次取得列車資訊
        /// </summary>
        /// <param name="id">車次</param>
        /// <returns>列車資訊</returns>
        public static Train GetTrainById(int id) {

        }*/
    }
}