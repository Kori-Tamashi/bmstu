using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eventor.Migrations
{
    /// <inheritdoc />
    public partial class ImproveRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_feedback",
                table: "feedback");

            migrationBuilder.RenameTable(
                name: "feedback",
                newName: "feedbacks");

            migrationBuilder.RenameColumn(
                name: "day_id",
                table: "items",
                newName: "item_id");

            migrationBuilder.RenameColumn(
                name: "event_id",
                table: "days",
                newName: "menu_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_feedbacks",
                table: "feedbacks",
                column: "feedback_id");

            migrationBuilder.CreateTable(
                name: "events_days",
                columns: table => new
                {
                    event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    day_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_events_days", x => new { x.event_id, x.day_id });
                    table.ForeignKey(
                        name: "FK_events_days_days_day_id",
                        column: x => x.day_id,
                        principalTable: "days",
                        principalColumn: "day_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_events_days_events_event_id",
                        column: x => x.event_id,
                        principalTable: "events",
                        principalColumn: "event_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "menu_items",
                columns: table => new
                {
                    menu_id = table.Column<Guid>(type: "uuid", nullable: false),
                    item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_menu_items", x => new { x.menu_id, x.item_id });
                    table.ForeignKey(
                        name: "FK_menu_items_items_item_id",
                        column: x => x.item_id,
                        principalTable: "items",
                        principalColumn: "item_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_menu_items_menu_menu_id",
                        column: x => x.menu_id,
                        principalTable: "menu",
                        principalColumn: "menu_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "persons_days",
                columns: table => new
                {
                    person_id = table.Column<Guid>(type: "uuid", nullable: false),
                    day_id = table.Column<Guid>(type: "uuid", nullable: false),
                    DayDBModelId = table.Column<Guid>(type: "uuid", nullable: true),
                    PersonDayDBModelDayId = table.Column<Guid>(type: "uuid", nullable: true),
                    PersonDayDBModelPersonId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_persons_days", x => new { x.person_id, x.day_id });
                    table.ForeignKey(
                        name: "FK_persons_days_days_DayDBModelId",
                        column: x => x.DayDBModelId,
                        principalTable: "days",
                        principalColumn: "day_id");
                    table.ForeignKey(
                        name: "FK_persons_days_persons_days_PersonDayDBModelPersonId_PersonDa~",
                        columns: x => new { x.PersonDayDBModelPersonId, x.PersonDayDBModelDayId },
                        principalTable: "persons_days",
                        principalColumns: new[] { "person_id", "day_id" });
                    table.ForeignKey(
                        name: "FK_persons_days_persons_person_id",
                        column: x => x.person_id,
                        principalTable: "persons",
                        principalColumn: "person_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users_events",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users_events", x => new { x.user_id, x.event_id });
                    table.ForeignKey(
                        name: "FK_users_events_events_event_id",
                        column: x => x.event_id,
                        principalTable: "events",
                        principalColumn: "event_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_users_events_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_events_days_day_id",
                table: "events_days",
                column: "day_id");

            migrationBuilder.CreateIndex(
                name: "IX_menu_items_item_id",
                table: "menu_items",
                column: "item_id");

            migrationBuilder.CreateIndex(
                name: "IX_persons_days_DayDBModelId",
                table: "persons_days",
                column: "DayDBModelId");

            migrationBuilder.CreateIndex(
                name: "IX_persons_days_PersonDayDBModelPersonId_PersonDayDBModelDayId",
                table: "persons_days",
                columns: new[] { "PersonDayDBModelPersonId", "PersonDayDBModelDayId" });

            migrationBuilder.CreateIndex(
                name: "IX_users_events_event_id",
                table: "users_events",
                column: "event_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "events_days");

            migrationBuilder.DropTable(
                name: "menu_items");

            migrationBuilder.DropTable(
                name: "persons_days");

            migrationBuilder.DropTable(
                name: "users_events");

            migrationBuilder.DropPrimaryKey(
                name: "PK_feedbacks",
                table: "feedbacks");

            migrationBuilder.RenameTable(
                name: "feedbacks",
                newName: "feedback");

            migrationBuilder.RenameColumn(
                name: "item_id",
                table: "items",
                newName: "day_id");

            migrationBuilder.RenameColumn(
                name: "menu_id",
                table: "days",
                newName: "event_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_feedback",
                table: "feedback",
                column: "feedback_id");
        }
    }
}
