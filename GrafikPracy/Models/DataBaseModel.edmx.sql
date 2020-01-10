
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/06/2020 22:42:52
-- Generated from EDMX file: C:\Users\Marcin\source\repos\GrafikPracy\GrafikPracy\Models\DataBaseModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_PracownikUrlop]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Urlop] DROP CONSTRAINT [FK_PracownikUrlop];
GO
IF OBJECT_ID(N'[dbo].[FK_PracownikDzienRoboczyPracownika]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DzienRoboczyPracownika] DROP CONSTRAINT [FK_PracownikDzienRoboczyPracownika];
GO
IF OBJECT_ID(N'[dbo].[FK_DzienTygodniaGodzina]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Godzina] DROP CONSTRAINT [FK_DzienTygodniaGodzina];
GO
IF OBJECT_ID(N'[dbo].[FK_StanowiskoStanowiskoMiejsca]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StanowiskoMiejsca] DROP CONSTRAINT [FK_StanowiskoStanowiskoMiejsca];
GO
IF OBJECT_ID(N'[dbo].[FK_GodzinaStanowiskoMiejsca]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StanowiskoMiejsca] DROP CONSTRAINT [FK_GodzinaStanowiskoMiejsca];
GO
IF OBJECT_ID(N'[dbo].[FK_DzienTygodniaDzienRoboczyPracownika]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DzienRoboczyPracownika] DROP CONSTRAINT [FK_DzienTygodniaDzienRoboczyPracownika];
GO
IF OBJECT_ID(N'[dbo].[FK_UrlopDzienUrlopu]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DzienUrlopu] DROP CONSTRAINT [FK_UrlopDzienUrlopu];
GO
IF OBJECT_ID(N'[dbo].[FK_DzienDzienUrlopu]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DzienUrlopu] DROP CONSTRAINT [FK_DzienDzienUrlopu];
GO
IF OBJECT_ID(N'[dbo].[FK_GrafikCzas]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Czas] DROP CONSTRAINT [FK_GrafikCzas];
GO
IF OBJECT_ID(N'[dbo].[FK_CzasPracownikNaStanowisku]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PracownikNaStanowisku] DROP CONSTRAINT [FK_CzasPracownikNaStanowisku];
GO
IF OBJECT_ID(N'[dbo].[FK_PracownikPracownikNaStanowisku]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PracownikNaStanowisku] DROP CONSTRAINT [FK_PracownikPracownikNaStanowisku];
GO
IF OBJECT_ID(N'[dbo].[FK_StanowiskoPracownikNaStanowisku]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PracownikNaStanowisku] DROP CONSTRAINT [FK_StanowiskoPracownikNaStanowisku];
GO
IF OBJECT_ID(N'[dbo].[FK_PracownikStanowiskoPracownika]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StanowiskoPracownika] DROP CONSTRAINT [FK_PracownikStanowiskoPracownika];
GO
IF OBJECT_ID(N'[dbo].[FK_StanowiskoStanowiskoPracownika]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StanowiskoPracownika] DROP CONSTRAINT [FK_StanowiskoStanowiskoPracownika];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Pracownik]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Pracownik];
GO
IF OBJECT_ID(N'[dbo].[Urlop]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Urlop];
GO
IF OBJECT_ID(N'[dbo].[Dzien]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Dzien];
GO
IF OBJECT_ID(N'[dbo].[DzienTygodnia]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DzienTygodnia];
GO
IF OBJECT_ID(N'[dbo].[DzienRoboczyPracownika]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DzienRoboczyPracownika];
GO
IF OBJECT_ID(N'[dbo].[Stanowisko]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Stanowisko];
GO
IF OBJECT_ID(N'[dbo].[StanowiskoMiejsca]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StanowiskoMiejsca];
GO
IF OBJECT_ID(N'[dbo].[Godzina]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Godzina];
GO
IF OBJECT_ID(N'[dbo].[Grafik]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Grafik];
GO
IF OBJECT_ID(N'[dbo].[DzienUrlopu]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DzienUrlopu];
GO
IF OBJECT_ID(N'[dbo].[Czas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Czas];
GO
IF OBJECT_ID(N'[dbo].[PracownikNaStanowisku]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PracownikNaStanowisku];
GO
IF OBJECT_ID(N'[dbo].[StanowiskoPracownika]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StanowiskoPracownika];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Pracownik'
CREATE TABLE [dbo].[Pracownik] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Imie] nvarchar(30)  NOT NULL,
    [Nazwisko] nvarchar(30)  NOT NULL,
    [GodzinWUmowie] int  NOT NULL,
    [Email] nvarchar(100)  NOT NULL,
    [Haslo] nvarchar(max)  NOT NULL,
    [Administrator] bit  NOT NULL
);
GO

-- Creating table 'Urlop'
CREATE TABLE [dbo].[Urlop] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Zatwierdzony] bit  NOT NULL,
    [Powod] nvarchar(1000)  NOT NULL,
    [Pracownik_Id] int  NOT NULL,
    [Pracownik_Email] nvarchar(100)  NOT NULL
);
GO

-- Creating table 'Dzien'
CREATE TABLE [dbo].[Dzien] (
    [Data] datetime  NOT NULL
);
GO

-- Creating table 'DzienTygodnia'
CREATE TABLE [dbo].[DzienTygodnia] (
    [Id] int  NOT NULL,
    [Nazwa] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'DzienRoboczyPracownika'
CREATE TABLE [dbo].[DzienRoboczyPracownika] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Poczatek] datetime  NOT NULL,
    [Koniec] datetime  NOT NULL,
    [Pracownik_Id] int  NOT NULL,
    [Pracownik_Email] nvarchar(100)  NOT NULL,
    [DzienTygodnia_Id] int  NOT NULL
);
GO

-- Creating table 'Stanowisko'
CREATE TABLE [dbo].[Stanowisko] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nazwa] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'StanowiskoMiejsca'
CREATE TABLE [dbo].[StanowiskoMiejsca] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Minimum] int  NOT NULL,
    [Maksimum] int  NOT NULL,
    [Stanowisko_Id] int  NOT NULL,
    [Godzina_Id] int  NOT NULL
);
GO

-- Creating table 'Godzina'
CREATE TABLE [dbo].[Godzina] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Poczatek] datetime  NOT NULL,
    [Koniec] datetime  NOT NULL,
    [DzienTygodnia_Id] int  NOT NULL
);
GO

-- Creating table 'Grafik'
CREATE TABLE [dbo].[Grafik] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Zatwierdzony] datetime  NULL,
    [Poczatek] datetime  NOT NULL,
    [Koniec] datetime  NOT NULL
);
GO

-- Creating table 'DzienUrlopu'
CREATE TABLE [dbo].[DzienUrlopu] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Urlop_Id] int  NOT NULL,
    [Dzien_Data] datetime  NOT NULL
);
GO

-- Creating table 'Czas'
CREATE TABLE [dbo].[Czas] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Poczatek] datetime  NOT NULL,
    [Koniec] datetime  NOT NULL,
    [Grafik_Id] int  NOT NULL
);
GO

-- Creating table 'PracownikNaStanowisku'
CREATE TABLE [dbo].[PracownikNaStanowisku] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Czas_Id] int  NOT NULL,
    [Pracownik_Id] int  NOT NULL,
    [Pracownik_Email] nvarchar(100)  NOT NULL,
    [Stanowisko_Id] int  NOT NULL
);
GO

-- Creating table 'StanowiskoPracownika'
CREATE TABLE [dbo].[StanowiskoPracownika] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Pracownik_Id] int  NOT NULL,
    [Pracownik_Email] nvarchar(100)  NOT NULL,
    [Stanowisko_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id], [Email] in table 'Pracownik'
ALTER TABLE [dbo].[Pracownik]
ADD CONSTRAINT [PK_Pracownik]
    PRIMARY KEY CLUSTERED ([Id], [Email] ASC);
GO

-- Creating primary key on [Id] in table 'Urlop'
ALTER TABLE [dbo].[Urlop]
ADD CONSTRAINT [PK_Urlop]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Data] in table 'Dzien'
ALTER TABLE [dbo].[Dzien]
ADD CONSTRAINT [PK_Dzien]
    PRIMARY KEY CLUSTERED ([Data] ASC);
GO

-- Creating primary key on [Id] in table 'DzienTygodnia'
ALTER TABLE [dbo].[DzienTygodnia]
ADD CONSTRAINT [PK_DzienTygodnia]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DzienRoboczyPracownika'
ALTER TABLE [dbo].[DzienRoboczyPracownika]
ADD CONSTRAINT [PK_DzienRoboczyPracownika]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Stanowisko'
ALTER TABLE [dbo].[Stanowisko]
ADD CONSTRAINT [PK_Stanowisko]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'StanowiskoMiejsca'
ALTER TABLE [dbo].[StanowiskoMiejsca]
ADD CONSTRAINT [PK_StanowiskoMiejsca]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Godzina'
ALTER TABLE [dbo].[Godzina]
ADD CONSTRAINT [PK_Godzina]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Grafik'
ALTER TABLE [dbo].[Grafik]
ADD CONSTRAINT [PK_Grafik]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DzienUrlopu'
ALTER TABLE [dbo].[DzienUrlopu]
ADD CONSTRAINT [PK_DzienUrlopu]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Czas'
ALTER TABLE [dbo].[Czas]
ADD CONSTRAINT [PK_Czas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PracownikNaStanowisku'
ALTER TABLE [dbo].[PracownikNaStanowisku]
ADD CONSTRAINT [PK_PracownikNaStanowisku]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'StanowiskoPracownika'
ALTER TABLE [dbo].[StanowiskoPracownika]
ADD CONSTRAINT [PK_StanowiskoPracownika]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Pracownik_Id], [Pracownik_Email] in table 'Urlop'
ALTER TABLE [dbo].[Urlop]
ADD CONSTRAINT [FK_PracownikUrlop]
    FOREIGN KEY ([Pracownik_Id], [Pracownik_Email])
    REFERENCES [dbo].[Pracownik]
        ([Id], [Email])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PracownikUrlop'
CREATE INDEX [IX_FK_PracownikUrlop]
ON [dbo].[Urlop]
    ([Pracownik_Id], [Pracownik_Email]);
GO

-- Creating foreign key on [Pracownik_Id], [Pracownik_Email] in table 'DzienRoboczyPracownika'
ALTER TABLE [dbo].[DzienRoboczyPracownika]
ADD CONSTRAINT [FK_PracownikDzienRoboczyPracownika]
    FOREIGN KEY ([Pracownik_Id], [Pracownik_Email])
    REFERENCES [dbo].[Pracownik]
        ([Id], [Email])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PracownikDzienRoboczyPracownika'
CREATE INDEX [IX_FK_PracownikDzienRoboczyPracownika]
ON [dbo].[DzienRoboczyPracownika]
    ([Pracownik_Id], [Pracownik_Email]);
GO

-- Creating foreign key on [DzienTygodnia_Id] in table 'Godzina'
ALTER TABLE [dbo].[Godzina]
ADD CONSTRAINT [FK_DzienTygodniaGodzina]
    FOREIGN KEY ([DzienTygodnia_Id])
    REFERENCES [dbo].[DzienTygodnia]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DzienTygodniaGodzina'
CREATE INDEX [IX_FK_DzienTygodniaGodzina]
ON [dbo].[Godzina]
    ([DzienTygodnia_Id]);
GO

-- Creating foreign key on [Stanowisko_Id] in table 'StanowiskoMiejsca'
ALTER TABLE [dbo].[StanowiskoMiejsca]
ADD CONSTRAINT [FK_StanowiskoStanowiskoMiejsca]
    FOREIGN KEY ([Stanowisko_Id])
    REFERENCES [dbo].[Stanowisko]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StanowiskoStanowiskoMiejsca'
CREATE INDEX [IX_FK_StanowiskoStanowiskoMiejsca]
ON [dbo].[StanowiskoMiejsca]
    ([Stanowisko_Id]);
GO

-- Creating foreign key on [Godzina_Id] in table 'StanowiskoMiejsca'
ALTER TABLE [dbo].[StanowiskoMiejsca]
ADD CONSTRAINT [FK_GodzinaStanowiskoMiejsca]
    FOREIGN KEY ([Godzina_Id])
    REFERENCES [dbo].[Godzina]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GodzinaStanowiskoMiejsca'
CREATE INDEX [IX_FK_GodzinaStanowiskoMiejsca]
ON [dbo].[StanowiskoMiejsca]
    ([Godzina_Id]);
GO

-- Creating foreign key on [DzienTygodnia_Id] in table 'DzienRoboczyPracownika'
ALTER TABLE [dbo].[DzienRoboczyPracownika]
ADD CONSTRAINT [FK_DzienTygodniaDzienRoboczyPracownika]
    FOREIGN KEY ([DzienTygodnia_Id])
    REFERENCES [dbo].[DzienTygodnia]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DzienTygodniaDzienRoboczyPracownika'
CREATE INDEX [IX_FK_DzienTygodniaDzienRoboczyPracownika]
ON [dbo].[DzienRoboczyPracownika]
    ([DzienTygodnia_Id]);
GO

-- Creating foreign key on [Urlop_Id] in table 'DzienUrlopu'
ALTER TABLE [dbo].[DzienUrlopu]
ADD CONSTRAINT [FK_UrlopDzienUrlopu]
    FOREIGN KEY ([Urlop_Id])
    REFERENCES [dbo].[Urlop]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UrlopDzienUrlopu'
CREATE INDEX [IX_FK_UrlopDzienUrlopu]
ON [dbo].[DzienUrlopu]
    ([Urlop_Id]);
GO

-- Creating foreign key on [Dzien_Data] in table 'DzienUrlopu'
ALTER TABLE [dbo].[DzienUrlopu]
ADD CONSTRAINT [FK_DzienDzienUrlopu]
    FOREIGN KEY ([Dzien_Data])
    REFERENCES [dbo].[Dzien]
        ([Data])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DzienDzienUrlopu'
CREATE INDEX [IX_FK_DzienDzienUrlopu]
ON [dbo].[DzienUrlopu]
    ([Dzien_Data]);
GO

-- Creating foreign key on [Grafik_Id] in table 'Czas'
ALTER TABLE [dbo].[Czas]
ADD CONSTRAINT [FK_GrafikCzas]
    FOREIGN KEY ([Grafik_Id])
    REFERENCES [dbo].[Grafik]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GrafikCzas'
CREATE INDEX [IX_FK_GrafikCzas]
ON [dbo].[Czas]
    ([Grafik_Id]);
GO

-- Creating foreign key on [Czas_Id] in table 'PracownikNaStanowisku'
ALTER TABLE [dbo].[PracownikNaStanowisku]
ADD CONSTRAINT [FK_CzasPracownikNaStanowisku]
    FOREIGN KEY ([Czas_Id])
    REFERENCES [dbo].[Czas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CzasPracownikNaStanowisku'
CREATE INDEX [IX_FK_CzasPracownikNaStanowisku]
ON [dbo].[PracownikNaStanowisku]
    ([Czas_Id]);
GO

-- Creating foreign key on [Pracownik_Id], [Pracownik_Email] in table 'PracownikNaStanowisku'
ALTER TABLE [dbo].[PracownikNaStanowisku]
ADD CONSTRAINT [FK_PracownikPracownikNaStanowisku]
    FOREIGN KEY ([Pracownik_Id], [Pracownik_Email])
    REFERENCES [dbo].[Pracownik]
        ([Id], [Email])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PracownikPracownikNaStanowisku'
CREATE INDEX [IX_FK_PracownikPracownikNaStanowisku]
ON [dbo].[PracownikNaStanowisku]
    ([Pracownik_Id], [Pracownik_Email]);
GO

-- Creating foreign key on [Stanowisko_Id] in table 'PracownikNaStanowisku'
ALTER TABLE [dbo].[PracownikNaStanowisku]
ADD CONSTRAINT [FK_StanowiskoPracownikNaStanowisku]
    FOREIGN KEY ([Stanowisko_Id])
    REFERENCES [dbo].[Stanowisko]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StanowiskoPracownikNaStanowisku'
CREATE INDEX [IX_FK_StanowiskoPracownikNaStanowisku]
ON [dbo].[PracownikNaStanowisku]
    ([Stanowisko_Id]);
GO

-- Creating foreign key on [Pracownik_Id], [Pracownik_Email] in table 'StanowiskoPracownika'
ALTER TABLE [dbo].[StanowiskoPracownika]
ADD CONSTRAINT [FK_PracownikStanowiskoPracownika]
    FOREIGN KEY ([Pracownik_Id], [Pracownik_Email])
    REFERENCES [dbo].[Pracownik]
        ([Id], [Email])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PracownikStanowiskoPracownika'
CREATE INDEX [IX_FK_PracownikStanowiskoPracownika]
ON [dbo].[StanowiskoPracownika]
    ([Pracownik_Id], [Pracownik_Email]);
GO

-- Creating foreign key on [Stanowisko_Id] in table 'StanowiskoPracownika'
ALTER TABLE [dbo].[StanowiskoPracownika]
ADD CONSTRAINT [FK_StanowiskoStanowiskoPracownika]
    FOREIGN KEY ([Stanowisko_Id])
    REFERENCES [dbo].[Stanowisko]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StanowiskoStanowiskoPracownika'
CREATE INDEX [IX_FK_StanowiskoStanowiskoPracownika]
ON [dbo].[StanowiskoPracownika]
    ([Stanowisko_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------