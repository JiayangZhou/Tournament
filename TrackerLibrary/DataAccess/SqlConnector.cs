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
    } 
}
