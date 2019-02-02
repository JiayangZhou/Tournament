using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;

namespace TournamentUI
{
    public partial class TournamentOperator : Form
    {
        public Tournament OpTournament = new Tournament();

        //public Team emptyTeam = new Team() { TeamName = "EmptyTeam" };
        //public List<Team> ProcessTeams = new List<Team>();
        //public int teamNumber=0;
        //public int count = 0;
        //public int round = 1;
        ////public int totalRound = 0;
        //public List<Team> WinnerTeams = new List<Team>();
        //List<Team> MatchupTeams = new List<Team>();
        //Team first = new Team();
        //Team second = new Team();
        //float flo = 3.02f;

        public List<Matchup> avaliableMatchups = new List<Matchup>();
        int matchupIndex = 0;
        Team finalWinner = new Team();
        //int roundIndex = 1;
        //Matchup selectedOne = new Matchup();

        public TournamentOperator(Tournament reciver)
        {
            InitializeComponent();
            //OpTournament = tournament;
            
            //ProcessTeams = OpTournament.EnteredTeams;
            //MatchupLogic.CreateRounds(OpTournament);
            OpTournament = GiveTheCompeting(reciver);
            tournamentNameLabel.Text = OpTournament.TournamentName;
            foreach (List<Matchup> matchups in OpTournament.Rounds)
            {
                avaliableMatchups.AddRange(matchups);
            }
            avaliableMatchups = GiveParent(avaliableMatchups);
            matchupListBox.DataSource = avaliableMatchups;
            matchupListBox.DisplayMember = "Id";
            Matchup firstMatchup = new Matchup();
            firstMatchup = avaliableMatchups.First();

            if (firstMatchup.Entries.First().CompetingTeamId!=0)
            {
                Team teamDisplay = new Team();
                foreach (Team team in OpTournament.EnteredTeams)
                {
                    if (team.Id == firstMatchup.Entries.First().CompetingTeamId)
                    {
                        teamDisplay = team;
                    }
                }
                teamOneNameLabel.Text = teamDisplay.TeamName; 
            }
            else
            {
                teamOneNameLabel.Text = "Bot";
            }

            if (firstMatchup.Entries.Count > 1)
            {
                if (firstMatchup.Entries[1].CompetingTeamId != 0)
                {
                    Team teamDisplay = new Team();
                    foreach (Team team in OpTournament.EnteredTeams)
                    {
                        if (team.Id == firstMatchup.Entries.First().CompetingTeamId)
                        {
                            teamDisplay = team;
                        }
                    }
                    teamTwoNameLabel.Text = teamDisplay.TeamName;

                }
            }
            else
            {
                teamTwoNameLabel.Text = "Bot";
            }

            int n = OpTournament.EnteredTeams.Count;
            int rounds = 0;
            while (n > Math.Pow(2, rounds))
            {
                rounds++;
            }
            int roundsReversed = 1;
            int[] roundsArray =new int[rounds];
            for (int i = 0; i < rounds; i++)
            {
                roundsArray[i] = roundsReversed;
                roundsReversed++;
            }

            roundDropDown.DataSource = roundsArray;
        }

        private void unplayedOnlyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void roundDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateListBox((int)roundDropDown.SelectedItem);
        }
        private void UpdateListBox(int round)
        {
            matchupListBox.DataSource = null;
            List<Matchup> matchups = new List<Matchup>();
            foreach (Matchup matchup in avaliableMatchups)
            {
                if (matchup.Round==round)
                {
                    matchups.Add(matchup);
                }
            }
            matchupListBox.DataSource = matchups;
            matchupListBox.DisplayMember = "Id";

        }

        public List<Matchup> GiveParent(List<Matchup> matchups)
        {
            List<Matchup> ms =new List<Matchup>();
            Matchup m = new Matchup();
            foreach (Matchup matchup in matchups)
            {
                
                foreach (MatchupEntry matchupEntry in matchup.Entries)
                {
                    if (matchupEntry.ParentId!=0)
                    {
                        matchupEntry.ParentMatchup = FindMatchupById(matchupEntry.ParentId);
                    }
                }
            }
            
            return matchups;
        }

        private Matchup FindMatchupById(int parentId)
        {
            Matchup output = new Matchup();
            List<Matchup> av = new List<Matchup>();
            foreach (List<Matchup> matchups in OpTournament.Rounds)
            {
                av.AddRange(matchups);
            }

            foreach (Matchup matchup in av)
            {
                if (matchup.Id==parentId)
                {
                    output = matchup;
                    break;
                }
            }

            return output;
        }

