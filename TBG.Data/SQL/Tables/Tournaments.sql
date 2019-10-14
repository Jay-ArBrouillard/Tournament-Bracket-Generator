CREATE TABLE `Tournaments` (
 `TournamentId` int(11) NOT NULL AUTO_INCREMENT,
 `TournamentName` varchar(50) NOT NULL,
 `EntryFee` decimal(10,0) NOT NULL,
 PRIMARY KEY (`TournamentId`)
)