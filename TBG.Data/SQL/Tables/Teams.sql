CREATE TABLE `Teams` (
 `TeamId` int(11) NOT NULL AUTO_INCREMENT,
 `TeamName` varchar(50) NOT NULL,
 `Wins` int(11) NOT NULL DEFAULT '0',
 `Losses` int(11) NOT NULL DEFAULT '0',
 PRIMARY KEY (`TeamId`)
)