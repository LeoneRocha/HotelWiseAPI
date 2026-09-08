using HotelWise.Data.Context;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelWise.Data.Migrations.MySql;

/// <inheritdoc />
[DbContext(typeof(HotelWiseDbContextMysql))]
[Migration("20260907225500_ChatSessionHistoryUtf8mb4")]
public partial class ChatSessionHistoryUtf8mb4 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Necessário para persistir respostas Gemini com emoji (4-byte UTF-8).
        migrationBuilder.Sql("""
            ALTER TABLE `ChatSessionHistory`
              CONVERT TO CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
            """);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            ALTER TABLE `ChatSessionHistory`
              CONVERT TO CHARACTER SET latin1 COLLATE latin1_swedish_ci;
            """);
    }
}
