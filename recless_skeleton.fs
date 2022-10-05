module Recless.Base
open System;
open System.Collections.Generic;

// type aliases
type Vec<'A> = ResizeArray<'A>
type Conslist<'A> = 'A list   // not used much
type HashMap<'A,'B> = Dictionary<'A,'B>

let mutable TRACE = true;  // global tracing flag

// LL(1) parser generator
// grammar symbols are just strings

// type of item on the valuestack (parse stack)
type Stackitem<'AT> =
  {
    symbol: string; // should correspond to a grammar symbol
    value: 'AT;   // semantic value
    line: int;   // lexical position
    column: int;
  }
let new_stackitem<'T>(s,v:'T,l,c) =
  {Stackitem.symbol=s; value=v; line=l; column=c;}

// This is called an "active pattern": allows to look at just value with
// pattern matching: match t with | Item(v) -> ...
let (|Item|) (t:Stackitem<'AT>) = Item(t.value)


//////////////////// Grammar Representation

type Production<'T> =    // production rule
  {
    lhs: string
    rhs: Vec<string>;
    mutable action: Vec<Stackitem<'T>> -> 'T;
  }

type Grammar<'T> =
  {
    Symbols : HashSet<string>;
    Nonterminals : HashSet<string>;
    mutable startsymbol : string;
    Productions : Vec<Production<'T>>;
    //Rulesfor : HashMap<string,HashSet<int>>;
    Follow: HashMap<string,HashSet<string>>;
    First : HashMap<string,HashSet<string>>;
    Nullable : HashSet<string>;
    LL1Table : HashMap<string,HashMap<string,int>>;
    valueterminals : HashMap<string,string*(string -> 'T)>;
    lexterminals: HashMap<string,string>; // maps ":" to "COLON"
  }
  // impl Grammar:
  member this.terminal (s:string) =
    this.Symbols.Contains(s) && not(this.Nonterminals.Contains(s))
  member this.nonterminal (s:string) = this.Nonterminals.Contains(s)

  // function to add valueterminal to grammar, with function to convert
  member this.valueterminal(tname:string,tokenname:string,conv) =
    if not(this.Nonterminals.Contains(tname)) then
      this.Symbols.Add(tname) |> ignore
      this.valueterminals.[tokenname] <- (tname,conv)
    else
      printfn "CONFLICTING DEFINITION OF SYMBOL %s AS NONTERMINAL" tname

  member this.lexterminal(tname:string, tokenform:string) =
    if not(this.Nonterminals.Contains(tname)) then
      this.Symbols.Add(tname) |> ignore
      this.lexterminals.[tokenform] <- tname
    else
      printfn "CONFLICTING DEFINITION OF SYMBOL %s IGNORED" tname

  member this.terminals(tnames:string list) =
    for tname in tnames do 
      if not(this.Nonterminals.Contains(tname)) then
        this.Symbols.Add(tname) |> ignore
      else
        printfn "CONFLICTING DEFINITION OF SYMBOL %s IGNORED" tname

  member this.nonterminals(tnames:string list) =
    for tname in tnames do 
      if not(this.Symbols.Contains(tname) && not(this.Nonterminals.Contains(tname))) then
        this.Symbols.Add(tname) |> ignore
        this.Nonterminals.Add(tname) |> ignore
      else
        printfn "CONFLICTING DEFINITION OF SYMBOL %s IGNORED" tname

  // call like:
  // grammar1.production("E --> T E1", fun V -> ...)

  member this.production(rawrule:string, action) =
    let ruleform = rawrule.Split([|"-->"; ":"; " ";|],StringSplitOptions.RemoveEmptyEntries)
    if ruleform.Length>0 then
      if not(this.Nonterminals.Contains(ruleform.[0])) then
        raise(Exception(sprintf "%s is not recognized as a non-terminal symbol" (ruleform.[0])))
      let lhs = ruleform.[0] 
      let rhs = Vec<string>();
      for i in 1..ruleform.Length-1 do
        if not(this.Symbols.Contains(ruleform.[i])) then
          raise(Exception(sprintf "Symbol %s is not recognized" (ruleform.[i])))
        rhs.Add(ruleform.[i])
      let rule = {Production.lhs=lhs; rhs=rhs; action=action;}
      this.Productions.Add(rule) |> ignore
      //this.Productions.Count-1   // returns the index of the rule added
    else
      printfn "Malformed production rule rejected"
  //this.production

  // add a production with default semantic action
  member this.production_default(s) =
    this.production(s,fun v -> Unchecked.defaultof<'T>)

  member this.set_action(ri, action) =
    this.Productions.[ri].action <- action
  
  member this.set_start(s:string) =
    if not(this.Nonterminals.Contains(s)) then
      printfn "The start symbol must be declared as a non-terminal symbol"
    else
      this.startsymbol <-s

  member this.printrule(ri) =
    let rule = this.Productions.[ri]
    printf "(%d) %s --> " ri rule.lhs
    for sym in rule.rhs do printf "%s " sym
    printfn ""
  member this.printgrammar() =
    for i in 0..this.Productions.Count-1 do this.printrule(i)
    

  
  ///// LL parser setup: Nullable, First and Follow closures:
  member this.find_nullables() =
    let mutable progress = true
    while progress do
      progress <- false
      for rule in this.Productions do
        let mutable rhsnullable = true
        let mutable i = 0;
        while i<rule.rhs.Count && rhsnullable do
          if not(this.Nullable.Contains(rule.rhs.[i])) then
            rhsnullable <- false // give me a break!
          i<-i+1
        // for each symbol on rhs of rule
        if rhsnullable then progress<-this.Nullable.Add(rule.lhs)||progress
      //for each rule
    //while progress
  //nullable


  member this.nullableseq (seq:Vec<string>, starti) =
    let mutable ax =true
    let mutable i = starti
    while i<seq.Count && ax do
      if not(this.Nullable.Contains(seq.[i])) then ax<-false
      i <- i+1
    ax

  member this.find_first() =
    for rule in this.Productions do
       this.First.[rule.lhs] <- new HashSet<string>()
    let mutable progress = true
    while progress do
      progress <- false
      for rule in this.Productions do
        let mutable rhsnullable = true
        let mutable i = 0;
        while i<rule.rhs.Count && rhsnullable do
          if rhsnullable then 
            if this.terminal(rule.rhs.[i]) then
              if not(this.First.[rule.lhs].Contains(rule.rhs.[i])) then 
                progress<- this.First.[rule.lhs].Add(rule.rhs.[i])||progress
            else
               let tempCount = this.First.[rule.lhs].Count
               this.First.[rule.lhs].UnionWith(this.First.[rule.rhs.[i]])
               if tempCount <> this.First.[rule.lhs].Count then
                 progress <- true
          if not(this.Nullable.Contains(rule.rhs.[i])) then
            rhsnullable <- false // give me a break!
          i<-i+1
        // for each symbol on rhs of rule
      //for each rule
    //while progress
  //first

  //Takes a starting index and returns the set of First symbols.
  member this.firstseq (seq:Vec<string>, starti) = 
    let setax = new HashSet<string>()
    let mutable i = starti
    let mutable rhsnullable = true
    while i<seq.Count && rhsnullable do
      if this.terminal(seq.[i]) then
         setax.Add(seq.[i])
         rhsnullable <- false
      else
        for x in this.First.[seq.[i]] do
          setax.Add(x)
        rhsnullable <- not(this.Nullable.Contains(seq.[i]))
      i <- i+1
    setax 

   //If the end is non-terminal or nullable, add B -> A
  member this.find_follow() =  // stub only
    for rule in this.Productions do
       this.Follow.[rule.lhs] <- new HashSet<string>()
    this.Follow.[this.startsymbol].Add("EOF") |> ignore
    let mutable progress = true
    while progress do
      progress <- false
      for rule in this.Productions do
        for i = 1 to rule.rhs.Count-1 do
          if i < rule.rhs.Count-1 then
            if this.nonterminal(rule.rhs.[i+1]) then
              for symbol in this.First.[rule.rhs.[i+1]] do
                progress <- this.Follow.[rule.rhs.[i]].Add(symbol)||progress
              if this.Nullable.Contains(rule.rhs.[i+1]) then
                for symbol in this.Follow.[rule.lhs] do
                  progress <- this.Follow.[rule.rhs.[i]].Add(symbol)||progress
            else
              progress <- this.Follow.[rule.rhs.[i]].Add(rule.rhs.[i+1]) || progress
          else if this.Nullable.Contains(rule.rhs.[i]) then
            for symbol in this.Follow.[rule.lhs] do
              progress <- this.Follow.[rule.rhs.[i]].Add(symbol) || progress
        // for each symbol on rhs of rule
      //for each rule
    //while progress
  //first

  (*
  //If the end is non-terminal or nullable, add B -> A
  member this.find_follow() =  // stub only
    for rule in this.Productions do
       this.Follow.[rule.lhs] <- new HashSet<string>()
    this.Follow(this.startsymbol).Add("$")
    let mutable progress = true
    while progress do
      progress <- false
      for rule in this.Productions do
        let mutable i = 1;
        let mutable proceed = true
        //printfn "%s" (rule.ToString())
        while i < rule.rhs.Count && proceed do
          if this.nonterminal(rule.rhs.[i+1] then
             //firstSeq
             this.Follow.[rule.rhs.[i]].UnionWith(this.firstseq(rule.rhs,i+1))
          if this.terminal(rule.rhs.[i]) then
            progress <- this.First.[rule.lhs].Add(rule.rhs.[i])
            proceed <- false
          else 
            let tempCount = this.Follow.[rule.rhs.[i]].Count
            if this.nullableseq(rule.rhs, i+1) then //Correct
              this.Follow.[rule.rhs.[i]].UnionWith(this.Follow.[rule.lhs])
            else
              this.Follow.[rule.rhs.[i]].UnionWith(this.firstseq(rule.rhs,i+1)) 
            if tempCount <> this.Follow.[rule.rhs.[i]].Count then
                progress <- true
          i<-i+1
        // for each symbol on rhs of rule
      //for each rule
    //while progress
  //first
*)

  member this.make_table() =
    for nt in this.Nonterminals do
      this.LL1Table.Add(nt,HashMap<string,int>())
    let mutable i = -1
    for rule in this.Productions do
      i <- i + 1
      for t in this.firstseq(rule.rhs, 0) do
         this.LL1Table.[rule.lhs].Add(t, i)
      if this.nullableseq(rule.rhs, 0) then
        for b in this.Follow.[rule.lhs] do
          this.LL1Table.[rule.lhs].Add(b, i)
  // rest needs to be done by students...
  //create table


//////// end of impl Grammar

let new_grammar<'AT>(start:string) =
  let gmr = 
    { 
      Symbols = HashSet<string>();
      Grammar.Nonterminals = HashSet<string>();      
      startsymbol = start;
      Productions = Vec<Production<'AT>>();
      Follow = HashMap<string,HashSet<string>>();
      First = HashMap<string,HashSet<string>>();
      Nullable = HashSet<string>();
      LL1Table = HashMap<string,HashMap<string,int>>();
      valueterminals = HashMap<string,string*(string -> 'AT)>();
      lexterminals = HashMap<string,string>();
    }
  gmr.Nonterminals.Add(start) |> ignore
  gmr.Symbols.Add(start) |> ignore
  gmr.Symbols.Add("EOF") |> ignore
  gmr.Symbols.Add("METASTART") |> ignore
  gmr.Nonterminals.Add("METASTART") |> ignore
  gmr.production(sprintf "METASTART --> %s EOF" start, fun v->v.[0].value)
  gmr
//new_grammar with start symbol


/////////////////////// RUNTIME PARSER
  
type LLparser<'T> =
  {
    Gmr: Grammar<'T>;
    parsestack : Stack<string>;
    rulestack : Stack<int*int>; // rule number and position on VALUESTACK
    valuestack : Stack<Stackitem<'T>>;
    mutable errors : bool;  // determines if errors occurred
    lexer : Fussless.AbstractLexer<unit>; //returns RawTokens
  }
  //  member this.convert_token (rt:Fussless.RawToken) =
  //    new_stackitem("dummy",Unchecked.defaultof<'T>,0,0)

  member this.next_la() =  // get next input symbol, place on stack
    let rt = this.lexer.next_lt()  //RawToken
    // check valueterminal
    if this.Gmr.valueterminals.ContainsKey(rt.token_name) then
      let (tname,f) = this.Gmr.valueterminals.[rt.token_name]
      let v = f(rt.token_text)
      new_stackitem(tname,v,rt.line,rt.column)
    else if this.Gmr.lexterminals.ContainsKey(rt.token_name) then
      let tname = this.Gmr.lexterminals.[rt.token_name]
      new_stackitem(tname,Unchecked.defaultof<'T>,rt.line,rt.column)
    else
      new_stackitem(rt.token_name,Unchecked.defaultof<'T>,rt.line,rt.column)

  /////
  // parsing is done in two phases.  First phase is the pure, top-down parsing
  // phase. Upon success, the second phase goes bottom up, applying
  // semantic action functions and synthesizing semantic values.  The semantic
  // actions must be pure functions (stateless).  One possible enhancement
  // is to attach two semantic actions to each production: one to be applied
  // in the forward stage and can have side-effects.
  /////
  member this.parse_top_down() =
    this.parsestack.Push("EOF");
    this.parsestack.Push(this.Gmr.startsymbol);
    this.rulestack.Push(0,0); // 0 is always meta-start rule, position 0
    // push startrule number (0) on rulestack
    let mutable stop = false
    let mutable lookahead = this.next_la()
    while not(stop) do
      let next = this.parsestack.Pop(); // expected symbol, no value
      if this.Gmr.Nonterminals.Contains(next) then // nonterminal
        let row = this.Gmr.LL1Table.[next]
        if row.ContainsKey(lookahead.symbol) then
          let ri = row.[lookahead.symbol]
          this.rulestack.Push(ri,this.valuestack.Count)
          if TRACE then
              try (printf "lookahead %A, rule " lookahead.value) with | _ -> 
                 printf "lookahead %s, rule " lookahead.symbol
              this.Gmr.printrule(ri)
          // push rhs of rule ri on stack
          let rule = this.Gmr.Productions.[ri]
          let mutable i = rule.rhs.Count-1
          while i>=0 do
            this.parsestack.Push(rule.rhs.[i])
            i<-i-1
          //while
        else
          printfn "PARSE ERROR line %d column %d, UNEXPECTED SYMBOL %s" (lookahead.line) (lookahead.column) (lookahead.symbol)
          this.errors<-true
      else if this.Gmr.terminal(next) then
        if next=lookahead.symbol then
          this.valuestack.Push(lookahead);
          if TRACE then
            printfn "  pushed %s on valuestack " lookahead.symbol
          if lookahead.symbol="EOF" then stop <- true
          else lookahead <- this.next_la()
        else
          printfn "PARSE ERROR line %d column %d, EXPECTING %s BUT GOT %s" (lookahead.line) (lookahead.column) next (lookahead.symbol)
          this.errors<-true
    // parse loop
  // parse_top_down

  member this.compose_bottom_up() =
    let mutable line = 0
    let mutable column = 0
    let bustack = Stack<Stackitem<'T>>(); // new stack to push onto
    while this.rulestack.Count > 0 do
      let (ri,vsi) = this.rulestack.Pop()
      let rule = this.Gmr.Productions.[ri]
      // shove valuestack to bustack, form arg for sem action:
      let mutable i = this.valuestack.Count-1
      while i>=vsi do
        let last = this.valuestack.Pop()
        bustack.Push(last)
        i <-i-1
      //shove

      let semargs = Vec<Stackitem<'T>>();
      for _ in 1..rule.rhs.Count do
         let nextitem = bustack.Pop()
         if line=0 && column=0 then
            line <- nextitem.line
            column<-nextitem.column
         semargs.Add(nextitem)
         if TRACE then
             printf "   popped values "
             for arg in semargs do
               try (printf "%A, " arg.value) with | _ -> printf "%s(null), " arg.symbol
             printfn ""
      let newval = (rule.action semargs)
      bustack.Push(new_stackitem(rule.lhs,newval,line,column))
      if TRACE then
        printfn "pushed %A value for %s" newval rule.lhs
    //while stack not empty
    if bustack.Count=1 then
      this.valuestack.Clear()
      this.valuestack.Push(bustack.Pop())
    else
      printfn "PARSING FAILED: %d values left on stack" bustack.Count
      while TRACE && this.valuestack.Count>0 do
        printfn "stack item %A" (bustack.Pop())
  //compose_bottom_up

  member this.parse() =
    this.parse_top_down()
    if TRACE then printfn "top-down phase complete"
    if not(this.errors) then
      this.compose_bottom_up()
      if this.valuestack.Count=1 then 
        this.valuestack.Pop().value
      else
        Unchecked.defaultof<'T>
    else
      Unchecked.defaultof<'T>        
  //parse      

  /////////// end of impl LLparser

let make_parser<'AT>(gmr:Grammar<'AT>,lexer) =
  gmr.find_nullables()
  if TRACE then
    for n in gmr.Nullable do printf "Nullable %s, " n
    printfn ""
  gmr.find_first()
  if TRACE then
    for kvp in gmr.First do
      printf "First(%s)= " kvp.Key
      for s in kvp.Value do printf "%s, " s
      printfn ""
  gmr.find_follow()
  if TRACE then
    for kvp in gmr.Follow do
      printf "Follow(%s)= " kvp.Key
      for s in kvp.Value do printf "%s, " s
      printfn ""  
  gmr.make_table()
  {
    LLparser.Gmr = gmr;
    parsestack = Stack<string>();
    rulestack = Stack<int*int>();
    valuestack = Stack<Stackitem<'AT>>();
    errors = false;
    lexer = lexer;
  }
// make_parser


/////////////////////////////////////////////////////////////////////
////////////////// testing with a sample grammar and action functions

//TRACE <- true

// semantic value type
type semval_t = Number of int | Continuation of (int -> int) | Nothing

// semantic actions for rules, must be purely functional (stateless)
let semact1 (rhs:Vec<Stackitem<semval_t>>) =  // E --> T E1
  match (rhs.[0].value, rhs.[1].value) with
    | (Number(x), Continuation(f)) -> Number(f x)
    | _ -> Nothing

let semact2 (rhs:Vec<Stackitem<semval_t>>) =  // E1 --> - T E1
  match (rhs.[1].value, rhs.[2].value) with
    | (Number(x), Continuation(f)) -> Continuation(fun y->f(y-x))
    | _ -> Nothing

let semact3 (rhs:Vec<Stackitem<semval_t>>) =  // E1 --> + T E1
  match (rhs.[1].value, rhs.[2].value) with
    | (Number(x), Continuation(f)) -> Continuation(fun y->f(y+x))
    | _ -> Nothing    

let semact4 (rhs:Vec<Stackitem<semval_t>>) =  // E1 --> epsilon
  Continuation(fun x -> x)

// semact5 for T --> F T1 is same as semact1
let semact5 = semact1

let semact6 (rhs:Vec<Stackitem<semval_t>>) =  // T1 --> * F T1
  match (rhs.[1].value, rhs.[2].value) with
    | (Number(x), Continuation(f)) -> Continuation(fun y->f(y*x))
    | _ -> Nothing

let semact7 (rhs:Vec<Stackitem<semval_t>>) =  // T1 --> DIV F T1
  match (rhs.[1].value, rhs.[2].value) with
    | (Number(x), Continuation(f)) -> Continuation(fun y->f(y/x))
    | _ -> Nothing

(* Do this for a right-associative division operator.  However, 
   this will only work if * is also made right-associative

let semact7r (rhs:Vec<Stackitem<semval_t>>) =  // T1 --> DIV T
  match rhs.[1].value with
    | Number(x) -> Continuation(fun y->(y/x))
    | _ -> Nothing

The rule T1 --> DIV T is derived from the LR grammar T --> F DIV T | F
after left-factoring and the rule T1 --> DIV F T1 is derived from
T --> T DIV F | F  after left-recursion elimination.  Forgive the ugly
grammar: some people like it.  I can only stomach it after I noticed
that the semantic value of T1 can be a continuation function.  Otherwise
you will have to build a "tree" just as ugly as the grammar, something
that will bear no resemblance to the AST you ultimately want, just to
change the associativity of the operator.
*)

let semact8 = semact4  // T1 --> epsilon

let semact9 (rhs:Vec<Stackitem<semval_t>>) =  // F --> num
  rhs.[0].value

let semact10 (rhs:Vec<Stackitem<semval_t>>) =  // F --> ( E )
  rhs.[1].value
    

let G = new_grammar<semval_t>("E")
G.terminals(["+";"-";"*";"(";")"; "#"] )
G.valueterminal("num","Num",fun n -> Number(int n))
G.lexterminal("DIV","/")
G.nonterminals(["T"; "F"; "E1"; "T1"; "Z"]);
G.production("E --> T E1",semact1)  
G.production("E1 --> - T E1",semact2)
G.production("E1 --> + T E1", semact3)
G.production("E1 --> ", semact4)
G.production("T --> F T1", semact5)
G.production("T1 --> * F T1",semact6)
G.production("T1 --> DIV F T1",semact7)
G.production("T1 -->",semact8)
G.production("F --> num",semact9)
G.production("F --> ( E )",semact10)
//G.production("Z --> T1 # ", semact10)

if TRACE then G.printgrammar()

// main
printf "Enter Expression: "
let lexer1 = Fussless.ll1lexer(Console.ReadLine()); // compile with ll1_lex.dll
let parser1 = make_parser(G,lexer1);

let result = parser1.parse()
if not(parser1.errors) then printfn "result = %A" result
