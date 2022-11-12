module QuackCompiler
open System
open System.IO
open System.Collections.Generic;
open Option;
open QuackTypechecker;
open LLVMIR
open Quack;

type LLVMCompiler = {
  symbol_table: SymbolTable;
  program: LLVMprogram;
  mutable gindex: int;
  mutable lindex: int;
} with
  member this.translate_type(typeExpr: lltype) =
    match typeExpr with
     | LLint -> Basic("i32")
     | LLfloat -> Basic("f64")
     | _ -> Basic("Terrible things happening")

  member this.uniqueID(prefix:string) =
    sprintf "%s%d_%d" prefix this.gindex this.lindex
  
  member this.add_basic_block(func: LLVMFunction, lbl: string, preds: Vec<BasicBlock>) =
    this.gindex <- this.gindex + 1
    func.body.Add(
      {
        label = lbl;
        body = Vec<Instruction>();
        predecessors = preds;
        ssamap = HashMap<string,string>();
      }
    )

  member this.add_to_body(instruction: Instruction, func: LLVMFunction) = 
      this.lindex <- this.lindex + 1
      func.body.[this.gindex].body.Add(instruction)
    
  member this.compile_expression(expr: expr, func: LLVMFunction) =
    match expr with
      | Integer(x) -> Iconst(x) 
      | Floatpt(x) -> Fconst(x) 
      | Strlit(x) -> Sconst(x)
      | Binop(op,a,b) ->
        let compiledA = this.compile_expression(a, func)
        let compiledB = this.compile_expression(b, func)
        let regName = this.uniqueID("R")
        this.add_to_body(Binaryop (regName, op, this.translate_type(LLint), compiledA, compiledB), func)
        Register(regName)
      | Uniop(op,a) ->
        let compiledA = this.compile_expression(a, func)
        let regName = this.uniqueID("R")
        this.add_to_body(Unaryop (regName, op, None, this.translate_type(LLint), compiledA), func)
        Register(regName)  
      | TypedDefine(varbox, expr) -> 
        let (varType,varName) = varbox.value
        let someType = match this.symbol_table.get_entry(varName) with
          | Some(x) -> x.typeof
          | None -> LLunknown
        let expType = this.translate_type(someType)
        let compiledExpr = this.compile_expression(expr,func)
        this.add_to_body(Alloca(varName, expType, None),func)
        this.add_to_body(Store(expType,compiledExpr, Register(varName), None),func)  
        Register(varName)
      | Var(varName) ->
        let result_type = match this.symbol_table.get_entry(varName.value) with
          | Some(x) -> x.typeof
          | None -> LLunknown
        //Load instruction
        let newReg = this.uniqueID("R");
        this.add_to_body(Load(newReg, this.translate_type(result_type), Register(newReg),None),func)
        //Return register you loaded to
        Register(newReg)
      | CodeBlock(exprs) -> 
        for newexprs in exprs do 
          this.compile_expression(newexprs.value,func)
        Register("r1")
      | Ifelse(conditionBox,a,b) ->
        let condition = this.compile_expression(conditionBox.value,func)
        let creg = this.uniqueID("R");
        //Downcast condition to a boolean
        this.add_to_body(Cast(creg,"trunc", Basic("i32"), condition, Basic("i1")),func)
        //Generate phi labels
        let label1 = this.uniqueID("IfTrue")
        let label0 = this.uniqueID("IfFalse")
        let endlabel = this.uniqueID("EndIf")
        this.add_to_body(Bri1(Register(creg),label1,label0),func)
        let predVector = new Vec<BasicBlock>()
        predVector.Add(func.body.[this.gindex])
        //Create if block
        this.add_basic_block(func, label1,predVector)
        let compiled_a = this.compile_expression(a,func);
        //Create else block
        this.add_basic_block(func, label0, predVector)
        let compiled_b = this.compile_expression(b,func);
        //Create post block - results of if statement
        this.add_basic_block(func, label0, new Vec<BasicBlock>())
        let compiled_b = this.compile_expression(b,func);
        
        //Get label for basic block so that we can phi from it
        
        Iconst(3);//This should return a register or something to phi to
      | _ -> Iconst(3)
