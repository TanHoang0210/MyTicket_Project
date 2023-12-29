BEGIN TRANSACTION;
GO

ALTER TABLE [dbo].[OrderDetail] ADD [ExchangeRefundRequest] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

ALTER TABLE [dbo].[OrderDetail] ADD [TransferRefundRequest] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

ALTER TABLE [dbo].[OrderDetail] ADD [TransferTransactionNo] nvarchar(max) NULL;
GO

ALTER TABLE [dbo].[Order] ADD [RefundRequest] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

ALTER TABLE [dbo].[Order] ADD [TransactionNo] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231228142921_AddTransactionNo', N'7.0.13');
GO

COMMIT;
GO

