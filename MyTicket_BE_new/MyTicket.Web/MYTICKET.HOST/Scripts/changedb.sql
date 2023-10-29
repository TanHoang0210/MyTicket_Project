BEGIN TRANSACTION;
GO

CREATE TABLE [dbo].[Customer] (
    [Id] int NOT NULL IDENTITY,
    [FullName] nvarchar(256) NOT NULL,
    [ShortName] nvarchar(128) NULL,
    [Phone] varchar(128) NULL,
    [Email] varchar(128) NULL,
    [Address] nvarchar(2024) NOT NULL,
    [TaxCode] nvarchar(18) NOT NULL,
    [Language] nvarchar(18) NOT NULL,
    [CreatedDate] datetime2 NULL DEFAULT (getdate()),
    [CreatedBy] int NULL,
    [ModifiedDate] datetime2 NULL,
    [ModifiedBy] int NULL,
    [DeletedDate] datetime2 NULL,
    [Deleted] bit NOT NULL,
    [DeletedBy] int NULL,
    CONSTRAINT [PK_Customer] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [dbo].[Suppiler] (
    [Id] int NOT NULL IDENTITY,
    [FullName] nvarchar(256) NOT NULL,
    [ShortName] nvarchar(128) NULL,
    [Phone] varchar(128) NULL,
    [Email] varchar(128) NULL,
    [Address] nvarchar(2024) NOT NULL,
    [TaxCode] nvarchar(18) NOT NULL,
    [Language] nvarchar(18) NOT NULL,
    [CreatedDate] datetime2 NULL DEFAULT (getdate()),
    [CreatedBy] int NULL,
    [ModifiedDate] datetime2 NULL,
    [ModifiedBy] int NULL,
    [DeletedDate] datetime2 NULL,
    [Deleted] bit NOT NULL,
    [DeletedBy] int NULL,
    CONSTRAINT [PK_Suppiler] PRIMARY KEY ([Id])
);
GO

CREATE INDEX [IX_Customer] ON [dbo].[Customer] ([Deleted], [FullName], [ShortName]);
GO

CREATE INDEX [IX_Suppiler] ON [dbo].[Suppiler] ([Deleted], [FullName], [ShortName]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231029091216_AddCustomerSuplierEntity', N'7.0.13');
GO

COMMIT;
GO

