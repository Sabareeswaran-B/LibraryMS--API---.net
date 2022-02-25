using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryMS.Migrations
{
    public partial class InitialMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Author",
                columns: table => new
                {
                    AuthorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorAge = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryLanguage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "true")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Author", x => x.AuthorId);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeRole = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "clerk"),
                    EmployeeAge = table.Column<int>(type: "int", nullable: false),
                    EmployeeSalary = table.Column<int>(type: "int", nullable: false),
                    EmployeeEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeePhoneNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "true")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.EmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "Visitor",
                columns: table => new
                {
                    VisitorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VisitorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VisitorAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VisitorAge = table.Column<int>(type: "int", nullable: false),
                    VisitorEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VisitorPhoneNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsEmployee = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAuthor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "true")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visitor", x => x.VisitorId);
                });

            migrationBuilder.CreateTable(
                name: "Book",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReleasedYear = table.Column<int>(type: "int", nullable: false),
                    Edition = table.Column<int>(type: "int", nullable: false),
                    CopiesAvailable = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "true")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.BookId);
                    table.ForeignKey(
                        name: "FK_Book_Author_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Author",
                        principalColumn: "AuthorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lending",
                columns: table => new
                {
                    LendingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VisitorId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    LendedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Lendinglimit = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<int>(type: "int", nullable: false, defaultValue: "true")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lending", x => x.LendingId);
                    table.ForeignKey(
                        name: "FK_Lending_Book_BookId",
                        column: x => x.BookId,
                        principalTable: "Book",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lending_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lending_Visitor_VisitorId",
                        column: x => x.VisitorId,
                        principalTable: "Visitor",
                        principalColumn: "VisitorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Book_AuthorId",
                table: "Book",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Lending_BookId",
                table: "Lending",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Lending_EmployeeId",
                table: "Lending",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Lending_VisitorId",
                table: "Lending",
                column: "VisitorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lending");

            migrationBuilder.DropTable(
                name: "Book");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Visitor");

            migrationBuilder.DropTable(
                name: "Author");
        }
    }
}
