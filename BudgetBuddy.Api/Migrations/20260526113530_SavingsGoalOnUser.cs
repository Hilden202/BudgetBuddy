using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetBuddy.Api.Migrations
{
    /// <inheritdoc />
    public partial class SavingsGoalOnUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoalAmount",
                table: "Savings");

            migrationBuilder.AddColumn<decimal>(
                name: "SavingsGoal",
                table: "AspNetUsers",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SavingsGoal",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<decimal>(
                name: "GoalAmount",
                table: "Savings",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
