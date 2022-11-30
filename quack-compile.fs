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
  member this.translate_type(typeExpr: lltype, ?value:expr) =
    match typeExpr with
     | LLint -> Basic("i32")
     | LLfloat -> Basic("f64")
     | LLstring -> 
      let strValue = match value with
        | Some(Strlit(x)) -> x
        | _ -> "Bad things happening"
      Array_t(strValue.Replace("/n","x").Replace("\0a", "x").Replace("\00","x").Length-1, Basic("i8"))
<<<<<<< HEAD
     | LLvar(name) -> Basic("i32")
     | LLpointer(pType) -> Pointer(this.translate_type(pType))
     | LLfun(args,rType) -> Func_t((args |> List.map(fun x -> this.translate_type(x))),this.translate_type(rType))
=======
>>>>>>> 76b5f934c56805a26126dc6b147cfd0769834802
     | _ -> Basic("Terrible things happening")
     
  member this.translate_op(op:string) =
   match op with
    //Math Operators
    | "+" -> "add"
    | "-" -> "sub"
<<<<<<< HEAD
    | "*" -> "mul"
=======
    | "*" -> "mult"
>>>>>>> 76b5f934c56805a26126dc6b147cfd0769834802
    | "%" -> "srem"
    //Boolean Operators
    | "and" -> "and"
    | "or" -> "or"
    //Comparison Operators
    | "=" -> "eq"
    | ">" -> "sgt"
    | "<" -> "slt"
    | "<=" -> "sle"
    | ">=" -> "sge"
    | "!=" -> "ne"
    //Default Case
    | _ -> "op_not_defined"

  member this.get_type(var: string) = 
    match this.symbol_table.get_entry(var) with
      | Some(x) -> this.translate_type(x.typeof)
      | None -> Basic("void")
