using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Ankieter.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "QuestionnaireSqls",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestionnaireSqls_ApplicationUserId",
                table: "QuestionnaireSqls",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionnaireSqls_AspNetUsers_ApplicationUserId",
                table: "QuestionnaireSqls",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionnaireSqls_AspNetUsers_ApplicationUserId",
                table: "QuestionnaireSqls");

            migrationBuilder.DropIndex(
                name: "IX_QuestionnaireSqls_ApplicationUserId",
                table: "QuestionnaireSqls");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "QuestionnaireSqls");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");
        }
    }
}
