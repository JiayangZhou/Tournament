using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public static class MatchupLogic //Static classes are sealed and therefore cannot be inherited
    {
        //Cannot declare a variable of static type calss
        private static Random rng = new Random();
        //private static List<Team> reallyMatchup = new List<Team>();
        //private static Team emptyTeam = new Team() { TeamName=""};
        //Order the teams list randomly
        //Check if list has enough number of teams and if teams number is 2 ^ Round
        //Create the first round
        //Create the rounds after

        public static void CreateRounds(Tournament model)
        {
            List<Team> randomizedTeams = RandomizeTeams(model.EnteredTeams);
            int rounds = FindOutRounds(randomizedTeams);
            int n = randomizedTeams.Count;
            int m = (int)Math.Pow(2, rounds);
            int emptyTeamsNum = m - n;
            //for (int i = 0; i < emptyTeamsNum; i++)
            //{
            //    randomizedTeams.Add(emptyTeam);
            //}
            model.Rounds.Add(CreateFirstRound(emptyTeamsNum, randomizedTeams));
            OtherRounds(model, rounds);

        }

        private static void OtherRounds(Tournament model, int rounds)
        {
            int round = 2;
            int count = 0;
            List<Matchup> previousRound = model.Rounds[count];
            List<Matchup> currentRound = new List<Matchup>();
            Matchup current = new Matchup();
            while (round<=rounds)
            {
                foreach (Matchup matchup in previousRound)
                {
                    
                    current.Entries.Add(new MatchupEntry { ParentMatchup = matchup });
                    if (current.Entries.Count>1)
                    {
                        current.Round = round;
                        currentRound.Add(current);
                        current = new Matchup();
                    }                  
                }
                round++;
                //count++;
                //model.Rounds[count] = currentRound;
                model.Rounds.Add(currentRound);
                previousRound = currentRound;
                currentRound= new List<Matchup>();
            }           
        }

        private static List<Matchup> CreateFirstRound(int emptyTeams, List<Team> teams)
        {
            List<Matchup> output = new List<Matchup>();
            Matchup current = new Matchup();
            //reallyMatchup = teams.Skip(emptyTeams);
            foreach (Team team in teams)
            {
                current.Entries.Add(new MatchupEntry { TeamCompeting = team });
                if (emptyTeams>0||current.Entries.Count>1)
                {
                    current.Round = 1;
                    output.Add(current);
                    current = new Matchup();
                    if (emptyTeams>0)  
                    {
                        emptyTeams--;
                    }
                }
                
            }
            //Put every team from first round in pairs to List<Matchup>
            return output;
        }

        private static int FindOutRounds(List<Team> teams)
        {
            int n = teams.Count;
            int rounds = 0;
            while (n >= Math.Pow(2,rounds))
            {
                rounds++;
            }
            return rounds;
        }

        private static List<Team> RandomizeTeams(List<Team> teams)
        {
            //List<string> cards = new List<string>();
            //List<string> shuffledcards = cards.OrderBy(a => Guid.NewGuid()).ToList();
            int n = teams.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);//n+1 stands maxValue
                Team value = teams[k];
                teams[k] = teams[n]; //teams[n] is the last element in the list
                teams[n] = value;
            }
            return teams;
        }
    }
}
