BEGIN TRANSACTION;
GO

ALTER TABLE [dbo].[Order] ADD [TransDate] datetime2 NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231229074818_AltelTransdate', N'7.0.13');
GO

COMMIT;
GO

