IF OBJECT_ID('dbo.MailLogs', 'U') IS NULL
BEGIN
	CREATE TABLE [dbo].[MailLogs]
	(
		Id 			int 			not null identity,
		UserId		int				not null,
		MailType	int				not null,
		ToAddr		nvarchar(100)	not null,
		CcAddr		nvarchar(100)	not null,
		Subject		nvarchar(200)	not null,
		Message		nvarchar(MAX) 	not null,
		Attachment	nvarchar(200) 	null,
		CreateTime	datetime		not null,
		SentTime	datetime		null,
		Status		int 			not null,
		ErrorMsg	nvarchar(MAX)	not null,
		CONSTRAINT PK_MailLogs PRIMARY KEY(Id)
	);
END
go