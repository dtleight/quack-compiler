module Quack
open System
open System.IO
open System.Collections.Generic;
open Option;
open Fussless
open RuntimeParser
open QuackTypechecker
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
  
  printfn "%s" (result.ToString());;
  printfn "--------------------";;
  printfn "Type Checking";;
  printfn "--------------------";;
 symbol_table.infer_type(result,true);;
 //printfn "%A" symbol_table
