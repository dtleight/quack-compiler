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
    closures: SortedDictionary<string, int*lltype>;
    parent_scope:table_frame option;
  }

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
  }
  member this.add_entry(s:string,t:lltype,a:expr option) = //overwrite
    this.global_index <- this.global_index + 1
    this.current_frame.entries.Add(s,{typeof=t; gindex=this.global_index; ast_rep=a;})
 
  member this.push_frame(n,line,column) =
    this.calculate_closure()
    let newframe =
      {
        table_frame.name=n;
        entries=HashMap<string,table_entry>();
        parent_scope = Some(this.current_frame);
        closures = SortedDictionary<string,int*lltype>();
      }  
    this.frame_hash.[(line,column)] <- newframe
    this.current_frame <- newframe
    

  member this.pop_frame() =
    this.current_frame.parent_scope |> Option.map (fun p -> this.current_frame<-p)
    //this.current_frame <- 
    //  match this.current_frame.parent_scope with
    //    | Some(x) -> x
    //    | _ -> this.current_frame

  member this.report_error(line:int, column:int, error:string) =
         printfn "Error at Line %d,  Column %d : %s" (line+1) (column+1) error
  
  member this.detail_trace(name:string) =
         printfn "-----------------"
         printfn "%s" name
         printfn "-----------------"
         
  member this.is_numeric(t:lltype) = 
         t = LLint || t = LLfloat
  member this.grounded_type(t:lltype) =
    match t with
      | LLunknown | LLvar(_) | LLuntypable -> false
      | LList(t) -> this.grounded_type t
      | lltype.LLtuple(vs) -> List.forall this.grounded_type vs
      | LLfun(args,rtype) ->  List.forall this.grounded_type (rtype::args)
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
        let result = this.get_entry(box.value);
        match result with 
          |None -> this.report_error(line,column,"Undeclared variable \"" + box.value + "\""); LLuntypable
          |Some(x) -> x.typeof;
      | TypedDefine(box, exp) -> 
         let (atype, var) = box.value
         let (line,column) = box.line, box.column
         //Recursion handling
         match exp with 
           | TypedLambda(_,rtype,_) when rtype <> LLunknown -> this.add_entry(var, rtype, None);
           | TypedLambda(_,_,_) -> this.add_entry(var, LLvar("A"), None);
           | _ -> ();
         match atype with
           | LLunknown -> 
               let vartype = this.infer_type(exp);
               match exp,vartype with
                 | (TypedLambda(_,_,_),vartype) -> vartype //Avoid redeclaring recursive functions
                 | (_,LLuntypable) -> LLuntypable 
                 | (exp,vartype) -> 
                    this.add_entry(var,vartype,Some(exp))
                    vartype
           | _ ->
               let exptype = this.infer_type(exp); 
               if atype <> exptype then 
                 this.report_error(line,column, "Type Mismatch")
                 LLuntypable
               else
                 this.add_entry(var,atype,Some(exp))
                 atype
      | TypedLambda(arglist,rtype, exp) -> this.infer_lambda(arglist,rtype,exp)
      | TypedLet(box, exp, boxexp) -> 
          //New frames need to be completed here
          let (atype, var) = box.value
          let (line,column) = box.line, box.column
          match atype with
            | LLunknown -> 
                this.report_error(line,column,"Unimplemented");
                LLuntypable
            | _ -> atype; //Add a stack entry here
      //Loops and Selections
      | Whileloop(box, code) ->
          let (line,column) = box.line, box.column 
          if this.is_numeric(this.infer_type(box.value)) then
            this.infer_type(code)
          else
            this.report_error(line,column,"Invalid loop condition")
            LLuntypable 
      | Ifelse(box, a, b) ->
          let (line,column) = box.line, box.column 
          if this.is_numeric(this.infer_type(box.value)) then
            let atype,btype = this.infer_type(a), this.infer_type(b);
            if atype = btype then
              atype 
            else 
              this.report_error(line,column,"Type mismatch")
              LLuntypable
          else
            this.report_error(line,column,"Invalid if condition")
            LLuntypable 
      | _ -> LLunit //DO SOMETHING HERE


  //LAMBDA FUNCTIONS
  member this.infer_lambda(arglist: (lltype*string) list, rtype: lltype, exp: LBox<expr>) = 
    let (line,column) = exp.line, exp.column
    let parent_frame = this.current_frame;
    let mutable argtypes = [];
    //Push a new stack frame and add bindings for arguments.
    this.push_frame("Anonymous Lambda " + (line.ToString()) + ":" + (column.ToString()),line,column)
    for arg in arglist do
      let (vartype, var) = arg
      this.add_entry(var,vartype,None)
      argtypes <- vartype::argtypes;
    let ftype = match rtype with
      | LLunknown -> 
         LLfun(argtypes,LLunknown) //Perform type inference here
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
    
  member this.collect_vars() =
    for x in this.current_frame.entries do
      //Add Anonymous function skips, add function skips
      let frame = x.Value
      printfn "-------------------"
      printfn "%A" x.Value
      
      //this.closure.TryInsert(x.Key,)
      
  member this.calculate_closure() = 
    this.collect_vars()  
    
             
  //member find_frame()
//Access the hashmap for this frame, 
  member this.get_entry(x:string) =
    let mutable lookup_frame = this.current_frame
    let mutable found = false
    let mutable breakFree = false
    while not found && not breakFree do
      if lookup_frame.entries.ContainsKey(x) then 
        found <- true
      else
        lookup_frame <- 
          match this.current_frame.parent_scope with
            | Some(x) -> x
            | _ -> breakFree <- true; this.current_frame
    if found then Some(lookup_frame.entries.[x]) else None;; 

// global symbol table
let mutable global_frame = // root frame
  {
    table_frame.name = "global";
    entries= HashMap<string,table_entry>();
    parent_scope = None;
    closures = SortedDictionary<string,int*lltype>();
  }
let symbol_table =
  {
    SymbolTable.current_frame=global_frame;
    global_index = 0;
    frame_hash = HashMap<(int*int),table_frame>();
  }
