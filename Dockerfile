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

FROM ubuntu:20.04

ENV MONO_VERSION 6.10.0.104

RUN apt-get update && \
    apt-get install -y --no-install-recommends gnupg dirmngr && \
    rm -rf /var/lib/apt/lists/* && \
    export GNUPGHOME="$(mktemp -d)" && \
    gpg --batch --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF && \
    gpg --batch --export --armor 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF > /etc/apt/trusted.gpg.d/mono.gpg.asc && \
    gpgconf --kill all && \
    rm -rf "$GNUPGHOME" && \
    apt-key list | grep Xamarin && \
    apt-get purge -y --auto-remove gnupg dirmngr

RUN echo "deb http://download.mono-project.com/repo/ubuntu stable-focal/snapshots/$MONO_VERSION main" > /etc/apt/sources.list.d/mono-official-stable.list && \
    apt-get update && \
    apt-get install -y mono-runtime && \
    rm -rf /var/lib/apt/lists/* /tmp/*

COPY --from=builder /workdir/src/Analyzer/bin/Release/net461/publish/*dll /opt/docker/bin/
COPY --from=builder /workdir/src/Analyzer/bin/Release/net461/publish/*.exe /opt/docker/bin/

RUN adduser -u 2004 --disabled-password docker

ENTRYPOINT [ "mono", "/opt/docker/bin/Analyzer.exe" ]
