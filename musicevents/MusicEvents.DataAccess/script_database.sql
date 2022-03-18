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

CREATE TABLE [Genres] (
    [Id] int NOT NULL IDENTITY,
    [Description] nvarchar(50) NOT NULL,
    [Status] bit NOT NULL,
    CONSTRAINT [PK_Genres] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220119025422_InitialMigration', N'6.0.2');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Events] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(100) NOT NULL,
    [Description] nvarchar(500) NOT NULL,
    [DateEvent] datetime2 NOT NULL,
    [TicketsQuantity] int NOT NULL,
    [UnitPrice] decimal(8,2) NOT NULL,
    [ImageUrl] nvarchar(max) NULL,
    [GenreId] int NOT NULL,
    [Status] bit NOT NULL,
    CONSTRAINT [PK_Events] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Events_Genres_GenreId] FOREIGN KEY ([GenreId]) REFERENCES [Genres] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Sale] (
    [Id] int NOT NULL IDENTITY,
    [SaleDate] datetime2 NOT NULL,
    [EventId] int NOT NULL,
    [Quantity] int NOT NULL,
    [UnitPrice] decimal(8,2) NOT NULL,
    [TotalSale] decimal(8,2) NOT NULL,
    [UserId] nvarchar(36) NOT NULL,
    [Status] bit NOT NULL,
    CONSTRAINT [PK_Sale] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Sale_Events_EventId] FOREIGN KEY ([EventId]) REFERENCES [Events] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Events_GenreId] ON [Events] ([GenreId]);
GO

CREATE INDEX [IX_Sale_EventId] ON [Sale] ([EventId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220121022359_TablasAdicionales', N'6.0.2');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Events]') AND [c].[name] = N'ImageUrl');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Events] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Events] ALTER COLUMN [ImageUrl] nvarchar(max) NULL;
GO

CREATE INDEX [IX_Sale_UserId] ON [Sale] ([UserId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220121023519_IndexForUserId', N'6.0.2');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [FirstName] nvarchar(100) NOT NULL,
    [LastName] nvarchar(100) NOT NULL,
    [Age] int NOT NULL,
    [TypeDocument] int NOT NULL,
    [DocumentNumber] nvarchar(20) NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
GO

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
GO

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220202015603_TablasIdentity', N'6.0.2');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Sale] ADD [OperationNumber] nvarchar(8) NOT NULL DEFAULT N'';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220209014842_AddOperationNumber', N'6.0.2');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE PROCEDURE uspSelectEventById (@Id INT)
            AS
                BEGIN

            SELECT
            S.Id,
            E.DateEvent,
            G.Description Genre,
                E.ImageUrl,
            E.Title,
            S.OperationNumber,
            U.FirstName + ' ' + U.LastName FullName,
            S.Quantity,
            S.SaleDate,
            S.TotalSale
                FROM Sale S
            INNER JOIN AspNetUsers U ON S.UserId = U.Id
            INNER JOIN Events E ON E.Id = S.EventId
            INNER JOIN Genres G ON G.Id = E.GenreId
            WHERE S.Id = @Id

            END
GO

CREATE PROCEDURE uspSelectEventByUserId(@UserId NVARCHAR(36))
            AS
                BEGIN

            SELECT
            S.Id,
            E.DateEvent,
            G.Description Genre,
                E.ImageUrl,
            E.Title,
            S.OperationNumber,
            U.FirstName + ' ' + U.LastName FullName,
            S.Quantity,
            S.SaleDate,
            S.TotalSale
                FROM Sale S
            INNER JOIN AspNetUsers U ON S.UserId = U.Id
            INNER JOIN Events E ON E.Id = S.EventId
            INNER JOIN Genres G ON G.Id = E.GenreId
            WHERE U.Id = @UserId

            END
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220209020632_SaleProcedures', N'6.0.2');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE PROCEDURE uspReportSales (@EventID int, @DateInit DATE, @DateEnd DATE)
            AS
                BEGIN

            SELECT

            DAY(S.SaleDate) AS Day,
                SUM(S.TotalSale) AS TotalSale
                FROM Sale S(NOLOCK)
            WHERE S.EventId = @EventID
            AND CAST(S.SaleDate AS DATE) BETWEEN @DateInit AND @DateEnd
            GROUP BY DAY(S.SaleDate)

            END
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220211012423_AddReportsSP', N'6.0.2');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE OR ALTER PROCEDURE uspReportSales(@GenreId int, @DateInit DATE, @DateEnd DATE)
AS
BEGIN

    SELECT DAY(S.SaleDate)  AS Day,
           SUM(S.TotalSale) AS TotalSale
    FROM Sale S (NOLOCK)
    INNER JOIN Events E (NOLOCK) on E.Id = S.EventId
    WHERE E.GenreId = @GenreId
      AND CAST(S.SaleDate AS DATE) BETWEEN @DateInit AND @DateEnd
    GROUP BY DAY(S.SaleDate)

END
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220211014608_FixeReportsSP', N'6.0.2');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Sale]') AND [c].[name] = N'OperationNumber');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Sale] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Sale] ALTER COLUMN [OperationNumber] nvarchar(8) NULL;
GO

ALTER TABLE [Events] ADD [Finalized] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'LastName');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [AspNetUsers] ALTER COLUMN [LastName] nvarchar(100) NULL;
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'FirstName');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [AspNetUsers] ALTER COLUMN [FirstName] nvarchar(100) NULL;
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'DocumentNumber');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [AspNetUsers] ALTER COLUMN [DocumentNumber] nvarchar(20) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220211215327_ConcertFinalized', N'6.0.2');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Events] ADD [Place] nvarchar(100) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220308155129_PlaceForEvent', N'6.0.2');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE PROCEDURE uspSelectEventCollection(@GenreID INT, @DateInit DATE = NULL, @DateEnd DATE = NULL)
AS
BEGIN

    SELECT S.Id,
           E.DateEvent,
           G.Description                  Genre,
           E.ImageUrl,
           E.Title,
           S.OperationNumber,
           U.FirstName + ' ' + U.LastName FullName,
           S.Quantity,
           S.SaleDate,
           S.TotalSale
    FROM Sale S
             INNER JOIN AspNetUsers U ON S.UserId = U.Id
             INNER JOIN Events E ON E.Id = S.EventId
             INNER JOIN Genres G ON G.Id = E.GenreId
    WHERE E.GenreId = @GenreID
    AND (@DateInit IS NULL OR (S.SaleDate BETWEEN @DateInit AND @DateEnd))
    OPTION (RECOMPILE)
END
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220310054531_SaleCollectionStore', N'6.0.2');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE OR ALTER PROCEDURE uspSelectEventCollection(@GenreID INT, @DateInit DATE = NULL, @DateEnd DATE = NULL)
AS
BEGIN

    SELECT S.Id,
           E.DateEvent,
           G.Description                  Genre,
           E.ImageUrl,
           E.Title,
           S.OperationNumber,
           U.FirstName + ' ' + U.LastName FullName,
           S.Quantity,
           S.SaleDate,
           S.TotalSale
    FROM Sale S
             INNER JOIN AspNetUsers U ON S.UserId = U.Id
             INNER JOIN Events E ON E.Id = S.EventId
             INNER JOIN Genres G ON G.Id = E.GenreId
    WHERE E.GenreId = @GenreID
    AND (@DateInit IS NULL OR (CAST(S.SaleDate AS DATE) BETWEEN @DateInit AND @DateEnd))
    OPTION (RECOMPILE)
END
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220310060101_SaleFilterDate', N'6.0.2');
GO

COMMIT;
GO

