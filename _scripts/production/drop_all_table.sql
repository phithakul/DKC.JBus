USE JBus;
GO

exec sp_msforeachtable 'Drop table ?'
GO