BEGIN TRANSACTION;
GO

CREATE TABLE [dbo].[Notification] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [IsSeen] bit NOT NULL,
    [CreateDate] datetime2 NOT NULL,
    [CustomerId] int NULL,
    [EventDetailId] int NULL,
    CONSTRAINT [PK_Notification] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231228094142_AddEntityNotification', N'7.0.13');
GO

COMMIT;
GO

