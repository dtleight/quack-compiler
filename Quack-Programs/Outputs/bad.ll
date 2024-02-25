target triple = "x86_64-pc-windows-msvc19.33.31629"
declare void @lambda7c_printint(i32)
declare void @lambda7c_printfloat(double)
declare void @lambda7c_printstr(i8*)
declare i8* @malloc(i32)
declare void @free(i8*)
declare i8* @memcpy(i8*, i8*, i32)



define i32 @main()
{
	mainstart:
		%f_1 = alloca i32
		store i32 3, i32* %f_1
		%x_2 = alloca i32
		store i32 2, i32* %x_2
		%y_3 = alloca i32
		store i32 7, i32* %y_3
		ret i32 0

}

