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
    public partial class CreateTournament : Form, IPrizeRequester,ITeamRequester
    {

        List<Team> avaliableTeams = new List<Team>();
        List<Team> selectedTeams = new List<Team>();
        List<Prize> selectedPrizes = new List<Prize>();
        ITournamentRequester receiver;
        public CreateTournament(ITournamentRequester passer)
        {
            InitializeComponent();
            InitializeTeamDropDown();
            InitializePrizesListbox();
            receiver = passer;
        }

        private void InitializeTeamDropDown()
        {
            avaliableTeams= GlobalConfig.Connections.GetAllTeams();
            selectTeamDropDown.DataSource = avaliableTeams;
            selectTeamDropDown.DisplayMember = "TeamName";

        }

        private void UpdateTeamDropDwon()
        {
            selectTeamDropDown.DataSource = null;
            selectTeamDropDown.DataSource = avaliableTeams;
            selectTeamDropDown.DisplayMember = "TeamName";
        }

        private void addTeamButton_Click(object sender, EventArgs e)
        {
            if ((Team)selectTeamDropDown.SelectedItem != null)
            {
                avaliableTeams.Remove((Team)selectTeamDropDown.SelectedItem);
                selectedTeams.Add((Team)selectTeamDropDown.SelectedItem);
                UpdateTeamsListBox();
                UpdateTeamDropDwon(); 
            }
            else
            {
                MessageBox.Show("Nothing selected");
            }
        }
        private void UpdateTeamsListBox()
        {
            teamsListBox.DataSource = null;
            teamsListBox.DataSource = selectedTeams;
            teamsListBox.DisplayMember = "TeamName";
        }

        private void deleteTeamButton_Click(object sender, EventArgs e)
        {
            if ((Team)teamsListBox.SelectedItem != null)
            {
                selectedTeams.Remove((Team)teamsListBox.SelectedItem);
                avaliableTeams.Add((Team)teamsListBox.SelectedItem);
                UpdateTeamsListBox();
                UpdateTeamDropDwon(); 
            }
            else
            {
                MessageBox.Show("Nothing selected");
            }
        }

        private void InitializePrizesListbox()
        {
            selectedPrizes = GlobalConfig.Connections.GetAllPrizes();
            prizesListBox.DataSource = selectedPrizes;
            prizesListBox.DisplayMember = "BriefInfo";
        }

        private void deletePrizeButton_Click(object sender, EventArgs e)
        {
            if ((Prize)prizesListBox.SelectedItem!=null)
            {
                selectedPrizes.Remove((Prize)prizesListBox.SelectedItem);
                UpdatePrizesListbox(); 
            }
            else
            {
                MessageBox.Show("Nothing selected");
            }
        }

        private void UpdatePrizesListbox()
        {
            prizesListBox.DataSource = null;
            prizesListBox.DataSource = selectedPrizes;
            prizesListBox.DisplayMember = "BriefInfo";
        }

        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            //Call the CreatePrize
            CreatePrize form = new CreatePrize(this);//this represents this specific instance
            form.Show();
            
        }

        public void PrizeComplete(Prize model)
        {
            selectedPrizes.Add(model);
            //prizesListBox.Update();
            UpdatePrizesListbox();          
        }

        private void createNewLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CreateTeam form = new CreateTeam(this);
            form.Show();
        }

        public void TeamComplete(Team model)
        {
            avaliableTeams.Add(model);
            UpdateTeamDropDwon();
        }

        private void createTournamentButton_Click(object sender, EventArgs e)
        {
            //Validate entryFeePerTeamValue.Text
            decimal fee = 0;
            bool feeAccept = decimal.TryParse(entryFeePerTeamValue.Text, out fee);
            if (!feeAccept)
            {
                MessageBox.Show("Enter a valid Entry Fee", "Invalid data", MessageBoxButtons.OK, MessageBoxIcon.Error);//Control + Shift + Space to check all overloads
            }
            //Control and Click to open a full window of overloads
            Tournament tm = new Tournament();
            tm.TournamentName = tournamentNameValue.Text;
            tm.EntryFee = 0;
            if (feeAccept)
            {
                tm.EntryFee = fee;
            }

            tm.EnteredTeams = selectedTeams;
            tm.Prizes = selectedPrizes;
            tm.Active = 1;

            foreach (Team team in tm.EnteredTeams)
            {
                if (tm.TournamentTeamsString != null)
                {
                    tm.TournamentTeamsString = tm.TournamentTeamsString + $"|{team.Id}";
                }
                else
                {
                    tm.TournamentTeamsString = tm.TournamentTeamsString + $"{team.Id}";
                }
            }

            foreach (Prize prize in tm.Prizes)
            {
                if (tm.TournamentPrizesString != null)
                {
                    tm.TournamentPrizesString = tm.TournamentPrizesString + $"|{prize.Id}";
                }
                else
                {
                    tm.TournamentPrizesString = tm.TournamentPrizesString + $"{prize.Id}";
                }
            }
            MatchupLogic.CreateRounds(tm);
            

            GlobalConfig.Connections.CreateTournament(tm);
            receiver.TournamentComplete(tm);
            MessageBox.Show("Tournament created", "Attention");
            //TournamentOperator op = new TournamentOperator(tm);
            //op.Show();
            this.Close();  
            

        }
    }
}
