using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Data_Layer;
using MTCG.HttpServer;
using MTCG.Models;
using Newtonsoft.Json;

namespace MTCG.Business_Layer
{
    public class ScoreboardManager
    {
        private readonly ScoreboardDao _scoreboardDao;

        public ScoreboardManager(ScoreboardDao scoreboardDao)
        {
            _scoreboardDao = scoreboardDao;
        }

        public HttpResponse GetScoreboard(Dictionary<string,string> headers)
        {
            if (!headers.ContainsKey("Authorization:"))
            {
                return new HttpResponse(StatusCode.Unauthorized, "Unauthorized error");
            }

            UserStats[] scoreboard = _scoreboardDao.GetScoreboard();
            return new HttpResponse(StatusCode.Ok, "The scoreboard could be retrieved successfully.\n"
                                                   + JsonConvert.SerializeObject(scoreboard));
        }
    }
}
