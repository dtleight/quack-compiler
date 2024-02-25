# quack-compiler
This is a compiler written primarily in F# for my CSC 124 class at Hofstra. Quack as a programming language is built to closely emulate the syntax of languages like Java.

## Instructions and Operating the Compiler
The quack compiler has several scripts to increase the ease of use and developer efficiency when working with the compiler

To compile all of the files for the compiler, utilze the following command.
```shell
./compileQuack
```

To run the compiler on a given file, use the command. This automatically runs the sequence defined in manual compilation. 
```shell
./runCompiler [Path to .quack file]
```

### Manual Compilation
In order to debug issues in the compilation stage, the following manual process can be run for any stage of compilation

## Abstract Syntax Trees
The grammar for the abstract syntax tree is defined in quack.grammar. The following cases are defined.

### Primitive Values and Types
  The grammar defines the following 4 primitive types
  * Integers (LLint)
  * Floatpt (LLfloat)
  * LLstring (LLstring)
  * ListLiteral (LLlist)
  * Nil (LLunit)
### Compound Values and Types
  * Generic Type (lltype)
  * Pointers (llpointer)
  * Tuples (LLtuple of lltype)
### Arithmetic and Processing Operations
  The abstract syntax defines the following arithmetic operations
  * Uniop
  * Binop
  These operations are denoted by a symbol identifying the operation to perform. 
  * Unary Operations
   - (-) Negation
   - (print) Print function 
   - (not) Binary negation
   - (car) List deconstruction item
   - (cdr) List deconstruction remainder
   - (~) Can't remember 
### Functions
#### Declarations
  Functions show up in the abstract syntax as either
  - TypedLambda or Lambda (LLfun)
  During the typecheck stage, Lambdas get converted into typed lambdas through type inference. Function application takes place through the use of the operator. The type of lambda , ```LLFun``` is further specified by it's argument and return types. The full type declaration of a LLFun type looks like
  ``` LLfun of (lltype list)*lltype ```
with the type list being the list of arguments and the final lltype being the return type of the function.
#### Application
  Functions are called in the abstract syntax through the Apply operation.

