using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwOpenData.Railways.Comparers {
    public class StationComparer : IEqualityComparer<Station> {
        public bool Equals(Station x, Station y) {
            return x == y;
        }

        public int GetHashCode(Station obj) {
            return obj.Id;
        }
    }
}
