using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwOpenData.Railways;

namespace Test {
    class Program {
        static void Main(string[] args) {
            Station[] stationList = Station.GetAllStations();
        }
    }
}
