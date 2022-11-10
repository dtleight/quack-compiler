module Quack
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

// create parser
let parser1 = make_parser();

let runInteractive() = 
  Console.Write("Enter Expression: ");
  let lexer1 = quacklexer<unit>(Console.ReadLine());
  parse_with(parser1,lexer1);;

let runFileBased(filename) = 
  let lines = File.ReadAllText(filename)
  let lexer1 = quacklexer<unit>(lines);
  parse_with(parser1,lexer1);;
// create lexer

let args = System.Environment.GetCommandLineArgs()
let resulted = 
  match args.Length with
    | 2 -> runFileBased(args.[1])
    | _ -> runInteractive();

let result = 
  match resulted with
    | Some(x) -> x
    | _ -> Label("Program did not parse");;
    
let quack_compiler = {
  LLVMCompiler.symbol_table = symbol_table;
  program = prog;
  gindex = -1;
  lindex = 0;
};;

let f_args = Vec<(LLVMtype*string)>()
let b = Vec<BasicBlock>()
let attrs = Vec<string>()

let main_function = { 
  name = "main";
  formal_args = f_args;
  return_type = Basic("void");
  body = b;
  attributes = attrs;
 };;
 

 
 printfn "--------------------";;
 printfn "AST Representation";;
 printfn "--------------------";;
 printfn "%s" (result.ToString());;
 printfn "--------------------";;
 printfn "Type Checking";;
 printfn "--------------------";;
 symbol_table.infer_type(result,false) |> ignore;;
 printfn "--------------------";;
 printfn "Code Generation";;
 printfn "--------------------";;
 quack_compiler.add_basic_block(main_function);
 quack_compiler.program.functions.Add(main_function);
 quack_compiler.compile_expression(result,main_function) |> ignore;
 printfn "%s" (programToString(quack_compiler.program));;
