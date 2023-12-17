BEGIN TRANSACTION;
GO

ALTER TABLE [dbo].[Event] ADD [IsOutStanding] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231216071753_AddColumnOuStanding', N'7.0.13');
GO

COMMIT;
GO

