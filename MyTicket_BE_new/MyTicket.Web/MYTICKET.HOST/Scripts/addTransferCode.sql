BEGIN TRANSACTION;
GO

ALTER TABLE [dbo].[OrderDetail] ADD [TransferCode] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231129123143_AddColumnTransferCode', N'7.0.13');
GO

COMMIT;
GO

