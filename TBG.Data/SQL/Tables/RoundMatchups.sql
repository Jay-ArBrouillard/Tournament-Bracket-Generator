CREATE TABLE RoundMatchups (
    round_matchup_id    int(11) NOT NULL AUTO_INCREMENT,
    round_id            int(11) NOT NULL,
    matchup_id          int(11) NOT NULL,
	matchup_number      int(11) NOT NULL,
    PRIMARY KEY (round_matchup_id),
    FOREIGN KEY (round_id)
        REFERENCES Rounds (round_id)
		ON DELETE CASCADE,
    FOREIGN KEY (matchup_id)
        REFERENCES Matchups (matchup_id)
		ON DELETE CASCADE
);
