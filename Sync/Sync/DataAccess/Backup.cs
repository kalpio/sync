using FluentNHibernate.Mapping;
using System;

namespace Sync.DataAccess
{
    public enum Folder
    {
        A,
        B
    }

    public class Backup
    {
        public virtual DateTime CreatedAt { get; set; }

        public virtual Folder Folder { get; set; }

        public virtual int Id { get; set; }

        public virtual string RelativePath { get; set; }
    }

    public class BackupMap : ClassMap<Backup>
    {
        public BackupMap()
        {
            Table("backups");

            Id(x => x.Id)
                .Column("id")
                .GeneratedBy.Identity();

            Map(x => x.CreatedAt)
                .Column("created_at")
                .Not.Nullable();

            Map(x => x.Folder)
                .Column("folder")
                .Not.Nullable();

            Map(x => x.RelativePath)
                .Column("relative_path")
                .CustomSqlType("varchar")
                .Length(4096)
                .Not.Nullable();
        }
    }
}
