IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [dbo].[Customer] (
    [Id] int NOT NULL IDENTITY,
    [FirstName] nvarchar(256) NOT NULL,
    [LastName] nvarchar(128) NOT NULL,
    [Country] int NOT NULL,
    [Nationality] int NOT NULL,
    [Address] nvarchar(2024) NULL,
    [Gender] int NOT NULL,
    [DateOfBirth] datetime2 NULL,
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

CREATE TABLE [dbo].[EventType] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(264) NOT NULL,
    [Description] nvarchar(1024) NULL,
    [EventTypeImage] nvarchar(1024) NULL,
    [CreatedDate] datetime2 NULL,
    [CreatedBy] int NULL,
    [ModifiedDate] datetime2 NULL,
    [ModifiedBy] int NULL,
    [DeletedDate] datetime2 NULL,
    [Deleted] bit NOT NULL,
    [DeletedBy] int NULL,
    CONSTRAINT [PK_EventType] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [dbo].[OpenIddictApplications] (
    [Id] nvarchar(450) NOT NULL,
    [ClientId] nvarchar(100) NULL,
    [ClientSecret] nvarchar(max) NULL,
    [ConcurrencyToken] nvarchar(50) NULL,
    [ConsentType] nvarchar(50) NULL,
    [DisplayName] nvarchar(max) NULL,
    [DisplayNames] nvarchar(max) NULL,
    [Permissions] nvarchar(max) NULL,
    [PostLogoutRedirectUris] nvarchar(max) NULL,
    [Properties] nvarchar(max) NULL,
    [RedirectUris] nvarchar(max) NULL,
    [Requirements] nvarchar(max) NULL,
    [Type] nvarchar(50) NULL,
    CONSTRAINT [PK_OpenIddictApplications] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [dbo].[OpenIddictScopes] (
    [Id] nvarchar(450) NOT NULL,
    [ConcurrencyToken] nvarchar(50) NULL,
    [Description] nvarchar(max) NULL,
    [Descriptions] nvarchar(max) NULL,
    [DisplayName] nvarchar(max) NULL,
    [DisplayNames] nvarchar(max) NULL,
    [Name] nvarchar(200) NULL,
    [Properties] nvarchar(max) NULL,
    [Resources] nvarchar(max) NULL,
    CONSTRAINT [PK_OpenIddictScopes] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [dbo].[Role] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(256) NOT NULL,
    [UserType] int NOT NULL,
    [Description] nvarchar(1024) NULL,
    [CreatedDate] datetime2 NULL,
    [CreatedBy] int NULL,
    [ModifiedDate] datetime2 NULL,
    [ModifiedBy] int NULL,
    [DeletedDate] datetime2 NULL,
    [Deleted] bit NOT NULL,
    [DeletedBy] int NULL,
    CONSTRAINT [PK_Role] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [dbo].[Suppiler] (
    [Id] int NOT NULL IDENTITY,
    [FullName] nvarchar(256) NOT NULL,
    [ShortName] nvarchar(128) NULL,
    [Address] nvarchar(2024) NOT NULL,
    [TaxCode] nvarchar(18) NOT NULL,
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

CREATE TABLE [dbo].[User] (
    [Id] int NOT NULL IDENTITY,
    [Username] varchar(128) NOT NULL,
    [Password] varchar(128) NOT NULL,
    [Email] varchar(128) NULL,
    [Phone] varchar(128) NULL,
    [UserType] int NOT NULL,
    [CustomerId] int NULL,
    [SupplierId] int NULL,
    [Status] int NOT NULL DEFAULT 1,
    [CreatedDate] datetime2 NULL DEFAULT (getdate()),
    [CreatedBy] int NULL,
    [ModifiedDate] datetime2 NULL,
    [ModifiedBy] int NULL,
    [DeletedDate] datetime2 NULL,
    [DeletedBy] int NULL,
    [Deleted] bit NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [dbo].[Venue] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(264) NOT NULL,
    [Address] nvarchar(264) NULL,
    [Capacity] int NOT NULL,
    [Description] nvarchar(1024) NULL,
    [Image] nvarchar(max) NULL,
    [CreatedDate] datetime2 NULL,
    [CreatedBy] int NULL,
    [ModifiedDate] datetime2 NULL,
    [ModifiedBy] int NULL,
    [DeletedDate] datetime2 NULL,
    [Deleted] bit NOT NULL,
    [DeletedBy] int NULL,
    CONSTRAINT [PK_Venue] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [dbo].[Order] (
    [Id] int NOT NULL IDENTITY,
    [OrderCode] nvarchar(max) NOT NULL,
    [CustomerId] int NOT NULL,
    [Status] int NOT NULL,
    [OrderDate] datetime2 NOT NULL,
    [Total] decimal(18,2) NOT NULL,
    [CreatedDate] datetime2 NULL DEFAULT (getdate()),
    [CreatedBy] int NULL,
    [ModifiedDate] datetime2 NULL,
    [ModifiedBy] int NULL,
    [DeletedDate] datetime2 NULL,
    [Deleted] bit NOT NULL,
    [DeletedBy] int NULL,
    CONSTRAINT [PK_Order] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Order_Customer_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customer] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [dbo].[OpenIddictAuthorizations] (
    [Id] nvarchar(450) NOT NULL,
    [ApplicationId] nvarchar(450) NULL,
    [ConcurrencyToken] nvarchar(50) NULL,
    [CreationDate] datetime2 NULL,
    [Properties] nvarchar(max) NULL,
    [Scopes] nvarchar(max) NULL,
    [Status] nvarchar(50) NULL,
    [Subject] nvarchar(400) NULL,
    [Type] nvarchar(50) NULL,
    CONSTRAINT [PK_OpenIddictAuthorizations] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_OpenIddictAuthorizations_OpenIddictApplications_ApplicationId] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[OpenIddictApplications] ([Id])
);
GO

CREATE TABLE [dbo].[RolePermission] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] int NOT NULL,
    [PermissionKey] varchar(128) NOT NULL,
    [CreatedDate] datetime2 NULL,
    [CreatedBy] int NULL,
    CONSTRAINT [PK_RolePermission] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RolePermission_Role_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [dbo].[Event] (
    [Id] int NOT NULL IDENTITY,
    [EventName] nvarchar(264) NOT NULL,
    [EventTypeId] int NULL,
    [SupplierId] int NULL,
    [EventDescription] nvarchar(1024) NULL,
    [ExchangePolicy] nvarchar(1024) NULL,
    [AdmissionPolicy] nvarchar(1024) NULL,
    [EventImage] nvarchar(1024) NULL,
    [Status] int NOT NULL DEFAULT 1,
    [IsExChange] bit NOT NULL,
    [StartEventDate] datetime2 NOT NULL,
    [CreatedDate] datetime2 NULL DEFAULT (getdate()),
    [CreatedBy] int NULL,
    [ModifiedDate] datetime2 NULL,
    [ModifiedBy] int NULL,
    [DeletedDate] datetime2 NULL,
    [Deleted] bit NOT NULL,
    [DeletedBy] int NULL,
    CONSTRAINT [PK_Event] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Event_EventType_EventTypeId] FOREIGN KEY ([EventTypeId]) REFERENCES [dbo].[EventType] ([Id]),
    CONSTRAINT [FK_Event_Suppiler_SupplierId] FOREIGN KEY ([SupplierId]) REFERENCES [dbo].[Suppiler] ([Id])
);
GO

CREATE TABLE [dbo].[UserRole] (
    [Id] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [RoleId] int NOT NULL,
    [CreatedDate] datetime2 NULL,
    [CreatedBy] int NULL,
    [ModifiedDate] datetime2 NULL,
    [ModifiedBy] int NULL,
    [DeletedDate] datetime2 NULL,
    [Deleted] bit NOT NULL,
    [DeletedBy] int NULL,
    CONSTRAINT [PK_UserRole] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserRole_Role_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserRole_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [dbo].[OpenIddictTokens] (
    [Id] nvarchar(450) NOT NULL,
    [ApplicationId] nvarchar(450) NULL,
    [AuthorizationId] nvarchar(450) NULL,
    [ConcurrencyToken] nvarchar(50) NULL,
    [CreationDate] datetime2 NULL,
    [ExpirationDate] datetime2 NULL,
    [Payload] nvarchar(max) NULL,
    [Properties] nvarchar(max) NULL,
    [RedemptionDate] datetime2 NULL,
    [ReferenceId] nvarchar(100) NULL,
    [Status] nvarchar(50) NULL,
    [Subject] nvarchar(400) NULL,
    [Type] nvarchar(50) NULL,
    CONSTRAINT [PK_OpenIddictTokens] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_OpenIddictTokens_OpenIddictApplications_ApplicationId] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[OpenIddictApplications] ([Id]),
    CONSTRAINT [FK_OpenIddictTokens_OpenIddictAuthorizations_AuthorizationId] FOREIGN KEY ([AuthorizationId]) REFERENCES [dbo].[OpenIddictAuthorizations] ([Id])
);
GO

CREATE TABLE [dbo].[EventDetail] (
    [Id] int NOT NULL IDENTITY,
    [EventId] int NOT NULL,
    [VenueId] int NOT NULL,
    [OrganizationDay] datetime2 NOT NULL,
    [StartSaleTicketDate] datetime2 NOT NULL,
    [EndSaleTicketDate] datetime2 NOT NULL,
    [SeatSelectType] int NOT NULL,
    [HavingSeatMap] bit NOT NULL,
    [EventSeatMapImage] nvarchar(1024) NULL,
    [Status] int NOT NULL DEFAULT 1,
    [CreatedDate] datetime2 NULL DEFAULT (getdate()),
    [CreatedBy] int NULL,
    [ModifiedDate] datetime2 NULL,
    [ModifiedBy] int NULL,
    [DeletedDate] datetime2 NULL,
    [Deleted] bit NOT NULL,
    [DeletedBy] int NULL,
    CONSTRAINT [PK_EventDetail] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_EventDetail_Event_EventId] FOREIGN KEY ([EventId]) REFERENCES [dbo].[Event] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [dbo].[TicketEvent] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(264) NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [EventDetailId] int NOT NULL,
    [Status] int NOT NULL DEFAULT 1,
    [CreatedDate] datetime2 NULL DEFAULT (getdate()),
    [CreatedBy] int NULL,
    [ModifiedDate] datetime2 NULL,
    [ModifiedBy] int NULL,
    [DeletedDate] datetime2 NULL,
    [Deleted] bit NOT NULL,
    [DeletedBy] int NULL,
    CONSTRAINT [PK_TicketEvent] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TicketEvent_EventDetail_EventDetailId] FOREIGN KEY ([EventDetailId]) REFERENCES [dbo].[EventDetail] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [dbo].[Ticket] (
    [Id] int NOT NULL IDENTITY,
    [TicketCode] nvarchar(max) NULL,
    [TicketEventId] int NOT NULL,
    [SeatCode] nvarchar(10) NOT NULL,
    [Status] int NOT NULL,
    [CustomerId] int NULL,
    CONSTRAINT [PK_Ticket] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Ticket_TicketEvent_TicketEventId] FOREIGN KEY ([TicketEventId]) REFERENCES [dbo].[TicketEvent] ([Id])
);
GO

