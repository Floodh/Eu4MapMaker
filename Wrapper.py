import subprocess

def run_mapmaker():

    outcome = subprocess.call(['bin/Debug/net6.0/EU4MapMaker.exe'])
    outcome = 0
    print("ran C# .exe file, outcome :", outcome)
    return

def set_tags(array_str):

    textfile = open("input/tags.txt", 'w')    # mode w will delete all previous data
    text = ""
    for tag in array_str:
        text += tag + "\n"

    textfile.write(text)
    textfile.close()



if __name__ == '__main__':
    run_mapmaker()