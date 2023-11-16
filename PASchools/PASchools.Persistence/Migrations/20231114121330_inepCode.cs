using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PASchools.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class inepCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InepCode",
                table: "Schools",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InepCode",
                table: "Schools");
        }
    }
}
