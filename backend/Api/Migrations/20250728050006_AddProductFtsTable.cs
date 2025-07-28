using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class AddProductFtsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE VIRTUAL TABLE ""ProductFTS"" USING fts5(
                    ""ProductId"" UNINDEXED,
                    ""Name"",
                    ""Description"",
                    ""Category"",
                    ""Brand"",
                    ""Sku""
                );
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER ""Product_after_insert"" AFTER INSERT ON ""Products""
                BEGIN
                    INSERT INTO ""ProductFTS""(""ProductId"", ""Name"", ""Description"", ""Category"", ""Brand"", ""Sku"")
                    VALUES (new.""Id"", new.""Name"", new.""Description"", new.""Category"", new.""Brand"", new.""Sku"");
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER ""Product_after_delete"" AFTER DELETE ON ""Products""
                BEGIN
                    -- The FTS5 'delete' command requires passing the old values.
                    INSERT INTO ""ProductFTS""(""ProductFTS"", ""ProductId"", ""Name"", ""Description"", ""Category"", ""Brand"", ""Sku"")
                    VALUES('delete', old.""Id"", old.""Name"", old.""Description"", old.""Category"", old.""Brand"", old.""Sku"");
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER ""Product_after_update"" AFTER UPDATE ON ""Products""
                BEGIN
                    -- First, delete the old entry.
                    INSERT INTO ""ProductFTS""(""ProductFTS"", ""ProductId"", ""Name"", ""Description"", ""Category"", ""Brand"", ""Sku"")
                    VALUES('delete', old.""Id"", old.""Name"", old.""Description"", old.""Category"", old.""Brand"", old.""Sku"");
                    -- Then, insert the new entry.
                    INSERT INTO ""ProductFTS""(""ProductId"", ""Name"", ""Description"", ""Category"", ""Brand"", ""Sku"")
                    VALUES (new.""Id"", new.""Name"", new.""Description"", new.""Category"", new.""Brand"", new.""Sku"");
                END;
            ");

            migrationBuilder.Sql(@"
                INSERT INTO ""ProductFTS""(""ProductId"", ""Name"", ""Description"", ""Category"", ""Brand"", ""Sku"")
                SELECT ""Id"", ""Name"", ""Description"", ""Category"", ""Brand"", ""Sku"" FROM ""Products"";
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP TRIGGER ""Product_after_insert"";");
            migrationBuilder.Sql(@"DROP TRIGGER ""Product_after_delete"";");
            migrationBuilder.Sql(@"DROP TRIGGER ""Product_after_update"";");
            migrationBuilder.Sql(@"DROP TABLE ""ProductFTS"";");
        }
    }
}
