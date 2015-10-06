IF OBJECT_ID('dbo.Locations', 'U') IS NULL 
BEGIN 
	CREATE TABLE [dbo].[Locations]
	(
		Id 				int 			not null,
		RegionId 		int 			not null,
		RegionName 		nvarchar(20) 	not null,
		ProvinceId 		int 			null,
		ProvinceName 	nvarchar(20) 	null,
		AmphurId 		int 			null,
		AmphurName 		nvarchar(30)	null,		
		Depth			int 			not null,
		CreatedDate		datetime		not null
		CONSTRAINT DF_Locations_CreatedDate DEFAULT(GETDATE()),
		CONSTRAINT PK_Locations PRIMARY KEY CLUSTERED (Id ASC)
	);
END