#include<stdio.h>
#include<stdlib.h>
#include<stdint.h>
typedef int32_t i32;

i32 lambda7c_cin() {
  i32 x;
  printf(">> ");
  scanf("%d",&x); // for windows
  //scanf_s("%d",&x);  
  return x;
}//lambda7c_cin()
i32 lambda7c_expt(i32 x, i32 n)  {  //x**n
  i32 ax = 1;
  i32 fct = x;
  while (n>0)
    {
      if (n%2==1) ax*=fct;
      fct *=fct;
      n/=2;
    }
  return ax;          
}
void lambda7c_printint(i32 x) {
  printf("%d",x);
}
void lambda7c_printfloat(double x) {
  printf("%f",x);
}
void lambda7c_printstr(char* x) {
  printf("%s",x);
}
void lambda7c_newline() {
  printf("\n");
}
