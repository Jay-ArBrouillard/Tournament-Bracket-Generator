CREATE TABLE `TournamentTeamPeople` (
 `TournamentId` int(11) DEFAULT NULL,
 `TeamId` int(11) DEFAULT NULL,
 `PersonId` int(11) DEFAULT NULL,
 KEY `TournamentId` (`TournamentId`),
 KEY `TeamId` (`TeamId`),
 KEY `PersonId` (`PersonId`),
 CONSTRAINT `TournamentTeamPeople_ibfk_1` FOREIGN KEY (`TournamentId`) REFERENCES `Tournaments` (`TournamentId`),
 CONSTRAINT `TournamentTeamPeople_ibfk_2` FOREIGN KEY (`TeamId`) REFERENCES `Teams` (`TeamId`),
 CONSTRAINT `TournamentTeamPeople_ibfk_3` FOREIGN KEY (`PersonId`) REFERENCES `People` (`PersonId`)
)