# made for Mono and GNU make

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
