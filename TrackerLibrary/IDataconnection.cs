using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public interface IDataConnection
    {
        Prize CreatePrize(Prize model);
        Person CreatePerson(Person model);
        Team CreateTeam(Team model);
        List<Person> GetAllPerson();
        List<Team> GetAllTeams();
        List<Prize> GetAllPrizes();
        Tournament CreateTournament(Tournament model);
        List<Tournament> GetAllTournaments();
        void UpdateMatchups(Matchup model);
    }
}
