using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using RustDesk_Configurer.Properties;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;

namespace RustDesk_Configurer
{
    public partial class GUI : Form
    {
        #region "Object Texts"
        private const string SERVER_CONNECTED = "Online";
        private const string SERVER_UNKNOWN = "Offline";
        private const string REPAIR_TEXT = "Repair";
        #endregion
        private string[] serverData;
        

        public GUI()
        {
            InitializeComponent();

            this.Load += GUI_Load;
            this.installBtn.Click += ApplySettings;
        }

        private async Task<string[]> GetServerData()
        {
            HttpClient requester = new HttpClient();
            HttpResponseMessage serverData;
            while (true)
            {
                try
                {
                    serverData = await requester.GetAsync(Resources.SERVER_ENDPOINT_URL);
                    if (serverData.IsSuccessStatusCode)
                    {
                        statusLbl.Text = SERVER_CONNECTED;
                        statusLbl.ForeColor = Color.Green;
                        return (await serverData.Content.ReadAsStringAsync()).Split();
                    }
                    else throw new HttpRequestException();
                }
                catch (HttpRequestException)
                {
                    statusLbl.Text = SERVER_UNKNOWN;
                    statusLbl.ForeColor = Color.Red;
                    if (MessageBox.Show("There was an error contacting the server\nWould you like to try again?", Program.APP_NAME, MessageBoxButtons.RetryCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                    {
                        if (MessageBox.Show("Would you like to use preconfigured server data?\nRemember that these could be obsolete!", Program.APP_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) Application.Exit();
                        return new string[] { Resources.DEFAULT_RELAY_ID, Resources.DEFAULT_RELAY_ADDRESS, string.Empty, Resources.DEFAULT_RELAY_KEY };
                    }
                }
            }
        }


        private async void GUI_Load(object sender, EventArgs e)
        {
            if (MessageBox.Show("By using this software you accept the RustDesk License Terms", Program.APP_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) Environment.Exit(1);

            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "RustDesk", "rustdesk.exe"))) installBtn.Text = REPAIR_TEXT;
            serverData = await GetServerData();
            installBtn.Enabled = true;
        }
        private void ApplySettings(object sender, EventArgs e)
        {
            if (object.ReferenceEquals(installBtn.Text, REPAIR_TEXT))
            {
                throw new NotImplementedException("Check for update and install config");
            }
            throw new NotImplementedException("Install RustDesk and config");
        }
    }
}
