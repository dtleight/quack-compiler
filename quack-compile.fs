module QuackCompiler
open System
open System.IO
open System.Collections.Generic;
open Option;
open QuackLibraryLinker;
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
     | LLunit -> Void_t
     | LLint -> Basic("i32")
     | LLfloat -> Basic("double")
     | LLstring -> 
        let strValue = 
          match value with
            | Some(Strlit(x)) -> x
            | _ -> "Bad things happening"
        Array_t(strValue.Replace("/n","x").Replace("\0a", "x").Replace("\00","x").Length-1, Basic("i8"))
     //| LList(lType) -> 
     // let avalue = match expr with
     //   | Some(x) ->""
     //   | _ -> ""
     // Array_t()
     | LLvar(name) -> Basic("i32")
     | LLpointer(pType) -> Pointer(this.translate_type(pType))
     | LLfun(args,rType) -> Func_t((args |> List.map(fun x -> this.translate_type(x))),this.translate_type(rType))
     | _ -> Basic("Terrible things happening")
     
  member this.translate_op(op:string, t: LLVMtype) =
   match op,t with
    //Math Operators
    | ("+",Basic("double")) -> "fadd"
    | ("-",Basic("double")) -> "fsub"
    | ("*",Basic("double")) -> "fmul"
    | ("/",Basic("double")) -> "fdiv"
    | ("%",Basic("double")) -> "frem"
    | ("+",_) -> "add"
    | ("-",_) -> "sub"
    | ("*",_) -> "mul"
    | ("/",_) -> "sdiv"
    | ("%",_) -> "srem"
   
    //Boolean Operators
    | ("and",_) -> "and"
    | ("or",_) -> "or"
    //Comparison Operators
    | ("=",Basic("double")) -> "oeq"
    | (">",Basic("double")) -> "ogt"
    | ("<",Basic("double")) -> "olt"
    | ("<=",Basic("double")) -> "ole"
    | (">=",Basic("double")) -> "oge"
    | ("!=",Basic("double")) -> "one"
    | ("eq",_) -> "eq"
    | (">",_) -> "sgt"
    | ("<",_) -> "slt"
    | ("<=",_) -> "sle"
    | (">=",_) -> "sge"
    | ("!=",_) -> "ne"
    //Default Case
    | _ -> "op_not_defined"

  member this.get_type(var: string) = 
    match this.symbol_table.current_frame.get_entry(var) with
      | Some(x) -> this.translate_type(x.typeof)
      | None -> Basic("void")
      
  member this.get_varname(var:string, frame: table_frame, ?prefix:string) =
    let pre = defaultArg prefix ""
    match frame.get_entry(var) with
      | Some(x) -> sprintf "%s%s_%d" pre var x.gindex
      | None -> "No variable binding exists"
  
  member this.vectorMap(l: 'a list, f: 'a->'b) =
    let rVec = Vec<'b>();
    for x in l do
      rVec.Add(f(x))
    rVec;
     
  member this.uniqueID(prefix:string) =
    sprintf "%s_%d" prefix this.lindex
  
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
    
  member this.compile_expression(expr: expr, func: LLVMFunction) =
    match expr with
      | Integer(x) -> Iconst(x) 
      | Floatpt(x) -> Fconst(x) 
      | Strlit(x) -> 
        let mutable trimString = x.[1..x.Length-2] //Trim end characters
        trimString <- trimString + "\00"
        Sconst(trimString.Replace("/n", "\0a")) //This should return a pointer
      | Binop("=",a,b) -> //Special case- Destructive Assignment
        match a with
          | Var(a) -> //You could just compile a here theoretically
            let varname = this.get_varname(a.value,symbol_table.current_frame)
            let compiledExpr = this.compile_expression(b,func)
            //You need the gindex here as well
            this.add_to_body(Store(this.get_type(a.value),compiledExpr, Register(varname), None),func)  
            compiledExpr
          | _ -> Register("BAD THINGS HAPPENED") 
      | Binop(op,a,b) -> //Math operations, comparisons and boolean operations
        let compiledA = this.compile_expression(a,func) //Handle ands seperately
        let compiledB = this.compile_expression(b,func)
        let aType = symbol_table.infer_type(a);
        let mutable regName = this.uniqueID("R")
        let currReg = regName //Needed for additional casts
        let rType = this.translate_type(aType)
        match (op) with
        | ">" | "<" | ">=" | "<=" | "eq" | "!=" -> //Comparison operators
          match rType with
            | Basic("i32") ->
              this.add_to_body(Icmp(regName, this.translate_op(op,rType), rType, compiledA, compiledB),func)
              regName <- this.uniqueID("R");
              this.add_to_body(Cast(regName,"zext", Basic("i1"), Register(currReg), Basic("i32")),func)
            | Basic("double") ->
              this.add_to_body(Fcmp(regName, this.translate_op(op,rType), rType, compiledA, compiledB),func)
              regName <- this.uniqueID("R");
        | _ -> //Numeric and boolean operators
          this.add_to_body(Binaryop(regName, this.translate_op(op,rType), rType, compiledA, compiledB), func) 
        Register(regName)
      | Uniop(op,a) ->
        let compiledA = this.compile_expression(a, func)
        let regName = this.uniqueID("R")
        let aType = symbol_table.infer_type(a);
        this.add_to_body(Unaryop (regName, op, None, this.translate_type(aType), compiledA), func)
        Register(regName)  
      | TypedDefine(varbox, expr) -> 
        let (varType,varName) = varbox.value
        let expType = this.get_type(varName)
        //Add gindex to variable name
        let newName = this.get_varname(varName,symbol_table.current_frame)
        match expType with
          | Array_t(_,_) ->
            let compiledExpr = this.compile_expression(expr,func)
            this.add_global_const(Globalconst(varName,expType,compiledExpr,Some("align 1")));
          | Func_t(args,rType) -> //Add function to program.
            let line,column = varbox.line,varbox.column
            match expr with
              | TypedLambda(largs, lrtype, lexp) -> 
                let funFrame = symbol_table.locate_frame(lexp.line,lexp.column)
                let fargs = this.vectorMap(largs, fun (x,y) -> (this.translate_type(x), (this.get_varname(y,funFrame,"farg_"))))
                let cargs = Vec<LLVMtype*string>();
                for farg in fargs do
                  cargs.Add(farg);
                //Add closure arguments into formal arguments
                for closureVar in funFrame.closure do
                  let ((cvar,cindex),ctype) = closureVar.Key, closureVar.Value;
                  cargs.Add((Pointer(this.translate_type(ctype)), sprintf "%s_%d" cvar cindex));
                //Store current indexes
                let tempL = this.lindex;
                let tempG = this.gindex;
                let tempFrame = symbol_table.current_frame
                this.lindex <- 0;
                this.gindex <- -1;
                symbol_table.current_frame <- funFrame
                let newFunction = this.add_function(varName,cargs, rType)
                //Add variable bindings
                for larg in largs do
                  let (ltype,lname) = larg
                  let ulname = this.get_varname(lname,funFrame)
                  let ufname = this.get_varname(lname,funFrame, "farg_")
                  this.add_to_body(Alloca(ulname, this.translate_type(ltype), None),newFunction)
                  this.add_to_body(Store(this.translate_type(ltype),Register(ufname), Register(ulname),None),newFunction) 
                let rreg = this.compile_expression(expr,newFunction);
                //Add an instruction to return the register - Implicit returns
                match rType with
                  | Void_t -> this.add_to_body(Ret_noval,newFunction);
                  | _ ->  this.add_to_body(Ret(rType, rreg),newFunction);
                this.lindex <- tempL
                this.gindex <- tempG
                symbol_table.current_frame <- tempFrame; 
              | _ -> printfn "Error in compiler generation"
          | _ -> 
            let compiledExpr = this.compile_expression(expr,func)
            //Add store instructions
            this.add_to_body(Alloca(newName, expType, None),func)
            this.add_to_body(Store(expType,compiledExpr, Register(newName), None),func)  
        Register(newName) // -> Should be edest
      | Var(varName) ->
        //Lookup the symbol table for varName to get gindex
        let newvarname = this.get_varname(varName.value,symbol_table.current_frame)
        let newReg = this.uniqueID(newvarname);
        this.add_to_body(Load(newReg, this.get_type(varName.value), Register(newvarname),None),func)
        Register(newReg)
      | TypedLet(box,exp, boxexp) -> 
          let (_,varName) = box.value
          let funFrame = symbol_table.locate_frame(box.line,box.column)
          //Store current frame information and move down to lower frame
          printf "Compiling typed Let";
          let tempL = this.lindex;
          let tempG = this.gindex;
          let tempFrame = symbol_table.current_frame
          this.lindex <- 0;
          this.gindex <- -1;
          symbol_table.current_frame <- funFrame
          //Populate the closure arguments
          let cargs = Vec<LLVMtype*string>();
          let mutable targs = [];
          //Add arguments for variable
          let expType = this.get_type(varName)
          let expArgName = sprintf "larg_%s" varName
          cargs.Add(expType, expArgName);
          targs <- targs @ [expType];
          //Add closure arguments into formal arguments
          for closureVar in funFrame.closure do
            let ((cvar,cindex),ctype) = closureVar.Key, closureVar.Value;
            cargs.Add((Pointer(this.translate_type(ctype)), sprintf "%s_%d" cvar cindex));
            targs <- targs @ [Pointer(this.translate_type(ctype))];
          let funName = this.uniqueID(varName);
           //Get return type
          let rType = symbol_table.locate_struct(boxexp.line,boxexp.column);
          let newFunction = this.add_function(funName,cargs, this.translate_type(rType))
          //Add variable bindings for let variable
          let uVarName = this.get_varname(varName, symbol_table.current_frame);
          this.add_to_body(Alloca(uVarName, expType, None),newFunction)
          this.add_to_body(Store(expType,Register(expArgName), Register(uVarName),None),newFunction) 
          //Compile rest of function
          let rreg = this.compile_expression(boxexp.value,newFunction);
          match rType with
            | LLunit -> this.add_to_body(Ret_noval,newFunction);
            | _ ->  this.add_to_body(Ret(this.translate_type(rType), rreg),newFunction);
          //Reset table frame
          this.lindex <- tempL
          this.gindex <- tempG
          symbol_table.current_frame <- tempFrame; 
          //Call code
          let expReg = this.compile_expression(exp,func); //Get argument value
          let mutable callargs = [(expType,expReg)] 
          //Add closure arguments
          for carg in funFrame.closure do
            let (cname,cval) = carg.Key;
            let cType = Pointer(this.translate_type(carg.Value));
            callargs <- callargs @ [(cType,Register(sprintf "%s_%d" cname cval))];
          let dreg = this.uniqueID("r");
          match rType with
            | LLunit -> this.add_to_body(Call(None, Basic("void"), targs, funName, callargs),func);Novalue
            | _ ->  this.add_to_body(Call(Some(sprintf "%s%s" "%" dreg), this.translate_type(rType), targs, funName, callargs),func); Register(dreg);
          
      | TypedLambda(_, _, expr) -> this.compile_expression(expr.value,func)
      | CodeBlock(exprs) -> 
        //This is its own code, you could create BB's here
        let mutable result = Novalue
        for newexprs in exprs do 
          result <- this.compile_expression(newexprs.value,func)
        result
      | Apply(box,[boxexpr]) when box.value = "print" -> //Change this to an apply
        let (a) = boxexpr.value
        let mutable compiledA = this.compile_expression(a,func)
        //Check type of A
        let mutable aType = this.translate_type(symbol_table.infer_type(a),a)
        let printType = 
          match aType with
            | Basic("i32") | Func_t(_,Basic("i32")) -> "lambda7c_printint"
            | Basic("double") | Func_t(_,Basic("double")) -> "lambda7c_printfloat"
            | _ -> "lambda7c_printstr"
        if printType = "lambda7c_printstr" then //String-specific
          let aReg = this.uniqueID("A")
          let aVar = this.uniqueID("SVAR")
          this.add_global_const(Globalconst(aVar,aType,compiledA,Some("align 1")));
          compiledA <- Register(aReg)
          this.add_to_body(Structfield(aReg, aType,Global(aVar),Iconst(0)),func)
          aType <- Pointer(Basic("i8"))
        this.add_to_body(Call(None, Basic("void"), [aType], printType, [(aType,compiledA)]),func)
        Novalue
      | Apply(box,args) when box.value = "input" -> //Change this to an apply
        //Check type of A
        let nreg = this.uniqueID("R");
        this.add_to_body(Call(Some(sprintf "%s%s" "%" nreg), Basic("i32"), [], "lambda7c_cin", []),func);
        Register(nreg);
      | Apply(varbox,args) ->
        let funName = varbox.value;
        let line,column = varbox.line,varbox.column;
        //Get closure here or something
        let closure = 
          match symbol_table.current_frame.get_entry(funName) with
            | Some(x) -> 
              match x.ast_rep with
                | Some(TypedLambda(_,_,newbox)) -> symbol_table.locate_frame(newbox.line, newbox.column).closure
                | _ -> symbol_table.current_frame.closure
            | _ -> symbol_table.current_frame.closure
        let (nargs,rType) = 
          match symbol_table.current_frame.get_entry(funName) with
            | Some(x) ->  
              match x.typeof with
                | LLfun(fargs,frType) ->
                  let mutable targs = fargs |> List.map(fun x -> this.translate_type(x))
                  for carg in closure do
                    let ctype = carg.Value
                    targs <- targs @ [Pointer(this.translate_type(ctype))]
                  (targs,this.translate_type(frType))
                | _ -> ([Basic("void")],Basic("void"))
            | None -> ([Basic("No definition found")],Basic("No definition found"))
        //Generate arguments
        let mutable values = args |> List.map(fun x -> this.compile_expression(x.value,func))
        for carg in closure do
          let (cname,cval) = carg.Key;
          values <- values @ [Register(sprintf "%s_%d" cname cval)]
        let mutable finalargs =[]
        for i in [0..values.Length-1] do
          finalargs <- finalargs @ [(nargs.[i], values.[i])]
        //Add method call
        match rType with
          | Void_t -> this.add_to_body(Call(None, rType, nargs, funName, finalargs),func); Novalue
          | _ -> 
            let nreg = this.uniqueID("R");
            this.add_to_body(Call(Some(sprintf "%s%s" "%" nreg), rType, nargs, funName, finalargs),func);
            Register(nreg);
       | Ifelse(conditionBox,a,b) ->
        let condition = this.compile_expression(conditionBox.value,func)
        let creg = this.uniqueID("R");
        //Downcast condition to a boolean
        this.add_to_body(Cast(creg,"trunc", Basic("i32"), condition, Basic("i1")),func)
        //Create remaining basic blocks
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
        let endreg = this.uniqueID("result");
        let rType = symbol_table.locate_struct(conditionBox.line,conditionBox.column);
        if compiled_a <> Novalue && compiled_b <> Novalue then 
          this.add_to_body(Phi2(endreg,this.translate_type(rType),compiled_a, label1, compiled_b, label0),func);
        Register(endreg)//This should return a register or something to phi to
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
        let endreg = this.uniqueID("result");
        let rType = symbol_table.locate_struct(conditionBox.line,conditionBox.column);
        if compiled_a <> Novalue then 
          this.add_to_body(Phi2(endreg,this.translate_type(rType),compiled_a, label1, compiled_a, label1),func);
        Register(endreg)
      | Import(x) -> compileLibrary(this.program, x.value.[1..x.value.Length-2]); Novalue; 
      | Class(box,p) -> 
        let classFrame = symbol_table.locate_frame(box.line,box.column)
        //Store current frame information and move down to lower frame
        let tempL = this.lindex;
        let tempG = this.gindex;
        let tempFrame = symbol_table.current_frame
        this.lindex <- 0;
        this.gindex <- -1;
        symbol_table.current_frame <- classFrame
        printfn "Compiling class body"
        printfn "%A" symbol_table.current_frame.entries
        let mutable classVariables = []
        for keyval in symbol_table.current_frame.entries do
          if symbol_table.member_type(keyval.Value.typeof) then
            classVariables <- classVariables @ [keyval.Value.typeof]
        printfn "%A" classVariables
        this.add_global_const(Structdec(box.value, this.vectorMap(classVariables, fun x -> this.translate_type(x))))
        printfn "%A" func
        this.compile_expression(p,func);
        //Reset table frame
        this.lindex <- tempL
        this.gindex <- tempG
        symbol_table.current_frame <- tempFrame; 
        Novalue;
      | _ -> Iconst(3)
