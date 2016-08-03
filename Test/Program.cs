using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwOpenData.Railways;
using TwOpenData.Railways.Fares;

namespace Test {
    class Program {
        static void Main(string[] args) {
            var result = Fare.GetFares(
                Station.GetStationByName("臺北"),
                Station.GetStationByName("臺南"),
                TrainDirection.South
                );
        }
    }
}
