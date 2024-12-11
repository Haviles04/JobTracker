using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobTracker.Migrations
{
    /// <inheritdoc />
    public partial class updateTools : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tool_Jobs_JobId",
                table: "Tool");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tool",
                table: "Tool");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Tool");

            migrationBuilder.RenameTable(
                name: "Tool",
                newName: "Tools");

            migrationBuilder.RenameIndex(
                name: "IX_Tool_JobId",
                table: "Tools",
                newName: "IX_Tools_JobId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NeedsRepair",
                table: "Tools",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tools",
                table: "Tools",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tools_Jobs_JobId",
                table: "Tools",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tools_Jobs_JobId",
                table: "Tools");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tools",
                table: "Tools");

            migrationBuilder.DropColumn(
                name: "NeedsRepair",
                table: "Tools");

            migrationBuilder.RenameTable(
                name: "Tools",
                newName: "Tool");

            migrationBuilder.RenameIndex(
                name: "IX_Tools_JobId",
                table: "Tool",
                newName: "IX_Tool_JobId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Tool",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tool",
                table: "Tool",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tool_Jobs_JobId",
                table: "Tool",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id");
        }
    }
}
