/*
值區計數應該設定為索引鍵中
相異值之最大預期值的兩倍
左右，捨入為最接近 2 的次方。
*/

CREATE TABLE [dbo].[Table1]
(
	[Id] INT NOT NULL PRIMARY KEY NONCLUSTERED HASH WITH (BUCKET_COUNT = 131072)
) WITH (MEMORY_OPTIMIZED = ON)

GO

/*
不要變更資料庫路徑或名稱變數。
任何 sqlcmd 變數都會在組建和部署期間
經過適當取代。
*/

ALTER DATABASE [$(DatabaseName)]
	ADD FILEGROUP [Table1_FG] CONTAINS MEMORY_OPTIMIZED_DATA