CREATE TABLE [dbo].[OrderDetail] (
    [Id] int NOT NULL IDENTITY,
    [OrderId] int NOT NULL,
    [EventDetailId] int NOT NULL,
    [TicketId] int NOT NULL,
    [IsTransfer] int NULL,
    [TransferStatus] int NULL,
    [CustomerTransfer] int NULL,
    [IsExchange] int NULL,
    [ExchangeStatus] int NULL,
    [QrCode] nvarchar(max) NULL,
    [CreatedDate] datetime2 NULL DEFAULT (getdate()),
    [CreatedBy] int NULL,
    [ModifiedDate] datetime2 NULL,
    [ModifiedBy] int NULL,
    [DeletedDate] datetime2 NULL,
    [Deleted] bit NOT NULL,
    [DeletedBy] int NULL,
    CONSTRAINT [PK_OrderDetail] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_OrderDetail_EventDetail_EventDetailId] FOREIGN KEY ([EventDetailId]) REFERENCES [dbo].[EventDetail] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_OrderDetail_Order_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Order] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_OrderDetail_Ticket_TicketId] FOREIGN KEY ([TicketId]) REFERENCES [dbo].[Ticket] ([Id]) ON DELETE CASCADE
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedBy', N'CustomerId', N'Deleted', N'DeletedBy', N'DeletedDate', N'Email', N'ModifiedBy', N'ModifiedDate', N'Password', N'Phone', N'Status', N'SupplierId', N'UserType', N'Username') AND [object_id] = OBJECT_ID(N'[dbo].[User]'))
    SET IDENTITY_INSERT [dbo].[User] ON;
