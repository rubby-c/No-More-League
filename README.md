# No More League
A service that keeps deleting League of Legends that cannot be removed normally.

## How to install:

(If you have Visual Studio installed)

Run this in the visual studio developer console: ```installutil [executable]```

(If you do not have Visual Studio installed)

- Compile or download a release and run this in admin powershell.
- Make sure to replace "[executable]" with the path to the executable.
```
New-Service -Name "No More League" -BinaryPathName [executable]
```

<details> 
  <summary>How to remove? [Don't click unless you want to lose your sanity] </summary>
  
  ### (If you have visual studio installed)
  
  Run this in the visual studio developer console: 
  
  ```installutil /u [executable]```
  
  ### (If you do not have visual studio installed)
  1st way: Run Windows in safe mode and delete the executable file.
  
  2nd way: Run this in admin powershell:
  ```Remove-Service -Name "No More League"```
  then this: ```sc.exe delete "No More League"```
</details>
