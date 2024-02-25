module QuackTypechecker
open Fussless
open Option
open RuntimeParser
open Quack
open System.Collections.Generic


type table_entry =
  {
     mutable typeof: lltype;
     gindex: int;  // unqiue index for brute-force alpha-conversion
     ast_rep : expr option;  // link to ast representation (just pointer)
  }
and table_frame =
  {
    name : string;
    entries:HashMap<string, table_entry>;
    closure: SortedDictionary<string*int, lltype>;
    parent_scope:table_frame option;
  }
  member this.get_entry(x:string) =
    let mutable lookup_frame = this
    let mutable found = false
    let mutable breakFree = false
    while (not found) && (not breakFree) do
      if lookup_frame.entries.ContainsKey(x) then 
        found <- true
      else
        lookup_frame <- 
          match lookup_frame.parent_scope with
            | Some(x) -> x
            | _ -> breakFree <- true; lookup_frame
    if found then Some(lookup_frame.entries.[x]) else None;; 

let numeric_binops = ["+"; "-"; "/"; "*"; "^";]
let comparison_binops = ["eq"; "neq"; "<"; ">"; "<="; ">=";]
let boolean_binops = ["or"; "and";]
let numeric_uniops = ["-";]
let boolean_uniops = ["not";]
let variable_uniops = ["car"; "cdr"; "display"; ]



