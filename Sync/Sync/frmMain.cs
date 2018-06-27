using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;
using NLog;

namespace Sync
{
  public delegate void ChangeMainStatusDelegate(string value);

  public partial class FrmMain : Form
  {
    private          Settings _settings;
    private readonly Logger   _logger = LogManager.GetCurrentClassLogger();
    private          bool     _close;
    private SyncJob  _syncJob;

    public FrmMain()
    {
      InitializeComponent();
      lblCopyright.Text  = @"© Piotr Kalina";
      lblConfigPath.Text = GetConfigPath();
      _settings          = LoadSettings();

      if (!_settings.IsEmpty())
      {
        txtFolderA.Text               = _settings.FolderA;
        txtFolderB.Text               = _settings.FolderB;
        txtInterval.Value             = _settings.Interval;
        cmbDirection.SelectedIndex    = (int) _settings.Direction;
        cmbIntervalType.SelectedIndex = (int) _settings.IntervalType;
        txtStartAtDate.Value          = _settings.StartAt.Date;
        txtStartAtTime.Value          = _settings.StartAt;
        chkSkipDeleteFolderB.Checked  = _settings.SkipDeleteInFolderB;
        chkSkipDeleteFolderA.Checked  = _settings.SkipDeleteInFolderA;
#if !DEBUG
                hideWindow();
#endif
        DoSync();
      }
      else
      {
        _settings.ReplicaIdFolderA = Guid.NewGuid();
        _settings.ReplicaIdFolderB = Guid.NewGuid();
      }

      SyncJob.ChangeMainStatusCallback += ChangeStatus;
    }

    private void ChangeStatus(string value)
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
      HideWindow();
    }

    private void btnFolderA_Click(object sender, EventArgs e)
    {
      var fb = new FolderBrowserDialog
      {
        Description         = @"Choose folder...",
        RootFolder          = Environment.SpecialFolder.MyComputer,
        ShowNewFolderButton = true
      };

      if (fb.ShowDialog() != DialogResult.OK) return;

      _settings.FolderA = fb.SelectedPath;
      txtFolderA.Text   = _settings.FolderA;
    }

    private void btnFolderB_Click(object sender, EventArgs e)
    {
      var fb = new FolderBrowserDialog
      {
        Description         = @"Choose folder...",
        RootFolder          = Environment.SpecialFolder.MyComputer,
        ShowNewFolderButton = true
      };

      if (fb.ShowDialog() != DialogResult.OK) return;

      _settings.FolderB = fb.SelectedPath;
      txtFolderB.Text   = _settings.FolderB;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (!Save())
      {
        MessageBox.Show(@"Sorry, I can't save the configuration!", @"Error", MessageBoxButtons.OK,
          MessageBoxIcon.Error);
      }
      else
      {
        HideWindow();
      }
    }

    private void HideWindow()
    {
      WindowState   = FormWindowState.Minimized;
      ShowInTaskbar = false;
      Hide();
      notify.Visible = true;
    }

    private void ShowWindow()
    {
      WindowState    = FormWindowState.Normal;
      notify.Visible = true;
      ShowInTaskbar  = true;
      Visible        = true;
      Activate();
    }

    private bool Save()
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

        DoSync();

        return true;
      }
      catch (Exception ex)
      {
        _logger.Error(ex);
        return false;
      }
    }

    private static string GetConfigPath()
    {
      return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "kalpio", "sync",
        "config.json");
    }

    private Settings LoadSettings()
    {
      try
      {
        var path = GetConfigPath();
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

    private void DoSync()
    {
      _logger.Debug("doSync start...");
      _syncJob = new SyncJob(_settings);
      _syncJob.Start();
    }

    private void notifyContextMenu_Open_Click(object sender, EventArgs e)
    {
      ShowWindow();
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
        HideWindow();
      }

      _syncJob.Stop();
    }

    private void txtFolderA_TextChanged(object sender, EventArgs e)
    {
      if (Directory.Exists(txtFolderA.Text))
      {
        _settings.FolderA    = txtFolderA.Text;
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
        _settings.FolderB    = txtFolderB.Text;
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
      _settings                     = new Settings();
      txtFolderA.Text               = _settings.FolderA;
      txtFolderB.Text               = _settings.FolderB;
      txtInterval.Value             = _settings.Interval;
      cmbIntervalType.SelectedIndex = (int) _settings.IntervalType;
      cmbDirection.SelectedIndex    = (int) _settings.Direction;
      txtStartAtDate.Value          = _settings.StartAt.Date;
      txtStartAtTime.Value          = _settings.StartAt.Date;
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

    private void lblConfigPath_OnDoubleClick(object sender, EventArgs eventArgs)
    {
      var filePath = lblConfigPath.Text;
      var dir      = Path.GetDirectoryName(filePath);

      if (!Directory.Exists(dir)) return;

      Process.Start("explorer.exe", dir);
    }
  }
}
