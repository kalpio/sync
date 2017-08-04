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

    public partial class FrmMain : Form
    {
        private Settings _settings;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private bool _close;
        private const string JobName = "sync_job";

        public FrmMain()
        {
            InitializeComponent();
            lblCopyright.Text = @"© Piotr Kalina";
            lblConfigPath.Text = getConfigPath();
            _settings = load();
            JobScheduler.Get().Start();

            if (!_settings.IsEmpty())
            {
                txtFolderA.Text = _settings.FolderA;
                txtFolderB.Text = _settings.FolderB;
                txtInterval.Value = _settings.Interval;
                cmbDirection.SelectedIndex = (int) _settings.Direction;
                cmbIntervalType.SelectedIndex = (int) _settings.IntervalType;
                txtStartAtDate.Value = _settings.StartAt.Date;
                txtStartAtTime.Value = _settings.StartAt;
                chkSkipDeleteFolderB.Checked = _settings.SkipDeleteInFolderB;
                chkSkipDeleteFolderA.Checked = _settings.SkipDeleteInFolderA;
#if !DEBUG
                hideWindow();
#endif
                doSync();
            }
            else
            {
                _settings.ReplicaIdFolderA = Guid.NewGuid();
                _settings.ReplicaIdFolderB = Guid.NewGuid();
            }

            SyncJob.ChangeMainStatusCallback += changeStatus;
        }

        private void changeStatus(string value)
        {
            if (lblStatusInWindow.InvokeRequired)
            {
                lblStatusInWindow.Invoke((MethodInvoker) delegate
                {
                    lblStatusInWindow.Text = value;

                    if (value.Length >= 64)
                    {
                        value = $"{value.Substring(0, 60)}...";
                    }

                    notify.Text = value;
                });
            }

            Application.DoEvents();
        }

        private void btnAnuluj_Click(object sender, EventArgs e)
        {
            hideWindow();
        }

        private void btnFolderA_Click(object sender, EventArgs e)
        {
            var fb = new FolderBrowserDialog
            {
                Description = @"Choose folder...",
                RootFolder = Environment.SpecialFolder.MyComputer,
                ShowNewFolderButton = true
            };

            if (fb.ShowDialog() != DialogResult.OK) return;

            _settings.FolderA = fb.SelectedPath;
            txtFolderA.Text = _settings.FolderA;
        }

        private void btnFolderB_Click(object sender, EventArgs e)
        {
            var fb = new FolderBrowserDialog
            {
                Description = @"Choose folder...",
                RootFolder = Environment.SpecialFolder.MyComputer,
                ShowNewFolderButton = true
            };

            if (fb.ShowDialog() != DialogResult.OK) return;

            _settings.FolderB = fb.SelectedPath;
            txtFolderB.Text = _settings.FolderB;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!save())
            {
                MessageBox.Show(@"Sorry, I can't save the configuration!", @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                hideWindow();
            }
        }

        private void hideWindow()
        {
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
            Hide();
            notify.Visible = true;
        }

        private void showWindow()
        {
            WindowState = FormWindowState.Normal;
            notify.Visible = true;
            ShowInTaskbar = true;
            Visible = true;
            Activate();
        }

        private bool save()
        {
            try
            {
                var content = JsonConvert.SerializeObject(_settings, Formatting.None);
                var dirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "kalpio", "sync");
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                const string fileName = "config.json";
                File.WriteAllText(Path.Combine(dirPath, fileName), content, Encoding.UTF8);

                doSync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return false;
            }
        }

        private static string getConfigPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "kalpio", "sync",
                "config.json");
        }

        private Settings load()
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
                _logger.Error(ex);
            }

            return new Settings();
        }

        private void doSync()
        {
            _logger.Debug("doSync start...");
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHss");

            if (JobScheduler.JobExist(JobName, "Workers"))
            {
                JobScheduler.Get().DeleteJob(new JobKey(JobName, "Workers"));
            }

            var jobData = new JobDataMap {{"settings", _settings}};

            var job = JobBuilder.Create<SyncJob>()
                .WithIdentity(JobName, "Workers")
                .SetJobData(jobData)
                .Build();

            var triggerKey = $"trigger_{timestamp}";

            var trigger = TriggerBuilder.Create()
                .WithIdentity(triggerKey, "Workers")
                .WithCalendarIntervalSchedule(x =>
                {
                    x.WithInterval(_settings.Interval, (IntervalUnit) (int) _settings.IntervalType);
                })
                .StartAt(_settings.StartAt)
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
            _close = true;
            Close();
        }

        private void txtInterval_ValueChanged(object sender, EventArgs e)
        {
            _settings.Interval = Convert.ToInt32(txtInterval.Value);
        }

        private void cmbIntervalType_SelectedIndexChanged(object sender, EventArgs e)
        {
            _settings.IntervalType = (IntervalType) cmbIntervalType.SelectedIndex;
        }

        private void cmbDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            _settings.Direction = (Direction) cmbDirection.SelectedIndex;
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_close)
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
                _settings.FolderA = txtFolderA.Text;
                txtFolderA.BackColor = Color.White;
            }
            else
            {
                MessageBox.Show(@"Folder A - folder already exists!", @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                txtFolderA.Focus();
                txtFolderA.BackColor = Color.Red;
            }
        }

        private void txtFolderB_TextChanged(object sender, EventArgs e)
        {
            if (Directory.Exists(txtFolderB.Text))
            {
                _settings.FolderB = txtFolderB.Text;
                txtFolderB.BackColor = Color.White;
            }
            else
            {
                MessageBox.Show(@"Folder B - folder already exists!", @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                txtFolderB.Focus();
                txtFolderB.BackColor = Color.Red;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            _settings = new Settings();
            txtFolderA.Text = _settings.FolderA;
            txtFolderB.Text = _settings.FolderB;
            txtInterval.Value = _settings.Interval;
            cmbIntervalType.SelectedIndex = (int) _settings.IntervalType;
            cmbDirection.SelectedIndex = (int) _settings.Direction;
            txtStartAtDate.Value = _settings.StartAt.Date;
            txtStartAtTime.Value = _settings.StartAt.Date;
        }

        private void txtStartAtDate_ValueChanged(object sender, EventArgs e)
        {
            _settings.StartAt = txtStartAtDate.Value.Date;
        }

        private void txtStartAtTime_ValueChanged(object sender, EventArgs e)
        {
            _settings.StartAt = _settings.StartAt
                .AddHours(txtStartAtTime.Value.Hour)
                .AddMinutes(txtStartAtTime.Value.Minute);
        }

        private void chkSkipDeleteFolderB_CheckedChanged(object sender, EventArgs e)
        {
            _settings.SkipDeleteInFolderB = chkSkipDeleteFolderB.Checked;
        }

        private void chkSkipDeleteFolderA_CheckedChanged(object sender, EventArgs e)
        {
            _settings.SkipDeleteInFolderA = chkSkipDeleteFolderA.Checked;
        }
    }
}
