target triple = "x86_64-pc-windows-msvc19.33.31629"
declare void @lambda7c_printint(i32)
declare void @lambda7c_printfloat(double)
declare void @lambda7c_printstr(i8*)
declare i32 @lambda7c_cin()
declare i8* @malloc(i32)
declare void @free(i8*)
declare i8* @memcpy(i8*, i8*, i32)



define i32 @main()
{
	mainstart:
		%a_1 = alloca i32
		store i32 3, i32* %a_1
		ret i32 0

}

define i32 @TestMain()
{
	mainstart:
		%No variable binding exists = alloca void
		store void 0, void* %No variable binding exists
		%No variable binding exists = alloca void
		store void 0, void* %No variable binding exists
		%No variable binding exists = alloca void
		store void 0, void* %No variable binding exists
		%No variable binding exists = alloca void
		store void 0, void* %No variable binding exists
		call void (i32) @lambda7c_printint(i32 3)
		%No variable binding exists = alloca void
		store void default, void* %No variable binding exists
		ret i32 0

}

