target triple = "x86_64-pc-windows-msvc19.33.31629"
declare void @lambda7c_printint(i32)
declare void @lambda7c_printfloat(double)
declare void @lambda7c_printstr(i8*)

<<<<<<< HEAD
=======
@str1 =  constant [ 6 x i8] c"hello\00", align 1
%struct.bigstruct =  type {i32, i8, double}
>>>>>>> 76b5f934c56805a26126dc6b147cfd0769834802


define i32 @main()
{
	mainstart:
		%sum = alloca i32
		store i32 0, i32* %sum
		%i = alloca i32
		store i32 1, i32* %i
		%R0_4 = load i32, i32* %i
		%R0_5 = icmp sle i32 %R0_4, 100
		%R0_6 = zext i1 %R0_5 to i32
		%R0_7 = trunc i32 %R0_6 to i1
		br i1 %R0_7, label %WhileBody0_8, label %WhileEnd0_8

	WhileBody0_8:
		%R1_9 = load i32, i32* %sum
		%R1_10 = load i32, i32* %i
		%R1_11 = add i32 %R1_9, %R1_10
		store i32 %R1_11, i32* %sum
		%R1_13 = load i32, i32* %i
		%R1_14 = add i32 %R1_13, 1
		store i32 %R1_14, i32* %i
		%R1_16 = load i32, i32* %i
		%R1_17 = icmp sle i32 %R1_16, 100
		%R1_18 = zext i1 %R1_17 to i32
		%R1_19 = trunc i32 %R1_18 to i1
		br i1 %R1_19, label %WhileBody0_8, label %WhileEnd0_8

	WhileEnd0_8:
		%R2_21 = load i32, i32* %sum
		call void (i32) @lambda7c_printint(i32 %R2_21)
		ret i32 0

}

