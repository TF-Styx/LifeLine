CREATE OR REPLACE VIEW "V_Employee_Admin_ListItem" AS
	SELECT e."Id", 
		   e."Surname", 
		   e."Name", 
		   e."Patronymic",
		   e."DateEntry",
		   e."Rating",
		   e."Avatar",

		   g."Id" AS "GenderId",
		   g."Name" AS "GenderName"
	
	FROM "Employees" AS e
	LEFT JOIN "Genders" AS g ON e."GenderId" = g."Id";