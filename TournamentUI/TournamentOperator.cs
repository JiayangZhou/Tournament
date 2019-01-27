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

        public TournamentOperator(Tournament tournament)
        {
            InitializeComponent();
            OpTournament = tournament;
            tournamentNameLabel.Text = OpTournament.TournamentName;
            //ProcessTeams = OpTournament.EnteredTeams;
            //MatchupLogic.CreateRounds(OpTournament);

            InitializeMatchup();
        }
        private void InitializeMatchup()
        {

        }

        private void leaveThisTournamentButton_Click(object sender, EventArgs e)
        {

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
