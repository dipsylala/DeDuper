# DeDuper

Takes an input directory and iterates through the structure, placing the unique files in a directory structure based on their name and hash

Example Input directory
input/
├─ samefile.dll
├─ subdirectory/
│  ├─ samefile.dll
│  ├─ uniquefile.dll

Example Output directory

output
output/
├─ 00/
│  ├─ samefile.dll - 00f1f4e380f699b0bbb5998986e9c3a1/
│  │  ├─ samefile.dll
├─ ac/
│  ├─ uniquefile.dll - accb867d1022208b6244a1504ad61c6e/
│  │  ├─ uniquefile.dll
