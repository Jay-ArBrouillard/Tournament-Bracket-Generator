CREATE TABLE TournamentEntriesV2 (
    tournament_entry_id     int(11) NOT NULL AUTO_INCREMENT,
    tournament_id           int(11) NOT NULL,
    team_id                 int(11) NOT NULL,
    seed                    decimal(11,10) NOT NULL,
    PRIMARY KEY (tournament_entry_id),
    FOREIGN KEY (tournament_id)
        REFERENCES TournamentsV2 (tournament_id),
    FOREIGN KEY (team_id)
        REFERENCES Teams (team_id)
);
