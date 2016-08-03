using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwOpenData.Railways {
    /// <summary>
    /// 將列車類型轉換為主要列車類型(如普悠瑪為自強號)
    /// </summary>
    public class TrainTypesConverter {
        /// <summary>
        /// 將列車類型轉換為主要列車類型(如普悠瑪為自強號)
        /// </summary>
        /// <param name="type">列車類型</param>
        /// <returns>主要列車類型</returns>
        public TrainTypes Convert(TrainTypes type) {
            return (TrainTypes)((int)Math.Round(((int)type)/ 10.0) * 10);
        }
    }
}
