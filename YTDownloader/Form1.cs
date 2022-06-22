using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoLibrary;
using MediaToolkit;
using System.IO;

namespace YTDownloader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Boolean format = true;
        private async void btnStart_Click(object sender, EventArgs e)
        {
            using(FolderBrowserDialog dlg = new FolderBrowserDialog() { Description= "please select a folder" })
            {
                if(dlg.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Download has started please wait...", "Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    var yt = YouTube.Default;
                    var Video = await yt.GetVideoAsync(txtURL.Text);
                    lblStatus.Text = "Downloading...";
                    btnStart.Enabled = false;
                    try
                    {
                        File.WriteAllBytes(dlg.SelectedPath + @"\" + Video.FullName, await Video.GetBytesAsync());

                    }
                    catch (Exception)
                    {
                        MessageBox.Show("No Ethernet Connection Please Try Again");
                        throw;
                    }
                    
                    var inputfile = new MediaToolkit.Model.MediaFile { Filename = dlg.SelectedPath + @"\" + Video.FullName };
                    var outputfile = new MediaToolkit.Model.MediaFile { Filename = $"{dlg.SelectedPath + @"\" + Video.FullName}.mp3" };



                    using (var enging =new Engine())
                    {
                        enging.GetMetadata(inputfile);
                        enging.Convert(inputfile,outputfile);
                    }
                    if (format == true)
                    {
                        File.Delete(dlg.SelectedPath + @"\" + Video.FullName);
                    }
                    else
                    {
                        File.Delete($"{dlg.SelectedPath + @"\" + Video.FullName}.mp3");
                    }
                    
                    MessageBox.Show("Download is Completed!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtURL.Text = "";
                    lblStatus.Text = "......";
                    btnStart.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Please Select a Folder :", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    
                }
            }

        }

        private void txtURL_MouseClick(object sender, MouseEventArgs e)
        {
            txtURL.Clear();
            txtURL.ForeColor = Color.Black;
        }
    }
}
