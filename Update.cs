using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoUpdaterDotNET;


//using AutoUpdaterDotNET;

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

            logTextBox.AppendText(Environment.NewLine);
            logTextBox.AppendText("업데이트 서버와 통신시작 " + DateTime.Now.ToString("h:mm:ss tt"));

            AutoUpdater.Start("https://raw.githubusercontent.com/E2Slayer/BDOKRPatchData/master/Release/config.xml");
            //AutoUpdater.Mandatory = false;
            AutoUpdater.CheckForUpdateEvent += AutoUpdaterOnCheckForUpdateEvent;
            // AutoUpdater.DownloadPath = Environment.CurrentDirectory;
             
            //AutoUpdater.UpdateFormSize = new System.Drawing.Size(800, 600);
            // AutoUpdater.UpdateMode = Mode.Forced;
            // System.Windows.Forms.MessageBox.Show("done", "Message");
        }

        private void AutoUpdaterOnCheckForUpdateEvent(UpdateInfoEventArgs args)
        {
            if (args != null)
            {
                if (args.IsUpdateAvailable)
                {
                    DialogResult dialogResult;
                    if (args.Mandatory)
                    {
                        dialogResult =
                            MessageBox.Show(
                                $@"새로운버전 {args.CurrentVersion}이 발견되었습니다. \n현재 사용하시는 버전은 {args.InstalledVersion}입니다. \n확인(OK)를 누르시면 업데이트를 진행합니다.", @"업데이트 가능",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                    }
                    else
                    {
                        dialogResult =
                            MessageBox.Show(
                                $@"새로운버전 {args.CurrentVersion}이 발견되었습니다. \n현재 사용하시는 버전은 {args.InstalledVersion}입니다. \n업데이트 하시겠습니까?", @"업데이트 가능",
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
                        isUpdated = true;
                    }
                }
                else
                {
                    MessageBox.Show(@"현재 업데이트한 가능한버전이 없습니다. \n최신버전을 사용중이십니다.", @"업데이트 정보",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    logTextBox.AppendText(Environment.NewLine);
                    logTextBox.AppendText("최신버전 사용중");
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
            this.Hide();
            var installForm = new Install();
            installForm.Closed += (s, args) => this.Close();
            installForm.Show();
        }
    }
}
