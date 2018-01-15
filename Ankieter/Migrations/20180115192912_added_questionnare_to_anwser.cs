using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Ankieter.Migrations
{
    public partial class added_questionnare_to_anwser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestionnareId",
                table: "AnswersSql",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnswersSql_QuestionnareId",
                table: "AnswersSql",
                column: "QuestionnareId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswersSql_QuestionnaireSqls_QuestionnareId",
                table: "AnswersSql",
                column: "QuestionnareId",
                principalTable: "QuestionnaireSqls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswersSql_QuestionnaireSqls_QuestionnareId",
                table: "AnswersSql");

            migrationBuilder.DropIndex(
                name: "IX_AnswersSql_QuestionnareId",
                table: "AnswersSql");

            migrationBuilder.DropColumn(
                name: "QuestionnareId",
                table: "AnswersSql");
        }
    }
}
