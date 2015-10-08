IF OBJECT_ID('dbo.AppSettings', 'U') IS NULL
BEGIN
	CREATE TABLE [dbo].[AppSettings]
	(
		Id 			int 			not null identity,
		Setting		nvarchar(50)	not null,
		Value		nvarchar(max) 	null,
		CONSTRAINT PK_AppSettings PRIMARY KEY(Id)
	);
	CREATE UNIQUE INDEX UX_Setting ON [dbo].[AppSettings](Setting);

	INSERT INTO [dbo].[AppSettings] (Setting, Value) VALUES
	('UseProxyServer',N'false'),
	('HttpProxyAddress',N''),
	('HttpProxyPort',N'80'),
	('HttpProxyUsername',N''),
	('HttpProxyPassword',N''),
	('DomainControllerIP',N'10.2.1.4'),
	('DomainName',N'dkc.net');
END
go