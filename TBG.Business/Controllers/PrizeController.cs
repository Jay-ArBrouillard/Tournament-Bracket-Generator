using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TBG.Business.Models;
using TBG.Core.Interfaces;

namespace TBG.Business.Controllers
{
    public class PrizeController : IPrizeController
    {
        public IPrize ValidatePrize(String inName, String inPerc)
        {

            if (inName == null || inPerc == null)
                return null;

            if (!isValidName(inName))
                return null;
            
            if (!isValidPercent(inPerc))
                return null;


            IPrize prize = buildPrize(inName, inPerc);
            return prize;
        }

        private IPrize buildPrize(String inName, String inPerc)
        {
            IPrize prize = new Prize();
            prize.PrizeAmount = 0;
            prize.PrizeName = inName;

             //Converts prize percent to a double
            decimal prizePerc = 0;
            if (inPerc.Contains("%"))
            {
                string temp = inPerc.Substring(0, inPerc.Length - 1);
                prizePerc = (decimal.Parse(temp)) / 100;
            }
            else
            {
                prizePerc = decimal.Parse(inPerc);
            }

            prize.PrizePercent = prizePerc;
            return prize;
        }

        private bool isValidName(string inName)
        {
            Match match = Regex.Match(inName, @"[\\\/:\[\]\(\)\=]");
            if (match.Success)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool isValidPercent(string inPerc)
        {
            Match match = Regex.Match(inPerc, @"\b(?<!\.)(?!0+(?:\.0+)?%)(?:\d|[1-9]\d|100)(?:(?<!100)\.\d+)?%");
            Match match2 = Regex.Match(inPerc, @".\d{2}");
            if (match.Success || match2.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
