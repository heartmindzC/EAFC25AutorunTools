using System;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace EAFC25_Modded_Autorun
{
    public partial class LoginForm : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        private string configFile = "config.txt";
        public LoginForm()
        {
            InitializeComponent();
            LoadSavedPaths(); // Tải đường dẫn đã lưu từ file khi khởi động ứng dụng


            //Double Buffered Picture box
            this.DoubleBuffered = true;
            txtPath1.Visible = false; // Ẩn TextBox 1
            txtPath2.Visible = false; // Ẩn TextBox 2
            btnBrowse1.Visible = false; // Ẩn nút Browse 1
            btnBrowse2.Visible = false; // Ẩn nút Browse 2
            labelBrowse2.Visible = false; // Ẩn label Browse 2
            labelBrowsePath.Visible = false; // Ẩn label Browse 1


        }
        private void toggleLanguage_Click(object sender, EventArgs e)
        {
            if (toggleLanguage.Checked == true)
            {
                picGoldenStar.Visible = true;
                labelLanguage.Text = "VN";
                btnLogin.Text = "Chạy Tự Động";
                btnCopySquads.Text = "Sao Chép Squads";
                btnBackupProfile.Text = "Sao Lưu Mods Profile";
                btnRestoreProfile.Text = "Khôi Phục Mods Profile";
            }
            else
            {
                picGoldenStar.Visible = false;
                labelLanguage.Text = "EN";
                btnLogin.Text = "Auto Run";
                btnCopySquads.Text = "Auto Copy Squads";
                btnBackupProfile.Text = "Backup Mods Profile";
                btnRestoreProfile.Text = "Restore Mods Profile";

            }
        }
        private void btnBrowse1_Click(object sender, EventArgs e)
        {
            string selectedPath = BrowseForFolder("Chọn Đường Dẫn Thư Mục FIFA MOD MANAGER");

            if (!string.IsNullOrEmpty(selectedPath))
            {
                txtPath1.Text = selectedPath; // Hiển thị đường dẫn trong TextBox 1
                SaveLECheckingStateToLocal(); // Cập nhật & lưu cả 2 đường dẫn vào file
            }
        }
        private void btnBrowse2_Click(object sender, EventArgs e)
        {
            string selectedPath = BrowseForFolder("Chọn Đường Dẫn Thư Mục Live Editor");

            if (!string.IsNullOrEmpty(selectedPath))
            {
                txtPath2.Text = selectedPath; // Hiển thị đường dẫn trong TextBox 2
                SaveLECheckingStateToLocal(); // Cập nhật & lưu cả 2 đường dẫn vào file
            }
        }
        private string BrowseForFolder(string description)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = description; // Mô tả cho hộp thoại
                folderDialog.ShowNewFolderButton = true;

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    return folderDialog.SelectedPath;
                }
            }
            return null;
        }
        private void btnFacebook_Click(object sender, EventArgs e)
        {
            // Mở Facebook trong trình duyệt mặc định
            Process.Start(new ProcessStartInfo("https://www.facebook.com/FCUniverseVN/") { UseShellExecute = true });
        }
        private void btnWebsite_Click(object sender, EventArgs e)
        {
            // Mở Facebook trong trình duyệt mặc định
            Process.Start(new ProcessStartInfo("https://sites.google.com/view/fcuniverse") { UseShellExecute = true });
        }
        private void checkboxLE_CheckedChanged(object sender, EventArgs e)
        {
            // Lưu trạng thái của checkbox vào file khi người dùng thay đổi
            SaveLECheckingStateToLocal();
        }
        private void SaveLECheckingStateToLocal()
        {
            try
            {
                var LEChecking = checkboxLE.Checked; // Correctly retrieve the Checked property
                if (LEChecking) // Use a valid boolean comparison
                {
                    // Lưu trạng thái checkbox vào file
                    File.WriteAllText(configFile, "Live Editor Auto-open: True");
                }
                else
                {
                    // Lưu trạng thái checkbox vào file
                    File.WriteAllText(configFile, "Live Editor Auto-open: False");
                    // Lưu trạng thái checkbox vào file
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi khi lưu trạng thái tùy chọn mở Live Editor", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private void LoadSavedPaths()
        {
            try
            {

                string savedPaths = File.ReadAllText(configFile);

                //Load trạng thái LE checkbox đã lưu trong config.txt
                if (savedPaths.Contains("True"))
                {
                    checkboxLE.Checked = true;
                }
                else
                {
                    checkboxLE.Checked = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải đường dẫn: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAutoRun_Click(object sender, EventArgs e)
        {
            string path = txtPath1.Text;
            string path2 = txtPath2.Text;
            string pathNew = Application.StartupPath;

            //try
            //{
            //    RunApplication(@"D:\EA SPORTS FC 25\FC25.exe", 10,false);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Lỗi khi chạy Origin: {ex.Message}", "Lỗi");
            //}
            if (checkboxLE.Checked)
            {
                try
                {
                    RunApplication(System.IO.Path.Combine(pathNew, "runtimes", "win", "lib", "net7.0", "System", "le", "Launcher.exe"), 5, false);

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi chạy Live Editor: {ex.Message}", "Lỗi");
                }


                try
                {
                    RunApplication(System.IO.Path.Combine(pathNew, "runtimes", "win", "lib", "net7.0", "System", "mm", "FIFA Mod Manager.exe"), 5, false);

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi chạy FIFA Mod Manager: {ex.Message}", "Lỗi");
                }
            }
            else
            {
                try
                {
                    RunApplication(System.IO.Path.Combine(pathNew, "runtimes", "win", "lib", "net7.0", "System", "mm", "FIFA Mod Manager.exe"), 5, false);

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi chạy FIFA Mod Manager: {ex.Message}", "Lỗi");
                }
            }


            // không sleep, tự đóng ứng dụng sau khi đã thực thi
            this.Close();

        }


        private void btnAddingSquadsFile_Click(object sender, EventArgs e)
        {
            string exePath = Application.StartupPath;
            string squadsFilePath = System.IO.Path.Combine(exePath, "batch", "autocopy.bat");
            try
            {
                RunApplication(squadsFilePath, 5, false);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chạy squads.bat: {ex.Message}", "Lỗi");
            }

        }


        public static void btnCopySquadFiles_Click(object sender, EventArgs e)
        {   
            string path = Application.StartupPath;
            string sourcePath = Path.Combine(path,"batch") ;

            string localPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string destinationPath = System.IO.Path.Combine(localPath, "EA SPORTS FC 25", "settings");
            if (!Directory.Exists(sourcePath))
            {
                MessageBox.Show("Thư mục nguồn không tồn tại!", "Lỗi");
                return;
            }

            // Tạo thư mục đích nếu chưa tồn tại
            Directory.CreateDirectory(destinationPath);

            // Lấy tất cả các file trong thư mục nguồn
            string[] files = Directory.GetFiles(sourcePath);

            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);

                if (fileName.IndexOf("squads", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    string destFile = Path.Combine(destinationPath, fileName);
                    File.Copy(file, destFile, true); // Ghi đè nếu file đã tồn tại
                }
            }
            MessageBox.Show("Đã sao chép squads thành công!", "Thông báo");
        }

        private void RunApplication(string appPath, int delaySeconds, bool requiresElevation, string autoUsername = null, string autoPassword = null)
        {
            if (System.IO.File.Exists(appPath))
            {
                try
                {
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = appPath,
                        WorkingDirectory = System.IO.Path.GetDirectoryName(appPath),
                        UseShellExecute = true
                    };

                    if (requiresElevation)
                    {
                        startInfo.Verb = "runas";
                    }

                    Process process = Process.Start(startInfo);
                    System.Threading.Thread.Sleep(delaySeconds * 1000);

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi chạy ứng dụng: {ex.Message}", "Lỗi");
                }

            }
            else
            {
                MessageBox.Show($"Không tìm thấy file: {appPath}", "Lỗi");
            }
        }
        private void btnClose_MouseEnter(object sender, EventArgs e)
        {
            btnClose.FillColor = Color.Red;
            btnClose.ForeColor = Color.White;
        }
        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            btnClose.FillColor = Color.White;
            btnClose.ForeColor = Color.FromArgb(31, 33, 33);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnBackupProfile_Click(object sender, EventArgs e)
        {
            // get path file from browse path, then find json file in path browsed
            string path = Application.StartupPath;
            string jsonFilePath = System.IO.Path.Combine(path, "runtimes", "win", "lib", "net7.0", "System", "mm", "FIFA Mod Manager.json");
            string jsonFilePath2 = System.IO.Path.Combine(path, "runtimes", "win", "lib", "net7.0", "System", "mm", "FIFA Mod Manager.json.bak");
            string backupFilePath = System.IO.Path.Combine(path, "runtimes", "win", "lib", "net7.0", "System", "mm", "backup_json_profile");
            //check backup folder exist, if not create it
            if (!System.IO.Directory.Exists(backupFilePath))
            {
                System.IO.Directory.CreateDirectory(backupFilePath);
            }
            try
            {
                System.IO.File.Copy(jsonFilePath, backupFilePath + @"\FIFA Mod Manager.json", true);
                System.IO.File.Copy(jsonFilePath2, backupFilePath + @"\FIFA Mod Manager.json.bak", true);
                MessageBox.Show("Đã sao lưu thành công!", "Thông báo");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi sao lưu: {ex.Message}", "Lỗi");
            }
        }

        private void btnRestoreProfile_Click(object sender, EventArgs e)
        {

            // copy 2 files json from backup folder to txtPath1 path\
            string path = Application.StartupPath;
            string jsonFilePath = System.IO.Path.Combine(path, "runtimes", "win", "lib", "net7.0", "System", "mm", "backup_json_profile", "FIFA Mod Manager.json");
            string jsonFilePath2 = System.IO.Path.Combine(path, "runtimes", "win", "lib", "net7.0", "System", "mm", "backup_json_profile", "FIFA Mod Manager.json.bak");
            string backupFilePath = System.IO.Path.Combine(path, "runtimes", "win", "lib", "net7.0", "System", "mm", "backup_json_profile");
            string restoreFilePath = System.IO.Path.Combine(path, "runtimes", "win", "lib", "net7.0", "System", "mm");
            if (!System.IO.Directory.Exists(backupFilePath))
            {
                MessageBox.Show("Thư mục sao lưu không tồn tại!", "Lỗi");
            }
            // copy 2 files json from backup folder to txtPath1 path\
            try
            {
                System.IO.File.Copy(jsonFilePath, restoreFilePath + @"\FIFA Mod Manager.json", true);
                System.IO.File.Copy(jsonFilePath2, restoreFilePath + @"\FIFA Mod Manager.json.bak", true);
                MessageBox.Show("Đã khôi phục thành công!", "Thông báo");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi khôi phục: {ex.Message}", "Lỗi");
            }




        }

        private void btnOpenFileLocation_Click(object sender, EventArgs e)
        {
            //open file location by explorer
            string localPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string filePath = System.IO.Path.Combine(localPath, "EA SPORTS FC 25", "settings");
            if (Directory.Exists(filePath))
            {
                // Mở Explorer và chọn file
                Process.Start("explorer.exe", filePath);
            }
            else
            {
                MessageBox.Show("Không tìm thấy file!", "Lỗi");
            }

        }
    }
}