using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public class Tournament
    {
        public int Id { get; set; }
        public string TournamentName { get; set; }
        public decimal EntryFee { get; set; }
        public List<Team> EnteredTeams { get; set; } = new List<Team>();//When the property is a list, you got to give it a new List<Type>()
        public List<Prize> Prizes { get; set; } = new List<Prize>();
        public List<List<Matchup>> Rounds { get; set; } = new List<List<Matchup>>();
        public int Active = 0;
        public string TournamentTeamsString { get; set; }
        public string TournamentPrizesString { get; set; }
        public string TournamentRoundsString { get; set; }
    }
}
