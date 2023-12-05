BEGIN TRANSACTION;
GO

ALTER TABLE [dbo].[OrderDetail] ADD [ExchangeCode] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231129082657_AddColumnExchangCode', N'7.0.13');
GO

COMMIT;
GO