        private void LoadMatchup()
        {
            matchupIndex++;
            teamOneScoreValue.Text = "";
            teamTwoScoreValue.Text = "";
            if ((matchupIndex+1)<=avaliableMatchups.Count)
            {
                if (avaliableMatchups[matchupIndex].Entries[0].TeamCompeting==null)
                {
                    if (avaliableMatchups[matchupIndex].Entries[0].ParentMatchup!=null)
                    {
                        avaliableMatchups[matchupIndex].Entries[0].TeamCompeting = avaliableMatchups[matchupIndex].Entries[0].ParentMatchup.winner;
                        avaliableMatchups[matchupIndex].Entries[0].CompetingTeamId = avaliableMatchups[matchupIndex].Entries[0].ParentMatchup.winner.Id;
                    }
                }

                if (avaliableMatchups[matchupIndex].Entries.Count > 1)
                {
                    if (avaliableMatchups[matchupIndex].Entries[1].TeamCompeting == null)
                    {
                        if (avaliableMatchups[matchupIndex].Entries[1].ParentMatchup != null)
                        {
                            avaliableMatchups[matchupIndex].Entries[1].TeamCompeting = avaliableMatchups[matchupIndex].Entries[1].ParentMatchup.winner;
                            avaliableMatchups[matchupIndex].Entries[1].CompetingTeamId = avaliableMatchups[matchupIndex].Entries[1].ParentMatchup.winner.Id;
                        }
                    } 
                }

                if (avaliableMatchups[matchupIndex].Entries.First().TeamCompeting!=null)
                {
                    //Team teamDisplay = new Team();
                    //foreach (Team team in OpTournament.EnteredTeams)
                    //{
                    //    if (team.Id == avaliableMatchups[matchupIndex].Entries.First().CompetingTeamId)
                    //    {
                    //        teamDisplay = team;
                    //    }
                    //}
                    teamOneNameLabel.Text = avaliableMatchups[matchupIndex].Entries.First().TeamCompeting.TeamName;
                }
                else
                {
                    teamOneNameLabel.Text = "Bot";
                }

                if (avaliableMatchups[matchupIndex].Entries.Count > 1)
                {
                    //if (avaliableMatchups[matchupIndex].Entries[1].CompetingTeamId != 0)
                    //{
                    //    Team teamDisplay = new Team();
                    //    foreach (Team team in OpTournament.EnteredTeams)
                    //    {
                    //        if (team.Id == avaliableMatchups[matchupIndex].Entries[1].CompetingTeamId)
                    //        {
                    //            teamDisplay = team;
                    //        }
                    //    }
                    //    teamTwoNameLabel.Text = teamDisplay.TeamName;

                    //}
                    if (avaliableMatchups[matchupIndex].Entries.First().TeamCompeting != null)
                    {
                        teamTwoNameLabel.Text = avaliableMatchups[matchupIndex].Entries[1].TeamCompeting.TeamName;
                    }
                }
                else
                {
                    teamTwoNameLabel.Text = "Bot";
                }
            }
            else
            {
                MessageBox.Show("Rounds finished!");
                
                OpTournament = PutThingsTogether(OpTournament, avaliableMatchups);

                foreach (Matchup matchup in avaliableMatchups)
                {
                    GlobalConfig.Connections.UpdateMatchups(matchup);
                }
                

                finalWinner = avaliableMatchups[matchupIndex - 1].winner;
            }
        }

        private Tournament PutThingsTogether(Tournament tournament, List<Matchup> avaliableMatchups)
        {
            int n = tournament.EnteredTeams.Count;
            int rounds = 0;
            while (n > Math.Pow(2, rounds))
            {
                rounds++;
            }
            int roundsReversed = 1;
            List<List<Matchup>> matchupsL = new List<List<Matchup>>();
            List<Matchup> ms = new List<Matchup>();
            int counter = 0;
            foreach (Matchup matchup in avaliableMatchups)
            {
                counter++;
                if (roundsReversed == matchup.Round)
                {
                    ms.Add(matchup);
                    if (counter == avaliableMatchups.Count)
                    {
                        matchupsL.Add(ms);
                    }
                }
                else
                {
                    roundsReversed++;
                    matchupsL.Add(ms);
                    ms = new List<Matchup>();
                    if (roundsReversed == matchup.Round)
                    {
                        ms.Add(matchup);
                    }
                    if (counter == avaliableMatchups.Count)
                    {
                        matchupsL.Add(ms);
                    }
                }
            }

            tournament.Rounds = matchupsL;
            return tournament;
        }


        private void scoreButton_Click(object sender, EventArgs e)
        {
            avaliableMatchups[matchupIndex].Entries[0].Score = double.Parse(teamOneScoreValue.Text);
            if (avaliableMatchups[matchupIndex].Entries.Count>1)
            {
                avaliableMatchups[matchupIndex].Entries[1].Score = double.Parse(teamTwoScoreValue.Text);

                if (avaliableMatchups[matchupIndex].Entries[0].Score > avaliableMatchups[matchupIndex].Entries[1].Score)
                {
                    avaliableMatchups[matchupIndex].winner = avaliableMatchups[matchupIndex].Entries[0].TeamCompeting;
                    avaliableMatchups[matchupIndex].winnerId = avaliableMatchups[matchupIndex].winner.Id;
                }
                else
                {
                    avaliableMatchups[matchupIndex].winner = avaliableMatchups[matchupIndex].Entries[1].TeamCompeting;
                    avaliableMatchups[matchupIndex].winnerId = avaliableMatchups[matchupIndex].winner.Id;
                }

            }
            else
            {
                avaliableMatchups[matchupIndex].winner = avaliableMatchups[matchupIndex].Entries[0].TeamCompeting;
                avaliableMatchups[matchupIndex].winnerId = avaliableMatchups[matchupIndex].winner.Id;
            }



            LoadMatchup();
        }

