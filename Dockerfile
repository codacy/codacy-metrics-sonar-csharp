ARG MONO_VERSION=6.12.0.122

FROM ubuntu:20.04 as builder

ARG MONO_VERSION
ENV APT_MONO_VERSION=${MONO_VERSION}-0xamarin1+ubuntu2004b1
ENV TZ=Europe/Lisbon
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

# Install complete mono and its dependencies
RUN apt-get -y update && \
    apt install -y gnupg ca-certificates && \
    apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF && \
    echo "deb https://download.mono-project.com/repo/ubuntu stable-focal main" | tee /etc/apt/sources.list.d/mono-official-stable.list && \
    apt-get -y update && \
    apt-get -y install mono-complete=${APT_MONO_VERSION} build-essential nuget unzip

# Install dotnet sdk
RUN apt-get -y update && \
    apt-get install -y apt-transport-https wget make && \
    wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    rm packages-microsoft-prod.deb && \
    apt-get update && \
    apt-get install -y dotnet-sdk-6.0

COPY . /workdir
WORKDIR /workdir

RUN make publish
RUN make run-tests

FROM mono:$MONO_VERSION

COPY --from=builder /workdir/src/Analyzer/bin/Release/net48/publish/Analyzer.exe /opt/docker/bin/
COPY --from=builder /workdir/src/Analyzer/bin/Release/net48/publish/*.dll /opt/docker/bin/
RUN adduser -u 2004 --disabled-password docker

ENTRYPOINT [ "mono", "/opt/docker/bin/Analyzer.exe" ]