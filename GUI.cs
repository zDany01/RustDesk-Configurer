using RustDesk_Configurer.Properties;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RustDesk_Configurer
{
    public partial class GUI : Form
    {
        #region "Object Texts"
        private const string SERVER_CONNECTED = "Online";
        private const string SERVER_UNKNOWN = "Offline";
        private const string REPAIR_TEXT = "Repair";
        #endregion
        private const string RUSTDESK_GH_API_ENDPOINT = "https://api.github.com/repos/rustdesk/rustdesk/releases/latest";
        private readonly HttpClient httpClient = new HttpClient();
        private readonly string rsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "RustDesk", "rustdesk.exe");
        private string configString;


        public GUI()
        {
            InitializeComponent();

            this.Load += GUI_Load;
            this.installBtn.Click += ApplySettings;
        }

        private async Task<string> GetServerData()
        {
            HttpResponseMessage serverData;
            while (true)
            {
                try
                {
                    serverData = await httpClient.GetAsync(Resources.SERVER_ENDPOINT_URL);
                    if (serverData.IsSuccessStatusCode)
                    {
                        statusLbl.Text = SERVER_CONNECTED;
                        statusLbl.ForeColor = Color.Green;
                        return await serverData.Content.ReadAsStringAsync();
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
                        return Resources.DEFAULT_CONFIG_STRING;
                    }
                }
            }
        }
        private uint GetVersionValue(string semver)
        {
            if (semver is null) return 0;

            uint result = 0;
            string[] versions = semver.Split('.').Take(3).Reverse().ToArray();
            for (int i = 0, j = 1; i < versions.Length; i++, j *= 10)
            {
                uint temp;
                if (!uint.TryParse(versions[i], out temp)) throw new FormatException("String version not formatted correctly.");
                result += (uint)(temp * j);
            }
            return result;
        }
        private void WaitForRustDesk(Process installer)
        {
            installer.WaitForExit();
            foreach (Process p in Process.GetProcessesByName("rustdesk")) p.WaitForExit();
        }


        private async void GUI_Load(object sender, EventArgs e)
        {
            if (MessageBox.Show("By using this software you accept the RustDesk License Terms", Program.APP_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) Environment.Exit(1);

            if (File.Exists(rsPath)) installBtn.Text = REPAIR_TEXT;
            configString = await GetServerData();
            installBtn.Enabled = true;
        }
        private async void ApplySettings(object sender, EventArgs e)
        {
            installBtn.Enabled = false;
            statusPbr.Style = ProgressBarStyle.Marquee;
            HttpRequestMessage ghAPIRequest = new HttpRequestMessage(HttpMethod.Get, RUSTDESK_GH_API_ENDPOINT);
            HttpRequestHeaders requestHeaders = ghAPIRequest.Headers;
            requestHeaders.Accept.ParseAdd("application/vnd.github+json");
            requestHeaders.UserAgent.Add(new ProductInfoHeaderValue("DotNet-APP", null));
            requestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");

            JsonElement releaseData = JsonDocument.Parse((await httpClient.SendAsync(ghAPIRequest)).Content.ReadAsStreamAsync().Result).RootElement;
            uint rsLastVer = GetVersionValue(releaseData.GetProperty("tag_name").GetString());
            string rsDwnLink = null;
            foreach (JsonElement releaseBin in releaseData.GetProperty("assets").EnumerateArray())
            {
                if (releaseBin.GetProperty("name").GetString().EndsWith("64.exe"))
                {
                    rsDwnLink = releaseBin.GetProperty("browser_download_url").GetString();
                    break;
                }
            }
            if (rsDwnLink is null) throw new FileNotFoundException("Unable to find the latest windows release");

            uint rsCurrentVer = 0;
            if (object.ReferenceEquals(installBtn.Text, REPAIR_TEXT))
            {
                string prodVer = FileVersionInfo.GetVersionInfo(rsPath).ProductVersion;
                rsCurrentVer = GetVersionValue(prodVer.Substring(0, prodVer.IndexOf('+')));
            }


            if (rsLastVer > rsCurrentVer)
            {
                Process.Start("net", "stop ruskdesk").WaitForExit();
                foreach (Process p in Process.GetProcessesByName("rustdesk")) p.Kill();

                string tmpDwn = Path.GetTempFileName();
                File.WriteAllBytes(tmpDwn, await httpClient.GetByteArrayAsync(rsDwnLink));
                string exeTmp = tmpDwn.Replace(".tmp", ".exe");
                File.Move(tmpDwn, exeTmp);

                Process rsInstaller = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = exeTmp,
                        Arguments = "--silent-install",
                        CreateNoWindow = true
                    }
                };
                rsInstaller.Start();
                await Task.Run(() => WaitForRustDesk(rsInstaller));
                File.Delete(exeTmp);
            }

            if (!File.Exists(rsPath))
            {
                MessageBox.Show("There was an error while installing the service\nPlease make sure the application is run under admin rights", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            foreach (Process p in Process.GetProcessesByName("rustdesk")) p.Kill();

            Process rsConfig = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = rsPath,
                    Arguments = $"--config {configString}",
                    UseShellExecute = true,
                    CreateNoWindow = true
                }
            };

            rsConfig.Start();
            await Task.Run(() => WaitForRustDesk(rsConfig));
            statusPbr.Value = 100;
            statusPbr.Style = ProgressBarStyle.Continuous;
            MessageBox.Show("Installation Done", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Exit();
        }
    }
}
