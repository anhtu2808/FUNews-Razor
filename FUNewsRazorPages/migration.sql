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
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250620044315_InitDatabase'
)
BEGIN
    CREATE TABLE [Category] (
        [CategoryId] smallint NOT NULL IDENTITY,
        [CategoryName] nvarchar(100) NOT NULL,
        [CategoryDescription] nvarchar(250) NOT NULL,
        [ParentCategoryId] smallint NULL,
        [IsActive] bit NULL,
        CONSTRAINT [PK_Category] PRIMARY KEY ([CategoryId]),
        CONSTRAINT [FK_Category_Category_ParentCategoryId] FOREIGN KEY ([ParentCategoryId]) REFERENCES [Category] ([CategoryId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250620044315_InitDatabase'
)
BEGIN
    CREATE TABLE [SystemAccount] (
        [AccountId] smallint NOT NULL IDENTITY,
        [AccountName] nvarchar(100) NULL,
        [AccountEmail] nvarchar(70) NULL,
        [AccountRole] int NULL,
        [AccountPassword] nvarchar(70) NULL,
        CONSTRAINT [PK_SystemAccount] PRIMARY KEY ([AccountId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250620044315_InitDatabase'
)
BEGIN
    CREATE TABLE [Tags] (
        [TagId] int NOT NULL IDENTITY,
        [TagName] nvarchar(50) NULL,
        [Note] nvarchar(400) NULL,
        CONSTRAINT [PK_Tags] PRIMARY KEY ([TagId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250620044315_InitDatabase'
)
BEGIN
    CREATE TABLE [NewsArticle] (
        [NewsArticleId] nvarchar(20) NOT NULL,
        [NewsTitle] nvarchar(400) NULL,
        [Headline] nvarchar(150) NOT NULL,
        [CreatedDate] datetime2 NULL,
        [NewsContent] nvarchar(4000) NULL,
        [NewsSource] nvarchar(400) NULL,
        [UrlThumbnails] nvarchar(500) NULL,
        [CategoryId] smallint NULL,
        [NewsStatus] bit NULL,
        [CreatedById] smallint NULL,
        [UpdatedById] smallint NULL,
        [ModifiedDate] datetime2 NULL,
        [SystemAccountAccountId] smallint NULL,
        [SystemAccountAccountId1] smallint NULL,
        CONSTRAINT [PK_NewsArticle] PRIMARY KEY ([NewsArticleId]),
        CONSTRAINT [FK_NewsArticle_Category_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Category] ([CategoryId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_NewsArticle_SystemAccount_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [SystemAccount] ([AccountId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_NewsArticle_SystemAccount_SystemAccountAccountId] FOREIGN KEY ([SystemAccountAccountId]) REFERENCES [SystemAccount] ([AccountId]),
        CONSTRAINT [FK_NewsArticle_SystemAccount_SystemAccountAccountId1] FOREIGN KEY ([SystemAccountAccountId1]) REFERENCES [SystemAccount] ([AccountId]),
        CONSTRAINT [FK_NewsArticle_SystemAccount_UpdatedById] FOREIGN KEY ([UpdatedById]) REFERENCES [SystemAccount] ([AccountId]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250620044315_InitDatabase'
)
BEGIN
    CREATE TABLE [NewsTag] (
        [NewsArticleId] nvarchar(20) NOT NULL,
        [TagId] int NOT NULL,
        CONSTRAINT [PK_NewsTag] PRIMARY KEY ([NewsArticleId], [TagId]),
        CONSTRAINT [FK_NewsTag_NewsArticle_NewsArticleId] FOREIGN KEY ([NewsArticleId]) REFERENCES [NewsArticle] ([NewsArticleId]) ON DELETE CASCADE,
        CONSTRAINT [FK_NewsTag_Tags_TagId] FOREIGN KEY ([TagId]) REFERENCES [Tags] ([TagId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250620044315_InitDatabase'
)
BEGIN
    CREATE INDEX [IX_Category_ParentCategoryId] ON [Category] ([ParentCategoryId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250620044315_InitDatabase'
)
BEGIN
    CREATE INDEX [IX_NewsArticle_CategoryId] ON [NewsArticle] ([CategoryId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250620044315_InitDatabase'
)
BEGIN
    CREATE INDEX [IX_NewsArticle_CreatedById] ON [NewsArticle] ([CreatedById]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250620044315_InitDatabase'
)
BEGIN
    CREATE INDEX [IX_NewsArticle_SystemAccountAccountId] ON [NewsArticle] ([SystemAccountAccountId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250620044315_InitDatabase'
)
BEGIN
    CREATE INDEX [IX_NewsArticle_SystemAccountAccountId1] ON [NewsArticle] ([SystemAccountAccountId1]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250620044315_InitDatabase'
)
BEGIN
    CREATE INDEX [IX_NewsArticle_UpdatedById] ON [NewsArticle] ([UpdatedById]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250620044315_InitDatabase'
)
BEGIN
    CREATE INDEX [IX_NewsTag_TagId] ON [NewsTag] ([TagId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250620044315_InitDatabase'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250620044315_InitDatabase', N'9.0.6');
END;

COMMIT;
GO

