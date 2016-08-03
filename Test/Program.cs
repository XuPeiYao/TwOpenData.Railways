using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwOpenData.Railways;
using TwOpenData.Railways.Fares;
using TwOpenData.Railways.Extensions;
using TwOpenData.Railways.Comparers;

namespace Test {
    class Program {
        static void Main(string[] args) {
            var tainanTodayTrains = Station.GetStationByName("臺南").GetTrains(DateTime.Now).ToArray();
            var KSTodayTrains = Station.GetStationByName("高雄").GetTrains(DateTime.Now).ToArray();

            var tainanToKS = tainanTodayTrains.Intersect(KSTodayTrains,new TrainComparer());

            //高雄南下區間到高雄
            var localTrains = tainanToKS.Where(x => x.Level == TrainLevels.Local)
                .Where(x=>x.Direction == TrainDirection.CCKW);
        }
    }
}
