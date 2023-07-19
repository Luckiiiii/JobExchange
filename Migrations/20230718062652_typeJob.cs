using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobExchange.Migrations
{
    /// <inheritdoc />
    public partial class typeJob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobInfos_TypeJob_TypeJobId",
                table: "JobInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TypeJob",
                table: "TypeJob");

            migrationBuilder.RenameTable(
                name: "TypeJob",
                newName: "TypeJobs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TypeJobs",
                table: "TypeJobs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobInfos_TypeJobs_TypeJobId",
                table: "JobInfos",
                column: "TypeJobId",
                principalTable: "TypeJobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobInfos_TypeJobs_TypeJobId",
                table: "JobInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TypeJobs",
                table: "TypeJobs");

            migrationBuilder.RenameTable(
                name: "TypeJobs",
                newName: "TypeJob");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TypeJob",
                table: "TypeJob",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobInfos_TypeJob_TypeJobId",
                table: "JobInfos",
                column: "TypeJobId",
                principalTable: "TypeJob",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
