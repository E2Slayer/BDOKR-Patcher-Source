using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BDOKRPatch
{
    public partial class Install : MetroFramework.Forms.MetroForm
    {

        private string selectedPath = @"";
        private bool comChecker = false;
        public Install()
        {
            InitializeComponent();
        }

        private void disgreeRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (disgreeRadioButton.Checked)
            {
                nextButton1.Enabled = false;
            }
            else
            {
                nextButton1.Enabled = false;
            }
        }

        private void AgreeRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (AgreeRadioButton.Checked)
            {
                nextButton1.Enabled = true;
            }
            else
            {
                nextButton1.Enabled = false;
            }
        }


        private void locationButton_Click(object sender, EventArgs e)
        {
            Communication_Checker();
            // var fileContent = string.Empty;
            // var filePath = string.Empty;

            /*
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
            }
            */

            using (var fbd = new FolderBrowserDialog())
            {
                fbd.RootFolder = Environment.SpecialFolder.MyComputer;
                fbd.SelectedPath = locationTextBox.Text;
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {

                    locationTextBox.Text = fbd.SelectedPath;

                    if (File.Exists(fbd.SelectedPath + @"\ads\languagedata_en.loc"))
                    {
                       // System.Windows.Forms.MessageBox.Show("올바른 경로인걸 확인했습니다. 진행하셔도 좋습니다.", "Message");
                        MetroFramework.MetroMessageBox.Show(this, "올바른 Black Desert Online 폴더를 찾았습니다. \n진행하셔도 좋습니다", "정보", MessageBoxButtons.OK, MessageBoxIcon.None);
                        nextButton2.Enabled = true;
                        selectedPath = fbd.SelectedPath;
                    }
                    else
                    {
                       // System.Windows.Forms.MessageBox.Show("Black Desert Online 폴더를 제대로 선택하셨는지 확인해주세요", "Message");
                        MetroFramework.MetroMessageBox.Show(this, "Black Desert Online 폴더를 제대로 선택하셨는지 확인해주세요", "경고", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //locationTextBox.Text = "경로 설정을 다시해주세요";
                        nextButton2.Enabled = false;
                    }



                }
            }

           // MessageBox.Show(fileContent, "File Content at path: " + filePath, MessageBoxButtons.OK);
        }

        private void steamRadio_CheckedChanged(object sender, EventArgs e)
        {
            locationTextBox.Text = @"C:\Program Files (x86)\Steam\steamapps\common\Black Desert Online";
        }


        private void nonSteamRadio_CheckedChanged(object sender, EventArgs e)
        {
            locationTextBox.Text = @"C:\Program Files (x86)\Black Desert Online";
        }

        private void nextButton1_Click(object sender, EventArgs e)
        {
            Communication_Checker();
            metroTabControl1.SelectedTab = metroTabPage2;
            this.metroTabPage2.Parent = this.metroTabControl1;
            this.metroTabPage2.Enabled = true;

        }

        private void nextButton2_Click(object sender, EventArgs e)
        {
            Communication_Checker();
            metroTabControl1.SelectedTab = metroTabPage3;
            this.metroTabPage3.Parent = this.metroTabControl1;
            this.metroTabPage3.Enabled = true;
            this.FontComboBox.SelectedIndex = 0;
        }

        private void FontComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
             *             "윤고딕체 (한국검사폰트)",
            "나눔스퀘어체 Bold",
            "나눔바른고딕체",
            "양진체",
            "빙그래체2 Bold",
            "배민 을지로체"
             */
            switch (FontComboBox.SelectedIndex)
            {
                case 0: // yoongo thic
                    fontPreview.Image = Properties.Resources.yanggothic;
                    break;
                case 1: // nanum square
                    fontPreview.Image = Properties.Resources.nanumsquare;
                    break;
                case 2: // nanum barun
                    fontPreview.Image = Properties.Resources.barungothic;
                    break;
                case 3: // yangjin
                    fontPreview.Image = Properties.Resources.yangjin;
                    break;
                case 4: // bing
                    fontPreview.Image = Properties.Resources.bing;
                    break;
                case 5: // bemin
                    fontPreview.Image = Properties.Resources.bemin;
                    break;
            }
        }


        public void Download(string url, string location, string display)
        {

           // Console.WriteLine("Download Started");

            logTextBox.AppendText(Environment.NewLine);
            logTextBox.AppendText("[다운로드시작 - "+ DateTime.Now.ToString("h:mm:ss tt") + "] "+ display );
   
            WebClient webClient = new WebClient();
           // ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            webClient.QueryString.Add("file", display); // here you can add values
           // webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            try
            {
                webClient.DownloadFileAsync(new Uri(url), location);
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show("\n Download Error :" + ex.Message.ToString(), "Message");
               // logTextBox.Text += "\n Download Error: " + ex.Message.ToString() + "\n";

                logTextBox.AppendText(Environment.NewLine);
                logTextBox.AppendText("[다운로드에러 - " + DateTime.Now.ToString("h:mm:ss tt") + "] " + ex.Message.ToString() );
                logTextBox.AppendText(Environment.NewLine);
                logTextBox.AppendText("[다운로드에러파일] " + display);
                logTextBox.AppendText(Environment.NewLine);
                logTextBox.AppendText("개발자에게 이 메세지를 보고해주세요.");
            }

        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            return;
            //logTextBox.Text += "\n Download Completed " + sender.ToString() + "\n";
            string fileIdentifier = ((System.Net.WebClient)(sender)).QueryString["file"];
            logTextBox.AppendText(Environment.NewLine);
            logTextBox.AppendText("[다운로드완료 - " + DateTime.Now.ToString("h:mm:ss tt") + "] " + fileIdentifier);
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            //logTextBox.Text += 
        }

        private void patchButton_Click(object sender, EventArgs e)
        {
            Communication_Checker();
            // selectedPath = locationTextBox.Text;
            string currentLocation = selectedPath + @"\";

            string pathAds = currentLocation + @"\ads";
            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(pathAds))
                {
                    logTextBox.AppendText(Environment.NewLine);
                    logTextBox.AppendText("[정보 - " + DateTime.Now.ToString("h:mm:ss tt") + "] ads 폴더가 이미 존재합니다. 폴더를 생성하지 않습니다.");
                    // Console.WriteLine("That path exists already.");

                }
                else
                {
                    // Try to create the directory.
                    DirectoryInfo diAds = Directory.CreateDirectory(pathAds);
                    //Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path));
                    logTextBox.AppendText(Environment.NewLine);
                    logTextBox.AppendText("[정보 - " + DateTime.Now.ToString("h:mm:ss tt") + "] ads 폴더 생성완료");
                }

            }
            catch (Exception er)
            {
                //Console.WriteLine("The process failed: {0}", er.ToString());
                logTextBox.AppendText(Environment.NewLine);
                logTextBox.AppendText("[에러 - " + DateTime.Now.ToString("h:mm:ss tt") + "] 폴더 생성중 에러가 발생했습니다");
                logTextBox.AppendText(Environment.NewLine);
                logTextBox.AppendText(er.ToString());
            }


            
            if (File.Exists(currentLocation + @"ads\languagedata_en_backup.loc") && File.Exists(currentLocation + @"ads\languagedata_en.loc"))
            {
                logTextBox.AppendText(Environment.NewLine);
                logTextBox.AppendText("[정보 - " + DateTime.Now.ToString("h:mm:ss tt") + "] languagedata_en.loc 백업이 이미 되어있습니다. 백업하지않습니다");
            }
            else if (!File.Exists(currentLocation + @"ads\languagedata_en_backup.loc") && File.Exists(currentLocation + @"ads\languagedata_en.loc"))
            {

                System.IO.File.Move(currentLocation + @"ads\languagedata_en.loc", currentLocation + @"ads\languagedata_en_backup.loc");
                logTextBox.AppendText(Environment.NewLine);
                logTextBox.AppendText("[정보 - " + DateTime.Now.ToString("h:mm:ss tt") + "] languagedata_en.loc 원본 백업완료");
            }
            

            Download(@"https://github.com/E2Slayer/BDOKRPatchData/raw/master/Data/languagedata_en_toKR.PAZ", currentLocation + @"ads\languagedata_en_toKR.PAZ", "languagedata_en_toKR.PAZ");
            Download(@"https://github.com/E2Slayer/BDOKRPatchData/raw/master/Data/languagedata_en.loc", currentLocation + @"ads\languagedata_en.loc", "languagedata_en.loc");
            Download(@"https://raw.githubusercontent.com/E2Slayer/BDOKRPatchData/master/KRPVersion", currentLocation + @"ads\KRPVersion", "KRPVersion.chk");

            /*
             *             "윤고딕체 (한국검사폰트)",
            "나눔스퀘어체 Bold",
            "나눔바른고딕체",
            "양진체",
            "빙그래체2 Bold",
            "배민 을지로체"
             */

            string fontURL = @"";
             switch (FontComboBox.SelectedIndex)
             {
                 case 0: // yoongo thic
                    fontURL = @"https://github.com/E2Slayer/BDOKRPatchData/raw/master/Data/font/YoonGothic.ttf";
                     break;
                 case 1: // nanum square
                    fontURL = @"https://github.com/E2Slayer/BDOKRPatchData/raw/master/Data/font/NanumSquareB.ttf";
                    break;
                 case 2: // nanum barun
                    fontURL = @"https://github.com/E2Slayer/BDOKRPatchData/raw/master/Data/font/NanumBarunGothic.ttf";
                    break;
                 case 3: // yangjin
                    fontURL = @"https://github.com/E2Slayer/BDOKRPatchData/raw/master/Data/font/YangJin.ttf";
                    break;
                 case 4: // bing
                    fontURL = @"https://github.com/E2Slayer/BDOKRPatchData/raw/master/Data/font/Binggrae%E2%85%A1-Bold.ttf";
                    break;
                 case 5: // bemin
                    fontURL = @"https://github.com/E2Slayer/BDOKRPatchData/raw/master/Data/font/BMEULJIROTTF.ttf";
                    break;
             }

             
            string path = currentLocation + @"\prestringtable";
            try
            {
                 // Determine whether the directory exists.
                 if (Directory.Exists(path))
                 {
                     logTextBox.AppendText(Environment.NewLine);
                     logTextBox.AppendText("[정보 - " + DateTime.Now.ToString("h:mm:ss tt") + "] prestringtable 폴더가 이미 존재합니다. 폴더를 생성하지 않습니다.");
                   // Console.WriteLine("That path exists already.");
                     
                 }
                 else
                 {
                     // Try to create the directory.
                     DirectoryInfo di = Directory.CreateDirectory(path);
                     //Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path));
                     logTextBox.AppendText(Environment.NewLine);
                     logTextBox.AppendText("[정보 - " + DateTime.Now.ToString("h:mm:ss tt") + "] prestringtable 폴더 생성완료");

                     path += @"\font";
                     DirectoryInfo di2 = Directory.CreateDirectory(path);
                     //onsole.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path));
                     logTextBox.AppendText(Environment.NewLine);
                     logTextBox.AppendText("[정보 - " + DateTime.Now.ToString("h:mm:ss tt") + "] font 폴더 생성완료");
                 }

            }
            catch (Exception er) 
            {
                 //Console.WriteLine("The process failed: {0}", er.ToString());
                 logTextBox.AppendText(Environment.NewLine);
                 logTextBox.AppendText("[에러 - " + DateTime.Now.ToString("h:mm:ss tt") + "] 폴더 생성중 에러가 발생했습니다");
                 logTextBox.AppendText(Environment.NewLine);
                 logTextBox.AppendText(er.ToString());
            }


            Download(fontURL, currentLocation + @"\prestringtable\font\pearl.ttf", "pearl.ttf");


            //Task.Delay(1000).Wait();


            logTextBox.AppendText(Environment.NewLine);
            logTextBox.AppendText("[알림 - " + DateTime.Now.ToString("h:mm:ss tt") + "] 모든 작업이 완료되었습니다");
            logTextBox.AppendText(Environment.NewLine);
            logTextBox.AppendText("===============================================================");
            logTextBox.AppendText(Environment.NewLine);
            logTextBox.AppendText("한글패치완료 ! 프로그램을 종료하셔도됩니다.");
            logTextBox.AppendText(Environment.NewLine);
            logTextBox.AppendText("===============================================================");
            //logTextBox.AppendText(Environment.NewLine);
            // logTextBox.AppendText(fontURL);
            // WebClient wb = new WebClient();
            //wb.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.33 Safari/537.36");
            // wb.DownloadFile("https://github.com/E2Slayer/BDOKRPatchData/raw/master/Data/languagedata_en_toKR.PAZ", @"C:\temp\languagedata_en_toKR.PAZ");
        }

        private void nextButton3_Click(object sender, EventArgs e)
        {
            metroTabControl1.SelectedTab = metroTabPage4;
           // this.metroTabPage3.Parent = this.metroTabControl1;
            this.metroTabPage4.Enabled = true;
        }

        private void Install_Load(object sender, EventArgs e)
        {
            Communication_Loader();
            Communication_Checker();
        }

        private void Communication_Loader()
        {
            var url = "https://raw.githubusercontent.com/E2Slayer/BDOKRPatchData/master/Release/user.ini";
            var textFromFile = (new WebClient()).DownloadString(url);
            string validation = "3053";


            if (string.Equals(textFromFile, validation))
            {
                comChecker = true;

            }
            else
            {
                comChecker = false;
            }
        }

        private void Communication_Checker()
        {
            if (comChecker) 
                return;

            MetroFramework.MetroMessageBox.Show(this, "통신서버와 연결을 실패했습니다. \n잠시뒤에 시도해주세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Error);
            System.Windows.Forms.Application.ExitThread();
            System.Windows.Forms.Application.Exit();
        }
    }
}
