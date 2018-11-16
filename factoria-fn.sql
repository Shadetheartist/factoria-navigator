
drop function ConsititutionalRecipe;
go

create function ConsititutionalRecipe (@RecipeId int)
returns @returnTable table (
	Id int, 
	Tier int not null,
	Ticks int not null,
	ProductEntity_Id int not null,
	ProductEntity_Amount int not null,
	IngredientEntityA_Id int not null,
	IngredientEntityA_Amount int not null,
	IngredientEntityB_Id int not null,
	IngredientEntityB_Amount int not null
)
as begin

	with 
	aCte as (
		select
			* 
		from 
			Recipes
		where 
			Id = @RecipeId
		union all
			select r.* from Recipes as r
				inner join aCte as c on c.IngredientEntityA_Id = r.ProductEntity_Id or c.IngredientEntityB_Id = r.ProductEntity_Id),
	bCte as (
		select
			* 
		from 
			Recipes
		where 
			Id = @RecipeId
		union all
			select r.* from Recipes as r 
				inner join bCte as c on c.IngredientEntityB_Id = r.ProductEntity_Id or c.IngredientEntityB_Id = r.ProductEntity_Id)

	insert into @returnTable select * from aCte union all (select * from bCte);
	delete from @returnTable where Id = @RecipeId;
	insert into @returnTable select * from Recipes where Id = @RecipeId

	return
end

go






select SUM(Ticks * ProductEntity_Amount) from ConsititutionalRecipe(25) 


select 
	r.Id,
	r.Tier,
	r.Ticks,

	pE.Id as ProductId,
	pE.EntityName as ProductName,
	r.ProductEntity_Amount as ProductAmount,

	aE.Id as IngredientAId,
	aE.EntityName as IngredientA,
	r.IngredientEntityA_Amount as IngredientA_Amount,

	bE.Id as IngredientBId,
	bE.EntityName as IngredientB,
	r.IngredientEntityB_Amount as IngredientB_Amount
from 
	ConsititutionalRecipe(16) as r
	join Entities as pE on pE.Id = r.ProductEntity_Id
	join Entities as aE on aE.Id = r.IngredientEntityA_Id
	join Entities as bE on bE.Id = r.IngredientEntityB_Id


