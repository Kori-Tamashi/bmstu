using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eventor.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "days",
                columns: table => new
                {
                    day_id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "varchar(255)", nullable: false),
                    sequence_number = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<double>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_days", x => x.day_id);
                });

            migrationBuilder.CreateTable(
                name: "events",
                columns: table => new
                {
                    event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    location_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "varchar(255)", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    person_count = table.Column<int>(type: "integer", nullable: false),
                    days_count = table.Column<int>(type: "integer", nullable: false),
                    percent = table.Column<double>(type: "numeric", nullable: false),
                    rating = table.Column<double>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_events", x => x.event_id);
                });

            migrationBuilder.CreateTable(
                name: "feedbacks",
                columns: table => new
                {
                    feedback_id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    person_id = table.Column<Guid>(type: "uuid", nullable: false),
                    comment = table.Column<string>(type: "varchar(255)", nullable: false),
                    rating = table.Column<double>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_feedbacks", x => x.feedback_id);
                });

            migrationBuilder.CreateTable(
                name: "items",
                columns: table => new
                {
                    day_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "varchar(255)", nullable: false),
                    type = table.Column<int>(type: "item_type", nullable: false),
                    price = table.Column<double>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_items", x => x.day_id);
                });

            migrationBuilder.CreateTable(
                name: "locations",
                columns: table => new
                {
                    location_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "varchar(255)", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<double>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_locations", x => x.location_id);
                });

            migrationBuilder.CreateTable(
                name: "menu",
                columns: table => new
                {
                    menu_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "varchar(255)", nullable: false),
                    cost = table.Column<double>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_menu", x => x.menu_id);
                });

            migrationBuilder.CreateTable(
                name: "persons",
                columns: table => new
                {
                    person_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "varchar(255)", nullable: false),
                    type = table.Column<int>(type: "person_type", nullable: false),
                    paid = table.Column<bool>(type: "bool", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_persons", x => x.person_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "varchar(255)", nullable: false),
                    phone = table.Column<string>(type: "varchar(255)", nullable: false),
                    gender = table.Column<int>(type: "gender", nullable: false),
                    password = table.Column<string>(type: "varchar(255)", nullable: false),
                    role = table.Column<int>(type: "role", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "days");

            migrationBuilder.DropTable(
                name: "events");

            migrationBuilder.DropTable(
                name: "feedback");

            migrationBuilder.DropTable(
                name: "items");

            migrationBuilder.DropTable(
                name: "locations");

            migrationBuilder.DropTable(
                name: "menu");

            migrationBuilder.DropTable(
                name: "persons");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
