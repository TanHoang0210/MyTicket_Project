BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[dbo].[Customer]') AND [c].[name] = N'Nationality');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [dbo].[Customer] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [dbo].[Customer] ALTER COLUMN [Nationality] nvarchar(512) NULL;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[dbo].[Customer]') AND [c].[name] = N'Country');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [dbo].[Customer] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [dbo].[Customer] ALTER COLUMN [Country] nvarchar(512) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231216181211_AlterIntToStringContry', N'7.0.13');
GO

COMMIT;
GO

