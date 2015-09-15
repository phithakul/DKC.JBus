USE [master];
GO

CREATE DATABASE [MTLEdata] COLLATE Thai_CI_AS;
GO
    
ALTER DATABASE [MTLEdata] MODIFY FILE 
(NAME = N'MTLEdata' , SIZE = 500MB , MAXSIZE = UNLIMITED, FILEGROWTH = 100MB);
GO

ALTER DATABASE [MTLEdata] MODIFY FILE 
(NAME = N'MTLEdata_log' , SIZE = 200MB , MAXSIZE = 2GB , FILEGROWTH = 10%);
GO


IF NOT EXISTS 
    (SELECT name  
     FROM master.sys.server_principals
     WHERE name = 'db_username')
BEGIN
    CREATE LOGIN [db_username]
		WITH PASSWORD = N'db_password',
		CHECK_POLICY     = OFF,
		CHECK_EXPIRATION = OFF;
END

USE [MTLEdata];
GO

IF NOT EXISTS 
	(SELECT name
	FROM sys.database_principals 
	WHERE name = N'db_username')
BEGIN
    CREATE USER [db_username] FOR LOGIN [db_username] WITH DEFAULT_SCHEMA=[dbo];
    EXEC sp_addrolemember N'db_owner', N'db_username';
END;
GO
