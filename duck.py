# -*- coding: utf-8 -*-
from termcolor import colored
class bcolors:
    YELLOW = '\033[93m'
    ORANGE = '\033[33m'
  
color_duck = '''                                                                                        
                                            bbbbbbbbbb                                  
                                          bbyyyyyyyyyybb                                
                                        bbyyyyyyyyyyyyyybb                              
                                        bbyyyyyyyybbbbyybbbbbbbbbb                      
                            bb          bbyyyyyyyybbbbyybboooooobb                      
                          bbyybb        bbyyyyyyyyyyyyyybboooooobb                      
                          bbyyyybb      bbyyyyyyyyyyyyyybbbbbbbb                        
                        bbyyyyyyyybb      bbyyyyyyyyyyyybb                              
                        bbyyyyyyyybbbbbbbbbbbbyyyyyyyybb                                
                        bbyyyyyyyybbyyyyyyyyyyyyyyyyyyyybb                              
                        bbyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyybb                            
                        bbyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyybb                            
                        bbyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyybb                            
                        bbyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyybb                            
                        bbyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyybb                            
                        bbyyyyyyyyyyyyyyyyyyyyyyyyyyyyyybb                              
                          bbyyyyyyyyyyyyyyyyyyyyyyyyyybb                                
                            bbbbbbyyyyyyyyyyyyyyyybbbb                                  
                                  bbbbbbbbbbbbbbbb                                      
'''                              
output_duck = ''''''
colorMap = {
"b":(bcolors.YELLOW + "░"),
"y":(bcolors.YELLOW + "█"),
"o":(bcolors.ORANGE + "█"),
" ": " ",
"\n": "\n"
}
for x in color_duck:
  output_duck += colorMap[x] 
print(output_duck)
