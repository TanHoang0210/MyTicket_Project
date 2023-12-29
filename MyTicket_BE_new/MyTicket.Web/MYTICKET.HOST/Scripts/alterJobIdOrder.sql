BEGIN TRANSACTION;
GO

ALTER TABLE [dbo].[Order] ADD [BackgroundJobId] int NOT NULL DEFAULT 0;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231228185856_AltelJobOrder', N'7.0.13');
GO

COMMIT;
GO

