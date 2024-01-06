using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MTCG.Routing
{
    public class IdRouteParser
    {
        public bool IsMatch(string resourcePath, string routePattern)
        {
            //zB website.com/search?key=keyword&page=1 --> erkennt search als id, rest als query parameter
            var pattern = "^" + routePattern.Replace("{id}", ".*").Replace("/", "\\/") + "(\\?.*)?$"; //sucht ? (nicht regex zeichen) kann auch keine query parameter haben (? am ende)
            //{id} mit .* ersetzen (alle möglichen zeichen beliebig oft), / mit \/ ersetzen weil / spezielle bedeutung, mehr \ am ende mit weiteren zeicvhen möglich (query parameter)
            return Regex.IsMatch(resourcePath, pattern);
            //-> schaun ob path diesem Muster entspricht
        }

        public Dictionary<string, string> ParseParameters(string resourcePath, string routePattern)
        {
            // query parameters
            var parameters = ParseQueryParameters(resourcePath);

            // id parameter
            var id = ParseIdParameter(resourcePath, routePattern);
            if (id != null)
            {
                parameters["id"] = id;
            }

            return parameters;
        }

        private string? ParseIdParameter(string resourcePath, string routePattern)
        {
            //{id} mit pattern ersetzen, das extrahiert werden soll
            var pattern = "^" + routePattern.Replace("{id}", "(?<id>[^\\?\\/]*)").Replace("/", "\\/") + "$";
            //extrahiert id (alles außer schrägstrich und fragezeichen)
            var result = Regex.Match(resourcePath, pattern);
            //extrahierten string aus "id" zurückgeben
            return result.Groups["id"].Success ? result.Groups["id"].Value : null;
        }
        
        public bool IsFormatPlainMatch(string resourcePath, string routePattern)
        {
            //has /deck with ?format=plain ?
            var pattern = "^" + routePattern.Replace("/", "\\/") + "(\\?format=plain)?$";
            //should not be path just /deck
            var deckPattern = "^" + routePattern.Replace("/", "\\/") + "$";

            return Regex.IsMatch(resourcePath, pattern) && !Regex.IsMatch(resourcePath, deckPattern);
        }


        private Dictionary<string, string> ParseQueryParameters(string route)
        {
            var parameters = new Dictionary<string, string>();

            var query = route
                .Split("?", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Skip(1)
                .FirstOrDefault();

            if (query != null)
            {
                var keyValues = query
                    .Split("&", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Select(p => p.Split("=", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
                    .Where(p => p.Length == 2);

                foreach (var p in keyValues)
                {
                    parameters[p[0]] = p[1];
                }
            }

            return parameters;
        }

    }
}
