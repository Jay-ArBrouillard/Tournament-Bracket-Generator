CREATE TABLE TeamMembers (
    person_team_id  int(11) NOT NULL AUTO_INCREMENT,
    team_id         int(11) NOT NULL,
    person_id       int(11) NOT NULL,
    PRIMARY KEY (person_team_id),
    FOREIGN KEY (team_id)
        REFERENCES Teams (team_id),
    FOREIGN KEY (person_id)
        REFERENCES Persons (person_id)
);
