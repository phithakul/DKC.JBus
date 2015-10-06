IF OBJECT_ID('dbo.BudgetDebits', 'U') IS NULL 
BEGIN 
	CREATE TABLE [dbo].[BudgetDebits]
	(
		Id				int				not null identity,
		Code			nvarchar(15) 	not null,
		[Date]			datetime		not null,
		Department		nvarchar(50) 	not null,
		Section			nvarchar(50) 	not null,
		JobCode			nvarchar(10) 	not null,
		JobName			nvarchar(50) 	not null,
		Amount			decimal(8,0) 	not null,
		Note			nvarchar(200) 	not null,		
		RetrievedDate	datetime		not null,		
		CONSTRAINT PK_BudgetDebits PRIMARY KEY(Id)
	);
	CREATE UNIQUE INDEX UX_Code ON [dbo].BudgetDebits(Code);
END
go
