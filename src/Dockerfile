FROM mcr.microsoft.com/dotnet/core/runtime-deps:3.1

RUN apt-get update && apt-get install -y curl && apt-get clean

WORKDIR /srv
COPY ./publish .
RUN chmod +x RefactorThis.Api

ENTRYPOINT /srv/RefactorThis.Api