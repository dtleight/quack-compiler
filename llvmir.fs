module LLVMIR
open System;
open Option;
open System.Collections;

(* Abstract representation of Simplified LLVM IR:  (includes assignment)

LLVM looks like a monster but actually a lot of the stuff you see in a file
produced by   clang -emit-llvm -S   is optional.  Also, not all possible
instructions are as important as others, especially all the possible forms
of the notorious 'getelementptr' instruction. So the abstract representation
types defined here also define a small subset of the LLVM instruction set.
It includes what I believe are the essential instructions that you will need
to write a basic compiler.  We may have to tweak or add a few new cases as
necessary.  The main types are:

LLVMtype : represents LLVM type expressions such as i32
LLVMexpr : expressions can be registers, %r1, or constants 1, etc
Instruction: this is the most important type to study and understand
LLVMdeclaration : for global declarations such as for string literals
BasicBlock : all instructions are in basic blocks, each block has a label
LLVMfunction : represents a function declaration, which is always global
LLVMprogram : top level structure consisting of declarations and functions

There is (currently) only one function in this file: 'destination', which
retrives the destination register of an instruction as an Option<string>.
Not all instructions have destinations.  For example, for %r1 = add i32 ...
the destination is Some("r1").
*)

type Vec<'T> = ResizeArray<'T>
type HashMap<'K,'V> = System.Collections.Generic.Dictionary<'K,'V>
type Conslist<'K> = 'K list

type LLVMtype =
  | Basic of string     // i32: Basic("i32")
  | Pointer of LLVMtype //i32*:  Pointer(Basic("i32"))
  | Array_t of int*LLVMtype  // [5 x i8], Array_t(5,Basic("i8"))
  | Func_t of LLVMtype list*LLVMtype
  | Userstruct of string
  | Ellipsis  // special case for optional args, like printf(i8*,...)
  | Void_t

type LLVMexpr =
  | Iconst of int
  | Sconst of string
  | Fconst of float
  | I1const of bool  // booleans of type i1 are true and false
  | Register of string   // %r1
  | Global of string     // @str1
  | Label of string      // not really used
  | Novalue     // default

type Instruction =  
  // terminator instructions that ends a Basic Block:
  | Ret of LLVMtype*LLVMexpr  // ret i32 4
  | Ret_noval                 // ret
  | Br_uc of string   // unconditional jump: br label %label1
  | Bri1 of LLVMexpr*string*string //always on i1
  // memory ops: store i32 3, i32* %a, optional "align 4", type i32* is omitted
  | Load of string*LLVMtype*LLVMexpr*Option<string>
  | Store of LLVMtype*LLVMexpr*LLVMexpr*Option<string>  //pointer type omitted
  | Alloca of string*LLVMtype*Option<int> // %r1 = alloca i64, align 8
  // ALU ops, always begins with a destination : %r1 = ...
  | Binaryop of string*string*LLVMtype*LLVMexpr*LLVMexpr
  | Unaryop of string*string*Option<string>*LLVMtype*LLVMexpr // fneg float %r1
  // casting operations like %r2 = zext i32 %r1 to i64
  | Cast of string*string*LLVMtype*LLVMexpr*LLVMtype
  // comparison and selection ops, phi
  | Icmp of string*string*LLVMtype*LLVMexpr*LLVMexpr //1st string is dest
  | Fcmp of string*string*LLVMtype*LLVMexpr*LLVMexpr
  | SelectTrue of string*LLVMtype*LLVMexpr*LLVMexpr
  | Phi2 of string*LLVMtype*LLVMexpr*string*LLVMexpr*string
  | Phi of string*LLVMtype*Conslist<(LLVMexpr*string)> //not as common as Phi2
  // function call: option dest,ret type,
  | Call of Option<string>*LLVMtype*Conslist<LLVMtype>*string*Conslist<(LLVMtype*LLVMexpr)>
  // simplified forms of getelementptr for array address and struct field
  | Arrayindex of string*int*LLVMtype*LLVMexpr*LLVMexpr
  | Structfield of string*LLVMtype*LLVMexpr*LLVMexpr
  // other llvm instructions not covered by above must be encoded as:
  | Verbatim of string //generic "other" instruction, default case, comments

