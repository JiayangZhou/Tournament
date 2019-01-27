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
    public partial class CreatePrize : Form
    {
        IPrizeRequester receiver;//It must be assigned to
        public CreatePrize(IPrizeRequester caller)
        {
            InitializeComponent();
      
            receiver = caller;
        }
      
        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                Prize model = new Prize(placeNumberValue.Text, placeNameValue.Text, prizeAmountValue.Text, prizePercentageValue.Text);
                /*foreach (IDataConnection dc in GlobalConfig.Connections)
                {
                    dc.CreatePrize(model);
                }*/
                GlobalConfig.Connections.CreatePrize(model);
                //Prize prize = new Prize { PlaceName = placeNameValue.Text };

                placeNumberValue.Text = "";
                placeNameValue.Text = "";
                prizeAmountValue.Text = "0";
                prizePercentageValue.Text = "0";
                MessageBox.Show("Data updated");
                receiver.PrizeComplete(model);
                //CreateTournament formHelp = new CreateTournament();
                //formHelp.UpdatePrizesListbox();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid data");
            }
            
        }

        private bool ValidateForm()
        {
            bool output = true;
            int placeNumber = 0;
            double prizePercentage = 0;
            decimal prizeAmount = 0;
            bool placeNumberValid = int.TryParse(placeNumberValue.Text, out placeNumber);
            bool prizeAmountValid = decimal.TryParse(prizeAmountValue.Text, out prizeAmount);
            bool prizePercentageValid = double.TryParse(prizePercentageValue.Text, out prizePercentage);

            if (placeNumberValid==false||placeNumber<1)
            {
                output = false;
            }
            //if (placeNameValue.Text == null)
            //    output = false;
            if (placeNameValue.Text.Length ==0)
                output = false;
            /*if (prizeAmountValid == false || prizePercentageValid == false)
            {
                output = false;
            }*/
            if (prizeAmount < 0 || prizePercentage < 0)
            {
                output = false;
            }
            if (prizeAmount > 0 && prizePercentage > 0)
            {
                output = false;
            }
            if (prizeAmount == 0 && prizePercentage == 0)
            {
                output = false;
            }
            if (prizePercentage < 0 || prizePercentage > 100)
            {
                output = false;
            }
            /*if (prizeAmountValue.Text == null && prizePercentageValue.Text==null)
                output = false;
            if (prizeAmountValue.Text != null && prizePercentageValue.Text != null)
                output = false;*/


            return output;  
        }

        
    }
}