INSERT INTO [dbo].[User] ([Id], [CreatedBy], [CustomerId], [Deleted], [DeletedBy], [DeletedDate], [Email], [ModifiedBy], [ModifiedDate], [Password], [Phone], [Status], [SupplierId], [UserType], [Username])
VALUES (1, NULL, NULL, CAST(0 AS bit), NULL, NULL, NULL, NULL, NULL, '46F94C8DE14FB36680850768FF1B7F2A', NULL, 1, NULL, 1, 'admin');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedBy', N'CustomerId', N'Deleted', N'DeletedBy', N'DeletedDate', N'Email', N'ModifiedBy', N'ModifiedDate', N'Password', N'Phone', N'Status', N'SupplierId', N'UserType', N'Username') AND [object_id] = OBJECT_ID(N'[dbo].[User]'))
    SET IDENTITY_INSERT [dbo].[User] OFF;
GO

CREATE INDEX [IX_Customer] ON [dbo].[Customer] ([Deleted], [LastName]);
GO

CREATE INDEX [IX_Event] ON [dbo].[Event] ([EventName], [EventTypeId]);
GO

CREATE INDEX [IX_Event_EventTypeId] ON [dbo].[Event] ([EventTypeId]);
GO

CREATE INDEX [IX_Event_SupplierId] ON [dbo].[Event] ([SupplierId]);
GO

