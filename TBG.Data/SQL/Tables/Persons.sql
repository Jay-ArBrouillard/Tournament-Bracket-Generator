CREATE TABLE Persons (
    person_id       int(11) NOT NULL AUTO_INCREMENT,
    first_name      varchar(50) NOT NULL,
    last_name       varchar(50) NOT NULL,
    email           varchar(50) NOT NULL,
    phone           varchar(50) NOT NULL,
    wins            int(5) NOT NULL DEFAULT '0',
    losses          int(5) NOT NULL DEFAULT '0',
    PRIMARY KEY (person_id)
);