        private void leaveThisTournamentButton_Click(object sender, EventArgs e)
        {

        }

        private Tournament GiveTheCompeting(Tournament tournament)
        {
            List<Matchup> avMatchups = new List<Matchup>();
            foreach (List<Matchup> matchups in tournament.Rounds)
            {
                avMatchups.AddRange(matchups);
            }

            foreach (Matchup matchup in avMatchups)
            {


                Team team1 = new Team();
                foreach (Team team in tournament.EnteredTeams)
                {
                    if (team.Id == matchup.Entries[0].CompetingTeamId)
                    {
                        team1 = team;
                    }
                }
                if (matchup.Entries[0].CompetingTeamId !=0)
                {
                    matchup.Entries[0].TeamCompeting = team1; 
                }
                if (matchup.Entries.Count > 1)
                {
                    Team team2 = new Team();
                    foreach (Team team in tournament.EnteredTeams)
                    {
                        if (team.Id == matchup.Entries[1].CompetingTeamId)
                        {
                            team2 = team;
                        }
                    }
                    if (matchup.Entries[1].CompetingTeamId !=0)
                    {
                        matchup.Entries[1].TeamCompeting = team2; 
                    }
                }

            }
            int n = tournament.EnteredTeams.Count;
            int rounds = 0;
            while (n > Math.Pow(2, rounds))
            {
                rounds++;
            }
            int roundsReversed = 1;
            List<List<Matchup>> matchupsL = new List<List<Matchup>>();
            List<Matchup> ms = new List<Matchup>();
            int counter = 0;
            foreach (Matchup matchup in avMatchups)
            {
                counter++;
                if (roundsReversed==matchup.Round)
                {
                    ms.Add(matchup);
                    if (counter == avMatchups.Count)
                    {
                        matchupsL.Add(ms);
                    }
                }
                else
                {
                    roundsReversed++;
                    matchupsL.Add(ms);
                    ms = new List<Matchup>();
                    if (roundsReversed == matchup.Round)
                    {
                        ms.Add(matchup);
                    }
                    if (counter== avMatchups.Count)
                    {
                        matchupsL.Add(ms);
                    }
                }
            }

            tournament.Rounds = matchupsL;
            return tournament;
        }









        //public void InitializeMatchup()
        //{
        //    teamNumber = OpTournament.EnteredTeams.Count();
        //    if (OpTournament.EnteredTeams.Count % 2 == 1)
        //    {
        //        OpTournament.EnteredTeams.Add(emptyTeam);
        //        teamNumber++;
        //    }

        //    first = OpTournament.EnteredTeams.First();
        //    OpTournament.EnteredTeams.RemoveAt(0);
        //    second = OpTournament.EnteredTeams.First();
        //    teamOneNameLabel.Text = first.TeamName;
        //    teamTwoNameLabel.Text = second.TeamName;

        //}
        //public void GetTwoTeams()
        //{
        //    List<Team> matchup = new List<Team>();
        //    List<Team> matchupName = new List<Team>();
        //    if (count * 2 != teamNumber)
        //    {
        //        matchup.AddRange(ProcessTeams.Skip(count * 2).Take(2));
        //        matchupName = matchup;

        //        //if (teamNumber % 2 != 0)
        //        //{
        //        //    ProcessTeams.Add(emptyTeam);
        //        //}
        //        count++;
        //        first = matchup.First();
        //        matchup.RemoveAt(0);
        //        second = matchup.First();


        //        teamOneNameLabel.Text = matchupName.First().TeamName;
        //        matchupName.RemoveAt(0);
        //        teamTwoNameLabel.Text=matchupName.First().TeamName;
        //        //MessageBox.Show("This is the last round"); 
        //    }
        //    else
        //    {
        //        MessageBox.Show("Current round Finshed");
        //        teamNumber = WinnerTeams.Count();
        //        ProcessTeams = WinnerTeams;
        //        count = 0;
        //        round++;
        //    }

        //    //return matchup;
        //}

        //private void scoreButton_Click(object sender, EventArgs e)
        //{

        //    Team winner = new Team();

        //    GetTwoTeams();           
        //    first.Score = int.Parse(teamOneScoreValue.Text);
        //    second.Score = int.Parse(teamTwoScoreValue.Text);
        //    if (first.Score>second.Score)
        //    {
        //        winner = first;
        //    }
        //    else if (second.Score> first.Score)
        //    {
        //        winner = second;
        //    }
        //    WinnerTeams.Add(winner);

        //    teamOneScoreValue.Text = "";
        //    teamTwoScoreValue.Text = "";

        //}
    }
}
