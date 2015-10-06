IF OBJECT_ID('dbo.BusRequestPays', 'U') IS NULL
BEGIN
	CREATE TABLE [dbo].[BusRequestPays]
	(
        Id 				int				not null identity,
		BusRequestId	int				not null,
		PayMethodId		int				not null,
		Amount			decimal(8,0)	not null,
		CONSTRAINT PK_BusRequestPays PRIMARY KEY(Id)
	);
	CREATE UNIQUE INDEX UX_BusRequestId_PayMethodId ON [dbo].[BusRequestPays](BusRequestId,PayMethodId);
END
go