using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;
using NLog;
using Quartz;

namespace Sync
{
    public delegate void ChangeMainStatusDelegate(string value);

    public partial class frmMain : Form
    {
        Settings settings;
        Logger logger = LogManager.GetCurrentClassLogger();
        bool close = false;
        string jobName = "sync_job";

        public frmMain()
        {
            InitializeComponent();
            lblCopyright.Text = "\u00A9 Piotr Kalina";
            lblConfigPath.Text = getConfigPath();
            settings = load();
            JobScheduler.Get().Start();

            if (!settings.IsEmpty())
            {
                txtFolderA.Text = settings.FolderA;
                txtFolderB.Text = settings.FolderB;
                txtInterval.Value = settings.Interval;
                cmbDirection.SelectedIndex = (int)settings.Direction;
                cmbIntervalType.SelectedIndex = (int)settings.IntervalType;

                hideWindow();
                doSync();
            }
            else
            {
                settings.ReplicaIdFolderA = Guid.NewGuid();
                settings.ReplicaIdFolderB = Guid.NewGuid();
            }

            SyncJob.ChangeMainStatusCallback += new ChangeMainStatusDelegate(changeStatus);
        }

        void changeStatus(string value)
        {
            if (lblStatusInWindow.InvokeRequired)
            {
                lblStatusInWindow.Invoke((MethodInvoker)delegate
                {
                    lblStatusInWindow.Text = value;
                });
            }
            notify.Text = value;

            Application.DoEvents();
        }

        private void btnAnuluj_Click(object sender, EventArgs e)
        {
            hideWindow();
        }

        private void btnFolderA_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fb = new FolderBrowserDialog()
            {
                Description = "Wybierz folder...",
                RootFolder = Environment.SpecialFolder.MyComputer,
                ShowNewFolderButton = true
            };
            if (fb.ShowDialog() == DialogResult.OK)
            {
                settings.FolderA = fb.SelectedPath;
                txtFolderA.Text = settings.FolderA;
            }
        }

        private void btnFolderB_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fb = new FolderBrowserDialog()
            {
                Description = "Wybierz folder...",
                RootFolder = Environment.SpecialFolder.MyComputer,
                ShowNewFolderButton = true
            };
            if (fb.ShowDialog() == DialogResult.OK)
            {
                settings.FolderB = fb.SelectedPath;
                txtFolderB.Text = settings.FolderB;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!save())
            {
                MessageBox.Show("Nie udało się zapisać konfiguracji!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                hideWindow();
            }
        }

        void hideWindow()
        {
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
            Hide();
            notify.Visible = true;
        }

        void showWindow()
        {
            WindowState = FormWindowState.Normal;
            notify.Visible = true;
            ShowInTaskbar = true;
            Visible = true;
            Activate();
        }

        bool save()
        {
            try
            {
                var content = JsonConvert.SerializeObject(settings, Formatting.None);
                var dirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "kalpio", "sync");
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                var fileName = "config.json";
                File.WriteAllText(Path.Combine(dirPath, fileName), content, Encoding.UTF8);

                doSync();

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return false;
            }
        }

        string getConfigPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "kalpio", "sync", "config.json");
        }

        Settings load()
        {
            try
            {
                var path = getConfigPath();
                if (File.Exists(path))
                {
                    var content = File.ReadAllText(path, Encoding.UTF8);
                    return JsonConvert.DeserializeObject<Settings>(content);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            return new Settings();
        }

        void doSync()
        {
            logger.Debug("doSync start...");
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHss");

            if (JobScheduler.JobExist(jobName, "Workers"))
            {
                JobScheduler.Get().DeleteJob(new JobKey(jobName, "Workers"));
            }

            var jobData = new JobDataMap { { "settings", settings } };

            IJobDetail job = JobBuilder.Create<SyncJob>()
                .WithIdentity(jobName, "Workers")
                .SetJobData(jobData)
                .Build();

            var triggerKey = $"trigger_{timestamp}";

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(triggerKey, "Workers")
                .WithCalendarIntervalSchedule((x) =>
                {
                    x.WithInterval(settings.Interval, (IntervalUnit)((int)settings.IntervalType));
                })
                .StartAt(settings.StartAt)
                .WithPriority(1)
                .Build();

            JobScheduler.Get().ScheduleJob(job, trigger);
        }

        private void notifyContextMenu_Open_Click(object sender, EventArgs e)
        {
            showWindow();
        }

        private void notifyContextMenu_Close_Click(object sender, EventArgs e)
        {
            close = true;
            Close();
        }

        private void txtInterval_ValueChanged(object sender, EventArgs e)
        {
            settings.Interval = Convert.ToInt32(txtInterval.Value);
        }

        private void cmbIntervalType_SelectedIndexChanged(object sender, EventArgs e)
        {
            settings.IntervalType = (IntervalType)cmbIntervalType.SelectedIndex;
        }

        private void cmbDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            settings.Direction = (Direction)cmbDirection.SelectedIndex;
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!close)
            {
                e.Cancel = true;
                hideWindow();
            }
            JobScheduler.Get().Shutdown();
        }

        private void txtFolderA_TextChanged(object sender, EventArgs e)
        {
            if (Directory.Exists(txtFolderA.Text))
            {
                settings.FolderA = txtFolderA.Text;
                txtFolderA.BackColor = Color.White;
            }
            else
            {
                MessageBox.Show("Folder A - folder already exists!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFolderA.Focus();
                txtFolderA.BackColor = Color.Red;
            }
        }

        private void txtFolderB_TextChanged(object sender, EventArgs e)
        {
            if (Directory.Exists(txtFolderB.Text))
            {
                settings.FolderB = txtFolderB.Text;
                txtFolderB.BackColor = Color.White;
            }
            else
            {
                MessageBox.Show("Folder B - folder already exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFolderB.Focus();
                txtFolderB.BackColor = Color.Red;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            settings = new Settings();
            txtFolderA.Text = settings.FolderA;
            txtFolderB.Text = settings.FolderB;
            txtInterval.Value = settings.Interval;
            cmbIntervalType.SelectedIndex = (int)settings.IntervalType;
            cmbDirection.SelectedIndex = (int)settings.Direction;
            txtStartAtDate.Value = settings.StartAt.Date;
            txtStartAtTime.Value = settings.StartAt.Date;
        }

        private void txtStartAtDate_ValueChanged(object sender, EventArgs e)
        {
            settings.StartAt = txtStartAtDate.Value.Date;
        }

        private void txtStartAtTime_ValueChanged(object sender, EventArgs e)
        {
            settings.StartAt = settings.StartAt
                .AddHours(txtStartAtTime.Value.Hour)
                .AddMinutes(txtStartAtTime.Value.Minute);
        }
    }
}
