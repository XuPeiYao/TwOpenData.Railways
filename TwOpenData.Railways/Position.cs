using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwOpenData.Railways {
    /// <summary>
    /// 經緯度位置
    /// </summary>
    public class Position {
        /// <summary>
        /// 經度
        /// </summary>
        public double X { get; private set; }

        /// <summary>
        /// 緯度
        /// </summary>
        public double Y { get; private set; }
        
        internal Position(double x, double y) {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// 由經緯度字串轉換為物件
        /// </summary>
        /// <param name="positionString">經緯度字串</param>
        /// <returns>位置物件</returns>
        internal static Position Parse(string positionString) {
            try {
                var pair = positionString.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x=>double.Parse(x))
                    .ToArray();
                return new Position(pair[0], pair[1]);
            } catch {
                throw new FormatException();
            }
        }

        /// <summary>
        /// 嘗試由經緯度字串轉換為物件
        /// </summary>
        /// <param name="positionString">經緯度字串</param>
        /// <param name="result">位置物件</param>
        /// <returns>轉換是否成功</returns>
        internal static bool TryParse(string positionString, out Position result) {
            try {
                result = Parse(positionString);
                return true;
            } catch {
                result = null;
                return false;
            }
        }
    }
}
