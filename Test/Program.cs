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
            var result = Station.GetStationByName("臺北")
                .GetFares(Station.GetStationByName("臺南"));
        }
    }
}
