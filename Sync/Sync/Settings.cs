using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace Sync
{
    internal class Settings
    {
        public const string BackupFolderName = "___backups";

        public Settings()
        {
            IntervalType = IntervalType.Minute;
            Direction = Direction.UploadAndDownload;
            ReplicaIdFolderA = Guid.Empty;
            ReplicaIdFolderB = Guid.Empty;
            Interval = 10;
            StartAt = DateTime.Now;
            SkipDeleteInFolderB = false;
        }

        public string FolderA { get; set; }

        public string FolderB { get; set; }

        public int Interval { get; set; }

        public IntervalType IntervalType { get; set; }

        public Direction Direction { get; set; }

        public bool SkipDeleteInFolderB { get; set; }

        public bool SkipDeleteInFolderA { get; set; }

        public Guid ReplicaIdFolderA { get; set; }

        public Guid ReplicaIdFolderB { get; set; }

        public DateTime StartAt { get; set; }

        public bool IsEmpty()
        {
            return string.IsNullOrWhiteSpace(FolderA) || string.IsNullOrWhiteSpace(FolderB) ||
                   Interval <= 0 ||
                   ReplicaIdFolderA == Guid.Empty ||
                   ReplicaIdFolderB == Guid.Empty;
        }

        public static string GetSettingsDir()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "kalpio", "sync");
        }

        internal static string GetDbFilePath()
        {
            return Path.Combine(GetSettingsDir(), "sync.db");
        }

        public static string GetConnectionString()
        {
            var path = GetDbFilePath();

            if (!File.Exists(path)) {
                File.Create(path);
            }

            var model = new SQLiteConnectionStringBuilder
            {
                DataSource = path,
                ForeignKeys = true,
                DateTimeKind = DateTimeKind.Utc
            };

            return model.ConnectionString;
        }
    }

    internal enum IntervalType
    {
        Millisecond = 0,
        Second = 1,
        Minute = 2,
        Hour = 3,
        Day = 4,
        Week = 5,
        Month = 6,
        Year = 7
    }

    internal enum Direction
    {
        Upload = 0,
        Download,
        UploadAndDownload
    }
}
