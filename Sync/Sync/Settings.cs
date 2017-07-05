using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sync
{
    class Settings
    {
        public Settings()
        {
            IntervalType = IntervalType.Minutes;
            Direction = Direction.UploadAndDownload;
            ReplicaIdFolderA = Guid.Empty;
            ReplicaIdFolderB = Guid.Empty;
            Interval = 10;
        }

        public string FolderA { get; set; }

        public string FolderB { get; set; }

        public int Interval { get; set; }

        public IntervalType IntervalType { get; set; }

        public Direction Direction { get; set; }

        public Guid ReplicaIdFolderA { get; set; }

        public Guid ReplicaIdFolderB { get; set; }

        public bool IsEmpty()
        {
            return (string.IsNullOrWhiteSpace(FolderA) || string.IsNullOrWhiteSpace(FolderB)) ||
                Interval <= 0 ||
                ReplicaIdFolderA == Guid.Empty ||
                ReplicaIdFolderB == Guid.Empty;
        }
    }

    enum IntervalType
    {
        Minutes = 0,
        Hourly,
        Daily,
        Weekly,
        Monthly
    }

    enum Direction
    {
        Upload = 0,
        Download,
        UploadAndDownload
    }
}
