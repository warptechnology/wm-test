CREATE procedure [dbo].[AddOrUpdate]
	@id int,
	@value int
as
begin
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
	BEGIN TRANSACTION
		if exists (select [SecondTaskModels].[ID] from [dbo].[SecondTaskModels] where [SecondTaskModels].[ID] = @id)
			begin
				update [dbo].[SecondTaskModels]
				set Value=@value where [SecondTaskModels].[ID]=@id
			end
		else
			begin
				insert into [dbo].[SecondTaskModels]([Value])
				values (@value)
			end		
	COMMIT TRANSACTION
	return @id
end