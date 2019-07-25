export FrameworkPathOverride=$(shell dirname $(shell which mono))/../lib/mono/4.5/
SONAR_VERSION=$(shell cat .SONAR_VERSION | tr -d '\n')
BUILD_CMD=dotnet build --no-restore /property:GenerateFullPaths=true

LIBRARIES_FOLDER=.lib
PACKAGES_FOLDER=.packages
RESOURCE_FOLDER=.res

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

xunit:
	@cd $(shell pwd)/test/Analyzer.Tests/ && dotnet xunit

publish:
	dotnet publish -c Release -f net461

clean:
	rm -rf .lib/ .packages/ .res/
