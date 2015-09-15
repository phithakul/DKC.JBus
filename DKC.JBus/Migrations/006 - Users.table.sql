IF OBJECT_ID('dbo.Users', 'U') IS NULL
BEGIN
	CREATE TABLE [dbo].[Users]
	(
		Id			int 		not null identity,
		Username		nvarchar(50) 	not null,
		Hash			nvarchar(100)	null,		
		UserType		int				not null,
		FullName		nvarchar(100)	null,
		MobileNo 		nvarchar(10) 	not null CONSTRAINT [DF_Users_MobileNo]  DEFAULT (''),
		Email			nvarchar(100)	not null CONSTRAINT [DF_Users_Email]  DEFAULT (''),
		Active	 		bit 			not null CONSTRAINT DFT_Users_Active DEFAULT(1),
		CreatedDate		datetime		not null CONSTRAINT DFT_Users_CreatedDate DEFAULT(GETDATE()),
		UpdatedDate		datetime		not null CONSTRAINT DFT_Users_UpdatedDate DEFAULT(GETDATE()),
		LastActivityDate		datetime		not null CONSTRAINT DFT_Users_LastActivityDate DEFAULT(GETDATE()),
		CONSTRAINT PK_Users PRIMARY KEY(Id)
	);
	CREATE UNIQUE INDEX UX_Username ON [dbo].[Users](Username);

	INSERT INTO [dbo].[Users] (Username, Hash, UserType, IsAgent) VALUES
	(N'admin', N'AL+ZfHbpSDNonpIXk4c9eCL3Gy1SXZwyYcG6ePJIhj7tiEGS8jkRbi/rIARFetltpg==', 1, 0),
	(N'a', N'ANkxabdXz+a/21SOmfgXJxPTqKwCmqDN8nrc2MJVW/lCdYSTSawdJ7WyN65FIQE0bA==', 3, 0),
    (N'b', N'AGjohfkl8rPaVcbtk54lnMQeysdoVbyX0NSJMI8uMAfjrZE9NMVW22QVjGhGXaJ85Q==', 2, 0);
END;
go
-- password = admin