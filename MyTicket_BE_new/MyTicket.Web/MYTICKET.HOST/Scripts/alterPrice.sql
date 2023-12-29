BEGIN TRANSACTION;
GO

ALTER TABLE [dbo].[OrderDetail] ADD [Price] decimal(18,2) NOT NULL DEFAULT 0.0;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231228183428_AlterPriceOrderDetail', N'7.0.13');
GO

COMMIT;
GO

