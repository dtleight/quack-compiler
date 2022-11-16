target triple = "x86_64-pc-windows-msvc19.33.31629"
declare void @lambda7c_printint(i32)
declare void @lambda7c_printfloat(double)
declare void @lambda7c_printstr(i8*)

@str1 =  constant [ 6 x i8] c"hello\00", align 1
%struct.bigstruct =  type {i32, i8, double}


define i32 @main()
{
	mainstart:
		%x = alloca i32
		store i32 1, i32* %x
		%R0_2 = load i32, i32* %x
		%R0_3 = add i32 %R0_2, 1
		store i32 %R0_3, i32* %x
		%R0_5 = load i32, i32* %x
		call void (i32) @lambda7c_printint(i32 %R0_5)
		ret i32 0

}

