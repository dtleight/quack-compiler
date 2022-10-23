module QuackTypechecker
open Fussless
open RuntimeParser
open Quack


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
    let newframe =
      {
        table_frame.name=n;
        entries=HashMap<string,table_entry>();
        parent_scope = Some(this.current_frame);
      }
    this.frame_hash.[(line,column)] <- newframe
    this.current_frame <- newframe

  member this.pop_frame() =
    //this.current_frame.parent_scope |> Map (fun p -> this.current_frame<-p)
    this.current_frame <- 
      match this.current_frame.parent_scope with
        | Some(x) -> x
        | _ -> this.current_frame

  member this.report_error(line:int, column:int, error:string) =
         printfn "Error at Line %d,  Column %d : %s" line column error
  
  member this.detail_trace(name:string) =
         printfn "-----------------"
         printfn "%s" name
         printfn "-----------------"
         
  member this.is_numeric(t:lltype) = 
         t = LLint || t = LLfloat

  member this.infer_type(expression:expr, ?traceback:bool) = 
    let trace = defaultArg traceback false
    match expression with 
      | Integer(_) -> LLint
      | Floatpt(_) -> LLfloat
      | Strlit(_) -> LLstring
      | Binop("+",a,b) | Binop("-",a,b)| Binop("*",a,b) | Binop("/",a,b) | Binop("%",a,b) | Binop("^",a,b)  -> 
        let (atype,btype) = (this.infer_type(a), this.infer_type(b))
        if atype=btype && this.is_numeric(atype) then atype else LLuntypable
      | Binop("and",a,b) | Binop("or",a,b) ->
        let (atype,btype) = (this.infer_type(a), this.infer_type(b))
        if atype=btype && (atype=LLint) then LLint else LLuntypable
      | Binop("eq",a,b) | Binop("neq",a,b) | Binop("<",a,b) | Binop(">",a,b) | Binop("<=",a,b) | Binop(">=",a,b) ->
        let (atype,btype) = (this.infer_type(a), this.infer_type(b))
        if atype=btype && this.is_numeric(atype) then LLint else LLuntypable
      | Uniop("-",a) ->
        let atype = this.infer_type(a)
        if this.is_numeric(atype) then atype else LLuntypable
      | Uniop("not",a) ->
        let atype = this.infer_type(a)
        if atype = LLint then LLint else LLuntypable
      | Var(x) ->  let result = this.get_entry(x);
                   match result with 
                     |LLuntypable -> this.report_error(0,0,"Undeclared variable \"" + x + "\""); LLuntypable
                     | _ -> result
      | CodeBlock(se) -> 
        let mutable retType = LLunknown
        for x in se do
          retType <- if retType = LLuntypable then LLuntypable else this.infer_type(x.value,trace)
          if trace then printfn "%s: %s" (x.value.ToString()) (retType.ToString())
        if retType <> LLuntypable then LLunit else LLuntypable
        
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
                    LLunit
           | _ ->
               let exptype = this.infer_type(exp); 
               if atype <> exptype then 
                 this.report_error(line,column, "Type Mismatch")
                 LLuntypable
               else
                 this.add_entry(var,atype,Some(exp))
                 LLunit
      | TypedLambda(arglist,rtype, exp) ->
         let (line,column) = exp.line, exp.column
         let parent_frame = this.current_frame;
         //Push a new stack frame and add bindings for arguments.
         this.push_frame("Anonymous Lambda " + (line.ToString()) + ":" + (column.ToString()),line,column)
         for arg in arglist do
           let (vartype, var) = arg
           this.add_entry(var,vartype,None)
         this.current_frame <- parent_frame //Reset the stack frame.
         match rtype with
           | LLunknown -> LLunknown //Perform type inference here
           | _ -> 
               if rtype = this.infer_type(exp.value) then
                 rtype
               else
                 this.report_error(line,column, "Return Type Mismatch")
                 LLunknown  
      | TypedLet(box, exp, boxexp) -> 
          let (atype, var) = box.value
          let (line,column) = box.line, box.column
          match atype with
            | LLunknown -> 
                this.report_error(line,column,"Unimplemented");
                LLuntypable
            | _ -> atype; //Add a stack entry here
      | _ -> LLunit //DO SOMETHING HERE

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
    if found then lookup_frame.entries.[x].typeof else LLuntypable;; 

// global symbol table
let mutable global_frame = // root frame
  {
    table_frame.name = "global";
    entries= HashMap<string,table_entry>();
    parent_scope = None;
  }
let symbol_table =
  {
    SymbolTable.current_frame=global_frame;
    global_index = 0;
    frame_hash = HashMap<(int*int),table_frame>();
  }
