using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeSwap.Infrastructure.Migrations.TimeSwapApi
{
    /// <inheritdoc />
    public partial class UserProfileNullableMajorId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Categories_MajorCategoryId",
                table: "UserProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Industries_MajorIndustryId",
                table: "UserProfiles");

            migrationBuilder.AlterColumn<int>(
                name: "MajorIndustryId",
                table: "UserProfiles",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "MajorCategoryId",
                table: "UserProfiles",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Categories_MajorCategoryId",
                table: "UserProfiles",
                column: "MajorCategoryId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Industries_MajorIndustryId",
                table: "UserProfiles",
                column: "MajorIndustryId",
                principalTable: "Industries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Categories_MajorCategoryId",
                table: "UserProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Industries_MajorIndustryId",
                table: "UserProfiles");

            migrationBuilder.AlterColumn<int>(
                name: "MajorIndustryId",
                table: "UserProfiles",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MajorCategoryId",
                table: "UserProfiles",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Categories_MajorCategoryId",
                table: "UserProfiles",
                column: "MajorCategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Industries_MajorIndustryId",
                table: "UserProfiles",
                column: "MajorIndustryId",
                principalTable: "Industries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
