### О проекте
Домашнее задание по мониторингу.  
Проект состоит из следующих компонентов:  
* Солюшен .NET в папке ./server, который собирается в два образа: core:local и dialogs:local (контейнеры core и dialogs).
* Dockerfile и сид базы данных postgres в папке ./db, который собирается в образ db:local (контейнер pg_master).
* В папке tests находятся запросы для расширения VSCode REST Client и экспорты коллекций и окружений Postman.
* В docker-compose.yml подключаются Redis, Redis Insight, RabbitMQ, PGAdmin, Prometheus, Grafana, сервер Zabbix, Web-GUI Zabbix.
### Начало работы
Склонировать проект, сделать cd в корень репозитория и запустить Docker Compose.  
Дождаться статуса healthy на контейнерах postgres.  
```bash
https://github.com/npctheory/highload-monitoring.git
cd highload-monitoring
docker compose up --build -d
```
### Zabbix  

### Prometheus-Grafana
