# Build

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

COPY *.csproj *.sln ./

# mitigation for https://github.com/NuGet/Home/issues/10491
RUN curl -o /usr/local/share/ca-certificates/verisign.crt -SsL https://crt.sh/?d=1039083 && update-ca-certificates
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

# Exec

FROM ubuntu:latest

RUN apt-get update

RUN apt-get install -y wget && apt-get update
RUN wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb && apt-get update

RUN apt-get install -y apt-transport-https \
    dotnet-runtime-5.0 \
    libopus0 \
    libopus-dev \
    opus-tools \
    libsodium-dev \ 
    && apt-get update

RUN apt-get install -y software-properties-common && apt-get update
RUN add-apt-repository universe && apt-get update
RUN apt-get install -y ffmpeg && apt-get update

WORKDIR /app/out
COPY --from=build-env /app/out .

ENTRYPOINT ["./Sirtatji"]