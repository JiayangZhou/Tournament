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
    public partial class CreateTeam : Form
    {      
        ITeamRequester receiver;
        public CreateTeam(ITeamRequester caller)
        {
            InitializeComponent();

            //TestData();
            LoadPersonToDropDown();

            WireUpLists();

            receiver = caller;

        }
        public List<Person> avaliablePerson = new List<Person>();
        public List<Person> selectedPerson = new List<Person>();

        private void LoadPersonToDropDown()
        {
            avaliablePerson = GlobalConfig.Connections.GetAllPerson();
        }
        //private void TestData()
        //{
        //    avaliablePeople.Add(new Person { FirstName = "Jia", LastName = "Zhou" });
        //    avaliablePeople.Add(new Person { FirstName = "Tim", LastName = "Smith" });
        //}
        private void WireUpLists()
        {
            //selectTeamMemberLabelDropDown.Refresh();
            //teamMembersListBox.Refresh();
            selectTeamMemberLabelDropDown.DataSource = null;
            teamMembersListBox.DataSource = null;
            //To clear up and then put the value again
            selectTeamMemberLabelDropDown.DataSource = avaliablePerson;
            selectTeamMemberLabelDropDown.DisplayMember = "FullName";

            teamMembersListBox.DataSource = selectedPerson;
            teamMembersListBox.DisplayMember = "FullName";
            
        }
        private void createMemberButton_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                MessageBox.Show("Invalid data");
            }
            else
            {
                Person p = new Person(firstNameValue.Text, lastNameValue.Text, emailValue.Text, cellphoneValue.Text);
                GlobalConfig.Connections.CreatePerson(p);
                MessageBox.Show("Data updated");
                firstNameValue.Text = "";
                lastNameValue.Text = "";
                emailValue.Text = "";
                cellphoneValue.Text = "";

                //LoadPersonToDropDown();
                avaliablePerson.Add(p);

                WireUpLists();
            }          
        }

        private bool ValidateForm()
        {
            if (firstNameValue.Text.Length==0)
            {
                return false;
            }
            if (lastNameValue.Text.Length == 0)
            {
                return false;
            }
            if (emailValue.Text.Length == 0)
            {
                return false;
            }
            if (cellphoneValue.Text.Length == 0)
            {
                return false;
            }            
            return true;
        }
        private void addMemberButton_Click(object sender, EventArgs e)
        {
            if ((Person)selectTeamMemberLabelDropDown.SelectedItem == null)
            {
                MessageBox.Show("Nothing selected");
            }
            else
            {
                avaliablePerson.Remove((Person)selectTeamMemberLabelDropDown.SelectedItem);
                selectedPerson.Add((Person)selectTeamMemberLabelDropDown.SelectedItem);
            }
            

            
            WireUpLists();
        }

        private void deleteMemberButton_Click(object sender, EventArgs e)
        {
            if ((Person)teamMembersListBox.SelectedItem==null)
            {
                MessageBox.Show("Nothing selected");
            }
            else
            {
                selectedPerson.Remove((Person)teamMembersListBox.SelectedItem);
                avaliablePerson.Add((Person)teamMembersListBox.SelectedItem);
            }
            

            WireUpLists();
        }

        private void createTeamButton_Click(object sender, EventArgs e)
        {
            Team team = new Team();
            team.TeamName = teamNameValue.Text;
            team.TeamMembers.AddRange(selectedPerson);
            foreach (Person p in team.TeamMembers)
            {
                if (team.TeamMembersString!=null)
                {
                    team.TeamMembersString = team.TeamMembersString + $"|{p.Id}"; 
                }
                else
                {
                    team.TeamMembersString = team.TeamMembersString + $"{p.Id}";
                }
            }
            GlobalConfig.Connections.CreateTeam(team);
            MessageBox.Show("Team created");

            teamNameValue.Text = "";
            receiver.TeamComplete(team);
            this.Close();
        }
    }
}
