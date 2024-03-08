using Microsoft.EntityFrameworkCore.Migrations;

namespace YourNamespace.Migrations
{
    public partial class migrasi100 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Tambahkan kolom Description ke tabel Activities
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Activities",
                nullable: true);

            // Tambahkan kolom RobotId ke tabel Activities
            migrationBuilder.AddColumn<int>(
                name: "RobotId",
                table: "Activities",
                nullable: false,
                defaultValue: 0);

            // Tambahkan index untuk kolom RobotId
            migrationBuilder.CreateIndex(
                name: "IX_Activities_RobotId",
                table: "Activities",
                column: "RobotId");

            // Buat tabel baru ef_temp_Activities
            migrationBuilder.CreateTable(
                name: "ef_temp_Activities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Activitiy = table.Column<string>(nullable: true),
                    Date = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    RobotId = table.Column<int>(nullable: false),
                    Robotname = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    // Tambahkan constraint kunci asing untuk RobotId
                    table.ForeignKey(
                        name: "FK_Activities_Robots_RobotId",
                        column: x => x.RobotId,
                        principalTable: "Robots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Tambahkan data dari tabel Activities ke tabel ef_temp_Activities
            migrationBuilder.Sql("INSERT INTO ef_temp_Activities (Id, Activitiy, Date, Description, RobotId, Robotname) SELECT Id, Activitiy, Date, Description, RobotId, Robotname FROM Activities");

            // Tambahkan constraint kunci asing untuk RobotId di tabel ef_temp_Activities
            migrationBuilder.Sql("PRAGMA foreign_keys = 0");
            migrationBuilder.Sql("UPDATE ef_temp_Activities SET RobotId = 0 WHERE RobotId IS NULL");
            migrationBuilder.Sql("PRAGMA foreign_keys = 1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Hapus tabel ef_temp_Activities jika rollback migrasi
            migrationBuilder.DropTable(
                name: "ef_temp_Activities");

            // Hapus kolom Description dari tabel Activities
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Activities");

            // Hapus kolom RobotId dari tabel Activities
            migrationBuilder.DropColumn(
                name: "RobotId",
                table: "Activities");

            // Hapus index untuk kolom RobotId
            migrationBuilder.DropIndex(
                name: "IX_Activities_RobotId",
                table: "Activities");
        }
    }
}
