IF OBJECT_ID('dbo.Users', 'U') IS NULL
BEGIN
	CREATE TABLE [dbo].[Users]
	(
		Id			int 		not null identity,
		Username		nvarchar(50) 	not null,		
		UserType		int				not null,
		FullName		nvarchar(100)	not null,
		MemberType		nvarchar(20)	not null,
		Department		nvarchar(50)	not null,
		Section			nvarchar(50)	not null,
		MobileNo 		nvarchar(10) 	null,
		Email			nvarchar(100)	null,
		CreationDate	datetime		not null,		
		LastActivityDate	datetime		null,
		CONSTRAINT PK_Users PRIMARY KEY(Id)
	);
	CREATE UNIQUE INDEX UX_Username ON [dbo].[Users](Username);

	INSERT INTO [dbo].[Users] (Username,UserType,FullName,MemberType,Department,Section,CreationDate) VALUES
	(N'vphithakul', 1, '', '', '', '', GETDATE()); -- admin
END;
go
-- password = admin