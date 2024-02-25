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
		%R_0 = icmp eq i32 1, 1
		%R_1 = zext i1 %R_0 to i32
		%R_2 = trunc i32 %R_1 to i1
		br i1 %R_2, label %IfTrue_3, label %IfFalse_3

	IfTrue_3:
		call void (i32) @lambda7c_printint(i32 1)
		%R_5 = icmp eq i32 2, 1
		%R_6 = zext i1 %R_5 to i32
		%R_7 = trunc i32 %R_6 to i1
		br i1 %R_7, label %IfTrue_8, label %IfFalse_8

	IfTrue_8:
		call void (i32) @lambda7c_printint(i32 2)
		br label %EndIf_8

	IfFalse_8:
		call void (i32) @lambda7c_printint(i32 3)
		%R_12 = icmp eq i32 2, 1
		%R_13 = zext i1 %R_12 to i32
		%R_14 = trunc i32 %R_13 to i1
		br i1 %R_14, label %IfTrue_15, label %IfFalse_15

	IfTrue_15:
		call void (i32) @lambda7c_printint(i32 2)
		br label %EndIf_15

	IfFalse_15:
		call void (i32) @lambda7c_printint(i32 3)
		br label %EndIf_15

	EndIf_15:
		br label %EndIf_8

	EndIf_8:
		br label %EndIf_3

	IfFalse_3:
		%R_22 = icmp eq i32 2, 1
		%R_23 = zext i1 %R_22 to i32
		%R_24 = trunc i32 %R_23 to i1
		br i1 %R_24, label %IfTrue_25, label %IfFalse_25

	IfTrue_25:
		call void (i32) @lambda7c_printint(i32 2)
		br label %EndIf_25

	IfFalse_25:
		call void (i32) @lambda7c_printint(i32 3)
		br label %EndIf_25

	EndIf_25:
		call void (i32) @lambda7c_printint(i32 2)
		br label %EndIf_3

	EndIf_3:
		ret i32 0

}