<<<<<<< HEAD
  
  member this.vectorMap(l: 'a list, f: 'a->'b) =
    let rVec = Vec<'b>();
    for x in l do
      rVec.Add(f(x))
    rVec;
=======
>>>>>>> 76b5f934c56805a26126dc6b147cfd0769834802
     
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
 
  member this.add_global_const(declaration:LLVMdeclaration) =
      this.program.global_declarations.Add(declaration)
<<<<<<< HEAD
  
  member this.add_function(fName: string, f_args: Vec<(LLVMtype*string)>, rType: LLVMtype ) = 
      let newFunc = 
        { 
          name = fName;
          formal_args = f_args;
          return_type = rType;
          body = Vec<BasicBlock>();
          attributes = Vec<string>();
          bblocator = HashMap<string,int>();
        };
      this.program.functions.Add(newFunc);
      this.add_basic_block(newFunc, (sprintf "%smain" fName), Vec<BasicBlock>())
      newFunc;
=======
>>>>>>> 76b5f934c56805a26126dc6b147cfd0769834802
    
  member this.compile_expression(expr: expr, func: LLVMFunction) =
    match expr with
      | Integer(x) -> Iconst(x) 
      | Floatpt(x) -> Fconst(x) 
      | Strlit(x) -> 
<<<<<<< HEAD
        let mutable trimString = x.[1..x.Length-2] //Trim end characters
        trimString <- trimString + "\00"
        Sconst(trimString.Replace("/n", "\0a")) //This should return a pointer
      | Binop("=",a,b) -> //Special case- Destructive Assignment
=======
        // -/0a is one character, translate /n-which is /n/n to /0a
        let mutable trimString = x.[1..x.Length-2] //Trim end characters
        trimString <- trimString + "\00"
        Sconst(trimString.Replace("/n", "\0a")) 
      | Binop("=",a,b) -> //Destructive Assignment
>>>>>>> 76b5f934c56805a26126dc6b147cfd0769834802
        match a with
          | Var(a) -> //You could just compile a here theoretically
            let compiledExpr = this.compile_expression(b,func)
            this.add_to_body(Store(this.get_type(a.value),compiledExpr, Register(a.value), None),func)  
            Register(a.value)
          | _ -> Register("BAD THINGS HAPPENED") 
      | Binop(op,a,b) -> //Math operations, comparisons and boolean operations
        let compiledA = this.compile_expression(a,func) //Handle ands seperately
        let compiledB = this.compile_expression(b,func)
        let mutable regName = this.uniqueID("R")
        let currReg = regName //Needed for additional casts
        let rType = this.translate_type(LLint)
        match (op) with
        | ">" | "<" | ">=" | "<=" | "eq" | "!=" -> //Comparison operators 
          this.add_to_body(Icmp(regName, this.translate_op(op), rType, compiledA, compiledB),func)//cast back to a i32 
          regName <- this.uniqueID("R");
          this.add_to_body(Cast(regName,"zext", Basic("i1"), Register(currReg), Basic("i32")),func)
        | _ -> //Numeric and boolean operators
          this.add_to_body(Binaryop(regName, this.translate_op(op), rType, compiledA, compiledB), func) 
<<<<<<< HEAD
        Register(regName)
=======
        Register(regName) //Return the register the function loaded to
      | Uniop("print",a) ->
        let mutable compiledA = this.compile_expression(a,func)
        //Check type of A
        let mutable aType = this.translate_type(symbol_table.infer_type(a),a)
        let printType = match aType with
          | Basic("i32") -> "lambda7c_printint"
          | _ -> "lambda7c_printstr"
        if printType = "lambda7c_printstr" then //String-specific
          let aReg = this.uniqueID("A")
          let aVar = this.uniqueID("SVAR")
          this.add_global_const(Globalconst(aVar,aType,compiledA,Some("align 1")));
          compiledA <- Register(aReg)
          this.add_to_body(Structfield(aReg, aType,Global(aVar),Iconst(0)),func)
          aType <- Pointer(Basic("i8"))
        this.add_to_body(Call(None, Basic("void"), [aType], printType, [(aType,compiledA)]),func)
        Register("printreg");
>>>>>>> 76b5f934c56805a26126dc6b147cfd0769834802
      | Uniop(op,a) ->
        let compiledA = this.compile_expression(a, func)
        let regName = this.uniqueID("R")
        this.add_to_body(Unaryop (regName, op, None, this.translate_type(LLint), compiledA), func)
        Register(regName)  
      | TypedDefine(varbox, expr) -> 
<<<<<<< HEAD
        let (varType,varName) = varbox.value //This should be unique at some point based on gindex
        let expType = this.get_type(varName)
        //let compiledExpr = this.compile_expression(expr,func)
        match expType with
          | Array_t(_,_) ->
            let compiledExpr = this.compile_expression(expr,func)
            //let aReg = this.uniqueID("str_")
            this.add_global_const(Globalconst(varName,expType,compiledExpr,Some("align 1")));
          | Func_t(args,rType) -> //Add function to program.
            let line,column = varbox.line,varbox.column
            match expr with
              | TypedLambda(largs, lrtype, lexp) -> 
                let funFrame = symbol_table.locate_frame(lexp.line,lexp.column)
                //Make formal arguments useful
                let fargs = this.vectorMap(largs, fun (x,y) -> (this.translate_type(x), sprintf "farg_%s_%d" y this.gindex))
                //Add closure arguments into formal arguments
                for closureVar in funFrame.closure do
                  let (cvar,(_,ctype)) = closureVar.Key, closureVar.Value;
                  fargs.Add((Pointer(this.translate_type(ctype)), cvar));
                //Store current indexes
                let tempL = this.lindex;
                let tempG = this.gindex;
                this.lindex <- 0;
                this.gindex <- -1;
                let newFunction = this.add_function(varName,fargs, rType)
                let rreg = this.compile_expression(expr,newFunction);
                //Add an instruction to return the register - Implicit returns
                this.add_to_body(Ret(rType, rreg),newFunction);
                this.lindex <- tempL
                this.gindex <- tempG
              | _ -> printfn "Error in compiler generation"
          | _ -> 
            let compiledExpr = this.compile_expression(expr,func)
            this.add_to_body(Alloca(varName, expType, None),func)
            //Add gindex here --SSA is a nightmare
            this.add_to_body(Store(expType,compiledExpr, Register(varName), None),func)  
        Register(varName) // -> Should be edest
      | Var(varName) ->
        let newReg = this.uniqueID("R");
        this.add_to_body(Load(newReg, this.get_type(varName.value), Register(varName.value),None),func)
=======
        let (varType,varName) = varbox.value
        let expType = this.get_type(varName)
        let compiledExpr = this.compile_expression(expr,func)
        this.add_to_body(Alloca(varName, expType, None),func)
        //Add gindex here --SSA is a nightmare
        this.add_to_body(Store(expType,compiledExpr, Register(varName), None),func)  
        Register(varName) // -> Should be edest
      | Var(varName) ->
        //Load instruction
        let newReg = this.uniqueID("R");
        this.add_to_body(Load(newReg, this.get_type(varName.value), Register(varName.value),None),func)
        //Return register you loaded to
>>>>>>> 76b5f934c56805a26126dc6b147cfd0769834802
        Register(newReg)
      | TypedLambda(_, _, expr) -> this.compile_expression(expr.value,func)
      | CodeBlock(exprs) -> 
        //This is its own code, you could create BB's here
        for newexprs in exprs do 
          this.compile_expression(newexprs.value,func)
        Register("codeblock-reg")
<<<<<<< HEAD
      | Apply(box,[boxexpr]) when box.value = "print" -> //Change this to an apply
        let (a) = boxexpr.value
        let mutable compiledA = this.compile_expression(a,func)
        //Check type of A
        let mutable aType = this.translate_type(symbol_table.infer_type(a),a)
        let printType = match aType with
          | Basic("i32") -> "lambda7c_printint"
          | _ -> "lambda7c_printstr"
        if printType = "lambda7c_printstr" then //String-specific
          let aReg = this.uniqueID("A")
          let aVar = this.uniqueID("SVAR")
          this.add_global_const(Globalconst(aVar,aType,compiledA,Some("align 1")));
          compiledA <- Register(aReg)
          this.add_to_body(Structfield(aReg, aType,Global(aVar),Iconst(0)),func)
          aType <- Pointer(Basic("i8"))
        this.add_to_body(Call(None, Basic("void"), [aType], printType, [(aType,compiledA)]),func)
        Register("printreg");
      | Apply(varbox,args) ->
        let funName = varbox.value;
        let line,column = varbox.line,varbox.column;
        let (args,rType) = match symbol_table.get_entry(funName) with
          | Some(x) ->  match x.typeof with
            | LLfun(fargs,frType) -> 
              ((fargs |> List.map(fun x -> this.translate_type(x)),this.translate_type(frType)))
            | _ -> ([Basic("void")],Basic("void"))
          | None -> ([Basic("void")],Basic("void"))
        this.add_to_body(Call(None, rType, args, funName, []),func)
        Iconst(3)
        
       | Ifelse(conditionBox,a,b) ->
=======
      | Ifelse(conditionBox,a,b) ->
>>>>>>> 76b5f934c56805a26126dc6b147cfd0769834802
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
        this.add_to_body(Br_uc(endlabel),func)
        //Create else block
        this.add_basic_block(func, label0, predVector)
        let compiled_b = this.compile_expression(b,func);
        this.add_to_body(Br_uc(endlabel),func)
        //Create post block - results of if statement
        this.add_basic_block(func, endlabel, new Vec<BasicBlock>())
        Iconst(3);//This should return a register or something to phi to
      | Whileloop(conditionBox,a) ->
        //This logic could go in its own basic block for easy condition reuse.
        let condition = this.compile_expression(conditionBox.value,func)
        let creg = this.uniqueID("R");
        //Downcast condition to a boolean
        this.add_to_body(Cast(creg,"trunc", Basic("i32"), condition, Basic("i1")),func)
        //Generate phi labels
        let label1 = this.uniqueID("WhileBody")
        let endLabel = this.uniqueID("WhileEnd")
        this.add_to_body(Bri1(Register(creg),label1,endLabel),func)
        let predVector = new Vec<BasicBlock>()
        predVector.Add(func.body.[this.gindex])
        //Create while block
        this.add_basic_block(func, label1,predVector)
        let compiled_a = this.compile_expression(a,func);
        //Go back through this logic, it recompiles the condition
        let condition2 = this.compile_expression(conditionBox.value,func)
        let creg2 = this.uniqueID("R");
        this.add_to_body(Cast(creg2,"trunc", Basic("i32"), condition2, Basic("i1")),func)
        this.add_to_body(Bri1(Register(creg2),label1,endLabel),func)
        //Create post block
        this.add_basic_block(func, endLabel, new Vec<BasicBlock>())
        Iconst(3)
      | _ -> Iconst(3)
