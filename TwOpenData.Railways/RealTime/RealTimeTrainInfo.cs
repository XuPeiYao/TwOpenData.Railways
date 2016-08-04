using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TwOpenData.Railways.RealTime {
    /// <summary>
    /// 即時
    /// 列車資訊
    /// </summary>
    public class RealTimeTrainInfo {
        private Uri DataSource;

        /// <summary>
        /// 列車資訊
        /// </summary>
        public Train Train { get; private set; }

        /// <summary>
        /// 最後通過車站
        /// </summary>
        public Station LastPassed { get; private set; }

        /// <summary>
        /// 列車延遲時間
        /// </summary>
        public TimeSpan Delay { get; private set; }


        private string innerString(string obj,string start,string end) {
            var result = obj;
            result = result.Substring(obj.IndexOf(start) + start.Length);
            result = result.Substring(0, result.IndexOf(end));
            return result;
        }

        /// <summary>
        /// 非同步刷新最新資訊
        /// </summary>
        /// <returns></returns>
        public async Task RefreshAsync() {
            HttpClient client = new HttpClient();
            HtmlDocument HTMLDoc = new HtmlDocument();
            HTMLDoc.LoadHtml(await client.GetStringAsync(DataSource));

            var script = HTMLDoc.DocumentNode.Descendants("script")
                .Where(x => x.InnerHtml?.Length > 0).Select(x => x.InnerHtml).ToArray();

            var tempAry = script.First()
                .Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select((x, i) => new { index = i, item = x })
                .GroupBy(x => Math.Floor(x.index / 4.0));

            this.LastPassed = null;
            this.Delay = new TimeSpan();

            foreach (var item in tempAry) {
                string[] temp = item.Select(x=>x.item).ToArray();
                if(temp[3] == "TRSearchResult.push('x')") {
                    this.LastPassed = await Station.GetStationByNameAsync(
                        innerString(temp[0],"'","'")
                        );
                }                
            }

            var time = new TimeSpan(0, int.Parse(innerString(script.Last(), "=", ";")),0);
            this.Delay= time;
        }

        /// <summary>
        /// 刷新最新資訊
        /// </summary>
        public void Refresh() {
            RefreshAsync().GetAwaiter();
        }

        public static async Task<RealTimeTrainInfo> GetRealTimeTrainInfoAsync(Train train) {
            if (train.Timetable.Date.Date != DateTime.Now.Date) {
                throw new InvalidOperationException("來源列車資訊並非來自今日時刻表");
            }
            RealTimeTrainInfo result = new RealTimeTrainInfo();
            result.Train = train;
            string date = DateTime.Now.ToString("yyyy/MM/dd");
            result.DataSource = new Uri($"http://twtraffic.tra.gov.tw/twrail/mobile/ie_traindetail.aspx?searchdate={date}&traincode={train.Id}");

            await result.RefreshAsync();
            return result;
        }

        public static RealTimeTrainInfo GetRealTimeTrainInfo(Train train) {
            return GetRealTimeTrainInfoAsync(train).GetAwaiter().GetResult();
        }
    }
}
