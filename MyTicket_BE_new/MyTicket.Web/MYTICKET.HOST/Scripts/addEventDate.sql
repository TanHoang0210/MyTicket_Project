BEGIN TRANSACTION;
GO

ALTER TABLE [dbo].[Event] ADD [StartEventDate] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231107160136_AlterEntityEvent', N'7.0.13');
GO

COMMIT;
GO

