FROM mcr.microsoft.com/dotnet/sdk:3.1
COPY /RabbitMQSender /RabbitSrc
RUN mkdir RabbitApp
WORKDIR /RabbitSrc
RUN dotnet restore 
RUN dotnet build -c Release
WORKDIR /RabbitApp
RUN cp -a /RabbitSrc/bin/Release/netcoreapp3.1/. .
ENTRYPOINT [ "dotnet", "RabbitMQSender.dll" ]