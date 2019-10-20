CREATE TABLE Matchups (
    matchup_id      int(11) NOT NULL AUTO_INCREMENT,
    winner_id       int(11),
    loser_id        int(11),
    PRIMARY KEY (matchup_id)
);
