BEGIN TRANSACTION;
GO

ALTER TABLE [dbo].[OrderDetail] ADD [ExchangeCancelDate] datetime2 NULL;
GO

ALTER TABLE [dbo].[OrderDetail] ADD [ExchangeDoneDate] datetime2 NULL;
GO

ALTER TABLE [dbo].[OrderDetail] ADD [TransferCancelDate] datetime2 NULL;
GO

ALTER TABLE [dbo].[OrderDetail] ADD [TransferDoneDate] datetime2 NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231224091113_AddColumnDateTransferExchange', N'7.0.13');
GO

COMMIT;
GO

