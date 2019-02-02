using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public class SqlConnector : IDataConnection
    {
        private const string db = "Tournament";

        public Person CreatePerson(Person model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnString(db)))
            {

                var p = new DynamicParameters();
                p.Add("@FirstName", model.FirstName);
                p.Add("@LastName", model.LastName);
                p.Add("@EmailAddress", model.EmailAddress);
                p.Add("@CellphoeNumber", model.CellphoeNumber);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spPerson_Insert", p, commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("@id");
                return model;
            }
        }

        public Prize CreatePrize(Prize model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnString(db)))
            {

                var p = new DynamicParameters();
                p.Add("@PlaceNumber",model.PlaceNumber);
                p.Add("@PlaceName", model.PlaceName);
                p.Add("@PrizeAmount",model.PrizeAmount);
                p.Add("@PrizePercentage", model.PrizePercentage);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spPrize_Insert", p, commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("@id");
                return model;
            }
        }

        public Team CreateTeam(Team model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnString(db)))
            {

                var p = new DynamicParameters();
                p.Add("@TeamName", model.TeamName);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spTeam_Insert", p, commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("@id");

                foreach (Person tempp in model.TeamMembers)
                {
                    p = new DynamicParameters();
                    p.Add("@TeamId", model.Id);
                    p.Add("@PersonId", tempp.Id);
                    p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                    connection.Execute("dbo.spTeamMembers_Insert", p, commandType: CommandType.StoredProcedure);

                }
                return model;
            }
        }

        public Tournament CreateTournament(Tournament model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnString(db)))
            {

                var p = new DynamicParameters();
                p.Add("@TournamentName", model.TournamentName);
                p.Add("@EntryFee", model.EntryFee);
                p.Add("@Active", model.Active);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);


                connection.Execute("dbo.spTournament_Insert", p, commandType: CommandType.StoredProcedure);
                model.Id = p.Get<int>("@id");

                foreach (Team t in model.EnteredTeams)
                {
                    p = new DynamicParameters();
                    p.Add("@TournamentId", model.Id);
                    p.Add("@TeamId", t.Id);
                    p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                    connection.Execute("dbo.spTournamentEntry_Insert", p, commandType: CommandType.StoredProcedure);
                }

                foreach (Prize z in model.Prizes)
                {
                    p = new DynamicParameters();
                    p.Add("@TournamentId", model.Id);
                    p.Add("@PrizeId", z.Id);
                    p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                    connection.Execute("spTournamentPrize_Insert", p, commandType: CommandType.StoredProcedure);
                }

                //List<List<Matchup>> Rounds
                //List<MatchupEntry> Entries
                //Loop through the rounds
                //Loop through the matchups
                //Save the matchup
                //Loop through the entries and save them
                foreach (List<Matchup> matchups in model.Rounds)
                {
                    foreach (Matchup matchup in matchups)
                    {
                        p = new DynamicParameters();
                        p.Add("@TournamentId", model.Id);
                        p.Add("@MatchupRound", matchup.Round);
                        p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                        connection.Execute("spMatchup_Insert", p, commandType: CommandType.StoredProcedure);
                        matchup.Id = p.Get<int>("@id");

                        foreach (MatchupEntry entry in matchup.Entries)
                        {
                            p = new DynamicParameters();
                            p.Add("@MatchupId", matchup.Id);                          
                            if (entry.ParentMatchup == null)
                            {
                                p.Add("@ParentMatchupId", null);
                            }
                            else
                            {
                                p.Add("@ParentMatchupId", entry.ParentMatchup.Id);
                            }
                            if (entry.TeamCompeting==null)
                            {
                                p.Add("@TeamCompetingId", null);
                            }
                            else
                            {
                                p.Add("@TeamCompetingId", entry.TeamCompeting.Id);
                            }
                            p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                            connection.Execute("spMatchupEntry_Insert", p, commandType: CommandType.StoredProcedure);
                            entry.Id = p.Get<int>("@id");
                        }
                    }


                }
                return model;
            }
        }

        public void UpdateMatchups(Matchup model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnString(db)))
            {

                var p = new DynamicParameters();
                p.Add("@winnerId", model.winnerId);
                p.Add("@id", model.Id);

                connection.Execute("dbo.spMatchup_Update", p, commandType: CommandType.StoredProcedure);

                foreach (MatchupEntry entry in model.Entries)
                {
                    p = new DynamicParameters();
                    p.Add("@id", entry.Id);
                    if (entry.CompetingTeamId!=0)
                    {
                        p.Add("@CompetingTeamId", entry.CompetingTeamId); 
                    }
                    if (entry.Score!=0)
                    {
                        p.Add("@Score", entry.Score); 
                    }

                    connection.Execute("dbo.spMatchupEntry_Update", p, commandType: CommandType.StoredProcedure);

                }               
            }
        }

        public List<Person> GetAllPerson()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnString(db)))
            {
                return connection.Query<Person>("dbo.spGetAllPerson").ToList();
               
            }
        }

        public List<Prize> GetAllPrizes()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnString(db)))
            {
                return connection.Query<Prize>("dbo.spGetAllPrizes").ToList();

            }
        }

        public List<Team> GetAllTeams()
        {
            List<Team> teams = new List<Team>();
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnString(db)))
            {
                teams=connection.Query<Team>("dbo.spGetAllTeams").ToList();
                foreach (Team team in teams)
                {
                    //var p = new DynamicParameters();
                    //p.Add("@TeamId", team.Id);
                    //team.TeamMembers = connection.Query<Person>("dbo.spTeamMembers_GetByTeam", p, commandType: CommandType.StoredProcedure).ToList();
                    team.TeamMembers = connection.Query<Person>("dbo.spTeamMembers_GetByTeam @TeamId", new { TeamId = team.Id }).ToList();//Both are working
                }
            }

            return teams;
        }

        public List<Tournament> GetAllTournaments()
        {
            List<Tournament> tournaments = new List<Tournament>();
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnString(db)))
            {
                tournaments = connection.Query<Tournament>("dbo.spGetAllTournaments").ToList();
                foreach (Tournament tournament in tournaments)
                {
                    var p = new DynamicParameters();
                    p.Add("@TournamentId", tournament.Id);
                    tournament.EnteredTeams = connection.Query<Team>("dbo.spTeam_GetByTournament", p, commandType: CommandType.StoredProcedure).ToList();
                    foreach (Team team in tournament.EnteredTeams)
                    {
                        team.TeamMembers = connection.Query<Person>("dbo.spTeamMembers_GetByTeam @TeamId", new { TeamId = team.Id }).ToList();
                    }
                    tournament.Prizes= connection.Query<Prize>("dbo.spPrize_GetByTournament", p, commandType: CommandType.StoredProcedure).ToList();
                    List<Matchup> matchups = new List<Matchup>();
                    matchups = connection.Query<Matchup>("dbo.spMatchup_GetByTournament", p, commandType: CommandType.StoredProcedure).ToList();
                    foreach (Matchup matchup in matchups)
                    {
                        p = new DynamicParameters();
                        p.Add("@MatchupId", matchup.Id); 
                        matchup.Entries=connection.Query<MatchupEntry>("spMatchupEntry_GetByMatchup", p, commandType: CommandType.StoredProcedure).ToList();
                    }
                    //A list of Matchup
                    //Convert to a list of List<Matchup> according to the rounds
                    //int n = tournament.EnteredTeams.Count;
                    //int rounds = 0;
                    //while (n >= Math.Pow(2, rounds))
                    //{
                    //    rounds++;
                    //}
                    int roundsReversed =1;
                    List<List<Matchup>> matchupsL = new List<List<Matchup>>();
                    List<Matchup> receiver = new List<Matchup>();
                    int counter = 0;
                    foreach (Matchup matchup in matchups)
                    {
                        counter++;
                        if (matchup.Round == roundsReversed)
                        {
                            receiver.Add(matchup);

                            if (counter == matchups.Count)
                            {
                                matchupsL.Add(receiver);

                            }
                        }
                        else
                        {
                            roundsReversed++;
                            matchupsL.Add(receiver);
                            receiver = new List<Matchup>();
                            if (matchup.Round == roundsReversed)
                            {
                                receiver.Add(matchup);

                            }
                            if (counter == matchups.Count)
                            {
                                matchupsL.Add(receiver);

                            }
                        }
                    }
                    tournament.Rounds = matchupsL;

                }
            }

            return tournaments;
        }

        
    } 
}
