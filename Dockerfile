FROM mcr.microsoft.com/dotnet/sdk:3.1-alpine3.14 AS builder

COPY . /workdir
WORKDIR /workdir

RUN make
RUN make publish
RUN make xunit

FROM mono:6.10

COPY --from=builder /workdir/src/Analyzer/bin/Release/net461/publish/*dll /opt/docker/bin/
COPY --from=builder /workdir/src/Analyzer/bin/Release/net461/publish/*.exe /opt/docker/bin/

RUN adduser -u 2004 --disabled-password docker

ENTRYPOINT [ "mono", "/opt/docker/bin/Analyzer.exe" ]
