using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace BDOKRPatch
{


    public partial class Install : MetroFramework.Forms.MetroForm
    {
        class ConfigStructure
        {
            public ConfigStructure(int steam, string installPath, int validatedirectory, int font, int languageselection)
            {
                Steam = steam;
                InstallPath = installPath;
                Validatedirectory = validatedirectory;
                Font = font;
                Languageselection = languageselection;
            }

            public int Steam { get; set; }
            public string InstallPath { get; set; }
            public int Validatedirectory { get; set; }
            public int Font { get; set; }
            public int Languageselection { get; set; }


        }


        private readonly ConfigStructure configList = new ConfigStructure(0, "None", 0, 1, 0);

        private string selectedPath = @"";
        private bool comChecker = false;

        private readonly Queue qt = new Queue();
        private int installType = 0;



        public Install()
        {
            InitializeComponent();
        }



        private void locationButton_Click(object sender, EventArgs e)
        {
            Communication_Checker();

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
                        MetroFramework.MetroMessageBox.Show(this, "올바른 Black Desert Online 폴더를 찾았습니다. \n진행하셔도 좋습니다", "정보", MessageBoxButtons.OK, MessageBoxIcon.None);
                        nextButton2.Enabled = true;
                        selectedPath = fbd.SelectedPath;
                    }
                    else
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Black Desert Online 폴더를 제대로 선택하셨는지 확인해주세요", "경고", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        nextButton2.Enabled = false;
                    }
                }
            }
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

            if (configList.Validatedirectory == 1)
            {
                MetroFramework.MetroMessageBox.Show(this, "이전 설치정보를 발견했습니다. \n한글패치탭으로 바로 넘어갑니다.\n\n다른 설정을 바꾸시고 싶으시면\n탭을 수동으로 선택하시면 설정을 바꿀 수 있습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                metroTabControl1.SelectedTab = metroTabPage4;
                nextButton2.Enabled = true;
                this.metroTabPage3.Enabled = true;

                this.metroTabPage4.Enabled = true;

                if (configList.Steam == 1)
                {
                    steamRadio.Checked = true;
                }

                locationTextBox.Text = configList.InstallPath;
                FontComboBox.SelectedIndex = configList.Font;

                switch (configList.Languageselection)
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
            logTextBox.AppendText("[다운로드시작 - " + DateTime.Now.ToString("h:mm:ss tt") + "] " + display);

            WebClient webClient = new WebClient();
            // ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            webClient.DownloadFileCompleted += Completed;
            webClient.QueryString.Add("file", display); // here you can add values
                                                        // webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            try
            {
                webClient.DownloadFileAsync(new Uri(url), location);
            }
            catch (Exception ex)
            {
                logTextBox.AppendText(Environment.NewLine);
                logTextBox.AppendText("[다운로드에러 - " + DateTime.Now.ToString("h:mm:ss tt") + "] " + ex.Message);
                logTextBox.AppendText(Environment.NewLine);
                logTextBox.AppendText("[다운로드에러파일] " + display);
                logTextBox.AppendText(Environment.NewLine);
                logTextBox.AppendText("개발자에게 이 메세지를 보고해주세요.");
            }

        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {

            if (qt.Count >= 1)
            {
                string fileIdentifier = ((WebClient)(sender)).QueryString["file"];
                logTextBox.AppendText(Environment.NewLine);
                logTextBox.AppendText("[다운로드완료] - " + DateTime.Now.ToString("h:mm:ss tt") + "] " + fileIdentifier);
                qt.Dequeue();

                if (qt.Count == 0)
                {
                    logTextBox.AppendText(Environment.NewLine);
                    logTextBox.AppendText("[알림 - " + DateTime.Now.ToString("h:mm:ss tt") + "] 모든 작업이 완료되었습니다");
                    logTextBox.AppendText(Environment.NewLine);
                    logTextBox.AppendText("===============================================================");
                    logTextBox.AppendText(Environment.NewLine);
                    if (installType == 1)
                    {
                        logTextBox.AppendText("한글패치가 완료되었습니다 !! 프로그램을 종료하셔도됩니다.");
                        patchButton.Enabled = true;
                        uninstallButton.Enabled = true;
                        installType = 0;
                    }
                    else if (installType == 2)
                    {
                        logTextBox.AppendText("한글패치가 삭제되었습니다. 프로그램을 종료하셔도됩니다.");
                        patchButton.Enabled = true;
                        uninstallButton.Enabled = true;

                        installType = 0;
                    }

                    logTextBox.AppendText(Environment.NewLine);
                    logTextBox.AppendText("===============================================================");
                }
            }
        }

        private void patchButton_Click(object sender, EventArgs e)
        {
            Communication_Checker();
            patchButton.Enabled = false;
            uninstallButton.Enabled = false;
            installType = 1;

            string selectedLangStr = "None";

            XmlDocument doc = new XmlDocument();
            doc.Load(Environment.CurrentDirectory + "\\config.xml");
            XmlNodeList xNodeList = doc.SelectNodes("/root");
            //MetroFramework.MetroMessageBox.Show(this, "config.xml을 열수없습니다. \n프로그램을 업데이트해주세요", "경고", MessageBoxButtons.OK, MessageBoxIcon.Error);


            try
            {
                doc.SelectSingleNode("//root/steam").InnerText = steamRadio.Checked ? "1" : "0";
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

            }
            catch (Exception exception)
            {
                MetroFramework.MetroMessageBox.Show(this, "config.xml 저장하는 도중 에러가 발생했습니다. \n이 메세지를 개발자에게 보여주세요. \n" + exception, "경고", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

            doc.Save(Environment.CurrentDirectory + "\\config.xml");

            

            // selectedPath = locationTextBox.Text;
            string currentLocation = selectedPath + @"\";

            string pathAds = currentLocation + @"\ads";





            qt.Clear();

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
                    Directory.CreateDirectory(pathAds);
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


            /*
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
            */


            Download(@"https://github.com/E2Slayer/BDOKRPatchData/raw/master/Data/languagedata_en_toKR.PAZ", currentLocation + @"ads\languagedata_" + selectedLangStr + "_toKR.PAZ", "languagedata_" + selectedLangStr + "_toKR.PAZ");
            qt.Enqueue(true);
            Download(@"https://github.com/E2Slayer/BDOKRPatchData/raw/master/Data/languagedata_en.loc", currentLocation + @"ads\languagedata_" + selectedLangStr + ".loc", "languagedata_" + selectedLangStr + ".loc");
            qt.Enqueue(true);

            //  Download(@"https://raw.githubusercontent.com/E2Slayer/BDOKRPatchData/master/KRPVersion", currentLocation + @"ads\KRPVersion", "KRPVersion.chk");

            /*
             *             "리디바탕",
            "나눔스퀘어체 Bold",
            "나눔바른고딕체",
            "양진체",
            "빙그래체2 Bold",
            "배민 을지로체"
             */

            string fontURL = @"https://github.com/E2Slayer/BDOKRPatchData/raw/master/Data/font/";
            switch (FontComboBox.SelectedIndex)
            {
                case 0: // ridi
                    fontURL += @"RIDIBatang.ttf";
                    break;
                case 1: // nanum square
                    fontURL += @"NanumSquareB.ttf";
                    break;
                case 2: // nanum barun
                    fontURL += @"NanumBarunGothic.ttf";
                    break;
                case 3: // yangjin
                    fontURL += @"YangJin.ttf";
                    break;
                case 4: // bing
                    fontURL += @"Binggrae%E2%85%A1-Bold.ttf";
                    break;
                case 5: // bemin
                    fontURL += @"BMEULJIROTTF.ttf";
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
                    Directory.CreateDirectory(path);
                    //Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path));
                    logTextBox.AppendText(Environment.NewLine);
                    logTextBox.AppendText("[정보 - " + DateTime.Now.ToString("h:mm:ss tt") + "] prestringtable 폴더 생성완료");

                    path += @"\font";
                    Directory.CreateDirectory(path);
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
            qt.Enqueue(true);

            //Task.Delay(1000).Wait();


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


            if (!File.Exists(Environment.CurrentDirectory + "\\config.xml"))
            {
                MetroFramework.MetroMessageBox.Show(this, "config.xml을 열수없습니다. \n프로그램을 재설치해주세요", "경고", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.ExitThread();
                Application.Exit();
            }

            XmlDocument doc = new XmlDocument();

            doc.Load(Environment.CurrentDirectory + "\\config.xml");
            XmlNodeList xNodeList = doc.SelectNodes("/root");

            if (xNodeList != null)
            {
                foreach (XmlNode xNode in xNodeList)
                {
                    try
                    {
                        configList.Steam = XmlConvert.ToInt32(xNode["steam"]?.InnerText ?? throw new InvalidOperationException());
                        configList.InstallPath = xNode["installpath"]?.InnerText;
                        configList.Validatedirectory = XmlConvert.ToInt32(xNode["validatedirectory"]?.InnerText ?? throw new InvalidOperationException());
                        configList.Font = XmlConvert.ToInt32(xNode["font"]?.InnerText ?? throw new InvalidOperationException());
                        configList.Languageselection = XmlConvert.ToInt32(xNode["languageselection"]?.InnerText ?? throw new InvalidOperationException());
                    }
                    catch (Exception exception)
                    {
                        MetroFramework.MetroMessageBox.Show(this, "config.xml을 여는 도중 에러가 발생했습니다. \n프로그램을 재설치해주세요. \n" + exception, "경고", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw;
                    }
                }

                selectedPath = configList.InstallPath;
            }
            else
            {
                MetroFramework.MetroMessageBox.Show(this, "config.xml을 찾을 수 없습니다. \n프로그램을 재설치해주세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.ExitThread();
                Application.Exit();
            }
        }

        private void Communication_Loader()
        {
            var url = "https://raw.githubusercontent.com/E2Slayer/BDOKRPatchData/master/Release/user.ini";
            var textFromFile = (new WebClient()).DownloadString(url);
            string validation = "3053";
            comChecker = string.Equals(textFromFile, validation);
        }

        private void Communication_Checker()
        {
            if (comChecker)
                return;

            MetroFramework.MetroMessageBox.Show(this, "통신서버와 연결을 실패했습니다. \n잠시뒤에 시도해주세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.ExitThread();
            Application.Exit();
        }

        private void uninstallButton_Click(object sender, EventArgs e)
        {

            installType = 2;
            DialogResult dialogResult =
                MessageBox.Show(
                    "BDO 한글패치된 파일들을 삭제합니다. \n모든 패치파일들은 원상복귀가 됩니다. \n진행하시겠습니까? ", @"한글패치 삭제",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);
            if (dialogResult.Equals(DialogResult.Yes) || dialogResult.Equals(DialogResult.OK))
            {
                try
                {
                    string[] regionList = { "en", "fr", "de", "sp" }; //region list 

                    patchButton.Enabled = false;
                    qt.Clear(); //clear the queue 


                    string currentLocation = selectedPath + @"\";
                    DirectoryInfo di = new DirectoryInfo(currentLocation + @"prestringtable\");
                    if (Directory.Exists(currentLocation + @"prestringtable\"))
                    {
                        logTextBox.AppendText(Environment.NewLine);
                        logTextBox.AppendText("[정보 - " + DateTime.Now.ToString("h:mm:ss tt") + "] prestringtable 폴더 삭제중...");


                        foreach (FileInfo file in di.GetFiles())
                        {
                            file.Delete(); // Delete all files in the folder
                        }

                        foreach (DirectoryInfo dir in di.GetDirectories())
                        {
                            dir.Delete(true); // Now delete folders
                        }

                    }
                    else
                    {
                        logTextBox.AppendText(Environment.NewLine);
                        logTextBox.AppendText("[정보 - " + DateTime.Now.ToString("h:mm:ss tt") + "] prestringtable 폴더가 존재하지 않습니다.");

                    }

                    logTextBox.AppendText(Environment.NewLine);
                    logTextBox.AppendText("[정보 - " + DateTime.Now.ToString("h:mm:ss tt") + "] locale 원본파일들 다운로드 시작...");


                    foreach (var region in regionList)
                    {
                        Download(@"https://github.com/E2Slayer/BDOKRPatchData/raw/master/Original/languagedata_" + region + ".loc", currentLocation + @"ads\languagedata_" + region + ".loc", "languagedata_" + region + ".loc");
                        qt.Enqueue(true);

                        if (File.Exists(currentLocation + @"ads\languagedata_" + region + "_backup.loc"))
                        {
                            File.Delete(currentLocation + @"ads\languagedata_" + region + "_backup.loc");
                        }

                        if (File.Exists(currentLocation + @"ads\languagedata_" + region + "_toKR.PAZ"))
                        {
                            File.Delete(currentLocation + @"ads\languagedata_" + region + "_toKR.PAZ");
                        }
                    }

                    /*
                    Download(@"https://github.com/E2Slayer/BDOKRPatchData/raw/master/Original/languagedata_en.loc", currentLocation + @"ads\languagedata_en.loc", "languagedata_en.loc");
                    qt.Enqueue(true);
                    Download(@"https://github.com/E2Slayer/BDOKRPatchData/raw/master/Original/languagedata_fr.loc", currentLocation + @"ads\languagedata_fr.loc", "languagedata_fr.loc");
                    qt.Enqueue(true);
                    Download(@"https://github.com/E2Slayer/BDOKRPatchData/raw/master/Original/languagedata_de.loc", currentLocation + @"ads\languagedata_de.loc", "languagedata_de.loc");
                    qt.Enqueue(true);
                    Download(@"https://github.com/E2Slayer/BDOKRPatchData/raw/master/Original/languagedata_sp.loc", currentLocation + @"ads\languagedata_sp.loc", "languagedata_sp.loc");
                    qt.Enqueue(true);
                    */

                    /*
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
                    */
                    logTextBox.AppendText(Environment.NewLine);
                    logTextBox.AppendText("[정보 - " + DateTime.Now.ToString("h:mm:ss tt") + "] 번역 PAZ파일 삭제중...");
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
            MessageBox.Show(
                "BDO Korean Patch 설정에 오신것을 환영합니다.\r\n먼저, 진행하기전 주의사항을 읽어주세요.\r\n\r\nBlackDesert Online 의 Term of Use 따르면\r\n12.1.7. Modification of the Game Client\r\nThe User is only allowed to use third party programs affecting \r\nthe Game Client to the extent such programs do not significantly \r\naffect the gameplay and do not violate any provision of the Agreement. \r\nThe use of any such use third party program must be validated first by \r\nKakao Games Europe. For that purpose, the User should \r\ncontact customer support before installing \r\nand using such use third party program.\r\n라고 명시되어 있습니다.\r\n\r\n이 한글패치는 대화 및 언어 스크립트쪽만 변경하기때문에\r\n정지의 매우 희박하고 또한 그 전에 한글패치 정지 전례가 없었습니다만\r\n한글패치가 언제나 100% 안전한건 아닙니다.\r\n하지만, 99.9% 정도는 정지당할확률이 없다고 생각합니다.\r\n\r\n\r\n추후의 이 프로그램인하여 발생되는 \r\n모든 문제는 사용자 본인에게 있습니다.\r\n\r\nI, E2Slayer(Developer), shall not be responsible, \r\nnor have any liability whatsoever, \r\nany TOS violation under BlackdesertOnline\r\n\r\n",
                "Agreement", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
