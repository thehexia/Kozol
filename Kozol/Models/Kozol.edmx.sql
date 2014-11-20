
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/19/2014 21:29:19
-- Generated from EDMX file: C:\Users\Evan\My Programming\Kozol\Kozol\Models\Kozol.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Kozol];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Destination]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Messages] DROP CONSTRAINT [FK_Destination];
GO
IF OBJECT_ID(N'[dbo].[FK_Sender]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Messages] DROP CONSTRAINT [FK_Sender];
GO
IF OBJECT_ID(N'[dbo].[FK_Administrators_Channel]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Administrators] DROP CONSTRAINT [FK_Administrators_Channel];
GO
IF OBJECT_ID(N'[dbo].[FK_Administrators_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Administrators] DROP CONSTRAINT [FK_Administrators_User];
GO
IF OBJECT_ID(N'[dbo].[FK_Creator]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Channels] DROP CONSTRAINT [FK_Creator];
GO
IF OBJECT_ID(N'[dbo].[FK_Speakers_Channel]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Speakers] DROP CONSTRAINT [FK_Speakers_Channel];
GO
IF OBJECT_ID(N'[dbo].[FK_Speakers_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Speakers] DROP CONSTRAINT [FK_Speakers_User];
GO
IF OBJECT_ID(N'[dbo].[FK_UserFriendship]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Friendships] DROP CONSTRAINT [FK_UserFriendship];
GO
IF OBJECT_ID(N'[dbo].[FK_UserFriendship1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Friendships] DROP CONSTRAINT [FK_UserFriendship1];
GO
IF OBJECT_ID(N'[dbo].[FK_InvitesSent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Invites] DROP CONSTRAINT [FK_InvitesSent];
GO
IF OBJECT_ID(N'[dbo].[FK_InvitesReceived]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Invites] DROP CONSTRAINT [FK_InvitesReceived];
GO
IF OBJECT_ID(N'[dbo].[FK_Invitations]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Invites] DROP CONSTRAINT [FK_Invitations];
GO
IF OBJECT_ID(N'[dbo].[FK_Attachment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Images] DROP CONSTRAINT [FK_Attachment];
GO
IF OBJECT_ID(N'[dbo].[FK_UserRoleMapUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserRoleMaps] DROP CONSTRAINT [FK_UserRoleMapUser];
GO
IF OBJECT_ID(N'[dbo].[FK_UserRoleMapRole]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserRoleMaps] DROP CONSTRAINT [FK_UserRoleMapRole];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Messages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Messages];
GO
IF OBJECT_ID(N'[dbo].[Channels]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Channels];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[Friendships]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Friendships];
GO
IF OBJECT_ID(N'[dbo].[Invites]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Invites];
GO
IF OBJECT_ID(N'[dbo].[Images]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Images];
GO
IF OBJECT_ID(N'[dbo].[UserRoleMaps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserRoleMaps];
GO
IF OBJECT_ID(N'[dbo].[UserRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserRoles];
GO
IF OBJECT_ID(N'[dbo].[Administrators]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Administrators];
GO
IF OBJECT_ID(N'[dbo].[Speakers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Speakers];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Messages'
CREATE TABLE [dbo].[Messages] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Timestamp] datetime  NOT NULL,
    [Text] nvarchar(max)  NOT NULL,
    [Destination_ID] int  NOT NULL,
    [Sender_ID] int  NOT NULL
);
GO

-- Creating table 'Channels'
CREATE TABLE [dbo].[Channels] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(20)  NOT NULL,
    [Created] datetime  NOT NULL,
    [Capacity] int  NOT NULL,
    [Mode_Slow] bit  NOT NULL,
    [Mode_Admin] bit  NOT NULL,
    [Mode_Quiet] bit  NOT NULL,
    [Mode_Invite] bit  NOT NULL,
    [Creator_ID] int  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Email] nvarchar(50)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [LastActivity] datetime  NULL,
    [LastLogin] datetime  NULL,
    [Created] datetime  NULL,
    [Salt] nvarchar(max)  NOT NULL,
    [NameFirst] nvarchar(max)  NULL,
    [NameLast] nvarchar(max)  NULL,
    [Username] nvarchar(20)  NOT NULL,
    [Avatar] uniqueidentifier  NULL,
    [Avatar_Custom] bit  NOT NULL,
    [Public_Key_n] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Friendships'
CREATE TABLE [dbo].[Friendships] (
    [Accepted] datetime  NULL,
    [SenderID] int  NOT NULL,
    [ReceiverID] int  NOT NULL
);
GO

-- Creating table 'Invites'
CREATE TABLE [dbo].[Invites] (
    [Shared_Key] nvarchar(max)  NULL,
    [SenderID] int  NOT NULL,
    [ReceiverID] int  NOT NULL,
    [ChannelID] int  NOT NULL
);
GO

-- Creating table 'Images'
CREATE TABLE [dbo].[Images] (
    [ID] uniqueidentifier  NOT NULL,
    [Message_ID] int  NOT NULL
);
GO

-- Creating table 'UserRoleMaps'
CREATE TABLE [dbo].[UserRoleMaps] (
    [UserID] int  NOT NULL,
    [RoleID] int  NOT NULL
);
GO

-- Creating table 'UserRoles'
CREATE TABLE [dbo].[UserRoles] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Administrators'
CREATE TABLE [dbo].[Administrators] (
    [Administrations_ID] int  NOT NULL,
    [Administrators_ID] int  NOT NULL
);
GO

-- Creating table 'Speakers'
CREATE TABLE [dbo].[Speakers] (
    [Speaches_ID] int  NOT NULL,
    [Speakers_ID] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'Messages'
ALTER TABLE [dbo].[Messages]
ADD CONSTRAINT [PK_Messages]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Channels'
ALTER TABLE [dbo].[Channels]
ADD CONSTRAINT [PK_Channels]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [SenderID], [ReceiverID] in table 'Friendships'
ALTER TABLE [dbo].[Friendships]
ADD CONSTRAINT [PK_Friendships]
    PRIMARY KEY CLUSTERED ([SenderID], [ReceiverID] ASC);
GO

-- Creating primary key on [SenderID], [ReceiverID], [ChannelID] in table 'Invites'
ALTER TABLE [dbo].[Invites]
ADD CONSTRAINT [PK_Invites]
    PRIMARY KEY CLUSTERED ([SenderID], [ReceiverID], [ChannelID] ASC);
GO

-- Creating primary key on [ID] in table 'Images'
ALTER TABLE [dbo].[Images]
ADD CONSTRAINT [PK_Images]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [UserID], [RoleID] in table 'UserRoleMaps'
ALTER TABLE [dbo].[UserRoleMaps]
ADD CONSTRAINT [PK_UserRoleMaps]
    PRIMARY KEY CLUSTERED ([UserID], [RoleID] ASC);
GO

-- Creating primary key on [ID] in table 'UserRoles'
ALTER TABLE [dbo].[UserRoles]
ADD CONSTRAINT [PK_UserRoles]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [Administrations_ID], [Administrators_ID] in table 'Administrators'
ALTER TABLE [dbo].[Administrators]
ADD CONSTRAINT [PK_Administrators]
    PRIMARY KEY CLUSTERED ([Administrations_ID], [Administrators_ID] ASC);
GO

-- Creating primary key on [Speaches_ID], [Speakers_ID] in table 'Speakers'
ALTER TABLE [dbo].[Speakers]
ADD CONSTRAINT [PK_Speakers]
    PRIMARY KEY CLUSTERED ([Speaches_ID], [Speakers_ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Destination_ID] in table 'Messages'
ALTER TABLE [dbo].[Messages]
ADD CONSTRAINT [FK_Destination]
    FOREIGN KEY ([Destination_ID])
    REFERENCES [dbo].[Channels]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Destination'
CREATE INDEX [IX_FK_Destination]
ON [dbo].[Messages]
    ([Destination_ID]);
GO

-- Creating foreign key on [Sender_ID] in table 'Messages'
ALTER TABLE [dbo].[Messages]
ADD CONSTRAINT [FK_Sender]
    FOREIGN KEY ([Sender_ID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Sender'
CREATE INDEX [IX_FK_Sender]
ON [dbo].[Messages]
    ([Sender_ID]);
GO

-- Creating foreign key on [Administrations_ID] in table 'Administrators'
ALTER TABLE [dbo].[Administrators]
ADD CONSTRAINT [FK_Administrators_Channel]
    FOREIGN KEY ([Administrations_ID])
    REFERENCES [dbo].[Channels]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Administrators_ID] in table 'Administrators'
ALTER TABLE [dbo].[Administrators]
ADD CONSTRAINT [FK_Administrators_User]
    FOREIGN KEY ([Administrators_ID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Administrators_User'
CREATE INDEX [IX_FK_Administrators_User]
ON [dbo].[Administrators]
    ([Administrators_ID]);
GO

-- Creating foreign key on [Creator_ID] in table 'Channels'
ALTER TABLE [dbo].[Channels]
ADD CONSTRAINT [FK_Creator]
    FOREIGN KEY ([Creator_ID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Creator'
CREATE INDEX [IX_FK_Creator]
ON [dbo].[Channels]
    ([Creator_ID]);
GO

-- Creating foreign key on [Speaches_ID] in table 'Speakers'
ALTER TABLE [dbo].[Speakers]
ADD CONSTRAINT [FK_Speakers_Channel]
    FOREIGN KEY ([Speaches_ID])
    REFERENCES [dbo].[Channels]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Speakers_ID] in table 'Speakers'
ALTER TABLE [dbo].[Speakers]
ADD CONSTRAINT [FK_Speakers_User]
    FOREIGN KEY ([Speakers_ID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Speakers_User'
CREATE INDEX [IX_FK_Speakers_User]
ON [dbo].[Speakers]
    ([Speakers_ID]);
GO

-- Creating foreign key on [SenderID] in table 'Friendships'
ALTER TABLE [dbo].[Friendships]
ADD CONSTRAINT [FK_UserFriendship]
    FOREIGN KEY ([SenderID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ReceiverID] in table 'Friendships'
ALTER TABLE [dbo].[Friendships]
ADD CONSTRAINT [FK_UserFriendship1]
    FOREIGN KEY ([ReceiverID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserFriendship1'
CREATE INDEX [IX_FK_UserFriendship1]
ON [dbo].[Friendships]
    ([ReceiverID]);
GO

-- Creating foreign key on [SenderID] in table 'Invites'
ALTER TABLE [dbo].[Invites]
ADD CONSTRAINT [FK_InvitesSent]
    FOREIGN KEY ([SenderID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ReceiverID] in table 'Invites'
ALTER TABLE [dbo].[Invites]
ADD CONSTRAINT [FK_InvitesReceived]
    FOREIGN KEY ([ReceiverID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvitesReceived'
CREATE INDEX [IX_FK_InvitesReceived]
ON [dbo].[Invites]
    ([ReceiverID]);
GO

-- Creating foreign key on [ChannelID] in table 'Invites'
ALTER TABLE [dbo].[Invites]
ADD CONSTRAINT [FK_Invitations]
    FOREIGN KEY ([ChannelID])
    REFERENCES [dbo].[Channels]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Invitations'
CREATE INDEX [IX_FK_Invitations]
ON [dbo].[Invites]
    ([ChannelID]);
GO

-- Creating foreign key on [Message_ID] in table 'Images'
ALTER TABLE [dbo].[Images]
ADD CONSTRAINT [FK_Attachment]
    FOREIGN KEY ([Message_ID])
    REFERENCES [dbo].[Messages]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Attachment'
CREATE INDEX [IX_FK_Attachment]
ON [dbo].[Images]
    ([Message_ID]);
GO

-- Creating foreign key on [UserID] in table 'UserRoleMaps'
ALTER TABLE [dbo].[UserRoleMaps]
ADD CONSTRAINT [FK_UserRoleMapUser]
    FOREIGN KEY ([UserID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [RoleID] in table 'UserRoleMaps'
ALTER TABLE [dbo].[UserRoleMaps]
ADD CONSTRAINT [FK_UserRoleMapRole]
    FOREIGN KEY ([RoleID])
    REFERENCES [dbo].[UserRoles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserRoleMapRole'
CREATE INDEX [IX_FK_UserRoleMapRole]
ON [dbo].[UserRoleMaps]
    ([RoleID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------