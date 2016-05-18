# Made for Mono and GNU make
# Requirements:
# - Mono is required to be available on PATH.
# - The test target requires the "NUC" parameter that specifies NUnitv2.6.4 console runner. E.g.:
#       make NUC="/path/to/nunit-console.exe" test

MAIN_PROJ = 02-api
TEST_PROJ = 02-api-test

ALL : build clean test
.PHONY : ALL

build :
	xbuild /p:Configuration=Release dpp_du.sln
clean :
	rm -rf "$(MAIN_PROJ)"/bin/ "$(MAIN_PROJ)"/obj/ "$(TEST_PROJ)"/bin/ "$(TEST_PROJ)"/obj/
test :
	mono "${NUC}" "$(TEST_PROJ)/bin/Debug/$(TEST_PROJ).dll"
