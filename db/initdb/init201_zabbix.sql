CREATE USER zabbix WITH ENCRYPTED PASSWORD 'zabbix_pass';
CREATE DATABASE zabbix OWNER zabbix;
GRANT ALL PRIVILEGES ON DATABASE zabbix TO zabbix;