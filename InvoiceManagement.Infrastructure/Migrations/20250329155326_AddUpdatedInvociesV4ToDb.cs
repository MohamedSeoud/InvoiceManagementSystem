using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUpdatedInvociesV4ToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TotalAmount",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldComputedColumnSql: "0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TotalAmount",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: false,
                computedColumnSql: "0",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldDefaultValue: 0m);
        }
    }
}
