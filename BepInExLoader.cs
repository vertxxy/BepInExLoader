using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BepInExLoader
{
    public partial class BepInExLoader : Form
    {
        public BepInExLoader()
        {
            InitializeComponent();
            InitializeUI();
            _ = LoadVersionsAsync();
        }

        private void InitializeUI()
        {
            Text = "BepInEx Installer";
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            browseButton.Text = "Browse";
            installButton.Text = "Install BepInEx";
            installButton.Enabled = false;

            pathTextBox.Height = browseButton.Height;
            pathTextBox.Width = ClientSize.Width - browseButton.Width - 12;
            pathTextBox.Left = 0;
            pathTextBox.Top = browseButton.Top;

            browseButton.Left = pathTextBox.Right + 4;
            browseButton.Top = pathTextBox.Top;

            installButton.Left = (this.ClientSize.Width - installButton.Width) / 2;
            installButton.Top = (this.ClientSize.Height - installButton.Height) / 2;

            versionSelect.Left = (this.ClientSize.Width - versionSelect.Width) / 2;
            versionSelect.Top = (installButton.Bottom + 15);
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            using var folderDialog = new FolderBrowserDialog();
            if (folderDialog.ShowDialog() != DialogResult.OK)
                return;

            string selectedPath = folderDialog.SelectedPath;
            if (IsValidGorillaTagDirectory(selectedPath))
            {
                pathTextBox.Text = selectedPath;
                installButton.Enabled = true;
            }
        }

        private bool IsValidGorillaTagDirectory(string folderPath)
        {
            string gorillaTagExe = Path.Combine(folderPath, "Gorilla Tag.exe");
            if (!File.Exists(gorillaTagExe))
            {
                MessageBox.Show(
                    "The selected folder does not contain Gorilla Tag.exe.",
                    "Missing File",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }

            return true;
        }

        private async void installButton_Click(object sender, EventArgs e)
        {
            string folderPath = pathTextBox.Text;
            if (!IsValidGorillaTagDirectory(folderPath))
            {
                browseButton_Click(sender, e);
                return;
            }

            installButton.Enabled = false;
            browseButton.Enabled = false;

            try
            {
                await DownloadAndInstallLatestBepInEx(folderPath);
                MessageBox.Show(
                    "BepInEx installed successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Installation failed: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                installButton.Enabled = true;
                browseButton.Enabled = true;
            }
        }

        private async Task DownloadAndInstallLatestBepInEx(string installDirectory)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("BepInExInstaller");

            string selectedVersion = versionSelect.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(selectedVersion))
                throw new Exception("No version selected.");

            string apiUrl = $"https://api.github.com/repos/BepInEx/BepInEx/releases/tags/{selectedVersion}";
            string json = await client.GetStringAsync(apiUrl);

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            string winUrl = null;
            string genericX64Url = null;

            foreach (var asset in root.GetProperty("assets").EnumerateArray())
            {
                string name = asset.GetProperty("name").GetString().ToLower();

                if (!name.EndsWith(".zip"))
                    continue;

                if (name.Contains("win_x64"))
                {
                    winUrl = asset.GetProperty("browser_download_url").GetString();
                    break;
                }

                if (name.Contains("x64") &&
                    !name.Contains("linux") &&
                    !name.Contains("mac"))
                {
                    genericX64Url ??= asset.GetProperty("browser_download_url").GetString();
                }
            }

            string downloadUrl = winUrl ?? genericX64Url;

            if (string.IsNullOrEmpty(downloadUrl))
                throw new Exception("Could not find a valid Windows x64 ZIP in this release.");

            string tempZip = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".zip");

            using (var response = await client.GetAsync(downloadUrl))
            {
                response.EnsureSuccessStatusCode();

                using var fs = new FileStream(tempZip, FileMode.Create, FileAccess.Write, FileShare.None);
                await response.Content.CopyToAsync(fs);
            }

            if (!Directory.Exists(installDirectory))
                Directory.CreateDirectory(installDirectory);

            ZipFile.ExtractToDirectory(tempZip, installDirectory, overwriteFiles: true);

            File.Delete(tempZip);
        }

        private async Task LoadVersionsAsync()
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("BepInExInstaller");

            string apiUrl = "https://api.github.com/repos/BepInEx/BepInEx/releases";
            string json = await client.GetStringAsync(apiUrl);

            using var doc = JsonDocument.Parse(json);

            versionSelect.Items.Clear();

            foreach (var release in doc.RootElement.EnumerateArray())
            {
                string tag = release.GetProperty("tag_name").GetString();

                bool hasValidAsset = false;

                foreach (var asset in release.GetProperty("assets").EnumerateArray())
                {
                    string assetName = asset.GetProperty("name").GetString();

                    if (assetName.Contains("_x64") && assetName.EndsWith(".zip"))
                    {
                        hasValidAsset = true;
                        break;
                    }
                }

                if (hasValidAsset)
                    versionSelect.Items.Add(tag);
            }

            if (versionSelect.Items.Count > 0)
                versionSelect.SelectedIndex = 0;
        }
    }
}