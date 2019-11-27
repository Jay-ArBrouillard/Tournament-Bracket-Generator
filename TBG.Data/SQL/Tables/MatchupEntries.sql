CREATE TABLE MatchupEntriesV2 (
	matchup_entry_id			int(11) NOT NULL AUTO_INCREMENT,
	matchup_id					int(11) NOT NULL,
	tournament_entry_id         int(11) NOT NULL,
	score						int(5) NOT NULL DEFAULT '0',
    PRIMARY KEY (matchup_entry_id),
    FOREIGN KEY (matchup_id)
        REFERENCES MatchupsV2 (matchup_id),
    FOREIGN KEY (tournament_entry_id)
        REFERENCES TournamentEntriesV2 (tournament_entry_id)
);
