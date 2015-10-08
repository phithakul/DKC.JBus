IF OBJECT_ID('dbo.BusRequests', 'U') IS NULL 
BEGIN 
	CREATE TABLE [dbo].[BusRequests]
	(
		Id 				int 			not null identity,
		Code			int				not null,
		RequestUserId	int				not null,
		CreationDate	datetime		not null,
		RequesterPhone	nvarchar(10) 	not null,
		CoorName		nvarchar(30) 	not null,
		CoorPhone		nvarchar(10) 	not null,
		Amount			decimal(8,0)	not null,
		TravelType		int				not null,
		ReqReceipt		bit 			not null,
		IsCancel		bit 			not null,
		RequestStatus	int				not null,		
		RowVersion		timestamp		not null,
		CONSTRAINT PK_BusRequests PRIMARY KEY(Id)
	);
	CREATE UNIQUE INDEX UX_Code ON [dbo].[BusRequests](Code);
END
go