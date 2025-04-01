using FluentMigrator;

namespace StudyFlow.Infrastructure.DataAccess.Migrations.Versions
{
    [Migration(DatabaseVersion.TABLE_REFRESH_TOKEN, "Create table to save refresh tokens.")]
    public class Version0000005 : VersionBase
    {
        public override void Up()
        {
            CreateTable("RefreshTokens")
                .WithColumn("Value").AsString().NotNullable()
                .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("FK_RefreshToken_User_Id", "Users", "Id");       
        }
    }
}
