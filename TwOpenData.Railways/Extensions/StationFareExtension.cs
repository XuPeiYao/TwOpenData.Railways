using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwOpenData.Railways.Fares;

namespace TwOpenData.Railways.Extensions {
    public static class StationFareExtension {
        /// <summary>
        /// 非同步取得本站到到達站的票價資訊
        /// </summary>
        /// <param name="arrival">到達站</param>
        /// <returns>票價資訊</returns>
        public static async Task<Fare[]> GetFaresAsync(this Station THIS, Station arrival) {
            return await Fare.GetFaresAsync(THIS, arrival);
        }

        /// <summary>
        /// 取得本站到到達站的票價資訊
        /// </summary>
        /// <param name="arrival">到達站</param>
        /// <returns>票價資訊</returns>
        public static Fare[] GetFares(this Station THIS, Station arrival) {
            return THIS.GetFaresAsync(arrival).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 非同步取得本站到到達站的票價資訊
        /// </summary>
        /// <param name="arrival">到達站</param>
        /// <param name="direction">列車方向</param>
        /// <returns>票價資訊</returns>
        public static async Task<Fare[]> GetFaresAsync(this Station THIS, Station arrival, TrainDirection direction) {
            return await Fare.GetFaresAsync(THIS, arrival, direction);
        }

        /// <summary>
        /// 取得本站到到達站的票價資訊
        /// </summary>
        /// <param name="arrival">到達站</param>
        /// <param name="direction">列車方向</param>
        /// <returns>票價資訊</returns>
        public static Fare[] GetFares(this Station THIS, Station arrival, TrainDirection direction) {
            return THIS.GetFaresAsync(arrival, direction).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 非同步取得本站與到達站間的票價
        /// </summary>
        /// <param name="arrival">到達站</param>
        /// <param name="trainType">列車類型</param>
        /// <param name="fareType">票價類型</param>
        /// <returns>票價</returns>
        public static async Task<int> GetFaresPriceAsync(this Station THIS, Station arrival, TrainTypes trainType, FareTypes fareType) {
            return await Fare.GetFaresPriceAsync(THIS, arrival, trainType, fareType);
        }

        /// <summary>
        /// 取得本站與到達站間的票價
        /// </summary>
        /// <param name="arrival">到達站</param>
        /// <param name="trainType">列車類型</param>
        /// <param name="fareType">票價類型</param>
        /// <returns>票價</returns>
        public static int GetFaresPrice(this Station THIS, Station arrival, TrainTypes trainType, FareTypes fareType) {
            return THIS.GetFaresPriceAsync(arrival, trainType, fareType).GetAwaiter().GetResult();
        }
    }
}
