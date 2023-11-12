BEGIN TRANSACTION;
GO

DROP INDEX [IX_Customer] ON [dbo].[Customer];
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[dbo].[User]') AND [c].[name] = N'FullName');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [dbo].[User] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [dbo].[User] DROP COLUMN [FullName];
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[dbo].[Suppiler]') AND [c].[name] = N'Email');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [dbo].[Suppiler] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [dbo].[Suppiler] DROP COLUMN [Email];
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[dbo].[Suppiler]') AND [c].[name] = N'Phone');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [dbo].[Suppiler] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [dbo].[Suppiler] DROP COLUMN [Phone];
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[dbo].[Customer]') AND [c].[name] = N'Email');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [dbo].[Customer] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [dbo].[Customer] DROP COLUMN [Email];
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[dbo].[Customer]') AND [c].[name] = N'Language');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [dbo].[Customer] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [dbo].[Customer] DROP COLUMN [Language];
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[dbo].[Customer]') AND [c].[name] = N'Phone');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [dbo].[Customer] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [dbo].[Customer] DROP COLUMN [Phone];
GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[dbo].[Customer]') AND [c].[name] = N'ShortName');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [dbo].[Customer] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [dbo].[Customer] DROP COLUMN [ShortName];
GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[dbo].[Customer]') AND [c].[name] = N'TaxCode');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [dbo].[Customer] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [dbo].[Customer] DROP COLUMN [TaxCode];
GO

EXEC sp_rename N'[dbo].[Customer].[FullName]', N'FirstName', N'COLUMN';
GO

ALTER TABLE [dbo].[Customer] ADD [Country] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [dbo].[Customer] ADD [DateOfBirth] datetime2 NULL;
GO

ALTER TABLE [dbo].[Customer] ADD [Gender] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [dbo].[Customer] ADD [LastName] nvarchar(128) NOT NULL DEFAULT N'';
GO

ALTER TABLE [dbo].[Customer] ADD [Nationality] int NOT NULL DEFAULT 0;
GO

CREATE INDEX [IX_Customer] ON [dbo].[Customer] ([Deleted], [LastName]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231112055348_AlterEntityCustomer', N'7.0.13');
GO

COMMIT;
GO