CREATE INDEX [IX_EventDetail] ON [dbo].[EventDetail] ([EventId], [OrganizationDay]);
GO

CREATE INDEX [IX_EventType] ON [dbo].[EventType] ([Name]);
GO

CREATE UNIQUE INDEX [IX_OpenIddictApplications_ClientId] ON [dbo].[OpenIddictApplications] ([ClientId]) WHERE [ClientId] IS NOT NULL;
GO

CREATE INDEX [IX_OpenIddictAuthorizations_ApplicationId_Status_Subject_Type] ON [dbo].[OpenIddictAuthorizations] ([ApplicationId], [Status], [Subject], [Type]);
GO

CREATE UNIQUE INDEX [IX_OpenIddictScopes_Name] ON [dbo].[OpenIddictScopes] ([Name]) WHERE [Name] IS NOT NULL;
GO

CREATE INDEX [IX_OpenIddictTokens_ApplicationId_Status_Subject_Type] ON [dbo].[OpenIddictTokens] ([ApplicationId], [Status], [Subject], [Type]);
GO

CREATE INDEX [IX_OpenIddictTokens_AuthorizationId] ON [dbo].[OpenIddictTokens] ([AuthorizationId]);
GO

CREATE UNIQUE INDEX [IX_OpenIddictTokens_ReferenceId] ON [dbo].[OpenIddictTokens] ([ReferenceId]) WHERE [ReferenceId] IS NOT NULL;
GO

CREATE INDEX [IX_Order] ON [dbo].[Order] ([CustomerId]);
GO

CREATE INDEX [IX_OrderDetail] ON [dbo].[OrderDetail] ([EventDetailId], [TicketId]);
GO

CREATE INDEX [IX_OrderDetail_OrderId] ON [dbo].[OrderDetail] ([OrderId]);
GO

CREATE UNIQUE INDEX [IX_OrderDetail_TicketId] ON [dbo].[OrderDetail] ([TicketId]);
GO

CREATE INDEX [IX_Role] ON [dbo].[Role] ([Deleted], [Name]);
GO

CREATE INDEX [IX_RolePermission] ON [dbo].[RolePermission] ([RoleId], [PermissionKey]);
GO

CREATE INDEX [IX_Suppiler] ON [dbo].[Suppiler] ([Deleted], [FullName], [ShortName]);
GO

CREATE INDEX [IX_Ticket] ON [dbo].[Ticket] ([SeatCode]);
GO

CREATE INDEX [IX_Ticket_TicketEventId] ON [dbo].[Ticket] ([TicketEventId]);
GO

CREATE INDEX [IX_TicketEvent] ON [dbo].[TicketEvent] ([EventDetailId], [Name]);
GO

CREATE INDEX [IX_User] ON [dbo].[User] ([Deleted], [Username]);
GO

CREATE INDEX [IX_UserRole] ON [dbo].[UserRole] ([Deleted], [UserId], [RoleId]);
GO

CREATE INDEX [IX_UserRole_RoleId] ON [dbo].[UserRole] ([RoleId]);
GO

CREATE INDEX [IX_UserRole_UserId] ON [dbo].[UserRole] ([UserId]);
GO

CREATE INDEX [IX_Venue] ON [dbo].[Venue] ([Deleted], [Name]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231122075247_FirstMigration', N'7.0.13');
GO

COMMIT;
GO

