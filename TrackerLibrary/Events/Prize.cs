using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public class Prize
    {
        public int Id { get; set; }
        public int PlaceNumber { get; set; }
        public string PlaceName { get; set; }
        public decimal PrizeAmount { get; set; }
        public double PrizePercentage { get; set; }
        public string BriefInfo
        {
            get
            {
                return $"{PlaceName} {PrizeAmount} {PrizePercentage}";
            }

        }


        public Prize()
        {

        }
        
        public Prize(string placeNumber, string placeName, string prizeAmount, string prizePercentage)
        {
            PlaceName = placeName;
            int placenumber = 0;
            bool placenumberExtract = int.TryParse(placeNumber, out placenumber);
            PlaceNumber = placenumber;

            int prizeamount = 0;
            bool prizeamountExtract = int.TryParse(prizeAmount, out prizeamount);
            PrizeAmount = prizeamount;

            double prizepercentage = 0;
            bool prizepercentageExtract = double.TryParse(prizePercentage, out prizepercentage);
            PrizePercentage = prizepercentage;
        }
    }
}
