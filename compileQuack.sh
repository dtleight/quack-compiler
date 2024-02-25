python duck.py
fsharpc runCompiler.fs
fsharpc --target:library llvmir.fs
fsharpc --target:library quack-typecheck.fs -r quack_lex.dll -r quackparser.dll -r RuntimeParser.dll -r absLexer.dll
 fsharpc --target:library quack-types.fs -r quack_lex.dll -r quackparser.dll -r RuntimeParser.dll -r absLexer.dll -r llvmir.dll
fsharpc --target:library quack-library-linker.fs  -r llvmir.dll -r quack-typecheck.dll -r quackparser.dll -r RuntimeParser.dll -r quack-compile.dll -r absLexer.dll -r quack_lex.dll 
fsharpc --target:library quack-compile.fs -r quack_lex.dll -r quack-typecheck.dll -r llvmir.dll -r quack-library-linker.dll -r quack-library-linker.dll
fsharpc runCompiler.fs
make GRAMMAR=quack MAIN=quackmain
