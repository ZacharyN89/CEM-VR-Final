import os, subprocess
import csv
import sys

#Set input path
if len(sys.argv) > 1:
    path = os.getcwd()
    input_path = path+'\\'+sys.argv[1]
else:
    print("NO INPUT FOLDER GIVEN - INPUT FOLDER MUST BE IN SAME DIR")

#Only check bounding boxes if given a bounding box file to use.
if len(sys.argv) > 2:
    subprocess.call(["python", ".\check_boxes.py", sys.argv[2]])

#Types of headstone set by AI model: 1=Standard 2=Grass 3=Unique
#Only use bounding boxes if given a bounding box file
if os.path.isfile("boxes.txt") and len(sys.argv) > 2:
    #Create output folders
    if not os.path.exists(path+"\Standard_Output"):
        os.makedirs(path+"\Standard_Output")
    if not os.path.exists(path+"\Dynamic_Output"):
        os.makedirs(path+"\Dynamic_Output")

    with open("boxes.txt") as file:
        tsv_file = csv.reader(file, delimiter="\t")
        for line in tsv_file:
            check = 0
            if line[2] == '1' and os.path.isfile(input_path+"\\"+line[0]) == True:
                check = 1
                subprocess.call(["python", ".\docscan_args.py", input_path, line[0], "Standard_Output", "Standard", line[1]]) #Headstone version
                #If file was not created that means the program crashed, alert the user.
                if os.path.isfile(os.getcwd()+"\Standard_Output\\"+line[0]) == False and check == 1:
                    print("FAILED:", end="")
                    print(line[0])
            elif (line[2] == '2' or line[2] == '3') and os.path.isfile(input_path+"\\"+line[0]) == True:
                subprocess.call(["python", ".\docscan_args.py", input_path, line[0], "Dynamic_Output", "Dynamic", line[1]]) #Dynamic version
                #If file was not created that means the program crashed, alert the user.
                if os.path.isfile(os.getcwd()+"\Dynamic_Output\\"+line[0]) == False and check == 1:
                    print("FAILED:", end="")
                    print(line[0])
#If not using AI modeled bounding boxes run the non-bounding script on all headstone (Set to check for width and run different warping.)
else:
    #Create output folder
    if not os.path.exists(path+"\Output"):
        os.makedirs(path+"\Output")

    print("NO BOUNDING BOX FILE, RUNNING DYNAMIC DETECTION")
    for f in os.listdir(input_path):
        subprocess.call(["python", ".\docscan_args.py", input_path, f, "Output"])

#Run image combiner on standard headstone output folder.
if os.path.isfile("boxes.txt"):
    subprocess.call(["python", ".\ImageCombiner.py", "Standard_Output"])
else:
    subprocess.call(["python", ".\ImageCombiner.py", "Output"])
        
