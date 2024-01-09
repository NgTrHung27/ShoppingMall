using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAPM.Migrations
{
    /// <inheritdoc />
    public partial class InitPlus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChucNangs",
                columns: table => new
                {
                    MACN = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TENCN = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChucNangs", x => x.MACN);
                });

            migrationBuilder.CreateTable(
                name: "PhanQuyens",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MACN = table.Column<int>(type: "int", nullable: false),
                    MACV = table.Column<int>(type: "int", nullable: false),
                    GHICHU = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhanQuyens", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChucNangs");

            migrationBuilder.DropTable(
                name: "PhanQuyens");
        }
    }
}
