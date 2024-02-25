target triple = "x86_64-pc-windows-msvc19.33.31629"
declare void @lambda7c_printint(i32)
declare void @lambda7c_printfloat(double)
declare void @lambda7c_printstr(i8*)



define i32 @main()
{
	mainstart:
		%pi_1 = alloca f64
		store f64 3.1415927, f64* %pi_1
		ret i32 0

}

