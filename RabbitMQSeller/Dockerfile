FROM mcr.microsoft.com/dotnet/sdk:3.1
COPY /RabbitMQSeller /RabbitSrc/RabbitMQSeller
RUN mkdir RabbitApp
COPY /DataStuff /RabbitSrc/DataStuff
WORKDIR /RabbitSrc/RabbitMQSeller
RUN dotnet restore "RabbitMQSeller.csproj"
RUN dotnet build "RabbitMQSeller.csproj" -c Release
WORKDIR /RabbitApp
RUN cp -a /RabbitSrc/RabbitMQSeller/bin/Release/netcoreapp3.1/. .
ENTRYPOINT [ "dotnet", "RabbitMQSeller.dll" ]