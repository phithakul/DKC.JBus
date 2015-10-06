IF OBJECT_ID('dbo.BusOperators', 'U') IS NULL 
BEGIN 
	CREATE TABLE [dbo].[BusOperators]
	(
		Id					int				not null identity,		
		Name				nvarchar(50) 	not null,
		Phone				nvarchar(50) 	not null,
		Email				nvarchar(100) 	not null,
		BankAcctName 		nvarchar(100)	not null,
		BankAcctNo 			char(10) 		null,
		BankName 			nvarchar(50) 	not null,
		BankBranch 			nvarchar(50) 	not null,
		Contact				nvarchar(1000) 	not null,
		Note				nvarchar(200) 	not null,
		RowVersion			timestamp		not null,
		CONSTRAINT PK_BusOperators PRIMARY KEY(Id)
	);
	CREATE UNIQUE INDEX UX_Name ON [dbo].BusOperators(Name);	
END
go
