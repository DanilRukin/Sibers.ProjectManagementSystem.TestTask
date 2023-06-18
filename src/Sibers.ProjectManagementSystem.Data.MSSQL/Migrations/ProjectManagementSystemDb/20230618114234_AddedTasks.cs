﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sibers.ProjectManagementSystem.Data.MSSQL.Migrations.ProjectManagementSystemDb
{
    /// <inheritdoc />
    public partial class AddedTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Priority_Value = table.Column<int>(type: "int", nullable: false),
                    TaskStatus = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "ToDo"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    ContractorEmployeeId = table.Column<int>(type: "int", nullable: true),
                    AuthorEmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Employees_AuthorEmployeeId",
                        column: x => x.AuthorEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tasks_Employees_ContractorEmployeeId",
                        column: x => x.ContractorEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tasks_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AuthorEmployeeId",
                table: "Tasks",
                column: "AuthorEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ContractorEmployeeId",
                table: "Tasks",
                column: "ContractorEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ProjectId",
                table: "Tasks",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");
        }
    }
}