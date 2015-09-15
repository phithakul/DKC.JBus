USE MtlEdata;
GO

exec sp_msforeachtable 'Drop table ?'
GO