import os, subprocess
import csv

path = os.getcwd()
path = path+'/input'
rec_path = 'boxes.txt'

if os.path.isfile(rec_path):
    print("FOUND")
    with open(rec_path) as file:
        tsv_file = csv.reader(file, delimiter="\t")
        for line in tsv_file:
            s = line[0].split("-")
            if s[0] == "PMA" and line[2] == 'True':
                subprocess.call(["python3.8", "docscan_args.py", line[0], line[1]])
            if os.path.isfile(os.getcwd()+"/output/"+line[0]) == False:
                print("FAILED:", end="")
                print(line[0])
else:
    for f in os.listdir(path):
        subprocess.call(["python3.8", "docscan_args.py", f])
