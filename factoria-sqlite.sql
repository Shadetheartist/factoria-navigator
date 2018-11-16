drop table RecipeRequiredUpgrades;
drop table Recipes;
drop table Products;
drop table Upgrades;

create table Products (
	ProductCode int not null,
	ProductName varchar(255) not null
);

create table Upgrades (
	UpgradeName varchar(255) not null,
	UpgradeType varchar(255) not null,
	Stage int not null
);

create table Recipes (
	InputCode int not null, 
	RecipeName varchar(255), 
	InputCode int not null, 
	Tier int not null,
	Ticks int not null,
	Product_Id int not null,
	Product_Amount int not null,
	IngredientA_Id int not null ,
	IngredientA_Amount int not null,
	IngredientB_Id int not null,
	IngredientB_Amount int not null
);

create table RecipeRequiredUpgrades (
	RecipeId int not null,
	UpgradeId int not null,
	primary key (RecipeId, UpgradeId)
);

insert into Upgrades (UpgradeName, UpgradeType, Stage) values 
('basic filtration 1',		1, 1),
('basic filtration 2',		1, 2),
('basic filtration 3',		1, 3),
('advanced filtration',		1, 4),
('advanced filtration 2',	1, 5),
('advanced filtration 3',	1, 6),
('perfect filtration',		1, 7)
;


insert into Products (ProductCode, ProductName) values
(0, 'nothing'),

--t1
(1, 'dirt'), 
(2, 'coal'),
(3, 'iron ore'),
(4, 'copper ore'),
(5, 'tin ore'),
(6, 'water'),
(7, 'mineral rich dirt'),

--t2
(8, 'sand'), --dirt+
(9, 'mud'), --dirt + water
(10, 'purified water'), --water+
(11, 'carbon'), --coal+
(12, 'iron ingot'), --iron+
(13, 'copper ingot'), --copper+
(14, 'tin ingot'), --tin+
(15, 'mineral slurry'), --mineral rich dirt + water

--t3
--16
(16, 'filter material'), --sand + carbon
(17, 'gold ore'), --mineral slurry + purified water
(18, 'silver ore'), --mineral slurry + purified water
(19, 'bronze ingot'), --copper ingot + tin ingot
(20, 'steel ingot'), --iron ingot + carbon
(21, 'iron plate'), --iron ingot+
(22, 'copper plate'), --iron ingot+
(23, 'tin plate'), --tin ingot+

--t4
(24, 'steel plate'), --steel ingot+

--t5
(25, 'filter') --steel plate + filter material
;

insert into Recipes (InputCode, RecipeName, Tier, Product_Id, Product_Amount, Ticks, IngredientA_Id, IngredientA_Amount, IngredientB_Id, IngredientB_Amount) values 
--T1
(0, 'mine dirt',			1,	1, 4, 4,  0, 0,  0, 0), --dirt
(0, 'mine coal',			1,	2, 2, 2,  0, 0,  0, 0), --coal
(0, 'mine iron',			1,  3, 2, 4,  0, 0,  0, 0), --iron ore
(0, 'mine copper',			1,  4, 2, 6,  0, 0,  0, 0), --copper ore
(0, 'mine tin',			1,  5, 2, 6,  0, 0,  0, 0), --tin ore
(0, 'pump water',			1,  6, 4, 4,  0, 0,  0, 0), --water
(0, 'mine rich dirt',		1,  7, 4, 8,  0, 0,  0, 0), --mineral rich dirt

--T2
(0, 'sift dirt',			2,   8, 2, 8,   1, 2,  0, 0), --sand
(0, 'make mud',			2,   9, 2, 8,   1, 2,  6, 2), --mud
(0, 'purify water',		2,  10, 2, 16,  6, 4,  0, 0), --purified water
(0, 'make carbon',			2,  11, 1, 8,   2, 2,  0, 0), --carbon
(0, 'forge iron ingot',	2,  12, 2, 8,   3, 4,  2, 1), --iron ingot
(0, 'forge copper ingot',  2,  13, 2, 12,  4, 4,  2, 1), --copper ingot
(0, 'forge tin ingot',     2,  14, 2, 12,  5, 4,  2, 1), --tin ingot
(0, 'make mineral slurry', 2,  15, 4, 16,  7, 4,  0, 0), --mineral slurry

--T3
(0, 'make filter material',	3,   16, 2,  32,   8,  8,   11, 8), --filter material
(0, 'extract gold ore',		3,   17, 2, 128,   15, 8,   10, 8), --gold ore
(0, 'extract silver ore',		3,   18, 2,  64,   15, 8,   10, 8), --silver ore
(0, 'forge bronze ingot',		3,   19, 2,  32,   12, 2,   13, 2), --bronze ingot
(0, 'forge steel ingot',		3,   20, 2,  32,   12, 6,   11, 2), --steel ingot
(0, 'forge iron plate',		3,   21, 2,  16,   12, 4,   0,  0), --iron plate
(0, 'forge copper plate',		3,   22, 2,  24,   12, 4,   0,  0), --copper plate
(0, 'forge tin plate',			3,   23, 2,  24,   12, 4,   0,  0), --tin plate

--T4
(0, 'forge steel plate',	4,  24, 2, 64,  20, 4,  0, 0), --steel plate

--T5
(0, 'make filter', 5,  25, 1, 512,  24, 2,  16, 4), --filter

--T6
(0, 'gold filtration', 5,  17, 4, 128,  15, 8,  0, 0) --gold ore (filter up 1)
;


insert into RecipeRequiredUpgrades (RecipeId, UpgradeId) 
values 
(
	(select rowid from Recipes where RecipeName = 'gold filtration' limit 1),
	(select rowid from Upgrades where UpgradeName = 'basic filtration 1' limit 1)
);