(*
The instruction set represented by the F# 'Instruction' type is a simplified
subset of LLVM instructions, encoding only the most commonly used forms that
you will encounter while writing LLVM program by hand, or when generating LLVM
code.  All other LLVM instructions must be encoded with the union-variant
Instruction.Verbatim.

For many of these instructions, information that's most common or obvious
are omitted.  For example, in a conditional branch instruction such as

  br i1 %r1, label %label1, label %label2

The type of of %r1 is almost always i1, and is thus omitted from the abstract
representation. Such an instruction is encoded as

  Instruction.Bri1(LLVMexpr.Register("r1"), "label1", "label2")

In case the type is not 'i1', then the instruction would have to be encoded
as Verbatim("...").

Similar, for a load operation

  %r2 = load i32, i32* %r1  ;no , align4 at the end

The type 'i32*' is omitted since it is derivable from i32.  I don't think
it's even possible to use another type.  The "align 4" is optional. The
encoding of this instruction is

Instruction.Load("r2",LLVMtype.Basic_t("i32"),LLVMexpr.Register("r1"),None)

Replace None with Some("align 4") if desired.

For instructions that commonly returns a value, the first string is the
destination register:

  %r3 = add i32 %r1, 1

is encoded as

  Instruction.Binaryop("r3","add",Basic("i32"),Register("r1"),Iconst(1))

The Call (function call) instruction has an optional destination, in case
there is no return value for the function being called.  The Call instruction
has another optional component: a list of types that's required for calling
certain types of functions like printf.  The list is empty if no type is
specified.

  %r2 = call i32 @func1(i8* %r1, double 1.0)

is encoded as

  Call(Some("r2"),Basic("i32"),[],"func1",[(Pointer(Basic("i8")),Register("r1")),(Basic("double"),Fconst(1.0))])

The empty list [] represents that no additional type information is needed.
On the other hand, calls to printf requires a a type specifier for printf:

  call i32 (i8*,...) @printf(i8* %r2, i32 %r1)

is encoded as

  Call(None,Basic("i32"),[Pointer(Basic("i8")),Ellipsis],"printf",[(Pointer(Basic("i8")),Register("r2")), (Basic("i32"),Register("r1"))])

The above instruction is probably preceeded by something like:

  %r2 = getelementptr inbounds [9 x i8], [9 x i8]* @str1, i64 0, i64 0 

The getelementptr instruction has grown from a relatively easy to understand
operation to an absolute monster.  Only two commonly used forms are included
(others have to be made Verbatim). This above assignment to %r2 is encoded as

  Arrayindex("r2",9,Basic("i8"),Global("str1"),Iconst(0))

This is used to find the address of an array element, so the above is the
address of str1[0] in an array of 9 bytes (char[9]): The last expression
is the array index.  Note that getelementptr always computes an ADDRESS:
it does not extract the value, which must be done with 'load'.

The other form of getelementptr encoded as an Instruction is

  %r2=getelementptr inbounds %bigstruct, %bigstruct* %r1 i32 0, i32 1

This is encoded as

  Structfield("r2",Usertype("bigstruct"),Register("r1"),Iconst(1))

For example, given struct bigstruct { int x; char y; } b;  &b.y can be
retrieved with this instruction.  Nested struct access (a.b.c) will require
multiple Structfield instructions.

//// Examples of all other instructions (abstract form : concrete form)

Instruction.Ret(LLVMtype.Basic("i32"),LLVMexpr.Register("r1"))  : ret i32 %r1
Ret_noval   :
    ret
Br_uc("loopstart")  :
    br label %loopstart
Bri1(Register("r2"),"label1","label2") :
    br i1 %r2, label %label1, label %label2
Store(Basic("i8"),Iconst(1),Register("r1"),Some("align 1")) :
    store i8 1, i8* %r1, align 1 ; align is not really needed
Alloca("r1",Basic("i32"),Some("align 4")) : %r1 = alloca i32, align 4
    ;alloca allocates on the stack.  Call function malloc for heap allocation
Unaryop("r2","fneg",None,Basic("float"),Register("r1")) : fneg float %r1
    ; fneg is the only unary operation, option is for "fast-math flags"
    ; ... but "fast-math" is not for Hofstra students :-0
Cast("r2","bitcast",Pointer(Basic("i8")),Register("r1"),Pointer(Basic("i32"))):
    %r2 = bitcast i8* %r1 to i32*
Icmp("r2","sle",Basic("i32"),Register("r1"),Iconst(1)) :
    %r2 = icmp sle i32 %r1, 1   ;sle is signed <=
Fcmp("r2","oeq",Basic("float"),Fconst(4.0),Register("r1")) :
    %r2 = fcmp oeq float 4.0, %r1   ;oeq is ordered equality
SelectTrue("r3",Basic("i32"),Register("r1"),Register("r2")) :
    %r3 = select i1 true, i32 %r1, i32 %r2 ;why would you select false
Phi2("r2",Basic("i32"),Iconst(1),"block1",Register("r1"),"block2") :
    %r2 = phi i32 [1, %block1], [%r1, "block2"]
Phi("3",Basic("i8"),[(Iconst(1),"bb1"),(Iconst(2),"bb2"),(Iconst(3),"bb3")] :
    %3 = phi i8 [1, %bb1], [2, %bb2], [3, %bb3] ;better to stick to Phi2
Call(Some("r1"),Basic("i64"),"factorial",[(Basic("i8"),Iconst(8))])
    %r1 = call i64 @factorial(i8 6)
Verbatim("; comments start with ; so you can add ; to end of line")
    
*)

// extracts the destination register from an instruction, returns string option
let destination = function
  | Load(s,_,_,_) | Alloca(s,_,_) | Unaryop(s,_,_,_,_) -> Some(s)
  | Binaryop(s,_,_,_,_) | Call(Some(s),_,_,_,_) -> Some(s)
  | Cast(s,_,_,_,_) | Icmp(s,_,_,_,_) | Fcmp(s,_,_,_,_) -> Some(s)
  | SelectTrue(s,_,_,_) | Phi2(s,_,_,_,_,_) | Phi(s,_,_) -> Some(s)
  | Arrayindex(s,_,_,_,_) | Structfield(s,_,_,_) -> Some(s) 
  | _ -> None  // includes case for Verbatim


type LLVMdeclaration =
  | Globalconst of string*LLVMtype*LLVMexpr*Option<string>
  | Externfunc of LLVMtype*string*Vec<LLVMtype>
  | Structdec of string*Vec<LLVMtype>
  | Verbatim_dec of string // cases not covered by above

(*
   LLVM declarations are special cases of globally scoped instructions.
   LLVM allows forward reference

 Globalconst("str1",Array_t(6,Basic("i8")),Sconst("hello\00"),Some("align 1")) :
    @str1 = constant [6 x i8] c"hello\00", align 1
 Externfunc(Basic("i32"),"printf",[Pointer(Basic("i8")),Ellipsis]) :
    declare i32 @printf(i8*, ...)
 Structdec("bigstruct",[Basic("i32"),Basic("i8"),Basic("double")]) :
    %struct.bigstruct = type { i32, i8, double }
*)


type BasicBlock =
  {
     label: string;
     body: Vec<Instruction>; // last instruction must be a terminator
     predecessors : Vec<BasicBlock>; //control-flow graph, not used for now
     ssamap: HashMap<string,string>;
  }

type LLVMFunction =
  {
     name: string;
     formal_args : Vec<(LLVMtype*string)>;
     return_type : LLVMtype;  // only basic and pointer types can be returned
     body: Vec<BasicBlock>;
     attributes : Vec<string>; // like "dso_local", "#1", or ["","#1"] 
     bblocator: HashMap<string,int>;
  }

type LLVMprogram =
  {
     preamble : string;   // arbitrary stuff like target triple
     global_declarations : Vec<LLVMdeclaration>;
     functions: Vec<LLVMFunction>;
     postamble : string;  // stuff you don't want to know about
  }
  
let rec map(vec: Vec<'a>, f: 'a -> 'b): Vec<'b> =
  let retVec = Vec<'b>();
  for x in vec do
    retVec.Add(f(x))
  retVec;;
let trim(str:string) = 
  str.[1..str.Length-2];;

let rec listmap(vec: 'a list, f: 'a -> 'b): string =
  sprintf "(%s)" (trim(trim(sprintf "%A" (vec |> List.map f))));; 
  
let rec flatten(vec: Vec<'a>, initial: 'a) = 
  let mutable output = initial
  for x in vec do
    output <- output + x
  output;; 
  
let stringify(x: Vec<string>, o: string, c:string) =
   sprintf "%s%s%s" o (String.concat ", " x) c;;
   
let exprToString(expr: LLVMexpr)  =
  match expr with
    | Iconst(i) -> i.ToString();
    | Sconst(s) -> s;
    | Fconst(f) -> f.ToString();
    | I1const(b) -> b.ToString();  // booleans of type i1 are true and false
    | Register(x) -> "%" + x;   // %r1
    | Global(x) -> "@" + x;     // @str1
    | Label(x) -> x;      // not really used
    | Novalue -> "default";;    // default

let rec typeToString(typeExpr: LLVMtype) =
  match typeExpr with
    | Basic(x) -> x;
    | Pointer(x) -> typeToString(x) + "*";
    | Array_t(len,x) -> sprintf  "[%d x %s]" len (typeToString(x));
    | Userstruct(x) -> x;
    | Ellipsis -> "Ellipsis";  // special case for optional args, like printf(i8*,...)
    | Void_t -> "";
    

let instructionToString(instruction: Instruction) = 
  match instruction with
    // terminator instructions that ends a Basic Block: - Done
    | Ret(typeExpr, expr) -> sprintf "ret %s %s" (typeToString(typeExpr))  (exprToString(expr));
    | Ret_noval -> "ret";
    | Br_uc(label) -> sprintf "br label %s" ("%"+label);
    | Bri1 (expr, label1, label2) -> sprintf "br i1 %s, label %s, label %s" (exprToString(expr)) ("%"+label1)  ("%"+label2);
    // memory ops: store i32 3, i32* %a, optional "align 4", type i32* is omitted
    | Load(var, typeExpr, expr, None) -> (sprintf "%s = load %s, %s %s" ("%"+var) (typeToString(typeExpr)) (typeToString(Pointer(typeExpr))) (exprToString(expr))) 
    | Load(var, typeExpr, expr, Some(post)) -> (sprintf "%s = load %s, %s %s, %s" ("%"+var) (typeToString(typeExpr)) (typeToString(Pointer(typeExpr))) (exprToString(expr))) post
    | Store(typeExpr, expr1, expr2, Some(post)) -> sprintf "store %s %s, %s %s, %s" (typeToString(typeExpr))(exprToString(expr1)) (typeToString(Pointer(typeExpr))) (exprToString(expr2)) post
    | Store(typeExpr, expr1, expr2, None) -> sprintf "store %s %s, %s %s" (typeToString(typeExpr)) (exprToString(expr1)) (typeToString(Pointer(typeExpr))) (exprToString(expr2))
    | Alloca(var, typeExpr, Some(post))-> sprintf "%s = alloca %s, %s" ("%"+var) (typeToString(typeExpr)) (post.ToString());
    | Alloca (var, typeExpr, None)-> sprintf "%s = alloca %s" ("%"+var) (typeToString(typeExpr));
    // ALU ops, always begins with a destination : %r1 = ...
    | Binaryop(var, op, typeExpr, expr1, expr2) -> sprintf "%s = %s %s %s, %s" ("%"+var) op (typeToString(typeExpr)) (exprToString(expr1)) (exprToString(expr2));  
    | Unaryop (var, op, None, typeExpr, expr1) ->  sprintf "%s = %s %s %s" ("%"+var) op (typeToString(typeExpr)) (exprToString(expr1));
    // casting operations like %r2 = zext i32 %r1 to i64
    | Cast(var, op, typeExpr, expr, typeExpr2) -> sprintf "%s = %s %s %s to %s" ("%"+var) op (typeToString(typeExpr))  (exprToString(expr)) (typeToString(typeExpr2));
    // comparison and selection ops, phi
    | Icmp(var, op, typeExpr, dest, expr2) -> sprintf "%s = icmp %s %s %s, %s" ("%" + var) op (typeToString(typeExpr)) (exprToString(dest)) (exprToString(expr2)); //Not yet finished
    | Fcmp(var, op, typeExpr, dest, expr2) -> sprintf "%s = fcmp %s %s %s, %s" ("%" + var) op (typeToString(typeExpr)) (exprToString(dest)) (exprToString(expr2)); 
    | SelectTrue(var, typeExpr, expr1, expr2) -> "%" + var + " = select i1 true, " + typeToString(typeExpr) + " " + exprToString(expr1) + ", " + typeToString(typeExpr) + " " + exprToString(expr2);
    //PHI Statements
    | Phi2(var, typeExpr, expr1, label1, expr2, label2) -> "%" + var + " = phi " + typeToString(typeExpr) + " [" + exprToString(expr1) + ", %" + label1 + "], [" + exprToString(expr2) + ", %" + label2 + "]";
    | Phi(var, typeExpr, list) -> "%" + var + " = phi " + typeToString(typeExpr); //%3 = phi i8 [1, %bb1], [2, %bb2], [3, %bb3] ;better to stick to Phi2
    //Function calls
    | Call(Some(prefix),typeExpr, typeList,func,args) -> sprintf "%s = call %s %s @%s%s" prefix (typeToString(typeExpr)) (listmap(typeList, fun x -> typeToString(x))) func (listmap(args, fun (x,y) -> sprintf "%s %s" (typeToString(x)) (exprToString(y))))
    | Call(None,typeExpr, typeList,func,args) -> sprintf "call %s %s @%s%s" (typeToString(typeExpr)) (listmap(typeList, fun x -> typeToString(x))) func (listmap(args, fun (x,y) -> sprintf "%s %s" (typeToString(x)) (exprToString(y))))
    // simplified forms of getelementptr for array address and struct field
    | Arrayindex(var, value, typeExpr, expr1, expr2) -> "%" + var + " = getelementptr inbounds [" + (value.ToString()) + " x " + (typeToString(typeExpr)) + "], [" + (value.ToString()) + " x " + (typeToString(typeExpr)) + "]* " +  (exprToString(expr1)) + ",  i64 0, i64 " + (exprToString(expr2));
    | Structfield(var, typeExpr, expr1, expr2) -> sprintf "%s = getelementptr inbounds %s, %s %s, i64 %s, i64 %s" ("%"+var) (typeToString(typeExpr)) (typeToString(Pointer(typeExpr))) (exprToString(expr1)) (exprToString(expr2)) (exprToString(expr2))
    | Verbatim(content) -> content; //generic "other" instruction, default case, comments
    | _ -> "Instruction";;
  
let declarationToString(declaration: LLVMdeclaration) =
  match declaration with
    | Globalconst(name,typeExpr,expr, Some(strPos)) -> (sprintf "@%s =  constant %s c\"%s\", %s" name (typeToString(typeExpr)) (exprToString(expr)) strPos);
    | Globalconst(name,typeExpr,expr, None) -> (sprintf "@%s =  constant %s c\"%s\"" name (typeToString(typeExpr)) (exprToString(expr)));
    | Externfunc(typeExpr,name,types) -> (sprintf "%s %s %s" (typeToString(typeExpr)) name (stringify(map(types, fun x -> typeToString(x)),"[","]"))) ;
    | Structdec(name,types) -> 
        sprintf "%sstruct.%s =  type %s" "%" name (stringify(map(types, fun x -> typeToString(x)),"{","}"));
    | Verbatim_dec(content) -> content;
    
let functionToString(func: LLVMFunction) =
  sprintf "define %s @%s%s\n{\n%s}\n" (typeToString(func.return_type)) func.name (stringify(map(func.formal_args, fun (a,b)-> sprintf "%s:%s" (typeToString(a)) b),"(",")")) (flatten(map(func.body, (fun x-> sprintf "\t%s:\n%s\n" (x.label) (flatten(map(x.body, fun y-> (sprintf "\t\t%s\n" (instructionToString(y)))),"")))),""));;
  
let programToString(program: LLVMprogram) =
  let declarations = flatten(map(program.global_declarations, fun x -> sprintf "%s\n" (declarationToString(x))),"")
  let functions = flatten(map(program.functions, fun x -> sprintf "%s\n" (functionToString(x))),"")
  sprintf "%s\n\n%s\n\n%s" program.preamble declarations functions;;

 let prog = 
  {
    preamble = "target triple = \"x86_64-pc-windows-msvc19.33.31629\"\ndeclare void @lambda7c_printint(i32)\ndeclare void @lambda7c_printfloat(double)\ndeclare void @lambda7c_printstr(i8*)";
    global_declarations = Vec<LLVMdeclaration>();
    functions = Vec<LLVMFunction>();
    postamble = ""
  };;
