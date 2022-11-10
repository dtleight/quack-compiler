python duck.py
fsharpc --target:library llvmir.fs
fsharpc --target:library quack-typecheck.fs -r quack_lex.dll -r quackparser.dll -r RuntimeParser.dll -r absLexer.dll
fsharpc --target:library quack-compile.fs -r quack_lex.dll -r quack-typecheck.dll -r llvmir.dll
make GRAMMAR=quack MAIN=quackmain
