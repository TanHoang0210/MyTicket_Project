BEGIN TRANSACTION;
GO

ALTER TABLE [dbo].[OrderDetail] ADD [ExchangeDate] datetime2 NULL;
GO

ALTER TABLE [dbo].[OrderDetail] ADD [TransferDate] datetime2 NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231207162814_AddColumnDateExchangeTransfer', N'7.0.13');
GO

COMMIT;
GO

