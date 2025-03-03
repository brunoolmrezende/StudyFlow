using FluentMigrator;

namespace StudyFlow.Infrastructure.DataAccess.Migrations.Versions
{
    [Migration(DatabaseVersion.TABLE_TOPIC, "Create table to save the topics.")]
    public class Version0000003 : VersionBase
    {
        public override void Up()
        {
            CreateTable("Topics")
                .WithColumn("Name").AsString(255).NotNullable()
                .WithColumn("Description").AsString(255).Nullable()
                .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("FK_Topic_User_Id", "Users", "Id")
                .WithColumn("SubjectId").AsInt64().NotNullable().ForeignKey("FK_Topic_Subject_Id", "Subjects", "Id");
        }
    }
}
