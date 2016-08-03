using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwOpenData.Railways.Comparers {
    public class TrainComparer : IEqualityComparer<Train> {
        public bool Equals(Train x, Train y) {
            return x == y;
        }

        public int GetHashCode(Train obj) {
            return obj.Id;
        }
    }
}
