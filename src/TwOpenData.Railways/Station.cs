using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwOpenData.Railways {
    /// <summary>
    /// 車站
    /// </summary>
    public class Station {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EnglishName { get; set; }

        public static int GetFares(Station start,Station end,TrainTypes type= TrainTypes.區間) {

        }
    }
}
