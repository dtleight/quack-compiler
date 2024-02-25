target triple = "x86_64-pc-windows-msvc19.33.31629"
declare void @lambda7c_printint(i32)
declare void @lambda7c_printfloat(double)
declare void @lambda7c_printstr(i8*)



define i32 @main()
{
	mainstart:
		%x_1 = alloca i32
		store i32 3, i32* %x_1
		%y_2 = alloca i32
		store i32 2, i32* %y_2
		%z_3 = alloca i32
		store i32 1, i32* %z_3
		call void (i32, i32*, i32*, i32*) @fun(i32 2, i32* %x_1, i32* %y_2, i32* %z_3)
		call void (i32, i32*, i32*, i32*) @fun2(i32 17, i32* %x_1, i32* %y_2, i32* %z_3)
		call void (i32, i32*, i32*, i32*) @fun3(i32 105, i32* %x_1, i32* %y_2, i32* %z_3)
		%R_9 = call i32 (i32, i32*, i32*, i32*) @fun4(i32 3, i32* %x_1, i32* %y_2, i32* %z_3)
		ret i32 0

}

define void @fun(i32 %farg_y_6, i32* %x_1, i32* %y_2, i32* %z_3)
{
	funmain:
		%y_6 = alloca i32
		store i32 %farg_y_6, i32* %y_6
		%z_3_2 = load i32, i32* %z_3
		call void (i32) @lambda7c_printint(i32 %z_3_2)
		ret void

}

define void @fun2(i32 %farg_a_10, i32* %x_1, i32* %y_2, i32* %z_3)
{
	fun2main:
		%a_10 = alloca i32
		store i32 %farg_a_10, i32* %a_10
		%a_10_2 = load i32, i32* %a_10
		call void (i32) @lambda7c_printint(i32 %a_10_2)
		ret void

}

define void @fun3(i32 %farg_y_14, i32* %x_1, i32* %y_2, i32* %z_3)
{
	fun3main:
		%y_14 = alloca i32
		store i32 %farg_y_14, i32* %y_14
		%y_14_2 = load i32, i32* %y_14
		call void (i32) @lambda7c_printint(i32 %y_14_2)
		ret void

}

define i32 @fun4(i32 %farg_c_18, i32* %x_1, i32* %y_2, i32* %z_3)
{
	fun4main:
		%c_18 = alloca i32
		store i32 %farg_c_18, i32* %c_18
		%c_18_2 = load i32, i32* %c_18
		ret i32 %c_18_2

}

