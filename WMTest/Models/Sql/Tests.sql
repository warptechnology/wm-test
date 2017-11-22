
DECLARE @i AS INT
EXEC @i = [dbo].[TransferMoney] 
	@Source = 1,
	@Destination = 2,
	@Ammount = 55.0
SELECT @i