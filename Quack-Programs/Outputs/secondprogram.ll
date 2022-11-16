target triple = "x86_64-pc-windows-msvc19.33.31629"
declare void @lambda7c_printint(i32)
declare void @lambda7c_printfloat(double)
declare void @lambda7c_printstr(i8*)

@str1 =  constant [6 x i8] c"hello\00", align 1
%struct.bigstruct =  type {i32, i8, double}
@SVAR5_38 =  constant [11 x i8] c"The gcd = \00", align 1
@SVAR5_44 =  constant [2 x i8] c"\0a\00", align 1


define i32 @main()
{
	mainstart:
		%a = alloca i32
		store i32 12, i32* %a
		%b = alloca i32
		store i32 18, i32* %b
		%R0_4 = load i32, i32* %a
		%R0_5 = icmp ne i32 %R0_4, 0
		%R0_6 = zext i1 %R0_5 to i32
		%R0_7 = load i32, i32* %b
		%R0_8 = icmp ne i32 %R0_7, 0
		%R0_9 = zext i1 %R0_8 to i32
		%R0_10 = and i32 %R0_6, %R0_9
		%R0_11 = trunc i32 %R0_10 to i1
		br i1 %R0_11, label %WhileBody0_12, label %WhileEnd0_12

	WhileBody0_12:
		%R1_13 = load i32, i32* %a
		%R1_14 = load i32, i32* %b
		%R1_15 = icmp sgt i32 %R1_13, %R1_14
		%R1_16 = zext i1 %R1_15 to i32
		%R1_17 = trunc i32 %R1_16 to i1
		br i1 %R1_17, label %IfTrue1_18, label %IfFalse1_18

	IfTrue1_18:
		%R2_19 = load i32, i32* %a
		%R2_20 = load i32, i32* %b
		%R2_21 = srem i32 %R2_19, %R2_20
		store i32 %R2_21, i32* %a
		br label %EndIf1_18

	IfFalse1_18:
		%R3_24 = load i32, i32* %b
		%R3_25 = load i32, i32* %a
		%R3_26 = srem i32 %R3_24, %R3_25
		store i32 %R3_26, i32* %b
		br label %EndIf1_18

	EndIf1_18:
		%R4_29 = load i32, i32* %a
		%R4_30 = icmp ne i32 %R4_29, 0
		%R4_31 = zext i1 %R4_30 to i32
		%R4_32 = load i32, i32* %b
		%R4_33 = icmp ne i32 %R4_32, 0
		%R4_34 = zext i1 %R4_33 to i32
		%R4_35 = and i32 %R4_31, %R4_34
		%R4_36 = trunc i32 %R4_35 to i1
		br i1 %R4_36, label %WhileBody0_12, label %WhileEnd0_12

	WhileEnd0_12:
		%A5_38 = getelementptr inbounds [11 x i8], [11 x i8]* @SVAR5_38, i64 0, i64 0
		call void (i8*) @lambda7c_printstr(i8* %A5_38)
		%R5_40 = load i32, i32* %a
		%R5_41 = load i32, i32* %b
		%R5_42 = add i32 %R5_40, %R5_41
		call void (i32) @lambda7c_printint(i32 %R5_42)
		%A5_44 = getelementptr inbounds [2 x i8], [2 x i8]* @SVAR5_44, i64 0, i64 0
		call void (i8*) @lambda7c_printstr(i8* %A5_44)
		ret i32 0

}

