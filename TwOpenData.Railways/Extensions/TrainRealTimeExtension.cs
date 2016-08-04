using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwOpenData.Railways.RealTime;

namespace TwOpenData.Railways.Extensions {
    public static class TrainRealTimeExtension {
        public static async Task<RealTimeTrainInfo> GetRealTimeInfoAsync(this Train train) {
            return await RealTimeTrainInfo.GetRealTimeTrainInfoAsync(train);
        }
        public static RealTimeTrainInfo GetRealTimeInfo(this Train train) {
            return train.GetRealTimeInfoAsync().GetAwaiter().GetResult();
        }
    }
}
