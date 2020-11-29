FROM mcr.microsoft.com/dotnet/sdk:3.1
COPY /RabbitMQLogger /RabbitSrc/RabbitMQLogger
RUN mkdir RabbitApp
COPY /DataStuff /RabbitSrc/DataStuff
WORKDIR /RabbitSrc/RabbitMQLogger
RUN dotnet restore "RabbitMQLogger.csproj"
RUN dotnet build "RabbitMQLogger.csproj" -c Release
WORKDIR /RabbitApp
RUN cp -a /RabbitSrc/RabbitMQLogger/bin/Release/netcoreapp3.1/. .
ENTRYPOINT [ "dotnet", "RabbitMQLogger.dll" ]