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
    public partial class TournamentDashboard : Form, ITournamentRequester
    {
        List<Tournament> avaliableTournaments = GlobalConfig.Connections.GetAllTournaments();
        public TournamentDashboard()
        {
            InitializeComponent();
            InitializeDashboard();
        }

        public void InitializeDashboard()
        {
            loadExistingtournamentDropDown.DataSource = null;

            loadExistingtournamentDropDown.DataSource = avaliableTournaments;
            loadExistingtournamentDropDown.DisplayMember = "TournamentName";
        }

        private void loadTournamentButton_Click(object sender, EventArgs e)
        {
            Tournament selectedTournament = new Tournament();
            selectedTournament = (Tournament)loadExistingtournamentDropDown.SelectedItem;
            //ILoadTournament passer = (ILoadTournament)selectedTournament;
            TournamentOperator Op = new TournamentOperator(selectedTournament);
            Op.Show();
        }

        private void createTournamentButton_Click(object sender, EventArgs e)
        {
            CreateTournament createOne = new CreateTournament(this);//Pass this, so Createtournament knows who creates it
            createOne.Show();
        }

        public void TournamentComplete(Tournament tournament)
        {
            avaliableTournaments.Add(tournament);
            InitializeDashboard();
        }
    }
}
