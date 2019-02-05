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
    public partial class ImageOperator : Form
    {
        private string imageLocation = "";

        public ImageOperator()
        {
            InitializeComponent();
            
        }

        private void findPicButton_Click(object sender, EventArgs e)
        {           
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "JPG files(*.jpg)|*.jpg|PNG files(*.png)|*.png";
                if (dialog.ShowDialog()==DialogResult.OK)
                {
                    imageLocation = dialog.FileName;
                    teamMemberPictureBox.ImageLocation = imageLocation;
                }


            }
            catch (Exception)
            {

                MessageBox.Show("Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void DisplayImage()
        {
            teamMemberPictureBox.ImageLocation = null;
            teamMemberPictureBox.ImageLocation = imageLocation;
        }

        private void change1Button_Click(object sender, EventArgs e)
        {
            ImageProcessor processor = new ImageProcessor();
            //imageLocation=processor.RotateImage(imageLocation);
            DisplayImage();
        }
    }
}
