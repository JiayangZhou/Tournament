using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using TrackerLibrary.DataAccess.TextProcessor;

namespace TrackerLibrary.DataAccess
{
    public class TextConnector : IDataConnection
    {
        private const string PrizesFile = "Prizes.csv";
        private const string PeopleFile = "People.csv";
        private const string TeamsFile = "Teams.csv";
        private const string TournamentsFile = "Tournaments.csv";
        private const string MatchupsFile = "MatchupFiles.csv";
        private const string MatchupEntriesFile = "MatchupEntriesFile.csv";
        //private int matchupId = 0;
        //private int entryId = 0;

        public Person CreatePerson(Person model)
        {
            List<Person> person = PeopleFile.FullFilePath().LoadFile().ConvertToPerson();

            int currentId = 1;

            if (person.Count > 0)
            {
                currentId = person.OrderByDescending(x => x.Id).First().Id + 1;
                //In the case that you are not passing anything in and you wanna accord to "x" from inside to order it
            }

            model.Id = currentId;
            person.Add(model);
            person.SaveToPersonFile(PeopleFile);           
            return model; 
        }

        public Prize CreatePrize(Prize model)
        {
            //Load the file
            //Convert the file to List<Prize>
            //Find IDs and max ID
            List<Prize> prizes = PrizesFile.FullFilePath().LoadFile().ConvertToPrize();

            int currentId = 1;

            if (prizes.Count>0)
            {
                currentId= prizes.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;         
            //Add the new record with new ID (max + 1)

            prizes.Add(model);
            prizes.SaveToPrizeFile(PrizesFile);
            //Convert List<Prize> to List<string>
            //Save to text file            
            return model;

        }

        public Team CreateTeam(Team model)
        {
            List<Team> team = TeamsFile.FullFilePath().LoadFile().ConvertToTeam();

            int currentId = 1;

            if (team.Count > 0)
            {
                currentId = team.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;
            //Add the new record with new ID (max + 1)

            team.Add(model);
            team.SaveToTeamFile(TeamsFile);
            //Convert List<Prize> to List<string>
            //Save to text file            
           

            return model;
        }

        public Tournament CreateTournament(Tournament model)
        {


            List<Tournament> tournament = TournamentsFile.FullFilePath().LoadFile().ConvertToTournament();

            int currentId = 1;

            if (tournament.Count > 0)
            {
                currentId = tournament.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;

            List<Matchup> matchupsHere= MatchupsFile.FullFilePath().LoadFile().ConvertToMatchup();
            List<MatchupEntry> matchupEntriesHere = MatchupEntriesFile.FullFilePath().LoadFile().ConvertToMatchupEntry();
            //Add the new record with new ID (max + 1)
            int currentMatchupId = 1;
            int currentMatchupEntryId = 1;
            foreach (List<Matchup> matchups in model.Rounds)
            {
                if (matchupsHere.Count > 0 && currentMatchupId == 1)
                {
                    currentMatchupId = matchupsHere.OrderByDescending(x => x.Id).First().Id + 1;
                }
               
                foreach (Matchup matchup in matchups)
                {
                    matchup.Id = currentMatchupId;
                    currentMatchupId++;
                    if (matchupEntriesHere.Count > 0 && currentMatchupEntryId == 1)
                    {
                        currentMatchupEntryId = matchupEntriesHere.OrderByDescending(x => x.Id).First().Id + 1;
                    }
                    foreach (MatchupEntry entry in matchup.Entries)
                    {
                        entry.Id = currentMatchupEntryId;
                        currentMatchupEntryId++;
                    }
                }

            }

            foreach (List<Matchup> matchups in model.Rounds)
            {
                if (model.TournamentRoundsString != null)
                {
                    model.TournamentRoundsString = model.TournamentRoundsString + $"|";
                }
                foreach (Matchup matchup in matchups)
                {
                    foreach (MatchupEntry entry in matchup.Entries)
                    {
                        //id|id
                        if (matchup.entriesString=="")
                        {
                            matchup.entriesString = $"{entry.Id}";
                        }
                        else
                        {
                            matchup.entriesString = matchup.entriesString + $"|{entry.Id}";
                        }
                        
                        if (entry.ParentMatchup!=null)
                        {
                            entry.ParentId = entry.ParentMatchup.Id; 
                        }
                    }

                    if (matchup==matchups.Last())
                    {
                        model.TournamentRoundsString = model.TournamentRoundsString + $"{matchup.Id}";
                    }
                    else
                    {
                        model.TournamentRoundsString = model.TournamentRoundsString + $"{matchup.Id}^";
                    }
                    
                }
            }


            SaveRoundsFile(model, MatchupsFile, MatchupEntriesFile);


            tournament.Add(model);
            tournament.SaveToTournamentFile(TournamentsFile);          


            return model;
        }

        public static void SaveRoundsFile(Tournament tournament, string matchupsFile, string entriesFile)
        {
            List<Matchup> matchupsL= matchupsFile.FullFilePath().LoadFile().ConvertToMatchup();
            //List<MatchupEntry> matchupentriesL = entriesFile.FullFilePath().LoadFile().ConvertToMatchupEntry();

            List<string> lines = new List<string>();
            List<string> lines2 = new List<string>();
            foreach (Matchup matchup in matchupsL)
            {
                //Id,entriesString,winnerId,Round
                lines.Add($"{matchup.Id},{matchup.entriesString},{matchup.winnerId},{matchup.Round}");

                foreach (MatchupEntry entry in matchup.Entries)
                {
                    //Id,CompetingTeamId,Score,ParentId
                    lines2.Add($"{entry.Id},{entry.CompetingTeamId},{entry.Score},{entry.ParentId}");
                }
            }

            //List<List<Matchup>> matchups = tournament.Rounds;
            foreach (List<Matchup> matchups in tournament.Rounds)
            {
                foreach (Matchup matchup in matchups)
                {
                    //Id,entriesString,winnerId,Round
                    lines.Add($"{matchup.Id},{matchup.entriesString},{matchup.winnerId},{matchup.Round}");

                    foreach (MatchupEntry entry in matchup.Entries)
                    {
                        //Id,CompetingTeamId,Score,ParentId
                        lines2.Add($"{entry.Id},{entry.CompetingTeamId},{entry.Score},{entry.ParentId}");
                    }
                }              
                //Add @ after $ if you wanna add a line return 
            }
            File.WriteAllLines(matchupsFile.FullFilePath(), lines);
            File.WriteAllLines(entriesFile.FullFilePath(), lines2);
        }

        public List<Person> GetAllPerson()
        {
            List<Person> person = new List<Person>();
            return person = PeopleFile.FullFilePath().LoadFile().ConvertToPerson();
        }

        public List<Prize> GetAllPrizes()
        {
            List<Prize> prize = new List<Prize>();
            return prize = PrizesFile.FullFilePath().LoadFile().ConvertToPrize2();
        }

        public List<Team> GetAllTeams()
        {
            List<Team> team = new List<Team>();
            return team = TeamsFile.FullFilePath().LoadFile().ConvertToTeam();
        } 
    }   
}
