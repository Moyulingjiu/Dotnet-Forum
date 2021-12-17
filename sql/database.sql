CREATE DATABASE IF NOT EXISTS dotnet_forum DEFAULT CHARSET utf8 COLLATE utf8_general_ci;

CREATE USER 'dbuser'@'localhost' IDENTIFIED BY '12345678';
CREATE USER 'dbuser'@'%' IDENTIFIED BY '12345678';

GRANT ALL ON dotnet_forum.* TO 'dbuser'@'localhost';
GRANT ALL ON dotnet_forum.* TO 'dbuser'@'%';

FLUSH PRIVILEGES;