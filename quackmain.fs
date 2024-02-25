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
//open QuackTypes

// create parser
let parser1 = make_parser();
// create compiler
let quack_compiler = {
  LLVMCompiler.symbol_table = symbol_table;
  program = prog;
  gindex = -1;
  lindex = 0;
};;
// Create main function
let main_function = { 
  name = "main";
  formal_args = Vec<(LLVMtype*string)>();
  return_type = Basic("i32");
  body = Vec<BasicBlock>();
  attributes = Vec<string>();
  bblocator = HashMap<string,int>();
 };;

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

let args = System.Environment.GetCommandLineArgs()
let resulted = 
  match args.Length with
    | 2 -> runFileBased(args.[1])
    | _ -> runInteractive();

//Handle parser errors
if parser1.err_occurred then
  printfn "%A" parser1.NextToken
  printfn "Program did not parse, error at line %d, column %d" parser1.line parser1.column
  failwith "";
let result = 
  match resulted with
    | Some(x) -> x
    | _ -> Label("Program did not parse");;
 

 
 //printfn "--------------------";;
 //printfn "AST Representation";;
 //printfn "--------------------";;
 //printfn "%s" (result.ToString());;
 printfn "--------------------";;
 printfn "Symbol Table";;
 printfn "--------------------";;
 symbol_table.init_frame(result);;
 symbol_table.infer_type(result,false) |> ignore;;
 symbol_table.calculate_closure();
 symbol_table.print_vars();
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
