BIN_DIR = bin
RELEASE_DIR = $(BIN_DIR)/Release
DEBUG_DIR = $(BIN_DIR)/Debug

SOURCES = Config.cs IConfig.cs IniConfig.cs
OUT_LIB = "IniConfiguration.dll"

ADD_OPTIONS = 
TARGET = $(RELEASE_DIR)

ifdef MODE
	ifeq ($(MODE), DEBUG)
		ADD_OPTIONS += -debug 
		TARGET = $(DEBUG_DIR)
	endif
endif

$(OUT_LIB): $(SOURCES)
	mkdir -p $(TARGET) && mcs -target:library -out:$(TARGET)/$(OUT_LIB) $(ADD_OPTIONS) $(SOURCES)
	
.PHONY: clean
clean:
	rm -Rf $(BIN_DIR)