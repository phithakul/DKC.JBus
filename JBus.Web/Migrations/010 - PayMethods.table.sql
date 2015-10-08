IF OBJECT_ID('dbo.PayMethods', 'U') IS NULL 
BEGIN 
	CREATE TABLE [dbo].[PayMethods]
	(
        Id 				int 			not null identity,
		Name	 		nvarchar(20) 	not null,
		IsOwnerDebit	bit				not null,
		CONSTRAINT PK_PayMethods PRIMARY KEY(Id)
	);
	CREATE UNIQUE INDEX UX_Name ON [dbo].PayMethods(Name);
	INSERT INTO [dbo].[PayMethods] (Name,IsOwnerDebit) VALUES 
	(N'ใบฟ้าตัดบัญชี', 1),
	(N'เงินสด', 0),
	(N'งบส่วนกลาง', 0);
END
go