using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Data_Layer;

namespace MTCG.Business_Layer
{
    public class CardManager
    {
        private readonly CardDao _cardDao;

        public CardManager(CardDao cardDao)
        {
            _cardDao = cardDao;
        }
        
        
    }
}
