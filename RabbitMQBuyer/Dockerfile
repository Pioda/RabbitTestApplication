FROM mcr.microsoft.com/dotnet/sdk:3.1
COPY /RabbitMQBuyer /RabbitSrc/RabbitMQBuyer
RUN mkdir RabbitApp
COPY /DataStuff /RabbitSrc/DataStuff
WORKDIR /RabbitSrc/RabbitMQBuyer
RUN dotnet restore "RabbitMQBuyer.csproj"
RUN dotnet build "RabbitMQBuyer.csproj" -c Release
WORKDIR /RabbitApp
RUN cp -a /RabbitSrc/RabbitMQBuyer/bin/Release/netcoreapp3.1/. .
ENTRYPOINT [ "dotnet", "RabbitMQBuyer.dll" ]