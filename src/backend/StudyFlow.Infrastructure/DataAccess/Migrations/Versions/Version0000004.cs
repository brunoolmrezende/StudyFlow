using FluentMigrator;

namespace StudyFlow.Infrastructure.DataAccess.Migrations.Versions
{
    [Migration(DatabaseVersion.TABLE_REVIEW, "Create table to save the reviews.")]
    public class Version0000004 : VersionBase
    {
        public override void Up()
        {
            CreateTable("Reviews")
                .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("FK_Review_User_Id", "Users", "Id")
                .WithColumn("TopicId").AsInt64().NotNullable().ForeignKey("FK_Review_Topic_Id", "Topics", "Id")
                .WithColumn("ScheduledDate").AsDateTime().NotNullable()
                .WithColumn("Difficulty").AsInt32().NotNullable()
                .WithColumn("Status").AsInt32().NotNullable();
        }
    }
}
