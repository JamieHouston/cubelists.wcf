USE [master]
GO

Alter Database CubeLists
  SET SINGLE_USER With ROLLBACK IMMEDIATE

/****** Object:  Database [CubeLists]    Script Date: 01/05/2012 14:14:38 ******/
IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'CubeLists')
DROP DATABASE [CubeLists]
GO

USE [master]
GO

/****** Object:  Database [CubeLists]    Script Date: 01/05/2012 14:14:23 ******/
CREATE DATABASE [CubeLists] ON  PRIMARY 
( NAME = N'CubeLists', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10.MSSQLSERVER\MSSQL\DATA\CubeLists.mdf' , SIZE = 2048KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'CubeLists_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10.MSSQLSERVER\MSSQL\DATA\CubeLists_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [CubeLists] SET COMPATIBILITY_LEVEL = 100
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CubeLists].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [CubeLists] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [CubeLists] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [CubeLists] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [CubeLists] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [CubeLists] SET ARITHABORT OFF 
GO

ALTER DATABASE [CubeLists] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [CubeLists] SET AUTO_CREATE_STATISTICS ON 
GO

ALTER DATABASE [CubeLists] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [CubeLists] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [CubeLists] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [CubeLists] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [CubeLists] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [CubeLists] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [CubeLists] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [CubeLists] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [CubeLists] SET  DISABLE_BROKER 
GO

ALTER DATABASE [CubeLists] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [CubeLists] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [CubeLists] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [CubeLists] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [CubeLists] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [CubeLists] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [CubeLists] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [CubeLists] SET  READ_WRITE 
GO

ALTER DATABASE [CubeLists] SET RECOVERY FULL 
GO

ALTER DATABASE [CubeLists] SET  MULTI_USER 
GO

ALTER DATABASE [CubeLists] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [CubeLists] SET DB_CHAINING OFF 
GO

USE CubeLists
GO
CREATE TABLE [dbo].[Cube](
	[ID] [int] IDENTITY NOT NULL,
	[KeyName] [varchar](50) NOT NULL,
	[CubeValue] [varchar](100) NULL,
	[ParentKey] [varchar](50) NULL,
	[CubeType] [varchar](50) NULL,
 CONSTRAINT [PK_Cube] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

 CREATE USER [Intellagent_Demo] FOR LOGIN [Intellagent_Demo]
 EXEC sp_addrolemember N'db_owner',N'Intellagent_Demo'
 
USE [CubeLists]
GO

/****** Object:  StoredProcedure [dbo].[CreateCube]    Script Date: 01/16/2012 09:40:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[CreateCube](
@KeyName	VARCHAR(50),
@CubeValue		VARCHAR(255),
@ParentKey	VARCHAR(50),
@CubeType	VARCHAR(50))
AS
INSERT INTO Cube(KeyName,CubeType,ParentKey,CubeValue)
VALUES
(@KeyName, @CubeType, @ParentKey, @CubeValue)
SELECT @@IDENTITY

GO

USE [CubeLists]
GO

/****** Object:  StoredProcedure [dbo].[DeleteCubeByKey]    Script Date: 01/16/2012 09:40:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[DeleteCubeByKey](
@KeyName	varchar(50))
AS
DELETE FROM Cube WHERE KeyName = @KeyName

GO

USE [CubeLists]
GO

/****** Object:  StoredProcedure [dbo].[GetCubeByKey]    Script Date: 01/16/2012 09:40:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[GetCubeByKey](
@KeyName	varchar(50))
AS
SELECT * FROM Cube WHERE KeyName = @KeyName

GO

USE [CubeLists]
GO

/****** Object:  StoredProcedure [dbo].[GetCubes]    Script Date: 01/16/2012 09:40:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[GetCubes]
AS
SELECT * FROM Cube

GO

USE [CubeLists]
GO

/****** Object:  StoredProcedure [dbo].[GetCubesByParentKey]    Script Date: 01/16/2012 09:40:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[GetCubesByParentKey](
@ParentKey	varchar(50))
AS
SELECT * FROM Cube WHERE ParentKey = @ParentKey

GO

CREATE PROCEDURE [dbo].[GetCubeWithChildren](
@ParentKey varchar(50))
AS
BEGIN
SELECT * FROM Cube WHERE KeyName = @ParentKey
SELECT * FROM Cube WHERE ParentKey = @ParentKey
END
GO


/*** CREATE DEFAULTS **/

-- System Master
EXEC CreateCube 'System', 'System', 'Master', 'Config'
GO

-- Master List
EXEC CreateCube 'Lists', 'Lists', 'System', 'Lists'
GO

-- Example first list
EXEC CreateCube 'List1', 'List 1', 'Lists', 'Lists'
GO

-- Master Type
EXEC CreateCube 'Types', 'Types', 'System', 'Types'
GO

-- Default system types
EXEC CreateCube 'String', 'String', 'Types', 'Types'
GO

EXEC CreateCube 'Bool', 'Checkbox', 'Types', 'Types'
GO

-- Master Entity
EXEC CreateCube 'Entities', 'Entities', 'System', 'Types'
GO

USE CubeLists
 CREATE USER [CubeList] FOR LOGIN [CubeList]
 EXEC sp_addrolemember N'db_owner',N'CubeList'
