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
    name:'template project'
    desc:'project focuses on the correct using of commands '

    execution:
    {
    	execute_if:
        {
            condition:'PC == "mypc"'
            filename:'||defaultbrowser||'
            params:'www.cppreference.com'
        }

        controlflow:
        {
            condition:'dialog'
            dialogtext:'yer or no ?'
            yes:
            {
            }
            no:
            {
            }    
        }

        execute:'||bin||/daily_remarks/daily_remarks.exe'

        execute:
        {
			 filename:'||defaultbrowser||'
			 params:'www.aktuality.sk'
		}

   		deletefile:"||mroot||/temp/tttt/gear_watch/connect.bat"

    	deletefiles:
        {
            source:'||mroot||/temp/tttt'
            recursive:'false'
            pattern:'(\\.bat)$'
        }
        deletefiles:'||mroot||/temp/tttt'
        

		zipfolder:
        {
            source:'||mroot||/test'
            zipfile:'||mroot||/temp/ahoj.zip'
        }
    }
}

```

Simple script to copy configuration files that uses only shortcut commands

``` javascript
project:
{
    name:'export portable etc'
    desc:'creating interchangeable etc folder'

    execution:
    {
        deletefolder:'||test||/etc'
        deletefile:'||test||/etc.zip'
        
        copyfolder: '||mroot||/system/etc ---> ||test||/etc'

        deletefolder:'||test||/etc/daily_remarks'
        deletefolder:'||test||/etc/wox_plugins/devel_sandbox'
        deletefolder:'||test||/etc/wox_plugins/guitar_chords'
        deletefolder:'||test||/etc/wox_plugins/restarurat_menu'
        deletefile:'||test||/etc/envvars.xml'

        zipfolder:'||test||/etc ---> ||test||/etc.zip'
        execute: '||dcommander|| -C -T ||test||'
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
- [Execute](#execute_if)
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
```javascript
wait: 3000
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
```javascript
execute: 'notepad.exe hello.exe'
```

### execute_if

```xml
<action
	type="execute_if" 
	desc="Starting the process with the given parameters if condition is met"

	condition=""
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
```javascript
newfolder: '||test||\tesing_folder'
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
```javascript
copyfolder: '||test||\tesing_folder ---> ||temp||\temp_folder'
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
```javascript
copyfile: '||test||\tesing-file.txt ---> ||temp||\temp-file.txt'
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
```javascript
deletefiles: '||test|| , (\\.png)$'
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
```javascript
deletefolders: '||test||'
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
```javascript
zipfolder: '||test||\testing_folder ---> ||temp||\archive.zip'
zipfolder: '||test||\testing_folder' // in place
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
```javascript
showdialog: 'this is informative message'
```

---
