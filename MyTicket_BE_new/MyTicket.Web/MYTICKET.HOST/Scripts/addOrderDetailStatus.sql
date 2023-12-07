BEGIN TRANSACTION;
GO

ALTER TABLE [dbo].[OrderDetail] ADD [Status] int NOT NULL DEFAULT 0;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231205084216_AddColumnStatusOrderDetail', N'7.0.13');
GO

COMMIT;
GO

