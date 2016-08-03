using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwOpenData.Railways;
using TwOpenData.Railways.Fares;
using TwOpenData.Railways.Extensions;
namespace Test {
    class Program {
        static void Main(string[] args) {
            /*var result = Station.GetStationByName("臺北")
                .GetFares(Station.GetStationByName("臺南"));*/

            var table =  Timetable.GetTimetableByDate(DateTime.Now);
            var C = table.Trains.Where(x => x.Level == TrainLevels.Local).ToArray();

            var C2 = C.Where(x => x.Origin.Station.Name == "嘉義").ToArray();
            var C3 = C2.Where(x=>x.Direction == TrainDirection.CCKW).ToArray();


            var C4 = Station.GetStationByName("臺南").GetFares(Station.GetStationByName("嘉義"));


        }
    }
}
