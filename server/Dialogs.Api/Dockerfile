FROM mcr.microsoft.com/dotnet/sdk:8.0

RUN apt-get update
RUN apt-get install -y wget gnupg lsb-release
RUN wget https://repo.zabbix.com/zabbix/6.4/debian/pool/main/z/zabbix-release/zabbix-release_6.4-1+debian12_all.deb
RUN dpkg -i zabbix-release_6.4-1+debian12_all.deb
RUN apt install zabbix-agent2
COPY ./Dialogs.Api/zabbix-agent/zabbix_agent2.conf /etc/zabbix/zabbix_agent2.conf

WORKDIR /app
COPY . .

ENV PATH="$PATH:/root/.dotnet/tools"
ENV ASPNETCORE_ENVIRONMENT=Production

RUN dotnet restore ./Dialogs.Api/Dialogs.Api.csproj
RUN dotnet publish ./Dialogs.Api/Dialogs.Api.csproj -c Release -o /app/publish

WORKDIR /app/publish

EXPOSE 80 82 10050 5001

ENTRYPOINT ["sh", "-c", "zabbix_agent2 -f & dotnet Dialogs.Api.dll"]
