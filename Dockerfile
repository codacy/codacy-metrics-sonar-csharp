FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS builder

RUN echo "deb http://download.mono-project.com/repo/debian wheezy main" | tee /etc/apt/sources.list.d/mono-xamarin.list && \
	apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF && \
	apt-get update && \
	apt-get install -y mono-complete build-essential nuget unzip

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
