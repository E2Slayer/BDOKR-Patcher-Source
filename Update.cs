using AutoUpdaterDotNET;
using System;
using System.Net;
using System.Windows.Forms;


namespace BDOKRPatch
{
    public partial class Update : MetroFramework.Forms.MetroForm
    {
        public Update()
        {
            InitializeComponent();
        }

        private bool isUpdated = false;

        private void Update_Load(object sender, EventArgs e)
        {
            // Initializing
            logTextBox.AppendText(Environment.NewLine);
            logTextBox.AppendText("통신서버와 연결중... ");

            // Getting context from the server 
            var url = "https://raw.githubusercontent.com/E2Slayer/BDOKRPatchData/master/Release/user.ini";
            var textFromFile = (new WebClient()).DownloadString(url);

            // Validation code is 3053
            string validation = "3053";



            // if the text from the server and validation code are matched
            if (string.Equals(textFromFile, validation))
            {

                // Display message of succeession 
                logTextBox.AppendText(Environment.NewLine);
                logTextBox.AppendText("통신서버와 연결성공 ! ");

                // Start the auto-updater 
                logTextBox.AppendText(Environment.NewLine);
                logTextBox.AppendText("업데이트 서버와 통신시작 " + DateTime.Now.ToString("h:mm:ss tt"));

                // Start the updater based on github config.xml 
                AutoUpdater.Start("https://raw.githubusercontent.com/E2Slayer/BDOKRPatchData/master/Release/config.xml");

                // Register update eveent
                AutoUpdater.CheckForUpdateEvent += AutoUpdaterOnCheckForUpdateEvent;
            }
            else
            {

                // if the validation fails, do not update
                logTextBox.AppendText(Environment.NewLine);
                logTextBox.AppendText("통신서버와 연결실패 ");
                MetroFramework.MetroMessageBox.Show(this, "통신서버와 연결을 실패했습니다. \n잠시뒤에 시도해주세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void AutoUpdaterOnCheckForUpdateEvent(UpdateInfoEventArgs args)
        {
            if (args != null)
            {
                if (args.IsUpdateAvailable)
                {
                    logTextBox.AppendText(Environment.NewLine);
                    logTextBox.AppendText($"새로운 버전 {args.CurrentVersion} 발견 ");
                    DialogResult dialogResult;
                    if (args.Mandatory)
                    {
                        dialogResult =
                            MessageBox.Show(
                                $"새로운버전 {args.CurrentVersion}이 발견되었습니다. \n현재 사용하시는 버전은 {args.InstalledVersion}입니다. \n확인(OK)를 누르시면 업데이트를 진행합니다.", @"업데이트 가능",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                    }
                    else
                    {
                        dialogResult =
                            MessageBox.Show(
                                $"새로운버전 {args.CurrentVersion}이 발견되었습니다. \n현재 사용하시는 버전은 {args.InstalledVersion}입니다. \n업데이트 하시겠습니까?", @"업데이트 가능",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Information);
                    }

                    // Uncomment the following line if you want to show standard update dialog instead.
                    // AutoUpdater.ShowUpdateForm();

                    if (dialogResult.Equals(DialogResult.Yes) || dialogResult.Equals(DialogResult.OK))
                    {
                        try
                        {
                            if (AutoUpdater.DownloadUpdate())
                            {
                                logTextBox.AppendText(Environment.NewLine);
                                logTextBox.AppendText("업데이트 완료");
                                Application.Exit();

                            }
                        }
                        catch (Exception exception)
                        {
                            logTextBox.AppendText(Environment.NewLine);
                            logTextBox.AppendText("업데이트 실패");
                            MessageBox.Show(exception.Message, exception.GetType().ToString(), MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        logTextBox.AppendText(Environment.NewLine);
                        logTextBox.AppendText("업데이트 취소");
                        isUpdated = true;
                    }
                }
                else
                {
                    // MessageBox.Show("최신버전을 사용중이십니다.", @"업데이트 정보",MessageBoxButtons.OK, MessageBoxIcon.Information);
                    logTextBox.AppendText(Environment.NewLine);
                    logTextBox.AppendText("업데이트 서버와 통신종료 - 이유: 최신버전");
                    logTextBox.AppendText(Environment.NewLine);
                    logTextBox.AppendText("최신버전 사용중입니다 !");
                    isUpdated = true;
                }
            }
            else
            {
                MessageBox.Show(
                        @"서버와 연결이 실패했습니다. 나중에 시도 해주세요.",
                        @"업데이트 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logTextBox.AppendText(Environment.NewLine);
                logTextBox.AppendText("서버와 통신 실패");
                isUpdated = false;
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (isUpdated)
            {
                this.Hide();
                var installForm = new Install();
                installForm.Closed += (s, args) => this.Close();
                installForm.Show();
            }
            else
            {
                MessageBox.Show(
                    "업데이트가 실패했습니다. \n프로그램을 종료합니다.",
                    @"업데이트 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Windows.Forms.Application.ExitThread();
                //System.Windows.Forms.Application.Exit();
            }
        }
    }
}
