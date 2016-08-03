using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwOpenData.Railways {
    /// <summary>
    /// 將列車類型轉換為主要列車類型(如普悠瑪為自強號)
    /// </summary>
    public static class TrainTypesConverter {
        /// <summary>
        /// 將列車類型轉換為主要列車類型(如普悠瑪為自強號)
        /// </summary>
        /// <param name="type">列車等級</param>
        /// <returns>主要列車等級</returns>
        public static TrainLevels Convert(TrainLevels type) {
            return (TrainLevels)((int)Math.Round(((int)type)/ 10.0) * 10);
        }

        /// <summary>
        /// 由列車型號轉換為列車等級
        /// </summary>
        /// <param name="type">列車型號</param>
        /// <returns>列車等級</returns>
        public static TrainLevels Convert(int type) {
            switch (type) {
                case 1102:
                    return TrainLevels.TzeChiang_Tarko;
                case 1107:
                    return TrainLevels.TzeChiang_Puyuma;                
            }
            if (type >= 1100 && type <= 1108) return TrainLevels.TzeChiang;
            if (type >= 1110 && type <= 1115) return TrainLevels.ChuKuang;
            if (type == 1120) return TrainLevels.FuHsing;
            if (type == 1131) return TrainLevels.Local;
            if (type == 1132) return TrainLevels.FastLocal;
            return TrainLevels.Ordinary;
        }
    }
}
