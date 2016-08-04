using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwOpenData.Railways.RealTime;

namespace TwOpenData.Railways.Extensions {
    public static class TrainRealTimeExtension {
        /// <summary>
        /// 非同步取得目前列車即時資訊
        /// </summary>
        /// <param name="train"></param>
        /// <returns>列車即時資訊</returns>
        public static async Task<RealTimeTrainInfo> GetRealTimeInfoAsync(this Train train) {
            return await RealTimeTrainInfo.GetRealTimeTrainInfoAsync(train);
        }

        /// <summary>
        /// 取得目前列車即時資訊
        /// </summary>
        /// <param name="train"></param>
        /// <returns>列車即時資訊</returns>
        public static RealTimeTrainInfo GetRealTimeInfo(this Train train) {
            return train.GetRealTimeInfoAsync().GetAwaiter().GetResult();
        }
    }
}
