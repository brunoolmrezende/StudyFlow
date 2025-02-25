using FluentMigrator;

namespace StudyFlow.Infrastructure.DataAccess.Migrations.Versions
{
    [Migration(DatabaseVersion.TABLE_SUBJECT, "Create table to save the subjects.")]
    public class Version0000002 : VersionBase
    {
        public override void Up()
        {
            CreateTable("Subjects")
                .WithColumn("Name").AsString(255).NotNullable()
                .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("FK_Subjec_User_Id", "Users", "Id");
        }
    }
}
