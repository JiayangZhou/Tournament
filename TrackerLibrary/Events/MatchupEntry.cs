using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public class MatchupEntry
    {
        public int Id { get; set; }
        public int CompetingTeamId { get; set; }
        public Team TeamCompeting { get; set; }
        public double Score { get; set; }
        public int ParentId { get; set; }
        public Matchup ParentMatchup { get; set; }
        //It has to have the same name as what you have in the Sql database
    }
}
