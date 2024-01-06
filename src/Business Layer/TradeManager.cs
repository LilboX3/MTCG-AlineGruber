using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Data_Layer;

namespace MTCG.Business_Layer
{
    public class TradeManager
    {
        private readonly TradeDao _tradeDao;

        public TradeManager(TradeDao tradeDao)
        {
            _tradeDao = tradeDao;
        }
    }
}
