USE Test;
GO

exec sp_msforeachtable 'Drop table ?'
GO