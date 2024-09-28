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
Агент находится на той же машине что и микросервис Dialogs. Устанавливается в [докерфайле](https://github.com/npctheory/highload-monitoring/blob/main/server/Dialogs.Api/Dockerfile). Сервер и Веб-интерфейс подключаются в docker-compose.yml. БД в pg_master.  
Веб-интерфейс доступен по адресу [http://localhost:10054/](http://localhost:10054/).  
Как зайти в веб-интерфейс Zabbix:  

[zabbix.webm](https://github.com/user-attachments/assets/63c954db-91e3-4cc4-9046-1db918f5b6c5)


### Prometheus-Grafana  
Из микросервиса Dialogs в формате OpenTelemetry экспортируются метрики в Promehteus. Как подключается экспортер: [Dialogs.Api/Program.cs](https://github.com/npctheory/highload-monitoring/blob/main/server/Dialogs.Api/Program.cs):  
```csharp
builder.Services.AddOpenTelemetry()
    .WithMetrics(x =>
    {
        x.AddPrometheusExporter();
        
        x.AddRuntimeInstrumentation();
        x.AddProcessInstrumentation();

        x.AddMeter(
            "Microsoft.AspNetCore.Hosting",
            "Microsoft.AspNetCore.Server.Kestrel");

        x.AddView("request-duration", new ExplicitBucketHistogramConfiguration
        {
            Boundaries = new double[] { 0, 0.005, 0.01, 0.025, 0.05, 0.075, 0.1, 0.25, 0.5, 0.75, 1, 2.5, 5, 7.5, 10 }
        });
    });
```
Prometheus и Grafana подключаются в docker-compose.yml.  
Контейнер Grafana доступен по адресу [http://localhost:3000]([url](http://localhost:3000)).  
В Grafana импортируются готовые дата-сорс и дашборд.  
Как запустить дашборд Grafana:  

[prometheusgrafana.webm](https://github.com/user-attachments/assets/1c92ace5-d681-4758-875a-88632ae5192c)

