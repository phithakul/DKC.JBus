IF OBJECT_ID('dbo.SmsLogs', 'U') IS NULL 
BEGIN 
	CREATE TABLE [dbo].[SmsLogs]
	(
		Id 			int 			not null identity,
		UserId		int				not null,
		MobileNo	nvarchar(15) 	not null,
		Message		nvarchar(500) 	not null,
		SentTime	datetime 		not null,
		SmsStatus	int 			not null,
		TaskId		nvarchar(20) 	not null,
		MessageId	nvarchar(20) 	not null,
		ErrorMsg	nvarchar(200) 	not null,
		CONSTRAINT PK_SmsLogs PRIMARY KEY(Id)
	);
	CREATE INDEX UX_ProjectType ON [dbo].[SmsLogs](ProjectType);
END
go