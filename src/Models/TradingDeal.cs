using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models
{
    public class TradingDeal
    {
        //Id of the deal
        public string Id {  get; set; }
        //Id of card offered
        public string CardToTrade { get; set; }
        //Required card type
        public string Type { get; set; }
        //Required min damage
        public double MinDamage { get; set; }
        //Userid of owner
        public int UserId { get; set; }

        public TradingDeal(string tradeId, string type, double minDamage, string cardToTrade, int userId)
        {
            Id = tradeId;
            Type = type;
            MinDamage = minDamage;
            CardToTrade = cardToTrade;
            UserId = userId;
        }

    }
}
