<img src="https://www.novell.com/developer/plugin-sdk/Action-flow.png" title="ActionFlow" alt="ActionFlow">


# ActionFlow

> process-executor 

> file operation manager

---

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


## wait

```xml

<action
		type="wait" 
		name=""
	
		desc="Slow down the process for millions of milliseconds"
		duration_ms="" 
/>

```

### execute

<action
		type="execute" 
		name=""
		desc="Starting the process with the given parameters"

		filename=""
		params="" 
/>		 


## copyfolder

<action 
		type="copyfolder" 
		name=""
		desc="recursively copying a directory with subdirectories and files that complete the copy pattern"

		source=""
		destination="" 
		copy_filepattern="(.)" 
		copy_dirpattern="(.)" 
/>

## copyfile

<action 
		type="copyfile" 
		name=""
		desc="Copy the file"

		source=""
		destination="" 
/>	
## deletefiles

<action 
		type="deletefiles" 
		name=""
		desc="Deleting files from the specified directory that complete the delete pattern"

		source=""
		delete_filepattern="" 
		recursive="true" 
/>	

## deletefolders

<action 
		type="deletefolders" 
		name=""
		desc="Delete folders"

		source=""
		delete_folderpattern="" 
/>		 

## zipfolder

<action
		type="zipfolder" 
		name=""
		desc="Create a zip archive from the selected folder to the specified target file"

		source=""
		zipfile="" 
/>		 

## showdialog

<action 
		type="showdialog" 
		name=""
		desc="Show dialog with message"

		message=""
		messagetype="info" 
/>		

---
