# run with make   GRAMMAR=grammarname   such as GRAMMAR=test1

# Configuration is for mono on linux-like environments

GRAMMAR = _NO_GRAMMAR_GIVEN_
FUSSLESS = ./
TARGETDIR = ./
FSC = fsharpc
CSC = mcs
LEX = $(FUSSLESS)lex.exe
RUSTLR = rustlr
#RUSTLR = ../target/release/rustlr.exe
RUSTLROPTIONS = -genlex
#MAIN = $(GRAMMAR)main

ifdef MAIN
$(MAIN).exe : $(MAIN).fs $(GRAMMAR)_lex.dll $(GRAMMAR)parser.dll
	$(FSC) --target:library quack-typecheck.fs /r:$(GRAMMAR)_lex.dll /r:$(GRAMMAR)parser.dll /r:$(FUSSLESS)RuntimeParser.dll /r:$(FUSSLESS)absLexer.dll
	$(FSC) $(MAIN).fs /r:$(GRAMMAR)_lex.dll /r:$(GRAMMAR)parser.dll /r:$(FUSSLESS)RuntimeParser.dll /r:$(FUSSLESS)absLexer.dll /r:quack-typecheck.dll$(ADDITIONAL)
endif

$(GRAMMAR)_lex.dll $(GRAMMAR)parser.dll : $(GRAMMAR)_lex.cs $(GRAMMAR)parser.fs
	$(CSC) /t:library $(GRAMMAR)_lex.cs /r:$(FUSSLESS)absLexer.dll $(CSADDITIONAL)
	$(FSC) -a $(GRAMMAR)parser.fs /r:$(FUSSLESS)RuntimeParser.dll /r:$(FUSSLESS)absLexer.dll $(ADDITIONAL)

$(GRAMMAR)_lex.cs : $(GRAMMAR).lex
	$(LEX) $(GRAMMAR).lex	

$(GRAMMAR).lex $(GRAMMAR)parser.fs : $(GRAMMAR).grammar
	$(RUSTLR) $(GRAMMAR).grammar -fsharp $(RUSTLROPTIONS)


clean:
	rm -f $(GRAMMAR).lex $(GRAMMAR)parser.fs $(GRAMMAR)_lex.cs $(GRAMMAR)*.dll
# note: clean will delete everything built
