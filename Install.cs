using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace BDOKRPatch
{
    public partial class Install : MetroFramework.Forms.MetroForm
    {

        private string selectedPath = @"";
        private bool comChecker = false;

        private int steam = 0;
        private string installPath = @"";
        private int validatedirectory = 0;
        private int font = 1;
        private int languageselection = 0;

        public Install()
        {
            InitializeComponent();
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

            if (validatedirectory == 1)
            {
                MetroFramework.MetroMessageBox.Show(this, "이전 설치정보를 발견했습니다. \n한글패치탭으로 바로 넘어갑니다.\n\n다른 설정을 바꾸시고 싶으시면\n탭을 수동으로 선택하시면 설정을 바꿀 수 있습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                metroTabControl1.SelectedTab = metroTabPage4;
                nextButton2.Enabled = true;
                this.metroTabPage3.Enabled = true;

                this.metroTabPage4.Enabled = true;

                if (steam == 1)
                {
                    steamRadio.Checked = true;
                }

                locationTextBox.Text = installPath;
                FontComboBox.SelectedIndex = font;

                switch (languageselection)
                {
                    case 0: //english
                        metroRadioButton1.Checked = true;
                        break;
                    case 1:
                        metroRadioButton2.Checked = true;
                        break;
                    case 2:
                        metroRadioButton3.Checked = true;
                        break;
                    case 3:
                        metroRadioButton4.Checked = true;
                        break;
                }
            }


        }

        private void nextButton2_Click(object sender, EventArgs e)
        {
            Communication_Checker();
            metroTabControl1.SelectedTab = metroTabPage3;
            this.metroTabPage3.Parent = this.metroTabControl1;
            this.metroTabPage3.Enabled = true;
            
        }


        private void nextButton3_Click(object sender, EventArgs e)
        {
            metroTabControl1.SelectedTab = metroTabPage4;
            // this.metroTabPage3.Parent = this.metroTabControl1;
            this.metroTabPage4.Enabled = true;
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
                    fontPreview.Image = Properties.Resources.ridi;
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


            string selectedLangStr = "None";

            XmlDocument doc = new XmlDocument();
            doc.Load(Environment.CurrentDirectory + "\\config.xml");
            XmlNodeList xNodeList = doc.SelectNodes("/root");
            //MetroFramework.MetroMessageBox.Show(this, "config.xml을 열수없습니다. \n프로그램을 업데이트해주세요", "경고", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (doc != null)
            {
                if (steamRadio.Checked)
                {
                    doc.SelectSingleNode("//root/steam").InnerText = "1";
                }
                else
                {
                    doc.SelectSingleNode("//root/steam").InnerText = "0";
                }

                doc.SelectSingleNode("//root/installpath").InnerText = selectedPath;
                doc.SelectSingleNode("//root/validatedirectory").InnerText = "1";
                doc.SelectSingleNode("//root/font").InnerText = (FontComboBox.SelectedIndex).ToString();

                int lang = 0;

                if (metroRadioButton1.Checked)
                {
                    lang = 0;
                    selectedLangStr = "en";
                }
                else if (metroRadioButton2.Checked)
                {
                    lang = 1;
                    selectedLangStr = "fr";
                }
                else if (metroRadioButton3.Checked)
                {
                    lang = 2;
                    selectedLangStr = "de";
                }
                else if (metroRadioButton4.Checked)
                {
                    lang = 3;
                    selectedLangStr = "sp";
                }

                doc.SelectSingleNode("//root/languageselection").InnerText = (lang).ToString();


                doc.Save(Environment.CurrentDirectory + "\\config.xml");

            }

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


            
            if (File.Exists(currentLocation + @"ads\languagedata_en_backup.loc") && File.Exists(currentLocation + @"ads\languagedata_"+ selectedLangStr+".loc"))
            {
                logTextBox.AppendText(Environment.NewLine);
                logTextBox.AppendText("[정보 - " + DateTime.Now.ToString("h:mm:ss tt") + "] languagedata_en.loc 백업이 이미 되어있습니다. 백업하지않습니다");
            }
            else if (!File.Exists(currentLocation + @"ads\languagedata_en_backup.loc") && File.Exists(currentLocation + @"ads\languagedata_" + selectedLangStr + ".loc"))
            {

                System.IO.File.Move(currentLocation + @"ads\languagedata_en.loc", currentLocation + @"ads\languagedata_" + selectedLangStr + "_backup.loc");
                logTextBox.AppendText(Environment.NewLine);
                logTextBox.AppendText("[정보 - " + DateTime.Now.ToString("h:mm:ss tt") + "] languagedata_en.loc 원본 백업완료");
            }
            

            Download(@"https://github.com/E2Slayer/BDOKRPatchData/raw/master/Data/languagedata_en_toKR.PAZ", currentLocation + @"ads\languagedata_" + selectedLangStr + "_toKR.PAZ", "languagedata_" + selectedLangStr + "_toKR.PAZ");
            Download(@"https://github.com/E2Slayer/BDOKRPatchData/raw/master/Data/languagedata_en.loc", currentLocation + @"ads\languagedata_" + selectedLangStr + ".loc", "languagedata_" + selectedLangStr + ".loc");
          //  Download(@"https://raw.githubusercontent.com/E2Slayer/BDOKRPatchData/master/KRPVersion", currentLocation + @"ads\KRPVersion", "KRPVersion.chk");

            /*
             *             "리디바탕",
            "나눔스퀘어체 Bold",
            "나눔바른고딕체",
            "양진체",
            "빙그래체2 Bold",
            "배민 을지로체"
             */

            string fontURL = @"";
             switch (FontComboBox.SelectedIndex)
             {
                 case 0: // ridi
                    fontURL = @"https://github.com/E2Slayer/BDOKRPatchData/raw/master/Data/font/RIDIBatang.ttf";
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

        private void Install_Load(object sender, EventArgs e)
        {
            // System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();

            //FileVersionInfo fileVersion = FileVersionInfo.GetVersionInfo(assembly.Location);

            // versionLabel.Text = "Version "+fileVersion.FileVersion;

            this.FontComboBox.SelectedIndex = 1;
            metroTabControl1.SelectedIndex = 0;
            Communication_Loader();
            Communication_Checker();

            using (WebClient client = new WebClient())
            {
                var htmlData = client.DownloadData("https://raw.githubusercontent.com/E2Slayer/BDOKRPatchData/master/Release/changelog.txt");
                var htmlCode = Encoding.UTF8.GetString(htmlData);
                patchNoteTextBox.Text = htmlCode;
            }


            XmlDocument doc = new XmlDocument();
            doc.Load(Environment.CurrentDirectory+"\\config.xml");
            XmlNodeList xNodeList = doc.SelectNodes("/root");

            if (xNodeList != null)
            {
                foreach (XmlNode xNode in xNodeList)
                {
                    //XmlConvert.ToBoolean(xNode["validatedirectory"].InnerText);
                    steam = XmlConvert.ToInt32(xNode["steam"].InnerText);
                    installPath = xNode["installpath"].InnerText;
                    validatedirectory = XmlConvert.ToInt32(xNode["validatedirectory"].InnerText);
                    font = XmlConvert.ToInt32(xNode["font"].InnerText);
                    languageselection = XmlConvert.ToInt32(xNode["languageselection"].InnerText);
                }

                selectedPath = installPath;


                //Console.WriteLine(languageselection);
            }
            else
            {
                MetroFramework.MetroMessageBox.Show(this, "config.xml을 열수없습니다. \n프로그램을 업데이트해주세요", "경고", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Windows.Forms.Application.ExitThread();
                System.Windows.Forms.Application.Exit();
            }
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

        private void uninstallButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult =
                MessageBox.Show(
                    $"BDO 한글패치된 파일들을 삭제합니다. \n 모든 패치파일들은 원상복귀가 됩니다. \n진행하시겠습니까? ", @"한글패치 삭제",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);
            if (dialogResult.Equals(DialogResult.Yes) || dialogResult.Equals(DialogResult.OK))
            {
                try
                {
                    string currentLocation = selectedPath + @"\";
                   // string pathAds = currentLocation + @"\ads";


                    System.IO.DirectoryInfo di = new DirectoryInfo(currentLocation + @"prestringtable\");
                    if (Directory.Exists(currentLocation + @"prestringtable\"))
                    {
                        logTextBox.AppendText(Environment.NewLine);
                        logTextBox.AppendText("[정보 - " + DateTime.Now.ToString("h:mm:ss tt") + "] prestringtable 폴더 삭제중...");


                        foreach (FileInfo file in di.GetFiles())
                        {
                            file.Delete();
                        }

                        foreach (DirectoryInfo dir in di.GetDirectories())
                        {
                            dir.Delete(true);
                        }

                    }
                    else
                    {
                        logTextBox.AppendText(Environment.NewLine);
                        logTextBox.AppendText("[정보 - " + DateTime.Now.ToString("h:mm:ss tt") + "] prestringtable 폴더가 존재하지 않습니다.");

                    }


                    
                    Download(@"https://github.com/E2Slayer/BDOKRPatchData/raw/master/Data/languagedata_en.loc", currentLocation + @"ads\languagedata_en.loc", "languagedata_en.loc");
                    Download(@"https://github.com/E2Slayer/BDOKRPatchData/raw/master/Data/languagedata_fr.loc", currentLocation + @"ads\languagedata_fr.loc", "languagedata_fr.loc");
                    Download(@"https://github.com/E2Slayer/BDOKRPatchData/raw/master/Data/languagedata_de.loc", currentLocation + @"ads\languagedata_de.loc", "languagedata_de.loc");
                    Download(@"https://github.com/E2Slayer/BDOKRPatchData/raw/master/Data/languagedata_sp.loc", currentLocation + @"ads\languagedata_sp.loc", "languagedata_sp.loc");
                    logTextBox.AppendText(Environment.NewLine);
                    logTextBox.AppendText("[정보 - " + DateTime.Now.ToString("h:mm:ss tt") + "] locale 원본파일 다운로드중...");


                    if (File.Exists(currentLocation + @"ads\languagedata_en_backup.loc"))
                    {
                        File.Delete(currentLocation + @"ads\languagedata_en_backup.loc");
                    }

                    if (File.Exists(currentLocation + @"ads\languagedata_fr_backup.loc"))
                    {
                        File.Delete(currentLocation + @"ads\languagedata_fr_backup.loc");
                    }

                    if (File.Exists(currentLocation + @"ads\languagedata_de_backup.loc"))
                    {
                        File.Delete(currentLocation + @"ads\languagedata_de_backup.loc");
                    }

                    if (File.Exists(currentLocation + @"ads\languagedata_sp_backup.loc"))
                    {
                        File.Delete(currentLocation + @"ads\languagedata_sp_backup.loc");
                    }

                    logTextBox.AppendText(Environment.NewLine);
                    logTextBox.AppendText("[정보 - " + DateTime.Now.ToString("h:mm:ss tt") + "] 백업파일 삭제중...");


                    if (File.Exists(currentLocation + @"ads\languagedata_en_toKR.PAZ"))
                    {
                        File.Delete(currentLocation + @"ads\languagedata_en_toKR.PAZ");
                    }

                    if (File.Exists(currentLocation + @"ads\languagedata_fr_toKR.PAZ"))
                    {
                        File.Delete(currentLocation + @"ads\languagedata_fr_toKR.PAZ");
                    }

                    if (File.Exists(currentLocation + @"ads\languagedata_de_toKR.PAZ"))
                    {
                        File.Delete(currentLocation + @"ads\languagedata_de_toKR.PAZ");
                    }

                    if (File.Exists(currentLocation + @"ads\languagedata_sp_toKR.PAZ"))
                    {
                        File.Delete(currentLocation + @"ads\languagedata_sp_toKR.PAZ");
                    }

                    logTextBox.AppendText(Environment.NewLine);
                    logTextBox.AppendText("[정보 - " + DateTime.Now.ToString("h:mm:ss tt") + "] 번역 PAZ파일 삭제중...");

                    logTextBox.AppendText(Environment.NewLine);
                    logTextBox.AppendText("[알림 - " + DateTime.Now.ToString("h:mm:ss tt") + "] 모든 작업이 완료되었습니다");
                    logTextBox.AppendText(Environment.NewLine);
                    logTextBox.AppendText("===============================================================");
                    logTextBox.AppendText(Environment.NewLine);
                    logTextBox.AppendText("한글패치 삭제 완료. 프로그램을 종료하셔도됩니다.");
                    logTextBox.AppendText(Environment.NewLine);
                    logTextBox.AppendText("===============================================================");

                }
                catch (Exception exception)
                {
                    logTextBox.AppendText(Environment.NewLine);
                    logTextBox.AppendText("[에러 - " + DateTime.Now.ToString("h:mm:ss tt") + "] 에러로 인한 한글패치 삭제 취소");
                    MessageBox.Show(exception.Message, exception.GetType().ToString(), MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            else
            {
                logTextBox.AppendText(Environment.NewLine);
                logTextBox.AppendText("[정보 - " + DateTime.Now.ToString("h:mm:ss tt") + "] 한글패치 삭제 취소");
            }



        }

        private void agreementText_Click(object sender, EventArgs e)
        {
            MessageBox.Show("BDO Korean Patch 설정에 오신것을 환영합니다.\r\n먼저, 진행하기전 주의사항을 읽어주세요.\r\n\r\nBlackDesert Online 의 Term of Use 따르면\r\n12.1.7. Modification of the Game Client\r\nThe User is only allowed to use third party programs affecting \r\nthe Game Client to the extent such programs do not significantly \r\naffect the gameplay and do not violate any provision of the Agreement. \r\nThe use of any such use third party program must be validated first by \r\nKakao Games Europe. For that purpose, the User should \r\ncontact customer support before installing \r\nand using such use third party program.\r\n라고 명시되어 있습니다.\r\n\r\n이 한글패치는 대화 및 언어 스크립트쪽만 변경하기때문에\r\n정지의 매우 희박하고 또한 그 전에 한글패치 정지 전례가 없었습니다만\r\n한글패치가 언제나 100% 안전한건 아닙니다.\r\n하지만, 99.9% 정도는 정지당할확률이 없다고 생각합니다.\r\n\r\n\r\n추후의 이 프로그램인하여 발생되는 \r\n모든 문제는 사용자 본인에게 있습니다.\r\n\r\nI, E2Slayer(Developer), shall not be responsible, \r\nnor have any liability whatsoever, \r\nany TOS violation under BlackdesertOnline\r\n\r\n", "Agreement", MessageBoxButtons.OK,MessageBoxIcon.Information);
           // MetroFramework.MetroMessageBox.Show(this, "BDO Korean Patch 설정에 오신것을 환영합니다.\r\n먼저, 진행하기전 주의사항을 읽어주세요.\r\n\r\nBlackDesert Online 의 Term of Use 따르면\r\n12.1.7. Modification of the Game Client\r\nThe User is only allowed to use third party programs affecting \r\nthe Game Client to the extent such programs do not significantly \r\naffect the gameplay and do not violate any provision of the Agreement. \r\nThe use of any such use third party program must be validated first by \r\nKakao Games Europe. For that purpose, the User should \r\ncontact customer support before installing \r\nand using such use third party program.\r\n라고 명시되어 있습니다.\r\n\r\n이 한글패치는 대화 및 언어 스크립트쪽만 변경하기때문에\r\n정지의 매우 희박하고 또한 그 전에 한글패치 정지 전례가 없었습니다만\r\n한글패치로 인해서 정지를 먹으실 가능성은 언제나 존재합니다.\r\n\r\n\r\n추후의 이 프로그램인하여 발생되는 \r\n모든 문제는 사용자 본인에게 있습니다.\r\n\r\nI, E2Slayer(Developer), shall not be responsible, \r\nnor have any liability whatsoever, \r\nany TOS violation under BlackdesertOnline\r\n\r\n", "정보", MessageBoxButtons.OK, MessageBoxIcon.None);
        }
    }
}
