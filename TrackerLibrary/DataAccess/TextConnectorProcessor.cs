using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.DataAccess.TextProcessor
{
    public static class TextConnectorProcessor
    {
        private const string PeopleFile = "People.csv";
        private const string PrizesFile = "Prizes.csv";
        private const string TeamsFile = "Teams.csv";
        private const string MatchupsFile = "MatchupFiles.csv";
        private const string MatchupEntriesFile = "MatchupEntriesFile.csv";
        //Load the file or Create a new List<string>
        //Convert the file to List<Prize> 
        //Find IDs and max ID
        //Add the new record with new ID (max + 1)
        //Convert List<Prize> to List<string>
        //Save to text file

        public static string FullFilePath(this string fileName) //PrizeModels.csv
        {
            return $"{ConfigurationManager.AppSettings["filePath"]}\\{fileName}";
        }

        public static List<string> LoadFile(this string file)
        {
            if (!File.Exists(file))
            {
                return new List<string>();
            }
            return File.ReadAllLines(file).ToList();
        }

        public static List<Prize> ConvertToPrize(this List<string> lines)
        {
            List<Prize> output = new List<Prize>();
            foreach (string line in lines)
            {
                if (line!="")
                {
                    string[] cols = line.Split(',');
                    Prize p = new Prize();
                    p.Id = int.Parse(cols[0]);
                    p.PlaceNumber = int.Parse(cols[1]);
                    p.PlaceName = cols[2];
                    p.PrizeAmount = decimal.Parse(cols[3]);
                    p.PrizePercentage = double.Parse(cols[4]);
                    output.Add(p); 
                }
            }

            return output;
        }
        public static List<Prize> ConvertToPrize2(this List<string> lines)
        {
            List<Prize> output = new List<Prize>();
            foreach (string line in lines)
            {
                if (line != "")
                {
                    string[] cols = line.Split(',');
                    Prize p = new Prize();
                    p.Id = int.Parse(cols[0]);
                    p.PlaceNumber = int.Parse(cols[1]);
                    p.PlaceName = cols[2];
                    p.PrizeAmount = decimal.Parse(cols[3]);
                    p.PrizePercentage = double.Parse(cols[4]);
                    output.Add(p); 
                }
            }

            return output;
        }
        public static void SaveToPrizeFile(this List<Prize> models, string fileName)
        {
            List<string> lines = new List<string>();
            foreach (Prize p in models)
            {
                lines.Add($"{p.Id},{p.PlaceNumber},{p.PlaceName},{p.PrizeAmount},{p.PrizePercentage}");
            }
            File.WriteAllLines(fileName.FullFilePath(),lines);
        }

        public static List<Person> ConvertToPerson(this List<string> lines)
        {
            List<Person> output = new List<Person>();
            foreach (string line in lines)
            {
                if (line!="")
                {
                    string[] cols = line.Split(',');
                    Person p = new Person();
                    p.Id = int.Parse(cols[0]);
                    p.FirstName = cols[1];
                    p.LastName = cols[2];
                    p.EmailAddress = cols[4];
                    p.CellphoeNumber = cols[3];
                    output.Add(p); 
                }
            }

            return output;
        }
        public static void SaveToPersonFile(this List<Person> models, string fileName)
        {
            List<string> lines = new List<string>();
            foreach (Person p in models)
            {
                lines.Add($"{p.Id},{p.FirstName},{p.LastName},{p.EmailAddress},{p.CellphoeNumber}");
            }
            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static List<Team> ConvertToTeam(this List<string> lines)
        {
            List<Team> output = new List<Team>();
            foreach (string line in lines)
            {
                if (line!="")
                {
                    //1,TeamFirst,1|2|3
                    string[] cols = line.Split(',');
                    Team p = new Team();
                    p.Id = int.Parse(cols[0]);
                    p.TeamName = cols[1];
                    p.TeamMembersString = cols[2];
                    string[] membersId = p.TeamMembersString.Split('|');
                    foreach (string member in membersId)
                    {
                        int memberId = int.Parse(member);
                        p.TeamMembers.Add(PeopleFile.FullFilePath().LoadFile().GetOnePerson((int)memberId));
                    }

                    output.Add(p); 
                }
            }

            return output;
        }
        public static void SaveToTeamFile(this List<Team> models, string fileName)
        {
            List<string> lines = new List<string>();
            foreach (Team p in models)
            {
                lines.Add($"{p.Id},{p.TeamName},{p.TeamMembersString}");
            }
            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static List<Tournament> ConvertToTournament(this List<string> lines)
        {
            List<Tournament> output = new List<Tournament>();
            foreach (string line in lines)
            {
                if (line!="")
                {
                    //1, TournamentFirst, 1|2|3 (Teams), 1|2|3 (Prizes),id^id^id^|id^id
                    string[] cols = line.Split(',');
                    Tournament p = new Tournament();
                    p.Id = int.Parse(cols[0]);
                    p.TournamentName = cols[1];
                    p.TournamentTeamsString = cols[2];
                    p.TournamentPrizesString = cols[3];
                    p.TournamentRoundsString = cols[4];
                    string[] teamsId = p.TournamentTeamsString.Split('|');
                    string[] prizesId = p.TournamentPrizesString.Split('|');
                    string[] roundsId = p.TournamentRoundsString.Split('|');
                    foreach (string teamId in teamsId)
                    {
                        int id = int.Parse(teamId);
                        p.EnteredTeams.Add(TeamsFile.FullFilePath().LoadFile().GetOneTeam(id));
                    }
                    foreach (string prizeId in prizesId)
                    {
                        int id = int.Parse(prizeId);
                        p.Prizes.Add(PrizesFile.FullFilePath().LoadFile().GetOnePrize(id));
                    }

                    foreach (string roundId in roundsId)
                    {
                        List<Matchup> rounds = new List<Matchup>();
                        string[] matchupsId = roundId.Split('^');
                        foreach (string matchupId  in matchupsId)
                        {
                            int id = int.Parse(matchupId);
                            rounds.Add(MatchupsFile.FullFilePath().LoadFile().GetOneMatchup(id));
                        }
                        p.Rounds.Add(rounds);
                    }
                    output.Add(p); 
                }
            }

            return output;
        }
        public static List<Matchup> ConvertToMatchup(this List<string> lines)
        {
            List<Matchup> output = new List<Matchup>();
            foreach (string line in lines)
            {
                if (line!="")
                {
                    //Id,id|id,winnerId,Round
                    string[] cols = line.Split(',');
                    Matchup p = new Matchup();
                    p.Id = int.Parse(cols[0]);
                    p.entriesString = cols[1];
                    p.winnerId = int.Parse(cols[2]);
                    p.Round = int.Parse(cols[3]);
                    string[] entries = p.entriesString.Split('|');
                    foreach (string entry in entries)
                    {

                        p.Entries.Add(MatchupEntriesFile.FullFilePath().LoadFile().GetOneMatchupEntry(int.Parse(entry)));
                    }
                    if (p.winnerId != 0)
                    {
                        p.winner = TeamsFile.FullFilePath().LoadFile().GetOneTeam(p.winnerId);
                    }
                    output.Add(p); 
                }
            }

            return output;
        }
        public static List<MatchupEntry> ConvertToMatchupEntry(this List<string> lines)
        {
            List<MatchupEntry> output = new List<MatchupEntry>();
            foreach (string line in lines)
            {
                if (line!="")
                {
                    //Id,CompetingTeamId,Score,ParentId
                    string[] cols = line.Split(',');
                    MatchupEntry p = new MatchupEntry();
                    p.Id = int.Parse(cols[0]);
                    p.Id = int.Parse(cols[0]);
                    p.CompetingTeamId = int.Parse(cols[1]);
                    p.Score = int.Parse(cols[2]);
                    p.ParentId = int.Parse(cols[3]);
                    if (p.CompetingTeamId != 0)
                    {
                        p.TeamCompeting = TeamsFile.FullFilePath().LoadFile().GetOneTeam(p.CompetingTeamId);
                    }
                    if (p.ParentId != 0)
                    {
                        p.ParentMatchup = MatchupsFile.FullFilePath().LoadFile().GetOneMatchup(p.ParentId);
                    }
                    output.Add(p); 
                }
            }

            return output;
        }
        public static void SaveToTournamentFile(this List<Tournament> models, string fileName)
        {
            List<string> lines = new List<string>();
            foreach (Tournament p in models)
            {
                lines.Add($"{p.Id},{p.TournamentName},{p.TournamentTeamsString},{p.TournamentPrizesString},{p.TournamentRoundsString}");
                //Add @ after $ if you wanna add a line return 
            }
            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        

        public static Matchup GetOneMatchup(this List<string> lines, int memberId)
        {
            Matchup output = new Matchup();
            foreach (string line in lines)
            {
                if (line!="")
                {
                    //Id,id|id,winnerId,Round
                    string[] cols = line.Split(',');
                    if (int.Parse(cols[0]) == memberId)
                    {
                        Matchup p = new Matchup();
                        p.Id = int.Parse(cols[0]);
                        p.entriesString = cols[1];
                        p.winnerId = int.Parse(cols[2]);
                        p.Round = int.Parse(cols[3]);
                        string[] entries = p.entriesString.Split('|');
                        foreach (string entry in entries)
                        {
                            if (entry!="")
                            {
                                p.Entries.Add(MatchupEntriesFile.FullFilePath().LoadFile().GetOneMatchupEntry(int.Parse(entry))); 
                            }
                        }
                        if (p.winnerId != 0)
                        {
                            p.winner = TeamsFile.FullFilePath().LoadFile().GetOneTeam(p.winnerId);
                        }
                        output = p;
                    } 
                }
            }

            return output;
        }
        public static MatchupEntry GetOneMatchupEntry(this List<string> lines, int memberId)
        {
            MatchupEntry output = new MatchupEntry();
            foreach (string line in lines)
            {
                if (line!="")
                {
                    string[] cols = line.Split(',');
                    if (int.Parse(cols[0]) == memberId)
                    {
                        //Id,CompetingTeamId,Score,ParentId
                        MatchupEntry p = new MatchupEntry();
                        p.Id = int.Parse(cols[0]);
                        p.CompetingTeamId = int.Parse(cols[1]);
                        p.Score = int.Parse(cols[2]);
                        p.ParentId = int.Parse(cols[3]);
                        if (p.CompetingTeamId != 0)
                        {
                            p.TeamCompeting = TeamsFile.FullFilePath().LoadFile().GetOneTeam(p.CompetingTeamId);
                        }
                        if (p.ParentId != 0)
                        {
                            p.ParentMatchup = MatchupsFile.FullFilePath().LoadFile().GetOneMatchup(p.ParentId);
                        }
                        output = p;
                    } 
                }
            }

            return output;
        }
        public static Person GetOnePerson(this List<string> lines, int memberId)
        {
            Person output = new Person();
            foreach (string line in lines)
            {
                if (line!="")
                {
                    string[] cols = line.Split(',');
                    if (int.Parse(cols[0]) == memberId)
                    {
                        Person p = new Person();
                        p.Id = int.Parse(cols[0]);
                        p.FirstName = cols[1];
                        p.LastName = cols[2];
                        p.EmailAddress = cols[4];
                        p.CellphoeNumber = cols[3];
                        output = p;
                    } 
                }
            }

            return output;
        }
        public static Team GetOneTeam(this List<string> lines, int teamId)
        {
            Team output = new Team();
            //1,TeamFirst,1|2|3
            foreach (string line in lines)
            {
                if (line!="")
                {
                    string[] cols = line.Split(',');
                    if (int.Parse(cols[0]) == teamId)
                    {
                        Team p = new Team();
                        p.Id = int.Parse(cols[0]);
                        p.TeamName = cols[1];
                        p.TeamMembersString = cols[2];
                        string[] members = p.TeamMembersString.Split('|');
                        foreach (string memberId in members)
                        {
                            int id = int.Parse(memberId);
                            Person person = new Person();
                            person = PeopleFile.FullFilePath().LoadFile().GetOnePerson(id);

                            p.TeamMembers.Add(person);
                        }
                        output = p; 
                    }
                }
            }

            return output;
        }
        public static Prize GetOnePrize(this List<string> lines, int prizeId)
        {
            Prize output = new Prize();
            foreach (string line in lines)
            {
                if (line!="")
                {
                    string[] cols = line.Split(',');
                    if (int.Parse(cols[0]) == prizeId)
                    {
                        Prize p = new Prize();
                        p.Id = int.Parse(cols[0]);
                        p.PlaceNumber = int.Parse(cols[1]);
                        p.PlaceName = cols[2];
                        p.PrizeAmount = decimal.Parse(cols[3]);
                        p.PrizePercentage = double.Parse(cols[4]);

                        output = p;
                    } 
                }
            }

            return output;
        }     
    }
}
