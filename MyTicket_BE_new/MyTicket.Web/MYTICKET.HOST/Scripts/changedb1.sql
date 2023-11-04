BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[dbo].[Suppiler]') AND [c].[name] = N'Language');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [dbo].[Suppiler] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [dbo].[Suppiler] DROP COLUMN [Language];
GO

CREATE TABLE [dbo].[EventType] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(264) NOT NULL,
    [Description] nvarchar(1024) NULL,
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

CREATE TABLE [dbo].[EventDetail] (
    [Id] int NOT NULL IDENTITY,
    [EventId] int NOT NULL,
    [VenueId] int NOT NULL,
    [OrganizationDay] datetime2 NOT NULL,
    [StartSaleTicketDate] datetime2 NOT NULL,
    [EndSaleTicketDate] datetime2 NOT NULL,
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
    [Quantity] int NOT NULL,
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
    [TicketEventId] int NOT NULL,
    [SeatCode] nvarchar(10) NOT NULL,
    [Status] int NOT NULL DEFAULT 1,
    CONSTRAINT [PK_Ticket] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Ticket_TicketEvent_TicketEventId] FOREIGN KEY ([TicketEventId]) REFERENCES [dbo].[TicketEvent] ([Id]) ON DELETE CASCADE
);
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

CREATE INDEX [IX_Ticket] ON [dbo].[Ticket] ([SeatCode]);
GO

CREATE INDEX [IX_Ticket_TicketEventId] ON [dbo].[Ticket] ([TicketEventId]);
GO

CREATE INDEX [IX_TicketEvent] ON [dbo].[TicketEvent] ([EventDetailId], [Name]);
GO

CREATE INDEX [IX_Venue] ON [dbo].[Venue] ([Deleted], [Name]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231104164253_AddEntityForTicketManager', N'7.0.13');
GO

COMMIT;
GO

