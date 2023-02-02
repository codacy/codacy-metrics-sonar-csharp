BUILD_CMD=dotnet build --no-restore /property:GenerateFullPaths=true

all: configure build

configure:
	dotnet restore

build-all:
	$(BUILD_CMD)

build: src/Analyzer

build:
	$(BUILD_CMD) $^

build-seed:
	$(BUILD_CMD) src/Seed

publish:
	dotnet publish -c Release -f net6

run-tests:
	dotnet test

clean:
	rm -rf packages
