First create a MySQL Database named 'elbjungs'
Then add a Table 

CREATE TABLE `produkte` (
	`id` INT(11) NOT NULL AUTO_INCREMENT,
	`Produkt` VARCHAR(255) NULL DEFAULT NULL,
	`Beschreibung` TEXT NULL DEFAULT NULL,
	`Preis` DECIMAL(10,2) NULL DEFAULT NULL,
	`Anzahl` DECIMAL(10,2) NULL DEFAULT NULL,
	`Gesamtpreis` DECIMAL(10,2) NULL DEFAULT NULL,
	PRIMARY KEY (`id`)
)
COLLATE='utf8mb4_general_ci'
ENGINE=InnoDB
AUTO_INCREMENT=19
;
