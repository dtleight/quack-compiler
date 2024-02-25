module QuackTypes
open System
open System.Collections.Generic
open Option;
open Fussless
open RuntimeParser
open LLVMIR
open Quack

let listvec(l:'a list) = 
  let v = Vec<'a>();
  for element in l do
    v.Add(element);
  v;;
  
let addFunction(fName: string, f_args: (LLVMtype*string) list, rType: LLVMtype, funbody: BasicBlock list) =
  prog.functions.Add(
    {
      name = fName;
      formal_args = listvec(f_args);
      return_type = rType;
      body = listvec(funbody);
      attributes = Vec<string>();
      bblocator = HashMap<string,int>();  
    });;

let betterGEP(fName:string, fType: LLVMtype, fieldIndex: int ) =
  Verbatim(sprintf "%s = getelementptr %s this, i32 0, i32 %d" fName (typeToString(fType)) fieldIndex);; 
    
let initializeField(fName:string, fType: LLVMtype, fieldIndex: int ) =
  Verbatim(sprintf "%s = getelementptr %s this, i32 0, i32 %d" fName (typeToString(fType)) fieldIndex);; 
    

(*
  String Type
  
    Fields:
      0:Mem Address: i8*
      1:Length: i32 
      2:Size: i32 
      
    Methods:
      Create
      Append
      Delete
      Expand
*)

//Typedef
prog.global_declarations.Add(Structdec("LLVMstring", listvec([Pointer(Basic("i8")); Basic("i32"); Basic("i32")])));
let structPtr = Pointer(Userstruct("LLVMstring"));
//----------------------
//Method implementations
//----------------------
//Create Method
let createbb1 = {
  label = "createstart";
  ssamap = HashMap<string,string>();
  predecessors =  Vec<BasicBlock>();
  body = listvec([
    initializeField("address", structPtr,0);
    Store(Pointer(Basic("i8")), Nullptr, Register("address"),None);
    initializeField("length", structPtr,1);
    Store(Basic("i32"), Iconst(0), Register("length"),None);
    initializeField("size", structPtr, 2);
    Store(Basic("i32"), Iconst(0), Register("size"),None);
    Ret_noval
  ]);
};
  
addFunction("Create_String",[structPtr, "this"],Void_t,
  [
    createbb1
  ]
);

//Delete Method
let deletebb1 = {
  label = "delete_check";
  ssamap = HashMap<string,string>();
  predecessors =  Vec<BasicBlock>();
  body = listvec([
    betterGEP("delete_reg1", structPtr,0);
    Load("delete_reg2", Pointer(Basic("i8")), Register("delete_reg1"),None);
    Icmp("delete_reg3", "ne", Pointer(Basic("i8")), Register("delete_reg2"), Nullptr);
    Bri1(Register("delete_reg3"),"delete_run","delete_end");
  ]);
};
  //call void @free(i8* %2)
  //br label %free_close
let deletebb2 = {
  label = "delete_run";
  ssamap = HashMap<string,string>();
  predecessors =  Vec<BasicBlock>();
  body = listvec([
    Call(None, Void_t, [Pointer(Basic("i8"))], "free", [(Pointer(Basic("i8")),Register("2"))]);
    Br_uc("delete_end")
  ]);
};
  
let deletebb3 = {
  label = "delete_end";
  ssamap = HashMap<string,string>();
  predecessors =  Vec<BasicBlock>();
  body = listvec([
    Ret_noval
  ]);
};
  
addFunction("Delete_String",[(structPtr,"this")], Void_t,
  [
    deletebb1;
    deletebb2;
    deletebb3
  ]
);

let resizebb1 = {
  label = "resize_run";
  ssamap = HashMap<string,string>();
  predecessors =  Vec<BasicBlock>();
  body = listvec([
    Call(Some("maddress"), Pointer(Basic("i8")), [Pointer(Basic("i8"))], "malloc", [(Pointer(Basic("i8")),Register("value"))]);
   //Load variables
    betterGEP("1", structPtr,0);
   Load("buffer", Pointer(Basic("i8")), Register("1"),None);
    betterGEP("2", structPtr,1);
   Load("length", Basic("i32"), Register("2"),None);
   Call(Some("curr"), Pointer(Basic("i8")), [Pointer(Basic("i8"));Pointer(Basic("i8"));Basic("i32")], "memcpy", [(Pointer(Basic("i8")),Register("maddress"));(Pointer(Basic("i8")),Register("buffer"));(Basic("i32"),Register("length"))]);
   Call(None, Void_t, [Pointer(Basic("i8"))], "free", [(Pointer(Basic("i8")),Register("buffer"))]);
   Store(Pointer(Basic("i8")), Register("madress"), Register("address"),None);
   betterGEP("4", structPtr,2);
   Store(Basic("i32"), Register("arg"), Register("4"),None);
   Ret_noval
  ]);
};
  
addFunction("Resize_String",[(structPtr, "this"); (Basic("i8"),"arg")], Void_t,
  [
    resizebb1
  ]
);

let appendbb1 = {
  label = "append_run";
  ssamap = HashMap<string,string>();
  predecessors =  Vec<BasicBlock>();
  body = listvec([
   betterGEP("1", structPtr,1);
   Load("length", Pointer(Basic("i8")), Register("1"),None);
   betterGEP("2", structPtr,2);
   Load("size", Basic("i32"), Register("2"),None);
   Icmp("expand", "eq", Basic("i32"), Register("length"), Register("size"));
   Bri1(Register("expand"),"grow","append_stop")
  ]);
};

let appendbb2 = {
  label = "grow";
  ssamap = HashMap<string,string>();
  predecessors =  Vec<BasicBlock>();
  body = listvec([
    Binaryop("5", "imul", Basic("i32"), Register("size"), Iconst(2));
    Call(None, Void_t, [structPtr;Basic("i32")], "Resize_String", [(structPtr,Register("this")); (Basic("i32"),Register("5"))]);
    Br_uc("append_stop")
  ]);
};
let appendbb3 = {
  label = "append_stop";
  ssamap = HashMap<string,string>();
  predecessors =  Vec<BasicBlock>();
  body = listvec([
    betterGEP("6", structPtr,2);
    Load("madress", Pointer(Basic("i8")), Register("6"),None);
    betterGEP("7", Pointer(Basic("i8")),2);
    Store(Basic("i8"), Register("arg"), Register("7"),None);
    Binaryop("8", "add", Basic("i32"), Register("length"), Iconst(1));
    Store(Basic("i32"), Register("8"), Register("1"),None);
    Ret_noval
  ]);
};
  

addFunction("Append_String",[(structPtr,"this");(Basic("i8"),"arg")], Void_t,
  [
    appendbb1;
    appendbb2;
    appendbb3
  ]
);

(*
  Array Types
    Fields:
      Mem Address
      Length
      Mem Size

prog.global_declarations.Add(Structdec("LLVMarray",[Pointer(Basic("i8")), Basic("i32"), Basic("i32")]))
*)
//HashMap types

