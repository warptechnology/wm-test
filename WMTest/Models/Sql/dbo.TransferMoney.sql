CREATE PROCEDURE [dbo].[TransferMoney]
	@Source int,
	@Destination int,
	@Ammount money
AS
begin
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
	BEGIN transaction
		begin try
			if not exists (select [ThirdTaskModels].[ID] from [dbo].[ThirdTaskModels] where [ThirdTaskModels].[ID] = @Source)
				begin
					rollback transaction;
					return 0;
				end
			if not exists (select [ThirdTaskModels].[ID] from [dbo].[ThirdTaskModels] where [ThirdTaskModels].[ID] = @Destination)
				begin
					rollback transaction;
					return 1;
				end
			declare @Balance as money;
			select @Balance = [ThirdTaskModels].Balance from [dbo].[ThirdTaskModels] where [ThirdTaskModels].[ID] = @Source;
			if(@Balance < @Ammount) 
				begin
					rollback transaction;
					return 3;
				end
			update [dbo].[ThirdTaskModels]  
			SET [ThirdTaskModels].[Balance] = [ThirdTaskModels].[Balance] + @Ammount
			where [ThirdTaskModels].[ID] = @Destination;  
			update [dbo].[ThirdTaskModels]  
			SET [ThirdTaskModels].[Balance] = [ThirdTaskModels].[Balance] - @Ammount
			where [ThirdTaskModels].[ID] = @Source;
		

		end try
		begin catch
			if (XACT_STATE() <> 0)
			begin	
				rollback transaction;
				return 4;
			end
		end catch
	COMMIT transaction
	return 5;
end