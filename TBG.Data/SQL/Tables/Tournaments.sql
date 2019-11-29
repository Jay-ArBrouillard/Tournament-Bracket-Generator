CREATE TABLE `Tournaments` (
 `tournament_id` int(11) NOT NULL AUTO_INCREMENT,
 `user_id` int(11) NOT NULL,
 `tournament_name` varchar(50) NOT NULL,
 `entry_fee` decimal(10,2) NOT NULL,
 `total_prize_pool` decimal(10,2) NOT NULL,
 `tournament_type_id` int(11) DEFAULT NULL,
 `active_round` int(2) NOT NULL,
 PRIMARY KEY (`tournament_id`),
 FOREIGN KEY (user_id)
        REFERENCES Users (user_id),
FOREIGN KEY (tournament_type_id)
        REFERENCES TournamentTypes (tournament_type_id)
)