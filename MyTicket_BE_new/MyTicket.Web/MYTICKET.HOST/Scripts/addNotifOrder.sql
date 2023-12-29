BEGIN TRANSACTION;
GO

ALTER TABLE [dbo].[Notification] ADD [OrderDetailId] int NULL;
GO

ALTER TABLE [dbo].[Notification] ADD [OrderId] int NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231228105808_AddOrderInfoNotification', N'7.0.13');
GO

COMMIT;
GO

