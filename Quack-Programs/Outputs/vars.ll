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
		%x_1 = alloca i32
		store i32 1, i32* %x_1
		call void (i32, i32*) @x_0(i32 2, i32* %x_1)
		ret i32 0

}

define i32 @f(i32 %farg_y_4, i32* %x_1)
{
	fmain:
		%y_4 = alloca i32
		store i32 %farg_y_4, i32* %y_4
		%y_4_2 = load i32, i32* %y_4
		%x_1_3 = load i32, i32* %x_1
		%R_4 = add i32 %y_4_2, %x_1_3
		ret i32 %R_4

}

define void @x_0(i32 %larg_x, i32* %x_1)
{
	x_0main:
		%x_6 = alloca i32
		store i32 %larg_x, i32* %x_6
		%x_6_2 = load i32, i32* %x_6
		%R_3 = call i32 (i32, i32*, i32*, i32*) @g(i32 %x_6_2, i32* %test_11, i32* %x_1, i32* %x_6)
		%test_16 = alloca i32
		store i32 %R_3, i32* %test_16
		%test_16_6 = load i32, i32* %test_16
		call void (i32) @lambda7c_printint(i32 %test_16_6)
		ret void

}

define i32 @g(i32 %farg_y_14, i32* %test_11, i32* %x_1, i32* %x_6)
{
	gmain:
		%y_14 = alloca i32
		store i32 %farg_y_14, i32* %y_14
		%y_14_2 = load i32, i32* %y_14
		%R_3 = call i32 (i32, i32*) @f(i32 %y_14_2, i32* %x_1)
		%x_6_4 = load i32, i32* %x_6
		%R_5 = add i32 %R_3, %x_6_4
		ret i32 %R_5

}