type SymbolTable =  // wrapping structure for symbol table frames
  {
     mutable current_frame: table_frame;
     mutable global_index: int;
     frame_hash:HashMap<(int*int),table_frame>;
     struct_hash: HashMap<(int*int), lltype>;
  }
  member this.add_entry(s:string,t:lltype,a:expr option) = //overwrite
    this.global_index <- this.global_index + 1
    if  not(this.current_frame.entries.TryAdd(s,{typeof=t; gindex=this.global_index; ast_rep=a;})) then
      this.current_frame.entries.[s] <- {typeof=t; gindex=this.global_index; ast_rep=a;}
      
  member this.push_frame(n,line,column) =
    let newframe =
      {
        table_frame.name=n;
        entries=HashMap<string,table_entry>();
        parent_scope = Some(this.current_frame);
        closure = SortedDictionary<string*int,lltype>();
      }  
    this.frame_hash.[(line,column)] <- newframe
    this.current_frame <- newframe
    this.calculate_closure();
    

  member this.pop_frame() =
    this.current_frame.parent_scope |> Option.map (fun p -> this.current_frame<-p)

  member this.report_error(line:int, column:int, error:string) =
         printfn "Error at Line %d,  Column %d : %s" (line+1) (column+1) error
  
  member this.detail_trace(name:string) =
         printfn "-----------------"
         printfn "%s" name
         printfn "-----------------"
  member this.is_closure_member(t: lltype) =
    match t with
      | LLfun(_,_) -> false
      | _ -> true;
  member this.is_numeric(t:lltype) = 
         t = LLint || t = LLfloat
  member this.grounded_type(t:lltype) =
    match t with
      | LLunknown | LLvar(_) | LLuntypable -> false
      | LList(t) -> this.grounded_type t
      | lltype.LLtuple(vs) -> List.forall this.grounded_type vs
      | LLfun(args,rtype) ->  List.forall this.grounded_type (rtype::args)
      | _ -> true
   member this.member_type(t:lltype) = 
    match t with
      | LLfun(args,rtype) -> false
      | _ -> true
  //This can have additional arguments.
  member this.infer_type(expression:expr, ?traceback:bool) = 
    let trace = defaultArg traceback false
    match expression with
      //Literals 
      | Integer(_) -> LLint
      | Floatpt(_) -> LLfloat
      | Strlit(_) -> LLstring
      //Operators
      | Binop("+",a,b) | Binop("-",a,b)| Binop("*",a,b) | Binop("/",a,b) | Binop("%",a,b) | Binop("^",a,b)  -> 
        let (atype,btype) = (this.infer_type(a), this.infer_type(b))
        if atype=btype && this.is_numeric(atype) then atype else LLuntypable
      | Binop("and",a,b) | Binop("or",a,b) ->
        let (atype,btype) = (this.infer_type(a), this.infer_type(b))
        if atype=btype && (atype=LLint) then LLint else LLuntypable
      | Binop("eq",a,b) | Binop("!=",a,b) | Binop("<",a,b) | Binop(">",a,b) | Binop("<=",a,b) | Binop(">=",a,b) ->
        let (atype,btype) = (this.infer_type(a), this.infer_type(b))
        if atype=btype && this.is_numeric(atype) then LLint else LLuntypable
      | Binop("=",a,b) -> if this.infer_type(b) = this.infer_type(a) then this.infer_type(a) else LLuntypable
      | Uniop("-",a) ->
        let atype = this.infer_type(a)
        if this.is_numeric(atype) then atype else LLuntypable
      | Uniop ("display", a) -> if this.infer_type(a) <> LLuntypable  then LLunit else LLuntypable
      | Uniop("not",a) ->
        let atype = this.infer_type(a)
        if atype = LLint then LLint else LLuntypable
      //Sequences
      | CodeBlock(se) -> 
        let mutable retType = LLunknown
        for x in se do
          let expType = this.infer_type(x.value,trace)
          retType <- if retType = LLuntypable then LLuntypable else expType
          if trace then printfn "%s" (retType.ToString())
        if retType <> LLuntypable then retType else LLuntypable
      | ListLiteral(exprs) ->
        let mutable iType = LLunknown
        for exp in exprs do
          let expType = this.infer_type(exp.value)
          if iType = LLunknown then iType <- expType;
          if expType <> iType then  iType <- LLuntypable;
          if trace then printfn "%s : %s" (exp.value.ToString()) (iType.ToString())
        LList(iType)
      //Definitions
      | Var(box) ->  
        let(line,column) = box.line, box.column
        let result = this.current_frame.get_entry(box.value);
        match result with 
          |None -> this.report_error(line,column,"Undeclared variable \"" + box.value + "\""); LLuntypable
          |Some(x) -> x.typeof;
      | TypedDefine(box, exp) -> 
         let (atype, var) = box.value // let x: int =
         let (line,column) = box.line, box.column 
         //Recursion handling - this adds a binding for the function
         match exp with
           | TypedLambda(_,rtype,_) when rtype <> LLunknown -> this.add_entry(var, LLfun([LLint],rtype), None);
           | TypedLambda(_,_,_) -> this.add_entry(var, LLfun([LLint],LLvar("A")), None);
           | _ -> ();
         match atype with
           | LLunknown -> //Perform type inference
               let vartype = this.infer_type(exp);// Get the type of the expression(non-grounded)
               match exp,vartype with
                 | (TypedLambda(narglist,nrtype,lexp),ntype) -> 
                    //Infer the return type
                    let lType = this.infer_lambda(narglist,nrtype,lexp,lexp.line, lexp.column,var); 
                    this.add_entry(var,lType,Some(exp)) 
                    lType
                 | (_,LLuntypable) -> LLuntypable 
                 | (nexp,ntype) -> //A is a grounded type so you can just add an entry with its type
                    this.add_entry(var,ntype,Some(exp))
                    vartype
           | _ ->
               let exptype = this.infer_type(exp); 
               if atype <> exptype then 
                 this.report_error(line,column, "Type Mismatch")
                 LLuntypable
               else
                 this.add_entry(var,atype,Some(exp))
                 atype
      //Anonymous Lambdas
      | TypedLambda(arglist,rtype, exp) -> this.infer_lambda(arglist,rtype,exp, exp.line, exp.column)
      | TypedLet(box, exp, boxexp) -> 
          //New frames need to be completed here
          let (bvar) = boxexp.value;  //Ensuing lambda
          let (bline,bcolumn) = boxexp.line, boxexp.column; //Ensuing lambda
          let (atype, avar) = box.value; //var to inject
          let (line,column) = box.line, box.column;
          let eType = this.infer_type(exp); //Type for inject var
          let parent_frame = this.current_frame;
          //Push a new stack frame and add bindings for arguments.
          this.push_frame((sprintf "Let %s %d:%d" avar line column),line,column);
          this.add_entry(avar,eType,Some(exp)) 
         
          //Typecheck the body
          let rbType = this.infer_type(boxexp.value);
          let rType = 
            match atype with
              | LLunknown -> this.infer_type(boxexp.value);
              | _ -> atype;
          this.current_frame <- parent_frame //Reset the stack frame
          this.struct_hash.[(bline,bcolumn)] <- rType 
          rType
      | Apply(box,boxexp) when box.value = "print"-> LLunit
      | Apply(box,boxexp) when box.value = "input"-> LLint 
      | Apply(box,boxexp) ->
         let var = box.value 
         let (line,column) = box.line, box.column
         match this.current_frame.get_entry(var) with 
          | Some(entry) -> 
            printfn "Found a table entry"
            match entry.typeof with
              | LLfun(_,rType) -> rType
              | _ -> entry.typeof
          |_ -> LLuntypable
      //Loops and Selections
      | Whileloop(box, code) ->
          let (line,column) = box.line, box.column 
          if this.is_numeric(this.infer_type(box.value)) then
            let rType = this.infer_type(code)
            this.struct_hash.[(line,column)] <- rType
            rType
          else
            this.report_error(line,column,"Invalid loop condition")
            LLuntypable 
      | Ifelse(box, a, b) ->
          let (line,column) = box.line, box.column 
          if this.is_numeric(this.infer_type(box.value)) then
            let atype,btype = this.infer_type(a), this.infer_type(b);
            if atype = btype then
              this.struct_hash.[(line,column)] <- atype
              atype 
            else 
              this.report_error(line,column,"Type mismatch")
              LLuntypable
          else
            this.report_error(line,column,"Invalid if condition")
            LLuntypable 
      | Class(name,body) -> 
            let (line,col) = (name.line,name.column)
            this.push_frame((sprintf "Class %s %d:%d" name.value line col), line,col)
            this.infer_type(body)
            this.pop_frame();
            LLunit
      | Import(name) -> LLunit    
      | _ -> LLunit //DO SOMETHING HERE


  //LAMBDA FUNCTIONS
  member this.infer_lambda(arglist: (lltype*string) list, rtype: lltype, exp: LBox<expr>, line: int, column: int, ?assignment:  string) = 
    let aName = defaultArg assignment "Anonymous"
    let (line,column) = exp.line, exp.column;
    let parent_frame = this.current_frame;
    let mutable argtypes = [];
    //Push a new stack frame and add bindings for arguments.
    this.push_frame((sprintf "Lambda %s %d:%d" aName line column),line,column);
    for arg in arglist do
      let (vartype, var) = arg //Why isn't type inference happening here
      this.add_entry(var,vartype,None) 
      argtypes <- vartype::argtypes;
    let ftype = 
      match rtype with
        | LLunknown -> 
           let nrtype = this.infer_type(exp.value);
           if nrtype <> LLuntypable then LLfun(argtypes,nrtype) else LLuntypable
        | _ -> 
           if rtype = this.infer_type(exp.value) then
            LLfun(argtypes,rtype)
           else
            this.report_error(line,column, "Return Type Mismatch");
            LLuntypable
    this.current_frame <- parent_frame //Reset the stack frame 
    //if ftype <> LLuntypable then  
      //this.add_entry(var,ftype,Some(exp)); //Issue is here
    ftype  
    
  //Collect variables up the stack frame - You can change this to grab only the necessary ones
  //Have a set of bound and free variables. when you need a variable, bind it.
  member this.collect_vars() =
    let mutable curr_frame = Some(this.current_frame)
    while curr_frame <> None do
      (curr_frame |> Option.map (fun frame -> 
      for x in frame.entries do
        //Add Anonymous function skips, add function skips
        if this.is_closure_member(x.Value.typeof) then 
           this.current_frame.closure.TryAdd((x.Key, x.Value.gindex),x.Value.typeof) |> ignore
      curr_frame <- frame.parent_scope;
     )) |> ignore
  member this.calculate_closure() = 
    this.collect_vars()  
  
  member this.init_frame(init: expr) =
   this.push_frame("Main Frame", 0, 0);
    
  member this.locate_frame(line: int, column: int) =
    this.frame_hash.[(line,column)]
  
  member this.locate_struct(line:int, column: int) =
    this.struct_hash.[(line,column)]
    
  member this.print_vars() =
    for x in this.frame_hash do
      printfn "%s" (String.replicate 75 "-")
      printfn "\t\t\tFrame: %s" x.Value.name
      printfn "%s" (String.replicate 75 "-")
      printfn "|%22s | %48s|" "Variable" "Type"
      printfn "%s" (String.replicate 75 "-")
      for y in x.Value.entries do
        printfn "|%22s | %48s|" y.Key (y.Value.typeof.ToString())
      printfn "|%22s | %48s|" "closure" ((sprintf "%A" (x.Value.closure.Keys |> Seq.map(fun (k,i) -> k))).Replace("seq ", ""))
      printfn "%s" (String.replicate 75 "-");;
    
// global symbol table
let mutable global_frame = // root frame
  {
    table_frame.name = "global";
    entries= HashMap<string,table_entry>();
    parent_scope = None;
    closure = SortedDictionary<string*int,lltype>();
  }
let symbol_table =
  {
    SymbolTable.current_frame=global_frame;
    global_index = 0;
    frame_hash = HashMap<(int*int),table_frame>();
    struct_hash = HashMap<(int*int),lltype>();
  }
