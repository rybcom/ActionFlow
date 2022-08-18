---

![](action_flow_image.png)

---

# ActionFlow

* process-executor 

* file operation manager

* controling action flow by conditions

---

## Action flow project structure

> Create action_flow project file and save it as **.af** file 

```javascript
project:
{
    name:'template project',
    desc:'project focuses on the correct using of commands ',

    execution:
    {
    	controlflow:
        {
            condition:'dialog',
            dialogtext:'yer or no ?',
            yes:
            {
            	wait:'1000',
            	showdialog:'how are you doing ?'
            	execute:'||bin||/daily_remarks/daily_remarks.exe',
            },
            
            no:
            {
				execute:
				{
					filename:'||defaultbrowser||',
					params:'www.aktuality.sk'
				},
            }    
        },

		deletefile:'||mroot||/temp/tttt/gear_watch/connect.bat',

    	deletefiles:
        {
            source:'||mroot||/temp/tttt',
            recursive:'false',
            pattern:'^*(.bat)'
        },

        deletefiles:'||mroot||/temp/tttt',

		zipfolder:
        {
            source:'||mroot||/test',
            zipfile:'||mroot||/temp/ahoij.zip'
        },
    }
}

```

---

## Example running actionflow file

> set **action_flow.exe** as  default app for **.af** files

or run

```console

action_flow.exe testing_project_file.af

```

## Available actions

- [Wait](#wait)
- [Execute](#execute)
- [Copy folder](#copyfolder)
- [Copy file](#copyfile)
- [Delete files](#deletefiles)
- [Delete folders](#deletefolders)
- [Zip folder](#zipfolder)
- [Show dialog](#showdialog)

---

### wait

```xml
<action
	type="wait" 

	desc="Slow down the process for millions of milliseconds"
	duration_ms="" 
/>
```

### execute

```xml
<action
	type="execute" 
	desc="Starting the process with the given parameters"

	filename=""
	params="" 
/>		 
```

### newfolder

```xml
<action 
	type="newfolder" 
	desc="create empty folder"

	path=""
/>
```

### copyfolder

```xml
<action 
	type="copyfolder" 
	desc="recursively copying a directory with subdirectories and files that complete the copy pattern"

	source=""
	destination="" 
	copy_filepattern="(.)" 
	copy_dirpattern="(.)" 
/>
```

### copyfile

```xml
<action 
	type="copyfile" 
	desc="Copy the file"

	source=""
	destination="" 
/>	
```

### deletefiles

```xml
<action 
	type="deletefiles" 
	desc="Deleting files from the specified directory that complete the delete pattern"

	source=""
	pattern:"(\\.png)$"
	recursive="true" 
/>	
```
### deletefolders

```xml
<action 
	type="deletefolders" 
	desc="Delete folders"

	source=""
	delete_folderpattern="" 
/>		 
```
### zipfolder

```xml
<action
	type="zipfolder" 
	desc="Create a zip archive from the selected folder to the specified target file"

	source=""
	zipfile="" 
/>		 
```
### showdialog

```xml
<action 
	type="showdialog" 
	desc="Show dialog with message"

	message=""
	messagetype="info" 
/>		
```

---
