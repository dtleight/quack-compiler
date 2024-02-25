open System
open System.IO
open System.Collections.Generic;
open Option;
open Fussless;
open RuntimeParser;
open Quack;

let parser1 = make_parser();
let args = System.Environment.GetCommandLineArgs()

let runFileBased(filename) = 
  let lines = File.ReadAllText(filename)
  let lexer1 = quacklexer<unit>(lines);
  parse_with(parser1,lexer1);
  

let resulted = runFileBased(args.[1]) 


printfn "%A" resulted
