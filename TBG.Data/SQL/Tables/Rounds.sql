CREATE TABLE Rounds (
    round_id        int(11) NOT NULL AUTO_INCREMENT,
    tournament_id   int(11) NOT NULL,
    round_num       int NOT NULL,
    PRIMARY KEY (round_id),
    FOREIGN KEY (tournament_id)
        REFERENCES Tournaments (tournament_id)
		ON DELETE CASCADE
);
