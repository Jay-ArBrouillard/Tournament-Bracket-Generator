CREATE TABLE `Matchups` (
 `matchup_id` int(11) NOT NULL AUTO_INCREMENT,
 `round_id` int(11) NOT NULL,
 `completed` tinyint(1) NOT NULL DEFAULT '0',
 PRIMARY KEY (`matchup_id`),
 FOREIGN KEY (round_id)
        REFERENCES Rounds (round_id)
)