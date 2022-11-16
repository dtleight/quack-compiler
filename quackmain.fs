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

let mut inFile = "Quack-Program/test.quack";;

let writeToFile(filename:string, program:string) = 
  File.WriteAllText(filename.Replace(".quack", ".ll").Replace("Quack-Programs","Quack-Programs/Outputs"),program);

let runInteractive() = 
  Console.Write("Enter Expression: ");
  let lexer1 = quacklexer<unit>(Console.ReadLine());
  parse_with(parser1,lexer1);;
  

let runFileBased(filename) = 
  let lines = File.ReadAllText(filename)
  let lexer1 = quacklexer<unit>(lines);
  parse_with(parser1,lexer1);
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
  return_type = Basic("i32");
  body = b;
  attributes = attrs;
  bblocator = HashMap<string,int>();
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
 quack_compiler.add_basic_block(main_function, "mainstart", Vec<BasicBlock>());
 quack_compiler.program.functions.Add(main_function);
 quack_compiler.compile_expression(result,main_function) |> ignore;
 quack_compiler.add_to_body(Ret(Basic("i32"), Iconst(0)),main_function);;
 let output = programToString(quack_compiler.program);;
 printfn "%s" output;;
 if args.Length = 2 then writeToFile(args.[1],output);;
