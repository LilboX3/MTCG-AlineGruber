using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models
{
    internal class TradingDeal
    {
        //Id of the deal
        public string Id {  get; set; }
        //Id of card offered
        public string CardToTrade { get; set; }
        //Required card type
        public string Type { get; set; }
        //Required min damage

    }
}
