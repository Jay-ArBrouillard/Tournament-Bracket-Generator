CREATE TABLE Users (
    user_id     int(11) NOT NULL AUTO_INCREMENT,
    user_name   varchar(50) NOT NULL,
    password    varchar(50) NOT NULL,
    active      tinyint(1) NOT NULL DEFAULT '0',
    admin       tinyint(1) NOT NULL DEFAULT '0',
    last_login  datetime DEFAULT NULL,
    PRIMARY KEY (user_id)
);
