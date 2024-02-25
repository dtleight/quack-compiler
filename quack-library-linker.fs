module QuackLibraryLinker
open System
open System.IO
open System.Collections.Generic;
open Option;
open Fussless
open RuntimeParser
open QuackTypechecker
open QuackCompiler
open LLVMIR
open Quack

let parser1 = make_parser();

let cProg = 
  {
    preamble = "";
    global_declarations = Vec<LLVMdeclaration>();
    functions = Vec<LLVMFunction>();
    postamble = ""
  };;
  
let quack_compiler = {
  LLVMCompiler.symbol_table = symbol_table;
  program = prog;
  gindex = -1;
  lindex = 0;
};;



let compileLibrary(prog,filename) = 
  let lines = File.ReadAllText(filename)
  let lexer1 = quacklexer<unit>(lines);
  let resulted = parse_with(parser1,lexer1);
  // Create main function
  let main_function2 = { 
    name = sprintf "%s Initializer Constructor" "Test";
    formal_args = Vec<(LLVMtype*string)>();
    return_type = Basic("i32");
    body = Vec<BasicBlock>();
    attributes = Vec<string>();
    bblocator = HashMap<string,int>();
  }
  let result = match resulted with
    | Some(x) -> x
    | None -> Integer(3)
  printfn "--------------------"
  printfn "Library Symbol Table"
  printfn "--------------------"
  symbol_table.infer_type(result,false) |> ignore
  symbol_table.calculate_closure()
  symbol_table.print_vars();
  printfn "--------------------"
  printfn "Library Code Generation"
  printfn "--------------------"
  quack_compiler.add_basic_block(main_function2, "initConstructor", Vec<BasicBlock>());
  quack_compiler.program.functions.Add(main_function2);
  quack_compiler.compile_expression(result,main_function2) |> ignore;
  quack_compiler.add_to_body(Ret(Basic("i32"), Iconst(0)),main_function2)
  let output = programToString(quack_compiler.program)
  output;;

