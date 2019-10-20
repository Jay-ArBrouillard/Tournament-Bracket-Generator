CREATE TABLE Prizes (
    prize_id        int(11) NOT NULL AUTO_INCREMENT,
    prize_name      varchar(50) NOT NULL,
    prize_amount    decimal NOT NULL,
    prize_percent   double NOT NULL,
    PRIMARY KEY (prize_id)
);
