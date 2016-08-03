using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwOpenData.Railways.Extensions {
    public static class StationTimetableExtension {
        /// <summary>
        /// 非同步取得指定日期經目前車站的列車資訊
        /// </summary>
        /// <param name="station"></param>
        /// <param name="date">日期</param>
        /// <returns>列車資訊</returns>
        public static async Task<Train[]> GetTrainsAsync(this Station station,DateTime date) {
            var table = await Timetable.GetTimetableByDateAsync(date);
            return table.Trains.Where(x => x.StoppingAt.Any(y => y.Station == station))
                .OrderBy(x => x.Origin.Departure)
                .ToArray();
        }

        /// <summary>
        /// 取得指定日期經目前車站的列車資訊
        /// </summary>
        /// <param name="station"></param>
        /// <param name="date">日期</param>
        /// <returns>列車資訊</returns>
        public static Train[] GetTrains(this Station station, DateTime date) {
            return station.GetTrainsAsync(date).GetAwaiter().GetResult();
        }
    }
}
