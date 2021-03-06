USE [master];
GO

DECLARE @DB_NAME VARCHAR(255)
DECLARE @DB_USERNAME VARCHAR(255)
DECLARE @DB_PASSWORD VARCHAR(255)

SET @DB_NAME = 'JBus'
SET @DB_USERNAME = 'JBus'
SET @DB_PASSWORD = 'JBus'

DECLARE @CREATE_TABLE_TEMPLATE VARCHAR(MAX)
DECLARE @CREATE_LOGIN_TEMPLATE VARCHAR(MAX)
DECLARE @CREATE_USER_TEMPLATE VARCHAR(MAX)

SET @CREATE_TABLE_TEMPLATE = '
IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N''{DB_NAME}'')
BEGIN
  CREATE DATABASE {DB_NAME} COLLATE Thai_CI_AS
  ALTER DATABASE {DB_NAME} MODIFY FILE (NAME = N''{DB_NAME}'' , SIZE = 5MB , MAXSIZE = UNLIMITED, FILEGROWTH = 1MB)
  ALTER DATABASE {DB_NAME} MODIFY FILE (NAME = N''{DB_NAME}_log'' , SIZE = 2MB , MAXSIZE = 2GB , FILEGROWTH = 10%)
END'
SET @CREATE_LOGIN_TEMPLATE = '
IF NOT EXISTS (SELECT name FROM master.sys.server_principals WHERE name = ''{DB_USERNAME}'')
BEGIN
    CREATE LOGIN [{DB_USERNAME}]
		WITH PASSWORD = N''{DB_PASSWORD}'',
		CHECK_POLICY     = OFF,
		CHECK_EXPIRATION = OFF;
END'
SET @CREATE_USER_TEMPLATE = '
USE [{DB_NAME}]
IF NOT EXISTS 
  (SELECT name
    FROM sys.database_principals 
    WHERE name = N''{DB_USERNAME}'')
BEGIN
    CREATE USER [{DB_USERNAME}] FOR LOGIN [{DB_USERNAME}] WITH DEFAULT_SCHEMA=[dbo];
    EXEC sp_addrolemember N''db_owner'', N''{DB_USERNAME}'';
END;'

DECLARE @SQL_SCRIPT VARCHAR(MAX)

SET @SQL_SCRIPT = REPLACE(@CREATE_TABLE_TEMPLATE, '{DB_NAME}', @DB_NAME)
EXECUTE (@SQL_SCRIPT)

SET @SQL_SCRIPT = REPLACE(@CREATE_LOGIN_TEMPLATE, '{DB_USERNAME}', @DB_USERNAME)
SET @SQL_SCRIPT = REPLACE(@SQL_SCRIPT, '{DB_PASSWORD}', @DB_PASSWORD)
EXECUTE (@SQL_SCRIPT)

SET @SQL_SCRIPT = REPLACE(@CREATE_USER_TEMPLATE, '{DB_NAME}', @DB_NAME)
SET @SQL_SCRIPT = REPLACE(@SQL_SCRIPT, '{DB_USERNAME}', @DB_USERNAME)
EXECUTE (@SQL_SCRIPT)
