CREATE TABLE TournamentPrizes (
    tournament_prize_id int(11) NOT NULL AUTO_INCREMENT,
    tournament_id       int(11) NOT NULL,
    prize_id            int(11) NOT NULL,
    place_id            int(11) NOT NULL,
    PRIMARY KEY (tournament_prize_id),
    FOREIGN KEY (tournament_id)
        REFERENCES Tournaments (tournament_id),
    FOREIGN KEY (prize_id)
        REFERENCES Prizes (prize_id)
);
