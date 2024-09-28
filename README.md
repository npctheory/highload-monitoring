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
Настройки агента находится в [докерфайле](https://github.com/npctheory/highload-monitoring/blob/main/server/Dialogs.Api/Dockerfile) Dialogs. БД в pg_master.  
Веб-интерфейс доступен по адресу [http://localhost:10054/](http://localhost:10054/).  



### Prometheus-Grafana  
Настройки телеметрии [Dialogs.Api/Program.cs](https://github.com/npctheory/highload-monitoring/blob/main/server/Dialogs.Api/Program.cs):  
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
Контейнер доступен по адресу [http://localhost:3000]([url](http://localhost:3000)).
