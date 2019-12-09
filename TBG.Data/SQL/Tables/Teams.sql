CREATE TABLE `Teams` (
 `team_id` int(11) NOT NULL AUTO_INCREMENT,
 `team_name` varchar(50) NOT NULL,
 `wins` int(5) NOT NULL DEFAULT '0',
 `losses` int(5) NOT NULL DEFAULT '0',
 PRIMARY KEY (`team_id`)
)